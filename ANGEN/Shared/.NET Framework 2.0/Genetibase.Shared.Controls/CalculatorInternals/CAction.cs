/* -----------------------------------------------
 * CAction.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalculatorInternals
{
	internal sealed class CAction : IAction
	{
		public State GetState(State currentState)
		{
			currentState.CurrentValue = 0;
			currentState.FractionDigitCount = 0;
			currentState.IsCalculated = false;
			currentState.IsFractional = false;
			currentState.IsSequentialEvaluation = false;
			currentState.Operation = OperationManager.Empty;
			currentState.PreviousValue = 0;

			return currentState;
		}
	}
}
