/* -----------------------------------------------
 * NuGenButtonStateTracker.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Set the state using the <see cref="MouseDown"/>, <see cref="MouseEnter"/>, etc. methods.
	/// Retrieve the state using the <see cref="INuGenControlStateTracker.GetControlState"/> method.
	/// </summary>
	public class NuGenButtonStateTracker : NuGenControlStateTracker, INuGenButtonStateTracker
	{
		#region INuGenButtonStateTracker Members

		/*
		 * MouseDown
		 */

		/// <summary>
		/// </summary>
		public void MouseDown()
		{
			this.SetState(NuGenControlState.Pressed);
		}

		/*
		 * MouseEnter
		 */

		/// <summary>
		/// </summary>
		public void MouseEnter()
		{
			this.SetState(NuGenControlState.Hot);
		}

		/*
		 * MouseLeave
		 */

		/// <summary>
		/// </summary>
		public void MouseLeave()
		{
			if (this.Focused)
			{
				this.SetState(NuGenControlState.Focused);
			}
			else
			{
				this.SetState(NuGenControlState.Normal);
			}
		}

		/*
		 * MouseUp
		 */

		/// <summary>
		/// </summary>
		public void MouseUp()
		{
			this.SetState(NuGenControlState.Hot);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenButtonStateTracker"/> class.
		/// </summary>
		public NuGenButtonStateTracker()
		{

		}

		#endregion
	}
}
