using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Genetibase.NuGenMediImage.UI.Controls;
using Genetibase.NuGenMediImage.UI.Dialogs;
using Genetibase.NuGenMediImage.Handlers;
using System.ComponentModel.Design;


namespace Genetibase.NuGenMediImage.UI.Menus
{
    public partial class PhotoMenu : RibbonPopup
    {
        private bool cancelclose;
        private NuGenMediImageCtrl parent = null;
        private NuGenMediImageCtrl ngMediImage = null;
        
        MenuButtonCollection mbc;

        internal System.Windows.Forms.Control.ControlCollection MenuItemControls
        {
            get
            {   
                return this.flowLayoutPanel1.Controls;
            }            
        }

        public PhotoMenu(NuGenMediImageCtrl parent)
        {
        	mbc = new MenuButtonCollection(this.flowLayoutPanel1);
        	mbc.CollectionChanged+= new EventHandler(mbc_CollectionChanged);
            InitializeComponent();
            this.parent = parent;
            ngMediImage = (NuGenMediImageCtrl)this.parent;

            this.btnExit.NgMediImage = ngMediImage;
            this.btnHelp.NgMediImage = ngMediImage;
            this.btnPrint.NgMediImage = ngMediImage;
            this.btnSaveAs.NgMediImage = ngMediImage;
            this.btnLoad.NgMediImage = ngMediImage;
            this.ribbonButton6.NgMediImage = ngMediImage;

            this.flowLayoutPanel1.ControlAdded += new ControlEventHandler(flowLayoutPanel1_ControlAdded);
            this.flowLayoutPanel1.ControlRemoved += new ControlEventHandler(flowLayoutPanel1_ControlRemoved);
            SizeMenu();
            
        }


        void flowLayoutPanel1_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control.GetType() == typeof(Genetibase.NuGenMediImage.UI.Controls.RibbonButton))
            {
                RibbonButton button = (RibbonButton)e.Control;

                button.Dock = System.Windows.Forms.DockStyle.Fill;
                button.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                button.ForeColor = System.Drawing.Color.Black;
                button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                button.IsFlat = true;
                button.IsPressed = false;
                button.Location = new System.Drawing.Point(1, 179);
                button.Margin = new System.Windows.Forms.Padding(1);
                button.Name = "button";
                button.Padding = new System.Windows.Forms.Padding(2);
                button.Size = new System.Drawing.Size(125, 39);
                button.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;                
                button.NgMediImage = this.ngMediImage;
            }

            SizeMenu();
        }

        void flowLayoutPanel1_ControlRemoved(object sender, ControlEventArgs e)
        {
            SizeMenu();
        }

        internal void AddSeperator()
        {
            PictureBox p = new PictureBox();
            p.Size = pictureBox1.Size;
            p.BackColor = System.Drawing.Color.Silver;
            //p.Location = pictureBox1.Location;

            this.flowLayoutPanel1.Controls.Add(p);
        }

        internal void AddButton(Genetibase.NuGenMediImage.UI.Controls.RibbonButton button)
        {
            button.SizeChanged += new EventHandler(button_SizeChanged);
            this.flowLayoutPanel1.Controls.Add(button);            
        }

        void button_SizeChanged(object sender, EventArgs e)
        {
            Genetibase.NuGenMediImage.UI.Controls.RibbonButton button = (Genetibase.NuGenMediImage.UI.Controls.RibbonButton)sender;

            if (button.Height != 39)
                button.Height = 39;
        }


        private void SizeMenu()
        {
            int height = 0;
            int count = 0;

            foreach (Control c in this.flowLayoutPanel1.Controls)
            {
                count+=4;

                height += c.Height;
            }

            height += count;
            height += 5;

            this.Height = height;
            this.flowLayoutPanel1.Height = height;
        }

        private void PopulateMBC()
        {
        	
        }
        
        private void mbc_CollectionChanged(object sender, EventArgs e)
        {
        
        }

        private void PhotoMenu_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(115, 115, 115)), 0, 0, this.Width - 1, this.Height - 1);
        }

        private void PhotoMenu_Deactivate(object sender, EventArgs e)
        {
               
                   if (!this.cancelclose)
                       this.Hide();        
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public new void Hide()
        {
            ngMediImage.popup = false;
            base.Hide();
        }

        public new void Close()
        {
            ngMediImage.popup = false;
            base.Close();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.cancelclose = true;
            DialogResult res = openFileDialog1.ShowDialog();
            
            Application.DoEvents();

            if (res == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;                

                switch (openFileDialog1.FilterIndex)
                {
                    case 1:
                        ngMediImage.LoadImage(fileName);
                        break;

                    case 2:
                        ngMediImage.LoadPPMImage(fileName);
                        break;


                    case 3:
                        ngMediImage.LoadAnalyzeImage(fileName);
                        break;

                    case 4:
                        ngMediImage.LoadInterImage(fileName);
                        break;

                    case 5:
                        ngMediImage.LoadDICOMImage(fileName);
                        break;

                    case 6:
                        this.Hide();
                        DlgOpenRaw dlgOpenRaw = new DlgOpenRaw();

                        if (dlgOpenRaw.ShowDialog() == DialogResult.OK)
                        {
                            int width = (int)dlgOpenRaw.numWidth.Value;
                            int height = (int)dlgOpenRaw.numHeight.Value;

                            int offset = (int)dlgOpenRaw.numOffset.Value;
                            int bits = (int)dlgOpenRaw.numBits.Value;
                            int slices = (int)dlgOpenRaw.numSlices.Value;

                            RAWImage.Format format = RAWImage.Format.Interleaved;

                            if (dlgOpenRaw.chkBoxPlanar.Checked)
                                format = RAWImage.Format.Planar;
                            ngMediImage.LoadRawImage(fileName, bits, offset, width, height, slices, format);
                            this.Hide();    
                        }
                        break;
                }
            }

            Application.DoEvents();
            this.cancelclose = false;
            this.Hide();
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            if (savePictureFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (savePictureFileDialog.FilterIndex == 1 || savePictureFileDialog.FilterIndex == 2)
                {
                    ImageFormat fmt = ImageFormat.Jpeg;
                    if (savePictureFileDialog.FilterIndex == 2)
                        fmt = ImageFormat.Bmp;

                    Application.DoEvents();
                    ngMediImage.Save(savePictureFileDialog.FileName, fmt);
                    Application.DoEvents();
                }
                else if (savePictureFileDialog.FilterIndex == 3)
                {
                    Application.DoEvents();

                    ngMediImage.SaveAnnotation(savePictureFileDialog.FileName);

                    Application.DoEvents();
                }
            }
            this.Hide();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.cancelclose = true;
            ngMediImage.Print();
            Application.DoEvents();
            this.cancelclose = false;
            this.Hide();            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.cancelclose = true;
            ngMediImage.CloseFile();
            Application.DoEvents();
            this.cancelclose = false;
            System.GC.Collect();
            this.Hide();       
        }
    }
    
    public class MenuButtonCollection: System.Windows.Forms.Control.ControlCollection
    {
    	public MenuButtonCollection( Control ctrl ): base(ctrl){}
    	
    	public event EventHandler CollectionChanged;
    	
    	public void FireCollectionChanged()
    	{
    		if( CollectionChanged != null )
    		{
    			CollectionChanged(this,EventArgs.Empty);
    		}
    	}
    	
		public void AddRange(RibbonButton[] controls)
		{
			base.AddRange(controls);
		}
    	
		public void Add(RibbonButton value)
		{
			base.Add(value);
			FireCollectionChanged();
		}
		
		public int GetChildIndex(RibbonButton child, bool throwException)
		{
			return base.GetChildIndex(child, throwException);
		}
		
		public void Remove(RibbonButton value)
		{
			base.Remove(value);
			FireCollectionChanged();
		}
		
		public void SetChildIndex(RibbonButton child, int newIndex)
		{
			base.SetChildIndex(child, newIndex);
			FireCollectionChanged();
		}
		
		public new RibbonButton this[string key]
		{
			get 
			{
				return (RibbonButton) base[key];
			}
		}
    }
    
}

