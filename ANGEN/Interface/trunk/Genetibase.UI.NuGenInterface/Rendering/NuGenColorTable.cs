/* -----------------------------------------------
 * NuGenColorTable.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.UI.NuGenInterface.Rendering
{
	/// <summary>
	/// Represents a color table to imitate Microsoft Office 2007 style.
	/// </summary>
	public class NuGenColorTable
	{
		#region Properties.Public

		/*
		 * ColorScheme
		 */

		private NuGenColorScheme _ColorScheme = NuGenColorScheme.Blue;

		/// <summary>
		/// Gets the <see cref="NuGenColorScheme"/> this <see cref="NuGenColorTable"/> was initialized with.
		/// </summary>
		public NuGenColorScheme ColorScheme
		{
			get
			{
				return _ColorScheme;
			}
		}

		/*
		 * Form
		 */

		private NuGenFormColorTable _Form = null;

		/// <summary>
		/// Gets or sets the color table for the caption of the form to imitate Microsoft Office 2007 style.
		/// </summary>
		public NuGenFormColorTable Form
		{
			get
			{
				if (_Form == null)
				{
					_Form = new NuGenFormColorTable();
				}

				return _Form;
			}
			set
			{
				_Form = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenColorTable"/> class.
		/// </summary>
		public NuGenColorTable()
			: this(NuGenColorScheme.Blue)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenColorTable"/> class.
		/// </summary>
		/// <param name="colorScheme">Specifies the <see cref="NuGenColorScheme"/>
		/// that is used to intialize this <see cref="NuGenColorTable"/>.</param>
		public NuGenColorTable(NuGenColorScheme colorScheme)
		{
			NuGenColorTableInitializer.InitializeColorTable(this, colorScheme);
		}

		#endregion
	}
}
