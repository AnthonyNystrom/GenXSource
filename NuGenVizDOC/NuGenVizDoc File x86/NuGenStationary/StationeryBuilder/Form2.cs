using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace StationeryBuilder
{
	/// <summary>
	/// Summary description for Form2.
	/// </summary>
	public class Form2 : System.Windows.Forms.Form
	{
		private Agilix.Ink.Doodle doodle1;
		private Agilix.Ink.Note.Note note1;
		private Agilix.Ink.Note.NoteBox noteBox1;
		private Agilix.Ink.Note.NoteToolbar noteToolbar1;
		private Agilix.Ink.Scribble.Scribble scribble1;
		private Agilix.Ink.Scribble.ScribbleBox scribbleBox1;
		private Agilix.Ink.Scribble.ScribbleContainer scribbleContainer1;
		private Agilix.Ink.Scribble.ScribbleToolbar scribbleToolbar1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form2()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.doodle1 = new Agilix.Ink.Doodle();
			this.note1 = new Agilix.Ink.Note.Note();
			this.noteBox1 = new Agilix.Ink.Note.NoteBox();
			this.noteToolbar1 = new Agilix.Ink.Note.NoteToolbar();
			this.scribble1 = new Agilix.Ink.Scribble.Scribble();
			this.scribbleBox1 = new Agilix.Ink.Scribble.ScribbleBox();
			this.scribbleContainer1 = new Agilix.Ink.Scribble.ScribbleContainer();
			this.scribbleToolbar1 = new Agilix.Ink.Scribble.ScribbleToolbar();
			this.SuspendLayout();
			// 
			// doodle1
			// 
			this.doodle1.AllowDrop = true;
			this.doodle1.BackColor = System.Drawing.Color.White;
			this.doodle1.Location = new System.Drawing.Point(0, 0);
			this.doodle1.Name = "doodle1";
			this.doodle1.Size = new System.Drawing.Size(368, 344);
			this.doodle1.TabIndex = 0;
			this.doodle1.Text = "doodle1";
			// 
			// note1
			// 
			this.note1.AllowDrop = true;
			this.note1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.note1.BackColor = System.Drawing.Color.White;
			this.note1.HighlightColor = System.Drawing.Color.Gold;
			this.note1.Location = new System.Drawing.Point(4, 4);
			this.note1.Name = "note1";
			this.note1.Size = new System.Drawing.Size(368, 344);
			this.note1.TabIndex = 1;
			this.note1.Text = "note1";
			// 
			// noteBox1
			// 
			this.noteBox1.BackColor = System.Drawing.Color.White;
			this.noteBox1.HighlightColor = System.Drawing.Color.Gold;
			this.noteBox1.Location = new System.Drawing.Point(8, 8);
			this.noteBox1.Name = "noteBox1";
			this.noteBox1.Size = new System.Drawing.Size(496, 448);
			this.noteBox1.TabIndex = 2;
			this.noteBox1.Text = "noteBox1";
			// 
			// noteToolbar1
			// 
			this.noteToolbar1.Dock = System.Windows.Forms.DockStyle.Top;
			this.noteToolbar1.Location = new System.Drawing.Point(0, 0);
			this.noteToolbar1.Name = "noteToolbar1";
			this.noteToolbar1.Size = new System.Drawing.Size(544, 55);
			this.noteToolbar1.TabIndex = 3;
			// 
			// scribble1
			// 
			this.scribble1.AllowDrop = true;
			this.scribble1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.scribble1.BackColor = System.Drawing.Color.White;
			this.scribble1.Location = new System.Drawing.Point(4, 4);
			this.scribble1.Name = "scribble1";
			this.scribble1.Size = new System.Drawing.Size(368, 344);
			this.scribble1.TabIndex = 4;
			this.scribble1.Text = "scribble1";
			// 
			// scribbleBox1
			// 
			this.scribbleBox1.BackColor = System.Drawing.SystemColors.Control;
			this.scribbleBox1.HighlightElementColor = System.Drawing.Color.FromArgb(((System.Byte)(189)), ((System.Byte)(208)), ((System.Byte)(220)));
			this.scribbleBox1.Location = new System.Drawing.Point(8, 8);
			this.scribbleBox1.Name = "scribbleBox1";
			this.scribbleBox1.Size = new System.Drawing.Size(472, 464);
			this.scribbleBox1.TabColor = System.Drawing.SystemColors.ControlDark;
			this.scribbleBox1.TabIndex = 5;
			this.scribbleBox1.Text = "scribbleBox1";
			// 
			// scribbleContainer1
			// 
			this.scribbleContainer1.ActionButtonColor = System.Drawing.SystemColors.Control;
			this.scribbleContainer1.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.scribbleContainer1.HighlightElementColor = System.Drawing.Color.FromArgb(((System.Byte)(189)), ((System.Byte)(208)), ((System.Byte)(220)));
			this.scribbleContainer1.Location = new System.Drawing.Point(12, 12);
			this.scribbleContainer1.Name = "scribbleContainer1";
			this.scribbleContainer1.Size = new System.Drawing.Size(472, 464);
			this.scribbleContainer1.TabColor = System.Drawing.SystemColors.ControlDark;
			this.scribbleContainer1.TabIndex = 6;
			this.scribbleContainer1.Text = "scribbleContainer1";
			// 
			// scribbleToolbar1
			// 
			this.scribbleToolbar1.Dock = System.Windows.Forms.DockStyle.Top;
			this.scribbleToolbar1.Location = new System.Drawing.Point(0, 55);
			this.scribbleToolbar1.Name = "scribbleToolbar1";
			this.scribbleToolbar1.Size = new System.Drawing.Size(544, 83);
			this.scribbleToolbar1.TabIndex = 7;
			// 
			// Form2
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(544, 502);
			this.Controls.Add(this.scribbleToolbar1);
			this.Controls.Add(this.scribbleContainer1);
			this.Controls.Add(this.scribbleBox1);
			this.Controls.Add(this.scribble1);
			this.Controls.Add(this.noteToolbar1);
			this.Controls.Add(this.noteBox1);
			this.Controls.Add(this.note1);
			this.Controls.Add(this.doodle1);
			this.Name = "Form2";
			this.Text = "Form2";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
