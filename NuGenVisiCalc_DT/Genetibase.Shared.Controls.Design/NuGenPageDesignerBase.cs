/* -----------------------------------------------
 * NuGenPageDesignerBase.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides basic functionality for such designers as <see cref="NuGenTabPageDesigner"/> and the like.
	/// </summary>
	public abstract class NuGenPageDesignerBase : ScrollableControlDesigner
	{
		/*
		 * SelectionRules
		 */

		/// <summary>
		/// Gets the selection rules that indicate the movement capabilities of a component.
		/// </summary>
		/// <value></value>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.Design.SelectionRules"></see> values.</returns>
		public override SelectionRules SelectionRules
		{
			get
			{
				return base.SelectionRules & ~SelectionRules.AllSizeable;
			}
		}

		/*
		 * BorderPen
		 */

		/// <summary>
		/// </summary>
		protected Pen BorderPen
		{
			get
			{
				Color borderColor = (this.Control.BackColor.GetBrightness() < 0.5)
					? ControlPaint.Light(this.Control.BackColor)
					: ControlPaint.Dark(this.Control.BackColor)
					;
				Pen borderPen = new Pen(borderColor);
				borderPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
				return borderPen;
			}
		}

		/*
		 * DoDragDrop
		 */

		/// <summary>
		/// </summary>
		/// <param name="e"></param>
		internal void DoDragDrop(DragEventArgs e)
		{
			this.OnDragDrop(e);
		}

		/*
		 * DoDragEnter
		 */

		/// <summary>
		/// </summary>
		/// <param name="e"></param>
		internal void DoDragEnter(DragEventArgs e)
		{
			this.OnDragEnter(e);
		}

		/*
		 * DoDragLeave
		 */

		/// <summary>
		/// </summary>
		/// <param name="e"></param>
		internal void DoDragLeave(EventArgs e)
		{
			this.OnDragLeave(e);
		}

		/*
		 * DoDragOver
		 */

		/// <summary>
		/// </summary>
		/// <param name="e"></param>
		internal void DoDragOver(DragEventArgs e)
		{
			this.OnDragOver(e);
		}

		/*
		 * DoGiveFeedback
		 */

		/// <summary>
		/// </summary>
		/// <param name="e"></param>
		internal void DoGiveFeedback(GiveFeedbackEventArgs e)
		{
			this.OnGiveFeedback(e);
		}

		/*
		 * GetControlGlyph
		 */

		/// <summary>
		/// </summary>
		/// <param name="selectionType"></param>
		/// <returns></returns>
		protected override ControlBodyGlyph GetControlGlyph(GlyphSelectionType selectionType)
		{
			this.OnSetCursor();
			return new ControlBodyGlyph(Rectangle.Empty, Cursor.Current, this.Control, this);
		}

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Raises the paint adornments event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
		protected override void OnPaintAdornments(PaintEventArgs e)
		{
			using (Pen pen = this.BorderPen)
			{
				Rectangle tweakedRectangle = new Rectangle(
					this.Control.ClientRectangle.Left + 1,
					this.Control.ClientRectangle.Top + 1,
					this.Control.ClientRectangle.Width - 3,
					this.Control.ClientRectangle.Height - 3
				);

				e.Graphics.DrawRectangle(pen, tweakedRectangle);
			}

			base.OnPaintAdornments(e);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPageDesignerBase"/> class.
		/// </summary>
		protected NuGenPageDesignerBase()
		{
			base.AutoResizeHandles = true;
		}
	}
}
