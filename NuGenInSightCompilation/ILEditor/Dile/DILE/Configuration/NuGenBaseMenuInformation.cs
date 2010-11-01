using System;
using System.Collections.Generic;
using System.Text;

using Dile.UI;
using System.Xml.Serialization;

namespace Dile.Configuration
{
	[Serializable()]
	public class NuGenBaseMenuInformation : NuGenIMenuInformation
	{
		private MenuFunction menuFunction;
		[XmlAttribute()]
		public MenuFunction MenuFunction
		{
			get
			{
				return menuFunction;
			}
			set
			{
				menuFunction = value;
			}
		}

		public NuGenBaseMenuInformation()
		{
		}

		public NuGenBaseMenuInformation(MenuFunction menuFunction)
			: this()
		{
			MenuFunction = menuFunction;
		}
	}
}