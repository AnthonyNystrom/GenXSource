/* -----------------------------------------------
 * INuGenTaskXmlService.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Xml;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// Indicates that this class provides service methods for XML processing.
	/// </summary>
	public interface INuGenTaskXmlService
	{
		/// <summary>
		/// </summary>
		void AppendChild(XmlNode nodeToAppendChildTo, string nodeName, string nodeText);

		/// <summary>
		/// </summary>
		XmlNode AppendChild(XmlNode nodeToAppendChildTo, string nodeName);
		
		/// <summary>
		/// </summary>
		string GetChildText(XmlNode nodeToContainChild, string childNodeName, string defaultChildNodeText);
	}
}
