using System;
using System.ComponentModel;
using Genetibase.MathX.Core;

namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>Store settings of parametric 3D function</summary>
	[Serializable()]
    [FunctionCodeAttribute("Point3D Calculate(double t)\n{", "}", "Function Calculate(t As Double) as Point3D", "End Function")]
	public class Parametric3DParameters : Function3DParameters
	{
		#region Private Instance Fields

		private double _min = -10;
		private double _max = 10;
		private int _numPoints = 200;
		private string _formulaY = "";
		private string _formulaZ = "";

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

		/// <summary>Get or set formula for Z dependent from <strong>t</strong> parameter.</summary>
		public string FormulaZ
		{
			get
			{
				return _formulaZ;
			}
			set
			{
				if (_formulaZ == value)
					return;
				_formulaZ = value;
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

		#endregion

		#region Public Contructors

		/// <summary>
		/// Initializes a new instance of the <strong>Parametric3DParameters</strong>
		/// class.
		/// </summary>
		public Parametric3DParameters()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <strong>Parametric3DParameters</strong> class
		/// on the specified formulas.
		/// </summary>
		public Parametric3DParameters(string formula, string formulaY, string formulaZ)
		{
			_formula = formula;
			_formulaY = formulaY;
			_formulaZ = formulaZ;
			InitCode();
		}


		/// <summary>
		/// Initializes a new instance of the <strong>Parametric3DParameters</strong> class
		/// on the specified formulas.
		/// </summary>
		public Parametric3DParameters(string formula, string formulaY, string formulaZ, double min, double max,  int numPoints)
		{
			_formula = formula;
			_formulaY = formulaY;
			_formulaZ = formulaZ;
			_min = min;
			_max = max;
			_numPoints = numPoints;
			InitCode();
		}

		/// <summary>
		/// Initializes a new instance of the <strong>Parametric3DParameters</strong> class
		/// on the specified formulas.
		/// </summary>
		public Parametric3DParameters(string codeExpression, CodeLanguage codeLanguage, double min, double max, int numPoints)
		{
			_code = codeExpression;
			_codeLanguage = codeLanguage;
			_min = min;
			_max = max;
			_numPoints = numPoints;
		}

		#endregion

		#region FunctionParameters Members
		protected override void InitFunction()
		{
			if (this.SourceType == SourceType.Equation)
			{
				_function = new Parameter3DFunction(_formula, _formulaY, _formulaZ);
			}
			else
			{
//				Parametric3DEvalutor evalutor = new Parametric3DEvalutor();
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
//				_function = new Parameter3DFunction(new Parameter3DFunctionDelegate(evalutor.Invoke));			
				FunctionEvalutor evalutor = new FunctionEvalutor(this.CodeBody, new string[]{"t"},this.CodeLanguage);
				_function = new Parameter3DFunction((Parameter3DFunctionDelegate)evalutor.CreateDelegate(typeof(Parameter3DFunctionDelegate)));

			}

		}

		protected override void InitCode()
		{
			switch(_codeLanguage)
			{
				case CodeLanguage.CSharp:
					_code = Function3DParameters.CSharpFunction3DParametric.Replace("%%BODY%%", " return new Point3D(" + _formula+"," +_formulaY+"," +_formulaZ+ ");");
					break;
				case CodeLanguage.VBNET:
					_code = Function3DParameters.VBFunction3DParametric.Replace("%%BODY%%", " Return " + _formula);
					break;
			}
		}
		#endregion
	}
}
