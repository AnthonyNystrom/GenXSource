/* ------------------------------------------------
 * BlogListModel.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Next2Friends.CrossPoster.Client.Properties;
using System.Xml;
using System.IO;

namespace Next2Friends.CrossPoster.Client.Logic
{
    sealed class BlogListModel : ListModel<BlogDescriptor>
    {
        /// <summary>
        /// Creates a new instance of the <code>BlogListModel</code> class.
        /// </summary>
        public BlogListModel()
        {
        }

        public void LoadFromXml(String path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            _list = new List<BlogDescriptor>();
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            var blogListNode = xmlDoc.SelectSingleNode("blog_list");
            var nodeList = blogListNode.SelectNodes("blog");

            foreach (XmlNode node in nodeList)
            {
                var blogDescriptor = new BlogDescriptor();
                blogDescriptor.LoadFromXml(node);
                AddItem(blogDescriptor);
            }
        }

        public void SaveToXml(String path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            var xmlDoc = new XmlDocument();
            var blogListNode = xmlDoc.CreateElement("blog_list");
            xmlDoc.AppendChild(blogListNode);

            foreach (BlogDescriptor blogDescriptor in _list)
                SaveBlogToXml(blogListNode, blogDescriptor);

            var directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            xmlDoc.Save(path);
        }

        private static void SaveBlogToXml(XmlNode node, BlogDescriptor blogDescriptor)
        {
            var blogNode = node.AppendChild(node.OwnerDocument.CreateElement("blog"));
            blogDescriptor.SaveToXml(blogNode);
        }
    }
}
