using System;
using System.Xml;

namespace UseCaseMakerLibrary
{
	public class ActiveActor : IXMLNodeSerializable
	{
		#region Class Members
		private Boolean isPrimary = false;
		private String actorUniqueID = String.Empty;
		#endregion

		#region Constructors
		internal ActiveActor()
		{
		}
		#endregion

		#region Public Properties
		public String ActorUniqueID
		{
			get
			{
				return this.actorUniqueID;
			}
			set
			{
				this.actorUniqueID = value;
			}
		}

		public Boolean IsPrimary
		{
			get
			{
				return this.isPrimary;
			}
			set
			{
				this.isPrimary = value;
			}
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
