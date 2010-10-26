/* -----------------------------------------------
 * FloatingToolForm.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.Shared.Windows;
using Genetibase.WinApi;

namespace Genetibase.NuGenVisiCalc
{
	[System.ComponentModel.DesignerCategory("Code")]
	internal class FloatingToolForm : VisiCalcFormBase
	{
		#region Events

		/*
		 * ProcessCmdKeyEvent
		 */

		public event CmdKeysEventHandler ProcessCmdKeyEvent;

		/*
		 * RelinquishFocus
		 */

		/// <summary>
		/// Occurs when it is appropriate for the parent to steal focus.
		/// </summary>
		public event EventHandler RelinquishFocus;

		protected virtual void OnRelinquishFocus()
		{
			if (RelinquishFocus != null)
			{
				RelinquishFocus(this, EventArgs.Empty);
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		protected override Boolean ProcessCmdKey(ref Message msg, Keys keyData)
		{
			Boolean result = false;

			if (NuGenKeys.IsArrowKey(keyData))
			{
				KeyEventArgs kea = new KeyEventArgs(keyData);

				switch (msg.Msg)
				{
					case WinUser.WM_KEYDOWN:
					{
						this.OnKeyDown(kea);
						return kea.Handled;
					}
				}
			}
			else
			{
				if (ProcessCmdKeyEvent != null)
				{
					result = ProcessCmdKeyEvent(this, ref msg, keyData);
				}
			}

			if (!result)
			{
				result = base.ProcessCmdKey(ref msg, keyData);
			}

			return result;
		}

		protected override void OnResizeEnd(EventArgs e)
		{
			base.OnResizeEnd(e);
			OnRelinquishFocus();
		}

		#endregion

		#region EventHandlers

		private void _controlAddedHandler(Object sender, ControlEventArgs e)
		{
			e.Control.ControlAdded += _controlAddedDelegate;
			e.Control.ControlRemoved += _controlRemovedDelegate;
			e.Control.KeyUp += _keyUpDelegate;
		}

		private void _controlRemovedHandler(Object sender, ControlEventArgs e)
		{
			e.Control.ControlAdded -= _controlAddedDelegate;
			e.Control.ControlRemoved -= _controlRemovedDelegate;
			e.Control.KeyUp -= _keyUpDelegate;
		}

		private void _keyUpHandler(Object sender, KeyEventArgs e)
		{
			if (!e.Handled)
			{
				this.OnKeyUp(e);
			}
		}

		#endregion

		private ControlEventHandler _controlAddedDelegate;
		private ControlEventHandler _controlRemovedDelegate;
		private KeyEventHandler _keyUpDelegate;

		public FloatingToolForm()
		{
			this.KeyPreview = true;
			_controlAddedDelegate = new ControlEventHandler(_controlAddedHandler);
			_controlRemovedDelegate = new ControlEventHandler(_controlRemovedHandler);
			_keyUpDelegate = new KeyEventHandler(_keyUpHandler);

			this.ControlAdded += _controlAddedDelegate; // we don't override OnControlAdded so we can re-use the method (see code below for ControlAdded)
			this.ControlRemoved += _controlRemovedDelegate;
			this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
			this.ForceActiveTitleBar = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = SizeGripStyle.Hide;
		}
	}
}
