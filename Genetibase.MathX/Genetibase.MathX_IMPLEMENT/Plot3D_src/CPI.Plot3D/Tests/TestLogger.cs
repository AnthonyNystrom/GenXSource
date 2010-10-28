using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Tests
{
    static class TestLogger
    {
        # region Static Fields

        public static bool LogTests = false;
        public static System.IO.TextWriter LogFile = Console.Out;

        # endregion

        # region Static Methods

        public static void Log(params string[] paramValues)
        {
            if (LogTests == false)
                return;

            StackFrame frame = new StackFrame(1, false);
            MethodBase method = frame.GetMethod();

            string methodName = method.Name;

            ParameterInfo[] parameters = method.GetParameters();

            if (parameters.Length != paramValues.Length)
                throw new ArgumentException("Number of values specified is different than the method's parameter count.", "paramValues");

            string[] parameterNames = new string[parameters.Length];
            string[] parameterTypes = new string[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                parameterTypes[i] = parameters[i].ParameterType.Name;
            }

            LogInternal(methodName, parameterTypes, paramValues);
        }

        private static void LogInternal(string name, string[] parameterTypes, string[] parameterValues)
        {
            LogFile.Write("Executing {0}({1})", name, string.Join(", ", parameterTypes));

            if (parameterValues.Length > 0)
                LogFile.WriteLine(" with values [{0}].", string.Join(", ", parameterValues));
            else
                LogFile.WriteLine(".");
        }

        # endregion

    }
}
