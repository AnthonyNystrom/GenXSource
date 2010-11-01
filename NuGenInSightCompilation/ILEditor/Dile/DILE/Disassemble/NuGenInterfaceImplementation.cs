using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;

namespace Dile.Disassemble
{
	public class NuGenInterfaceImplementation : NuGenTokenBase
	{
		private NuGenTypeDefinition typeDefinition;
		public NuGenTypeDefinition TypeDefinition
		{
			get
			{
				return typeDefinition;
			}
			private set
			{
				typeDefinition = value;
			}
		}

		private uint interfaceToken;
		public uint InterfaceToken
		{
			get
			{
				return interfaceToken;
			}
			private set
			{
				interfaceToken = value;
			}
		}

		public NuGenInterfaceImplementation(NuGenIMetaDataImport2 import, uint token, NuGenTypeDefinition typeDefinition, uint interfaceToken)
		{
			Token = token;
			TypeDefinition = typeDefinition;
			InterfaceToken = interfaceToken;
		}
	}
}