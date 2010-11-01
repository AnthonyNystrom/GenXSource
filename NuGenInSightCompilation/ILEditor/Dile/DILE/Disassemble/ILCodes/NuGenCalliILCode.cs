using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public class NuGenCalliILCode : BaseParameterILCode<uint, NuGenStandAloneSignature>
	{
		public override void DecodeParameter()
		{
			Text = string.Format("{0} {1}", OpCode.Name, DecodedParameter.Text);
		}
	}
}