using System;

namespace Genetibase.Debug
{
	internal sealed class StringCycleBuffer
	{
		private int _capacity;
		private int _writePosition;
		private int _numberItems;
		private string[] _items;
		private bool[] highlight_;

		public StringCycleBuffer(int capacity)
		{
			_capacity = capacity;
			_writePosition = 0;
			_numberItems = 0;
			_items = new string[_capacity];
			highlight_ = new bool[_capacity];
		}

		public void Add(string value)
		{
			Add(value, false);
		}
		
		public void Add(string value, bool highlight)
		{
			_items[_writePosition] = value;
			highlight_[_writePosition] = highlight;
			_writePosition = (_writePosition + 1) % _capacity;
			if(_numberItems < _capacity)
			{
				_numberItems++;
			}
		}
		
		public int Length
		{
			get { return _numberItems; }	
		}

		public string this[int index]
		{
			get { return _items[GetItemPhysicalIndex(index)]; }	
		}
		
		public bool IsHighlighted(int index)
		{
			return highlight_[GetItemPhysicalIndex(index)];
		}

		private int GetItemPhysicalIndex(int virtualIndex)
		{
			int physicalIndex;

			if(virtualIndex < 0 || virtualIndex >= _numberItems)
			{
				throw new ArgumentOutOfRangeException("virtualIndex");
			}

			if(_numberItems == _capacity)
			{
				physicalIndex = (_writePosition + virtualIndex)	% _capacity;
			}
			else
			{
				physicalIndex = virtualIndex;	
			}

			return physicalIndex;
		}
	}
}
