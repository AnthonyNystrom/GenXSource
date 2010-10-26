/* -----------------------------------------------
 * DivideOperation.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalculatorInternals
{
	internal sealed class DivideOperation : IOperation
	{
		public State Evaluate(State currentState)
		{
			if (currentState.CurrentValue == 0)
			{
				throw new DivideByZeroException();
			}

			currentState.CurrentValue = currentState.PreviousValue / currentState.CurrentValue;
			return currentState;
		}
	}
}
