/* -----------------------------------------------
 * NuGenFieldInfo.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Genetibase.Shared.Reflection
{
	/// <summary>
	/// <see cref="FieldInfo"/> wrapper.
	/// </summary>
	public sealed class NuGenFieldInfo
	{
		private FieldInfo _fieldInfo;
		private object _instance;

		/// <summary>
		/// </summary>
		/// <typeparam name="F"></typeparam>
		/// <returns></returns>
		public F GetValue<F>()
		{
			return (F)this.GetValueInternal();
		}

		/// <summary>
		/// </summary>
		public NuGenFields Fields
		{
			get
			{
				return new NuGenFields(this.GetValueInternal());
			}
		}

		/// <summary>
		/// </summary>
		public NuGenEvents Events
		{
			get
			{
				return new NuGenEvents(this.GetValueInternal());
			}
		}

		/// <summary>
		/// </summary>
		public NuGenMethods Methods
		{
			get
			{
				return new NuGenMethods(this.GetValueInternal());
			}
		}

		/// <summary>
		/// </summary>
		public NuGenProperties Properties
		{
			get
			{
				return new NuGenProperties(this.GetValueInternal());
			}
		}

		/// <summary>
		/// Returns the value of the associated field for the associated object.
		/// </summary>
		/// <returns></returns>
		private object GetValueInternal()
		{
			return _fieldInfo.GetValue(_instance);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFieldInfo"/> class.
		/// </summary>
		/// <param name="fieldInfo"></param>
		/// <param name="instance"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="fieldInfo"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="instance"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenFieldInfo(FieldInfo fieldInfo, object instance)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}

			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}


			_fieldInfo = fieldInfo;
			_instance = instance;
		}
	}
}
