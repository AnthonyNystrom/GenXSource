/* -----------------------------------------------
 * PlusAction.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalculatorInternals
{
	internal sealed class PlusAction : IAction
	{
		public State GetState(State currentState)
		{
			currentState = LogicManager.ProcessOperation(currentState, OperationManager.Plus);
			return LogicManager.Evaluate(currentState);
		}
	}
}
