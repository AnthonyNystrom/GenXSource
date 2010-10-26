/* -----------------------------------------------
 * NuGenControlStateTracker.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Supports <see cref="NuGenControlState.Normal"/>, <see cref="NuGenControlState.Focused"/>, and
	/// <see cref="NuGenControlState.Disabled"/> states.
	/// </summary>
	public class NuGenControlStateTracker : INuGenControlStateTracker
	{
		#region INuGenControlStateTracker Members

		/*
		 * Enabled
		 */

		/// <summary>
		/// </summary>
		/// <param name="value"><see langword="true"/> to enable; <see langword="false"/> to disable.</param>
		public void Enabled(bool value)
		{
			if (value)
			{
				if (this.Focused)
				{
					_state = NuGenControlState.Focused;
				}
				else
				{
					_state = NuGenControlState.Normal;
				}
			}
			else
			{
				_state = NuGenControlState.Disabled;
			}
		}

		/*
		 * GotFocus
		 */

		/// <summary>
		/// </summary>
		public void GotFocus()
		{
			this.Focused = true;
			_state = NuGenControlState.Focused;
		}

		/*
		 * LostFocus
		 */

		/// <summary>
		/// </summary>
		public void LostFocus()
		{
			this.Focused = false;
			_state = NuGenControlState.Normal;
		}

		/*
		 * GetControlState
		 */

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public NuGenControlState GetControlState()
		{
			return _state;
		}

		#endregion

		#region Properties.Protected

		/*
		 * Focused
		 */

		private bool _focused;

		/// <summary>
		/// </summary>
		protected bool Focused
		{
			get
			{
				return _focused;
			}
			set
			{
				_focused = value;
			}
		}

		#endregion

		#region Methods.Protected

		/*
		 * SetState
		 */

		private NuGenControlState _state = NuGenControlState.Normal;

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		protected void SetState(NuGenControlState value)
		{
			_state = value;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenControlStateTracker"/> class.
		/// </summary>
		public NuGenControlStateTracker()
		{

		}

		#endregion
	}
}
