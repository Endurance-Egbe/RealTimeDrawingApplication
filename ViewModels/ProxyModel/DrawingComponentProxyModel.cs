using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using View.ViewModels.Common.CustomColor;

namespace View.ViewModels.ProxyModel
{
    public class DrawingComponentProxyModel
    {
        public string Title { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }
        public int LineThickness { get; set; }
        public int StrokeThickness { get; set; }
        public CustomColor SelectedStroke { get; set; }
        public CustomColor SelectedFillColor { get; set; }
        public CustomColor SelectedBorderColor { get; set; }
        //public ProjectProxyModel ProjectProxyModel  { get; set; }
    }
}
