namespace TestApplication
{
    partial class Form1
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
            this.nuGenPSurface1 = new Genetibase.MathX.NuGenPSurface();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // nuGenPSurface1
            // 
            this.nuGenPSurface1.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.nuGenPSurface1.BackSurfaceColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(170)))), ((int)(((byte)(0)))));
            this.nuGenPSurface1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nuGenPSurface1.DrawBorder = false;
            this.nuGenPSurface1.Expression_X = "(2 + cos(v/2)* sin(u) - sin(v/2)* sin(2 *u))* cos(v)";
            this.nuGenPSurface1.Expression_Y = "(2 + cos(v/2)* sin(u) - sin(v/2)* sin(2 *u))* sin(v)";
            this.nuGenPSurface1.Expression_Z = "sin(v/2)* sin(u) + cos(v/2) *sin(2* u)";
            this.nuGenPSurface1.FrontSurfaceColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(210)))), ((int)(((byte)(0)))));
            this.nuGenPSurface1.GridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(4)))));
            this.nuGenPSurface1.GridSpacing = ((uint)(50u));
            this.nuGenPSurface1.Location = new System.Drawing.Point(0, 0);
            this.nuGenPSurface1.Margin = new System.Windows.Forms.Padding(4);
            this.nuGenPSurface1.Name = "nuGenPSurface1";
            this.nuGenPSurface1.OptimizeLevel = 5;
            this.nuGenPSurface1.PolygonSelection = true;
            this.nuGenPSurface1.SelectionOutlineColor = System.Drawing.Color.Red;
            this.nuGenPSurface1.SelectionTextColor = System.Drawing.Color.Black;
            this.nuGenPSurface1.Shape = Genetibase.MathX.NuGenPSurface.Shapes.Klein_2;
            this.nuGenPSurface1.Size = new System.Drawing.Size(516, 709);
            this.nuGenPSurface1.TabIndex = 0;
            this.nuGenPSurface1.uMax = "2*pi";
            this.nuGenPSurface1.uMin = "0";
            this.nuGenPSurface1.UseAntialiasing = true;
            this.nuGenPSurface1.vMax = "2*pi";
            this.nuGenPSurface1.vMin = "0";
            this.nuGenPSurface1.DoubleClick += new System.EventHandler(this.nuGenPSurface1_DoubleClick);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGrid1.Location = new System.Drawing.Point(516, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.SelectedObject = this.nuGenPSurface1;
            this.propertyGrid1.Size = new System.Drawing.Size(227, 709);
            this.propertyGrid1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Screenshot";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(81, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(99, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Export XAML";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 709);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.nuGenPSurface1);
            this.Controls.Add(this.propertyGrid1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Genetibase.MathX.NuGenPSurface nuGenPSurface1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;






    }
}

