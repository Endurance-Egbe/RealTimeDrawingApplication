using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.ViewModels.Common
{
    public class ComponentModel
    {
        public Guid PropertyId { get; set; }
        public object Value { get; set; }
        public PropertyType PropertyType { get; set; }
        public ComponentEnum ComponentEnum { get; set; }
    }
    public enum PropertyType
    {
        Title, X, Y, Width, Height, StrokeThickness, Stroke, FillColor, BorderColor, ShowBorder
    }
}
