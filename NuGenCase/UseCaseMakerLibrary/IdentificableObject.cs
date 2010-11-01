using System;
using System.Xml;

namespace UseCaseMakerLibrary
{
	public class IdentificableObject : IIdentificableObject, IXMLNodeSerializable
	{
		#region Class Members
		private Int32 id = -1;
		private String name = String.Empty;
		private String prefix = String.Empty;
		private String uniqueID = String.Empty;
		private Package owner = null;
		private UserViewStatus userViewStatus = new UserViewStatus();
		#endregion

		#region Constructors
		internal IdentificableObject()
		{
			MakeUniqueID();
		}

		internal IdentificableObject(String name, String prefix, Int32 id)
		{
			MakeUniqueID();
			this.name = name;
			this.prefix = prefix;
			this.id = id;
		}

		internal IdentificableObject(String name, String prefix, Int32 id, Package owner)
		{
			MakeUniqueID();
			this.name = name;
			this.prefix = prefix;
			this.id = id;
			this.owner = owner;
		}
		#endregion

		#region Public Properties
		[XMLSerializeAsAttribute]
		public String UniqueID
		{
			get
			{
				return this.uniqueID;
			}
			set
			{
				this.uniqueID = value;
			}
		}

		[XMLSerializeIgnore]
		public Package Owner
		{
			get
			{
				return this.owner;
			}
			set
			{
				this.owner = value;
			}
		}

		[XMLSerializeAsAttribute]
		public String Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		[XMLSerializeAsAttribute]
		public Int32 ID
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		[XMLSerializeAsAttribute]
		public String Prefix
		{
			get
			{
				return this.prefix;
			}
			set
			{
				this.prefix = value;
			}
		}

		[XMLSerializeAsAttribute(true)]
		public String Path
		{
			get
			{
				string path = this.Prefix + this.ID.ToString();
				IdentificableObject owner = this.Owner;
				while(owner != null)
				{
					path = owner.Prefix + owner.ID.ToString() + "." + path;
					owner = owner.Owner;
				}
				return path;
			}
		}

		[XMLSerializeIgnore]
		public String ElementID
		{
			get
			{
				return this.prefix + this.id.ToString();
			}
		}

		[XMLSerializeIgnore]
		public UserViewStatus ObjectUserViewStatus
		{
			get
			{
				return this.userViewStatus;
			}
		}
		#endregion
	
		#region Private Methods
		private void MakeUniqueID()
		{
			Guid guid = Guid.NewGuid();
			uniqueID = guid.ToString();
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
