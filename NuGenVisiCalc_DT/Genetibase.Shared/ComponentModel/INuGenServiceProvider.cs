/* -----------------------------------------------
 * INuGenServiceProvider.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Indicates that this class defines a mechanism for retrieving a service object;
	/// that is, an object that provides custom support to other objects. 
	/// </summary>
	public interface INuGenServiceProvider
	{
		/// <summary>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T GetService<T>() where T : class;
	}
}
