/* -----------------------------------------------
 * NuGenPictureBox.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls.Design;
using Genetibase.Shared;
using Genetibase.Shared.Drawing;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Controls
{
	/// <summary>
	/// A picture box with ScaleToFit and AutoScroll features.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenPictureBoxDesigner))]
	[ToolboxItem(true)]
	public class NuGenPictureBox : Panel
	{
		#region Properties.Appearance

		/*
		 * DisplayMode
		 */

		/// <summary>
		/// Determines the image display mode for this <see cref="T:NuGenPictureBox"/>.
		/// </summary>
		private NuGenDisplayMode displayMode = NuGenDisplayMode.ScaleToFit;

		/// <summary>
		/// Gets or sets the image display mode for this <see cref="T:NuGenPictureBox"/>.
		/// </summary>
		[Browsable(true)]
		[Category("Appearance")]
		[DefaultValue(NuGenDisplayMode.ScaleToFit)]
		[Description("Determines the image display mode for this picture box.")]
		public NuGenDisplayMode DisplayMode
		{
			[DebuggerStepThrough]
			get 
			{
				return this.displayMode;
			}
			set 
			{
				if (this.displayMode != value) 
				{
					this.displayMode = value;

					switch (this.displayMode)
					{
						case NuGenDisplayMode.ActualSize:
							this.AutoScrollMinSize = (this.Image != null) ? this.Image.Size : Size.Empty;
							break;
						case NuGenDisplayMode.ScaleToFit:
						case NuGenDisplayMode.StretchToFit:
							this.AutoScrollMinSize = Size.Empty;
							break;
					}

					this.OnDisplayModeChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenPictureBox.DisplayModeChanged"/> event identifier.
		/// </summary>
		private static readonly object EventDisplayModeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:NuGenPictureBox.DisplayMode"/> property changes.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Occurs when the value of the DisplayMode property changes.")]
		public event EventHandler DisplayModeChanged
		{
			add { this.Events.AddHandler(NuGenPictureBox.EventDisplayModeChanged, value); }
			remove { this.Events.RemoveHandler(NuGenPictureBox.EventDisplayModeChanged, value); }
		}

		/// <summary>
		/// Raises the <see cref="E:NuGenPictureBox.DisplayModeChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected virtual void OnDisplayModeChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[NuGenPictureBox.EventDisplayModeChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/*
		 * Image
		 */

		/// <summary>
		/// Determines the image to display for this <see cref="T:NuGenPictureBox"/>.
		/// </summary>
		private Image image = null;

		/// <summary>
		/// Gets or sets the image to display for this <see cref="T:NuGenPictureBox"/>.
		/// </summary>
		[Browsable(true)]
		[Category("Appearance")]
		[DefaultValue(null)]
		[Description("Determines the image to display for this picture box.")]
		public Image Image
		{
			[DebuggerStepThrough]
			get 
			{
				return this.image;
			}
			set
			{
				if (this.image != value)
				{
					this.image = value;

					if (this.DisplayMode == NuGenDisplayMode.ActualSize)
					{
						this.AutoScrollMinSize = this.Image.Size;
					}

					this.OnImageChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenPictureBox.ImageChanged"/> event identifier.
		/// </summary>
		private static readonly object EventImageChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:NuGenPictureBox.Image"/> property changes.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Occurs when the value of the Image property changes.")]
		public event EventHandler ImageChanged
		{
			add { this.Events.AddHandler(NuGenPictureBox.EventImageChanged, value); }
			remove { this.Events.RemoveHandler(NuGenPictureBox.EventImageChanged, value); }
		}

		/// <summary>
		/// Raises the <see cref="E:NuGenPictureBox.ImageChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected virtual void OnImageChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[NuGenPictureBox.EventImageChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#region Properties.Hidden

		/*
		 * BackgroundImage
		 */

		/// <summary>
		/// Gets or sets the background image displayed in the control.
		/// </summary>
		/// <value></value>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/// <summary>
		/// Occurs when the value of the <see cref="P:NuGenPictureBox.BackgroundImage"/> property changes.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageChanged
		{
			add { base.BackgroundImageChanged += value; }
			remove { base.BackgroundImageChanged -= value; }
		}

		#endregion

		#region Properties.Public.Overriden

		/// <summary>
		/// Gets or sets a value indicating whether the container will allow the user to scroll to any controls placed outside of its visible boundaries.
		/// </summary>
		/// <value></value>
		[DefaultValue(true)]
		public override bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				base.AutoScroll = value;
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
			if (this.Image != null) 
			{
				Graphics g = e.Graphics;

				switch (this.DisplayMode)
				{
					case NuGenDisplayMode.ActualSize:
						g.DrawImage(
							this.Image,
							this.AutoScrollPosition.X,
							this.AutoScrollPosition.Y,
							this.Image.Width,
							this.Image.Height
							);
						break;
					case NuGenDisplayMode.ScaleToFit:
						Rectangle fitRect = NuGenControlPaint.ScaleToFit(this.ClientRectangle, this.Image);
						g.DrawImage(
							this.Image,
							fitRect
							);
						break;
					case NuGenDisplayMode.StretchToFit:
						g.DrawImage(
							this.Image,
							this.DisplayRectangle
							);
						break;
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPictureBox"/> class.
		/// </summary>
		public NuGenPictureBox()
		{
			NuGenControlPaint.SetStyle(this, ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
			NuGenControlPaint.SetStyle(this, ControlStyles.Selectable, false);

			this.AutoScroll = true;
		}

		#endregion
	}
}
