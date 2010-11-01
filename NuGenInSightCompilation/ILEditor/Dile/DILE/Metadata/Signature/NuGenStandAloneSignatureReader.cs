using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;
using Dile.Metadata;

namespace Dile.Metadata.Signature
{
	public class NuGenStandAloneSignatureReader : NuGenMethodSignatureReader
	{
		private List<NuGenBaseSignatureItem> varargParameters;
		public List<NuGenBaseSignatureItem> VarargParameters
		{
			get
			{
				return varargParameters;
			}
			private set
			{
				varargParameters = value;
			}
		}

		private bool sentinelFound = false;
		public bool SentinelFound
		{
			get
			{
				return sentinelFound;
			}
			private set
			{
				sentinelFound = value;
			}
		}

		private NuGenBaseSignatureItem fieldSignatureItem;
		public NuGenBaseSignatureItem FieldSignatureItem
		{
			get
			{
				return fieldSignatureItem;
			}
			private set
			{
				fieldSignatureItem = value;
			}
		}

		public NuGenStandAloneSignatureReader(Dictionary<uint, NuGenTokenBase> allTokens, IntPtr signatureBlob, uint signatureLength)
			: base(allTokens, signatureBlob, signatureLength)
		{
		}

		public override void ReadSignature()
		{
			uint data;
			int dataLength = Convert.ToInt32(NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out data));

			CallingConvention = (CorCallingConvention)data;
			NuGenHelperFunctions.StepIntPtr(ref signatureBlob, dataLength);

			if (CallingConvention == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_FIELD)
			{
				ReadFieldSignature(ref signatureBlob);
			}
			else if (CallingConvention == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_LOCAL_SIG)
			{
				ReadLocalVarSignature(ref signatureBlob);
			}
			else
			{
				ReadStandAloneMethodSignature(ref signatureBlob);
			}
		}

		private void ReadLocalVarSignature(ref IntPtr signatureBlob)
		{
			uint count;

			NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out count));

			if (count > 0)
			{
				Parameters = new List<NuGenBaseSignatureItem>();
			}

			int index = 0;
			while (index < count)
			{
				index++;
				uint data;
				uint dataLength = NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out data);
				bool pinned = ((CorElementType)data == CorElementType.ELEMENT_TYPE_PINNED);
				
				if (pinned)
				{
					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
					dataLength = NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out data);
				}

				bool byRef = ((CorElementType)data == CorElementType.ELEMENT_TYPE_BYREF);

				if (byRef)
				{
					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
				}

				NuGenBaseSignatureItem signatureItem = ReadSignatureItem(ref signatureBlob);
				NuGenTypeSignatureItem typeSignatureItem = null;

				if (signatureItem is NuGenArraySignatureItem)
				{
					typeSignatureItem = ((NuGenArraySignatureItem)signatureItem).Type;
				}
				else if (signatureItem is NuGenTypeSignatureItem)
				{
					typeSignatureItem = (NuGenTypeSignatureItem)signatureItem;
				}

				if (typeSignatureItem != null)
				{
					typeSignatureItem.ByRef = byRef;
					typeSignatureItem.Pinned = pinned;
				}

				Parameters.Add(signatureItem);
			}
		}

		private void ReadStandAloneMethodSignature(ref IntPtr signatureBlob)
		{
			uint paramCount = 0;

			if (CallingConvention == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_HASTHIS)
			{
				uint data;
				NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out data));
				CorCallingConvention explicitThis = (CorCallingConvention)data;

				if (explicitThis == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_EXPLICITTHIS)
				{
					CallingConvention |= CorCallingConvention.IMAGE_CEE_CS_CALLCONV_EXPLICITTHIS;
				}
				else
				{
					paramCount = data;
				}
			}
			else
			{
				NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out paramCount));
			}

			ReturnType = ReadSignatureItem(ref signatureBlob);

			if (paramCount > 0)
			{
				Parameters = new List<NuGenBaseSignatureItem>();

				int paramIndex = 0;
				while (paramIndex < paramCount)
				{
					uint data;
					NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out data);
					CorElementType elementType = (CorElementType)data;

					if (elementType == CorElementType.ELEMENT_TYPE_SENTINEL)
					{
						throw new NotImplementedException("Sentinel found.");
					}

					if (SentinelFound)
					{
						if (VarargParameters == null)
						{
							VarargParameters = new List<NuGenBaseSignatureItem>();
						}

						VarargParameters.Add(ReadSignatureItem(ref signatureBlob));
					}
					else
					{
						Parameters.Add(ReadSignatureItem(ref signatureBlob));
					}
					paramIndex++;
				}
			}
		}

		private void ReadFieldSignature(ref IntPtr signatureBlob)
		{
			FieldSignatureItem = ReadSignatureItem(ref signatureBlob);
		}
	}
}