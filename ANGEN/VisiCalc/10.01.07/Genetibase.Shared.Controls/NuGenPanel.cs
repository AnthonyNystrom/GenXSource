/* -----------------------------------------------
 * NuGenPanel.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.PanelInternals;
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
	/// <seealso cref="Panel"/>
	/// </summary>
	[ToolboxItem(false)]
	[DefaultEvent("Paint")]
	[DefaultProperty("Dock")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenPanelDesigner")]
	[NuGenSRDescription("Description_Panel")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenPanel : NuGenControl
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

		/*
		 * ExtendedBackground
		 */

		private bool _extendedBackground;

		/// <summary>
		/// Indicates whether to draw the extended or standard background.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Panel_ExtendedBackground")]
		public bool ExtendedBackground
		{
			get
			{
				return _extendedBackground;
			}
			set
			{
				if (_extendedBackground != value)
				{
					_extendedBackground = value;
					this.OnExtendedBackgroundChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _extendedBackgroundChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ExtendedBackground"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Panel_ExtendedBackgroundChanged")]
		public event EventHandler ExtendedBackgroundChanged
		{
			add
			{
				this.Events.AddHandler(_extendedBackgroundChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_extendedBackgroundChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ExtendedBackgroundChanged"/> event.
		/// </summary>
		protected virtual void OnExtendedBackgroundChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_extendedBackgroundChanged, e);
		}

		#endregion

		#region Properties.Public.Overridden

		/*
		 * BackColor
		 */

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

		/*
		 * TabStop
		 */

		/// <summary>
		/// Gets or sets a value indicating whether the user can give the focus to this control using the TAB key.
		/// </summary>
		/// <value></value>
		/// <returns>true if the user can give the focus to the control using the TAB key; otherwise, false. The default is true.This property will always return true for an instance of the <see cref="T:System.Windows.Forms.Form"></see> class.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[DefaultValue(false)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		#endregion

		#region Properties.Hidden

		/*
		 * BackgroundImage
		 */

		/// <summary>
		/// Do not use this property. Any value will not affect the appearance.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/*
		 * BackgroundImageLayout
		 */

		/// <summary>
		/// Do not use this property. Any value will not affect the appearance.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		/*
		 * BorderStyle
		 */

		/// <summary>
		/// Do not use this property. Any value will not affect the appearance.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new BorderStyle BorderStyle
		{
			get
			{
				return BorderStyle.None;
			}
			set
			{
				base.BorderStyle = value;
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * Renderer
		 */

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

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");

			NuGenBorderPaintParams paintParams = new NuGenBorderPaintParams(e.Graphics);
			paintParams.Bounds = this.ClientRectangle;
			paintParams.DrawBorder = this.DrawBorder;
			paintParams.State = this.StateTracker.GetControlState();

			if (this.ExtendedBackground)
			{
				this.Renderer.DrawExtendedBackground(paintParams);
			}
			else
			{
				this.Renderer.DrawBackground(paintParams);
			}

			this.Renderer.DrawBorder(paintParams);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPanel"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenControlStateTracker"/><para/>
		/// <see cref="INuGenPanelRenderer"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenPanel(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.BackColor = Color.Transparent;
			this.TabStop = false;

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, false);
			this.SetStyle(ControlStyles.UserPaint, true);
		}
	}
}
