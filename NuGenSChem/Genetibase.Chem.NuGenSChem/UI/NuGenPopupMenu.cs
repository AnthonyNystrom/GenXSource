using System;
using System.Collections.Generic;
using System.Text;
using Genetibase.UI;
using System.Windows.Forms;
using System.Security.Permissions;

namespace Genetibase.Chem.NuGenSChem
{
    class NuGenPopupMenu : RibbonPopup
    {
        private const int CS_DROPSHADOW = 0x00020000;

        private bool poppingUp;

        private NuGenPopupMenu parent;

        private NuGenEventHandler handler;

        //All this menus potential submenus
        private List<NuGenPopupMenu> children;

        public NuGenPopupMenu()
        {
            children = new List<NuGenPopupMenu>();
        }

        public NuGenPopupMenu(NuGenEventHandler handler, NuGenPopupMenu parent)
        {
            this.handler = handler;
            this.parent = parent;
            children = new List<NuGenPopupMenu>();
        }

        protected NuGenEventHandler Handler
        {
            get
            {
                return handler;
            }
        }

        public void AddChild(NuGenPopupMenu child)
        {
            children.Add(child);
        }

        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
            get
            {
                CreateParams parameters = base.CreateParams;

                if (DropShadowSupported)
                {
                    parameters.ClassStyle = (parameters.ClassStyle | CS_DROPSHADOW);
                }

                return parameters;
            }
        }

        protected override void OnDeactivate(EventArgs e)
        {
            if (ChildIsPoppingUp())
                return;

            poppingUp = false;

            if(parent != null)
                parent.ChildDeactivated();

            Hide();
        }

        protected bool ChildIsPoppingUp()
        {
            foreach (NuGenPopupMenu menu in children)
            {
                if (menu.PoppingUp)
                    return true;
            }

            return false;
        }        

        //Override to add logic to this method
        public virtual void ChildDeactivated() { }

        public virtual void InitializeDefaults()
        {
            foreach (NuGenPopupMenu menu in children)
            {
                menu.InitializeDefaults();
            }
        }

        public virtual void EnableControls()
        {
            foreach (NuGenPopupMenu menu in children)
            {
                menu.EnableControls();
            }
        }

        public bool PoppingUp
        {
            get
            {
                return poppingUp;                
            }

            set
            {
                poppingUp = value;
            }
        }

        public static bool DropShadowSupported
        {
            get { return IsWindowsXPOrAbove; }
        }

        public static bool IsWindowsXPOrAbove
        {
            get
            {
                OperatingSystem system = Environment.OSVersion;
                bool runningNT = system.Platform == PlatformID.Win32NT;

                return runningNT && system.Version.CompareTo(new Version(5, 1, 0, 0)) >= 0;
            }
        }
    }
}
