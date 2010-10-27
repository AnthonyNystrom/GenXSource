/* ------------------------------------------------
 * BlogDescriptor.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Globalization;
using Next2Friends.CrossPoster.Client.Xml;

namespace Next2Friends.CrossPoster.Client.Logic
{
    sealed class BlogDescriptor
    {
        /// <summary>
        /// Creates a new instance of the <code>BlogDescriptor</code> class.
        /// </summary>
        public BlogDescriptor()
        {
        }

        public String Address { get; set; }
        public String BlogName { get; set; }
        public BlogType BlogType { get; set; }
        public String Password { get; set; }
        public String Username { get; set; }

        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            Address = XmlService.GetChildText(xmlNode, "address", "");
            BlogName = XmlService.GetChildText(xmlNode, "blog_name", "");
            BlogType = (BlogType)Enum.Parse(typeof(BlogType), XmlService.GetChildText(xmlNode, "blog_type", "Blogger"));
            Password = XmlService.GetChildText(xmlNode, "password", "");
            Username = XmlService.GetChildText(xmlNode, "username", "");
        }

        public void SaveToXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            XmlService.AppendChild(xmlNode, "address", Address);
            XmlService.AppendChild(xmlNode, "blog_name", BlogName);
            XmlService.AppendChild(xmlNode, "blog_type", BlogType.ToString());
            XmlService.AppendChild(xmlNode, "password", Password);
            XmlService.AppendChild(xmlNode, "username", Username);
        }
    }
}
