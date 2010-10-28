using System;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Collections;
using System.Diagnostics;

using Genetibase.MathX.NugenCCalc.Adapters.IChartAdapter;
using Genetibase.MathX.Core;
using Genetibase.MathX.Core.Plotters;


namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>
	/// 	<para>The NugenCCalcComponent component is the primary object controlling a chart
	/// displayed and 3D function evaluation.</para>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NugenCCalc3D), "ico.chart.ico")]
	[Designer(typeof(Design.NugenCCalc3DDesigner), typeof(IDesigner))]	
	[Serializable()]
    [DesignerSerializer(typeof(NugenCCalcSerializer), typeof(CodeDomSerializer))]
	public class NugenCCalc3D : NugenCCalcBase
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>Initializes a new instance of the NugenCCalcComponent3D class</summary>
		public NugenCCalc3D(System.ComponentModel.IContainer container) : this()
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			container.Add(this);
		}

		/// <summary>Initializes a new instance of the NugenCCalcComponent3D class</summary>
		public NugenCCalc3D()
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			_functionParameters = new Explicit3DParameters();
			InitializeComponent();
			InitComponent();
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

		}
		#endregion

		#region Private Component Fields

		

		#endregion

		#region Public Nonbrowsable Component Properties

		/// <summary>
		/// Get chart adapter for current chart control
		/// </summary>
		[Browsable(false)]
		new public IChartAdapter3D CurrentAdapter
		{
			get
			{
				return (IChartAdapter3D)_currentAdapter;
			}
		}

		#endregion

		#region Public Browsable Component Properties

		/// <summary>Get or set function parameters</summary>
		[Description("Source for component")]
		[Category("Source properties")]
		[TypeConverter(typeof(Design.Converters.FunctionsParametersConverter))]
        [RefreshProperties(RefreshProperties.All)]
		new public Function3DParameters FunctionParameters
		{
			get
			{
				return (Function3DParameters)_functionParameters;
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

		#region Private Component Methods
		

		/// <summary>
		/// Init all component data
		/// </summary>
		private void InitComponent()
		{
			Assembly[] assemblies = AdaptersLoader.LoadAdapters(supportedCharts);
			_chartAdapters = AdaptersLoader.GetCustomAdapters(assemblies, typeof(IChartAdapter3D));
			this.ComponentDataChanged +=new EventHandler(NugenCCalcComponent_ComponentDataChanged);
		}

		protected override void BindChartData()
		{
			
		}



		#endregion

		#region Public Component Methods

		/// <summary>
		/// Plot with exist source and current chart control
		/// </summary>
		/// <example>
		/// 	<code lang="CS" description="The following example create new function and plot it on chart">
		/// NugenCCalcComponent3D nugenCCalcComponent1 = new NugenCCalcComponent3D(); 
		/// nugenCCalcComponent1.FunctionParameters = new Explicit3DParameters("x*x+y"); 
		/// nugenCCalcComponent1.ChartControl = c1Chart3D; 
		/// nugenCCalcComponent1.Plot();
		/// </code>
		/// </example>
		public override void Plot()
		{
			Plot(this._chartControl, (Function3DParameters)_functionParameters);
		}


		/// <summary>
		/// Plot with exist source and new destination chart
		/// </summary>
		/// <example>
		/// 	<code lang="CS" description="The following example create new function and plot it on given chart">
		/// NugenCCalcComponent3D nugenCCalcComponent1 = new NugenCCalcComponent3D();  
		/// nugenCCalcComponent1.FunctionParameters = new Explicit3DParameters("x*x+y");  
		/// nugenCCalcComponent1.Plot(c1Chart3D);
		/// </code>
		/// </example>
		/// <param name="chartControl">Destination chart</param>
		public void Plot(object chartControl)
		{
			Plot(chartControl, (Function3DParameters)_functionParameters);
		}


		/// <summary>
		/// Plot with new source and new destination chart
		/// </summary>
		/// <example>
		/// 	<code lang="CS" description="The following example plot given function on given chart">
		/// NugenCCalcComponent3D nugenCCalcComponent1 = new NugenCCalcComponent3D();  
		/// nugenCCalcComponent1.Plot(c1Chart3D, new Explicit3DParameters("x*x+y"));
		/// </code>
		/// </example>
		public void Plot(object chartControl, Function3DParameters functionParameters)
		{
			if (DesignMode)
				return;

			OnBeforePlot();
			if (chartControl == null)
				throw new ArgumentNullException("Chart control is null");


			if (functionParameters == null)
				throw new ArgumentNullException("functionParameters");

			if (functionParameters.Code == "")
			{
				throw new ArgumentNullException("Source Code");
			}

			IChartAdapter3D adapter = null;
			if (chartControl != _chartControl)
			{
				adapter = (IChartAdapter3D)GetChartAdapter(chartControl);
				adapter.SetChartControl(chartControl);
			}
			else
				adapter = (IChartAdapter3D)_currentAdapter;

			if (adapter == null)
				throw new Exception("Chart not supported");


			Function function = functionParameters.Function;
			
			if (function is Explicit3DFunction)
			{
				PlotExplicit3DFunction(adapter, (Explicit3DParameters)functionParameters);
			}

			if (function is Implicit3DFunction)
			{
				PlotImplicit3DFunction(adapter, (Implicit3DParameters)functionParameters);
			}

			if (function is Parameter3DFunction)
			{
				PlotParametric3DFunction(adapter, (Parametric3DParameters)functionParameters);
			}

			if (function is ParametricSurface)
			{
				PlotParametricSurface(adapter, (ParametricSurfaceParameters)functionParameters);
			}

			OnAfterPlot();
		}

	#endregion

		#region Private Instance Methods(Plot)
		private void PlotExplicit3DFunction(IChartAdapter3D adapter, Explicit3DParameters functionParameters)
		{
			Explicit3DFunctionPlotter plotter = 
				new Explicit3DFunctionPlotter((Explicit3DFunction)functionParameters.Function);
			double[,] result = null;
			result = plotter.Plot(functionParameters.PointA, functionParameters.PointB, functionParameters.AreaSize);

			adapter.PlotSurface(functionParameters.PointA, functionParameters.PointB,result);
		}

		private void PlotImplicit3DFunction(IChartAdapter3D adapter, Implicit3DParameters functionParameters)
		{
			Implicit3DFunctionPlotter plotter = 
				new Implicit3DFunctionPlotter((Implicit3DFunction)functionParameters.Function);
			plotter.GridFactor = functionParameters.GridFactor;

			Point3D[] result = null;
			result = plotter.Plot(functionParameters.Point3DA, functionParameters.Point3DB, functionParameters.PixelSizeX,functionParameters.PixelSizeY, functionParameters.PixelSizeZ);
			adapter.PlotPoints(result);
		}

		private void PlotParametric3DFunction(IChartAdapter3D adapter, Parametric3DParameters functionParameters)
		{
			Parameter3DFunctionPlotter plotter = 
				new Parameter3DFunctionPlotter((Parameter3DFunction)functionParameters.Function);
			Point3D[] result = null;
			result = plotter.Plot(functionParameters.Min, functionParameters.Max, functionParameters.NumPoints);
			adapter.PlotPoints(result);
		}

		private void PlotParametricSurface(IChartAdapter3D adapter, ParametricSurfaceParameters functionParameters)
		{
			ParametricSurfacePlotter plotter = 
				new ParametricSurfacePlotter((ParametricSurface)functionParameters.Function);
			Point3D[] result = null;
			result = plotter.Plot(functionParameters.MinU, functionParameters.MaxU, functionParameters.MinV, functionParameters.MaxV, functionParameters.NumPoints);
			adapter.PlotPoints(result);
		}
        

		#endregion
	}
}
