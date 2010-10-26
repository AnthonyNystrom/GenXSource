/* -----------------------------------------------
 * OperatorDescriptorComparer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.NuGenVisiCalc.Operators
{
	internal sealed class OperatorDescriptorComparer : IComparer<OperatorDescriptor>
	{
		public Int32 Compare(OperatorDescriptor x, OperatorDescriptor y)
		{
			if (x == null && y == null)
			{
				return 0;
			}

			if (x == null && y != null)
			{
				return -1;
			}

			if (x != null && y == null)
			{
				return 1;
			}

			return String.Compare(x.ToString(), y.ToString());
		}
	}
}
