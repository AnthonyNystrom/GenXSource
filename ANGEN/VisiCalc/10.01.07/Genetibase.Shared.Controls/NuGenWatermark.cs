/* -----------------------------------------------
 * NuGenWatermark.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ComponentModel;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.Shared.ComponentModel;
using System.Drawing.Design;
using Genetibase.Shared.Drawing;
using System.Drawing.Imaging;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenWatermark), "Resources.NuGenIcon.png")]
	[DefaultEvent("Click")]
	[DefaultProperty("Watermark")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenWatermarkDesigner")]
	[NuGenSRDescription("Description_Watermark")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenWatermark : UserControl
	{
		#region Properties.Appearance

		/*
		 * Desaturate
		 */

		private bool _desaturate;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Watermark_Desaturate")]
		public bool Desaturate
		{
			get
			{
				return _desaturate;
			}
			set
			{
				if (_desaturate != value)
				{
					_desaturate = value;
					this.OnDesaturateChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _desaturateChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Desaturate"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Watermark_DesaturateChanged")]
		public event EventHandler DesaturateChanged
		{
			add
			{
				this.Events.AddHandler(_desaturateChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_desaturateChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="DesaturateChanged"/> event.
		/// </summary>
		protected virtual void OnDesaturateChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_desaturateChanged, e);
		}

		/*
		 * Watermark
		 */

		private Image _watermark;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Watermark_Watermark")]
		public Image Watermark
		{
			get
			{
				return _watermark;
			}
			set
			{
				if (_watermark != value)
				{
					_watermark = value;
					this.OnWatermarkChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _watermarkChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Watermark"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Watermark_WatermarkChanged")]
		public event EventHandler WatermarkChanged
		{
			add
			{
				this.Events.AddHandler(_watermarkChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_watermarkChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenWatermark.WatermarkChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWatermarkChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_watermarkChanged, e);
		}

		/*
		 * WatermarkTransparency
		 */

		private int _watermarkTransparency = 80;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(80)]
		[Editor("Genetibase.Shared.Design.NuGenTransparencyEditor", typeof(UITypeEditor))]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Watermark_WatermarkTransparency")]
		[TypeConverter("Genetibase.Shared.Design.NuGenTransparencyConverter")]
		public int WatermarkTransparency
		{
			get
			{
				return _watermarkTransparency;
			}
			set
			{
				if (_watermarkTransparency != value)
				{
					_watermarkTransparency = value;
					this.OnWatermarkTransparencyChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _watermarkTransparencyChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="WatermarkTransparency"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Watermark_WatermarkTransparencyChanged")]
		public event EventHandler WatermarkTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(_watermarkTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_watermarkTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenWatermark.WatermarkTransparencyChanged"/> event.
		/// </summary>
		protected virtual void OnWatermarkTransparencyChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_watermarkTransparencyChanged, e);
		}

		#endregion

		#region Properties.Services

		private INuGenEventInitiatorService _initiator;

		/// <summary>
		/// </summary>
		protected virtual INuGenEventInitiatorService Initiator
		{
			get
			{
				if (_initiator == null)
				{
					_initiator = new NuGenEventInitiatorService(this, this.Events);
				}

				return _initiator;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (this.Watermark != null)
			{
				if (this.Desaturate)
				{
					NuGenWatermarkRenderer.DrawDesaturatedWatermark(
						e.Graphics
						, _watermark
						, this.ClientRectangle
						, _watermarkTransparency
					);
				}
				else
				{
					NuGenWatermarkRenderer.DrawWatermark(
						e.Graphics
						, _watermark
						, this.ClientRectangle
						, _watermarkTransparency
					);
				}
			}

			base.OnPaint(e);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWatermark"/> class.
		/// </summary>
		public NuGenWatermark()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, false);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.TabStop = false;
		}
	}
}
