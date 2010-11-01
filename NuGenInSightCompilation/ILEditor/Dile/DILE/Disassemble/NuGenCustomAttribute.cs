using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;
using System.Runtime.InteropServices;

namespace Dile.Disassemble
{
	public class NuGenCustomAttribute : NuGenTokenBase
	{
		private NuGenTokenBase owner;
		public NuGenTokenBase Owner
		{
			get
			{
				return owner;
			}
			private set
			{
				owner = value;
			}
		}

		private uint type;
		public uint Type
		{
			get
			{
				return type;
			}
			private set
			{
				type = value;
			}
		}

		private IntPtr blob;
		public IntPtr Blob
		{
			get
			{
				return blob;
			}
			private set
			{
				blob = value;
			}
		}

		private uint blobLength;
		public uint BlobLength
		{
			get
			{
				return blobLength;
			}
			private set
			{
				blobLength = value;
			}
		}

		private static StringBuilder nameBuilder = new StringBuilder();
		private static StringBuilder NameBuilder
		{
			get
			{
				return nameBuilder;
			}
		}

		public NuGenCustomAttribute(NuGenIMetaDataImport2 import, uint token, NuGenTokenBase owner, uint type, IntPtr blob, uint blobLength)
		{
			Token = token;
			Owner = owner;
			Type = type;
			Blob = blob;
			BlobLength = blobLength;
		}

		public void SetText(Dictionary<uint, NuGenTokenBase> allTokens)
		{
			NameBuilder.Length = 0;
			NameBuilder.Append(".custom ");
			NameBuilder.Append(allTokens[type]);

			NameBuilder.Append(" = (");
			uint lastByte = BlobLength - 1;
			for (int blobIndex = 0; blobIndex < BlobLength; blobIndex++)
			{
				NameBuilder.Append(NuGenHelperFunctions.FormatAsHexNumber(Marshal.ReadByte(Blob), 2));

				if (blobIndex < lastByte)
				{
					NameBuilder.Append(" ");
					NuGenHelperFunctions.StepIntPtr(ref blob);
				}
			}
			NameBuilder.Append(")");

			Name = NameBuilder.ToString();
		}
	}
}