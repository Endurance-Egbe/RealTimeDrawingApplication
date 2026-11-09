using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.Infrastructure;
using View.Infrastructure.Query;
using View.Models;
using View.ViewModels.Common.CustomColor;
using View.ViewModels.ProxyModel;

namespace View.ViewModels.DatabaseServices
{
    public class DrawingComponentService
    {
        
        public static void CreateDrawings(ProjectProxyModel projectProxy, List<DrawingComponentProxyModel> drawingComponent)
        {
            List<DrawingComponentModel> drawingModels = new List<DrawingComponentModel>();
            drawingModels= ComponentValidationToDatabase(drawingComponent);
            var sqlite = new SQLiteEFCore();
            var model = new DrawingComponentEFCoreRepository(sqlite);
            ProjectModel projectModel = model.GetCurrentProject(projectProxy.ProjectName);
            IEnumerable<DrawingComponentModel> drawingComponentModel = model.GetDrawingComponents(projectModel);            
            if (drawingComponentModel!=null)
            {
                DeleteDrawings(projectProxy);          
            }
          
                foreach (var drawing in drawingModels)
                {
                    drawing.Project = projectModel;
                    model.CreateModel(drawing);

                }
        }
        public static IEnumerable<DrawingComponentProxyModel> GetDrawings(ProjectProxyModel projectProxy)
        {
            List<DrawingComponentProxyModel> drawingModel = new List<DrawingComponentProxyModel>();
            var sqlite = new SQLiteEFCore();
            var model = new DrawingComponentEFCoreRepository(sqlite);
            ProjectModel projectModel = model.GetCurrentProject(projectProxy.ProjectName);
            List<DrawingComponentModel> drawingComponents  = model.GetDrawingComponents(projectModel);
            drawingModel=ComponentValidationFromDatabase(drawingComponents);
            //component.
            return drawingModel;
        }
        public static List<DrawingComponentModel> ComponentValidationToDatabase(List<DrawingComponentProxyModel> drawingComponentProxy)
        {
            return DrawingComponentMapper.ToDatabaseModels(drawingComponentProxy);
        }
        public static List<DrawingComponentProxyModel> ComponentValidationFromDatabase(List<DrawingComponentModel> drawingComponents)
        {
            return DrawingComponentMapper.ToProxyModels(drawingComponents);
        }
        public static List<DrawingComponentModel> GetDrawingComponentModels(ProjectProxyModel projectProxy)
        {
            List<DrawingComponentProxyModel> drawingModel = new List<DrawingComponentProxyModel>();
            var sqlite = new SQLiteEFCore();
            var model = new DrawingComponentEFCoreRepository(sqlite);
            ProjectModel projectModel = model.GetCurrentProject(projectProxy.ProjectName);
            List<DrawingComponentModel> drawingComponents = model.GetDrawingComponents(projectModel);
            return drawingComponents;
        }
        public static void DeleteDrawings(ProjectProxyModel projectProxy)
        {
            var sqlite = new SQLiteEFCore();
            var model = new DrawingComponentEFCoreRepository(sqlite);
            ProjectModel projectModel = model.GetCurrentProject(projectProxy.ProjectName);
            List<DrawingComponentModel> drawingComponents = model.GetDrawingComponents(projectModel);
            foreach (var item in drawingComponents)
            {
                model.DeleteModel(item);
            }
        }
    }
}
