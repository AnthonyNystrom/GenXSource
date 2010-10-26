/* -----------------------------------------------
 * NuGenFormStateDescriptor.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenInterface
{
	/// <summary>
	/// </summary>
	public struct NuGenFormStateDescriptor
	{
		#region Properties.Public

		/*
		 * BackColor
		 */

		private Color _BackColor;

		/// <summary>
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
		 * Padding
		 */

		private Padding _Padding;

		/// <summary>
		/// </summary>
		public Padding Padding
		{
			get
			{
				return _Padding;
			}
			set
			{
				_Padding = value;
			}
		}

		/*
		 * Styles
		 */

		private Dictionary<ControlStyles, bool> _Styles;

		/// <summary>
		/// </summary>
		public Dictionary<ControlStyles, bool> Styles
		{
			get
			{
				if (_Styles == null)
				{
					_Styles = new Dictionary<ControlStyles, bool>();
				}

				return _Styles;
			}
		}

		#endregion
	}
}
