using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace View.Models
{
    
    public class DrawingComponentModel
    {
        [JsonIgnore]
        [XmlIgnore]
        public int DrawingComponentId { get; set; }
        [XmlAttribute]
        public string Title { get; set; }
        [XmlAttribute]
        public double X { get; set; }
        [XmlAttribute]
        public double Y { get; set; }
        [XmlAttribute]
        public double Width { get; set; }
        [XmlAttribute]
        public double Height { get; set; }
        [XmlAttribute]
        public int LineThickness { get; set; }
        [XmlAttribute]
        public int StrokeThickness { get; set; }
        [XmlAttribute]
        public string SelectedStroke { get; set; }
        [XmlAttribute]
        public string SelectedFillColor { get; set; }
        [XmlAttribute]
        public string SelectedBorderColor { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public virtual ProjectModel Project { get; set; }

        
    }
}
