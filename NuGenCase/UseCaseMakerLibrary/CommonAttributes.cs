using System;
using System.Xml;

namespace UseCaseMakerLibrary
{
	public class CommonAttributes : IXMLNodeSerializable
	{
		#region Class Members
		private String description = String.Empty;
		private String notes = String.Empty;
		private RelatedDocuments relatedDocuments = new RelatedDocuments();
		#endregion

		#region Constructors
		#endregion

		#region Public Properties
		public String Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		public String Notes
		{
			get
			{
				return this.notes;
			}
			set
			{
				this.notes = value;
			}
		}

		public RelatedDocuments RelatedDocuments
		{
			get
			{
				return this.relatedDocuments;
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
