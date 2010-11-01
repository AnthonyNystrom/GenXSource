/* -----------------------------------------------
 * NuGenTaskXmlService.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.NuGenTaskList.Properties;

using System;
using System.Diagnostics;
using System.Xml;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// Provides service methods for XML processing.
	/// </summary>
	public class NuGenTaskXmlService : INuGenTaskXmlService
	{
		#region Methods.Public

		/*
		 * AppendChild
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="nodeToAppendChildTo"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="nodeName"/> is <see langword="null"/>.
		/// </exception>
		public void AppendChild(XmlNode nodeToAppendChildTo, string nodeName, string nodeText)
		{
			if (nodeToAppendChildTo == null)
			{
				throw new ArgumentNullException("nodeToAppendChildFor");
			}

			if (nodeName == null)
			{
				throw new ArgumentNullException("nodeName");
			}

			XmlNode childNode = this.AppendChild(nodeToAppendChildTo, nodeName);
			childNode.InnerText = XmlConvert.EncodeName(nodeText);
		}

		/// <summary>
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="nodeToAppendChildTo"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="nodeName"/> is <see langword="null"/>.
		/// </exception>
		public XmlNode AppendChild(XmlNode nodeToAppendChildTo, string nodeName)
		{
			if (nodeToAppendChildTo == null)
			{
				throw new ArgumentNullException("nodeToAppendChildTo");
			}

			if (nodeName == null)
			{
				throw new ArgumentNullException("nodeName");
			}

			XmlDocument ownerXmlDoc = nodeToAppendChildTo.OwnerDocument;
			Debug.Assert(ownerXmlDoc != null, "ownerXmlDoc != null");

			XmlNode childNode = ownerXmlDoc.CreateElement(nodeName);
			nodeToAppendChildTo.AppendChild(childNode);

			return childNode;
		}

		/*
		 * GetChildText
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="nodeToContainChild"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="childNodeName"/> is <see langword="null"/>.
		/// </exception>
		public string GetChildText(XmlNode nodeToContainChild, string childNodeName, string defaultChildNodeText)
		{
			if (nodeToContainChild == null)
			{
				throw new ArgumentNullException("nodeToContainChild");
			}

			if (childNodeName == null)
			{
				throw new ArgumentNullException("childNodeName");
			}

			XmlNode childNode = nodeToContainChild[childNodeName];
			string childText = defaultChildNodeText;

			if (childNode != null && childNode.InnerText != null)
			{
				childText = XmlConvert.DecodeName(childNode.InnerText);
			}

			return childText;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTaskXmlService"/> class.
		/// </summary>
		public NuGenTaskXmlService()
		{

		}

		#endregion
	}
}
