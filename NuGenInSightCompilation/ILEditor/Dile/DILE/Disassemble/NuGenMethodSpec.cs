using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata.Signature;

namespace Dile.Disassemble
{
	public class NuGenMethodSpec : NuGenTextTokenBase, NuGenIHasSignature
	{
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

		private NuGenTokenBase parent;
		public NuGenTokenBase Parent
		{
			get
			{
				return parent;
			}
			set
			{
				parent = value;
			}
		}

		private NuGenMethodSpecSignatureReader signatureReader;
		public NuGenBaseSignatureReader SignatureReader
		{
			get
			{
				ReadSignature();

				return signatureReader;
			}
		}

		private NuGenAssembly assembly;
		private NuGenAssembly Assembly
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

		public NuGenMethodSpec(uint token, NuGenTokenBase parent, IntPtr signature, uint signatureLength, NuGenAssembly	assembly)
		{
			Token = token;
			Parent = parent;
			Signature = signature;
			SignatureLength = signatureLength;
		}

		private void ReadSignature()
		{
			if (signatureReader == null)
			{
				signatureReader = new NuGenMethodSpecSignatureReader(Assembly.AllTokens, Signature, SignatureLength);
				signatureReader.ReadSignature();
			}
		}

		protected override void CreateText(Dictionary<uint, NuGenTokenBase> allTokens)
		{
			base.CreateText(allTokens);
			StringBuilder nameBuilder = null;

			if (Name == null || Name.Length == 0)
			{
				nameBuilder = new StringBuilder();
				nameBuilder.Append("<");
				ReadSignature();

				if (signatureReader.Arguments != null)
				{
					for (int index = 0; index < signatureReader.Arguments.Count; index++)
					{
						NuGenBaseSignatureItem argument = signatureReader.Arguments[index];
						NuGenHelperFunctions.SetSignatureItemToken(allTokens, argument);
						nameBuilder.Append(argument);

						if (index < signatureReader.Arguments.Count - 1)
						{
							nameBuilder.Append(", ");
						}
					}
				}

				nameBuilder.Append(">(");

				string parentAsString = Parent.ToString();

				Name = parentAsString.Replace("(", nameBuilder.ToString());
			}
		}
	}
}