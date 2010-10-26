/* -----------------------------------------------
 * CEAction.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalculatorInternals
{
	internal sealed class CEAction : IAction
	{
		public State GetState(State currentState)
		{
			currentState.CurrentValue = 0;
			return currentState;
		}
	}
}
