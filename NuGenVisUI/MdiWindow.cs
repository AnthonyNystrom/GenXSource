using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Settings;
using Genetibase.VisUI.Common;
using Genetibase.VisUI.Resources;
using Genetibase.VisUI.Settings;
using Janus.Windows.Ribbon;

namespace Genetibase.VisUI.Controls
{
    /// <summary>
    /// Encapsulates a generic MDI Window.
    /// This is the UI base for a few Genetibase projects
    /// </summary>
    public partial class MdiWindow : Form
    {
        protected string appDir, baseDir;
        protected bool devSettings;
        protected HashTableSettings globalSettings;

        protected RecentFiles recentFiles;

        Ribbon ribbon;
        protected ViewTab currentSelected;

        protected ICommonDeviceInterface cdi;

        public void Init(HashTableSettings gSettings, Ribbon mainRibbon)
        {
            ribbon = mainRibbon;

            devSettings = (bool)gSettings["DeveloperMode"];

            appDir = Application.StartupPath + "\\";
            globalSettings = gSettings;

            // load recent files
            if (File.Exists(appDir + "recentFiles.xml"))
            {
                recentFiles = RecentFiles.LoadFromFile(appDir + "recentFiles.xml");
                recentFiles.ClearDeadEntires();
            }
            else
                recentFiles = new RecentFiles();

            RebuildRecentFilesMenu();

            baseDir = /*(string)gSettings["Base.Path"];*/Path.GetFullPath(ConfigurationManager.AppSettings[(devSettings ? "dev@" : "") + "Base.Path.Relative"].Replace("%STARTUP%", Application.StartupPath));
            if (!Directory.Exists(baseDir))
                throw new ApplicationException("Base directory does not exist! : " + baseDir);

            cdi = ICommonDeviceInterface.NewInterface((byte)globalSettings["CDI.Adapter"], baseDir);
            cdi.ResourceLoader.RegisterContentLoader(new LayerContentLoader());
            cdi.ResourceLoader.RegisterContentLoader(new BooScriptContentLoader());

            ShowHideGroups(false);
        }

        protected void RebuildRecentFilesMenu()
        {
            ribbon.ControlBoxMenu.RightCommands.Clear();

            //int numFilesMax = int.Parse(ChemDevEnv.Properties.Resources.NumberOfRecentFilesShown);

            int count = 1;
            for (int file = recentFiles.Files.Count - 1; file >= 0; file--)
            {
                string text = count + " " + PathTools.ShortenPath(recentFiles.Files[file].Filename, 40);
                DropDownCommand item = new DropDownCommand("rencetFile#" + file, text);
                item.Tag = recentFiles.Files[file];
                item.Click += OpenRecentFile;
                ribbon.ControlBoxMenu.RightCommands.Add(item);

                count++;
            }
        }

        private void OpenRecentFile(object sender, CommandEventArgs e)
        {
            OpenRecentFile(((RecentFiles.RecentFile)(e.Command.Tag)).Filename);
        }

        protected virtual void OpenRecentFile(string file) { }
        protected virtual void ShowHideGroups(bool show) { }

        protected virtual void OnTabChanged()
        {
            // load settings if view tab
            if (ActiveMdiChild is ViewTab)
            {
                currentSelected = (ViewTab)ActiveMdiChild;
                ShowHideGroups(true);
            }
            else
            {
                currentSelected = null;
                // disable groups
                ShowHideGroups(false);
            }
        }

        private void MdiWindow_MdiChildActivate(object sender, EventArgs e)
        {
            OnTabChanged();
        }
    }
}
