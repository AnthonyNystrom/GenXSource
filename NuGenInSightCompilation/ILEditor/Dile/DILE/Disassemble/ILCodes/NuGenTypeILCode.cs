using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public class NuGenTypeILCode : BaseParameterILCode<uint, NuGenTokenBase>
	{
		public override void DecodeParameter()
		{
			string parameterText = string.Empty;

			if (DecodedParameter != null)
			{
				Type decodedParameterType = DecodedParameter.GetType();

				if (decodedParameterType == typeof(NuGenMethodDefinition))
				{	
					parameterText = ((NuGenMethodDefinition)DecodedParameter).Text;
				}
				else if (decodedParameterType == typeof(NuGenMemberReference))
				{
					parameterText = ((NuGenMemberReference)DecodedParameter).Text;
				}
				else if (decodedParameterType == typeof(NuGenTypeReference))
				{
					parameterText = ((NuGenTypeReference)DecodedParameter).FullName;
				}
				else if (decodedParameterType == typeof(NuGenTypeDefinition))
				{	
					parameterText = ((NuGenTypeDefinition)DecodedParameter).FullName;
				}
				else if (decodedParameterType == typeof(NuGenTypeSpecification))
				{
					parameterText = ((NuGenTypeSpecification)DecodedParameter).ToString();
				}
			}

			Text = string.Format("{0} {1}", OpCode.Name, parameterText);
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