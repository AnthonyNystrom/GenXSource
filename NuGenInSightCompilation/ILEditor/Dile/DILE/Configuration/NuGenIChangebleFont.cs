using System;

using System.Drawing;
using System.Windows.Forms;
using Dile.UI;


namespace Dile.Configuration
{
	interface NuGenIChangebleFont
	{
		NuGenSerializableFont DefaultFont
		{
			get;
			set;
		}

		NuGenBasePanel DockContent
		{
			get;
			set;
		}

		Font Font
		{
			get;
			set;
		}

		NuGenSerializableFont SerializableFont
		{
			get;
			set;
		}

		NuGenSerializableFont TempFont
		{
			get;
			set;
		}
	}
}
