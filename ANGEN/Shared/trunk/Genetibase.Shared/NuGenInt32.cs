/* -----------------------------------------------
 * NuGenInt32.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Properties;

using System;
using System.Globalization;

namespace Genetibase.Shared
{
	/// <summary>
	/// <see cref="Int32"/> wrapper with bounds check. Throws <see cref="ArgumentException"/>
	/// if the specified <see cref="Value"/> is out of the specified bounds.
	/// </summary>
	public class NuGenInt32 : NuGenEventInitiator
	{
		#region Properties.Public

		/*
		 * Maximum
		 */

		/// <summary>
		/// Determines the upper bound for this <see cref="NuGenInt32"/>.
		/// </summary>
		private Int32 _maximum;

		/// <summary>
		/// Gets or sets the upper bound for this <see cref="NuGenInt32"/>.
		/// </summary>
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
					this.OnMaximumChanged(EventArgs.Empty);

					Int32 newMinimum = Math.Min(_minimum, _maximum);

					if (newMinimum != _minimum)
					{
						_minimum = newMinimum;
						this.OnMinimumChanged(EventArgs.Empty);
					}

					Int32 newValue = Math.Min(_value, _maximum);

					if (newValue != _value)
					{
						_value = newValue;
						this.OnValueChanged(EventArgs.Empty);
					}
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
		/// Will bubble the <see cref="MaximumChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMaximumChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_maximumChanged, e);
		}

		/*
		 * Minimum
		 */

		/// <summary>
		/// Determines the lower bound for this <see cref="NuGenInt32"/>.
		/// </summary>
		private Int32 _minimum;

		/// <summary>
		/// Gets or sets the lower bound for this <see cref="NuGenInt32"/>.
		/// </summary>
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
					this.OnMinimumChanged(EventArgs.Empty);

					Int32 newMaximum = Math.Max(_maximum, _minimum);

					if (newMaximum != _maximum)
					{
						_maximum = newMaximum;
						this.OnMaximumChanged(EventArgs.Empty);
					}

					Int32 newValue = Math.Max(_value, _minimum);

					if (newValue != _value)
					{
						_value = newValue;
						this.OnValueChanged(EventArgs.Empty);
					}
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
		/// Will bubble the <see cref="MinimumChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMinimumChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_minimumChanged, e);
		}

		/*
		 * Value
		 */

		/// <summary>
		/// Determines the value this <see cref="NuGenInt32"/> contains.
		/// </summary>
		private Int32 _value;

		/// <summary>
		/// Gets or sets the value this <see cref="NuGenInt32"/> contains.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <paramref name="value"/> is out of the specified bounds.
		/// </exception>
		public Int32 Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (!this.CheckLBound(value))
				{
					throw new ArgumentException(
						String.Format(CultureInfo.CurrentCulture, Resources.Argument_InvalidCheckLBound, this.Minimum),
						"value"
					);
				}

				if (!this.CheckUBound(value))
				{
					throw new ArgumentException(
						String.Format(CultureInfo.CurrentCulture, Resources.Argument_InvalidCheckUBound, this.Maximum),
						"value"
					);
				}

				if (_value != value)
				{
					_value = value;
					this.OnValueChanged(EventArgs.Empty);
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
		/// Will bubble the <see cref="E:Genetibase.Shared.NuGenInt32.ValueChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnValueChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_valueChanged, e);
		}

		#endregion

		#region Properties.Public.Static

		/*
		 * Empty
		 */

		private static readonly NuGenInt32 _empty = new NuGenInt32(0, 0);

		/// <summary>
		/// <c>Maximum = 0</c>. <c>Minimum = 0</c>. <c>Value = 0</c>.
		/// </summary>
		public static NuGenInt32 Empty
		{
			get
			{
				return _empty;
			}
		}

		#endregion

		#region Methods.Public.Overridden

		/*
		 * Equals
		 */

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
		/// </returns>
		public override Boolean Equals(Object obj)
		{
			NuGenInt32 compared = obj as NuGenInt32;

			if (compared != null)
			{
				if (
					compared.Value == this.Value
					&& compared.Minimum == this.Minimum
					)
				{
					return compared.Maximum == this.Maximum;
				}
			}

			return false;
		}

		/*
		 * GetHashCode
		 */

		/// <summary>
		/// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override Int32 GetHashCode()
		{
			return (this.Value & 0xFFFF) << 16 | this.Maximum >> 16 | this.Minimum >> 24;
		}

		/*
		 * ToString
		 */

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override String ToString()
		{
			return this.Value.ToString(CultureInfo.CurrentCulture);
		}

		#endregion

		#region Methods.Private

		/*
		 * CheckLBound
		 */

		private Boolean CheckLBound(Int32 value)
		{
			return value >= this.Minimum;
		}

		/*
		 * CheckUBound
		 */

		private Boolean CheckUBound(Int32 value)
		{
			return value <= this.Maximum;
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenInt32"/> class.
		/// </summary>
		public NuGenInt32(Int32 minimum, Int32 maximum)
		{
			this.Minimum = Math.Min(minimum, maximum);
			this.Maximum = Math.Max(minimum, maximum);

			this.Value = this.Minimum;
		}
	}
}
