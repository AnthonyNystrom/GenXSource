/* -----------------------------------------------
 * NuGenRenderer.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Genetibase.UI.NuGenInterface.Rendering
{
	/// <summary>
	/// Represents a renderer for controls to imitate Microsoft Office 2007 style.
	/// </summary>
	public class NuGenRenderer : NuGenRendererBase
	{
		#region Properties.Public

		/*
		 * ColorTable
		 */

		private NuGenColorTable _ColorTable = null;

		/// <summary>
		/// Read-only.
		/// </summary>
		public override NuGenColorTable ColorTable
		{
			get
			{
				if (_ColorTable == null)
				{
					_ColorTable = new NuGenColorTable();
				}

				return _ColorTable;
			}
			set
			{
				_ColorTable = value;
				Debug.Assert(_ColorTable != null, "this.colorTable != null");
			}
		}

		#endregion
	}
}
