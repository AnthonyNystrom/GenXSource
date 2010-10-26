/* -----------------------------------------------
 * ICommandTarget.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Windows.Controls.Framework.Commands
{
	/// <summary>
	/// </summary>
	public interface ICommandTarget
	{
		/// <summary>
		/// </summary>
		event CommandChangedEventHandler CommandChanged;

		/// <summary>
		/// </summary>
		void Execute(String commandName);
		/// <summary>
		/// </summary>
		Object GetCommandProperty(String commandName, String propertyName);
		/// <summary>
		/// </summary>
		void SetCommandProperty(String commandName, String propertyName, Object propertyValue);

		/// <summary>
		/// </summary>
		ICommandCollection Commands
		{
			get;
		}
	}
}
