/* -----------------------------------------------
 * NuGenPositiveInt32.cs
 * Copyright © 2006 Alex Nesterov
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
	/// <see cref="NuGenInt32"/> wrapper that supports only positive values.
	/// </summary>
	public class NuGenPositiveInt32 : NuGenEventInitiator
	{
		/*
		 * Value
		 */

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
		/// Will bubble the <see cref="E:Genetibase.Shared.NuGenPositiveInt32.ValueChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnValueChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_valueChanged, e);
		}

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
		public override bool Equals(object obj)
		{
			NuGenPositiveInt32 compared = obj as NuGenPositiveInt32;

			if (compared != null)
			{
				return compared.Value == this.Value;
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
		public override int GetHashCode()
		{
			return this.Value;
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
		public override string ToString()
		{
			return this.Value.ToString(CultureInfo.CurrentCulture);				
		}

		/// <summary>
		/// </summary>
		public static implicit operator NuGenInt32(NuGenPositiveInt32 positiveInt32)
		{
			NuGenInt32 int32 = new NuGenInt32(1, int.MaxValue);
			int32.Value = positiveInt32.Value;
			return int32;
		}

		private NuGenInt32 _int;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPositiveInt32"/> class.
		/// </summary>
		public NuGenPositiveInt32()
		{
			_int = new NuGenInt32(1, int.MaxValue);
			_int.ValueChanged += delegate(object sender, EventArgs e)
			{
				this.OnValueChanged(e);
			};
		}
	}
}
