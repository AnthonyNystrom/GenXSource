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

namespace Genetibase.NuGenMock
{
	/// <summary>
	/// Abstract class that implements the <see cref="IExpectation"/> interface.
	/// </summary>
	/// <remarks/>
	public abstract class AbstractExpectation : IExpectation
	{
		/// <summary>
		/// Verifys current object and all <see cref="MockObject"/> fields within.
		/// </summary>
		public abstract void Verify();

		private Boolean _hasExpectations;

		/// <summary>
		/// Gets or sets the value indicating whether the object has expectations.
		/// </summary>
		public Boolean HasExpectations
		{
			get
			{
				return _hasExpectations;
			}
			set
			{
				_hasExpectations = value;
			}
		}

		private Boolean _verifyImmediate;

		/// <summary>
		/// Gets or sets the value indicating whether the object should be verified immediately.
		/// </summary>
		public Boolean VerifyImmediate
		{
			get
			{
				return _verifyImmediate;
			}
			set
			{
				_verifyImmediate = value;
			}
		}

		/// <summary>
		/// Gets the value indicating whether the object should verify itself immediately.
		/// </summary>
		public Boolean ShouldCheckImmediately
		{
			get
			{
				return _verifyImmediate && _hasExpectations;
			}
		}

		private Boolean _isStrict;

		/// <summary>
		/// Gets or sets the value indicating whether the expectation should be strict or not.
		/// This means that as long as expectations are met, any other object state will be ignored.
		/// If this is true, only the set expectations will be allowed.
		/// </summary>
		public Boolean Strict
		{
			get
			{
				return _isStrict;
			}
			set
			{
				_isStrict = value;
			}
		}

		/// <summary>
		/// Gets the value indicating whether this object has been verified.
		/// </summary>
		public abstract Boolean IsVerified
		{
			get;
		}
	}
}
