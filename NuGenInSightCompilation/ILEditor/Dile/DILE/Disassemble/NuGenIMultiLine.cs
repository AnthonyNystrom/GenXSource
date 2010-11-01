using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble.ILCodes;

namespace Dile.Disassemble
{
	public interface NuGenIMultiLine
	{
		List<NuGenCodeLine> CodeLines
		{
			get;
			set;
		}

		string HeaderText
		{
			get;
		}

		bool IsInMemory
		{
			get;
		}

		void Initialize();
	}
}
