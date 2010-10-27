using System;

namespace Genetibase.Debug
{
	public sealed class NuGenTSelector
	{
		private int _start;
		private int _length;

		public event EventHandler NuGenTSelectorUpdated;

		public NuGenTSelector()
		{
			_start = 0;
			_length = 0;
		}

		public int Start
		{
			get
			{
				return _start;
			}

			set
			{
				_start = value;
				OnNuGenTSelectorUpdated();
			}
		}

		public int Length
		{
			get
			{
				return _length;
			}

			set
			{
				_length = value;
				OnNuGenTSelectorUpdated();
			}
		}

		public bool IsLineSelected(int line)
		{
			return (line >= _start && line < (_start + _length));
		}

		private void OnNuGenTSelectorUpdated()
		{
			if(NuGenTSelectorUpdated != null)
			{
				NuGenTSelectorUpdated(this, EventArgs.Empty);
			}
		}
	}
}
