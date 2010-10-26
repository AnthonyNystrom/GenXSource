using System.IO;
using Genetibase.OSwCommon;

namespace Genetibase.NuGenDEMVis.Data
{
    public class FileType : IFileFilterType
    {
        private readonly string[] extensions;
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of the FileType class.
        /// </summary>
        /// <param name="extensions"></param>
        /// <param name="name"></param>
        public FileType(string name, string[] extensions)
        {
            this.extensions = extensions;
            this.name = name;
        }

        public string[] Extensions
        {
            get { return extensions; }
        }

        public string Name
        {
            get { return name; }
        }

        public virtual bool IsFileType(string file)
        {
            // check extension against this types extensions
            string ext = Path.GetExtension(file).Substring(1).ToLower();
            foreach (string extention in extensions)
            {
                if (ext == extention)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
