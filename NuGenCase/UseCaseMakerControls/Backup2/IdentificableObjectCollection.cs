using System;
using System.Collections;
using System.Xml;

namespace UseCaseMakerLibrary
{
	/// <summary>
	/// Descrizione di riepilogo per IdentificableObjectCollection.
	/// </summary>
	public class IdentificableObjectCollection : ICollection, IIdentificableObject, IXMLNodeSerializable
	{
		#region Private Enumerators and Constants
		#endregion

		#region Public Enumerators and Constants
		#endregion

		#region Class Members
		private ArrayList items = new ArrayList();
		private IdentificableObject ia = new IdentificableObject();
		#endregion

		#region Constructors
		internal IdentificableObjectCollection()
		{
			//
			// TODO: aggiungere qui la logica del costruttore
			//
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Returns the number of elements in the MenuItemCollection
		/// </summary>
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

		#region IIdentificableObject implementation
		[XMLSerializeAsAttribute]
		public String UniqueID
		{
			get
			{
				return this.ia.UniqueID;
			}
			set
			{
				this.ia.UniqueID = value;
			}
		}

		[XMLSerializeIgnore]
		public Package Owner
		{
			get
			{
				return this.ia.Owner;
			}
			set
			{
				this.ia.Owner = value;
			}
		}

		[XMLSerializeAsAttribute]
		public String Name
		{
			get
			{
				return this.ia.Name;
			}
			set
			{
				this.ia.Name = value;
			}
		}

		[XMLSerializeAsAttribute]
		public Int32 ID
		{
			get
			{
				return this.ia.ID;
			}
			set
			{
				this.ia.ID = value;
			}
		}

		[XMLSerializeAsAttribute]
		public String Prefix
		{
			get
			{
				return this.ia.Prefix;
			}
			set
			{
				this.ia.Prefix = value;
			}
		}

		[XMLSerializeAsAttribute(true)]
		public String Path
		{
			get
			{
				if(Owner == null)
				{
					return string.Empty;
				}
				return Owner.Path;
			}
		}

		[XMLSerializeIgnore]
		public String ElementID
		{
			get
			{
				return Owner.ElementID;
			}
		}
		#endregion
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

		public ICollection Sorted(string propertyName)
		{
			ArrayList sorter = new ArrayList(this);
			sorter.Sort(new PropertySorter(propertyName,"ASC"));
			this.Clear();
			foreach(object element in sorter)
			{
				this.Add(element);
			}
			return this;
		}

		public object FindByName(String name)
		{
			IdentificableObject element = null;
			foreach(IdentificableObject tmpElement in this)
			{
				if(tmpElement.Name == name)
				{
					element = tmpElement;
				}
			}

			return element;
		}

		public object FindByUniqueID(String uniqueID)
		{
			IdentificableObject element = null;
			foreach(IdentificableObject tmpElement in this)
			{
				if(tmpElement.UniqueID == uniqueID)
				{
					element = tmpElement;
				}
			}

			return element;
		}

		public object FindByElementID(String elementID)
		{
			IdentificableObject element = null;
			foreach(IdentificableObject tmpElement in this)
			{
				if(tmpElement.ElementID == elementID)
				{
					element = tmpElement;
				}
			}

			return element;
		}

		public object FindByPath(String path)
		{
			IdentificableObject element = null;
			foreach(IdentificableObject tmpElement in this)
			{
				if(tmpElement.Path == path)
				{
					element = tmpElement;
				}
			}

			return element;
		}

		public Int32 GetNextFreeID()
		{
			int id = 0;
			foreach(IdentificableObject tmpElement in this)
			{
				if(tmpElement.ID > id)
				{
					id = tmpElement.ID;
				}
			}

			return (id + 1);
		}
		#endregion

		#region Protected Methods
		#endregion

		#region Private Methods
		#endregion

		#region IXMLNodeSerializable Implementation
		public XmlNode XmlSerialize(XmlDocument document, object instance, string propertyName, bool deep)
		{
			return XmlSerializer.XmlSerialize(document,this,propertyName,true);
		}

		public void XmlDeserialize(XmlNode fromNode, object instance)
		{
			XmlSerializer.XmlDeserialize(fromNode,instance);
		}
		#endregion
	}
}
