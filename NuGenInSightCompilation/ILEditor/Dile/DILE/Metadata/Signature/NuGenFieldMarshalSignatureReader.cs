using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public class NuGenFieldMarshalSignatureReader : NuGenBaseSignatureReader
	{
		private NuGenTypeSignatureItem marshalAsType;
		public NuGenTypeSignatureItem MarshalAsType
		{
			get
			{
				return marshalAsType;
			}
			private set
			{
				marshalAsType = value;
			}
		}

		public NuGenFieldMarshalSignatureReader(Dictionary<uint, NuGenTokenBase> allTokens, IntPtr signatureBlob, uint signatureLength)
			: base(allTokens, signatureBlob, signatureLength)
		{
		}

		public override void ReadSignature()
		{
			MarshalAsType = ReadType(ref signatureBlob);
		}

		public override void SetGenericParametersOfMethod(List<NuGenGenericParameter> genericParameters)
		{
			if (HasGenericMethodParameter)
			{
				NuGenHelperFunctions.SetSignatureItemToken(AllTokens, MarshalAsType, genericParameters);
			}
		}
	}
}