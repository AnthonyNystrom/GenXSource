using System;
using System.Drawing.Design;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Genetibase.MathX.Core;


namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>
	/// Represents the method that handles the FunctionParameters
	/// </summary>
	public delegate void ParametersChangeHandler(FunctionParameters parameters, EventArgs e);

	/// <summary>
	/// 	<para>Provides the <b>abstract</b> base class for the all function
	/// parameters.</para>
	/// </summary>
	[Serializable()]
	[XmlInclude(typeof(Function2DParameters))]
	[XmlInclude(typeof(Function2DParameters))]
	[XmlInclude(typeof(Explicit2DParameters))]
	[XmlInclude(typeof(Explicit3DParameters))]
	[XmlInclude(typeof(Implicit3DParameters))]
	[XmlInclude(typeof(Implicit2DParameters))]
	[XmlInclude(typeof(Parametric2DParameters))]
	[XmlInclude(typeof(Parametric3DParameters))]
	[XmlInclude(typeof(ParametricSurfaceParameters))]
	public abstract class FunctionParameters
	{
		#region Public Constansts

		public const string CSharpFunction1Param = "double Calculate(double x) { %%BODY%% }";
		public const string CSharpFunction2Param = "double Calculate(double x, double y) { %%BODY%% }";
		public const string CSharpFunction3Param = "double Calculate(double x, double y, double z) { %%BODY%% }";
		public const string CSharpFunction2DParametric = "Point2D Calculate(double t) { %%BODY%% }";
		public const string CSharpFunction3DParametric = "Point3D Calculate(double t) { %%BODY%% }";
		public const string CSharpFunction3DParametricSurface = "Point3D Calculate(double u, double v) { %%BODY%% }";


		public const string VBFunction1Param = "Function Calculate(x As Double) as Double %%BODY%% End Function";
		public const string VBFunction2Param = "Function Calculate(x As Double, y As Double) as Double %%BODY%% End Function";
		public const string VBFunction3Param = "Function Calculate(x As Double, y As Double, z As Double) as Double %%BODY%% End Function";
		public const string VBFunction2DParametric = "Function Calculate(t As Double) as Point2D %%BODY%% End Function";
		public const string VBFunction3DParametric = "Function Calculate(t As Double) as Point3D %%BODY%% End Function";
		public const string VBFunction3DParametricSurface = "Function Calculate(u As Double, v As Double) as Point3D %%BODY%% End Function";

		#endregion

		#region Private Instance Fields
		protected string _name = "";

		protected CodeLanguage _codeLanguage = CodeLanguage.CSharp;
		protected string _code = "";
		protected string _formula = "";
		protected SourceType _sourceType = SourceType.Equation;

		[NonSerialized()]
		protected Function _function = null;
		[NonSerialized()]
		protected bool _isDitry = true;

		#endregion

		#region Public Events

		/// <summary>
		/// Event to notify that the item is changed
		/// </summary>
		public virtual event ParametersChangeHandler Changed;

		protected virtual void NotifyChanged()
		{
			if( null != Changed )
			{
				Changed(this, new EventArgs());
			}
		}

		#endregion

		#region Public Instance Properties

		/// <summary>Get Function (from MathX)</summary>
		[Browsable(false)]
		[XmlIgnore()]
		public Function Function
		{
			get
			{
				if (_isDitry || _function == null)
				{
					InitFunction();
					_isDitry = false;
				}
				return _function;
			}
//			set
//			{
//				if (_function == value)
//					return;
//				_function = value;
//			}
		}

		/// <summary>Get or set name of function(for designer).</summary>
		[Browsable(false)]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (_name != value)
				{
					_name = value;
				}
			}
		}


		/// <summary>Get or set formula</summary>
		[RefreshProperties(RefreshProperties.All)]
        [Editor(typeof(Design.Editors.SourceEditor), typeof(UITypeEditor))]
		public string Formula
		{
			get
			{
				return _formula;
			}
			set
			{
				if (_formula != value)
				{
					_formula = value;
					_isDitry = true;
					InitCode();
					NotifyChanged();
				}
			}
		}


		/// <summary>Get or set code language for code expression. C# or VB.NET</summary>
		//[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof(Design.Converters.EnumDescConverter))]		
		[Description("C# or VB.NET")]
		public CodeLanguage CodeLanguage
		{
			get
			{
				return _codeLanguage;
			}
			set
			{
				if (_codeLanguage != value)
				{
					_codeLanguage = value;
					_isDitry = true;
					NotifyChanged();
				}
			}
		}


		/// <summary>Get or set code expression</summary>
		[Description("Source code for this expression")]
		[RefreshProperties(RefreshProperties.All)]
		//[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [Editor(typeof(Design.Editors.SourceEditor), typeof(UITypeEditor))]
		public string Code
		{
			get
			{
				return _code;
			}
			set
			{
				if (_code != value)
				{
					_code = value;
					_isDitry = true;
					NotifyChanged();
				}
			}
		}


		/// <summary>
		/// Get code expression header
		/// </summary>
		[Browsable(false)]
		[XmlIgnore()]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
		public string CodeHeader
		{
			get
			{
				Regex re = null;
				MatchCollection mc = null;
				switch(_codeLanguage)
				{
					case CodeLanguage.CSharp:
						re = new Regex(".*?{", RegexOptions.IgnoreCase|RegexOptions.Singleline);
						mc = re.Matches(_code);
						if (mc.Count != 0)
							return mc[0].ToString().Trim();
						break;
					case CodeLanguage.VBNET:
						re = new Regex(".*?\\) As Double", RegexOptions.IgnoreCase|RegexOptions.Singleline);
						mc = re.Matches(_code);
						if (mc.Count != 0)
							return mc[0].ToString().Trim();
						break;
				}
				return "";
			}
		}


		/// <summary>
		/// Get code expression footer
		/// </summary>
		[Browsable(false)]
		[XmlIgnore()]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
		public string CodeFooter
		{
			get
			{
				switch(_codeLanguage)
				{
					case CodeLanguage.CSharp:
						return "}";
					case CodeLanguage.VBNET:
						return "End Function";
				}
				return "";
			}
		}


		/// <summary>
		/// Get code expression body
		/// </summary>
		[Browsable(false)]
		[XmlIgnore()]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
		public string CodeBody
		{
			get
			{
				Regex re = null;
				MatchCollection mc = null;
				switch(_codeLanguage)
				{
					case CodeLanguage.CSharp:
						re = new Regex("(?<={).*(?=})", RegexOptions.IgnoreCase|RegexOptions.Singleline);
						mc = re.Matches(_code);
						if (mc.Count != 0)
							return mc[0].ToString().Trim();
						break;
					case CodeLanguage.VBNET:
						re = new Regex("(?<=\\) as\\sDoubl)e.*(?=End\\sFunction)", RegexOptions.IgnoreCase|RegexOptions.Singleline);
						mc = re.Matches(_code);
						if (mc.Count != 0)
							return mc[0].ToString().Substring(1).Trim();
						break;
				}
				return "";	
			}
		}


		/// <summary>Get or set type of source. Equation or CodeExpression</summary>
		[TypeConverter(typeof(Design.Converters.EnumDescConverter))]		
		[RefreshProperties(RefreshProperties.All)]
		[Description("Type of source for component. Equation or Code Expression")]
		public SourceType SourceType
		{
			get
			{
				return this._sourceType;
			}
			set
			{
				this._sourceType = value;
				if (_sourceType == SourceType.Equation)
					InitCode();
				NotifyChanged();
			}		
		}
		#endregion

		#region ICloneable Members

		/// <summary>
		/// When overridden in a derived class, creates an exact copy of this
		/// <see cref="FunctionParameters"/>.
		/// </summary>
		public object Clone()
		{
			return base.MemberwiseClone();
		}

		#endregion

		#region Public Instance Methods

		/// <summary>Returns a String that represents the current <b>Object</b>.</summary>
		public override string ToString()
		{
			return _name;
		}


		protected abstract void InitCode();

		protected abstract void InitFunction();

		#endregion
	}
}
