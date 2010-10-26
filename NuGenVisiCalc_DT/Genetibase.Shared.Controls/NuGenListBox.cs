/* -----------------------------------------------
 * NuGenListBox.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.ListBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// <seealso cref="ListBox"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenListBox), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenListBox : ListBox, INuGenImageListProvider
	{
		#region Properties.Appearance

		/*
		 * ImageList
		 */

		private ImageList _imageList;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ListBox_ImageList")]
		public ImageList ImageList
		{
			get
			{
				return _imageList;
			}
			set
			{
				if (_imageList != value)
				{
					_imageList = value;
					this.Invalidate();

					this.OnImageListChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _imageListChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ImageList"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ListBox_ImageListChanged")]
		public event EventHandler ImageListChanged
		{
			add
			{
				this.Events.AddHandler(_imageListChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_imageListChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ImageListChanged"/> event.
		/// </summary>
		protected virtual void OnImageListChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_imageListChanged, e);
		}

		#endregion

		#region Properties.Behavior

		/*
		 * HotTrack
		 */

		private bool _hotTrack;

		/// <summary>
		/// Gets or sets the value indicating whether an item under the cursor is highlighted automatically.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ListBox_HotTrack")]
		public bool HotTrack
		{
			get
			{
				return _hotTrack;
			}
			set
			{
				if (_hotTrack != value)
				{
					_hotTrack = value;
					this.OnHotTrackChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _hotTrackChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="HotTrack"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ListBox_HotTrackChanged")]
		public event EventHandler HotTrackChanged
		{
			add
			{
				this.Events.AddHandler(_hotTrackChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_hotTrackChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="HotTrackChanged"/> event.
		/// </summary>
		protected virtual void OnHotTrackChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_hotTrackChanged, e);
		}

		#endregion

		#region Properties.Hidden

		/*
		 * DrawMode
		 */

		/// <summary>
		/// Do not use this property. Any value will not affect the appearance.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new DrawMode DrawMode
		{
			get
			{
				return base.DrawMode;
			}
			set
			{
				base.DrawMode = value;
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * ImageListService
		 */

		private INuGenImageListService _imageListService;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenImageListService ImageListService
		{
			get
			{
				if (_imageListService == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_imageListService = this.ServiceProvider.GetService<INuGenImageListService>();

					if (_imageListService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenImageListService>();
					}
				}

				return _imageListService;
			}
		}

		/*
		 * Initiator
		 */

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

		/*
		 * Renderer
		 */

		private INuGenListBoxRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenListBoxRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenListBoxRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenListBoxRenderer>();
					}
				}

				return _renderer;
			}
		}

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider;

		/// <summary>
		/// </summary>
		protected INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		/*
		 * StateTracker
		 */

		private INuGenControlStateTracker _stateTracker;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenControlStateTracker StateTracker
		{
			get
			{
				if (_stateTracker == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					INuGenControlStateService stateService = this.ServiceProvider.GetService<INuGenControlStateService>();

					if (stateService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenControlStateService>();
					}

					_stateTracker = stateService.CreateStateTracker();
				}

				return _stateTracker;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnDrawItem
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.ListBox.DrawItem"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"></see> that contains the event data.</param>
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (e.Index < 0 || this.Items.Count < 1)
			{
				return;
			}

			NuGenItemParams itemParams = new NuGenItemParams(e.Graphics);

			itemParams.BackgroundColor = e.BackColor;
			itemParams.Bounds = e.Bounds;
			itemParams.Font = this.Font;
			itemParams.ForeColor = e.ForeColor;
			itemParams.State = e.State;

			if (this.ImageList != null)
			{
				itemParams.Image = this.ImageListService.FindImageAtIndex(this.ImageList, e.Index);
			}

			itemParams.Text = this.GetItemText(this.Items[e.Index]);
			this.Renderer.DrawItem(NuGenItemService.BuildItemPaintParams(itemParams));
		}

		/*
		 * OnEnabledChanged
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			this.StateTracker.Enabled(this.Enabled);
			base.OnEnabledChanged(e);
		}

		/*
		 * OnMeasureItem
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.ListBox.MeasureItem"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MeasureItemEventArgs"></see> that contains the event data.</param>
		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			if (e.Index < 0 || this.Items.Count < 1)
			{
				return;
			}

			Size itemSize = NuGenItemService.GetItemSize(
				e.Graphics,
				this.ImageList,
				this.GetItemText(this.Items[e.Index]),
				this.Font
			);

			e.ItemWidth = itemSize.Width;
			e.ItemHeight = itemSize.Height;
		}

		/*
		 * OnMouseMove
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (this.HotTrack)
			{
				int index = this.IndexFromPoint(e.Location);

				if (index != -1)
				{
					this.SelectedIndex = index;
				}
			}

			base.OnMouseMove(e);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenListBox"/> class.
		/// </summary>
		public NuGenListBox()
			: this(NuGenServiceManager.ListBoxServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenListBox"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenControlStateService"/></para>
		/// <para><see cref="INuGenListBoxRenderer"/></para>
		/// <para><see cref="INuGenImageListService"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenListBox(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;
			this.DrawMode = DrawMode.OwnerDrawVariable;
		}
	}
}
