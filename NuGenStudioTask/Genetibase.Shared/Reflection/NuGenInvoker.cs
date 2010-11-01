/* -----------------------------------------------
 * NuGenInvoker.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Diagnostics;
using System.Reflection;

namespace Genetibase.Shared.Reflection
{
	/// <summary>
	/// Simplifies using of Reflection.
	/// </summary>
	public static class NuGenInvoker
	{
		#region Declarations

		/// <summary>
		/// All members with any access modifiers.
		/// </summary>
		private static readonly BindingFlags FULL_ACCESS = 0
			| BindingFlags.Instance
			| BindingFlags.NonPublic
			| BindingFlags.Public
			| BindingFlags.Static
			;

		#endregion

		#region Methods.Public.Static
		
		/*
		 * FindMember
		 */

		/// <summary>
		/// Retrieves the <see cref="T:System.Reflection.MethodInfo"/> for the specified method with the
		/// specified signature and access modifiers on the specified object.
		/// </summary>
		/// <param name="obj">Specifies the object that contains the method  specified by the methodName.</param>
		/// <param name="methodName">Specifies the method name.</param>
		/// <param name="signature">Specifies the method signature. <see langword="null"/> to look for a
		/// parameterless method.</param>
		/// <param name="flags">Specifies the method access modifiers.</param>
		/// <returns><see langword="null"/> if the specified object does not contain any methods with the
		/// specified parameters.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para>
		///		<paramref name="obj"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="methodName"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="methodName"/> is an emtpy string.
		/// </para>
		/// </exception>
		public static MethodInfo FindMember(object obj, string methodName, Type[] signature, BindingFlags flags)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			if (string.IsNullOrEmpty(methodName))
			{
				throw new ArgumentNullException("methodName");
			}

			MethodInfo[] methods = (MethodInfo[])FindMembers(obj, methodName, MemberTypes.Method, flags);

			foreach (MethodInfo method in methods)
			{
				if (CompareSignature(signature, method.GetParameters()))
				{
					return method;
				}
			}

			return null;
		}

		/*
		 * FindMember
		 */

		/// <summary>
		/// Retrieves the <see cref="T:System.Reflection.MemberInfo"/> for the specified member with the specified access modifiers
		/// on the specified object.
		/// </summary>
		/// <param name="obj">Specifies the object that contains the member specified by the <paramref name="memberName"/>.</param>
		/// <param name="memberName">Specifies the member name.</param>
		/// <param name="members">Specifies the member type.</param>
		/// <param name="flags">Specifies the member access modifiers.</param>
		/// <returns><see langword="null"/> if the specified object does not contain the specified member with
		/// the specified parameters.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para>
		///		<paramref name="obj"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="memberName"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="memberName"/> is an emtpy string.
		/// </para>
		/// </exception>
		public static MemberInfo FindMember(object obj, string memberName, MemberTypes members, BindingFlags flags)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			if (string.IsNullOrEmpty(memberName))
			{
				throw new ArgumentNullException("memberName");
			}

			MemberInfo[] memberInfo = FindMembers(obj, memberName, members, flags);
			return null != memberInfo && 1 == memberInfo.Length ? memberInfo[0] : null;
		}

		/*
		 * FindMember
		 */

		/// <summary>
		/// Retrieves the <see cref="T:System.Reflection.MethodInfo"/> for the specified member with the specified access modifiers
		/// on the specified type.
		/// </summary>
		/// <param name="type">Specifies the type that contains the member specified by the <paramref name="memberName"/>.</param>
		/// <param name="memberName">Specifies the member name.</param>
		/// <param name="members">Specifies the member type.</param>
		/// <param name="flags">Specifies the member access modifier.</param>
		/// <returns><see langword="null"/> if the specified type does not contain the specified member with
		/// the specified paramters.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para><paramref name="type"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="memberName"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="memberName"/> is an empty string.</para>
		/// </exception>
		public static MemberInfo FindMember(Type type, string memberName, MemberTypes members, BindingFlags flags)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			if (string.IsNullOrEmpty(memberName))
			{
				throw new ArgumentNullException("memberName");
			}

			MemberInfo[] memberInfo = FindMembers(type, memberName, members, flags);
			return null != memberInfo && 1 == memberInfo.Length ? memberInfo[0] : null;
		}

		/*
		 * FindProperty
		 */

		/// <summary>
		/// Retrieves the <see cref="T:System.Reflection.PropertyInfo"/> for the specified property on the
		/// specified object.
		/// </summary>
		/// <param name="obj">Specifies the object that contains the property specified by the <paramref name="memberName"/>.</param>
		/// <param name="memberName">Specifies the member name.</param>
		/// <param name="flags">Specifies the member access modifiers.</param>
		/// <returns><see langword="null"/> if the specified object does not contain the specified property.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para><paramref name="obj"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="memberName"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="memberName"/> is an empty string.</para>
		/// </exception>
		public static PropertyInfo FindProperty(object obj, string memberName, BindingFlags flags)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			if (string.IsNullOrEmpty(memberName))
			{
				throw new ArgumentNullException("memberName");
			}

			return (PropertyInfo)NuGenInvoker.FindMember(obj, memberName, MemberTypes.Property, flags);
		}

		/*
		 * GetProperty
		 */

		/// <summary>
		/// Gets the value of the specified property on the specified object.
		/// </summary>
		/// <param name="obj">Specifies the object to get the property on.</param>
		/// <param name="propertyName">Specifies the property to get the value of.</param>
		/// <returns>Specifies property value.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para>
		///		<paramref name="obj"/> is <see langword="null"/>.
		///	</para>
		/// -or-
		/// <para>
		///		<paramref name="propertyName"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public static object GetProperty(object obj, string propertyName)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			if (string.IsNullOrEmpty(propertyName))
			{
				throw new ArgumentNullException("propertyName");
			}

			return NuGenInvoker.FindProperty(obj, propertyName, NuGenInvoker.FULL_ACCESS).GetValue(obj, null);
		}

		/*
		 * GetProperty
		 */

		/// <summary>
		/// Gets the value of the specified property on the specified object.
		/// </summary>
		/// <param name="obj">Specifies the object to get the property on.</param>
		/// <param name="flags">Specifies the property access modifiers.</param>
		/// <param name="propertyName">Specifies the property to get the value of.</param>
		/// <returns>Specifies property value.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para>
		///		<paramref name="obj"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="propertyName"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="propertyName"/> is an empty string.
		/// </para>
		/// </exception>
		public static object GetProperty(object obj, BindingFlags flags, string propertyName)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			if (string.IsNullOrEmpty(propertyName))
			{
				throw new ArgumentNullException("propertyName");
			}

			return NuGenInvoker.FindProperty(obj, propertyName, flags).GetValue(obj, null);
		}

		/*
		 * InvokeMethod
		 */
		
		/// <summary>
		/// Invokes the specified method with the specified parameters on the specified object.
		/// </summary>
		/// <param name="obj">Specifies the object to invoke the method on.</param>
		/// <param name="methodName">Specifies the method name to invoke.</param>
		/// <param name="args">Specifies optional parameters to pass to the method being invoked.</param>
		/// <returns><see langword="null"/> if the invoked method does not return any value.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="obj"/> is <see langword="null"/>
		/// -or-
		/// <paramref name="methodName"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="methodName"/> is an empty string.
		/// </exception>
		/// <exception cref="T:System.NullReferenceException">Specified method was not found.</exception>
		/// <exception cref="T:System.Reflection.TargetParameterCountException">
		/// Passed parameters count does not match the invoked method parameters count.
		/// </exception>
		/// <exception cref="T:System.Reflection.AmbiguousMatchException">
		/// Specified method has several overloads.
		/// </exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// <paramref name="obj"/> does not contain the method with the specified parameters. Try using the
		/// <see cref="M:Genetibase.Shared.Reflection.NuGenInvoker.InvokeMethodWithSignature"/> method instead.
		/// </exception>
		public static object InvokeMethod(object obj, string methodName, params object[] args)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			if (string.IsNullOrEmpty(methodName))
			{
				throw new ArgumentNullException("methodName");
			}

			MethodInfo method = (MethodInfo)FindMember(obj, methodName, MemberTypes.Method, FULL_ACCESS);

			if (method == null)
			{
				throw new InvalidOperationException(Properties.Resources.InvalidOperation_MethodInvokationFailed);
			}

			return method.Invoke(obj, args);
		}

		/*
		 * InvokeMethod
		 */

		/// <summary>
		/// Invokes the specified method with the specified parameters and access modifiers on the specified object.
		/// </summary>
		/// <param name="obj">Specifies the object that contains the method specified by the methodName.</param>
		/// <param name="methodName">Specifies the method name.</param>
		/// <param name="flags">Specifies the method access modifiers.</param>
		/// <param name="args">Specifies optional parameters to pass to the method being invoked.</param>
		/// <returns><see langword="null"/> if the invoked method does not return any value.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para>
		///		<paramref name="obj"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="methodName"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="methodName"/> is an emtpy string.
		/// </para>
		/// </exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="methodName"/> is an empty string.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// Specified object does not contain the method with the specified parameters. Try using
		/// the <see cref="M:NuGenInvoker.InvokeMethodWithSignature"/> method instead.
		/// </exception>
		public static object InvokeMethod(object obj, string methodName, BindingFlags flags, params object[] args)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			if (string.IsNullOrEmpty(methodName))
			{
				throw new ArgumentNullException("methodName");
			}

			MethodInfo method = (MethodInfo)FindMember(obj, methodName, MemberTypes.Method, flags);

			if (method == null)
			{
				throw new InvalidOperationException(Properties.Resources.InvalidOperation_MethodInvokationFailed);
			}

			return method.Invoke(obj, args);
		}

		/*
		 * InvokeStaticMethod
		 */

		/// <summary>
		/// Invokes the specified method with the specified parameters on the specified type.
		/// </summary>
		/// <param name="type">Specifies the type that contains the method specified by the methodName.</param>
		/// <param name="methodName">Specifies the method name.</param>
		/// <param name="args">Specifies optional parameters to pass to the method being invoked.</param>
		/// <returns><see langword="null"/> if the invoked method does not return any value.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para>
		///		<paramref name="type"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="methodName"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="methodName"/> is an empty string.
		/// </para>
		/// </exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="methodName"/> is an empty string.</exception>
		public static object InvokeStaticMethod(Type type, string methodName, params object[] args)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			if (string.IsNullOrEmpty(methodName))
			{
				throw new ArgumentNullException("methodName");
			}

			MethodInfo method = (MethodInfo)FindMember(type, methodName, MemberTypes.Method, FULL_ACCESS);

			if (method == null)
			{
				throw new InvalidOperationException(Properties.Resources.InvalidOperation_MethodInvokationFailed);
			}

			return method.Invoke(null, args);
		}

		/*
		 * InvokeMethodWithSignature
		 */

		/// <summary>
		/// Invokes the specified method with the specified parameters and signature on the specified object.
		/// </summary>
		/// <param name="obj">Specifies the object that contains the method specified by the methodName.</param>
		/// <param name="methodName">Specifies the method name.</param>
		/// <param name="signature">Specifies the method signature.</param>
		/// <param name="args">Specifies optional parameters to pass to the method being invoked.</param>
		/// <returns><see langword="null"/> if the invoked method does not return any value.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		/// <paramref name="obj"/> does not contain the method with the specified parameters.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para>
		///		<paramref name="obj"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="methodName"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="methodName"/> is an empty string.
		/// </para>
		/// </exception>
		public static object InvokeMethodWithSignature(object obj, string methodName, Type[] signature, params object[] args)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			if (string.IsNullOrEmpty(methodName))
			{
				throw new ArgumentNullException("methodName");
			}

			MethodInfo method = FindMember(obj, methodName, signature, FULL_ACCESS);

			if (method == null)
			{
				throw new InvalidOperationException(Properties.Resources.InvalidOperation_MethodInvokationFailed);
			}
			
			return method.Invoke(obj, args);
		}

		/*
		 * SetProperty
		 */

		/// <summary>
		/// Sets the specified value to the property with the specified name on the specified object.
		/// </summary>
		/// <param name="obj">Specifies the object to set the property on.</param>
		/// <param name="propertyName">Specifies the name of the property to set.</param>
		/// <param name="value">Specifies the value to set for the property.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para>
		///		<paramref name="obj"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="propertyName"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="propertyName"/> is an empty string.
		/// </para>
		/// </exception>
		public static void SetProperty(object obj, string propertyName, object value)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			if (string.IsNullOrEmpty(propertyName))
			{
				throw new ArgumentNullException("propertyName");
			}

			NuGenInvoker.FindProperty(obj, propertyName, NuGenInvoker.FULL_ACCESS).SetValue(obj, value, null);
		}

		/*
		 * SetProperty
		 */

		/// <summary>
		/// Sets the specified value to the property with the specified name on the specified object.
		/// </summary>
		/// <param name="obj">Specifies the object to set the property on.</param>
		/// <param name="flags">Specifies the property access modifiers.</param>
		/// <param name="propertyName">Specifies the name of the property to set.</param>
		/// <param name="value">Specifies the value to set for the property.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para>
		///		<paramref name="obj"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="propertyName"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="propertyName"/> is an empty string.
		/// </para>
		/// </exception>
		public static void SetProperty(object obj, BindingFlags flags, string propertyName, object value)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			if (string.IsNullOrEmpty("propertyName"))
			{
				throw new ArgumentNullException("propertyName");
			}

			NuGenInvoker.FindProperty(obj, propertyName, flags).SetValue(obj, value, null);
		}

		#endregion

		#region Methods.Private.Static

		/// <summary>
		/// Compares the specified signature with the retrieved method parameter list.
		/// </summary>
		/// <param name="signature">Specifies the signature to compare with.</param>
		/// <param name="parameters">Specifies the retrieved method parameter list.</param>
		/// <returns><see langword="true"/> if the specified signature and parameters are equal; otherwise,
		/// <see langword="false"/>.</returns>
		private static bool CompareSignature(Type[] signature, ParameterInfo[] parameters)
		{
			if (signature == null && (parameters == null || parameters.Length == 0))
			{
				return true;
			}
			
			if (signature == null || parameters == null)
			{
				return false;
			}
			
			if (signature.Length != parameters.Length)
			{
				return false;
			}

			for (int i = 0; i < signature.Length; i++)
			{
				if (signature[i] != parameters[i].ParameterType)
					return false;
			}

			return true;
		}

		/// <summary>
		/// Retrieves an array of <see cref="MemberInfo"/> object for the specified member with the specified
		/// access modifiers on the specified object.
		/// </summary>
		/// <param name="obj">Specifies the object that contains the member specified by the memberName.</param>
		/// <param name="memberName">Specifies the member name.</param>
		/// <param name="members">Specifies the member type.</param>
		/// <param name="flags">Specifies the member access modifiers.</param>
		/// <returns><see langword="null"/> if the specified object does not contain the specified member with
		/// the specified parameters.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para>
		///		<paramref name="obj"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="memberName"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="memberName"/> is an empty string.
		/// </para>
		/// </exception>
		private static MemberInfo[] FindMembers(object obj, string memberName, MemberTypes members, BindingFlags flags)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			if (string.IsNullOrEmpty(memberName))
			{
				throw new ArgumentNullException("memberName");
			}

			return FindMembers(obj.GetType(), memberName, members, flags);
		}

		/// <summary>
		/// Retrieves an array of <see cref="MemberInfo"/> objects for the specified member with the specified
		/// access modifiers on the specified type.
		/// </summary>
		/// <param name="type">Specifies the type that contains the member specified by the memberName.</param>
		/// <param name="memberName">Specifies the member name.</param>
		/// <param name="members">Specifies the member type.</param>
		/// <param name="flags">Specifies the member access modifiers.</param>
		/// <returns><see langword="null"/> if the specified type does not contain the specified member with
		/// the specified parameters.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para>
		///		<paramref name="type"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="memberName"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="memberName"/> is an empty string.
		/// </para>
		/// </exception>
		private static MemberInfo[] FindMembers(Type type, string memberName, MemberTypes members, BindingFlags flags)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			if (string.IsNullOrEmpty(memberName))
			{
				throw new ArgumentNullException("memberName");
			}

			MemberInfo[] memberInfo = null;

			while (true)
			{
				memberInfo = type.GetMember(memberName, members, flags);

				if (null != memberInfo && 0 < memberInfo.Length) 
				{
					break;
				}

				type = type.BaseType;

				if (null == type)
				{
					break;
				}
			}

			return memberInfo;
		}

		#endregion
	}
}
