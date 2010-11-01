using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Genetibase.Chem.NuGenSChem
{
    public static class Utility
    {
        private static string _applicationDirectory;
        public static string ApplicationDirectory
        {
            get
            {
                if (_applicationDirectory == null)
                    _applicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

                return _applicationDirectory;
            }
        }

        public static string GetFullPath(string path)
        {
            string absPath = ApplicationDirectory;
            if (!absPath.EndsWith("\\"))
                absPath += "\\";

            path = path.Replace('/', '\\');

            if (path.StartsWith("\\"))
                path = path.Substring(1);

            absPath += path;

            return absPath;
        }

        public static double SafeDouble(object obj)
        {
            if (obj == null)
                return 0; 

            double result;

            if (double.TryParse(obj.ToString(), out result))
            {
                return result; 
            }

            try
            {
                return Convert.ToDouble(obj); 
            }
            catch (Exception)
            {
                return 0; 
            }
        }

        public static int SafeInt(object obj)
        {
            if (obj == null)
                return 0;

            int result;

            if (int.TryParse(obj.ToString(), out result))
            {
                return result;
            }

            try
            {
                return Convert.ToInt32(obj);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
