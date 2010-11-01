using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;
using System.Runtime.InteropServices;

namespace Dile.Metadata.Signature
{
	public class NuGenMarshallingDescriptorReader: NuGenBaseSignatureReader
	{
		private NuGenMashallingDescriptorItem marshallingDescriptor;
		public NuGenMashallingDescriptorItem MarshallingDescriptor
		{
			get
			{
				return marshallingDescriptor;
			}
			private set
			{
				marshallingDescriptor = value;
			}
		}

		private int parameterCount;
		public int ParameterCount
		{
			get
			{
				return parameterCount;
			}
			set
			{
				parameterCount = value;
			}
		}

		public NuGenMarshallingDescriptorReader(Dictionary<uint, NuGenTokenBase> allTokens, IntPtr signatureBlob, uint signatureLength, int parameterCount) : base(allTokens, signatureBlob, signatureLength)
		{
			ParameterCount = parameterCount;
		}

		public override void ReadSignature()
		{
			MarshallingDescriptor = ReadNativeType();
		}

		private string ReadString()
		{
			string result = null;

			uint stringLength;
			NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(SignatureBlob, out stringLength));

			if (stringLength > 0)
			{
				byte[] text = new byte[stringLength];

				for (int index = 0; index < stringLength; index++)
				{
					text[index] = Marshal.ReadByte(signatureBlob, index);
				}

				NuGenHelperFunctions.StepIntPtr(ref signatureBlob, stringLength);
				UTF8Encoding encoding = new UTF8Encoding();
				result = encoding.GetString(text, 0, text.Length);
			}

			return result;
		}

		private NuGenMashallingDescriptorItem ReadVariantType()
		{
			NuGenMashallingDescriptorItem result = new NuGenMashallingDescriptorItem();

			uint data;
			NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(SignatureBlob, out data));

			result.VariantType = (VariantType)data;
			result.IsNativeType = false;

			return result;
		}

		private NuGenMashallingDescriptorItem ReadNativeType()
		{
			NuGenMashallingDescriptorItem result = new NuGenMashallingDescriptorItem();

			uint data;
			NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(SignatureBlob, out data));

			result.NativeType = (CorNativeType)data;
			result.IsNativeType = true;

			switch(result.NativeType)
			{
				case CorNativeType.NATIVE_TYPE_SAFEARRAY:
					result.NextItem = ReadVariantType();
					break;

				case CorNativeType.NATIVE_TYPE_CUSTOMMARSHALER:
					result.Guid = ReadString();
					result.UnmanagedType = ReadString();
					result.ManagedType = ReadString();
					result.Cookie = ReadString();
					break;

				case CorNativeType.NATIVE_TYPE_ARRAY:
					result.NextItem = ReadNativeType();

					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(SignatureBlob, out data));
					result.ParamNumber = (int)data;

					if (result.ParamNumber > ParameterCount)
					{
						result.ParamNumber = -1;
					}

					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(SignatureBlob, out data));
					result.ElemMultiply = (int)data;

					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(SignatureBlob, out data));
					result.NumberElem = (int)data;
					break;

				case CorNativeType.NATIVE_TYPE_FIXEDSYSSTRING:
					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(SignatureBlob, out data));
					result.NumberElem = (int)data;
					break;

				case CorNativeType.NATIVE_TYPE_FIXEDARRAY:
					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(SignatureBlob, out data));
					result.NumberElem = (int)data;

					result.NextItem = ReadNativeType();
					break;
			}

			return result;
		}
	}
}