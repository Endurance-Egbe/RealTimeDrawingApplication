using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using View.ViewModels.Common.CustomColor;

namespace View.Models
{
    [Serializable()]
    public partial class PartialDrawingComponentModel:DrawingComponentModel, ISerializable
    {
        
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Title", Title);
            info.AddValue("X", X);
            info.AddValue("Y", Y);
            info.AddValue("Width", Width);
            info.AddValue("Height", Height);
            info.AddValue("LineThickness", LineThickness);
            info.AddValue("StrokeThickness", StrokeThickness);
            info.AddValue("SelectedStroke", SelectedStroke);
            info.AddValue("SelectedFillColor", SelectedFillColor);
            info.AddValue("SelectedBorderColor", SelectedBorderColor);

        }
        public PartialDrawingComponentModel(SerializationInfo info, StreamingContext context)
        {
            Title = (string)info.GetValue("Title", typeof(string));
            X = (double)info.GetValue("X", typeof(double));
            Y = (double)info.GetValue("Y", typeof(double));
            Width = (double)info.GetValue("Width", typeof(double));
            Height = (double)info.GetValue("Height", typeof(double));
            LineThickness = (int)info.GetValue("LineThickness", typeof(int));
            SelectedStroke = (string)info.GetValue("SelectedStroke", typeof(string));
            SelectedFillColor = (string)info.GetValue("SelectedFillColor", typeof(string));
            SelectedBorderColor = (string)info.GetValue("SelectedBorderColor", typeof(string));

        }
    }
}
