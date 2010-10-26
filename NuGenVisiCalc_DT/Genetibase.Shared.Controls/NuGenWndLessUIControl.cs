/* -----------------------------------------------
 * NuGenWndLessUIControl.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a <see cref="NuGenWndLessControl"/> that uses <see cref="INuGenButtonStateService"/>
	/// to track its state.
	/// </summary>
	public class NuGenWndLessUIControl : NuGenWndLessControl
	{
		private INuGenButtonStateTracker _buttonStateTracker;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenButtonStateTracker ButtonStateTracker
		{
			get
			{
				if (_buttonStateTracker == null)
				{
					INuGenButtonStateService service = this.ServiceProvider.GetService<INuGenButtonStateService>();

					if (service == null)
					{
						throw new NuGenServiceNotFoundException<INuGenButtonStateService>();
					}

					_buttonStateTracker = service.CreateStateTracker();
					Debug.Assert(_buttonStateTracker != null, "_buttonStateTracker != null");
				}

				return _buttonStateTracker;
			}
		}

		private INuGenServiceProvider _serviceProvider;

		/// <summary>
		/// </summary>
		protected INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.EnabledChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			this.ButtonStateTracker.Enabled(this.Enabled);
			base.OnEnabledChanged(e);
		}

		/// <summary>
		/// Add custom logic before the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.GotFocus"/> event will be raised.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnGotFocus(EventArgs e)
		{
			this.ButtonStateTracker.GotFocus();
			base.OnGotFocus(e);
		}

		/// <summary>
		/// Add custom logic before the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.LostFocus"/> event will be raised.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLostFocus(EventArgs e)
		{
			this.ButtonStateTracker.LostFocus();
			base.OnLostFocus(e);
		}

		/// <summary>
		/// Add custom logic before the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.MouseDown"/> event will be raised.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			this.ButtonStateTracker.MouseDown();
			base.OnMouseDown(e);
		}

		/// <summary>
		/// Add custom logic before the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.MouseEnter"/> event will be raised.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseEnter(EventArgs e)
		{
			this.ButtonStateTracker.MouseEnter();
			base.OnMouseEnter(e);
		}

		/// <summary>
		/// Add custom logic before the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.MouseLeave"/> event will be raised.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseLeave(EventArgs e)
		{
			this.ButtonStateTracker.MouseLeave();
			base.OnMouseLeave(e);
		}

		/// <summary>
		/// Add custom logic before the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.MouseUp"/> event will be raised.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			this.ButtonStateTracker.MouseUp();
			base.OnMouseUp(e);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWndLessUIControl"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenButtonStateService"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenWndLessUIControl(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;
		}
	}
}
