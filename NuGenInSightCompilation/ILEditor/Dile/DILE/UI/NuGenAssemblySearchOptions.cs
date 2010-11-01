using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.UI
{
	public class NuGenAssemblySearchOptions
	{
		private NuGenAssembly assembly;
		private NuGenAssembly Assembly
		{
			get
			{
				return assembly;
			}
			set
			{
				assembly = value;
			}
		}

		private SearchOptions selectedOptions;
		public SearchOptions SelectedOptions
		{
			get
			{
				return selectedOptions;
			}
			set
			{
				selectedOptions = value;
			}
		}

		public NuGenAssemblySearchOptions(NuGenAssembly assembly)
		{
			Assembly = assembly;
			SelectedOptions = Assembly.SearchOptions;
		}

		public bool IsSearchOptionChosen(SearchOptions searchOption)
		{
			return ((SelectedOptions & searchOption) == searchOption);
		}

		public void SetSearchOption(SearchOptions searchOption)
		{
			SelectedOptions |= searchOption;
		}

		public void ClearSearchOption(SearchOptions searchOption)
		{
			SelectedOptions ^= searchOption;
		}

		public void UpdateAssemblySearchOptions()
		{
			Assembly.SearchOptions = SelectedOptions;
		}

		public override string ToString()
		{
			return Assembly.FileName;
		}
	}
}