using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace Dile.Metadata
{
	[GuidAttribute("06A3EA8B-0225-11d1-BF72-00C04FC31E12"), ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface NuGenIMapToken
	{
		int Map(uint tkImp, uint tkEmit);
	}
}