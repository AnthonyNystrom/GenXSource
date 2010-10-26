using System.Windows.Forms;
using Genetibase.VisUI.Resources;

namespace NuGenVisUI
{
    public partial class ScriptingHostExplorer : Form
    {
        public ScriptingHostExplorer()
        {
            InitializeComponent();

            scriptEditorControl1.NewDocument();
        }
    }
}