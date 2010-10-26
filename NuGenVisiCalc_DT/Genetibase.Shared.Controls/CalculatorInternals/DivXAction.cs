/* -----------------------------------------------
 * DivXAction.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalculatorInternals
{
	internal sealed class DivXAction : IAction
	{
		public State GetState(State currentState)
		{
			currentState.CurrentValue = Math.Pow(currentState.CurrentValue, -1);
			return currentState;
		}
	}
}
