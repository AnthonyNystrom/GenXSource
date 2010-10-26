/* -----------------------------------------------
 * NuGenThumbnail.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenThumbnail
	{
		private class RotateButton : NuGenWndLessUIControl
		{
			private ImageRotationStyle _style;

			public ImageRotationStyle Style
			{
				get
				{
					return _style;
				}
				set
				{
					_style = value;
				}
			}

			private INuGenThumbnailRenderer _renderer;

			/// <summary>
			/// </summary>
			/// <exception cref="NuGenServiceNotFoundException"/>
			protected INuGenThumbnailRenderer Renderer
			{
				get
				{
					if (_renderer == null)
					{
						Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
						_renderer = this.ServiceProvider.GetService<INuGenThumbnailRenderer>();

						if (_renderer == null)
						{
							throw new NuGenServiceNotFoundException<INuGenThumbnailRenderer>();
						}
					}

					return _renderer;
				}
			}

			/// <summary>
			/// Add custom logic before the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.Paint"/> event will be raised.
			/// </summary>
			/// <param name="e"></param>
			protected override void OnPaint(PaintEventArgs e)
			{
				NuGenControlState currentState = this.ButtonStateTracker.GetControlState();
				NuGenPaintParams paintParams = new NuGenPaintParams(e.Graphics);
				paintParams.Bounds = this.Bounds;
				paintParams.State = currentState;

				switch (this.Style)
				{
					case ImageRotationStyle.CCW:
					{
						this.Renderer.DrawCCWRotateButton(paintParams);
						break;
					}
					default:
					{
						this.Renderer.DrawCWRotateButton(paintParams);
						break;
					}
				}

				base.OnPaint(e);
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="RotateButton"/> class.
			/// </summary>
			/// <param name="serviceProvider"><para>Requires:</para>
			/// 	<para><see cref="INuGenButtonStateService"/></para>
			///		<para><see cref="INuGenThumbnailRenderer"/></para>
			/// </param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			public RotateButton(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
			}
		}
	}
}
