using System;
using System.Collections;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Genetibase.MathX.Core
{
	/// <summary>Compiles mathematical expression into runtime assembly.</summary>
	public class ExpressionCompiler
	{
		private static readonly ICodeCompiler _compiler = new Microsoft.CSharp.CSharpCodeProvider().CreateCompiler();

		private MethodInfo _methodInfo = null;
		
		private ExpressionTree[] _trees;				

		public ExpressionCompiler(ExpressionTree tree)
		{
			_trees = new ExpressionTree[]{ tree };
		}

		public ExpressionCompiler(ExpressionTree[] trees)
		{
			_trees = trees;
		}


		

		private void Compile(string codeExpressionMask, Type returnType)
		{
			string codeExpression = codeExpressionMask;

			for (int i = 0; i < _trees.Length; i++)
			{
				codeExpression = codeExpression.Replace("{"+i.ToString()+"}",_trees[i].ToCodeExpression());
			}
			
			CodeCompileUnit cu = new CodeCompileUnit();
			CodeNamespace ns = new CodeNamespace("Genetibase.MathX.Core.Runtime");
			CodeMemberMethod method = new CodeMemberMethod();
			CodeTypeDeclaration cl = new CodeTypeDeclaration("RuntimeFunction");
			CompilerResults compile_result;

			cu.Namespaces.Add(ns);
			ns.Imports.Add(new CodeNamespaceImport("System"));
			ns.Imports.Add(new CodeNamespaceImport("Genetibase.MathX.Core"));

			ns.Types.Add(cl);

			foreach (string variable in _trees[0].Variables )
				method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(double),variable));
		
			method.ReturnType = new CodeTypeReference( returnType);

			cl.TypeAttributes = TypeAttributes.Public;
			cl.Members.Add( method);
			method.Attributes = MemberAttributes.Public | MemberAttributes.Static;
			method.Name="Eval";

			method.Statements.Clear();
			method.Statements.Add(new CodeExpressionStatement(new CodeSnippetExpression( codeExpression)));
			

			CompilerParameters compparams = new CompilerParameters(new string[]{"mscorlib.dll", this.GetType().Assembly.Location});			
			compparams.GenerateInMemory=true;
		

			compile_result = _compiler.CompileAssemblyFromDom( compparams, cu);

			if ( compile_result == null || compile_result.Errors.Count > 0 )
			{
				string errors = "";
				if (compile_result != null)
				foreach (CompilerError error in compile_result.Errors)
					errors += error.ToString() + Environment.NewLine;
				throw new Exception("Failed to compile " + errors);
			};
				
			_methodInfo = compile_result.CompiledAssembly.GetType(
				"Genetibase.MathX.Core.Runtime.RuntimeFunction").GetMethod("Eval");
		}

		public System.Delegate CreateDelegate(Type type)
		{
			if (_trees.Length == 1) 
				return CreateDelegate(type, "return {0};");

			string[] args = (string[])Array.CreateInstance(typeof(string),_trees.Length);
			for (int i = 0; i < args.Length; i++)
			{
				args[i] = "{"+i.ToString()+"}";
			}

			return CreateDelegate(type, string.Format("return {0};",string.Join("+",args)));
		}

		public System.Delegate CreateDelegate(Type type, string codeExpressionMask)
		{
			Compile(codeExpressionMask, type.GetMethod("Invoke").ReturnType);										

			return Delegate.CreateDelegate(type,_methodInfo);		
		}

	}
}
