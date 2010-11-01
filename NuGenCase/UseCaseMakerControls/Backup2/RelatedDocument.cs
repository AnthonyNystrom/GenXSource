using System;
using System.Xml;

namespace UseCaseMakerLibrary
{
	/// <summary>
	/// Descrizione di riepilogo per RelatedDocument.
	/// </summary>
	public class RelatedDocument : IXMLNodeSerializable
	{
		#region Class Members
		String fileName = String.Empty;
		#endregion

		#region Constructors
		public RelatedDocument()
		{
		}
		#endregion

		#region Public Properties
		public String FileName
		{
			get
			{
				return this.fileName;
			}
			set
			{
				this.fileName = value;
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
