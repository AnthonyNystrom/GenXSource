using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble
{
	public abstract class NuGenTextTokenBase : NuGenTokenBase, NuGenILazyInitialized
	{
		private bool isCreateTextCalled = false;
		protected bool IsCreateTextCalled
		{
			get
			{
				return isCreateTextCalled;
			}
			private set
			{
				isCreateTextCalled = value;
			}
		}

		public void LazyInitialize(Dictionary<uint, NuGenTokenBase> allTokens)
		{
			if (!IsCreateTextCalled)
			{
				CreateText(allTokens);
				IsCreateTextCalled = true;
			}
		}

		protected virtual void CreateText(Dictionary<uint, NuGenTokenBase> allTokens)
		{
		}
	}
}
