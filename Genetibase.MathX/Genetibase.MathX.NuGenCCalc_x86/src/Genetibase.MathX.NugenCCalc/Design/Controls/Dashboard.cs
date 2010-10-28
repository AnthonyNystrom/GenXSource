using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Genetibase.MathX.NugenCCalc.Design.Controls
{
	public delegate void DashboardItemChangeHandler(object sender, StringArgs s);

	/// <summary>
	/// Summary description for Dashboard.
	/// </summary>
	[ToolboxItem(false)]
	public class Dashboard : System.Windows.Forms.UserControl
	{
        private System.Windows.Forms.ImageList imageList;
        private System.ComponentModel.IContainer components;
        private Panel panel1;
        private FlatButton btnSource;
        private FlatButton btnDestination;
        private FlatButton flatButton1;
        private FlatButton btnExpressionRepository;
		private ListViewItem _currentItem;
		public event DashboardItemChangeHandler OnDashboardItemChange;

		public Dashboard()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.flatButton1 = new Genetibase.MathX.NugenCCalc.Design.Controls.FlatButton();
            this.btnExpressionRepository = new Genetibase.MathX.NugenCCalc.Design.Controls.FlatButton();
            this.btnDestination = new Genetibase.MathX.NugenCCalc.Design.Controls.FlatButton();
            this.btnSource = new Genetibase.MathX.NugenCCalc.Design.Controls.FlatButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "");
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.flatButton1);
            this.panel1.Controls.Add(this.btnExpressionRepository);
            this.panel1.Controls.Add(this.btnDestination);
            this.panel1.Controls.Add(this.btnSource);
            this.panel1.Location = new System.Drawing.Point(8, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(147, 376);
            this.panel1.TabIndex = 2;
            // 
            // flatButton1
            // 
            this.flatButton1.HighlightColor = System.Drawing.Color.Gainsboro;
            this.flatButton1.HighlightHoverColor = System.Drawing.Color.DarkGray;
            this.flatButton1.HoverColor = System.Drawing.Color.Silver;
            this.flatButton1.Image = ((System.Drawing.Image)(resources.GetObject("flatButton1.Image")));
            this.flatButton1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.flatButton1.Location = new System.Drawing.Point(0, 240);
            this.flatButton1.Name = "flatButton1";
            this.flatButton1.Size = new System.Drawing.Size(145, 80);
            this.flatButton1.TabIndex = 3;
            this.flatButton1.Text = "Equation Repository";
            this.flatButton1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.flatButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.flatButton1.UseVisualStyleBackColor = true;
            this.flatButton1.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnExpressionRepository
            // 
            this.btnExpressionRepository.HighlightColor = System.Drawing.Color.Gainsboro;
            this.btnExpressionRepository.HighlightHoverColor = System.Drawing.Color.DarkGray;
            this.btnExpressionRepository.HoverColor = System.Drawing.Color.Silver;
            this.btnExpressionRepository.Image = ((System.Drawing.Image)(resources.GetObject("btnExpressionRepository.Image")));
            this.btnExpressionRepository.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExpressionRepository.Location = new System.Drawing.Point(0, 160);
            this.btnExpressionRepository.Name = "btnExpressionRepository";
            this.btnExpressionRepository.Size = new System.Drawing.Size(145, 80);
            this.btnExpressionRepository.TabIndex = 2;
            this.btnExpressionRepository.Text = "Code Expression Repository";
            this.btnExpressionRepository.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExpressionRepository.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExpressionRepository.UseVisualStyleBackColor = true;
            this.btnExpressionRepository.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnDestination
            // 
            this.btnDestination.HighlightColor = System.Drawing.Color.Gainsboro;
            this.btnDestination.HighlightHoverColor = System.Drawing.Color.DarkGray;
            this.btnDestination.HoverColor = System.Drawing.Color.Silver;
            this.btnDestination.Image = ((System.Drawing.Image)(resources.GetObject("btnDestination.Image")));
            this.btnDestination.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDestination.Location = new System.Drawing.Point(0, 80);
            this.btnDestination.Name = "btnDestination";
            this.btnDestination.Size = new System.Drawing.Size(145, 80);
            this.btnDestination.TabIndex = 1;
            this.btnDestination.Text = "Destination Properties";
            this.btnDestination.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDestination.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDestination.UseVisualStyleBackColor = true;
            this.btnDestination.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnSource
            // 
            this.btnSource.HighlightColor = System.Drawing.Color.Gainsboro;
            this.btnSource.HighlightHoverColor = System.Drawing.Color.DarkGray;
            this.btnSource.HoverColor = System.Drawing.Color.Silver;
            this.btnSource.Image = ((System.Drawing.Image)(resources.GetObject("btnSource.Image")));
            this.btnSource.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSource.Location = new System.Drawing.Point(0, 0);
            this.btnSource.Name = "btnSource";
            this.btnSource.Size = new System.Drawing.Size(145, 80);
            this.btnSource.TabIndex = 0;
            this.btnSource.Text = "Source Properties";
            this.btnSource.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSource.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSource.UseVisualStyleBackColor = true;
            this.btnSource.Checked = true;
            this.btnSource.Click += new System.EventHandler(this.btn_Click);
            // 
            // Dashboard
            // 
            this.BackColor = System.Drawing.Color.DarkKhaki;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "Dashboard";
            this.Size = new System.Drawing.Size(163, 384);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		protected void DashboardItemChange(object sender, StringArgs e)
		{
			if (OnDashboardItemChange!=null)
			{
				OnDashboardItemChange(this, e);
			}
		}

        private void btn_Click(object sender, EventArgs e)
        {
            DashboardItemChange(this, new StringArgs((sender as FlatButton).Text));
        }
	}
}
