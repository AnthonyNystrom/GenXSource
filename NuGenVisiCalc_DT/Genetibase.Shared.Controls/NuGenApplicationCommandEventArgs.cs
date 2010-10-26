/* -----------------------------------------------
 * NuGenApplicationCommandEventArgs.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Provides data for the <see cref="NuGenCommandManagerBase.ApplicationCommandExecute"/>
	/// and <see cref="NuGenCommandManagerBase.ApplicationCommandUpdate"/> events.
	/// </summary>
	public class NuGenApplicationCommandEventArgs : EventArgs
	{
		private NuGenApplicationCommand _applicationCommand;
		private object _item;

		/// <summary>
		/// Initializes a new instance of the UICommandEventArgs class. 
		/// </summary>
		/// <param name="applicationCommand">The ApplicationCommand causing the event.</param>
		/// <param name="item">The item causing the event.</param>
		public NuGenApplicationCommandEventArgs(NuGenApplicationCommand applicationCommand, object item)
		{
			_applicationCommand = applicationCommand;
			_item = item;
		}

		/// <summary>
		/// Gets the command causing the event.
		/// </summary>
		/// <value>The command for the event that has been invoked</value>
		public NuGenApplicationCommand ApplicationCommand
		{
			get
			{
				return _applicationCommand;
			}
		}

		/// <summary>
		/// Gets the item causing the event.
		/// </summary>
		/// <value>The item for the event that has been invoked.</value>
		public object Item
		{
			get
			{
				return _item;
			}
		}
	}
}
