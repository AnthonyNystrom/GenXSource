using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public class NuGenPropertySignatureReader : NuGenMethodSignatureReader
	{
		public NuGenPropertySignatureReader(Dictionary<uint, NuGenTokenBase> allTokens, IntPtr signatureBlob, uint signatureLength)
			: base(allTokens, signatureBlob, signatureLength)
		{
		}

		public override void  ReadSignature()
		{
			uint data;
			int dataLength = Convert.ToInt32(NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out data));

			CallingConvention = (CorCallingConvention)data;
			NuGenHelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
			uint paramCount = 0;

			NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out paramCount));

			ReturnType = ReadSignatureItem(ref signatureBlob);

			if (paramCount > 0)
			{
				Parameters = new List<NuGenBaseSignatureItem>();
			}

			int paramIndex = 0;
			while (paramIndex < paramCount && signatureBlob.ToInt32() < SignatureEnd)
			{
				Parameters.Add(ReadSignatureItem(ref signatureBlob));
				paramIndex++;
			}
		}
	}
}