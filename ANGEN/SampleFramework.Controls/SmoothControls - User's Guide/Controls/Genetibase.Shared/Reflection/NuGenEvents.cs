/* -----------------------------------------------
 * NuGenEvents.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Genetibase.Shared.Reflection
{
	/// <summary>
	/// </summary>
	public sealed class NuGenEvents
	{
		private Object _instance;

		/// <summary>
		/// Retrieves the event with the specified name.
		/// </summary>
		/// <param name="eventName"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="eventName"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="eventName"/> is an empty string.</para>
		/// </exception>
		/// <exception cref="NuGenEventNotFoundException"/>
		public NuGenEventInfo this[String eventName]
		{
			get
			{
				if (String.IsNullOrEmpty(eventName))
				{
					throw new ArgumentNullException("eventName");
				}

				EventInfo eventInfo = MemberFinder.FindEvent(_instance.GetType(), eventName);

				if (eventInfo == null)
				{
					throw new NuGenEventNotFoundException(eventName, _instance.GetType());
				}

				return new NuGenEventInfo(
					eventInfo,
					_instance
				);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenEvents"/> class.
		/// </summary>
		/// <param name="instance"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="instance"/></para>
		/// </exception>
		public NuGenEvents(Object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}

			_instance = instance;
		}
	}
}
