// *****************************************************************************
// 
//  Copyright 2004, Weifen Luo
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Weifen Luo
//  and are supplied subject to licence terms.
// 
//  WinFormsUI Library Version 1.0
// *****************************************************************************
using System;
using System.Drawing;
using Netron.Neon.WinFormsUI;

namespace Netron.Neon.WinFormsUI.Netron
{
	public class Extender
	{
		

		public class DockPaneStripOverrideFactory : DockPanelExtender.IDockPaneStripFactory
		{
			public DockPaneStripBase CreateDockPaneStrip(DockPane pane)
			{
				return new DockPaneStripOverride(pane);
			}
		}

		public class AutoHideStripOverrideFactory : DockPanelExtender.IAutoHideStripFactory
		{
			public AutoHideStripBase CreateAutoHideStrip(DockPanel panel)
			{
				return new AutoHideStripOverride(panel);
			}
		}

		public class DockPaneCaptionFromBaseFactory : DockPanelExtender.IDockPaneCaptionFactory
		{
			public DockPaneCaptionBase CreateDockPaneCaption(DockPane pane)
			{
				return new DockPaneCaptionFromBase(pane);
			}
		}

		public class DockPaneTabFromBaseFactory : DockPanelExtender.IDockPaneTabFactory
		{
			public DockPaneTab CreateDockPaneTab(IDockContent content)
			{
				return new DockPaneTabFromBase(content);
			}
		}

		public class DockPaneStripFromBaseFactory : DockPanelExtender.IDockPaneStripFactory
		{
			public DockPaneStripBase CreateDockPaneStrip(DockPane pane)
			{
				return new DockPaneStripFromBase(pane);
			}
		}

		public  class AutoHideTabFromBaseFactory : DockPanelExtender.IAutoHideTabFactory
		{
			public AutoHideTab CreateAutoHideTab(IDockContent content)
			{
				return new AutoHideTabFromBase(content);
			}
		}

		public class AutoHideStripFromBaseFactory : DockPanelExtender.IAutoHideStripFactory
		{
			public AutoHideStripBase CreateAutoHideStrip(DockPanel panel)
			{
				return new AutoHideStripFromBase(panel);
			}
		}

        public static void SetExtender(DockPanel dockPanel)
        {
             dockPanel.Extender.AutoHideTabFactory = new Extender.AutoHideTabFromBaseFactory();
            dockPanel.Extender.DockPaneTabFactory = new Extender.DockPaneTabFromBaseFactory();
            dockPanel.Extender.AutoHideStripFactory = new Extender.AutoHideStripFromBaseFactory();
            dockPanel.Extender.DockPaneCaptionFactory = new Extender.DockPaneCaptionFromBaseFactory();
            dockPanel.Extender.DockPaneStripFactory = new Extender.DockPaneStripFromBaseFactory();
        }
	
	}
}
