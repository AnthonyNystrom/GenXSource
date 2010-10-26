/* -----------------------------------------------
 * NuGenPanelEx.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.PanelExInternals;
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
	[ToolboxItem(false)]
	[DefaultEvent("Paint")]
	[DefaultProperty("Dock")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenPanelExDesigner")]
	[NuGenSRDescription("Description_PanelEx")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenPanelEx : NuGenControl
	{
		#region Properties.Appearance

		private bool _drawShadow = true;

		/// <summary>
		/// Gets or sets the value indicating whether to draw the shadow.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_PanelEx_DrawShadow")]
		public bool DrawShadow
		{
			get
			{
				return _drawShadow;
			}
			set
			{
				if (_drawShadow != value)
				{
					_drawShadow = value;
					this.OnDrawShadowChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _drawShadowChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="DrawShadow"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PanelEx_DrawShadowChanged")]
		public event EventHandler DrawShadowChanged
		{
			add
			{
				this.Events.AddHandler(_drawShadowChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_drawShadowChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="DrawShadowChanged"/> event.
		/// </summary>
		protected virtual void OnDrawShadowChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_drawShadowChanged, e);
		}

		#endregion

		#region Properties.Public.Overridden

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[DefaultValue(typeof(Color), "Transparent")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		/// <summary>
		/// Gets the rectangle that represents the virtual display area of the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Rectangle"></see> that represents the display area of the control.</returns>
		public override Rectangle DisplayRectangle
		{
			get
			{
				return this.Renderer.GetDisplayRectangle(this.ClientRectangle, this.DrawShadow);
			}
		}

		#endregion

		#region Properties.Services

		private INuGenPanelExRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenPanelExRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenPanelExRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenPanelExRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			NuGenPaintParams paintParams = new NuGenPaintParams(e.Graphics);
			paintParams.Bounds = this.ClientRectangle;
			paintParams.State = this.StateTracker.GetControlState();

			if (this.DrawShadow)
			{
				this.Renderer.DrawShadow(paintParams);
			}

			paintParams.Bounds = this.DisplayRectangle;
			this.Renderer.DrawBackground(paintParams);
			this.Renderer.DrawBorder(paintParams);

			base.OnPaint(e);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPanelEx"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		///		<para><see cref="INuGenPanelExRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenPanelEx(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, false);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, false);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.BackColor = Color.Transparent;
		}
	}
}
