using System;
using System.ComponentModel;
using Genetibase.MathX.Core;

namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>Store settings of parametric 3D surface.</summary>
	[Serializable()]
    [FunctionCodeAttribute("Point3D Calculate(double u, double v)\n{", "}", "Function Calculate(u As Double, v As Double) as Point3D", "End Function")]
	public class ParametricSurfaceParameters : Function3DParameters
	{
		#region Private Instance Fields

		private double _minU = -10;
		private double _maxU = 10;
		private double _minV = -10;
		private double _maxV = 10;
		private int _numPoints = 200;
		private string _formulaY = "";
		private string _formulaZ = "";

		#endregion

		#region Public Instance Properties

		/// <summary>
		/// Get or set formula for Y dependent from <strong>u</strong> and <strong>v</strong>
		/// parameter.
		/// </summary>
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

		/// <summary>
		/// Get or set formula for Z dependent from <strong>u</strong> and <strong>v</strong>
		/// parameter.
		/// </summary>
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


		/// <summary>Get or set min U value for MathX plotters</summary>
		[Description("Minimum value of X axes")]
		[DefaultValue(-10)]
		public double MinU
		{
			get
			{
				return _minU;
			}
			set
			{
				if (this._minU != value)
				{
					this._minU = value;
					NotifyChanged();
				}
			}
		}


		/// <summary>Get or set max U value for MathX plotters</summary>
		[Description("Maximum value of X axes")]
		[DefaultValue(10)]
		public double MaxU
		{
			get
			{
				return _maxU;
			}
			set
			{
				if (this._maxU != value)
				{
					this._maxU = value;
					NotifyChanged();
				}
			}
		}


		/// <summary>Get or set min V value for MathX plotters</summary>
		[Description("Minimum value of X axes")]
		[DefaultValue(-10)]
		public double MinV
		{
			get
			{
				return _minV;
			}
			set
			{
				if (this._minV != value)
				{
					this._minV = value;
					NotifyChanged();
				}
			}
		}


		/// <summary>Get or set max V value for MathX plotters</summary>
		[Description("Maximum value of X axes")]
		[DefaultValue(10)]
		public double MaxV
		{
			get
			{
				return _maxV;
			}
			set
			{
				if (this._maxV != value)
				{
					this._maxV = value;
					NotifyChanged();
				}
			}
		}
		#endregion

		#region Public Contructors

		/// <summary>
		/// Initializes a new instance of the <strong>ParametricSurfaceParameters</strong>
		/// class.
		/// </summary>
		public ParametricSurfaceParameters()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <strong>ParametricSurfaceParameters</strong>
		/// class on the specified formulas.
		/// </summary>
		public ParametricSurfaceParameters(string formula, string formulaY, string formulaZ)
		{
			_formula = formula;
			_formulaY = formulaY;
			_formulaZ = formulaZ;
			InitCode();
		}


		/// <summary>
		/// Initializes a new instance of the <strong>ParametricSurfaceParameters</strong>
		/// class on the specified formulas.
		/// </summary>
		public ParametricSurfaceParameters(string formula, string formulaY, string formulaZ, double minU, double maxU, double minV, double maxV,  int numPoints)
		{
			_formula = formula;
			_formulaY = formulaY;
			_formulaZ = formulaZ;
			_minU = minU;
			_maxU = maxU;
			_minV = minV;
			_maxV = maxV;
			_numPoints = numPoints;
			InitCode();
		}

		/// <summary>
		/// Initializes a new instance of the <strong>ParametricSurfaceParameters</strong>
		/// class on the specified formulas.
		/// </summary>
		public ParametricSurfaceParameters(string codeExpression, CodeLanguage codeLanguage, double minU, double maxU, double minV, double maxV, int numPoints)
		{
			_code = codeExpression;
			_codeLanguage = codeLanguage;
			_minU = minU;
			_maxU = maxU;
			_minV = minV;
			_maxV = maxV;
			_numPoints = numPoints;
		}

		#endregion

		#region FunctionParameters Members
		protected override void InitFunction()
		{
			if (this.SourceType == SourceType.Equation)
			{
				_function = new ParametricSurface(_formula, _formulaY, _formulaZ);
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
				FunctionEvalutor evalutor = new FunctionEvalutor(this.CodeBody, new string[]{"u", "v"},this.CodeLanguage);
				_function = new ParametricSurface((ParametricSurfaceDelegate)evalutor.CreateDelegate(typeof(ParametricSurfaceDelegate)));

			}

		}

		protected override void InitCode()
		{
			switch(_codeLanguage)
			{
				case CodeLanguage.CSharp:
					_code = Function3DParameters.CSharpFunction3DParametricSurface.Replace("%%BODY%%", " return new Point3D(" + _formula+"," +_formulaY+"," +_formulaZ+ ");");
					break;
				case CodeLanguage.VBNET:
					_code = Function3DParameters.VBFunction3DParametricSurface.Replace("%%BODY%%", " Return " + _formula);
					break;
			}
		}
		#endregion
	}
}
