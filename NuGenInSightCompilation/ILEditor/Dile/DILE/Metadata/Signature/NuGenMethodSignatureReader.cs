using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public class NuGenMethodSignatureReader : NuGenBaseSignatureReader
	{
		private List<NuGenBaseSignatureItem> parameters;
		public List<NuGenBaseSignatureItem> Parameters
		{
			get
			{
				return parameters;
			}
			protected set
			{
				parameters = value;
			}
		}

		private CorCallingConvention callingConvention;
		public CorCallingConvention CallingConvention
		{
			get
			{
				return callingConvention;
			}
			protected set
			{
				callingConvention = value;
			}
		}

		private NuGenBaseSignatureItem returnType;
		public NuGenBaseSignatureItem ReturnType
		{
			get
			{
				return returnType;
			}
			protected set
			{
				returnType = value;
			}
		}

		public NuGenMethodSignatureReader(Dictionary<uint, NuGenTokenBase> allTokens, IntPtr signatureBlob, uint signatureLength)
			: base(allTokens, signatureBlob, signatureLength)
		{
		}

		public override void ReadSignature()
		{	
			uint data;
			int dataLength = Convert.ToInt32(NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out data));

			CallingConvention = (CorCallingConvention)data;
			NuGenHelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
			uint paramCount = 0;

			if (CallingConvention != CorCallingConvention.IMAGE_CEE_CS_CALLCONV_FIELD)
			{
				dataLength = Convert.ToInt32(NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out paramCount));
				NuGenHelperFunctions.StepIntPtr(ref signatureBlob, dataLength);

				if ((CallingConvention & CorCallingConvention.IMAGE_CEE_CS_CALLCONV_GENERIC) == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_GENERIC)
				{
					dataLength = Convert.ToInt32(NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out paramCount));
					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
				}
			}

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

		public override void SetGenericParametersOfMethod(List<NuGenGenericParameter> genericParameters)
		{
			if (HasGenericMethodParameter)
			{
				NuGenHelperFunctions.SetSignatureItemToken(AllTokens, ReturnType, genericParameters);

				if (Parameters != null)
				{
					foreach (NuGenBaseSignatureItem parameter in Parameters)
					{
						NuGenHelperFunctions.SetSignatureItemToken(AllTokens, parameter, genericParameters);
					}
				}
			}
		}
	}
}