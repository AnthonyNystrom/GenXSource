using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Netron.Neon.OfficePickers
{
    internal class SelectableLineWidths
    {
        private float mWidth;

        public float Width
        {
            get { return mWidth; }
            set { mWidth = value; }
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

        public SelectableLineWidths(float width)
        {
            mWidth = width;
        }
    }
}