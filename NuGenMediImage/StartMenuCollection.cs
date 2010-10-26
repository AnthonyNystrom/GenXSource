using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenMediImage
{
    public class StartMenuCollection : List<Genetibase.NuGenMediImage.UI.Controls.RibbonButton>
    {
        Genetibase.NuGenMediImage.UI.Menus.PhotoMenu photoMenu = null;

        public StartMenuCollection()
        {
        }

        internal void Init( Genetibase.NuGenMediImage.UI.Menus.PhotoMenu menu)
        {
            photoMenu = menu;
            //foreach (Control c in photoMenu.MenuItemControls)
            //{
            //    base.Add(c);                  
            //}                        
        }

        public new void Add(Genetibase.NuGenMediImage.UI.Controls.RibbonButton button)
        {
            base.Add(button);
            photoMenu.AddButton(button);
        }

        public void AddSeperator()
        {
            photoMenu.AddSeperator();
        }
    }
}
