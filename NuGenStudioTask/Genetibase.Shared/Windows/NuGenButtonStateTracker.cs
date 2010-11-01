/* -----------------------------------------------
 * NuGenButtonStateTracker.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Set the state using the <see cref="MouseDown"/>, <see cref="MouseEnter"/>, etc. methods.
	/// Retrieve the state using the <see cref="INuGenControlStateTracker.GetControlState"/> method.
	/// </summary>
	public class NuGenButtonStateTracker : NuGenControlStateTrackerBase, INuGenButtonStateTracker
	{
		#region INuGenButtonStateTracker Members

		/*
		 * MouseDown
		 */

		/// <summary>
		/// </summary>
		/// <param name="target"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="target"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void MouseDown(object target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}

			this.SetState(target, NuGenControlState.Pressed);
		}
		
		/*
		 * MouseEnter
		 */

		/// <summary>
		/// </summary>
		/// <param name="target"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="target"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void MouseEnter(object target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}

			this.SetState(target, NuGenControlState.Hot);
		}

		/*
		 * MouseLeave
		 */

		/// <summary>
		/// </summary>
		/// <param name="target"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="target"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void MouseLeave(object target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}

			if (this.Focused)
			{
				this.SetState(target, NuGenControlState.Focused);
			}
			else
			{
				this.SetState(target, NuGenControlState.Normal);
			}
		}

		/*
		 * MouseUp
		 */

		/// <summary>
		/// </summary>
		/// <param name="target"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="target"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void MouseUp(object target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}

			this.SetState(target, NuGenControlState.Hot);
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
