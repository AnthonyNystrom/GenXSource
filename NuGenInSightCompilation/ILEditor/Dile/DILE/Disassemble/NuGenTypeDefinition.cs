using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Debug.Expressions;
using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.Metadata.Signature;
using Dile.UI;
using Dile.UI.Debug;
using System.Runtime.InteropServices;

namespace Dile.Disassemble
{
	public class NuGenTypeDefinition : NuGenTextTokenBase, NuGenIMultiLine
	{
		public bool IsInMemory
		{
			get
			{
				return ModuleScope.Assembly.IsInMemory;
			}
		}

		public override SearchOptions ItemType
		{
			get
			{
				return SearchOptions.TypeDefinition;
			}
		}

		private NuGenModuleScope moduleScope;
		public NuGenModuleScope ModuleScope
		{
			get
			{
				return moduleScope;
			}
			private set
			{
				moduleScope = value;
			}
		}

		public override string Name
		{
			get
			{
				return name;
			}
			set
			{
				base.Name = value.TrimEnd('\0');
			}
		}

		private CorTypeAttr flags;
		public CorTypeAttr Flags
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

		private uint baseTypeToken;
		public uint BaseTypeToken
		{
			get
			{
				return baseTypeToken;
			}
			private set
			{
				baseTypeToken = value;
			}
		}

		private Dictionary<uint, NuGenMethodDefinition> methodDefinitions;
		public Dictionary<uint, NuGenMethodDefinition> MethodDefinitions
		{
			get
			{
				return methodDefinitions;
			}
			private set
			{
				methodDefinitions = value;
			}
		}

		private Dictionary<uint, NuGenFieldDefinition> fieldDefinitions;
		public Dictionary<uint, NuGenFieldDefinition> FieldDefinitions
		{
			get
			{
				return fieldDefinitions;
			}
			private set
			{
				fieldDefinitions = value;
			}
		}

		private bool isNestedType = false;
		public bool IsNestedType
		{
			get
			{
				return isNestedType;
			}
			private set
			{
				isNestedType = value;
			}
		}

		private NuGenTypeDefinition enclosingType = null;
		public NuGenTypeDefinition EnclosingType
		{
			get
			{
				return enclosingType;
			}
			private set
			{
				enclosingType = value;
			}
		}

		private string shortName = null;
		public string ShortName
		{
			get
			{
				if (shortName == null)
				{
					CreateFullName();
				}

				return shortName;
			}
			private set
			{
				shortName = value;
			}
		}

		private string fullName = null;
		public string FullName
		{
			get
			{
				if (fullName == null)
				{
					CreateFullName();
				}

				return fullName;
			}
			private set
			{
				fullName = value;
			}
		}

		public string Namespace
		{
			get
			{
				string result = string.Empty;
				int lastDotPosition = FullName.LastIndexOf('.');

				if (lastDotPosition > -1)
				{
					result = FullName.Substring(0, lastDotPosition);
				}

				return result;
			}
		}

		private List<NuGenInterfaceImplementation> interfaceImplementations;
		public List<NuGenInterfaceImplementation> InterfaceImplementations
		{
			get
			{
				return interfaceImplementations;
			}
			private set
			{
				interfaceImplementations = value;
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

		private Dictionary<uint, NuGenProperty> properties;
		public Dictionary<uint, NuGenProperty> Properties
		{
			get
			{
				return properties;
			}
			private set
			{
				properties = value;
			}
		}

		public string HeaderText
		{
			get
			{
				return Name;
			}
		}

		private uint packSize;
		public uint PackSize
		{
			get
			{
				return packSize;
			}
			private set
			{
				packSize = value;
			}
		}

		private uint classSize;
		public uint ClassSize
		{
			get
			{
				return classSize;
			}
			private set
			{
				classSize = value;
			}
		}

		private bool layoutSpecified;
		public bool LayoutSpecified
		{
			get
			{
				return layoutSpecified;
			}
			private set
			{
				layoutSpecified = value;
			}
		}

		private List<NuGenPermissionSet> permissionSets;
		public List<NuGenPermissionSet> PermissionSets
		{
			get
			{
				ReadMetadata();

				return permissionSets;
			}
			private set
			{
				permissionSets = value;
			}
		}

		private List<NuGenGenericParameter> genericParameters;
		public List<NuGenGenericParameter> GenericParameters
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

		private static StringBuilder textBuilder = new StringBuilder();
		private static StringBuilder TextBuilder
		{
			get
			{
				return textBuilder;
			}
		}

		private static StringBuilder fullNameBuilder = new StringBuilder();
		private static StringBuilder FullNameBuilder
		{
			get
			{
				return fullNameBuilder;
			}
		}

		private List<NuGenTypeDefinition> nestedTypes = null;
		private List<NuGenTypeDefinition> NestedTypes
		{
			get
			{
				return nestedTypes;
			}
			set
			{
				nestedTypes = value;
			}
		}

		public bool IsValueType
		{
			get
			{
				bool result = false;

				if (ModuleScope.Assembly.AllTokens.ContainsKey(BaseTypeToken))
				{
					NuGenTokenBase baseTypeTokenObject = ModuleScope.Assembly.AllTokens[BaseTypeToken];
					NuGenTypeDefinition baseType = baseTypeTokenObject as NuGenTypeDefinition;

					if (baseType == null)
					{
						NuGenTypeReference baseTypeReference = baseTypeTokenObject as NuGenTypeReference;

						if (baseTypeReference != null)
						{
							baseType = NuGenHelperFunctions.FindTypeByName(baseTypeReference.Name, baseTypeReference.ReferencedAssembly);
						}
					}

					if (baseType != null && baseType.FullName == NuGenConstants.ValueTypeName)
					{
						result = true;
					}
				}

				return result;
			}
		}

		public NuGenTypeDefinition(NuGenIMetaDataImport2 import, NuGenModuleScope moduleScope, uint token) : this(import, moduleScope, string.Empty, token, CorTypeAttr.tdAbstract, 0)
		{
		}

		public NuGenTypeDefinition(NuGenIMetaDataImport2 import, NuGenModuleScope moduleScope, string name, uint token, CorTypeAttr flags, uint baseTypeToken)
		{
			ModuleScope = moduleScope;
			Name = name;
			Token = token;
			Flags = flags;
			BaseTypeToken = baseTypeToken;

			IsNestedType = (((Flags & CorTypeAttr.tdNestedAssembly) == CorTypeAttr.tdNestedAssembly) || ((Flags & CorTypeAttr.tdNestedFamANDAssem) == CorTypeAttr.tdNestedFamANDAssem) || ((Flags & CorTypeAttr.tdNestedFamily) == CorTypeAttr.tdNestedFamily) || ((Flags & CorTypeAttr.tdNestedFamORAssem) == CorTypeAttr.tdNestedFamORAssem) || ((Flags & CorTypeAttr.tdNestedPrivate) == CorTypeAttr.tdNestedPrivate) || ((Flags & CorTypeAttr.tdNestedPublic) == CorTypeAttr.tdNestedPublic));

			NuGenHelperFunctions.GetMemberReferences(ModuleScope.Assembly, Token);
			GetFieldDefinitions(import);
			GetMethodDefinitions(import);
			GetMethodImplementations(import);
			GetImplementedInterfaces(import);
			GetClassLayout(import);
			GetProperties(import);
			GenericParameters = NuGenHelperFunctions.EnumGenericParameters(import, ModuleScope.Assembly, this);
		}

		private void GetClassLayout(NuGenIMetaDataImport2 import)
		{
			CorTypeAttr classLayout = Flags & CorTypeAttr.tdLayoutMask;

			if (NuGenHelperFunctions.EnumContainsValue(classLayout, CorTypeAttr.tdExplicitLayout) || NuGenHelperFunctions.EnumContainsValue(classLayout, CorTypeAttr.tdSequentialLayout))
			{
				NuGenCOR_FIELD_OFFSET[] fieldOffsets = new NuGenCOR_FIELD_OFFSET[NuGenProject.DefaultArrayCount];
				uint count;

				try
				{
					import.GetClassLayout(Token, out packSize, fieldOffsets, Convert.ToUInt32(fieldOffsets.Length), out count, out classSize);

					LayoutSpecified = true;
				}
				catch (COMException)
				{
					LayoutSpecified = false;
				}
			}
			else
			{
				LayoutSpecified = false;
			}
		}

		private void GetMethodDefinitions(NuGenIMetaDataImport2 import)
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] methodDefs = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			import.EnumMethods(ref enumHandle, Token, methodDefs, Convert.ToUInt32(methodDefs.Length), out count);

			if (count > 0)
			{
				MethodDefinitions = new Dictionary<uint, NuGenMethodDefinition>();
			}

			while (count > 0)
			{
				for (uint methodDefsIndex = 0; methodDefsIndex < count; methodDefsIndex++)
				{
					uint token = methodDefs[methodDefsIndex];
					uint typeDefToken;
					uint methodNameLength;
					uint methodFlags;
					IntPtr signature;
					uint signatureLength;
					uint rva;
					uint implementationFlags;

					import.GetMethodProps(token, out typeDefToken, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out methodNameLength, out methodFlags, out signature, out signatureLength, out rva, out implementationFlags);

					if (methodNameLength > NuGenProject.DefaultCharArray.Length)
					{
						NuGenProject.DefaultCharArray = new char[methodNameLength];

						import.GetMethodProps(token, out typeDefToken, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out methodNameLength, out methodFlags, out signature, out signatureLength, out rva, out implementationFlags);
					}

					NuGenMethodDefinition methodDefinition = new NuGenMethodDefinition(import, this, NuGenHelperFunctions.GetString(NuGenProject.DefaultCharArray, 0, methodNameLength), token, methodFlags, signature, signatureLength, rva, implementationFlags);
					MethodDefinitions[token] = methodDefinition;
					ModuleScope.Assembly.AllTokens[token] = methodDefinition;
				}

				import.EnumMethods(ref enumHandle, Token, methodDefs, Convert.ToUInt32(methodDefs.Length), out count);
			}

			import.CloseEnum(enumHandle);
		}

		private void GetMethodImplementations(NuGenIMetaDataImport2 import)
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] methodBodies = new uint[NuGenProject.DefaultArrayCount];
			uint[] methodImpls = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			import.EnumMethodImpls(ref enumHandle, Token, methodBodies, methodImpls, Convert.ToUInt32(methodImpls.Length), out count);

			while (count > 0)
			{
				for (uint index = 0; index < count; index++)
				{
					MethodDefinitions[methodBodies[index]].Overrides = methodImpls[index];
				}

				import.EnumMethodImpls(ref enumHandle, Token, methodBodies, methodImpls, Convert.ToUInt32(methodImpls.Length), out count);
			}

			import.CloseEnum(enumHandle);
		}

		private void GetFieldDefinitions(NuGenIMetaDataImport2 import)
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] fieldDefs = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			import.EnumFields(ref enumHandle, Token, fieldDefs, Convert.ToUInt32(fieldDefs.Length), out count);

			if (count > 0)
			{
				FieldDefinitions = new Dictionary<uint, NuGenFieldDefinition>();
			}

			while (count > 0)
			{
				for (uint fieldDefsIndex = 0; fieldDefsIndex < count; fieldDefsIndex++)
				{
					uint token = fieldDefs[fieldDefsIndex];
					uint typeDefToken;
					uint fieldNameLength;
					uint fieldFlags;
					IntPtr signature;
					uint signatureLength;
					uint defaultValueType;
					IntPtr defaultValue;
					uint defaultValueLength;

					import.GetFieldProps(token, out typeDefToken, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out fieldNameLength, out fieldFlags, out signature, out signatureLength, out defaultValueType, out defaultValue, out defaultValueLength);

					if (fieldNameLength > NuGenProject.DefaultCharArray.Length)
					{
						NuGenProject.DefaultCharArray = new char[fieldNameLength];

						import.GetFieldProps(token, out typeDefToken, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out fieldNameLength, out fieldFlags, out signature, out signatureLength, out defaultValueType, out defaultValue, out defaultValueLength);
					}

					NuGenFieldDefinition fieldDefinition = new NuGenFieldDefinition(import, this, NuGenHelperFunctions.GetString(NuGenProject.DefaultCharArray, 0, fieldNameLength), token, signature, signatureLength, fieldFlags, defaultValueType, defaultValue, defaultValueLength);
					FieldDefinitions[token] = fieldDefinition;
					ModuleScope.Assembly.AllTokens[token] = fieldDefinition;
				}

				import.EnumFields(ref enumHandle, Token, fieldDefs, Convert.ToUInt32(fieldDefs.Length), out count);
			}

			import.CloseEnum(enumHandle);
		}

		private void GetImplementedInterfaces(NuGenIMetaDataImport2 import)
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] interfaces = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			import.EnumInterfaceImpls(ref enumHandle, Token, interfaces, Convert.ToUInt32(interfaces.Length), out count);

			if (count > 0)
			{
				InterfaceImplementations = new List<NuGenInterfaceImplementation>();
			}

			while (count > 0)
			{
				for (uint interfacesIndex = 0; interfacesIndex < count; interfacesIndex++)
				{
					uint token = interfaces[interfacesIndex];
					uint typeDefToken;
					uint interfaceToken;

					import.GetInterfaceImplProps(token, out typeDefToken, out interfaceToken);

					NuGenInterfaceImplementation implementation = new NuGenInterfaceImplementation(import, token, this, interfaceToken);
					InterfaceImplementations.Add(implementation);
					ModuleScope.Assembly.AllTokens[token] = implementation;
				}

				import.EnumInterfaceImpls(ref enumHandle, Token, interfaces, Convert.ToUInt32(interfaces.Length), out count);
			}

			import.CloseEnum(enumHandle);
		}

		private void GetProperties(NuGenIMetaDataImport2 import)
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] propertyTokens = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			import.EnumProperties(ref enumHandle, Token, propertyTokens, Convert.ToUInt32(propertyTokens.Length), out count);

			if (count > 0)
			{
				Properties = new Dictionary<uint, NuGenProperty>();
			}

			while (count > 0)
			{
				for (uint propertyTokensIndex = 0; propertyTokensIndex < count; propertyTokensIndex++)
				{
					uint token = propertyTokens[propertyTokensIndex];
					uint typeDefToken;
					uint nameLength;
					uint flags;
					IntPtr signature;
					uint signatureLength;
					uint elementType;
					IntPtr defaultValue;
					uint defaultValueLength;
					uint setterMethodToken;
					uint getterMethodToken;
					uint[] otherMethods = new uint[NuGenProject.DefaultArrayCount];
					uint otherMethodsCount;

					import.GetPropertyProps(token, out typeDefToken, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out nameLength, out flags, out signature, out signatureLength, out elementType, out defaultValue, out defaultValueLength, out setterMethodToken, out getterMethodToken, otherMethods, Convert.ToUInt32(otherMethods.Length), out otherMethodsCount);

					if (nameLength > NuGenProject.DefaultCharArray.Length || otherMethodsCount > otherMethods.Length)
					{
						NuGenProject.DefaultCharArray = new char[nameLength];
						otherMethods = new uint[otherMethodsCount];

						import.GetPropertyProps(token, out typeDefToken, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out nameLength, out flags, out signature, out signatureLength, out elementType, out defaultValue, out defaultValueLength, out setterMethodToken, out getterMethodToken, otherMethods, Convert.ToUInt32(otherMethods.Length), out otherMethodsCount);
					}

					NuGenProperty property = new NuGenProperty(import, token, this, NuGenHelperFunctions.GetString(NuGenProject.DefaultCharArray, 0, nameLength), flags, signature, signatureLength, elementType, defaultValue, defaultValueLength, setterMethodToken, getterMethodToken, otherMethods, otherMethodsCount);
					Properties[token] = property;
					ModuleScope.Assembly.AllTokens[token] = property;
				}

				import.EnumProperties(ref enumHandle, Token, propertyTokens, Convert.ToUInt32(propertyTokens.Length), out count);
			}

			import.CloseEnum(enumHandle);
		}

		public void FindEnclosingType(NuGenIMetaDataImport2 import)
		{
			if (IsNestedType)
			{
				uint enclosingTypeToken;
				import.GetNestedClassProps(Token, out enclosingTypeToken);

				EnclosingType = ModuleScope.TypeDefinitions[enclosingTypeToken];

				if (EnclosingType.NestedTypes == null)
				{
					EnclosingType.NestedTypes = new List<NuGenTypeDefinition>();
				}

				EnclosingType.NestedTypes.Add(this);
			}
		}

		private string MemberAccessAsString()
		{
			string result = string.Empty;
			CorTypeAttr memberAccess = Flags & CorTypeAttr.tdVisibilityMask;

			switch (memberAccess)
			{
				case CorTypeAttr.tdNotPublic:
					result = "private ";
					break;

				case CorTypeAttr.tdPublic:
					result = "public ";
					break;

				case CorTypeAttr.tdNestedFamANDAssem:
					result = "nested famandassem ";
					break;

				case CorTypeAttr.tdNestedAssembly:
					result = "nested assembly ";
					break;

				case CorTypeAttr.tdNestedFamily:
					result = "nested family ";
					break;

				case CorTypeAttr.tdNestedFamORAssem:
					result = "nested famorassem ";
					break;

				case CorTypeAttr.tdNestedPrivate:
					result = "nested private ";
					break;

				case CorTypeAttr.tdNestedPublic:
					result = "nested public ";
					break;
			}

			return result;
		}

		private string LayoutAsString()
		{
			string result = string.Empty;
			CorTypeAttr layout = Flags & CorTypeAttr.tdLayoutMask;

			switch (layout)
			{
				case CorTypeAttr.tdAutoLayout:
					result = "auto ";
					break;

				case CorTypeAttr.tdSequentialLayout:
					result = "sequential ";
					break;

				case CorTypeAttr.tdExplicitLayout:
					result = "explicit ";
					break;
			}

			return result;
		}

		private string SpecialSemanticsAsString()
		{
			string result = string.Empty;

			if ((Flags & CorTypeAttr.tdAbstract) == CorTypeAttr.tdAbstract)
			{
				result = "abstract ";
			}
			else if ((Flags & CorTypeAttr.tdSealed) == CorTypeAttr.tdSealed)
			{
				result = "sealed ";
			}
			else if ((Flags & CorTypeAttr.tdSpecialName) == CorTypeAttr.tdSpecialName)
			{
				result = "specialname ";
			}

			return result;
		}

		private string ImplementationAsString()
		{
			string result = string.Empty;

			result = NuGenHelperFunctions.EnumAsString(Flags, CorTypeAttr.tdImport, "import ");
			result += NuGenHelperFunctions.EnumAsString(Flags, CorTypeAttr.tdSerializable, "serializable ");

			return result;
		}

		private string StringFormatAsString()
		{
			string result = string.Empty;
			CorTypeAttr stringFormat = Flags & CorTypeAttr.tdStringFormatMask;

			switch (stringFormat)
			{
				case CorTypeAttr.tdAnsiClass:
					result = "ansi ";
					break;

				case CorTypeAttr.tdUnicodeClass:
					result = "unicode ";
					break;

				case CorTypeAttr.tdAutoClass:
					result = "auto ";
					break;
			}

			return result;
		}

		private string ReservedFlagsAsString()
		{
			string result = string.Empty;

			result = NuGenHelperFunctions.EnumAsString(Flags, CorTypeAttr.tdBeforeFieldInit, "beforefieldinit ");

			if ((Flags & CorTypeAttr.tdForwarder) == CorTypeAttr.tdForwarder)
			{
				throw new NotImplementedException("Unknown type flag value (forwarder).");
			}

			CorTypeAttr reservedFlag = Flags & CorTypeAttr.tdReservedMask;

			result += NuGenHelperFunctions.EnumAsString(reservedFlag, CorTypeAttr.tdRTSpecialName, "rtspecialname ");

			return result;
		}

		public void Initialize()
		{
			ReadMetadata();
			CodeLines = new List<NuGenCodeLine>();
			TextBuilder.Length = 0;
			TextBuilder.Append(".class ");

			CorTypeAttr semantics = Flags & CorTypeAttr.tdClassSemanticsMask;

			if (semantics == CorTypeAttr.tdInterface)
			{
				TextBuilder.Append("interface ");
			}

			NuGenHelperFunctions.AddWordToStringBuilder(TextBuilder, MemberAccessAsString());
			NuGenHelperFunctions.AddWordToStringBuilder(TextBuilder, LayoutAsString());
			NuGenHelperFunctions.AddWordToStringBuilder(TextBuilder, StringFormatAsString());
			NuGenHelperFunctions.AddWordToStringBuilder(TextBuilder, SpecialSemanticsAsString());
			NuGenHelperFunctions.AddWordToStringBuilder(TextBuilder, ImplementationAsString());
			NuGenHelperFunctions.AddWordToStringBuilder(TextBuilder, ReservedFlagsAsString());
			TextBuilder.Append(Name);

			if (GenericParameters != null)
			{
				TextBuilder.Append("<");

				for (int index = 0; index < GenericParameters.Count; index++)
				{
					NuGenGenericParameter genericParameter = GenericParameters[index];

					TextBuilder.Append(genericParameter.Text);

					if (index < GenericParameters.Count - 1)
					{
						TextBuilder.Append(", ");
					}
				}

				TextBuilder.Append(">");
			}

			NuGenCodeLine definition = new NuGenCodeLine(0, TextBuilder.ToString());
			CodeLines.Add(definition);

			if (ModuleScope.Assembly.AllTokens.ContainsKey(BaseTypeToken))
			{
				TextBuilder.Length = 0;
				TextBuilder.Append("extends ");
				TextBuilder.Append(ModuleScope.Assembly.AllTokens[BaseTypeToken]);

				NuGenCodeLine extends = new NuGenCodeLine(1, TextBuilder.ToString());
				CodeLines.Add(extends);
			}

			if (InterfaceImplementations != null && InterfaceImplementations.Count > 0)
			{
				TextBuilder.Length = 0;
				for (int index = 0; index < InterfaceImplementations.Count; index++)
				{
					NuGenInterfaceImplementation implementation = InterfaceImplementations[index];

					if (index == 0)
					{
						TextBuilder.Append("implements ");
					}
					else
					{
						TextBuilder.Append(", ");
					}

					TextBuilder.Append(ModuleScope.Assembly.AllTokens[implementation.InterfaceToken]);
				}

				NuGenCodeLine implements = new NuGenCodeLine(1, TextBuilder.ToString());
				CodeLines.Add(implements);
			}

			NuGenCodeLine bracket = new NuGenCodeLine(0, "{");
			CodeLines.Add(bracket);

			if (LayoutSpecified)
			{
				CodeLines.Add(new NuGenCodeLine(1, string.Format(".pack {0}", PackSize)));
				CodeLines.Add(new NuGenCodeLine(1, string.Format(".size {0}", ClassSize)));
			}

			if (CustomAttributes != null)
			{
				NuGenAssembly assembly = ModuleScope.Assembly;

				foreach (NuGenCustomAttribute customAttribute in CustomAttributes)
				{
					customAttribute.SetText(ModuleScope.Assembly.AllTokens);
					CodeLines.Add(new NuGenCodeLine(1, customAttribute.Name));
				}
			}

			if (PermissionSets != null)
			{
				foreach (NuGenPermissionSet permissionSet in PermissionSets)
				{
					CodeLines.Add(new NuGenCodeLine(1, permissionSet.Name));
				}
			}

			bracket = new NuGenCodeLine(0, "} //end of class " + FullName);
			CodeLines.Add(bracket);
		}

		private void CreateFullName()
		{
			FullNameBuilder.Length = 0;
			NuGenTypeDefinition typeDef = this;

			while (typeDef != null)
			{
				FullNameBuilder.Insert(0, string.Format("{0}/", typeDef.Name));
				typeDef = typeDef.EnclosingType;
			}
			FullNameBuilder.Remove(FullNameBuilder.Length - 1, 1);

			ShortName = FullNameBuilder.ToString();

			if (GenericParameters != null)
			{
				FullNameBuilder.Append("<");

				for (int index = 0; index < GenericParameters.Count; index++)
				{
					NuGenGenericParameter genericParameter = GenericParameters[index];

					genericParameter.LazyInitialize(ModuleScope.Assembly.AllTokens);
					FullNameBuilder.Append(genericParameter.Text);

					if (index < GenericParameters.Count - 1)
					{
						FullNameBuilder.Append(", ");
					}
				}

				FullNameBuilder.Append(">");
			}

			FullName = FullNameBuilder.ToString();
		}

		public override string ToString()
		{
			return FullName;
		}

		private K FindTokenBaseByName<T, K>(Dictionary<T, K> tokenBaseObjects, string tokenBaseName, MemberType memberTypeToSearch) where K : NuGenTokenBase
		{
			K result = null;

			if (tokenBaseObjects != null && tokenBaseObjects.Count > 0)
			{
				Dictionary<T, K>.ValueCollection values = tokenBaseObjects.Values;
				Dictionary<T, K>.ValueCollection.Enumerator enumerator = values.GetEnumerator();

				while (result == null && enumerator.MoveNext())
				{
					K value = enumerator.Current;

					if (value.Name == tokenBaseName)
					{
						result = value;
					}
				}
			}

			if (result == null && ModuleScope.Assembly.AllTokens.ContainsKey(BaseTypeToken))
			{
				NuGenTokenBase baseTypeTokenObject = ModuleScope.Assembly.AllTokens[BaseTypeToken];
				NuGenTypeDefinition baseType = baseTypeTokenObject as NuGenTypeDefinition;

				if (baseType == null)
				{
					NuGenTypeReference baseTypeReference = baseTypeTokenObject as NuGenTypeReference;

					if (baseTypeReference != null)
					{
						baseType = NuGenHelperFunctions.FindTypeByName(baseTypeReference.Name, baseTypeReference.ReferencedAssembly);
					}
				}

				if (baseType != null)
				{
					switch (memberTypeToSearch)
					{
						case MemberType.Field:
							result = (K)(NuGenTokenBase)baseType.FindTokenBaseByName<uint, NuGenFieldDefinition>(baseType.FieldDefinitions, tokenBaseName, memberTypeToSearch);
							break;

						case MemberType.Method:
							result = (K)(NuGenTokenBase)baseType.FindTokenBaseByName<uint, NuGenMethodDefinition>(baseType.MethodDefinitions, tokenBaseName, memberTypeToSearch);
							break;

						case MemberType.Property:
							result = (K)(NuGenTokenBase)baseType.FindTokenBaseByName<uint, NuGenProperty>(baseType.Properties, tokenBaseName, memberTypeToSearch);
							break;
					}
				}
			}

			return result;
		}

		public NuGenProperty FindPropertyByName(string propertyName)
		{
			propertyName = NuGenHelperFunctions.QuoteName(propertyName);

			return FindTokenBaseByName<uint, NuGenProperty>(Properties, propertyName, MemberType.Property);
		}

		public NuGenMethodDefinition FindMethodDefinitionByName(string methodName)
		{
			methodName = NuGenHelperFunctions.QuoteMethodName(methodName);

			return FindTokenBaseByName<uint, NuGenMethodDefinition>(MethodDefinitions, methodName, MemberType.Method);
		}

		public List<NuGenMethodDefinition> FindMethodDefinitionsByName(string methodName, int parameterCount)
		{
			List<NuGenMethodDefinition> result = new List<NuGenMethodDefinition>();
			methodName = NuGenHelperFunctions.QuoteMethodName(methodName);
			Dictionary<uint, NuGenMethodDefinition>.ValueCollection values = MethodDefinitions.Values;
			Dictionary<uint, NuGenMethodDefinition>.ValueCollection.Enumerator enumerator = values.GetEnumerator();

			while (enumerator.MoveNext())
			{
				int expectedParameterCountTemp = (enumerator.Current.IsStatic || enumerator.Current.Name == NuGenConstants.ConstructorMethodName ? parameterCount : parameterCount - 1);

				if (enumerator.Current.Name == methodName)
				{
					if (enumerator.Current.Parameters == null)
					{
						if (expectedParameterCountTemp == 0)
						{
							result.Add(enumerator.Current);
						}
					}
					else if (enumerator.Current.Parameters.Count == expectedParameterCountTemp)
					{
						result.Add(enumerator.Current);
					}
					else if (enumerator.Current.Parameters.Count < expectedParameterCountTemp || enumerator.Current.Parameters.Count == expectedParameterCountTemp + 1)
					{
						NuGenParameter lastParameter = enumerator.Current.Parameters[enumerator.Current.Parameters.Count - 1];

						if (lastParameter.ParamArrayAttributeExists())
						{
							result.Add(enumerator.Current);
						}
					}
				}
			}

			if (ModuleScope.Assembly.AllTokens.ContainsKey(BaseTypeToken))
			{
				NuGenTokenBase baseTypeTokenObject = ModuleScope.Assembly.AllTokens[BaseTypeToken];
				NuGenTypeDefinition baseType = baseTypeTokenObject as NuGenTypeDefinition;

				if (baseType == null)
				{
					NuGenTypeReference baseTypeReference = baseTypeTokenObject as NuGenTypeReference;

					if (baseTypeReference != null)
					{
						baseType = NuGenHelperFunctions.FindTypeByName(baseTypeReference.Name, baseTypeReference.ReferencedAssembly) as NuGenTypeDefinition;
					}
				}

				if (baseType != null)
				{
					result.AddRange(baseType.FindMethodDefinitionsByName(methodName, parameterCount));
				}
			}

			return result;
		}

		public NuGenFieldDefinition FindFieldDefinitionByName(string fieldName)
		{
			fieldName = NuGenHelperFunctions.QuoteName(fieldName);
			return FindTokenBaseByName<uint, NuGenFieldDefinition>(FieldDefinitions, fieldName, MemberType.Field);
		}

		protected override void ReadMetadataInformation()
		{
			base.ReadMetadataInformation();
			NuGenAssembly assembly = ModuleScope.Assembly;

			try
			{
				assembly.OpenMetadataInterfaces();

				CustomAttributes = NuGenHelperFunctions.EnumCustomAttributes(assembly.Import, ModuleScope.Assembly, this);

				if (((Flags & CorTypeAttr.tdReservedMask) & CorTypeAttr.tdHasSecurity) == CorTypeAttr.tdHasSecurity)
				{
					PermissionSets = NuGenHelperFunctions.EnumPermissionSets(assembly.Import, Token);
				}
			}
			finally
			{
				assembly.CloseMetadataInterfaces();
			}
		}

		public NuGenTypeDefinition FindNestedTypeByName(string nestedTypeName)
		{
			NuGenTypeDefinition result = null;

			if (NestedTypes != null && NestedTypes.Count > 0)
			{
				int index = 0;

				while (result == null && index < NestedTypes.Count)
				{
					NuGenTypeDefinition nestedType = NestedTypes[index++];

					if (nestedType.Name == nestedTypeName)
					{
						result = nestedType;
					}
				}
			}

			return result;
		}

		public NuGenMethodDefinition FindImplicitCastOperator(NuGenEvaluationContext context, NuGenDebugExpressionResult parameter, NuGenTypeSignatureItem expectedParameter, bool isExpectedParameterArray)
		{
			if (NuGenHelperFunctions.IsArrayElementType(expectedParameter.ElementType))
			{
				expectedParameter = (NuGenTypeSignatureItem)expectedParameter.NextItem;
			}

			NuGenTypeDefinition expectedParameterType = expectedParameter.TokenObject as NuGenTypeDefinition;

			if (expectedParameterType == null)
			{
				NuGenTypeReference typeReference = expectedParameter.TokenObject as NuGenTypeReference;

				if (typeReference == null)
				{
					expectedParameterType = NuGenHelperFunctions.GetTypeByElementType(expectedParameter.ElementType);

					if (expectedParameterType == null)
					{
						throw new InvalidOperationException(string.Format("The definition of the type ({0}) could not be found.", Enum.GetName(typeof(CorElementType), expectedParameter.ElementType)));
					}
				}
				else
				{
					expectedParameterType = NuGenHelperFunctions.FindTypeByName(typeReference.Name, typeReference.ReferencedAssembly);

					if (expectedParameterType == null)
					{
						throw new InvalidOperationException(string.Format("The type definition of {0} could not be found. Perhaps the {1} assembly is not loaded.", typeReference.FullName, typeReference.ReferencedAssembly));
					}
				}
			}

			return FindImplicitCastOperator(context, parameter, expectedParameterType, isExpectedParameterArray);
		}

		public NuGenMethodDefinition FindImplicitCastOperator(NuGenEvaluationContext context, NuGenDebugExpressionResult parameter, NuGenTypeDefinition expectedParameterType, bool isExpectedParameterArray)
		{
			NuGenMethodDefinition result = null;

			List<NuGenDebugExpressionResult> parameters = new List<NuGenDebugExpressionResult>(1);
			parameters.Add(parameter);

			if (NuGenHelperFunctions.HasValueClass(parameter.ResultValue))
			{
				NuGenTypeDefinition parameterType = NuGenHelperFunctions.FindTypeOfValue(context, parameter);

				if (parameterType == null)
				{
					throw new InvalidOperationException("The type of a parameter value could not be determined while searching for implicit cast operators.");
				}

				if (parameterType != this)
				{
					result = parameterType.FindMethodDefinitionByParameter(context, "op_Implicit", parameters, expectedParameterType, isExpectedParameterArray);
				}
			}

			if (expectedParameterType != this)
			{
				NuGenMethodDefinition implicitOperatorMethod = expectedParameterType.FindMethodDefinitionByParameter(context, "op_Implicit", parameters, expectedParameterType, isExpectedParameterArray);

				if (implicitOperatorMethod != null)
				{
					if (result != null)
					{
						throw new InvalidOperationException(string.Format("Two suitable implicit cast operators have been found: {0}, {1}", result.Text, implicitOperatorMethod.Text));
					}

					result = implicitOperatorMethod;
				}
			}

			return result;
		}

		private bool CanConvert(CorElementType valueType, NuGenTypeSignatureItem expectedType)
		{
			bool result = false;

			switch(valueType)
			{
				case CorElementType.ELEMENT_TYPE_CHAR:
					switch(expectedType.ElementType)
					{
						case CorElementType.ELEMENT_TYPE_U2:
						case CorElementType.ELEMENT_TYPE_I4:
						case CorElementType.ELEMENT_TYPE_U4:
						case CorElementType.ELEMENT_TYPE_I8:
						case CorElementType.ELEMENT_TYPE_U8:
						case CorElementType.ELEMENT_TYPE_R4:
						case CorElementType.ELEMENT_TYPE_R8:
							result = true;
							break;

						case CorElementType.ELEMENT_TYPE_VALUETYPE:
							if (expectedType.TokenObject.Name == NuGenConstants.DecimalTypeName)
							{
								result = true;
							}
							break;
					}
					break;

				case CorElementType.ELEMENT_TYPE_I1:
					switch (expectedType.ElementType)
					{
						case CorElementType.ELEMENT_TYPE_I:
						case CorElementType.ELEMENT_TYPE_I2:
						case CorElementType.ELEMENT_TYPE_I4:
						case CorElementType.ELEMENT_TYPE_I8:
						case CorElementType.ELEMENT_TYPE_R4:
						case CorElementType.ELEMENT_TYPE_R8:
							result = true;
							break;

						case CorElementType.ELEMENT_TYPE_VALUETYPE:
							if (expectedType.TokenObject.Name == NuGenConstants.DecimalTypeName)
							{
								result = true;
							}
							break;
					}
					break;

				case CorElementType.ELEMENT_TYPE_U1:
					switch(expectedType.ElementType)
					{
						case CorElementType.ELEMENT_TYPE_I:
						case CorElementType.ELEMENT_TYPE_U:
						case CorElementType.ELEMENT_TYPE_U2:
						case CorElementType.ELEMENT_TYPE_I4:
						case CorElementType.ELEMENT_TYPE_U4:
						case CorElementType.ELEMENT_TYPE_I8:
						case CorElementType.ELEMENT_TYPE_U8:
						case CorElementType.ELEMENT_TYPE_R4:
						case CorElementType.ELEMENT_TYPE_R8:
							result = true;
							break;

						case CorElementType.ELEMENT_TYPE_VALUETYPE:
							if (expectedType.TokenObject.Name == NuGenConstants.DecimalTypeName)
							{
								result = true;
							}
							break;
					}
					break;

				case CorElementType.ELEMENT_TYPE_I2:
					switch(expectedType.ElementType)
					{
						case CorElementType.ELEMENT_TYPE_I:
						case CorElementType.ELEMENT_TYPE_I4:
						case CorElementType.ELEMENT_TYPE_I8:
						case CorElementType.ELEMENT_TYPE_R4:
						case CorElementType.ELEMENT_TYPE_R8:
							result = true;
							break;

						case CorElementType.ELEMENT_TYPE_VALUETYPE:
							if (expectedType.TokenObject.Name == NuGenConstants.DecimalTypeName)
							{
								result = true;
							}
							break;
					}
					break;

				case CorElementType.ELEMENT_TYPE_U2:
					switch(expectedType.ElementType)
					{
						case CorElementType.ELEMENT_TYPE_I:
						case CorElementType.ELEMENT_TYPE_U:
						case CorElementType.ELEMENT_TYPE_I4:
						case CorElementType.ELEMENT_TYPE_U4:
						case CorElementType.ELEMENT_TYPE_I8:
						case CorElementType.ELEMENT_TYPE_U8:
						case CorElementType.ELEMENT_TYPE_R4:
						case CorElementType.ELEMENT_TYPE_R8:
							result = true;
							break;

						case CorElementType.ELEMENT_TYPE_VALUETYPE:
							if (expectedType.TokenObject.Name == NuGenConstants.DecimalTypeName)
							{
								result = true;
							}
							break;
					}
					break;

				case CorElementType.ELEMENT_TYPE_I4:
					switch(expectedType.ElementType)
					{
						case CorElementType.ELEMENT_TYPE_I8:
						case CorElementType.ELEMENT_TYPE_R4:
						case CorElementType.ELEMENT_TYPE_R8:
							result = true;
							break;

						case CorElementType.ELEMENT_TYPE_VALUETYPE:
							if (expectedType.TokenObject.Name == NuGenConstants.DecimalTypeName)
							{
								result = true;
							}
							break;
					}
					break;

				case CorElementType.ELEMENT_TYPE_U4:
					switch (expectedType.ElementType)
					{
						case CorElementType.ELEMENT_TYPE_U8:
						case CorElementType.ELEMENT_TYPE_I8:
						case CorElementType.ELEMENT_TYPE_R4:
						case CorElementType.ELEMENT_TYPE_R8:
							result = true;
							break;

						case CorElementType.ELEMENT_TYPE_VALUETYPE:
							if (expectedType.TokenObject.Name == NuGenConstants.DecimalTypeName)
							{
								result = true;
							}
							break;
					}
					break;

				case CorElementType.ELEMENT_TYPE_I8:
				case CorElementType.ELEMENT_TYPE_U8:
					switch(expectedType.ElementType)
					{
						case CorElementType.ELEMENT_TYPE_R4:
						case CorElementType.ELEMENT_TYPE_R8:
							result = true;
							break;

						case CorElementType.ELEMENT_TYPE_VALUETYPE:
							if (expectedType.TokenObject.Name == NuGenConstants.DecimalTypeName)
							{
								result = true;
							}
							break;
					}
					break;

				case CorElementType.ELEMENT_TYPE_R4:
					if (expectedType.ElementType == CorElementType.ELEMENT_TYPE_R8)
					{
						result = true;
					}
					break;
			}

			return result;
		}

		public bool IsSubclassOrImplements(NuGenTypeDefinition typeDefinition)
		{
			bool result = false;

			if (typeDefinition == this)
			{
				result = true;
			}
			else
			{
				NuGenTypeDefinition baseType = NuGenHelperFunctions.FindTypeByToken(ModuleScope.Assembly, BaseTypeToken);

				if (baseType != null)
				{
					result = baseType.IsSubclassOrImplements(typeDefinition);
				}

				if (!result && InterfaceImplementations != null)
				{
					int index = 0;

					while (!result && index < InterfaceImplementations.Count)
					{
						NuGenInterfaceImplementation interfaceImplementation = InterfaceImplementations[index++];
						NuGenTypeDefinition interfaceTypeDefinition = NuGenHelperFunctions.FindTypeByToken(ModuleScope.Assembly, interfaceImplementation.InterfaceToken);

						result = interfaceTypeDefinition.IsSubclassOrImplements(typeDefinition);
					}
				}
			}

			return result;
		}

		private bool ParameterTypesMatch(NuGenTypeDefinition currentParameterType, NuGenTypeSignatureItem definedParameter)
		{
			bool result = false;

			if (currentParameterType != null)
			{
				if (currentParameterType.FullName == definedParameter.GetTokenObjectName(false))
				{
					result = true;
				}
				else
				{
					CorElementType currentParameterElementType = NuGenHelperFunctions.GetElementTypeByName(currentParameterType.FullName);

					if (currentParameterElementType != CorElementType.ELEMENT_TYPE_END && currentParameterElementType == definedParameter.ElementType)
					{
						result = true;
					}
					else
					{
						NuGenTypeDefinition currentParameterBaseType = NuGenHelperFunctions.FindTypeByToken(currentParameterType.ModuleScope.Assembly, currentParameterType.BaseTypeToken);

						if (currentParameterBaseType != null)
						{
							result = ParameterTypesMatch(currentParameterBaseType, definedParameter);
						}

						if (!result && currentParameterType != null && currentParameterType.InterfaceImplementations != null)
						{
							int index = 0;

							while (!result && index < currentParameterType.InterfaceImplementations.Count)
							{
								NuGenInterfaceImplementation interfaceImplementation = currentParameterType.InterfaceImplementations[index++];
								NuGenTypeDefinition interfaceTypeDefinition = NuGenHelperFunctions.FindTypeByToken(currentParameterType.ModuleScope.Assembly, interfaceImplementation.InterfaceToken);

								result = ParameterTypesMatch(interfaceTypeDefinition, definedParameter);
							}
						}
					}
				}
			}

			return result;
		}

		private bool ParameterTypesMatch(NuGenEvaluationContext context, NuGenMethodDefinition methodDefinition, NuGenDebugExpressionResult currentParameter, int parameterIndex, bool automaticConversionEnabled, bool isParamsParameter)
		{
			NuGenTypeSignatureItem definedParameter = (NuGenTypeSignatureItem)methodDefinition.MethodSignatureReader.Parameters[parameterIndex];

			return ParameterTypesMatch(context, methodDefinition, currentParameter, definedParameter, automaticConversionEnabled, isParamsParameter);
		}

		private bool ParameterTypesMatch(NuGenEvaluationContext context, NuGenMethodDefinition methodDefinition, NuGenDebugExpressionResult currentParameter, NuGenTypeSignatureItem definedParameter, bool automaticConversionEnabled, bool isParamsParameter)
		{
			bool result = false;

			if (isParamsParameter)
			{
				NuGenTypeSignatureItem arrayElementItem = (NuGenTypeSignatureItem)definedParameter.NextItem;
				definedParameter = arrayElementItem;
			}

			CorElementType currentParameterType = (CorElementType)currentParameter.ResultValue.ElementType;
			CorElementType definedParameterType = definedParameter.ElementType;

			if (NuGenHelperFunctions.HasValueClass(currentParameterType) && NuGenHelperFunctions.HasValueClass(definedParameterType))
			{
				result = true;
			}
			
			if (result || (currentParameterType == definedParameterType) ||
				(automaticConversionEnabled && (CanConvert(currentParameterType, definedParameter))))
			{
				if (result && (currentParameterType == CorElementType.ELEMENT_TYPE_OBJECT || definedParameterType == CorElementType.ELEMENT_TYPE_OBJECT))
				{
					result = true;
				}
				else if (NuGenHelperFunctions.HasValueClass(currentParameterType))
				{
					NuGenTypeDefinition currentParameterTypeDefinition = NuGenHelperFunctions.FindTypeOfValue(context, currentParameter);

					result = ParameterTypesMatch(currentParameterTypeDefinition, definedParameter);
				}
				else
				{
					result = true;
				}
			}

			if (!result && automaticConversionEnabled && methodDefinition.Name != NuGenConstants.ImplicitOperatorMethodName && FindImplicitCastOperator(context, currentParameter, definedParameter, NuGenHelperFunctions.IsArrayElementType(definedParameter.ElementType)) != null)
			{
				result = true;
			}

			return result;
		}

		public NuGenMethodDefinition FindMethodDefinitionByParameter(NuGenEvaluationContext context, string methodName, List<NuGenDebugExpressionResult> parameters)
		{
			return FindMethodDefinitionByParameter(context, methodName, parameters, null, false);
		}

		public NuGenMethodDefinition FindMethodDefinitionByParameter(NuGenEvaluationContext context, string methodName, List<NuGenDebugExpressionResult> parameters, NuGenTypeDefinition expectedReturnType, bool isArrayReturnTypeExpected)
		{
			List<NuGenMethodDefinition> possibleMethods = FindMethodDefinitionsByName(methodName, parameters.Count);
			NuGenMethodDefinition result = null;

			result = SearchPossibleMethods(context, parameters, possibleMethods, expectedReturnType, isArrayReturnTypeExpected, false);

			return result;
		}

		private NuGenMethodDefinition SearchPossibleMethods(NuGenEvaluationContext context, List<NuGenDebugExpressionResult> parameters, List<NuGenMethodDefinition> possibleMethods, NuGenTypeDefinition expectedReturnType, bool isArrayReturnTypeExpected, bool isRecursiveCall)
		{
			NuGenMethodDefinition result = null;
			int methodIndex = 0;
			bool paramsParameterFound = false;

			while (result == null && methodIndex < possibleMethods.Count)
			{
				paramsParameterFound = false;
				NuGenMethodDefinition method = possibleMethods[methodIndex++];
				int parameterIndexModifier = (method.IsStatic || method.Name == NuGenConstants.ConstructorMethodName ? 0 : 1);
				bool allParameterTypesMatch = true;

				if (method.Parameters == null)
				{
					if (parameters.Count > parameterIndexModifier)
					{
						allParameterTypesMatch = false;
					}
					else if (!method.IsStatic && method.Name != NuGenConstants.ConstructorMethodName)
					{
						if (parameters.Count == 0)
						{
							allParameterTypesMatch = false;
						}
						else
						{
							NuGenTypeDefinition thisTypeDefinition = NuGenHelperFunctions.FindTypeOfValue(context, parameters[0]);

							allParameterTypesMatch = NuGenHelperFunctions.TypeDefinitionsMatch(thisTypeDefinition, method.BaseTypeDefinition);
						}
					}
				}
				
				if (method.Parameters != null && allParameterTypesMatch)
				{
					int parameterIndex = parameterIndexModifier;
					int methodParameterIndex = 0;

					while (allParameterTypesMatch && parameterIndex < parameters.Count)
					{
						if (methodParameterIndex == method.Parameters.Count - 1 && !paramsParameterFound)
						{
							paramsParameterFound = method.Parameters[methodParameterIndex].ParamArrayAttributeExists();
						}

						if (ParameterTypesMatch(context, method, parameters[parameterIndex], methodParameterIndex, isRecursiveCall, paramsParameterFound))
						{
							parameterIndex++;

							if (methodParameterIndex < method.Parameters.Count - 1)
							{
								methodParameterIndex++;
							}
						}
						else
						{
							allParameterTypesMatch = false;
						}
					}

					if (method.Parameters.Count != parameters.Count && !paramsParameterFound)
					{
						paramsParameterFound = method.Parameters[method.Parameters.Count - 1].ParamArrayAttributeExists();
					}
				}

				if (allParameterTypesMatch && expectedReturnType != null)
				{
					NuGenTypeSignatureItem returnSignature = (NuGenTypeSignatureItem)method.MethodSignatureReader.ReturnType;

					if (isArrayReturnTypeExpected)
					{
						if (NuGenHelperFunctions.IsArrayElementType(returnSignature.ElementType))
						{
							returnSignature = (NuGenTypeSignatureItem)returnSignature.NextItem;
						}
						else
						{
							allParameterTypesMatch = false;
						}
					}
					
					if (allParameterTypesMatch && !ParameterTypesMatch(expectedReturnType, returnSignature))
					{
						allParameterTypesMatch = false;
					}
				}

				if (allParameterTypesMatch)
				{
					result = method;

					if (isRecursiveCall || paramsParameterFound)
					{
						ExecuteAutomaticParameterConversion(context, parameters, method, parameterIndexModifier, paramsParameterFound);
					}
				}
			}

			if (result == null && !isRecursiveCall)
			{
				result = SearchPossibleMethods(context, parameters, possibleMethods, expectedReturnType, isArrayReturnTypeExpected, true);
			}

			return result;
		}

		private void ExecuteAutomaticParameterConversion(NuGenEvaluationContext context, List<NuGenDebugExpressionResult> parameters, NuGenMethodDefinition method, int parameterIndexModifier, bool paramsParameterFound)
		{
			if (method.Parameters != null)
			{
				for (int index = 0; index < method.Parameters.Count; index++)
				{
					NuGenTypeSignatureItem methodSignatureParameter = (NuGenTypeSignatureItem)method.MethodSignatureReader.Parameters[index];

					if (index == method.Parameters.Count - 1 && paramsParameterFound)
					{
						NuGenTypeDefinition methodParameterTypeDef = null;
						NuGenTypeSignatureItem methodParameterArrayElementType = (NuGenTypeSignatureItem)methodSignatureParameter.NextItem;
						ClassWrapper parameterClass = null;

						if (methodParameterArrayElementType.TokenObject != null)
						{
							methodParameterTypeDef = methodParameterArrayElementType.TokenObject as NuGenTypeDefinition;

							if (methodParameterTypeDef == null)
							{
								NuGenTypeReference methodParameterTypeRef = methodParameterArrayElementType.TokenObject as NuGenTypeReference;

								if (methodParameterTypeRef != null)
								{
									methodParameterTypeDef = NuGenHelperFunctions.FindTypeByName(methodParameterTypeRef.Name, methodParameterTypeRef.ReferencedAssembly) as NuGenTypeDefinition;
								}
							}

							if (methodParameterTypeDef == null)
							{
								throw new InvalidOperationException(string.Format("The type definition of the {0} method's last parameter could not be found.", method.Text));
							}

							parameterClass = NuGenHelperFunctions.FindClassOfTypeDefintion(context, methodParameterTypeDef);
						}

						uint arrayLength = Convert.ToUInt32(parameters.Count - index - parameterIndexModifier);
						ValueWrapper paramsArrayValueWrapper = null;
						ArrayValueWrapper paramsArray = null;

						context.EvalWrapper.NewArray((int)methodParameterArrayElementType.ElementType, parameterClass, arrayLength);

						NuGenBaseEvaluationResult evaluationResult = context.EvaluationHandler.RunEvaluation(context.EvalWrapper);

						if (evaluationResult.IsSuccessful)
						{
							paramsArrayValueWrapper = evaluationResult.Result;
							paramsArray = paramsArrayValueWrapper.ConvertToArrayValue();
						}
						else
						{
							evaluationResult.ThrowExceptionAccordingToReason();
						}

						int parameterIndex = parameterIndexModifier + index;

						for (int remainingIndex = parameterIndex; remainingIndex < parameters.Count; remainingIndex++)
						{
							NuGenDebugExpressionResult parameterExpression = parameters[remainingIndex];

							ConvertParameter(context, parameterExpression, methodParameterArrayElementType);

							paramsArray = paramsArrayValueWrapper.ConvertToArrayValue();

							ValueWrapper paramsArrayElement = paramsArray.GetElementAtPosition(Convert.ToUInt32(remainingIndex - parameterIndex));

							if (NuGenHelperFunctions.HasValueClass(parameterExpression.ResultValue))
							{
								paramsArrayElement.SetValue(parameterExpression.ResultValue);
							}
							else
							{
								NuGenHelperFunctions.CastDebugValue(parameterExpression.ResultValue, paramsArrayElement);
							}
						}

						parameters.RemoveRange(parameterIndex, parameters.Count - parameterIndex);
						parameters.Add(new NuGenDebugExpressionResult(context, paramsArrayValueWrapper));
					}
					else
					{
						NuGenDebugExpressionResult parameterExpression = parameters[parameterIndexModifier + index];

						ConvertParameter(context, parameterExpression, methodSignatureParameter);
					}
				}
			}
		}

		private void ConvertParameter(NuGenEvaluationContext context, NuGenDebugExpressionResult parameterExpression, NuGenTypeSignatureItem methodParameter)
		{
			NuGenTypeDefinition parameterTypeDefinition = null;

			if (NuGenHelperFunctions.HasValueClass(parameterExpression.ResultValue))
			{
				parameterTypeDefinition = NuGenHelperFunctions.FindTypeOfValue(context, parameterExpression);
			}
			else
			{
				parameterTypeDefinition = NuGenHelperFunctions.GetTypeByElementType((CorElementType)parameterExpression.ResultValue.ElementType);
			}

			if (!ParameterTypesMatch(parameterTypeDefinition, methodParameter))
			{
				if (CanConvert((CorElementType)parameterExpression.ResultValue.ElementType, methodParameter))
				{
					if (methodParameter.TokenObject != null && methodParameter.TokenObject.Name == NuGenConstants.DecimalTypeName)
					{
						parameterExpression.ResultValue = NuGenHelperFunctions.CastToDecimal(context, parameterExpression.ResultValue);
					}
					else
					{
						ValueWrapper castedParameter = context.EvalWrapper.CreateValue((int)methodParameter.ElementType, null);
						NuGenHelperFunctions.CastDebugValue(parameterExpression.ResultValue, castedParameter);
						parameterExpression.ResultValue = castedParameter;
					}
				}
				else
				{
					NuGenMethodDefinition implicitOperatorMethod = FindImplicitCastOperator(context, parameterExpression, methodParameter, NuGenHelperFunctions.IsArrayElementType(methodParameter.ElementType));

					if (implicitOperatorMethod == null)
					{
						throw new InvalidOperationException(string.Format("No suitable implicit cast operator has been found to cast a value from {0} type to {1} type.", parameterTypeDefinition.FullName, methodParameter.GetTokenObjectName(true)));
					}

					List<ValueWrapper> arguments = new List<ValueWrapper>();
					arguments.Add(parameterExpression.ResultValue);

					NuGenBaseEvaluationResult evaluationResult = context.EvaluationHandler.CallMethod(context, implicitOperatorMethod, arguments);

					if (evaluationResult.IsSuccessful)
					{
						parameterExpression.ResultValue = evaluationResult.Result;
					}
					else
					{
						evaluationResult.ThrowExceptionAccordingToReason();
					}
				}
			}
		}

		public NuGenMethodDefinition SearchToStringMethod(NuGenTypeDefinition stringTypeDef)
		{
			NuGenMethodDefinition result = null;

			if (MethodDefinitions != null)
			{
				Dictionary<uint, NuGenMethodDefinition>.Enumerator methodDefEnumerator = MethodDefinitions.GetEnumerator();

				while (result == null && methodDefEnumerator.MoveNext())
				{
					NuGenMethodDefinition method = methodDefEnumerator.Current.Value;

					if (!method.IsStatic && method.Name == NuGenConstants.ToStringMethodName && ParameterTypesMatch(stringTypeDef, (NuGenTypeSignatureItem)method.MethodSignatureReader.ReturnType))
					{
						if (method.Parameters == null || method.Parameters.Count == 0 || (method.Parameters.Count == 1 && method.Parameters[0].ParamArrayAttributeExists()))
						{
							result = method;
						}
					}
				}
			}

			return result;
		}
	}
}