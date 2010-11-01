using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;
using Dile.Metadata.Signature;
using System.Runtime.InteropServices;

namespace Dile.Disassemble
{
	public class NuGenTypeSpecification : NuGenTextTokenBase, NuGenIHasSignature
	{
		private IntPtr signatureBlob;
		public IntPtr SignatureBlob
		{
			get
			{
				return signatureBlob;
			}
			private set
			{
				signatureBlob = value;
			}
		}

		private uint signatureBlobLength;
		public uint SignatureBlobLength
		{
			get
			{
				return signatureBlobLength;
			}
			private set
			{
				signatureBlobLength = value;
			}
		}

		private NuGenTypeSpecSignatureReader signatureReader;
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

		public NuGenTypeSpecification(NuGenAssembly assembly, uint token, IntPtr signatureBlob, uint signatureBlobLength)
		{
			Token = token;
			SignatureBlob = signatureBlob;
			SignatureBlobLength = signatureBlobLength;
			Assembly = assembly;

			NuGenHelperFunctions.GetMemberReferences(Assembly, Token);
		}

		private void ReadSignature()
		{
			if (signatureReader == null)
			{
				signatureReader = new NuGenTypeSpecSignatureReader(Assembly.AllTokens, SignatureBlob, SignatureBlobLength);
				signatureReader.ReadSignature();
			}
		}

		protected override void CreateText(Dictionary<uint, NuGenTokenBase> allTokens)
		{
			base.CreateText(allTokens);
			ReadSignature();
			NuGenHelperFunctions.SetSignatureItemToken(allTokens, signatureReader.Type);
			Name = signatureReader.Type.ToString();

			if (signatureReader.GenericParameters != null)
			{
				StringBuilder nameBuilder = new StringBuilder(Name);
				nameBuilder.Append("<");

				for (int index = 0; index < signatureReader.GenericParameters.Count; index++)
				{
					NuGenBaseSignatureItem genericParameter = signatureReader.GenericParameters[index];

					NuGenHelperFunctions.SetSignatureItemToken(allTokens, genericParameter);
					nameBuilder.Append(genericParameter);

					if (index < signatureReader.GenericParameters.Count - 1)
					{
						nameBuilder.Append(", ");
					}
				}

				nameBuilder.Append(">");
				Name = nameBuilder.ToString();
			}
		}

		public override string ToString()
		{
			LazyInitialize(Assembly.AllTokens);

			return Name;
		}
	}
}