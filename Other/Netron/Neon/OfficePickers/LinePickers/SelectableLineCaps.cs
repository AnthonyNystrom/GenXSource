using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Netron.Neon.OfficePickers
{
    internal class SelectableLineCaps
    {
        private ConnectionCap mLineCap;

        public ConnectionCap ConnectionCap
        {
            get { return mLineCap; }
            set { mLineCap = value; }
        }

        private bool mSelected = false;

        public bool Selected
        {
            get { return mSelected; }
            set { mSelected = value; }
        }

        private bool mHotTrack = false;

        public bool HotTrack
        {
            get { return mHotTrack; }
            set { mHotTrack = value; }
        }

        public SelectableLineCaps(ConnectionCap cap)
        {
            mLineCap = cap;
        }
    }

    
}