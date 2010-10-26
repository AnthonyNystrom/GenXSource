using System;
using System.Reflection;
using System.Collections;
	
namespace DotNetMock
{
	/// <summary>
	/// Verifies objects that implement the IVerifiable interface.
	/// </summary>
	public sealed class Verifier
	{
		/// <summary>
		/// Gets BindingFlags to determine which fields to call verify on.
		/// </summary>
		private static readonly BindingFlags _bindingFlags = 
			(
			BindingFlags.DeclaredOnly
			| BindingFlags.Public
			| BindingFlags.NonPublic
			| BindingFlags.Instance
			| BindingFlags.Static
			);
		/// <summary>
		/// Keeps a list of objects that have been verified
		/// </summary>
		private static IList _verifiedObjects = new ArrayList();
		#region Static

		/// <summary>
		/// Calls Verify() with the object and it's type.
		/// </summary>
		/// <param name="verifiableObject">Object to verify</param>
		public static void Verify(Object verifiableObject) 
		{
			if ( !_verifiedObjects.Contains( verifiableObject.GetHashCode() ) )
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
			_verifiedObjects.Add( verifiableObject.GetHashCode() );
		}
		/// <summary>
		/// Sets the object to the Verifiable interface, then calls verify().
		/// </summary>
		/// <param name="verifiableField">Current Field.</param>
		/// <param name="verifiableObject">Current Object.</param>
		private static void verifyField(FieldInfo verifiableField, Object verifiableObject)
		{
			Object fieldValue = verifiableField.GetValue(verifiableObject);
			
			IVerifiable aVerifiable;
			if ((aVerifiable = fieldValue as IVerifiable) != null)
			{
				if ( ! aVerifiable.IsVerified ) 
				{
					aVerifiable.Verify();
				}
			}
		}
		/// <summary>
		/// Determines if the given type is System.Object.
		/// </summary>
		/// <param name="aType">Type to use.</param>
		/// <returns>True/False.</returns>
		private static  bool isRootType(Type aType)
		{
			return (typeof(Object).Equals(aType));
		}
		#endregion
	}
}
