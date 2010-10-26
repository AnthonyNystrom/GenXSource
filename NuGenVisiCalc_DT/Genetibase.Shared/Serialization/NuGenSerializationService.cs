/* -----------------------------------------------
 * NuGenSerializationService.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace Genetibase.Shared.Serialization
{
	/// <summary>
	/// If the base class is not marked with the <see cref="SerializableAttribute"/> attribute, implement
	/// the <see cref="T:System.Runtime.Serialization.ISerializable"/> interface in the subclass and 
	/// perform serialization/deserialization with the methods provided by the
	/// <see cref="NuGenSerializationService"/> class.
	/// </summary>
	public static class NuGenSerializationService
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

		#region Methods.Public.Static.Deserialize

		/// <summary>
		/// Initializes the specified target from the stream.
		/// </summary>
		/// <param name="target">Specifies the target to initialize from the stream.</param>
		/// <param name="targetType">Specifies the type of the target to initialize from the stream.</param>
		/// <param name="serializationInfo">Specifies the data needed to serialize the target.</param>
		/// <param name="streamingContext">Specifies the source and destination of a given serialized stream,
		/// as well as means for serialization to retain that context and an additional caller-defined context.</param>
		/// <exception cref="T:System.ArgumentException">
		/// <para>
		///		<paramref name="target"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="targetType"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="serializationInfo"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public static void Deserialize(
			object target,
			Type targetType,
			SerializationInfo serializationInfo,
			StreamingContext streamingContext
			)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}

			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}

			if (serializationInfo == null)
			{
				throw new ArgumentNullException("serializationInfo");
			}

			if (targetType == typeof(object))
			{
				return;
			}

			FieldInfo[] fields = targetType.GetFields(FULL_ACCESS);

			foreach (FieldInfo field in fields)
			{
				if (field.IsNotSerialized)
				{
					continue;
				}

				object fieldValue = serializationInfo.GetValue(
					string.Format(CultureInfo.InvariantCulture, "{0} + {1}", targetType.Name, field.Name),
					field.FieldType
					);

				field.SetValue(
					target,
					fieldValue
					);
			}

			Deserialize(
				target,
				targetType.BaseType,
				serializationInfo,
				streamingContext
				);
		}

		#endregion

		#region Methods.Public.Static.Serialize

		/// <summary>
		/// Serializes the specified target.
		/// </summary>
		/// <param name="target">Specifies the target to serialize.</param>
		/// <param name="targetType">Specifies the type of the target to serialize.</param>
		/// <param name="serializationInfo">Specifies the data needed to serialize the target.</param>
		/// <param name="streamingContext">Specifies the source and destination of a given serialized stream,
		/// as well as means for serialization to retain that context and an additional caller-defined context.</param>
		/// <exception cref="T:System.ArgumentException">
		/// <para><paramref name="target"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="targetType"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="serializationInfo"/> is <see langword="null"/>.</para>
		/// </exception>
		public static void Serialize(
			object target,
			Type targetType,
			SerializationInfo serializationInfo,
			StreamingContext streamingContext
			)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}

			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}

			if (serializationInfo == null)
			{
				throw new ArgumentNullException("serializationInfo");
			}

			if (targetType == typeof(object))
			{
				return;
			}

			FieldInfo[] fields = targetType.GetFields(FULL_ACCESS);

			foreach (FieldInfo field in fields) 
			{
				if (field.IsNotSerialized) 
				{
					continue;
				}
 
				serializationInfo.AddValue(
					string.Format(CultureInfo.InvariantCulture, "{0} + {1}", targetType.Name, field.Name),
					field.GetValue(target)
					);
			}

			Serialize(
				target,
				targetType.BaseType,
				serializationInfo,
				streamingContext
				);
		}

		#endregion
	}
}
