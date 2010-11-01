using System;

using System.Collections.Generic;

namespace Dile.Disassemble
{
	public interface NuGenILazyInitialized
	{
		void LazyInitialize(Dictionary<uint, NuGenTokenBase> allTokens);
	}
}