/* -----------------------------------------------
 * IDescriptor.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.NuGenVisiCalc
{
	internal interface IDescriptor
	{
		NodeBase CreateNode(Canvas canvas);
	}
}
