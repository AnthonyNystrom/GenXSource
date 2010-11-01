/* -----------------------------------------------
 * NuGenControlStateTrackerBase.cs
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
	/// </summary>
	public class NuGenControlStateTrackerBase : INuGenControlStateTracker
	{
		#region INuGenControlStateTracker Members

		/*
		 * Enabled
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="target"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void Enabled(object target)
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
		 * Disabled
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="target"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void Disabled(object target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}

			this.SetState(target, NuGenControlState.Disabled);
		}

		/*
		 * GotFocus
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="target"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void GotFocus(object target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}

			this.Focused = true;
			this.SetState(target, NuGenControlState.Focused);
		}

		/*
		 * LostFocus
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="target"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void LostFocus(object target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}

			this.Focused = false;
			this.SetState(target, NuGenControlState.Normal);
		}

		/*
		 * GetControlState
		 */

		/// <summary>
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="target"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenControlState GetControlState(object target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}

			Debug.Assert(this.States != null, "this.States != null");

			if (this.States.ContainsKey(target))
			{
				return this.States[target];
			}

			return NuGenControlState.Normal;
		}

		#endregion

		#region Properties.Protected

		/*
		 * Focused
		 */

		private bool _focused = false;

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

		/*
		 * States
		 */

		private Dictionary<object, NuGenControlState> _states = null;

		/// <summary>
		/// </summary>
		protected Dictionary<object, NuGenControlState> States
		{
			get
			{
				if (_states == null)
				{
					_states = new Dictionary<object, NuGenControlState>();
				}

				return _states;
			}
		}

		#endregion

		#region Methods.Protected

		/*
		 * SetState
		 */

		/// <summary>
		/// </summary>
		/// <param name="target"></param>
		/// <param name="stateToSet"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="target"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		protected void SetState(object target, NuGenControlState stateToSet)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}

			Debug.Assert(this.States != null, "this.States != null");

			if (this.States.ContainsKey(target))
			{
				this.States[target] = stateToSet;
			}
			else
			{
				this.States.Add(target, stateToSet);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenControlStateTrackerBase"/> class.
		/// </summary>
		protected NuGenControlStateTrackerBase()
		{
		}

		#endregion
	}
}
