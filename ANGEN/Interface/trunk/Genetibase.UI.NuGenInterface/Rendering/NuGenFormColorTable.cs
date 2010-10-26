/* -----------------------------------------------
 * NuGenFormColorTable.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Genetibase.UI.NuGenInterface.Rendering
{
	/// <summary>
	/// Encapsulates color table for a form to imitate Microsoft Office 2007 style.
	/// </summary>
	public class NuGenFormColorTable
	{
		#region Properties.Public

		/*
		 * Active
		 */

		private NuGenFormStateColorTable _Active = null;

		/// <summary>
		/// Gets or sets the color table for the form's caption in its active state.
		/// </summary>
		public NuGenFormStateColorTable Active
		{
			get
			{
				if (_Active == null)
				{
					_Active = new NuGenFormStateColorTable();
				}

				return _Active;
			}
			set
			{
				_Active = value;
			}
		}

		/*
		 * BackColor
		 */

		private Color _BackColor = Color.Empty;

		/// <summary>
		/// Gets or sets the background color for the form.
		/// </summary>
		public Color BackColor
		{
			get
			{
				return _BackColor;
			}
			set
			{
				_BackColor = value;
			}
		}

		/*
		 * Inactive
		 */

		private NuGenFormStateColorTable _Inactive = null;

		/// <summary>
		/// Gets or sets the color table for the form's caption in its inactive state.
		/// </summary>
		public NuGenFormStateColorTable Inactive
		{
			get
			{
				if (_Inactive == null)
				{
					_Inactive = new NuGenFormStateColorTable();
				}

				return _Inactive;
			}
			set
			{
				_Inactive = value;
			}
		}

		#endregion
	}
}
