namespace NuGenSVisualLib.Rendering.Chem
{
    partial class Chem3DControl
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
            if (renderContext != null)
                renderContext.Dispose();
            renderingThread.Abort();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moleculeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.shadingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moleculesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bySeriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byElementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lightingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.renderingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.infoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.supportedFormatsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.schemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ballStickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawAtomsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.thinLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thickLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blendedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.spacingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aToBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.betweenBondsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.stickEndsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.roundedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.spaceFillToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.metaballsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editSchemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripMenuItem1,
            this.shadingToolStripMenuItem,
            this.lightingToolStripMenuItem,
            this.renderingToolStripMenuItem,
            this.toolStripMenuItem2,
            this.infoToolStripMenuItem1,
            this.toolStripMenuItem3,
            this.schemeToolStripMenuItem,
            this.editSchemeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(140, 198);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moleculeToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.openToolStripMenuItem.Text = "Open New";
            // 
            // moleculeToolStripMenuItem
            // 
            this.moleculeToolStripMenuItem.Name = "moleculeToolStripMenuItem";
            this.moleculeToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.moleculeToolStripMenuItem.Text = "Molecule";
            this.moleculeToolStripMenuItem.Click += new System.EventHandler(this.moleculeToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(136, 6);
            // 
            // shadingToolStripMenuItem
            // 
            this.shadingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.moleculesToolStripMenuItem});
            this.shadingToolStripMenuItem.Enabled = false;
            this.shadingToolStripMenuItem.Name = "shadingToolStripMenuItem";
            this.shadingToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.shadingToolStripMenuItem.Text = "Shading";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // moleculesToolStripMenuItem
            // 
            this.moleculesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bySeriesToolStripMenuItem,
            this.byElementToolStripMenuItem});
            this.moleculesToolStripMenuItem.Name = "moleculesToolStripMenuItem";
            this.moleculesToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.moleculesToolStripMenuItem.Text = "Molecules";
            // 
            // bySeriesToolStripMenuItem
            // 
            this.bySeriesToolStripMenuItem.Name = "bySeriesToolStripMenuItem";
            this.bySeriesToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.bySeriesToolStripMenuItem.Text = "By Series";
            // 
            // byElementToolStripMenuItem
            // 
            this.byElementToolStripMenuItem.Name = "byElementToolStripMenuItem";
            this.byElementToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.byElementToolStripMenuItem.Text = "By Element";
            // 
            // lightingToolStripMenuItem
            // 
            this.lightingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem1});
            this.lightingToolStripMenuItem.Enabled = false;
            this.lightingToolStripMenuItem.Name = "lightingToolStripMenuItem";
            this.lightingToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.lightingToolStripMenuItem.Text = "Lighting";
            // 
            // editToolStripMenuItem1
            // 
            this.editToolStripMenuItem1.Name = "editToolStripMenuItem1";
            this.editToolStripMenuItem1.Size = new System.Drawing.Size(94, 22);
            this.editToolStripMenuItem1.Text = "Edit";
            this.editToolStripMenuItem1.Click += new System.EventHandler(this.editToolStripMenuItem1_Click);
            // 
            // renderingToolStripMenuItem
            // 
            this.renderingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStripMenuItem,
            this.helpersToolStripMenuItem});
            this.renderingToolStripMenuItem.Name = "renderingToolStripMenuItem";
            this.renderingToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.renderingToolStripMenuItem.Text = "Rendering";
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.infoToolStripMenuItem.Text = "Info";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // helpersToolStripMenuItem
            // 
            this.helpersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.axisToolStripMenuItem});
            this.helpersToolStripMenuItem.Enabled = false;
            this.helpersToolStripMenuItem.Name = "helpersToolStripMenuItem";
            this.helpersToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.helpersToolStripMenuItem.Text = "Helpers";
            // 
            // axisToolStripMenuItem
            // 
            this.axisToolStripMenuItem.CheckOnClick = true;
            this.axisToolStripMenuItem.Name = "axisToolStripMenuItem";
            this.axisToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.axisToolStripMenuItem.Text = "Axis";
            this.axisToolStripMenuItem.CheckedChanged += new System.EventHandler(this.axisToolStripMenuItem_CheckedChanged);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(136, 6);
            // 
            // infoToolStripMenuItem1
            // 
            this.infoToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.supportedFormatsToolStripMenuItem});
            this.infoToolStripMenuItem1.Name = "infoToolStripMenuItem1";
            this.infoToolStripMenuItem1.Size = new System.Drawing.Size(139, 22);
            this.infoToolStripMenuItem1.Text = "Info";
            // 
            // supportedFormatsToolStripMenuItem
            // 
            this.supportedFormatsToolStripMenuItem.Name = "supportedFormatsToolStripMenuItem";
            this.supportedFormatsToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.supportedFormatsToolStripMenuItem.Text = "Supported Formats";
            this.supportedFormatsToolStripMenuItem.Click += new System.EventHandler(this.supportedFormatsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(136, 6);
            // 
            // schemeToolStripMenuItem
            // 
            this.schemeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ballStickToolStripMenuItem,
            this.spaceFillToolStripMenuItem,
            this.metaballsToolStripMenuItem});
            this.schemeToolStripMenuItem.Name = "schemeToolStripMenuItem";
            this.schemeToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.schemeToolStripMenuItem.Text = "Scheme";
            // 
            // ballStickToolStripMenuItem
            // 
            this.ballStickToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawAtomsToolStripMenuItem,
            this.toolStripMenuItem6,
            this.thinLinesToolStripMenuItem,
            this.thickLinesToolStripMenuItem,
            this.blendedToolStripMenuItem,
            this.toolStripMenuItem4,
            this.spacingsToolStripMenuItem,
            this.toolStripMenuItem5,
            this.stickEndsToolStripMenuItem});
            this.ballStickToolStripMenuItem.Name = "ballStickToolStripMenuItem";
            this.ballStickToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.ballStickToolStripMenuItem.Text = "Ball + Stick";
            this.ballStickToolStripMenuItem.Click += new System.EventHandler(this.ballStickToolStripMenuItem_Click);
            // 
            // drawAtomsToolStripMenuItem
            // 
            this.drawAtomsToolStripMenuItem.CheckOnClick = true;
            this.drawAtomsToolStripMenuItem.Name = "drawAtomsToolStripMenuItem";
            this.drawAtomsToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.drawAtomsToolStripMenuItem.Text = "Draw Balls";
            this.drawAtomsToolStripMenuItem.Click += new System.EventHandler(this.drawAtomsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(147, 6);
            // 
            // thinLinesToolStripMenuItem
            // 
            this.thinLinesToolStripMenuItem.CheckOnClick = true;
            this.thinLinesToolStripMenuItem.Name = "thinLinesToolStripMenuItem";
            this.thinLinesToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.thinLinesToolStripMenuItem.Text = "Thin Sticks";
            this.thinLinesToolStripMenuItem.Click += new System.EventHandler(this.thinLinesToolStripMenuItem_Click);
            // 
            // thickLinesToolStripMenuItem
            // 
            this.thickLinesToolStripMenuItem.CheckOnClick = true;
            this.thickLinesToolStripMenuItem.Name = "thickLinesToolStripMenuItem";
            this.thickLinesToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.thickLinesToolStripMenuItem.Text = "Thick Sticks";
            this.thickLinesToolStripMenuItem.Click += new System.EventHandler(this.thickLinesToolStripMenuItem_Click);
            // 
            // blendedToolStripMenuItem
            // 
            this.blendedToolStripMenuItem.CheckOnClick = true;
            this.blendedToolStripMenuItem.Name = "blendedToolStripMenuItem";
            this.blendedToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.blendedToolStripMenuItem.Text = "Blended Sticks";
            this.blendedToolStripMenuItem.Click += new System.EventHandler(this.blendedToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(147, 6);
            // 
            // spacingsToolStripMenuItem
            // 
            this.spacingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aToBToolStripMenuItem,
            this.betweenBondsToolStripMenuItem});
            this.spacingsToolStripMenuItem.Name = "spacingsToolStripMenuItem";
            this.spacingsToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.spacingsToolStripMenuItem.Text = "Spacings";
            // 
            // aToBToolStripMenuItem
            // 
            this.aToBToolStripMenuItem.CheckOnClick = true;
            this.aToBToolStripMenuItem.Name = "aToBToolStripMenuItem";
            this.aToBToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.aToBToolStripMenuItem.Text = "A to B";
            this.aToBToolStripMenuItem.Click += new System.EventHandler(this.aToBToolStripMenuItem_Click);
            // 
            // betweenBondsToolStripMenuItem
            // 
            this.betweenBondsToolStripMenuItem.CheckOnClick = true;
            this.betweenBondsToolStripMenuItem.Name = "betweenBondsToolStripMenuItem";
            this.betweenBondsToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.betweenBondsToolStripMenuItem.Text = "Between Bonds";
            this.betweenBondsToolStripMenuItem.Click += new System.EventHandler(this.betweenBondsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(147, 6);
            // 
            // stickEndsToolStripMenuItem
            // 
            this.stickEndsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.roundedToolStripMenuItem,
            this.flatToolStripMenuItem,
            this.pointToolStripMenuItem,
            this.openToolStripMenuItem1});
            this.stickEndsToolStripMenuItem.Name = "stickEndsToolStripMenuItem";
            this.stickEndsToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.stickEndsToolStripMenuItem.Text = "Stick Ends";
            // 
            // roundedToolStripMenuItem
            // 
            this.roundedToolStripMenuItem.Enabled = false;
            this.roundedToolStripMenuItem.Name = "roundedToolStripMenuItem";
            this.roundedToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.roundedToolStripMenuItem.Text = "Rounded";
            this.roundedToolStripMenuItem.Click += new System.EventHandler(this.roundedToolStripMenuItem_Click);
            // 
            // flatToolStripMenuItem
            // 
            this.flatToolStripMenuItem.Name = "flatToolStripMenuItem";
            this.flatToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.flatToolStripMenuItem.Text = "Closed";
            this.flatToolStripMenuItem.Click += new System.EventHandler(this.flatToolStripMenuItem_Click);
            // 
            // pointToolStripMenuItem
            // 
            this.pointToolStripMenuItem.Name = "pointToolStripMenuItem";
            this.pointToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.pointToolStripMenuItem.Text = "Point";
            this.pointToolStripMenuItem.Click += new System.EventHandler(this.pointToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(122, 22);
            this.openToolStripMenuItem1.Text = "Open";
            this.openToolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem1_Click);
            // 
            // spaceFillToolStripMenuItem
            // 
            this.spaceFillToolStripMenuItem.Enabled = false;
            this.spaceFillToolStripMenuItem.Name = "spaceFillToolStripMenuItem";
            this.spaceFillToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.spaceFillToolStripMenuItem.Text = "Space Fill";
            // 
            // metaballsToolStripMenuItem
            // 
            this.metaballsToolStripMenuItem.Enabled = false;
            this.metaballsToolStripMenuItem.Name = "metaballsToolStripMenuItem";
            this.metaballsToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.metaballsToolStripMenuItem.Text = "Metaballs";
            // 
            // editSchemeToolStripMenuItem
            // 
            this.editSchemeToolStripMenuItem.Name = "editSchemeToolStripMenuItem";
            this.editSchemeToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.editSchemeToolStripMenuItem.Text = "Edit Scheme";
            this.editSchemeToolStripMenuItem.Click += new System.EventHandler(this.editSchemeToolStripMenuItem_Click);
            // 
            // Chem3DControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Name = "Chem3DControl";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moleculeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem shadingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moleculesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bySeriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byElementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lightingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem renderingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem axisToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem supportedFormatsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem schemeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ballStickToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spaceFillToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem metaballsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem thinLinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thickLinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem blendedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem drawAtomsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem spacingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aToBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem betweenBondsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem stickEndsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem roundedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editSchemeToolStripMenuItem;
    }
}
