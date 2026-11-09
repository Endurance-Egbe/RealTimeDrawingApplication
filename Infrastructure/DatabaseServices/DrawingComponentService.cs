using System.Collections.Generic;
using View.Models;
using View.ViewModels.ProxyModel;

namespace View.ViewModels.DatabaseServices
{
    public class DrawingComponentService
    {
        private readonly IDrawingComponentRepository repository;

        public DrawingComponentService(IDrawingComponentRepository repository)
        {
            this.repository = repository;
        }

        public void CreateDrawings(ProjectProxyModel projectProxy, List<DrawingComponentProxyModel> drawingComponents)
        {
            var drawingModels = ComponentValidationToDatabase(drawingComponents);
            var project = repository.GetCurrentProject(projectProxy.ProjectName);
            var existingDrawings = repository.GetDrawingComponents(project);

            if (existingDrawings != null)
            {
                DeleteDrawings(projectProxy);
            }

            foreach (var drawing in drawingModels)
            {
                drawing.Project = project;
                repository.Create(drawing);
            }
        }

        public IEnumerable<DrawingComponentProxyModel> GetDrawings(ProjectProxyModel projectProxy)
        {
            var project = repository.GetCurrentProject(projectProxy.ProjectName);
            return ComponentValidationFromDatabase(repository.GetDrawingComponents(project));
        }

        public List<DrawingComponentModel> GetDrawingComponentModels(ProjectProxyModel projectProxy)
        {
            var project = repository.GetCurrentProject(projectProxy.ProjectName);
            return repository.GetDrawingComponents(project);
        }

        public void DeleteDrawings(ProjectProxyModel projectProxy)
        {
            var project = repository.GetCurrentProject(projectProxy.ProjectName);
            var drawings = repository.GetDrawingComponents(project);
            foreach (var drawing in drawings)
            {
                repository.Delete(drawing);
            }
        }

        public static List<DrawingComponentModel> ComponentValidationToDatabase(
            List<DrawingComponentProxyModel> drawingComponents)
        {
            return DrawingComponentMapper.ToDatabaseModels(drawingComponents);
        }

        public static List<DrawingComponentProxyModel> ComponentValidationFromDatabase(
            List<DrawingComponentModel> drawingComponents)
        {
            return DrawingComponentMapper.ToProxyModels(drawingComponents);
        }
    }
}
