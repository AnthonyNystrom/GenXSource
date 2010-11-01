using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Xml.Serialization;
using System.Windows.Forms;
using Dile.UI;


namespace Dile.Configuration
{
	public class NuGenCodeEditorFontSettings : NuGenIChangebleFont
	{
		private NuGenSerializableFont defaultFont;
		[XmlIgnore()]
		public NuGenSerializableFont DefaultFont
		{
			get
			{
				return defaultFont;
			}
			set
			{
				defaultFont = value;
			}
		}

		private string title;
		[XmlIgnore()]
		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
			}
		}

		#region IChangebleFont Members

		[XmlIgnore()]
        public NuGenBasePanel DockContent
		{
			get
			{
				return null;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		[XmlIgnore()]
		public Font Font
		{
			get
			{
				return (SerializableFont == null ? null : SerializableFont.GetFont());
			}
			set
			{
				SerializableFont = new NuGenSerializableFont(value);
			}
		}

		private NuGenSerializableFont serializableFont;
		public NuGenSerializableFont SerializableFont
		{
			get
			{
				return serializableFont;
			}
			set
			{
				serializableFont = value;
			}
		}

		private NuGenSerializableFont tempFont;
		[XmlIgnore()]
		public NuGenSerializableFont TempFont
		{
			get
			{
				return tempFont;
			}
			set
			{
				tempFont = value;
			}
		}

		#endregion

		public override string ToString()
		{
			return Title;
		}
	}
}