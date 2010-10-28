using System;
using System.ComponentModel;
using System.Drawing;
using Genetibase.MathX.Core;

namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>
	/// Store settings of Implicit 3D function
	/// </summary>
	[Serializable()]
    [FunctionCodeAttribute("double Calculate(double x, double y, double z)\n{", "}", "Function Calculate(x As Double, y As Double, z As Double) as Double", "End Function")]
	public class Implicit3DParameters : Function3DParameters
	{
		#region Private Instance Fields
		private Point3D _point3DA = new Point3D(-10, -10, -10);
		private Point3D _point3DB = new Point3D(10, 10, 10);

		private int _pixelSizeX = 1;
		private int _pixelSizeY = 1;
		private int _pixelSizeZ = 1;

		private int _gridFactor = 20;
		#endregion

		#region Public Constructors
		/// <summary>
		/// Initializes a new instance of the <strong>Implicit3DParameters</strong>
		/// class.
		/// </summary>
		public Implicit3DParameters()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <strong>Implicit3DParameters</strong> class on
		/// the specified formula.
		/// </summary>
		/// <param name="formula">Formula</param>
		public Implicit3DParameters(string formula)
		{
			_formula = formula;
			InitCode();
		}

		/// <summary>
		/// Initializes a new instance of the <strong>Implicit3DParameters</strong> class on
		/// the specified formula, left-top corner point, right-bottom corner point and grid
		/// factor.
		/// </summary>
		/// <param name="formula">Formula</param>
		/// <param name="point3DA">Left-top corner point</param>
		/// <param name="point3DB">Right-bottom corner point</param>
		/// <param name="gridFactor">
		/// This parameter specify graphics details. Increasing of this parameter give more
		/// detailed graphic but decrease speed of evaluation.
		/// </param>
		public Implicit3DParameters(string formula, Point3D point3DA, Point3D point3DB, int gridFactor)
		{
			_formula = formula;
			_point3DA = point3DA;
			_point3DB = point3DB;
			_gridFactor = gridFactor;
			InitCode();
		}

		/// <summary>
		/// Initializes a new instance of the <strong>Implicit3DParameters</strong> class on
		/// the specified code, code language, left-top corner point, right-bottom corner point and
		/// grid factor.
		/// </summary>
		/// <param name="codeExpression">Code</param>
		/// <param name="codeLanguage">Code language</param>
		/// <param name="point3DA">Left-top corner point</param>
		/// <param name="point3DB">Right-bottom corner point</param>
		/// <param name="gridFactor">
		/// This parameter specify graphics details. Increasing of this parameter give more
		/// detailed graphic but decrease speed of evaluation.
		/// </param>
		public Implicit3DParameters(string codeExpression, CodeLanguage codeLanguage, Point3D point3DA, Point3D point3DB, int gridFactor)
		{
			_code = codeExpression;
			_codeLanguage = codeLanguage;
			_point3DA = point3DA;
			_point3DB = point3DB;
			_gridFactor = gridFactor;
		}
		#endregion

		#region Public Instance Properties

		/// <summary>Get or set left-top corner point</summary>
		[Description("Left-bottom corner of plot area")]
		[TypeConverter(typeof(Design.Converters.Point3DConverter))]		
		public Point3D Point3DA
		{
			get
			{
				return _point3DA;
			}
			set
			{
				if (this._point3DA != value)
				{
					this._point3DA = value;
					NotifyChanged();
				}
			}
		}


		/// <summary>Get or set right-bottom corner point</summary>
		[Description("Top-right corner of plot area")]
		[TypeConverter(typeof(Design.Converters.Point3DConverter))]		
		public Point3D Point3DB
		{
			get
			{
				return _point3DB;
			}
			set
			{
				if (this._point3DB != value)
				{
					this._point3DB = value;
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


		/// <summary>Get or set size of one point on chart by X value</summary>
		[Description("PixelSizeX")]
		public int PixelSizeX
		{
			get
			{
				return _pixelSizeX;
			}
			set
			{
				if (this._pixelSizeX != value)
				{
					this._pixelSizeX = value;
					NotifyChanged();
				}
			}
		}

		/// <summary>Get or set size of one point on chart by Y value</summary>
		[Description("PixelSizeY")]
		public int PixelSizeY
		{
			get
			{
				return _pixelSizeY;
			}
			set
			{
				if (this._pixelSizeY != value)
				{
					this._pixelSizeY = value;
					NotifyChanged();
				}
			}
		}


		/// <summary>Get or set size of one point on chart by Z value</summary>
		[Description("PixelSizeZ")]
		public int PixelSizeZ
		{
			get
			{
				return _pixelSizeZ;
			}
			set
			{
				if (this._pixelSizeZ != value)
				{
					this._pixelSizeZ = value;
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
					_code = Function3DParameters.CSharpFunction3Param.Replace("%%BODY%%", " return " + _formula + ";");
					break;
				case CodeLanguage.VBNET:
					_code = Function3DParameters.VBFunction3Param.Replace("%%BODY%%", " Return " + _formula);
					break;
			}
		}

		protected override void InitFunction()
		{
			if (this.SourceType == SourceType.Equation)
			{
				_function = new Implicit3DFunction(this.Formula);
			}
			else
			{
//				TrivariateFunctionEvalutor evalutor = new TrivariateFunctionEvalutor();
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
//				_function = new Implicit3DFunction(new TrivariateRealFunction(evalutor.Invoke));
				FunctionEvalutor evalutor = new FunctionEvalutor(this.CodeBody, new string[]{"x", "y", "z"},this.CodeLanguage);
				_function = new Implicit3DFunction((TrivariateRealFunction)evalutor.CreateDelegate(typeof(TrivariateRealFunction)));

			}
		}
		#endregion
	}
}
