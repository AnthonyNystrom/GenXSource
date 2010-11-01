using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.Metadata.Signature;
using Dile.UI;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace Dile.Disassemble
{
	public class NuGenMethodDefinition : NuGenTextTokenBase, NuGenIMultiLine, NuGenIHasSignature
	{
		//Signature token values start from 0x11000001. This default value seem to indicate that there's no LocalVarSig.
		private const int DefaultSignatureValue = 0x11000000;

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
				return SearchOptions.MethodDefinition;
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

		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				name = NuGenHelperFunctions.QuoteMethodName(value);
			}
		}

		private bool entryPoint = false;
		public bool EntryPoint
		{
			get
			{
				return entryPoint;
			}
			set
			{
				entryPoint = value;
			}
		}

		private CorMethodAttr flags;
		public CorMethodAttr Flags
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

		public bool IsStatic
		{
			get
			{
				return ((flags & CorMethodAttr.mdStatic) == CorMethodAttr.mdStatic);
			}
		}

		private IntPtr signaturePointer;
		public IntPtr SignaturePointer
		{
			get
			{
				return signaturePointer;
			}
			private set
			{
				signaturePointer = value;
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

		private string text = null;
		public string Text
		{
			get
			{
				if (text == null)
				{
					ReadSignature();
					string returnTypeText = signatureReader.ReturnType.ToString();
					string callingConventionName = NuGenHelperFunctions.GetCallingConventionName(CallingConvention);

					StringBuilder parameterListBuilder = new StringBuilder();
					parameterListBuilder.Append("(");

					if (signatureReader.Parameters != null)
					{
						for (int parametersIndex = 0; parametersIndex < signatureReader.Parameters.Count; parametersIndex++)
						{
							NuGenBaseSignatureItem parameterItem = signatureReader.Parameters[parametersIndex];
							NuGenParameter parameter = FindParameterByOrdinalIndex(parametersIndex + 1);

							string parameterItemAsString = parameterItem.ToString();
							string attributeText = (parameter == null ? string.Empty : parameter.GetAttributeText());

							if (parametersIndex == signatureReader.Parameters.Count - 1)
							{
								parameterListBuilder.Append(parameterItemAsString);
							}
							else
							{
								parameterListBuilder.Append(parameterItemAsString);
								parameterListBuilder.Append(", ");
							}
						}
					}

					parameterListBuilder.Append(")");

					if (callingConventionName.Length > 0)
					{
						text = string.Format("{0} {1} {2}{3}{4}{5}", callingConventionName, returnTypeText, BaseTypeDefinition.FullName, (BaseTypeDefinition.FullName.Length == 0 ? string.Empty : "::"), Name, parameterListBuilder.ToString());
					}
					else
					{
						text = string.Format("{0} {1}{2}{3}{4}", returnTypeText, BaseTypeDefinition.FullName, (BaseTypeDefinition.FullName.Length == 0 ? string.Empty : "::"), Name, parameterListBuilder.ToString());
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

		private List<NuGenParameter> parameters;
		public List<NuGenParameter> Parameters
		{
			get
			{
				return parameters;
			}
			private set
			{
				parameters = value;
			}
		}

		private string displayName = null;
		public string DisplayName
		{
			get
			{
				if (displayName == null)
				{
					NuGenAssembly assembly = BaseTypeDefinition.ModuleScope.Assembly;
					ReadSignature();
					string returnTypeText = signatureReader.ReturnType.ToString();
					NuGenParameter returnParameter = FindParameterByOrdinalIndex(0);
					DisplayNameBuilder.Length = 0;
					DisplayNameBuilder.Append(Name);
					DisplayNameBuilder.Append(" : ");
					DisplayNameBuilder.Append(returnTypeText);

					if (GenericParameters != null)
					{
						DisplayNameBuilder.Append("<");

						for (int index = 0; index < GenericParameters.Count; index++)
						{
							NuGenGenericParameter genericParameter = GenericParameters[index];

							genericParameter.LazyInitialize(assembly.AllTokens);
							DisplayNameBuilder.Append(genericParameter.Text);

							if (index < GenericParameters.Count - 1)
							{
								DisplayNameBuilder.Append(", ");
							}
						}

						DisplayNameBuilder.Append(">");
					}

					DisplayNameBuilder.Append("(");

					if (signatureReader.Parameters != null)
					{
						for (int parametersIndex = 0; parametersIndex < signatureReader.Parameters.Count; parametersIndex++)
						{
							NuGenBaseSignatureItem parameterItem = signatureReader.Parameters[parametersIndex];
							NuGenParameter parameter = FindParameterByOrdinalIndex(parametersIndex + 1);

							string parameterItemAsString = parameterItem.ToString();
							string attributeText = (parameter == null ? string.Empty : parameter.GetAttributeText());

							if (parametersIndex == signatureReader.Parameters.Count - 1)
							{
								DisplayNameBuilder.Append(parameterItemAsString);
							}
							else
							{
								DisplayNameBuilder.Append(parameterItemAsString);
								DisplayNameBuilder.Append(", ");
							}
						}
					}

					DisplayNameBuilder.Append(")");
					DisplayName = DisplayNameBuilder.ToString();
				}

				return displayName;
			}
			private set
			{
				displayName = value;
			}
		}

		public string HeaderText
		{
			get
			{
				return string.Format("{0}.{1}", BaseTypeDefinition.Name, Name);
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

		private CorCallingConvention callingConvention;
		public CorCallingConvention CallingConvention
		{
			get
			{
				return callingConvention;
			}
			private set
			{
				callingConvention = value;
			}
		}

		private uint overrides = 0;
		public uint Overrides
		{
			get
			{
				return overrides;
			}
			set
			{
				overrides = value;
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
			set
			{
				pinvokeMap = value;
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

		private List<NuGenMethodSpec> methodSpecs;
		public List<NuGenMethodSpec> MethodSpecs
		{
			get
			{
				return methodSpecs;
			}
			private set
			{
				methodSpecs = value;
			}
		}

		private NuGenMethodSignatureReader signatureReader;
		public NuGenBaseSignatureReader SignatureReader
		{
			get
			{
				ReadSignature();

				return signatureReader;
			}
		}

		public NuGenMethodSignatureReader MethodSignatureReader
		{
			get
			{
				ReadSignature();

				return signatureReader;
			}
		}

		private NuGenProperty ownerProperty = null;
		public NuGenProperty OwnerProperty
		{
			get
			{
				return ownerProperty;
			}
			set
			{
				ownerProperty = value;
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

		private static StringBuilder displayNameBuilder = new StringBuilder();
		private static StringBuilder DisplayNameBuilder
		{
			get
			{
				return displayNameBuilder;
			}
		}

		private uint localVarSigToken;
		private uint LocalVarSigToken
		{
			get
			{
				return localVarSigToken;
			}
			set
			{
				localVarSigToken = value;
			}
		}

		private ulong methodAddress;
		private ulong MethodAddress
		{
			get
			{
				return methodAddress;
			}
			set
			{
				methodAddress = value;
			}
		}

		public NuGenMethodDefinition(NuGenIMetaDataImport2 import, NuGenTypeDefinition typeDefinition, string name, uint token, uint flags, IntPtr signaturePointer, uint signatureLength, uint rva, uint implementationFlags)
		{
			BaseTypeDefinition = typeDefinition;
			Name = name;
			Token = token;
			Flags = (CorMethodAttr)flags;
			SignaturePointer = signaturePointer;
			SignatureLength = signatureLength;
			Rva = rva;
			ImplementationFlags = (CorMethodImpl)implementationFlags;

			NuGenAssembly assembly = BaseTypeDefinition.ModuleScope.Assembly;

			NuGenHelperFunctions.GetMemberReferences(assembly, token);
			EnumParameters(import);
			GenericParameters = NuGenHelperFunctions.EnumGenericParameters(import, assembly, this);
			MethodSpecs = NuGenHelperFunctions.EnumMethodSpecs(import, assembly, this);
			
			if (assembly.ModuleScope.DebuggedModule != null)
			{
				FunctionWrapper debuggedFunction = assembly.ModuleScope.DebuggedModule.GetFunction(Token);

				try
				{
					LocalVarSigToken = debuggedFunction.GetLocalVarSigToken();

					if (LocalVarSigToken == DefaultSignatureValue)
					{
						LocalVarSigToken = 0;
					}

					MethodAddress = debuggedFunction.GetAddress();
				}
				catch (COMException comException)
				{
					//0x8013130a exception means that the method is native.
					if ((uint)comException.ErrorCode == 0x8013130a)
					{
						LocalVarSigToken = 0;
					}
				}
			}
		}

		private void EnumParameters(NuGenIMetaDataImport2 import)
		{
			NuGenAssembly assembly = BaseTypeDefinition.ModuleScope.Assembly;
			IntPtr enumHandle = IntPtr.Zero;
			uint[] parameterTokens = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			import.EnumParams(ref enumHandle, Token, parameterTokens, Convert.ToUInt32(parameterTokens.Length), out count);

			if (count > 0)
			{
				Parameters = new List<NuGenParameter>();
			}

			while (count > 0)
			{
				for (uint parameterTokensIndex = 0; parameterTokensIndex < count; parameterTokensIndex++)
				{
					uint methodToken;
					uint parameterToken = parameterTokens[parameterTokensIndex];
					uint ordinalIndex;
					uint nameLength;
					uint attributeFlags;
					uint elementType;
					IntPtr defaultValue;
					uint defaultValueLength;

					import.GetParamProps(parameterToken, out methodToken, out ordinalIndex, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out nameLength, out attributeFlags, out elementType, out defaultValue, out defaultValueLength);

					if (nameLength > NuGenProject.DefaultCharArray.Length)
					{
						NuGenProject.DefaultCharArray = new char[nameLength];

						import.GetParamProps(parameterToken, out methodToken, out ordinalIndex, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out nameLength, out attributeFlags, out elementType, out defaultValue, out defaultValueLength);
					}

					NuGenParameter parameter = new NuGenParameter(import, assembly.AllTokens, parameterToken, this, ordinalIndex, NuGenHelperFunctions.GetString(NuGenProject.DefaultCharArray, 0, nameLength), attributeFlags, elementType, defaultValue, defaultValueLength);
					Parameters.Add(parameter);
					assembly.AllTokens[Token] = parameter;
				}

				import.EnumParams(ref enumHandle, Token, parameterTokens, Convert.ToUInt32(parameterTokens.Length), out count);
			}

			import.CloseEnum(enumHandle);

			if (Parameters != null)
			{
				Parameters.Sort();

				foreach (NuGenParameter parameter in Parameters)
				{
					parameter.ReadMarshalInformation(import, assembly.AllTokens, Parameters.Count);
				}
			}
		}

		private string MemberAccessAsString()
		{
			string result = string.Empty;
			CorMethodAttr memberAccess = Flags & CorMethodAttr.mdMemberAccessMask;

			switch (memberAccess)
			{
				case CorMethodAttr.mdPrivateScope:
					result = "privatescope ";
					break;

				case CorMethodAttr.mdPrivate:
					result = "private ";
					break;

				case CorMethodAttr.mdFamANDAssem:
					result = "famandassem ";
					break;

				case CorMethodAttr.mdAssem:
					result = "assembly ";
					break;

				case CorMethodAttr.mdFamily:
					result = "family ";
					break;

				case CorMethodAttr.mdFamORAssem:
					result = "famorassem ";
					break;

				case CorMethodAttr.mdPublic:
					result = "public ";
					break;
			}

			return result.ToString();
		}

		private string MethodContractAsString()
		{
			StringBuilder result = new StringBuilder();

			result.Append(NuGenHelperFunctions.EnumAsString(Flags, CorMethodAttr.mdHideBySig, "hidebysig "));

			result.Append(NuGenHelperFunctions.EnumAsString(Flags, CorMethodAttr.mdStatic, "static "));

			result.Append(NuGenHelperFunctions.EnumAsString(Flags, CorMethodAttr.mdFinal, "final "));

			result.Append(NuGenHelperFunctions.EnumAsString(Flags, CorMethodAttr.mdVirtual, "virtual "));

			return result.ToString();
		}

		private string VTableLayoutAsString()
		{
			string result = string.Empty;

			result = NuGenHelperFunctions.EnumAsString(Flags, CorMethodAttr.mdNewSlot, "newslot ");

			return result;
		}

		private string MethodImplementationAsString()
		{
			StringBuilder result = new StringBuilder();

			result.Append(NuGenHelperFunctions.EnumAsString(Flags, CorMethodAttr.mdAbstract, "abstract "));
			result.Append(NuGenHelperFunctions.EnumAsString(Flags, CorMethodAttr.mdSpecialName, "specialname "));

			return result.ToString();
		}

		private string InteropAttributesAsString()
		{
			StringBuilder result = new StringBuilder();

			if ((Flags & CorMethodAttr.mdPinvokeImpl) == CorMethodAttr.mdPinvokeImpl)
			{
				result.Append("pinvokeimpl(");

				if (PinvokeMap == null)
				{
					result.Append("/* No map */");
				}
				else
				{
					result.Append(PinvokeMap);
				}

				result.Append(") ");
			}

			result.Append(NuGenHelperFunctions.EnumAsString(Flags, CorMethodAttr.mdUnmanagedExport, "unmanagedexp "));

			return result.ToString();
		}

		private string ReservedFlagsAsString()
		{
			StringBuilder result = new StringBuilder();

			if ((Flags & CorMethodAttr.mdReservedMask) == CorMethodAttr.mdReservedMask)
			{
				throw new NotSupportedException("Unknown method flag value (reserved mask).");
			}

			result.Append(NuGenHelperFunctions.EnumAsString(Flags, CorMethodAttr.mdRTSpecialName, "rtspecialname "));
			result.Append(NuGenHelperFunctions.EnumAsString(Flags, CorMethodAttr.mdRequireSecObject, "reqsecobj "));

			return result.ToString();
		}

		private string CodeImplementationAsString()
		{
			string result = string.Empty;
			CorMethodImpl implementation = ImplementationFlags & CorMethodImpl.miCodeTypeMask;

			switch (implementation)
			{
				case CorMethodImpl.miIL:
					result = "cil ";
					break;

				case CorMethodImpl.miNative:
					result = "native ";
					break;

				case CorMethodImpl.miOPTIL:
					result = "optil ";
					break;

				case CorMethodImpl.miRuntime:
					result = "runtime ";
					break;
			}

			return result;
		}

		private string ManagedImplementationAsString()
		{
			string result = string.Empty;
			CorMethodImpl implementation = ImplementationFlags & CorMethodImpl.miManagedMask;

			switch (implementation)
			{
				case CorMethodImpl.miUnmanaged:
					result = "unmanaged ";
					break;

				case CorMethodImpl.miManaged:
					result = "managed ";
					break;
			}

			return result;
		}

		private string ImplementationInfoAsString()
		{
			StringBuilder result = new StringBuilder();

			result.Append(NuGenHelperFunctions.EnumAsString(ImplementationFlags, CorMethodImpl.miForwardRef, "forwardref "));

			result.Append(NuGenHelperFunctions.EnumAsString(ImplementationFlags, CorMethodImpl.miPreserveSig, "preservesig "));

			result.Append(NuGenHelperFunctions.EnumAsString(ImplementationFlags, CorMethodImpl.miInternalCall, "internalcall "));

			result.Append(NuGenHelperFunctions.EnumAsString(ImplementationFlags, CorMethodImpl.miSynchronized, "synchronized "));

			result.Append(NuGenHelperFunctions.EnumAsString(ImplementationFlags, CorMethodImpl.miNoInlining, "noinlining "));

			return result.ToString();
		}

		private NuGenCodeLine CreateMethodHead(string definition)
		{
			NuGenCodeLine result = new NuGenCodeLine();
			StringBuilder text = new StringBuilder();

			text.Append(".method ");
			NuGenHelperFunctions.AddWordToStringBuilder(text, MemberAccessAsString());
			NuGenHelperFunctions.AddWordToStringBuilder(text, MethodContractAsString());
			NuGenHelperFunctions.AddWordToStringBuilder(text, VTableLayoutAsString());
			NuGenHelperFunctions.AddWordToStringBuilder(text, MethodImplementationAsString());
			NuGenHelperFunctions.AddWordToStringBuilder(text, InteropAttributesAsString());
			NuGenHelperFunctions.AddWordToStringBuilder(text, ReservedFlagsAsString());
			text.Append(definition);
			NuGenHelperFunctions.AddWordToStringBuilder(text, CodeImplementationAsString());
			NuGenHelperFunctions.AddWordToStringBuilder(text, ManagedImplementationAsString());
			NuGenHelperFunctions.AddWordToStringBuilder(text, ImplementationInfoAsString());

			result.Text = text.ToString();

			return result;
		}

		private int FindCodeLine(uint offset)
		{
			int result = -1;
			bool found = false;
			int ilCodeIndex = 0;

			while (!found && ilCodeIndex < CodeLines.Count)
			{
				NuGenCodeLine codeLine = CodeLines[ilCodeIndex++];

				if (codeLine is NuGenBaseILCode)
				{
					NuGenBaseILCode ilCode = (NuGenBaseILCode)codeLine;

					if (ilCode.Offset == offset)
					{
						result = ilCodeIndex - 1;
						found = true;
					}
				}
			}

			return result;
		}

		private void AddExceptionCodeLines(uint offset, uint length, string name, string endComment, bool finallyClause)
		{
			int start = FindCodeLine(offset);

			if (start > -1)
			{
				int end = FindCodeLine(offset + length);

				if (end > -1)
				{
					int indentation = ((NuGenBaseILCode)CodeLines[start]).Indentation;

					for (int ilCodeIndex = start; ilCodeIndex < end; ilCodeIndex++)
					{
						CodeLines[ilCodeIndex].Indentation++;
					}

					NuGenCodeLine nameDefinition = new NuGenCodeLine(indentation, name);
					NuGenCodeLine bracket = new NuGenCodeLine(indentation, "{");

					CodeLines.Insert(start, bracket);
					CodeLines.Insert(start, nameDefinition);

					end += 2;
					if (finallyClause && indentation > 1)
					{
						indentation--;
					}
					bracket = new NuGenCodeLine(indentation, string.Format("}} {0}", endComment));
					CodeLines.Insert(end, bracket);
				}
			}
		}

		private void AddExceptionHandlingCodeLines(NuGenExceptionClause clause)
		{
			if (clause.Flags == CorExceptionFlag.COR_ILEXCEPTION_CLAUSE_FILTER)
			{
				string filter = string.Format(".try IL_{0} to IL_{1} filter IL_{2} handler IL_{3} to IL_{4}", NuGenHelperFunctions.FormatAsHexNumber(clause.TryOffset, 4), NuGenHelperFunctions.FormatAsHexNumber(clause.TryOffset + clause.TryLength, 4), NuGenHelperFunctions.FormatAsHexNumber(clause.FilterOffset, 4), NuGenHelperFunctions.FormatAsHexNumber(clause.HandlerOffset, 4), NuGenHelperFunctions.FormatAsHexNumber(clause.HandlerOffset + clause.HandlerLength, 4));

				int position = FindCodeLine(clause.HandlerOffset + clause.HandlerLength);

				CodeLines.Insert(position + 1, new NuGenCodeLine(0, filter));
			}
			else
			{
				bool finallyClause = (clause.Flags == CorExceptionFlag.COR_ILEXCEPTION_CLAUSE_FINALLY);

				AddExceptionCodeLines(clause.TryOffset, clause.TryLength, ".try", "// end try", finallyClause);

				if (finallyClause)
				{
					AddExceptionCodeLines(clause.HandlerOffset, clause.HandlerLength, "finally", "// end finally", false);
				}
				else
				{
					string handlerName = string.Format("catch {0}", BaseTypeDefinition.ModuleScope.Assembly.AllTokens[clause.ClassToken]);

					AddExceptionCodeLines(clause.HandlerOffset, clause.HandlerLength, handlerName, "// end handler", false);
				}
			}
		}

		private void ReadMethodDataSections(BinaryReader assemblyReader)
		{
			bool moreSections = true;
			byte moreSectionsValue = (byte)CorILMethodSect.CorILMethod_Sect_MoreSects;
			byte fatFormatValue = (byte)CorILMethodSect.CorILMethod_Sect_FatFormat;
			byte exceptionHandlingTableValue = (byte)CorILMethodSect.CorILMethod_Sect_EHTable;

			while (moreSections)
			{
				int bytesToRead = Convert.ToInt32(assemblyReader.BaseStream.Position % 4);

				if (bytesToRead > 0)
				{
					assemblyReader.ReadBytes(4 - bytesToRead);
				}

				byte kind = assemblyReader.ReadByte();

				if ((kind & exceptionHandlingTableValue) != exceptionHandlingTableValue)
				{
					throw new NotImplementedException("The method data section is not an exception handling table.");
				}

				moreSections = ((kind & moreSectionsValue) == moreSectionsValue);
				int dataSize = 0;
				int clauseNumber = 0;
				bool fatFormat = ((kind & fatFormatValue) == fatFormatValue);

				if (fatFormat)
				{
					dataSize = assemblyReader.ReadByte() + assemblyReader.ReadByte() * 0x100 + assemblyReader.ReadByte() * 0x10000;
					clauseNumber = dataSize / 24;
				}
				else
				{
					dataSize = assemblyReader.ReadByte();
					//Read padding.
					assemblyReader.ReadBytes(2);
					clauseNumber = dataSize / 12;
				}

				for (int clauseIndex = 0; clauseIndex < clauseNumber; clauseIndex++)
				{
					NuGenExceptionClause clause = new NuGenExceptionClause();

					if (fatFormat)
					{
						clause.Flags = (CorExceptionFlag)assemblyReader.ReadUInt32();
						clause.TryOffset = assemblyReader.ReadUInt32();
						clause.TryLength = assemblyReader.ReadUInt32();
						clause.HandlerOffset = assemblyReader.ReadUInt32();
						clause.HandlerLength = assemblyReader.ReadUInt32();
					}
					else
					{
						clause.Flags = (CorExceptionFlag)assemblyReader.ReadUInt16();
						clause.TryOffset = assemblyReader.ReadUInt16();
						clause.TryLength = assemblyReader.ReadByte();
						clause.HandlerOffset = assemblyReader.ReadUInt16();
						clause.HandlerLength = assemblyReader.ReadByte();
					}

					if (clause.Flags == CorExceptionFlag.COR_ILEXCEPTION_CLAUSE_NONE)
					{
						clause.ClassToken = assemblyReader.ReadUInt32();
					}
					else
					{
						clause.FilterOffset = assemblyReader.ReadUInt32();
					}

					AddExceptionHandlingCodeLines(clause);
				}
			}
		}

		private ulong FindMethodHeader()
		{
			ulong result = MethodAddress;
			byte[] possibleLocalVarSigToken = BaseTypeDefinition.ModuleScope.Assembly.DebuggedProcess.ReadMemory(MethodAddress - 4, 4);

			if (BitConverter.ToUInt32(possibleLocalVarSigToken, 0) == LocalVarSigToken)
			{
				result = MethodAddress - 12;
			}
			else
			{
				result = MethodAddress - 1;
			}

			return result;
		}

		private void ReadILCode()
		{
			NuGenCodeLine start = new NuGenCodeLine();
			start.Text = "{";
			CodeLines.Add(start);

			if (EntryPoint)
			{
				CodeLines.Add(new NuGenCodeLine(1, ".entrypoint"));
			}

			if (Overrides != 0)
			{
				NuGenTokenBase overridenMember = BaseTypeDefinition.ModuleScope.Assembly.AllTokens[Overrides];
				Type overridenMemberType = overridenMember.GetType();
				string memberName;

				if (GenericParameters != null && overridenMember is NuGenIHasSignature)
				{
					NuGenIHasSignature hasSignature = (NuGenIHasSignature)overridenMember;
					hasSignature.SignatureReader.SetGenericParametersOfMethod(GenericParameters);
				}

				if (overridenMemberType == typeof(NuGenMemberReference))
				{
					NuGenMemberReference memberReference = (NuGenMemberReference)overridenMember;
					memberName = memberReference.Text;
				}
				else if (overridenMemberType == typeof(NuGenMethodDefinition))
				{
					NuGenMethodDefinition methodDefinition = (NuGenMethodDefinition)overridenMember;
					memberName = string.Format("{0}::{1}", methodDefinition.BaseTypeDefinition.FullName, methodDefinition.Name);
				}
				else
				{
					throw new NotImplementedException(string.Format("Unhandled overriden member type ('{0}').", overridenMemberType.FullName));
				}

				CodeLines.Add(new NuGenCodeLine(1, ".override method " + memberName));
			}

			if (CustomAttributes != null)
			{
				NuGenAssembly assembly = BaseTypeDefinition.ModuleScope.Assembly;

				foreach (NuGenCustomAttribute customAttribute in CustomAttributes)
				{
					CodeLines.Add(new NuGenCodeLine(1, customAttribute.Name));
				}
			}

			int codeSizePosition = 2;

			if (Parameters != null)
			{
				NuGenAssembly assembly = BaseTypeDefinition.ModuleScope.Assembly;

				try
				{
					assembly.OpenMetadataInterfaces();

					foreach (NuGenParameter parameter in Parameters)
					{
						parameter.LazyInitialize(BaseTypeDefinition.ModuleScope.Assembly.AllTokens);
						bool hasDefault = ((parameter.AttributeFlags & CorParamAttr.pdHasDefault) == CorParamAttr.pdHasDefault);

						if (hasDefault)
						{
							NuGenCodeLine defaultParameter = new NuGenCodeLine();
							defaultParameter.Indentation = 1;
							defaultParameter.Text = parameter.DefaultValueAsString;
							CodeLines.Add(defaultParameter);
							codeSizePosition++;
						}

						if (parameter.CustomAttributes != null)
						{
							if (!hasDefault)
							{
								NuGenCodeLine parameterLine = new NuGenCodeLine(1, string.Format(".param [{0}]", parameter.OrdinalIndex));
								CodeLines.Add(parameterLine);
								codeSizePosition++;
							}

							foreach (NuGenCustomAttribute customAttribute in parameter.CustomAttributes)
							{
								NuGenCodeLine customAttributeLine = new NuGenCodeLine(1, customAttribute.Name);
								CodeLines.Add(customAttributeLine);

								codeSizePosition++;
							}
						}
					}
				}
				finally
				{
					assembly.CloseMetadataInterfaces();
				}
			}

			if (PermissionSets != null)
			{
				foreach (NuGenPermissionSet permissionSet in PermissionSets)
				{
					NuGenCodeLine permissionSetLine = new NuGenCodeLine(1, permissionSet.Name);
					CodeLines.Add(permissionSetLine);

					codeSizePosition++;
				}
			}

			if (((Rva > 0) || BaseTypeDefinition.ModuleScope.Assembly.IsInMemory) && ((ImplementationFlags & CorMethodImpl.miCodeTypeMask) != CorMethodImpl.miNative))
			{
				NuGenAssembly assembly = BaseTypeDefinition.ModuleScope.Assembly;
				BinaryReader assemblyReader = null;

				if (assembly.IsInMemory)
				{
					NuGenInMemoryMethodStream methodStream = new NuGenInMemoryMethodStream(assembly.DebuggedProcess, FindMethodHeader());
					assemblyReader = new BinaryReader(methodStream);
				}
				else
				{
					assembly.OpenAssemblyReader();
					assemblyReader = assembly.AssemblyReader;
					assemblyReader.BaseStream.Position = assembly.GetMethodAddress(Rva);
				}

				byte methodHeader = assemblyReader.ReadByte();
				int methodLength = 0;
				bool moreSects = false;

				if ((methodHeader & (byte)ILMethodHeader.CorILMethod_FatFormat) == (byte)ILMethodHeader.CorILMethod_FatFormat)
				{
					byte methodHeaderByte2 = assemblyReader.ReadByte();
					moreSects = ((methodHeader & (byte)CorILMethodFlags.CorILMethod_MoreSects) == (byte)CorILMethodFlags.CorILMethod_MoreSects);

					byte sizeOfHeader = Convert.ToByte((methodHeaderByte2 >> 4) * 4);
					ushort maxStack = assemblyReader.ReadUInt16();
					methodLength = assemblyReader.ReadInt32();
					uint localVarSigToken = assemblyReader.ReadUInt32();

					NuGenCodeLine maxStackLine = new NuGenCodeLine(1, string.Format(".maxstack {0}", maxStack));
					CodeLines.Add(maxStackLine);

					if (localVarSigToken != 0 && BaseTypeDefinition.ModuleScope.Assembly.StandAloneSignatures != null && BaseTypeDefinition.ModuleScope.Assembly.StandAloneSignatures.ContainsKey(localVarSigToken))
					{
						string initLocals = string.Empty;

						if ((methodHeader & (byte)CorILMethodFlags.CorILMethod_InitLocals) == (byte)CorILMethodFlags.CorILMethod_InitLocals)
						{
							initLocals = "init ";
						}

						NuGenStandAloneSignature standAloneSignature = BaseTypeDefinition.ModuleScope.Assembly.StandAloneSignatures[localVarSigToken];

						if (GenericParameters != null && standAloneSignature.SignatureReader.HasGenericMethodParameter)
						{
							standAloneSignature.SignatureReader.SetGenericParametersOfMethod(GenericParameters);
							standAloneSignature.LazyInitialize(BaseTypeDefinition.ModuleScope.Assembly.AllTokens);
						}

						NuGenCodeLine variablesLine = new NuGenCodeLine(1, string.Format(".locals {0}{1}", initLocals, standAloneSignature.Text));
						CodeLines.Add(variablesLine);
					}
				}
				else if ((methodHeader & (byte)ILMethodHeader.CorILMethod_TinyFormat) == (byte)ILMethodHeader.CorILMethod_TinyFormat)
				{
					methodLength = methodHeader >> 2;
				}

				NuGenCodeLine codeSize = new NuGenCodeLine(1, string.Format("// Code size {0} (0x{1:x})", methodLength, methodLength));

				if (CustomAttributes != null)
				{
					codeSizePosition++;
				}

				if (EntryPoint)
				{
					codeSizePosition++;
				}

				if (Overrides != 0)
				{
					codeSizePosition++;
				}

				CodeLines.Insert(codeSizePosition, codeSize);

				if ((ImplementationFlags & CorMethodImpl.miNative) != CorMethodImpl.miNative)
				{
					byte[] methodCode = new byte[methodLength];
					assemblyReader.Read(methodCode, 0, methodLength);
					int methodCodeIndex = 0;

					while (methodCodeIndex < methodCode.Length)
					{
						int offset = methodCodeIndex;
						short opCodeValue = methodCode[methodCodeIndex++];

						if (opCodeValue == 0xFE)
						{
							opCodeValue = (short)(opCodeValue * 0x100 + methodCode[methodCodeIndex++]);
						}

						if (NuGenOpCodeGroups.OpCodesByValue.ContainsKey(opCodeValue))
						{
							OpCode opCode = NuGenOpCodeGroups.OpCodesByValue[opCodeValue];
							OpCodeGroup opCodeGroup = NuGenOpCodeGroups.GetGroupOfOpCode(opCode);
							int parameterSize = 0;

							switch (opCodeGroup)
							{
								case OpCodeGroup.ByteArgumentParameter:
								case OpCodeGroup.ByteParameter:
								case OpCodeGroup.ByteVariableParameter:
								case OpCodeGroup.SbyteLocationParameter:
								case OpCodeGroup.SbyteParameter:
									parameterSize = 1;
									break;

								case OpCodeGroup.UshortArgumentParameter:
								case OpCodeGroup.UshortParameter:
								case OpCodeGroup.UshortVariableParameter:
									parameterSize = 2;
									break;

								case OpCodeGroup.FloatParameter:
								case OpCodeGroup.FieldParameter:
								case OpCodeGroup.IntLocationParameter:
								case OpCodeGroup.IntParameter:
								case OpCodeGroup.MethodParameter:
								case OpCodeGroup.StringParameter:
								case OpCodeGroup.TypeParameter:
								case OpCodeGroup.Calli:
								case OpCodeGroup.Ldtoken:
								case OpCodeGroup.Switch:
									parameterSize = 4;
									break;

								case OpCodeGroup.DoubleParameter:
								case OpCodeGroup.LongParameter:
									parameterSize = 8;
									break;
							}

							ulong parameter = 0;

							if (parameterSize > 0)
							{
								parameter = NuGenHelperFunctions.GetILCodeParameter(methodCode, methodCodeIndex, parameterSize);
							}

							NuGenBaseILCode code = null;

							switch (opCodeGroup)
							{
								case OpCodeGroup.Parameterless:
									code = new NuGenBaseILCode();
									break;

								case OpCodeGroup.FieldParameter:
									NuGenFieldILCode fieldILCode = new NuGenFieldILCode();
									code = fieldILCode;
									fieldILCode.Parameter = Convert.ToUInt32(parameter);
									fieldILCode.DecodedParameter = assembly.AllTokens[fieldILCode.Parameter];
									if (GenericParameters != null)
									{
										fieldILCode.SetGenericsMethodParameters(assembly.AllTokens, GenericParameters);
									}
									break;

								case OpCodeGroup.MethodParameter:
									NuGenMethodILCode methodILCode = new NuGenMethodILCode();
									code = methodILCode;
									methodILCode.Parameter = Convert.ToUInt32(parameter);
									methodILCode.DecodedParameter = assembly.AllTokens[methodILCode.Parameter];
									if (GenericParameters != null)
									{
										methodILCode.SetGenericsMethodParameters(assembly.AllTokens, GenericParameters);
									}
									break;

								case OpCodeGroup.StringParameter:
									NuGenStringILCode stringILCode = new NuGenStringILCode();
									code = stringILCode;
									stringILCode.Parameter = Convert.ToUInt32(parameter);
									stringILCode.DecodedParameter = assembly.UserStrings[stringILCode.Parameter];
									break;

								case OpCodeGroup.TypeParameter:
									NuGenTypeILCode typeILCode = new NuGenTypeILCode();
									code = typeILCode;
									typeILCode.Parameter = Convert.ToUInt32(parameter);
									typeILCode.DecodedParameter = assembly.AllTokens[typeILCode.Parameter];
									if (GenericParameters != null)
									{
										typeILCode.SetGenericsMethodParameters(assembly.AllTokens, GenericParameters);
									}
									break;

								case OpCodeGroup.IntLocationParameter:
									LocationILCode<int> intLocationILCode = new LocationILCode<int>();
									code = intLocationILCode;
									intLocationILCode.Parameter = (int)parameter;
									intLocationILCode.DecodedParameter = intLocationILCode.Parameter;
									break;

								case OpCodeGroup.SbyteLocationParameter:
									LocationILCode<sbyte> sbyteLocationILCode = new LocationILCode<sbyte>();
									code = sbyteLocationILCode;
									sbyteLocationILCode.Parameter = (sbyte)parameter;
									sbyteLocationILCode.DecodedParameter = sbyteLocationILCode.Parameter;
									break;

								case OpCodeGroup.ByteParameter:
									NumberILCode<byte> byteNumberILCode = new NumberILCode<byte>();
									code = byteNumberILCode;
									byteNumberILCode.Parameter = (byte)parameter;
									byteNumberILCode.DecodedParameter = byteNumberILCode.Parameter;
									break;

								case OpCodeGroup.UshortParameter:
									NumberILCode<ushort> ushortNumberILCode = new NumberILCode<ushort>();
									code = ushortNumberILCode;
									ushortNumberILCode.Parameter = (ushort)parameter;
									ushortNumberILCode.DecodedParameter = ushortNumberILCode.Parameter;
									break;

								case OpCodeGroup.SbyteParameter:
									NumberILCode<sbyte> sbyteNumberILCode = new NumberILCode<sbyte>();
									code = sbyteNumberILCode;
									sbyteNumberILCode.Parameter = (sbyte)parameter;
									sbyteNumberILCode.DecodedParameter = sbyteNumberILCode.Parameter;
									break;

								case OpCodeGroup.IntParameter:
									NumberILCode<int> intNumberILCode = new NumberILCode<int>();
									code = intNumberILCode;
									intNumberILCode.Parameter = (int)parameter;
									intNumberILCode.DecodedParameter = intNumberILCode.Parameter;
									break;

								case OpCodeGroup.LongParameter:
									NumberILCode<long> longNumberILCode = new NumberILCode<long>();
									code = longNumberILCode;
									longNumberILCode.Parameter = (long)parameter;
									longNumberILCode.DecodedParameter = longNumberILCode.Parameter;
									break;

								case OpCodeGroup.FloatParameter:
									NumberILCode<float> floatNumberILCode = new NumberILCode<float>();
									code = floatNumberILCode;
									floatNumberILCode.Parameter = NuGenHelperFunctions.ConvertToSingle(parameter);
									floatNumberILCode.DecodedParameter = floatNumberILCode.Parameter;
									break;

								case OpCodeGroup.DoubleParameter:
									NumberILCode<double> doubleNumberILCode = new NumberILCode<double>();
									code = doubleNumberILCode;
									doubleNumberILCode.Parameter = NuGenHelperFunctions.ConvertToDouble(parameter);
									doubleNumberILCode.DecodedParameter = doubleNumberILCode.Parameter;
									break;

								case OpCodeGroup.ByteArgumentParameter:
									ArgumentILCode<byte> byteArgumentILCode = new ArgumentILCode<byte>();
									code = byteArgumentILCode;
									byteArgumentILCode.Parameter = (byte)parameter;
									byteArgumentILCode.DecodedParameter = NameOfParameter(parameter);
									break;

								case OpCodeGroup.UshortArgumentParameter:
									ArgumentILCode<ushort> ushortArgumentILCode = new ArgumentILCode<ushort>();
									code = ushortArgumentILCode;
									ushortArgumentILCode.Parameter = (ushort)parameter;
									ushortArgumentILCode.DecodedParameter = NameOfParameter(parameter);
									break;

								case OpCodeGroup.ByteVariableParameter:
									VariableILCode<byte> byteVariableILCode = new VariableILCode<byte>();
									code = byteVariableILCode;
									byteVariableILCode.Parameter = (byte)parameter;
									byteVariableILCode.DecodedParameter = byteVariableILCode.Parameter;
									break;

								case OpCodeGroup.UshortVariableParameter:
									VariableILCode<ushort> ushortVariableILCode = new VariableILCode<ushort>();
									code = ushortVariableILCode;
									ushortVariableILCode.Parameter = (ushort)parameter;
									ushortVariableILCode.DecodedParameter = ushortVariableILCode.Parameter;
									break;

								case OpCodeGroup.Calli:
									NuGenCalliILCode calliILCode = new NuGenCalliILCode();
									code = calliILCode;
									calliILCode.Parameter = Convert.ToUInt32(parameter);
									calliILCode.DecodedParameter = assembly.StandAloneSignatures[calliILCode.Parameter];
									break;

								case OpCodeGroup.Ldtoken:
									NuGenLdtokenILCode ldtokenILCode = new NuGenLdtokenILCode();
									code = ldtokenILCode;
									ldtokenILCode.Parameter = Convert.ToUInt32(parameter);
									ldtokenILCode.DecodedParameter = assembly.AllTokens[ldtokenILCode.Parameter];

									if (GenericParameters != null)
									{
										ldtokenILCode.SetGenericsMethodParameters(assembly.AllTokens, GenericParameters);
									}
									break;

								case OpCodeGroup.Switch:
									NuGenSwitchILCode switchILCode = new NuGenSwitchILCode();
									code = switchILCode;
									ulong addressIndex = 0;
									switchILCode.Parameter = new int[parameter];
									parameterSize += Convert.ToInt32(parameter * 4);

									while (addressIndex < parameter)
									{
										int jumpAddress = (int)NuGenHelperFunctions.GetILCodeParameter(methodCode, methodCodeIndex + Convert.ToInt32((addressIndex + 1) * 4), 4);
										switchILCode.Parameter[addressIndex++] = jumpAddress;
									}
									break;
							}

							if (code != null)
							{
								code.Offset = offset;
								code.OpCode = opCode;
								code.DecodeParameter();
							}

							code.Indentation = 1;

							CodeLines.Add(code);
							methodCodeIndex += parameterSize;
						}
					}

					NuGenCodeLine end = new NuGenCodeLine();
					StringBuilder endText = new StringBuilder();
					endText.Append("} //end of method ");

					if (BaseTypeDefinition.FullName != null && BaseTypeDefinition.FullName.Length > 0)
					{
						endText.Append(BaseTypeDefinition.FullName);
						endText.Append("::");
					}

					endText.Append(Name);
					end.Text = endText.ToString();

					CodeLines.Add(end);

					if (moreSects)
					{
						ReadMethodDataSections(assemblyReader);
					}
				}

				if (assembly.IsInMemory)
				{
					assemblyReader.Close();
				}
				else
				{
					assembly.CloseAssemblyReader();
				}
			}
			else
			{
				NuGenCodeLine end = new NuGenCodeLine();
				end.Text = string.Format("}} // end of method {0}{1}{2}", BaseTypeDefinition.Name, (BaseTypeDefinition.Name.Length > 0 ? "::" : string.Empty), Name);
				CodeLines.Add(end);
			}
		}

		public void Initialize()
		{
			CodeLines = new List<NuGenCodeLine>();

			ReadSignature();
			ReadMetadata();
			CodeLines.Insert(0, CreateMethodHead(GetDefinitionLine(BaseTypeDefinition.ModuleScope.Assembly.AllTokens)));
			ReadILCode();
		}

		private string NameOfParameter(ulong ordinalIndex)
		{
			string result = string.Empty;

			if (((CallingConvention & CorCallingConvention.IMAGE_CEE_CS_CALLCONV_HASTHIS) != CorCallingConvention.IMAGE_CEE_CS_CALLCONV_HASTHIS))
			{
				result = FindParameterByOrdinalIndex(Convert.ToInt32(++ordinalIndex)).Name;
			}
			else if (ordinalIndex == 0)
			{
				result = "0";
			}
			else
			{
				result = FindParameterByOrdinalIndex(Convert.ToInt32(ordinalIndex)).Name;
			}

			return result;
		}

		private NuGenParameter FindParameterByOrdinalIndex(int ordinalIndex)
		{
			NuGenParameter result = null;

			if (Parameters != null)
			{
				int index = 0;

				while (index < Parameters.Count && result == null)
				{
					NuGenParameter parameter = Parameters[index++];

					if (parameter.OrdinalIndex == ordinalIndex)
					{
						result = parameter;
					}
				}
			}

			return result;
		}

		private string GetDefinitionLine(Dictionary<uint, NuGenTokenBase> allTokens)
		{
			string callingConventionName = NuGenHelperFunctions.GetCallingConventionName(CallingConvention);
			ReadSignature();
			string returnTypeText = signatureReader.ReturnType.ToString();
			NuGenParameter returnParameter = FindParameterByOrdinalIndex(0);
			DefinitionBuilder.Length = 0;
			
			if (GenericParameters != null)
			{
				DefinitionBuilder.Append("<");

				for (int index = 0; index < GenericParameters.Count; index++)
				{
					NuGenGenericParameter genericParameter = GenericParameters[index];
					DefinitionBuilder.Append(genericParameter.Text);

					if (index < GenericParameters.Count - 1)
					{
						DefinitionBuilder.Append(", ");
					}
				}

				DefinitionBuilder.Append(">");
			}

			DefinitionBuilder.Append("(");

			if (signatureReader.Parameters != null)
			{
				for (int parametersIndex = 0; parametersIndex < signatureReader.Parameters.Count; parametersIndex++)
				{
					NuGenBaseSignatureItem parameterItem = signatureReader.Parameters[parametersIndex];
					NuGenParameter parameter = FindParameterByOrdinalIndex(parametersIndex + 1);

					if (parametersIndex != 0)
					{
						DefinitionBuilder.Append("    ");
					}

					string parameterItemAsString = parameterItem.ToString();
					string attributeText = (parameter == null ? string.Empty : parameter.GetAttributeText());

					if (parametersIndex == signatureReader.Parameters.Count - 1)
					{
						DefinitionBuilder.Append(attributeText);
						DefinitionBuilder.Append(parameterItemAsString);

						if ((parameter != null) && (parameter.AttributeFlags & CorParamAttr.pdHasFieldMarshal) == CorParamAttr.pdHasFieldMarshal)
						{
							DefinitionBuilder.Append(" ");
							DefinitionBuilder.Append(parameter.MarshalAsTypeString);
						}

						if (parameter == null || parameter.Name == null || parameter.Name.Length == 0)
						{
							DefinitionBuilder.Append(" A_");
							DefinitionBuilder.Append(parametersIndex);
						}
						else
						{
							DefinitionBuilder.Append(" ");
							DefinitionBuilder.Append(parameter.Name);
						}
					}
					else
					{
						DefinitionBuilder.Append(attributeText);
						DefinitionBuilder.Append(parameterItemAsString);

						if ((parameter != null) && (parameter.AttributeFlags & CorParamAttr.pdHasFieldMarshal) == CorParamAttr.pdHasFieldMarshal)
						{
							DefinitionBuilder.Append(" ");
							DefinitionBuilder.Append(parameter.MarshalAsTypeString);
						}

						if (parameter == null || parameter.Name == null || parameter.Name.Length == 0)
						{
							DefinitionBuilder.Append(" A_");
							DefinitionBuilder.Append(parametersIndex);
							DefinitionBuilder.Append("\n");
						}
						else
						{
							DefinitionBuilder.Append(" ");
							DefinitionBuilder.Append(parameter.Name);
							DefinitionBuilder.Append(",\n");
						}
					}
				}
			}

			DefinitionBuilder.Append(") ");
			DefinitionBuilder.Insert(0, Name);

			if ((returnParameter != null) && ((returnParameter.AttributeFlags & CorParamAttr.pdHasFieldMarshal) == CorParamAttr.pdHasFieldMarshal))
			{
				DefinitionBuilder.Insert(0, " ");
				DefinitionBuilder.Insert(0, returnParameter.MarshalAsTypeString);
			}

			DefinitionBuilder.Insert(0, " ");
			DefinitionBuilder.Insert(0, returnTypeText);
			
			if (callingConventionName.Length > 0)
			{	
				DefinitionBuilder.Insert(0, " ");
				DefinitionBuilder.Insert(0, callingConventionName);
			}

			return DefinitionBuilder.ToString();
		}

		private void ReadSignature()
		{
			if (signatureReader == null)
			{
				signatureReader = new NuGenMethodSignatureReader(BaseTypeDefinition.ModuleScope.Assembly.AllTokens, SignaturePointer, SignatureLength);
				signatureReader.ReadSignature();
				CallingConvention = signatureReader.CallingConvention;
				NuGenHelperFunctions.SetSignatureItemToken(BaseTypeDefinition.ModuleScope.Assembly.AllTokens, signatureReader.ReturnType, GenericParameters);

				if (signatureReader.Parameters != null)
				{
					for (int parametersIndex = 0; parametersIndex < signatureReader.Parameters.Count; parametersIndex++)
					{
						NuGenBaseSignatureItem parameterItem = signatureReader.Parameters[parametersIndex];

						NuGenHelperFunctions.SetSignatureItemToken(BaseTypeDefinition.ModuleScope.Assembly.AllTokens, parameterItem, GenericParameters);
					}
				}
			}
		}

		protected override void ReadMetadataInformation()
		{
			base.ReadMetadataInformation();
			NuGenAssembly assembly = BaseTypeDefinition.ModuleScope.Assembly;

			try
			{
				assembly.OpenMetadataInterfaces();

				CustomAttributes = NuGenHelperFunctions.EnumCustomAttributes(assembly.Import, assembly, this);

				if (CustomAttributes != null)
				{
					foreach (NuGenCustomAttribute customAttribute in CustomAttributes)
					{
						customAttribute.SetText(assembly.AllTokens);
					}
				}

				if ((Flags & CorMethodAttr.mdPinvokeImpl) == CorMethodAttr.mdPinvokeImpl)
				{
					PinvokeMap = NuGenHelperFunctions.ReadPinvokeMap(assembly.Import, assembly, Token);
				}

				if (((Flags & CorMethodAttr.mdReservedMask) & CorMethodAttr.mdHasSecurity) == CorMethodAttr.mdHasSecurity)
				{
					PermissionSets = NuGenHelperFunctions.EnumPermissionSets(assembly.Import, Token);
				}
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