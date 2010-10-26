/* -----------------------------------------------
 * NuGenRtfColorTable.cs
 * Author: Alex Nesterov
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Drawing;

namespace Genetibase.Controls.Service
{
	/// <summary>
	/// Encapsulates RTF color.
	/// </summary>
	class NuGenRtfColorTable
	{
		#region Declarations

		private int numberOfColors = 0;
		private string colortbl;
		private Hashtable loadedColors = new Hashtable();

		#endregion

		#region Methods.Public

		/// <summary>
		/// Adds the specified color to the collection.
		/// </summary>
		/// <param name="key">Specifies the color to add to the collection.</param>
		/// <returns></returns>
		public int UseColor(Color key)
		{
			if (loadedColors.ContainsKey(key))
			{
				return (int)loadedColors[key];
			}
			else
			{
				colortbl += "\\red" + key.R + "\\green" + key.G + "\\blue" + key.B + ";";
				loadedColors.Add(key, ++numberOfColors);
				
				return numberOfColors;
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
			return colortbl + "}";
		}

		#endregion

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRtfColorTable"/> class.
		/// </summary>
		public NuGenRtfColorTable()
		{
			colortbl = "{\\colortbl;";
		}
		
		#endregion
	}
}
