/* -----------------------------------------------
 * NuGenRibbonManager.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Environment;
using Genetibase.Shared.Windows;
using Genetibase.UI.NuGenInterface.ComponentModel;
using Genetibase.UI.NuGenInterface.Design;
using Genetibase.UI.NuGenInterface.Drawing;
using Genetibase.UI.NuGenInterface.Rendering;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Genetibase.UI.NuGenInterface.Properties;

namespace Genetibase.UI.NuGenInterface
{
	/// <summary>
	/// Imitates Microsoft Office 2007 style for the <see cref="Form"/> that hosts
	/// this <see cref="NuGenRibbonManager"/> and manages ribbon components
	/// look-and-feel.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenRibbonManagerDesigner))]
	[NuGenSRDescription("NuGenRibbonManagerDescription")]
	[ToolboxItem(true)]
	public partial class NuGenRibbonManager : Component
	{
		#region Declarations.Consts

		private const float OFFSET = 0.3f;

		#endregion

		#region Properties.Appearance

		/*
		 * ColorScheme
		 */

		private NuGenColorScheme _colorScheme = NuGenColorScheme.Blue;

		/// <summary>
		/// Gets or sets the <see cref="NuGenColorScheme"/> to use.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenColorScheme.Blue)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("ColorSchemeDescription")]
		public NuGenColorScheme ColorScheme
		{
			get
			{
				return _colorScheme;
			}
			set
			{
				if (_colorScheme != value)
				{
					_colorScheme = value;
					this.OnColorSchemeChanged(EventArgs.Empty);
					this.Renderer.ColorTable = new NuGenColorTable(_colorScheme);

					/* Do not use Debug.Assert here because ColorScheme property is initialized earlier
					 * than HostForm property during start-up and assertion will fail as HostForm has not
					 * been initialized yet and its current value is null. */

					if (this.HostForm != null)
					{
						this.HostForm.BackColor = this.Renderer.ColorTable.Form.BackColor;
						this.HostForm.Invalidate(true);
					}
				}
			}
		}

		private static readonly object _colorSchemeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ColorScheme"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ColorSchemeChangedDescription")]
		public event EventHandler ColorSchemeChanged
		{
			add
			{
				this.Events.AddHandler(_colorSchemeChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_colorSchemeChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="ColorSchemeChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnColorSchemeChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_colorSchemeChanged);
		}

		#endregion

		#region Properties.NonBrowsable

		/*
		 * HostForm
		 */

		private Form _hostForm = null;

		/// <summary>
		/// Gets or sets the <see cref="Form"/> that hosts this <see cref="NuGenRibbonManager"/>.
		/// </summary>
		[Browsable(false)]
		public Form HostForm
		{
			get
			{
				return _hostForm;
			}
			set
			{
				if (_hostForm != value)
				{
					this.ResetHostForm();
					_hostForm = value;
					this.InitializeHostForm();
					this.OnHostFormChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _HostFormChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:NuGenRibbonManager.HostForm"/> property changes.
		/// </summary>
		[Browsable(false)]
		public event EventHandler HostFormChanged
		{
			add
			{
				this.Events.AddHandler(_HostFormChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_HostFormChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="HostFormChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnHostFormChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[_HostFormChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/*
		 * IsFormActive
		 */

		private bool _IsFormActive = true;

		/// <summary>
		/// Gets the value indicating whether the
		/// <see cref="P:Genetibase.UI.NuGenRibbonManager.HostForm"/> is in the activated state.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsFormActive
		{
			get
			{
				return _IsFormActive;
			}
			protected set
			{
				if (_IsFormActive != value)
				{
					_IsFormActive = value;
					this.OnIsFormActiveChanged(EventArgs.Empty);

					Debug.Assert(this.HostForm != null, "this.HostForm != null");

					if (this.HostForm != null)
					{
						this.HostForm.Invalidate(true);
					}
				}
			}
		}

		private static readonly object _IsFormActiveChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:NuGenRibbonManager.IsFormActive"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("IsFormActiveChangedDescription")]
		public event EventHandler IsFormActiveChanged
		{
			add
			{
				this.Events.AddHandler(_IsFormActiveChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_IsFormActiveChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="IsFormActiveChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnIsFormActiveChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[_IsFormActiveChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/*
		 * Renderer
		 */

		private NuGenRendererBase _Renderer = null;

		/// <summary>
		/// Gets or sets the <see cref="NuGenRendererBase"/> that
		/// is used to render controls hosted by this
		/// <see cref="NuGenRibbonManager"/>.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public NuGenRendererBase Renderer
		{
			get
			{
				if (_Renderer == null)
				{
					_Renderer = new NuGenRenderer();
				}

				return _Renderer;
			}
			set
			{
				_Renderer = value;
				Debug.Assert(_Renderer != null, "_Renderer != null");
			}
		}

		#endregion

		#region Properties.Protected

		/*
		 * FormStateStore
		 */

		private INuGenFormStateStore _formStateStore = null;

		/// <summary>
		/// Read-only.
		/// </summary>
		protected INuGenFormStateStore FormStateStore
		{
			get
			{
				if (_formStateStore == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_formStateStore = this.ServiceProvider.GetService<INuGenFormStateStore>();

					if (_formStateStore == null)
					{
						throw new NuGenServiceNotFoundException(typeof(INuGenFormStateStore));
					}
				}

				return _formStateStore;
			}
		}

        /*
         * MessageFilter
         */

        private INuGenMessageFilter _messageFilter = null;

        /// <summary>
        /// Read-only.
        /// </summary>
        protected INuGenMessageFilter MessageFilter
        {
            get
            {
                if (_messageFilter == null)
                {
                    Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
                    _messageFilter = this.ServiceProvider.GetService<INuGenMessageFilter>();

                    if (_messageFilter == null)
                    {
                        throw new NuGenServiceNotFoundException<INuGenMessageFilter>();
                    }
                }

                return _messageFilter;
            }
        }

		/*
		 * MessageProcessor
		 */

		private INuGenMessageProcessor _messageProcessor = null;

		/// <summary>
		/// Read-only.
		/// </summary>
		protected INuGenMessageProcessor MessageProcessor
		{
			get
			{
				if (_messageProcessor == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_messageProcessor = this.ServiceProvider.GetService<INuGenMessageProcessor>();

					if (_messageProcessor == null)
					{
						throw new NuGenServiceNotFoundException<INuGenMessageProcessor>();
					}
				}

				return _messageProcessor;
			}
		}

		/*
		 * RibbonFormProperties
		 */

		private INuGenRibbonFormProperties _ribbonFormProperties = null;

		/// <summary>
		/// Read-only.
		/// </summary>
		protected INuGenRibbonFormProperties RibbonFormProperties
		{
			get
			{
				if (_ribbonFormProperties == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_ribbonFormProperties = this.ServiceProvider.GetService<INuGenRibbonFormProperties>();

					if (_ribbonFormProperties == null)
					{
						throw new NuGenServiceNotFoundException(typeof(INuGenRibbonFormProperties));
					}
				}

				return _ribbonFormProperties;
			}
		}

		#endregion

		#region Properties.Protected.Virtual

		/*
		 * Initiator
		 */

		private INuGenEventInitiatorService _initiator = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenEventInitiatorService Initiator
		{
			get
			{
				if (_initiator == null)
				{
					_initiator = new NuGenEventInitiatorService(this, this.Events);
				}

				return _initiator;
			}
		}

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		#endregion

		#region Methods.Protected.Virtual

		/*
		 * DrawFormBorders
		 */

		/// <summary>
		/// Draws borders for this <see cref="HostForm"/>.
		/// </summary>
		/// <param name="g">Specifies GDI+ drawing surface to draw on.</param>
		/// <exception cref="ArgumentNullException"><paramref name="g"/> is <see langword="null"/>.</exception>
		protected virtual void DrawFormBorders(Graphics g)
		{
			Debug.Assert(g != null, "g != null");

			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			Debug.Assert(this.HostForm != null, "this.HostForm != null");

			if (this.HostForm != null)
			{
				NuGenLinearGradientColorTable[] colorTables = this.GetBorderColors();
				RectangleF borderRectangle = new RectangleF(
					OFFSET,
					OFFSET,
					this.HostForm.Width - (1 + OFFSET),
					this.HostForm.Height - (1 + OFFSET)
					);
				int borderWidth = colorTables.Length;
				SmoothingMode previousSmoothingMode = g.SmoothingMode;

				g.SmoothingMode = SmoothingMode.HighQuality;

				int colorTableIndex = 0;

				for (int i = 0; i < borderWidth; i++)
				{
					NuGenLinearGradientColorTable colorTable = colorTables[colorTableIndex];

					using (GraphicsPath gp = this.GetFormPath(borderRectangle))
					{
						NuGenRibbonFormPainter.DrawBorders(
							g,
							gp,
							colorTable.StartColor,
							colorTable.EndColor,
							colorTable.GradientAngle,
							colorTables.Length
						);
					}

					borderRectangle.Inflate(-1, -1);

					if (colorTableIndex < (colorTables.Length - 1))
					{
						colorTableIndex++;
					}
				}

				g.SmoothingMode = previousSmoothingMode;
			}
		}

		/*
		 * DrawNonClientArea
		 */

		/// <summary>
		/// Draws the non client area for the <see cref="HostForm"/>.
		/// </summary>
		/// <param name="g">Specifies the GDI+ drawing surface.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="g"/> is <see langword="null"/>.</exception>
		protected virtual void DrawNonClientArea(Graphics g)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			this.DrawFormBorders(g);
		}

		/*
		 * GetBorderColors
		 */

		/// <summary>
		/// Retrieves an array of the <see cref="NuGenLinearGradientColorTable"/> instances that describe
		/// border colors. <see cref="NuGenLinearGradientColorTable"/> at zero index is used to draw the
		/// outer most border.
		/// </summary>
		protected virtual NuGenLinearGradientColorTable[] GetBorderColors()
		{
			Debug.Assert(this.Renderer != null, "this.Renderer != null");
			Debug.Assert(this.Renderer.ColorTable != null, "this.Renderer.ColorTable != null");
			Debug.Assert(this.Renderer.ColorTable.Form != null, "this.Renderer.ColorTable.Form != null");
			Debug.Assert(this.Renderer.ColorTable.Form.Active != null, "this.Renderer.ColorTable.Form.Active != null");
			Debug.Assert(this.Renderer.ColorTable.Form.Inactive != null, "this.Renderer.ColorTable.Form.Inactive != null");

			NuGenFormStateColorTable colorTable = this.IsFormActive
				? this.Renderer.ColorTable.Form.Active
				: this.Renderer.ColorTable.Form.Inactive
				;

			Debug.Assert(colorTable != null, "colorTable != null");

			return new NuGenLinearGradientColorTable[] {
				new NuGenLinearGradientColorTable(colorTable.OuterBorder, Color.Empty),
				new NuGenLinearGradientColorTable(colorTable.InnerBorder, Color.Empty)
			};
		}

		/*
		 * GetFormPath
		 */

		/// <summary>
		/// Builds a <see cref="T:System.Drawing.Drawing2D.GraphicsPath"/> according to the specified
		/// bounds.
		/// </summary>
		/// <param name="bounds">Specifies form bounds.</param>
		/// <returns></returns>
		protected virtual GraphicsPath GetFormPath(RectangleF bounds)
		{
			Debug.Assert(this.HostForm != null, "this.HostForm != null");

			if (this.HostForm != null && this.HostForm.WindowState == FormWindowState.Normal)
			{
				GraphicsPath gp = new GraphicsPath();
				bool rightToLeft = this.HostForm.RightToLeft == RightToLeft.Yes;

				/* Top-left corner */

				NuGenRibbonFormPainter.AddArcFromDescriptor(
					gp,
					NuGenRibbonFormPainter.BuildArcDescriptor(
						bounds,
						rightToLeft
							? this.RibbonFormProperties.TopRightCornerSize
							: this.RibbonFormProperties.TopLeftCornerSize
							,
						NuGenCornerStyle.TopLeft)
				);

				/* Top-right corner */

				NuGenRibbonFormPainter.AddArcFromDescriptor(
					gp,
					NuGenRibbonFormPainter.BuildArcDescriptor(
						bounds,
						rightToLeft
							? this.RibbonFormProperties.TopLeftCornerSize
							: this.RibbonFormProperties.TopRightCornerSize
							,
						NuGenCornerStyle.TopRight)
				);

				/* Bottom-right corner */

				NuGenRibbonFormPainter.AddArcFromDescriptor(
					gp,
					NuGenRibbonFormPainter.BuildArcDescriptor(
						bounds,
						this.RibbonFormProperties.BottomRightCornerSize,
						NuGenCornerStyle.BottomRight)
				);

				/* Bottom-left corner */

				NuGenRibbonFormPainter.AddArcFromDescriptor(
					gp,
					NuGenRibbonFormPainter.BuildArcDescriptor(
						bounds,
						this.RibbonFormProperties.BottomLeftCornerSize,
						NuGenCornerStyle.BottomLeft)
				);

				gp.CloseAllFigures();
				return gp;
			}
			else
			{
				GraphicsPath gp = new GraphicsPath();
				gp.AddRectangle(bounds);
				return gp;
			}
		}

		/*
		 * GetRegion
		 */

		/// <summary>
		/// Builds a <see cref="T:System.Drawing.Region"/> for the <see cref="P:NuGenRibbonManager.HostForm"/>.
		/// </summary>
		protected virtual Region GetRegion()
		{
			Debug.Assert(this.HostForm != null, "this.HostForm != null");

			if (this.HostForm != null && this.HostForm.WindowState == FormWindowState.Normal)
			{
				GraphicsPath gp = this.GetFormPath(
					new Rectangle(0, 0, this.HostForm.Width - 1, this.HostForm.Height - 1)
				);

				Region rgn = new Region();

				rgn.MakeEmpty();
				rgn.Union(gp);
				gp.Widen(SystemPens.Control);
				rgn.Union(gp);

				return rgn;
			}
			else
			{
				return null;
			}
		}

		/*
		 * InitializeHostForm
		 */

		/// <summary>
		/// Initializes the <see cref="HostForm"/>.
		/// </summary>
		/// <remarks>Note for inheritors. Make sure to check the <see cref="NuGenRibbonManager.HostForm"/>
		/// for <see langword="null"/>.</remarks>
		protected virtual void InitializeHostForm()
		{
			if (!this.DesignMode && this.HostForm != null)
			{
				Debug.Assert(this.FormStateStore != null, "this.FormStateStore != null");
				this.FormStateStore.StoreFormState(this.HostForm);
                this.MessageFilter.TargetControl = this.HostForm;

				NuGenWmHandlerList msgMap = this.MessageProcessor.MessageMap;

				foreach (int wmId in msgMap)
				{
					this.MessageFilter.MessageMap.AddWmHandler(wmId, msgMap[wmId]);
				}

				NuGenControlPaint.SetStyle(
					this.HostForm,
					ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint,
					true
				);

				this.HostForm.BackColor = this.Renderer.ColorTable.Form.BackColor;
				this.HostForm.Padding = new Padding(
					this.RibbonFormProperties.DisplayRectangleReductionHorizontal,
					this.RibbonFormProperties.DisplayRectangleReductionTop,
					this.RibbonFormProperties.DisplayRectangleReductionHorizontal,
					this.RibbonFormProperties.DisplayRectangleReductionBottom
				);

				this.HostForm.HandleCreated += this.HostForm_HandleCreated;
				this.HostForm.Paint += this.HostForm_Paint;
				this.HostForm.Resize += this.HostForm_Resize;
			}
		}

		/*
		 * ResetHostForm
		 */

		/// <summary>
		/// Resets the <see cref="HostForm"/>.
		/// </summary>
		/// <remarks>Note for inheritors. Make sure to check the <see cref="HostForm"/>
		/// for <see langword="null"/>.</remarks>
		protected virtual void ResetHostForm()
		{
			if (!this.DesignMode && this.HostForm != null)
			{
				this.MessageFilter.TargetControl = null;

				this.HostForm.HandleCreated -= this.HostForm_HandleCreated;
				this.HostForm.Paint -= this.HostForm_Paint;
				this.HostForm.Resize -= this.HostForm_Resize;
			}
		}

		#endregion

		#region EventHandlers

		private void HostForm_HandleCreated(object sender, EventArgs e)
		{
			if (sender is Form)
			{
				Form form = (Form)sender;

				if (NuGenOS.IsCompositionEnabled)
				{
					int pvAttribute = 0;

					DwmApi.DwmGetWindowAttribute(
						form.Handle,
						DwmApi.DWMWINDOWATTRIBUTE.DWMWA_NCRENDERING_ENABLED,
						ref pvAttribute,
						Marshal.SizeOf(pvAttribute)
					);

					if (pvAttribute != 0)
					{
						pvAttribute = 1;

						DwmApi.DwmSetWindowAttribute(
							form.Handle,
							DwmApi.DWMWINDOWATTRIBUTE.DWMWA_NCRENDERING_POLICY,
							ref pvAttribute,
							Marshal.SizeOf(pvAttribute)
						);

						DwmApi.DwmSetWindowAttribute(
							form.Handle,
							DwmApi.DWMWINDOWATTRIBUTE.DWMWA_TRANSITIONS_FORCEDISABLED,
							ref pvAttribute,
							Marshal.SizeOf(pvAttribute)
						);
					}
				}
			}
		}

		private void HostForm_Paint(object sender, PaintEventArgs e)
		{
			Debug.Assert(this.HostForm != null, "this.HostForm != null");

			if (this.HostForm != null)
			{
				this.HostForm.Region = this.GetRegion();
				this.DrawFormBorders(e.Graphics);
			}
		}

		private void HostForm_Resize(object sender, EventArgs e)
		{
			Debug.Assert(this.HostForm != null, "this.HostForm != null");

			if (this.HostForm != null)
			{
				this.HostForm.Invalidate();
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRibbonManager"/> class.
		/// </summary>
		/// 
		/// <param name="serviceProvider">
		/// Requires:
        /// <para><see cref="INuGenFormStateStore"/></para>
        /// <para><see cref="INuGenRibbonFormProperties"/></para>
        /// <para><see cref="INuGenMessageFilter"/></para>
		/// <para><see cref="INuGenMessageProcessor"/></para>
        /// </param>
		/// 
		/// <param name="hostForm">
		/// Specifies the <see cref="T:System.Windows.Forms.Form"/> that will host
		/// this <see cref="NuGenRibbonManager"/>. Can be <see langword="null"/>.
		/// </param>
		public NuGenRibbonManager(INuGenServiceProvider serviceProvider, Form hostForm)
			: this(serviceProvider)
		{
			this.HostForm = hostForm;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRibbonManager"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:
		/// <para><see cref="INuGenFormStateStore"/></para>
        /// <para><see cref="INuGenRibbonFormProperties"/></para>
        /// <para><see cref="INuGenMessageFilter"/></para>
		/// <para><see cref="INuGenMessageProcessor"/></para>
		/// </param>
		public NuGenRibbonManager(INuGenServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRibbonManager"/> class.
		/// </summary>
		/// <param name="hostForm">Specifies the <see cref="T:System.Windows.Forms.Form"/> that will host
		/// this <see cref="NuGenRibbonManager"/>. Can be <see langword="null"/>.</param>
		public NuGenRibbonManager(Form hostForm)
			: this()
		{
			this.HostForm = hostForm;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRibbonManager"/> class.
		/// </summary>
		public NuGenRibbonManager()
			: this(new NuGenRibbonServiceProvider())
		{
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"></see> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.ResetHostForm();
			}

			base.Dispose(disposing);
		}

		#endregion
	}
}
