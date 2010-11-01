/* -----------------------------------------------
 * NuGenBevel.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls.Design;
using Genetibase.Shared.Drawing;
using Genetibase.WinApi;

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Controls
{
	/// <summary>
	/// VCL TBevel analogue.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenBevelDesigner))]
	[ToolboxItem(true)]
	public class NuGenBevel : UserControl
	{
		#region Declarations
		
		private Container components = null;
		
		#endregion

		#region Properties.Protected.Overriden

		/// <summary>
		/// Gets the default size for this <see cref="T:NuGenBevel"/>.
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(200, SystemInformation.Border3DSize.Height);
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			ControlPaint.DrawBorder3D(
				e.Graphics,
				this.ClientRectangle.X,
				this.ClientRectangle.Y,
				this.Width,
				SystemInformation.Border3DSize.Height,
				Border3DStyle.SunkenOuter,
				Border3DSide.All
				);
		}

		/// <summary>
		/// Performs the work of setting the
		/// specified bounds of this control.
		/// </summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left"/> property value of the control.</param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Right"/> property value of the control.</param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width"/> property value of the control.</param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height"/> property value of the control.</param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified"/> values.</param>
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if ((specified & BoundsSpecified.Height) != 0)
			{
				if (height != SystemInformation.Border3DSize.Height)
				{
					height = SystemInformation.Border3DSize.Height;
				}
			}

			base.SetBoundsCore (x, y, width, height, specified);
		}

		/// <summary>
		/// Processes Windows messages.
		/// </summary>
		/// <param name="m">The message to process.</param>
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WinUser.WM_MOUSEACTIVATE)
			{
				m.Result = (IntPtr)WinUser.MA_NOACTIVATEANDEAT;
			}
			else 
			{
				base.WndProc(ref m);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenBevel"/> class.
		/// </summary>
		public NuGenBevel()
		{
			InitializeComponent();

			NuGenControlPaint.SetStyle(this, ControlStyles.Selectable, false);
			NuGenControlPaint.SetStyle(this, ControlStyles.ResizeRedraw, true);

			this.TabStop = false;
		}
		
		#endregion

		#region Dispose

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null) components.Dispose();
			}
			
			base.Dispose(disposing);
		}
		
		#endregion

		#region Component Designer generated code
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		
		#endregion
	}
}
