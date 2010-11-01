using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using UseCaseMakerLibrary;

namespace UseCaseMaker
{
	/// <summary>
	/// Descrizione di riepilogo per frmRefSelector.
	/// </summary>
	public class frmRefSelector : System.Windows.Forms.Form
	{
		private Model model;
		private UseCase caller;
		private DependencyItem.ReferenceType callerRefType;
		private UseCase selected;
		private Localizer localizer;

		private System.Windows.Forms.Label lblStereotypeTitle;
		private System.Windows.Forms.TextBox tbStereotype;
		private System.Windows.Forms.Label lblUpperUseCase;
		private System.Windows.Forms.Label lblLowerUseCase;
		private System.Windows.Forms.Label lblDepFromTitle;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TreeView tvModelBrowser;
		private System.Windows.Forms.ImageList imgListModelBrowser;
		private System.Windows.Forms.GroupBox gbRelationship;
		private System.Windows.Forms.Label lblUseCaseTitle;
		private System.Windows.Forms.Button btnSwap;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblStereotype;
		private System.ComponentModel.IContainer components;

		public frmRefSelector(UseCase caller, Model model, Localizer localizer)
		{
			//
			// Necessario per il supporto di Progettazione Windows Form
			//
			InitializeComponent();

			//
			// TODO: aggiungere il codice del costruttore dopo la chiamata a InitializeComponent
			//
			this.caller = caller;
			this.model = model;
			this.localizer = localizer;
			this.localizer.LocalizeControls(this);
			this.lblUpperUseCase.Text = caller.Name;

			BuildView(this.model);

			this.lblLowerUseCase.Text = "";
			this.selected = null;
			this.btnOK.Enabled = false;
			this.btnSwap.Enabled = false;
			this.tvModelBrowser.SelectedNode = null;
		}

		/// <summary>
		/// Pulire le risorse in uso.
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

		#region Codice generato da Progettazione Windows Form
		/// <summary>
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmRefSelector));
			this.lblStereotypeTitle = new System.Windows.Forms.Label();
			this.tbStereotype = new System.Windows.Forms.TextBox();
			this.lblUseCaseTitle = new System.Windows.Forms.Label();
			this.tvModelBrowser = new System.Windows.Forms.TreeView();
			this.imgListModelBrowser = new System.Windows.Forms.ImageList(this.components);
			this.gbRelationship = new System.Windows.Forms.GroupBox();
			this.lblStereotype = new System.Windows.Forms.Label();
			this.btnSwap = new System.Windows.Forms.Button();
			this.lblDepFromTitle = new System.Windows.Forms.Label();
			this.lblLowerUseCase = new System.Windows.Forms.Label();
			this.lblUpperUseCase = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.gbRelationship.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblStereotypeTitle
			// 
			this.lblStereotypeTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblStereotypeTitle.Location = new System.Drawing.Point(8, 8);
			this.lblStereotypeTitle.Name = "lblStereotypeTitle";
			this.lblStereotypeTitle.Size = new System.Drawing.Size(120, 16);
			this.lblStereotypeTitle.TabIndex = 0;
			this.lblStereotypeTitle.Text = "[Stereotype]";
			// 
			// tbStereotype
			// 
			this.tbStereotype.Location = new System.Drawing.Point(136, 8);
			this.tbStereotype.Name = "tbStereotype";
			this.tbStereotype.Size = new System.Drawing.Size(360, 20);
			this.tbStereotype.TabIndex = 1;
			this.tbStereotype.Text = "";
			this.tbStereotype.TextChanged += new System.EventHandler(this.tbStereotype_TextChanged);
			// 
			// lblUseCaseTitle
			// 
			this.lblUseCaseTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblUseCaseTitle.Location = new System.Drawing.Point(8, 40);
			this.lblUseCaseTitle.Name = "lblUseCaseTitle";
			this.lblUseCaseTitle.Size = new System.Drawing.Size(120, 16);
			this.lblUseCaseTitle.TabIndex = 2;
			this.lblUseCaseTitle.Text = "[Use case]";
			// 
			// tvModelBrowser
			// 
			this.tvModelBrowser.HideSelection = false;
			this.tvModelBrowser.ImageList = this.imgListModelBrowser;
			this.tvModelBrowser.Location = new System.Drawing.Point(136, 40);
			this.tvModelBrowser.Name = "tvModelBrowser";
			this.tvModelBrowser.Size = new System.Drawing.Size(360, 128);
			this.tvModelBrowser.TabIndex = 3;
			this.tvModelBrowser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvModelBrowser_AfterSelect);
			// 
			// imgListModelBrowser
			// 
			this.imgListModelBrowser.ImageSize = new System.Drawing.Size(16, 16);
			this.imgListModelBrowser.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListModelBrowser.ImageStream")));
			this.imgListModelBrowser.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// gbRelationship
			// 
			this.gbRelationship.Controls.Add(this.lblStereotype);
			this.gbRelationship.Controls.Add(this.btnSwap);
			this.gbRelationship.Controls.Add(this.lblDepFromTitle);
			this.gbRelationship.Controls.Add(this.lblLowerUseCase);
			this.gbRelationship.Controls.Add(this.lblUpperUseCase);
			this.gbRelationship.Controls.Add(this.pictureBox1);
			this.gbRelationship.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.gbRelationship.Location = new System.Drawing.Point(8, 176);
			this.gbRelationship.Name = "gbRelationship";
			this.gbRelationship.Size = new System.Drawing.Size(488, 184);
			this.gbRelationship.TabIndex = 4;
			this.gbRelationship.TabStop = false;
			this.gbRelationship.Text = "[Relationship]";
			// 
			// lblStereotype
			// 
			this.lblStereotype.BackColor = System.Drawing.Color.White;
			this.lblStereotype.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblStereotype.Location = new System.Drawing.Point(16, 96);
			this.lblStereotype.Name = "lblStereotype";
			this.lblStereotype.Size = new System.Drawing.Size(160, 23);
			this.lblStereotype.TabIndex = 9;
			this.lblStereotype.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnSwap
			// 
			this.btnSwap.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnSwap.Location = new System.Drawing.Point(376, 24);
			this.btnSwap.Name = "btnSwap";
			this.btnSwap.Size = new System.Drawing.Size(104, 24);
			this.btnSwap.TabIndex = 4;
			this.btnSwap.Text = "[Swap]";
			this.btnSwap.Click += new System.EventHandler(this.btnSwapUseCases_Click);
			// 
			// lblDepFromTitle
			// 
			this.lblDepFromTitle.BackColor = System.Drawing.Color.White;
			this.lblDepFromTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblDepFromTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblDepFromTitle.Location = new System.Drawing.Point(232, 48);
			this.lblDepFromTitle.Name = "lblDepFromTitle";
			this.lblDepFromTitle.Size = new System.Drawing.Size(128, 24);
			this.lblDepFromTitle.TabIndex = 6;
			this.lblDepFromTitle.Text = "[depends from]";
			this.lblDepFromTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblLowerUseCase
			// 
			this.lblLowerUseCase.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.lblLowerUseCase.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblLowerUseCase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblLowerUseCase.Location = new System.Drawing.Point(160, 128);
			this.lblLowerUseCase.Name = "lblLowerUseCase";
			this.lblLowerUseCase.Size = new System.Drawing.Size(160, 24);
			this.lblLowerUseCase.TabIndex = 3;
			this.lblLowerUseCase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblUpperUseCase
			// 
			this.lblUpperUseCase.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.lblUpperUseCase.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblUpperUseCase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblUpperUseCase.Location = new System.Drawing.Point(56, 48);
			this.lblUpperUseCase.Name = "lblUpperUseCase";
			this.lblUpperUseCase.Size = new System.Drawing.Size(152, 24);
			this.lblUpperUseCase.TabIndex = 2;
			this.lblUpperUseCase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 24);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(360, 152);
			this.pictureBox1.TabIndex = 8;
			this.pictureBox1.TabStop = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.CausesValidation = false;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(256, 368);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(120, 24);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "[Cancel]";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Location = new System.Drawing.Point(128, 368);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(120, 24);
			this.btnOK.TabIndex = 5;
			this.btnOK.Text = "[OK]";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// frmRefSelector
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(506, 400);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.gbRelationship);
			this.Controls.Add(this.tvModelBrowser);
			this.Controls.Add(this.lblUseCaseTitle);
			this.Controls.Add(this.tbStereotype);
			this.Controls.Add(this.lblStereotypeTitle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmRefSelector";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "[Select use case dependency]";
			this.gbRelationship.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	
		public String Stereotype
		{
			get
			{
				return this.tbStereotype.Text;
			}
		}

		public UseCase SelectedUseCase
		{
			get
			{
				return this.selected;
			}
		}

		public DependencyItem.ReferenceType ReferenceType
		{
			get
			{
				return this.callerRefType;
			}
		}
		
		private void BuildView(object element)
		{
			if(element.GetType() == typeof(Model))
			{
				Model model = (Model)element;
				AddElement(model,null);
				foreach(UseCase useCase in model.UseCases.Sorted("ID"))
				{
					useCase.Owner = model;
					AddElement(useCase,useCase.Owner);
				}
				foreach(Package subPackage in model.Packages.Sorted("ID"))
				{
					subPackage.Owner = model;
					BuildView(subPackage);
				}
			}
			if(element.GetType() == typeof(Package))
			{
				Package package = (Package)element;
				AddElement(package,package.Owner);
				foreach(UseCase useCase in package.UseCases.Sorted("ID"))
				{
					useCase.Owner = package;
					AddElement(useCase,useCase.Owner);
				}
				foreach(Package subPackage in package.Packages.Sorted("ID"))
				{
					subPackage.Owner = package;
					BuildView(subPackage);
				}
			}
		}

		private void AddElement(object element, object owner)
		{
			String ownerUniqueID = String.Empty;

			if(element.GetType() == typeof(Model))
			{
				Model model = (Model)element;
				tvModelBrowser.Nodes.Clear();
				TreeNode node = new TreeNode(model.Name + " (" + model.ElementID + ")");
				node.Tag = model.UniqueID;
				tvModelBrowser.Nodes.Add(node);
				tvModelBrowser.SelectedNode = node;
				TreeNode ownerNode = node;
				node = new TreeNode(this.localizer.GetValue("Globals","UseCases"),1,1);
				node.Tag = model.UseCases.UniqueID;
				ownerNode.Nodes.Add(node);
			}

			if(element.GetType() == typeof(Package))
			{
				Package package = (Package)element;
				ownerUniqueID = ((Package)owner).UniqueID;
				TreeNode node = new TreeNode(package.Name + " (" + package.ElementID + ")");
				node.Tag = package.UniqueID;
				TreeNode ownerNode = this.FindNode(null,ownerUniqueID);
				if(ownerNode != null)
				{
					ownerNode.Nodes.Add(node);
					tvModelBrowser.SelectedNode = node;
					ownerNode = node;
					node = new TreeNode(this.localizer.GetValue("Globals","UseCases"),1,1);
					node.Tag = package.UseCases.UniqueID;
					ownerNode.Nodes.Add(node);
				}
			}
			if(element.GetType() == typeof(UseCase))
			{
				UseCase useCase = (UseCase)element;
				Package package = (Package)owner;
				TreeNode node = new TreeNode(useCase.Name + " (" + useCase.ElementID + ")",2,2);
				node.Tag = useCase.UniqueID;
				TreeNode ownerNode = this.FindNode(null,package.UniqueID);
				if(ownerNode != null)
				{
					foreach(TreeNode subNode in ownerNode.Nodes)
					{
						if((String)subNode.Tag == package.UseCases.UniqueID)
						{
							subNode.Nodes.Add(node);
							tvModelBrowser.SelectedNode = node;
							break;
						}
					}
				}
			}
		}

		private TreeNode FindNode(TreeNode parent, String parentUniqueID)
		{
			TreeNode node = null;
			TreeNode retNode = null;

			if(tvModelBrowser.Nodes.Count == 0)
			{
				return null;
			}
			
			if(parent == null)
			{
				node = tvModelBrowser.Nodes[0];
			}
			else
			{
				node = parent;
			}

			if((String)node.Tag == parentUniqueID)
			{
				return node;
			}

			foreach(TreeNode child in node.Nodes)
			{
				if((String)child.Tag == parentUniqueID)
				{
					retNode = child;
					break;
				}
				if(child.Nodes.Count > 0)
				{
					retNode = this.FindNode(child,parentUniqueID);
					if(retNode != null)
					{
						break;
					}
				}
			}

			return retNode;
		}

		private void tvModelBrowser_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			TreeNode node = tvModelBrowser.SelectedNode;
			object element = model.FindElementByUniqueID((String)node.Tag);
			if(element.GetType() == typeof(UseCase))
			{
				selected = (UseCase)element;
				this.lblUpperUseCase.Text = caller.Name;
				this.lblLowerUseCase.Text = selected.Name;
				this.btnOK.Enabled = true;
				this.btnSwap.Enabled = true;
			}
		}

		private void btnSwapUseCases_Click(object sender, System.EventArgs e)
		{
			string tmp = this.lblUpperUseCase.Text;
			this.lblUpperUseCase.Text = this.lblLowerUseCase.Text;
			this.lblLowerUseCase.Text = tmp;
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			if(this.lblUpperUseCase.Text == this.caller.Name)
			{
				this.callerRefType = DependencyItem.ReferenceType.Client;
			}
			else
			{
				this.callerRefType = DependencyItem.ReferenceType.Supplier;
			}
		}

		private void tbStereotype_TextChanged(object sender, System.EventArgs e)
		{
			if(tbStereotype.Text != "")
			{
				lblStereotype.Text = "<<" + tbStereotype.Text + ">>";
			}
			else
			{
				lblStereotype.Text = "";
			}
		}
	}
}
