using System;
using System.Collections.Generic;
using System.Text;
using Genetibase.UI;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Printing;

namespace Genetibase.Chem.NuGenSChem
{
    class NuGenMainPopupMenu : NuGenPopupMenu
    {
        private RibbonButton newButton;
        private RibbonButton openButton;
        private RibbonButton viewButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private RibbonButton saveButton;
        private RibbonButton exportButton;
        private RibbonButton saveAsButton;
        private RibbonButton quitButton;
        private PictureBox pictureBox2;

        private NuGenViewPopupMenu viewMenu;
        private PictureBox pictureBox3;
        private RibbonButton ribbonButton1;
        private RibbonButton ribbonButton2;
        private NuGenExportPopupMenu exportMenu;
        private RibbonButton ribbonButton3;
        private TemplateSelector templatesMenu;

        public NuGenMainPopupMenu(NuGenEventHandler handler, NuGenPopupMenu parent):base(handler, parent)
        {            
            InitializeComponent();

            viewMenu = new NuGenViewPopupMenu(handler, this);
            exportMenu = new NuGenExportPopupMenu(handler, this);
            templatesMenu = new TemplateSelector(handler, this);

            AddChild(viewMenu);
            AddChild(exportMenu);
            AddChild(templatesMenu);
        }

        public override void InitializeDefaults()
        {
            base.InitializeDefaults();

            this.newButton.Enabled = false;
            this.openButton.Enabled = false;
        }

        public override void EnableControls()
        {
            base.EnableControls();

            this.newButton.Enabled = true;
            this.openButton.Enabled = true;
        }    
        

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NuGenMainPopupMenu));
            this.newButton = new Genetibase.UI.RibbonButton();
            this.openButton = new Genetibase.UI.RibbonButton();
            this.saveButton = new Genetibase.UI.RibbonButton();
            this.viewButton = new Genetibase.UI.RibbonButton();
            this.exportButton = new Genetibase.UI.RibbonButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.saveAsButton = new Genetibase.UI.RibbonButton();
            this.quitButton = new Genetibase.UI.RibbonButton();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.ribbonButton1 = new Genetibase.UI.RibbonButton();
            this.ribbonButton2 = new Genetibase.UI.RibbonButton();
            this.ribbonButton3 = new Genetibase.UI.RibbonButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // newButton
            // 
            this.newButton.AllowDrop = true;
            this.newButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.newButton.Command = null;
            this.newButton.IsFlat = true;
            this.newButton.IsPressed = false;
            this.newButton.Location = new System.Drawing.Point(5, 178);
            this.newButton.Margin = new System.Windows.Forms.Padding(1);
            this.newButton.Name = "newButton";
            this.newButton.Padding = new System.Windows.Forms.Padding(2);
            this.newButton.Size = new System.Drawing.Size(144, 52);
            this.newButton.TabIndex = 1;
            this.newButton.Text = "New";
            this.newButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.newButton.Click += new System.EventHandler(this.newButton_Click);
            this.newButton.MouseEnter += new System.EventHandler(this.MouseEnterDefault);
            // 
            // openButton
            // 
            this.openButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.openButton.Command = null;
            this.openButton.IsFlat = true;
            this.openButton.IsPressed = false;
            this.openButton.Location = new System.Drawing.Point(5, 230);
            this.openButton.Margin = new System.Windows.Forms.Padding(1);
            this.openButton.Name = "openButton";
            this.openButton.Padding = new System.Windows.Forms.Padding(2);
            this.openButton.Size = new System.Drawing.Size(144, 52);
            this.openButton.TabIndex = 1;
            this.openButton.Text = "Open";
            this.openButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            this.openButton.MouseEnter += new System.EventHandler(this.MouseEnterDefault);
            // 
            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.saveButton.Command = null;
            this.saveButton.IsFlat = true;
            this.saveButton.IsPressed = false;
            this.saveButton.Location = new System.Drawing.Point(5, 282);
            this.saveButton.Margin = new System.Windows.Forms.Padding(1);
            this.saveButton.Name = "saveButton";
            this.saveButton.Padding = new System.Windows.Forms.Padding(2);
            this.saveButton.Size = new System.Drawing.Size(144, 52);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save";
            this.saveButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            this.saveButton.MouseEnter += new System.EventHandler(this.MouseEnterDefault);
            // 
            // viewButton
            // 
            this.viewButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.viewButton.Command = null;
            this.viewButton.Image = ((System.Drawing.Image)(resources.GetObject("viewButton.Image")));
            this.viewButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.viewButton.IsFlat = true;
            this.viewButton.IsPressed = false;
            this.viewButton.Location = new System.Drawing.Point(5, 5);
            this.viewButton.Margin = new System.Windows.Forms.Padding(1);
            this.viewButton.Name = "viewButton";
            this.viewButton.Padding = new System.Windows.Forms.Padding(2);
            this.viewButton.Size = new System.Drawing.Size(144, 52);
            this.viewButton.TabIndex = 1;
            this.viewButton.Text = "View";
            this.viewButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.viewButton.Click += new System.EventHandler(this.viewButton_MouseEnter);
            this.viewButton.MouseEnter += new System.EventHandler(this.viewButton_MouseEnter);
            // 
            // exportButton
            // 
            this.exportButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.exportButton.Command = null;
            this.exportButton.Image = ((System.Drawing.Image)(resources.GetObject("exportButton.Image")));
            this.exportButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.exportButton.IsFlat = true;
            this.exportButton.IsPressed = false;
            this.exportButton.Location = new System.Drawing.Point(5, 57);
            this.exportButton.Margin = new System.Windows.Forms.Padding(1);
            this.exportButton.Name = "exportButton";
            this.exportButton.Padding = new System.Windows.Forms.Padding(2);
            this.exportButton.Size = new System.Drawing.Size(144, 52);
            this.exportButton.TabIndex = 1;
            this.exportButton.Text = "Export";
            this.exportButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.exportButton.Click += new System.EventHandler(this.exportButton_MouseEnter);
            this.exportButton.MouseEnter += new System.EventHandler(this.exportButton_MouseEnter);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Silver;
            this.pictureBox1.Location = new System.Drawing.Point(0, 169);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(155, 1);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // saveAsButton
            // 
            this.saveAsButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.saveAsButton.Command = null;
            this.saveAsButton.IsFlat = true;
            this.saveAsButton.IsPressed = false;
            this.saveAsButton.Location = new System.Drawing.Point(5, 334);
            this.saveAsButton.Margin = new System.Windows.Forms.Padding(1);
            this.saveAsButton.Name = "saveAsButton";
            this.saveAsButton.Padding = new System.Windows.Forms.Padding(2);
            this.saveAsButton.Size = new System.Drawing.Size(144, 52);
            this.saveAsButton.TabIndex = 1;
            this.saveAsButton.Text = "Save As";
            this.saveAsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.saveAsButton.Click += new System.EventHandler(this.saveAsButton_Click);
            this.saveAsButton.MouseEnter += new System.EventHandler(this.MouseEnterDefault);
            // 
            // quitButton
            // 
            this.quitButton.AllowDrop = true;
            this.quitButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.quitButton.Command = null;
            this.quitButton.IsFlat = true;
            this.quitButton.IsPressed = false;
            this.quitButton.Location = new System.Drawing.Point(5, 521);
            this.quitButton.Margin = new System.Windows.Forms.Padding(1);
            this.quitButton.Name = "quitButton";
            this.quitButton.Padding = new System.Windows.Forms.Padding(2);
            this.quitButton.Size = new System.Drawing.Size(144, 52);
            this.quitButton.TabIndex = 1;
            this.quitButton.Text = "Quit";
            this.quitButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
            this.quitButton.MouseEnter += new System.EventHandler(this.MouseEnterDefault);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Silver;
            this.pictureBox2.Location = new System.Drawing.Point(0, 512);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(155, 1);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Silver;
            this.pictureBox3.Location = new System.Drawing.Point(0, 392);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(155, 1);
            this.pictureBox3.TabIndex = 4;
            this.pictureBox3.TabStop = false;
            // 
            // ribbonButton1
            // 
            this.ribbonButton1.AllowDrop = true;
            this.ribbonButton1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton1.Command = null;
            this.ribbonButton1.IsFlat = true;
            this.ribbonButton1.IsPressed = false;
            this.ribbonButton1.Location = new System.Drawing.Point(5, 401);
            this.ribbonButton1.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton1.Name = "ribbonButton1";
            this.ribbonButton1.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton1.Size = new System.Drawing.Size(144, 52);
            this.ribbonButton1.TabIndex = 3;
            this.ribbonButton1.Text = "Print";
            this.ribbonButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton1.Click += new System.EventHandler(this.ribbonButton1_Click);
            // 
            // ribbonButton2
            // 
            this.ribbonButton2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton2.Command = null;
            this.ribbonButton2.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton2.Image")));
            this.ribbonButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton2.IsFlat = true;
            this.ribbonButton2.IsPressed = false;
            this.ribbonButton2.Location = new System.Drawing.Point(5, 109);
            this.ribbonButton2.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton2.Name = "ribbonButton2";
            this.ribbonButton2.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton2.Size = new System.Drawing.Size(144, 52);
            this.ribbonButton2.TabIndex = 1;
            this.ribbonButton2.Text = "Templates";
            this.ribbonButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton2.Click += new System.EventHandler(this.templatesButton_MouseEnter);
            this.ribbonButton2.MouseEnter += new System.EventHandler(this.templatesButton_MouseEnter);
            // 
            // ribbonButton3
            // 
            this.ribbonButton3.AllowDrop = true;
            this.ribbonButton3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton3.Command = null;
            this.ribbonButton3.IsFlat = true;
            this.ribbonButton3.IsPressed = false;
            this.ribbonButton3.Location = new System.Drawing.Point(5, 453);
            this.ribbonButton3.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton3.Name = "ribbonButton3";
            this.ribbonButton3.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton3.Size = new System.Drawing.Size(144, 52);
            this.ribbonButton3.TabIndex = 3;
            this.ribbonButton3.Text = "Print Preview";
            this.ribbonButton3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton3.Click += new System.EventHandler(this.ribbonButton3_Click);
            // 
            // NuGenMainPopupMenu
            // 
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(155, 581);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.ribbonButton3);
            this.Controls.Add(this.ribbonButton1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.saveAsButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.viewButton);
            this.Controls.Add(this.quitButton);
            this.Controls.Add(this.newButton);
            this.Controls.Add(this.ribbonButton2);
            this.Controls.Add(this.exportButton);
            this.Name = "NuGenMainPopupMenu";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.NuGenPopupMenu_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);

        }

        void exportButton_MouseEnter(object sender, EventArgs e)
        {
            exportMenu.Location = ((RibbonButton)sender).PointToScreen(new Point(exportButton.Width, viewButton.Location.Y + 15));
            exportMenu.PoppingUp = true;
            exportMenu.Show();
            viewMenu.Hide();
            templatesMenu.Hide();
        }

        void saveAsButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Handler.SaveAs();
        }

        void newButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Handler.New();
        }

        public override void ChildDeactivated()
        {
            Rectangle clientArea = new Rectangle(0, 0, Width, Height);

            Point screenCoord = PointToClient(MousePosition);

            if(!clientArea.Contains(screenCoord) && ! Focused)            
                base.OnDeactivate(null);
        }

        void MouseEnterDefault(object sender, EventArgs e)
        {
            viewMenu.Hide();
            exportMenu.Hide();
            templatesMenu.Hide();
        }

        void viewButton_MouseEnter(object sender, EventArgs e)
        {
            viewMenu.Location = ((RibbonButton)sender).PointToScreen(new Point(viewButton.Width, viewButton.Location.Y + 5));
            viewMenu.PoppingUp = true;
            viewMenu.Show();
            exportMenu.Hide();
            templatesMenu.Hide();
        }

        void NuGenPopupMenu_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Pen p = new Pen(Brushes.Gray);
            e.Graphics.DrawRectangle(p, new Rectangle(0,0,Width -1, Height -1));
        }

        void openButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Handler.Open();
        }

        void saveButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Handler.Save();
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            } catch(Exception ex )
            {
                Application.Exit();
            }
        }

        private void ribbonButton1_Click(object sender, EventArgs e)
        {
            this.Hide();

            PrintDialog dlg = new PrintDialog();            

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Handler.Document().Print();
            }
        }

        private void ribbonButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            PrintPreviewDialog dlg = new PrintPreviewDialog();
            dlg.Document = Handler.Document();
            dlg.ShowDialog();
        }

        private void templatesButton_MouseEnter(object sender, EventArgs e)
        {
            templatesMenu.Location = ((RibbonButton)sender).PointToScreen(new Point(viewButton.Width, viewButton.Location.Y + 5));
            templatesMenu.PoppingUp = true;
            templatesMenu.Show();
            viewMenu.Hide();
            exportMenu.Hide();
        }
    }
}
