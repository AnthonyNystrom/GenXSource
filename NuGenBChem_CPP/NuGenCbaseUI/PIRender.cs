using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

using NGVChem;
namespace NGVChem
{
    public delegate void PropertyChangeEvent(CPIProperty propertyEntity,object propertyObj);
    public partial class PIRender : UserControl
    {
        private ProteinUIInterface dllInterface = new ProteinUIInterface();
        private Panel residuesPanel;
        private Panel selectPanel;
        private Panel pdbTreePanel;
        
        public event PropertyChangeEvent OnPropertyChange;
        public PIRender()
        {
            InitializeComponent();
            OnPropertyChange += new PropertyChangeEvent(delegate(CPIProperty propertyEntity, object propertyObj)
            {
               
            });
        }
        protected override void Dispose(bool disposing)
        {
            try
            {
                dllInterface.CleanResource();
                dllInterface.Dispose();
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
            catch (System.Exception e)
            {
            	 
            }
           
        }
 
        private void PIRender_Load(object sender, EventArgs e)
        {
            IntPtr hWnd = this.Handle;

            Module appModule = Assembly.GetExecutingAssembly().GetModules()[0];
            IntPtr hInstance = System.Runtime.InteropServices.Marshal.GetHINSTANCE(appModule);
            dllInterface.InitUI(hInstance,hWnd);

            dllInterface.OnPropertyChange += new PDBPropertChange(delegate(CPIProperty propertyEntity) 
             {
                 this.OnPropertyChange(propertyEntity, propertyEntity); 
             });

            GetSelectPanel();
            GetResiduesPanel();
            GetPDBTreePanel();
            this.DoubleBuffered = false;
 
        }
 
        protected override void DefWndProc(ref Message m)
        {
            base.DefWndProc(ref m);
            dllInterface.WndProcess(ref m);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            dllInterface.OnPaint();
        }
 
         
        /// <summary>
        /// Open PDB embedded  dialog.
        /// </summary>
        public void OpenPDB()
        {
            //dllInterface.OpenPDBFile();
            FileInfo fileInfo = new FileInfo(Application.ExecutablePath);
            PDBOpenDialogControl dlg = new PDBOpenDialogControl();
            dlg.FileDlgFilter = "PDB Files (*.ent;*.pdb)|*.ent; *.pdb|All Files (*.*)|*.*";
            dlg.FileDlgInitialDirectory = fileInfo.Directory.FullName + "\\DownloadPDB";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.dllInterface.OpenPDBFile(dlg.FileDlgFileNames);
            }
            else if (dlg.DownloadPDBList.Count > 0)
            {
                this.dllInterface.OpenPDBFile(dlg.DownloadPDBList.ToArray());
            }
             
        }
        /// <summary>
        /// Open PDB file
        /// </summary>
        /// <param name="fileName">pdb file name</param>
        public void OpenPDB(String fileName)
        {
            dllInterface.OpenPDBFile(fileName);
        }

        /// <summary>
        /// Open workspace.(embedded dialog)
        /// </summary>
        public void OpenWorkSpace()
        {
            //dllInterface.OpenWorkspace();
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Workspace file(*.piw)|*.piw";
            dlg.Multiselect= false;

            FileInfo fileInfo = new FileInfo(Application.ExecutablePath);
            dlg.InitialDirectory = fileInfo.Directory.FullName + "\\Workspace";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                OpenPDB(dlg.FileName);
            }
        }

        /// <summary>
        /// Open workspace file name
        /// </summary>
        /// <param name="workSpaceName">work space file name</param>
        public void OpenWorkSpace(String workSpaceName)
        {
            dllInterface.OpenWorkspace(workSpaceName);
        }
        
        /// <summary>
        /// Save work space.
        /// </summary>
        /// <param name="workSpaceName"></param>
        public void SaveWorkSpace(String workSpaceName)
        {
            dllInterface.OnSaveWorkspace(workSpaceName);
        }
        public void SaveWorkSpace()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Workspace file(*.piw)|*.piw";
            FileInfo fileInfo = new FileInfo(Application.ExecutablePath);
            dlg.InitialDirectory = fileInfo.Directory.FullName + "\\Workspace";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SaveWorkSpace(dlg.FileName);
            }
        }

        /// <summary>
        /// Close opened pdbs .
        /// </summary>
        public void OnClosePdb()
        {
            this.dllInterface.OnClosePdb();
        }

        /// <summary>
        /// Add a pdb to current pdb.
        /// </summary>
        public void OnAddPdb()
        {
            //this.dllInterface.OnAddPdb();
            PDBOpenDialogControl dlg = new PDBOpenDialogControl();
            dlg.FileDlgFilter = "PDB Files (*.ent;*.pdb)|*.ent; *.pdb|All Files (*.*)|*.*";

            FileInfo fileInfo = new FileInfo(Application.ExecutablePath);
            dlg.FileDlgInitialDirectory = fileInfo.Directory.FullName + "\\DownloadPDB";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.dllInterface.AddPDB(dlg.FileDlgFileNames);
            }
            else if (dlg.DownloadPDBList.Count > 0)
            {
                this.dllInterface.AddPDB(dlg.DownloadPDBList.ToArray());
            }
        }

        /// <summary>
        /// Set output callback method.
        /// </summary>
        /// <param name="outputMethod"></param>
        public void SetOutPutDelegate(OutputMethod outputMethod)
        {
            dllInterface.SetOutputDelegate(outputMethod);
        }
        
        /// <summary>
        /// Set get header information callback method.
        /// </summary>
        /// <param name="outputMethod"></param>
        public void SetHeaderInfoDelegate(GetHeaderInfoMethod outputMethod)
        {
            dllInterface.SetHeaderInfoDelegate(outputMethod);
        }

        /// <summary>
        /// Set Init progress callback method.
        /// </summary>
        /// <param name="outputMethod"></param>
        public void SetInitProgressDelegate(ProgressMethod outputMethod)
        {
            dllInterface.SetInitProgressDelegate(outputMethod);
        }

        /// <summary>
        /// Set end progress callback method.
        /// </summary>
        /// <param name="outputMethod"></param>
        public void SetEndProgressDelegate(ProgressMethod outputMethod)
        {
            dllInterface.SetEndProgressDelegate(outputMethod);
        }
       
        /// <summary>
        /// Set reset progress callback method.
        /// </summary>
        /// <param name="outputMethod"></param>
        public void SetResetProgressDelegate(ProgressMethod outputMethod)
        {
            dllInterface.SetResetProgressDelegate(outputMethod);
        }
        
        /// <summary>
        /// Set output progress callback method.
        /// </summary>
        /// <param name="outputMethod"></param>
        public void SetProgressDelegate(CurrentProgressValue outputMethod)
        {
            dllInterface.SetSetProgressDelegate(outputMethod);
        }
 
        /// <summary>
        /// Get PDB list combox control
        /// </summary>
        /// <returns></returns>
        public ComboBox GetCombox()
        {
            return this.dllInterface.GetPDBCombox();
        }

        /// <summary>
        /// Get Residues panel.
        /// </summary>
        /// <returns></returns>
        public Panel GetResiduesPanel()
        {
            if(residuesPanel==null)
            {
                residuesPanel = new Panel();
                residuesPanel.Dock = DockStyle.Fill;
                dllInterface.CreateResiduesPanel(residuesPanel.Handle);
                residuesPanel.SizeChanged += new EventHandler(delegate(object sender, EventArgs e) {
                    dllInterface.ResiduesSizeChange(residuesPanel.Width, residuesPanel.Height);
                });
            }
            return residuesPanel;
        }

        /// <summary>
        /// Get PDB Tree Panel
        /// </summary>
        /// <returns></returns>
        public Panel GetPDBTreePanel()
        {
            if (pdbTreePanel == null)
            {
                pdbTreePanel = new Panel();
                pdbTreePanel.Dock = DockStyle.Fill;
                dllInterface.CreatePDBTreePanel(pdbTreePanel.Handle);
                pdbTreePanel.SizeChanged += new EventHandler(delegate(object sender, EventArgs e)
                {
                    dllInterface.PDBTreePanelSizeChange(pdbTreePanel.Width, pdbTreePanel.Height);
                });
            }
            return pdbTreePanel;
        }

        /// <summary>
        /// Get Selection Panel
        /// </summary>
        /// <returns></returns>
        public Panel GetSelectPanel()
        {
            if (selectPanel == null)
            {
                selectPanel = new Panel();
                selectPanel.Dock = DockStyle.Fill;
                dllInterface.CreateSelectPanel(selectPanel.Handle);
                selectPanel.SizeChanged += new EventHandler(delegate(object sender, EventArgs e)
                {
                    dllInterface.SelectPanelSizeChange(selectPanel.Width, selectPanel.Height);
                });
            }
            return selectPanel;
        }

        /// <summary>
        /// Get Current property object.
        /// </summary>
        /// <returns></returns>
        public Object GetProperty()
        {
            return dllInterface.GetPDBProperty();
        }

        /// <summary>
        /// Set property value.
        /// </summary>
        public void ChangePropertyValue(Object entity,String propertyName,object value)
        {
           dllInterface.SetPropertyValue(entity, propertyName, value);
        }

        /// <summary>
        /// Screen shot
        /// </summary>
        public void OnFileScreenshot()
        {
            dllInterface.OnFileScreenshot();
        }
 
 
        #region Others
        public void OnButtonGoFirst()
        {
            this.dllInterface.OnButtonGoFirst();
        }

        public void OnButtonGoLast()
        {
            this.dllInterface.OnButtonGoLast();
        }

        public void OnButtonNextFrame()
        {
            this.dllInterface.OnButtonNextFrame();
        }

        public void OnButtonPlay()
        {
            this.dllInterface.OnButtonPlay();
        }

        public void OnButtonPlayFast()
        {
            this.dllInterface.OnButtonPlayFast();
        }

        public void OnButtonPlaySlow()
        {
            this.dllInterface.OnButtonPlaySlow();
        }

        public void OnButtonPrevFrame()
        {
            this.dllInterface.OnButtonPrevFrame();
        }
        public void OnButtonStop()
        {
            this.dllInterface.OnButtonStop();
        }

        public void OnNextActivePDB()
        {
            this.dllInterface.OnNextActivePDB();
        }

        public void OnNextSelectionList()
        {
            this.dllInterface.OnNextSelectionList();
        }

        public void OnPrevActivePDB()
        {
            this.dllInterface.OnPrevActivePDB();
        }

        public void OnPrevSelectionList()
        {
            this.dllInterface.OnPrevSelectionList();
        }

        public void OnActivePDB()
        {
            dllInterface.OnActivePDB();
        }
        public void OnUnion()
        {
            dllInterface.OnUnion();
        }
        public void OnIntersect()
        {
            dllInterface.OnIntersect();
        }
        public void OnSubtract()
        {
            dllInterface.OnSubtract();
        }
        public void OnOperatResult()
        {
            dllInterface.OnOperatResult();
        }
        public void OnSaveSelection()
        {
            dllInterface.OnSaveSelection();
        }
        public void OnDisplayResiduesSelected()
        {
            dllInterface.OnDisplayResiduesSelected();
        }
        #endregion
        
        #region ToolBar
        /// <summary>
        /// Set transform of each Bio-unit
        /// If it is pressend, all Bio-unit act as one molecule,
        /// If not, each biounit has each transform
        /// </summary>
        public void OnAttatchBiounit()
        {
            this.dllInterface.OnAttatchBiounit();
        }

        /// <summary>
        /// Add SpaceFill Display.
        /// Add current selection to new VP with SpaceFill display method,
        /// In Script, this value is IDisplayStyle.SpaceFill
        /// </summary>
        public void OnButtonBall()
        {
            this.dllInterface.OnButtonBall();
        }

        /// <summary>
        /// Add Stick Display
        /// Add current selection to new VP with Stick display method
        /// In Script, this value is IDisplayStyle.BallnStick
        /// </summary>
        public void OnButtonBallStick()
        {
            this.dllInterface.OnButtonBallStick();
        }

        /// <summary>
        /// Add Surface Display
        /// Add current selection to new VP with Surface display method
        /// </summary>
        public void OnButtonDotsurface()
        {
            this.dllInterface.OnButtonDotsurface();
        }

        /// <summary>
        /// Add Surface Display
        /// </summary>
        /// <param name="id">1-10</param>
        public void OnButtonDotsurfaceWithResolution(uint id)
        {
            this.dllInterface.OnButtonDotsurfaceWithResolution(id);
        }

        public void OnFlagMoleculeSelectionCenter()
        {
            this.dllInterface.OnFlagMoleculeSelectionCenter();
        }

        /// <summary>
        /// Add Ribbon Display
        /// Add current selection to new VP with Ribbon display method
        /// </summary>
        public void OnButtonRibbon()
        {
            this.dllInterface.OnButtonRibbon();
        }

        /// <summary>
        /// Add Stick Display
        /// Add current selection to new VP with Stick display method
        /// </summary>
        public void OnButtonStick()
        {
            this.dllInterface.OnButtonStick();
        }

        /// <summary>
        /// Add Wireframe Display
        /// Add current selection to new VP with Wireframe display method
        /// </summary>
        public void OnButtonWireframe()
        {
            this.dllInterface.OnButtonWireframe();
        }

        /// <summary>
        /// Move Center
        /// </summary>
        public void OnCenterMolecule()
        {
            this.dllInterface.OnCenterMolecule();
        }

        /// <summary>
        /// Visualize Bio-unit
        /// </summary>
        public void OnDisplayBioUnit()
        {
            this.dllInterface.OnDisplayBioUnit();
        }

        /// <summary>
        /// MQ Algorithm
        /// </summary>
        public void OnSurfaceBiounitGenAlgorithmMQ()
        {
            this.dllInterface.OnSurfaceBiounitGenAlgorithmMQ();
        }

        /// <summary>
        /// MSMS Algorithm
        /// </summary>
        public void OnSurfaceBiounitGenAlgorithmMSMS()
        {
            this.dllInterface.OnSurfaceBiounitGenAlgorithmMSMS();
        }

        public void OnSurfaceGenAlgorithmMQ()
        {
            this.dllInterface.OnSurfaceGenAlgorithmMQ();
        }

        public void OnSurfaceGenAlgorithmMSMS()
        {
            this.dllInterface.OnSurfaceGenAlgorithmMSMS();
        }

        /// <summary>
        /// Add SpaceFill Display
        /// Add current selection to new VP with SpaceFill display method
        /// </summary>
        public void OnViewAll()
        {
            this.dllInterface.OnViewAll();
        }

        public void OnViewAllDisplayParams()
        {
            this.dllInterface.OnViewAllDisplayParams();
        }

        public void OnDisplayBioUnitStyle(int mode)
        {
            this.dllInterface.OnDisplayBioUnitStyle(mode);
        }

        public void OnDisplayBioUnitSurface(int quality)
        {
            this.dllInterface.OnDisplayBioUnitSurface(quality);
        }
        #endregion

        public void ShowLog(bool bShow)
        {
            dllInterface.ShowLog(bShow);
        }
 
    }
}
