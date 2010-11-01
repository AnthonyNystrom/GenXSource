using System;
using System.Collections.Generic;
using System.Text;

using Dile.Controls;

namespace Dile.UI
{
	public class NuGenCodeObjectDisplayOptions
	{
		private List<NuGenBaseLineDescriptor> specialLinesToAdd = null;
		public List<NuGenBaseLineDescriptor> SpecialLinesToAdd
		{
			get
			{
				return specialLinesToAdd;
			}
			set
			{
				specialLinesToAdd = value;
			}
		}

		private NuGenBaseLineDescriptor currentLine = null;
		public NuGenBaseLineDescriptor CurrentLine
		{
			get
			{
				return currentLine;
			}
			set
			{
				currentLine = value;
			}
		}

		private uint navigateToOffset = 0;
		public uint NavigateToOffset
		{
			get
			{
				return navigateToOffset;
			}
			set
			{
				navigateToOffset = value;
				IsNavigateSet = true;
			}
		}

		private bool isNavigateSet = false;
		public bool IsNavigateSet
		{
			get
			{
				return isNavigateSet;
			}
			set
			{
				isNavigateSet = value;
			}
		}

		public NuGenCodeObjectDisplayOptions()
		{
		}

		public NuGenCodeObjectDisplayOptions(int instructionOffset, bool exactLocation)
		{
			if (exactLocation)
			{
				CurrentLine = new NuGenCurrentLine(instructionOffset);
			}
			else
			{
				CurrentLine = new NuGenCallerLine(instructionOffset);
			}
		}
	}
}