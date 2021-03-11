using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace View.ViewModels.ComponentServices
{
    public class TextBoxComponent : TextBox, IPropertyWindow
    {
        public TextBoxComponent()//double width = 50, double height = 50, string text = "Text")
        {
            //SelectedFillColor = new CustomColor();
            FontSize = 14;
            Width = 50;
            Height = 20;
            Text = "text";
            // Background = SelectedFillColor.ColorType;
        }
        public string Title { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public ComponentEnum ComponentEnum { get; set; }
        public int Fontsize { get; set; }
        public Guid Id { get; set; }
        public int StrokeThickness { get; set; }
        public SolidColorBrush SelectedStroke { get; set; }
        public SolidColorBrush SelectedFillColor { get; set; }
        public SolidColorBrush SelectedBorderColor { get; set; }
        public int LineThickness { get; set; }
        //public ComponentType ComponentType { get; set; }
        public bool ShowBorder { get; set; }

        public object GetComponent()
        {
            throw null;
        }
    }
}