/* -----------------------------------------------
 * NuGenHotKeySelector.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.CheckBoxInternals;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.DropDownInternals;
using Genetibase.Shared.Controls.HotKeySelectorInternals;
using Genetibase.Shared.Controls.LabelInternals;
using Genetibase.Shared.Controls.PanelInternals;
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
	/// A combo box like control with <see cref="NuGenHotKeyPopup"/> window as a drop down.
	/// </summary>
	[ToolboxItem(false)]
	public class NuGenHotKeySelector : NuGenDropDown
	{
		#region Properties.Behavior

		/*
		 * SelectedHotKeys
		 */

		private Keys _selectedHotKeys;

		/// <summary>
		/// Gets or sets the hot key combination selected by the user.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Keys), "None")]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_HotKeySelector_SelectedHotKeys")]
		public Keys SelectedHotKeys
		{
			get
			{
				return _selectedHotKeys;
			}
			set
			{
				_selectedHotKeys = value;
				base.Text = this.KeyConverter.ConvertToString(_selectedHotKeys);
				_hotKeyPopup.SetSelectedHotKeys(_selectedHotKeys);

				this.OnSelectedHotKeysChanged(EventArgs.Empty);
			}
		}

		private static readonly object _selectedHotKeysChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SelectedHotKeys"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_HotKeySelector_SelectedHotKeysChanged")]
		public event EventHandler SelectedHotKeysChanged
		{
			add
			{
				this.Events.AddHandler(_selectedHotKeysChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_selectedHotKeysChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenHotKeySelector.SelectedHotKeysChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSelectedHotKeysChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_selectedHotKeysChanged, e);
		}

		#endregion

		#region Properties.Private

		private TypeConverter _keyConverter;

		private TypeConverter KeyConverter
		{
			get
			{
				if (_keyConverter == null)
				{
					_keyConverter = TypeDescriptor.GetConverter(typeof(Keys));
					Debug.Assert(_keyConverter != null, "_keyConverter != null");
				}

				return _keyConverter;
			}
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

		#region EventHandlers

		private void _hotKeyPopup_SelectionAccepted(object sender, NuGenHotKeyEventArgs e)
		{
			this.SelectedHotKeys = e.HotKeys;
			this.CloseDropDown();
		}

		private void _hotKeyPopup_SelectionCanceled(object sender, EventArgs e)
		{
			this.CloseDropDown();
		}

		#endregion

		private NuGenHotKeyPopup _hotKeyPopup;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenHotKeySelector"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		///		<para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateTracker"/></para>
		/// 	<para><see cref="INuGenControlStateTracker"/></para>
		///		<para><see cref="INuGenButtonLayoutManager"/></para>
		///		<para><see cref="INuGenButtonRenderer"/></para>
		///		<para><see cref="INuGenCheckBoxLayoutManager"/></para>
		///		<para><see cref="INuGenCheckBoxRenderer"/></para>
		///		<para><see cref="INuGenComboBoxRenderer"/></para>
		///		<para><see cref="INuGenDropDownRenderer"/></para>
		///		<para><see cref="INuGenLabelLayoutManager"/></para>
		/// 	<para><see cref="INuGenLabelRenderer"/></para>
		/// 	<para><see cref="INuGenPanelRenderer"/></para>
		///		<para><see cref="INuGenControlImageManager"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenHotKeySelector(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_hotKeyPopup = new NuGenHotKeyPopup(serviceProvider);
			_hotKeyPopup.SelectionAccepted += _hotKeyPopup_SelectionAccepted;
			_hotKeyPopup.SelectionCanceled += _hotKeyPopup_SelectionCanceled;

			base.PopupBorderStyle = FormBorderStyle.None;
			base.PopupControl = _hotKeyPopup;
			base.PopupSize = _hotKeyPopup.Size;

			this.SelectedHotKeys = Keys.None;
		}
	}
}
