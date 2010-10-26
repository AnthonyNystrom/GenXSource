using System;
using System.IO;

namespace Genetibase.NuGenDEMVis.Data
{
    public abstract class DataProfile
    {
        private readonly string name;
        private readonly string desc;
        private readonly FileType[] fileTypes;
        protected SubProfile[] subProfiles;

        public class SubProfile
        {
            public string Name;
            public string Desc;
            public IProfileDataFilter Filter;

            /// <summary>
            /// Initializes a new instance of the SubProfile class.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="desc"></param>
            public SubProfile(string name, string desc, IProfileDataFilter dataFilter)
            {
                Name = name;
                Desc = desc;
                Filter = dataFilter;
            }
        }

        public interface IProfileDataFilter
        {
            object FilterData(IDataSourceReader dataSrc);
        }

        /// <summary>
        /// Initializes a new instance of the DataProfile class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        /// <param name="fileTypes"></param>
        public DataProfile(string name, string desc, FileType[] fileTypes)
        {
            this.name = name;
            this.desc = desc;
            this.fileTypes = fileTypes;
        }

        /// <summary>
        /// Initializes a new instance of the DataProfile class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        /// <param name="availableTypes"></param>
        /// <param name="extensions"></param>
        public DataProfile(string name, string desc, FileType[] availableTypes, string[] extensions)
        {
            this.name = name;
            this.desc = desc;
            
            // find types
            fileTypes = new FileType[extensions.Length];
            for (int i = 0; i < extensions.Length; i++)
            {
                foreach (FileType type in availableTypes)
                {
                    foreach (string ext in type.Extensions)
                    {
                        if (ext == extensions[i])
                        {
                            fileTypes[i] = type;
                            break;
                        }
                    }
                }
                if (fileTypes[i] == null)
                    throw new Exception("FileType not found for " + extensions[i]);
            }
        }

        public string Name
        {
            get { return name; }
        }

        public string Desc
        {
            get { return desc; }
        }

        public FileType[] FileTypes
        {
            get { return fileTypes; }
        }

        public SubProfile[] SubProfiles
        {
            get { return subProfiles; }
        }

        public bool MatchProfile(string file, out FileType fileType)
        {
            string fExt = Path.GetExtension(file);
            // check file extenions
            foreach (FileType fType in fileTypes)
            {
                // check extensions
                foreach (string ext in fType.Extensions)
                {
                    if (ext == fExt)
                    {
                        if (fType.IsFileType(file))
                        {
                            fileType = fType;
                            return true;
                        }
                        break;
                    }
                }
            }
            fileType = null;
            return false;
        }

        public bool MatchProfile(FileType fileType)
        {
            // check file extenions
            foreach (FileType fType in fileTypes)
            {
                if (fType == fileType)
                    return true;
            }
            return false;
        }
    }
}