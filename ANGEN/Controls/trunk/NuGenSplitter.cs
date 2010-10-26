/* -----------------------------------------------
 * NuGenSplitter.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NuGenControlPaint = Genetibase.Shared.Drawing.NuGenControlPaint;

using Genetibase.Shared;

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Controls
{
	/// <summary>
	/// Represents a splitter for the <see cref="T:NuGenSideBar"/>.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	class NuGenSplitter : UserControl
	{
		#region Declarations

		private bool shouldResize = false;
		private Point clickPoint = Point.Empty;

		#endregion

		#region Properties.Public

		/// <summary>
		/// Determines the minimum width of the target control.
		/// </summary>
		private int internalMinimumWidth = 50;

		/// <summary>
		/// Gets or sets the minimum width of the target control.
		/// </summary>
		public int MinimumWidth
		{
			get { return this.internalMinimumWidth; }
			set
			{
				if (this.internalMinimumWidth != value)
				{
					this.internalMinimumWidth = value;
				}
			}
		}

		#endregion

		#region Properties.Protected.Overriden

		protected override Size DefaultSize
		{
			get
			{
				return new Size(5, 100);
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Raises the mouse down event.
		/// </summary>
		/// <param name="e">An <see cref="System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.clickPoint = new Point(e.X, e.Y);
			this.shouldResize = true;
		}
		
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			
			if (this.shouldResize && this.Parent != null)
			{
				int deltaX = e.X - this.clickPoint.X;
				
				if (this.Dock == DockStyle.Left)
				{
					int width = this.Parent.Width - deltaX;

					if (width < this.MinimumWidth)
						return;
					else
					{
						this.Parent.Left += deltaX;
						this.Parent.Width -= deltaX;
					}
				}
				else
					this.Parent.Width = Math.Max(this.Parent.Width + deltaX, this.MinimumWidth);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.shouldResize = false;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSplitter"/> class.
		/// </summary>
		public NuGenSplitter()
		{
			this.Cursor = Cursors.VSplit;
			this.Dock = DockStyle.Right;
		}

		#endregion
	}
}
