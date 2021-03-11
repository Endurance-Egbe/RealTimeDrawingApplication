using System;
using System.Collections.Generic;
//using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;


namespace View.ViewModels.ShapeServices
{
    public class ShapeComponent : Grid, IPropertyWindow
    {

        private SolidColorBrush shapeFill = Brushes.Blue;

        private bool _showBorder;
        private SolidColorBrush selectedStroke = Brushes.Blue;
        private SolidColorBrush selectedBorderColor = Brushes.Blue;

        public ShapeComponent(Geometry geometry, ComponentEnum shape)
        {
            Geometry = geometry;
            Shape = shape;
            Width = 60;
            Height = 60;
            //Fill = shapeFill.ColorType;
            Shapes = this;
            Title = Shape.ToString();
            Id = Guid.NewGuid();
            
        }

        public Geometry Geometry { get; set; }
        public string Title { get; set; }
        public double X { get; set; }
        public double Y { get; set; }


        public ComponentEnum Shape { get; set; }
        public Brush Border { get; set; }
        public int Fontsize { get; set; }
        public Guid Id { get; set; }
        public SolidColorBrush SelectedStroke { get => selectedStroke; set => selectedStroke = value; }
        public SolidColorBrush SelectedFillColor { get { return shapeFill; } set { shapeFill = value; } }
        public SolidColorBrush SelectedBorderColor { get => selectedBorderColor; set => selectedBorderColor = value; }
        public int LineThickness { get; set; }
        //public ComponentType ComponentType { get; set; }

        public ShapeComponent Shapes { get; set; }
        //protected override Geometry DefiningGeometry => Geometry;

        public object GetComponent()
        {
            Shapes.Children.Clear();
            var border = new Border();
            var path = new Path() { Data = Geometry };
            path.Stretch = Stretch.Fill;
            //var shape = path as ShapeComponent;
            path.Fill = shapeFill;
            path.Stroke = shapeFill;
            path.StrokeThickness = LineThickness;
            path.Width = Width;
            path.Height = Height;
            path.Stroke = SelectedStroke;
            path.StrokeThickness = LineThickness;

            path.Height = Height - 10;
            path.Width = Width - 10;
            path.HorizontalAlignment = HorizontalAlignment.Center;
            path.VerticalAlignment = VerticalAlignment.Center;
            border.Child = path;
            Shapes.Children.Add(border);
            if (ShowBorder)
            {
                border.BorderBrush = Brushes.Red;
                border.BorderThickness = new Thickness(1);
                Shapes.Children.Add(new Border
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 5,
                    Height = 5,
                    Background = Brushes.Purple
                });
                Shapes.Children.Add(new Border
                {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 5,
                    Height = 5,
                    Background = Brushes.Purple
                });
                Shapes.Children.Add(new Border
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Width = 5,
                    Height = 5,
                    Background = Brushes.Purple
                });
                Shapes.Children.Add(new Border
                {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Width = 5,
                    Height = 5,
                    Background = Brushes.Purple
                });
            }


            return path;
        }
        //public event PropertyChangedEventHandler PropertyChanged;

        public bool ShowBorder { get => _showBorder; set { _showBorder = value;  } }

        public ComponentEnum ComponentEnum { get ; set ; }

       
    }
}
