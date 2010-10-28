using System;
using System.ComponentModel;
using Genetibase.MathX.Core;

namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>Store settings of parametric 2D function</summary>
	[Serializable()]
    [FunctionCodeAttribute("Point2D Calculate(double t)\n{", "}", "Function Calculate(t As Double) as Point2D", "End Function")]
	public class Parametric2DParameters : Function2DParameters
	{
		#region Private Instance Fields

		private PlotMode _plotMode = PlotMode.ByNumPoints;
		private double _min = -10;
		private double _max = 10;
		private double _step = 0.1;
		private int _numPoints = 200;
		private string _formulaY = "";

		#endregion

		#region Public Instance Properties

		/// <summary>Get or set formula for Y dependent from <strong>t</strong> parameter.</summary>
		public string FormulaY
		{
			get
			{
				return _formulaY;
			}
			set
			{
				if (_formulaY == value)
					return;
				_formulaY = value;
			}
		}

		/// <summary>Get or set plot mode</summary>
		[Description("Plot mode")]
		[TypeConverter(typeof(Design.Converters.EnumDescConverter))]
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
		#endregion

		#region Public Contructors

		/// <summary>
		/// Initializes a new instance of the <strong>Parametric2DParameters</strong>
		/// class.
		/// </summary>
		public Parametric2DParameters()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <strong>Parametric2DParameters</strong> class
		/// on the specified formulas.
		/// </summary>
		/// <param name="formula">Formula for X dependent from <strong>t</strong> parameter.</param>
		/// <param name="formulaY">Formula for Y dependent from <strong>t</strong> parameter.</param>
		public Parametric2DParameters(string formula, string formulaY)
		{
			_formula = formula;
			_formulaY = formulaY;
			InitCode();
		}


		/// <summary>
		/// Initializes a new instance of the <strong>Parametric2DParameters</strong> class
		/// on the specified formulas.
		/// </summary>
		/// <param name="formula">Formula for X dependent from <strong>t</strong> parameter.</param>
		/// <param name="formulaY">Formula for Y dependent from <strong>t</strong> parameter.</param>
		public Parametric2DParameters(string formula, string formulaY, PlotMode plotMode, double min, double max, double step, int numPoints)
		{
			_formula = formula;
			_formulaY = formulaY;
			_plotMode = plotMode;
			_min = min;
			_max = max;
			_step = step;
			_numPoints = numPoints;
			InitCode();
		}

		/// <summary>
		/// Initializes a new instance of the <strong>Parametric2DParameters</strong> class
		/// on the specified code and code language.
		/// </summary>
		public Parametric2DParameters(string codeExpression, CodeLanguage codeLanguage, PlotMode plotMode, double min, double max, double step, int numPoints)
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

		#region FunctionParameters Members
		protected override void InitFunction()
		{
			if (this.SourceType == SourceType.Equation)
			{
				_function = new Parameter2DFunction(_formula, _formulaY);
			}
			else
			{
//				Parametric2DEvalutor evalutor = new Parametric2DEvalutor();
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
//				_function = new Parameter2DFunction(new Parameter2DFunctionDelegate(evalutor.Invoke));			
				FunctionEvalutor evalutor = new FunctionEvalutor(this.CodeBody, new string[]{"t"},this.CodeLanguage);
				_function = new Parameter2DFunction((Parameter2DFunctionDelegate)evalutor.CreateDelegate(typeof(Parameter2DFunctionDelegate)));

			}

			if (this.DerivativeMode)
			{
				for (int i = 0; i < this.OrderOfDerivative; i++)
				{
					_function = _function.Derivative;
				}
			}
		}

		protected override void InitCode()
		{
			switch(_codeLanguage)
			{
				case CodeLanguage.CSharp:
					_code = Function2DParameters.CSharpFunction2DParametric.Replace("%%BODY%%", " return new Point2D(" + _formula+"," +_formulaY+ ");");
					break;
				case CodeLanguage.VBNET:
					_code = Function2DParameters.VBFunction2DParametric.Replace("%%BODY%%", " Return " + _formula);
					break;
			}
		}
		#endregion

	}
}
