using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.Metadata.Signature;
using Dile.UI;

namespace Dile.Disassemble
{
	public class NuGenFieldDefinition : NuGenTextTokenBase, NuGenIMultiLine, NuGenIHasSignature
	{
		public bool IsInMemory
		{
			get
			{
				return BaseTypeDefinition.ModuleScope.Assembly.IsInMemory;
			}
		}

		public override SearchOptions ItemType
		{
			get
			{
				return SearchOptions.FieldDefintion;
			}
		}

		private string text = null;
		public string Text
		{
			get
			{
				if (text == null)
				{
					if (BaseTypeDefinition.FullName.Length > 0)
					{
						text = string.Format("{0} {1}::{2}", FieldTypeName, BaseTypeDefinition.FullName, Name);
					}
					else
					{
						text = string.Format("{0} {1}", FieldTypeName, Name);
					}
				}

				return text;
			}
			private set
			{
				text = value;
			}
		}

		private List<NuGenCodeLine> codeLines;
		public List<NuGenCodeLine> CodeLines
		{
			get
			{
				return codeLines;
			}
			set
			{
				codeLines = value;
			}
		}

		public string HeaderText
		{
			get
			{
				return string.Format("{0}.{1}", BaseTypeDefinition.Name, Name);
			}
		}

		private NuGenTypeDefinition baseTypeDefinition;
		public NuGenTypeDefinition BaseTypeDefinition
		{
			get
			{
				return baseTypeDefinition;
			}
			private set
			{
				baseTypeDefinition = value;
			}
		}

		private IntPtr signatureBlob;
		public IntPtr SignatureBlob
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

		private uint signatureBlobLength;
		public uint SignatureBlobLength
		{
			get
			{
				return signatureBlobLength;
			}
			private set
			{
				signatureBlobLength = value;
			}
		}

		private CorFieldAttr flags;
		public CorFieldAttr Flags
		{
			get
			{
				return flags;
			}
			private set
			{
				flags = value;
			}
		}

		private CorElementType defaultValueType;
		public CorElementType DefaultValueType
		{
			get
			{
				return defaultValueType;
			}
			private set
			{
				defaultValueType = value;
			}
		}

		private IntPtr defaultValue;
		public IntPtr DefaultValue
		{
			get
			{
				return defaultValue;
			}
			private set
			{
				defaultValue = value;
			}
		}

		private uint defaultValueLength;
		public uint DefaultValueLength
		{
			get
			{
				return defaultValueLength;
			}
			private set
			{
				defaultValueLength = value;
			}
		}

		private object defaultValueNumber = null;
		public object DefaultValueNumber
		{
			get
			{
				ReadMetadata();

				return defaultValueNumber;
			}
			private set
			{
				defaultValueNumber = value;
			}
		}

		private List<NuGenCustomAttribute> customAttributes;
		public List<NuGenCustomAttribute> CustomAttributes
		{
			get
			{
				ReadMetadata();

				return customAttributes;
			}
			private set
			{
				customAttributes = value;
			}
		}

		private string fieldTypeName = null;
		public string FieldTypeName
		{
			get
			{
				if (fieldTypeName == null)
				{
					ReadSignature();

					if (signatureReader.GenericParameters != null)
					{
						FieldTypeNameBuilder.Length = 0;
						FieldTypeNameBuilder.Append("<");

						for (int index = 0; index < signatureReader.GenericParameters.Count; index++)
						{
							NuGenBaseSignatureItem genericParameter = signatureReader.GenericParameters[index];
							NuGenHelperFunctions.SetSignatureItemToken(BaseTypeDefinition.ModuleScope.Assembly.AllTokens, genericParameter);
							FieldTypeNameBuilder.Append(genericParameter);

							if (index < signatureReader.GenericParameters.Count - 1)
							{
								FieldTypeNameBuilder.Append(", ");
							}
						}

						FieldTypeNameBuilder.Append(">");

						fieldTypeName = FieldTypeNameBuilder.ToString();
					}
					else
					{
						NuGenHelperFunctions.SetSignatureItemToken(BaseTypeDefinition.ModuleScope.Assembly.AllTokens, signatureReader.Type);
						fieldTypeName = signatureReader.Type.ToString();
					}
				}

				return fieldTypeName;
			}
			private set
			{
				fieldTypeName = value;
			}
		}

		private uint rva;
		public uint Rva
		{
			get
			{
				return rva;
			}
			private set
			{
				rva = value;
			}
		}

		private CorMethodImpl implementationFlags;
		public CorMethodImpl ImplementationFlags
		{
			get
			{
				return implementationFlags;
			}
			private set
			{
				implementationFlags = value;
			}
		}

		private string defaultValueAsString;
		public string DefaultValueAsString
		{
			get
			{
				ReadMetadata();

				return defaultValueAsString;
			}
			private set
			{
				defaultValueAsString = value;
			}
		}

		private string marshalAsTypeString;
		public string MarshalAsTypeString
		{
			get
			{
				ReadMetadata();

				return marshalAsTypeString;
			}
			private set
			{
				marshalAsTypeString = value;
			}
		}

		private string pinvokeMap;
		public string PinvokeMap
		{
			get
			{
				ReadMetadata();

				return pinvokeMap;
			}
			private set
			{
				pinvokeMap = value;
			}
		}

		private NuGenFieldSignatureReader signatureReader;
		public NuGenBaseSignatureReader SignatureReader
		{
			get
			{
				ReadSignature();

				return signatureReader;
			}
		}

		private static StringBuilder fieldTypeNameBuilder = new StringBuilder();
		private static StringBuilder FieldTypeNameBuilder
		{
			get
			{
				return fieldTypeNameBuilder;
			}
		}

		public NuGenFieldDefinition(NuGenIMetaDataImport2 import, NuGenTypeDefinition typeDefinition, string name, uint token, IntPtr signatureBlob, uint signatureBlobLength, uint flags, uint defaultValueType, IntPtr defaultValue, uint defaultValueLength)
		{
			BaseTypeDefinition = typeDefinition;
			Name = name;
			Token = token;
			SignatureBlob = signatureBlob;
			SignatureBlobLength = signatureBlobLength;
			Flags = (CorFieldAttr)flags;
			DefaultValueType = (CorElementType)defaultValueType;
			DefaultValue = defaultValue;
			DefaultValueLength = defaultValueLength;
		}

		private void ReadSignature()
		{
			if (signatureReader == null)
			{
				signatureReader = new NuGenFieldSignatureReader(BaseTypeDefinition.ModuleScope.Assembly.AllTokens, SignatureBlob, SignatureBlobLength);
				signatureReader.ReadSignature();
			}
		}

		private string MemberAccessAsString()
		{
			string result = string.Empty;
			CorFieldAttr memberAccess = Flags & CorFieldAttr.fdFieldAccessMask;

			switch (memberAccess)
			{
				case CorFieldAttr.fdPrivateScope:
					result = "privatescope ";
					break;

				case CorFieldAttr.fdPrivate:
					result = "private ";
					break;

				case CorFieldAttr.fdFamANDAssem:
					result = "famandassem ";
					break;

				case CorFieldAttr.fdAssembly:
					result = "assembly ";
					break;

				case CorFieldAttr.fdFamily:
					result = "family ";
					break;

				case CorFieldAttr.fdFamORAssem:
					result = "famorassem ";
					break;

				case CorFieldAttr.fdPublic:
					result = "public ";
					break;
			}

			return result;
		}

		private string ContractAttributesAsString()
		{
			string result = string.Empty;

			result = NuGenHelperFunctions.EnumAsString(Flags, CorFieldAttr.fdStatic, "static ");
			result += NuGenHelperFunctions.EnumAsString(Flags, CorFieldAttr.fdInitOnly, "initonly ");
			result += NuGenHelperFunctions.EnumAsString(Flags, CorFieldAttr.fdLiteral, "literal ");
			result += NuGenHelperFunctions.EnumAsString(Flags, CorFieldAttr.fdNotSerialized, "notserialized ");
			result += NuGenHelperFunctions.EnumAsString(Flags, CorFieldAttr.fdSpecialName, "specialname ");

			if ((Flags & CorFieldAttr.fdPinvokeImpl) == CorFieldAttr.fdPinvokeImpl)
			{
				result += "pinvokeimpl(";

				if (PinvokeMap == null)
				{
					result += "/* No map */";
				}
				else
				{
					result += PinvokeMap;
				}

				result += ") ";
			}

			return result;
		}

		private string ReservedFlagsAsString()
		{
			string result = string.Empty;
			CorFieldAttr reservedFlags = Flags & CorFieldAttr.fdReservedMask;

			result = NuGenHelperFunctions.EnumAsString(Flags, CorFieldAttr.fdRTSpecialName, "rtsspecialname ");

			result += NuGenHelperFunctions.EnumAsString(Flags, CorFieldAttr.fdHasFieldMarshal, "marshal ");

			return result;
		}

		public void Initialize()
		{
			ReadMetadata();
			CodeLines = new List<NuGenCodeLine>();
			StringBuilder definition = new StringBuilder();

			definition.Append(".field ");
			definition.Append(ContractAttributesAsString());
			definition.Append(MemberAccessAsString());
			definition.Append(ReservedFlagsAsString());
			definition.Append(FieldTypeName);
			definition.Append(" ");
			definition.Append(Name);

			if ((Flags & CorFieldAttr.fdHasDefault) == CorFieldAttr.fdHasDefault)
			{
				definition.Append(" = ");
				definition.Append(DefaultValueAsString);
			}

			if (((Flags & CorFieldAttr.fdReservedMask) & CorFieldAttr.fdHasFieldRVA) == CorFieldAttr.fdHasFieldRVA)
			{
				definition.Append(" at D_");
				definition.Append(NuGenHelperFunctions.FormatAsHexNumber(Rva, 8));
			}

			CodeLines.Add(new NuGenCodeLine(0, definition.ToString()));

			if (CustomAttributes != null)
			{
				NuGenAssembly assembly = BaseTypeDefinition.ModuleScope.Assembly;

				foreach (NuGenCustomAttribute customAttribute in CustomAttributes)
				{
					customAttribute.SetText(assembly.AllTokens);
					CodeLines.Add(new NuGenCodeLine(0, customAttribute.Name));
				}
			}
		}

		protected override void CreateText(Dictionary<uint, NuGenTokenBase> allTokens)
		{
			base.CreateText(allTokens);
			ReadSignature();
			NuGenBaseSignatureItem signatureItem = signatureReader.Type;
			NuGenHelperFunctions.SetSignatureItemToken(allTokens, signatureItem);

			if (((Flags & CorFieldAttr.fdReservedMask) & CorFieldAttr.fdHasFieldRVA) == CorFieldAttr.fdHasFieldRVA)
			{
				NuGenAssembly assembly = BaseTypeDefinition.ModuleScope.Assembly;

				try
				{
					assembly.OpenMetadataInterfaces();
					uint rva;
					uint implFlags;

					assembly.Import.GetRVA(Token, out rva, out implFlags);

					Rva = rva;
					ImplementationFlags = (CorMethodImpl)implFlags;
				}
				finally
				{
					assembly.CloseMetadataInterfaces();
				}
			}
		}

		public bool IsReformattableDefaultValue()
		{
			return (DefaultValueNumber != null);
		}

		public string GetFormattedDefaultValue()
		{
			string result = null;

			if (IsReformattableDefaultValue())
			{
				result = NuGenHelperFunctions.FormatNumber(DefaultValueNumber);
			}
			else
			{
				result = DefaultValueAsString;
			}

			return result;
		}

		protected override void ReadMetadataInformation()
		{
			base.ReadMetadataInformation();
			NuGenAssembly assembly = BaseTypeDefinition.ModuleScope.Assembly;

			try
			{
				assembly.OpenMetadataInterfaces();

				if ((Flags & CorFieldAttr.fdHasDefault) == CorFieldAttr.fdHasDefault)
				{
					object defaultValueNumber;
					DefaultValueAsString = NuGenHelperFunctions.ReadDefaultValue(DefaultValueType, DefaultValue, DefaultValueLength, out defaultValueNumber);
					DefaultValueNumber = defaultValueNumber;
				}

				if ((Flags & CorFieldAttr.fdHasFieldMarshal) == CorFieldAttr.fdHasFieldMarshal)
				{
					MarshalAsTypeString = string.Format("marshal({0})", NuGenHelperFunctions.ReadMarshalDescriptor(assembly.Import, BaseTypeDefinition.ModuleScope.Assembly.AllTokens, Token, 0));
				}

				if ((Flags & CorFieldAttr.fdPinvokeImpl) == CorFieldAttr.fdPinvokeImpl)
				{
					PinvokeMap = NuGenHelperFunctions.ReadPinvokeMap(assembly.Import, BaseTypeDefinition.ModuleScope.Assembly, Token);
				}

				CustomAttributes = NuGenHelperFunctions.EnumCustomAttributes(assembly.Import, BaseTypeDefinition.ModuleScope.Assembly, this);
			}
			finally
			{
				assembly.CloseMetadataInterfaces();
			}
		}

		public override string ToString()
		{
			LazyInitialize(BaseTypeDefinition.ModuleScope.Assembly.AllTokens);

			return Text;
		}
	}
}