/* -----------------------------------------------
 * NuGenGraphHost.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Design;
using Genetibase.Shared;
using Genetibase.Meters.Design;
using Genetibase.Meters.ComponentModel;
using Genetibase.WinApi;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Meters
{
	/// <summary>
	/// Base control for graph hosts.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenGraphHostDesigner))]
	[ToolboxItem(false)]
	public class NuGenGraphHost : NuGenGenericBase
	{
		#region Properties.Appearance

		/*
		 * ExtraPerformance
		 */

		/// <summary>
		/// Gets or sets the value indicating whether to use native GDI for rendering.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ExtraPerformance")]
		public Boolean ExtraPerformance
		{
			get
			{
				Debug.Assert(_PushGraph != null, "this.pushGraph != null");
				return _PushGraph.ExtraPerformance;
			}
			set
			{
				Debug.Assert(_PushGraph != null, "this.pushGrah != null");
				_PushGraph.ExtraPerformance = value;
			}
		}

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGraphHost.ExtraPerformance"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ExtraPerformanceChanged")]
		public event EventHandler ExtraPerformanceChanged
		{
			add
			{
				_PushGraph.ExtraPerformanceChanged += value;
			}
			remove
			{
				_PushGraph.ExtraPerformanceChanged -= value;
			}
		}

		/*
		 * ShowGrid
		 */

		/// <summary>
		/// Gets or sets the value indicating whether to show the grid.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ShowGrid")]
		[DefaultValue(true)]
		public Boolean ShowGrid
		{
			get
			{
				return _PushGraph.ShowGrid;
			}
			set
			{
				_PushGraph.ShowGrid = value;
				this.OnShowGridChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _showGridChanged = new Object();

		/// <summary>
		/// Occurs when the value of ShowGrid property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ShowGridChanged")]
		public event EventHandler ShowGridChanged
		{
			add
			{
				this.Events.AddHandler(_showGridChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_showGridChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGraphHost.ShowGridChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		public virtual void OnShowGridChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_showGridChanged, e);
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
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_Step")]
		[DefaultValue(5)]
		public virtual Int32 Step
		{
			get
			{
				return _PushGraph.Step;
			}
			set
			{
				_PushGraph.Step = value;
				this.OnStepChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _stepChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGraphHost.StepChanged"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_StepChanged")]
		public event EventHandler StepChanged
		{
			add
			{
				this.Events.AddHandler(_stepChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_stepChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGraphHost.StepChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnStepChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_stepChanged, e);
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
		[NuGenSRCategory("Category_Grid")]
		[NuGenSRDescription("Description_GridColor")]
		[DefaultValue(typeof(Color), "Green")]
		public virtual Color GridColor
		{
			get
			{
				return _PushGraph.GridColor;
			}
			set
			{
				_PushGraph.GridColor = value;
				this.OnGridColorChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _gridColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGraphHost.GridColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[Description("Description_GridColorChanged")]
		public event EventHandler GridColorChanged
		{
			add
			{
				this.Events.AddHandler(_gridColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_gridColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGraphHost.GridColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnGridColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_gridColorChanged, e);
		}

		/*
		 * GridStep
		 */

		/// <summary>
		/// Gets or sets the step for the grid.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Grid")]
		[NuGenSRDescription("Description_GridStep")]
		[DefaultValue(15)]
		public virtual Int32 GridStep
		{
			get
			{
				return _PushGraph.GridStep;
			}
			set
			{
				_PushGraph.GridStep = value;
				this.OnGridStepChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _gridStepChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGraphHost.GridStep"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_GridStepChanged")]
		public event EventHandler GridStepChanged
		{
			add
			{
				this.Events.AddHandler(_gridStepChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_gridStepChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGraphHost.GridStepChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnGridStepChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_gridStepChanged, e);
		}

		/*
		 * GridTransparency
		 */

		/// <summary>
		/// Gets or sets the grid transparency level.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Grid")]
		[NuGenSRDescription("Description_GridTransparency")]
		[DefaultValue(0)]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual Int32 GridTransparency
		{
			get
			{
				return _PushGraph.GridTransparency;
			}
			set
			{
				_PushGraph.GridTransparency = value;
				this.OnGridTransparencyChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _gridTransparencyChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGraphHost.GridTransparency"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_GridTransparencyChanged")]
		public event EventHandler GridTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(_gridTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_gridTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGraphHost.GridTransparencyChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnGridTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_gridTransparencyChanged, e);
		}

		#endregion

		#region Properties.Protected.Overriden

		/// <summary>
		/// Gets the <c>Genetibase.UI.NuGenBarBase</c> Object with the specified parameters set.
		/// </summary>
		protected override NuGenBarBase Bar
		{
			get
			{
				return _PushGraph;
			}
		}

		/// <summary>
		/// Gets or sets the background color for the meter.
		/// </summary>
		[DefaultValue(typeof(Color), "Black")]
		public override Color BackgroundColor
		{
			get
			{
				return base.BackgroundColor;
			}
			set
			{
				base.BackgroundColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the background style for the control.
		/// </summary>
		[DefaultValue(NuGenBackgroundStyle.Constant)]
		public override NuGenBackgroundStyle BackgroundStyle
		{
			get
			{
				return base.BackgroundStyle;
			}
			set
			{
				base.BackgroundStyle = value;
			}
		}

		#endregion

		/// <summary>
		/// The incorporated <see cref="Genetibase.Meters.NuGenPushGraphBar"/> control.
		/// </summary>
		protected NuGenPushGraphBar _PushGraph;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenGraphHost"/> class.
		/// </summary>
		public NuGenGraphHost()
		{
			InitializeComponent();

			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			this.BackColor = Color.Transparent;
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			_PushGraph = new NuGenPushGraphBar();
			this.SuspendLayout();
			// 
			// pushGraph
			// 
			_PushGraph.BackGradientEndColor = System.Drawing.Color.Coral;
			_PushGraph.BackGradientStartColor = System.Drawing.Color.Yellow;
			_PushGraph.BackTubeGradientEndColor = System.Drawing.Color.Coral;
			_PushGraph.BackTubeGradientStartColor = System.Drawing.Color.Yellow;
			_PushGraph.Dock = System.Windows.Forms.DockStyle.Fill;
			_PushGraph.Location = new System.Drawing.Point(0, 0);
			_PushGraph.Name = "pushGraph";
			_PushGraph.Size = new System.Drawing.Size(250, 100);
			_PushGraph.TabIndex = 0;
			// 
			// NuGenGraphHost
			// 
			this.Controls.Add(_PushGraph);
			this.Name = "NuGenGraphHost";
			this.Size = new System.Drawing.Size(250, 100);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
