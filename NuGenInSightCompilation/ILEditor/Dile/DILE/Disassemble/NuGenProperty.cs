using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.Metadata.Signature;
using Dile.UI;

namespace Dile.Disassemble
{
	public class NuGenProperty : NuGenTextTokenBase, NuGenIMultiLine, NuGenIHasSignature
	{
		#region IMultiLine Members

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

		public bool IsInMemory
		{
			get
			{
				return BaseTypeDefinition.ModuleScope.Assembly.IsInMemory;
			}
		}

		#endregion

		public override SearchOptions ItemType
		{
			get
			{
				return SearchOptions.Property;
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

		private CorPropertyAttr flags;
		public CorPropertyAttr Flags
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

		private IntPtr signature;
		public IntPtr Signature
		{
			get
			{
				return signature;
			}
			private set
			{
				signature = value;
			}
		}

		private uint signatureLength;
		public uint SignatureLength
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

		private CorElementType elementType;
		public CorElementType ElementType
		{
			get
			{
				return elementType;
			}
			private set
			{
				elementType = value;
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

		private uint setterMethodToken;
		public uint SetterMethodToken
		{
			get
			{
				return setterMethodToken;
			}
			private set
			{
				setterMethodToken = value;
			}
		}

		private uint getterMethodToken;
		public uint GetterMethodToken
		{
			get
			{
				return getterMethodToken;
			}
			private set
			{
				getterMethodToken = value;
			}
		}

		private uint[] otherMethods;
		public uint[] OtherMethods
		{
			get
			{
				return otherMethods;
			}
			private set
			{
				otherMethods = value;
			}
		}

		private uint otherMethodsCount;
		public uint OtherMethodsCount
		{
			get
			{
				return otherMethodsCount;
			}
			private set
			{
				otherMethodsCount = value;
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

		private string definition;
		public string Definition
		{
			get
			{
				return definition;
			}
			private set
			{
				definition = value;
			}
		}

		private NuGenPropertySignatureReader signatureReader;
		public NuGenBaseSignatureReader SignatureReader
		{
			get
			{
				ReadSignature();

				return signatureReader;
			}
		}

		private static StringBuilder definitionBuilder = new StringBuilder();
		private static StringBuilder DefinitionBuilder
		{
			get
			{
				return definitionBuilder;
			}
		}

		public NuGenBaseSignatureItem PropertyType
		{
			get
			{
				return signatureReader.ReturnType;
			}
		}

		public NuGenProperty(NuGenIMetaDataImport2 import, uint token, NuGenTypeDefinition baseTypeDefinition, string name, uint flags, IntPtr signature, uint signatureLength, uint elementType, IntPtr defaultValue, uint defaultValueLength, uint setterMethodToken, uint getterMethodToken, uint[] otherMethods, uint otherMethodsCount)
		{
			Token = token;
			BaseTypeDefinition = baseTypeDefinition;
			Name = name;
			Flags = (CorPropertyAttr)flags;
			Signature = signature;
			SignatureLength = signatureLength;
			ElementType = (CorElementType)elementType;
			DefaultValue = defaultValue;
			DefaultValueLength = defaultValueLength;

			if (DefaultValueLength > 0)
			{
				throw new NotImplementedException("Default value is given for the property.");
			}

			SetterMethodToken = setterMethodToken;
			GetterMethodToken = getterMethodToken;
			OtherMethods = otherMethods;
			OtherMethodsCount = otherMethodsCount;
		}

		private void ReadSignature()
		{
			if (signatureReader == null)
			{
				signatureReader = new NuGenPropertySignatureReader(BaseTypeDefinition.ModuleScope.Assembly.AllTokens, Signature, SignatureLength);
				signatureReader.ReadSignature();
			}
		}

		protected override void CreateText(Dictionary<uint, NuGenTokenBase> allTokens)
		{
			base.CreateText(allTokens);
			ReadSignature();
			NuGenHelperFunctions.SetSignatureItemToken(allTokens, signatureReader.ReturnType);

			NuGenAssembly assembly = BaseTypeDefinition.ModuleScope.Assembly;
			DefinitionBuilder.Length = 0;

			if ((Flags & CorPropertyAttr.prSpecialName) == CorPropertyAttr.prSpecialName)
			{
				DefinitionBuilder.Append("specialname ");
			}

			CorPropertyAttr reservedFlags = Flags & CorPropertyAttr.prReservedMask;

			if ((reservedFlags & CorPropertyAttr.prRTSpecialName) == CorPropertyAttr.prRTSpecialName)
			{
				DefinitionBuilder.Append("rtsspecialname ");
			}

			if ((signatureReader.CallingConvention & CorCallingConvention.IMAGE_CEE_CS_CALLCONV_HASTHIS) == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_HASTHIS)
			{
				DefinitionBuilder.Append("instance ");
			}

			DefinitionBuilder.Append(signatureReader.ReturnType);
			DefinitionBuilder.Append(" ");
			DefinitionBuilder.Append(Name);
			DefinitionBuilder.Append("(");

			if (signatureReader.Parameters != null)
			{
				for (int parametersIndex = 0; parametersIndex < signatureReader.Parameters.Count; parametersIndex++)
				{
					NuGenBaseSignatureItem parameter = signatureReader.Parameters[parametersIndex];
					NuGenHelperFunctions.SetSignatureItemToken(allTokens, parameter);
					DefinitionBuilder.Append(parameter);

					if (parametersIndex < signatureReader.Parameters.Count - 1)
					{
						DefinitionBuilder.Append(", ");
					}
				}
			}

			DefinitionBuilder.Append(")");
			Definition = DefinitionBuilder.ToString();
		}

		public void Initialize()
		{
			ReadMetadata();
			NuGenAssembly assembly = BaseTypeDefinition.ModuleScope.Assembly;
			CodeLines = new List<NuGenCodeLine>();
			NuGenCodeLine definitionLine = new NuGenCodeLine();
			definitionLine.Indentation = 0;

			CodeLines.Add(definitionLine);
			CodeLines.Add(new NuGenCodeLine(0, "{"));

			if (CustomAttributes != null)
			{
				foreach (NuGenCustomAttribute customAttribute in CustomAttributes)
				{
					customAttribute.SetText(assembly.AllTokens);
					CodeLines.Add(new NuGenCodeLine(1, customAttribute.Name));
				}
			}

			if (assembly.AllTokens.ContainsKey(GetterMethodToken))
			{
				NuGenMethodDefinition getMethod = (NuGenMethodDefinition)assembly.AllTokens[GetterMethodToken];
				CodeLines.Add(new NuGenCodeLine(1, ".get " + getMethod.Text));
			}

			for (int index = 0; index < OtherMethodsCount; index++)
			{
				uint token = OtherMethods[index];

				if (assembly.AllTokens.ContainsKey(token))
				{
					NuGenMethodDefinition otherMethod = (NuGenMethodDefinition)assembly.AllTokens[token];
					CodeLines.Add(new NuGenCodeLine(1, ".other " + otherMethod.Text));
				}
			}

			if (assembly.AllTokens.ContainsKey(SetterMethodToken))
			{
				NuGenMethodDefinition setMethod = (NuGenMethodDefinition)assembly.AllTokens[setterMethodToken];
				CodeLines.Add(new NuGenCodeLine(1, ".set " + setMethod.Text));
			}

			Definition = ".property " + Definition;
			definitionLine.Text = Definition;

			CodeLines.Add(new NuGenCodeLine(0, string.Format("}} //end of property {0}::{1}", BaseTypeDefinition.Name, Name)));
		}

		protected override void ReadMetadataInformation()
		{
			base.ReadMetadataInformation();
			NuGenAssembly assembly = BaseTypeDefinition.ModuleScope.Assembly;

			try
			{
				assembly.OpenMetadataInterfaces();

				CustomAttributes = NuGenHelperFunctions.EnumCustomAttributes(assembly.Import, BaseTypeDefinition.ModuleScope.Assembly, this);
			}
			finally
			{
				assembly.CloseMetadataInterfaces();
			}
		}
	}
}