//using System;
//using System.Reflection;
//using System.Collections;
//using System.CodeDom;
//using System.CodeDom.Compiler;

//namespace Genetibase.MathX.NugenCCalc
//{
//    /// <summary>This delegate just for internal use.</summary>
//    public delegate void CustomEventHandler(params object[] args);

//    /// <summary>This class just for internal use.</summary>
//    public class EventSubscriber
//    {
//        private static readonly ICodeCompiler _compiler = new Microsoft.CSharp.CSharpCodeProvider().CreateCompiler();

//        private EventInfo _eventInfo;
//        private Type _compiledClass;

//        public EventSubscriber(EventInfo ei)
//        {
//            _eventInfo = ei;			
//            Compile();	
//        }

//        public void Subscribe(object obj, CustomEventHandler handler)
//        {
//            Activator.CreateInstance(_compiledClass,new object[]{obj,handler});
//        }

//        private Hashtable GetAssemblyReferencesByType(Type t)
//        {		
//            Hashtable result = new Hashtable();
//            GetAssemblyReferencesByType(t,result);
//            return result;
//        }

//        private void GetAssemblyReferencesByType(Type t, Hashtable references)
//        {
//            if (t.BaseType!=null) GetAssemblyReferencesByType(t.BaseType,references);
//            foreach (Type i in t.GetInterfaces())
//            {
//                GetAssemblyReferencesByType(i,references);
//            }
//            if (!references.ContainsKey(t.Assembly.Location))
//                references.Add(t.Assembly.Location,t.Assembly);
//        }

//        private void Compile()
//        {
//            /*
			
//            Lets generate something like that
			
//            public class ButtonClickEventSubscriber
//            {
//                CustomEventHandler _handler;
				
//                public ButtonClickEventSubscriber(object obj, CustomEventHandler handler)
//                {
//                    _handler = handler;
//                    ((button)obj).Click += new ClickEventHandler(Event);
//                }
				
//                private Event(object sender, EventArgs args)
//                {
//                    _handler(new object[]{sender,args});
//                }	
//            }
//            */

			
//            ParameterInfo[] eventParams = _eventInfo.EventHandlerType.GetMethod("Invoke").GetParameters();

//            string className = String.Format("{0}{1}EventSubscriber",_eventInfo.DeclaringType.Name,_eventInfo.Name);

//            CodeCompileUnit cu = new CodeCompileUnit();
//            CodeNamespace ns = new CodeNamespace("Genetibase.MathX.NugenCCalc.Runtime");
			
//            CodeMemberField field = new CodeMemberField(typeof(CustomEventHandler), "_handler");

//            CodeConstructor constructor = new CodeConstructor();
//            constructor.Attributes = MemberAttributes.Public;

//            CodeMemberMethod method = new CodeMemberMethod();
//            CodeTypeDeclaration cl = new CodeTypeDeclaration(className);
//            CompilerResults compile_result;

//            cu.Namespaces.Add(ns);
//            ns.Imports.Add(new CodeNamespaceImport("System"));
//            ns.Imports.Add(new CodeNamespaceImport("Genetibase.MathX.NugenCCalc"));

//            ns.Types.Add(cl);

//            cl.TypeAttributes = TypeAttributes.Public;
//            cl.Members.Add(field);
//            cl.Members.Add(method);
//            cl.Members.Add(constructor);
			
//            cu.ReferencedAssemblies.Add(this.GetType().Assembly.Location);

//            foreach(object o in GetAssemblyReferencesByType(_eventInfo.DeclaringType).Keys)
//            {
//                cu.ReferencedAssemblies.Add((string)o);
//            }
			
//            // Constructor

//            constructor.Attributes = MemberAttributes.Public;			
//            constructor.Parameters.Add(
//                new CodeParameterDeclarationExpression(typeof(object),"obj"));
//            constructor.Parameters.Add(
//                new CodeParameterDeclarationExpression(typeof(CustomEventHandler),"handler"));
//            constructor.Statements.Clear();

//            constructor.Statements.Add(new CodeExpressionStatement(
//                new CodeSnippetExpression("_handler = handler")));
//            constructor.Statements.Add(new CodeExpressionStatement(
//                new CodeSnippetExpression(
//                string.Format("(({0})obj).{1} += new {2}(Event)",_eventInfo.DeclaringType.FullName,_eventInfo.Name,_eventInfo.EventHandlerType.FullName)
//                )));

//            // Event method

//            method.ReturnType = null;
//            method.Attributes = MemberAttributes.Public;
//            method.Name="Event";

//            ArrayList paramNames = new ArrayList();
			
//            foreach (ParameterInfo pi in eventParams)
//            {
//                method.Parameters.Add(new CodeParameterDeclarationExpression(pi.ParameterType,pi.Name));
//                paramNames.Add (pi.Name);
//            }

//            method.Statements.Clear();
//            method.Statements.Add(new CodeExpressionStatement(
//                new CodeSnippetExpression(
//                String.Format("_handler({0})",string.Join(", ",(string[])paramNames.ToArray(typeof(string))))
//                )));

//            // Compile code

//            CompilerParameters compparams = new CompilerParameters();
//            compparams.GenerateInMemory=true;
			
//            compile_result = _compiler.CompileAssemblyFromDom( compparams, cu);
			
//            if ( compile_result == null || compile_result.Errors.Count > 0 )
//                throw new Exception("Cant compile");

//            _compiledClass = compile_result.CompiledAssembly.GetType(
//                "Genetibase.MathX.NugenCCalc.Runtime." + className);		
//        }

//    }
//}
