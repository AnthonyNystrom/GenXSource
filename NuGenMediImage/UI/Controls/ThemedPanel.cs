using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.NuGenMediImage.UI.Controls
{
    public class ThemedPanel : System.Windows.Forms.Panel
    {
        public ThemedPanel()
        {
            this.BackColor = Color.FromArgb(230, 233, 240);
        }

        
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            RibbonControl.RenderSelection(e.Graphics, rect, 2f, true);            
        }        
    }
}
