using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace Genetibase.MathX.NugenCCalc.Design.Controls
{
	public class EquationRepositoryView : Genetibase.MathX.NugenCCalc.Design.Controls.PropertyView
	{
		private bool _is3DDesigner = false;

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.Button btDelete;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btEdit;
		private System.Windows.Forms.Button btUIt;
		private System.Windows.Forms.Button btCreate;
		private Genetibase.MathX.NugenCCalc.Design.Controls.EquationExplorer equationExplorer;
		private System.ComponentModel.IContainer components = null;

		public EquationRepositoryView(NugenCCalcBase component)
		{
			_component = component;

			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		public EquationRepositoryView(NugenCCalcBase component, bool is3DDesigner)
		{
			_component = component;
			_is3DDesigner = is3DDesigner;
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EquationRepositoryView));
            this.label2 = new System.Windows.Forms.Label();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.btDelete = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btEdit = new System.Windows.Forms.Button();
            this.btUIt = new System.Windows.Forms.Button();
            this.btCreate = new System.Windows.Forms.Button();
            this.equationExplorer = new Genetibase.MathX.NugenCCalc.Design.Controls.EquationExplorer();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.ImageIndex = 0;
            this.label2.ImageList = this.imageList;
            this.label2.Location = new System.Drawing.Point(120, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(336, 32);
            this.label2.TabIndex = 20;
            this.label2.Text = "Equation Repository";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            // 
            // btDelete
            // 
            this.btDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btDelete.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btDelete.Location = new System.Drawing.Point(384, 120);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(75, 23);
            this.btDelete.TabIndex = 18;
            this.btDelete.Tag = "Delete equation";
            this.btDelete.Text = "Delete";
            this.btDelete.UseVisualStyleBackColor = false;
            this.btDelete.Click += new System.EventHandler(this.button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(0, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(456, 8);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            // 
            // btEdit
            // 
            this.btEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btEdit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btEdit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btEdit.Location = new System.Drawing.Point(384, 88);
            this.btEdit.Name = "btEdit";
            this.btEdit.Size = new System.Drawing.Size(75, 23);
            this.btEdit.TabIndex = 17;
            this.btEdit.Tag = "Edit equation";
            this.btEdit.Text = "Edit";
            this.btEdit.UseVisualStyleBackColor = false;
            this.btEdit.Click += new System.EventHandler(this.button_Click);
            // 
            // btUIt
            // 
            this.btUIt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btUIt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btUIt.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btUIt.Location = new System.Drawing.Point(384, 416);
            this.btUIt.Name = "btUIt";
            this.btUIt.Size = new System.Drawing.Size(75, 23);
            this.btUIt.TabIndex = 15;
            this.btUIt.Tag = "Paste equation data to component";
            this.btUIt.Text = "Use It!";
            this.btUIt.UseVisualStyleBackColor = false;
            this.btUIt.Click += new System.EventHandler(this.button_Click);
            // 
            // btCreate
            // 
            this.btCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCreate.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btCreate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCreate.Location = new System.Drawing.Point(384, 56);
            this.btCreate.Name = "btCreate";
            this.btCreate.Size = new System.Drawing.Size(75, 23);
            this.btCreate.TabIndex = 16;
            this.btCreate.Tag = "Create new equation";
            this.btCreate.Text = "Create";
            this.btCreate.UseVisualStyleBackColor = false;
            this.btCreate.Click += new System.EventHandler(this.button_Click);
            // 
            // equationExplorer
            // 
            this.equationExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.equationExplorer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.equationExplorer.Location = new System.Drawing.Point(0, 56);
            this.equationExplorer.Name = "equationExplorer";
            this.equationExplorer.Size = new System.Drawing.Size(376, 384);
            this.equationExplorer.TabIndex = 21;
            this.equationExplorer.OnItemAction += new Genetibase.MathX.NugenCCalc.Design.ItemActionEventHandler(this.equationExplorer_OnItemAction);
            // 
            // EquationRepositoryView
            // 
            this.Controls.Add(this.equationExplorer);
            this.Controls.Add(this.btDelete);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btEdit);
            this.Controls.Add(this.btUIt);
            this.Controls.Add(this.btCreate);
            this.Controls.Add(this.label2);
            this.Name = "EquationRepositoryView";
            this.Size = new System.Drawing.Size(464, 448);
            this.Load += new System.EventHandler(this.EquationRepositoryView_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void EquationRepositoryView_Load(object sender, System.EventArgs e)
		{
			if (_is3DDesigner)
				this.equationExplorer.Is3DDesigner = true;
			this.equationExplorer.Equations = PredefinedSettings.Instance.Equations;
			this.equationExplorer.Init();
			BindEvents();
		}

		/// <summary>
		/// 
		/// </summary>
		private void BindEvents()
		{
			PredefinedSettings.Instance.Equations.OnChange +=new EventHandler(Equations_OnChange);
			PredefinedSettings.Instance.Equations.OnItemChange+=new ParametersChangeHandler(Equations_OnItemChange);
		}

		private void CreateEquation()
		{
			EquationForm frmEquation = new EquationForm();
			if (_is3DDesigner)
				frmEquation.Component = new NugenCCalc3D();
			else
				frmEquation.Component = new NugenCCalc2D();
			if (frmEquation.ShowDialog() == DialogResult.OK)
			{
				PredefinedSettings.Instance.Equations.Add(frmEquation.Component.FunctionParameters);
			}
			this.equationExplorer.Init();
		}


		private void DeleteEquation(FunctionParameters equation)
		{
			if (equation != null)
			{
				PredefinedSettings.Instance.Equations.Remove(equation);
				this.equationExplorer.Init();
			}
		}


		private void EditEquation(FunctionParameters equation)
		{
			if (equation != null)
			{
				FunctionParameters newEquation = (FunctionParameters)equation.Clone();

				NugenCCalcBase component = null;
				if (_is3DDesigner)
				{
					component = new NugenCCalc3D();
					component.FunctionParameters = (Function3DParameters)newEquation;
				}
				else
				{
					component = new NugenCCalc2D();
					component.FunctionParameters = (Function2DParameters)newEquation;
				}


				EquationForm frmEquation = new EquationForm(component);

				if (frmEquation.ShowDialog() == DialogResult.OK)
				{
					equation.Name = component.FunctionParameters.Name;
					equation.Formula = component.FunctionParameters.Formula;
					//equation.FunctionType = frmEquation.Equation.FunctionType;
				}
			}
			this.equationExplorer.Init();
		}


		private void UseEquation(FunctionParameters equation)
		{
			if (equation != null)
			{
				if (MessageBox.Show(this, "Are you sure that you want use this equation as source for component?", "Equation repository", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
                    //_component.FunctionParameters = equation;
                    _component.FunctionParameters = (FunctionParameters)Activator.CreateInstance(equation.GetType());
                    _component.FunctionParameters.Formula = equation.Formula;
                    if (_component.FunctionParameters.GetType() == typeof(Parametric2DParameters))
                        (_component.FunctionParameters as Parametric2DParameters).FormulaY = (equation as Parametric2DParameters).FormulaY;
                    if (_component.FunctionParameters.GetType() == typeof(Parametric3DParameters))
                    {
                        (_component.FunctionParameters as Parametric3DParameters).FormulaY = (equation as Parametric3DParameters).FormulaY;
                        (_component.FunctionParameters as Parametric3DParameters).FormulaZ = (equation as Parametric3DParameters).FormulaZ;
                    }
                    if (_component.FunctionParameters.GetType() == typeof(ParametricSurfaceParameters))
                    {
                        (_component.FunctionParameters as ParametricSurfaceParameters).FormulaY = (equation as ParametricSurfaceParameters).FormulaY;
                        (_component.FunctionParameters as ParametricSurfaceParameters).FormulaZ = (equation as ParametricSurfaceParameters).FormulaZ;
                    }
                    _component.FunctionParameters.Name = equation.Name;
                    _component.FunctionParameters.SourceType = SourceType.Equation;

				}
			}
		}


		private void equationExplorer_OnItemAction(object sender, ItemEventArgs e)
		{
			switch(e.Action)
			{
				case "Create":
					CreateEquation();
					break;
				case "Edit":
					EditEquation(e.Item);
					break;
				case "Delete":
					DeleteEquation(e.Item);
					break;
				case "Use It!":
					UseEquation(e.Item);
					break;
				default:
					MessageBox.Show(this, "Unknown menu item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
			}
		}


		private void button_Click(object sender, System.EventArgs e)
		{
			switch(((Button)sender).Text)
			{
				case "Create":
					CreateEquation();
					break;
				case "Delete":
					DeleteEquation(equationExplorer.Selected);
					break;
				case "Edit":
					EditEquation(equationExplorer.Selected);
					break;
				case "Use It!":
					UseEquation(equationExplorer.Selected);
					break;		
				default:
					MessageBox.Show(this, "Unknown button", "Equation Repository", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;		
			}
		}


		private void Equations_OnChange(object sender, EventArgs e)
		{
			PredefinedSettings.SavePredefinedSettings();
		}


		private void Equations_OnItemChange(FunctionParameters item, EventArgs e)
		{
			PredefinedSettings.SavePredefinedSettings();
		}

	}
}

