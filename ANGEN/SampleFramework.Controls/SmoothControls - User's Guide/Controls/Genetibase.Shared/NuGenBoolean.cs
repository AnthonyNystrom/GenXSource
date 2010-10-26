/* -----------------------------------------------
 * NuGenBoolean.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetibase.Shared
{
	/// <summary>
	/// Provides additional services for <see cref="System.Boolean"/>.
	/// </summary>
	public static class NuGenBoolean
	{
		/// <summary>
		/// Boxed false value. Use to improve performace. 
		/// </summary>
		public static Object FalseBox = false;

		/// <summary>
		/// Boxed true value. Use to improve performance
		/// </summary>
		public static Object TrueBox = true;

		/// <summary>
		/// Returns appropriate boxed boolean value,
		/// i.e. <see cref="FalseBox"/> for <see langword="false"/> and <see cref="TrueBox"/> for <see langword="true"/>.
		/// </summary>
		public static Object Boxed(this Boolean value)
		{
			if (value)
			{
				return NuGenBoolean.TrueBox;
			}

			return NuGenBoolean.FalseBox;
		}
	}
}
