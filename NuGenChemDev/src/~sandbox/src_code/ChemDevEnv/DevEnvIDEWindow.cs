using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Janus.Windows.Ribbon;
using NuGenSVisualLib.Chem;
using NuGenSVisualLib.Rendering.Chem;
using NuGenSVisualLib.Rendering.Chem.Structures;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Settings;
using Org.OpenScience.CDK.Interfaces;

namespace ChemDevEnv
{
    public partial class DevEnvIDEWindow : Form
    {
        ViewTab currentSelected;
        RecentFiles recentFiles;
        string appDir;
        ICommonDeviceInterface cdi;

        HashTableSettings globalSettings;
        string lastLocation;
        OpenFileDialog openDlg;

        bool devSettings;

        public DevEnvIDEWindow(HashTableSettings gSettings)
        {
            InitializeComponent();

            devSettings = (bool)gSettings["DeveloperMode"];

            appDir = Application.StartupPath + "\\";

            globalSettings = gSettings;

            // load recent files
            if (File.Exists(appDir + ChemDevEnv.Properties.Resources.RecentFilesListLocation))
            {
                recentFiles = RecentFiles.LoadFromFile(ChemDevEnv.Properties.Resources.RecentFilesListLocation);
                recentFiles.ClearDeadEntires();
            }
            else
                recentFiles = new RecentFiles();

            RebuildRecentFilesMenu();

            cdi = ICommonDeviceInterface.NewInterface((byte)globalSettings["CDI.Adapter"], Path.GetFullPath(Application.StartupPath + ConfigurationSettings.AppSettings[(devSettings ? "dev@" : "") + "Base.Path.Relative"]));
        }

        private void RebuildRecentFilesMenu()
        {
            ribbon1.ControlBoxMenu.RightCommands.Clear();

            //int numFilesMax = int.Parse(ChemDevEnv.Properties.Resources.NumberOfRecentFilesShown);
            
            int count = 1;
            for (int file = recentFiles.Files.Count - 1; file >= 0; file--)
            {
                string text = count + " " + PathTool.ShortenPath(recentFiles.Files[file].Filename, 40);
                DropDownCommand item = new DropDownCommand("rencetFile#" + file, text);
                item.Tag = recentFiles.Files[file];
                item.Click += OpenRecentFile;
                ribbon1.ControlBoxMenu.RightCommands.Add(item);

                count++;
            }
        }

        void OpenRecentFile(object sender, CommandEventArgs e)
        {
            OpenMolecule(((RecentFiles.RecentFile)(e.Command.Tag)).Filename);
        }

        private void OpenMolecule(string filename)
        {
            //try
            //{
                LoadingProgressDlg dlg = new LoadingProgressDlg(filename);
                dlg.ShowDialog();
                IChemFileWrapper chemFile = dlg.LoadedChemFile;//MoleculeLoader.LoadFromFile(filename);
                Chem3DControl control = new Chem3DControl();
                ViewTab tab = new ViewTab(control);

                tab.Text = chemFile.filename;
                tab.MdiParent = this;
                tab.Show();

                HashTableSettings settings = new HashTableSettings();
                settings["Base.Path"] = Path.GetFullPath(Application.StartupPath + ConfigurationSettings.AppSettings[(devSettings ? "dev@" : "") + "Base.Path.Relative"]); // /*+ "\\");*/ + "..\\..\\..\\..\\..\\..\\");
                control.Init(settings, cdi);
                control.LoadFile(chemFile);
                control.OnEntitySelected += control_OnEntitySelected;
                control.OnRenderUpdate = onRenderUpdate;

                OnTabChanged();
            //}
            //catch (UserLevelException ule)
            //{
            //    MessageBox.Show(this, ule.Message, "Problem Loading File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}
            //catch { MessageBox.Show(this, "Error loading", "Problem Loading File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; }

            recentFiles.AddFile(filename, RecentFiles.RecentFileType.Molecule);
            RebuildRecentFilesMenu();

            // load outline
            LoadOutline((IChemFile)control.GetRootNode());
        }

        private void LoadOutline(IChemFile root)
        {
            outlineTreeView.Nodes.Clear();
            TreeNode rootNode = new TreeNode(root.GetType().Name + ":" + root.ID);
            BuildNodes(rootNode, root.ChemSequences);
            outlineTreeView.Nodes.Add(rootNode);
        }

        private void BuildNodes(TreeNode rootNode, IChemObject[] objects)
        {
            foreach (IChemObject obj in objects)
            {
                TreeNode node;
                if (obj is IChemSequence)
                {
                    IChemSequence seq = (IChemSequence)obj;
                    node = new TreeNode(obj.GetType().Name + ":" + seq.ID);
                    BuildNodes(node, seq.ChemModels);
                }
                else if (obj is IChemModel)
                {
                    IChemModel mdl = (IChemModel)obj;
                    node = new TreeNode(mdl.GetType().Name + ":" + mdl.ID);
                    if (mdl.Crystal != null)
                        node.Nodes.Add(mdl.Crystal.GetType().Name + ":" + mdl.Crystal.ID);
                    else
                        node.Nodes.Add("ICrystal:null");
                    if (mdl.RingSet != null)
                        node.Nodes.Add(mdl.RingSet.GetType().Name + ":" + mdl.RingSet.ID);
                    else
                        node.Nodes.Add("IRingSet:null");
                    if (mdl.SetOfReactions != null)
                        node.Nodes.Add(mdl.SetOfReactions.GetType().Name + ":" + mdl.SetOfReactions.ID);
                    else
                        node.Nodes.Add("ISetOfReactions:null");
                    BuildNodes(node, mdl.SetOfMolecules.Molecules);
                }
                else if (obj is IMolecule)
                {
                    IMolecule mol = (IMolecule)obj;
                    node = new TreeNode(mol.GetType().Name + ":" + mol.ID);
                    BuildNodes(node, mol.Atoms);
                    BuildNodes(node, mol.Bonds);
                }
                else if (obj is IAtom)
                {
                    IAtom atom = (IAtom)obj;
                    node = new TreeNode(atom.GetType().Name + ":" + atom.ID);
                }
                else if (obj is IBond)
                {
                    IBond bond = (IBond)obj;
                    node = new TreeNode(bond.GetType().Name + ":" + bond.ID);
                }
                else
                    node = new TreeNode(obj.GetType().Name + ":" + obj.ID);

                rootNode.Nodes.Add(node);
            }
        }

        void control_OnEntitySelected(ChemEntity entity)
        {
            object obj = null;
            if (entity.CdkObject is IAtom)
            {
                AtomWrapper atom = new AtomWrapper((IAtom)entity.CdkObject);
                obj = atom;
                ListViewItem[] connectedItems = atom.GetConnectedItemsForListView(currentSelected.ChemControl, uiConnectedList.Groups[0], uiConnectedList.Groups[1]);
                uiConnectedList.Items.Clear();
                if (connectedItems != null)
                    uiConnectedList.Items.AddRange(connectedItems);
            }
            propertyGrid1.SelectedObject = obj;
        }

        private void OpenMolecule()
        {
            if (openDlg == null)
            {
                openDlg = new OpenFileDialog();
                openDlg.Title = "Open Molecule";

                int filterIdx;
                //if (lastLocation == null)
                //    lastLocation = Application.StartupPath;
                //openDlg.InitialDirectory = Environment.CurrentDirectory;
                openDlg.Filter = MoleculeLoader.CreateOpenFilter(out filterIdx, null);
            }
            //dlg.FilterIndex = filterIdx;
            if (openDlg.ShowDialog(this) == DialogResult.OK)
            {
                OpenMolecule(openDlg.FileName);
            }
            //dlg.Dispose();
        }

        private void EditShading()
        {
            ViewTab view = (ViewTab)ActiveMdiChild;
            view.ChemControl.OpenEditShadingDialog();
        }

        private void ShowHideGroups(bool show)
        {
            molInfoGroup.Enabled = show;

            viewBgGroup.Enabled = show;
            viewFontGroup.Enabled = show;
            bgColorContainerControl.Enabled = show;
            viewFontContainerControl.Enabled = show;
            viewFontMiscContainerControl.Enabled = false;

            bgColorContainerControl.Enabled = show;
        }

        #region Recording
        /*private void StopViewRecording()
        {
            if (currentSelected != null)
                currentSelected.ChemControl.StopRecording();
        }

        private void StartViewRecording()
        {
            if (currentSelected != null)
                currentSelected.ChemControl.StartRecording(null);
        }

        private void EditRecordingSettings()
        {
            VideoEncSettings dlg = new VideoEncSettings(RecordingSettings.DefaultsInstance);
            dlg.ShowDialog();
            dlg.Dispose();
        }*/
        #endregion

        #region Events

        void OnTabChanged()
        {
            // load settings if view tab
            if (ActiveMdiChild is ViewTab)
            {
                currentSelected = (ViewTab)ActiveMdiChild;
                HashTableSettings settings = currentSelected.ChemControl.Settings;

                if (settings != null)
                {
                    // set values
                    bgColorButton.SelectedColor = (Color)settings["View3D.BgClr"];

                    label2.Text = currentSelected.Text;
                    label4.Text = currentSelected.ChemControl.NumAtoms.ToString();
                    label6.Text = currentSelected.ChemControl.NumBonds.ToString();
                }

                ShowHideGroups(true);
            }
            else
            {
                // disable groups
                ShowHideGroups(false);
            }
        }

        private void DevEnvIDEWindow_Load(object sender, EventArgs e)
        {
            MdiChildActivate += DevEnvIDEWindow_MdiChildActivate;

            WelcomeForm dlg = new WelcomeForm();
            dlg.MdiParent = this;
            dlg.Text = "Welcome";
            dlg.Show();
        }

        void DevEnvIDEWindow_MdiChildActivate(object sender, EventArgs e)
        {
            OnTabChanged();
        }

        private void DevEnvIDEWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // save recent files
            recentFiles.SaveToFile(appDir + ChemDevEnv.Properties.Resources.RecentFilesListLocation);
        }

        private void newMoleculeCommand_Click(object sender, CommandEventArgs e)
        {
            OpenMolecule();
        }
        
        private void bgColorButton_SelectedColorChanged(object sender, EventArgs e)
        {
            if (currentSelected != null)
            {
                currentSelected.ChemControl.BackColor = bgColorButton.SelectedColor;
                currentSelected.Refresh();
            }
        }

        private void buttonCommand2_Click(object sender, CommandEventArgs e)
        {
            cdi.OpenRenderingInfoDialog();
        }

        private void dropDownCommand3_Click(object sender, CommandEventArgs e)
        {
            Close();
        }

        private void dropDownCommand7_Click(object sender, CommandEventArgs e)
        {
            ActiveMdiChild.Close();
        }

        private void buttonCommand1_Click(object sender, CommandEventArgs e)
        {
            EditShading();
        }

        private void buttonCommand8_Click(object sender, CommandEventArgs e)
        {
            cdi.OpenSupportedFormatsDlg();
        }

        private void dropDownCommand8_Click(object sender, CommandEventArgs e)
        {
            using (OptionsWindow optionsWin = new OptionsWindow(globalSettings))
            {
                optionsWin.ShowDialog(this);
            }
        }
        #endregion

        private void selectButtonCmd_CheckedChanged(object sender, CommandEventArgs e)
        {
            // toggle selection
            currentSelected.ChemControl.SelectMode(selectButtonCmd.Checked);
        }

        private void selectAllButtonCmd_Click(object sender, CommandEventArgs e)
        {
            // select all
            currentSelected.ChemControl.SelectAll();
        }

        private void msrDistBtnCmd_Click(object sender, CommandEventArgs e)
        {
            // measure distance
            currentSelected.ChemControl.MeasureDistance();
        }

        private void uiTakeSSBtnCmd_Click(object sender, CommandEventArgs e)
        {
            // check options
            if (uiSSOutFileBtnCmd.Checked)
            {
                // prompt for filename
            }
            else if (uiSSOutClipBtnCmd.Checked)
            {
                Bitmap img = currentSelected.ChemControl.TakeScreenshotToBitmap();
                Clipboard.SetData(DataFormats.Bitmap, img);
            }
        }

        private void uiLowDetailMvBtnCmd_Click(object sender, CommandEventArgs e)
        {
            // toggle low-detail movement
            currentSelected.ChemControl.LowDetailMovement = uiLowDetailMvBtnCmd.Checked;
        }

        private void onRenderUpdate(double time)
        {
            double timeR = Math.Round(time, 0, MidpointRounding.AwayFromZero);
            uiStatusBar1.Panels[1].Text = "Rendering Time: " + timeR + "ms";
        }

        private void selMoveBtCmd_Click(object sender, CommandEventArgs e)
        {
            // tell control to go into move mode
            currentSelected.ChemControl.ControlMode = ControlMode.SelectionMovement;
        }

        private void selRotBtnCmd_Click(object sender, CommandEventArgs e)
        {
            // tell control to go into rotate mode
            currentSelected.ChemControl.ControlMode = ControlMode.SelectionRotation;
        }

        private void buttonCommand29_Click(object sender, CommandEventArgs e)
        {
            currentSelected.ChemControl.ControlMode = ControlMode.ViewMovement;
        }
    }
}