using System.Windows.Forms;
using System.Drawing;
using Genetibase.UI;
using System.IO;

namespace Genetibase.Chem.NuGenSChem
{
    partial class MainWindow : NuGenEventHandler
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuAtomTypes = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ribbonControl1 = new Genetibase.UI.RibbonControl();
            this.ribbonTab1 = new Genetibase.UI.RibbonTab();
            this.ribbonTab2 = new Genetibase.UI.RibbonTab();
            this.ribbonGroup3 = new Genetibase.UI.RibbonGroup();
            this.addElementButton = new Genetibase.UI.RibbonButton();
            this.editElementButton = new Genetibase.UI.RibbonButton();
            this.ribbonGroup2 = new Genetibase.UI.RibbonGroup();
            this.unknownBondButton = new Genetibase.UI.RibbonButton();
            this.zeroBondButton = new Genetibase.UI.RibbonButton();
            this.declinedBondButton = new Genetibase.UI.RibbonButton();
            this.inclinedBondButton = new Genetibase.UI.RibbonButton();
            this.tripleBondButton = new Genetibase.UI.RibbonButton();
            this.doubleBondButton = new Genetibase.UI.RibbonButton();
            this.singleBondButton = new Genetibase.UI.RibbonButton();
            this.ribbonGroup1 = new Genetibase.UI.RibbonGroup();
            this.chargeButton = new Genetibase.UI.RibbonButton();
            this.rotateButton = new Genetibase.UI.RibbonButton();
            this.eraseButton = new Genetibase.UI.RibbonButton();
            this.selectButton = new Genetibase.UI.RibbonButton();
            this.ribbonTab3 = new Genetibase.UI.RibbonTab();
            this.ribbonGroup5 = new Genetibase.UI.RibbonGroup();
            this.prevGroupButton = new Genetibase.UI.RibbonButton();
            this.nextGroupButton = new Genetibase.UI.RibbonButton();
            this.prevAtomButton = new Genetibase.UI.RibbonButton();
            this.nextAtomButton = new Genetibase.UI.RibbonButton();
            this.selectAllButton = new Genetibase.UI.RibbonButton();
            this.ribbonGroup4 = new Genetibase.UI.RibbonGroup();
            this.pasteButton = new Genetibase.UI.RibbonButton();
            this.copyButton = new Genetibase.UI.RibbonButton();
            this.cutButton = new Genetibase.UI.RibbonButton();
            this.redoButton = new Genetibase.UI.RibbonButton();
            this.undoButton = new Genetibase.UI.RibbonButton();
            this.ribbonTab5 = new Genetibase.UI.RibbonTab();
            this.ribbonGroup8 = new Genetibase.UI.RibbonGroup();
            this.showHydButton = new Genetibase.UI.RibbonButton();
            this.ribbonGroup7 = new Genetibase.UI.RibbonGroup();
            this.deleteActualButton = new Genetibase.UI.RibbonButton();
            this.createActualButton = new Genetibase.UI.RibbonButton();
            this.ribbonGroup6 = new Genetibase.UI.RibbonGroup();
            this.zeroExplicitButton = new Genetibase.UI.RibbonButton();
            this.clearExplicitButton = new Genetibase.UI.RibbonButton();
            this.setExplicitButton = new Genetibase.UI.RibbonButton();
            this.ribbonTab6 = new Genetibase.UI.RibbonTab();
            this.ribbonGroup10 = new Genetibase.UI.RibbonGroup();
            this.removeWedgesButton = new Genetibase.UI.RibbonButton();
            this.cycleWedgesButton = new Genetibase.UI.RibbonButton();
            this.ribbonGroup11 = new Genetibase.UI.RibbonGroup();
            this.showStereolabelsButton = new Genetibase.UI.RibbonButton();
            this.setSEbutton = new Genetibase.UI.RibbonButton();
            this.setRZbutton = new Genetibase.UI.RibbonButton();
            this.invertStereochemistryButton = new Genetibase.UI.RibbonButton();
            this.templateButton = new Genetibase.UI.RibbonButton();
            this.editDialogButton = new Genetibase.UI.RibbonButton();
            this.normaliseButton = new Genetibase.UI.RibbonButton();
            this.menuAtomTypes.SuspendLayout();
            this.ribbonControl1.SuspendLayout();
            this.ribbonTab2.SuspendLayout();
            this.ribbonGroup3.SuspendLayout();
            this.ribbonGroup2.SuspendLayout();
            this.ribbonGroup1.SuspendLayout();
            this.ribbonTab3.SuspendLayout();
            this.ribbonGroup5.SuspendLayout();
            this.ribbonGroup4.SuspendLayout();
            this.ribbonTab5.SuspendLayout();
            this.ribbonGroup8.SuspendLayout();
            this.ribbonGroup7.SuspendLayout();
            this.ribbonGroup6.SuspendLayout();
            this.ribbonTab6.SuspendLayout();
            this.ribbonGroup10.SuspendLayout();
            this.ribbonGroup11.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuAtomTypes
            // 
            this.menuAtomTypes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cToolStripMenuItem,
            this.nToolStripMenuItem,
            this.oToolStripMenuItem,
            this.hToolStripMenuItem,
            this.fToolStripMenuItem,
            this.clToolStripMenuItem,
            this.brToolStripMenuItem,
            this.iToolStripMenuItem,
            this.sToolStripMenuItem,
            this.pToolStripMenuItem});
            this.menuAtomTypes.Name = "menuAtomTypes";
            this.menuAtomTypes.Size = new System.Drawing.Size(96, 224);
            this.menuAtomTypes.Opening += new System.ComponentModel.CancelEventHandler(this.menuAtomTypes_Opening);
            // 
            // cToolStripMenuItem
            // 
            this.cToolStripMenuItem.Name = "cToolStripMenuItem";
            this.cToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.cToolStripMenuItem.Text = "C";
            this.cToolStripMenuItem.Click += new System.EventHandler(this.cToolStripMenuItem_Click);
            // 
            // nToolStripMenuItem
            // 
            this.nToolStripMenuItem.Name = "nToolStripMenuItem";
            this.nToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.nToolStripMenuItem.Text = "N";
            this.nToolStripMenuItem.Click += new System.EventHandler(this.cToolStripMenuItem_Click);
            // 
            // oToolStripMenuItem
            // 
            this.oToolStripMenuItem.Name = "oToolStripMenuItem";
            this.oToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.oToolStripMenuItem.Text = "O";
            this.oToolStripMenuItem.Click += new System.EventHandler(this.cToolStripMenuItem_Click);
            // 
            // hToolStripMenuItem
            // 
            this.hToolStripMenuItem.Name = "hToolStripMenuItem";
            this.hToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.hToolStripMenuItem.Text = "H";
            this.hToolStripMenuItem.Click += new System.EventHandler(this.cToolStripMenuItem_Click);
            // 
            // fToolStripMenuItem
            // 
            this.fToolStripMenuItem.Name = "fToolStripMenuItem";
            this.fToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.fToolStripMenuItem.Text = "F";
            this.fToolStripMenuItem.Click += new System.EventHandler(this.cToolStripMenuItem_Click);
            // 
            // clToolStripMenuItem
            // 
            this.clToolStripMenuItem.Name = "clToolStripMenuItem";
            this.clToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.clToolStripMenuItem.Text = "Cl";
            this.clToolStripMenuItem.Click += new System.EventHandler(this.cToolStripMenuItem_Click);
            // 
            // brToolStripMenuItem
            // 
            this.brToolStripMenuItem.Name = "brToolStripMenuItem";
            this.brToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.brToolStripMenuItem.Text = "Br";
            this.brToolStripMenuItem.Click += new System.EventHandler(this.cToolStripMenuItem_Click);
            // 
            // iToolStripMenuItem
            // 
            this.iToolStripMenuItem.Name = "iToolStripMenuItem";
            this.iToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.iToolStripMenuItem.Text = "I";
            this.iToolStripMenuItem.Click += new System.EventHandler(this.cToolStripMenuItem_Click);
            // 
            // sToolStripMenuItem
            // 
            this.sToolStripMenuItem.Name = "sToolStripMenuItem";
            this.sToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.sToolStripMenuItem.Text = "S";
            this.sToolStripMenuItem.Click += new System.EventHandler(this.cToolStripMenuItem_Click);
            // 
            // pToolStripMenuItem
            // 
            this.pToolStripMenuItem.Name = "pToolStripMenuItem";
            this.pToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.pToolStripMenuItem.Text = "P";
            this.pToolStripMenuItem.Click += new System.EventHandler(this.cToolStripMenuItem_Click);
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.Controls.Add(this.ribbonTab1);
            this.ribbonControl1.Controls.Add(this.ribbonTab2);
            this.ribbonControl1.Controls.Add(this.ribbonTab3);
            this.ribbonControl1.Controls.Add(this.ribbonTab5);
            this.ribbonControl1.Controls.Add(this.ribbonTab6);
            this.ribbonControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.SelectedIndex = 1;
            this.ribbonControl1.Size = new System.Drawing.Size(684, 97);
            this.ribbonControl1.TabIndex = 5;
            this.ribbonControl1.OnPopup += new Genetibase.UI.RibbonPopupEventHandler(this.ribbonControl1_OnPopup);
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.Location = new System.Drawing.Point(4, 25);
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Size = new System.Drawing.Size(647, 68);
            this.ribbonTab1.TabIndex = 0;
            this.ribbonTab1.Text = "Menu";
            // 
            // ribbonTab2
            // 
            this.ribbonTab2.Controls.Add(this.ribbonGroup3);
            this.ribbonTab2.Controls.Add(this.ribbonGroup2);
            this.ribbonTab2.Controls.Add(this.ribbonGroup1);
            this.ribbonTab2.Location = new System.Drawing.Point(4, 25);
            this.ribbonTab2.Name = "ribbonTab2";
            this.ribbonTab2.Size = new System.Drawing.Size(676, 68);
            this.ribbonTab2.TabIndex = 1;
            this.ribbonTab2.Text = "Tool";
            // 
            // ribbonGroup3
            // 
            this.ribbonGroup3.Controls.Add(this.addElementButton);
            this.ribbonGroup3.Controls.Add(this.editElementButton);
            this.ribbonGroup3.Location = new System.Drawing.Point(581, 6);
            this.ribbonGroup3.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonGroup3.Name = "ribbonGroup3";
            this.ribbonGroup3.Size = new System.Drawing.Size(92, 58);
            this.ribbonGroup3.TabIndex = 7;
            this.ribbonGroup3.TabStop = false;
            this.ribbonGroup3.Text = "Elements";
            // 
            // addElementButton
            // 
            this.addElementButton.Command = "C";
            this.addElementButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addElementButton.ForeColor = System.Drawing.Color.LimeGreen;
            this.addElementButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.ASelect;
            this.addElementButton.IsFlat = true;
            this.addElementButton.IsPressed = false;
            this.addElementButton.Location = new System.Drawing.Point(49, 5);
            this.addElementButton.Margin = new System.Windows.Forms.Padding(1);
            this.addElementButton.Name = "addElementButton";
            this.addElementButton.Padding = new System.Windows.Forms.Padding(2);
            this.addElementButton.Size = new System.Drawing.Size(36, 35);
            this.addElementButton.TabIndex = 11;
            this.addElementButton.Text = "C";
            this.addElementButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.addElementButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // editElementButton
            // 
            this.editElementButton.Command = "Edit Atom";
            this.editElementButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 0.0001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editElementButton.ForeColor = System.Drawing.Color.Transparent;
            this.editElementButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.AEdit;
            this.editElementButton.IsFlat = true;
            this.editElementButton.IsPressed = false;
            this.editElementButton.Location = new System.Drawing.Point(5, 5);
            this.editElementButton.Margin = new System.Windows.Forms.Padding(1);
            this.editElementButton.Name = "editElementButton";
            this.editElementButton.Padding = new System.Windows.Forms.Padding(2);
            this.editElementButton.Size = new System.Drawing.Size(36, 35);
            this.editElementButton.TabIndex = 10;
            this.editElementButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.editElementButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // ribbonGroup2
            // 
            this.ribbonGroup2.Controls.Add(this.normaliseButton);
            this.ribbonGroup2.Controls.Add(this.unknownBondButton);
            this.ribbonGroup2.Controls.Add(this.zeroBondButton);
            this.ribbonGroup2.Controls.Add(this.declinedBondButton);
            this.ribbonGroup2.Controls.Add(this.inclinedBondButton);
            this.ribbonGroup2.Controls.Add(this.tripleBondButton);
            this.ribbonGroup2.Controls.Add(this.doubleBondButton);
            this.ribbonGroup2.Controls.Add(this.singleBondButton);
            this.ribbonGroup2.Location = new System.Drawing.Point(188, 6);
            this.ribbonGroup2.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonGroup2.Name = "ribbonGroup2";
            this.ribbonGroup2.Size = new System.Drawing.Size(391, 59);
            this.ribbonGroup2.TabIndex = 6;
            this.ribbonGroup2.TabStop = false;
            this.ribbonGroup2.Text = "Bonds";
            // 
            // unknownBondButton
            // 
            this.unknownBondButton.Command = "Unknown Bond";
            this.unknownBondButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 0.0001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unknownBondButton.ForeColor = System.Drawing.Color.Transparent;
            this.unknownBondButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.BUnknown;
            this.unknownBondButton.IsFlat = true;
            this.unknownBondButton.IsPressed = false;
            this.unknownBondButton.Location = new System.Drawing.Point(269, 5);
            this.unknownBondButton.Margin = new System.Windows.Forms.Padding(1);
            this.unknownBondButton.Name = "unknownBondButton";
            this.unknownBondButton.Padding = new System.Windows.Forms.Padding(2);
            this.unknownBondButton.Size = new System.Drawing.Size(36, 35);
            this.unknownBondButton.TabIndex = 7;
            this.unknownBondButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.unknownBondButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // zeroBondButton
            // 
            this.zeroBondButton.Command = "Zero Bond";
            this.zeroBondButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 0.0001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zeroBondButton.ForeColor = System.Drawing.Color.Transparent;
            this.zeroBondButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.BZero;
            this.zeroBondButton.IsFlat = true;
            this.zeroBondButton.IsPressed = false;
            this.zeroBondButton.Location = new System.Drawing.Point(225, 5);
            this.zeroBondButton.Margin = new System.Windows.Forms.Padding(1);
            this.zeroBondButton.Name = "zeroBondButton";
            this.zeroBondButton.Padding = new System.Windows.Forms.Padding(2);
            this.zeroBondButton.Size = new System.Drawing.Size(36, 35);
            this.zeroBondButton.TabIndex = 6;
            this.zeroBondButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.zeroBondButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // declinedBondButton
            // 
            this.declinedBondButton.Command = "Declined Bond";
            this.declinedBondButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 0.0001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.declinedBondButton.ForeColor = System.Drawing.Color.Transparent;
            this.declinedBondButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.BDeclined;
            this.declinedBondButton.IsFlat = true;
            this.declinedBondButton.IsPressed = false;
            this.declinedBondButton.Location = new System.Drawing.Point(181, 5);
            this.declinedBondButton.Margin = new System.Windows.Forms.Padding(1);
            this.declinedBondButton.Name = "declinedBondButton";
            this.declinedBondButton.Padding = new System.Windows.Forms.Padding(2);
            this.declinedBondButton.Size = new System.Drawing.Size(36, 35);
            this.declinedBondButton.TabIndex = 5;
            this.declinedBondButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.declinedBondButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // inclinedBondButton
            // 
            this.inclinedBondButton.Command = "Inclined Bond";
            this.inclinedBondButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 0.0001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inclinedBondButton.ForeColor = System.Drawing.Color.Transparent;
            this.inclinedBondButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.BInclined;
            this.inclinedBondButton.IsFlat = true;
            this.inclinedBondButton.IsPressed = false;
            this.inclinedBondButton.Location = new System.Drawing.Point(137, 5);
            this.inclinedBondButton.Margin = new System.Windows.Forms.Padding(1);
            this.inclinedBondButton.Name = "inclinedBondButton";
            this.inclinedBondButton.Padding = new System.Windows.Forms.Padding(2);
            this.inclinedBondButton.Size = new System.Drawing.Size(36, 35);
            this.inclinedBondButton.TabIndex = 4;
            this.inclinedBondButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.inclinedBondButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // tripleBondButton
            // 
            this.tripleBondButton.Command = "Triple Bond";
            this.tripleBondButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 0.0001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tripleBondButton.ForeColor = System.Drawing.Color.Transparent;
            this.tripleBondButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.BTriple;
            this.tripleBondButton.IsFlat = true;
            this.tripleBondButton.IsPressed = false;
            this.tripleBondButton.Location = new System.Drawing.Point(93, 5);
            this.tripleBondButton.Margin = new System.Windows.Forms.Padding(1);
            this.tripleBondButton.Name = "tripleBondButton";
            this.tripleBondButton.Padding = new System.Windows.Forms.Padding(2);
            this.tripleBondButton.Size = new System.Drawing.Size(36, 35);
            this.tripleBondButton.TabIndex = 3;
            this.tripleBondButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tripleBondButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // doubleBondButton
            // 
            this.doubleBondButton.Command = "Double Bond";
            this.doubleBondButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 0.0001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doubleBondButton.ForeColor = System.Drawing.Color.Transparent;
            this.doubleBondButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.BDouble;
            this.doubleBondButton.IsFlat = true;
            this.doubleBondButton.IsPressed = false;
            this.doubleBondButton.Location = new System.Drawing.Point(49, 5);
            this.doubleBondButton.Margin = new System.Windows.Forms.Padding(1);
            this.doubleBondButton.Name = "doubleBondButton";
            this.doubleBondButton.Padding = new System.Windows.Forms.Padding(2);
            this.doubleBondButton.Size = new System.Drawing.Size(36, 35);
            this.doubleBondButton.TabIndex = 2;
            this.doubleBondButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.doubleBondButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // singleBondButton
            // 
            this.singleBondButton.Command = "Single Bond";
            this.singleBondButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 0.0001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.singleBondButton.ForeColor = System.Drawing.Color.Transparent;
            this.singleBondButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.BSingle;
            this.singleBondButton.IsFlat = true;
            this.singleBondButton.IsPressed = false;
            this.singleBondButton.Location = new System.Drawing.Point(5, 5);
            this.singleBondButton.Margin = new System.Windows.Forms.Padding(1);
            this.singleBondButton.Name = "singleBondButton";
            this.singleBondButton.Padding = new System.Windows.Forms.Padding(2);
            this.singleBondButton.Size = new System.Drawing.Size(36, 35);
            this.singleBondButton.TabIndex = 1;
            this.singleBondButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.singleBondButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // ribbonGroup1
            // 
            this.ribbonGroup1.Controls.Add(this.chargeButton);
            this.ribbonGroup1.Controls.Add(this.rotateButton);
            this.ribbonGroup1.Controls.Add(this.eraseButton);
            this.ribbonGroup1.Controls.Add(this.selectButton);
            this.ribbonGroup1.Location = new System.Drawing.Point(6, 6);
            this.ribbonGroup1.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonGroup1.Name = "ribbonGroup1";
            this.ribbonGroup1.Size = new System.Drawing.Size(180, 59);
            this.ribbonGroup1.TabIndex = 0;
            this.ribbonGroup1.TabStop = false;
            this.ribbonGroup1.Text = "Workspace";
            // 
            // chargeButton
            // 
            this.chargeButton.Command = "Charge";
            this.chargeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 0.0001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chargeButton.ForeColor = System.Drawing.Color.Transparent;
            this.chargeButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.ACharge;
            this.chargeButton.IsFlat = true;
            this.chargeButton.IsPressed = false;
            this.chargeButton.Location = new System.Drawing.Point(135, 5);
            this.chargeButton.Margin = new System.Windows.Forms.Padding(1);
            this.chargeButton.Name = "chargeButton";
            this.chargeButton.Padding = new System.Windows.Forms.Padding(2);
            this.chargeButton.Size = new System.Drawing.Size(36, 35);
            this.chargeButton.TabIndex = 5;
            this.chargeButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chargeButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // rotateButton
            // 
            this.rotateButton.Command = "Rotator";
            this.rotateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 0.0001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rotateButton.ForeColor = System.Drawing.Color.Transparent;
            this.rotateButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.Rotator;
            this.rotateButton.IsFlat = true;
            this.rotateButton.IsPressed = false;
            this.rotateButton.Location = new System.Drawing.Point(93, 5);
            this.rotateButton.Margin = new System.Windows.Forms.Padding(1);
            this.rotateButton.Name = "rotateButton";
            this.rotateButton.Padding = new System.Windows.Forms.Padding(2);
            this.rotateButton.Size = new System.Drawing.Size(36, 35);
            this.rotateButton.TabIndex = 3;
            this.rotateButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rotateButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // eraseButton
            // 
            this.eraseButton.AllowDrop = true;
            this.eraseButton.Command = "Eraser";
            this.eraseButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.Erasor;
            this.eraseButton.IsFlat = true;
            this.eraseButton.IsPressed = false;
            this.eraseButton.Location = new System.Drawing.Point(49, 5);
            this.eraseButton.Margin = new System.Windows.Forms.Padding(1);
            this.eraseButton.Name = "eraseButton";
            this.eraseButton.Padding = new System.Windows.Forms.Padding(2);
            this.eraseButton.Size = new System.Drawing.Size(36, 35);
            this.eraseButton.TabIndex = 2;
            this.eraseButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.eraseButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // selectButton
            // 
            this.selectButton.Command = "Cursor";
            this.selectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 0.0001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectButton.ForeColor = System.Drawing.Color.Transparent;
            this.selectButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.Cursor;
            this.selectButton.IsFlat = true;
            this.selectButton.IsPressed = false;
            this.selectButton.Location = new System.Drawing.Point(5, 5);
            this.selectButton.Margin = new System.Windows.Forms.Padding(1);
            this.selectButton.Name = "selectButton";
            this.selectButton.Padding = new System.Windows.Forms.Padding(2);
            this.selectButton.Size = new System.Drawing.Size(36, 35);
            this.selectButton.TabIndex = 1;
            this.selectButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.selectButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // ribbonTab3
            // 
            this.ribbonTab3.Controls.Add(this.ribbonGroup5);
            this.ribbonTab3.Controls.Add(this.ribbonGroup4);
            this.ribbonTab3.Location = new System.Drawing.Point(4, 25);
            this.ribbonTab3.Name = "ribbonTab3";
            this.ribbonTab3.Size = new System.Drawing.Size(647, 68);
            this.ribbonTab3.TabIndex = 2;
            this.ribbonTab3.Text = "Edit";
            // 
            // ribbonGroup5
            // 
            this.ribbonGroup5.Controls.Add(this.prevGroupButton);
            this.ribbonGroup5.Controls.Add(this.nextGroupButton);
            this.ribbonGroup5.Controls.Add(this.prevAtomButton);
            this.ribbonGroup5.Controls.Add(this.nextAtomButton);
            this.ribbonGroup5.Controls.Add(this.selectAllButton);
            this.ribbonGroup5.Location = new System.Drawing.Point(240, 6);
            this.ribbonGroup5.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonGroup5.Name = "ribbonGroup5";
            this.ribbonGroup5.Size = new System.Drawing.Size(272, 59);
            this.ribbonGroup5.TabIndex = 6;
            this.ribbonGroup5.TabStop = false;
            this.ribbonGroup5.Text = "Selection";
            // 
            // prevGroupButton
            // 
            this.prevGroupButton.Command = "Previous Group";
            this.prevGroupButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevGroupButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.prevGroupButton.IsFlat = true;
            this.prevGroupButton.IsPressed = false;
            this.prevGroupButton.Location = new System.Drawing.Point(213, 5);
            this.prevGroupButton.Margin = new System.Windows.Forms.Padding(1);
            this.prevGroupButton.Name = "prevGroupButton";
            this.prevGroupButton.Padding = new System.Windows.Forms.Padding(2);
            this.prevGroupButton.Size = new System.Drawing.Size(55, 35);
            this.prevGroupButton.TabIndex = 5;
            this.prevGroupButton.Text = "Previous Group";
            this.prevGroupButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.prevGroupButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // nextGroupButton
            // 
            this.nextGroupButton.Command = "Next Group";
            this.nextGroupButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextGroupButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.nextGroupButton.IsFlat = true;
            this.nextGroupButton.IsPressed = false;
            this.nextGroupButton.Location = new System.Drawing.Point(160, 5);
            this.nextGroupButton.Margin = new System.Windows.Forms.Padding(1);
            this.nextGroupButton.Name = "nextGroupButton";
            this.nextGroupButton.Padding = new System.Windows.Forms.Padding(2);
            this.nextGroupButton.Size = new System.Drawing.Size(45, 35);
            this.nextGroupButton.TabIndex = 4;
            this.nextGroupButton.Text = "Next Group";
            this.nextGroupButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.nextGroupButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // prevAtomButton
            // 
            this.prevAtomButton.Command = "Previous Atom";
            this.prevAtomButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevAtomButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.prevAtomButton.IsFlat = true;
            this.prevAtomButton.IsPressed = false;
            this.prevAtomButton.Location = new System.Drawing.Point(97, 5);
            this.prevAtomButton.Margin = new System.Windows.Forms.Padding(1);
            this.prevAtomButton.Name = "prevAtomButton";
            this.prevAtomButton.Padding = new System.Windows.Forms.Padding(2);
            this.prevAtomButton.Size = new System.Drawing.Size(55, 35);
            this.prevAtomButton.TabIndex = 3;
            this.prevAtomButton.Text = "Previous Atom";
            this.prevAtomButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.prevAtomButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // nextAtomButton
            // 
            this.nextAtomButton.AllowDrop = true;
            this.nextAtomButton.Command = "Next Atom";
            this.nextAtomButton.IsFlat = true;
            this.nextAtomButton.IsPressed = false;
            this.nextAtomButton.Location = new System.Drawing.Point(53, 5);
            this.nextAtomButton.Margin = new System.Windows.Forms.Padding(1);
            this.nextAtomButton.Name = "nextAtomButton";
            this.nextAtomButton.Padding = new System.Windows.Forms.Padding(2);
            this.nextAtomButton.Size = new System.Drawing.Size(36, 35);
            this.nextAtomButton.TabIndex = 2;
            this.nextAtomButton.Text = "Next Atom";
            this.nextAtomButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.nextAtomButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // selectAllButton
            // 
            this.selectAllButton.Command = "Select All";
            this.selectAllButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectAllButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.selectAllButton.IsFlat = true;
            this.selectAllButton.IsPressed = false;
            this.selectAllButton.Location = new System.Drawing.Point(5, 5);
            this.selectAllButton.Margin = new System.Windows.Forms.Padding(1);
            this.selectAllButton.Name = "selectAllButton";
            this.selectAllButton.Padding = new System.Windows.Forms.Padding(2);
            this.selectAllButton.Size = new System.Drawing.Size(42, 35);
            this.selectAllButton.TabIndex = 1;
            this.selectAllButton.Text = "Select All";
            this.selectAllButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.selectAllButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // ribbonGroup4
            // 
            this.ribbonGroup4.Controls.Add(this.pasteButton);
            this.ribbonGroup4.Controls.Add(this.copyButton);
            this.ribbonGroup4.Controls.Add(this.cutButton);
            this.ribbonGroup4.Controls.Add(this.redoButton);
            this.ribbonGroup4.Controls.Add(this.undoButton);
            this.ribbonGroup4.Location = new System.Drawing.Point(6, 6);
            this.ribbonGroup4.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonGroup4.Name = "ribbonGroup4";
            this.ribbonGroup4.Size = new System.Drawing.Size(231, 59);
            this.ribbonGroup4.TabIndex = 1;
            this.ribbonGroup4.TabStop = false;
            this.ribbonGroup4.Text = "Workspace";
            // 
            // pasteButton
            // 
            this.pasteButton.Command = "Paste";
            this.pasteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 0.0001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pasteButton.ForeColor = System.Drawing.Color.Transparent;
            this.pasteButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.EPaste;
            this.pasteButton.IsFlat = true;
            this.pasteButton.IsPressed = false;
            this.pasteButton.Location = new System.Drawing.Point(189, 5);
            this.pasteButton.Margin = new System.Windows.Forms.Padding(1);
            this.pasteButton.Name = "pasteButton";
            this.pasteButton.Padding = new System.Windows.Forms.Padding(2);
            this.pasteButton.Size = new System.Drawing.Size(36, 35);
            this.pasteButton.TabIndex = 5;
            this.pasteButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.pasteButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // copyButton
            // 
            this.copyButton.Command = "Copy";
            this.copyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 0.0001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyButton.ForeColor = System.Drawing.Color.Transparent;
            this.copyButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.ECopy;
            this.copyButton.IsFlat = true;
            this.copyButton.IsPressed = false;
            this.copyButton.Location = new System.Drawing.Point(143, 5);
            this.copyButton.Margin = new System.Windows.Forms.Padding(1);
            this.copyButton.Name = "copyButton";
            this.copyButton.Padding = new System.Windows.Forms.Padding(2);
            this.copyButton.Size = new System.Drawing.Size(36, 35);
            this.copyButton.TabIndex = 5;
            this.copyButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.copyButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // cutButton
            // 
            this.cutButton.Command = "Cut";
            this.cutButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 0.0001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cutButton.ForeColor = System.Drawing.Color.Transparent;
            this.cutButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.ECut;
            this.cutButton.IsFlat = true;
            this.cutButton.IsPressed = false;
            this.cutButton.Location = new System.Drawing.Point(97, 5);
            this.cutButton.Margin = new System.Windows.Forms.Padding(1);
            this.cutButton.Name = "cutButton";
            this.cutButton.Padding = new System.Windows.Forms.Padding(2);
            this.cutButton.Size = new System.Drawing.Size(36, 35);
            this.cutButton.TabIndex = 4;
            this.cutButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cutButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // redoButton
            // 
            this.redoButton.Command = "Redo";
            this.redoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 0.0001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.redoButton.ForeColor = System.Drawing.Color.Transparent;
            this.redoButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.Redo;
            this.redoButton.IsFlat = true;
            this.redoButton.IsPressed = false;
            this.redoButton.Location = new System.Drawing.Point(51, 5);
            this.redoButton.Margin = new System.Windows.Forms.Padding(1);
            this.redoButton.Name = "redoButton";
            this.redoButton.Padding = new System.Windows.Forms.Padding(2);
            this.redoButton.Size = new System.Drawing.Size(36, 35);
            this.redoButton.TabIndex = 3;
            this.redoButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.redoButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // undoButton
            // 
            this.undoButton.AllowDrop = true;
            this.undoButton.Command = "Undo";
            this.undoButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.Undo;
            this.undoButton.IsFlat = true;
            this.undoButton.IsPressed = false;
            this.undoButton.Location = new System.Drawing.Point(5, 5);
            this.undoButton.Margin = new System.Windows.Forms.Padding(1);
            this.undoButton.Name = "undoButton";
            this.undoButton.Padding = new System.Windows.Forms.Padding(2);
            this.undoButton.Size = new System.Drawing.Size(36, 35);
            this.undoButton.TabIndex = 2;
            this.undoButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.undoButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // ribbonTab5
            // 
            this.ribbonTab5.Controls.Add(this.ribbonGroup8);
            this.ribbonTab5.Controls.Add(this.ribbonGroup7);
            this.ribbonTab5.Controls.Add(this.ribbonGroup6);
            this.ribbonTab5.Location = new System.Drawing.Point(4, 25);
            this.ribbonTab5.Name = "ribbonTab5";
            this.ribbonTab5.Size = new System.Drawing.Size(647, 68);
            this.ribbonTab5.TabIndex = 4;
            this.ribbonTab5.Text = "Hydrogen";
            // 
            // ribbonGroup8
            // 
            this.ribbonGroup8.AutoSize = true;
            this.ribbonGroup8.Controls.Add(this.showHydButton);
            this.ribbonGroup8.Location = new System.Drawing.Point(7, 6);
            this.ribbonGroup8.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonGroup8.Name = "ribbonGroup8";
            this.ribbonGroup8.Size = new System.Drawing.Size(76, 59);
            this.ribbonGroup8.TabIndex = 6;
            this.ribbonGroup8.TabStop = false;
            // 
            // showHydButton
            // 
            this.showHydButton.AllowDrop = true;
            this.showHydButton.Command = "Show Hydrogen";
            this.showHydButton.IsFlat = true;
            this.showHydButton.IsPressed = true;
            this.showHydButton.Location = new System.Drawing.Point(4, 5);
            this.showHydButton.Margin = new System.Windows.Forms.Padding(1);
            this.showHydButton.Name = "showHydButton";
            this.showHydButton.Padding = new System.Windows.Forms.Padding(2);
            this.showHydButton.Size = new System.Drawing.Size(68, 35);
            this.showHydButton.TabIndex = 3;
            this.showHydButton.Text = "Show Hydrogen";
            this.showHydButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.showHydButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // ribbonGroup7
            // 
            this.ribbonGroup7.Controls.Add(this.deleteActualButton);
            this.ribbonGroup7.Controls.Add(this.createActualButton);
            this.ribbonGroup7.Location = new System.Drawing.Point(258, 6);
            this.ribbonGroup7.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonGroup7.Name = "ribbonGroup7";
            this.ribbonGroup7.Size = new System.Drawing.Size(105, 59);
            this.ribbonGroup7.TabIndex = 8;
            this.ribbonGroup7.TabStop = false;
            this.ribbonGroup7.Text = "Actual";
            // 
            // deleteActualButton
            // 
            this.deleteActualButton.AllowDrop = true;
            this.deleteActualButton.Command = "Delete Actual";
            this.deleteActualButton.IsFlat = true;
            this.deleteActualButton.IsPressed = false;
            this.deleteActualButton.Location = new System.Drawing.Point(55, 5);
            this.deleteActualButton.Margin = new System.Windows.Forms.Padding(1);
            this.deleteActualButton.Name = "deleteActualButton";
            this.deleteActualButton.Padding = new System.Windows.Forms.Padding(2);
            this.deleteActualButton.Size = new System.Drawing.Size(42, 35);
            this.deleteActualButton.TabIndex = 2;
            this.deleteActualButton.Text = "Delete";
            this.deleteActualButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.deleteActualButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // createActualButton
            // 
            this.createActualButton.Command = "Create Actual";
            this.createActualButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createActualButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.createActualButton.IsFlat = true;
            this.createActualButton.IsPressed = false;
            this.createActualButton.Location = new System.Drawing.Point(5, 5);
            this.createActualButton.Margin = new System.Windows.Forms.Padding(1);
            this.createActualButton.Name = "createActualButton";
            this.createActualButton.Padding = new System.Windows.Forms.Padding(2);
            this.createActualButton.Size = new System.Drawing.Size(42, 35);
            this.createActualButton.TabIndex = 1;
            this.createActualButton.Text = "Create";
            this.createActualButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.createActualButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // ribbonGroup6
            // 
            this.ribbonGroup6.Controls.Add(this.zeroExplicitButton);
            this.ribbonGroup6.Controls.Add(this.clearExplicitButton);
            this.ribbonGroup6.Controls.Add(this.setExplicitButton);
            this.ribbonGroup6.Location = new System.Drawing.Point(85, 6);
            this.ribbonGroup6.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonGroup6.Name = "ribbonGroup6";
            this.ribbonGroup6.Size = new System.Drawing.Size(171, 59);
            this.ribbonGroup6.TabIndex = 8;
            this.ribbonGroup6.TabStop = false;
            this.ribbonGroup6.Text = "Explicit";
            // 
            // zeroExplicitButton
            // 
            this.zeroExplicitButton.Command = "Zero Explicit";
            this.zeroExplicitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zeroExplicitButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.zeroExplicitButton.IsFlat = true;
            this.zeroExplicitButton.IsPressed = false;
            this.zeroExplicitButton.Location = new System.Drawing.Point(108, 5);
            this.zeroExplicitButton.Margin = new System.Windows.Forms.Padding(1);
            this.zeroExplicitButton.Name = "zeroExplicitButton";
            this.zeroExplicitButton.Padding = new System.Windows.Forms.Padding(2);
            this.zeroExplicitButton.Size = new System.Drawing.Size(55, 35);
            this.zeroExplicitButton.TabIndex = 3;
            this.zeroExplicitButton.Text = "Zero";
            this.zeroExplicitButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.zeroExplicitButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // clearExplicitButton
            // 
            this.clearExplicitButton.AllowDrop = true;
            this.clearExplicitButton.Command = "Clear Explicit";
            this.clearExplicitButton.IsFlat = true;
            this.clearExplicitButton.IsPressed = false;
            this.clearExplicitButton.Location = new System.Drawing.Point(55, 5);
            this.clearExplicitButton.Margin = new System.Windows.Forms.Padding(1);
            this.clearExplicitButton.Name = "clearExplicitButton";
            this.clearExplicitButton.Padding = new System.Windows.Forms.Padding(2);
            this.clearExplicitButton.Size = new System.Drawing.Size(42, 35);
            this.clearExplicitButton.TabIndex = 2;
            this.clearExplicitButton.Text = "Clear";
            this.clearExplicitButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.clearExplicitButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // setExplicitButton
            // 
            this.setExplicitButton.Command = "Set Explicit";
            this.setExplicitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setExplicitButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.setExplicitButton.IsFlat = true;
            this.setExplicitButton.IsPressed = false;
            this.setExplicitButton.Location = new System.Drawing.Point(5, 5);
            this.setExplicitButton.Margin = new System.Windows.Forms.Padding(1);
            this.setExplicitButton.Name = "setExplicitButton";
            this.setExplicitButton.Padding = new System.Windows.Forms.Padding(2);
            this.setExplicitButton.Size = new System.Drawing.Size(42, 35);
            this.setExplicitButton.TabIndex = 1;
            this.setExplicitButton.Text = "Set";
            this.setExplicitButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.setExplicitButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // ribbonTab6
            // 
            this.ribbonTab6.Controls.Add(this.ribbonGroup10);
            this.ribbonTab6.Controls.Add(this.ribbonGroup11);
            this.ribbonTab6.Location = new System.Drawing.Point(4, 25);
            this.ribbonTab6.Name = "ribbonTab6";
            this.ribbonTab6.Size = new System.Drawing.Size(647, 68);
            this.ribbonTab6.TabIndex = 5;
            this.ribbonTab6.Text = "Stereochemistry";
            // 
            // ribbonGroup10
            // 
            this.ribbonGroup10.Controls.Add(this.removeWedgesButton);
            this.ribbonGroup10.Controls.Add(this.cycleWedgesButton);
            this.ribbonGroup10.Location = new System.Drawing.Point(253, 6);
            this.ribbonGroup10.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonGroup10.Name = "ribbonGroup10";
            this.ribbonGroup10.Size = new System.Drawing.Size(114, 59);
            this.ribbonGroup10.TabIndex = 11;
            this.ribbonGroup10.TabStop = false;
            this.ribbonGroup10.Text = "Wedges";
            // 
            // removeWedgesButton
            // 
            this.removeWedgesButton.AllowDrop = true;
            this.removeWedgesButton.Command = "Remove Wedges";
            this.removeWedgesButton.IsFlat = true;
            this.removeWedgesButton.IsPressed = false;
            this.removeWedgesButton.Location = new System.Drawing.Point(55, 5);
            this.removeWedgesButton.Margin = new System.Windows.Forms.Padding(1);
            this.removeWedgesButton.Name = "removeWedgesButton";
            this.removeWedgesButton.Padding = new System.Windows.Forms.Padding(2);
            this.removeWedgesButton.Size = new System.Drawing.Size(51, 35);
            this.removeWedgesButton.TabIndex = 2;
            this.removeWedgesButton.Text = "Remove";
            this.removeWedgesButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.removeWedgesButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // cycleWedgesButton
            // 
            this.cycleWedgesButton.Command = "Cycle Wedges";
            this.cycleWedgesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cycleWedgesButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cycleWedgesButton.IsFlat = true;
            this.cycleWedgesButton.IsPressed = false;
            this.cycleWedgesButton.Location = new System.Drawing.Point(5, 5);
            this.cycleWedgesButton.Margin = new System.Windows.Forms.Padding(1);
            this.cycleWedgesButton.Name = "cycleWedgesButton";
            this.cycleWedgesButton.Padding = new System.Windows.Forms.Padding(2);
            this.cycleWedgesButton.Size = new System.Drawing.Size(42, 35);
            this.cycleWedgesButton.TabIndex = 1;
            this.cycleWedgesButton.Text = "Cycle";
            this.cycleWedgesButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cycleWedgesButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // ribbonGroup11
            // 
            this.ribbonGroup11.Controls.Add(this.showStereolabelsButton);
            this.ribbonGroup11.Controls.Add(this.setSEbutton);
            this.ribbonGroup11.Controls.Add(this.setRZbutton);
            this.ribbonGroup11.Controls.Add(this.invertStereochemistryButton);
            this.ribbonGroup11.Location = new System.Drawing.Point(6, 6);
            this.ribbonGroup11.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonGroup11.Name = "ribbonGroup11";
            this.ribbonGroup11.Size = new System.Drawing.Size(245, 59);
            this.ribbonGroup11.TabIndex = 10;
            this.ribbonGroup11.TabStop = false;
            this.ribbonGroup11.Text = "Stereochemistry";
            // 
            // showStereolabelsButton
            // 
            this.showStereolabelsButton.AllowDrop = true;
            this.showStereolabelsButton.Command = "Show Stereolabels";
            this.showStereolabelsButton.IsFlat = true;
            this.showStereolabelsButton.IsPressed = false;
            this.showStereolabelsButton.Location = new System.Drawing.Point(4, 5);
            this.showStereolabelsButton.Margin = new System.Windows.Forms.Padding(1);
            this.showStereolabelsButton.Name = "showStereolabelsButton";
            this.showStereolabelsButton.Padding = new System.Windows.Forms.Padding(2);
            this.showStereolabelsButton.Size = new System.Drawing.Size(81, 35);
            this.showStereolabelsButton.TabIndex = 3;
            this.showStereolabelsButton.Text = "Show Stereolabels";
            this.showStereolabelsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.showStereolabelsButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // setSEbutton
            // 
            this.setSEbutton.Command = "Set S/E";
            this.setSEbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setSEbutton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.setSEbutton.IsFlat = true;
            this.setSEbutton.IsPressed = false;
            this.setSEbutton.Location = new System.Drawing.Point(194, 5);
            this.setSEbutton.Margin = new System.Windows.Forms.Padding(1);
            this.setSEbutton.Name = "setSEbutton";
            this.setSEbutton.Padding = new System.Windows.Forms.Padding(2);
            this.setSEbutton.Size = new System.Drawing.Size(44, 35);
            this.setSEbutton.TabIndex = 3;
            this.setSEbutton.Text = "Set S/E";
            this.setSEbutton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.setSEbutton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // setRZbutton
            // 
            this.setRZbutton.AllowDrop = true;
            this.setRZbutton.Command = "Set R/Z";
            this.setRZbutton.IsFlat = true;
            this.setRZbutton.IsPressed = false;
            this.setRZbutton.Location = new System.Drawing.Point(141, 5);
            this.setRZbutton.Margin = new System.Windows.Forms.Padding(1);
            this.setRZbutton.Name = "setRZbutton";
            this.setRZbutton.Padding = new System.Windows.Forms.Padding(2);
            this.setRZbutton.Size = new System.Drawing.Size(42, 35);
            this.setRZbutton.TabIndex = 2;
            this.setRZbutton.Text = "Set R/Z";
            this.setRZbutton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.setRZbutton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // invertStereochemistryButton
            // 
            this.invertStereochemistryButton.Command = "Invert Stereochemistry";
            this.invertStereochemistryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.invertStereochemistryButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.invertStereochemistryButton.IsFlat = true;
            this.invertStereochemistryButton.IsPressed = false;
            this.invertStereochemistryButton.Location = new System.Drawing.Point(91, 5);
            this.invertStereochemistryButton.Margin = new System.Windows.Forms.Padding(1);
            this.invertStereochemistryButton.Name = "invertStereochemistryButton";
            this.invertStereochemistryButton.Padding = new System.Windows.Forms.Padding(2);
            this.invertStereochemistryButton.Size = new System.Drawing.Size(42, 35);
            this.invertStereochemistryButton.TabIndex = 1;
            this.invertStereochemistryButton.Text = "Invert";
            this.invertStereochemistryButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.invertStereochemistryButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // templateButton
            // 
            this.templateButton.Command = "Templates";
            this.templateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 0.0001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.templateButton.ForeColor = System.Drawing.Color.Transparent;
            this.templateButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.Template;
            this.templateButton.IsFlat = true;
            this.templateButton.IsPressed = false;
            this.templateButton.Location = new System.Drawing.Point(137, 5);
            this.templateButton.Margin = new System.Windows.Forms.Padding(1);
            this.templateButton.Name = "templateButton";
            this.templateButton.Padding = new System.Windows.Forms.Padding(2);
            this.templateButton.Size = new System.Drawing.Size(36, 35);
            this.templateButton.TabIndex = 4;
            this.templateButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.templateButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // editDialogButton
            // 
            this.editDialogButton.Command = "Edit...";
            this.editDialogButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editDialogButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.editDialogButton.Image = global::Genetibase.Chem.NuGenSChem.Properties.Resources.EDialog;
            this.editDialogButton.IsFlat = true;
            this.editDialogButton.IsPressed = false;
            this.editDialogButton.Location = new System.Drawing.Point(5, 5);
            this.editDialogButton.Margin = new System.Windows.Forms.Padding(1);
            this.editDialogButton.Name = "editDialogButton";
            this.editDialogButton.Padding = new System.Windows.Forms.Padding(2);
            this.editDialogButton.Size = new System.Drawing.Size(36, 35);
            this.editDialogButton.TabIndex = 1;
            this.editDialogButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.editDialogButton.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // editorPane1
            // 
            /*this.editorPane1.BackColor = System.Drawing.Color.Black;
            this.editorPane1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorPane1.Location = new System.Drawing.Point(0, 0);
            this.editorPane1.Name = "editorPane1";
            this.editorPane1.Selected = new bool[0];
            this.editorPane1.Size = this.Size;
            this.editorPane1.TabIndex = 4;
            this.editorPane1.Text = "editorPane1";
            /*/ 
            // normaliseButton
            // 
            this.normaliseButton.Command = "Unknown Bond";
            this.normaliseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.normaliseButton.ForeColor = System.Drawing.Color.Black;
            this.normaliseButton.IsFlat = true;
            this.normaliseButton.IsPressed = false;
            this.normaliseButton.Location = new System.Drawing.Point(314, 5);
            this.normaliseButton.Margin = new System.Windows.Forms.Padding(1);
            this.normaliseButton.Name = "normaliseButton";
            this.normaliseButton.Padding = new System.Windows.Forms.Padding(2);
            this.normaliseButton.Size = new System.Drawing.Size(72, 35);
            this.normaliseButton.TabIndex = 7;
            this.normaliseButton.Text = "Normalise";
            this.normaliseButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.normaliseButton.Click += new System.EventHandler(this.normaliseButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ribbonControl1);
            this.Controls.Add(this.editorPane1);
            this.Name = "MainWindow";
            this.Size = new System.Drawing.Size(684, 512);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.menuAtomTypes.ResumeLayout(false);
            this.ribbonControl1.ResumeLayout(false);
            this.ribbonTab2.ResumeLayout(false);
            this.ribbonGroup3.ResumeLayout(false);
            this.ribbonGroup2.ResumeLayout(false);
            this.ribbonGroup1.ResumeLayout(false);
            this.ribbonTab3.ResumeLayout(false);
            this.ribbonGroup5.ResumeLayout(false);
            this.ribbonGroup4.ResumeLayout(false);
            this.ribbonTab5.ResumeLayout(false);
            this.ribbonTab5.PerformLayout();
            this.ribbonGroup8.ResumeLayout(false);
            this.ribbonGroup7.ResumeLayout(false);
            this.ribbonGroup6.ResumeLayout(false);
            this.ribbonTab6.ResumeLayout(false);
            this.ribbonGroup10.ResumeLayout(false);
            this.ribbonGroup11.ResumeLayout(false);
            this.ResumeLayout(false);

        }        

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            this.editor.Size = this.Size;
        }

        void ribbonControl1_OnPopup(object sender)
        {
            menu.Location = ((RibbonControl)sender).PointToScreen(new Point(ribbonControl1.Location.X + 5, ribbonControl1.Location.Y + 22));
            menu.Show();
        }

        public void New()
        {
            if (MolData().NumAtoms() > 0)
            {
                if (MessageBox.Show(this, "Clear current structure and open new document?", "New", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                    return;
            }
            Clear();
        }

        public void SaveAs()
        {
            System.Windows.Forms.FileDialog chooser = new SaveFileDialog();
            chooser.InitialDirectory = Directory.GetCurrentDirectory();
            chooser.Filter = "NuGenChem Files | *.ngc";            

            if (chooser.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;

            SaveAs(chooser.FileName); 
        }

        public void ExportMOL()
        {
            System.Windows.Forms.FileDialog chooser = new SaveFileDialog();
            chooser.InitialDirectory = Directory.GetCurrentDirectory();            
            chooser.Filter = "MDL MOL Files | *.mol";
            if ((int)chooser.ShowDialog(this) != (int)System.Windows.Forms.DialogResult.OK)
                return;

            ExportMOL(chooser.FileName); 
        }

        public void ExportCML()
        {
            FileDialog chooser = new SaveFileDialog();
            chooser.InitialDirectory = Directory.GetCurrentDirectory();            
            chooser.Filter = "XML Files | *.xml";
            if ((int)chooser.ShowDialog(this) != (int)System.Windows.Forms.DialogResult.OK)
                return;

            ExportCML(chooser.FileName); 
        }

        public void ExportBMP()
        {
            System.Windows.Forms.FileDialog chooser = new SaveFileDialog();
            chooser.InitialDirectory = Directory.GetCurrentDirectory();
            chooser.Filter = "Bitmap Files | *.bmp";
            if ((int)chooser.ShowDialog(this) != (int)System.Windows.Forms.DialogResult.OK)
                return;

            ExportBMP(chooser.FileName); 
        }

        public void Open()
        {
            System.Windows.Forms.FileDialog chooser = new OpenFileDialog();
            chooser.InitialDirectory = Directory.GetCurrentDirectory();
            chooser.Filter = "Molecular Structures|*.el;*.mol;*.sdf|CML Files|*.xml;*.cml|All Files|*.*";
            if (chooser.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;

            Open(chooser.FileName); 
        }				

        public void ZoomFull()
        {
            editor.ZoomFull();
        }

        public void ZoomIn()
        {
            editor.ZoomIn(1.5);
        }

        public void ZoomOut()
        {
            editor.ZoomOut(1.5);
        }

        public void ShowElements()
        {
            editor.SetShowMode(EditorPane.SHOW_ELEMENTS);
        }

        public void ShowAllElements()
        {
            editor.SetShowMode(EditorPane.SHOW_ALL_ELEMENTS);
        }

        public void ShowIndices()
        {
            editor.SetShowMode(EditorPane.SHOW_INDEXES);
        }

        public void ShowRingID()
        {
            editor.SetShowMode(EditorPane.SHOW_RINGID);
        }

        public void ShowCIPPriority()
        {
            editor.SetShowMode(EditorPane.SHOW_PRIORITY);
        }

        #endregion

        private EditorPane editorPane1;
        private System.Windows.Forms.ContextMenuStrip menuAtomTypes;
        private System.Windows.Forms.ToolStripMenuItem cToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem brToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pToolStripMenuItem;
        private Genetibase.UI.RibbonControl ribbonControl1;
        private Genetibase.UI.RibbonTab ribbonTab1;
        private Genetibase.UI.RibbonTab ribbonTab2;
        private Genetibase.UI.RibbonGroup ribbonGroup1;
        private Genetibase.UI.RibbonButton selectButton;
        private Genetibase.UI.RibbonGroup ribbonGroup2;
        private Genetibase.UI.RibbonButton declinedBondButton;
        private Genetibase.UI.RibbonButton inclinedBondButton;
        private Genetibase.UI.RibbonButton tripleBondButton;
        private Genetibase.UI.RibbonButton doubleBondButton;
        private Genetibase.UI.RibbonButton singleBondButton;
        private Genetibase.UI.RibbonButton chargeButton;
        private Genetibase.UI.RibbonButton templateButton;
        private Genetibase.UI.RibbonButton rotateButton;
        private Genetibase.UI.RibbonButton eraseButton;
        private Genetibase.UI.RibbonButton unknownBondButton;
        private Genetibase.UI.RibbonButton zeroBondButton;
        private Genetibase.UI.RibbonGroup ribbonGroup3;
        private Genetibase.UI.RibbonButton addElementButton;
        private Genetibase.UI.RibbonButton editElementButton;
        private Genetibase.UI.RibbonTab ribbonTab3;
        private Genetibase.UI.RibbonTab ribbonTab5;
        private Genetibase.UI.RibbonTab ribbonTab6;
        private Genetibase.UI.RibbonGroup ribbonGroup4;
        private Genetibase.UI.RibbonButton copyButton;
        private Genetibase.UI.RibbonButton cutButton;
        private Genetibase.UI.RibbonButton editDialogButton;
        private Genetibase.UI.RibbonButton pasteButton;
        private Genetibase.UI.RibbonButton redoButton;
        private Genetibase.UI.RibbonButton undoButton;
        private Genetibase.UI.RibbonGroup ribbonGroup5;
        private Genetibase.UI.RibbonButton nextGroupButton;
        private Genetibase.UI.RibbonButton prevAtomButton;
        private Genetibase.UI.RibbonButton nextAtomButton;
        private Genetibase.UI.RibbonButton selectAllButton;
        private Genetibase.UI.RibbonButton prevGroupButton;
        private Genetibase.UI.RibbonButton showHydButton;
        private Genetibase.UI.RibbonGroup ribbonGroup7;
        private Genetibase.UI.RibbonButton deleteActualButton;
        private Genetibase.UI.RibbonButton createActualButton;
        private Genetibase.UI.RibbonGroup ribbonGroup6;
        private Genetibase.UI.RibbonButton zeroExplicitButton;
        private Genetibase.UI.RibbonButton clearExplicitButton;
        private Genetibase.UI.RibbonButton setExplicitButton;
        private Genetibase.UI.RibbonGroup ribbonGroup8;
        private Genetibase.UI.RibbonGroup ribbonGroup10;
        private Genetibase.UI.RibbonButton removeWedgesButton;
        private Genetibase.UI.RibbonButton cycleWedgesButton;
        private Genetibase.UI.RibbonGroup ribbonGroup11;
        private Genetibase.UI.RibbonButton showStereolabelsButton;
        private Genetibase.UI.RibbonButton setSEbutton;
        private Genetibase.UI.RibbonButton setRZbutton;
        private Genetibase.UI.RibbonButton invertStereochemistryButton;

        private NuGenMainPopupMenu menu;
        private RibbonButton normaliseButton;
    }
}
