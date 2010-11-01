using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace Dile.Controls
{
	public class NuGenNextPossibleInstructionLine : NuGenBaseLineDescriptor
	{
		public override Color ForeColor
		{
			get
			{
				return Color.White;
			}
		}

		public override Color BackColor
		{
			get
			{
				return Color.LightSlateGray;
			}
		}

		public NuGenNextPossibleInstructionLine(int instructionOffset)
			: base(instructionOffset, false)
		{
		}
	}
}