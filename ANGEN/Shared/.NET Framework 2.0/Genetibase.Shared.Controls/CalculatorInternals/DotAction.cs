/* -----------------------------------------------
 * DotAction.cs
 * Copyright � 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalculatorInternals
{
	internal sealed class DotAction : IAction
	{
		public State GetState(State currentState)
		{
			currentState.IsFractional = true;
			return currentState;
		}
	}
}
