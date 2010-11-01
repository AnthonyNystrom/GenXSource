using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace NuGenInSightCompilation.Testing
{
    class TestComponent
    {
        public static void ShowControl(Control c)
        {
            Form f = new Form();
            f.Size = new Size(800, 600);
            f.Controls.Add(c);
            f.Text = c.Name;
            f.Show();
        }
    }
}
