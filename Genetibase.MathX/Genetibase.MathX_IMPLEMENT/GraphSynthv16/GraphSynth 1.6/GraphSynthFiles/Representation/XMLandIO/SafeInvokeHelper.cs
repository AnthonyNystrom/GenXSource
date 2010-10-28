using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace GraphSynth
{
    /* Safe Invoke Methods to access forms. */
    /* threading is difficult stuff, and VS.NET tries to warn/limit you to problems that will
     * occur in invoking routines across threads. We will need to do this so that the search
     * process run unencumbered while invoking changes to our display in the main thread. I've
     * borrowed the SafeInvokeHelper routine from the blog by John Wood:
     * http://dotnetjunkies.com/WebLog/johnwood/archive/2005/08/31/132267.aspx 
     * Thanks, John Wood, for your clear description and routines. */

    public class SafeInvokeHelper
    {
        static readonly ModuleBuilder builder;
        static readonly AssemblyBuilder myAsmBuilder;
        static readonly Hashtable methodLookup;

        #region Constructor
        static SafeInvokeHelper()
        {
            AssemblyName name = new AssemblyName();
            name.Name = "temp";
            myAsmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
            builder = myAsmBuilder.DefineDynamicModule("TempModule");
            methodLookup = new Hashtable();
        }
        #endregion
        #region main function call, Invoke
        /* here is the main function that all subsequent functions call. */
        public static object Invoke(System.Windows.Forms.Control obj, string methodName, params object[] paramValues)
        {
            try
            {
                Delegate del = null;
                string key = obj.GetType().Name + "." + methodName;
                Type tp;
                if (methodLookup.Contains(key))
                    tp = (Type)methodLookup[key];
                else
                {
                    Type[] paramList = new Type[obj.GetType().GetMethod(methodName).GetParameters().Length];
                    int n = 0;
                    foreach (ParameterInfo pi in obj.GetType().GetMethod(methodName).GetParameters()) paramList[n++] = pi.ParameterType;
                    TypeBuilder typeB = builder.DefineType("Del_" + obj.GetType().Name + "_" + methodName, TypeAttributes.Class | TypeAttributes.AutoLayout | TypeAttributes.Public | TypeAttributes.Sealed, typeof(MulticastDelegate), PackingSize.Unspecified);
                    ConstructorBuilder conB = typeB.DefineConstructor(MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, new Type[] { typeof(object), typeof(IntPtr) });
                    conB.SetImplementationFlags(MethodImplAttributes.Runtime);
                    MethodBuilder mb = typeB.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, obj.GetType().GetMethod(methodName).ReturnType, paramList);
                    mb.SetImplementationFlags(MethodImplAttributes.Runtime);
                    tp = typeB.CreateType();
                    methodLookup.Add(key, tp);
                }

                del = MulticastDelegate.CreateDelegate(tp, obj, methodName);
                return obj.Invoke(del, paramValues);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Form CrossThread Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        #endregion
    }
}