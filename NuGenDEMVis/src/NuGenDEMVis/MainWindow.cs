using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Genetibase.NuGenDEMVis.Controls;
using Genetibase.NuGenDEMVis.Data;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Scene;
using Genetibase.NuGenRenderCore.Settings;
using Genetibase.OSwCommon;
using Genetibase.VisUI.Controls;
using Genetibase.VisUI.Settings;
using Janus.Windows.ButtonBar;
using Janus.Windows.ExplorerBar;
using Janus.Windows.Ribbon;
using Microsoft.DirectX.Direct3D;
using NuGenVisUI;

namespace Genetibase.NuGenDEMVis.UI
{
    /// <summary>
    /// Main UI window
    /// </summary>
    public partial class MainWindow : MdiWindow
    {
        FileType[] fileTypes;
        DataProfile[] dataProfiles;

        string fileFilter;

        VisViewTab currentVis;

        public MainWindow(HashTableSettings gSettings, bool devMode)
        {
            InitializeComponent();

            if (gSettings == null)
                throw new Exception("No global settings loaded");
                //gSettings = HashTableSettings.LoadFromXml(NuGenDEMVis.Properties.Resource1.DefaultGlobalSettings, false);

            gSettings["DeveloperMode"] = devMode;

            Init(gSettings, ribbon1);

            LoadFileTypes();
            LoadProfiles();

            Text += " [" + gSettings["Assembly.Version"] + "]";

            // TODO: proper measurements and cap size?
            //explorerBarContainerControl3.Height = 64 * 4;
        }

        private void LoadProfiles()
        {
            dataProfiles = new DataProfile[] { new ClrIntensityDataProfile(fileTypes), new GISValueDataProfile(fileTypes) };
        }

        private void LoadFileTypes()
        {
            // parse all file types
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(NuGenDEMVis.Properties.Resource1.FileTypes);
            XmlNodeList fileTypeNodes = xml.SelectNodes("fileTypes/fileType");
            
            fileTypes = new FileType[fileTypeNodes.Count];
            for (int i = 0; i < fileTypes.Length; i++)
            {
                string[] exts = new string[fileTypeNodes[i].ChildNodes.Count];
                for (int e = 0; e < fileTypeNodes[i].ChildNodes.Count; e++)
                {
                    exts[e] = fileTypeNodes[i].ChildNodes[e].InnerText;
                }
                fileTypes[i] = new FileType(fileTypeNodes[i].Attributes["name"].InnerText, exts);
            }

            fileFilter = FileFilter.MakeFilterSortedByName(fileTypes, true, true, true);
        }

        protected override void OpenRecentFile(string file)
        {

        }

        protected override void OnTabChanged()
        {
            base.OnTabChanged();
            currentVis = (VisViewTab)currentSelected;

            // change settings / view
            if (currentVis != null)
            {
                // change data sources
                ButtonBarItem[] items = currentVis.VisViewControl.GetHeightSources();
                if (items != null)
                {
                    foreach (ButtonBarItem item in items)
                    {
                        AddToButtonBar(uiDSHMapBar, explorerBarContainerControl3, item);
                    }
                }
                items = currentVis.VisViewControl.GetDiffuseSources();
                if (items != null)
                {
                    foreach (ButtonBarItem item in items)
                    {
                        AddToButtonBar(uiDsDiffuseBar, explorerBarContainerControl4, item);
                    }
                }

                // change layers
                foreach (GeometryVisLayer layer in currentVis.VisViewControl.GeometryLayers)
                {
                    ButtonBarItem item = new ButtonBarItem(layer.Name);
                    item.Image = layer.Preview;
                    AddToButtonBar(ui3DLayersBar, explorerBarContainerControl1, item);
                }

                // update scene entity tree
                treeView1.Nodes.Clear();
                TreeNode sceneRoot = new TreeNode("Scene");
                
                SceneEntity[] entities = currentVis.VisViewControl.GetSceneEntities();
                foreach (SceneEntity entity in entities)
                {
                    sceneRoot.Nodes.Add(entity.GetType().ToString());
                }
                treeView1.Nodes.Add(sceneRoot);
            }
        }

        protected override void ShowHideGroups(bool show)
        {
            // explorere bars on panels
            uiDSrcExplorerBar.Enabled = show;
            uiLayersExplorerBar.Enabled = show;
            
            // content holders
            ClearButtonBar(ui3DLayersBar, explorerBarContainerControl1);
            ClearButtonBar(uiDSHMapBar, explorerBarContainerControl3);
            ClearButtonBar(uiDsDiffuseBar, explorerBarContainerControl4);

            // ribbon tabs
            ribbonTab1.Enabled = show;
            ribbonTab2.Enabled = show;

            //ribbonTab3.Enabled = show;
            ribbonGroup4.Enabled = show;
            ribbonGroup5.Enabled = show;
            ribbonGroup10.Enabled = show;

            ribbonTab4.Enabled = show;
        }

        private static void ClearButtonBar(ButtonBar buttonBar, ExplorerBarContainerControl containerControl)
        {
            buttonBar.Groups[0].Items.Clear();
            containerControl.Height = 64;
        }

        private static void AddToButtonBar(ButtonBar bar, ExplorerBarContainerControl container, ButtonBarItem item)
        {
            bar.Groups[0].Items.Add(item);
            container.Height = bar.Groups[0].Items.Count * 95;
        }

        private void uiNewVisFromImg_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            NewVisualizationFromImg();
        }

        private void NewVisualizationFromImg()
        {
            // open data sourcing dialog
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Title = "Open Image";
            openDlg.Filter = fileFilter;
            openDlg.InitialDirectory = baseDir;
            if (openDlg.ShowDialog(this) == DialogResult.OK)
            {
                NewVisDlg dlg = new NewVisDlg(openDlg.FileName, fileTypes, dataProfiles);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // create visualization
                    UI3DVisControl control = new UI3DVisControl();
                    VisViewTab tab = new VisViewTab(control);

                    control.Title = tab.Text = dlg.VisName;
                    tab.MdiParent = this;
                    tab.Show();

                    HashTableSettings localSettings = new HashTableSettings();
                    //localSettings["Base.Path"] = baseDir;
                    localSettings["GeometryVis.HeightShadingEnabled"] = false;
                    localSettings["GeometryVis.HeightShadingClr"] = Color.Red;
                    control.Init(localSettings, cdi);

                    OnTabChanged();

                    control.LoadVisualization(dlg.VisDataProfile, dlg.VisSubDataProfile, dlg.VisDataReader,
                                              dlg.VisDataSourceInfo);

                    OnTabChanged();

                    dlg.VisDataReader = null;

                    recentFiles.AddFile(openDlg.FileName, RecentFiles.RecentFileType.Molecule);
                    RebuildRecentFilesMenu();
                }
                dlg.Dispose();
            }
        }

        private void renderingStyleChanged(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            if (sender == buttonCommand14)
            {
                // points
                ((VisViewTab)currentSelected).ChangeRenderingFillMode(FillMode.Point);
            }
            else if (sender == buttonCommand15)
            {
                // wireframe
                ((VisViewTab)currentSelected).ChangeRenderingFillMode(FillMode.WireFrame);
            }
            else if (sender == buttonCommand16)
            {
                // solid
                ((VisViewTab)currentSelected).ChangeRenderingFillMode(FillMode.Solid);
            }
        }

        private void uiDsDiffuseBar_DoubleClick(object sender, System.EventArgs e)
        {
            // attempt to change diffuse source
            currentVis.ChangeDiffuseSource(uiDsDiffuseBar.SelectedItem.Index);
        }

        private void buttonCommand19_CheckedChanged(object sender, CommandEventArgs e)
        {
            UI3DVisControl.Axis axis = UI3DVisControl.Axis.X;
            if (sender == buttonCommand19)
                axis = UI3DVisControl.Axis.X;
            else if (sender == buttonCommand20)
                axis = UI3DVisControl.Axis.Y;
            else if (sender == buttonCommand21)
                axis = UI3DVisControl.Axis.Z;
            currentVis.ToggleAxis(axis, ((ButtonCommand)sender).Checked);
        }

        #region Device Commands

        private void buttonCommand22_Click(object sender, CommandEventArgs e)
        {
            // show info
            GraphicsDeviceInfoDlg dlg = new GraphicsDeviceInfoDlg((CommonDeviceInterface)cdi);
            dlg.ShowDialog(this);
        }

        private void buttonCommand23_Click(object sender, CommandEventArgs e)
        {
            // show profile
            GraphicsProfileInfoDlg dlg = new GraphicsProfileInfoDlg(currentSelected.ViewControl.CurrentProfile);
            dlg.ShowDialog(this);
        }

        private void buttonCommand24_Click(object sender, CommandEventArgs e)
        {
            // show stats
        }
        #endregion

        private void enableDisableToolStripMenuItem_CheckedChanged(object sender, System.EventArgs e)
        {
            // toggle a layer
            if (ui3DLayersBar.SelectedItem != null)
            {
                int index = ui3DLayersBar.SelectedItem.Index;
                currentVis.VisViewControl.GeometryLayers[index].Enabled = enableDisableToolStripMenuItem.Checked;
                currentVis.Invalidate();
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // load correct settings for this layer
            if (ui3DLayersBar.SelectedItem != null)
            {
                int index = ui3DLayersBar.SelectedItem.Index;
                enableDisableToolStripMenuItem.Checked = currentVis.VisViewControl.GeometryLayers[index].Enabled;
            }
        }

        private void buttonCommand13_Click(object sender, CommandEventArgs e)
        {
            // take a screenshot
            ScreenshotDlg dlg = new ScreenshotDlg(currentVis.VisViewControl);
            dlg.ShowDialog(this);
        }

        private void buttonCommand33_CheckedChanged(object sender, CommandEventArgs e)
        {
            // toggle height shading
            currentVis.VisViewControl.HeightShadingEnabled = buttonCommand33.Checked;
            currentVis.Invalidate();
        }

        private void uiColorButton1_SelectedColorChanged(object sender, System.EventArgs e)
        {
            // change height shading colour
            currentVis.VisViewControl.HeightShadingColor = uiColorButton1.SelectedColor;
            currentVis.Invalidate();
        }

        private void uiTrackBar1_ValueChanged(object sender, System.EventArgs e)
        {
            // change height shading colour
            currentVis.VisViewControl.HeightShadingColor = HSL_to_RGB(uiTrackBar1.Value / 255f, 240f / 255f, 120f / 255f);
            currentVis.Invalidate();
        }

        public static Color HSL_to_RGB(float h, float s, float l)
        {
            double r = 0, g = 0, b = 0;

            if (l == 0)
            {
                r = g = b = 0;
            }
            else
            {
                if (s == 0)
                    r = g = b = l;
                else
                {
                    double temp2 = ((l <= 0.5) ? l * (1.0 + s) : l + s - (l * s));
                    double temp1 = 2.0 * l - temp2;

                    double[] t3 = new double[] { h + 1.0 / 3.0, h, h - 1.0 / 3.0 };
                    double[] clr = new double[] { 0, 0, 0 };
                    for (int i = 0; i < 3; i++)
                    {
                        if (t3[i] < 0)
                            t3[i] += 1.0;

                        if (t3[i] > 1)
                            t3[i] -= 1.0;

                        if (6.0 * t3[i] < 1.0)
                            clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0;
                        else if (2.0 * t3[i] < 1.0)
                            clr[i] = temp2;
                        else if (3.0 * t3[i] < 2.0)
                            clr[i] = (temp1 + (temp2 - temp1) * ((2.0 / 3.0) - t3[i]) * 6.0);
                        else
                            clr[i] = temp1;
                    }
                    r = clr[0];
                    g = clr[1];
                    b = clr[2];
                }
            }
            return Color.FromArgb((int)(255 * r), (int)(255 * g), (int)(255 * b));
        }

        private void buttonCommand36_Click(object sender, CommandEventArgs e)
        {
            // new height measurement entity
            currentVis.VisViewControl.NewHeightMeasurer();
        }

        private void dropDownCommand5_Click(object sender, CommandEventArgs e)
        {
            Close();
        }

        private void textBox3_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void buttonCommand45_Click(object sender, CommandEventArgs e)
        {
            // pointer mode
        }

        private void buttonCommand46_Click(object sender, CommandEventArgs e)
        {
            // selection mode
        }

        private void buttonCommand47_Click(object sender, CommandEventArgs e)
        {
            // manipulation mode
        }

        private void buttonCommand5_Click(object sender, CommandEventArgs e)
        {
            // zoom to fit

        }
    }
}