/* -----------------------------------------------
 * ProgramOutput.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Genetibase.NuGenVisiCalc.ComponentModel;

namespace Genetibase.NuGenVisiCalc.Types
{
	[Serializable]
	[Type("ProgramOutput")]
	internal class ProgramOutput : NumericResult
	{
		private Int32 _outputIndex;

		[Browsable(true)]
		[NuGenSRCategory("Category_Schema")]
		[ReadOnly(true)]
		public Int32 OutputIndex
		{
			get
			{
				return _outputIndex;
			}
			set
			{
				_outputIndex = value;
			}
		}

		public ProgramOutput()
			: base()
		{
			Name = Header = "ProgramOutput";
		}
	}
}
