/* -----------------------------------------------
 * NumberParam.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.NuGenVisiCalc.Params
{
	[Serializable]
	[Param("Number")]
	internal class NumberParam : Types.Number
	{
		/// <summary>
		/// Intiaializes a new instance of the <see cref="NumberParam"/> class.
		/// </summary>
		public NumberParam()
		{
			Name = Header = "NumberParam";
		}
	}
}
