/* -----------------------------------------------
 * Engine.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalculatorInternals
{
	internal partial class Engine : NuGenEventInitiator
	{
		public double Value
		{
			get
			{
				return _state.CurrentValue;
			}
			set
			{
				_state.CurrentValue = value;
			}
		}

		private static readonly object _valueChanged = new object();

		public event EventHandler ValueChanged
		{
			add
			{
				this.Events.AddHandler(_valueChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_valueChanged, value);
			}
		}

		private void OnValueChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_valueChanged, e);
		}

		public void ProcessAction(IAction action)
		{
			_state = action.GetState(_state);	
		}

		private State _state;

		public Engine()
		{
			_state = new State();
			_state.CurrentValueChanged += delegate(object sender, EventArgs e)
			{
				this.OnValueChanged(e);
			};
		}
	}
}
