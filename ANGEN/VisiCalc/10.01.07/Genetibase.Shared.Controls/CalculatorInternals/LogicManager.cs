/* -----------------------------------------------
 * LogicManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalculatorInternals
{
	internal static class LogicManager
	{
		public static State Evaluate(State currentState)
		{
			double bufferCurrentValue;

			if (!currentState.IsSequentialEvaluation)
			{
				currentState.IsSequentialEvaluation = true;
				bufferCurrentValue = currentState.CurrentValue;
			}
			else
			{
				bufferCurrentValue = currentState.PreviousValue;
			}

			currentState = currentState.Operation.Evaluate(currentState);
			currentState.IsCalculated = true;
			currentState.PreviousValue = bufferCurrentValue;
			return currentState;
		}

		public static State ProcessDigit(State currentState, int digit)
		{
			if (currentState.IsCalculated)
			{
				LogicManager.ResetState(currentState);
			}

			if (currentState.IsFractional)
			{
				currentState.FractionDigitCount++;
				double fractionDivider = Math.Pow(10, currentState.FractionDigitCount);

				if (currentState.CurrentValue < 0)
				{
					currentState.CurrentValue = currentState.CurrentValue - digit / fractionDivider;
				}
				else
				{
					currentState.CurrentValue = currentState.CurrentValue + digit / fractionDivider;
				}

				currentState.CurrentValue = Math.Round(currentState.CurrentValue, currentState.FractionDigitCount);
			}
			else
			{
				currentState.CurrentValue = currentState.CurrentValue * 10 + digit;
			}

			return currentState;
		}

		public static State ProcessOperation(State currentState, IOperation operation)
		{
			currentState.IsCalculated = true;
			currentState.Operation = operation;
			return currentState;
		}

		private static void ResetState(State currentState)
		{
			currentState.PreviousValue = currentState.CurrentValue;
			currentState.CurrentValue = 0;
			currentState.FractionDigitCount = 0;
			currentState.IsCalculated = false;
			currentState.IsFractional = false;
			currentState.IsSequentialEvaluation = false;
		}
	}
}
