/* -----------------------------------------------
 * NuGenWindowStateTracker.cs
 * Copyright � 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Use <see cref="SetWindowState"/> to set the current window state
	/// (e.g. in <see cref="M:System.Windows.Forms.Form.OnSizeChanged"/> method).
	/// Use <see cref="GetWindowState"/> to retrieve the actual window state
	/// if the form is in normal or maximized state, or the previous window state if
	/// the form is minimized.
	/// </summary>
	public class NuGenWindowStateTracker : INuGenWindowStateTracker
	{
		/*
		 * GetLocation
		 */

		/// <summary>
		/// Returns the actual window location if the form is in the normal state, or restore bounds location otherwise.
		/// </summary>
		/// <param name="targetForm"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="targetForm"/> is <see langword="null"/>.</exception>
		public Point GetLocation(Form targetForm)
		{
			if (targetForm == null)
			{
				throw new ArgumentNullException("targetForm");
			}

			if (targetForm.WindowState == FormWindowState.Normal)
			{
				return targetForm.Bounds.Location;
			}

			return targetForm.RestoreBounds.Location;
		}

		/*
		 * GetSize
		 */

		/// <summary>
		/// Returns the actual window size if the form is in the normal state, or restore bounds size otherwise.
		/// </summary>
		/// <param name="targetForm"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="targetForm"/> is <see langword="null"/>.</exception>
		public Size GetSize(Form targetForm)
		{
			if (targetForm == null)
			{
				throw new ArgumentNullException("targetForm");
			}

			if (targetForm.WindowState == FormWindowState.Normal)
			{
				return targetForm.Bounds.Size;
			}

			return targetForm.RestoreBounds.Size;
		}

		/*
		 * GetWindowState
		 */

		/// <summary>
		/// Returns the actual window state if the <paramref name="targetForm"/> is in normal or maximized
		/// state, or the previous state if the form is minimized.
		/// </summary>
		/// <param name="targetForm"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="targetForm"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public FormWindowState GetWindowState(Form targetForm)
		{
			if (targetForm == null)
			{
				throw new ArgumentNullException("targetForm");
			}

			if (this.States.ContainsKey(targetForm))
			{
				return this.States[targetForm];
			}

			return FormWindowState.Normal;
		}

		/*
		 * SetWindowState
		 */

		/// <summary>
		/// </summary>
		/// <param name="targetForm"></param>
		/// <exception cref="ArgumentNullException">
		///	<para>
		///		<paramref name="targetForm"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void SetWindowState(Form targetForm)
		{
			if (targetForm == null)
			{
				throw new ArgumentNullException("targetForm");
			}

			if (this.States.ContainsKey(targetForm))
			{
				if (targetForm.WindowState != FormWindowState.Minimized)
				{
					this.States[targetForm] = targetForm.WindowState;
				}
			}
			else
			{
				this.States.Add(
					targetForm,
					targetForm.WindowState != FormWindowState.Minimized
						? targetForm.WindowState
						: FormWindowState.Normal
				);
			}
		}

		private Dictionary<Form, FormWindowState> _states;

		/// <summary>
		/// </summary>
		protected Dictionary<Form, FormWindowState> States
		{
			get
			{
				if (_states == null)
				{
					_states = new Dictionary<Form, FormWindowState>();
				}

				return _states;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWindowStateTracker"/> class.
		/// </summary>
		public NuGenWindowStateTracker()
		{
		}
	}
}