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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using Genetibase.NuGenMock.Properties;

namespace Genetibase.NuGenMock
{
	/// <summary>
	/// Represents an expectation for all kinds of values like strings, types, integers, etc.
	/// Extends the <see cref="AbstractStaticExpectation"/> class.
	/// </summary>
	public class ExpectationValue<T> : AbstractStaticExpectation
	{
		private T _actualValue;

		/// <summary>
		/// </summary>
		public T Actual
		{
			get
			{
				return _actualValue;
			}
			set
			{
				_actualValue = value;
				_isVerified = false;

				if (this.ShouldCheckImmediately)
				{
					this.Verify();
				}
			}
		}

		private T _expectedValue;

		/// <summary>
		/// </summary>
		public T Expected
		{
			get
			{
				return _expectedValue;
			}
			set
			{
				_expectedValue = value;
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
		/// </summary>
		public override void ClearActual()
		{
			_actualValue = default(T);
			_isVerified = false;
		}

		/// <summary>
		/// </summary>
		public override void ClearExpected()
		{
			_expectedValue = default(T);
			_isVerified = false;
			this.HasExpectations = false;
		}

		/// <summary>
		/// </summary>
		public override void Verify()
		{
			if (this.HasExpectations)
			{
				Assert.AreEqual<T>(
					_expectedValue
					, _actualValue
					, String.Format(
						CultureInfo.CurrentCulture
						, Resources.Message_ValuesNotEqual
						, this.name
						)
					);
				_isVerified = true;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Genetibase.NuGenMock.ExpectationValue`1"/> class.
		/// </summary>
		public ExpectationValue(String name)
			: base(name)
		{
			this.ClearActual();
		}
	}
}
