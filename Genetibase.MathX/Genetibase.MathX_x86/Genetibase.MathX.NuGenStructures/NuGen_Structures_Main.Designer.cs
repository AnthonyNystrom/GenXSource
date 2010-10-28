namespace Genetibase.MathX.NuGenStructures
{
    partial class NuGen_Structures_Main
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
            this.button_ok = new System.Windows.Forms.Button();
            this.option_box = new System.Windows.Forms.RadioButton();
            this.option_Pnt = new System.Windows.Forms.RadioButton();
            this.option_ray = new System.Windows.Forms.RadioButton();
            this.option_rgb = new System.Windows.Forms.RadioButton();
            this.option_vectors = new System.Windows.Forms.RadioButton();
            this.option_trafo = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // button_ok
            // 
            this.button_ok.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.button_ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ok.ForeColor = System.Drawing.SystemColors.Window;
            this.button_ok.Location = new System.Drawing.Point(180, 179);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(76, 37);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = false;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // option_box
            // 
            this.option_box.AutoSize = true;
            this.option_box.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.option_box.ForeColor = System.Drawing.SystemColors.WindowText;
            this.option_box.Location = new System.Drawing.Point(52, 68);
            this.option_box.Name = "option_box";
            this.option_box.Size = new System.Drawing.Size(53, 19);
            this.option_box.TabIndex = 2;
            this.option_box.TabStop = true;
            this.option_box.Text = "BOX";
            this.option_box.UseVisualStyleBackColor = true;
            this.option_box.CheckedChanged += new System.EventHandler(this.option_box_CheckedChanged);
            // 
            // option_Pnt
            // 
            this.option_Pnt.AutoSize = true;
            this.option_Pnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.option_Pnt.ForeColor = System.Drawing.SystemColors.WindowText;
            this.option_Pnt.Location = new System.Drawing.Point(52, 108);
            this.option_Pnt.Name = "option_Pnt";
            this.option_Pnt.Size = new System.Drawing.Size(52, 19);
            this.option_Pnt.TabIndex = 3;
            this.option_Pnt.TabStop = true;
            this.option_Pnt.Text = "PNT";
            this.option_Pnt.UseVisualStyleBackColor = true;
            // 
            // option_ray
            // 
            this.option_ray.AutoSize = true;
            this.option_ray.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.option_ray.ForeColor = System.Drawing.SystemColors.WindowText;
            this.option_ray.Location = new System.Drawing.Point(52, 150);
            this.option_ray.Name = "option_ray";
            this.option_ray.Size = new System.Drawing.Size(60, 19);
            this.option_ray.TabIndex = 4;
            this.option_ray.TabStop = true;
            this.option_ray.Text = "RAYS";
            this.option_ray.UseVisualStyleBackColor = true;
            this.option_ray.CheckedChanged += new System.EventHandler(this.option_ray_CheckedChanged);
            // 
            // option_rgb
            // 
            this.option_rgb.AutoSize = true;
            this.option_rgb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.option_rgb.ForeColor = System.Drawing.SystemColors.WindowText;
            this.option_rgb.Location = new System.Drawing.Point(52, 197);
            this.option_rgb.Name = "option_rgb";
            this.option_rgb.Size = new System.Drawing.Size(62, 19);
            this.option_rgb.TabIndex = 5;
            this.option_rgb.TabStop = true;
            this.option_rgb.Text = "RGBA";
            this.option_rgb.UseVisualStyleBackColor = true;
            // 
            // option_vectors
            // 
            this.option_vectors.AutoSize = true;
            this.option_vectors.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.option_vectors.ForeColor = System.Drawing.SystemColors.WindowText;
            this.option_vectors.Location = new System.Drawing.Point(180, 68);
            this.option_vectors.Name = "option_vectors";
            this.option_vectors.Size = new System.Drawing.Size(88, 19);
            this.option_vectors.TabIndex = 19;
            this.option_vectors.TabStop = true;
            this.option_vectors.Text = "VECTORS";
            this.option_vectors.UseVisualStyleBackColor = true;
            // 
            // option_trafo
            // 
            this.option_trafo.AutoSize = true;
            this.option_trafo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.option_trafo.ForeColor = System.Drawing.SystemColors.WindowText;
            this.option_trafo.Location = new System.Drawing.Point(180, 108);
            this.option_trafo.Name = "option_trafo";
            this.option_trafo.Size = new System.Drawing.Size(69, 19);
            this.option_trafo.TabIndex = 20;
            this.option_trafo.TabStop = true;
            this.option_trafo.Text = "TRAFO";
            this.option_trafo.UseVisualStyleBackColor = true;
            // 
            // NuGen_Structures_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(305, 254);
            this.Controls.Add(this.option_trafo);
            this.Controls.Add(this.option_vectors);
            this.Controls.Add(this.option_rgb);
            this.Controls.Add(this.option_ray);
            this.Controls.Add(this.option_Pnt);
            this.Controls.Add(this.option_box);
            this.Controls.Add(this.button_ok);
            this.Location = new System.Drawing.Point(30, 30);
            this.Name = "NuGen_Structures_Main";
            this.Text = "NuGen Structures Main Menu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.RadioButton option_box;
        private System.Windows.Forms.RadioButton option_Pnt;
        private System.Windows.Forms.RadioButton option_ray;
        private System.Windows.Forms.RadioButton option_rgb;
        private System.Windows.Forms.RadioButton option_vectors;
        private System.Windows.Forms.RadioButton option_trafo;
    }
}