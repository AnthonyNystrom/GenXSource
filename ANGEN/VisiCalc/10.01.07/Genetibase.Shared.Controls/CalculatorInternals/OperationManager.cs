/* -----------------------------------------------
 * OperationManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalculatorInternals
{
	internal static class OperationManager
	{
		public static IOperation Divide = new DivideOperation();
		public static IOperation Empty = new EmptyOperation();
		public static IOperation Minus = new MinusOperation();
		public static IOperation Multiply = new MutliplyOperation();
		public static IOperation Plus = new PlusOperation();
	}
}
