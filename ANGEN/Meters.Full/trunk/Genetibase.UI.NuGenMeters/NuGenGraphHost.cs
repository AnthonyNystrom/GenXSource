/* -----------------------------------------------
 * NuGenGraphHost.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using win = Genetibase.WinApi.WinUser;

using Genetibase.Shared.Design;
using Genetibase.Shared;
using Genetibase.UI.NuGenMeters.Design;
using Genetibase.UI.NuGenMeters.ComponentModel;
using Genetibase.WinApi;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.UI.NuGenMeters
{
	/// <summary>
	/// Base control for graph hosts.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenGraphHostDesigner))]
	[ToolboxItem(false)]
	public class NuGenGraphHost : NuGenGenericBase
	{
		#region Declarations

		/// <summary>
		/// The incorporated <c>Genetibase.NuGenPushGraphBar</c> control.
		/// </summary>
		protected NuGenPushGraphBar pushGraph;

		#endregion

		#region Properties.Appearance

		/*
		 * ExtraPerformance
		 */

		/// <summary>
		/// Gets or sets the value indicating whether to use native GDI for rendering.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("ExtraPerformanceDescription")]
		public bool ExtraPerformance
		{
			get
			{
				Debug.Assert(this.pushGraph != null, "this.pushGraph != null");
				return this.pushGraph.ExtraPerformance;
			}
			set
			{
				Debug.Assert(this.pushGraph != null, "this.pushGrah != null");
				this.pushGraph.ExtraPerformance = value;
			}
		}

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGraphHost.ExtraPerformance"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ExtraPerformanceChangedDescription")]
		public event EventHandler ExtraPerformanceChanged
		{
			add
			{
				this.pushGraph.ExtraPerformanceChanged += value;
			}
			remove
			{
				this.pushGraph.ExtraPerformanceChanged -= value;
			}
		}

		/*
		 * ShowGrid
		 */

		/// <summary>
		/// Gets or sets the value indicating whether to show the grid.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("ShowGridDescription")]
		[DefaultValue(true)]
		public bool ShowGrid
		{
			get { return this.pushGraph.ShowGrid; }
			set 
			{
				this.pushGraph.ShowGrid = value;
				this.OnShowGridChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventShowGridChanged = new object();

		/// <summary>
		/// Occurs when the value of ShowGrid property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ShowGridChangedDescription")]
		public event EventHandler ShowGridChanged
		{
			add
			{
				this.Events.AddHandler(EventShowGridChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventShowGridChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGraphHost.ShowGridChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		public virtual void OnShowGridChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventShowGridChanged, e);
		}

		#endregion

		#region Properties.Behavior

		/*
		 * Step
		 */

		/// <summary>
		/// Gets or sets the line step for the graph.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BehaviorCategory")]
		[NuGenSRDescription("StepDescription")]
		[DefaultValue(5)]
		public virtual int Step
		{
			get { return this.pushGraph.Step; }
			set 
			{
				this.pushGraph.Step = value;
				this.OnStepChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventStepChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGraphHost.StepChanged"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("StepChangedDescription")]
		public event EventHandler StepChanged
		{
			add
			{
				this.Events.AddHandler(EventStepChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventStepChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGraphHost.StepChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnStepChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventStepChanged, e);
		}

		#endregion

		#region Properties.Grid

		/*
		 * GridColor
		 */

		/// <summary>
		/// Gets or sets the color for the grid.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("GridCategory")]
		[NuGenSRDescription("GridColorDescription")]
		[DefaultValue(typeof(Color), "Green")]
		public virtual Color GridColor
		{
			get { return this.pushGraph.GridColor; }
			set 
			{
				this.pushGraph.GridColor = value;
				this.OnGridColorChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventGridColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGraphHost.GridColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[Description("GridColorChangedDescription")]
		public event EventHandler GridColorChanged
		{
			add
			{
				this.Events.AddHandler(EventGridColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventGridColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGraphHost.GridColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnGridColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventGridColorChanged, e);
		}

		/*
		 * GridStep
		 */

		/// <summary>
		/// Gets or sets the step for the grid.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("GridCategory")]
		[NuGenSRDescription("GridStepDescription")]
		[DefaultValue(15)]
		public virtual int GridStep
		{
			get { return this.pushGraph.GridStep; }
			set 
			{
				this.pushGraph.GridStep = value;
				this.OnGridStepChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventGridStepChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGraphHost.GridStep"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("GridStepChangedDescription")]
		public event EventHandler GridStepChanged
		{
			add
			{
				this.Events.AddHandler(EventGridStepChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventGridStepChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGraphHost.GridStepChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnGridStepChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventGridStepChanged, e);
		}

		/*
		 * GridTransparency
		 */

		/// <summary>
		/// Gets or sets the grid transparency level.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("GridCategory")]
		[NuGenSRDescription("GridTransparencyDescription")]
		[DefaultValue(0)]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual int GridTransparency
		{
			get { return this.pushGraph.GridTransparency; }
			set 
			{
				this.pushGraph.GridTransparency = value;
				this.OnGridTransparencyChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventGridTransparencyChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGraphHost.GridTransparency"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedDescription")]
		[NuGenSRDescription("GridTransparencyChangedDescription")]
		public event EventHandler GridTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(EventGridTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventGridTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGraphHost.GridTransparencyChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnGridTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventGridTransparencyChanged, e);
		}

		#endregion

		#region Properties.Protected.Overriden

		/// <summary>
		/// Gets the <c>Genetibase.UI.NuGenBarBase</c> object with the specified parameters set.
		/// </summary>
		protected override NuGenBarBase Bar
		{
			get { return this.pushGraph; }
		}

		/// <summary>
		/// Gets or sets the background color for the meter.
		/// </summary>
		[DefaultValue(typeof(Color), "Black")]
		public override Color BackgroundColor
		{
			get { return base.BackgroundColor; }
			set { base.BackgroundColor = value; }
		}

		/// <summary>
		/// Gets or sets the background style for the control.
		/// </summary>
		[DefaultValue(NuGenBackgroundStyle.Constant)]
		public override NuGenBackgroundStyle BackgroundStyle
		{
			get { return base.BackgroundStyle; }
			set { base.BackgroundStyle = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instanceof the <c>Genetibase.UI.NuGenGraphHost</c> class.
		/// </summary>
		public NuGenGraphHost()
		{
			InitializeComponent();

			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			
			this.BackColor = Color.Transparent;
		}

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pushGraph = new NuGenPushGraphBar();
			this.SuspendLayout();
			// 
			// pushGraph
			// 
			this.pushGraph.BackGradientEndColor = System.Drawing.Color.Coral;
			this.pushGraph.BackGradientStartColor = System.Drawing.Color.Yellow;
			this.pushGraph.BackTubeGradientEndColor = System.Drawing.Color.Coral;
			this.pushGraph.BackTubeGradientStartColor = System.Drawing.Color.Yellow;
			this.pushGraph.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pushGraph.Location = new System.Drawing.Point(0, 0);
			this.pushGraph.Name = "pushGraph";
			this.pushGraph.Size = new System.Drawing.Size(250, 100);
			this.pushGraph.TabIndex = 0;
			// 
			// NuGenGraphHost
			// 
			this.Controls.Add(this.pushGraph);
			this.Name = "NuGenGraphHost";
			this.Size = new System.Drawing.Size(250, 100);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
