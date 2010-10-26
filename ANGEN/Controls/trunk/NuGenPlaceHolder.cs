/* -----------------------------------------------
 * NuGenPlaceHolder.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Controls
{
	/// <summary>
	/// A panel that acts like a placeholder.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(false)]
	public class NuGenPlaceHolder : Panel
	{
		#region Properties.Protected.Overriden

		/// <summary>
		/// Gets the default size for this <see cref="T:NuGenPlaceHolder"/>.
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(
					SystemInformation.VerticalScrollBarWidth,
					SystemInformation.HorizontalScrollBarHeight
					);
			}
		}

		#endregion

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPlaceHolder"/> class.
		/// </summary>
		public NuGenPlaceHolder()
		{
			NuGenControlPaint.SetStyle(this, ControlStyles.Selectable, false);
			this.BackColor = SystemColors.Control;
		}
		
		#endregion
	}
}
