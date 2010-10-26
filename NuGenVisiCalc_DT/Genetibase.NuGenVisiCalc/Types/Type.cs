/* -----------------------------------------------
 * Type.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.NuGenVisiCalc.Types
{
	[Serializable]
	internal class Type : NodeBase
	{
		public override Object GetData()
		{
			return GetOutput(0).Data;
		}

		public override Object GetData(Int32 index)
		{
			return GetOutput(index).Data;
		}

		public override Object GetData(Int32 index, params Object[] options)
		{
			return GetData(index, options);
		}

		public override Object GetData(params Object[] options)
		{
			return GetData();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Type"/> class.
		/// </summary>
		public Type()
		{
		}
	}
}
