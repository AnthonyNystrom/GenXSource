using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public abstract class NuGenBaseTokenSignatureItem : NuGenBaseSignatureItem
	{
		private NuGenBaseSignatureItem nextItem = null;
		public NuGenBaseSignatureItem NextItem
		{
			get
			{
				return nextItem;
			}
			set
			{
				nextItem = value;
			}
		}

		private NuGenTokenBase tokenObject;
		public NuGenTokenBase TokenObject
		{
			get
			{
				return tokenObject;
			}
			set
			{
				tokenObject = value;
			}
		}

		private uint token = 0;
		public uint Token
		{
			get
			{
				return token;
			}
			set
			{
				token = value;
			}
		}

		public NuGenBaseTokenSignatureItem()
		{
		}

		public string GetTokenObjectName(bool fullName)
		{
			string tokenObjectName;
			tokenObjectName = string.Empty;

			if (TokenObject != null)
			{
				if (TokenObject is NuGenTypeDefinition)
				{
					tokenObjectName = ((NuGenTypeDefinition)TokenObject).ShortName;
				}
				else if (TokenObject is NuGenTypeReference)
				{
					if (fullName)
					{
						tokenObjectName = ((NuGenTypeReference)TokenObject).FullName;
					}
					else
					{
						tokenObjectName = ((NuGenTypeReference)TokenObject).Name;
					}
				}
				else
				{
					tokenObjectName = TokenObject.Name;
				}
			}

			return tokenObjectName;
		}
	}
}