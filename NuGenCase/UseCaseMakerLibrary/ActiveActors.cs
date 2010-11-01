using System;
using System.Collections;
using System.Xml;

namespace UseCaseMakerLibrary
{
	public class ActiveActors : ICollection, IXMLNodeSerializable
	{
		#region Class Members
		private ArrayList items = new ArrayList();
		#endregion

		#region Constructors
		internal ActiveActors()
		{
		}
		#endregion

		#region Public Properties
		[XMLSerializeIgnore]
		public int Count
		{
			get
			{
				return items.Count;
			}
		}

		[XMLSerializeIgnore]
		public bool IsSynchronized
		{
			get
			{
				return items.IsSynchronized;
			}
		}

		[XMLSerializeIgnore]
		public object SyncRoot
		{
			get
			{
				return items.SyncRoot;
			}
		}

		[XMLSerializeIgnore]
		public object this[int index]
		{
			get
			{
				return items[index];
			}
		}
		#endregion

		#region Public Methods
		public int Add(object item)
		{
			int result = items.Add(item);

			return result;
		}

		public void Clear()
		{
			items.Clear();
		}

		public bool Contains(object item)
		{
			return items.Contains(item);
		}

		public int IndexOf(object item)
		{
			return items.IndexOf(item);
		}

		public void Insert(int index, object item)
		{
			items.Insert(index, item);
		}

		public void Remove(object item)
		{
			items.Remove(item);
		}

		public void RemoveAt(int index)
		{
			items.RemoveAt(index);
		}

		public void CopyTo(Array array, int index)
		{
			items.CopyTo(array, index);
		}

		public IEnumerator GetEnumerator()
		{
			return items.GetEnumerator();
		}

		public object FindByUniqueID(String uniqueID)
		{
			ActiveActor aactor = null;
			foreach(ActiveActor tmpAActor in this.items)
			{
				if(tmpAActor.ActorUniqueID == uniqueID)
				{
					aactor = tmpAActor;
				}
			}

			return aactor;
		}
		#endregion

		#region IXMLNodeSerializable Implementation
		public XmlNode XmlSerialize(XmlDocument document, object instance, string propertyName, bool deep)
		{
			return XmlSerializer.XmlSerialize(document,this,propertyName,deep);
		}

		public void XmlDeserialize(XmlNode fromNode, object instance)
		{
			XmlSerializer.XmlDeserialize(fromNode,instance);
		}
		#endregion
	}
}
