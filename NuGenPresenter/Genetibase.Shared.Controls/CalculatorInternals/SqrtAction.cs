/* -----------------------------------------------
 * SqrtAction.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalculatorInternals
{
	internal sealed class SqrtAction : IAction
	{
		public State GetState(State currentState)
		{
			currentState.CurrentValue = Math.Sqrt(currentState.CurrentValue);
			currentState.IsCalculated = true;
			return currentState;
		}
	}
}
