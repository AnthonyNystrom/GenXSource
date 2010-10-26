#region License

/*
 * Copyright © 2004, 2005 Griffin Caprio & Choy Rim. All rights reserved.
 * 
 * Copyright © 2007 Alex Nesterov [Modifier]. All rights reseved.
 * mailto:a.nesterov@genetibase.com
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 * notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 * notice, this list of conditions and the following disclaimer in the
 * documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 * derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

#endregion

using System;
using System.Collections.Generic;
using Genetibase.NuGenMock.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace Genetibase.NuGenMock
{
	/// <summary>
	/// Abstract class that implements the <see cref="T:Genetibase.NuGenMock.IListExpectation`T"/> interface and extends the <see cref="AbstractStaticExpectation"/> class.
	/// </summary>
	/// <remarks/>
	public abstract class AbstractIListExpectation<T> : AbstractStaticExpectation, IListExpectation<T>
	{
		/// <summary>
		/// Checks the given values immediately.
		/// </summary>
		/// <param name="actual">Values to check.</param>
		protected abstract void CheckImmediateValues(T actual);

		/// <summary>
		/// </summary>
		public abstract IList<T> ActualList
		{
			get;
		}

		/// <summary>
		/// </summary>
		public abstract IList<T> ExpectedList
		{
			get;
		}

		private Boolean _isVerified;

		/// <summary>
		/// Gets the value indicating whether this object has been verified.
		/// </summary>
		/// <value></value>
		public override Boolean IsVerified
		{
			get
			{
				return _isVerified;
			}
		}

		/// <summary>
		/// Adds a value to the actual collection.
		/// </summary>
		/// <param name="actual">Item to add.</param>
		public void AddActual(T actual)
		{
			this.ActualList.Add(actual);
			_isVerified = false;

			if (this.ShouldCheckImmediately)
			{
				this.CheckImmediateValues(actual);
				_isVerified = true;
			}
		}

		/// <summary>
		/// Adds an array of values to the actual collection.
		/// </summary>
		/// <param name="actualMany">Items to add.</param>
		public void AddActualMany(T[] actualMany)
		{
			for (var i = 0; i < actualMany.Length; i++)
			{
				this.AddActual(actualMany[i]);
			}
		}

		/// <summary>
		/// Adds an enumeration of values to the actual collection.
		/// </summary>
		/// <param name="actualMany">Items to add.</param>
		public void AddActualMany(IEnumerable<T> actualMany)
		{
			IEnumerator<T> enumerator = actualMany.GetEnumerator();

			while (enumerator.MoveNext())
			{
				this.AddActual(enumerator.Current);
			}
		}

		/// <summary>
		/// Adds a list of values to the actual collection.
		/// </summary>
		/// <param name="actualMany">Items to add.</param>
		public void AddActualMany(IList<T> actualMany)
		{
			for (int i = 0; i < actualMany.Count; i++)
			{
				this.AddActual(actualMany[i]);
			}
		}

		/// <summary>
		/// Adds a value to the expected collection.
		/// </summary>
		/// <param name="expected">Item to add.</param>
		public void AddExpected(T expected)
		{
			this.ExpectedList.Add(expected);
			this.HasExpectations = true;
			_isVerified = false;
		}

		/// <summary>
		/// Adds several values to the expected collection.
		/// </summary>
		/// <param name="expectedMany">Items to add.</param>
		public void AddExpectedMany(T[] expectedMany)
		{
			for (var i = 0; i < expectedMany.Length; i++)
			{
				AddExpected(expectedMany[i]);
			}
		}

		/// <summary>
		/// Adds several values to the expected collection.
		/// </summary>
		/// <param name="expectedMany">Items to add.</param>
		public void AddExpectedMany(IEnumerable<T> expectedMany)
		{
			IEnumerator<T> enumerator = expectedMany.GetEnumerator();

			while (enumerator.MoveNext())
			{
				this.AddExpected(enumerator.Current);
			}
		}

		/// <summary>
		/// Adds several values to the expected collection.
		/// </summary>
		/// <param name="expectedMany">Items to add.</param>
		public void AddExpectedMany(IList<T> expectedMany)
		{
			for (var i = 0; i < expectedMany.Count; i++)
			{
				this.AddExpected(expectedMany[i]);
			}
		}

		/// <summary>
		/// </summary>
		public override void ClearActual()
		{
			this.ActualList.Clear();
			_isVerified = false;
		}

		/// <summary>
		/// </summary>
		public override void ClearExpected()
		{
			this.ExpectedList.Clear();
			this.HasExpectations = false;
			_isVerified = false;
		}

		/// <summary>
		/// Verifies the expectation collection.
		/// </summary>
		public override void Verify()
		{
			if (this.HasExpectations)
			{
				Assert.AreEqual(this.ExpectedList.Count, this.ActualList.Count, Resources.Message_CollectionCountNotEqual);

				for (var i = 0; i < this.ActualList.Count; i++)
				{
					Assert.AreEqual(
						this.ExpectedList[i]
						, this.ActualList[i]
						, String.Format(CultureInfo.CurrentCulture, Resources.Message_CollectionItemsNotEqual, i)
					);
				}

				_isVerified = true;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Genetibase.NuGenMock.AbstractIListExpectation`1"/> class.
		/// </summary>
		protected AbstractIListExpectation()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Genetibase.NuGenMock.AbstractIListExpectation`1"/> class.
		/// </summary>
		protected AbstractIListExpectation(String name)
			: base(name)
		{
		}
	}
}
