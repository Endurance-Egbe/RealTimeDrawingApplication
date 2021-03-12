using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using View.ViewModels.Common;
using View.ViewModels.Common.Event_Container;
using View.ViewModels.ComponentServices;
using Prism.Ioc;
using View.ViewModels.ProxyModel;
using View.ViewModels.ShapeServices;
using Prism.Mvvm;
using System.ComponentModel;
using System.Runtime.CompilerServices;
//using View.ViewModels.ShapeServices;

namespace View.ViewModels
{
    public class CustomCanvasViewModel : Canvas, INotifyPropertyChanged
    {
        private FrameworkElement selectedElement;
        private ComponentModel currentSelectedItem;

        public CustomCanvasViewModel()
        {
            AllowDrop = true;
            Background = Brushes.WhiteSmoke;
            CurrentSelectedItem = new ComponentModel();
            EventAggregator = GenericServiceLocator.ShellContainer.Resolve<IEventAggregator>();
            EventAggregator.GetEvent<ComponentPropertyPubSubEvent>().Subscribe(UpdateCanvasElement);
            EventAggregator.GetEvent<DrawingComponentEvent>().Subscribe(LoadDrawingComponents);
            EventAggregator.GetEvent<JsonImportedProxiesEvent>().Subscribe(LoadDrawingComponents);
        }
        //public IEnumerable<DrawingComponentProxyModel> DrawingComponentProxies  { get; set; }
        private void LoadDrawingComponents(IEnumerable<DrawingComponentProxyModel> drawingComponents)
        {

            //FrameworkElement component1 = null;
            //var component = component1 as IPropertyWindow;

            Children.Clear();
            if (drawingComponents != null)
            {
                List<DrawingComponentProxyModel> drawingComponentsModel = new List<DrawingComponentProxyModel>();
                drawingComponentsModel = drawingComponents.ToList();
                ComponentEnum componentEnum = ComponentEnum.TextBox;
                foreach (var drawing in drawingComponentsModel)
                {

                    switch (drawing.Title)
                    {
                        case "Triangle":
                            componentEnum = ComponentEnum.Triangle;
                            break;
                        case "Ecllipse":
                            componentEnum = ComponentEnum.Ecllipse;
                            break;
                        case "Rectangle":
                            componentEnum = ComponentEnum.Rectangle;
                            break;
                        case "Line":
                            componentEnum = ComponentEnum.Line;
                            break;
                        case "TextBox":
                            componentEnum = ComponentEnum.TextBox;
                            break;
                        default:
                            break;
                    }

                    ShapeServices.ShapeServices shapeServices = new ShapeServices.ShapeServices();
                    var getDefaultComponent = shapeServices.GetDefaultControl(componentEnum);
                    var _component = getDefaultComponent as IPropertyWindow;
                    _component.Title = drawing.Title;
                    _component.Width = drawing.Width;
                    _component.Height = drawing.Height;
                    _component.LineThickness = drawing.LineThickness;
                    _component.SelectedBorderColor = drawing.SelectedBorderColor.ColorType;
                    _component.SelectedFillColor = drawing.SelectedFillColor.ColorType;
                    _component.SelectedStroke = drawing.SelectedStroke.ColorType;
                    var frameworkComponent = _component.GetComponent() as FrameworkElement;
                    //var propertyComponent = frameworkComponent as IPropertyWindow;
                    //var getComponent = propertyComponent.GetComponent() as FrameworkElement;
                    Children.Add(getDefaultComponent);
                    SetLeft(getDefaultComponent, drawing.X);
                    SetTop(getDefaultComponent, drawing.Y);
                }

            }

        }
        private void UpdateCanvasElement(ComponentModel property)
        {
            if (property != null)
            {
                FrameworkElement component = null;
                foreach (FrameworkElement item in Children)
                {
                    var _item = (IPropertyWindow)item;
                    if (_item != null && _item.Id == property.PropertyId)
                    {
                        switch (property.PropertyType)
                        {
                            case PropertyType.Title:
                                _item.Title = (string)property.Value;
                                break;
                            case PropertyType.X:
                                _item.X = (double)property.Value;
                                SetLeft(item, _item.X);

                                break;
                            case PropertyType.Y:
                                _item.Y = (double)property.Value;
                                SetTop(item, _item.Y);
                                break;
                            case PropertyType.Width:
                                _item.Width = (double)property.Value;

                                break;
                            case PropertyType.Height:
                                _item.Height = (double)property.Value;
                                break;
                            case PropertyType.StrokeThickness:
                                _item.LineThickness = (int)property.Value;
                                break;
                            case PropertyType.Stroke:
                                _item.SelectedStroke = (SolidColorBrush)property.Value;
                                //_item.SelectedStroke = ColorCollectionsClass.ColorCollections.FirstOrDefault
                                //    (x => x.ColorType == property.Value as SolidColorBrush); 
                                break;
                            case PropertyType.FillColor:
                                _item.SelectedFillColor = property.Value as SolidColorBrush;
                                //_item.SelectedFillColor = ColorCollectionsClass.ColorCollections.FirstOrDefault
                                //(x => x.ColorType == property.Value as SolidColorBrush);
                                break;
                            case PropertyType.BorderColor:
                                _item.SelectedBorderColor = (SolidColorBrush)property.Value;
                                //_item.SelectedBorderColor = ColorCollectionsClass.ColorCollections.FirstOrDefault
                                //    (x => x.ColorType == property.Value as SolidColorBrush);
                                break;
                            default:
                                break;
                        }
                        component = _item as FrameworkElement;

                    }

                }
                if (component != null)
                {
                    var _component = component as IPropertyWindow;
                    component = _component.GetComponent() as FrameworkElement;
                    //EventAggregator.GetEvent<CanvasComponentEvent>().Publish(component);
                }
            }
        }

        private FrameworkElement SelectedElement { get => selectedElement; set { selectedElement = value; OnPropertyChanged(); } }
        public IEventAggregator EventAggregator { get; set; }
        public ComponentModel CurrentSelectedItem { get => currentSelectedItem; set { currentSelectedItem = value; OnPropertyChanged(); } }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected override void OnDrop(DragEventArgs e)
        {
            var item = e.Data.GetData("toolboxitem") as FrameworkElement;
            var currentMousePosition = e.GetPosition(this);
            var xValue = currentMousePosition.X;
            var yValue = currentMousePosition.Y;
            Canvas.SetLeft(item, xValue);
            Canvas.SetTop(item, yValue);
            Children.Add(item);


            if (item is IPropertyWindow componentProperty)
            {
                ResetPreviousComponent();
                componentProperty.ShowBorder = true;
                CurrentSelectedItem.PropertyType = PropertyType.ShowBorder;
                CurrentSelectedItem.Value = componentProperty.ShowBorder;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyType = PropertyType.Title;
                CurrentSelectedItem.Value = (string)componentProperty.Title;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyId = componentProperty.Id;
                CurrentSelectedItem.ComponentEnum = componentProperty.ComponentEnum;
                CurrentSelectedItem.PropertyType = PropertyType.X;
                CurrentSelectedItem.Value = (double)xValue;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyType = PropertyType.FillColor;
                CurrentSelectedItem.Value = componentProperty.SelectedFillColor;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyType = PropertyType.Stroke;
                CurrentSelectedItem.Value = componentProperty.SelectedStroke;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyType = PropertyType.BorderColor;
                CurrentSelectedItem.Value = componentProperty.SelectedBorderColor;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyType = PropertyType.Width;
                CurrentSelectedItem.Value = componentProperty.Width;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyType = PropertyType.Height;
                CurrentSelectedItem.Value = componentProperty.Height;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyType = PropertyType.Title;
                CurrentSelectedItem.Value = componentProperty.Title;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyType = PropertyType.Y;
                CurrentSelectedItem.Value = (double)yValue;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (SelectedElement != null)
            {
                var position = e.GetPosition(this);
                if (position.X < ActualWidth - SelectedElement.ActualWidth &&
                    position.Y < ActualHeight - SelectedElement.ActualHeight)
                {
                    SetLeft(SelectedElement, position.X);
                    SetTop(SelectedElement, position.Y);
                    CurrentSelectedItem.PropertyType = PropertyType.X;
                    CurrentSelectedItem.Value = (double)position.X;
                    EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                    CurrentSelectedItem.PropertyType = PropertyType.Y;
                    CurrentSelectedItem.Value = (double)position.Y;
                    EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                   

                }
                ResetPreviousComponent();
            }

        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            SelectedElement = e.Source as FrameworkElement;
            SelectedElement.MouseLeftButtonUp += SelectedElement_MouseLeftButtonUp;

            if (GetParent(SelectedElement) is IPropertyWindow component)
            {

                CurrentSelectedItem = new ComponentModel()
                {
                    PropertyId = component.Id,
                    ComponentEnum = component.ComponentEnum,

                };

                component.ShowBorder = true;
                SelectedElement = component.GetComponent() as FrameworkElement;
               
               
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
               
                
            }
            
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            SelectedElement = null;
            ResetPreviousComponent();
            base.OnMouseLeftButtonUp(e);
        }
        private void SelectedElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SelectedElement = null;
            
            base.OnMouseLeftButtonUp(e);
        }
        private FrameworkElement GetParent(FrameworkElement frameworkElement)
        {
            if (frameworkElement is ShapeComponent)
            {
                return frameworkElement;
            }
            else
            {
                if (frameworkElement.Parent == null)
                {
                    return null;
                }
                return GetParent(frameworkElement.Parent as FrameworkElement);
            }
        }
        private void ResetPreviousComponent()
        {
            FrameworkElement _component = null;
            foreach (FrameworkElement item in Children)
            {

                if (item is IPropertyWindow component && component.ShowBorder==true)
                {
                    _component = item;
                    break;
                }
            }
            if (_component != null)
            {
                var __component = (_component as IPropertyWindow);
                __component.ShowBorder = false;
                _component = __component.GetComponent() as FrameworkElement;
            }
        }


    }

}
