/* -----------------------------------------------
 * NuGenBinding.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Genetibase.Shared.Reflection
{
	/// <summary>
	/// Defines <see cref="BindingFlags"/> presets.
	/// </summary>
	public static class NuGenBinding
	{
		/// <summary>
		/// </summary>
		public static BindingFlags Instance
		{
			get
			{
				return 0
					| BindingFlags.Instance
					| BindingFlags.NonPublic
					| BindingFlags.Public
					;
			}
		}

		/// <summary>
		/// </summary>
		public static BindingFlags Static
		{
			get
			{
				return 0
					| BindingFlags.NonPublic
					| BindingFlags.Public
					| BindingFlags.Static
					;
			}
		}
	}
}
