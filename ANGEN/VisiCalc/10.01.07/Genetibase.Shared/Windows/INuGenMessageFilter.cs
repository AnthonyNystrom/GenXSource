/* -----------------------------------------------
 * INuGenMessageFilter.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Indicates that this class invokes message handlers according to the <see cref="MessageMap"/>.
	/// </summary>
	public interface INuGenMessageFilter
	{
		/// <summary>
		/// </summary>
		NuGenWmHandlerList MessageMap
		{
			get;
		}

		/// <summary>
		/// </summary>
		Control TargetControl
		{
			get;
			[SecurityPermission(SecurityAction.LinkDemand)]
			set;
		}
	}
}
