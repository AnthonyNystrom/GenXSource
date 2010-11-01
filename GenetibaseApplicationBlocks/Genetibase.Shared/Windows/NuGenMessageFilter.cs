/* -----------------------------------------------
 * NuGenMessageFilter.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Use the <see cref="MessageMap"/> property to specify the handlers that should be invoked if the
	/// specified message enters <see cref="WndProc"/>.
	/// </summary>
	public class NuGenMessageFilter : NuGenNativeWindow, INuGenMessageFilter
	{
		#region Properties.Public

		/*
		 * MessageMap
		 */

		private NuGenWmHandlerList _messageMap = null;

		/// <summary>
		/// </summary>
		public NuGenWmHandlerList MessageMap
		{
			get
			{
				if (_messageMap == null)
				{
					_messageMap = new NuGenWmHandlerList();
				}

				return _messageMap;
			}
		}

		/*
		 * TargetControl
		 */

		private Control _targetControl = null;

		/// <summary>
		/// Setting this property to <see langword="null"/> stops <see cref="MessageMap"/> handlers invokation.
		/// Note that <see cref="MessageMap"/> is not reset automatically every time a new value is set for
		/// <see cref="TargetControl"/>. You should do this manually or leave as is to continue process WM_X
		/// for the new <see cref="Control"/> with existing handlers.
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
						this.ResetTargetControl(_targetControl);
					}

					_targetControl = value;

					if (_targetControl != null)
					{
						this.InitializeTargetControl(_targetControl);
					}
				}
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Invokes the default window procedure associated with this window.
		/// </summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message"></see> that is associated with the current Windows message.</param>
		protected override void WndProc(ref Message m)
		{
			NuGenWmHandler wmHandler = this.MessageMap[m.Msg];

			if (wmHandler != null)
			{
				wmHandler(ref m, base.WndProc);
			}
			else
			{
				base.WndProc(ref m);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMessageFilter"/> class.
		/// </summary>
		public NuGenMessageFilter()
		{
		}

		#endregion
	}
}
