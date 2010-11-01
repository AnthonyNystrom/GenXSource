using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;
using System.Runtime.InteropServices;

namespace Dile.Metadata.Signature
{
	public abstract class NuGenBaseSignatureReader
	{
		private uint signatureLength;
		protected uint SignatureLength
		{
			get
			{
				return signatureLength;
			}
			private set
			{
				signatureLength = value;
			}
		}

		private Dictionary<uint, NuGenTokenBase> allTokens;
		public Dictionary<uint, NuGenTokenBase> AllTokens
		{
			get
			{
				return allTokens;
			}
			private set
			{
				allTokens = value;
			}
		}

		private uint signatureEnd;
		protected uint SignatureEnd
		{
			get
			{
				return signatureEnd;
			}
			private set
			{
				signatureEnd = value;
			}
		}

		protected IntPtr signatureBlob;
		protected IntPtr SignatureBlob
		{
			get
			{
				return signatureBlob;
			}
			private set
			{
				signatureBlob = value;
			}
		}

		private bool hasGenericMethodParameter;
		public bool HasGenericMethodParameter
		{
			get
			{
				return hasGenericMethodParameter;
			}
			protected set
			{
				hasGenericMethodParameter = value;
			}
		}

		public NuGenBaseSignatureReader(Dictionary<uint, NuGenTokenBase> allTokens, IntPtr signatureBlob, uint signatureLength)
		{
			AllTokens = allTokens;
			SignatureBlob = signatureBlob;
			SignatureLength = signatureLength;
			SignatureEnd = Convert.ToUInt32(signatureBlob.ToInt32()) + signatureLength;
		}

		public abstract void ReadSignature();

		public virtual void SetGenericParametersOfMethod(List<NuGenGenericParameter> genericParameters)
		{
		}

		protected NuGenBaseSignatureItem ReadSignatureItem(ref IntPtr signatureBlob)
		{
			NuGenBaseSignatureItem result = null;
			uint data;
			uint dataLength = NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out data);
			CorElementType elementType = (CorElementType)data;

			switch (elementType)
			{
				case CorElementType.ELEMENT_TYPE_CMOD_OPT:
				case CorElementType.ELEMENT_TYPE_CMOD_REQD:
					NuGenCustomModifier customModifier = ReadCustomModifier(ref signatureBlob);
					result = customModifier;
					customModifier.NextItem = ReadSignatureItem(ref signatureBlob);
					break;

				case CorElementType.ELEMENT_TYPE_END:
					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
					result = new NuGenEndSignatureItem();
					break;

				case CorElementType.ELEMENT_TYPE_BYREF:
					NuGenHelperFunctions.StepIntPtr(ref signatureBlob);
					result = ReadSignatureItem(ref signatureBlob);
					NuGenTypeSignatureItem typeItem = null;

					if (result is NuGenArraySignatureItem)
					{
						typeItem = ((NuGenArraySignatureItem)result).Type;
						typeItem.ByRef = true;
					}
					else if (result is NuGenVarSignatureItem)
					{
						NuGenVarSignatureItem varItem = ((NuGenVarSignatureItem)result);
						varItem.ByRef = true;
					}
					else
					{
						while (result is NuGenCustomModifier)
						{
							result = ((NuGenCustomModifier)result).NextItem;
						}

						typeItem = (NuGenTypeSignatureItem)result;
						typeItem.ByRef = true;
					}
					break;

				case CorElementType.ELEMENT_TYPE_TYPEDBYREF:
					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
					NuGenElementSignatureItem elementItem = new NuGenElementSignatureItem();
					elementItem.ElementType = elementType;
					result = elementItem;
					break;

				case CorElementType.ELEMENT_TYPE_PINNED:
					NuGenHelperFunctions.StepIntPtr(ref signatureBlob);
					result = ReadSignatureItem(ref signatureBlob);
					((NuGenTypeSignatureItem)result).Pinned = true;
					break;

				case CorElementType.ELEMENT_TYPE_ARRAY:
					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
					uint value;
					NuGenArraySignatureItem arrayItem = new NuGenArraySignatureItem();
					arrayItem.Type = (NuGenTypeSignatureItem)ReadSignatureItem(ref signatureBlob);
					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out value));
					arrayItem.Rank = value;

					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out value));
					if (value > 0)
					{
						arrayItem.Sizes = new List<uint>(Convert.ToInt32(value));

						for (int index = 0; index < value; index++)
						{
							uint size;

							NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out size));
							arrayItem.Sizes.Add(size);
						}
					}

					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out value));
					if (value > 0)
					{
						arrayItem.LoBounds = new List<uint>(Convert.ToInt32(value));

						for (int index = 0; index < value; index++)
						{
							uint loBound;

							NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out loBound));
							arrayItem.LoBounds.Add(loBound);
						}
					}

					result = arrayItem;
					break;

				case CorElementType.ELEMENT_TYPE_GENERICINST:
					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
					NuGenTypeSignatureItem genericType = (NuGenTypeSignatureItem)ReadType(ref signatureBlob);
					result = genericType;
					uint genericParametersCount = 0;
					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(SignatureBlob, out genericParametersCount));

					if (genericParametersCount > 0)
					{
						genericType.GenericParameters = new List<NuGenBaseSignatureItem>();

						for (uint genericParametersIndex = 0; genericParametersIndex < genericParametersCount; genericParametersIndex++)
						{
							genericType.GenericParameters.Add(ReadSignatureItem(ref signatureBlob));
						}
					}
					break;

				case CorElementType.ELEMENT_TYPE_FNPTR:
					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
					NuGenMethodRefSignatureReader signatureReader = new NuGenMethodRefSignatureReader(AllTokens, signatureBlob, SignatureLength);
					signatureReader.ReadSignature();
					signatureBlob = signatureReader.SignatureBlob;
					result = new NuGenFunctionPointerSignatureItem(signatureReader);
					break;

				case CorElementType.ELEMENT_TYPE_MVAR:
				case CorElementType.ELEMENT_TYPE_VAR:
					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
					byte number = Marshal.ReadByte(signatureBlob);
					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, 1);
					NuGenVarSignatureItem varSignatureItem = new NuGenVarSignatureItem(number, elementType);
					result = varSignatureItem;

					if (!HasGenericMethodParameter && varSignatureItem.MethodParameter)
					{
						HasGenericMethodParameter = true;
					}
					break;

				case CorElementType.ELEMENT_TYPE_MAX:
				case CorElementType.ELEMENT_TYPE_R4_HFA:
				case CorElementType.ELEMENT_TYPE_R8_HFA:
					throw new NotImplementedException(string.Format("Unknown signature element ('{0}').", elementType));

				default:
					result = ReadType(ref signatureBlob);
					break;
			}

			return result;
		}

		protected NuGenCustomModifier ReadCustomModifier(ref IntPtr signatureBlob)
		{
			NuGenCustomModifier result = new NuGenCustomModifier();

			uint data;
			NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out data));

			result.Modifier = (CorElementType)data;
			NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out data));
			result.TypeToken = NuGenSignatureCompression.CorSigUncompressToken(data);

			return result;
		}

		protected NuGenTypeSignatureItem ReadType(ref IntPtr signatureBlob)
		{
			NuGenTypeSignatureItem result = new NuGenTypeSignatureItem();

			uint data;
			NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out data));

			result.ElementType = (CorElementType)data;
			switch (result.ElementType)
			{
				case CorElementType.ELEMENT_TYPE_VALUETYPE:
				case CorElementType.ELEMENT_TYPE_CLASS:
					NuGenHelperFunctions.StepIntPtr(ref signatureBlob, NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out data));
					result.Token = NuGenSignatureCompression.CorSigUncompressToken(data);
					break;

				case CorElementType.ELEMENT_TYPE_SZARRAY:
				case CorElementType.ELEMENT_TYPE_PTR:
					CorElementType elementType = CorElementType.ELEMENT_TYPE_BYREF;
					bool addElementType = false;
					List<NuGenCustomModifier> customModifiers = new List<NuGenCustomModifier>();

					do
					{
						if (addElementType)
						{
							NuGenCustomModifier customModifier = ReadCustomModifier(ref signatureBlob);
							customModifiers.Add(customModifier);
						}

						addElementType = true;
						NuGenSignatureCompression.CorSigUncompressData(signatureBlob, out data);
						elementType = (CorElementType)data;
					} while (elementType == CorElementType.ELEMENT_TYPE_CMOD_OPT || elementType == CorElementType.ELEMENT_TYPE_CMOD_REQD);

					NuGenBaseSignatureItem nextItem = ReadSignatureItem(ref signatureBlob);
					result.NextItem = nextItem;

					if (nextItem.GetType() == typeof(NuGenTypeSignatureItem) && customModifiers.Count > 0)
					{
						((NuGenTypeSignatureItem)nextItem).CustomModifiers = customModifiers;
					}
					break;
			}

			return result;
		}
	}
}