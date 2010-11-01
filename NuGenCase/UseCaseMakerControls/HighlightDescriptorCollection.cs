using System;
using System.Collections;

namespace UseCaseMakerControls
{
	/// <summary>
	/// Summary description for SeperaratorCollection.
	/// </summary>
	public class HighLightDescriptorCollection
	{
		private ArrayList items = new ArrayList();
		public HighLightDescriptorCollection()
		{
		}

		public void AddRange(ICollection c)
		{
			items.AddRange(c);
		}


		#region IList Members
		public bool IsReadOnly
		{
			get
			{
				return items.IsReadOnly;
			}
		}

		public HighlightDescriptor this[int index]
		{
			get
			{
				return (HighlightDescriptor)items[index];
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

		public void Insert(int index, HighlightDescriptor value)
		{
			items.Insert(index, value);
		}

		public void Remove(HighlightDescriptor value)
		{
			items.Remove(value);
		}

		public bool Contains(HighlightDescriptor value)
		{
			return items.Contains(value);
		}

		public void Clear()
		{
			items.Clear();
		}

		public int IndexOf(HighlightDescriptor value)
		{
			return items.IndexOf(value);
		}

		public int Add(HighlightDescriptor value)
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
