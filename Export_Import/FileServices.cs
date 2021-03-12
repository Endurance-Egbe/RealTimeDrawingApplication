using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Export_Import
{
    public class FileServices
    {

        public static void SaveFile(string serialized)
        {
            SaveFileDialog saveFile = new SaveFileDialog()
            {
                Title="Save File",
                Filter="Text Document(*.txt) | *.txt",
                FileName=" "
            };
            if (saveFile.ShowDialog()==true)
            {
                StreamWriter streamWriter = new StreamWriter(File.Create(saveFile.FileName));
                streamWriter.Write(serialized);
                streamWriter.Dispose();
            }
            
        }
        public static string OpenFile()
        {
            string deserialized = null;
            OpenFileDialog openFile = new OpenFileDialog()
            {
                Title = "Open File",
                Filter = "Text Document(*.txt) | *.txt",
                FileName = " "
            };
            if (openFile.ShowDialog()==true)
            {
                StreamReader streamReader = new StreamReader(File.OpenRead(openFile.FileName));
                deserialized = streamReader.ReadToEnd();
                streamReader.Dispose();
            }
            return deserialized;
        }
    }
}
