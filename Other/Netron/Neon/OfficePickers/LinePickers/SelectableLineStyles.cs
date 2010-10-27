using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Netron.Neon.OfficePickers
{
    internal class SelectableLineStyles
    {
        private DashStyle mStyle;

        public DashStyle LineStyle
        {
            get { return mStyle; }
            set { mStyle = value; }
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

        public SelectableLineStyles(DashStyle style)
        {
            mStyle = style;
        }
    }
}