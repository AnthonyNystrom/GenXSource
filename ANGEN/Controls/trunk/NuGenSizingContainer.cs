/* -----------------------------------------------
 * NuGenSizingContainer.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls.Design;
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
	/// Acts as a container and resizes its child controls.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenSizingContainerDesigner))]
	[ToolboxItem(true)]
	public class NuGenSizingContainer : UserControl
	{
		#region Declarations

		/// <summary>
		/// Used to changed the width of this <see cref="T:NuGenSizingContainer"/>.
		/// </summary>
		private NuGenSplitter splitter = null;

		#endregion

		#region Properties.Behavior

		/// <summary>
		/// Determines the way this <see cref="T:NuGenSizingContainer"/> docks to its parent window.
		/// </summary>
		private NuGenSizingContainerMode internalSizingMode = NuGenSizingContainerMode.Left;

		/// <summary>
		/// Gets or sets the way this <see cref="T:NuGenSizingContainer"/> docks to its parent window.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[DefaultValue(NuGenSizingContainerMode.Left)]
		[Description("Determines the way this container docks to its parent window.")]
		public virtual NuGenSizingContainerMode SizingMode
		{
			get { return this.internalSizingMode; }
			set
			{
				if (this.internalSizingMode !=  value)
				{
					this.internalSizingMode = value;
					this.OnSizingModeChanged(EventArgs.Empty);
					this.SetLayout();
				}
			}
		}

		/// <summary>
		/// Occurs when the value of the <see cref="P:NuGenSizingContainer.SizingMode"/> property changes.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Occurs when the value of the SizingMode property changes.")]
		public event EventHandler SizingModeChanged;

		/// <summary>
		/// Raises the <see cref="E:NuGenSizingContainer.SizingModeChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected virtual void OnSizingModeChanged(EventArgs e)
		{
			if (this.SizingModeChanged != null)
				this.SizingModeChanged(this, e);
		}

		#endregion

		#region Properties.Protected.Overriden

		/// <summary>
		/// Gets the default size for this <see cref="T:NuGenSizingContainer"/>.
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(200, 200);
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.ControlAdded"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ControlEventArgs"/> that contains the event data.</param>
		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			this.SetLayout();
		}

		#endregion

		#region Methods.Private

		private void SetLayout()
		{
			switch (this.SizingMode)
			{
				case NuGenSizingContainerMode.Left:
					this.Dock = DockStyle.Left;
					this.splitter.Dock = DockStyle.Right;
					break;
				case NuGenSizingContainerMode.Right:
					this.Dock = DockStyle.Right;
					this.splitter.Dock = DockStyle.Left;
					break;
			}

			this.Controls.SetChildIndex(this.splitter, 0);
			this.splitter.SendToBack();
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSizingContainer"/> class.
		/// </summary>
		public NuGenSizingContainer()
		{
			this.SetStyle(ControlStyles.ResizeRedraw, true);

			/*
			 * splitter
			 */

			this.splitter = new NuGenSplitter();
			this.splitter.Parent = this;

			/*
			 * NuGenSizingContainer
			 */

			this.SetLayout();
		}

		#endregion
	}
}
