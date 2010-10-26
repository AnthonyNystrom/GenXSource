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
	/// Represents an expectation to verify the number of calls. Extends the <see cref="AbstractStaticExpectation"/> class.
	/// </summary>
	public class ExpectationCounter : AbstractStaticExpectation
	{
		/// <summary>
		/// </summary>
		public Int32 Expected
		{
			set
			{
				_expectedCalls = value;
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
		/// Resests Actual counter to 0.
		/// </summary>
		public override void ClearActual()
		{
			_actualCalls = 0;
			_isVerified = false;
		}

		/// <summary>
		/// Resets Expected counter to 0.
		/// </summary>
		public override void ClearExpected()
		{
			_expectedCalls = 0;
			_isVerified = false;
			this.HasExpectations = false;
		}

		/// <summary>
		/// Increments Actual counter.
		/// </summary>
		public void Inc()
		{
			_actualCalls++;

			if (this.ShouldCheckImmediately)
			{
				Assert.IsTrue(
					_actualCalls <= _expectedCalls
					, String.Format(
						CultureInfo.CurrentCulture
						, Resources.Message_IncImmediateCheck
						, this.name
						, _expectedCalls
						)
					);
				_isVerified = true;
			}
		}

		/// <summary>
		/// </summary>
		public override void Verify()
		{
			if (this.Strict)
			{
				if (this.HasExpectations)
				{
					Assert.AreEqual(
						_expectedCalls
						, _actualCalls
						, String.Format(CultureInfo.CurrentCulture, Resources.Message_ExpectedCountInvalid, this.name)
						);
					_isVerified = true;
				}
			}
			else
			{
				if (this.HasExpectations)
				{
					Assert.IsTrue(
						_actualCalls >= _expectedCalls
						, String.Format(CultureInfo.CurrentCulture, Resources.Message_ExpectedCountInvalid, this.name)
						);
					_isVerified = true;
				}
			}
		}

		private Int32 _expectedCalls;
		private Int32 _actualCalls;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExpectationCounter"/> class.
		/// Sets the <see cref="P:Genetibase.NuGenMock.ExpectationCounter.Strict"/> property to <see langword="true"/>.
		/// </summary>
		/// <param name="name"></param>
		public ExpectationCounter(String name)
			: base(name)
		{
			this.Strict = true;
		}
	}
}
