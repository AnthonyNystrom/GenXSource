using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Genetibase.NuGenMediImage.UI.Controls
{
    class NoScrollToControlPanel:System.Windows.Forms.Panel
    {
        Point scrolledTo = new Point(0,0);

        protected override void OnScroll(System.Windows.Forms.ScrollEventArgs se)
        {
            scrolledTo.X = this.DisplayRectangle.X;
            scrolledTo.Y = this.DisplayRectangle.Y;
        }

        protected override System.Drawing.Point ScrollToControl(System.Windows.Forms.Control activeControl)
        {
            return scrolledTo;
        }
    }
}
