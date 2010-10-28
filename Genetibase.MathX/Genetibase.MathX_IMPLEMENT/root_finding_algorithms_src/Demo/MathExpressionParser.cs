using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using RootFinding;

///////////////////////////////////////////////////////////////////////
//																	 //
// Source : http://www.thecodeproject.com/cs/algorithms/matheval.asp //
//																	 //
// Author : Marcin Cuprjak (aka Vlad Tepes)							 //
//																	 //
///////////////////////////////////////////////////////////////////////



namespace RootFinding
{
	internal class MathExpressionParser
	{
		CodeDomProvider m_Provider=null;
		CompilerParameters m_CompilerParameters=null;
		
		/// <summary>Constructor.</summary>
		public MathExpressionParser(string executablepathname) {
			// Create the compiler
			m_Provider=new Microsoft.CSharp.CSharpCodeProvider();
			// Set the parameters of the compiler
			m_CompilerParameters=new CompilerParameters(new string[] { "System.dll","RootFinding.dll",executablepathname });
			m_CompilerParameters.GenerateInMemory=true;
			m_CompilerParameters.GenerateExecutable=false;
		}

		public UnaryFunction Parse(string expr) {
			CheckExpression(ref expr);
			string src="using System;"+"using RootFinding;"+"class myfunclass:IUnaryFunction"+"{"+"public myfunclass(){}"+"public double Eval(double x)"+"{"+"return "+expr+";"+"}"+"}";
			CompilerResults results=m_Provider.CompileAssemblyFromSource(m_CompilerParameters,src);
			if(results.Errors.Count==0&&results.CompiledAssembly!=null) {
				Type ObjType=results.CompiledAssembly.GetType("myfunclass");
				try {
					IUnaryFunction func=null;
					if(ObjType!=null) func=Activator.CreateInstance(ObjType) as IUnaryFunction;
					return new UnaryFunction(func.Eval);
				} catch(Exception ex) {
					throw new MathExpressionParserException(GetExceptionMessage(ex.Message));
				}
			} else throw new MathExpressionParserException(GetExceptionMessage(results.Errors[0].ToString()));
		}

		private void CheckExpression(ref string expr) {
			expr=expr.Replace("ln","Math.Log");
			expr=expr.Replace("exp","Math.Exp");
			expr=expr.Replace("sqrt","Math.Sqrt");
			expr=expr.Replace("sin","Math.Sin");
			expr=expr.Replace("cos","Math.Cos");
			expr=expr.Replace("pi","Math.PI");
		}

		private string GetExceptionMessage(string message) {
			int i=message.LastIndexOf(':');
			return i!=-1?message.Remove(0,i+2):message;
		}
	}


	public class MathExpressionParserException : Exception {
		public MathExpressionParserException(Exception ex)
			: base("",ex) {
		}
		public MathExpressionParserException(string message)
			: base(message) {
		}
	}

}