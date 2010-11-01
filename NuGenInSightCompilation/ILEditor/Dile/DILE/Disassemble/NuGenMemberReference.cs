using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;
using Dile.Metadata.Signature;

namespace Dile.Disassemble
{
	public class NuGenMemberReference : NuGenTextTokenBase, NuGenIHasSignature
	{
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

		private uint typeDefToken;
		public uint TypeDefToken
		{
			get
			{
				return typeDefToken;
			}
			private set
			{
				typeDefToken = value;
			}
		}

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

		private string text = null;
		public string Text
		{
			get
			{
				if (text == null)
				{
					text = string.Empty;

					if (Assembly.AllTokens.ContainsKey(TypeDefToken))
					{
						NuGenTokenBase tokenObject = Assembly.AllTokens[TypeDefToken];
						Type tokenObjectType = tokenObject.GetType();

						if (tokenObjectType.IsSubclassOf(typeof(NuGenTextTokenBase)))
						{
							NuGenTextTokenBase textToken = (NuGenTextTokenBase)tokenObject;

							textToken.LazyInitialize(Assembly.AllTokens);
						}

						if (tokenObjectType == typeof(NuGenTypeDefinition))
						{
							ClassName = ((NuGenTypeDefinition)tokenObject).FullName;
						}
						else if (tokenObjectType == typeof(NuGenTypeReference))
						{
							NuGenTypeReference typeRef = (NuGenTypeReference)tokenObject;

							ClassName = typeRef.FullName;
						}
						else if (tokenObjectType == typeof(NuGenModuleReference) || tokenObjectType == typeof(NuGenTypeSpecification))
						{
							ClassName = tokenObject.Name;
						}
						else if (tokenObjectType == typeof(NuGenMethodDefinition))
						{
							NuGenMethodDefinition methodDef = (NuGenMethodDefinition)tokenObject;

							ClassName = methodDef.BaseTypeDefinition.FullName;
						}

						text = signatureReader.GetDefinition(ClassName, Name, false);
					}
				}

				return text;
			}
			private set
			{
				text = value;
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

		private string className;
		public string ClassName
		{
			get
			{
				return className;
			}
			private set
			{
				className = value;
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

		private NuGenMethodRefSignatureReader signatureReader;
		public NuGenBaseSignatureReader SignatureReader
		{
			get
			{
				return signatureReader;
			}
		}

		public NuGenMemberReference(NuGenAssembly assembly, string name, uint token, uint typeDefToken, IntPtr signatureBlob, uint signatureBlobLength)
		{
			Assembly = assembly;
			Name = name;
			Token = token;
			TypeDefToken = typeDefToken;
			SignatureBlob = signatureBlob;
			SignatureBlobLength = signatureBlobLength;

			signatureReader = new NuGenMethodRefSignatureReader(assembly.AllTokens, SignatureBlob, SignatureBlobLength);
			signatureReader.ReadSignature();
			MethodSpecs = NuGenHelperFunctions.EnumMethodSpecs(assembly.Import, assembly, this);
		}

		public override string ToString()
		{
			LazyInitialize(Assembly.AllTokens);

			return Text;
		}
	}
}