/* -----------------------------------------------
 * NuGenMeasureUnitsValueConverter.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Provides functionality to convert a value to its string representation according to current measure units.
	/// </summary>
	public class NuGenMeasureUnitsValueConverter : NuGenEventInitiator, INuGenMeasureUnitsValueConverter
	{
		/*
		 * Factor
		 */

		private NuGenInt32 _factorInternal;

		private NuGenInt32 FactorInternal
		{
			get
			{
				if (_factorInternal == null)
				{
					_factorInternal = new NuGenInt32(1, Int32.MaxValue);
					_factorInternal.Value = 1000;
				}

				return _factorInternal;
			}
		}

		/// <summary>
		/// Gets or sets the step for measure units. E.g. 1000 g = 1 kg. Then the factor will be 1000.
		/// </summary>
		/// <value></value>
		public Int32 Factor
		{
			get
			{
				return this.FactorInternal.Value;
			}
			set
			{
				if (this.FactorInternal.Value != value)
				{
					this.FactorInternal.Value = value;
					this.OnFactorChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Object _factorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="Factor"/> property changes.
		/// </summary>
		public event EventHandler FactorChanged
		{
			add
			{
				this.Events.AddHandler(_factorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_factorChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenMeasureUnitsValueConverter.FactorChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnFactorChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_factorChanged, e);
		}

		/*
		 * Increment
		 */

		private Int32 _increment = 1;

		/// <summary>
		/// Gets the amount to increment or decrement the value.
		/// </summary>
		/// <value></value>
		public Int32 Increment
		{
			get
			{
				return _increment;
			}
		}

		private static readonly Object _incrementChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="Increment"/> property changes.
		/// </summary>
		public event EventHandler IncrementChanged
		{
			add
			{
				this.Events.AddHandler(_incrementChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_incrementChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenMeasureUnitsValueConverter.IncrementChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnIncrementChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_incrementChanged, e);
		}

		/*
		 * MeasureUnits
		 */

		private String[] _measureUnits;

		/// <summary>
		/// Gets the list of available measure units. For weight it can be g, kg, T.
		/// </summary>
		/// <value></value>
		public String[] MeasureUnits
		{
			get
			{
				return _measureUnits;
			}
			set
			{
				if (_measureUnits != value)
				{
					_measureUnits = value;
					this.OnMeasureUnitsChanged(EventArgs.Empty);

					if (_measureUnits == null || _measureUnits.Length == 0)
					{
						Int32 increment = 1;

						if (increment != _increment)
						{
							_increment = increment;
							this.OnIncrementChanged(EventArgs.Empty);
						}
					}
					else
					{
						// Update Increment.
						Int32 result;
						this.TryParse(this.Text, CultureInfo.CurrentCulture, out result);
					}
				}
			}
		}

		private static readonly Object _measureUnitsChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="MeasureUnits"/> property changes.
		/// </summary>
		public event EventHandler MeasureUnitsChanged
		{
			add
			{
				this.Events.AddHandler(_measureUnitsChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_measureUnitsChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenMeasureUnitsValueConverter.MeasureUnitsChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMeasureUnitsChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_measureUnitsChanged, e);
		}

		/*
		 * Maximum
		 */

		private Int32 _maximum = 100;

		/// <summary>
		/// Gets or sets the maximum valid value.
		/// </summary>
		/// <value></value>
		public Int32 Maximum
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

		private static readonly Object _maximumChanged = new Object();

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
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenMeasureUnitsValueConverter.MaximumChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMaximumChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_maximumChanged, e);
		}

		/*
		 * Minimum
		 */

		private Int32 _minimum;

		/// <summary>
		/// Gets or sets the minimum valid value.
		/// </summary>
		/// <value></value>
		public Int32 Minimum
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

		private static readonly Object _minimumChanged = new Object();

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
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenMeasureUnitsValueConverter.MinimumChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMinimumChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_minimumChanged, e);
		}

		/*
		 * Text
		 */

		private String _text;

		/// <summary>
		/// Gets or sets the string value representation.
		/// </summary>
		/// <value></value>
		public String Text
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

					Int32 parsedValue = 0;

					if (this.TryParse(value
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

		private static readonly Object _textChanged = new Object();

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
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenMeasureUnitsValueConverter.TextChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnTextChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_textChanged, e);
		}

		/*
		 * Value
		 */

		private Int32 _previousValue;
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
		/// <value></value>
		public Int32 Value
		{
			get
			{
				int value = 0;

				if (this.TryParse(this.Text, CultureInfo.CurrentCulture, out value))
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
					this.Text = this.ConvertToString(value, CultureInfo.CurrentCulture);
					this.OnValueChanged(EventArgs.Empty);

					_previousValue = value;
				}
				else
				{
					this.Text = this.ConvertToString(value, CultureInfo.CurrentCulture);
				}
			}
		}

		private static readonly Object _valueChanged = new Object();

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
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenMeasureUnitsValueConverter.ValueChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnValueChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_valueChanged, e);
		}

		internal String ConvertToString(Int32 value, IFormatProvider provider)
		{
			if (this.MeasureUnits == null || this.MeasureUnits.Length == 0)
			{
				return Convert.ToString(value, provider);
			}
			else
			{
				Int32 unitIndex = 0;
				Single tempValue = value;

				while (tempValue >= this.Factor && unitIndex < this.MeasureUnits.Length - 1)
				{
					tempValue = tempValue / (Single)this.Factor;
					unitIndex++;
				}

				return String.Format(provider, "{0} {1}", tempValue, this.MeasureUnits[unitIndex]);
			}
		}

		internal Boolean TryParse(String s, IFormatProvider provider, out Int32 result)
		{
			if (s == null)
			{
				s = "";
			}

			String[] parts = s.Trim().Split(' ');

			if (parts.Length == 1)
			{
				Double value;
				bool tryParseResult = Double.TryParse(parts[0], NumberStyles.Float, provider, out value);
				this.UpdateIncrement(this.Factor, 0);
				result = Convert.ToInt32(value);
				return tryParseResult;
			}
			else if (parts.Length == 2)
			{
				String currentMeasureUnit = parts[1];
				Int32 unitIndex = -1;

				if (this.MeasureUnits != null)
				{
					for (Int32 i = 0; i < this.MeasureUnits.Length; i++)
					{
						if (this.MeasureUnits[i].Equals(currentMeasureUnit, StringComparison.CurrentCulture))
						{
							unitIndex = i;
							break;
						}
					}
				}

				if (unitIndex == -1)
				{
					result = 0;
					return false;
				}

				Double value;
				Boolean tryParseResult = Double.TryParse(parts[0], NumberStyles.Float, provider, out value);

				this.UpdateIncrement(this.Factor, unitIndex);
				result = Convert.ToInt32(value * this.Increment);
				return tryParseResult;
			}

			result = 0;
			return false;
		}

		private void UpdateIncrement(Int32 factor, Int32 unitIndex)
		{
			Int32 increment = Convert.ToInt32(Math.Pow(factor, unitIndex));

			if (_increment != increment)
			{
				_increment = increment;
				this.OnIncrementChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMeasureUnitsValueConverter"/> class.
		/// </summary>
		public NuGenMeasureUnitsValueConverter()
		{
		}
	}
}
