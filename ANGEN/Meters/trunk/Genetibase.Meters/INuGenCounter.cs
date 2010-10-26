/* -----------------------------------------------
 * INuGenCounter.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Shared;

namespace Genetibase.Meters
{
	/// <summary>
	/// Indicates that this class is a counter.
	/// </summary>
	public interface INuGenCounter : IDisposable
	{
		/// <summary>
		/// Occurs when the value on the counter changes.
		/// </summary>
		event NuGenTargetEventHandler ValueChanged;

		/// <summary>
		/// Occurs when this <see cref="T:Genetibase.Shared.INuGenCounter"/> is disposed.
		/// </summary>
		event EventHandler Disposed;

		/// <summary>
		/// Obtains a counter sample and returns the calculated value for it.
		/// </summary>
		/// <returns>The next calculated value that the system obtains for this counter.</returns>
		float NextValue();

		/// <summary>
		/// Gets or sets the name of the counter category for this counter. 
		/// </summary>
		String CategoryName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the description for this counter.
		/// </summary>
		String CounterHelp
		{
			get;
		}

		/// <summary>
		/// Gets or sets the name of the counter.
		/// </summary>
		String CounterName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the format for the counter.
		/// </summary>
		String CounterFormat
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets an instance name for this counter.
		/// </summary>
		String InstanceName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the computer name for this counter.
		/// </summary>
		String MachineName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the counter's name.
		/// </summary>
		String Name
		{
			get;
			set;
		}
	}
}
