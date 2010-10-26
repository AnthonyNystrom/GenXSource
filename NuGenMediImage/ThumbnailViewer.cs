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
    [Serializable,TypeConverter(typeof(ThumbnailViewerConverter))]
    [StructLayout(LayoutKind.Sequential)]
    public struct ThumbnailViewer
    {
        [NonSerialized]
        private NuGenMediImageCtrl _ctrl;        

        private bool _visible;
        private bool _collapsed;
        private bool _showBrowseButton;
        private ThumbnailFileType _thumbnailFileType;
        
        public ThumbnailViewer(bool visible,bool collapsed,bool showBrowseButton,ThumbnailFileType thumbnailFileType)
        {
            this._visible = visible;
            this._collapsed = collapsed;
            this._showBrowseButton = showBrowseButton;
            this._thumbnailFileType = thumbnailFileType;
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

        public ThumbnailFileType ThumbnailFileType
        {
            get { return _thumbnailFileType; }
            set { _thumbnailFileType = value; }
        }


        public bool Visible
        {
            get { return _visible; }
            set 
            { 
                _visible = value;
                _ctrl.internalThumbnailViewer.Visible = _visible;
            }
        }

        public bool ShowBrowseButton
        {
            get { return _showBrowseButton; }
            set 
            { 
                _showBrowseButton = value;

                if( _ctrl.internalThumbnailViewer.Collapsed )
                    _ctrl.internalBrowseButtonGroup.Visible = false;
                else
                    _ctrl.internalBrowseButtonGroup.Visible = value;
            }
        }

        public bool Collapsed
        {
            get { return _collapsed; }
            set 
            { 
                _collapsed = value;
                _ctrl.internalThumbnailViewer.Collapsed = _collapsed;

                if( _collapsed == true )
                    _ctrl.internalBrowseButtonGroup.Visible = false;
                else
                    _ctrl.internalBrowseButtonGroup.Visible = _showBrowseButton;

            }
        }
    }
}
