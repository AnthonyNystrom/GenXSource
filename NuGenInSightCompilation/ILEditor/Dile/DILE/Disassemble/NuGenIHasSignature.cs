using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata.Signature;

namespace Dile.Disassemble
{
	public interface NuGenIHasSignature
	{
		NuGenBaseSignatureReader SignatureReader
		{
			get;
		}
	}
}
