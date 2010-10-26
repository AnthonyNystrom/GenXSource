/* -----------------------------------------------
 * NuGenEventInfo.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Genetibase.Shared.Reflection
{
	/// <summary>
	/// <see cref="EventInfo"/> wrapper.
	/// </summary>
	public sealed class NuGenEventInfo
	{
		private EventInfo _eventInfo;
		private object _instance;

		/// <summary>
		/// Adds the specified handler to the associated event of the associated instance.
		/// </summary>
		/// <param name="method"></param>
		/// <exception cref="TargetException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="MethodAccessException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public void AddHandler(Delegate method)
		{
			_eventInfo.AddEventHandler(_instance, method);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenEventInfo"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="eventInfo"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="instance"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenEventInfo(EventInfo eventInfo, object instance)
		{
			if (eventInfo == null)
			{
				throw new ArgumentNullException("eventInfo");
			}

			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}

			_instance = instance;
			_eventInfo = eventInfo;
		}
	}
}
