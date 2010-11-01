using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public abstract class BaseParameterILCode<ParameterType, DecodedParameterType> : NuGenBaseILCode
	{
		private ParameterType parameter;
		public ParameterType Parameter
		{
			get
			{
				return parameter;
			}

			set
			{
				parameter = value;
			}
		}

		private DecodedParameterType decodedParameter;
		public DecodedParameterType DecodedParameter
		{
			get
			{
				return decodedParameter;
			}

			set
			{
				decodedParameter = value;
			}
		}

		public BaseParameterILCode()
		{
		}
	}
}