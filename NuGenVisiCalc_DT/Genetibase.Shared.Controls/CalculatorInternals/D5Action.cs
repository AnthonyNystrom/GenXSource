/* -----------------------------------------------
 * D5Action.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalculatorInternals
{
	internal sealed class D5Action : IAction
	{
		public State GetState(State currentState)
		{
			return LogicManager.ProcessDigit(currentState, 5);
		}
	}
}
