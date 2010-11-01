using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public class NuGenSwitchILCode : BaseParameterILCode<int[], string>
	{
		public NuGenSwitchILCode()
		{
		}

		private int RelativeTargetAddress(int location)
		{
			return Offset + Parameter.Length * 4 + 5 + location;
		}

		public override void DecodeParameter()
		{
			StringBuilder fullInstructionBuilder = new StringBuilder();
			fullInstructionBuilder.Append("switch (");

			for (int index = 0; index < Parameter.Length; index++)
			{
				fullInstructionBuilder.AppendFormat("IL_{0}", NuGenHelperFunctions.FormatAsHexNumber(RelativeTargetAddress(Parameter[index]), 4));

				if (index == Parameter.Length - 1)
				{
					fullInstructionBuilder.Append(")");
				}
				else
				{	
					fullInstructionBuilder.Append(", ");
				}
			}

			Text = fullInstructionBuilder.ToString();
		}
	}
}