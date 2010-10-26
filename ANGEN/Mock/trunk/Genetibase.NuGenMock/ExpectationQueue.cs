#region License

/*
 * Copyright © 2004, 2005 Evhen Khasenevich. All rights reserved.
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
using System.Globalization;
using Genetibase.NuGenMock.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.NuGenMock
{
	/// <summary>
	/// Represents an expectation for <see cref="T:System.Collections.Generic.Queue`1"/> collections.
	/// Extends the <see cref="AbstractStaticExpectation"/> class.
	/// </summary>
	/// <author></author>
	public class ExpectationQueue<T> : AbstractStaticExpectation
	{
		private Queue<T> _actualQueue = new Queue<T>();

		/// <summary>
		/// Enqueue/Dequeue Actual items.
		/// </summary>
		public T Actual
		{
			get
			{
				if (_actualQueue.Count == 0)
				{
					return default(T);
				}

				return _actualQueue.Dequeue();
			}
			set
			{
				_actualQueue.Enqueue(value);
				_isVerified = false;

				if (this.ShouldCheckImmediately)
				{
					this.Verify();
				}
			}
		}

		private Queue<T> _expectedQueue = new Queue<T>();

		/// <summary>
		/// Enqueue/Dequeue Expected items.
		/// </summary>
		public T Expected
		{
			get
			{
				if (_expectedQueue.Count == 0)
				{
					return default(T);
				}
				
				return _expectedQueue.Dequeue();
			}
			set
			{
				_expectedQueue.Enqueue(value);
				this.HasExpectations = true;
				_isVerified = false;
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
		/// Clears Actual items.
		/// </summary>
		public override void ClearActual()
		{
			_actualQueue.Clear();
			_isVerified = false;
		}

		/// <summary>
		/// Clears Expected items.
		/// </summary>
		public override void ClearExpected()
		{
			_expectedQueue.Clear();
			this.HasExpectations = false;
			_isVerified = false;
		}

		/// <summary>
		/// </summary>
		public override void Verify()
		{
			if (this.HasExpectations)
			{
				if (this.ShouldCheckImmediately)
				{
					Assert.AreEqual(
						this.Expected
						, this.Actual
						, String.Format(CultureInfo.CurrentCulture, Resources.Message_QueueValuesNotEqual, this.name)
						);
					_isVerified = true;
				}
				else
				{
					T expected;

					while (_actualQueue.Count > 0)
					{
						expected = this.Expected;

						Assert.AreEqual(
							expected
							, this.Actual
							, String.Format(CultureInfo.CurrentCulture, Resources.Message_QueueValuesNotEqual, this.name)
							);
					}
					
					//while ((expected = this.Expected) != null)
					//{
					//    if (_actualQueue.Count > 0)
					//    {
					//        Assert.AreEqual(
					//            expected
					//            , this.Actual
					//            , String.Format(CultureInfo.CurrentCulture, Resources.Message_QueueStringValuesNotEqual, this.name)
					//            );
					//    }
					//    else
					//    {
					//        Assert.Fail(
					//            String.Format(
					//                CultureInfo.CurrentCulture
					//                , Resources.Message_QueueValuesLeftExpected
					//                , _expectedQueue.Count + 1
					//                , this.name
					//                , expected
					//                )
					//            );
					//    }
					//}

					if (_actualQueue.Count > 0)
					{
						Assert.Fail(
							String.Format(
								CultureInfo.CurrentCulture
								, Resources.Message_QueueValuesLeftActual
								, _actualQueue.Count
								, this.name
								, this.Actual
								)
							);
					}

					_isVerified = true;
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Genetibase.NuGenMock.ExpectationQueue`1"/> class.
		/// </summary>
		public ExpectationQueue(String name)
			: base(name)
		{
			this.ClearActual();
		}
	}
}
