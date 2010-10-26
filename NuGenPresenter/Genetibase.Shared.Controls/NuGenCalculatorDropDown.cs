/* -----------------------------------------------
 * NuGenCalculatorDropDown.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.CalculatorInternals;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.DropDownInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.TextBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenCalculatorDropDown : NuGenDropDown
	{
		#region Properties.Behavior

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(0.0)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_Calculator_Value")]
		public double Value
		{
			get
			{
				return _calcPopup.Value;
			}
			set
			{
				_calcPopup.Value = value;
				base.Text = value.ToString(CultureInfo.CurrentUICulture);
			}
		}

		private static readonly object _valueChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Value"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calculator_ValueChanged")]
		public event EventHandler ValueChanged
		{
			add
			{
				this.Events.AddHandler(_valueChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_valueChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ValueChanged"/> event.
		/// </summary>
		protected virtual void OnValueChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_valueChanged, e);
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
				return FormBorderStyle.None;
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
				return null;
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
				return Size.Empty;
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

		private void _calcPopup_ValueAccepted(object sender, EventArgs e)
		{
			this.Value = _calcPopup.Value;
			this.CloseDropDown();
		}

		private void _calcPopup_ValueCanceled(object sender, EventArgs e)
		{
			this.CloseDropDown();
		}

		private NuGenCalculatorPopup _calcPopup;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCalculatorDropDown"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		///		<para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		///		<para><see cref="INuGenDropDownRenderer"/></para>
		///		<para><see cref="INuGenPanelRenderer"/></para>
		///		<para><see cref="INuGenTextBoxRenderer"/></para>
		///		<para><see cref="INuGenControlImageManager"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenCalculatorDropDown(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_calcPopup = new NuGenCalculatorPopup(serviceProvider);
			_calcPopup.ValueAccepted += _calcPopup_ValueAccepted;
			_calcPopup.ValueCanceled += _calcPopup_ValueCanceled;

			base.PopupBorderStyle = FormBorderStyle.None;
			base.PopupControl = _calcPopup;
			base.PopupSize = _calcPopup.Size;
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_calcPopup != null)
				{
					_calcPopup.ValueAccepted -= _calcPopup_ValueAccepted;
					_calcPopup.ValueCanceled -= _calcPopup_ValueCanceled;
					_calcPopup.Dispose();
					_calcPopup = null;					
				}
			}

			base.Dispose(disposing);
		}
	}
}
