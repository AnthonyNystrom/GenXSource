/* -----------------------------------------------
 * NuGenXmlTextReader.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Genetibase.Shared.Xml
{
	/// <summary>
	/// Custom <see cref="T:XmlTextReader"/>.
	/// </summary>
	public class NuGenXmlTextReader : XmlTextReader
	{
		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenXmlTextReader"/> class.
		/// </summary>
		/// <param name="stream">Specifies the stream to read from.</param>
		public NuGenXmlTextReader(Stream stream)
			: base(stream)
		{
		}
		
		#endregion
	}
}
