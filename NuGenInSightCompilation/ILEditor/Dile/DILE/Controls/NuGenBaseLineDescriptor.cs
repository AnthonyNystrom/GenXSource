using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace Dile.Controls
{
	public abstract class NuGenBaseLineDescriptor
	{
		public abstract Color ForeColor
		{
			get;
		}

		public abstract Color BackColor
		{
			get;
		}

		private int instructionOffset;
		public int InstructionOffset
		{
			get
			{
				return instructionOffset;
			}
			protected set
			{
				instructionOffset = value;
			}
		}

		private bool scrollToOffset;
		public bool ScrollToOffset
		{
			get
			{
				return scrollToOffset;
			}
			protected set
			{
				scrollToOffset = value;
			}
		}

		public NuGenBaseLineDescriptor(int instructionOffset, bool scrollToOffset)
		{
			InstructionOffset = instructionOffset;
			ScrollToOffset = scrollToOffset;
		}
	}
}