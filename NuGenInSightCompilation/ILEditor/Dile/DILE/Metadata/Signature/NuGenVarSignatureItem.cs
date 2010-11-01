using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public class NuGenVarSignatureItem : NuGenBaseSignatureItem
	{
		private byte number;
		public byte Number
		{
			get
			{
				return number;
			}
			private set
			{
				number = value;
			}
		}

		private NuGenGenericParameter genericParameter;
		public NuGenGenericParameter GenericParameter
		{
			get
			{
				return genericParameter;
			}
			set
			{
				genericParameter = value;
			}
		}

		private bool methodParameter;
		public bool MethodParameter
		{
			get
			{
				return methodParameter;
			}
			set
			{
				methodParameter = value;
			}
		}

		private bool byRef = false;
		public bool ByRef
		{
			get
			{
				return byRef;
			}
			set
			{
				byRef = value;
			}
		}

		public NuGenVarSignatureItem(byte number, CorElementType elementType)
		{
			Number = number;
			MethodParameter = (elementType == CorElementType.ELEMENT_TYPE_MVAR);
		}

		public override string ToString()
		{
			string result = string.Empty;

			if (MethodParameter)
			{
				result = "!!" + (GenericParameter == null ? Number.ToString() : GenericParameter.Name);
			}
			else
			{
				result = "!" + Number.ToString();
			}

			if (ByRef)
			{
				result += "&";
			}

			return result;
		}
	}
}
