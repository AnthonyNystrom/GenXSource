using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.UI;

namespace Dile.Disassemble
{
	public class NuGenFile : NuGenTokenBase, NuGenIMultiLine
	{
		public bool IsInMemory
		{
			get
			{
				return Assembly.IsInMemory;
			}
		}

		public override SearchOptions ItemType
		{
			get
			{
				return SearchOptions.File;
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
				name = value;
			}
		}

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

		#endregion

		public bool DisplayInTree
		{
			get
			{
				return ((Flags & CorFileFlags.ffContainsNoMetaData) == CorFileFlags.ffContainsNoMetaData);
			}
		}

		private IntPtr hash;
		public IntPtr Hash
		{
			get
			{
				return hash;
			}
			private set
			{
				hash = value;
			}
		}

		private uint hashLength;
		public uint HashLength
		{
			get
			{
				return hashLength;
			}
			private set
			{
				hashLength = value;
			}
		}

		private CorFileFlags flags;
		public CorFileFlags Flags
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

		public NuGenFile(NuGenAssembly assembly, uint token, string name, IntPtr hash, uint hashLength, uint flags)
		{
			Assembly = assembly;
			Token = token;
			Name = name;
			Hash = hash;
			HashLength = hashLength;
			Flags = (CorFileFlags)flags;
		}

		public void Initialize()
		{
			CodeLines = new List<NuGenCodeLine>();

			NuGenCodeLine definition = new NuGenCodeLine(0, "file " + Name);
			CodeLines.Add(definition);

			CodeLines.Add(new NuGenCodeLine(0, string.Format(".hash = {0}", NuGenHelperFunctions.ReadBlobAsString(Hash, HashLength))));
		}
	}
}