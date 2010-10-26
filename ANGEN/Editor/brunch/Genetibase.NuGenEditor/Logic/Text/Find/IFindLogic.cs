/* -----------------------------------------------
 * IFindLogic.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Windows.Controls.Data.Text;

namespace Genetibase.Windows.Controls.Logic.Text.Find
{
	/// <summary>
	/// </summary>
	public interface IFindLogic
	{
		/// <summary>
		/// </summary>
		IList<TextSpan> FindAll();
		/// <summary>
		/// </summary>
		TextSpan FindNext(Int32 startIndex, Boolean wraparound);

		/// <summary>
		/// </summary>
		Boolean MatchCase
		{
			get;
			set;
		}
		/// <summary>
		/// </summary>
		Boolean SearchReverse
		{
			get;
			set;
		}
		/// <summary>
		/// </summary>
		String SearchString
		{
			get;
			set;
		}
		/// <summary>
		/// </summary>
		TextBuffer TextBufferToSearch
		{
			get;
			set;
		}
		/// <summary>
		/// </summary>
		Boolean WholeWord
		{
			get;
			set;
		}
	}
}
