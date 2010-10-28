namespace GraphSynth.Forms
{
    partial class PropertiesWindow
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
            this.propertiesTabControl = new System.Windows.Forms.TabControl();
            this.nodeArcPropsTab = new System.Windows.Forms.TabPage();
            this.nodeArcProps = new System.Windows.Forms.PropertyGrid();
            this.graphRulePropsTab = new System.Windows.Forms.TabPage();
            this.graphRuleProps = new System.Windows.Forms.PropertyGrid();
            this.displayPropsTab = new System.Windows.Forms.TabPage();
            this.displayProps = new System.Windows.Forms.PropertyGrid();
            this.propertiesTabControl.SuspendLayout();
            this.nodeArcPropsTab.SuspendLayout();
            this.graphRulePropsTab.SuspendLayout();
            this.displayPropsTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertiesTabControl
            // 
            this.propertiesTabControl.Controls.Add(this.nodeArcPropsTab);
            this.propertiesTabControl.Controls.Add(this.graphRulePropsTab);
            this.propertiesTabControl.Controls.Add(this.displayPropsTab);
            this.propertiesTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertiesTabControl.Location = new System.Drawing.Point(0, 0);
            this.propertiesTabControl.Name = "propertiesTabControl";
            this.propertiesTabControl.SelectedIndex = 0;
            this.propertiesTabControl.Size = new System.Drawing.Size(299, 241);
            this.propertiesTabControl.TabIndex = 0;
            // 
            // nodeArcPropsTab
            // 
            this.nodeArcPropsTab.Controls.Add(this.nodeArcProps);
            this.nodeArcPropsTab.Location = new System.Drawing.Point(4, 22);
            this.nodeArcPropsTab.Name = "nodeArcPropsTab";
            this.nodeArcPropsTab.Size = new System.Drawing.Size(291, 215);
            this.nodeArcPropsTab.TabIndex = 2;
            this.nodeArcPropsTab.Text = "Node/Arc Properties";
            // 
            // nodeArcProps
            // 
            this.nodeArcProps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodeArcProps.Location = new System.Drawing.Point(0, 0);
            this.nodeArcProps.Name = "nodeArcProps";
            this.nodeArcProps.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.nodeArcProps.Size = new System.Drawing.Size(291, 215);
            this.nodeArcProps.TabIndex = 0;
            this.nodeArcProps.ToolbarVisible = false;
            this.nodeArcProps.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.Props_PropertyValueChanged);
            // 
            // graphRulePropsTab
            // 
            this.graphRulePropsTab.Controls.Add(this.graphRuleProps);
            this.graphRulePropsTab.Location = new System.Drawing.Point(4, 22);
            this.graphRulePropsTab.Name = "graphRulePropsTab";
            this.graphRulePropsTab.Padding = new System.Windows.Forms.Padding(3);
            this.graphRulePropsTab.Size = new System.Drawing.Size(291, 215);
            this.graphRulePropsTab.TabIndex = 1;
            this.graphRulePropsTab.Text = "Graph Properties";
            // 
            // graphRuleProps
            // 
            this.graphRuleProps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphRuleProps.Location = new System.Drawing.Point(3, 3);
            this.graphRuleProps.Name = "graphRuleProps";
            this.graphRuleProps.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.graphRuleProps.Size = new System.Drawing.Size(285, 209);
            this.graphRuleProps.TabIndex = 0;
            this.graphRuleProps.ToolbarVisible = false;
            this.graphRuleProps.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.Props_PropertyValueChanged);
            // 
            // displayPropsTab
            // 
            this.displayPropsTab.Controls.Add(this.displayProps);
            this.displayPropsTab.Location = new System.Drawing.Point(4, 22);
            this.displayPropsTab.Name = "displayPropsTab";
            this.displayPropsTab.Size = new System.Drawing.Size(291, 215);
            this.displayPropsTab.TabIndex = 3;
            this.displayPropsTab.Text = "Display Properties";
            // 
            // displayProps
            // 
            this.displayProps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.displayProps.Location = new System.Drawing.Point(0, 0);
            this.displayProps.Name = "displayProps";
            this.displayProps.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.displayProps.Size = new System.Drawing.Size(291, 215);
            this.displayProps.TabIndex = 0;
            this.displayProps.ToolbarVisible = false;
            this.displayProps.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.displayProps_PropertyValueChanged);
            // 
            // PropertiesWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 241);
            this.Controls.Add(this.propertiesTabControl);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PropertiesWindow";
            this.Text = "Properties Window";
            this.propertiesTabControl.ResumeLayout(false);
            this.nodeArcPropsTab.ResumeLayout(false);
            this.graphRulePropsTab.ResumeLayout(false);
            this.displayPropsTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl propertiesTabControl;
        private System.Windows.Forms.TabPage graphRulePropsTab;
        private System.Windows.Forms.TabPage nodeArcPropsTab;
        private System.Windows.Forms.TabPage displayPropsTab;
        private System.Windows.Forms.PropertyGrid graphRuleProps;
        private System.Windows.Forms.PropertyGrid nodeArcProps;
        private System.Windows.Forms.PropertyGrid displayProps;
    }
}