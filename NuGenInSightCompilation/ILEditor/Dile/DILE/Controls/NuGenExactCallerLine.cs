using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace Dile.Controls
{
	public class NuGenExactCallerLine : NuGenBaseLineDescriptor
	{
		public override Color ForeColor
		{
			get
			{
				return Color.Black;
			}
		}

		public override Color BackColor
		{
			get
			{
				return Color.LightGreen;
			}
		}

		public NuGenExactCallerLine(int instructionOffset)
			: base(instructionOffset, true)
		{
		}
	}
}