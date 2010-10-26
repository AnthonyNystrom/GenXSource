/* -----------------------------------------------
 * NuGenToolTip.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ToolTipInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenToolTip
	{
		private sealed class ToolTipControl : NuGenLayeredWindow
		{
			#region Properties.Services

			/*
			 * LayoutManager
			 */

			private INuGenToolTipLayoutManager _layoutManager;

			private INuGenToolTipLayoutManager LayoutManager
			{
				get
				{
					if (_layoutManager == null)
					{
						Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
						_layoutManager = this.ServiceProvider.GetService<INuGenToolTipLayoutManager>();

						if (_layoutManager == null)
						{
							throw new NuGenServiceNotFoundException<INuGenToolTipLayoutManager>();
						}
					}

					return _layoutManager;
				}
			}

			/*
			 * Renderer
			 */

			private INuGenToolTipRenderer _renderer;

			private INuGenToolTipRenderer Renderer
			{
				get
				{
					if (_renderer == null)
					{
						Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
						_renderer = this.ServiceProvider.GetService<INuGenToolTipRenderer>();

						if (_renderer == null)
						{
							throw new NuGenServiceNotFoundException<INuGenToolTipRenderer>();
						}
					}

					return _renderer;
				}
			}

			/*
			 * ServiceProvider
			 */

			private INuGenServiceProvider _serviceProvider;

			private INuGenServiceProvider ServiceProvider
			{
				get
				{
					return _serviceProvider;
				}
			}

			#endregion

			#region Methods.Public

			/// <summary>
			/// </summary>
			/// <param name="tooltipInfo"></param>
			/// <param name="location"></param>
			/// <exception cref="ArgumentNullException">
			/// <paramref name="tooltipInfo"/> is <see langword="null"/>.
			/// </exception>
			[SecurityPermission(SecurityAction.LinkDemand)]
			public void Show(NuGenToolTipInfo tooltipInfo, Point location)
			{
				if (tooltipInfo == null)
				{
					throw new ArgumentNullException("tooltipInfo");
				}

				_tooltipInfo = tooltipInfo;

				using (Graphics g = Graphics.FromHwnd(this.Handle))
				{
					this.LayoutManager.SetMinimumTooltipSize(this.MinimumSize);
					_layoutDescriptor = this.LayoutManager.BuildLayoutDescriptor(g, _tooltipInfo, _headerFont, _textFont);
				}

				this.Size = _layoutDescriptor.TooltipSize;
				base.Show(location);
			}

			#endregion

			#region Methods.Protected.Overridden

			protected override void OnFontChanged(EventArgs e)
			{
				base.OnFontChanged(e);

				_textFont = new Font(this.Font, FontStyle.Regular);
				_headerFont = new Font(this.Font, FontStyle.Bold);
			}

			/*
			 * OnPaint
			 */

			protected override void OnPaint(PaintEventArgs e)
			{
				Graphics g = e.Graphics;

				Rectangle bounds = this.ClientRectangle;
				Size shadowSize = this.LayoutManager.GetShadowSize();
				Rectangle shadowBounds = new Rectangle(
					bounds.Left + shadowSize.Width,
					bounds.Top + shadowSize.Height,
					bounds.Width - shadowSize.Width,
					bounds.Height - shadowSize.Height
				);
				Rectangle bodyBounds = new Rectangle(
					bounds.Left,
					bounds.Top,
					bounds.Width - shadowSize.Width,
					bounds.Height - shadowSize.Height
				);

				NuGenPaintParams paintParams = new NuGenPaintParams(g);
				paintParams.State = _state;

				paintParams.Bounds = shadowBounds;
				this.Renderer.DrawShadow(paintParams);

				paintParams.Bounds = bodyBounds;
				this.Renderer.DrawBackground(paintParams);
				this.Renderer.DrawBorder(paintParams);

				if (_tooltipInfo != null)
				{
					NuGenTextPaintParams textPaintParams = new NuGenTextPaintParams(paintParams);
					textPaintParams.TextAlign = ContentAlignment.MiddleLeft;
					textPaintParams.ForeColor = this.ForeColor;

					NuGenImagePaintParams imagePaintParams = new NuGenImagePaintParams(paintParams);

					if (_tooltipInfo.IsHeaderVisible)
					{
						textPaintParams.Bounds = _layoutDescriptor.HeaderBounds;
						textPaintParams.Font = _headerFont;
						textPaintParams.Text = _tooltipInfo.Header;

						this.Renderer.DrawHeaderText(textPaintParams);
					}

					if (_tooltipInfo.IsTextVisible)
					{
						textPaintParams.Bounds = _layoutDescriptor.TextBounds;
						textPaintParams.Font = _textFont;
						textPaintParams.Text = _tooltipInfo.Text;

						this.Renderer.DrawText(textPaintParams);
					}

					if (_tooltipInfo.IsImageVisible)
					{
						imagePaintParams.Bounds = _layoutDescriptor.ImageBounds;
						imagePaintParams.Image = _tooltipInfo.Image;

						this.Renderer.DrawImage(imagePaintParams);
					}

					if (_tooltipInfo.IsRemarksHeaderVisible)
					{
						textPaintParams.Bounds = _layoutDescriptor.RemarksHeaderBounds;
						textPaintParams.Font = _headerFont;
						textPaintParams.Text = _tooltipInfo.RemarksHeader;

						this.Renderer.DrawHeaderText(textPaintParams);

						if (_tooltipInfo.IsRemarksVisible)
						{
							textPaintParams.Bounds = _layoutDescriptor.RemarksBounds;
							textPaintParams.Font = _textFont;
							textPaintParams.Text = _tooltipInfo.Remarks;

							this.Renderer.DrawText(textPaintParams);
						}

						if (_tooltipInfo.IsRemarksImageVisible)
						{
							imagePaintParams.Bounds = _layoutDescriptor.RemarksImageBounds;
							imagePaintParams.Image = _tooltipInfo.RemarksImage;

							this.Renderer.DrawImage(imagePaintParams);
						}

						paintParams.Bounds = _layoutDescriptor.BevelBounds;
						this.Renderer.DrawBevel(paintParams);
					}
				}
			}

			#endregion

			private NuGenToolTipInfo _tooltipInfo;
			private NuGenToolTipLayoutDescriptor _layoutDescriptor;
			private Font _headerFont;
			private Font _textFont;
			private static readonly NuGenControlState _state = NuGenControlState.Normal;

			/// <summary>
			/// Initializes a new instance of the <see cref="ToolTipControl"/> class.
			/// </summary>
			/// <param name="serviceProvider">
			/// <para>Requires:</para>
			/// <para><see cref="INuGenToolTipRenderer"/></para>
			/// </param>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
			/// </exception>
			[SecurityPermission(SecurityAction.LinkDemand)]
			public ToolTipControl(INuGenServiceProvider serviceProvider)
			{
				if (serviceProvider == null)
				{
					throw new ArgumentNullException("serviceProvider");
				}

				_serviceProvider = serviceProvider;

				_headerFont = new Font(this.Font, FontStyle.Bold);
				_textFont = new Font(this.Font, FontStyle.Regular);
			}

			private bool _disposed;

			/// <summary>
			/// Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.Form"></see>.
			/// </summary>
			/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					if (!_disposed)
					{
						_headerFont.Dispose();
						_textFont.Dispose();

						_disposed = true;
					}
				}

				base.Dispose(disposing);
			}
		}
	}
}
