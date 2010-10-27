using System;
using System.Collections.Generic;
using System.Text;

namespace Next2Friends.ImageWorks.UI.NuGenImageWorks
{
    public class SlideChangedEventArgs : EventArgs
    {
        private bool visible;

        public bool Visible
        {
          get { return visible; }
          set { visible = value; }
        }

        public SlideChangedEventArgs(bool visible)
        {
            this.visible = visible;
        }
    }
}