/* -----------------------------------------------
 * TypeDescriptorComparer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.NuGenVisiCalc.Types
{
	internal sealed class TypeDescriptorComparer : IComparer<TypeDescriptor>
	{
		public Int32 Compare(TypeDescriptor x, TypeDescriptor y)
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
