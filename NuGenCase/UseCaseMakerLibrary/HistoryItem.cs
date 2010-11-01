using System;
using System.Xml;
using System.Globalization;

namespace UseCaseMakerLibrary
{
	/// <summary>
	/// Descrizione di riepilogo per HistoryItem.
	/// </summary>
	public class HistoryItem : IXMLNodeSerializable
	{
		#region Public Constants and Enumerators
		public enum HistoryType
		{
			Implementation = 0,
			Status = 1
		}
		#endregion

		#region Class Members
		private DateTime date;
		private HistoryType type;
		private Int32 action;
		private String notes = String.Empty;
		#endregion

		#region Constructors
		public HistoryItem()
		{
		}
		#endregion

		#region Public Properties
		[XMLSerializeIgnore]
		public String LocalizatedDateValue
		{
			get
			{
				return Date.ToString("d",DateTimeFormatInfo.CurrentInfo);
			}
		}

		[XMLSerializeIgnore]
		public DateTime Date
		{
			get
			{
				return this.date;
			}
			set
			{
				this.date = value;
			}
		}

		public String DateValue
		{
			get
			{
				return Convert.ToString(this.date,DateTimeFormatInfo.InvariantInfo);
			}
			set
			{
				this.date = Convert.ToDateTime(value,DateTimeFormatInfo.InvariantInfo);
			}
		}

		public HistoryType Type
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

		public Int32 Action
		{
			get
			{
				return this.action;
			}
			set
			{
				this.action = value;
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
