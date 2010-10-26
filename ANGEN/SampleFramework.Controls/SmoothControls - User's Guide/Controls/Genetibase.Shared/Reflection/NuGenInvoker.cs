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
	/// Provides functionality to operate Reflection information easily.
	/// </summary>
	public static class NuGenInvoker<T>
	{
		/// <summary>
		/// </summary>
		public static NuGenMethods Methods
		{
			get
			{
				return new NuGenMethods(typeof(T));
			}
		}
	}

	/// <summary>
	/// Provides functionality to operate Reflection information easily.
	/// </summary>
	public sealed class NuGenInvoker
	{
		/// <summary>
		/// </summary>
		public NuGenEvents Events
		{
			get
			{
				return new NuGenEvents(_instance);
			}
		}


		/// <summary>
		/// </summary>
		public NuGenFields Fields
		{
			get
			{
				return new NuGenFields(_instance);
			}
		}

		/// <summary>
		/// </summary>
		public NuGenMethods Methods
		{
			get
			{
				return new NuGenMethods(_instance);
			}
		}

		/// <summary>
		/// </summary>
		public NuGenProperties Properties
		{
			get
			{
				return new NuGenProperties(_instance);
			}
		}

		private Object _instance;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenInvoker"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="instance"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenInvoker(Object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}

			_instance = instance;
		}
	}
}
