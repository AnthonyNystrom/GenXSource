/* -----------------------------------------------
 * BooleanParam.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
