using System;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Diagnostics;

using Genetibase.MathX.NugenCCalc.Adapters;
using Genetibase.MathX.NugenCCalc.Adapters.IChartAdapter;
using Genetibase.MathX.Core;
using Genetibase.MathX.Core.Plotters;

namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>
	/// Provides the <strong>abstract</strong> base class for the NugenCCalc
	/// components.
	/// </summary>
	[ToolboxItem(false)]
	[Serializable()]
    public abstract class NugenCCalcBase : System.ComponentModel.Component, IAdapterContainer
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>Initializes a new instance of the NugenCCalcBase class</summary>
		public NugenCCalcBase(System.ComponentModel.IContainer container)
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			container.Add(this);
			InitializeComponent();
		}

		/// <summary>Initializes a new instance of the NugenCCalcBase class</summary>
		public NugenCCalcBase()
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			InitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		#region Private Component Fields

		protected string[] supportedCharts = new string[]{"Genetibase.MathX.NugenCCalc.Adapters, Culture=neutral, PublicKeyToken=7d1178b36f4c8251"};

		protected IChartAdapter[] _chartAdapters;
		protected IChartAdapter _currentAdapter = null;

		protected TimeSpan _lastEventTime = DateTime.Now.TimeOfDay;
		protected int _eventDelay = 500;
		protected bool _automaticMode = false;
		protected object _chartControl;

        protected object _owner;

		protected FunctionParameters _functionParameters = null;

		#endregion

		#region Private Component Methods
		
		/// <summary>
		/// Subscribe on source event(changed)
		/// </summary>
		protected void BindSourceEvents()
		{
			if (_functionParameters != null)
				_functionParameters.Changed+=new ParametersChangeHandler(_functionParameters_Changed);
		}


		/// <summary>
		/// Unsubscribe on source event(changed)
		/// </summary>
		protected void UnBindSourceEvents()
		{
			if (_functionParameters != null)
				_functionParameters.Changed -=new ParametersChangeHandler(_functionParameters_Changed);
		}


		/// <summary>
		/// Subscribe on seft event Changed
		/// </summary>
		/// <param name="item"></param>
		/// <param name="e"></param>
		private void _functionParameters_Changed(FunctionParameters parameters, EventArgs e)
		{
			NotifyDataChanged();
		}


		/// <summary>
		/// Init Chart adapter by chart type
		/// </summary>
		protected void InitChartAdapter()
		{

			_currentAdapter = GetChartAdapter(_chartControl);

			if (_currentAdapter == null)
				throw new ArgumentException("The chart is not supported");
			_currentAdapter.SetChartControl(_chartControl);
		}


		/// <summary>
		/// Subscribe on chart events(scale and paint)
		/// </summary>
		protected void BindChartEvents()
		{
			if (_currentAdapter != null)
			{
				_currentAdapter.ScopeChanged +=new EventHandler(Chart_Changed);
				_currentAdapter.SizeChanged +=new EventHandler(Chart_Changed);
			}
		}


		/// <summary>
		/// Unsubscribe on chart events(scale and paint)
		/// </summary>
		protected void UnBindChartEvents()
		{
			if (_currentAdapter != null)
			{
				_currentAdapter.ScopeChanged -=new EventHandler(Chart_Changed);
				_currentAdapter.SizeChanged -=new EventHandler(Chart_Changed);
			}
		}

		/// <summary>
		/// Get chart data
		/// </summary>
		protected abstract void BindChartData();

		/// <summary>
		/// Replot if chart parameters changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Chart_Changed(object sender, EventArgs e)
		{
			if (_automaticMode)
			{
				try
				{
					UnBindSourceEvents();
					BindChartData();
					BindSourceEvents();
					TimeSpan difTime = DateTime.Now.TimeOfDay - _lastEventTime;
					if (difTime > new TimeSpan(0,0,0,0,_eventDelay))
					{
						Plot();
						_lastEventTime = DateTime.Now.TimeOfDay;
					}
				}
				catch{}
			}
		}


		/// <summary>
		/// Replot if some component data changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void NugenCCalcComponent_ComponentDataChanged(object sender, EventArgs e)
		{
			if (_chartControl == null)
				return;

			try
			{
				if (_automaticMode)
				{
					BindChartData();
					Plot();
				}
			}
			catch{}
		}

		#endregion

		#region Component Events
		/// <summary>
		/// Triggers the AfterPlot event.
		/// </summary>
		protected void OnAfterPlot()
		{
			if (AfterPlot != null)
				AfterPlot(this, new EventArgs());
		}

		/// <summary>
		/// Triggers the BeforePlot event.
		/// </summary>
		protected void OnBeforePlot()
		{
			if (BeforePlot != null)
				BeforePlot(this, new EventArgs());
		}
		/// <summary>
		/// Triggers the ComponentDataChanged event.
		/// </summary>
		protected void NotifyDataChanged()
		{
			if (ComponentDataChanged != null)
				ComponentDataChanged(this, new EventArgs());
		}

		/// <summary>
		/// Occurs when component data changed
		/// </summary>
		public event EventHandler ComponentDataChanged;
		/// <summary>
		/// Occurs before plot
		/// </summary>
		public event EventHandler BeforePlot;
		/// <summary>
		/// Occurs after plot
		/// </summary>
		public event EventHandler AfterPlot;

		#endregion

		#region Public Nonbrowsable Component Properties

		/// <summary>
		/// Get chart adapter for current chart control
		/// </summary>
		/// <value>IChartAdapter</value>
		[Browsable(false)]
		public IChartAdapter CurrentAdapter
		{
			get
			{
				return _currentAdapter;
			}
		}

		#endregion

		#region Public Browsable Component Properties

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Owner
		{
			get
			{
				return this._owner;
			}
			set
			{
				if (this._owner != value)
				{
					if (value == null)
						throw new ArgumentNullException("owner");
					this._owner = value;
				}
			}
		}

		/// <summary>Get or set current chart control</summary>
		/// <example>
		/// 	<code lang="CS" description="The following example show how to set chart control for component">
		/// NugenCCalcComponent nugenCCalcComponent1 = new NugenCCalcComponent(); 
		/// nugenCCalcComponent1.ChartControl = c1Chart;
		/// </code>
		/// </example>
		[Editor(typeof(Design.Editors.DestinationChartEditor), typeof(UITypeEditor))]
		[Description("Destination chart control")]
		[Category("Destination properties")]
		public object ChartControl
		{
			get
			{
				return this._chartControl;
			}
			set
			{
				if (this._chartControl != value)
				{

					if (value == null)
						throw new ArgumentNullException("ChartControl");

					this._chartControl = value;

					InitChartAdapter();
                    
					NotifyDataChanged();
				}
			}
		}


		/// <summary>Get or set automatic mode.</summary>
		[Description("Set automatic mode")]
		[Category("Destination properties")]
		[DefaultValue(false)]
		public bool AutomaticMode
		{
			get
			{
				return _automaticMode;
			}
			set
			{
				if (_automaticMode != value)
				{
					_automaticMode = value;
					if (_automaticMode)
					{
						UnBindSourceEvents();
						BindChartData();
						BindSourceEvents();
						BindChartEvents();
					}
					else
						UnBindChartEvents();
					NotifyDataChanged();
				}
			}
		}


		/// <summary>Delay between chart events.</summary>
		[Description("Set active delay between scale or paint events from chart (in milliseconds)")] 
		[Category("Destination properties")]
		[DefaultValue(500)]
		public int EventDelay
		{
			get
			{
				return _eventDelay;
			}
			set
			{
				if (_eventDelay != value)
				{
					if (value < 0)
						throw new ArgumentException("Event delay cannot be less than zero");
					_eventDelay = value;				
				}
			}
		}


		/// <summary>Get or set function parameters</summary>
		/// <example>
		/// 	<code lang="CS" description="The following example show how to set function parameters for component">
		/// NugenCCalcComponent nugenCCalcComponent1 = new NugenCCalcComponent(); 
		/// nugenCCalcComponent1.FunctionParameters = new Explicit2DFunction("x*x");
		/// </code>
		/// </example>
		[TypeConverter(typeof(Design.Converters.FunctionsParametersConverter))]		
		[Description("Source for component")]
		[Category("Source properties")]
		[RefreshProperties(RefreshProperties.All)]
		public FunctionParameters FunctionParameters
		{
			get
			{
				return _functionParameters;
			}
			set
			{
				if (_functionParameters != value)
				{
					if (!DesignMode)
						UnBindSourceEvents();
					_functionParameters = value;
					if (!DesignMode)
						BindSourceEvents();
					NotifyDataChanged();
				}
			}
		}

		#endregion

		#region Public Component Methods

		/// <summary>
		/// Get <font size="1"><strong>IChartAdapter</strong></font> interface for given
		/// chart.
		/// </summary>
		/// <returns>IChartAdapter interface</returns>
		/// <param name="chartControl">Chart control.</param>
		public virtual IChartAdapter GetChartAdapter(object chartControl)
		{
			foreach(IChartAdapter adapter in _chartAdapters)
			{
				if (adapter.Validate(chartControl))
					return adapter;
			}
			return null;
		}


		/// <summary>Plot with existing source and current chart control</summary>
		public abstract void Plot();


		/// <returns>true if chart supported.</returns>
		/// <summary>Check if chart supported.</summary>
		/// <example>
		/// 	<code lang="CS" description="The following example check if chart supported">
		/// NugenCCalcComponent nugenCCalcComponent1 = new NugenCCalcComponent();
		/// nugenCCalcComponent1.FunctionParameters = new Explicit2DFunction("x*x");
		/// bool isSupported = nugenCCalcComponent1.ValidateControl(control);
		/// if (isSupported)
		/// {
		///     nugenCCalcComponent1.Plot(control);
		/// }
		/// </code>
		/// </example>
		/// <param name="control">Control</param>
		public bool ValidateControl(object control)
		{
			foreach(IChartAdapter adapter in _chartAdapters)
			{
				if (adapter.Validate(control))
					return true;
			}
			return false;
		}


		/// <summary>Resume binding all chart events</summary>
		public void ResumeChartBinding(bool replot)
		{
			BindChartEvents();
			BindSourceEvents();
			try
			{
				if (replot)
					Plot();
			}
			catch{}
		}


		/// <summary>Suspend binding all chart events</summary>
		public void SuspendChartBinding()
		{
			UnBindChartEvents();
			UnBindSourceEvents();
		}

		#endregion
	}
}
