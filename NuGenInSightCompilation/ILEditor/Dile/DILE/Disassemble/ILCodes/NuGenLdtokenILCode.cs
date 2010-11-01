using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public class NuGenLdtokenILCode : BaseParameterILCode<uint, NuGenTokenBase>
	{
		public NuGenLdtokenILCode()
		{
		}

		public override void DecodeParameter()
		{
			if (DecodedParameter is NuGenFieldDefinition)
			{
				Text = string.Format("ldtoken field {0}", ((NuGenFieldDefinition)DecodedParameter).Text);
			}
			else
			{
				Text = string.Format("ldtoken {0}", DecodedParameter.Name);
			}
		}

		public void SetGenericsMethodParameters(Dictionary<uint, NuGenTokenBase> allTokens, List<NuGenGenericParameter> genericParameters)
		{
			if (DecodedParameter is NuGenTextTokenBase && DecodedParameter is NuGenIHasSignature)
			{
				NuGenIHasSignature hasSignature = (NuGenIHasSignature)DecodedParameter;

				if (hasSignature.SignatureReader.HasGenericMethodParameter)
				{
					hasSignature.SignatureReader.SetGenericParametersOfMethod(genericParameters);
					((NuGenTextTokenBase)DecodedParameter).LazyInitialize(allTokens);
				}
			}
		}
	}
}