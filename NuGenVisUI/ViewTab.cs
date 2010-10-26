using System.Drawing;
using System.Windows.Forms;
using Genetibase.VisUI.Controls;

namespace Genetibase.VisUI.Controls
{
    public partial class ViewTab : Form
    {
        protected readonly UI3DControl viewControl;

        public UI3DControl ViewControl
        {
            get { return viewControl; }
        }

        public ViewTab(UI3DControl control)
        {
            InitializeComponent();

            viewControl = control;

            Controls.Add(control);
            control.Dock = DockStyle.Fill;
            control.BackColor = Color.White;
        }
    }
}