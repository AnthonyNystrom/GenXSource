using System;
using System.Windows.Forms;

namespace Netron.Neon.WinFormsUI
{
	internal class DummyControl : Control
	{
		public DummyControl()
		{
			SetStyle(ControlStyles.Selectable, false);
		}
	}
}
