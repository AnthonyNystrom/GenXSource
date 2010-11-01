using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public class NuGenFieldSignatureReader : NuGenBaseSignatureReader
	{
		private NuGenBaseSignatureItem type;
		public NuGenBaseSignatureItem Type
		{
			get
			{
				return type;
			}
			private set
			{
				type = value;
			}
		}

		private List<NuGenBaseSignatureItem> genericParameters;
		public List<NuGenBaseSignatureItem> GenericParameters
		{
			get
			{
				return genericParameters;
			}
			private set
			{
				genericParameters = value;
			}
		}

		public NuGenFieldSignatureReader(Dictionary<uint, NuGenTokenBase> allTokens, IntPtr signatureBlob, uint signatureLength)
			: base(allTokens, signatureBlob, signatureLength)
		{
		}

		public override void ReadSignature()
		{
			uint data;
			int dataLength = Convert.ToInt32(NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out data));

			CorCallingConvention callingConvention = (CorCallingConvention)data;
			NuGenHelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
			Type = ReadSignatureItem(ref signatureBlob);

			if (signatureBlob.ToInt32() < SignatureEnd)
			{
				uint genericParamCount;
				NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out genericParamCount));

				if (genericParamCount > 0)
				{
					GenericParameters = new List<NuGenBaseSignatureItem>();

					for (int genericParamIndex = 0; genericParamIndex < genericParamCount; genericParamIndex++)
					{
						GenericParameters.Add(ReadSignatureItem(ref signatureBlob));
					}
				}
			}
		}

		public override void SetGenericParametersOfMethod(List<NuGenGenericParameter> genericParameters)
		{
			if (HasGenericMethodParameter)
			{
				NuGenHelperFunctions.SetSignatureItemToken(AllTokens, Type, genericParameters);

				if (GenericParameters != null)
				{
					foreach (NuGenBaseSignatureItem genericParameter in GenericParameters)
					{
						NuGenHelperFunctions.SetSignatureItemToken(AllTokens, genericParameter, genericParameters);
					}
				}
			}
		}
	}
}