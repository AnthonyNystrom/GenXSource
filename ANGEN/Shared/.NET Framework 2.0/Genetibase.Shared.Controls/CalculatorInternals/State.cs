/* -----------------------------------------------
 * State.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalculatorInternals
{
	internal sealed class State : NuGenEventInitiator
	{
		private double _currentValue;

		public double CurrentValue
		{
			get
			{
				return _currentValue;
			}
			set
			{
				_currentValue = value;
				this.OnCurrentValueChanged(EventArgs.Empty);
			}
		}

		private static readonly object _currentValueChanged = new object();

		public event EventHandler CurrentValueChanged
		{
			add
			{
				this.Events.AddHandler(_currentValueChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_currentValueChanged, value);
			}
		}

		private void OnCurrentValueChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_currentValueChanged, e);
		}

		private int _fractionDigitCount;

		public int FractionDigitCount
		{
			get
			{
				return _fractionDigitCount;
			}
			set
			{
				_fractionDigitCount = value;
			}
		}

		private bool _isCalculated;

		public bool IsCalculated
		{
			get
			{
				return _isCalculated;
			}
			set
			{
				_isCalculated = value;
			}
		}

		private bool _isFractional;

		public bool IsFractional
		{
			get
			{
				return _isFractional;
			}
			set
			{
				_isFractional = value;
			}
		}

		private bool _isSequentialEvaluation;

		public bool IsSequentialEvaluation
		{
			get
			{
				return _isSequentialEvaluation;
			}
			set
			{
				_isSequentialEvaluation = value;
			}
		}

		private IOperation _operation;

		public IOperation Operation
		{
			get
			{
				if (_operation == null)
				{
					return OperationManager.Empty;
				}

				return _operation;
			}
			set
			{
				_operation = value;
			}
		}

		private double _previousValue;

		public double PreviousValue
		{
			get
			{
				return _previousValue;
			}
			set
			{
				_previousValue = value;
			}
		}

		public State()
		{
		}
	}
}
