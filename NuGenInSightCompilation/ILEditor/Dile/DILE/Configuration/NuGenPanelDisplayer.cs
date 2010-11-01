using System;
using System.Collections.Generic;
using System.Text;

using Dile.Configuration;
using Dile.UI;
using System.Drawing;
using System.Xml.Serialization;
using System.Windows.Forms;


namespace Dile.Configuration
{
	[Serializable()]
	public class NuGenPanelDisplayer : NuGenBaseMenuInformation, NuGenIChangebleFont
	{
		[XmlIgnore()]
		public NuGenSerializableFont DefaultFont
		{
			get
			{
				return NuGenSettings.DefaultFont;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		private bool panelVisible;
		[XmlAttribute()]
		public bool PanelVisible
		{
			get
			{
				return panelVisible;
			}
			set
			{
				panelVisible = value;
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

		[XmlIgnore()]
        public NuGenBasePanel DockContent
		{
			get
			{
				return Panel;
			}
			set
			{
				Panel = value as NuGenBasePanel;
			}
		}

		private NuGenBasePanel panel;
		[XmlIgnore()]
		public NuGenBasePanel Panel
		{
			get
			{
				return panel;
			}
			set
			{
				panel = value;
			}
		}
	}
}