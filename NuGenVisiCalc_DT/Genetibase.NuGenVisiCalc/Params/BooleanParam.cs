/* -----------------------------------------------
 * BooleanParam.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.NuGenVisiCalc.Params
{
	[Serializable]
	[Param("Boolean")]
	internal class BooleanParam : Types.Boolean
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BooleanParam"/> class.
		/// </summary>
		public BooleanParam()
		{
			Name = Header = "BooleanParam";
		}
	}
}
