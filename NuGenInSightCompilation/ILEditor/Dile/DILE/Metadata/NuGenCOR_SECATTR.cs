using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace Dile.Metadata
{
	public struct NuGenCOR_SECATTR
	{
		public uint tkCtor;
		public IntPtr pCustomAttribute;
		public uint cbCustomAttribute;
	}
}