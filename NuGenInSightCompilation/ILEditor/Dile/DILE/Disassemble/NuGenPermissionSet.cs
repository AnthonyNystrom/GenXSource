using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;
using System.Runtime.InteropServices;

namespace Dile.Disassemble
{
	public class NuGenPermissionSet : NuGenTokenBase
	{
		public override string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		private CorDeclSecurity securityFlag;
		public CorDeclSecurity SecurityFlag
		{
			get
			{
				return securityFlag;
			}
			private set
			{
				securityFlag = value;
			}
		}

		public NuGenPermissionSet(uint token, uint securityFlag, IntPtr signatureBlob, uint signatureBlobLength)
		{
			Token = Token;
			SecurityFlag = (CorDeclSecurity)securityFlag;

			byte[] description = new byte[signatureBlobLength];
			for (int offset = 0; offset < signatureBlobLength; offset++)
			{
				description[offset] = Marshal.ReadByte(signatureBlob, offset);
			}

			UnicodeEncoding encoding = new UnicodeEncoding();
			Name = string.Format(".permissionset {0} \"{1}\"", SecurityFlagAsString(), NuGenHelperFunctions.ShowEscapeCharacters(encoding.GetString(description, 0, description.Length)));
		}

		private string SecurityFlagAsString()
		{
			string result = string.Empty;
			SecurityFlag = SecurityFlag & CorDeclSecurity.dclActionMask;

			switch(SecurityFlag)
			{
				case CorDeclSecurity.dclAssert:
					result = "assert";
					break;

				case CorDeclSecurity.dclDemand:
					result = "demand";
					break;

				case CorDeclSecurity.dclDeny:
					result = "deny";
					break;

				case CorDeclSecurity.dclNonCasDemand:
					result = "noncasdemand";
					break;

				case CorDeclSecurity.dclNonCasInheritance:
					result = "noncasinheritance";
					break;

				case CorDeclSecurity.dclNonCasLinkDemand:
					result = "noncaslinkdemand";
					break;

				case CorDeclSecurity.dclPermitOnly:
					result = "permitonly";
					break;

				case CorDeclSecurity.dclPrejitDenied:
					result = "prejitdeny";
					break;

				case CorDeclSecurity.dclPrejitGrant:
					result = "prejitgrant";
					break;

				case CorDeclSecurity.dclRequest:
					result = "request";
					break;

				case CorDeclSecurity.dclRequestMinimum:
					result = "reqmin";
					break;

				case CorDeclSecurity.dclRequestOptional:
					result = "reqopt";
					break;

				case CorDeclSecurity.dclRequestRefuse:
					result = "reqrefuse";
					break;

				case CorDeclSecurity.dclLinktimeCheck:
					result = "linkcheck";
					break;

				case CorDeclSecurity.dclInheritanceCheck:
					result = "inheritcheck";
					break;

				default:
					throw new NotImplementedException(string.Format("Unknown security flag ('{0}').", SecurityFlag));
			}

			return result;
		}
	}
}
