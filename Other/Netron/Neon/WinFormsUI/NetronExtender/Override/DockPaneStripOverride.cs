using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Netron.Neon.WinFormsUI;
namespace Netron.Neon.WinFormsUI.Netron
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
