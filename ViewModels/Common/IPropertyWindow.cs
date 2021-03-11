using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace View.ViewModels
{
    public interface IPropertyWindow
    {
        Guid Id { get; set; }
        int Fontsize { get; set; }
        string Title { get; set; }
        double X { get; set; }
        double Y { get; set; }
        ComponentEnum ComponentEnum { get; set; }
        double Width { get; set; }
        double Height { get; set; }
        int LineThickness { get; set; }
        //ComponentType ComponentType { get; set; }
        SolidColorBrush SelectedStroke { get; set; }
        SolidColorBrush SelectedFillColor { get; set; }
        SolidColorBrush SelectedBorderColor { get; set; }
        bool ShowBorder { get; set; }
        object GetComponent();

    }
    public enum ComponentEnum { Ecllipse, Triangle, Rectangle, Line, TextBox }
}
