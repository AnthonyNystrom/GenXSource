using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Settings;

namespace Genetibase.NuGenRenderCore.Resources
{
    public interface IResource : IDisposable
    {
        string Id { get; }
        IResource[] Dependants { get; }
        ISharableResource[] SharedDependancies { get; }
    }

    public abstract class Resource : IResource
    {
        private readonly string id;
        private readonly IResource[] dependants;
        private readonly ISharableResource[] sharedDependancies;

        /// <summary>
        /// Initializes a new instance of the Resource class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dependants"></param>
        /// <param name="sharedDependancies"></param>
        public Resource(string id, IResource[] dependants, ISharableResource[] sharedDependancies)
        {
            this.id = id;
            this.dependants = dependants;
            this.sharedDependancies = sharedDependancies;
        }

        #region IResource Members

        public string Id
        {
            get { return id; }
        }

        public IResource[] Dependants
        {
            get { return dependants; }
        }

        public ISharableResource[] SharedDependancies
        {
            get { return sharedDependancies; }
        }
        #endregion

        #region IDisposable Members

        public abstract void Dispose();
        #endregion
    }

    public abstract class RzContentTypeLoader
    {
        private string[] contentTypes;
        private string[] fileExtensions;
        private int[] fileExtTypes;

        public RzContentTypeLoader(string[] contentTypes, string[] fileExtensions, int[] fileExtTypes)
        {
            this.contentTypes = contentTypes;
            this.fileExtensions = fileExtensions;
            this.fileExtTypes = fileExtTypes;
        }

        public abstract IResource LoadContent(string filePath, string rzPath, string subPath,
                                              string contentType, XmlNodeList rzNode,
                                              out IResource[] loadedDependants,
                                              out IResource[] loadedDependancies,
                                              DeviceInterface devIf);

        #region Properties

        public string[] ContentTypes
        {
            get { return contentTypes; }
        }

        public string[] FileExtensions
        {
            get { return fileExtensions; }
        }
        #endregion

        public string GetExtension(int extIdx)
        {
            return contentTypes[fileExtTypes[extIdx]];
        }
    }

    public class RzLoader
    {
        Dictionary<string, RzContentTypeLoader> contentLoaders;
        Dictionary<string, string> fileExtContentTypes;

        public RzLoader()
        {
            contentLoaders = new Dictionary<string, RzContentTypeLoader>();
            fileExtContentTypes = new Dictionary<string, string>();
        }

        public void RegisterContentLoader(RzContentTypeLoader loader)
        {
            foreach (string type in loader.ContentTypes)
            {
                contentLoaders.Add(type, loader);
            }
            string[] fileExts = loader.FileExtensions;
            if (fileExts != null)
            {
                int idx = 0;
                foreach (string ext in fileExts)
                {
                    fileExtContentTypes.Add(ext, loader.GetExtension(idx++));
                }
            }
        }

        public IResource LoadResource(Uri uri, DeviceInterface devIf, out IResource[] loadedDependants,
                                      out IResource[] loadedRependancies, out string realRzId)
        {
            // break down uri into any sub-file paths etc.
            string endPath = uri.Segments[uri.Segments.Length - 1];
            string filename;
            int start;
            string subPath = null;
            if ((start = endPath.IndexOf(':')) != -1)
            {
                subPath = endPath.Substring(start);
                filename = endPath.Substring(0, start);
            }
            else
                filename = endPath;

            //string rzPath = uri.LocalPath.Substring(2, uri.LocalPath.Length - endPath.Length + filename.Length - 2);
            string filepath;
            if (subPath == null)
                filepath = (string)HashTableSettings.Instance["Base.Path"] + uri.LocalPath.Substring(2);
            else
                filepath = (string)HashTableSettings.Instance["Base.Path"] + uri.LocalPath.Substring(2, uri.LocalPath.Length - 2 - subPath.Length);
            string metafile = filepath + ".xml";

            // find & read xml meta-data
            string contentType;
            XmlNodeList properties = null;
            if (!File.Exists(metafile) || !ReadMetaXml(metafile, out contentType, out properties))
            {
                // read / guess content-type
                string ext = Path.GetExtension(filename);
                if (!fileExtContentTypes.TryGetValue(ext, out contentType))
                    throw new Exception("Unable to determine content-type for extension: " + ext);
            }

            // load content-type
            RzContentTypeLoader loader;
            if (!contentLoaders.TryGetValue(contentType, out loader))
                throw new Exception("Unable to determine content-type loader for type: " + contentType);

            string rzPath = subPath != null ? uri.OriginalString.Substring(0, uri.OriginalString.Length - subPath.Length) : uri.OriginalString;
            IResource rz = loader.LoadContent(filepath, rzPath, subPath, contentType, properties,
                                              out loadedDependants, out loadedRependancies, devIf);
            if (subPath != null)
                realRzId = rzPath;
            else
                realRzId = uri.OriginalString;
            return rz;
        }

        private bool ReadMetaXml(string file, out string contentType, out XmlNodeList properties)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(file);
            XmlElement resource = (XmlElement)xml.SelectSingleNode("resource");
            contentType = null;
            properties = null;
            if (resource != null)
            {
                string mime = resource.Attributes["mime"].InnerText;
                MatchCollection keys = Regex.Matches(mime, "([a-zA-Z-]+):([a-z/]+)");
                if (keys != null)
                {
                    foreach (Match key in keys)
                    {
                        if (key.Groups[1].Value == "Content-Type")
                            contentType = key.Groups[2].Value;
                    }
                }
                properties = resource.ChildNodes;
                return true;
            }
            return false;
        }
    }
}