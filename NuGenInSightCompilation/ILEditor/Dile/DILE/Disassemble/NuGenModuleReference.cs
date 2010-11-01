using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;
using Dile.Disassemble.ILCodes;
using Dile.UI;

namespace Dile.Disassemble
{
	public class NuGenModuleReference : NuGenTokenBase, NuGenIMultiLine
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
				return SearchOptions.ModuleReference;
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

		public NuGenModuleReference(NuGenAssembly assembly, uint token, string name)
		{
			Token = token;
			Assembly = assembly;
			Name = name;
			NuGenHelperFunctions.GetMemberReferences(Assembly, Token);
		}

		public void Initialize()
		{
			CodeLines = new List<NuGenCodeLine>();
			CodeLines.Add(new NuGenCodeLine(0, ".module extern " + Name));
		}
	}
}