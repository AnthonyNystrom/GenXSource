using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public class NuGenMethodSpecSignatureReader : NuGenBaseSignatureReader
	{
		private CorCallingConvention callingConvention;
		public CorCallingConvention CallingConvention
		{
			get
			{
				return callingConvention;
			}
			private set
			{
				callingConvention = value;
			}
		}

		private List<NuGenBaseSignatureItem> arguments;
		public List<NuGenBaseSignatureItem> Arguments
		{
			get
			{
				return arguments;
			}
			private set
			{
				arguments = value;
			}
		}

		public NuGenMethodSpecSignatureReader(Dictionary<uint, NuGenTokenBase> allTokens, IntPtr signatureBlob, uint signatureLength): base(allTokens, signatureBlob, signatureLength)
		{
		}

		public override void ReadSignature()
		{
			uint data;
			int dataLength = Convert.ToInt32(NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out data));

			CallingConvention = (CorCallingConvention)data;
			NuGenHelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
			uint argumentCount = 0;

			dataLength = Convert.ToInt32(NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out argumentCount));
			NuGenHelperFunctions.StepIntPtr(ref signatureBlob, dataLength);

			if (argumentCount > 0)
			{
				Arguments = new List<NuGenBaseSignatureItem>();
			}

			int argumentIndex = 0;
			while (argumentIndex < argumentCount && signatureBlob.ToInt32() < SignatureEnd)
			{
				Arguments.Add(ReadSignatureItem(ref signatureBlob));
				argumentIndex++;
			}
		}

		public override void SetGenericParametersOfMethod(List<NuGenGenericParameter> genericParameters)
		{
			if (Arguments != null && HasGenericMethodParameter)
			{
				foreach (NuGenBaseSignatureItem signatureItem in Arguments)
				{
					NuGenHelperFunctions.SetSignatureItemToken(AllTokens, signatureItem, genericParameters);
				}
			}
		}
	}
}