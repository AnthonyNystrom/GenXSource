using System;
using System.ComponentModel;
using System.Drawing;
using Genetibase.MathX.Core;

namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>
	/// Store settings of Explicit 3D function
	/// </summary>
	[Serializable()]
    [FunctionCodeAttribute("double Calculate(double x, double y)\n{", "}", "Function Calculate(x As Double, y As Double) as Double", "End Function")]
	public class Explicit3DParameters : Function3DParameters
	{
		#region Private Instance Fields
		private Point2D _pointA = new Point2D(-10, -10);
		private Point2D _pointB = new Point2D(10, 10);
		private Size _areaSize = new Size(10,10);
		#endregion

		#region Public Constructors

		/// <summary>Initializes a new instance of the Explicit3DParameters class.</summary>
		public Explicit3DParameters()
		{

		}		

		/// <summary>
		/// Initializes a new instance of the Explicit2DParameters class on the specified
		/// formula.
		/// </summary>
		/// <param name="formula">Formula</param>
		public Explicit3DParameters(string formula)
		{
			_formula = formula;
			InitCode();
		}

		/// <summary>
		/// Initializes a new instance of the Explicit2DParameters class on the specified
		/// formula, left-top corner point, right-bottom corner point and area size.
		/// </summary>
		/// <param name="formula">Formula</param>
		/// <param name="pointA">Left-top corner point</param>
		/// <param name="pointB">Right-bottom corner point</param>
		/// <param name="size">Area size.</param>
		public Explicit3DParameters(string formula, Point2D pointA, Point2D pointB, Size size)
		{
			_formula = formula;
			_pointA = pointA;
			_pointB = pointB;
			_areaSize = size;
			InitCode();
		}


		/// <summary>
		/// Initializes a new instance of the Explicit2DParameters class on the specified
		/// code, code language, left-top corner point, right-bottom corner point and area
		/// size.
		/// </summary>
		/// <param name="codeExpression">code</param>
		/// <param name="codeLanguage">Code language.</param>
		/// <param name="pointA">Left-top corner point</param>
		/// <param name="pointB">Right-bottom corner point</param>
		/// <param name="size">Area size.</param>
		public Explicit3DParameters(string codeExpression, CodeLanguage codeLanguage, Point2D pointA, Point2D pointB, Size size)
		{
			_code = codeExpression;
			_codeLanguage = codeLanguage;
			_pointA = pointA;
			_pointB = pointB;
			_areaSize = size;
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
					_code = Function3DParameters.CSharpFunction2Param.Replace("%%BODY%%", " return " + _formula + ";");
					break;
				case CodeLanguage.VBNET:
					_code = Function3DParameters.VBFunction2Param.Replace("%%BODY%%", " Return " + _formula);
					break;
			}
		}

		protected override void InitFunction()
		{
			if (this.SourceType == SourceType.Equation)
			{
				_function = new Explicit3DFunction(this.Formula);
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
//				_function = new Explicit3DFunction(new BivariateRealFunction(evalutor.Invoke));
				FunctionEvalutor evalutor = new FunctionEvalutor(this.CodeBody, new string[]{"x", "y"},this.CodeLanguage);
				_function = new Explicit3DFunction((BivariateRealFunction)evalutor.CreateDelegate(typeof(BivariateRealFunction)));

			}
		}
		#endregion
	}
}
