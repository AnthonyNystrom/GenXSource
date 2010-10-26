/* -----------------------------------------------
 * NuGenDialogBlock.cs
 * Copyright © 2006-2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.DialogInternals;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	/// <summary>
	/// Ok/Cancel/... block.
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenDialogBlock : NuGenControl
	{
		#region Properties.Services

		private INuGenDialogBlockLayoutManager _layoutManager;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenDialogBlockLayoutManager LayoutManager
		{
			get
			{
				if (_layoutManager == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_layoutManager = this.ServiceProvider.GetService<INuGenDialogBlockLayoutManager>();

					if (_layoutManager == null)
					{
						throw new NuGenServiceNotFoundException<INuGenDialogBlockLayoutManager>();
					}
				}

				return _layoutManager;
			}
		}

		private INuGenPanelRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenPanelRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenPanelRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenPanelRenderer>();
					}
				}

				return _renderer;
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
			NuGenPaintParams paintParams = new NuGenPaintParams(e.Graphics);
			paintParams.Bounds = this.ClientRectangle;
			paintParams.State = this.StateTracker.GetControlState();

			this.Renderer.DrawBackground(paintParams);
		}

		#endregion

		private NuGenBevel _bevel;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDialogBlock"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		///		<para><see cref="INuGenDialogBlockLayoutManager"/></para>
		///		<para><see cref="INuGenPanelRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenDialogBlock(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, false);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
			this.SetStyle(ControlStyles.UserPaint, true);

			_bevel = new NuGenBevel();
			_bevel.Dock = DockStyle.Top;
			_bevel.Height = 2;
			_bevel.TabIndex = 0;
			_bevel.TabStop = false;
			_bevel.Parent = this;

			this.Dock = DockStyle.Bottom;
			this.Size = this.LayoutManager.GetDefaultSize();
		}		
	}
}
