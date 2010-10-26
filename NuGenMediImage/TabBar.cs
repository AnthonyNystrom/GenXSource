using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace Genetibase.NuGenMediImage
{
    [Serializable,TypeConverter(typeof(TabBarConverter))]
    [StructLayout(LayoutKind.Sequential)]
    public struct TabBar
    {
        [NonSerialized]
        private NuGenMediImageCtrl _ctrl;        

        private bool _visible;
        private bool _collapsed;
        private bool _showStartTab;

        public TabBar(bool visible, bool collapsed, bool showStartTab)
        {
            this._visible = visible;
            this._collapsed = collapsed;
            this._showStartTab = showStartTab;
            _ctrl = null;
        }

        internal NuGenMediImageCtrl Ctrl
        {
            get { return _ctrl; }
            set 
            { 
                _ctrl = value;                 
            }
        }

        public bool Visible
        {
            get { return _visible; }
            set 
            { 
                _visible = value;
                _ctrl.internalTabBar.Visible = _visible;
            }
        }

        public bool ShowStartTab
        {
            get { return _showStartTab; }
            set
            {
                _showStartTab = value;
                _ctrl.internalTabBar.ShowStartTab = _showStartTab;
            }
        }

        public bool Collapsed
        {
            get { return _collapsed; }
            set 
            { 
                _collapsed = value;
                _ctrl.internalTabBar.Collapsed = _collapsed;                
            }
        }

        //[Editor(typeof(CollectionEditor), typeof(UITypeEditor)),
        //    DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //public TabControl.TabPageCollection TabPages
        //{
        //    get
        //    {
        //        return _ctrl.internalTabBar.TabPages;
        //        //return new TabControl.TabPageCollection(_ctrl.internalTabBar);
        //    }
        //}
    }
}
