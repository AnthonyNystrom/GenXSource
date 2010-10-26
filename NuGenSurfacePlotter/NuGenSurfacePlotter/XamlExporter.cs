using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Markup;

namespace NuGenCRBase.Xaml
{
    class XamlExporter
    {
        public static void ExportToFile(object obj, string filename)
        {
			FileStream fs = File.Open(filename, FileMode.Create);
			XamlWriter.Save(obj, fs);
			fs.Close();
        }
    }
}
