using System;
using System.Xml;

namespace UseCaseMakerLibrary
{
	/// <summary>
	/// Descrizione di riepilogo per DependencyItem.
	/// </summary>
	public class DependencyItem : IXMLNodeSerializable
	{
		#region Public Constants and Enumerators
		public enum ReferenceType
		{
			None = 0,
			Client = 1,
			Supplier = 2
		}
		#endregion

		#region Class Members
		private String stereotype = "";
		private ReferenceType type = ReferenceType.None;
		private String partnerUniqueID = "";
		#endregion

		#region Constructors
		public DependencyItem()
		{
			//
			// TODO: aggiungere qui la logica del costruttore
			//
		}
		#endregion

		#region Public Properties
		public String Stereotype
		{
			get
			{
				return this.stereotype;
			}
			set
			{
				this.stereotype = value;
			}
		}

		public ReferenceType Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		public String PartnerUniqueID
		{
			get
			{
				return this.partnerUniqueID;
			}
			set 
			{
				this.partnerUniqueID = value;
			}
		}
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
