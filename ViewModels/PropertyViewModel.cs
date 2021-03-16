using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using View.ViewModels.Common;
using View.ViewModels.Common.CustomColor;
using View.ViewModels.Common.Event_Container;

namespace View.ViewModels
{
    public class PropertyViewModel : INotifyPropertyChanged
    {
        private string _title;
        private double _x;
        private double _y;
        private double _width;
        private double _height;
        private CustomColor _selectedFillColor = new CustomColor(); //{ ColorName = "Red", ColorType = Brushes.Red };
        private CustomColor _selectedBorderColor = new CustomColor(); //{ ColorName = "Red", ColorType = Brushes.Red };
        private int _strokeThickness;
        private CustomColor _selectedStrokeColor = new CustomColor();// { ColorName = "Red", ColorType = Brushes.Red };
        private int _lineThickness;


        public event PropertyChangedEventHandler PropertyChanged;
        public Dictionary<string, PropertyType> PropertyKeyValuePair = new Dictionary<string, PropertyType>
        {
            { nameof(Width),PropertyType.Width},
            { nameof(Height),PropertyType.Height},
            { nameof(Y),PropertyType.Y},
            { nameof(X),PropertyType.X},
            { nameof(Title),PropertyType.Title},
            { nameof(SelectedFillColor),PropertyType.FillColor},
            { nameof(SelectedBorderColor),PropertyType.BorderColor},
            {nameof(SelectedStrokeColor), PropertyType.Stroke },
            { nameof(StrokeThickness),PropertyType.StrokeThickness},
            { nameof(LineThickness),PropertyType.LineThickness}
        };


        public object GetValue(string propertyName)
        {

            switch (propertyName)
            {
                case nameof(X):
                    return X;
                case nameof(Y):
                    return Y;
                case nameof(Width):
                    return Width;
                case nameof(Height):
                    return Height;
                case nameof(SelectedBorderColor):
                    return SelectedBorderColor.ColorType;
                case nameof(SelectedFillColor):
                    return SelectedFillColor.ColorType;
                case nameof(SelectedStrokeColor):
                    return SelectedStrokeColor.ColorType;
                case nameof(StrokeThickness):
                    return StrokeThickness;
                    case nameof(LineThickness):
                    return LineThickness;
                default:
                    return null;
            }
        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {

            //var _value = GetValue(propertyName);
            var value = GetValue(propertyName);
            if (value != null)
            {
                CurrentSelectedItem.PropertyType = PropertyKeyValuePair[propertyName];
                //if (value is Brush)
                //{
                //    CurrentSelectedItem.Value = (value as Brush);
                //}
                CurrentSelectedItem.Value = value;
                EventAggregator.GetEvent<ComponentPropertyPubSubEvent>().Publish(CurrentSelectedItem);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public IEventAggregator EventAggregator { get; set; }
        public PropertyViewModel(IEventAggregator eventAggregator)
        {
            //EventUtility();


            ColorCollections = ColorCollectionsClass.LoadColorCollection();
            EventAggregator = eventAggregator;
            EventAggregator.GetEvent<UpdatePropertyWindow>().Subscribe(UpdatePropertyView);
            PropertyWindow = new ComponentModel();
            // SelectedFillColor = new CustomColor();
        }

        private void UpdatePropertyView(ComponentModel property)
        {
            if (property != null && property.PropertyId != null)
            {
                CurrentSelectedItem = property;
                switch (property.PropertyType)
                {
                    case PropertyType.Title:
                        Title = (string)property.Value;
                        break;
                    case PropertyType.X:
                        X = (double)property.Value;
                        break;
                    case PropertyType.Y:
                        Y = (double)property.Value;
                        break;
                    case PropertyType.Width:
                        Width = (double)property.Value;
                        break;
                    case PropertyType.Height:
                        Height = (double)property.Value;
                        break;
                    case PropertyType.StrokeThickness:
                        StrokeThickness = (int)property.Value;
                        break;
                    case PropertyType.Stroke:
                        //SelectedStrokeColor = (SolidColorBrush)property.Value;
                        SelectedStrokeColor = ColorCollections.FirstOrDefault(x => x.ColorType == property.Value as SolidColorBrush);
                        break;
                    case PropertyType.FillColor:
                        //SelectedStrokeColor =((CustomColor) property.Value).ColorType;
                        SelectedFillColor = ColorCollections.FirstOrDefault(x => x.ColorType == property.Value as SolidColorBrush);
                        break;
                    case PropertyType.BorderColor:
                        //SelectedBorderColor = (SolidColorBrush)property.Value;
                        SelectedBorderColor = ColorCollections.FirstOrDefault(x => x.ColorType == property.Value as SolidColorBrush);
                        break;
                    case PropertyType.LineThickness:
                        LineThickness = (int)property.Value;
                        break;
                    default:
                        break;
                }
            }
        }

        public ObservableCollection<CustomColor> ColorCollections { get; set; }

        //private IEventAggregator eventAggregator;

        //public SolidColorBrush FillColor { get => fillColor; set => fillColor = value; }
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        public double X
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged();
            }
        }
        public double Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged();
            }
        }
        public double Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }
        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }
        public int StrokeThickness
        {
            get => _strokeThickness;
            set
            {
                _strokeThickness = value;
                OnPropertyChanged();
            }
        }
        public int LineThickness
        {
            get => _lineThickness;
            set
            {
                _lineThickness = value;
                OnPropertyChanged();
            }
        }
        public CustomColor SelectedStrokeColor
        {
            get => _selectedStrokeColor;
            set
            {
                _selectedStrokeColor = value;
                OnPropertyChanged();
            }
        }
        public CustomColor SelectedFillColor
        {
            get => _selectedFillColor;
            set
            {
                _selectedFillColor = value;
                OnPropertyChanged();
            }
        }
        public CustomColor SelectedBorderColor
        {
            get => _selectedBorderColor;
            set
            {
                _selectedBorderColor = value;
                OnPropertyChanged();
            }
        }

        public ComponentModel PropertyWindow { get; set; }
        public ComponentModel CurrentSelectedItem { get; set; }

    }
}
