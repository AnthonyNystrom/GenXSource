/* -----------------------------------------------
 * NuGenColorBox.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.ColorBoxInternals;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.DropDownInternals;
using Genetibase.Shared.Controls.ListBoxInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.TabControlInternals;
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
	/// A combo box like control with a <see cref="NuGenColorBoxPopup"/> window as a drop down.
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenColorBox : NuGenDropDown
	{
		#region Properties.Behavior

		/*
		 * SelectedColor
		 */

		private Color _selectedColor;

		/// <summary>
		/// Gets or sets the color selected by the user.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ColorBox_SelectedColor")]
		public Color SelectedColor
		{
			get
			{
				if (_selectedColor.IsEmpty)
				{
					return _defaultSelectedColor;
				}

				return _selectedColor;
			}
			set
			{
				if (_selectedColor != value)
				{
					_selectedColor = value;
					base.Text = NuGenColorBox.GetColorText(_selectedColor);
					base.Image = this.GetColorSample(_selectedColor);
					_colorBoxPopup.SetSelectedColor(_selectedColor);

					this.OnSelectedColorChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Color _defaultSelectedColor = Color.Transparent;

		private void ResetSelectedColor()
		{
			this.SelectedColor = _defaultSelectedColor;
		}

		private bool ShouldSerializeSelectedColor()
		{
			return this.SelectedColor != _defaultSelectedColor;
		}

		private static readonly object _selectedColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SelectedColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ColorBox_SelectedColorChanged")]
		public event EventHandler SelectedColorChanged
		{
			add
			{
				this.Events.AddHandler(_selectedColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_selectedColorChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="SelectedColorChanged"/> event.
		/// </summary>
		protected virtual void OnSelectedColorChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_selectedColorChanged, e);
		}

		#endregion

		#region Properties.Hidden

		/*
		 * BackgroundImage
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/*
		 * BackgroundImageLayout
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		/*
		 * Image
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Image Image
		{
			get
			{
				return null;
			}
		}

		/*
		 * ImageIndex
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new int ImageIndex
		{
			get
			{
				return -1;
			}
		}

		/*
		 * ImageList
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ImageList ImageList
		{
			get
			{
				return null;
			}
		}

		/*
		 * PopupBorderStyle
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new FormBorderStyle PopupBorderStyle
		{
			get
			{
				return base.PopupBorderStyle;
			}
		}

		/*
		 * PopupControl
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Control PopupControl
		{
			get
			{
				return base.PopupControl;
			}
		}

		/*
		 * PopupSize
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Size PopupSize
		{
			get
			{
				return base.PopupSize;
			}
		}

		/*
		 * Text
		 */

		/// <summary>
		/// </summary>
		/// <value></value>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnSizeChanged
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.Image = this.GetColorSample(this.SelectedColor);
			base.OnSizeChanged(e);
		}

		#endregion

		#region Methods.Private

		private Image GetColorSample(Color color)
		{
			if (color == Color.Transparent)
			{
				color = Color.White;
			}

			Rectangle sampleRectangle = new Rectangle(0, 0, 28, Math.Max(6, this.ClientRectangle.Height - 6));
			Image image = new Bitmap(sampleRectangle.Width, sampleRectangle.Height);

			using (Graphics g = Graphics.FromImage(image))
			{
				using (SolidBrush sb = new SolidBrush(color))
				using (Pen pen = new Pen(Color.Black))
				{
					g.FillRectangle(sb, sampleRectangle);
					g.DrawRectangle(pen, NuGenControlPaint.BorderRectangle(sampleRectangle));
				}
			}

			return image;
		}

		private static string GetColorText(Color color)
		{
			return TypeDescriptor.GetConverter(color).ConvertToString(color);
		}

		#endregion

		#region EventHandlers

		private void _colorBoxPopup_ColorSelected(object sender, NuGenColorEventArgs e)
		{
			this.SelectedColor = e.SelectedColor;
			this.CloseDropDown();
		}

		#endregion

		private NuGenColorBoxPopup _colorBoxPopup;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenColorBox"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		///		<para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateTracker"/></para>
		/// 	<para><see cref="INuGenControlStateTracker"/></para>
		///		<para><see cref="INuGenControlImageManager"/></para>
		/// 	<para><see cref="INuGenTabStateTracker"/></para>
		/// 	<para><see cref="INuGenButtonRenderer"/></para>
		///		<para><see cref="INuGenDropDownRenderer"/></para>
		/// 	<para><see cref="INuGenImageListService"/></para>
		/// 	<para><see cref="INuGenListBoxRenderer"/></para>
		///		<para><see cref="INuGenPanelRenderer"/></para>
		///		<para><see cref="INuGenTabLayoutManager"/></para>
		/// 	<para><see cref="INuGenTabRenderer"/></para>
		/// 	<para><see cref="INuGenColorsProvider"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenColorBox(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_colorBoxPopup = new NuGenColorBoxPopup(serviceProvider);
			this.SelectedColor = _defaultSelectedColor;
			_colorBoxPopup.ColorSelected += _colorBoxPopup_ColorSelected;

			base.PopupBorderStyle = FormBorderStyle.None;
			base.PopupControl = _colorBoxPopup;
			base.PopupSize = _colorBoxPopup.Size;
		}
	}
}
