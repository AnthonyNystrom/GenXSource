using System;
using System.ComponentModel;
using Genetibase.MathX.Core;

namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>Store settings of Explicit 2D function</summary>
	[Serializable()]
    [FunctionCodeAttribute("double Calculate(double x)\n{", "}", "Function Calculate(x As Double) as Double", "End Function")]
	public class Explicit2DParameters : Function2DParameters
	{
		#region Private Instance Fields

		private PlotMode _plotMode = PlotMode.ByNumPoints;
		private double _min = -10;
		private double _max = 10;
		private double _step = 0.1;
		private int _numPoints = 200;
		private bool _isPolar = false;
		private bool _isInvert = false;

		#endregion

		#region Public Contructors

		/// <summary>Initializes a new instance of the Explicit2DParameters class.</summary>
		public Explicit2DParameters()
		{

		}

		/// <summary>Initializes a new instance of the Explicit2DParameters class on the specified formula.</summary>
		public Explicit2DParameters(string formula)
		{
			_formula = formula;
			InitCode();
		}

		/// <summary>
		/// Initializes a new instance of the Explicit2DParameters class on the specified
		/// formula, plot mode, min and max value, step and number of points.
		/// </summary>
		public Explicit2DParameters(string formula, PlotMode plotMode, double min, double max, double step, int numPoints)
		{
			_formula = formula;
			_plotMode = plotMode;
			_min = min;
			_max = max;
			_step = step;
			_numPoints = numPoints;
			InitCode();
		}

		/// <summary>Initializes a new instance of the Explicit2DParameters class.</summary>
		public Explicit2DParameters(string formula, PlotMode plotMode, double min, double max, double step, int numPoints, bool isPolar)
		{
			_formula = formula;
			_plotMode = plotMode;
			_min = min;
			_max = max;
			_step = step;
			_numPoints = numPoints;
			_isPolar = isPolar;
			InitCode();
		}

		/// <summary>
		/// Initializes a new instance of the Explicit2DParameters class on the specified
		/// code, code language, plot mode, min and max value, step and number of points.
		/// </summary>
		/// <param name="codeExpression">Code</param>
		/// <param name="codeLanguage">Code language</param>
		public Explicit2DParameters(string codeExpression, CodeLanguage codeLanguage, PlotMode plotMode, double min, double max, double step, int numPoints)
		{
			_code = codeExpression;
			_codeLanguage = codeLanguage;
			_plotMode = plotMode;
			_min = min;
			_max = max;
			_step = step;
			_numPoints = numPoints;
		}

		#endregion

		#region Public Instance Properties

		/// <summary>Get or set plot mode</summary>
		[Description("Plot mode")]
		[TypeConverter(typeof(Genetibase.MathX.NugenCCalc.Design.Converters.EnumDescConverter))]
		[DefaultValue(PlotMode.ByNumPoints)]
		public PlotMode PlotMode
		{
			get
			{
				return _plotMode;
			}
			set
			{
				if (this._plotMode != value)
				{
					this._plotMode = value;
					NotifyChanged();
				}
			}
		}


		/// <summary>Get or set number of points for MathX plotters</summary>
		[Description("Number of point for plot")]
		[DefaultValue(200)]
		public int NumPoints
		{
			get
			{
				return _numPoints;
			}
			set
			{
				if (this._numPoints != value)
				{
					if (value <= 0)
						throw new ArgumentException("NumPoints cannot be less(or equal) than zero");
					this._numPoints = value;
					NotifyChanged();
				}
			}
		}


		/// <summary>Get or set min value for MathX plotters</summary>
		[Description("Minimum value of X axes")]
		[DefaultValue(-10)]
		public double Min
		{
			get
			{
				return _min;
			}
			set
			{
				if (this._min != value)
				{
					this._min = value;
					NotifyChanged();
				}
			}
		}


		/// <summary>Get or set max value for MathX plotters</summary>
		[Description("Maximum value of X axes")]
		[DefaultValue(10)]
		public double Max
		{
			get
			{
				return _max;
			}
			set
			{
				if (this._max != value)
				{
					this._max = value;
					NotifyChanged();
				}
			}
		}


		/// <summary>Get or set step for MathX plotters</summary>
		[Description("Step")]
		[DefaultValue(0.1)]
		public double Step
		{
			get
			{
				return _step;
			}
			set
			{
				if (this._step != value)
				{
					if (value <= 0)
						throw new ArgumentException("Step cannot be less(or equal) than zero");
					this._step = value;
					NotifyChanged();
				}
			}
		}

		/// <summary>Indicated that ...</summary>
		[Description("Polar mode")]
		[DefaultValue(false)]
		public bool IsPolar
		{
			get
			{
				return _isPolar;
			}
			set
			{
				if (_isPolar == value)
					return;
				_isPolar = value;
				NotifyChanged();
			}
		}

		/// <summary>Indicated that ...</summary>
		[Description("Invert mode")]
		[DefaultValue(false)]
		public bool IsInvert
		{
			get
			{
				return _isInvert;
			}
			set
			{
				if (_isInvert == value)
					return;
				_isInvert = value;
				_isDitry = true;
				NotifyChanged();
			}
		}

		#endregion

		#region FunctionParameters Members
		protected override void InitCode()
		{
			switch(_codeLanguage)
			{
				case CodeLanguage.CSharp:
					_code = Function2DParameters.CSharpFunction1Param.Replace("%%BODY%%", " return " + _formula + ";");
					break;
				case CodeLanguage.VBNET:
					_code = Function2DParameters.VBFunction1Param.Replace("%%BODY%%", " Return " + _formula);
					break;
			}
		}


		protected override void InitFunction()
		{
			if (this.SourceType == SourceType.Equation)
			{
				_function = new Explicit2DFunction(this.Formula);
			}
			else
			{
//				RealFunctionEvalutor evalutor = new RealFunctionEvalutor();
//				evalutor.Text = this.CodeBody;
//				switch(this.CodeLanguage)
//				{
//					case CodeLanguage.CSharp:
//						evalutor.language= CodeLanguage.CSharp;
//						break;
//					case CodeLanguage.VBNET:
//						evalutor.language = CodeLanguage.VBNET;						
//						break;
//				}
//				_function = new Explicit2DFunction(new RealFunction(evalutor.Invoke));
				FunctionEvalutor evalutor = new FunctionEvalutor(this.CodeBody, new string[]{"x"},this.CodeLanguage);
				_function = new Explicit2DFunction((RealFunction)evalutor.CreateDelegate(typeof(RealFunction)));
			}

			if (_isInvert)
			{
				_function = FunctionFactory.InverseExplicit2DFunction((Explicit2DFunction)_function);
			}

			if (this.DerivativeMode)
			{
				for (int i = 0; i < this.OrderOfDerivative; i++)
				{
					_function = _function.Derivative;
					if (_function is Constant)
						return;
				}
			}
		}
		#endregion
	}
}
