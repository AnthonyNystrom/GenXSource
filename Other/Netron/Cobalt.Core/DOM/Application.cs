using System;
using System.Collections.Generic;
using System.Text;
using Netron.Diagramming.Win;
using System.Windows.Forms;
using Netron.Neon.WinFormsUI;
namespace Netron.Cobalt
{
    /// <summary>
    /// The root of the application's DOM. The one that binds them all together.
    /// </summary>
    public static partial class Application
    {
        #region Fields

        private static ShellControl mShell;

        /// <summary>
        /// the MainForm field
        /// </summary>
        private static MainForm mMainForm;
        /// <summary>
        /// the DockPanel field
        /// </summary>
        private static DockPanel mDockPanel;
        /// <summary>
        /// the Diagram field
        /// </summary>
        private static DiagramControl mDiagram;
        /// <summary>
        /// the WebBrowser field
        /// </summary>
        private static WebBrowser mWebBrowser;
        /// <summary>
        /// the Shapes field
        /// </summary>
        private static ListView mShapes;
         /// <summary>
        /// the Property field
        /// </summary>
        private static PropertyGrid mProperty;
        /// <summary>
        /// the output control
        /// </summary>
        private static NOutput mOutput;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the output control.
        /// </summary>
        /// <value>The output.</value>
        public static NOutput Output
        {
            get {
                if (mOutput == null)
                    CreateOutputForm();
                return mOutput; }
        }

        /// <summary>
        /// Gets or sets the MainForm
        /// </summary>
        public static MainForm MainForm
        {
            get { return mMainForm; }
            set { mMainForm = value; }
        }
        /// <summary>
        /// Gets or sets the Property
        /// </summary>
        public static PropertyGrid Property
        {
            get {
                if (mProperty == null)
                    CreateProperty();
                return mProperty; }
        }
        /// <summary>
        /// Gets or sets the DockPanel
        /// </summary>
        public static DockPanel DockPanel
        {
            get { return mDockPanel; }
            set { mDockPanel = value; }
        }
        /// <summary>
        /// Gets or sets the Shapes
        /// </summary>
        public static ListView Shapes
        {
            get {
                if (mShapes == null)
                    CreateShapesForm();
                return mShapes; }
            
        }
        /// <summary>
        /// Gets or sets the Diagram
        /// </summary>
        public static DiagramControl Diagram
        {
            get {
                if (mDiagram == null)
                    CreateDiagramCanvas();
                return mDiagram; }
        }
        /// <summary>
        /// Gets or sets the WebBrowser
        /// </summary>
        public static WebBrowser WebBrowser
        {
            get
            {
                if (mWebBrowser == null)
                    CreateWebBrowser();
                return mWebBrowser;
            }
        }
        #endregion

        #region Methods

        private static void CreateShellForm()
        {
            ShellForm frm = new ShellForm();
            mShell = frm.Shell;
            frm.Show(DockPanel, Properties.Settings.Default.ShellDockState);
            Tabs.Shell = frm;
        }

        /// <summary>
        /// Creates the web browser.
        /// </summary>
        private static void CreateWebBrowser()
        {
            BrowserForm frm = new BrowserForm();
            //DockPane pane = DockPanel.Extender.DockPaneFactory.CreateDockPane(frm, DockState.Document, true);
            //frm.MdiParent = MainForm;
            
            mWebBrowser = frm.WebBrowser;
            frm.Show(DockPanel, Properties.Settings.Default.WebBrowserDockState);
            Tabs.WebBrowser = frm; 
        }

        /// <summary>
        /// Creates the diagram canvas.
        /// </summary>
        private static void CreateDiagramCanvas()
        {
            DiagramForm frm = new DiagramForm();
            mDiagram = frm.DiagramControl;
            frm.Show(DockPanel, Properties.Settings.Default.DiagramDockState);
            Tabs.Diagram = frm;
        }

        /// <summary>
        /// Creates the property form.
        /// </summary>
        private static void CreateProperty()
        {
            PropertyForm frm = new PropertyForm();
            mProperty = frm.PropertyGrid;
            frm.Show(DockPanel, Properties.Settings.Default.PropertyDockState);
            Tabs.Property = frm;
        }

        /// <summary>
        /// Creates the shapes form.
        /// </summary>
        private static void CreateShapesForm()
        {
            ShapesForm frm = new ShapesForm();
            mShapes = frm.ShapesListView;
            frm.Show(DockPanel, Properties.Settings.Default.ShapesDockState);
            Tabs.Shapes = frm;
        }

        /// <summary>
        /// Creates the output form.
        /// </summary>
        private static void CreateOutputForm()
        {
            OutputForm frm = new OutputForm();
            mOutput = frm.Output;
            frm.Show(DockPanel, Properties.Settings.Default.OutputDockState);
            Tabs.Output = frm;
        }

        /// <summary>
        /// Shows the given content in the webbrowser.
        /// </summary>
        /// <param name="document">The document.</param>
        public static void ShowContent(string document)
        {
            Application.Tabs.WebBrowser.Show();
            Application.WebBrowser.DocumentText = document;
        }

        #endregion
   
      
    }
}
