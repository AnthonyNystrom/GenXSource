namespace ControlReflector
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this._button = new System.Windows.Forms.Button();
			this._checkBox = new System.Windows.Forms.CheckBox();
			this._linkLabel = new Genetibase.Shared.Controls.NuGenLinkLabel();
			this._linkLabelReflector = new Genetibase.Shared.Controls.NuGenControlReflector();
			this._checkBoxReflector = new Genetibase.Shared.Controls.NuGenControlReflector();
			this._buttonReflector = new Genetibase.Shared.Controls.NuGenControlReflector();
			this._propertyGrid = new Genetibase.Shared.Controls.NuGenPropertyGrid();
			this.SuspendLayout();
			// 
			// _button
			// 
			this._button.Location = new System.Drawing.Point(12, 12);
			this._button.Name = "_button";
			this._button.Size = new System.Drawing.Size(75, 23);
			this._button.TabIndex = 0;
			this._button.Text = "&Go";
			this._button.UseVisualStyleBackColor = true;
			// 
			// _checkBox
			// 
			this._checkBox.AutoSize = true;
			this._checkBox.Location = new System.Drawing.Point(12, 83);
			this._checkBox.Name = "_checkBox";
			this._checkBox.Size = new System.Drawing.Size(74, 17);
			this._checkBox.TabIndex = 1;
			this._checkBox.Text = "checkBox";
			this._checkBox.UseVisualStyleBackColor = true;
			// 
			// _linkLabel
			// 
			this._linkLabel.Image = ((System.Drawing.Image)(resources.GetObject("_linkLabel.Image")));
			this._linkLabel.Location = new System.Drawing.Point(12, 150);
			this._linkLabel.Name = "_linkLabel";
			this._linkLabel.Size = new System.Drawing.Size(112, 16);
			this._linkLabel.TabIndex = 2;
			this._linkLabel.Target = "http://www.genetibase.com/";
			this._linkLabel.Text = "Genetibase, Inc.";
			// 
			// _linkLabelReflector
			// 
			this._linkLabelReflector.ControlToReflect = this._linkLabel;
			this._linkLabelReflector.Location = new System.Drawing.Point(12, 172);
			this._linkLabelReflector.Name = "_linkLabelReflector";
			this._linkLabelReflector.Size = new System.Drawing.Size(112, 38);
			// 
			// _checkBoxReflector
			// 
			this._checkBoxReflector.ControlToReflect = this._checkBox;
			this._checkBoxReflector.Location = new System.Drawing.Point(12, 106);
			this._checkBoxReflector.Name = "_checkBoxReflector";
			this._checkBoxReflector.Size = new System.Drawing.Size(74, 38);
			// 
			// _buttonReflector
			// 
			this._buttonReflector.ControlToReflect = this._button;
			this._buttonReflector.Location = new System.Drawing.Point(12, 41);
			this._buttonReflector.Name = "_buttonReflector";
			this._buttonReflector.Size = new System.Drawing.Size(74, 36);
			// 
			// _propertyGrid
			// 
			this._propertyGrid.Location = new System.Drawing.Point(133, 12);
			this._propertyGrid.Name = "_propertyGrid";
			this._propertyGrid.SelectedObject = this._linkLabelReflector;
			this._propertyGrid.Size = new System.Drawing.Size(174, 198);
			this._propertyGrid.TabIndex = 8;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(320, 224);
			this.Controls.Add(this._propertyGrid);
			this.Controls.Add(this._buttonReflector);
			this.Controls.Add(this._checkBoxReflector);
			this.Controls.Add(this._linkLabelReflector);
			this.Controls.Add(this._linkLabel);
			this.Controls.Add(this._checkBox);
			this.Controls.Add(this._button);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "ControlReflector";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button _button;
		private System.Windows.Forms.CheckBox _checkBox;
		private Genetibase.Shared.Controls.NuGenLinkLabel _linkLabel;
		private Genetibase.Shared.Controls.NuGenControlReflector _linkLabelReflector;
		private Genetibase.Shared.Controls.NuGenControlReflector _checkBoxReflector;
		private Genetibase.Shared.Controls.NuGenControlReflector _buttonReflector;
		private Genetibase.Shared.Controls.NuGenPropertyGrid _propertyGrid;
	}
}

