/* -----------------------------------------------
 * PlusOperation.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalculatorInternals
{
	internal sealed class PlusOperation : IOperation
	{
		public State Evaluate(State currentState)
		{
			currentState.CurrentValue = currentState.PreviousValue + currentState.CurrentValue;
			return currentState;
		}
	}
}
