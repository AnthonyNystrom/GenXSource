/* -----------------------------------------------
 * NuGenTabPageDesigner.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenTabPage"/>.
	/// </summary>
	public class NuGenTabPageDesigner : ScrollableControlDesigner
	{
		#region Properties.Public.Overridden

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

		#endregion

		#region Properties.Protected

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

		#endregion

		#region Methods.Internal

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

		#endregion

		#region Methods.Public.Overridden

		/*
		 * CanBeParentedTo
		 */

		/// <summary>
		/// Indicates if this designer's control can be parented by the control of the specified designer.
		/// </summary>
		/// <param name="parentDesigner">The <see cref="T:System.ComponentModel.Design.IDesigner"></see> that manages the control to check.</param>
		/// <returns>
		/// true if the control managed by the specified designer can parent the control managed by this designer; otherwise, false.
		/// </returns>
		public override bool CanBeParentedTo(IDesigner parentDesigner)
		{
			if (parentDesigner != null)
			{
				return parentDesigner.Component is NuGenTabControl;
			}

			return false;
		}

		#endregion

		#region Methods.Protected.Overridden

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
					this.Control.ClientRectangle.Left,
					this.Control.ClientRectangle.Top + 1,
					this.Control.ClientRectangle.Width - 2,
					this.Control.ClientRectangle.Height - 2
				);

				e.Graphics.DrawRectangle(pen, tweakedRectangle);
			}

			base.OnPaintAdornments(e);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabPageDesigner"/> class.
		/// </summary>
		public NuGenTabPageDesigner()
		{
			base.AutoResizeHandles = true;
		}

		#endregion
	}
}
