namespace Demo
{
	partial class Demo
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.primitivesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.boxesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.spheresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cylindersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.conesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.torsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ellipsoidsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.bandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.booleanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.intersectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.unionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.subToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.intersectionContoursToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.kinematicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rotationSurfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rotationBodyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			//this.extrudeSurfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			//this.extrudeBodyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.spiralSurfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.spiralBodyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pipeSurfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			//this.pipeBodyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.surfacesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.meshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.faceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			//this.coonsFrom3CurvesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.coonsFrom4CurvesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.linearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fromClipsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.bodyFromClipsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openGLControl1 = new DemoOpenGLControl();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.primitivesToolStripMenuItem,
            this.booleanToolStripMenuItem,
            this.kinematicToolStripMenuItem,
            this.surfacesToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(828, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// primitivesToolStripMenuItem
			// 
			this.primitivesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boxesToolStripMenuItem,
            this.spheresToolStripMenuItem,
            this.cylindersToolStripMenuItem,
            this.conesToolStripMenuItem,
            this.torsToolStripMenuItem,
            this.ellipsoidsToolStripMenuItem,
            this.bandsToolStripMenuItem});
			this.primitivesToolStripMenuItem.Name = "primitivesToolStripMenuItem";
			this.primitivesToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
			this.primitivesToolStripMenuItem.Text = "Primitives";
			// 
			// boxesToolStripMenuItem
			// 
			this.boxesToolStripMenuItem.Name = "boxesToolStripMenuItem";
			this.boxesToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.boxesToolStripMenuItem.Text = "Boxes";
			this.boxesToolStripMenuItem.Click += new System.EventHandler(this.boxesToolStripMenuItem_Click);
			// 
			// spheresToolStripMenuItem
			// 
			this.spheresToolStripMenuItem.Name = "spheresToolStripMenuItem";
			this.spheresToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.spheresToolStripMenuItem.Text = "Spheres";
			this.spheresToolStripMenuItem.Click += new System.EventHandler(this.spheresToolStripMenuItem_Click);
			// 
			// cylindersToolStripMenuItem
			// 
			this.cylindersToolStripMenuItem.Name = "cylindersToolStripMenuItem";
			this.cylindersToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.cylindersToolStripMenuItem.Text = "Cylinders";
			this.cylindersToolStripMenuItem.Click += new System.EventHandler(this.cylindersToolStripMenuItem_Click);
			// 
			// conesToolStripMenuItem
			// 
			this.conesToolStripMenuItem.Name = "conesToolStripMenuItem";
			this.conesToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.conesToolStripMenuItem.Text = "Cones";
			this.conesToolStripMenuItem.Click += new System.EventHandler(this.conesToolStripMenuItem_Click);
			// 
			// torsToolStripMenuItem
			// 
			this.torsToolStripMenuItem.Name = "torsToolStripMenuItem";
			this.torsToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.torsToolStripMenuItem.Text = "Tors";
			this.torsToolStripMenuItem.Click += new System.EventHandler(this.torsToolStripMenuItem_Click);
			// 
			// ellipsoidsToolStripMenuItem
			// 
			this.ellipsoidsToolStripMenuItem.Name = "ellipsoidsToolStripMenuItem";
			this.ellipsoidsToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.ellipsoidsToolStripMenuItem.Text = "Ellipsoids";
			this.ellipsoidsToolStripMenuItem.Click += new System.EventHandler(this.ellipsoidsToolStripMenuItem_Click);
			// 
			// bandsToolStripMenuItem
			// 
			this.bandsToolStripMenuItem.Name = "bandsToolStripMenuItem";
			this.bandsToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.bandsToolStripMenuItem.Text = "Bands";
			this.bandsToolStripMenuItem.Click += new System.EventHandler(this.bandsToolStripMenuItem_Click);
			// 
			// booleanToolStripMenuItem
			// 
			this.booleanToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.intersectionToolStripMenuItem,
            this.unionToolStripMenuItem,
            this.subToolStripMenuItem,
            this.intersectionContoursToolStripMenuItem,
            this.sectionsToolStripMenuItem});
			this.booleanToolStripMenuItem.Name = "booleanToolStripMenuItem";
			this.booleanToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
			this.booleanToolStripMenuItem.Text = "Boolean";
			// 
			// intersectionToolStripMenuItem
			// 
			this.intersectionToolStripMenuItem.Name = "intersectionToolStripMenuItem";
			this.intersectionToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.intersectionToolStripMenuItem.Text = "Intersection";
			this.intersectionToolStripMenuItem.Click += new System.EventHandler(this.intersectionToolStripMenuItem_Click);
			// 
			// unionToolStripMenuItem
			// 
			this.unionToolStripMenuItem.Name = "unionToolStripMenuItem";
			this.unionToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.unionToolStripMenuItem.Text = "Union";
			this.unionToolStripMenuItem.Click += new System.EventHandler(this.unionToolStripMenuItem_Click);
			// 
			// subToolStripMenuItem
			// 
			this.subToolStripMenuItem.Name = "subToolStripMenuItem";
			this.subToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.subToolStripMenuItem.Text = "Sub";
			this.subToolStripMenuItem.Click += new System.EventHandler(this.subToolStripMenuItem_Click);
			// 
			// intersectionContoursToolStripMenuItem
			// 
			this.intersectionContoursToolStripMenuItem.Name = "intersectionContoursToolStripMenuItem";
			this.intersectionContoursToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.intersectionContoursToolStripMenuItem.Text = "Intersection Contours";
			this.intersectionContoursToolStripMenuItem.Click += new System.EventHandler(this.intersectionContoursToolStripMenuItem_Click);
			// 
			// sectionsToolStripMenuItem
			// 
			this.sectionsToolStripMenuItem.Name = "sectionsToolStripMenuItem";
			this.sectionsToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.sectionsToolStripMenuItem.Text = "Sections";
			this.sectionsToolStripMenuItem.Click += new System.EventHandler(this.sectionsToolStripMenuItem_Click);
			// 
			// kinematicToolStripMenuItem
			// 
			this.kinematicToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rotationSurfaceToolStripMenuItem,
            this.rotationBodyToolStripMenuItem,
           // this.extrudeSurfaceToolStripMenuItem,
            //this.extrudeBodyToolStripMenuItem,
            this.spiralSurfaceToolStripMenuItem,
            this.spiralBodyToolStripMenuItem,
            this.pipeSurfaceToolStripMenuItem/*,
            this.pipeBodyToolStripMenuItem*/});
			this.kinematicToolStripMenuItem.Name = "kinematicToolStripMenuItem";
			this.kinematicToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
			this.kinematicToolStripMenuItem.Text = "Kinematic";
			// 
			// rotationSurfaceToolStripMenuItem
			// 
			this.rotationSurfaceToolStripMenuItem.Name = "rotationSurfaceToolStripMenuItem";
			this.rotationSurfaceToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.rotationSurfaceToolStripMenuItem.Text = "Rotation surface";
			this.rotationSurfaceToolStripMenuItem.Click += new System.EventHandler(this.rotationSurfaceToolStripMenuItem_Click);
			// 
			// rotationBodyToolStripMenuItem
			// 
			this.rotationBodyToolStripMenuItem.Name = "rotationBodyToolStripMenuItem";
			this.rotationBodyToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.rotationBodyToolStripMenuItem.Text = "Rotation body";
			this.rotationBodyToolStripMenuItem.Click += new System.EventHandler(this.rotationBodyToolStripMenuItem_Click);
			// 
			// extrudeSurfaceToolStripMenuItem
			// 
			//this.extrudeSurfaceToolStripMenuItem.Name = "extrudeSurfaceToolStripMenuItem";
			//this.extrudeSurfaceToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			//this.extrudeSurfaceToolStripMenuItem.Text = "Extrude surface";
			//this.extrudeSurfaceToolStripMenuItem.Click += new System.EventHandler(this.extrudeSurfaceToolStripMenuItem_Click);
			// 
			// extrudeBodyToolStripMenuItem
			// 
			//this.extrudeBodyToolStripMenuItem.Name = "extrudeBodyToolStripMenuItem";
			//this.extrudeBodyToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			//this.extrudeBodyToolStripMenuItem.Text = "Extrude body";
			//this.extrudeBodyToolStripMenuItem.Click += new System.EventHandler(this.extrudeBodyToolStripMenuItem_Click);
			// 
			// spiralSurfaceToolStripMenuItem
			// 
			this.spiralSurfaceToolStripMenuItem.Name = "spiralSurfaceToolStripMenuItem";
			this.spiralSurfaceToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.spiralSurfaceToolStripMenuItem.Text = "Spiral surface";
			this.spiralSurfaceToolStripMenuItem.Click += new System.EventHandler(this.spiralSurfaceToolStripMenuItem_Click);
			// 
			// spiralBodyToolStripMenuItem
			// 
			this.spiralBodyToolStripMenuItem.Name = "spiralBodyToolStripMenuItem";
			this.spiralBodyToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.spiralBodyToolStripMenuItem.Text = "Spiral body";
			this.spiralBodyToolStripMenuItem.Click += new System.EventHandler(this.spiralBodyToolStripMenuItem_Click);
			// 
			// pipeSurfaceToolStripMenuItem
			// 
			this.pipeSurfaceToolStripMenuItem.Name = "pipeSurfaceToolStripMenuItem";
			this.pipeSurfaceToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.pipeSurfaceToolStripMenuItem.Text = "Pipe surface";
			this.pipeSurfaceToolStripMenuItem.Click += new System.EventHandler(this.pipeSurfaceToolStripMenuItem_Click);
			// 
			// pipeBodyToolStripMenuItem
			// 
			//this.pipeBodyToolStripMenuItem.Name = "pipeBodyToolStripMenuItem";
			//this.pipeBodyToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			//this.pipeBodyToolStripMenuItem.Text = "Pipe body";
			//this.pipeBodyToolStripMenuItem.Click += new System.EventHandler(this.pipeBodyToolStripMenuItem_Click);
			// 
			// surfacesToolStripMenuItem
			// 
			this.surfacesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.meshToolStripMenuItem,
            this.faceToolStripMenuItem,
            //this.coonsFrom3CurvesToolStripMenuItem,
            this.coonsFrom4CurvesToolStripMenuItem,
            this.linearToolStripMenuItem,
            this.fromClipsToolStripMenuItem,
            this.bodyFromClipsToolStripMenuItem});
			this.surfacesToolStripMenuItem.Name = "surfacesToolStripMenuItem";
			this.surfacesToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.surfacesToolStripMenuItem.Text = "Surfaces";
			// 
			// meshToolStripMenuItem
			// 
			this.meshToolStripMenuItem.Name = "meshToolStripMenuItem";
			this.meshToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.meshToolStripMenuItem.Text = "Mesh";
			this.meshToolStripMenuItem.Click += new System.EventHandler(this.meshToolStripMenuItem_Click);
			// 
			// faceToolStripMenuItem
			// 
			this.faceToolStripMenuItem.Name = "faceToolStripMenuItem";
			this.faceToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.faceToolStripMenuItem.Text = "Face";
			this.faceToolStripMenuItem.Click += new System.EventHandler(this.faceToolStripMenuItem_Click);
			// 
			// coonsFrom3CurvesToolStripMenuItem
			// 
			//this.coonsFrom3CurvesToolStripMenuItem.Name = "coonsFrom3CurvesToolStripMenuItem";
			//this.coonsFrom3CurvesToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			//this.coonsFrom3CurvesToolStripMenuItem.Text = "Coons from 3 curves";
			//this.coonsFrom3CurvesToolStripMenuItem.Click += new System.EventHandler(this.coonsFrom3CurvesToolStripMenuItem_Click);
			// 
			// coonsFrom4CurvesToolStripMenuItem
			// 
			this.coonsFrom4CurvesToolStripMenuItem.Name = "coonsFrom4CurvesToolStripMenuItem";
			this.coonsFrom4CurvesToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.coonsFrom4CurvesToolStripMenuItem.Text = "Coons from 4 curves";
			this.coonsFrom4CurvesToolStripMenuItem.Click += new System.EventHandler(this.coonsFrom4CurvesToolStripMenuItem_Click);
			// 
			// linearToolStripMenuItem
			// 
			this.linearToolStripMenuItem.Name = "linearToolStripMenuItem";
			this.linearToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.linearToolStripMenuItem.Text = "Linear";
			this.linearToolStripMenuItem.Click += new System.EventHandler(this.linearToolStripMenuItem_Click);
			// 
			// fromClipsToolStripMenuItem
			// 
			this.fromClipsToolStripMenuItem.Name = "fromClipsToolStripMenuItem";
			this.fromClipsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.fromClipsToolStripMenuItem.Text = "From clips";
			this.fromClipsToolStripMenuItem.Click += new System.EventHandler(this.fromClipsToolStripMenuItem_Click);
			// 
			// bodyFromClipsToolStripMenuItem
			// 
			this.bodyFromClipsToolStripMenuItem.Name = "bodyFromClipsToolStripMenuItem";
			this.bodyFromClipsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.bodyFromClipsToolStripMenuItem.Text = "Body from clips";
			this.bodyFromClipsToolStripMenuItem.Click += new System.EventHandler(this.bodyFromClipsToolStripMenuItem_Click);
			// 
			// openGLControl2
			// 
			this.openGLControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.openGLControl1.Location = new System.Drawing.Point(0, 24);
			this.openGLControl1.Name = "openGLControl2";
			this.openGLControl1.Size = new System.Drawing.Size(828, 630);
			this.openGLControl1.TabIndex = 2;
			// 
			// Demo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(828, 654);
			this.Controls.Add(this.openGLControl1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Demo";
			this.Text = "Form1";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem primitivesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem boxesToolStripMenuItem;
		private DemoOpenGLControl openGLControl1;
		private System.Windows.Forms.ToolStripMenuItem spheresToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cylindersToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem conesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem torsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ellipsoidsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem bandsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem booleanToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem intersectionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem unionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem subToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem intersectionContoursToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sectionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem kinematicToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rotationSurfaceToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rotationBodyToolStripMenuItem;
		//private System.Windows.Forms.ToolStripMenuItem extrudeSurfaceToolStripMenuItem;
		//private System.Windows.Forms.ToolStripMenuItem extrudeBodyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem spiralSurfaceToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem spiralBodyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pipeSurfaceToolStripMenuItem;
		//private System.Windows.Forms.ToolStripMenuItem pipeBodyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem surfacesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem meshToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem faceToolStripMenuItem;
		//private System.Windows.Forms.ToolStripMenuItem coonsFrom3CurvesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem coonsFrom4CurvesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem linearToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fromClipsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem bodyFromClipsToolStripMenuItem;
	}
}

