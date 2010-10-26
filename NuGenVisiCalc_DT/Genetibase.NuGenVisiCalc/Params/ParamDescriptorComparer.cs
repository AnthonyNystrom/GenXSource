/* -----------------------------------------------
 * ParamDescriptorComparer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.NuGenVisiCalc.Params
{
	internal sealed class ParamDescriptorComparer : IComparer<ParamDescriptor>
	{
		public Int32 Compare(ParamDescriptor x, ParamDescriptor y)
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
