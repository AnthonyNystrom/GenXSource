/* -----------------------------------------------
 * NuGenCulture.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Diagnostics;
using System.Globalization;

namespace Genetibase.Shared.Globalization
{
	/// <summary>
	/// Represents specific cultures.
	/// </summary>
	public static class NuGenCulture
	{
		#region Properties.Public.Static

		/*
		 * enUS
		 */

		private static CultureInfo _enUS;

		/// <summary>
		/// Gets the en-US culture.
		/// </summary>
		public static CultureInfo enUS
		{
			[DebuggerStepThrough]
			get
			{
				if (_enUS == null) 
				{
					_enUS = new CultureInfo("en-US");
				}

				return _enUS;
			}
		}

		#endregion
	}
}
