using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using View.Models;
using View.ViewModels.DatabaseServices;
using View.ViewModels.ProxyModel;

namespace View.Export_Import
{
    public class XmlService
    {
       
        public static void ExportWithXml(ProjectProxyModel projectProxyModel)
        {
            List<DrawingComponentModel>drawingComponents = DrawingComponentService.GetDrawingComponentModels(projectProxyModel);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<DrawingComponentModel>));
            SaveFileDialog saveFile = new SaveFileDialog()
            {
                Title = "Save File",
                Filter = "Xml Document(*.xml) | *.xml",
                FileName = " "
            };
            if (saveFile.ShowDialog() == true)
            {
                StreamWriter streamWriter = new StreamWriter(File.Create(saveFile.FileName));
                //streamWriter.Write(serialized);
                xmlSerializer.Serialize(streamWriter, drawingComponents);
                //streamWriter.Write(serialized);
                streamWriter.Dispose();
            }
            
        }
        public static IEnumerable<DrawingComponentProxyModel> ImportWithXml()
        {
            List<DrawingComponentProxyModel> drawingComponents = new List<DrawingComponentProxyModel>();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<DrawingComponentModel>));
            OpenFileDialog openFile = new OpenFileDialog()
            {
                Title = "Open File",
                Filter = "Xml Document(*.xml) | *.xml",
                FileName = " "
            };
            if (openFile.ShowDialog() == true)
            {
                StreamReader streamWriter = new StreamReader(File.OpenRead(openFile.FileName));
                
                var deserialize=xmlSerializer.Deserialize(streamWriter);
                List<DrawingComponentModel>drawingComponentsModel = (List<DrawingComponentModel>)deserialize;
                drawingComponents = DrawingComponentService.ComponentValidationFromDatabase(drawingComponentsModel);
                //streamWriter.Write(serialized);
                streamWriter.Dispose();
            }
            return drawingComponents;
        }
    }
}
