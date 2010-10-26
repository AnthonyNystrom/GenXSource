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

namespace Genetibase.NuGenMock
{
	/// <summary>
	/// Interface for all collection based expectations. Implements the <see cref="IExpectation"/> interface.
	/// </summary>
	public interface IListExpectation<T> : IExpectation
	{
		/// <summary>
		/// Adds object to actual collection.
		/// </summary>
		/// <param name="actual">Item to add.</param>
		void AddActual(T actual);
		
		/// <summary>
		/// Adds an array of objects to the actual collection.
		/// </summary>
		/// <param name="actualMany">Array of items to add.</param>
		void AddActualMany(T[] actualMany);
		
		/// <summary>
		/// Adds a collection that implements the <see cref="T:System.Collections.Generic.IEnumerable`1"/> interface to the actual collection.
		/// </summary>
		/// <param name="actualMany">Enumerator full of items to add to the actual collection.</param>
		void AddActualMany(IEnumerable<T> actualMany);
		
		/// <summary>
		/// Adds the elements of an object that implements the <see cref="T:System.Collections.Generic.IList`1"/> interface to the actual collection.
		/// </summary>
		/// <param name="actualMany">List of items to add to the actual collection.</param>
		void AddActualMany(IList<T> actualMany);
		
		/// <summary>
		/// Adds an object to the expected collection.
		/// </summary>
		/// <param name="expected">Item to add to the expected collection.</param>
		void AddExpected(T expected);
		
		/// <summary>
		/// Adds an array of objects to the expected collection.
		/// </summary>
		/// <param name="expectedMany">Items to add to the expected collection.</param>
		void AddExpectedMany(T[] expectedMany);
		
		/// <summary>
		/// Adds a collection that implements the <see cref="T:System.Collections.Generic.IEnumerable`1"/> interface to the expected collection.
		/// </summary>
		/// <param name="expectedMany">Enumerator full of objects to add to the expected collection.</param>
		void AddExpectedMany(IEnumerable<T> expectedMany);
		
		/// <summary>
		/// Adds the elements of an object that implements the <see cref="T:System.Collections.Generic.IList`1"/> interface to the expected collection.
		/// </summary>
		/// <param name="expectedMany"><see cref="T:System.Collections.Generic.IList`1"/> to add to the expected collection.</param>
		void AddExpectedMany(IList<T> expectedMany);
	}
}
