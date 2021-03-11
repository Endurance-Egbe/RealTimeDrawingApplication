using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Models
{
    public class DrawingComponentModel
    {
        public int DrawingComponentId { get; set; }

        public string Title { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }
        public int LineThickness { get; set; }
        public int StrokeThickness { get; set; }
        public string SelectedStroke { get; set; }
        public string SelectedFillColor { get; set; }
        public string SelectedBorderColor { get; set; }
        public virtual ProjectModel Project { get; set; }
    }
}
