/* -----------------------------------------------
 * NuGenToolButtonDesigner.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenToolButton"/>.
	/// </summary>
	class NuGenToolButtonDesigner : ControlDesigner
	{
		#region Declarations

		/// <summary>
		/// Used for tweaking.
		/// </summary>
		private const int PEN_WIDTH = 1;

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Called when the control that the designer is managing has painted its surface so the designer
		/// can paint any additional adornments on top of the control.
		/// </summary>
		/// <param name="e">An <see cref="System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
		protected override void OnPaintAdornments(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Rectangle tweakedRect = new Rectangle(
				this.Control.ClientRectangle.X,
				this.Control.ClientRectangle.Y,
				this.Control.ClientRectangle.Width - PEN_WIDTH,
				this.Control.ClientRectangle.Height - PEN_WIDTH
				);

			using (Pen pen = new Pen(SystemColors.ControlDarkDark))
			{
				pen.DashStyle = DashStyle.Dash;
				g.DrawRectangle(pen, tweakedRect);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenToolButtonDesigner"/> class.
		/// </summary>
		public NuGenToolButtonDesigner()
		{
		}

		#endregion
	}
}
