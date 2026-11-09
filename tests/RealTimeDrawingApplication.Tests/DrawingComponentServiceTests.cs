using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
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
    }
}
