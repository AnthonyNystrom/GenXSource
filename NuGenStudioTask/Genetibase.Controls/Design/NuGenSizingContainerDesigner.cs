/* -----------------------------------------------
 * NuGenSizingContainerDesigner.cs
 * Author: Alex Nesterov
 * --------------------------------------------- */

using NuGenControlPaint = Genetibase.Shared.Drawing.NuGenControlPaint;

using Genetibase.Shared.Drawing;
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
	/// Provides additional design-time functionality for this <see cref="NuGenSizingContainer"/>.
	/// </summary>
	public class NuGenSizingContainerDesigner : ParentControlDesigner
	{
		/// <summary>
		/// Raises the paint adornments event.
		/// </summary>
		/// <param name="e">An <see cref="System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
		protected override void OnPaintAdornments(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
	
			Rectangle tweakedRect = new Rectangle(
				this.Control.ClientRectangle.X,
				this.Control.ClientRectangle.Y,
				this.Control.ClientRectangle.Width - 6,
				this.Control.ClientRectangle.Height - 1
				);

			using (Pen pen = new Pen(SystemColors.ControlDark))
			{
				pen.DashStyle = DashStyle.Dash;
				g.DrawRectangle(pen, tweakedRect);
			}
		}	
	}
}
