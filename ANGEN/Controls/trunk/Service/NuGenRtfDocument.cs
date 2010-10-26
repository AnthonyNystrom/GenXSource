/* -----------------------------------------------
 * NuGenRtfDocument.cs
 * Author: Alex Nesterov
 * --------------------------------------------- */

using System;
using System.Drawing;

namespace Genetibase.Controls.Service
{
	/// <summary>
	/// Encapsulates RTF document.
	/// </summary>
	class NuGenRtfDocument
	{
		#region Declarations

		private NuGenRtfFontTable fonttbl;
		private NuGenRtfColorTable colortbl;
		private string header;
		private string document;

		#endregion

		#region Methods.Public

		/// <summary>
		/// Appends the specified text to this <see cref="T:NuGenRtfDocument"/>.
		/// </summary>
		/// <param name="text">Specifies the text to add to this <see cref="T:NuGenRtfDocument"/>.</param>
		public void AppendText(string text)
		{
			document += text;
		}

		/// <summary>
		/// Sets the specified font for this <see cref="T:NuGenRtfDocument"/>.
		/// </summary>
		/// <param name="fontName">Specifies the font name for this <see cref="T:NuGenRtfDocument"/>.</param>
		/// <returns></returns>
		public int UseFont(string fontName)
		{
			return fonttbl.UseFont(fontName);
		}

		/// <summary>
		/// Sets the specified color for this <see cref="T:NuGenRtfDocument"/>.
		/// </summary>
		/// <param name="fromArgb">Specifies the color for this <see cref="T:NuGenRtfDocument"/>.</param>
		/// <returns></returns>
		public int UseColor(Color fromArgb)
		{
			return colortbl.UseColor(fromArgb);
		}

		#endregion

		#region Methods.Public.Overriden
		
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			header += fonttbl.ToString() + colortbl.ToString();
			return header + "{" + document + "}}";
		}

		#endregion

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRtfDocument"/> class.
		/// </summary>
		public NuGenRtfDocument()
		{
			header = "{\\rtf1";
			fonttbl = new NuGenRtfFontTable();
			colortbl = new NuGenRtfColorTable();
		}
		
		#endregion
	}
}
