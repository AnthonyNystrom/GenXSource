using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public class NuGenTypeSpecSignatureReader : NuGenBaseSignatureReader
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

		public NuGenTypeSpecSignatureReader(Dictionary<uint, NuGenTokenBase> allTokens, IntPtr signatureBlob, uint signatureLength)
			: base(allTokens, signatureBlob, signatureLength)
		{
		}

		public override void ReadSignature()
		{
			uint data;
			uint dataLength = NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out data);
			CorElementType elementType = (CorElementType)data;

			Type = ReadSignatureItem(ref signatureBlob);
		}

		public override void SetGenericParametersOfMethod(List<NuGenGenericParameter> genericParameters)
		{
			if (HasGenericMethodParameter)
			{
				NuGenHelperFunctions.SetSignatureItemToken(AllTokens, Type, genericParameters);

				if (GenericParameters != null)
				{
					foreach (NuGenBaseSignatureItem signatureItem in GenericParameters)
					{
						NuGenHelperFunctions.SetSignatureItemToken(AllTokens, Type, genericParameters);
					}
				}
			}
		}
	}
}