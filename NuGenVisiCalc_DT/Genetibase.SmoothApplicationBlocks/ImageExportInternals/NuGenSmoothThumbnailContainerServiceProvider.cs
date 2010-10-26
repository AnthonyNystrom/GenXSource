/* -----------------------------------------------
 * NuGenSmoothThumbnailContainerServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.ScrollBarInternals;
using Genetibase.Shared.Controls.SwitcherInternals;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Controls.TrackBarInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls;
using Genetibase.SmoothControls.PanelInternals;
using Genetibase.SmoothControls.ScrollBarInternals;
using Genetibase.SmoothControls.SwitcherInternals;
using Genetibase.SmoothControls.ToolStripInternals;
using Genetibase.SmoothControls.TrackBarInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothApplicationBlocks.ImageExportInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlImageManager"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenPanelRenderer"/></para>
	/// <para><see cref="INuGenScrollBarRenderer"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// <para><see cref="INuGenSwitchButtonLayoutManager"/></para>
	/// <para><see cref="INuGenSwitchButtonRenderer"/></para>
	/// <para><see cref="INuGenThumbnailLayoutManager"/></para>
	/// <para><see cref="INuGenThumbnailRenderer"/></para>
	/// <para><see cref="INuGenToolStripRenderer"/></para>
	/// <para><see cref="INuGenTrackBarRenderer"/></para>
	/// <para><see cref="INuGenValueTrackerService"/></para>
	/// </summary>
	public class NuGenSmoothThumbnailContainerServiceProvider : NuGenImageControlServiceProvider
	{
		private INuGenPanelRenderer _panelRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenPanelRenderer PanelRenderer
		{
			get
			{
				if (_panelRenderer == null)
				{
					_panelRenderer = new NuGenSmoothPanelRenderer(this);
				}

				return _panelRenderer;
			}
		}

		private INuGenScrollBarRenderer _scrollBarRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenScrollBarRenderer ScrollBarRenderer
		{
			get
			{
				if (_scrollBarRenderer == null)
				{
					_scrollBarRenderer = new NuGenSmoothScrollBarRenderer(this);
				}

				return _scrollBarRenderer;
			}
		}

		private INuGenSwitchButtonLayoutManager _switchButtonLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenSwitchButtonLayoutManager SwitchButtonLayoutManager
		{
			get
			{
				if (_switchButtonLayoutManager == null)
				{
					_switchButtonLayoutManager = new NuGenSmoothSwitchButtonLayoutManager();
				}

				return _switchButtonLayoutManager;
			}
		}

		private INuGenSwitchButtonRenderer _switchButtonRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenSwitchButtonRenderer SwitchButtonRenderer
		{
			get
			{
				if (_switchButtonRenderer == null)
				{
					_switchButtonRenderer = new NuGenSmoothSwitchButtonRenderer(this);
				}

				return _switchButtonRenderer;
			}
		}

		private INuGenThumbnailLayoutManager _thumbnailLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenThumbnailLayoutManager ThumbnailLayoutManager
		{
			get
			{
				if (_thumbnailLayoutManager == null)
				{
					_thumbnailLayoutManager = new NuGenSmoothThumbnailLayoutManager();
				}

				return _thumbnailLayoutManager;
			}
		}

		private INuGenThumbnailRenderer _thumbnailRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenThumbnailRenderer ThumbnailRenderer
		{
			get
			{
				if (_thumbnailRenderer == null)
				{
					_thumbnailRenderer = new NuGenSmoothThumbnailRenderer(this);
				}

				return _thumbnailRenderer;
			}
		}

		private INuGenToolStripRenderer _toolStripRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenToolStripRenderer ToolStripRenderer
		{
			get
			{
				if (_toolStripRenderer == null)
				{
					_toolStripRenderer = new NuGenSmoothToolStripRenderer();
				}

				return _toolStripRenderer;
			}
		}

		private INuGenTrackBarRenderer _trackBarRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenTrackBarRenderer TrackBarRenderer
		{
			get
			{
				if (_trackBarRenderer == null)
				{
					_trackBarRenderer = new NuGenSmoothTrackBarRenderer(this);
				}

				return _trackBarRenderer;
			}
		}

		private INuGenValueTrackerService _valueTrackerService;

		/// <summary>
		/// </summary>
		protected virtual INuGenValueTrackerService ValueTrackerService
		{
			get
			{
				if (_valueTrackerService == null)
				{
					_valueTrackerService = new NuGenValueTrackerService();
				}

				return _valueTrackerService;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="serviceType"/> is <see langword="null"/>.</exception>
		protected override object GetService(Type serviceType)
		{
			if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}
			else if (serviceType == typeof(INuGenThumbnailRenderer))
			{
				Debug.Assert(this.ThumbnailRenderer != null, "this.ThumbnailRenderer != null");
				return this.ThumbnailRenderer;
			}
			else if (serviceType == typeof(INuGenPanelRenderer))
			{
				Debug.Assert(this.PanelRenderer != null, "this.PanelRenderer != null");
				return this.PanelRenderer;
			}
			else if (serviceType == typeof(INuGenScrollBarRenderer))
			{
				Debug.Assert(this.ScrollBarRenderer != null, "this.ScrollBarRenderer != null");
				return this.ScrollBarRenderer;
			}
			else if (serviceType == typeof(INuGenValueTrackerService))
			{
				Debug.Assert(this.ValueTrackerService != null, "this.ValueTrackerService != null");
				return this.ValueTrackerService;
			}
			else if (serviceType == typeof(INuGenThumbnailLayoutManager))
			{
				Debug.Assert(this.ThumbnailLayoutManager != null, "this.ThumbnailLayoutManager != null");
				return this.ThumbnailLayoutManager;
			}
			else if (serviceType == typeof(INuGenToolStripRenderer))
			{
				Debug.Assert(this.ToolStripRenderer != null, "this.ToolStripRenderer != null");
				return this.ToolStripRenderer;
			}
			else if (serviceType == typeof(INuGenSwitchButtonRenderer))
			{
				Debug.Assert(this.SwitchButtonRenderer != null, "this.SwitchButtonRenderer != null");
				return this.SwitchButtonRenderer;
			}
			else if (serviceType == typeof(INuGenSwitchButtonLayoutManager))
			{
				Debug.Assert(this.SwitchButtonLayoutManager != null, "this.SwitchButtonLayoutManager != null");
				return this.SwitchButtonLayoutManager;
			}
			else if (serviceType == typeof(INuGenTrackBarRenderer))
			{
				Debug.Assert(this.TrackBarRenderer != null, "this.TrackBarRenderer != null");
				return this.TrackBarRenderer;
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothThumbnailContainerServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothThumbnailContainerServiceProvider()
		{
		}
	}
}
