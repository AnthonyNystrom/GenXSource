/* -----------------------------------------------
 * DivideAction.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalculatorInternals
{
	internal sealed class DivideAction : IAction
	{
		public State GetState(State currentState)
		{
			return LogicManager.ProcessOperation(currentState, OperationManager.Divide);
		}
	}
}
