/* -----------------------------------------------
 * INuGenWmHandlerMapper.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Indicates that this class provides functionality for message maps initialization.
	/// </summary>
	public interface INuGenWmHandlerMapper
	{
		/// <summary>
		/// Looks for methods marked with <see cref="NuGenWmHandlerAttribute"/> and initializes a message map
		/// of type <see cref="NuGenWmHandlerList"/>.
		/// </summary>
		NuGenWmHandlerList BuildMessageMap(INuGenMessageProcessor messageProcessor);
	}
}
