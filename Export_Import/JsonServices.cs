using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.Models;
using View.ViewModels.DatabaseServices;
using View.ViewModels.ProxyModel;

namespace View.Export_Import
{
    public class JsonServices
    {
        public static string JsonSerializedObject (ProjectProxyModel projectProxyModel)
        {
            var drawingComponents = DrawingComponentService.GetDrawingComponentModels(projectProxyModel);
            string serializedDrawings = JsonConvert.SerializeObject(drawingComponents);
            return serializedDrawings;
        }
        public static IEnumerable<DrawingComponentProxyModel> JsonDeserializedObject(string serializedDrawings)
        {
            var deserializedDrawings = JsonConvert.DeserializeObject<List<DrawingComponentModel>>(serializedDrawings);
            var drawingProxyModel = DrawingComponentService.ComponentValidationFromDatabase(deserializedDrawings);
            return drawingProxyModel;
        }
    }
}
