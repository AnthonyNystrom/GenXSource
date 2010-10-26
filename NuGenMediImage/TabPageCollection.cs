using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenMediImage
{
    public class TabPageCollection : List<TabPage>
    {
        Genetibase.NuGenMediImage.UI.Controls.RibbonControl tabBar = null;

        public TabPageCollection()
        {
        }

        internal void Init(Genetibase.NuGenMediImage.UI.Controls.RibbonControl pTabBar)
        {
            tabBar = pTabBar;

            foreach (TabPage c in tabBar.TabPages)
            {
                this.Add(c);                  
            }                        
        }

        public new void Add(TabPage tbPage)
        {
            Genetibase.NuGenMediImage.UI.Controls.RibbonGroup g = new Genetibase.NuGenMediImage.UI.Controls.RibbonGroup();
            g.Size = new System.Drawing.Size(271, 71);
            g.Location = new System.Drawing.Point(4,4);
            tbPage.Controls.Add(g);            
            base.Add(tbPage);
        }
    }
}
