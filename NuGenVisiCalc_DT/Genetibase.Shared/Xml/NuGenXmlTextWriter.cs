/* -----------------------------------------------
 * NuGenXmlTextWriter.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Genetibase.Shared.Xml
{
	/// <summary>
	/// Custom <see cref="T:XmlTextWriter"/> that supports indentation and UTF-8 encoding.
	/// </summary>
	public class NuGenXmlTextWriter : XmlTextWriter
	{
		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenXmlTextWriter"/> class.
		/// </summary>
		/// <param name="stream">Specifies the stream to write to.</param>
		public NuGenXmlTextWriter(Stream stream)
			: base(stream, Encoding.UTF8)
		{
			this.Formatting = Formatting.Indented;
		}
		
		#endregion
	}
}
