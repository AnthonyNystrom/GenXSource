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
using System.Globalization;
using Genetibase.NuGenMock.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.NuGenMock
{
	/// <summary>
	/// Represents an expectation for ordered arrays. Extends the <see cref="AbstractStaticExpectation"/> class.
	/// </summary>
	public class ExpectationArray<T> : AbstractStaticExpectation
	{
		private T[] _actualArray = new T[0];

		/// <summary>
		/// Gets or sets the actual array.
		/// </summary>
		public T[] Actual
		{
			get
			{
				return _actualArray;
			}
			set
			{
				_actualArray = value;
				_isVerified = false;

				if (this.ShouldCheckImmediately)
				{
					this.Verify();
				}
			}
		}

		private T[] _expectedArray = new T[0];

		/// <summary>
		/// Gets or sets the expected array.
		/// </summary>
		public T[] Expected
		{
			get
			{
				return _expectedArray;
			}
			set
			{
				_expectedArray = value;
				_isVerified = false;
				this.HasExpectations = true;
			}
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
		/// Clears the actual array.
		/// </summary>
		public override void ClearActual()
		{
			_actualArray = new T[0];
			_isVerified = false;
		}

		/// <summary>
		/// Clears the expected array.
		/// </summary>
		public override void ClearExpected()
		{
			_expectedArray = new T[0];
			_isVerified = false;
			this.HasExpectations = false;
		}

		/// <summary>
		/// Verifies that the expected array and the actual array are equal
		/// </summary>
		public override void Verify()
		{
			if (_expectedArray == null && _actualArray != null)
			{
				throw new AssertionException(
					String.Format(CultureInfo.CurrentCulture, Resources.Assertion_ExpectedArrayIsNull, _actualArray.Length)
					);
			}

			if (_actualArray == null && _expectedArray != null)
			{
				throw new AssertionException(
					String.Format(CultureInfo.CurrentCulture, Resources.Assertion_ActualArrayIsNull, _expectedArray.Length)
					);
			}

			if (_expectedArray == null && _actualArray == null)
			{
				_isVerified = true;
				return;
			}

			if (_expectedArray != null && _actualArray != null)
			{
				if (this.Strict)
				{
					if (_actualArray.Length != _expectedArray.Length)
					{
						throw new AssertionException(
							String.Format(
								CultureInfo.CurrentCulture
								, Resources.Assertion_ArrayLengthNotEqual
								, _expectedArray.Length
								, _actualArray.Length
								)
							);
					}
				}
				else
				{
					if (_actualArray.Length < _expectedArray.Length)
					{
						throw new AssertionException(
							String.Format(
								CultureInfo.CurrentCulture
								, Resources.Assertion_ArrayLengthNotAcceptable
								, _expectedArray.Length
								, _actualArray.Length
								)
							);
					}
				}

				for (var i = 0; i < _expectedArray.Length; i++)
				{
					if (!_expectedArray[i].Equals(_actualArray[i]))
					{
						Assert.AreEqual(
							_expectedArray[i]
							, _actualArray[i]
							, String.Format(
								CultureInfo.CurrentCulture
								, Resources.Message_ArrayValuesNotEqual
								, i
								, _expectedArray[i]
								, _actualArray[i]
								)
							);
					}
				}

				_isVerified = true;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Genetibase.NuGenMock.ExpectationArray`1"/> class.
		/// </summary>
		public ExpectationArray(String name)
			: base(name)
		{
		}
	}
}
