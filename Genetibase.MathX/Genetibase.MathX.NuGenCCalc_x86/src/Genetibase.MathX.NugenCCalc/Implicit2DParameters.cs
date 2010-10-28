using System;
using System.ComponentModel;
using System.Drawing;
using Genetibase.MathX.Core;


namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>
	/// Store settings of Implicit 2D function
	/// </summary>
	[Serializable()]
    [FunctionCodeAttribute("double Calculate(double x, double y)\n{", "}", "Function Calculate(x As Double, y As Double) as Double", "End Function")]
    public class Implicit2DParameters : Function2DParameters
	{
		#region Private Instance Fields

		protected Point2D _pointA = new Point2D(-10, -10);
		protected Point2D _pointB = new Point2D(10, 10);

		protected int _gridFactor = 20;
		protected Size _areaSize = new Size(10,10);

		#endregion

		#region Public Constructors

		/// <summary>
		/// Initializes a new instance of the <strong>Implicit2DParameters</strong>
		/// class.
		/// </summary>
		public Implicit2DParameters()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <strong>Implicit2DParameters</strong> class on
		/// the specified formula.
		/// </summary>
		public Implicit2DParameters(string formula)
		{
			_formula = formula;
			InitCode();
		}

		/// <summary>
		/// Initializes a new instance of the <strong>Implicit2DParameters</strong> class on
		/// the specified formula, left-top corner point, right-bottom corner point, grid factor
		/// and area size.
		/// </summary>
		/// <param name="formula">Formula</param>
		/// <param name="pointA">Left-top corner point</param>
		/// <param name="pointB">Right-bottom corner point</param>
		/// <param name="gridFactor">
		/// This parameter specify graphics details. Increasing of this parameter give more
		/// detailed graphic but decrease speed of evaluation.
		/// </param>
		/// <param name="areaSize">formula, left-top corner point, right-bottom corner point and area size.</param>
		public Implicit2DParameters(string formula, Point2D pointA, Point2D pointB, int gridFactor, Size areaSize)
		{
			_formula = formula;
			_pointA = pointA;
			_pointB = pointB;
			_gridFactor = gridFactor;
			_areaSize = areaSize;
			InitCode();
		}

		/// <summary>
		/// Initializes a new instance of the <strong>Implicit2DParameters</strong> class on
		/// the specified code, code language, left-top corner point, right-bottom corner point,
		/// grid factor and area size.
		/// </summary>
		/// <param name="codeExpression">code</param>
		/// <param name="codeLanguage">Code language</param>
		/// <param name="pointA">Left-top corner point</param>
		/// <param name="pointB">Right-bottom corner point</param>
		/// <param name="gridFactor">
		/// This parameter specify graphics details. Increasing of this parameter give more
		/// detailed graphic but decrease speed of evaluation.
		/// </param>
		/// <param name="areaSize">Area size.</param>
		public Implicit2DParameters(string codeExpression, CodeLanguage codeLanguage, Point2D pointA, Point2D pointB, int gridFactor, Size areaSize)
		{
			_code = codeExpression;
			_codeLanguage = codeLanguage;
			_pointA = pointA;
			_pointB = pointB;
			_gridFactor = gridFactor;
			_areaSize = areaSize;
		}
		#endregion

		#region Public Instance Properties

		/// <summary>Get or set left-top corner point</summary>
		[Description("Left-bottom corner of plot area")]
		[TypeConverter(typeof(Design.Converters.Point2DConverter))]		
		public Point2D PointA
		{
			get
			{
				return _pointA;
			}
			set
			{
				if (this._pointA != value)
				{
					this._pointA = value;
					NotifyChanged();
				}
			}
		}


		/// <summary>Get or set right-bottom corner point</summary>
		[Description("Top-right corner of plot area")]
		[TypeConverter(typeof(Design.Converters.Point2DConverter))]		
		public Point2D PointB
		{
			get
			{
				return _pointB;
			}
			set
			{
				if (this._pointB != value)
				{
					this._pointB = value;
					NotifyChanged();
				}
			}
		}


		/// <summary>
		/// This parameter specify graphics details. Increasing of this parameter give more
		/// detailed graphic but decrease speed of evaluation.
		/// </summary>
		[Description("GridFactor. 2 minimum")]
		[TypeConverter(typeof(Int32Converter))]
		public int GridFactor
		{
			get
			{
				return _gridFactor;
			}
			set
			{
				if (this._gridFactor != value)
				{
					if (value < 2)
						throw new ArgumentException("Grid factor cannot be less than 2");
					this._gridFactor = value;
					NotifyChanged();
				}
			}
		}


		/// <summary>Get or set area size</summary>
		[Description("Size of plot area")]
		public Size AreaSize
		{
			get
			{
				return _areaSize;
			}
			set
			{
				if (this._areaSize != value)
				{
					this._areaSize = value;
					NotifyChanged();
				}
			}
		}
		#endregion

		#region FunctionParameters Members

		protected override void InitCode()
		{
			switch(_codeLanguage)
			{
				case CodeLanguage.CSharp:
					_code = Function2DParameters.CSharpFunction2Param.Replace("%%BODY%%", " return " + _formula + ";");
					break;
				case CodeLanguage.VBNET:
					_code = Function2DParameters.VBFunction2Param.Replace("%%BODY%%", " Return " + _formula);
					break;
			}
		}

		protected override void InitFunction()
		{
			if (this.SourceType == SourceType.Equation)
			{
				_function = new Implicit2DFunction(this.Formula);
			}
			else
			{
//				BivariateFunctionnEvalutor evalutor = new BivariateFunctionnEvalutor();
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
//				_function = new Implicit2DFunction(new BivariateRealFunction(evalutor.Invoke));
				FunctionEvalutor evalutor = new FunctionEvalutor(this.CodeBody, new string[]{"x", "y"},this.CodeLanguage);
				_function = new Implicit2DFunction((BivariateRealFunction)evalutor.CreateDelegate(typeof(BivariateRealFunction)));

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
