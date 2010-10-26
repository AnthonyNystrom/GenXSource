/* -----------------------------------------------
 * NuGenGradientPanel.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a panel with a linear gradient background.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenGradientPanel), "Resources.NuGenIcon.png")]
	[DefaultEvent("Paint")]
	[DefaultProperty("Watermark")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenGradientPanelDesigner")]
	[NuGenSRDescription("Description_GradientPanel")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenGradientPanel : NuGenWatermark
	{
		#region Properties.Appearance

		/*
		 * EndGradientColor
		 */

		private Color _endGradientColor;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_GradientPanel_EndGradientColor")]
		public Color EndGradientColor
		{
			get
			{
				if (_endGradientColor == Color.Empty)
				{
					return this.DefaultEndGradientColor;
				}

				return _endGradientColor;
			}
			set
			{
				if (_endGradientColor != value)
				{
					_endGradientColor = value;
					this.OnEndGradientColorChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly Color _defaultEndGradientColor = SystemColors.InactiveCaption;

		/// <summary>
		/// </summary>
		protected virtual Color DefaultEndGradientColor
		{
			get
			{
				return _defaultEndGradientColor;
			}
		}

		private void ResetEndGradientColor()
		{
			this.EndGradientColor = this.DefaultEndGradientColor;
		}

		private bool ShouldSerializeEndGradientColor()
		{
			return this.EndGradientColor != this.DefaultEndGradientColor;
		}

		private static readonly object _endGradientColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="EndGradientColorChanged"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_GradientPanel_EndGradientColorChanged")]
		public event EventHandler EndGradientColorChanged
		{
			add
			{
				this.Events.AddHandler(_endGradientColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_endGradientColorChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenGradientPanel.EndGradientColorChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnEndGradientColorChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_endGradientColorChanged, e);
		}

		/*
		 * GradientDirection
		 */

		private LinearGradientMode _gradientDirection = LinearGradientMode.ForwardDiagonal;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(LinearGradientMode.ForwardDiagonal)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_GradientPanel_GradientDirection")]
		public LinearGradientMode GradientDirection
		{
			get
			{
				return _gradientDirection;
			}
			set
			{
				if (_gradientDirection != value)
				{
					_gradientDirection = value;
					this.OnGradientDirectionChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _gradientDirectionChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="GradientDirection"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_GradientPanel_GradientDirectionChanged")]
		public event EventHandler GradientDirectionChanged
		{
			add
			{
				this.Events.AddHandler(_gradientDirectionChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_gradientDirectionChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenGradientPanel.GradientDirectionChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnGradientDirectionChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_gradientDirectionChanged, e);
		}

		/*
		 * StartGradientColor
		 */

		private Color _startGradientColor;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_GradientPanel_StartGradientColor")]
		public Color StartGradientColor
		{
			get
			{
				if (_startGradientColor == Color.Empty)
				{
					return this.DefaultStartGradientColor;
				}

				return _startGradientColor;
			}
			set
			{
				if (_startGradientColor != value)
				{
					_startGradientColor = value;
					this.OnStartGradientColorChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly Color _defaultStartGradientColor = SystemColors.InactiveCaptionText;

		/// <summary>
		/// </summary>
		protected virtual Color DefaultStartGradientColor
		{
			get
			{
				return _defaultStartGradientColor;
			}
		}

		private void ResetStartGradientColor()
		{
			this.StartGradientColor = this.DefaultStartGradientColor;
		}

		private bool ShouldSerializeStartGradientColor()
		{
			return this.StartGradientColor != this.DefaultStartGradientColor;
		}

		private static readonly object _startGradientColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="StartGradientColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_GradientPanel_StartGradientColorChanged")]
		public event EventHandler StartGradientColorChanged
		{
			add
			{
				this.Events.AddHandler(_startGradientColor, value);
			}
			remove
			{
				this.Events.RemoveHandler(_startGradientColor, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Conotrols.NuGenGradientPanel.StartGradientColorChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnStartGradientColorChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_startGradientColorChanged, e);
		}

		/*
		 * WatermarkAlign
		 */

		private ContentAlignment _watermarkAlign = ContentAlignment.BottomRight;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(ContentAlignment.BottomRight)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_GradientPanel_WatermarkAlign")]
		public ContentAlignment WatermarkAlign
		{
			get
			{
				return _watermarkAlign;
			}
			set
			{
				if (_watermarkAlign != value)
				{
					_watermarkAlign = value;
					this.OnWatermarkAlignChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _watermarkAlignChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="WatermarkAlign"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_GradientPanel_WatermarkAlignChanged")]
		public event EventHandler WatermarkAlignChanged
		{
			add
			{
				this.Events.AddHandler(_watermarkAlignChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_watermarkAlignChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="WatermarkAlignChanged"/> event.
		/// </summary>
		protected virtual void OnWatermarkAlignChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_watermarkAlignChanged, e);
		}

		/*
		 * WatermarkSize
		 */

		private Size _watermarkSize;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_GradientPanel_WatermarkSize")]
		public Size WatermarkSize
		{
			get
			{
				if (_watermarkSize == Size.Empty)
				{
					return this.DefaultWatermarkSize;
				}

				return _watermarkSize;
			}
			set
			{
				if (_watermarkSize != value)
				{
					_watermarkSize = value;
					this.OnWatermarkSizeChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly Size _defaultWatermarkSize = new Size(150, 150);

		/// <summary>
		/// </summary>
		protected virtual Size DefaultWatermarkSize
		{
			get
			{
				return _defaultWatermarkSize;
			}
		}

		private static readonly object _watermarkSizeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="WatermarkSize"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_GradientPanel_WatermarkSizeChanged")]
		public event EventHandler WatermarkSizeChanged
		{
			add
			{
				this.Events.AddHandler(_watermarkSizeChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_watermarkSizeChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenGradientPanel.WatermarkSizeChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWatermarkSizeChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_watermarkSizeChanged, e);
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle bounds = this.ClientRectangle;

			if (bounds.Width > 0 && bounds.Height > 0)
			{
				using (Brush brush = new LinearGradientBrush(bounds, this.StartGradientColor, this.EndGradientColor, this.GradientDirection))
				{
					g.FillRectangle(brush, bounds);
				}

				if (this.Watermark != null)
				{
					Rectangle watermarkBounds = NuGenControlPaint.ImageBoundsFromContentAlignment(
						this.WatermarkSize
						, this.ClientRectangle
						, this.WatermarkAlign
					);

					if (this.Desaturate)
					{
						NuGenWatermarkRenderer.DrawDesaturatedWatermark(
							g
							, this.Watermark
							, watermarkBounds
							, this.WatermarkTransparency
						);
					}
					else
					{
						NuGenWatermarkRenderer.DrawWatermark(
							g
							, this.Watermark
							, watermarkBounds
							, this.WatermarkTransparency
						);
					}
				}
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenGradientPanel"/> class.
		/// </summary>
		public NuGenGradientPanel()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, false);
			this.SetStyle(ControlStyles.UserPaint, true);
		}
	}
}
