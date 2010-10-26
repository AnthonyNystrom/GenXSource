/* -----------------------------------------------
 * EmptyOperation.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalculatorInternals
{
	internal sealed class EmptyOperation : IOperation
	{
		public State Evaluate(State currentState)
		{
			return currentState;
		}
	}
}
