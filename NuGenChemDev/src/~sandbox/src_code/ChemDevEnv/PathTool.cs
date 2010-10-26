using System;
using System.Collections.Generic;
using System.Text;

namespace ChemDevEnv
{
    class PathTool
    {
        public static string ShortenPath(string path, int maxLength)
        {
            // check if over length
            if (path.Length <= maxLength)
                return path;

            // chop of dirs after drive letter
            int chopStart = path.IndexOf('\\');
            int newSize = path.Length;
            int idx = chopStart + 1;

            while (newSize > maxLength)
            {
                int pos = path.IndexOf('\\', idx);
                if (pos != -1)
                    newSize = path.Length - pos;
                else
                    break;
                idx = pos + 1;
            }

            return path.Substring(0, chopStart + 1) + "..." + path.Substring(path.Length - newSize);
        }
    }
}
