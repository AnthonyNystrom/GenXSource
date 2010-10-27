using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing.Drawing2D;
using System.Configuration;
using Netron.Neon.WinFormsUI;
namespace Netron.Cobalt
{
    /// <summary>
    /// Main class or form of the application.
    /// </summary>
    public partial class MainForm : Form
    {

        #region Constructor and initialization
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            DockPanel.Parent = null;
            DockPanel.Parent = stripContainer.ContentPanel;
            //set the custom Netron extender for the docking environment
            Netron.Neon.WinFormsUI.Netron.Extender.SetExtender(DockPanel);
            //disply the current version in the form's caption
            this.Text = "Cobalt.IDE [Netron Library v" + AssemblyInfo.AssemblyVersion + "]";
            try
            {
                Application.DockPanel = this.DockPanel;
                Application.MainForm = this;
                //set to the default welcome state
                Workbenches.WelcomeBench();

                

                // Get the application configuration file.
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                ConfigurationSectionGroup group= config.GetSectionGroup("Addins");
                Directory.SetCurrentDirectory(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath));
                foreach (ConfigurationSection section in group.Sections)
                {
                    Trace.WriteLine("Found addin definition:" + (section as AddinSection).FileName);
                    string path = (section as AddinSection).FileName;
                    path = Path.GetFullPath(path);
                    Assembly tass = Assembly.LoadFile(path);
                    if (tass != null)
                    {
                        IAddin obj = tass.CreateInstance((section as AddinSection).Type) as IAddin;
                        Application.Addins.Extensions.Add(obj);
                    }
                }
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.Message);
            }

        }

        

        /// <summary>
        /// Just a loop over assemblies and some reflection
        /// </summary>
        private void ScanForAddinsTest()
        {
            string[] files = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "*.dll");
            Assembly tass = null;
            for (int i = 0; i < files.Length; i++)
            {

                try
                {
                    tass = Assembly.ReflectionOnlyLoadFrom(files[i]);
                    if (tass != null)
                    {
                        foreach (Type type in tass.GetTypes())
                        {
                            if (type.IsClass == true)
                            {
                                if (type.Name.Equals("CobaltAddin", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Application.Output.WriteLine("Found addin in assembly: '" + files[i] + "'");
                                }
                            }
                        }
                    }


                }
                catch (Exception exc)
                {

                    continue;
                }

            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //Bundle bundle = Netron.Diagramming.Core.Layout.Tree.CreateSpecificTree();
            //Application.Diagram.Unwrap(bundle);



        }

        #endregion

        #region 'File' menu item

        /// <summary>
        /// Handles the Click event of the exitMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        #endregion

        #region 'View' menu item

        /// <summary>
        /// Handles the Click event of the diagramToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void diagramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Tabs.Diagram.Show();
        }

        /// <summary>
        /// Handles the Click event of the shapesToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void shapesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Tabs.Shapes.Show();
        }

        /// <summary>
        /// Handles the Click event of the propertiesToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Tabs.Property.Show();
        }

        /// <summary>
        /// Handles the Click event of the browserToolStripMenuItem1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void browserToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //rightTabControl.SelectedTab = tabWeb;
            Application.Tabs.WebBrowser.Show();
        }
        /// <summary>
        /// Handles the Click event of the applicationSettingsMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void applicationSettingsMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationSettings settings = new ApplicationSettings(this);
            settings.ShowDialog(this);
        }
        private void outputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Tabs.Output.Show();
        }
        #endregion

        #region 'Help' menu
        /// <summary>
        /// Handles the Click event of the aboutToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAbout();
        }
        /// <summary>
        /// Handles the Click event of the classDocumentationToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void classDocumentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.WebBrowser.Navigate("doc://API/index.html");
        }
        /// <summary>
        /// Handles the Click event of the netronEULAToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void netronEULAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowEula();
        }
        /// <summary>
        /// Shows the about-info.
        /// </summary>
        private void ShowAbout()
        {
            try
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Cobalt.Resources.AboutTemplate.htm");
                if (stream != null)
                {
                    StreamReader reader = new StreamReader(stream);
                    string template = reader.ReadToEnd();
                    template = template.Replace("$title$", AssemblyInfo.AssemblyTitle);
                    template = template.Replace("$version$", AssemblyInfo.AssemblyVersion);
                    template = template.Replace("$company$", AssemblyInfo.AssemblyCompany);
                    template = template.Replace("$description$", AssemblyInfo.AssemblyDescription);

                    reader.Close();
                    stream.Close();
                    Application.ShowContent(template);

                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Title: {0}", AssemblyInfo.AssemblyTitle);
                    sb.Append("<br/>");
                    sb.AppendFormat("Version: {0}", AssemblyInfo.AssemblyVersion);
                    sb.Append("<br/>");
                    sb.AppendFormat("Company: {0}", AssemblyInfo.AssemblyCompany);
                    sb.Append("<br/>");
                    sb.AppendFormat("Description: {0}", AssemblyInfo.AssemblyDescription);

                    Application.ShowContent(sb.ToString());
                }
            }
            catch
            {
                Application.ShowContent("Unable to fetch the assembly information.");
            }

        }
        /// <summary>
        /// Shows the EULA.
        /// </summary>
        private void ShowEula()
        {
            string lic = string.Empty;

            try
            {
                System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Cobalt.Resources.Netron EULA.htm");
                if (stream != null)
                {
                    StreamReader reader = new System.IO.StreamReader(stream);
                    lic = reader.ReadToEnd();
                    Application.ShowContent(lic);
                    reader.Close();
                    stream.Close();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 'Undo/redo' menu items

        /// <summary>
        /// Handles the Click event of the undoButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void undoButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.Undo();
        }

        /// <summary>
        /// Handles the Click event of the redoButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void redoButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.Redo();
        }


        #endregion

        #region Main menu switching when the DockPanel's ActiveDocumentChanged is raised
        /// <summary>
        /// Handles the ActiveDocumentChanged event of the dockPanel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void dockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (DockPanel.ActiveDocument == Application.Tabs.WebBrowser)
                Application.Menus.BrowserMainMenu.Visible = true;
            else
                Application.Menus.BrowserMainMenu.Visible = false;

            if (DockPanel.ActiveDocument == Application.Tabs.Diagram)
            {
                Application.Menus.EditMainMenu.Visible = true;
                Application.Menus.DiagramMainMenu.Visible = true;
            }
            else
            {
                Application.Menus.EditMainMenu.Visible = false;
                Application.Menus.DiagramMainMenu.Visible = false;
            }

        }
        #endregion

        #region Utilities
        /// <summary>
        /// Handles the Tick event of the statusTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void statusTimer_Tick(object sender, EventArgs e)
        {
            StatusTimer.Stop();
            this.InfoStatusLabel.Visible = false;
        }
        #endregion



    }


}