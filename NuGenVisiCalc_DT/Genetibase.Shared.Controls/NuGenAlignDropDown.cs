/* -----------------------------------------------
 * NuGenAlignDropDown.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.DropDownInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.RadioButtonInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.Shared.Controls.ComponentModel;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a <see cref="NuGenDropDown"/> derived control that uses <see cref="NuGenAlignSelector"/> as a popup.
	/// </summary>
	[ToolboxItem(false)]
	public class NuGenAlignDropDown : NuGenDropDown
	{
		private ContentAlignment _selectedAlignment = ContentAlignment.MiddleCenter;

		/// <summary>
		/// Gets or sets the alignment selected by the user.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_AlignDropDown_SelectedAlignment")]
		public ContentAlignment SelectedAlignment
		{
			get
			{
				return _selectedAlignment;
			}
			set
			{
				if (_selectedAlignment != value)
				{
					_selectedAlignment = value;
					this.SetSelectedAlignment(value);
					this.OnSelectedAlignmentChanged(EventArgs.Empty);
				}
			}
		}

		private void SetSelectedAlignment(ContentAlignment value)
		{
			_alignSelector.Alignment = value;
			base.Text = NuGenAlignDropDown.GetAlignmentText(value);
		}

		private static readonly object _selectedAlignmentChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SelectedAlignment"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_AlignDropDown_SelectedAlignmentChanged")]
		public event EventHandler SelectedAlignmentChanged
		{
			add
			{
				this.Events.AddHandler(_selectedAlignmentChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_selectedAlignmentChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Shared.Controls.NuGenAlignDropDown.SelectedAlignmentChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnSelectedAlignmentChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_selectedAlignmentChanged, e);
		}

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

		private static string GetAlignmentText(ContentAlignment alignment)
		{
			return TypeDescriptor.GetConverter(alignment).ConvertToString(alignment);
		}

		private void _alignSelector_AlignmentAccepted(object sender, EventArgs e)
		{
			this.SelectedAlignment = _alignSelector.Alignment;
			this.CloseDropDown();
		}

		private void _alignSelector_AlignmentCanceled(object sender, EventArgs e)
		{
			this.CloseDropDown();
		}

		private NuGenAlignSelector _alignSelector;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenAlignDropDown"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateTracker"/></para>
		/// 	<para><see cref="INuGenControlStateTracker"/></para>
		/// 	<para><see cref="INuGenControlImageManager"/></para>
		/// 	<para><see cref="INuGenDropDownRenderer"/></para>
		/// 	<para><see cref="INuGenPanelRenderer"/></para>
		///		<para><see cref="INuGenRadioButtonLayoutManager"/></para>
		///		<para><see cref="INuGenRadioButtonRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenAlignDropDown(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_alignSelector = new NuGenAlignSelector(serviceProvider);
			_alignSelector.AlignmentAccepted += _alignSelector_AlignmentAccepted;
			_alignSelector.AlignmentCanceled += _alignSelector_AlignmentCanceled;

			base.PopupBorderStyle = FormBorderStyle.None;
			base.PopupControl = _alignSelector;
			base.PopupSize = _alignSelector.Size;

			this.SetSelectedAlignment(ContentAlignment.MiddleCenter);
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to dispose both managed and unmanaged resouces; <see langword="false"/> to relaease only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_alignSelector != null)
				{
					base.PopupControl = null;
					_alignSelector.AlignmentAccepted -= _alignSelector_AlignmentAccepted;
					_alignSelector.AlignmentCanceled -= _alignSelector_AlignmentCanceled;
					_alignSelector.Dispose();
					_alignSelector = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
