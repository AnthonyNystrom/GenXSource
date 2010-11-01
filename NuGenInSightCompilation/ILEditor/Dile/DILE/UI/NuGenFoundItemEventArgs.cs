using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.UI
{
	public class NuGenFoundItemEventArgs : EventArgs
	{
		private bool cancel = false;
		public bool Cancel
		{
			get
			{
				return cancel;
			}
			set
			{
				cancel = value;
			}
		}

		private NuGenTokenBase foundTokenObject;
		public NuGenTokenBase FoundTokenObject
		{
			get
			{
				return foundTokenObject;
			}
			private set
			{
				foundTokenObject = value;
			}
		}

		private NuGenAssembly assembly;
		public NuGenAssembly Assembly
		{
			get
			{
				return assembly;
			}
			private set
			{
				assembly = value;
			}
		}

		public NuGenFoundItemEventArgs(NuGenAssembly assembly, NuGenTokenBase foundTokenObject)
		{
			Assembly = assembly;
			FoundTokenObject = foundTokenObject;
		}
	}
}