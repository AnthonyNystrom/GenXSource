using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.MathX.NugenCCalc.Design.Controls
{
	public class ExpressionRepositoryView : Genetibase.MathX.NugenCCalc.Design.Controls.PropertyView
	{
		private bool _is3DDesigner = false;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btUIt;
		private System.Windows.Forms.Button btDelete;
		private System.Windows.Forms.Button btEdit;
		private System.Windows.Forms.Button btCreate;
		private Genetibase.MathX.NugenCCalc.Design.Controls.ExpressionExplorer expressionExplorer;
		private System.ComponentModel.IContainer components = null;

		public ExpressionRepositoryView(NugenCCalcBase component)
		{
			_component = component;

			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		public ExpressionRepositoryView(NugenCCalcBase component, bool is3DDesigner)
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExpressionRepositoryView));
            this.label2 = new System.Windows.Forms.Label();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btUIt = new System.Windows.Forms.Button();
            this.btDelete = new System.Windows.Forms.Button();
            this.btEdit = new System.Windows.Forms.Button();
            this.btCreate = new System.Windows.Forms.Button();
            this.expressionExplorer = new Genetibase.MathX.NugenCCalc.Design.Controls.ExpressionExplorer();
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
            this.label2.Location = new System.Drawing.Point(40, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(360, 32);
            this.label2.TabIndex = 17;
            this.label2.Text = "Expression Repository";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(0, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 8);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            // 
            // btUIt
            // 
            this.btUIt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btUIt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btUIt.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btUIt.Location = new System.Drawing.Point(328, 368);
            this.btUIt.Name = "btUIt";
            this.btUIt.Size = new System.Drawing.Size(75, 23);
            this.btUIt.TabIndex = 15;
            this.btUIt.Tag = "Paste expression data to component";
            this.btUIt.Text = "Use It!";
            this.btUIt.UseVisualStyleBackColor = false;
            this.btUIt.Click += new System.EventHandler(this.button_Click);
            // 
            // btDelete
            // 
            this.btDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btDelete.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btDelete.Location = new System.Drawing.Point(328, 120);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(75, 23);
            this.btDelete.TabIndex = 12;
            this.btDelete.Tag = "Delete expression";
            this.btDelete.Text = "Delete";
            this.btDelete.UseVisualStyleBackColor = false;
            this.btDelete.Click += new System.EventHandler(this.button_Click);
            // 
            // btEdit
            // 
            this.btEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btEdit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btEdit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btEdit.Location = new System.Drawing.Point(328, 88);
            this.btEdit.Name = "btEdit";
            this.btEdit.Size = new System.Drawing.Size(75, 23);
            this.btEdit.TabIndex = 14;
            this.btEdit.Tag = "Edit expression";
            this.btEdit.Text = "Edit";
            this.btEdit.UseVisualStyleBackColor = false;
            this.btEdit.Click += new System.EventHandler(this.button_Click);
            // 
            // btCreate
            // 
            this.btCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCreate.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btCreate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCreate.Location = new System.Drawing.Point(328, 56);
            this.btCreate.Name = "btCreate";
            this.btCreate.Size = new System.Drawing.Size(75, 23);
            this.btCreate.TabIndex = 13;
            this.btCreate.Tag = "Create new expression";
            this.btCreate.Text = "Create";
            this.btCreate.UseVisualStyleBackColor = false;
            this.btCreate.Click += new System.EventHandler(this.button_Click);
            // 
            // expressionExplorer
            // 
            this.expressionExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.expressionExplorer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.expressionExplorer.Location = new System.Drawing.Point(0, 56);
            this.expressionExplorer.Name = "expressionExplorer";
            this.expressionExplorer.Size = new System.Drawing.Size(320, 336);
            this.expressionExplorer.TabIndex = 18;
            this.expressionExplorer.OnItemAction += new Genetibase.MathX.NugenCCalc.Design.ItemActionEventHandler(this.expressionExplorer_OnItemAction);
            // 
            // ExpressionRepositoryView
            // 
            this.Controls.Add(this.expressionExplorer);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btUIt);
            this.Controls.Add(this.btDelete);
            this.Controls.Add(this.btEdit);
            this.Controls.Add(this.btCreate);
            this.Controls.Add(this.label2);
            this.Name = "ExpressionRepositoryView";
            this.Size = new System.Drawing.Size(408, 400);
            this.Load += new System.EventHandler(this.ExpressionRepositoryView_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void ExpressionRepositoryView_Load(object sender, System.EventArgs e)
		{
			if (_is3DDesigner)
				this.expressionExplorer.Is3DDesigner = true;
			this.expressionExplorer.Expressions = PredefinedSettings.Instance.Expressions;
			this.expressionExplorer.Init();
			BindEvents();
		}


		/// <summary>
		/// 
		/// </summary>
		private void BindEvents()
		{
			PredefinedSettings.Instance.Expressions.OnChange +=new EventHandler(Expressions_OnChange);
			PredefinedSettings.Instance.Expressions.OnItemChange +=new ParametersChangeHandler(Expressions_OnItemChange);
		}


		private void expressionExplorer_OnItemAction(object sender, ItemEventArgs e)
		{
			switch(e.Action)
			{
				case "Create":
					CreateExpression();
					break;
				case "Edit":
					EditExpression(e.Item);
					break;
				case "Delete":
					DeleteExpression(e.Item);
					break;
				case "Use It!":
					UseExpression(e.Item);
					break;
				default:
					MessageBox.Show(this, "Unknown menu item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
			}
		}

		private void CreateExpression()
		{
			ExpressionForm frmExpression = new ExpressionForm();
			if (_is3DDesigner)
				frmExpression.Component = new NugenCCalc3D();
			else
				frmExpression.Component = new NugenCCalc2D();

			frmExpression.Component.FunctionParameters.SourceType = SourceType.CodeExpression;
			if (frmExpression.ShowDialog() == DialogResult.OK)
			{
				PredefinedSettings.Instance.Expressions.Add(frmExpression.Component.FunctionParameters);
			}
			this.expressionExplorer.Init();
		}

		private void DeleteExpression(FunctionParameters expression)
		{
			if (expression != null)
			{
				PredefinedSettings.Instance.Expressions.Remove(expression);
				this.expressionExplorer.Init();
			}
		}

		private void EditExpression(FunctionParameters expression)
		{
			if (expression != null)
			{
				FunctionParameters newExpression = (FunctionParameters)expression.Clone();

				NugenCCalcBase component = null;
				if (_is3DDesigner)
				{
					component = new NugenCCalc3D();
					component.FunctionParameters = (Function3DParameters)newExpression;
				}
				else
				{
					component = new NugenCCalc2D();
					component.FunctionParameters = (Function2DParameters)newExpression;
				}
				ExpressionForm frmExpression = new ExpressionForm(component);

				if (frmExpression.ShowDialog() == DialogResult.OK)
				{
					expression.Name = frmExpression.Component.FunctionParameters.Name;
					expression.CodeLanguage = frmExpression.Component.FunctionParameters.CodeLanguage;
					expression.Code = frmExpression.Component.FunctionParameters.Code;
				}
			}
			this.expressionExplorer.Init();

		}

		private void UseExpression(FunctionParameters expression)
		{
			if (expression != null)
			{
				if (MessageBox.Show(this, "Are you sure that you want use this expression as source for component?", "Equation repository", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
                    _component.FunctionParameters = (FunctionParameters)Activator.CreateInstance(expression.GetType());
                    _component.FunctionParameters.Name = expression.Name;
                    _component.FunctionParameters.CodeLanguage = expression.CodeLanguage;
                    _component.FunctionParameters.Code = expression.Code;
                    _component.FunctionParameters.SourceType = SourceType.CodeExpression;
				}
			}
		}

		private void Expressions_OnChange(object sender, EventArgs e)
		{
			PredefinedSettings.SavePredefinedSettings();
		}

		private void Expressions_OnItemChange(FunctionParameters item, EventArgs e)
		{
			PredefinedSettings.SavePredefinedSettings();
		}


		private void button_Click(object sender, System.EventArgs e)
		{
			switch(((Button)sender).Text)
			{
				case "Create":
					CreateExpression();
					break;
				case "Delete":
					DeleteExpression(expressionExplorer.Selected);
					break;
				case "Edit":
					EditExpression(expressionExplorer.Selected);
					break;
				case "Use It!":
					UseExpression(expressionExplorer.Selected);
					break;		
				default:
					MessageBox.Show(this, "Unknown button", "Equation Repository", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;		
			}
		}
	}
}

