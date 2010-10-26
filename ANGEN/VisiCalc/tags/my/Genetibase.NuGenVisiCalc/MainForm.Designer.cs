/* -----------------------------------------------
 * MainForm.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;

namespace Genetibase.NuGenVisiCalc
{
	partial class MainForm
	{
		private IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> if managed resources should be disposed; otherwise, <see langword="false"/>.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
			this._statusStrip = new System.Windows.Forms.StatusStrip();
			this._toolStrip = new System.Windows.Forms.ToolStrip();
			this._schemaDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
			this._newSchemaButton = new System.Windows.Forms.ToolStripMenuItem();
			this._openSchemaButton = new System.Windows.Forms.ToolStripMenuItem();
			this._saveSchemaButton = new System.Windows.Forms.ToolStripMenuItem();
			this._schemaSeparator = new System.Windows.Forms.ToolStripSeparator();
			this._insertFromFileButton = new System.Windows.Forms.ToolStripMenuItem();
			this._exportToImageButton = new System.Windows.Forms.ToolStripMenuItem();
			this._schemaSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this._aboutButton = new System.Windows.Forms.ToolStripMenuItem();
			this._printSplitButton = new System.Windows.Forms.ToolStripSplitButton();
			this._printButton = new System.Windows.Forms.ToolStripMenuItem();
			this._previewButton = new System.Windows.Forms.ToolStripMenuItem();
			this._printSeparator = new System.Windows.Forms.ToolStripSeparator();
			this._pageSetupButton = new System.Windows.Forms.ToolStripMenuItem();
			this._zoomButton = new System.Windows.Forms.ToolStripDropDownButton();
			this._50zoomButton = new System.Windows.Forms.ToolStripMenuItem();
			this._75zoomButton = new System.Windows.Forms.ToolStripMenuItem();
			this._100zoomButton = new System.Windows.Forms.ToolStripMenuItem();
			this._125zoomButton = new System.Windows.Forms.ToolStripMenuItem();
			this._150zoomButton = new System.Windows.Forms.ToolStripMenuItem();
			this._200zoomButton = new System.Windows.Forms.ToolStripMenuItem();
			this._expressionCombo = new System.Windows.Forms.ToolStripComboBox();
			this._diagramFromExpressionButton = new System.Windows.Forms.ToolStripButton();
			this._expressionFromDiagramButton = new System.Windows.Forms.ToolStripButton();
			this._tabbedMdi = new Genetibase.Controls.NuGenTabbedMdi();
			this._toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// _statusStrip
			// 
			this._statusStrip.Location = new System.Drawing.Point(0, 424);
			this._statusStrip.Name = "_statusStrip";
			this._statusStrip.Size = new System.Drawing.Size(650, 22);
			this._statusStrip.TabIndex = 0;
			this._statusStrip.Text = "statusStrip1";
			// 
			// _toolStrip
			// 
			this._toolStrip.AutoSize = false;
			this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._schemaDropDownButton,
            this._printSplitButton,
            this._zoomButton,
            this._expressionCombo,
            this._diagramFromExpressionButton,
            this._expressionFromDiagramButton});
			this._toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this._toolStrip.Location = new System.Drawing.Point(0, 0);
			this._toolStrip.Name = "_toolStrip";
			this._toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this._toolStrip.Size = new System.Drawing.Size(650, 25);
			this._toolStrip.TabIndex = 1;
			// 
			// _schemaDropDownButton
			// 
			this._schemaDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._newSchemaButton,
            this._openSchemaButton,
            this._saveSchemaButton,
            this._schemaSeparator,
            this._insertFromFileButton,
            this._exportToImageButton,
            this._schemaSeparator2,
            this._aboutButton});
			this._schemaDropDownButton.Image = global::Genetibase.NuGenVisiCalc.Properties.Resources.Schema;
			this._schemaDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._schemaDropDownButton.Name = "_schemaDropDownButton";
			this._schemaDropDownButton.Size = new System.Drawing.Size(73, 22);
			this._schemaDropDownButton.Text = "&Schema";
			// 
			// _newSchemaButton
			// 
			this._newSchemaButton.Name = "_newSchemaButton";
			this._newSchemaButton.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this._newSchemaButton.Size = new System.Drawing.Size(177, 22);
			this._newSchemaButton.Text = "&New";
			this._newSchemaButton.Click += new System.EventHandler(this._newSchemaButton_Click);
			// 
			// _openSchemaButton
			// 
			this._openSchemaButton.Name = "_openSchemaButton";
			this._openSchemaButton.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this._openSchemaButton.Size = new System.Drawing.Size(177, 22);
			this._openSchemaButton.Text = "&Open...";
			// 
			// _saveSchemaButton
			// 
			this._saveSchemaButton.Name = "_saveSchemaButton";
			this._saveSchemaButton.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this._saveSchemaButton.Size = new System.Drawing.Size(177, 22);
			this._saveSchemaButton.Text = "&Save...";
			// 
			// _schemaSeparator
			// 
			this._schemaSeparator.Name = "_schemaSeparator";
			this._schemaSeparator.Size = new System.Drawing.Size(174, 6);
			// 
			// _insertFromFileButton
			// 
			this._insertFromFileButton.Name = "_insertFromFileButton";
			this._insertFromFileButton.Size = new System.Drawing.Size(177, 22);
			this._insertFromFileButton.Text = "&Insert From File...";
			// 
			// _exportToImageButton
			// 
			this._exportToImageButton.Name = "_exportToImageButton";
			this._exportToImageButton.Size = new System.Drawing.Size(177, 22);
			this._exportToImageButton.Text = "&Export To Image...";
			// 
			// _schemaSeparator2
			// 
			this._schemaSeparator2.Name = "_schemaSeparator2";
			this._schemaSeparator2.Size = new System.Drawing.Size(174, 6);
			// 
			// _aboutButton
			// 
			this._aboutButton.Name = "_aboutButton";
			this._aboutButton.Size = new System.Drawing.Size(177, 22);
			this._aboutButton.Text = "&About...";
			this._aboutButton.Click += new System.EventHandler(this._aboutButton_Click);
			// 
			// _printSplitButton
			// 
			this._printSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._printSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._printButton,
            this._previewButton,
            this._printSeparator,
            this._pageSetupButton});
			this._printSplitButton.Image = global::Genetibase.NuGenVisiCalc.Properties.Resources.Print;
			this._printSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._printSplitButton.Name = "_printSplitButton";
			this._printSplitButton.Size = new System.Drawing.Size(32, 22);
			this._printSplitButton.Text = "Print";
			// 
			// _printButton
			// 
			this._printButton.Name = "_printButton";
			this._printButton.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this._printButton.Size = new System.Drawing.Size(160, 22);
			this._printButton.Text = "&Print...";
			// 
			// _previewButton
			// 
			this._previewButton.Name = "_previewButton";
			this._previewButton.Size = new System.Drawing.Size(160, 22);
			this._previewButton.Text = "Print Pre&view...";
			// 
			// _printSeparator
			// 
			this._printSeparator.Name = "_printSeparator";
			this._printSeparator.Size = new System.Drawing.Size(157, 6);
			// 
			// _pageSetupButton
			// 
			this._pageSetupButton.Name = "_pageSetupButton";
			this._pageSetupButton.Size = new System.Drawing.Size(160, 22);
			this._pageSetupButton.Text = "Page &Setup...";
			// 
			// _zoomButton
			// 
			this._zoomButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._50zoomButton,
            this._75zoomButton,
            this._100zoomButton,
            this._125zoomButton,
            this._150zoomButton,
            this._200zoomButton});
			this._zoomButton.Image = global::Genetibase.NuGenVisiCalc.Properties.Resources.Zoom;
			this._zoomButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._zoomButton.Name = "_zoomButton";
			this._zoomButton.Size = new System.Drawing.Size(62, 22);
			this._zoomButton.Text = "&Zoom";
			// 
			// _50zoomButton
			// 
			this._50zoomButton.CheckOnClick = true;
			this._50zoomButton.Name = "_50zoomButton";
			this._50zoomButton.Size = new System.Drawing.Size(114, 22);
			this._50zoomButton.Text = "50%";
			this._50zoomButton.CheckedChanged += new System.EventHandler(this._zoomPercent_CheckedChanged);
			// 
			// _75zoomButton
			// 
			this._75zoomButton.CheckOnClick = true;
			this._75zoomButton.Name = "_75zoomButton";
			this._75zoomButton.Size = new System.Drawing.Size(114, 22);
			this._75zoomButton.Text = "75%";
			this._75zoomButton.CheckedChanged += new System.EventHandler(this._zoomPercent_CheckedChanged);
			// 
			// _100zoomButton
			// 
			this._100zoomButton.CheckOnClick = true;
			this._100zoomButton.Name = "_100zoomButton";
			this._100zoomButton.Size = new System.Drawing.Size(114, 22);
			this._100zoomButton.Text = "100%";
			this._100zoomButton.CheckedChanged += new System.EventHandler(this._zoomPercent_CheckedChanged);
			// 
			// _125zoomButton
			// 
			this._125zoomButton.CheckOnClick = true;
			this._125zoomButton.Name = "_125zoomButton";
			this._125zoomButton.Size = new System.Drawing.Size(114, 22);
			this._125zoomButton.Text = "125%";
			this._125zoomButton.CheckedChanged += new System.EventHandler(this._zoomPercent_CheckedChanged);
			// 
			// _150zoomButton
			// 
			this._150zoomButton.CheckOnClick = true;
			this._150zoomButton.Name = "_150zoomButton";
			this._150zoomButton.Size = new System.Drawing.Size(114, 22);
			this._150zoomButton.Text = "150%";
			this._150zoomButton.CheckedChanged += new System.EventHandler(this._zoomPercent_CheckedChanged);
			// 
			// _200zoomButton
			// 
			this._200zoomButton.CheckOnClick = true;
			this._200zoomButton.Name = "_200zoomButton";
			this._200zoomButton.ShortcutKeyDisplayString = "";
			this._200zoomButton.Size = new System.Drawing.Size(114, 22);
			this._200zoomButton.Text = "200%";
			this._200zoomButton.CheckedChanged += new System.EventHandler(this._zoomPercent_CheckedChanged);
			// 
			// _expressionCombo
			// 
			this._expressionCombo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this._expressionCombo.AutoSize = false;
			this._expressionCombo.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._expressionCombo.MaxDropDownItems = 16;
			this._expressionCombo.Name = "_expressionCombo";
			this._expressionCombo.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
			this._expressionCombo.Size = new System.Drawing.Size(250, 21);
			// 
			// _diagramFromExpressionButton
			// 
			this._diagramFromExpressionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._diagramFromExpressionButton.Image = global::Genetibase.NuGenVisiCalc.Properties.Resources.DiagramFromExpression;
			this._diagramFromExpressionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._diagramFromExpressionButton.Name = "_diagramFromExpressionButton";
			this._diagramFromExpressionButton.Size = new System.Drawing.Size(23, 22);
			this._diagramFromExpressionButton.Text = "&Diagram From Expression";
			// 
			// _expressionFromDiagramButton
			// 
			this._expressionFromDiagramButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._expressionFromDiagramButton.Image = global::Genetibase.NuGenVisiCalc.Properties.Resources.ExpressionFromDiagram;
			this._expressionFromDiagramButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._expressionFromDiagramButton.Name = "_expressionFromDiagramButton";
			this._expressionFromDiagramButton.Size = new System.Drawing.Size(23, 22);
			this._expressionFromDiagramButton.Text = "&Expression From Diagram";
			// 
			// _tabbedMdi
			// 
			this._tabbedMdi.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tabbedMdi.Location = new System.Drawing.Point(0, 25);
			this._tabbedMdi.Name = "_tabbedMdi";
			this._tabbedMdi.Size = new System.Drawing.Size(650, 399);
			this._tabbedMdi.TabIndex = 2;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(650, 446);
			this.Controls.Add(this._tabbedMdi);
			this.Controls.Add(this._toolStrip);
			this.Controls.Add(this._statusStrip);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "NuGenVisiCalc";
			this._toolStrip.ResumeLayout(false);
			this._toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip _statusStrip;
		private System.Windows.Forms.ToolStrip _toolStrip;
		private System.Windows.Forms.ToolStripSplitButton _printSplitButton;
		private System.Windows.Forms.ToolStripMenuItem _printButton;
		private System.Windows.Forms.ToolStripMenuItem _previewButton;
		private System.Windows.Forms.ToolStripSeparator _printSeparator;
		private System.Windows.Forms.ToolStripMenuItem _pageSetupButton;
		private System.Windows.Forms.ToolStripDropDownButton _schemaDropDownButton;
		private System.Windows.Forms.ToolStripMenuItem _newSchemaButton;
		private System.Windows.Forms.ToolStripMenuItem _openSchemaButton;
		private System.Windows.Forms.ToolStripMenuItem _saveSchemaButton;
		private System.Windows.Forms.ToolStripSeparator _schemaSeparator;
		private System.Windows.Forms.ToolStripMenuItem _insertFromFileButton;
		private System.Windows.Forms.ToolStripMenuItem _exportToImageButton;
		private System.Windows.Forms.ToolStripComboBox _expressionCombo;
		private System.Windows.Forms.ToolStripButton _expressionFromDiagramButton;
		private System.Windows.Forms.ToolStripButton _diagramFromExpressionButton;
		private System.Windows.Forms.ToolStripDropDownButton _zoomButton;
		private System.Windows.Forms.ToolStripMenuItem _50zoomButton;
		private System.Windows.Forms.ToolStripMenuItem _100zoomButton;
		private System.Windows.Forms.ToolStripMenuItem _125zoomButton;
		private System.Windows.Forms.ToolStripMenuItem _150zoomButton;
		private System.Windows.Forms.ToolStripMenuItem _200zoomButton;
		private System.Windows.Forms.ToolStripMenuItem _75zoomButton;
		private System.Windows.Forms.ToolStripSeparator _schemaSeparator2;
		private System.Windows.Forms.ToolStripMenuItem _aboutButton;
		private Genetibase.Controls.NuGenTabbedMdi _tabbedMdi;
	}
}

