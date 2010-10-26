/* -----------------------------------------------
 * NuGenSplitButtonBase.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.SplitButtonInternals;
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
	/// </summary>
	[Designer("Genetibase.Shared.Controls.Design.NuGenSplitButtonDesigner")]
	public abstract class NuGenSplitButtonBase : NuGenButtonBase
	{
		/*
		 * ImageAlign
		 */

		/// <summary>
		/// Gets or sets the alignment of the image on the button control.
		/// </summary>
		/// <value></value>
		/// <returns>One of the <see cref="T:System.Drawing.ContentAlignment"></see> values. The default value is MiddleCenter.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Drawing.ContentAlignment"></see> values. </exception>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[DefaultValue(ContentAlignment.MiddleLeft)]
		public new ContentAlignment ImageAlign
		{
			get
			{
				return base.ImageAlign;
			}
			set
			{
				base.ImageAlign = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the control has input focus.
		/// </summary>
		/// <value></value>
		/// <returns>true if the control has focus; otherwise, false.</returns>
		public override bool Focused
		{
			get
			{
				return base.Focused || base.IsDefault;
			}
		}

		private static readonly Size _defaultSize = new Size(121, 24);

		/// <summary>
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return _defaultSize;
			}
		}

		private INuGenSplitButtonLayoutManager _layoutManager;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenSplitButtonLayoutManager LayoutManager
		{
			get
			{
				if (_layoutManager == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_layoutManager = this.ServiceProvider.GetService<INuGenSplitButtonLayoutManager>();

					if (_layoutManager == null)
					{
						throw new NuGenServiceNotFoundException<INuGenSplitButtonLayoutManager>();
					}
				}

				return _layoutManager;
			}
		}

		private INuGenSplitButtonRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenSplitButtonRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenSplitButtonRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenSplitButtonRenderer>();
					}
				}

				return _renderer;
			}
		}

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			Debug.Assert(this.Renderer != null, "this.Renderer != null");

			Graphics g = e.Graphics;
			NuGenControlState currentState = this.ButtonStateTracker.GetControlState();
			Rectangle bounds = this.ClientRectangle;
			Rectangle arrowBounds = this.LayoutManager.GetArrowRectangle(bounds, this.RightToLeft);
			Rectangle contentBounds = this.LayoutManager.GetContentRectangle(bounds, arrowBounds, this.RightToLeft);

			NuGenPaintParams paintParams = new NuGenPaintParams(g);
			paintParams.Bounds = bounds;
			paintParams.State = currentState;

			this.Renderer.DrawBackground(paintParams);
			this.Renderer.DrawShadow(paintParams);
			this.Renderer.DrawBorder(paintParams);

			/* Image */

			Image image = this.Image;
			Rectangle imageBounds = Rectangle.Empty;
			ContentAlignment imageAlign = this.ImageAlign;

			if (image != null)
			{
				NuGenImagePaintParams imagePaintParams = new NuGenImagePaintParams(paintParams);
				imagePaintParams.Bounds = imageBounds = this.LayoutManager.GetImageBounds(
					new NuGenBoundsParams(
						contentBounds
						, imageAlign
						, new Rectangle(Point.Empty, image.Size)
						, this.RightToLeft
					)
				);
				imagePaintParams.Image = image;
				this.Renderer.DrawImage(imagePaintParams);
			}

			/* Text */

			if (imageBounds != Rectangle.Empty)
			{
				imageBounds.Inflate(3, 3);
			}

			NuGenTextPaintParams textPaintParams = new NuGenTextPaintParams(paintParams);

			textPaintParams.Bounds = this.LayoutManager.GetTextBounds(
				new NuGenBoundsParams(contentBounds, imageAlign, imageBounds, this.RightToLeft)
			);
			textPaintParams.Font = this.Font;
			textPaintParams.ForeColor = this.ForeColor;
			textPaintParams.Text = this.Text;
			textPaintParams.TextAlign = NuGenControlPaint.RTLContentAlignment(this.TextAlign, this.RightToLeft);

			this.Renderer.DrawText(textPaintParams);

			/* Arrow */

			paintParams.Bounds = arrowBounds;
			this.Renderer.DrawArrow(paintParams);

			/* SplitLine */

			paintParams.Bounds = this.LayoutManager.GetSplitLineRectangle(bounds, arrowBounds, this.RightToLeft);
			this.Renderer.DrawSplitLine(paintParams);
		}

		/// <summary>
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateTracker"/></para>
		/// 	<para><see cref="INuGenSplitButtonLayoutManager"/></para>
		///		<para><see cref="INuGenSplitButtonRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		protected NuGenSplitButtonBase(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.UseVisualStyleBackColor = false;
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.ImageAlign = ContentAlignment.MiddleLeft;
		}
	}
}
