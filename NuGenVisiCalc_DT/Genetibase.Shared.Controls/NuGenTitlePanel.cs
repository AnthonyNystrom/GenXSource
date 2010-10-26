/* -----------------------------------------------
 * NuGenTitlePanel.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.TitleInternals;
using Genetibase.Shared.Controls.TitlePanelInternals;
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
	[DefaultProperty("Text")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenTitlePanelDesigner")]
	[NuGenSRDescription("Description_TitlePanel")]
	public class NuGenTitlePanel : NuGenControl
	{
		#region Properties.Appearance

		/*
		 * DrawBorder
		 */

		private bool _drawBorder = true;

		/// <summary>
		/// Gets or sets the value indicating whether to draw the border for the panel.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Panel_DrawBorder")]
		public virtual bool DrawBorder
		{
			get
			{
				return _drawBorder;
			}
			set
			{
				if (_drawBorder != value)
				{
					_drawBorder = value;
					this.OnDrawBorderChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _drawBorderChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="DrawBorder"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Panel_DrawBorderChanged")]
		public event EventHandler DrawBorderChanged
		{
			add
			{
				this.Events.AddHandler(_drawBorderChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_drawBorderChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="DrawBorderChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDrawBorderChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_drawBorderChanged, e);
		}

		#endregion

		#region Properties.Public.Overridden

		/// <summary>
		/// Gets the rectangle that represents the virtual display area of the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Rectangle"></see> that represents the display area of the control.</returns>
		public override Rectangle DisplayRectangle
		{
			get
			{
				return Rectangle.FromLTRB(
					this.ClientRectangle.Left
					, this.ClientRectangle.Top + this.TitleHeight
					, this.ClientRectangle.Right
					, this.ClientRectangle.Bottom
				);
			}
		}

		/// <summary>
		/// </summary>
		/// <value></value>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				this.Invalidate();
			}
		}

		#endregion

		#region Properties.Private

		private int TitleHeight
		{
			get
			{
				return this.LayoutManager.GetTitleHeight();
			}
		}

		#endregion

		#region Properties.Services

		private INuGenTitlePanelLayoutManager _layoutManager;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenTitlePanelLayoutManager LayoutManager
		{
			get
			{
				if (_layoutManager == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_layoutManager = this.ServiceProvider.GetService<INuGenTitlePanelLayoutManager>();

					if (_layoutManager == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTitlePanelLayoutManager>();
					}
				}

				return _layoutManager;
			}
		}

		private INuGenPanelRenderer _panelRenderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenPanelRenderer PanelRenderer
		{
			get
			{
				if (_panelRenderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_panelRenderer = this.ServiceProvider.GetService<INuGenPanelRenderer>();

					if (_panelRenderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenPanelRenderer>();
					}
				}

				return _panelRenderer;
			}
		}

		private INuGenTitleRenderer _titleRenderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenTitleRenderer TitleRenderer
		{
			get
			{
				if (_titleRenderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_titleRenderer = this.ServiceProvider.GetService<INuGenTitleRenderer>();

					if (_titleRenderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTitleRenderer>();
					}
				}

				return _titleRenderer;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			NuGenItemPaintParams titlePaintParams = new NuGenItemPaintParams(e.Graphics);
			titlePaintParams.Bounds = new Rectangle(
				this.ClientRectangle.Left
				, this.ClientRectangle.Top
				, this.ClientRectangle.Width
				, this.TitleHeight
			);

			titlePaintParams.ContentAlign = this.RightToLeft == RightToLeft.Yes
				? ContentAlignment.MiddleRight
				: ContentAlignment.MiddleLeft
				;
			titlePaintParams.Font = this.Font;
			titlePaintParams.ForeColor = this.ForeColor;
			titlePaintParams.State = this.StateTracker.GetControlState();
			titlePaintParams.Text = this.Text;

			NuGenPaintParams bkgndPaintParams = new NuGenPaintParams(titlePaintParams);
			bkgndPaintParams.Bounds = this.DisplayRectangle;

			NuGenBorderPaintParams borderPaintParams = new NuGenBorderPaintParams(bkgndPaintParams);
			borderPaintParams.Bounds = this.ClientRectangle;
			borderPaintParams.DrawBorder = this.DrawBorder;

			this.PanelRenderer.DrawBackground(bkgndPaintParams);
			this.PanelRenderer.DrawBorder(borderPaintParams);

			this.TitleRenderer.DrawBackground(titlePaintParams);
			this.TitleRenderer.DrawBody(titlePaintParams);
			this.TitleRenderer.DrawBorder(titlePaintParams);

			base.OnPaint(e);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTitlePanel"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		///		<para><see cref="INuGenPanelRenderer"/></para>
		///		<para><see cref="INuGenTitleRenderer"/></para>
		///		<para><see cref="INuGenTitlePanelLayoutManager"/></para>
		///		<para><see cref="INuGenControlImageManager"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenTitlePanel(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, false);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.TabStop = false;
		}
	}
}
