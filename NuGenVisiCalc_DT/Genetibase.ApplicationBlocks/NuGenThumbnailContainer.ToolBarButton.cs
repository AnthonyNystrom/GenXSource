/* -----------------------------------------------
 * NuGenThumbnailContainer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.SwitcherInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenThumbnailContainer
	{
		private sealed class ToolBarButton : NuGenSwitchButton
		{
			private INuGenThumbnailLayoutManager _thumbnailLayoutManager;

			private INuGenThumbnailLayoutManager ThumbnailLayoutManager
			{
				get
				{
					if (_thumbnailLayoutManager == null)
					{
						Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
						_thumbnailLayoutManager = this.ServiceProvider.GetService<INuGenThumbnailLayoutManager>();

						if (_thumbnailLayoutManager == null)
						{
							throw new NuGenServiceNotFoundException<INuGenThumbnailLayoutManager>();
						}
					}

					return _thumbnailLayoutManager;
				}
			}

			/// <summary>
			/// Raises the <see cref="E:System.Windows.Forms.Control.RightToLeftChanged"/> event.
			/// </summary>
			/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
			protected override void OnRightToLeftChanged(EventArgs e)
			{
				base.OnRightToLeftChanged(e);

				if (this.RightToLeft == RightToLeft.Yes)
				{
					this.Dock = DockStyle.Right;
				}
				else
				{
					this.Dock = DockStyle.Left;
				}
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="ToolBarButton"/> class.
			/// </summary>
			/// <param name="serviceProvider"><para>Requires:</para>
			/// 	<para><see cref="INuGenButtonStateService"/></para>
			/// 	<para><see cref="INuGenControlStateService"/></para>
			/// 	<para><see cref="INuGenSwitchButtonLayoutManager"/></para>
			/// 	<para><see cref="INuGenSwitchButtonRenderer"/></para>
			///		<para><see cref="INuGenThumbnailLayoutManager"/></para>
			/// </param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			public ToolBarButton(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this.Dock = DockStyle.Left;
				this.ImageAlign = ContentAlignment.MiddleCenter;
				this.Size = this.ThumbnailLayoutManager.GetToolbarButtonSize();
			}
		}
	}
}
