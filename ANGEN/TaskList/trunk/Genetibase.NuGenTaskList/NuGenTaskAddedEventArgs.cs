/* -----------------------------------------------
 * NuGenTaskAddedEventArgs.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// </summary>
	public class NuGenTaskAddedEventArgs : NuGenSelectedTaskChangedEventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTaskAddedEventArgs"/> class.
		/// </summary>
		public NuGenTaskAddedEventArgs(string taskText)
			: base(taskText)
		{
		}
	}
}
