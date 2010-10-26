/* -----------------------------------------------
 * NuGenNonNegativeInt32.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Genetibase.Shared
{
	/// <summary>
	/// <see cref="NuGenInt32"/> wrapper that supports only non-negative values.
	/// </summary>
	public class NuGenNonNegativeInt32 : NuGenEventInitiator
	{
		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <seealso cref="NuGenInt32.Value"/>
		/// </exception>
		public int Value
		{
			get
			{
				Debug.Assert(_int != null, "_int != null");
				return _int.Value;
			}
			set
			{
				Debug.Assert(_int != null, "_int != null");
				_int.Value = value;
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
		/// Will bubble the <see cref="E:Genetibase.Shared.NuGenNonNegativeInt32.ValueChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnValueChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_valueChanged, e);
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
		/// </returns>
		public override bool Equals(object obj)
		{
			NuGenNonNegativeInt32 compared = obj as NuGenNonNegativeInt32;

			if (compared != null)
			{
				return compared.Value == this.Value;
			}

			return false;
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override int GetHashCode()
		{
			return this.Value;
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			return this.Value.ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// </summary>
		public static implicit operator NuGenInt32(NuGenNonNegativeInt32 nonNegativeInt32)
		{
			NuGenInt32 int32 = new NuGenInt32(0, int.MaxValue);
			int32.Value = nonNegativeInt32.Value;
			return int32;
		}

		private NuGenInt32 _int;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenNonNegativeInt32"/> class.
		/// </summary>
		public NuGenNonNegativeInt32()
		{
			_int = new NuGenInt32(0, int.MaxValue);
			_int.ValueChanged += delegate(object sender, EventArgs e)
			{
				this.OnValueChanged(e);
			};
		}
	}
}
