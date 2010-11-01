/* -----------------------------------------------
 * NuGenButtonBase.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenButtonBase : Button
	{
		#region Properties.Protected

		/*
		 * ButtonStateTracker
		 */

		private INuGenButtonStateTracker _buttonStateTracker = null;

		/// <summary>
		/// </summary>
		protected INuGenButtonStateTracker ButtonStateTracker
		{
			get
			{
				if (_buttonStateTracker == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_buttonStateTracker = this.ServiceProvider.GetService<INuGenButtonStateTracker>();

					if (_buttonStateTracker == null)
					{
						throw new NuGenServiceNotFoundException<INuGenButtonStateTracker>();
					}
				}

				return _buttonStateTracker;
			}
		}

		#endregion

		#region Properties.Protected.Virtual

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/*
		 * OnMouseDown
		 */

		/// <summary>
		/// Raises the <see cref="M:System.Windows.Forms.Control.OnMouseDown(System.Windows.Forms.MouseEventArgs)"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			this.ButtonStateTracker.MouseDown(this);
			base.OnMouseDown(e);
		}

		/*
		 * OnMouseEnter
		 */

		/// <summary>
		/// Raises the mouse enter event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnMouseEnter(EventArgs e)
		{
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			this.ButtonStateTracker.MouseEnter(this);
			base.OnMouseEnter(e);
		}

		/*
		 * OnMouseLeave
		 */

		/// <summary>
		/// Raises the mouse leave event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			this.ButtonStateTracker.MouseLeave(this);
			base.OnMouseLeave(e);
		}

		/*
		 * OnMouseUp
		 */

		/// <summary>
		/// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnMouseUp(System.Windows.Forms.MouseEventArgs)"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			this.ButtonStateTracker.MouseUp(this);
			base.OnMouseUp(e);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenButtonBase"/> class.
		/// </summary>
		protected NuGenButtonBase()
			: this(new NuGenButtonBaseServiceProvider())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenButtonBase"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenButtonStateTracker"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		protected NuGenButtonBase(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;

			this.FlatStyle = FlatStyle.Standard;
		}

		#endregion
	}
}
