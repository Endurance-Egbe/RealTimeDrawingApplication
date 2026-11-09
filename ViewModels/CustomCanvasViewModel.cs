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
        private bool isResizing;
        private string resizeDirection;
        private Point resizeStartPoint;
        private double resizeStartWidth;
        private double resizeStartHeight;
        private double resizeStartLeft;
        private double resizeStartTop;
        private const double MinimumComponentSize = 20;

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
                    _component.X = drawing.X;
                    _component.Y = drawing.Y;
                    _component.Width = drawing.Width;
                    _component.Height = drawing.Height;
                    _component.StrokeThickness = drawing.LineThickness;
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
            var xValue = currentMousePosition.X-item.Width/2;
            var yValue = currentMousePosition.Y-item.Height/2;
            Canvas.SetLeft(item, xValue);
            Canvas.SetTop(item, yValue);
            Children.Add(item);


            if (item is IPropertyWindow componentProperty)
            {
                ResetPreviousComponent();
                
                CurrentSelectedItem.PropertyType = PropertyType.StrokeThickness;
                CurrentSelectedItem.Value = (int)componentProperty.LineThickness;
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
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);//componentProperty.ShowBorder = true;
                CurrentSelectedItem.PropertyType = PropertyType.ShowBorder;
                componentProperty.ShowBorder=true;
                CurrentSelectedItem.Value = componentProperty.ShowBorder;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            var source = e.OriginalSource as DependencyObject;
            var componentElement = GetPropertyWindowElement(source);

            if (e.LeftButton != MouseButtonState.Pressed)
            {
                UpdateCursor(componentElement);
                return;
            }

            if (SelectedElement != null)
            {
                var position = e.GetPosition(this);

                if (isResizing)
                {
                    ResizeSelectedElement(position);
                }
                else if (position.X < ActualWidth - SelectedElement.Width &&
                    position.Y < ActualHeight - SelectedElement.Height)
                {
                    SetLeft(SelectedElement, position.X);
                    SetTop(SelectedElement, position.Y);
                    PublishProperty(PropertyType.X, position.X);
                    PublishProperty(PropertyType.Y, position.Y);
                }
            }
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            var source = e.OriginalSource as DependencyObject;
            var componentElement = GetPropertyWindowElement(source);
            var direction = GetResizeDirection(source);
            if (direction == null && componentElement != null)
            {
                direction = GetResizeDirection(componentElement, e.GetPosition(componentElement));
            }
            if (componentElement != null && direction != null)
            {
                SelectedElement = componentElement;
                resizeDirection = direction;
                BeginResize(e.GetPosition(this));
                e.Handled = true;
                return;
            }

            if (componentElement == null)
            {
                ResetPreviousComponent();
                SelectedElement = null;
                Cursor = Cursors.Arrow;
                CaptureMouse();
                base.OnMouseLeftButtonDown(e);
                return;
            }

            if (SelectedElement != componentElement)
            {
                ResetPreviousComponent();
            }
            SelectedElement = componentElement;
           
            if (SelectedElement != null &&
                GetParent(SelectedElement) is IPropertyWindow component)
            {
                component.ShowBorder = true;
                component.GetComponent();
                UpdateCursor(componentElement);

                CurrentSelectedItem = new ComponentModel()
                {
                    PropertyId = component.Id,
                    ComponentEnum = component.ComponentEnum,

                };

                CurrentSelectedItem.PropertyType = PropertyType.Width;
                CurrentSelectedItem.Value = (double)component.Width;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyType = PropertyType.Height;
                CurrentSelectedItem.Value = (double)component.Height;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyType = PropertyType.Title;
                CurrentSelectedItem.Value = (string)component.Title;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyType = PropertyType.BorderColor;
                CurrentSelectedItem.Value = (SolidColorBrush)component.SelectedBorderColor;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyType = PropertyType.FillColor;
                CurrentSelectedItem.Value = (SolidColorBrush)component.SelectedFillColor;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyType = PropertyType.Stroke;
                CurrentSelectedItem.Value = (SolidColorBrush)component.SelectedStroke;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyType = PropertyType.StrokeThickness;
                CurrentSelectedItem.Value = (int)component.LineThickness;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyType = PropertyType.LineThickness;
                CurrentSelectedItem.Value = (int)component.LineThickness;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyType = PropertyType.X;
                CurrentSelectedItem.Value = (double)component.X;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                CurrentSelectedItem.PropertyType = PropertyType.Y;
                CurrentSelectedItem.Value = (double)component.Y;
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
                
                EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
               
               
            }
            
            CaptureMouse();
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (IsMouseCaptured)
            {
                ReleaseMouseCapture();
            }
            isResizing = false;
            resizeDirection = null;
            UpdateCursor(SelectedElement);
            base.OnMouseLeftButtonUp(e);
        }

        private void BeginResize(Point startPoint)
        {
            resizeStartPoint = startPoint;
            resizeStartWidth = SelectedElement.Width;
            resizeStartHeight = SelectedElement.Height;
            resizeStartLeft = GetLeft(SelectedElement);
            resizeStartTop = GetTop(SelectedElement);
            if (double.IsNaN(resizeStartLeft))
            {
                resizeStartLeft = 0;
            }
            if (double.IsNaN(resizeStartTop))
            {
                resizeStartTop = 0;
            }

            isResizing = true;
            CaptureMouse();
        }

        private void ResizeSelectedElement(Point currentPoint)
        {
            var deltaX = currentPoint.X - resizeStartPoint.X;
            var deltaY = currentPoint.Y - resizeStartPoint.Y;
            var newWidth = resizeStartWidth;
            var newHeight = resizeStartHeight;
            var newLeft = resizeStartLeft;
            var newTop = resizeStartTop;

            if (resizeDirection.Contains("Right"))
            {
                newWidth = Math.Max(MinimumComponentSize, resizeStartWidth + deltaX);
            }
            if (resizeDirection.Contains("Left"))
            {
                newWidth = Math.Max(MinimumComponentSize, resizeStartWidth - deltaX);
                newLeft = resizeStartLeft + resizeStartWidth - newWidth;
            }
            if (resizeDirection.Contains("Bottom"))
            {
                newHeight = Math.Max(MinimumComponentSize, resizeStartHeight + deltaY);
            }
            if (resizeDirection.Contains("Top"))
            {
                newHeight = Math.Max(MinimumComponentSize, resizeStartHeight - deltaY);
                newTop = resizeStartTop + resizeStartHeight - newHeight;
            }

            SelectedElement.Width = newWidth;
            SelectedElement.Height = newHeight;
            SetLeft(SelectedElement, newLeft);
            SetTop(SelectedElement, newTop);
            var propertyWindow = SelectedElement as IPropertyWindow;
            if (propertyWindow != null)
            {
                propertyWindow.GetComponent();
            }
            PublishProperty(PropertyType.Width, newWidth);
            PublishProperty(PropertyType.Height, newHeight);
            PublishProperty(PropertyType.X, newLeft);
            PublishProperty(PropertyType.Y, newTop);
        }

        private void PublishProperty(PropertyType propertyType, object value)
        {
            CurrentSelectedItem.PropertyType = propertyType;
            CurrentSelectedItem.Value = value;
            EventAggregator.GetEvent<UpdatePropertyWindow>().Publish(CurrentSelectedItem);
        }

        private string GetResizeDirection(DependencyObject source)
        {
            var border = source as Border;
            return border == null ? null : border.Tag as string;
        }

        private string GetResizeDirection(FrameworkElement element, Point point)
        {
            if (element == null || !element.IsMouseOver)
            {
                return null;
            }

            const double edgeSize = 10;
            var left = point.X <= edgeSize;
            var right = point.X >= element.ActualWidth - edgeSize;
            var top = point.Y <= edgeSize;
            var bottom = point.Y >= element.ActualHeight - edgeSize;

            if (top && left)
            {
                return "TopLeft";
            }
            if (top && right)
            {
                return "TopRight";
            }
            if (bottom && left)
            {
                return "BottomLeft";
            }
            if (bottom && right)
            {
                return "BottomRight";
            }
            if (left)
            {
                return "Left";
            }
            if (right)
            {
                return "Right";
            }
            if (top)
            {
                return "Top";
            }
            if (bottom)
            {
                return "Bottom";
            }

            return null;
        }

        private void UpdateCursor(FrameworkElement componentElement)
        {
            if (componentElement == null || componentElement != SelectedElement)
            {
                Cursor = Cursors.Arrow;
                return;
            }

            var direction = GetResizeDirection(componentElement, Mouse.GetPosition(componentElement));
            var cursor = GetResizeCursor(direction) ?? Cursors.SizeAll;
            componentElement.Cursor = cursor;
            Cursor = cursor;
        }

        private Cursor GetResizeCursor(string direction)
        {
            switch (direction)
            {
                case "TopLeft":
                case "BottomRight":
                    return Cursors.SizeNWSE;
                case "TopRight":
                case "BottomLeft":
                    return Cursors.SizeNESW;
                case "Left":
                case "Right":
                    return Cursors.SizeWE;
                case "Top":
                case "Bottom":
                    return Cursors.SizeNS;
                default:
                    return null;
            }
        }

        private FrameworkElement GetPropertyWindowElement(DependencyObject source)
        {
            var current = source;
            while (current != null)
            {
                var element = current as FrameworkElement;
                if (element is IPropertyWindow)
                {
                    return element;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
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

                    component.ShowBorder = false;
                    //break;
                }
               
            }
            if (_component != null)
            {
                var __component = (_component as IPropertyWindow);

                _component = __component.GetComponent() as FrameworkElement;
                __component.ShowBorder = false;
            }
        }


    }

}
