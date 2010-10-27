/* ------------------------------------------------
 * XmlService.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Diagnostics;

namespace Next2Friends.CrossPoster.Client.Xml
{
    static class XmlService
    {
        /// <summary>
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nodeToAppendChildTo</code> is <code>null</code>, or
        /// if the specified <code>nodeName</code> is <code>null</code>.
        /// </exception>
        public static void AppendChild(XmlNode nodeToAppendChildTo, String nodeName, String nodeText)
        {
            if (nodeToAppendChildTo == null)
                throw new ArgumentNullException("nodeToAppendChildFor");
            if (nodeName == null)
                throw new ArgumentNullException("nodeName");

            var childNode = AppendChild(nodeToAppendChildTo, nodeName);
            childNode.InnerText = XmlConvert.EncodeName(nodeText);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nodeToAppendChildTo</code> is <code>null</code>, or
        /// if the specified <code>nodeName</code> is <code>null</code>.
        /// </exception>
        public static XmlNode AppendChild(XmlNode nodeToAppendChildTo, String nodeName)
        {
            if (nodeToAppendChildTo == null)
                throw new ArgumentNullException("nodeToAppendChildTo");
            if (nodeName == null)
                throw new ArgumentNullException("nodeName");

            var ownerXmlDoc = nodeToAppendChildTo.OwnerDocument;
            var childNode = ownerXmlDoc.CreateElement(nodeName);
            nodeToAppendChildTo.AppendChild(childNode);

            return childNode;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nodeToContainChild</code> is <code>null</code>, or
        /// if the specified <code>childNodeName</code> is <code>null</code>.
        /// </exception>
        public static String GetChildText(XmlNode nodeToContainChild, String childNodeName, String defaultChildNodeText)
        {
            if (nodeToContainChild == null)
                throw new ArgumentNullException("nodeToContainChild");
            if (childNodeName == null)
                throw new ArgumentNullException("childNodeName");

            var childNode = nodeToContainChild[childNodeName];
            var childText = defaultChildNodeText;

            if (childNode != null && childNode.InnerText != null)
                childText = XmlConvert.DecodeName(childNode.InnerText);

            return childText;
        }
    }
}
