using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Genetibase.NuGenAnnotation;

namespace Genetibase.NuGenMediImage.UI.Controls
{
    public partial class ViewerAnnot : UserControl
    {

        private DocManager docManager;

        public Viewer Viewer
        {
            get 
            {
                return (Viewer)this.drawArea.FirstControl;
            }            
        }

        public DrawArea DrawArea
        {
            get
            {
                return (DrawArea)this.drawArea;
            }
        }

        public ViewerAnnot()
        {
            InitializeComponent();
            this.viewer._drawArea = this.drawArea;

            //HAMID
            //drawArea.Owner = tempOwner;

            this.drawArea.AddViewer(this.viewer);
            this.DrawArea.MouseClick += new MouseEventHandler(DrawArea_MouseClick);
            this.viewer.MouseClick += new MouseEventHandler(viewer_MouseClick);
            //this.drawArea.BringToFront();

            // Helper objects (DocManager and others)
            InitializeHelperObjects();

            //HAMID
            drawArea.Initialize(null, docManager);
            ResizeDrawArea();
        }

        void DrawArea_MouseClick(object sender, MouseEventArgs e)
        {
            this.OnMouseClick(e);

        }

        void viewer_MouseClick(object sender, MouseEventArgs e)
        {
            this.OnMouseClick(e);
        }

        /// <summary>
        /// Set draw area to all form client space except toolbar
        /// </summary>
        private void ResizeDrawArea()
        {
            Rectangle rect = this.ClientRectangle;

            drawArea.Left = rect.Left;
            drawArea.Top = rect.Top;// +menuStrip1.Height + toolStrip1.Height;
            drawArea.Width = rect.Width;
            drawArea.Height = rect.Height;// -menuStrip1.Height - toolStrip1.Height;
            ;
        }



        private void InitializeHelperObjects()
        {
            int x = drawArea.TheLayers.ActiveLayerIndex;

            // DocManager
            DocManagerData data = new DocManagerData();
            //HAMID
            //data.FormOwner = tempOwner;
            data.UpdateTitle = true;
            data.FileDialogFilter = "Genetibase.NuGenAnnotation files (*.dtl)|*.dtl|All Files (*.*)|*.*";
            data.NewDocName = "Untitled.dtl";
            //data.RegistryPath = registryPath;

            docManager = new DocManager(data);
            docManager.RegisterFileType("dtl", "dtlfile", "Genetibase.NuGenAnnotation File");

            // Subscribe to DocManager events.
            //docManager.SaveEvent += docManager_SaveEvent;
            //docManager.LoadEvent += docManager_LoadEvent;

            docManager.DocChangedEvent += delegate(object o, EventArgs e)
            {
                drawArea.Refresh();
                drawArea.ClearHistory();
            };

            docManager.ClearEvent += delegate(object o, EventArgs e)
            {
                bool haveObjects = false;
                for (int i = 0; i < drawArea.TheLayers.Count; i++)
                {
                    if (drawArea.TheLayers[i].Graphics.Count > 1)
                    {
                        haveObjects = true;
                        break;
                    }
                }
                if (haveObjects)
                {
                    drawArea.TheLayers.Clear();
                    drawArea.ClearHistory();
                    drawArea.Refresh();
                }
            };

            docManager.NewDocument();
        }

        /// <summary>
        /// Load document from the stream supplied by DocManager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        //private void docManager_LoadEvent(object sender, SerializationEventArgs e)
        //{
        //    // DocManager asks to load document from supplied stream
        //    try
        //    {
        //        drawArea.TheLayers = (Layers)e.Formatter.Deserialize(e.SerializationStream);
        //    }
        //    catch (Exception)
        //    {

        //    }

        //}


        /// <summary>
        /// Save document to stream supplied by DocManager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        //private void docManager_SaveEvent(object sender, SerializationEventArgs e)
        //{
        //    // DocManager asks to save document to supplied stream
        //    try
        //    {
        //        e.Formatter.Serialize(e.SerializationStream, drawArea.TheLayers);
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}
    }
}
