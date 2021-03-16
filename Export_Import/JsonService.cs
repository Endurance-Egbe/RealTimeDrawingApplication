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
    public class JsonService
    {
        private  static string JsonSerializedObject (ProjectProxyModel projectProxyModel)
        {
            var drawingComponents = DrawingComponentService.GetDrawingComponentModels(projectProxyModel);
            string serializedDrawings = JsonConvert.SerializeObject(drawingComponents);
            return serializedDrawings;
        }
        private static IEnumerable<DrawingComponentProxyModel> JsonDeserializedObject(string serializedDrawings)
        {
            var deserializedDrawings = JsonConvert.DeserializeObject<List<DrawingComponentModel>>(serializedDrawings);
            var drawingProxyModel = DrawingComponentService.ComponentValidationFromDatabase(deserializedDrawings);
            return drawingProxyModel;
        }
        public static void ExportWithJson(ProjectProxyModel projectProxyModel)
        {
            string serializedDrawing = JsonSerializedObject(projectProxyModel);
            FileService.SaveFile(serializedDrawing);
        }
        public static IEnumerable<DrawingComponentProxyModel> ImportWithJson()
        {
            string filePath = FileService.OpenFile();
            var drawingProxyModel = JsonDeserializedObject(filePath);
            return drawingProxyModel;
        }
    }
}
