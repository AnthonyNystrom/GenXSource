using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Genetibase.UI.NuGenImageWorks.Undo;

namespace Genetibase.UI.NuGenImageWorks
{
    public partial class UndoRedoCtrl : UserControl
    {

        private UndoHelper undoHelper = null;
        private MainForm mainForm = null;

        public MainForm MainForm
        {
            get { return mainForm; }
            set { mainForm = value; }
        }

        internal UndoHelper UndoHelper
        {
            get { return undoHelper; }
            set { undoHelper = value; }
        }

        public UndoRedoCtrl()
        {
            InitializeComponent();
        }

        private void ribbonButton31_Click(object sender, EventArgs e)
        {
            if (undoHelper.CanUndo)
            {
                undoHelper.Undo();

                mainForm.SetLensParams();
                mainForm.SetFilmParams();
                mainForm.SetAtmosphereParams();
                mainForm.SetGammaParams();
                mainForm.SetGainParams();
                mainForm.SetOffsetParams();
                mainForm.SetEnhanceParams();
                mainForm.SetEnhance2Params();

                mainForm.FilterEffects();

                ////GB
                //Program.Filter.Do((Bitmap)Program.Destination.Image);
                
                //Image temp = Program.Destination.Image;
                //Program.Destination.Image = Program.Effects.Do((Bitmap)Program.Source.Image);

                //if (temp != null)
                //    temp.Dispose();

                //WaterMark wm = new WaterMark(mainForm.WaterMarkText, mainForm.TextAlign, mainForm.WaterMarkFont, mainForm.WaterMarkImage, mainForm.ImageAlign);

                //temp = Program.Destination.Image;
                //Program.Destination.Image = (Bitmap)wm.MarkImage( Program.Destination.Image );

                //if (temp != null)
                //    temp.Dispose();
            }
        }

        private void ribbonButton30_Click(object sender, EventArgs e)
        {
            if (undoHelper.CanRedo)
            {
                undoHelper.Redo();

                mainForm.SetLensParams();
                mainForm.SetFilmParams();
                mainForm.SetAtmosphereParams();
                mainForm.SetGammaParams();
                mainForm.SetGainParams();
                mainForm.SetOffsetParams();
                mainForm.SetEnhanceParams();
                mainForm.SetEnhance2Params();

                mainForm.FilterEffects();

                ////GB
                //Program.Filter.Do((Bitmap)Program.Destination.Image);

                //Image temp = Program.Destination.Image;
                //Program.Destination.Image = Program.Effects.Do((Bitmap)Program.Source.Image);

                //if (temp != null)
                //    temp.Dispose();

                //WaterMark wm = new WaterMark(mainForm.WaterMarkText, mainForm.TextAlign, mainForm.WaterMarkFont, mainForm.WaterMarkImage, mainForm.ImageAlign);

                //temp = Program.Destination.Image;
                //Program.Destination.Image = (Bitmap)wm.MarkImage(Program.Destination.Image);

                //if (temp != null)
                //    temp.Dispose();
            }
        }
    }
}
