/* -----------------------------------------------
 * INuGenMessageProcessor.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Indicates that this class provides an initialized message map to be linked to the 
	/// <see cref="INuGenMessageFilter.MessageMap"/> as well as functionality to process Windows messages.
	/// </summary>
	public interface INuGenMessageProcessor
	{
		/// <summary>
		/// Gets an initialized message map to be linked to the <see cref="INuGenMessageFilter.MessageMap"/>.
		/// </summary>
		NuGenWmHandlerList MessageMap
		{
			get;
		}
	}
}
