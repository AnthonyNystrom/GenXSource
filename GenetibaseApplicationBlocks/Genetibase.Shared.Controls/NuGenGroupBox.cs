/* -----------------------------------------------
 * NuGenGroupBox.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.GroupBoxInternals;
using Genetibase.Shared.Controls.Design;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// <seealso cref="GroupBox"/>
	/// </summary>
	[ToolboxItem(false)]
	[Designer(typeof(NuGenGroupBoxDesigner))]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenGroupBox : NuGenControlBase
	{
		#region Declarations.Fields

		private static readonly int _labelOffset = 10;
		private Rectangle _labelRectangle;
		private Rectangle _frameRectangle;

		#endregion

		#region Properties.Appearance

		/*
		 * HeaderImage
		 */

		private Image _headerImage;

		/// <summary>
		/// </summary>
		public Image HeaderImage
		{
			get
			{
				return _headerImage;
			}
			set
			{
				if (_headerImage != value)
				{
					_headerImage = value;
					this.OnHeaderImageChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _headerImageChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="HeaderImage"/> property changes.
		/// </summary>
		public event EventHandler HeaderImageChanged
		{
			add
			{
				this.Events.AddHandler(_headerImageChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_headerImageChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="HeaderImageChanged"/> event.
		/// </summary>
		protected virtual void OnHeaderImageChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_headerImageChanged, e);
		}

		/*
		 * Text
		 */

		/// <summary>
		/// Gets or sets the text associated with this group box.
		/// </summary>
		/// <value></value>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		/// <summary>
		/// Occurs when the value of the <see cref="Text"/> property changes.
		/// </summary>
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		#endregion

		#region Properties.Public.Overridden

		/*
		 * BackColor
		 */

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[DefaultValue(typeof(Color), "Transparent")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		/*
		 * DisplayRectangle
		 */

		/// <summary>
		/// Gets the rectangle that represents the virtual display area of the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Rectangle"></see> that represents the display area of the control.</returns>
		public override Rectangle DisplayRectangle
		{
			get
			{
				return new Rectangle(
					_frameRectangle.Left,
					_frameRectangle.Top + _labelRectangle.Height / 2,
					_frameRectangle.Width,
					_frameRectangle.Height - _labelRectangle.Height / 2
				);
			}
		}

		#endregion

		#region Properties.Protected.Overridden

		/*
		 * DefaultSize
		 */

		private static readonly Size _defaultSize = new Size(200, 200);

		/// <summary>
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return _defaultSize;
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * Renderer
		 */

		private INuGenGroupBoxRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenGroupBoxRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenGroupBoxRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenGroupBoxRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Debug.Assert(this.Renderer != null, "this.Renderer != null");
			
			Graphics g = e.Graphics;
			Rectangle bounds = this.ClientRectangle;
			NuGenControlState currentState = this.StateTracker.GetControlState();

			_labelRectangle = this.GetLabelRectangle(g, bounds);
			_frameRectangle = this.GetFrameRectangle(_labelRectangle, bounds);

			NuGenPaintParams paintParams = new NuGenPaintParams(this, g, _frameRectangle, currentState);

			this.Renderer.DrawBackground(paintParams);
			this.Renderer.DrawFrame(paintParams);

			NuGenTextPaintParams textPaintParams = new NuGenTextPaintParams(this, g, _labelRectangle, currentState, this.Text);
			textPaintParams.Font = this.Font;
			textPaintParams.ForeColor = this.ForeColor;
			textPaintParams.TextAlign = ContentAlignment.MiddleCenter;

			this.Renderer.DrawLabel(textPaintParams);
		}

		#endregion

		#region Methods.Protected.Virtual

		/*
		 * GetFrameRectangle
		 */

		/// <summary>
		/// Returns the bounds for the group box frame.
		/// </summary>
		/// <param name="labelRectangle"></param>
		/// <param name="clientRectangle"></param>
		/// <returns></returns>
		protected virtual Rectangle GetFrameRectangle(Rectangle labelRectangle, Rectangle clientRectangle)
		{
			Rectangle borderRectangle = NuGenControlPaint.BorderRectangle(clientRectangle);
			return new Rectangle(
				borderRectangle.Left,
				borderRectangle.Top + labelRectangle.Top + labelRectangle.Height / 2,
				borderRectangle.Width,
				borderRectangle.Height - labelRectangle.Top - labelRectangle.Height / 2
			);
		}

		/*
		 * GetLabelRectangle
		 */

		/// <summary>
		/// Returns the bounds for the group box label.
		/// </summary>
		/// <param name="g"></param>
		/// <param name="clientRectangle"></param>
		/// <returns></returns>
		protected virtual Rectangle GetLabelRectangle(Graphics g, Rectangle clientRectangle)
		{
			Debug.Assert(g != null, "g != null");

			Size textSize = g.MeasureString(this.Text, this.Font).ToSize();
			int textWidth = textSize.Width + 5;
			int textHeight = textSize.Height + 5;

			if (this.RightToLeft == RightToLeft.No)
			{
				return new Rectangle(
					clientRectangle.Left + _labelOffset,
					clientRectangle.Top,
					textWidth,
					textHeight
				);
			}

			return new Rectangle(
				clientRectangle.Right - _labelOffset - textWidth,
				clientRectangle.Top,
				textWidth,
				textHeight
			);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenGroupBox"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenControlStateService"/><para/>
		/// <see cref="INuGenGroupBoxRenderer"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenGroupBox(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, false);
			this.SetStyle(ControlStyles.Selectable, false);

			this.BackColor = Color.Transparent;
		}

		#endregion
	}
}
