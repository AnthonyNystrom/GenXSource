/* -----------------------------------------------
 * NuGenTargetEventArgs.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Diagnostics;

namespace Genetibase.Shared
{
	/// <summary>
	/// Contains target specific data.
	/// </summary>
	public class NuGenTargetEventArgs : EventArgs
	{
		#region Properties.Public

		/*
		 * Target
		 */

		/// <summary>
		/// Determines the target.
		/// </summary>
		private object _target = null;

		/// <summary>
		/// Gets the target.
		/// </summary>
		public object Target
		{
			[DebuggerStepThrough]
			get 
			{
				return _target;
			}
		}

		/*
		 * TargetData
		 */

		/// <summary>
		/// Determines the target specific data.
		/// </summary>
		private object _targetData = null;

		/// <summary>
		/// Gets the target specific data.
		/// </summary>
		public object TargetData
		{
			[DebuggerStepThrough]
			get 
			{
				return _targetData;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTargetEventArgs"/> class.
		/// </summary>
		public NuGenTargetEventArgs(object target, object targetData)
		{
			_target = target;
			_targetData = targetData;
		}

		#endregion
	}
}
