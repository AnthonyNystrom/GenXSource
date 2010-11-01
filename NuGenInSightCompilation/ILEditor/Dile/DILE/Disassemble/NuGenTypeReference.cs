using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;

namespace Dile.Disassemble
{
	public class NuGenTypeReference : NuGenTokenBase
	{
		private NuGenAssembly assembly;
		public NuGenAssembly Assembly
		{
			get
			{
				return assembly;
			}
			set
			{
				assembly = value;
			}
		}

		private uint resolutionScope;
		public uint ResolutionScope
		{
			get
			{
				return resolutionScope;
			}
			set
			{
				resolutionScope = value;
			}
		}

		private string referencedAssembly = string.Empty;
		public string ReferencedAssembly
		{
			get
			{
				return referencedAssembly;
			}
			set
			{
				referencedAssembly = value;
			}
		}

		private string fullName = string.Empty;
		public string FullName
		{
			get
			{
				return fullName;
			}
			set
			{
				fullName = value;
			}
		}

		public NuGenTypeReference(NuGenIMetaDataImport2 import, NuGenAssembly assembly, string name, uint token, uint resolutionScope)
		{
			Assembly = assembly;
			Name = name;
			Token = token;
			ResolutionScope = resolutionScope;
			NuGenHelperFunctions.GetMemberReferences(Assembly, Token);
		}

		public override string ToString()
		{
			return FullName;
		}
	}
}