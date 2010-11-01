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
	public class NuGenTabStateTracker : NuGenControlStateTrackerBase, INuGenTabStateTracker
	{
		#region INuGenTabStateTracker Members

		/*
		 * Deselect
		 */

		/// <summary>
		/// </summary>
		/// <param name="targetControl"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="targetControl"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void Deselect(Control targetControl)
		{
			if (targetControl == null)
			{
				throw new ArgumentNullException("targetControl");
			}

			this.SetState(targetControl, NuGenControlState.Normal);
		}

		/*
		 * MouseEnter
		 */

		/// <summary>
		/// </summary>
		/// <param name="targetControl"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="targetControl"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void MouseEnter(Control targetControl)
		{
			if (targetControl == null)
			{
				throw new ArgumentNullException("targetControl");
			}

			if (this.GetControlState(targetControl) != NuGenControlState.Pressed)
			{
				this.SetState(targetControl, NuGenControlState.Hot);
			}
		}

		/*
		 * MouseLeave
		 */

		/// <summary>
		/// </summary>
		/// <param name="targetControl"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="targetControl"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void MouseLeave(Control targetControl)
		{
			if (targetControl == null)
			{
				throw new ArgumentNullException("targetControl");
			}

			if (this.GetControlState(targetControl) != NuGenControlState.Pressed)
			{
				this.SetState(targetControl, NuGenControlState.Normal);
			}
		}

		/*
		 * Select
		 */

		/// <summary>
		/// </summary>
		/// <param name="targetControl"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="targetControl"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void Select(Control targetControl)
		{
			if (targetControl == null)
			{
				throw new ArgumentNullException("targetControl");
			}

			this.SetState(targetControl, NuGenControlState.Pressed);
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
