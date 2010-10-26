/* -----------------------------------------------
 * ActionManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalculatorInternals
{
	internal static class ActionManager
	{
		public static readonly IAction Backspace = new BackspaceAction();
		public static readonly IAction C = new CAction();
		public static readonly IAction CE = new CEAction();
		public static readonly IAction D0 = new D0Action();
		public static readonly IAction D1 = new D1Action();
		public static readonly IAction D2 = new D2Action();
		public static readonly IAction D3 = new D3Action();
		public static readonly IAction D4 = new D4Action();
		public static readonly IAction D5 = new D5Action();
		public static readonly IAction D6 = new D6Action();
		public static readonly IAction D7 = new D7Action();
		public static readonly IAction D8 = new D8Action();
		public static readonly IAction D9 = new D9Action();
		public static readonly IAction DivX = new DivXAction();
		public static readonly IAction Dot = new DotAction();
		public static readonly IAction Divide = new DivideAction();
		public static readonly IAction Evaluate = new EvaluateAction();
		public static readonly IAction Minus = new MinusAction();
		public static readonly IAction Multiply = new MultiplyAction();
		public static readonly IAction Percent = new PercentAction();
		public static readonly IAction Plus = new PlusAction();
		public static readonly IAction Sign = new SignAction();
		public static readonly IAction Sqrt = new SqrtAction();
	}
}
