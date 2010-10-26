/* -----------------------------------------------
 * PlusOperation.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
