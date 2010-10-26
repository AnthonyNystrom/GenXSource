/* -----------------------------------------------
 * NuGenRtfFontTable.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections;
using System.Diagnostics;

namespace Genetibase.Controls.Service
{
	/// <summary>
	/// Encapsulates RTF font.
	/// </summary>
	class NuGenRtfFontTable
	{
		#region Declarations

		private int numberOfFonts = 0;
		private string fonttbl;
		private Hashtable loadedFonts = new Hashtable();

		#endregion

		#region Methods.Public

		/// <summary>
		/// </summary>
		/// <param name="fontName"></param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="fontName"/> is <see langword="null"/>.</exception>
		public int UseFont(string fontName)
		{
			if (fontName == null)
			{
				throw new ArgumentNullException("fontName");
			}

			if (loadedFonts.Contains(fontName))
			{
				return (int)loadedFonts[fontName];
			}
			else
			{
				fonttbl += "{\\f" + (++numberOfFonts) + "\\fnil " + fontName + ";}";
				loadedFonts.Add(fontName, numberOfFonts);
				
				return numberOfFonts;
			}
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
			return fonttbl + "}";
		}

		#endregion

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRtfFontTable"/> class.
		/// </summary>
		public NuGenRtfFontTable()
		{
			fonttbl = "{\\fonttbl{\\f0\\froman Times New Roman;}";
		}
		
		#endregion
	}
}
