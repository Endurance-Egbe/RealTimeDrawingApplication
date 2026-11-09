using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;
using System.Windows.Media;
using View.ViewModels;
using View.ViewModels.Common;
using View.ViewModels.Common.CustomColor;
using View.ViewModels.Common.Event_Container;

namespace RealTimeDrawingApplication.Tests
{
    [TestClass]
    public class PropertyViewModelTests
    {
        [TestMethod]
        public void GetValue_ReturnsScalarAndTitleValues()
        {
            var viewModel = CreateViewModel();
            viewModel.Title = "Rectangle";
            viewModel.X = 12;
            viewModel.LineThickness = 4;

            Assert.AreEqual("Rectangle", viewModel.GetValue(nameof(PropertyViewModel.Title)));
            Assert.AreEqual(12d, viewModel.GetValue(nameof(PropertyViewModel.X)));
            Assert.AreEqual(4, viewModel.GetValue(nameof(PropertyViewModel.LineThickness)));
        }

        [TestMethod]
        public void GetValue_ReturnsSelectedColorBrush()
        {
            var viewModel = CreateViewModel();
            var color = viewModel.ColorCollections.First(x => x.ColorName == "Red");
            viewModel.SelectedFillColor = color;

            Assert.AreSame(color.ColorType,
                viewModel.GetValue(nameof(PropertyViewModel.SelectedFillColor)));
        }

        [TestMethod]
        public void PropertyKeyValuePair_MapsSupportedProperties()
        {
            var viewModel = CreateViewModel();

            Assert.AreEqual(PropertyType.Title,
                viewModel.PropertyKeyValuePair[nameof(PropertyViewModel.Title)]);
            Assert.AreEqual(PropertyType.FillColor,
                viewModel.PropertyKeyValuePair[nameof(PropertyViewModel.SelectedFillColor)]);
            Assert.AreEqual(PropertyType.LineThickness,
                viewModel.PropertyKeyValuePair[nameof(PropertyViewModel.LineThickness)]);
        }

        [TestMethod]
        public void PropertyChange_PublishesSelectedComponentUpdate()
        {
            var eventAggregator = new EventAggregator();
            var viewModel = new PropertyViewModel(eventAggregator)
            {
                CurrentSelectedItem = new ComponentModel { PropertyId = Guid.NewGuid() }
            };
            ComponentModel published = null;
            eventAggregator.GetEvent<ComponentPropertyPubSubEvent>()
                .Subscribe(component => published = component);

            viewModel.Title = "Updated title";

            Assert.IsNotNull(published);
            Assert.AreSame(viewModel.CurrentSelectedItem, published);
            Assert.AreEqual(PropertyType.Title, published.PropertyType);
            Assert.AreEqual("Updated title", published.Value);
        }

        [TestMethod]
        public void PropertyChange_RaisesPropertyChangedForSelectedComponent()
        {
            var viewModel = CreateViewModel();
            viewModel.CurrentSelectedItem = new ComponentModel { PropertyId = Guid.NewGuid() };
            string propertyName = null;
            viewModel.PropertyChanged += (sender, args) => propertyName = args.PropertyName;

            viewModel.Width = 250;

            Assert.AreEqual(nameof(PropertyViewModel.Width), propertyName);
        }

        [TestMethod]
        public void UpdateEvent_LoadsSelectedNumericProperty()
        {
            var eventAggregator = new EventAggregator();
            var viewModel = new PropertyViewModel(eventAggregator);
            var property = new ComponentModel
            {
                PropertyId = Guid.NewGuid(),
                PropertyType = PropertyType.Height,
                Value = 180d
            };

            eventAggregator.GetEvent<UpdatePropertyWindow>().Publish(property);

            Assert.AreSame(property, viewModel.CurrentSelectedItem);
            Assert.AreEqual(180d, viewModel.Height);
        }

        [DataTestMethod]
        [DataRow(nameof(PropertyViewModel.X), PropertyType.X)]
        [DataRow(nameof(PropertyViewModel.Y), PropertyType.Y)]
        [DataRow(nameof(PropertyViewModel.Width), PropertyType.Width)]
        [DataRow(nameof(PropertyViewModel.Height), PropertyType.Height)]
        [DataRow(nameof(PropertyViewModel.Title), PropertyType.Title)]
        [DataRow(nameof(PropertyViewModel.StrokeThickness), PropertyType.StrokeThickness)]
        [DataRow(nameof(PropertyViewModel.LineThickness), PropertyType.LineThickness)]
        [DataRow(nameof(PropertyViewModel.SelectedStrokeColor), PropertyType.Stroke)]
        [DataRow(nameof(PropertyViewModel.SelectedFillColor), PropertyType.FillColor)]
        [DataRow(nameof(PropertyViewModel.SelectedBorderColor), PropertyType.BorderColor)]
        public void PropertyKeyValuePair_ContainsEveryEditableProperty(
            string propertyName, PropertyType expectedPropertyType)
        {
            var viewModel = CreateViewModel();

            Assert.AreEqual(expectedPropertyType, viewModel.PropertyKeyValuePair[propertyName]);
        }

        [TestMethod]
        public void GetValue_ReturnsNullForUnknownProperty()
        {
            var viewModel = CreateViewModel();

            Assert.IsNull(viewModel.GetValue("UnknownProperty"));
        }

        private static PropertyViewModel CreateViewModel()
        {
            return new PropertyViewModel(new EventAggregator());
        }
    }
}
