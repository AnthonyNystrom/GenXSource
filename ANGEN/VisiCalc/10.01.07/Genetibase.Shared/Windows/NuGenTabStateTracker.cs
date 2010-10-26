/* -----------------------------------------------
 * NuGenTabStateTracker.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// </summary>
	public class NuGenTabStateTracker : NuGenControlStateTracker, INuGenTabStateTracker
	{
		#region INuGenTabStateTracker Members

		/*
		 * Deselect
		 */

		/// <summary>
		/// </summary>
		public void Deselect()
		{
			this.SetState(NuGenControlState.Normal);
		}

		/*
		 * MouseEnter
		 */

		/// <summary>
		/// </summary>
		public void MouseEnter()
		{
			if (this.GetControlState() != NuGenControlState.Pressed)
			{
				this.SetState(NuGenControlState.Hot);
			}
		}

		/*
		 * MouseLeave
		 */

		/// <summary>
		/// </summary>
		public void MouseLeave()
		{
			if (this.GetControlState() != NuGenControlState.Pressed)
			{
				this.SetState(NuGenControlState.Normal);
			}
		}

		/*
		 * Select
		 */

		/// <summary>
		/// </summary>
		public void Select()
		{
			this.SetState(NuGenControlState.Pressed);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabStateTracker"/> class.
		/// </summary>
		public NuGenTabStateTracker()
		{

		}

		#endregion
	}
}
