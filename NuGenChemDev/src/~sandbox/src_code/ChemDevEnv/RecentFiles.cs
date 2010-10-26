using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace ChemDevEnv
{
    [XmlRoot("recentFilesList")]
    public class RecentFiles
    {
        public enum RecentFileType
        {
            Molecule,
            Other
        }

        public class RecentFile
        {
            [XmlAttribute("type")]
            public RecentFileType Type;
            [XmlAttribute("filename")]
            public string Filename;
        }

        List<RecentFile> files;

        public RecentFiles ()
        {
            files = new List<RecentFile>();
        }

        public void ClearDeadEntires()
        {
            for (int i=0; i < files.Count; i++)
            {
                if (!File.Exists(files[i].Filename))
                    files.Remove(files[i]);
            }
        }

        [XmlArray("files"), XmlArrayItem("file")]
        public List<RecentFile> Files
        {
            get { return files; }
            set { files = value; }
        }

        public static RecentFiles LoadFromFile(string filename)
        {
            FileStream file = new FileStream(filename, FileMode.Open);
            XmlSerializer xs = new XmlSerializer(typeof(RecentFiles));
            XmlReader xmlreader = XmlReader.Create(file);
            RecentFiles recentFiles = (RecentFiles)xs.Deserialize(xmlreader);

            file.Close();
            return recentFiles;
        }

        public void SaveToFile(string filename)
        {
            FileStream file = new FileStream(filename, FileMode.Create);
            XmlSerializer xs = new XmlSerializer(typeof(RecentFiles));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(file, Encoding.UTF8);

            xs.Serialize(xmlTextWriter, this);

            xmlTextWriter.Close();
            file.Close();
        }

        public void AddFile(string filename, RecentFileType type)
        {
            // check not in list already - if so remove and add to end
            int idx = 0;
            bool remove = false;
            foreach (RecentFile file in files)
            {
                if (file.Filename.CompareTo(filename) == 0)
                {
                    remove = true;
                    break;
                }
                idx++;
            }
            if (remove)
                files.RemoveAt(idx);

            RecentFile _file = new RecentFile();
            _file.Filename = filename;
            _file.Type = type;

            files.Add(_file);
        }
    }
}
