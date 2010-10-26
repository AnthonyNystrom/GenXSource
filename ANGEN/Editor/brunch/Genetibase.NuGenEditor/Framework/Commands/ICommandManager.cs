/* -----------------------------------------------
 * ICommandManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

namespace Genetibase.Windows.Controls.Framework.Commands
{
	/// <summary>
	/// </summary>
	public interface ICommandManager : ICommandTarget
	{
		/// <summary>
		/// </summary>
		event CommandExecutionEventHandler CommandExecuted;
		/// <summary>
		/// </summary>
		event CommandExecutionEventHandler CommandExecuting;

		/// <summary>
		/// </summary>
		void AddTarget(ICommandTarget target);
		/// <summary>
		/// </summary>
		Boolean HandleShortcut(Key shortcutKey, ModifierKeys modifiers);
		/// <summary>
		/// </summary>
		void RemoveTarget(ICommandTarget target);
		/// <summary>
		/// </summary>
		void SetCommandPropertyDefault(String commandName, String propertyName, Object propertyValue);

		/// <summary>
		/// </summary>
		Boolean ShortcutsEnabled
		{
			get;
			set;
		}
	}


}
