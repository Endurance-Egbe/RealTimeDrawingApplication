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
        private static int projectId;
        public static void CreateDrawings(ProjectProxyModel projectProxy, List<DrawingComponentProxyModel> drawingComponent)
        {
            List<DrawingComponentModel> drawingModels = new List<DrawingComponentModel>();
            drawingModels= ComponentValidationToDatabase(drawingComponent);
            var sqlite = new SQLiteEFCore();
            var model = new DrawingComponentEFCoreRepository(sqlite);
            ProjectModel projectModel = model.GetCurrentProject(projectProxy.ProjectName);
            IEnumerable<DrawingComponentModel> drawingComponentModel = model.GetDrawingComponents(projectModel);            
            if (projectId==projectModel.ProjectId)
            {
                foreach (var item in drawingComponentModel)
                {
                    model.DeleteModel(item);
                }              
            }
            else
            {
                foreach (var drawing in drawingModels)
                {
                    drawing.Project = projectModel;
                    model.CreateModel(drawing);

                }
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
            List<DrawingComponentModel> drawingModel = new List<DrawingComponentModel>();
            foreach (var drawing in drawingComponentProxy)
            {
                drawingModel.Add(new DrawingComponentModel()
                {
                    Title = drawing.Title,
                    X = drawing.X,
                    Y = drawing.Y,
                    Width = drawing.Width,
                    Height = drawing.Height,
                    SelectedBorderColor = drawing.SelectedBorderColor.ColorName,
                    SelectedFillColor = drawing.SelectedFillColor.ColorName,
                    SelectedStroke = drawing.SelectedStroke.ColorName,
                    StrokeThickness = drawing.StrokeThickness,
                    LineThickness = drawing.LineThickness
                });
            }
            
            return drawingModel;
        }
        public static List<DrawingComponentProxyModel> ComponentValidationFromDatabase(List<DrawingComponentModel> drawingComponents)
        {
            List<DrawingComponentProxyModel> drawingComponentModel = new List<DrawingComponentProxyModel>();
            foreach (var drawing in drawingComponents)
            {
                
                drawingComponentModel.Add(new DrawingComponentProxyModel()
                {
                    Title = drawing.Title,
                    X = drawing.X,
                    Y = drawing.Y,
                    Width = drawing.Width,
                    Height = drawing.Height,
                    LineThickness = drawing.LineThickness,
                    SelectedBorderColor = ColorCollectionsClass.ColorCollections.FirstOrDefault(x => x.ColorName == drawing.SelectedBorderColor),
                    SelectedFillColor = ColorCollectionsClass.ColorCollections.FirstOrDefault(x => x.ColorName == drawing.SelectedFillColor),
                    SelectedStroke= ColorCollectionsClass.ColorCollections.FirstOrDefault(x => x.ColorName == drawing.SelectedStroke),
                    StrokeThickness= drawing.StrokeThickness
                }); ;
            }
           
            return drawingComponentModel;
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
    }
}
