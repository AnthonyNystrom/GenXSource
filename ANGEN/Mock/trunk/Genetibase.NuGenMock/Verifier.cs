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
using System.Collections;
using System.Reflection;

namespace Genetibase.NuGenMock
{
	/// <summary>
	/// Verifies objects that implement the <see cref="IVerifiable"/> interface.
	/// </summary>
	public sealed class Verifier
	{
		/// <summary>
		/// Gets BindingFlags to determine which fields to call verify on.
		/// </summary>
		private static readonly BindingFlags _bindingFlags = 0
			| BindingFlags.DeclaredOnly
			| BindingFlags.Public
			| BindingFlags.NonPublic
			| BindingFlags.Instance
			| BindingFlags.Static
			;

		/// <summary>
		/// Keeps a list of objects that have been verified
		/// </summary>
		private static IList _verifiedObjects = new ArrayList();

		/// <summary>
		/// Calls Verify() with the object and it's type.
		/// </summary>
		/// <param name="verifiableObject">Object to verify</param>
		public static void Verify(Object verifiableObject)
		{
			if (!_verifiedObjects.Contains(verifiableObject.GetHashCode()))
			{
				verify(verifiableObject, verifiableObject.GetType());
			}
		}

		/// <summary>
		/// Resets the list of verfied objects.  Currently this needs to be run whenever two or more Verify() root
		/// calls are made in a row.  Otherwise, unpredictable results will occur.  Typically, this should be called 
		/// in a Test fixtures TearDown method.
		/// </summary>
		public static void ResetVerifier()
		{
			_verifiedObjects.Clear();
		}

		/// <summary>
		/// Iterates through an object until it's at the root type.  
		/// Then it collects all the fields according to the BindingFlags and calls verify() on
		/// each field.
		/// </summary>
		/// <param name="verifiableObject">Object to verify.</param>
		/// <param name="currentType">Current Type.</param>
		private static void verify(Object verifiableObject, Type currentType)
		{
			if (isRootType(currentType))
			{
				return;
			}

			verify(verifiableObject, currentType.BaseType);
			FieldInfo[] fields = currentType.GetFields(_bindingFlags);

			foreach (FieldInfo field in fields)
			{
				verifyField(field, verifiableObject);
			}

			_verifiedObjects.Add(verifiableObject.GetHashCode());
		}

		/// <summary>
		/// Sets the object to the Verifiable interface, then calls verify().
		/// </summary>
		private static void verifyField(FieldInfo verifiableField, Object verifiableObject)
		{
			Object fieldValue = verifiableField.GetValue(verifiableObject);
			IVerifiable aVerifiable;

			if ((aVerifiable = fieldValue as IVerifiable) != null)
			{
				if (!aVerifiable.IsVerified)
				{
					aVerifiable.Verify();
				}
			}
		}

		/// <summary>
		/// Determines if the given type is <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="aType">Type to use.</param>
		private static bool isRootType(Type aType)
		{
			return (typeof(Object).Equals(aType));
		}
	}
}
