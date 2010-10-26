/* -----------------------------------------------
 * NuGenStringIntegerService.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Provides functionality to convert a string to its integer representation and vice versa.
	/// </summary>
	public class NuGenInt32ValueConverter : NuGenEventInitiator, INuGenInt32ValueConverter
	{
		/*
		 * Maximum
		 */

		private int _maximum = 100;

		/// <summary>
		/// Gets or sets the maximum valid value.
		/// </summary>
		public int Maximum
		{
			get
			{
				return _maximum;
			}
			set
			{
				if (_maximum != value)
				{
					_maximum = value;

					Debug.Assert(this.ValueInternal != null, "this.ValueInternal != null");
					this.ValueInternal.Maximum = _maximum;

					this.OnMaximumChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _maximumChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Maximum"/> property changes.
		/// </summary>
		public event EventHandler MaximumChanged
		{
			add
			{
				this.Events.AddHandler(_maximumChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_maximumChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenInt32ValueConverter.MaximumChanged"/> event.
		/// </summary>
		protected virtual void OnMaximumChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_maximumChanged, e);
		}


		/*
		 * Minimum
		 */

		private int _minimum;

		/// <summary>
		/// Gets or sets the minimum valid value.
		/// </summary>
		public int Minimum
		{
			get
			{
				return _minimum;
			}
			set
			{
				if (_minimum != value)
				{
					_minimum = value;

					Debug.Assert(this.ValueInternal != null, "this.ValueInternal != null");
					this.ValueInternal.Minimum = _minimum;

					this.OnMinimumChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _minimumChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Minimum"/> property changes.
		/// </summary>
		public event EventHandler MinimumChanged
		{
			add
			{
				this.Events.AddHandler(_minimumChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_minimumChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenInt32ValueConverter.MinimumChanged"/> event.
		/// </summary>
		protected virtual void OnMinimumChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_minimumChanged, e);
		}

		/*
		 * Text
		 */

		private string _text;

		/// <summary>
		/// Gets or sets the string value representation.
		/// </summary>
		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				if (_text != value)
				{
					_text = value;
					this.OnTextChanged(EventArgs.Empty);

					int parsedValue = 0;

					if (int.TryParse(value
						, NumberStyles.Integer
						, CultureInfo.CurrentCulture
						, out parsedValue) 
						&& parsedValue != this.ValueInternal.Value
						)
					{
						this.OnValueChanged(EventArgs.Empty);
					}
				}
			}
		}

		private static readonly object _textChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Text"/> property changes.
		/// </summary>
		public event EventHandler TextChanged
		{
			add
			{
				this.Events.AddHandler(_textChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_textChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Shared.Controls.NuGenInt32ValueConverter.TextChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnTextChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_textChanged, e);
		}

		/*
		 * Value
		 */

		private int _previousValue;
		private NuGenInt32 _valueInternal;

		private NuGenInt32 ValueInternal
		{
			get
			{
				if (_valueInternal == null)
				{
					_valueInternal = new NuGenInt32(this.Minimum, this.Maximum);
					_valueInternal.Value = this.Minimum;
				}

				return _valueInternal;
			}
		}

		/// <summary>
		/// </summary>
		public int Value
		{
			get
			{
				int value = 0;

				if (int.TryParse(this.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out value))
				{
					value = Math.Min(Math.Max(value, this.Minimum), this.Maximum);
					_previousValue = value;
				}
				else
				{
					value = _previousValue;
				}

				return value;
			}
			set
			{
				if (this.ValueInternal.Value != value)
				{
					this.ValueInternal.Value = value;
					this.Text = value.ToString(CultureInfo.CurrentCulture);
					this.OnValueChanged(EventArgs.Empty);

					_previousValue = value;
				}
				else
				{
					this.Text = value.ToString(CultureInfo.CurrentCulture);
				}
			}
		}

		private static readonly object _valueChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Value"/> property changes.
		/// </summary>
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

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Shared.Controls.NuGenInt32ValueConverter.ValueChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnValueChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_valueChanged, e);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenInt32ValueConverter"/> class.
		/// </summary>
		public NuGenInt32ValueConverter()
		{
		}
	}
}
