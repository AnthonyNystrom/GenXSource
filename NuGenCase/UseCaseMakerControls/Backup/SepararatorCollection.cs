using System;
using System.Collections;

namespace UseCaseMakerControls
{
	/// <summary>
	/// Summary description for SepararatorCollection.
	/// </summary>
	public class SepararatorCollection
	{
		private ArrayList items = new ArrayList();

		public SepararatorCollection()
		{
		}

		public void AddRange(ICollection c)
		{
			items.AddRange(c);
		}

		internal char[] GetAsCharArray()
		{
			return (char[])items.ToArray(typeof(char));
		}

		#region IList Members
		public bool IsReadOnly
		{
			get
			{
				return items.IsReadOnly;
			}
		}

		public char this[int index]
		{
			get
			{
				return (char)items[index];
			}
			set
			{
				items[index] = value;
			}
		}

		public void RemoveAt(int index)
		{
			items.RemoveAt(index);
		}

		public void Insert(int index, char value)
		{
			items.Insert(index, value);
		}

		public void Remove(char value)
		{
			items.Remove(value);
		}

		public bool Contains(char value)
		{
			return items.Contains(value);
		}

		public void Clear()
		{
			items.Clear();
		}

		public int IndexOf(char value)
		{
			return items.IndexOf(value);
		}

		public int Add(char value)
		{
			return items.Add(value);
		}

		public bool IsFixedSize
		{
			get
			{
				return items.IsFixedSize;
			}
		}
		#endregion

		#region ICollection Members
		public bool IsSynchronized
		{
			get
			{
				return items.IsSynchronized;
			}
		}

		public int Count
		{
			get
			{
				return items.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			items.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return items.SyncRoot;
			}
		}
		#endregion

		#region IEnumerable Members
		public IEnumerator GetEnumerator()
		{
			return items.GetEnumerator();
		}
		#endregion
	}
}
