using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.UI;

namespace Dile.Disassemble
{
	public class NuGenModuleScope : NuGenTokenBase, NuGenIMultiLine
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
				return Name;
			}
		}

		public bool IsInMemory
		{
			get
			{
				return Assembly.IsInMemory;
			}
		}
		#endregion

		public override SearchOptions ItemType
		{
			get
			{
				return SearchOptions.ModuleScope;
			}
		}

		private NuGenAssembly assembly;
		public NuGenAssembly Assembly
		{
			get
			{
				return assembly;
			}
			private set
			{
				assembly = value;
			}
		}

		private Guid mvid;
		public Guid Mvid
		{
			get
			{
				return mvid;
			}
			private set
			{
				mvid = value;
			}
		}

		private Dictionary<uint, NuGenTypeDefinition> typeDefinitions = new Dictionary<uint, NuGenTypeDefinition>();
		public Dictionary<uint, NuGenTypeDefinition> TypeDefinitions
		{
			get
			{
				return typeDefinitions;
			}
			private set
			{
				typeDefinitions = value;
			}
		}

		private ModuleWrapper debuggedModule;
		public ModuleWrapper DebuggedModule
		{
			get
			{
				return debuggedModule;
			}
			private set
			{
				debuggedModule = value;
			}
		}

		public NuGenModuleScope(NuGenIMetaDataImport2 import, NuGenAssembly assembly)
		{
			Assembly = assembly;
			uint moduleNameLength;
			import.GetScopeProps(NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out moduleNameLength, ref mvid);

			if (moduleNameLength > NuGenProject.DefaultCharArray.Length)
			{
				NuGenProject.DefaultCharArray = new char[moduleNameLength];

				import.GetScopeProps(NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out moduleNameLength, ref mvid);
			}

			Name = NuGenHelperFunctions.GetString(NuGenProject.DefaultCharArray, 0, moduleNameLength);

			uint token;
			import.GetModuleFromScope(out token);
			Token = token;
			Assembly.AllTokens[Token] = this;
		}

		public void EnumerateTypeDefinitions(NuGenIMetaDataImport2 import, ModuleWrapper debuggedModule)
		{
			DebuggedModule = debuggedModule;
			IntPtr enumHandle = IntPtr.Zero;
			uint[] typeDefs = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			import.EnumTypeDefs(ref enumHandle, typeDefs, Convert.ToUInt32(typeDefs.Length), out count);

			while (count > 0)
			{
				for (uint typeDefsIndex = 0; typeDefsIndex < count; typeDefsIndex++)
				{
					uint token = typeDefs[typeDefsIndex];
					uint typeNameLength;
					uint typeDefFlags;
					uint baseTypeToken;

					import.GetTypeDefProps(token, NuGenProject.DefaultCharArray,
Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out typeNameLength, out typeDefFlags, out baseTypeToken);

					if (typeNameLength > NuGenProject.DefaultCharArray.Length)
					{
						NuGenProject.DefaultCharArray = new char[typeNameLength];

						import.GetTypeDefProps(token, NuGenProject.DefaultCharArray,
Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out typeNameLength, out typeDefFlags, out baseTypeToken);
					}

					NuGenTypeDefinition typeDefinition = new NuGenTypeDefinition(import, this, NuGenHelperFunctions.GetString(NuGenProject.DefaultCharArray, 0, typeNameLength), token, (CorTypeAttr)typeDefFlags, baseTypeToken);
					TypeDefinitions[token] = typeDefinition;
					Assembly.AllTokens[token] = typeDefinition;
				}

				import.EnumTypeDefs(ref enumHandle, typeDefs, Convert.ToUInt32(typeDefs.Length), out count);
			}

			import.CloseEnum(enumHandle);

			foreach (NuGenTypeDefinition typeDefinition in TypeDefinitions.Values)
			{
				if (typeDefinition.IsNestedType)
				{
					typeDefinition.FindEnclosingType(import);
				}
			}

			DebuggedModule = null;
		}

		public void Initialize()
		{
			CodeLines = new List<NuGenCodeLine>();

			CodeLines.Add(new NuGenCodeLine(0, ".module " + Name));
			CodeLines.Add(new NuGenCodeLine(0, "// MVID: {" + Mvid.ToString() + "}"));
			CodeLines.Add(new NuGenCodeLine(0, ".imagebase 0x" + NuGenHelperFunctions.FormatAsHexNumber(Assembly.ImageBase, 8)));
			CodeLines.Add(new NuGenCodeLine(0, ".file alignment 0x" + NuGenHelperFunctions.FormatAsHexNumber(Assembly.FileAlignment, 8)));

			if (Assembly.IsPe32)
			{
				CodeLines.Add(new NuGenCodeLine(0, ".stackreserve 0x" + NuGenHelperFunctions.FormatAsHexNumber(Assembly.StackReserve, 8)));
			}
			else
			{
				CodeLines.Add(new NuGenCodeLine(0, ".stackreserve 0x" + NuGenHelperFunctions.FormatAsHexNumber(Assembly.StackReserve, 16)));
			}

			CodeLines.Add(new NuGenCodeLine(0, string.Format(".subsystem 0x{0} //{1}", NuGenHelperFunctions.FormatAsHexNumber(Assembly.Subsystem, 4), (Assembly.Subsystem == 2 ? "WINDOWS_CE" : "WINDOWS_GUI"))));
			CodeLines.Add(new NuGenCodeLine(0, ".corflags 0x" + NuGenHelperFunctions.FormatAsHexNumber(Assembly.CorFlags, 8)));

			string peFormat = "//PE Format: ";
			if (Assembly.IsPe32)
			{
				peFormat += "PE32 (32 bit assembly)";
			}
			else
			{
				peFormat += "PE32+ (64 bit assembly)";
			}
			CodeLines.Add(new NuGenCodeLine(0, peFormat));
		}
	}
}