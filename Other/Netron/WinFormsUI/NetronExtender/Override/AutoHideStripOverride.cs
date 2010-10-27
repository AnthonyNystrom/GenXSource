using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using WeifenLuo.WinFormsUI;
namespace WeifenLuo.WinFormsUI.Netron
{
	[ToolboxItem(false)]
	public class AutoHideStripOverride : AutoHideStripVS2003
	{
		protected internal AutoHideStripOverride(DockPanel dockPanel) : base(dockPanel)
		{
			BackColor = Color.Yellow;//SystemColors.ControlLight;
		}
	}
}
