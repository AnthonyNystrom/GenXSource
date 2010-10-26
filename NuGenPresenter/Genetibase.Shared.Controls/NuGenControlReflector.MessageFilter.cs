/* -----------------------------------------------
 * NuGenControlReflector.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenControlReflector
	{
		[SecurityPermission(SecurityAction.LinkDemand)]
		private sealed class MessageFilter : NativeWindow
		{
			#region Events

			public event EventHandler TargetControlPaint;

			private void OnTargetControlPaint(EventArgs e)
			{
				if (this.TargetControlPaint != null)
				{
					this.TargetControlPaint(this, e);
				}
			}

			#endregion

			#region Properties.Public

			/*
			 * TargetControl
			 */

			private Control _targetControl;

			/// <summary>
			/// </summary>
			public Control TargetControl
			{
				get
				{
					return _targetControl;
				}
				set
				{
					if (_targetControl != value)
					{
						if (_targetControl != null)
						{
							this.ReleaseHandle();

							_targetControl.HandleCreated -= _targetControl_HandleCreated;
							_targetControl.HandleDestroyed -= _targetControl_HandleDestroyed;
						}

						_targetControl = value;

						if (_targetControl != null)
						{
							if (_targetControl.IsHandleCreated)
							{
								this.AssignHandle(_targetControl.Handle);
							}

							_targetControl.HandleCreated += _targetControl_HandleCreated;
							_targetControl.HandleDestroyed += _targetControl_HandleDestroyed;
						}
					}
				}
			}

			#endregion

			#region Methods.Protected.Overridden

			/*
			 * WndProc
			 */

			/// <summary>
			/// Invokes the default window procedure associated with this window.
			/// </summary>
			/// <param name="m">A <see cref="T:System.Windows.Forms.Message"></see> that is associated with the current Windows message.</param>
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == WinUser.WM_PAINT)
				{
					this.OnTargetControlPaint(EventArgs.Empty);
				}

				base.WndProc(ref m);
			}

			#endregion

			#region EventHandlers.TargetControl

			private void _targetControl_HandleCreated(object sender, EventArgs e)
			{
				this.AssignHandle(((Control)sender).Handle);
			}

			private void _targetControl_HandleDestroyed(object sender, EventArgs e)
			{
				this.ReleaseHandle();
			}

			#endregion

			public MessageFilter()
			{
			}
		}
	}
}
