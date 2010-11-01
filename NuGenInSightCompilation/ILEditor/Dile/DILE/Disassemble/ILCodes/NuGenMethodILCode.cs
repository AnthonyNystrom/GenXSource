using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public class NuGenMethodILCode : BaseParameterILCode<uint, NuGenTokenBase>
	{
		public NuGenMethodILCode()
		{
		}

		public override void DecodeParameter()
		{
			if (DecodedParameter is NuGenMethodDefinition)
			{
				NuGenMethodDefinition methodDefinition = (NuGenMethodDefinition)DecodedParameter;

				Text = string.Format("{0} {1}", OpCode.Name, methodDefinition.Text);
			}
			else if (DecodedParameter is NuGenMemberReference)
			{
				NuGenMemberReference memberReference = (NuGenMemberReference)DecodedParameter;

				Text = string.Format("{0} {1}", OpCode.Name, memberReference.Text);
			}
			else if (DecodedParameter is NuGenMethodSpec)
			{
				NuGenMethodSpec methodSpec = (NuGenMethodSpec)DecodedParameter;

				Text = string.Format("{0} {1}", OpCode.Name, methodSpec.Name);
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