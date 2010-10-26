using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace Genetibase.NuGenMediImage
{
    [Serializable,TypeConverter(typeof(SliceViewerConverter))]
    [StructLayout(LayoutKind.Sequential)]
    public struct SliceViewer
    {
        [NonSerialized]
        private NuGenMediImageCtrl _ctrl;        

        private bool _visible;
        private bool _collapsed;
       
        public SliceViewer(bool visible,bool collapsed)
        {
            this._visible = visible;
            this._collapsed = collapsed;
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
                _ctrl.internalSliceViewer.Visible = _visible;
            }
        }

        public bool Collapsed
        {
            get { return _collapsed; }
            set 
            { 
                _collapsed = value;
                _ctrl.internalSliceViewer.Collapsed = _collapsed;                
            }
        }
    }
}
