using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using WeifenLuo.WinFormsUI;
namespace WeifenLuo.WinFormsUI.Netron
{
	[ToolboxItem(false)]
	public class DockPaneStripOverride : DockPaneStripVS2003
	{
		protected internal DockPaneStripOverride(DockPane pane) : base(pane)
		{
			BackColor = SystemColors.ControlLight;
		}
	}
}
