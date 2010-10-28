using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using Genetibase.MathX.Core;

namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>Provide methods for evaluate expressions.</summary>
	public class FunctionEvalutor
	{
		private static readonly ICodeCompiler _sharpCompiler = new Microsoft.CSharp.CSharpCodeProvider().CreateCompiler();
		private static readonly ICodeCompiler _vbCompiler = new Microsoft.VisualBasic.VBCodeProvider().CreateCompiler();

		private MethodInfo _methodInfo = null;
		
		private string _code;				
		private CodeLanguage _language = CodeLanguage.CSharp;
		private string[] _variables;

		/// <summary>
		/// Initializes a new instance of the FunctionEvalutor class on the specified code
		/// and list of variables
		/// </summary>
		/// <param name="code">Code</param>
		/// <param name="variables">List of variables</param>
		public FunctionEvalutor(string code, string[] variables)
		{
			_code = code;
			_variables = variables;
		}

		/// <summary>
		/// Initializes a new instance of the FunctionEvalutor class on the specified code,
		/// code language and list of variables.
		/// </summary>
		/// <param name="code">Code</param>
		/// <param name="variables">List of variables</param>
		/// <param name="language">Code language</param>
		public FunctionEvalutor(string code, string[] variables, CodeLanguage language)
		{
			_code = code;
			_variables = variables;
			_language = language;
		}
		

		private void Compile(string codeExpression, Type returnType)
		{
			CodeCompileUnit cu = new CodeCompileUnit();
			CodeNamespace ns = new CodeNamespace("Genetibase.MathX.Core.Runtime");
			CodeMemberMethod method = new CodeMemberMethod();
			CodeTypeDeclaration cl = new CodeTypeDeclaration("RuntimeFunction");
			CompilerResults compile_result = null;

			cu.Namespaces.Add(ns);
			ns.Imports.Add(new CodeNamespaceImport("System"));
			ns.Imports.Add(new CodeNamespaceImport("Genetibase.MathX.Core"));

			ns.Types.Add(cl);

			foreach (string variable in _variables)
				method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(double),variable));
		
			method.ReturnType = new CodeTypeReference(returnType);

			cl.TypeAttributes = TypeAttributes.Public;
			cl.Members.Add( method);
			method.Attributes = MemberAttributes.Public | MemberAttributes.Static;
			method.Name="Eval";

			method.Statements.Clear();
			method.Statements.Add(new CodeExpressionStatement(new CodeSnippetExpression(codeExpression)));

			CompilerParameters compparams = new CompilerParameters(new string[]{"mscorlib.dll", typeof(Point2D).Assembly.Location});
			compparams.GenerateInMemory=true;
		
			if (_language == CodeLanguage.CSharp)
			{
				if( _sharpCompiler!=null)
					compile_result = _sharpCompiler.CompileAssemblyFromDom(compparams, cu);
			}
			else
			{
				if( _vbCompiler!=null)
					compile_result = _vbCompiler.CompileAssemblyFromDom(compparams, cu);
			}

			if ( compile_result == null || compile_result.Errors.Count > 0 )
			{
				string outputString = "";
				foreach(string outString in compile_result.Output)
				{
					outputString+=outString;
				}
				throw new CompileException(outputString);
			}
				
			_methodInfo = compile_result.CompiledAssembly.GetType(
				"Genetibase.MathX.Core.Runtime.RuntimeFunction").GetMethod("Eval");
		}


		/// <summary>Create new delegate</summary>
		public System.Delegate CreateDelegate(Type type)
		{
			Compile(_code, type.GetMethod("Invoke").ReturnType);										

			return Delegate.CreateDelegate(type,_methodInfo);		
		}
	}
}
