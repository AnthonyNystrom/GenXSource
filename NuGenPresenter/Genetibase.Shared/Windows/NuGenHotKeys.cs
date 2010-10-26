/* -----------------------------------------------
 * NuGenHotKeys.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Represents functionality to map hot keys to specified operations.
	/// </summary>
	public class NuGenHotKeys
	{
		private IList<NuGenHotKeyOperation> _operations;

		/// <summary>
		/// </summary>
		public IList<NuGenHotKeyOperation> Operations
		{
			get
			{
				if (_operations == null)
				{
					_operations = new List<NuGenHotKeyOperation>();
				}

				return _operations;
			}
		}

		/// <summary>
		/// </summary>
		public void Process(KeyEventArgs e)
		{
			this.Process(e.KeyCode | e.Modifiers);
		}

		/// <summary>
		/// </summary>
		public void Process(Keys keyCode)
		{
			foreach (NuGenHotKeyOperation operation in this.Operations)
			{
				if (operation.HotKeys == keyCode)
				{
					operation.Handler();
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenHotKeys"/> class.
		/// </summary>
		public NuGenHotKeys()
		{
		}
	}
}
