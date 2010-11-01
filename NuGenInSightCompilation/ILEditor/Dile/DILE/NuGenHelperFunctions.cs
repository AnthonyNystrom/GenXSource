using System;
using System.Collections.Generic;
using System.Text;

using Dile.Configuration;
using Dile.Debug;
using Dile.Debug.Expressions;
using Dile.Disassemble;
using Dile.Metadata;
using Dile.Metadata.Signature;
using Dile.UI.Debug;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace Dile
{
	public sealed class NuGenHelperFunctions
	{
		private const int MaximumTextLength = 1024;
		private static double MaximumSingleFraction = 8388608;
		private static double MaximumDoubleFraction = 4503599627370496;

		private const string ConvertTypeName = "System.Convert";
		private const string ToDecimalMethodName = "ToDecimal";

		private static Dictionary<string, string> typeNameAbbreviations = null;
		private static Dictionary<string, string> TypeNameAbbreviations
		{
			get
			{
				return typeNameAbbreviations;
			}
			set
			{
				typeNameAbbreviations = value;
			}
		}

        private static Dictionary<object, object> taggedObjects = new Dictionary<object,object>();
        public static Dictionary<object, object> TaggedObjects
        {
            get
            {
                return taggedObjects;
            }
        }

		private static Dictionary<CorElementType, string> elementTypeAbbreviations = null;
		private static Dictionary<CorElementType, string> ElementTypeAbbreviations
		{
			get
			{
				return NuGenHelperFunctions.elementTypeAbbreviations;
			}
			set
			{
				NuGenHelperFunctions.elementTypeAbbreviations = value;
			}
		}

		private static Dictionary<char, string> escapedCharacters;
		private static Dictionary<char, string> EscapedCharacters
		{
			get
			{
				return escapedCharacters;
			}
			set
			{
				escapedCharacters = value;
			}
		}

		private static Dictionary<char, string> escapedStrings;
		private static Dictionary<char, string> EscapedStrings
		{
			get
			{
				return escapedStrings;
			}
			set
			{
				escapedStrings = value;
			}
		}

		private static List<char> hexaCharacters;
		private static List<char> HexaCharacters
		{
			get
			{
				return hexaCharacters;
			}
			set
			{
				hexaCharacters = value;
			}
		}

		static NuGenHelperFunctions()
		{
			TypeNameAbbreviations = new Dictionary<string, string>();

			TypeNameAbbreviations["bool"] = typeof(bool).FullName;
			TypeNameAbbreviations["char"] = typeof(char).FullName;
			TypeNameAbbreviations["sbyte"] = typeof(sbyte).FullName;
			TypeNameAbbreviations["byte"] = typeof(byte).FullName;
			TypeNameAbbreviations["short"] = typeof(short).FullName;
			TypeNameAbbreviations["ushort"] = typeof(ushort).FullName;
			TypeNameAbbreviations["int"] = typeof(int).FullName;
			TypeNameAbbreviations["uint"] = typeof(uint).FullName;
			TypeNameAbbreviations["long"] = typeof(long).FullName;
			TypeNameAbbreviations["ulong"] = typeof(ulong).FullName;
			TypeNameAbbreviations["float"] = typeof(float).FullName;
			TypeNameAbbreviations["double"] = typeof(double).FullName;
			TypeNameAbbreviations["decimal"] = typeof(decimal).FullName;
			TypeNameAbbreviations["object"] = typeof(object).FullName;
			TypeNameAbbreviations["string"] = typeof(string).FullName;

			ElementTypeAbbreviations = new Dictionary<CorElementType, string>();
			ElementTypeAbbreviations[CorElementType.ELEMENT_TYPE_BOOLEAN] = typeof(bool).FullName;
			ElementTypeAbbreviations[CorElementType.ELEMENT_TYPE_CHAR] = typeof(char).FullName;
			ElementTypeAbbreviations[CorElementType.ELEMENT_TYPE_I1] = typeof(sbyte).FullName;
			ElementTypeAbbreviations[CorElementType.ELEMENT_TYPE_U1] = typeof(byte).FullName;
			ElementTypeAbbreviations[CorElementType.ELEMENT_TYPE_I2] = typeof(short).FullName;
			ElementTypeAbbreviations[CorElementType.ELEMENT_TYPE_U2] = typeof(ushort).FullName;
			ElementTypeAbbreviations[CorElementType.ELEMENT_TYPE_I4] = typeof(int).FullName;
			ElementTypeAbbreviations[CorElementType.ELEMENT_TYPE_U4] = typeof(uint).FullName;
			ElementTypeAbbreviations[CorElementType.ELEMENT_TYPE_I8] = typeof(long).FullName;
			ElementTypeAbbreviations[CorElementType.ELEMENT_TYPE_U8] = typeof(ulong).FullName;
			ElementTypeAbbreviations[CorElementType.ELEMENT_TYPE_R4] = typeof(float).FullName;
			ElementTypeAbbreviations[CorElementType.ELEMENT_TYPE_R8] = typeof(double).FullName;
			ElementTypeAbbreviations[CorElementType.ELEMENT_TYPE_OBJECT] = typeof(object).FullName;
			ElementTypeAbbreviations[CorElementType.ELEMENT_TYPE_STRING] = typeof(string).FullName;

			EscapedCharacters = new Dictionary<char, string>(11);
			EscapedCharacters['\''] = "'";
			EscapedCharacters['"'] = "\"";
			EscapedCharacters['\\'] = "\\";
			EscapedCharacters['0'] = "\0";
			EscapedCharacters['a'] = "\a";
			EscapedCharacters['b'] = "\b";
			EscapedCharacters['f'] = "\f";
			EscapedCharacters['n'] = "\n";
			EscapedCharacters['r'] = "\r";
			EscapedCharacters['t'] = "\t";
			EscapedCharacters['v'] = "\v";

			EscapedStrings = new Dictionary<char, string>(11);
			EscapedStrings['\''] = "\\'";
			EscapedStrings['"'] = "\\\"";
			EscapedStrings['\\'] = "\\\\";
			EscapedStrings['\0'] = "\\0";
			EscapedStrings['\a'] = "\\a";
			EscapedStrings['\b'] = "\\b";
			EscapedStrings['\f'] = "\\f";
			EscapedStrings['\n'] = "\\n";
			EscapedStrings['\r'] = "\\r";
			EscapedStrings['\t'] = "\\t";
			EscapedStrings['\v'] = "\\v";

			HexaCharacters = new List<char>(6);
			HexaCharacters.Add('a');
			HexaCharacters.Add('b');
			HexaCharacters.Add('c');
			HexaCharacters.Add('d');
			HexaCharacters.Add('e');
			HexaCharacters.Add('f');
		}

		private NuGenHelperFunctions()
		{
		}

		public static string GetString(char[] charArray, uint startPosition, uint length)
		{
			StringBuilder result = new StringBuilder();
			result.Append(charArray, Convert.ToInt32(startPosition), Convert.ToInt32(length));

			return result.ToString().TrimEnd('\0');
		}

		public static Dictionary<uint, NuGenMemberReference> GetMemberReferences(NuGenAssembly assembly, uint tokenParent)
		{
			Dictionary<uint, NuGenMemberReference> result = null;
			IntPtr enumHandle = IntPtr.Zero;
			uint[] memberRefs = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			assembly.Import.EnumMemberRefs(ref enumHandle, tokenParent, memberRefs, Convert.ToUInt32(memberRefs.Length), out count);

			if (count > 0)
			{
				result = new Dictionary<uint, NuGenMemberReference>();
			}

			while (count > 0)
			{
				for (uint memberRefsIndex = 0; memberRefsIndex < count; memberRefsIndex++)
				{
					uint token = memberRefs[memberRefsIndex];
					uint memberRefNameLength;
					uint typeDefToken;
					IntPtr signatureBlob;
					uint signatureBlobLength;

					assembly.Import.GetMemberRefProps(token, out typeDefToken, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out memberRefNameLength, out signatureBlob, out signatureBlobLength);

					if (memberRefNameLength > NuGenProject.DefaultCharArray.Length)
					{
						NuGenProject.DefaultCharArray = new char[memberRefNameLength];

						assembly.Import.GetMemberRefProps(token, out typeDefToken, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out memberRefNameLength, out signatureBlob, out signatureBlobLength);
					}

					NuGenMemberReference memberReference = new NuGenMemberReference(assembly, GetString(NuGenProject.DefaultCharArray, 0, memberRefNameLength), token, typeDefToken, signatureBlob, signatureBlobLength);
					result[token] = memberReference;
					assembly.AllTokens[token] = memberReference;
				}

				assembly.Import.EnumMemberRefs(ref enumHandle, tokenParent, memberRefs, Convert.ToUInt32(memberRefs.Length), out count);
			}

			assembly.Import.CloseEnum(enumHandle);

			return result;
		}

		public static ulong GetILCodeParameter(byte[] ilCode, int offset, int parameterSize)
		{
			ulong result = 0;
			int ilCodeIndex = 0;

			while (ilCodeIndex < parameterSize)
			{
				result += ilCode[ilCodeIndex + offset] * (ulong)Math.Pow(0x100, ilCodeIndex);
				ilCodeIndex++;
			}

			return result;
		}

		public static void StepIntPtr(ref IntPtr pointer)
		{
			StepIntPtr(ref pointer, 1);
		}

		public static void StepIntPtr(ref IntPtr pointer, uint count)
		{
			StepIntPtr(ref pointer, Convert.ToInt32(count));
		}

		public static void StepIntPtr(ref IntPtr pointer, int count)
		{
			pointer = new IntPtr(pointer.ToInt32() + count);
		}

		public static void SetSignatureItemToken(NuGenTypeDefinition baseTypeDefinition, NuGenTypeSignatureItem typeSignatureItem)
		{
			SetSignatureItemToken(baseTypeDefinition.ModuleScope.Assembly.AllTokens, typeSignatureItem);
		}

		public static void SetSignatureItemToken(Dictionary<uint, NuGenTokenBase> allTokens, NuGenTypeSignatureItem typeSignatureItem)
		{
			if (typeSignatureItem.ElementType == CorElementType.ELEMENT_TYPE_VALUETYPE || typeSignatureItem.ElementType == CorElementType.ELEMENT_TYPE_CLASS || typeSignatureItem.ElementType == CorElementType.ELEMENT_TYPE_FNPTR)
			{
				typeSignatureItem.TokenObject = allTokens[typeSignatureItem.Token];
			}

			if (typeSignatureItem.NextItem != null)
			{
				SetSignatureItemToken(allTokens, typeSignatureItem.NextItem);
			}

			if (typeSignatureItem.CustomModifiers != null)
			{
				foreach(NuGenCustomModifier customModifier in typeSignatureItem.CustomModifiers)
				{
					SetSignatureItemToken(allTokens, customModifier);
				}
			}

			if (typeSignatureItem.GenericParameters != null)
			{
				foreach (NuGenBaseSignatureItem genericParameter in typeSignatureItem.GenericParameters)
				{
					SetSignatureItemToken(allTokens, genericParameter);
				}
			}
		}

		public static void SetSignatureItemToken(Dictionary<uint, NuGenTokenBase> allTokens, NuGenCustomModifier customModifier)
		{
			customModifier.TokenObject = allTokens[customModifier.TypeToken];

			if (customModifier.NextItem != null)
			{
				Type nextItemType = customModifier.NextItem.GetType();

				if (nextItemType == typeof(NuGenCustomModifier))
				{
					SetSignatureItemToken(allTokens, (NuGenCustomModifier)customModifier.NextItem);
				}
				else if (nextItemType == typeof(NuGenTypeSignatureItem))
				{
					SetSignatureItemToken(allTokens, (NuGenTypeSignatureItem)customModifier.NextItem);
				}
			}
		}

		public static void SetSignatureItemToken(Dictionary<uint, NuGenTokenBase> allTokens, NuGenBaseSignatureItem signatureItem)
		{
			SetSignatureItemToken(allTokens, signatureItem, null);
		}

		public static void SetSignatureItemToken(Dictionary<uint, NuGenTokenBase> allTokens, NuGenBaseSignatureItem signatureItem, List<NuGenGenericParameter> genericParameters)
		{
			if (signatureItem != null)
			{
				Type signatureItemType = signatureItem.GetType();

				if (signatureItemType == typeof(NuGenTypeSignatureItem))
				{
					SetSignatureItemToken(allTokens, (NuGenTypeSignatureItem)signatureItem);
				}
				else if (signatureItemType == typeof(NuGenCustomModifier))
				{
					SetSignatureItemToken(allTokens, (NuGenCustomModifier)signatureItem);
				}
				else if (signatureItemType == typeof(NuGenArraySignatureItem))
				{
					SetSignatureItemToken(allTokens, ((NuGenArraySignatureItem)signatureItem).Type);
				}
				else if (signatureItemType == typeof(NuGenVarSignatureItem) && genericParameters != null)
				{
					NuGenVarSignatureItem varSignatureItem = (NuGenVarSignatureItem)signatureItem;

					varSignatureItem.GenericParameter = genericParameters[varSignatureItem.Number];
				}
			}
		}

		private static double ConvertToFraction(ulong fraction, double maximumFraction)
		{	
			return fraction / maximumFraction;
		}

		public static float ConvertToSingle(ulong value)
		{
			float result = 0;
			//0x8000 0000 == 1000 0000 0000 0000 0000 0000 0000 0000
			long sign = ((value & 0x80000000) == 1 ? -1 : 1);
			//0x7F80 0000 == 0111 1111 1000 0000 0000 0000 0000 0000
			//0x0080 0000 == 0000 0000 1000 0000 0000 0000 0000 0000
			double exponent = (value & 0x7F800000) / 0x800000;
			//0x007F FFFF == 0000 0000 0111 1111 1111 1111 1111 1111
			ulong fraction = value & 0x7FFFFF;

			if (exponent > 0 && exponent < 255)
			{
				result = Convert.ToSingle(sign * Math.Pow(2, exponent - 127) * (1 + ConvertToFraction(fraction, MaximumSingleFraction)));
			}
			else if (exponent == 0)
			{
				if (fraction != 0)
				{
					result = Convert.ToSingle(sign * Math.Pow(2, exponent - 126) * (ConvertToFraction(fraction, MaximumSingleFraction)));
				}
				else
				{
					result = 0;
				}
			}
			else if (exponent == 255)
			{
				if (fraction == 0)
				{
					if (sign == 0)
					{
						result = float.PositiveInfinity;
					}
					else
					{
						result = float.NegativeInfinity;
					}
				}
				else
				{
					result = float.NaN;
				}
			}

			return result;
		}

		public static double ConvertToDouble(ulong value)
		{
			double result = 0;
			//0x8000 0000 0000 0000 == 1000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000
			long sign = ((value & 0x8000000000000000) == 1 ? -1 : 1);
			//0x7FF0 0000 0000 0000 == 0111 1111 1111 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000
			//0x0010 0000 0000 0000 == 0000 0000 0001 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000
			double exponent = (value & 0x7FF0000000000000) / 0x10000000000000;
			//0x000F FFFF FFFF FFFF == 0000 0000 0000 1111 1111 1111 1111 1111 1111 1111 1111 1111 1111 1111 1111 1111
			ulong fraction = value & 0xFFFFFFFFFFFFF;

			if (exponent > 0 && exponent < 2047)
			{
				result = sign * Math.Pow(2, exponent - 1023) * (1 + ConvertToFraction(fraction, MaximumDoubleFraction));
			}
			else if (exponent == 0)
			{
				if (fraction != 0)
				{
					result = sign * Math.Pow(2, exponent - 1022) * (ConvertToFraction(fraction, MaximumDoubleFraction));
				}
				else
				{
					result = 0;
				}
			}
			else if (exponent == 2047)
			{
				if (fraction == 0)
				{
					if (sign == 0)
					{
						result = float.PositiveInfinity;
					}
					else
					{
						result = float.NegativeInfinity;
					}
				}
				else
				{
					result = float.NaN;
				}
			}

			return result;
		}

		public static bool EnumContainsValue(Enum enumValue, Enum expectedValue)
		{
			int expectedValueAsInt = Convert.ToInt32(expectedValue);

			return ((Convert.ToInt32(enumValue) & expectedValueAsInt) == expectedValueAsInt);
		}

		public static string EnumAsString(Enum enumValue, Enum expectedValue, string name)
		{
			return (EnumContainsValue(enumValue, expectedValue) ? name : string.Empty);
		}

		public static void AddWordToStringBuilder(StringBuilder stringBuilder, string expression)
		{
			if (expression != null && expression.Length > 0)
			{
				stringBuilder.Append(expression);
			}
		}

		public static List<NuGenCustomAttribute> EnumCustomAttributes(NuGenIMetaDataImport2 import, NuGenAssembly assembly, NuGenTokenBase owner)
		{
			List<NuGenCustomAttribute> result = null;

			IntPtr enumHandle = IntPtr.Zero;
			uint[] tokens = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			import.EnumCustomAttributes(ref enumHandle, owner.Token, 0, tokens, Convert.ToUInt32(tokens.Length), out count);

			if (count > 0)
			{
				result = new List<NuGenCustomAttribute>();
			}

			while (count > 0)
			{
				for (uint tokensIndex = 0; tokensIndex < count; tokensIndex++)
				{
					uint token = tokens[tokensIndex];
					uint ownerToken;
					uint typeToken;
					IntPtr blob;
					uint blobLength;

					import.GetCustomAttributeProps(token, out ownerToken, out typeToken, out blob, out blobLength);
					NuGenCustomAttribute customAttribute = new NuGenCustomAttribute(import, token, owner, typeToken, blob, blobLength);
					assembly.AllTokens[token] = customAttribute;
					result.Add(customAttribute);
				}

				import.EnumCustomAttributes(ref enumHandle, owner.Token, 0, tokens, Convert.ToUInt32(tokens.Length), out count);
			}

			import.CloseEnum(enumHandle);

			return result;
		}

		public static string GetCallingConventionName(CorCallingConvention callingConvention)
		{
			string result = string.Empty;

			if (callingConvention != CorCallingConvention.IMAGE_CEE_CS_CALLCONV_GENERIC && ((callingConvention & CorCallingConvention.IMAGE_CEE_CS_CALLCONV_GENERIC) == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_GENERIC))
			{
				callingConvention -= CorCallingConvention.IMAGE_CEE_CS_CALLCONV_GENERIC;
			}

			switch (callingConvention)
			{
				case CorCallingConvention.IMAGE_CEE_UNMANAGED_CALLCONV_STDCALL:
					result = "unmanaged stdcall";
					break;

				case CorCallingConvention.IMAGE_CEE_UNMANAGED_CALLCONV_C:
					result = "unmanaged cdecl";
					break;

				case CorCallingConvention.IMAGE_CEE_UNMANAGED_CALLCONV_THISCALL:
					result = "unmanaged thiscall";
					break;

				case CorCallingConvention.IMAGE_CEE_UNMANAGED_CALLCONV_FASTCALL:
					result = "unmanaged fastcall";
					break;

				case CorCallingConvention.IMAGE_CEE_CS_CALLCONV_HASTHIS:
					result = "instance";
					break;

				case CorCallingConvention.IMAGE_CEE_CS_CALLCONV_EXPLICITTHIS:
					result = "explicit";
					break;

				case CorCallingConvention.IMAGE_CEE_CS_CALLCONV_VARARG:
					result = "vararg";
					break;

				case CorCallingConvention.IMAGE_CEE_CS_CALLCONV_DEFAULT:
					break;

				case CorCallingConvention.IMAGE_CEE_CS_CALLCONV_FIELD:
				case CorCallingConvention.IMAGE_CEE_CS_CALLCONV_LOCAL_SIG:
				case CorCallingConvention.IMAGE_CEE_CS_CALLCONV_GENERIC:
				case CorCallingConvention.IMAGE_CEE_CS_CALLCONV_GENERICINST:
					break;

				default:
					throw new NotImplementedException(string.Format("Unknown calling convention ('{0}'))", callingConvention));
			}

			return result;
		}

		public static byte[] ReadBlobAsByteArray(IntPtr blob, uint blobLength)
		{
			byte[] result = new byte[blobLength];
			int index = 0;

			while (index < blobLength)
			{
				result[index] = Marshal.ReadByte(blob, index);
				index++;
			}

			return result;
		}

		public static string ReadBlobAsString(IntPtr blob, uint blobLength)
		{
			StringBuilder result = new StringBuilder();

			result.Append("(");
			int index = 0;

			while (index < blobLength)
			{
				result.Append(NuGenHelperFunctions.FormatAsHexNumber(Marshal.ReadByte(blob, index), 2));
				index++;

				if (index < blobLength)
				{
					result.Append(" ");
				}
			}

			result.Append(")");

			return result.ToString();
		}

		public static string QuoteMethodName(string methodName)
		{
			return NuGenHelperFunctions.QuoteName(methodName, ".cctor", ".ctor", "{dtor}");
		}

		public static string QuoteName(string name)
		{
			return (NuGenOpCodeGroups.Keywords.Contains(name) ? string.Format("'{0}'", name) : name);
		}

		public static string QuoteName(string name, params string[] exceptions)
		{
			string result = name;

			if (exceptions != null && exceptions.Length > 0 && Array.IndexOf(exceptions, name) < 0)
			{
				result = QuoteName(name);
			}

			return result;
		}

		public static string ReadDefaultValue(CorElementType defaultValueType, IntPtr defaultValue, uint defaultValueLength, out object numericValue)
		{
			StringBuilder result = new StringBuilder();
			numericValue = null;
			
			switch (defaultValueType)
			{
				case CorElementType.ELEMENT_TYPE_BOOLEAN:
					result.Append("bool(");
					result.Append(Convert.ToString(Marshal.ReadByte(defaultValue) == 1).ToLower());
					result.Append(")");
					break;

				case CorElementType.ELEMENT_TYPE_I1:
					result.Append("int8(0x");
					sbyte sbyteNumber = (sbyte)Marshal.ReadByte(defaultValue);
					numericValue = sbyteNumber;
					result.Append(sbyteNumber.ToString("x"));
					result.Append(")");
					break;

				case CorElementType.ELEMENT_TYPE_I2:
					result.Append("int16(0x");
					short shortNumber = Marshal.ReadInt16(defaultValue);
					numericValue = shortNumber;
					result.Append(shortNumber.ToString("x"));
					result.Append(")");
					break;

				case CorElementType.ELEMENT_TYPE_I4:
					result.Append("int32(0x");
					int intNumber = Marshal.ReadInt32(defaultValue);
					numericValue = intNumber;
					result.Append(intNumber.ToString("x"));
					result.Append(")");
					break;

				case CorElementType.ELEMENT_TYPE_I8:
					result.Append("int64(0x");
					long longNumber = Marshal.ReadInt64(defaultValue);
					numericValue = longNumber;
					result.Append(longNumber.ToString("x"));
					result.Append(")");
					break;

				case CorElementType.ELEMENT_TYPE_U1:
					result.Append("uint8(0x");
					byte byteNumber = Marshal.ReadByte(defaultValue);
					numericValue = byteNumber;
					result.Append(byteNumber.ToString("x"));
					result.Append(")");
					break;

				case CorElementType.ELEMENT_TYPE_U2:
					result.Append("uint16(0x");
					ushort ushortNumber = (ushort)Marshal.ReadInt16(defaultValue);
					numericValue = ushortNumber;
					result.Append(ushortNumber.ToString("x"));
					result.Append(")");
					break;

				case CorElementType.ELEMENT_TYPE_U4:
					result.Append("uint32(0x");
					uint uintNumber = (uint)Marshal.ReadInt32(defaultValue);
					numericValue = uintNumber;
					result.Append(uintNumber.ToString("x"));
					result.Append(")");
					break;

				case CorElementType.ELEMENT_TYPE_U8:
					result.Append("uint64(0x");
					ulong ulongNumber = (ulong)Marshal.ReadInt64(defaultValue);
					numericValue = ulongNumber;
					result.Append(ulongNumber.ToString("x"));
					result.Append(")");
					break;

				case CorElementType.ELEMENT_TYPE_R4:
					result.Append("float32(");
					result.Append(NuGenHelperFunctions.ConvertToSingle((ulong)Marshal.ReadInt32(defaultValue)));
					result.Append(")");
					break;

				case CorElementType.ELEMENT_TYPE_R8:
					result.Append("float64(");
					result.Append(NuGenHelperFunctions.ConvertToDouble((ulong)Marshal.ReadInt64(defaultValue)));
					result.Append(")");
					break;

				case CorElementType.ELEMENT_TYPE_STRING:
					result.Append("\"");
					result.Append(Marshal.PtrToStringUni(defaultValue, Convert.ToInt32(defaultValueLength)));
					result.Append("\"");
					break;

				case CorElementType.ELEMENT_TYPE_CLASS:
					int value = Marshal.ReadInt32(defaultValue);

					if (value != 0)
					{
						throw new NotImplementedException("Default value is given for a class.");
					}

					result.Append("nullref");
					break;

				case CorElementType.ELEMENT_TYPE_CHAR:
					result.Append("char(0x");
					shortNumber = Marshal.ReadInt16(defaultValue);
					numericValue = shortNumber;
					result.Append(shortNumber.ToString("x"));
					result.Append(")");
					break;

				default:
					throw new NotImplementedException(string.Format("Unknown element type ('{0}').", defaultValueType));
			}

			return result.ToString();
		}

		public static NuGenMashallingDescriptorItem ReadMarshalDescriptor(NuGenIMetaDataImport2 import, Dictionary<uint, NuGenTokenBase> allTokens, uint token, int parameterCount)
		{
			IntPtr signatureBlob;
			uint signatureBlobLength;

			import.GetFieldMarshal(token, out signatureBlob, out signatureBlobLength);
			NuGenMarshallingDescriptorReader reader = new NuGenMarshallingDescriptorReader(allTokens, signatureBlob, signatureBlobLength, parameterCount);

			reader.ReadSignature();

			return reader.MarshallingDescriptor;
		}

		private static string PinvokeMapAsString(CorPinvokeMap pinvokeMap)
		{
			StringBuilder result = new StringBuilder();

			if ((pinvokeMap & CorPinvokeMap.pmSupportsLastError) == CorPinvokeMap.pmSupportsLastError)
			{
				result.Append("lasterr ");
			}

			CorPinvokeMap callingConvention = pinvokeMap & CorPinvokeMap.pmCallConvMask;

			if (callingConvention != CorPinvokeMap.pmCallConvMask)
			{
				switch(callingConvention)
				{
					case CorPinvokeMap.pmCallConvCdecl:
						result.Append("cdecl ");
						break;

					case CorPinvokeMap.pmCallConvFastcall:
						result.Append("fastcall ");
						break;

					case CorPinvokeMap.pmCallConvStdcall:
						result.Append("stdcall ");
						break;

					case CorPinvokeMap.pmCallConvThiscall:
						result.Append("thiscall ");
						break;

					case CorPinvokeMap.pmCallConvWinapi:
						result.Append("winapi ");
						break;
				}
			}

			CorPinvokeMap charSet = pinvokeMap & CorPinvokeMap.pmCharSetMask;

			if (charSet == CorPinvokeMap.pmCharSetMask)
			{
				result.Append("autochar ");
			}
			else
			{
				switch(charSet)
				{
					case CorPinvokeMap.pmCharSetAnsi:
						result.Append("ansi ");
						break;
						
					case CorPinvokeMap.pmCharSetAuto:
						result.Append("auto ");
						break;

					case CorPinvokeMap.pmCharSetUnicode:
						result.Append("unicode ");
						break;
				}
			}

			CorPinvokeMap bestFit = pinvokeMap & CorPinvokeMap.pmBestFitMask;

			if (bestFit == CorPinvokeMap.pmBestFitEnabled)
			{
				result.Append("bestfit:on");
			}
			else if (bestFit == CorPinvokeMap.pmBestFitDisabled)
			{
				result.Append("bestfit:off");
			}

			CorPinvokeMap charMapError = pinvokeMap & CorPinvokeMap.pmThrowOnUnmappableCharMask;

			if (charMapError == CorPinvokeMap.pmThrowOnUnmappableCharEnabled)
			{
				result.Append("charmaperror:on");
			}
			else if (charMapError == CorPinvokeMap.pmThrowOnUnmappableCharDisabled)
			{
				result.Append("charmaperror:off");
			}

			if ((pinvokeMap & CorPinvokeMap.pmNoMangle) == CorPinvokeMap.pmNoMangle)
			{
				result.Append("nomangle ");
			}

			if ((pinvokeMap & CorPinvokeMap.pmMaxValue) == CorPinvokeMap.pmMaxValue)
			{
				throw new NotImplementedException("pmMaxValue found.");
			}

			return result.ToString().TrimEnd();
		}

		public static string ReadPinvokeMap(NuGenIMetaDataImport2 import, NuGenAssembly assembly, uint token)
		{
			string result = null;
			uint mappingFlags;
			uint methodNameCount;
			uint moduleReferenceToken;

			try
			{
				import.GetPinvokeMap(token, out mappingFlags, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out methodNameCount, out moduleReferenceToken);

				if (methodNameCount > NuGenProject.DefaultCharArray.Length)
				{
					NuGenProject.DefaultCharArray = new char[methodNameCount];

					import.GetPinvokeMap(token, out mappingFlags, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out methodNameCount, out moduleReferenceToken);
				}

				NuGenModuleReference moduleReference = (NuGenModuleReference)assembly.AllTokens[moduleReferenceToken];

				if (moduleReference.Name == null || moduleReference.Name.Length == 0)
				{
					result = PinvokeMapAsString((CorPinvokeMap)mappingFlags);
				}
				else
				{
					result = "\"" + moduleReference.Name + "\" " + PinvokeMapAsString((CorPinvokeMap)mappingFlags);
				}
			}
			catch (COMException)
			{
			}

			return result;
		}

		public static List<NuGenPermissionSet> EnumPermissionSets(NuGenIMetaDataImport2 import, uint token)
		{
			List<NuGenPermissionSet> result = null;
			IntPtr enumHandle = IntPtr.Zero;
			uint[] permissionSets = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			import.EnumPermissionSets(ref enumHandle, token, 0, permissionSets, Convert.ToUInt32(permissionSets.Length), out count);

			if (count > 0)
			{
				result = new List<NuGenPermissionSet>();

				while (count > 0)
				{
					for (uint index = 0; index < count; index++)
					{
						uint permissionSetToken = permissionSets[index];
						uint securityFlag;
						IntPtr signatureBlob;
						uint signatureBlobLength;

						import.GetPermissionSetProps(permissionSetToken, out securityFlag, out signatureBlob, out signatureBlobLength);

						NuGenPermissionSet permissionSet = new NuGenPermissionSet(permissionSetToken, securityFlag, signatureBlob, signatureBlobLength);
						result.Add(permissionSet);
					}

					import.EnumPermissionSets(ref enumHandle, token, 0, permissionSets, Convert.ToUInt32(permissionSets.Length), out count);
				}

				import.CloseEnum(enumHandle);
			}

			return result;
		}

		private static void ReplaceCharacter(StringBuilder stringBuilder, int index, string newCharacter)
		{
			stringBuilder.Remove(index, 1);
			stringBuilder.Insert(index, newCharacter);
		}

		public static string ShowEscapeCharacters(string original)
		{
			return ShowEscapeCharacters(original, false);
		}

		public static string ShowEscapeCharacters(string original, bool quoteString)
		{
			StringBuilder result = new StringBuilder(original);

			for (int index = 0; index < result.Length; index++)
			{
				char currentChar = result[index];

				if (EscapedStrings.ContainsKey(currentChar))
				{
					ReplaceCharacter(result, index++, EscapedStrings[currentChar]);
				}
			}

			if (quoteString)
			{
				result.Insert(0, '"');
				result.Append('"');
			}

			return result.ToString();
		}

		public static string ConvertEscapedCharacters(string text, bool removeQuoteCharacters)
		{
			StringBuilder result = new StringBuilder();

			if (text.Length > 0)
			{
				result.Append(ConvertEscapedCharacters(text));

				if (removeQuoteCharacters && result[0] == '\"')
				{
					result.Remove(0, 1);
				}

				if (removeQuoteCharacters && result.Length > 0 && result[result.Length - 1] == '\"')
				{
					result.Remove(result.Length - 1, 1);
				}
			}

			return result.ToString();
		}

		private static string ConvertEscapedCharacters(string text)
		{
			StringBuilder result = new StringBuilder();
			int index = 0;

			while (index < text.Length)
			{
				int parsedCharLength;
				string parsedChar = NuGenHelperFunctions.ParseEscapedCharacter(text, index, out parsedCharLength);

				if (string.IsNullOrEmpty(parsedChar))
				{
					char character = text[index];

					if (character == '\\')
					{
						throw new InvalidOperationException("Unrecognized escape sequence encountered in the string: " + text);
					}
					else
					{
						result.Append(character);
						index++;
					}
				}
				else
				{
					result.Append(parsedChar);
					index += parsedCharLength;
				}
			}

			return result.ToString();
		}

		public static char? ParseHexaCharacter(string text, int startIndex)
		{
			char? result = null;

			if (startIndex < text.Length)
			{
				char character = text[startIndex];

				if (char.IsDigit(character))
				{
					result = character;
				}
				else
				{
					if (HexaCharacters.Contains(char.ToLowerInvariant(character)))
					{
						result = character;
					}
				}
			}

			return result;
		}

		public static string ParseEscapedCharacter(string text, int startIndex, out int parsedCharLength)
		{
			string result = null;
			parsedCharLength = 0;

			if (startIndex >= text.Length)
			{
				throw new ArgumentException("StartIndex cannot be greater than the length of text.", "startIndex");
			}

			if (startIndex + 1 < text.Length && text[startIndex] == '\\')
			{
				int position = startIndex + 1;
				char character = text[position];

				if (EscapedCharacters.ContainsKey(character))
				{
					result = EscapedCharacters[character];
					parsedCharLength = 2;
				}
				else
				{
					switch (character)
					{
						case 'u':
							if (position + 4 < text.Length)
							{
								int hexaCharCode;

								if (int.TryParse(text.Substring(position + 1, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out hexaCharCode))
								{
									result = Convert.ToString((char)hexaCharCode);
									parsedCharLength = 6;
								}
							}
							break;

						case 'x':
							if (position + 1 < text.Length)
							{
								position++;
								char? hexaCharacter = ParseHexaCharacter(text, position);

								if (hexaCharacter.HasValue)
								{
									string hexaString = string.Empty;
									int index = 0;

									while (hexaCharacter.HasValue && index < 4)
									{
										hexaString += Convert.ToString(hexaCharacter.Value);
										index++;
										hexaCharacter = ParseHexaCharacter(text, position + index);
									}

									int hexaCharCode;

									if (int.TryParse(hexaString, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out hexaCharCode))
									{
										result = Convert.ToString((char)hexaCharCode);
										parsedCharLength = index + 2;
									}
								}
							}
							break;

						case 'U':
							if (position + 8 < text.Length)
							{
								int hexaCharCode;

								if (int.TryParse(text.Substring(position + 1, 8), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out hexaCharCode))
								{
									try
									{
										result = Char.ConvertFromUtf32(hexaCharCode);
										parsedCharLength = 10;
									}
									catch
									{
									}
								}
							}
							break;
					}
				}
			}

			return result;
		}

		public static List<NuGenGenericParameter> EnumGenericParameters(NuGenIMetaDataImport2 import, NuGenAssembly assembly, NuGenTokenBase owner)
		{
			List<NuGenGenericParameter> result = null;

			IntPtr enumHandle = IntPtr.Zero;
			uint[] tokens = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			import.EnumGenericParams(ref enumHandle, owner.Token, tokens, Convert.ToUInt32(tokens.Length), out count);

			if (count > 0)
			{
				result = new List<NuGenGenericParameter>();
			}

			while (count > 0)
			{
				for (uint tokensIndex = 0; tokensIndex < count; tokensIndex++)
				{
					uint token = tokens[tokensIndex];
					uint sequence;
					uint attributes;
					uint tokenOfOwner;
					uint kind;
					uint nameCount;

					import.GetGenericParamProps(token, out sequence, out attributes, out tokenOfOwner, out kind, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out nameCount);

					if (nameCount > NuGenProject.DefaultCharArray.Length)
					{
						NuGenProject.DefaultCharArray = new char[nameCount];

						import.GetGenericParamProps(token, out sequence, out attributes, out tokenOfOwner, out kind, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out nameCount);
					}

					NuGenGenericParameter genericParameter = new NuGenGenericParameter(import, token, GetString(NuGenProject.DefaultCharArray, 0, nameCount), sequence, attributes, owner, kind);
					assembly.AllTokens[token] = genericParameter;
					result.Add(genericParameter);
				}

				import.EnumGenericParams(ref enumHandle, owner.Token, tokens, Convert.ToUInt32(tokens.Length), out count);
			}

			import.CloseEnum(enumHandle);

			if (result != null)
			{
				result.Sort();
			}

			return result;
		}

		public static List<NuGenMethodSpec> EnumMethodSpecs(NuGenIMetaDataImport2 import, NuGenAssembly assembly, NuGenTokenBase owner)
		{
			List<NuGenMethodSpec> result = null;

			IntPtr enumHandle = IntPtr.Zero;
			uint[] tokens = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			import.EnumMethodSpecs(ref enumHandle, owner.Token, tokens, Convert.ToUInt32(tokens.Length), out count);

			if (count > 0)
			{
				result = new List<NuGenMethodSpec>();
			}

			while (count > 0)
			{
				for (uint tokensIndex = 0; tokensIndex < count; tokensIndex++)
				{
					uint token = tokens[tokensIndex];
					uint tokenOfParent;
					IntPtr signature;
					uint signatureLength;

					import.GetMethodSpecProps(token, out tokenOfParent, out signature, out signatureLength);

					NuGenMethodSpec methodSpec = new NuGenMethodSpec(token, owner, signature, signatureLength, assembly);
					assembly.AllTokens[token] = methodSpec;
					result.Add(methodSpec);
				}

				import.EnumMethodSpecs(ref enumHandle, owner.Token, tokens, Convert.ToUInt32(tokens.Length), out count);
			}

			import.CloseEnum(enumHandle);

			return result;
		}

		public static string TrimString(char[] charArray)
		{
			string result = string.Empty;

			int index = 0;

			while (index < charArray.Length && charArray[index++] != '\0')
			{
			}

			result = new string(charArray, 0, index - 1);

			return result;
		}

		public static NuGenTokenBase FindObjectByToken(uint token, ModuleWrapper module)
		{
			string moduleName = string.Empty;
			bool isInMemoryModule = module.IsInMemory();

			if (isInMemoryModule)
			{
				moduleName = module.GetNameFromMetaData();
			}
			else
			{
				moduleName = module.GetName();

				try
				{
					moduleName = Path.GetFileNameWithoutExtension(moduleName);
				}
				catch
				{
				}
			}

			moduleName = moduleName.ToLower();

			return InternalFindObjectByToken(token, moduleName);
		}

		public static NuGenTokenBase FindObjectByToken(uint token, string moduleName, bool isInMemoryModule)
		{
			if (!isInMemoryModule)
			{
				try
				{
					moduleName = Path.GetFileNameWithoutExtension(moduleName);
				}
				catch
				{
				}
			}

			moduleName = moduleName.ToLower();

			return InternalFindObjectByToken(token, moduleName);
		}

		private static NuGenTokenBase InternalFindObjectByToken(uint token, string moduleName)
		{
			NuGenTokenBase result = null;
			int index = 0;

			while (result == null && index < NuGenProject.Instance.Assemblies.Count)
			{
				NuGenAssembly assembly = NuGenProject.Instance.Assemblies[index++];
				bool isInMemoryAssembly = assembly.IsInMemory;

				if (((isInMemoryAssembly && assembly.ModuleScope.Name.ToLower() == moduleName) || (!isInMemoryAssembly && Path.GetFileNameWithoutExtension(assembly.FullPath).ToLower() == moduleName)) && assembly.AllTokens.ContainsKey(token))
				{
					result = assembly.AllTokens[token];
				}
			}

			return result;
		}

		public static NuGenTypeDefinition GetTypeDefinition(ValueWrapper dereferencedObject)
		{
			if (dereferencedObject.IsBoxedValue())
			{
				dereferencedObject = dereferencedObject.UnboxValue();
			}

			ClassWrapper classObject = dereferencedObject.GetClassInformation();
			ModuleWrapper module = classObject.GetModule();
			uint classToken = classObject.GetToken();
			NuGenTokenBase tokenObject = FindObjectByToken(classToken, module);
			NuGenTypeDefinition typeDefinition = tokenObject as NuGenTypeDefinition;

			return typeDefinition;
		}

		public static NuGenTypeDefinition FindTypeByName(string fullTypeName, string moduleNameWithoutExtension)
		{
			NuGenTypeDefinition result = null;
			moduleNameWithoutExtension = moduleNameWithoutExtension.ToLower();

			int index = 0;
			while (result == null && index < NuGenProject.Instance.Assemblies.Count)
			{
				NuGenAssembly assembly = NuGenProject.Instance.Assemblies[index++];
				bool isInMemoryAssembly = assembly.IsInMemory;

				if (((isInMemoryAssembly && assembly.ModuleScope.Name.ToLower() == moduleNameWithoutExtension) || (!isInMemoryAssembly && Path.GetFileNameWithoutExtension(assembly.FullPath).ToLower() == moduleNameWithoutExtension)))
				{
					Dictionary<uint, NuGenTypeDefinition>.ValueCollection typeDefinitions = assembly.ModuleScope.TypeDefinitions.Values;
					Dictionary<uint, NuGenTypeDefinition>.ValueCollection.Enumerator typeDefinitionsEnumerator = typeDefinitions.GetEnumerator();

					while (result == null && typeDefinitionsEnumerator.MoveNext())
					{
						if (typeDefinitionsEnumerator.Current.Name == fullTypeName)
						{
							result = typeDefinitionsEnumerator.Current;
						}
					}
				}
			}

			if (result == null)
			{
				string expandedTypeName = NuGenHelperFunctions.ExpandTypeName(fullTypeName);

				if (string.CompareOrdinal(expandedTypeName, fullTypeName) != 0)
				{
					result = FindTypeByName(expandedTypeName, moduleNameWithoutExtension);
				}
			}

			return result;
		}

		public static NuGenTypeDefinition FindTypeByName(string typeName)
		{
			return FindTypeByName(typeName, 0);
		}

		public static NuGenTypeDefinition FindTypeByName(string typeName, int typeArgumentCount)
		{
			NuGenTypeDefinition result = null;

			if (typeArgumentCount > 0)
			{
				typeName = typeName + "`" + Convert.ToString(typeArgumentCount);
			}

			int assemblyIndex = 0;
			while (result == null && assemblyIndex < NuGenProject.Instance.Assemblies.Count)
			{
				NuGenAssembly assembly = NuGenProject.Instance.Assemblies[assemblyIndex++];
				Dictionary<uint, NuGenTypeDefinition>.ValueCollection typeDefinitions = assembly.ModuleScope.TypeDefinitions.Values;
				Dictionary<uint, NuGenTypeDefinition>.ValueCollection.Enumerator typeDefinitionsEnumerator = typeDefinitions.GetEnumerator();

				while (result == null && typeDefinitionsEnumerator.MoveNext())
				{
					if (typeDefinitionsEnumerator.Current.Name == typeName)
					{
						result = typeDefinitionsEnumerator.Current;
					}
				}
			}

			if (result == null)
			{
				string expandedTypeName = NuGenHelperFunctions.ExpandTypeName(typeName);

				if (string.CompareOrdinal(expandedTypeName, typeName) != 0)
				{
					result = FindTypeByName(expandedTypeName);
				}
			}

			return result;
		}

		public static NuGenTokenBase FindObjectByToken(uint token)
		{
			NuGenTokenBase result = null;

			int index = 0;
			while (result == null && index < NuGenProject.Instance.Assemblies.Count)
			{
				NuGenAssembly assembly = NuGenProject.Instance.Assemblies[index++];

				if (assembly.AllTokens.ContainsKey(token))
				{
					result = assembly.AllTokens[token];
				}
			}

			return result;
		}

		public static void AddItemToList<T>(List<T> list, T item, int maximumSize)
		{
			if (!list.Contains(item))
			{
				list.Insert(0, item);
			}

			if (list.Count > maximumSize)
			{
				list.RemoveRange(maximumSize, list.Count - maximumSize);
			}
		}

		public static void AddItemsToList<T>(List<T> list, T[] items, int maximumSize)
		{
			for (int index = 0; index < items.Length; index++)
			{
				T item = items[index];

				if (!list.Contains(item))
				{
					list.Insert(0, item);
				}
			}

			if (list.Count > maximumSize)
			{
				list.RemoveRange(maximumSize, list.Count - maximumSize);
			}
		}

		public static bool MoveItemInList<T>(List<T> list, T item, int newIndex)
		{
			bool result = false;

			if (list.Contains(item))
			{
				list.Remove(item);
				list.Insert(newIndex, item);
				result = true;
			}

			return result;
		}

		public static void CopyListElements(IList source, IList destination)
		{
			if (source != null && destination != null)
			{
				for (int index = 0; index < source.Count; index++)
				{
					destination.Add(source[index]);
				}
			}
		}

		public static string FormatAsHexNumber(sbyte number, int digits)
		{
			return number.ToString("x").PadLeft(digits, '0');
		}

		public static string FormatAsHexNumber(short number, int digits)
		{
			return number.ToString("x").PadLeft(digits, '0');
		}

		public static string FormatAsHexNumber(int number, int digits)
		{
			return number.ToString("x").PadLeft(digits, '0');
		}

		public static string FormatAsHexNumber(long number, int digits)
		{
			return number.ToString("x").PadLeft(digits, '0');
		}

		public static string FormatAsHexNumber(byte number, int digits)
		{
			return number.ToString("x").PadLeft(digits, '0');
		}

		public static string FormatAsHexNumber(ushort number, int digits)
		{
			return number.ToString("x").PadLeft(digits, '0');
		}

		public static string FormatAsHexNumber(uint number, int digits)
		{
			return number.ToString("x").PadLeft(digits, '0');
		}

		public static string FormatAsHexNumber(ulong number, int digits)
		{
			return number.ToString("x").PadLeft(digits, '0');
		}

		public static string FormatAsHexNumber(object number, int digits)
		{
			return string.Format("{0:x}", number).PadLeft(digits, '0');
		}

		public static string FormatNumber(object number)
		{
			string result = null;

			if (NuGenSettings.Instance.DisplayHexaNumbers)
			{
				result = string.Format("0x{0:x}", number);
			}
			else
			{
				result = Convert.ToString(number);
			}

			return result;
		}

		public static string TruncateText(string text)
		{
			string result = text;

			if (text != null && text.Length > MaximumTextLength)
			{
				result = text.Substring(0, MaximumTextLength - 3) + "...";
			}

			return result;
		}

		public static NuGenTypeDefinition FindTypeOfValue(ClassWrapper classWrapper)
		{
			NuGenTypeDefinition result = null;
			ModuleWrapper moduleWrapper = null;

			if (classWrapper == null)
			{
				throw new ArgumentNullException("classWrapper");
			}
			else
			{
				moduleWrapper = classWrapper.GetModule();

				result = NuGenHelperFunctions.FindObjectByToken(classWrapper.GetToken(), moduleWrapper) as NuGenTypeDefinition;
			}

			if (result == null)
			{
				throw new NugenMissingModuleException(string.Format("The module ({0}) that contains the class (token: {1}) is not loaded.", moduleWrapper.GetName(), FormatAsHexNumber(classWrapper.GetToken(), 8)), moduleWrapper);
			}

			return result;
		}

		public static NuGenTypeDefinition FindTypeOfValue(NuGenEvaluationContext context, NuGenDebugExpressionResult valueExpression)
		{
			NuGenTypeDefinition result = null;
			ClassWrapper valueClass = valueExpression.ResultClass;

			if (valueClass == null)
			{
				if (valueExpression.ResultValue.IsNull())
				{
					result = NuGenHelperFunctions.FindTypeByName(NuGenConstants.ObjectTypeName, NuGenConstants.MscorlibName);

					if (result == null)
					{
						ModuleWrapper mscorlibModule = FindModuleByNameWithoutExtension(context, NuGenConstants.MscorlibName);

						if (mscorlibModule == null)
						{
							throw new InvalidOperationException("The mscorlib module that contains the System.Object class is not loaded.");
						}
						else
						{
							throw new NugenMissingModuleException("The mscorlib module that contains the System.Object class is not loaded.", mscorlibModule.GetName());
						}
					}
				}
				else
				{
					valueClass = GetClassOfValue(context, valueExpression.ResultValue);
				}
			}

			if (valueClass != null)
			{
				result = FindTypeOfValue(valueClass);
			}

			return result;
		}

		public static bool HasValueClass(CorElementType elementType)
		{
			bool result = false;

			switch (elementType)
			{
				case CorElementType.ELEMENT_TYPE_STRING:
				case CorElementType.ELEMENT_TYPE_OBJECT:
				case CorElementType.ELEMENT_TYPE_CLASS:
				case CorElementType.ELEMENT_TYPE_BYREF:
				case CorElementType.ELEMENT_TYPE_PTR:
				case CorElementType.ELEMENT_TYPE_VALUETYPE:
				case CorElementType.ELEMENT_TYPE_ARRAY:
				case CorElementType.ELEMENT_TYPE_SZARRAY:
					result = true;
					break;
			}

			return result;
		}

		public static bool HasValueClass(ValueWrapper valueWrapper)
		{
			return (valueWrapper == null ? false : HasValueClass((CorElementType)valueWrapper.ElementType));
		}

		public static ClassWrapper FindClassOfTypeDefintion(NuGenEvaluationContext context, NuGenTypeDefinition typeDefinition)
		{
			ClassWrapper result = null;

			ModuleWrapper module = FindModuleOfType(context, typeDefinition);
		 	result = module.GetClass(typeDefinition.Token);

			return result;
		}

		public static ClassWrapper GetClassOfValue(NuGenEvaluationContext context, ValueWrapper valueWrapper)
		{
			ClassWrapper result = null;

			switch ((CorElementType)valueWrapper.ElementType)
			{
				case CorElementType.ELEMENT_TYPE_STRING:
				case CorElementType.ELEMENT_TYPE_OBJECT:
				case CorElementType.ELEMENT_TYPE_CLASS:
					if (valueWrapper != null)
					{
						valueWrapper = valueWrapper.DereferenceValue();

						if (valueWrapper != null)
						{
							if (valueWrapper.IsBoxedValue())
							{
								valueWrapper = valueWrapper.UnboxValue();
							}

							result = valueWrapper.GetClassInformation();
						}
					}
					break;

				case CorElementType.ELEMENT_TYPE_BYREF:
				case CorElementType.ELEMENT_TYPE_PTR:
					if (valueWrapper != null && !valueWrapper.IsNull())
					{
						result = GetClassOfValue(context, valueWrapper.DereferenceValue());
					}
					break;

				case CorElementType.ELEMENT_TYPE_VALUETYPE:
					if (valueWrapper != null && valueWrapper.IsBoxedValue())
					{
						valueWrapper = valueWrapper.UnboxValue();
					}
					result = valueWrapper.GetClassInformation();
					break;

				case CorElementType.ELEMENT_TYPE_ARRAY:
				case CorElementType.ELEMENT_TYPE_SZARRAY:
					NuGenTypeDefinition arrayType = NuGenHelperFunctions.FindTypeByName("System.Array", NuGenConstants.MscorlibName);

					if (arrayType == null)
					{
						ModuleWrapper mscorlibModule = FindModuleByNameWithoutExtension(context, NuGenConstants.MscorlibName);

						if (mscorlibModule == null)
						{
							throw new InvalidOperationException("The mscorlib module that contains the System.Array class is not loaded.");
						}
						else
						{
							throw new NugenMissingModuleException("The mscorlib module that contains the System.Array class is not loaded.", mscorlibModule.GetName());
						}
					}

					ModuleWrapper module = FindModuleOfType(context, arrayType);
					result = module.GetClass(arrayType.Token);
					break;

				default:
					throw new NuGenEvaluationException(string.Format("The class of the type ({0}) could not been found.", Enum.GetName(typeof(CorElementType), valueWrapper.ElementType)));
			}

			return result;
		}

		public static NuGenTypeDefinition FindTypeByToken(NuGenAssembly assembly, uint typeToken)
		{
			NuGenTypeDefinition result = null;

			if (assembly.AllTokens.ContainsKey(typeToken))
			{
				NuGenTokenBase typeTokenObject = assembly.AllTokens[typeToken];
				result = typeTokenObject as NuGenTypeDefinition;

				if (result == null && typeTokenObject.GetType() == typeof(NuGenTypeReference))
				{
					NuGenTypeReference typeReference = (NuGenTypeReference)typeTokenObject;
					result = NuGenHelperFunctions.FindTypeByName(typeReference.Name, typeReference.ReferencedAssembly);
				}
			}

			return result;
		}

		public static string ExpandTypeName(string typeShortName)
		{
			return (TypeNameAbbreviations.ContainsKey(typeShortName) ? TypeNameAbbreviations[typeShortName] : typeShortName);
		}

		public static CorElementType GetElementTypeByName(string typeName)
		{
			CorElementType result = CorElementType.ELEMENT_TYPE_END;
			typeName = ExpandTypeName(typeName);

			Dictionary<CorElementType, string>.Enumerator enumerator = ElementTypeAbbreviations.GetEnumerator();

			while (result == CorElementType.ELEMENT_TYPE_END && enumerator.MoveNext())
			{
				if (string.CompareOrdinal(enumerator.Current.Value, typeName) == 0)
				{
					result = enumerator.Current.Key;
				}
			}

			return result;
		}

		public static NuGenTypeDefinition GetTypeByElementType(CorElementType elementType)
		{
			NuGenTypeDefinition result = null;

			if (ElementTypeAbbreviations.ContainsKey(elementType))
			{
				result = FindTypeByName(GetTypeNameByElementType(elementType), NuGenConstants.MscorlibName);
			}

			return result;
		}

		public static string GetTypeNameByElementType(CorElementType elementType)
		{
			return (ElementTypeAbbreviations.ContainsKey(elementType) ? ElementTypeAbbreviations[elementType] : string.Empty);
		}

		public static void CastDebugValue(ValueWrapper sourceValue, ValueWrapper destinationValue)
		{
			CastDebugValue(sourceValue, destinationValue, (CorElementType)destinationValue.ElementType);
		}

		public static void CastDebugValue(ValueWrapper sourceValue, ValueWrapper destinationValue, CorElementType destinationType)
		{
			switch ((CorElementType)sourceValue.ElementType)
			{
				case CorElementType.ELEMENT_TYPE_BOOLEAN:
					SetDebugValue(sourceValue.GetGenericValue<bool>(), destinationValue, destinationType);
					break;

				case CorElementType.ELEMENT_TYPE_CHAR:
					SetDebugValue(sourceValue.GetGenericValue<char>(), destinationValue, destinationType);
					break;

				case CorElementType.ELEMENT_TYPE_I1:
					SetDebugValue(sourceValue.GetGenericValue<sbyte>(), destinationValue, destinationType);
					break;

				case CorElementType.ELEMENT_TYPE_U1:
					SetDebugValue(sourceValue.GetGenericValue<byte>(), destinationValue, destinationType);
					break;

				case CorElementType.ELEMENT_TYPE_I2:
					SetDebugValue(sourceValue.GetGenericValue<short>(), destinationValue, destinationType);
					break;

				case CorElementType.ELEMENT_TYPE_U2:
					SetDebugValue(sourceValue.GetGenericValue<ushort>(), destinationValue, destinationType);
					break;

				case CorElementType.ELEMENT_TYPE_I4:
					SetDebugValue(sourceValue.GetGenericValue<int>(), destinationValue, destinationType);
					break;

				case CorElementType.ELEMENT_TYPE_U4:
					SetDebugValue(sourceValue.GetGenericValue<uint>(), destinationValue, destinationType);
					break;

				case CorElementType.ELEMENT_TYPE_I8:
					SetDebugValue(sourceValue.GetGenericValue<long>(), destinationValue, destinationType);
					break;

				case CorElementType.ELEMENT_TYPE_U8:
					SetDebugValue(sourceValue.GetGenericValue<ulong>(), destinationValue, destinationType);
					break;

				case CorElementType.ELEMENT_TYPE_R4:
					SetDebugValue(sourceValue.GetGenericValue<float>(), destinationValue, destinationType);
					break;

				case CorElementType.ELEMENT_TYPE_R8:
					SetDebugValue(sourceValue.GetGenericValue<double>(), destinationValue, destinationType);
					break;
			}
		}

		private static void SetDebugValue(object value, ValueWrapper destinationValue, CorElementType destionationType)
		{
			switch (destionationType)
			{
				case CorElementType.ELEMENT_TYPE_BOOLEAN:
					destinationValue.SetGenericValue<bool>(Convert.ToBoolean(value));
					break;

				case CorElementType.ELEMENT_TYPE_CHAR:
					destinationValue.SetGenericValue<char>(Convert.ToChar(value));
					break;

				case CorElementType.ELEMENT_TYPE_I1:
					destinationValue.SetGenericValue<sbyte>(Convert.ToSByte(value));
					break;

				case CorElementType.ELEMENT_TYPE_U1:
					destinationValue.SetGenericValue<byte>(Convert.ToByte(value));
					break;

				case CorElementType.ELEMENT_TYPE_I2:
					destinationValue.SetGenericValue<short>(Convert.ToInt16(value));
					break;

				case CorElementType.ELEMENT_TYPE_U2:
					destinationValue.SetGenericValue<ushort>(Convert.ToUInt16(value));
					break;

				case CorElementType.ELEMENT_TYPE_I4:
					destinationValue.SetGenericValue<int>(Convert.ToInt32(value));
					break;

				case CorElementType.ELEMENT_TYPE_U4:
					destinationValue.SetGenericValue<uint>(Convert.ToUInt32(value));
					break;

				case CorElementType.ELEMENT_TYPE_I8:
					destinationValue.SetGenericValue<long>(Convert.ToInt64(value));
					break;

				case CorElementType.ELEMENT_TYPE_U8:
					destinationValue.SetGenericValue<ulong>(Convert.ToUInt64(value));
					break;

				case CorElementType.ELEMENT_TYPE_R4:
					destinationValue.SetGenericValue<float>(Convert.ToSingle(value));
					break;

				case CorElementType.ELEMENT_TYPE_R8:
					destinationValue.SetGenericValue<double>(Convert.ToDouble(value));
					break;
			}
		}

		public static ValueWrapper CastToDecimal(NuGenEvaluationContext context, ValueWrapper numberValue)
		{
			ValueWrapper result = null;
			NuGenTypeDefinition convertType = NuGenHelperFunctions.FindTypeByName(ConvertTypeName, NuGenConstants.MscorlibName);

			if (convertType == null)
			{
				throw new NugenMissingModuleException(string.Format("The {0} type definition could not be found. Perhaps, the {1} modules is not loaded.", ConvertTypeName, NuGenConstants.MscorlibName), NuGenConstants.MscorlibName);
			}

			List<NuGenDebugExpressionResult> toDecimalParameters = new List<NuGenDebugExpressionResult>();
			toDecimalParameters.Add(new NuGenDebugExpressionResult(context, numberValue));

			NuGenMethodDefinition convertToDecimalMethod = convertType.FindMethodDefinitionByParameter(context, ToDecimalMethodName, toDecimalParameters);

			if (convertToDecimalMethod == null)
			{
				throw new InvalidOperationException(string.Format("No suitable {0}::{1}() overloaded method found to convert the value to decimal.", ConvertTypeName, ToDecimalMethodName));
			}

			List<ValueWrapper> methodParameters = new List<ValueWrapper>();
			methodParameters.Add(numberValue);

			NuGenBaseEvaluationResult evaluationResult = context.EvaluationHandler.CallMethod(context, convertToDecimalMethod, methodParameters);

			if (evaluationResult.IsSuccessful)
			{
				result = evaluationResult.Result;
			}
			else
			{
				evaluationResult.ThrowExceptionAccordingToReason();
			}

			return result;
		}

		public static ValueWrapper CreateBoxedValue(NuGenEvaluationContext context, NuGenTypeDefinition typeDefinition)
		{
			ValueWrapper result = null;
			ModuleWrapper module = FindModuleOfType(context, typeDefinition);
			ClassWrapper classWrapper = module.GetClass(typeDefinition.Token);

			context.EvalWrapper.NewObjectNoConstructor(classWrapper);
			NuGenBaseEvaluationResult evaluationResult = context.EvaluationHandler.RunEvaluation(context.EvalWrapper);

			if (evaluationResult.IsSuccessful)
			{
				result = evaluationResult.Result;
			}

			return result;
		}

		public static bool IsArrayElementType(CorElementType elementType)
		{
			return (elementType == CorElementType.ELEMENT_TYPE_ARRAY || elementType == CorElementType.ELEMENT_TYPE_SZARRAY);
		}

		public static bool TypeDefinitionsMatch(NuGenTypeDefinition actualTypeDef, NuGenTypeDefinition expectedTypeDef)
		{
			bool result = false;

			if (actualTypeDef != null)
			{
				if (actualTypeDef == expectedTypeDef)
				{
					result = true;
				}
				else
				{
					if (actualTypeDef.ModuleScope.Assembly.AllTokens.ContainsKey(actualTypeDef.BaseTypeToken))
					{
						NuGenTypeDefinition actualBaseTypeDef = NuGenHelperFunctions.FindTypeByToken(actualTypeDef.ModuleScope.Assembly, actualTypeDef.BaseTypeToken);

						if (actualBaseTypeDef != null)
						{
							result = TypeDefinitionsMatch(actualBaseTypeDef, expectedTypeDef);
						}

						if (!result && actualTypeDef != null && actualTypeDef.InterfaceImplementations != null)
						{
							int index = 0;

							while (!result && index < actualTypeDef.InterfaceImplementations.Count)
							{
								NuGenInterfaceImplementation interfaceImplementation = actualTypeDef.InterfaceImplementations[index++];
								NuGenTypeDefinition interfaceTypeDefinition = NuGenHelperFunctions.FindTypeByToken(actualTypeDef.ModuleScope.Assembly, interfaceImplementation.InterfaceToken);

								result = TypeDefinitionsMatch(interfaceTypeDefinition, expectedTypeDef);
							}
						}
					}
				}
			}

			return result;
		}

		public static ModuleWrapper FindModuleOfType(NuGenEvaluationContext context, NuGenTypeDefinition typeDefinition)
		{
			ModuleWrapper result = null;
			List<ModuleWrapper> modules = context.ThreadWrapper.FindModulesByName(typeDefinition.ModuleScope.Assembly.FileName);

			if (modules.Count < 1)
			{
				throw new InvalidOperationException(string.Format("The containing module ({0}) of the {1} type could not be found.", typeDefinition.ModuleScope.Assembly.FileName, typeDefinition.FullName));
			}

			result = modules[0];

			return result;
		}

		public static ModuleWrapper FindModuleByNameWithoutExtension(NuGenEvaluationContext context, string moduleName)
		{
			ModuleWrapper result = null;

			List<ModuleWrapper> modules = context.ThreadWrapper.FindModulesByNameWithoutExtension(moduleName);

			if (modules.Count > 0)
			{
				result = modules[0];
			}

			return result;
		}

		private static void AddTypeArguments(List<NuGenTypeTreeNode> nodes, List<TypeWrapper> typeArguments)
		{
			if (typeArguments != null)
			{
				for (int index = 0; index < typeArguments.Count; index++)
				{
					TypeWrapper typeArgument = typeArguments[index];
					NuGenTypeDefinition typeDefinitionArgument = null;

					if (NuGenHelperFunctions.HasValueClass((CorElementType)typeArgument.ElementType))
					{
						typeDefinitionArgument = NuGenHelperFunctions.FindTypeOfValue(typeArgument.GetClass());
					}
					else
					{
						typeDefinitionArgument = NuGenHelperFunctions.GetTypeByElementType((CorElementType)typeArgument.ElementType);
					}

					NuGenTypeTreeNode typeNode = new NuGenTypeTreeNode(typeDefinitionArgument);
					nodes.Add(typeNode);

					AddTypeArguments(typeNode.ChildNodes, typeArgument.EnumerateTypeParameters());
				}
			}
		}

		public static NuGenTypeTreeNode GetValueTypeTree(ValueWrapper valueWrapper)
		{
			NuGenTypeTreeNode result = null;

			if (valueWrapper != null && valueWrapper.IsVersion2)
			{
				TypeWrapper valueWrapperExactType = valueWrapper.Version2.GetExactType();

				if (NuGenHelperFunctions.HasValueClass((CorElementType)valueWrapperExactType.ElementType))
				{
					NuGenTypeDefinition valueWrapperExactTypeDef = NuGenHelperFunctions.FindTypeOfValue(valueWrapperExactType.GetClass());
					result = new NuGenTypeTreeNode(valueWrapperExactTypeDef);

					AddTypeArguments(result.ChildNodes, valueWrapperExactType.EnumerateTypeParameters());
				}
			}

			return result;
		}
    }
}