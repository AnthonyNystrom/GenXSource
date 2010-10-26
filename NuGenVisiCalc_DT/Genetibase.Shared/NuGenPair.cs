/* -----------------------------------------------
 * NuGenPair.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared
{
	/// <summary>
	/// Represents a pair of values of the same type.
	/// </summary>
	public class NuGenPair<T>
	{
		private T _valueOne;

		/// <summary>
		/// </summary>
		public T GetValueOne()
		{
			return _valueOne;
		}

		/// <summary>
		/// </summary>
		public void SetValueOne(T value)
		{
			_valueOne = value;
		}

		private T _valueTwo;

		/// <summary>
		/// </summary>
		public T GetValueTwo()
		{
			return _valueTwo;
		}

		/// <summary>
		/// </summary>
		public void SetValueTwo(T value)
		{
			_valueTwo = value;
		}

		/// <summary>
		/// </summary>
		public void GetValues(out T valueOne, out T valueTwo)
		{
			valueOne = this.GetValueOne();
			valueTwo = this.GetValueTwo();
		}

		/// <summary>
		/// </summary>
		public void SetValues(T valueOne, T valueTwo)
		{
			this.SetValueOne(valueOne);
			this.SetValueTwo(valueTwo);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Genetibase.Shared.NuGenPair`1"/> class.
		/// </summary>
		public NuGenPair()
			: this(default(T), default(T))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Genetibase.Shared.NuGenPair`1"/> class.
		/// </summary>
		public NuGenPair(T valueOne, T valueTwo)
		{
			_valueOne = valueOne;
			_valueTwo = valueTwo;
		}
	}
}
