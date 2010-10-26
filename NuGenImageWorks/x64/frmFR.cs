using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Genetibase.UI.NuGenImageWorks.Undo;

namespace Genetibase.UI.NuGenImageWorks
{
    public partial class frmFR : Form
    {
        public FloorReflectionFilterProp FloorReflection = null;

        private DockStyle pos = DockStyle.None;

        public DockStyle ReflectionPos
        {
            get
            {
                return pos;
            }
            set
            {
                pos = value;

                if (pos == DockStyle.Top)
                {
                    top.IsPressed = true;
                    if (numbericUpDownOffset.Value > Program.Photo.Height)
                        numbericUpDownOffset.Value = Program.Photo.Height;

                }
                else if (pos == DockStyle.Bottom)
                {
                    bottom.IsPressed = true;
                    if (numbericUpDownOffset.Value > Program.Photo.Height)
                        numbericUpDownOffset.Value = Program.Photo.Height;

                }
                else if (pos == DockStyle.Left)
                {
                    left.IsPressed = true;
                    if (numbericUpDownOffset.Value > Program.Photo.Width)
                        numbericUpDownOffset.Value = Program.Photo.Width;
                }
                else if (pos == DockStyle.Right)
                {
                    right.IsPressed = true;
                    if (numbericUpDownOffset.Value > Program.Photo.Width)
                        numbericUpDownOffset.Value = Program.Photo.Width;
                }

                lblSelected.Text = pos.ToString();

                FloorReflection.DockPosition = value;
            }
        }

        public frmFR()
        {
            InitializeComponent();
        }

        private void ribbonButtonOp5OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ribbonButtonOp5Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void top_Click(object sender, EventArgs e)
        {
            ClearPressed();
            this.ReflectionPos = DockStyle.Top;
        }

        private void left_Click(object sender, EventArgs e)
        {
            ClearPressed();
            this.ReflectionPos = DockStyle.Left;
        }

        private void right_Click(object sender, EventArgs e)
        {
            ClearPressed();
            this.ReflectionPos = DockStyle.Right;
        }

        private void bottom_Click(object sender, EventArgs e)
        {
            ClearPressed();
            this.ReflectionPos = DockStyle.Bottom;
        }

        private void ClearPressed()
        {
            top.IsPressed = false;
            bottom.IsPressed = false;
            left.IsPressed = false;
            right.IsPressed = false;
            this.Invalidate(true);
        }

        private void frmFR_Load(object sender, EventArgs e)
        {
            FloorReflection = new FloorReflectionFilterProp(Effects2.FloorReflectionFilter.AlphaStart, Effects2.FloorReflectionFilter.AlphaEnd, Effects2.FloorReflectionFilter.DockPosition, Effects2.FloorReflectionFilter.Offset);

            this.numbericUpDownOffset.Maximum = Program.Photo.Width > Program.Photo.Height ? Program.Photo.Width : Program.Photo.Height;
            this.numbericUpDownAlphaStart.Value = FloorReflection.AlphaStart;
            this.numbericUpDownAlphaEnd.Value = FloorReflection.AlphaEnd;
            this.numbericUpDownOffset.Value = FloorReflection.Offset;
            this.ReflectionPos = FloorReflection.DockPosition;
            
        }

        private void numbericUpDownAlphaStart_ValueChanged(object sender, EventArgs e)
        {
            if (numbericUpDownAlphaStart.Value < numbericUpDownAlphaEnd.Value)
                numbericUpDownAlphaStart.Value = numbericUpDownAlphaEnd.Value;
            FloorReflection.AlphaStart = (int)numbericUpDownAlphaStart.Value;
        }

        private void numbericUpDownAlphaEnd_ValueChanged(object sender, EventArgs e)
        {
            if (numbericUpDownAlphaEnd.Value > numbericUpDownAlphaStart.Value)
                numbericUpDownAlphaEnd.Value = numbericUpDownAlphaStart.Value;

            FloorReflection.AlphaEnd = (int)numbericUpDownAlphaEnd.Value;
        }

        private void numbericUpDownOffset_ValueChanged(object sender, EventArgs e)
        {
            FloorReflection.Offset = (int)numbericUpDownOffset.Value;
        }
    }
}