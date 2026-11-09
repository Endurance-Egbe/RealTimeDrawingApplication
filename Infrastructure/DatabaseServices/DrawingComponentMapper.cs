using System.Collections.Generic;
using System.Linq;
using View.Models;
using View.ViewModels.Common.CustomColor;
using View.ViewModels.ProxyModel;

namespace View.ViewModels.DatabaseServices
{
    public static class DrawingComponentMapper
    {
        public static List<DrawingComponentModel> ToDatabaseModels(
            IEnumerable<DrawingComponentProxyModel> components)
        {
            return components.Select(component => new DrawingComponentModel
            {
                Title = component.Title,
                X = component.X,
                Y = component.Y,
                Width = component.Width,
                Height = component.Height,
                SelectedBorderColor = component.SelectedBorderColor.ColorName,
                SelectedFillColor = component.SelectedFillColor.ColorName,
                SelectedStroke = component.SelectedStroke.ColorName,
                StrokeThickness = component.StrokeThickness,
                LineThickness = component.LineThickness
            }).ToList();
        }

        public static List<DrawingComponentProxyModel> ToProxyModels(
            IEnumerable<DrawingComponentModel> components)
        {
            var colors = ColorCollectionsClass.ColorCollections
                ?? ColorCollectionsClass.LoadColorCollection();

            return components.Select(component => new DrawingComponentProxyModel
            {
                Title = component.Title,
                X = component.X,
                Y = component.Y,
                Width = component.Width,
                Height = component.Height,
                LineThickness = component.LineThickness,
                StrokeThickness = component.StrokeThickness,
                SelectedBorderColor = colors.FirstOrDefault(
                    color => color.ColorName == component.SelectedBorderColor),
                SelectedFillColor = colors.FirstOrDefault(
                    color => color.ColorName == component.SelectedFillColor),
                SelectedStroke = colors.FirstOrDefault(
                    color => color.ColorName == component.SelectedStroke)
            }).ToList();
        }
    }
}
