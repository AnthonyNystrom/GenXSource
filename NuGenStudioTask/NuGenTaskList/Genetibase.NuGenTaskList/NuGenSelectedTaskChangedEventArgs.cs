/* -----------------------------------------------
 * NuGenSelectedTaskChangedEventArgs.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using System;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// </summary>
	public class NuGenSelectedTaskChangedEventArgs : INuGenDEHEventArgs
	{
		#region Properties.Public

		/*
		 * IsTaskTextReadonly
		 */

		private bool _IsTaskTextReadonly = false;

		/// <summary>
		/// </summary>
		public bool IsTaskTextReadonly
		{
			get
			{
				return _IsTaskTextReadonly;
			}
		}

		/*
		 * TaskText
		 */

		private string _TaskText = "";

		/// <summary>
		/// </summary>
		public string TaskText
		{
			get
			{
				return _TaskText;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSelectedTaskChangedEventArgs"/> class.
		/// </summary>
		public NuGenSelectedTaskChangedEventArgs(string taskText, bool isTaskTextReadonly)
		{
			_TaskText = taskText;
			_IsTaskTextReadonly = isTaskTextReadonly;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSelectedTaskChangedEventArgs"/> class.
		/// </summary>
		public NuGenSelectedTaskChangedEventArgs(string taskText)
			: this(taskText, false)
		{
		}

		#endregion
	}
}
