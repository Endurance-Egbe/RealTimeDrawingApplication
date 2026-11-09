using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using View.Infrastructure;
using View.Models;
using View.ViewModels.Common.CustomColor;
using View.ViewModels.DatabaseServices;
using View.ViewModels.ProxyModel;

namespace RealTimeDrawingApplication.Tests
{
    [TestClass]
    public class DrawingComponentServiceTests
    {
        [TestMethod]
        public void ToDatabaseModelsMapsDrawingValuesAndColorNames()
        {
            var red = new CustomColor { ColorName = "Red" };
            var blue = new CustomColor { ColorName = "Blue" };
            var component = new DrawingComponentProxyModel
            {
                Title = "Rectangle",
                X = 10,
                Y = 20,
                Width = 100,
                Height = 50,
                LineThickness = 2,
                StrokeThickness = 3,
                SelectedBorderColor = red,
                SelectedFillColor = blue,
                SelectedStroke = red
            };

            var result = DrawingComponentMapper.ToDatabaseModels(
                new List<DrawingComponentProxyModel> { component });

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Rectangle", result[0].Title);
            Assert.AreEqual(10, result[0].X);
            Assert.AreEqual(20, result[0].Y);
            Assert.AreEqual("Red", result[0].SelectedBorderColor);
            Assert.AreEqual("Blue", result[0].SelectedFillColor);
            Assert.AreEqual("Red", result[0].SelectedStroke);
        }

        [TestMethod]
        public void ToProxyModelsRestoresDrawingValuesAndKnownColors()
        {
            ColorCollectionsClass.LoadColorCollection();
            var component = new DrawingComponentModel
            {
                Title = "Circle",
                X = 4,
                Y = 8,
                Width = 40,
                Height = 40,
                LineThickness = 1,
                StrokeThickness = 2,
                SelectedBorderColor = "Black",
                SelectedFillColor = "Yellow",
                SelectedStroke = "Blue"
            };

            var result = DrawingComponentMapper.ToProxyModels(
                new List<DrawingComponentModel> { component });

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Circle", result[0].Title);
            Assert.AreEqual(4, result[0].X);
            Assert.AreEqual("Black", result[0].SelectedBorderColor.ColorName);
            Assert.AreEqual("Yellow", result[0].SelectedFillColor.ColorName);
            Assert.AreEqual("Blue", result[0].SelectedStroke.ColorName);
        }

        [TestMethod]
        public void GetDrawings_MapsRepositoryModelsToProxyModels()
        {
            var project = new ProjectModel { ProjectName = "Sketch" };
            var repository = new InMemoryDrawingRepository(project);
            repository.Drawings.Add(new DrawingComponentModel
            {
                Title = "Rectangle",
                Width = 100,
                Height = 50,
                SelectedStroke = "Black",
                SelectedFillColor = "Red",
                SelectedBorderColor = "Blue"
            });

            var service = new DrawingComponentService(repository);
            var drawings = service.GetDrawings(new ProjectProxyModel { ProjectName = "Sketch" })
                .ToList();

            Assert.AreEqual(1, drawings.Count);
            Assert.AreEqual("Rectangle", drawings[0].Title);
            Assert.AreEqual("Red", drawings[0].SelectedFillColor.ColorName);
        }

        [TestMethod]
        public void DeleteDrawings_DeletesEveryDrawingInProject()
        {
            var project = new ProjectModel { ProjectName = "Sketch" };
            var repository = new InMemoryDrawingRepository(project);
            repository.Drawings.Add(new DrawingComponentModel());
            repository.Drawings.Add(new DrawingComponentModel());

            new DrawingComponentService(repository).DeleteDrawings(
                new ProjectProxyModel { ProjectName = "Sketch" });

            Assert.AreEqual(2, repository.Deleted.Count);
        }

        [TestMethod]
        public void CreateDrawings_ReplacesExistingDrawingsAndAssignsProject()
        {
            ColorCollectionsClass.LoadColorCollection();
            var project = new ProjectModel { ProjectName = "Sketch" };
            var repository = new InMemoryDrawingRepository(project);
            repository.Drawings.Add(new DrawingComponentModel());

            new DrawingComponentService(repository).CreateDrawings(
                new ProjectProxyModel { ProjectName = "Sketch" },
                new List<DrawingComponentProxyModel>
                {
                    new DrawingComponentProxyModel
                    {
                        Title = "New shape",
                        SelectedStroke = ColorCollectionsClass.ColorCollections
                            .First(color => color.ColorName == "Black"),
                        SelectedFillColor = ColorCollectionsClass.ColorCollections
                            .First(color => color.ColorName == "Red"),
                        SelectedBorderColor = ColorCollectionsClass.ColorCollections
                            .First(color => color.ColorName == "Blue")
                    }
                });

            Assert.AreEqual(1, repository.Deleted.Count);
            Assert.AreEqual(1, repository.Created.Count);
            Assert.AreSame(project, repository.Created[0].Project);
            Assert.AreEqual("New shape", repository.Created[0].Title);
        }

        private sealed class InMemoryDrawingRepository : IDrawingComponentRepository
        {
            private readonly ProjectModel project;

            public InMemoryDrawingRepository(ProjectModel project)
            {
                this.project = project;
            }

            public List<DrawingComponentModel> Drawings { get; } =
                new List<DrawingComponentModel>();
            public List<DrawingComponentModel> Created { get; } =
                new List<DrawingComponentModel>();
            public List<DrawingComponentModel> Deleted { get; } =
                new List<DrawingComponentModel>();

            public ProjectModel GetCurrentProject(string projectName)
            {
                return project.ProjectName == projectName ? project : null;
            }

            public List<DrawingComponentModel> GetDrawingComponents(ProjectModel currentProject)
            {
                return Drawings.ToList();
            }

            public void Create(DrawingComponentModel drawing)
            {
                Created.Add(drawing);
            }

            public void Delete(DrawingComponentModel drawing)
            {
                Deleted.Add(drawing);
            }
        }
    }
}
