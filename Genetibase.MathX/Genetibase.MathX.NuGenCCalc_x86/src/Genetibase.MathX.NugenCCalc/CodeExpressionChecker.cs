using System;
using System.Collections.Specialized;
using System.CodeDom;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using Genetibase.MathX.Core;

namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>Provide methods for check source code</summary>
	/// <example>
	/// 	<code lang="CS" description="The following example gets some code and check it with given code language">
	/// string code = "double Calc(double x)
	/// {
	///     double gain = 1.5;
	///     return x*gain;
	/// }
	/// "
	/// String resultString;
	///  
	/// CodeExpressionChecker.CheckCode(CodeLanguage.CSharp, code, ref resultString);
	/// </code>
	/// </example>
	public class CodeExpressionChecker
	{

        #region Public Static Methods
		private static int resultSpace = 3;

		/// <summary>Check code with given CodeDomProvider.</summary>
		/// <example>
		/// 	<code lang="CS" description="The following example gets some code and check it with given CodeDomProvider">
		/// string code = "double Calc(double x)
		/// {
		///     double gain = 1.5;
		///     return x*gain;
		/// }
		/// "
		/// String resultString;
		///  
		/// CodeExpressionChecker.CheckCode(new CSharpCodeProvider(), code, ref resultString);
		/// </code>
		/// </example>
		public static void CheckCode(CodeDomProvider provider, String sourceCode, ref String errorString)
		{
			ICodeCompiler compiler = provider.CreateCompiler();

			CompilerParameters cp = new CompilerParameters(new string[]{"mscorlib.dll", typeof(Point2D).Assembly.Location});
			cp.GenerateInMemory = true;       
			CompilerResults cr = compiler.CompileAssemblyFromSource(cp, sourceCode);
			if (cr.NativeCompilerReturnValue != 0)
			{
				int i = resultSpace;
				string outputString = "";

				while(i < cr.Output.Count)
				{
					outputString += cr.Output[i] + Environment.NewLine;
					i++;
				}
				// Return the results of compilation with error flag and error string
				errorString = outputString;
			}
			else
				errorString = "";
		}

		/// <summary>Check code with given CodeLanguage.</summary>
		/// <example>
		/// 	<code lang="CS" description="The following example gets some code and check it with given code language">
		/// string code = "double Calc(double x)
		/// {
		///     double gain = 1.5;
		///     return x*gain;
		/// }
		/// "
		/// String resultString;
		///  
		/// CodeExpressionChecker.CheckCode(CodeLanguage.CSharp, code, ref resultString);
		/// </code>
		/// </example>
		public static void CheckCode(CodeLanguage language, String sourceCode, ref String errorString)
		{
			switch(language)
			{
				case CodeLanguage.CSharp:
					CheckCode(new CSharpCodeProvider(), sourceCode, ref errorString);
					break;
				case CodeLanguage.VBNET:
					CheckCode(new VBCodeProvider(), sourceCode, ref errorString);
					break;
			}
		}


        #endregion Public Instance Methods

	}
}
