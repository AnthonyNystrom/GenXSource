/* -----------------------------------------------
 * NuGenRibbonControl.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.UI.NuGenInterface.ComponentModel;
using Genetibase.UI.NuGenInterface.Design;
using Genetibase.UI.NuGenInterface.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenInterface
{
	/// <summary>
	/// Microsoft Office 2007 ribbon implementation.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenRibbonControlDesigner))]
	[NuGenSRDescription("NuGenRibbonControlDescription")]
	[ToolboxItem(false)]
	public class NuGenRibbonControl : ContainerControl
	{
		#region Properties.Protected.Overriden

		/// <summary>
		/// Gets the default size for this <see cref="T:NuGenRibbonControl"/>.
		/// </summary>
		/// <value></value>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(200, 151);
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/*
		 * OnParentChanged
		 */

		/// <summary>
		/// Raises the parent changed event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);

			if (this.Parent != null && !(this.Parent is Form))
			{
				InvalidOperationException ex = new InvalidOperationException(
					Resources.InvalidOperation_ShouldBeHosted
					);

				this.Parent = null;
				throw ex;
			}

			if (this.Parent != null)
			{
				foreach (Control ctrl in this.Parent.Controls)
				{
					/* Only one instance of NuGenRibbonControl is allowed on the host form. */
					if (ctrl is NuGenRibbonControl && ctrl != this)
					{
						this.Parent = null;
					}
				}
			}
		}

		/*
		 * SetBoundsCore
		 */

		/// <summary>
		/// Performs the work of setting the specified bounds of this control.
		/// </summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left"></see> property value of the control.</param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top"></see> property value of the control.</param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width"></see> property value of the control.</param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height"></see> property value of the control.</param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified"></see> values.</param>
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if ((specified & BoundsSpecified.Height) != 0)
			{
				height = this.DefaultSize.Height;
			}

			base.SetBoundsCore(x, y, width, height, specified);
		}

		#endregion

		#region Methods.Protected.Virtual

		/*
		 * InitializeLayout
		 */

		/// <summary>
		/// Initializes this <see cref="NuGenRibbonControl"/> layout.
		/// </summary>
		protected virtual void InitializeLayout()
		{
			this.Dock = DockStyle.Top;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRibbonControl"/> class.
		/// </summary>
		public NuGenRibbonControl()
		{
			this.InitializeLayout();
		}

		#endregion
	}
}
