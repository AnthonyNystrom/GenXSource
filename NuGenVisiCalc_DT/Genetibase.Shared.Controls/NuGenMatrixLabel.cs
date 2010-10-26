/* -----------------------------------------------
 * NuGenMatrixLabel.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.MatrixLabelInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Renders text as a set of luminous dots.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenMatrixLabel), "Resources.NuGenIcon.png")]
	[DefaultEvent("Click")]
	[DefaultProperty("Text")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenMatrixLabelDesigner")]
	[NuGenSRDescription("Description_MatrixLabel")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenMatrixLabel : NuGenControl
	{
		#region Properties.Appearance

		/*
		 * OnColor
		 */

		private Color _onColor;

		/// <summary>
		/// Gets or sets the color of switched on dots.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_MatrixLabel_OnColor")]
		public Color OnColor
		{
			get
			{
				if (_onColor == Color.Empty)
				{
					return _defaultOnColor;
				}

				return _onColor;
			}
			set
			{
				if (_onColor != value)
				{
					_onColor = value;
					this.OnOnColorChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly Color _defaultOnColor = Color.LightGreen;

		private void ResetOnColor()
		{
			this.OnColor = _defaultOnColor;
		}

		private bool ShouldSerializeOnColor()
		{
			return this.OnColor != _defaultOnColor;
		}

		private static readonly Object _onColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="OnColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_MatrixLabel_OnColorChanged")]
		public event EventHandler OnColorChanged
		{
			add
			{
				this.Events.AddHandler(_onColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_onColorChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenMatrixLabel.OnColorChanged"/> event.
		/// </summary>
		protected virtual void OnOnColorChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_onColorChanged, e);
		}

		/*
		 * OnColorShadow
		 */

		private Color _onColorShadow;

		/// <summary>
		/// Gets or sets the color of switched on dot shadows.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_MatrixLabel_OnColorShadow")]
		public Color OnColorShadow
		{
			get
			{
				if (_onColorShadow == Color.Empty)
				{
					return _defaultOnColorShadow;
				}

				return _onColorShadow;
			}
			set
			{
				if (_onColorShadow != value)
				{
					_onColorShadow = value;
					this.OnOnColorShadowChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly Color _defaultOnColorShadow = Color.Green;

		private void ResetOnColorShadow()
		{
			this.OnColorShadow = _defaultOnColorShadow;
		}

		private bool ShouldSerializeOnColorShadow()
		{
			return this.OnColorShadow != _defaultOnColorShadow;
		}

		private static readonly Object _onColorShadowChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="OnColorShadow"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_MatrixLabel_OnColorShadowChanged")]
		public event EventHandler OnColorShadowChanged
		{
			add
			{
				this.Events.AddHandler(_onColorShadowChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_onColorShadowChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenMatrixLabel.OnColorShadowChanged"/> event.
		/// </summary>
		protected virtual void OnOnColorShadowChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_onColorShadowChanged, e);
		}

		/*
		 * OffColor
		 */

		private Color _offColor;

		/// <summary>
		/// Gets or sets the color of switched off dots.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_MatrixLabel_OffColor")]
		public Color OffColor
		{
			get
			{
				if (_offColor == Color.Empty)
				{
					return _defaultOffColor;
				}

				return _offColor;
			}
			set
			{
				if (_offColor != value)
				{
					_offColor = value;
					this.OnOffColorChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly Color _defaultOffColor = Color.DarkGreen;

		private void ResetOffColor()
		{
			this.OffColor = _defaultOffColor;
		}

		private bool ShouldSerializeOffColor()
		{
			return this.OffColor != _defaultOffColor;
		}

		private static readonly Object _offColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="OffColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_MatrixLabel_OffColorChanged")]
		public event EventHandler OffColorChanged
		{
			add
			{
				this.Events.AddHandler(_offColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_offColorChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenMatrixLabel.OffColorChanged"/> event.
		/// </summary>
		protected virtual void OnOffColorChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_offColorChanged, e);
		}

		/*
		 * OffColorShadow
		 */

		private Color _offColorShadow;

		/// <summary>
		/// Gets or sets the color of switched off dot shadows.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_MatrixLabel_OffColorShadow")]
		public Color OffColorShadow
		{
			get
			{
				if (_offColorShadow == Color.Empty)
				{
					return _defaultOffColorShadow;
				}

				return _offColorShadow;
			}
			set
			{
				if (_offColorShadow != value)
				{
					_offColorShadow = value;
					this.OnOffColorShadowChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly Color _defaultOffColorShadow = Color.Black;

		private void ResetOffColorShadow()
		{
			this.OffColorShadow = _defaultOffColorShadow;
		}

		private bool ShouldSerializeOffColorShadow()
		{
			return this.OffColorShadow != _defaultOffColorShadow;
		}

		private static readonly Object _offColorShadowChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="OffColorShadow"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_MatrixLabel_OffColorShadowChanged")]
		public event EventHandler OffColorShadowChanged
		{
			add
			{
				this.Events.AddHandler(_offColorShadowChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_offColorShadowChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenMatrixLabel.OffColorShadowChanged"/> event.
		/// </summary>
		protected virtual void OnOffColorShadowChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_offColorShadowChanged, e);
		}

		/*
		 * DotHeight
		 */

		private NuGenInt32 _dotHeightInternal;

		private NuGenInt32 DotHeightInternal
		{
			get
			{
				if (_dotHeightInternal == null)
				{
					_dotHeightInternal = new NuGenInt32(1, Int32.MaxValue);
					_dotHeightInternal.Value = 5;
				}

				return _dotHeightInternal;
			}
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <para><paramref name="value"/> should be greater or equal to 1.</para>
		/// </exception>
		[Browsable(true)]
		[DefaultValue(5)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_MatrixLabel_DotHeight")]
		public Int32 DotHeight
		{
			get
			{
				return this.DotHeightInternal.Value;
			}
			set
			{
				if (this.DotHeightInternal.Value != value)
				{
					this.DotHeightInternal.Value = value;
					this.OnDotHeightChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly Object _dotHeightChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="DotHeight"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_MatrixLabel_DotHeightChanged")]
		public event EventHandler DotHeightChanged
		{
			add
			{
				this.Events.AddHandler(_dotHeightChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_dotHeightChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenMatrixLabel.DotHeightChanged"/> event.
		/// </summary>
		protected virtual void OnDotHeightChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_dotHeightChanged, e);
		}

		/*
		 * DotWidth
		 */

		private NuGenInt32 _dotWidthInternal;

		private NuGenInt32 DotWidthInternal
		{
			get
			{
				if (_dotWidthInternal == null)
				{
					_dotWidthInternal = new NuGenInt32(1, Int32.MaxValue);
					_dotWidthInternal.Value = 5;
				}

				return _dotWidthInternal;
			}
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <para><paramref name="value"/> should be greater or equal to 1.</para>
		/// </exception>
		[Browsable(true)]
		[DefaultValue(5)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_MatrixLabel_DotWidth")]
		public int DotWidth
		{
			get
			{
				return this.DotWidthInternal.Value;
			}
			set
			{
				if (this.DotWidthInternal.Value != value)
				{
					this.DotWidthInternal.Value = value;
					this.OnDotWidthChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly Object _dotWidthChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="DotWidth"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_MatrixLabel_DotWidthChanged")]
		public event EventHandler DotWidthChanged
		{
			add
			{
				this.Events.AddHandler(_dotWidthChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_dotWidthChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenMatrixLabel.DotWidthChanged"/> event.
		/// </summary>
		protected virtual void OnDotWidthChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_dotWidthChanged, e);
		}

		/*
		 * DotSpace
		 */

		private NuGenInt32 _dotSpaceInternal;

		private NuGenInt32 DotSpaceInternal
		{
			get
			{
				if (_dotSpaceInternal == null)
				{
					_dotSpaceInternal = new NuGenInt32(0, Int32.MaxValue);
				}

				return _dotSpaceInternal;
			}
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <para><paramref name="value"/> should be greater or equal to 0.</para>
		/// </exception>
		[Browsable(true)]
		[DefaultValue(0)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_MatrixLabel_DotSpace")]
		public int DotSpace
		{
			get
			{
				return this.DotSpaceInternal.Value;
			}
			set
			{
				if (this.DotSpaceInternal.Value != value)
				{
					this.DotSpaceInternal.Value = value;
					this.OnDotSpaceChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly Object _dotSpaceChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="DotSpace"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_MatrixLabel_DotSpaceChanged")]
		public event EventHandler DotSpaceChanged
		{
			add
			{
				this.Events.AddHandler(_dotSpaceChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_dotSpaceChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenMatrixLabel.DotSpaceChanged"/> event.
		/// </summary>
		protected virtual void OnDotSpaceChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_dotSpaceChanged, e);
		}

		#endregion

		#region Properties.Public.Overridden

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[DefaultValue(typeof(Color), "Black")]
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

		/// <summary>
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

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.TextChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			this.Invalidate();
		}

		#endregion

		#region Properties.Protected.Overridden

		private static readonly Size _defaultSize = new Size(208, 32);

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

		private INuGenMatrixBuilder _matrixBuilder;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenMatrixBuilder MatrixBuilder
		{
			get
			{
				if (_matrixBuilder == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_matrixBuilder = this.ServiceProvider.GetService<INuGenMatrixBuilder>();

					if (_matrixBuilder == null)
					{
						throw new NuGenServiceNotFoundException<INuGenMatrixBuilder>();
					}
				}

				return _matrixBuilder;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.AutoSizeChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnAutoSizeChanged(EventArgs e)
		{
			base.OnAutoSizeChanged(e);
			this.Invalidate();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Char[] chars;

			if (this.Text != null)
			{
				chars = this.Text.ToCharArray();
			}
			else
			{
				chars = String.Empty.ToCharArray();
			}

			List<Boolean[,]> dotMatrix = new List<Boolean[,]>();
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.HighQuality;

			for (int i = 0; i < chars.Length; i++)
			{
				dotMatrix.Add(new Boolean[5, 5]);
			}

			// Loop through all chars in text.
			for (Int32 i = 0; i < chars.Length; i++)
			{
				this.MatrixBuilder.BuildCharMatrix(Char.ToLower(chars[i]), dotMatrix[i]);
			}

			Int32 charWidth = 5;

			for (Int32 i = 0; i < chars.Length; i++)
			{
				for (Int32 j = 0; j < charWidth; j++)
				{
					for (Int32 k = 0; k < 5; k++)
					{
						Int32 dotX, dotY, offsetX;
						offsetX = i * (this.DotWidth + this.DotSpace) * 6;
						dotX = offsetX + (j) * (this.DotWidth + this.DotSpace);
						dotY = (k) * (this.DotHeight + this.DotSpace);
						Rectangle dotArea = new Rectangle(dotX, dotY, this.DotWidth, this.DotHeight);

						if (dotArea.Width > 0 && dotArea.Height > 0)
						{
							if (dotMatrix[i][j, k])
							{
								using (LinearGradientBrush onBrush = new LinearGradientBrush(dotArea, this.OnColor, this.OnColorShadow, LinearGradientMode.ForwardDiagonal))
								{
									g.FillEllipse(onBrush, dotX, dotY, this.DotWidth, this.DotHeight);
								}
							}
							else
							{
								using (LinearGradientBrush offBrush = new LinearGradientBrush(dotArea, this.OffColor, this.OffColorShadow, LinearGradientMode.ForwardDiagonal))
								{
									g.FillEllipse(offBrush, dotX, dotY, this.DotWidth, this.DotHeight);
								}
							}
						}
					}
				}
			}

			if (this.AutoSize)
			{
				Int32 newWidth, newHeight;
				newWidth = chars.Length * this.DotWidth * 6;
				newHeight = 5 * this.DotHeight;
				PropertyDescriptor sizeDescriptor = TypeDescriptor.GetProperties(this)["Size"];

				try
				{
					sizeDescriptor.SetValue(this, new Size(newWidth, newHeight));
				}
				catch (InvalidOperationException)
				{
				}
			}

			base.OnPaint(e);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMatrixLabel"/> class.
		/// </summary>
		public NuGenMatrixLabel()
			: this(NuGenServiceManager.MatrixLabelServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMatrixLabel"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenButtonStateService"/></para>
		/// <para><see cref="INuGenControlStateService"/></para>
		/// <para><see cref="INuGenMatrixBuilder"/></para>
		/// </param>
		public NuGenMatrixLabel(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.BackColor = Color.Black;
		}
	}
}
