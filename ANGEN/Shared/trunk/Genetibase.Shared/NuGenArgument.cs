/* -----------------------------------------------
 * NuGenArguent.cs
 * Copyright � 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Genetibase.Shared.Reflection;

namespace Genetibase.Shared
{
	/// <summary>
	/// Provides methods for argument processing.
	/// </summary>
	public static class NuGenArgument
	{
		/*
		 * Exchange
		 */

		/// <summary>
		/// </summary>
		public static void Exchange<T>(ref T x, ref T y)
		{
			T buffer = x;
			x = y;
			y = buffer;
		}

		/*
		 * IsCompatibleType
		 */

		/// <summary>
		/// Indicates whether the type of the <paramref name="argument"/> is compatible with the 
		/// <paramref name="expectedType"/>. If <paramref name="argument"/> is <see langword="null"/>,
		/// <see langword="false"/> is returned despite the type specified by <paramref name="expectedType"/>.
		/// But if <paramref name="expectedType"/> is <see langword="null"/> an
		/// <see cref="T:System.ArgumentNullException"/> is thrown.
		/// </summary>
		/// 
		/// <param name="argument">Specifies the argument to check.</param>
		/// <param name="expectedType">Specifies the type that the passed <paramref name="argument"/> should
		/// be compatible with.</param>
		/// 
		/// <returns><see langword="true"/> if the type of the specified <paramref name="argument"/> is 
		/// compatible with <paramref name="expectedType"/>; otherwise, <see langword="false"/>.</returns>
		/// 
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="expectedType"/> is <see langword="null"/>.
		/// </exception>
		public static Boolean IsCompatibleType(Object argument, Type expectedType)
		{
			if (expectedType == null)
			{
				throw new ArgumentNullException("expectedType");
			}

			if (argument != null)
			{
				if (NuGenArgument.IsCompatibleType(argument.GetType(), expectedType))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Indicates whether <paramref name="actualType"/> is compatible with <paramref name="expectedType"/>.
		/// If <paramref name="actualType"/> is <see langword="null"/> <see langword="false"/> is returned 
		/// despite the type specified by <paramref name="compatibleType"/>. But if <paramref name="compatibleType"/>
		/// is <see langword="null"/> an <see cref="T:System.ArgumentNullException"/> is thrown.
		/// </summary>
		/// 
		/// <param name="actualType">Specifies the type to check.</param>
		/// <param name="compatibleType">Specifies the type <paramref name="actualType"/> should be compatible with.</param>
		/// 
		/// <returns><see langword="true"/> if the specified <paramref name="actualType"/> is compatible with the
		/// specified <paramref name="compatibleType"/>; otherwise, <see langword="false"/>.</returns>
		/// 
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="compatibleType"/> is <see langword="null"/>.
		/// </exception>
		public static Boolean IsCompatibleType(Type actualType, Type compatibleType)
		{
			if (compatibleType == null)
			{
				throw new ArgumentNullException("compatibleType");
			}

			if (actualType != null)
			{
				if (compatibleType.Equals(actualType)
					|| actualType.IsSubclassOf(compatibleType)
					|| compatibleType.IsAssignableFrom(actualType)
					)
				{
					return true;
				}
			}

			return false;
		}

		/*
		 * IsEven
		 */

		/// <summary>
		/// Determines whether the specified value is even.
		/// </summary>
		/// <param name="value">Specifies the value to check.</param>
		/// <returns>If the specified value is even, the return value is <see langword="true"/>; otherwise,
		/// the return value is <see langword="false"/>.</returns>
		[DebuggerStepThrough()]
		public static Boolean IsEven(Int32 value)
		{
			return (value & 0x1) == 0;
		}

		/*
		 * IsOdd
		 */

		/// <summary>
		/// Determines whether the specified value is odd.
		/// </summary>
		/// <param name="value">Specifies the value to check.</param>
		/// <returns>If the specified value is odd, the return value is <see langword="true"/>; otherwise,
		/// the return value is <see langword="false"/>.</returns>
		[DebuggerStepThrough()]
		public static Boolean IsOdd(Int32 value)
		{
			return !IsEven(value);
		}

		/*
		 * IsValidDirectoryName
		 */

		/// <summary>
		/// Determines whether the specified directory name is valid.
		/// </summary>
		/// <param name="directoryName">Specifies the directory name to check.</param>
		/// <returns><see langword="true"/> if the specified directory name is valid; otherwise, <see langword="false"/>.</returns>
		public static Boolean IsValidDirectoryName(String directoryName)
		{
			if (String.IsNullOrEmpty(directoryName))
			{
				return false;
			}

			if (directoryName.IndexOfAny(Path.GetInvalidPathChars()) > -1)
			{
				return false;
			}

			return true;
		}

		/*
		 * IsValidFileName
		 */

		/// <summary>
		/// Determines whether the specified filename is valid.
		/// </summary>
		/// <param name="fileName">Specifies the filename to check.</param>
		/// <returns><see langword="true"/> if the specified filename is valid; otherwise, <see langword="false"/>.</returns>
		public static Boolean IsValidFileName(String fileName)
		{
			if (String.IsNullOrEmpty(fileName))
			{
				return false;
			}

			if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) > -1)
			{
				return false;
			}

			return true;
		}
	}
}
