/* -----------------------------------------------
 * BackspaceAction.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Genetibase.Shared.Controls.CalculatorInternals
{
	internal class BackspaceAction : IAction
	{
		public State GetState(State currentState)
		{
			string currentValue = currentState.CurrentValue.ToString(CultureInfo.InvariantCulture);

			int dotIndex = -1;

			if (currentState.IsFractional)
			{
				dotIndex = currentValue.IndexOf('.');

				if (dotIndex == -1)
				{
					currentValue += ".";
				}
			}

			if (currentValue.Length == 1)
			{
				currentValue = "0";
			}
			else
			{
				currentValue = currentValue.Substring(0, currentValue.Length - 1);
				dotIndex = currentValue.IndexOf('.');

				if (dotIndex == -1)
				{
					currentState.FractionDigitCount = 0;
					currentState.IsFractional = false;
				}
				else
				{
					currentState.FractionDigitCount = currentValue.Length - dotIndex - 1;
				}
			}

			if (currentValue == "-")
			{
				currentValue = "0";
			}

			currentState.CurrentValue = double.Parse(currentValue, NumberStyles.Float, CultureInfo.InvariantCulture);
			return currentState;
		}
	}
}
