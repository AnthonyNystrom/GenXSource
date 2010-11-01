/* -----------------------------------------------
 * NuGenButton.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.Design;
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
	/// <seealso cref="Button"/>
	/// </summary>
	[Designer(typeof(NuGenButtonDesigner))]
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(false)]
	public class NuGenButton : NuGenButtonBase
	{
		#region Properties.Public.New

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

		#endregion

		#region Properties.Public.Overridden

		/*
		 * Focused
		 */

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

		#endregion

		#region Properties.Services

		/*
		 * LayoutManager
		 */

		private INuGenButtonLayoutManager _layoutManager;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenButtonLayoutManager LayoutManager
		{
			get
			{
				if (_layoutManager == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_layoutManager = this.ServiceProvider.GetService<INuGenButtonLayoutManager>();

					if (_layoutManager == null)
					{
						throw new NuGenServiceNotFoundException<INuGenButtonLayoutManager>();
					}
				}

				return _layoutManager;
			}
		}

		/*
		 * Renderer
		 */

		private INuGenButtonRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenButtonRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenButtonRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenButtonRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

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
			Rectangle contentBounds = this.LayoutManager.GetContentRectangle(this.ClientRectangle);

			NuGenPaintParams paintParams = new NuGenPaintParams(this, g, bounds, currentState);

			this.Renderer.DrawBackground(paintParams);
			this.Renderer.DrawShadow(paintParams);
			this.Renderer.DrawBorder(paintParams);

			Image image = this.Image;
			Rectangle imageBounds = Rectangle.Empty;
			ContentAlignment imageAlign = this.ImageAlign;

			if (image != null)
			{
				NuGenImagePaintParams imagePaintParams = new NuGenImagePaintParams(
					this,
					g,
					imageBounds = this.LayoutManager.GetImageBounds(new NuGenImageBoundsParams(contentBounds, image, imageAlign)),
					currentState,
					image
				);

				this.Renderer.DrawImage(imagePaintParams);
			}

			NuGenTextPaintParams textPaintParams = new NuGenTextPaintParams(
				this,
				g,
				this.LayoutManager.GetTextBounds(new NuGenTextBoundsParams(contentBounds, imageBounds, imageAlign)),
				currentState,
				this.Text
			);
			textPaintParams.Font = this.Font;
			textPaintParams.ForeColor = this.ForeColor;
			textPaintParams.TextAlign = this.TextAlign;

			this.Renderer.DrawText(textPaintParams);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenButton"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenButtonStateTracker"/></para>
		/// <para><see cref="INuGenButtonLayoutManager"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenButton(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.UseVisualStyleBackColor = false;

			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Opaque, false);

			this.ImageAlign = ContentAlignment.MiddleLeft;
		}

		#endregion
	}
}
