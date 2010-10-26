/* -----------------------------------------------
 * NuGenBevel.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Drawing;
using Genetibase.WinApi;

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// VCL TBevel analogue.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenBevel), "Resources.NuGenIcon.png")]
	[DefaultEvent("Paint")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenBevelDesigner")]
	[NuGenSRDescription("Description_Bevel")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenBevel : UserControl
	{
		/// <summary>
		/// Gets the default size for this <see cref="T:NuGenBevel"/>.
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(200, 24);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			ControlPaint.DrawBorder3D(
				e.Graphics,
				this.ClientRectangle.Left,
				this.ClientRectangle.Top + (this.ClientRectangle.Height - SystemInformation.Border3DSize.Height) / 2,
				this.Width,
				SystemInformation.Border3DSize.Height,
				Border3DStyle.SunkenOuter,
				Border3DSide.All
			);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenBevel"/> class.
		/// </summary>
		public NuGenBevel()
		{
			NuGenControlPaint.SetStyle(this, ControlStyles.Selectable, false);
			NuGenControlPaint.SetStyle(this, ControlStyles.ResizeRedraw, true);

			this.TabStop = false;
		}
	}
}
