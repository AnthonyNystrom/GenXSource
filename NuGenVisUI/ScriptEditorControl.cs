using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Genetibase.VisUI.Scripting;
using Janus.Windows.UI.Tab;
using Scintilla;
using Scintilla.Configuration;
using Scintilla.Configuration.SciTE;

namespace Genetibase.VisUI.Controls
{
    public partial class ScriptEditorControl : UserControl
    {
        List<ScriptDocument> openScripts;

        ScintillaConfig config;
        SciTEProperties properties;

        public ScriptEditorControl()
        {
            InitializeComponent();

            openScripts = new List<ScriptDocument>();

            FileInfo globalConfigFile = new FileInfo(@"C:\Users\Andrew\Documents\Visual Studio 2005\Projects\ScintillaNET\Configuration\global.properties");
            properties = new SciTEProperties();
            properties.Load(globalConfigFile);
            config = new ScintillaConfig(properties);
        }

        public ICollection<ScriptDocument> OpenScripts
        {
            get { return openScripts; }
        }

        public ScriptDocument NewDocument()
        {
            UITabPage page = new UITabPage("untitled.boo*");
            ScintillaControl editor = new ScintillaControl();
            editor.SmartIndentingEnabled = true;
            editor.Configuration = config;
            editor.ConfigurationLanguage = "cpp";
            editor.Dock = DockStyle.Fill;

            ScriptDocument doc = new ScriptDocument(editor);
            openScripts.Add(doc);

            page.Controls.Add(editor);
            uiTab1.TabPages.Add(page);

            return doc;
        }

        private void toolStripButton8_Click(object sender, System.EventArgs e)
        {
            // compile script
            listView1.Items.Clear();
            labelCommand1.Text = "Compiling...";
            labelCommand1.Visible = true;
            if (!openScripts[0].Compile())
            {
                if (openScripts[0].Errors != null)
                {
                    foreach (ScriptError error in openScripts[0].Errors)
                    {
                        ListViewItem item = new ListViewItem(new string[] { null, error.Line.ToString(), error.Description }, 1);
                        item.Tag = error;
                        listView1.Items.Add(item);
                    }
                }
                if (openScripts[0].Warnings != null)
                {
                    foreach (ScriptError warning in openScripts[0].Warnings)
                    {
                        ListViewItem item = new ListViewItem(new string[] { null, warning.Line.ToString(), warning.Description }, 0);
                        item.Tag = warning;
                        listView1.Items.Add(item);
                    }
                }
                labelCommand1.Text = string.Format("{0} errors, {1} warnings",
                                                   openScripts[0].Errors == null ? 0 : openScripts[0].Errors.Length,
                                                   openScripts[0].Warnings == null ? 0 : openScripts[0].Warnings.Length);
            }
            else
            {
                labelCommand1.Text = "Compile Successful";
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ScriptError error = (ScriptError)listView1.SelectedItems[0].Tag;
                openScripts[0].GotoLine(error.Line);
            }
        }
    }
}