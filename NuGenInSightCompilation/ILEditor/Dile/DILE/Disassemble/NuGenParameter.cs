using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;
using Dile.Metadata.Signature;
using System.Runtime.InteropServices;

namespace Dile.Disassemble
{
	public class NuGenParameter : NuGenTokenBase, IComparable, NuGenILazyInitialized
	{
		private const string ParamArrayAttribute = "System.ParamArrayAttribute::.ctor()";

		private NuGenMethodDefinition method;
		public NuGenMethodDefinition Method
		{
			get
			{
				return method;
			}
			private set
			{
				method = value;
			}
		}

		private uint ordinalIndex;
		public uint OrdinalIndex
		{
			get
			{
				return ordinalIndex;
			}
			private set
			{
				ordinalIndex = value;
			}
		}

		private CorParamAttr attributeFlags;
		public CorParamAttr AttributeFlags
		{
			get
			{
				return attributeFlags;
			}
			private set
			{
				attributeFlags = value;
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

		private string defaultValueAsString = string.Empty;
		public string DefaultValueAsString
		{
			get
			{
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
				return marshalAsTypeString;
			}
			private set
			{
				marshalAsTypeString = value;
			}
		}

		private List<NuGenCustomAttribute> customAttributes;
		public List<NuGenCustomAttribute> CustomAttributes
		{
			get
			{
				return customAttributes;
			}
			set
			{
				customAttributes = value;
			}
		}

		private static StringBuilder attributeTextBuilder = new StringBuilder();
		private static StringBuilder AttributeTextBuilder
		{
			get
			{
				return attributeTextBuilder;
			}
		}

		private bool isLazyInitialized = false;
		private bool IsLazyInitialized
		{
			get
			{
				return isLazyInitialized;
			}
			set
			{
				isLazyInitialized = value;
			}
		}

		public NuGenParameter(NuGenIMetaDataImport2 import, Dictionary<uint, NuGenTokenBase> allTokens, uint token, NuGenMethodDefinition method, uint ordinalIndex, string name, uint attributeFlags, uint elementType, IntPtr defaultValue, uint defaultValueLength)
		{
			Token = token;
			Method = method;
			OrdinalIndex = ordinalIndex;
			Name = name;
			AttributeFlags = (CorParamAttr)attributeFlags;
			ElementType = (CorElementType)elementType;
			DefaultValue = defaultValue;
			DefaultValueLength = defaultValueLength;
			ReadDefaultValue();
		}

		private void ReadDefaultValue()
		{
			if ((AttributeFlags & CorParamAttr.pdHasDefault) == CorParamAttr.pdHasDefault)
			{
				StringBuilder defaultValue = new StringBuilder();
				defaultValue.Append(".param [");
				defaultValue.Append(OrdinalIndex);
				defaultValue.Append("] = ");

				object defaultValueNumber;
				defaultValue.Append(NuGenHelperFunctions.ReadDefaultValue(ElementType, DefaultValue, DefaultValueLength, out defaultValueNumber));

				DefaultValueAsString = defaultValue.ToString();
			}
		}

		public void ReadMarshalInformation(NuGenIMetaDataImport2 import, Dictionary<uint, NuGenTokenBase> allTokens, int parameterCount)
		{
			if ((AttributeFlags & CorParamAttr.pdHasFieldMarshal) == CorParamAttr.pdHasFieldMarshal)
			{
				MarshalAsTypeString = string.Format("marshal({0})", NuGenHelperFunctions.ReadMarshalDescriptor(import, allTokens, Token, parameterCount));
			}
		}

		public string GetAttributeText()
		{
			AttributeTextBuilder.Length = 0;
			CorParamAttr refParam = (CorParamAttr.pdIn | CorParamAttr.pdOut);

			if ((AttributeFlags & refParam) == refParam)
			{
				AttributeTextBuilder.Append("ref ");
			}
			else if ((AttributeFlags & CorParamAttr.pdOut) == CorParamAttr.pdOut)
			{
				AttributeTextBuilder.Append("[out] ");
			}

			if ((AttributeFlags & CorParamAttr.pdIn) == CorParamAttr.pdIn)
			{
				AttributeTextBuilder.Append("[in] ");
			}

			if ((AttributeFlags & CorParamAttr.pdOptional) == CorParamAttr.pdOptional)
			{
				AttributeTextBuilder.Append("[opt] ");
			}

			return AttributeTextBuilder.ToString();
		}

		public int CompareTo(object obj)
		{
			if (obj == null || obj.GetType() != typeof(NuGenParameter))
			{
				throw new NotSupportedException();
			}

			NuGenParameter otherParameter = (NuGenParameter)obj;

			return OrdinalIndex.CompareTo(otherParameter.OrdinalIndex);
		}

		#region ILazyInitialized Members

		public void LazyInitialize(Dictionary<uint, NuGenTokenBase> allTokens)
		{
			if (!IsLazyInitialized)
			{
				InnerLazyInitialize(false);
				IsLazyInitialized = true;
			}
		}

		#endregion

		public void OpenMetadataAndInitialize()
		{
			if (!IsLazyInitialized)
			{
				InnerLazyInitialize(true);
				IsLazyInitialized = true;
			}
		}

		private void InnerLazyInitialize(bool openMetadata)
		{
			NuGenAssembly assembly = Method.BaseTypeDefinition.ModuleScope.Assembly;

			try
			{
				if (openMetadata)
				{
					assembly.OpenMetadataInterfaces();
				}

				CustomAttributes = NuGenHelperFunctions.EnumCustomAttributes(assembly.Import, assembly, this);
			}
			finally
			{
				if (openMetadata)
				{
					assembly.CloseMetadataInterfaces();
				}
			}

			if (CustomAttributes != null && CustomAttributes.Count > 0)
			{
				foreach (NuGenCustomAttribute customAttribute in CustomAttributes)
				{
					customAttribute.SetText(assembly.AllTokens);
				}
			}
		}

		public bool ParamArrayAttributeExists()
		{
			bool result = false;

			OpenMetadataAndInitialize();

			if (CustomAttributes != null && CustomAttributes.Count > 0)
			{
				int index = 0;

				while (!result && index < CustomAttributes.Count)
				{
					NuGenCustomAttribute customAttribute = CustomAttributes[index++];

					if (customAttribute.Name.Contains(ParamArrayAttribute))
					{
						result = true;
					}
				}
			}

			return result;
		}
	}
}