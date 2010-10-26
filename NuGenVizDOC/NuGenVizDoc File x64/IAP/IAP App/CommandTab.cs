using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace IAP_App
{
    public partial class CommandTab : UserControl
    {
        public CommandTab()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            LinearGradientBrush brush1 = new LinearGradientBrush(new Rectangle(0, 0, ((Panel)sender).Width, ((Panel)sender).Height), Color.FromArgb(207, 213, 225), Color.FromArgb(192, 199, 217), LinearGradientMode.Vertical);

            e.Graphics.FillRectangle(brush1, 0, 0, ((Panel)sender).Width, ((Panel)sender).Height);

            LinearGradientBrush brush2 = new LinearGradientBrush(new Rectangle(0, 0, ((Panel)sender).Width, ((Panel)sender).Height), Color.Transparent, Color.FromArgb(32, 0, 0, 0), LinearGradientMode.Vertical);
            LinearGradientBrush brush3 = new LinearGradientBrush(new Rectangle(0, 0, ((Panel)sender).Width, ((Panel)sender).Height), Color.Transparent, Color.FromArgb(64, 255, 255, 255), LinearGradientMode.Vertical);

            e.Graphics.DrawRectangle(new Pen(brush2, 1), new Rectangle(0, 0, ((Panel)sender).Width, ((Panel)sender).Height));
            e.Graphics.DrawRectangle(new Pen(brush3, 1), new Rectangle(0, -1, ((Panel)sender).Width, ((Panel)sender).Height));
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                LinearGradientBrush brush1 = new LinearGradientBrush(new Rectangle(0, 0, ((Panel)sender).Width / 2, ((Panel)sender).Height), Color.LightGray, Color.WhiteSmoke, LinearGradientMode.Horizontal);
                brush1.WrapMode = WrapMode.TileFlipX;

                LinearGradientBrush brush2 = new LinearGradientBrush(new Rectangle(0, 0, ((Panel)sender).Width, ((Panel)sender).Height), Color.FromArgb(32, 0, 0, 0), Color.Transparent, LinearGradientMode.Horizontal);
                LinearGradientBrush brush3 = new LinearGradientBrush(new Rectangle(0, 0, ((Panel)sender).Width, ((Panel)sender).Height), Color.FromArgb(64, 255, 255, 255), Color.Transparent, LinearGradientMode.Horizontal);

                Program.DrawRoundedRectangle(e.Graphics, brush1, new Pen(brush2, 1), new Rectangle(0, 0, ((Panel)sender).Width, ((Panel)sender).Height), 3, 3);
                Program.DrawRoundedRectangle(e.Graphics, null, new Pen(brush3, 1), new Rectangle(1, 1, ((Panel)sender).Width, ((Panel)sender).Height), 3, 3);
            }
            catch { }
        }

        private void commandTabButton1_Click(object sender, EventArgs e)
        {
            this.panel3.Visible = true;
            this.panel4.Visible = false;
        }

        private void commandTabButton2_Click(object sender, EventArgs e)
        {
            this.panel3.Visible = false;
            this.panel4.Visible = true;
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            this.contextMenuStrip1.Show((Label)sender, e.Location);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                ((PictureBox)sender).BackColor = this.colorDialog1.Color;

                IAP_Core.ImageOperations.Image.SetColor(this.pictureBox3.BackColor, this.pictureBox5.BackColor);

                Program.ImageBox.Invalidate();
            }
        }

        private void CommandTab_Load(object sender, EventArgs e)
        {
            Program.SelectionBox = this.textBox1;
            Program.HistogramBox = this.pictureBox4;
            Program.RangeBox = this.textBox2;
        }

        private void commandTabSectionButton1_Click(object sender, EventArgs e)
        {
            IAP_Core.ImageOperations.Rect = new Rectangle(0, 0, IAP_Core.ImageOperations.Image.Width, IAP_Core.ImageOperations.Image.Height);

            Program.SelectionBox.Text = IAP_Core.ImageOperations.Rect.X.ToString() + "; " + IAP_Core.ImageOperations.Rect.Y.ToString() + "; " + IAP_Core.ImageOperations.Rect.Width.ToString() + "; " + IAP_Core.ImageOperations.Rect.Height.ToString();
            Program.ImageBox.Invalidate();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                try
                {
                    string[] rc = textBox1.Text.Split(';');
                    IAP_Core.ImageOperations.Rect = new Rectangle(int.Parse(rc[0]), int.Parse(rc[1]), int.Parse(rc[2]), int.Parse(rc[3]));
                }
                catch
                {
                    textBox1.Text = IAP_Core.ImageOperations.Rect.X.ToString() + "; " + IAP_Core.ImageOperations.Rect.Y.ToString() + "; " + IAP_Core.ImageOperations.Rect.Width.ToString() + "; " + IAP_Core.ImageOperations.Rect.Height.ToString();
                    string[] rc = textBox1.Text.Split(';');
                    IAP_Core.ImageOperations.Rect = new Rectangle(int.Parse(rc[0]), int.Parse(rc[1]), int.Parse(rc[2]), int.Parse(rc[3]));
                }

                Program.ImageBox.Invalidate();
            }
        }

        private void pictureBox4_Paint(object sender, PaintEventArgs e)
        {
            int[] histogram = IAP_Core.ImageOperations.Image.Histogram;

            int max = 0;
            for (int i = 0; i < 256; i++)
                max = Math.Max(max, histogram[i]);

            LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, pictureBox4.Width, pictureBox4.Height), Color.LightSteelBlue, Color.SteelBlue, LinearGradientMode.Vertical);

            for (int i = 0; i < 256; i++)
                e.Graphics.DrawLine(new Pen(brush), i, 46, i, 46 - (int)(46.0 * ((double)histogram[i] / (double)max)));
        }

        private void commandTabSectionButton8_Click(object sender, EventArgs e)
        {
            try
            {
                string[] param = this.textBox4.Text.Split(';');
                IAP_Core.ImageOperations.Resize(int.Parse(param[0]), int.Parse(param[1]), (this.toolStripComboBox1.SelectedIndex == 1));
                IAP_Core.ImageOperations.Rect = new Rectangle(0, 0, int.Parse(param[0]), int.Parse(param[1]));

                Program.ZoomScale = 1.0;
                Program.ZoomBox.Text = "100%";

                Program.ImageBox.Image = IAP_Core.ImageOperations.Image.Bitmap;
                Program.ImageBox.Size = new Size(Program.ImageBox.Image.Width, Program.ImageBox.Image.Height);

                Program.SizeBox.Text = " Image Size: " + Program.ImageBox.Size.Width.ToString() + " x " + Program.ImageBox.Size.Height.ToString();
                Program.SelectionBox.Text = IAP_Core.ImageOperations.Rect.X.ToString() + "; " + IAP_Core.ImageOperations.Rect.Y.ToString() + "; " + IAP_Core.ImageOperations.Rect.Width.ToString() + "; " + IAP_Core.ImageOperations.Rect.Height.ToString();

                Program.ImageBox.Parent.Invalidate();
            }
            catch { }
        }

        private void commandTabSectionButton9_Click(object sender, EventArgs e)
        {
            IAP_Core.ImageOperations.Flip();

            Program.ImageBox.Invalidate();
        }

        private void commandTabSectionButton10_Click(object sender, EventArgs e)
        {
            IAP_Core.ImageOperations.Mirror();

            Program.ImageBox.Invalidate();
        }

        private void commandTabSectionButton13_Click(object sender, EventArgs e)
        {
            try
            {
                string[] param = this.textBox5.Text.Split(';');
                IAP_Core.ImageOperations.Clamp(byte.Parse(param[0]), byte.Parse(param[1]));

                Program.ImageBox.Invalidate();
            }
            catch { }
        }

        private void commandTabSectionButton12_Click(object sender, EventArgs e)
        {
            IAP_Core.ImageOperations.Normalization();

            Program.ImageBox.Invalidate();
        }

        private void commandTabSectionButton11_Click(object sender, EventArgs e)
        {
            IAP_Core.ImageOperations.Equalization();

            Program.ImageBox.Invalidate();
        }

        private void commandTabSectionButton14_Click(object sender, EventArgs e)
        {
            try
            {
                IAP_Core.ImageOperations.Convolution(this.textBox6.Lines, int.Parse(this.textBox7.Text), (this.toolStripComboBox2.SelectedIndex == 1));

                Program.ImageBox.Invalidate();
            }
            catch { }
        }

        private void commandTabSectionButton16_Click(object sender, EventArgs e)
        {
            try
            {
                IAP_Core.ImageOperations.Median(int.Parse(this.textBox8.Text));

                Program.ImageBox.Invalidate();
            }
            catch { }
        }

        private void commandTabSectionButton15_Click(object sender, EventArgs e)
        {
            IAP_Core.ImageOperations.Invert();

            Program.ImageBox.Invalidate();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (IAP_Core.ImageOperations.ImageBackup != null)
            {
                IAP_Core.ImageOperations.Image = new IAP_Core.Image(IAP_Core.ImageOperations.ImageBackup.Bitmap);
                IAP_Core.ImageOperations.Rect = new Rectangle(0, 0, IAP_Core.ImageOperations.Image.Width, IAP_Core.ImageOperations.Image.Height);

                Program.ImageBox.Image = IAP_Core.ImageOperations.Image.Bitmap;
                Program.ImageBox.Size = new Size(Program.ImageBox.Image.Width, Program.ImageBox.Image.Height);

                Program.SizeBox.Text = " Image Size: " + Program.ImageBox.Size.Width.ToString() + " x " + Program.ImageBox.Size.Height.ToString();
                Program.SelectionBox.Text = IAP_Core.ImageOperations.Rect.X.ToString() + "; " + IAP_Core.ImageOperations.Rect.Y.ToString() + "; " + IAP_Core.ImageOperations.Rect.Width.ToString() + "; " + IAP_Core.ImageOperations.Rect.Height.ToString();

                Program.ImageBox.Parent.Invalidate();

                Program.HistogramBox.Invalidate();
            }
        }

        private void textBox6_Click(object sender, EventArgs e)
        {
            this.comboBox1.Text = "Custom";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBox1.Text)
            {
                case "Box":
                    this.textBox6.Lines = new string[] { "1 1 1", "1 1 1", "1 1 1" };
                    this.textBox7.Text = "9";
                    break;
                case "Gaussian":
                    this.textBox6.Lines = new string[] { "0 1 0", "1 4 1", "0 1 0" };
                    this.textBox7.Text = "8"; 
                    break;
                case "Laplacian":
                    this.textBox6.Lines = new string[] { "-1 -1 -1", "-1 8 -1", "-1 -1 -1" };
                    this.textBox7.Text = "1";
                    break;
                case "Unsharp":
                    this.textBox6.Lines = new string[] { "-1 -1 -1", "-1 9 -1", "-1 -1 -1" };
                    this.textBox7.Text = "1";
                    break;
                case "Bump":
                    this.textBox6.Lines = new string[] { "-1 0 1", "-1 1 1", "-1 0 1" };
                    this.textBox7.Text = "1";
                    break;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    IAP_Core.ImageOperations.Image = new IAP_Core.IAPImage(this.openFileDialog1.FileName);
                    IAP_Core.ImageOperations.Rect = new Rectangle(0, 0, IAP_Core.ImageOperations.Image.Width, IAP_Core.ImageOperations.Image.Height);

                    this.commandTabSectionButton7.Enabled = false;
                    this.textBox2.Enabled = textBox3.Enabled = false;
                    this.commandTabSectionButton4.Enabled = this.commandTabSectionButton5.Enabled = this.commandTabSectionButton6.Enabled = false;

                    this.commandTabButton2.Enabled = true;
                    this.pictureBox2.Enabled = true;

                    this.commandTabSectionButton3.Title = "Forward";

                    Program.ZoomScale = 1.0;
                    Program.ZoomBox.Text = "100%";

                    Program.ImageBox.Image = IAP_Core.ImageOperations.Image.Bitmap;
                    Program.ImageBox.Size = new Size(Program.ImageBox.Image.Width, Program.ImageBox.Image.Height);

                    Program.SizeBox.Text = " Image Size: " + Program.ImageBox.Size.Width.ToString() + " x " + Program.ImageBox.Size.Height.ToString();
                    Program.SelectionBox.Text = IAP_Core.ImageOperations.Rect.X.ToString() + "; " + IAP_Core.ImageOperations.Rect.Y.ToString() + "; " + IAP_Core.ImageOperations.Rect.Width.ToString() + "; " + IAP_Core.ImageOperations.Rect.Height.ToString();

                    Program.ImageBox.Parent.Invalidate();
                }
                catch
                {
                    MessageBox.Show("Format not valid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                new IAP_Core.IAPImage(IAP_Core.ImageOperations.Image.Bitmap).Save(this.saveFileDialog1.FileName);
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    IAP_Core.ImageOperations.Image = new IAP_Core.BmpImage(this.openFileDialog2.FileName);
                    IAP_Core.ImageOperations.Rect = new Rectangle(0, 0, IAP_Core.ImageOperations.Image.Width, IAP_Core.ImageOperations.Image.Height);

                    this.commandTabSectionButton7.Enabled = false;
                    this.textBox2.Enabled = textBox3.Enabled = false;
                    this.commandTabSectionButton4.Enabled = this.commandTabSectionButton5.Enabled = this.commandTabSectionButton6.Enabled = false;

                    this.commandTabButton2.Enabled = true;
                    this.pictureBox2.Enabled = true;

                    this.commandTabSectionButton3.Title = "Forward";

                    Program.ZoomScale = 1.0;
                    Program.ZoomBox.Text = "100%";

                    Program.ImageBox.Image = IAP_Core.ImageOperations.Image.Bitmap;
                    Program.ImageBox.Size = new Size(Program.ImageBox.Image.Width, Program.ImageBox.Image.Height);

                    Program.SizeBox.Text = " Image Size: " + Program.ImageBox.Size.Width.ToString() + " x " + Program.ImageBox.Size.Height.ToString();
                    Program.SelectionBox.Text = IAP_Core.ImageOperations.Rect.X.ToString() + "; " + IAP_Core.ImageOperations.Rect.Y.ToString() + "; " + IAP_Core.ImageOperations.Rect.Width.ToString() + "; " + IAP_Core.ImageOperations.Rect.Height.ToString();

                    Program.ImageBox.Parent.Invalidate();
                }
                catch
                {
                    MessageBox.Show("Format not valid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog2.ShowDialog() == DialogResult.OK)
                new IAP_Core.BmpImage(IAP_Core.ImageOperations.Image.Bitmap).Save(this.saveFileDialog2.FileName);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void commandTabSectionButton3_Click(object sender, EventArgs e)
        {
            if (this.commandTabSectionButton3.Title == "Forward")
            {
                this.commandTabSectionButton3.Title = "Backward";

                this.commandTabButton2.Enabled = false;
                this.pictureBox2.Enabled = false;

                IAP_Core.ImageOperations.ImageBackup = new IAP_Core.Image(IAP_Core.ImageOperations.Image.Bitmap);
                IAP_Core.ImageOperations.Image = IAP_Core.ComplexImage.FromImage(IAP_Core.ImageOperations.Image);

                Program.ImageBox.Image = IAP_Core.ImageOperations.Image.Bitmap;
                Program.ImageBox.Invalidate();

                this.commandTabSectionButton7.Enabled = true;
                this.textBox2.Enabled = textBox3.Enabled = true;
                this.commandTabSectionButton4.Enabled = this.commandTabSectionButton5.Enabled = this.commandTabSectionButton6.Enabled = true;
            }
            else
            {
                this.commandTabSectionButton7.Enabled = false;
                this.textBox2.Enabled = textBox3.Enabled = false;
                this.commandTabSectionButton4.Enabled = this.commandTabSectionButton5.Enabled = this.commandTabSectionButton6.Enabled = false;

                IAP_Core.ImageOperations.Image = ((IAP_Core.ComplexImage)IAP_Core.ImageOperations.Image).ToImage();

                Program.ImageBox.Image = IAP_Core.ImageOperations.Image.Bitmap;
                Program.ImageBox.Invalidate();

                Program.HistogramBox.Invalidate();

                this.commandTabButton2.Enabled = true;
                this.pictureBox2.Enabled = true;

                this.commandTabSectionButton3.Title = "Forward";
            }
        }

        private void commandTabSectionButton7_Click(object sender, EventArgs e)
        {
            ((IAP_Core.ComplexImage)IAP_Core.ImageOperations.Image).Rollback();
            ((IAP_Core.ComplexImage)IAP_Core.ImageOperations.Image).Update();

            Program.ImageBox.Invalidate();
        }

        private void commandTabSectionButton4_Click(object sender, EventArgs e)
        {
            try
            {
                string[] r = this.textBox2.Text.Split(';');
                string[] v = this.textBox3.Text.Split(';');

                ((IAP_Core.ComplexImage)IAP_Core.ImageOperations.Image).Set(int.Parse(r[0]), int.Parse(r[1]), double.Parse(v[0]), double.Parse(v[1]));
                ((IAP_Core.ComplexImage)IAP_Core.ImageOperations.Image).Update();

                Program.ImageBox.Invalidate();
            }
            catch { }
        }

        private void commandTabSectionButton5_Click(object sender, EventArgs e)
        {
            try
            {
                string[] r = this.textBox2.Text.Split(';');
                string[] v = this.textBox3.Text.Split(';');

                ((IAP_Core.ComplexImage)IAP_Core.ImageOperations.Image).Add(int.Parse(r[0]), int.Parse(r[1]), double.Parse(v[0]), double.Parse(v[1]));
                ((IAP_Core.ComplexImage)IAP_Core.ImageOperations.Image).Update();

                Program.ImageBox.Invalidate();
            }
            catch { }
        }

        private void commandTabSectionButton6_Click(object sender, EventArgs e)
        {
            try
            {
                string[] r = this.textBox2.Text.Split(';');
                string[] v = this.textBox3.Text.Split(';');

                ((IAP_Core.ComplexImage)IAP_Core.ImageOperations.Image).Mult(int.Parse(r[0]), int.Parse(r[1]), double.Parse(v[0]), double.Parse(v[1]));
                ((IAP_Core.ComplexImage)IAP_Core.ImageOperations.Image).Update();

                Program.ImageBox.Invalidate();
            }
            catch { }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Program.ImageBox.Invalidate();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(sender, e);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                ((PictureBox)sender).BackColor = this.colorDialog1.Color;

                IAP_Core.ImageOperations.Image.SetColor(this.pictureBox3.BackColor, this.pictureBox5.BackColor);

                Program.ImageBox.Invalidate();
            }
        }
    }
}
