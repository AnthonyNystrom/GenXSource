/* -----------------------------------------------
 * ProgramDescriptorComparer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.NuGenVisiCalc.Programs
{
	internal sealed class ProgramDescriptorComparer : IComparer<ProgramDescriptor>
	{
		public Int32 Compare(ProgramDescriptor x, ProgramDescriptor y)
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
