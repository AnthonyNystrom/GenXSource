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
			this._viewDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
			this._propertiesButton = new System.Windows.Forms.ToolStripMenuItem();
			this._toolboxButton = new System.Windows.Forms.ToolStripMenuItem();
			this._outputButton = new System.Windows.Forms.ToolStripMenuItem();
			this._canvasDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
			this._canvasSizeToolStrip = new Genetibase.NuGenVisiCalc.ToolStripCanvasSize();
			this._separator = new System.Windows.Forms.ToolStripSeparator();
			this._printSplitButton = new System.Windows.Forms.ToolStripButton();
			this._zoomButton = new System.Windows.Forms.ToolStripDropDownButton();
			this._50zoomButton = new System.Windows.Forms.ToolStripMenuItem();
			this._75zoomButton = new System.Windows.Forms.ToolStripMenuItem();
			this._100zoomButton = new System.Windows.Forms.ToolStripMenuItem();
			this._125zoomButton = new System.Windows.Forms.ToolStripMenuItem();
			this._150zoomButton = new System.Windows.Forms.ToolStripMenuItem();
			this._200zoomButton = new System.Windows.Forms.ToolStripMenuItem();
			this._separator2 = new System.Windows.Forms.ToolStripSeparator();
			this._expressionFromDiagramButton = new System.Windows.Forms.ToolStripButton();
			this._diagramFromExpressionButton = new System.Windows.Forms.ToolStripButton();
			this._expressionCombo = new System.Windows.Forms.ToolStripComboBox();
			this._toolStripManager = new Genetibase.SmoothControls.NuGenSmoothToolStripManager();
			this._bkgndPanel = new Genetibase.SmoothControls.NuGenSmoothPanel();
			this._tabbedMdi = new Genetibase.NuGenVisiCalc.TabbedMdi();
			this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this._insertFileDialog = new System.Windows.Forms.SaveFileDialog();
			this._snapUI = new Genetibase.Shared.Controls.NuGenUISnap();
			this._toolStrip.SuspendLayout();
			this._bkgndPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _statusStrip
			// 
			this._statusStrip.Location = new System.Drawing.Point(0, 424);
			this._statusStrip.Name = "_statusStrip";
			this._statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
			this._statusStrip.Size = new System.Drawing.Size(632, 22);
			this._statusStrip.TabIndex = 0;
			this._statusStrip.Text = "statusStrip1";
			// 
			// _toolStrip
			// 
			this._toolStrip.AutoSize = false;
			this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._schemaDropDownButton,
            this._viewDropDownButton,
            this._canvasDropDownButton,
            this._separator,
            this._printSplitButton,
            this._zoomButton,
            this._separator2,
            this._expressionFromDiagramButton,
            this._diagramFromExpressionButton,
            this._expressionCombo});
			this._toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this._toolStrip.Location = new System.Drawing.Point(0, 0);
			this._toolStrip.Name = "_toolStrip";
			this._toolStrip.Size = new System.Drawing.Size(632, 25);
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
			this._schemaDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._schemaDropDownButton.Name = "_schemaDropDownButton";
			this._schemaDropDownButton.Size = new System.Drawing.Size(57, 22);
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
			this._openSchemaButton.Click += new System.EventHandler(this._openSchemaButton_Click);
			// 
			// _saveSchemaButton
			// 
			this._saveSchemaButton.Name = "_saveSchemaButton";
			this._saveSchemaButton.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this._saveSchemaButton.Size = new System.Drawing.Size(177, 22);
			this._saveSchemaButton.Text = "&Save...";
			this._saveSchemaButton.Click += new System.EventHandler(this._saveSchemaButton_Click);
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
			this._insertFromFileButton.Click += new System.EventHandler(this._insertFromFileButton_Click);
			// 
			// _exportToImageButton
			// 
			this._exportToImageButton.Name = "_exportToImageButton";
			this._exportToImageButton.Size = new System.Drawing.Size(177, 22);
			this._exportToImageButton.Text = "&Export To Image...";
			this._exportToImageButton.Click += new System.EventHandler(this._exportToImageButton_Click);
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
			// _viewDropDownButton
			// 
			this._viewDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this._viewDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._propertiesButton,
            this._toolboxButton,
            this._outputButton});
			this._viewDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("_viewDropDownButton.Image")));
			this._viewDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._viewDropDownButton.Name = "_viewDropDownButton";
			this._viewDropDownButton.Size = new System.Drawing.Size(42, 22);
			this._viewDropDownButton.Text = "&View";
			// 
			// _propertiesButton
			// 
			this._propertiesButton.Name = "_propertiesButton";
			this._propertiesButton.Size = new System.Drawing.Size(134, 22);
			this._propertiesButton.Text = "&Properties";
			this._propertiesButton.Click += new System.EventHandler(this._propertiesButton_Click);
			// 
			// _toolboxButton
			// 
			this._toolboxButton.Name = "_toolboxButton";
			this._toolboxButton.Size = new System.Drawing.Size(134, 22);
			this._toolboxButton.Text = "&Toolbox";
			this._toolboxButton.Click += new System.EventHandler(this._toolboxButton_Click);
			// 
			// _outputButton
			// 
			this._outputButton.Name = "_outputButton";
			this._outputButton.Size = new System.Drawing.Size(134, 22);
			this._outputButton.Text = "&Output";
			this._outputButton.Click += new System.EventHandler(this._outputButton_Click);
			// 
			// _canvasDropDownButton
			// 
			this._canvasDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this._canvasDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._canvasSizeToolStrip});
			this._canvasDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("_canvasDropDownButton.Image")));
			this._canvasDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._canvasDropDownButton.Name = "_canvasDropDownButton";
			this._canvasDropDownButton.Size = new System.Drawing.Size(56, 22);
			this._canvasDropDownButton.Text = "&Canvas";
			this._canvasDropDownButton.DropDownOpening += new System.EventHandler(this._canvasDropDownButton_DropDownOpening);
			this._canvasDropDownButton.DropDownClosed += new System.EventHandler(this._canvasDropDownButton_DropDownClosed);
			// 
			// _canvasSizeToolStrip
			// 
			this._canvasSizeToolStrip.AutoSize = false;
			this._canvasSizeToolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(236)))));
			this._canvasSizeToolStrip.CanvasSize = new System.Drawing.Size(1, 1);
			this._canvasSizeToolStrip.MaintainAspectRatio = false;
			this._canvasSizeToolStrip.Name = "_canvasSizeToolStrip";
			this._canvasSizeToolStrip.Size = new System.Drawing.Size(170, 100);
			this._canvasSizeToolStrip.Text = "_canvasSizeToolStrip";
			// 
			// _separator
			// 
			this._separator.Name = "_separator";
			this._separator.Size = new System.Drawing.Size(6, 25);
			// 
			// _printSplitButton
			// 
			this._printSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._printSplitButton.Image = global::Genetibase.NuGenVisiCalc.Properties.Resources.Print;
			this._printSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._printSplitButton.Name = "_printSplitButton";
			this._printSplitButton.Size = new System.Drawing.Size(23, 22);
			this._printSplitButton.Text = "Print";
			// 
			// _zoomButton
			// 
			this._zoomButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
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
			this._zoomButton.Size = new System.Drawing.Size(29, 22);
			this._zoomButton.Text = "&Zoom";
			// 
			// _50zoomButton
			// 
			this._50zoomButton.Name = "_50zoomButton";
			this._50zoomButton.Size = new System.Drawing.Size(114, 22);
			this._50zoomButton.Tag = "0,5";
			this._50zoomButton.Text = "50%";
			this._50zoomButton.Click += new System.EventHandler(this._zoomPercent_Click);
			// 
			// _75zoomButton
			// 
			this._75zoomButton.Name = "_75zoomButton";
			this._75zoomButton.Size = new System.Drawing.Size(114, 22);
			this._75zoomButton.Tag = "0,75";
			this._75zoomButton.Text = "75%";
			this._75zoomButton.Click += new System.EventHandler(this._zoomPercent_Click);
			// 
			// _100zoomButton
			// 
			this._100zoomButton.Name = "_100zoomButton";
			this._100zoomButton.Size = new System.Drawing.Size(114, 22);
			this._100zoomButton.Tag = "1";
			this._100zoomButton.Text = "100%";
			this._100zoomButton.Click += new System.EventHandler(this._zoomPercent_Click);
			// 
			// _125zoomButton
			// 
			this._125zoomButton.Name = "_125zoomButton";
			this._125zoomButton.Size = new System.Drawing.Size(114, 22);
			this._125zoomButton.Tag = "1,25";
			this._125zoomButton.Text = "125%";
			this._125zoomButton.Click += new System.EventHandler(this._zoomPercent_Click);
			// 
			// _150zoomButton
			// 
			this._150zoomButton.Name = "_150zoomButton";
			this._150zoomButton.Size = new System.Drawing.Size(114, 22);
			this._150zoomButton.Tag = "1,5";
			this._150zoomButton.Text = "150%";
			this._150zoomButton.Click += new System.EventHandler(this._zoomPercent_Click);
			// 
			// _200zoomButton
			// 
			this._200zoomButton.Name = "_200zoomButton";
			this._200zoomButton.ShortcutKeyDisplayString = "";
			this._200zoomButton.Size = new System.Drawing.Size(114, 22);
			this._200zoomButton.Tag = "2";
			this._200zoomButton.Text = "200%";
			this._200zoomButton.Click += new System.EventHandler(this._zoomPercent_Click);
			// 
			// _separator2
			// 
			this._separator2.Name = "_separator2";
			this._separator2.Size = new System.Drawing.Size(6, 25);
			// 
			// _expressionFromDiagramButton
			// 
			this._expressionFromDiagramButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._expressionFromDiagramButton.Image = global::Genetibase.NuGenVisiCalc.Properties.Resources.ExpressionFromDiagram;
			this._expressionFromDiagramButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._expressionFromDiagramButton.Name = "_expressionFromDiagramButton";
			this._expressionFromDiagramButton.Size = new System.Drawing.Size(23, 22);
			this._expressionFromDiagramButton.Text = "&Expression From Diagram";
			this._expressionFromDiagramButton.Click += new System.EventHandler(this._expressionFromDiagramButton_Click);
			// 
			// _diagramFromExpressionButton
			// 
			this._diagramFromExpressionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._diagramFromExpressionButton.Image = global::Genetibase.NuGenVisiCalc.Properties.Resources.DiagramFromExpression;
			this._diagramFromExpressionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._diagramFromExpressionButton.Name = "_diagramFromExpressionButton";
			this._diagramFromExpressionButton.Size = new System.Drawing.Size(23, 22);
			this._diagramFromExpressionButton.Text = "&Diagram From Expression";
			this._diagramFromExpressionButton.Click += new System.EventHandler(this._diagramFromExpressionButton_Click);
			// 
			// _expressionCombo
			// 
			this._expressionCombo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this._expressionCombo.AutoSize = false;
			this._expressionCombo.MaxDropDownItems = 16;
			this._expressionCombo.Name = "_expressionCombo";
			this._expressionCombo.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
			this._expressionCombo.Size = new System.Drawing.Size(250, 21);
			// 
			// _bkgndPanel
			// 
			this._bkgndPanel.Controls.Add(this._tabbedMdi);
			this._bkgndPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._bkgndPanel.DrawBorder = false;
			this._bkgndPanel.Location = new System.Drawing.Point(0, 25);
			this._bkgndPanel.Name = "_bkgndPanel";
			this._bkgndPanel.Size = new System.Drawing.Size(632, 399);
			// 
			// _tabbedMdi
			// 
			this._tabbedMdi.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tabbedMdi.Location = new System.Drawing.Point(0, 0);
			this._tabbedMdi.Name = "_tabbedMdi";
			this._tabbedMdi.Size = new System.Drawing.Size(632, 399);
			this._tabbedMdi.TabIndex = 0;
			this._tabbedMdi.StateChanged += new System.EventHandler<Genetibase.NuGenVisiCalc.MdiStateEventArgs>(this._tabbedMdi_StateChanged);
			// 
			// _snapUI
			// 
			this._snapUI.HostForm = this;
			this._snapUI.StickOnMove = false;
			this._snapUI.StickOnResize = false;
			this._snapUI.StickToOther = false;
			this._snapUI.StickToScreen = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(632, 446);
			this.Controls.Add(this._bkgndPanel);
			this.Controls.Add(this._toolStrip);
			this.Controls.Add(this._statusStrip);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "NuGenVisiCalc";
			this.Controls.SetChildIndex(this._statusStrip, 0);
			this.Controls.SetChildIndex(this._toolStrip, 0);
			this.Controls.SetChildIndex(this._bkgndPanel, 0);
			this._toolStrip.ResumeLayout(false);
			this._toolStrip.PerformLayout();
			this._bkgndPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip _statusStrip;
		private System.Windows.Forms.ToolStrip _toolStrip;
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
		private Genetibase.SmoothControls.NuGenSmoothToolStripManager _toolStripManager;
		private Genetibase.SmoothControls.NuGenSmoothPanel _bkgndPanel;
		private TabbedMdi _tabbedMdi;
		private System.Windows.Forms.ToolStripDropDownButton _viewDropDownButton;
		private System.Windows.Forms.ToolStripMenuItem _propertiesButton;
		private System.Windows.Forms.ToolStripMenuItem _toolboxButton;
		private System.Windows.Forms.ToolStripSeparator _separator;
		private System.Windows.Forms.ToolStripSeparator _separator2;
		private System.Windows.Forms.ToolStripButton _printSplitButton;
		private System.Windows.Forms.ToolStripDropDownButton _canvasDropDownButton;
		private ToolStripCanvasSize _canvasSizeToolStrip;
		private System.Windows.Forms.ToolStripMenuItem _outputButton;
		private System.Windows.Forms.SaveFileDialog _saveFileDialog;
		private System.Windows.Forms.OpenFileDialog _openFileDialog;
		private System.Windows.Forms.SaveFileDialog _insertFileDialog;
		private Genetibase.Shared.Controls.NuGenUISnap _snapUI;
	}
}

