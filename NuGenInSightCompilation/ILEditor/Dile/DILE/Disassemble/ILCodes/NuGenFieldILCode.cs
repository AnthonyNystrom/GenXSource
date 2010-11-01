using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public class NuGenFieldILCode : BaseParameterILCode<uint, NuGenTokenBase>
	{
		public override void DecodeParameter()
		{
			if (DecodedParameter is NuGenFieldDefinition)
			{
				Text = string.Format("{0} {1}", OpCode.Name, ((NuGenFieldDefinition)DecodedParameter).Text);
			}
			else
			{
				Text = string.Format("{0} {1}", OpCode.Name, DecodedParameter);
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