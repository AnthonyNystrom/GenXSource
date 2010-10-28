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

using Genetibase.MathX.NugenCCalc.Adapters;
using Genetibase.MathX.NugenCCalc.Adapters.IChartAdapter;
using Genetibase.MathX.Core;
using Genetibase.MathX.Core.Plotters;

namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>
    /// 	<para>The NugenCCalcComponent2D component is the primary object controlling a chart
	/// displayed and 2D function evaluation.</para>
	/// </summary>
	/// <example>
	/// 	<code lang="CS" description="The following example create new function and plot it on chart">
    /// NugenCCalcComponent2D nugenCCalcComponent1 = new NugenCCalcComponent2D(); 
	/// nugenCCalcComponent1.FunctionParameters = new Explicit2DParameters("x*x"); 
	/// nugenCCalcComponent1.ChartControl = c1Chart; 
	/// nugenCCalcComponent1.Plot();
	/// </code>
	/// </example>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NugenCCalc2D), "ico.line-chart.ico")]
	[Designer(typeof(Design.NugenCCalcDesigner), typeof(IDesigner))]
	[Serializable()]
    [DesignerSerializer(typeof(NugenCCalcSerializer), typeof(CodeDomSerializer))]
	public class NugenCCalc2D : NugenCCalcBase
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


        /// <summary>Initializes a new instance of the NugenCCalcComponent2D class</summary>
		public NugenCCalc2D	(System.ComponentModel.IContainer container) : this()
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			container.Add(this);
		}

		/// <summary>Initializes a new instance of the NugenCCalcComponent2D class</summary>
		public NugenCCalc2D()
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			_functionParameters = new Explicit2DParameters();
			InitializeComponent();
			InitComponent();
		}


		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
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


		private Series _series = new Series("newSeries", Color.Red);


		#endregion

		#region Public Nonbrowsable Component Properties

		/// <summary>
		/// Get chart adapter for current chart control
		/// </summary>
		/// <example>
		/// 	<code lang="CS" description="The following example show how to set axes for current chart">
		/// NugenCCalcComponent2D nugenCCalcComponent1 = new NugenCCalcComponent2D(); 
		/// nugenCCalcComponent1.FunctionParameters = new Explicit2DParameters("x*x"); 
		/// nugenCCalcComponent1.ChartControl = c1Chart; 
		/// nugenCCalcComponent1.CurrentAdapter.SetAxes(0,5,0,10);
		/// </code>
		/// </example>
		[Browsable(false)]
		new public IChartAdapter2D CurrentAdapter
		{
			get
			{
				return (IChartAdapter2D)_currentAdapter;
			}
		}

		#endregion

		#region Public Browsable Component Properties

//		[Description("Plot in design mode")]
//		[Category("Destination properties")]
//		[DefaultValue(false)]
//		[DesignOnly(true)]
//		public bool PlotInDesignMode
//		{
//			get
//			{
//				return _plotInDesignMode;
//			}
//			set
//			{
//				if (_plotInDesignMode != value)
//				{
//					_plotInDesignMode = value;
//					NotifyDataChanged();
//				}
//			}
//		}
		/// <summary>Get or set current series on chart</summary>
		/// <value>Series</value>
		/// <example>
		/// 	<code lang="CS" description="The following example set new series for NugenCCalcComponent2D">
		/// NugenCCalcComponent2D ccalc = new NugenCCalcComponent2D();
		/// ccalc.Series = new Series("newSeries", Color.Red);
		/// </code>
		/// </example>
		[TypeConverter(typeof(ExpandableObjectConverter))] 
		[Editor(typeof(Design.Editors.SeriesEditor), typeof(UITypeEditor))]
		[Description("Source for component")]
		[Category("Source properties")]
		public Series Series
		{
			get
			{
				return _series;
			}
			set
			{
				_series = value;
			}
		}



		//[TypeConverter(typeof(ExpandableObjectConverter))] 
		/// <summary>Get or set FunctionParameters</summary>
		[Description("Source for component")]
		[Category("Source properties")]
		[TypeConverter(typeof(Design.Converters.FunctionsParametersConverter))]
        [RefreshProperties(RefreshProperties.All)]
		new public Function2DParameters FunctionParameters
		{
			get
			{
				return (Function2DParameters)_functionParameters;
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
			//System.Windows.Forms.MessageBox.Show("assemblies" + assemblies.Length.ToString());
			_chartAdapters = AdaptersLoader.GetCustomAdapters(assemblies, typeof(IChartAdapter2D));
			//System.Windows.Forms.MessageBox.Show("_chartAdapters" + _chartAdapters.Length.ToString());
			this.ComponentDataChanged +=new EventHandler(NugenCCalcComponent_ComponentDataChanged);
		}


		/// <summary>
		/// Get chart data
		/// </summary>
		protected override void BindChartData()
		{
			if (_currentAdapter != null)
			{
				if (FunctionParameters is Implicit2DParameters)
				{
					((Implicit2DParameters)FunctionParameters).PointA = new Point2D(((IChartAdapter2D)_currentAdapter).MinX, ((IChartAdapter2D)_currentAdapter).MinY);
					((Implicit2DParameters)FunctionParameters).PointB = new Point2D(((IChartAdapter2D)_currentAdapter).MaxX, ((IChartAdapter2D)_currentAdapter).MaxY);
					((Implicit2DParameters)FunctionParameters).AreaSize = ((IChartAdapter2D)_currentAdapter).PlotAreaSize;
				}
				else
				{
					((Explicit2DParameters)FunctionParameters).Min = ((IChartAdapter2D)_currentAdapter).MinX;
					((Explicit2DParameters)FunctionParameters).Max = ((IChartAdapter2D)_currentAdapter).MaxX;			
				}
			}
		}

		#endregion

		#region Public Component Methods

		/// <summary>
		/// Plot with exist source and current chart control
		/// </summary>
		/// <exception cref="System.ArgumentNullException" caption="ArgumentNullException">Chart control or FunctionParameters cannot be null</exception>
		/// <exception cref="System.ArgumentException" caption="ArgumentException">Code or Formula of source cannot be empty</exception>
		/// <exception cref="System.NotSupportedException" caption="NotSupportedException">Chart not supported</exception>
		/// <example>
		/// 	<code lang="CS" description="The following example create new function and plot it on chart">
		/// NugenCCalcComponent2D nugenCCalcComponent1 = new NugenCCalcComponent2D();
		/// nugenCCalcComponent1.FunctionParameters = new Explicit2DParameters("x*x");
		/// nugenCCalcComponent1.ChartControl = c1Chart;
		/// nugenCCalcComponent1.Plot();
		/// </code>
		/// </example>
		public override void Plot()
		{
			Plot(this._chartControl, (Function2DParameters)_functionParameters);
		}


		/// <summary>
		/// Plot with exist source and new destination chart
		/// </summary>
		/// <exception cref="System.ArgumentNullException" caption="ArgumentNullException">Chart control or FunctionParameters cannot be null</exception>
		/// <exception cref="System.ArgumentException" caption="ArgumentException">Code or Formula of source cannot be empty</exception>
		/// <exception cref="System.NotSupportedException" caption="NotSupportedException">Chart not supported</exception>
		/// <example>
		/// 	<code lang="CS" description="The following example create new function and plot it on given chart">
		/// NugenCCalcComponent2D nugenCCalcComponent1 = new NugenCCalcComponent2D(); 
		/// nugenCCalcComponent1.FunctionParameters = new Explicit2DParameters("x*x"); 
		/// nugenCCalcComponent1.Plot(c1Chart);
		/// </code>
		/// </example>
		public void Plot(object chartControl)
		{
			Plot(chartControl, (Function2DParameters)_functionParameters);
		}


		/// <summary>
		/// Plot with new source and new destination chart
		/// </summary>
		/// <exception cref="System.ArgumentNullException" caption="ArgumentNullException">Chart control or FunctionParameters cannot be null</exception>
		/// <exception cref="System.ArgumentException" caption="ArgumentException">Code or Formula of source cannot be empty</exception>
		/// <exception cref="System.NotSupportedException" caption="NotSupportedException">Chart not supported</exception>
		/// <example>
		/// 	<code lang="CS" description="The following example plot given function on given chart">
		/// NugenCCalcComponent2D nugenCCalcComponent1 = new NugenCCalcComponent2D();  
		/// nugenCCalcComponent1.Plot(c1Chart, new Explicit2DParameters("x*x"));
		/// </code>
		/// </example>
		public void Plot(object chartControl, Function2DParameters functionParameters)
		{
			if (DesignMode)
				return;

			OnBeforePlot();

			if (chartControl == null)
				throw new ArgumentNullException("ChartControl");

			if (functionParameters == null)
				throw new ArgumentNullException("functionParameters");

			if (functionParameters.Code == "")
			{
				throw new ArgumentException("Code or Formula of source cannot be empty");
			}


			IChartAdapter2D adapter = null;
			if (chartControl != _chartControl)
			{
				adapter = (IChartAdapter2D)GetChartAdapter(chartControl);
				adapter.SetChartControl(chartControl);
			}
			else
				adapter = (IChartAdapter2D)_currentAdapter;

			if (adapter == null)
				throw new NotSupportedException("Chart not supported");


			Function function = functionParameters.Function;

			if (function is Constant)
			{
				if (functionParameters is Explicit2DParameters)
					PlotConstantFunction(adapter, (Explicit2DParameters)functionParameters);

				if (functionParameters is Implicit2DParameters)
					PlotConstantFunction(adapter, (Implicit2DParameters)functionParameters);
			}

			if (function is Explicit2DFunction)
			{
				if (functionParameters is Explicit2DParameters)
					PlotExplicitFunction(adapter, (Explicit2DParameters)functionParameters);

				if (functionParameters is Implicit2DParameters)
					PlotExplicitFunction(adapter, (Implicit2DParameters)functionParameters);
			}

			if (function is Implicit2DFunction)
			{
				PlotImplicit2DFunction(adapter, (Implicit2DParameters)functionParameters);
			}

			if (function is Parameter2DFunction)
			{
				PlotParametric2DFunction(adapter, (Parametric2DParameters)functionParameters);
			}
			OnAfterPlot();
		}

		#endregion

		#region Private Plot Methods

		private void PlotExplicitFunction(IChartAdapter2D adapter, Explicit2DParameters explicitParameters)
		{
			Explicit2DFunctionPlotter plotter = 
				new Explicit2DFunctionPlotter((Explicit2DFunction)explicitParameters.Function);

			double[] result = null;

			switch(explicitParameters.PlotMode)
			{
				case PlotMode.ByNumPoints:
					result = plotter.Plot(explicitParameters.Min, explicitParameters.Max, explicitParameters.NumPoints);
					break;
                //case PlotMode.ByClientArea:
                //    throw new NotSupportedException("This type of PlotMode not supported");
				case PlotMode.ByStep:
					result = plotter.Plot(explicitParameters.Min, explicitParameters.Max, explicitParameters.Step);
					break;
			}

			if (!_automaticMode)
			{
				double[] tempArray = (double[])result.Clone();
				Array.Sort(tempArray);
				double min = tempArray[0];
				Array.Reverse(tempArray);
				double max = tempArray[0];
				adapter.SetAxes(explicitParameters.Min, explicitParameters.Max, min, max);
			}

			if (explicitParameters.IsPolar)
			{
				adapter.PlotPolar(result, 0);
			}
			else
			{
				adapter.Plot(result, _series);
			}
		}


		private void PlotExplicitFunction(IChartAdapter2D adapter, Implicit2DParameters implicitParameters)
		{
			Explicit2DFunctionPlotter plotter = 
				new Explicit2DFunctionPlotter((Explicit2DFunction)implicitParameters.Function);

			double[] result = null;

			plotter.Plot(implicitParameters.PointA, implicitParameters.PointB, implicitParameters.AreaSize);

			adapter.Plot(result, _series);
		}


		private void PlotParametric2DFunction(IChartAdapter2D adapter, Parametric2DParameters parametricParameters)
		{
			Parameter2DFunctionPlotter plotter = 
				new Parameter2DFunctionPlotter((Parameter2DFunction)parametricParameters.Function);
			Point2D[] result = null;
			switch(parametricParameters.PlotMode)
			{
				case PlotMode.ByNumPoints:
					result = plotter.Plot(parametricParameters.Min, parametricParameters.Max, parametricParameters.NumPoints);
					break;
                //case PlotMode.ByClientArea:
                //    throw new NotSupportedException("This type of PlotMode not supported");
				case PlotMode.ByStep:
					result = plotter.Plot(parametricParameters.Min, parametricParameters.Max, parametricParameters.Step);
					break;
			}

			if (!_automaticMode)
			{
				if (adapter.MinY != 0 && adapter.MaxY != 0)
					adapter.SetAxes(parametricParameters.Min, parametricParameters.Max, adapter.MinY, adapter.MaxY);
			}

			adapter.Plot(result, _series);

		}

		private void PlotConstantFunction(IChartAdapter2D adapter, Implicit2DParameters implicitParameters)
		{
			ConstantFunctionPlotter plotter = 
				new ConstantFunctionPlotter((Constant)implicitParameters.Function);
			double[] result = null;
			//result = plotter.Plot()

			adapter.Plot(result, _series);

		}


		private void PlotConstantFunction(IChartAdapter2D adapter, Explicit2DParameters explicitParameters)
		{
			ConstantFunctionPlotter plotter = 
				new ConstantFunctionPlotter((Constant)explicitParameters.Function);
			double[] result = null;
			switch(explicitParameters.PlotMode)
			{
				case PlotMode.ByNumPoints:
					result = plotter.Plot(explicitParameters.Min, explicitParameters.Max, explicitParameters.NumPoints);
					break;
                //case PlotMode.ByClientArea:
                //    throw new NotSupportedException("This type of PlotMode not supported");
				case PlotMode.ByStep:
					result = plotter.Plot(explicitParameters.Min, explicitParameters.Max, explicitParameters.Step);
					break;
			}

			if (!_automaticMode)
			{
				double[] tempArray = (double[])result.Clone();
				Array.Sort(tempArray);
				double min = tempArray[0];
				Array.Reverse(tempArray);
				double max = tempArray[0];
				adapter.SetAxes(explicitParameters.Min, explicitParameters.Max, min, max);
			}

			adapter.Plot(result, _series);

		}


		private void PlotImplicit2DFunction(IChartAdapter2D adapter, Implicit2DParameters implicitParameters)
		{
			Implicit2DFunctionPlotter plotter = 
				new Implicit2DFunctionPlotter((Implicit2DFunction)implicitParameters.Function);

			plotter.GridFactor = implicitParameters.GridFactor;
			Point2D[] resultPoints = plotter.Plot(
				implicitParameters.PointA, 
				implicitParameters.PointB, 
				implicitParameters.AreaSize);

			adapter.SetAxes(implicitParameters.PointA.X, 
				implicitParameters.PointB.X, 
				implicitParameters.PointA.Y, 
				implicitParameters.PointB.Y);

			adapter.Plot(resultPoints, _series);
		}
		#endregion
	}
}
