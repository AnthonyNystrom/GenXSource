/* -----------------------------------------------
 * IDescriptor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
