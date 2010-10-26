/* -----------------------------------------------
 * INuGenValueTracker.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// </summary>
	public interface INuGenValueTracker : IDisposable
	{
		/// <summary>
		/// </summary>
		int LargeChange
		{
			get;
			set;
		}

		/// <summary>
		/// </summary>
		int Maximum
		{
			get;
			set;
		}

		/// <summary>
		/// </summary>
		int Minimum
		{
			get;
			set;
		}

		/// <summary>
		/// </summary>
		int SmallChange
		{
			get;
			set;
		}

		/// <summary>
		/// </summary>
		int Value
		{
			get;
			set;
		}

		/// <summary>
		/// </summary>
		void LargeChangeDown();
		
		/// <summary>
		/// </summary>
		void LargeChangeUp();
		
		/// <summary>
		/// </summary>
		void SmallChangeDown();
		
		/// <summary>
		/// </summary>
		void SmallChangeUp();
	}
}
