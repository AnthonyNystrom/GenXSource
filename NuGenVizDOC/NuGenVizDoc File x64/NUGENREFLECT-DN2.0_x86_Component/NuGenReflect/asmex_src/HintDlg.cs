/***
 * 
 *  ASMEX by RiskCare Ltd.
 * 
 * This source is copyright (C) 2002 RiskCare Ltd. All rights reserved.
 * 
 * Disclaimer:
 * This code is provided 'as is', with absolutely no warranty expressed or
 * implied.  Any use of this code is at your own risk.
 *   
 * You are hereby granted the right to redistribute this source unmodified
 * in its original archive. 
 * You are hereby granted the right to use this code, or code based on it,
 * provided that you acknowledge RiskCare Ltd somewhere in the documentation
 * of your application. 
 * You are hereby granted the right to distribute changes to this source, 
 * provided that:
 * 
 * 1 -- This copyright notice is retained unchanged 
 * 2 -- Your changes are clearly marked 
 * 
 * Enjoy!
 * 
 * --------------------------------------------------------------------
 * 
 * If you use this code or have comments on it, please mail me at 
 * support@jbrowse.com or ben.peterson@riskcare.com
 * 
 */

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;

namespace Asmex
{
	/// <summary>
	/// Summary description for HintDlg.
	/// </summary>
	public class HintDlg : System.Windows.Forms.Form
	{
		int t;
		string[] hints;
		Bitmap bmp, bg;

		private System.Timers.Timer timer1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public HintDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			hints = new string[41];

			hints[0] = "Right click on resources";
			hints[1] = "to save them to a file.";
			hints[2] = "..oOo..";
			hints[3] = "Right click on assemblies";
			hints[4] = "to execute them.";
			hints[5] = "..oOo..";
			hints[6] = "Drag files from explorer";
			hints[7] = "to open them.";
			hints[8] = "..oOo..";
			hints[9] = "Right click on an Assembly";
			hints[10] = "Reference to try to open the";
			hints[11] = "assembly.  Asmex looks in the";
			hints[12] = "path you specify to resolve";
			hints[13] = "the Assembly Reference.";
			hints[14] = "..oOo..";
			hints[15] = "You can right click on types";
			hints[16] = "or type members to run";
			hints[17] = "ILDASM and view a";
			hints[18] = "disassembly.";
			hints[19] = "..oOo..";
			hints[20] = "If you drag a file to a view";
			hints[21] = "it will appear in that view";
			hints[22] = "as a new root node.  If you";
			hints[23] = "drag to the background, a new";
			hints[24] = "view will be created for your";
			hints[25] = "file.";
			hints[26] = "..oOo..";
			hints[27] = "Asmex needs Admin rights in";
			hints[28] = "order to enumerate the GAC,";
			hints[29] = "so if you aren't admin";
			hints[30] = "'Open Assembly' might";
			hints[31] = "not work.";
			hints[32] = "..oOo..";
			hints[33] = "This hint box is the sort of.";
			hints[34] = "simple Drawing2D trick";
			hints[35] = "that doesn't impress ";
			hints[36] = "anyone at all.";
			hints[37] = "..oOo..";
			hints[38] = "Thank you for using Asmex.";
			hints[39] = "Please send feedback!";
			hints[40] = "..oOo..";

			bmp = new Bitmap(1,1);
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
			this.timer1 = new System.Timers.Timer();
			((System.ComponentModel.ISupportInitialize)(this.timer1)).BeginInit();
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 60;
			this.timer1.SynchronizingObject = this;
			this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
			// 
			// HintDlg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(600, 307);
			this.Name = "HintDlg";
			this.Text = "HintDlg";
			this.Load += new System.EventHandler(this.HintDlg_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintHandler);
			((System.ComponentModel.ISupportInitialize)(this.timer1)).EndInit();

		}
		#endregion

		private void PaintHandler(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			

			GraphicsPath p = new GraphicsPath();

			if (bmp.Width != Width || bmp.Height != Height)
			{
				bmp = new Bitmap(Width, Height, e.Graphics);
			}

			Graphics bb = Graphics.FromImage(bmp);

			//bb.FillRectangle(new SolidBrush(Color.Black), e.ClipRectangle);
			bb.DrawImage(bg, 0,0,Width,Height);



			// Create a path and add a rectangle.
			GraphicsPath myPath = new GraphicsPath();
			RectangleF srcRect = new RectangleF(0, 0, 300, 900);
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			//myPath.AddRectangle(srcRect);

			for(int i=0;i < hints.Length; ++i)
			{
				RectangleF txtRect = new RectangleF();
				txtRect.X = 20;
				txtRect.Width = 260;
				txtRect.Y = 900-((230 + t + (hints.Length-i)*18) % 900);
				txtRect.Height = 20;

				if (txtRect.Bottom < 900)
					myPath.AddString(hints[i], FontFamily.GenericSansSerif, (int)FontStyle.Regular,
					18, txtRect, sf);

			}

			int middle = Width/2;

			PointF point1 = new PointF(middle-20, 10);
			PointF point2 = new PointF(middle+20, 10);
			PointF point3 = new PointF(-800, Height+680);
			PointF point4 = new PointF(Width+800, Height+680);

			PointF[] destPoints = {point1, point2, point3, point4};
			// Create a translation matrix.
			Matrix translateMatrix = new Matrix();
			translateMatrix.Translate(100, 0);
			// Warp the source path (rectangle).
			myPath.Warp(destPoints, srcRect);
			// Draw the warped path (rectangle) to the screen.
			bb.FillPath(new LinearGradientBrush(new PointF(0,0), new PointF(0,Height), Color.DarkRed, Color.Yellow), myPath);

			e.Graphics.DrawImageUnscaled(bmp, 0,0,Width,Height);


		}

		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
		{

		}

		private void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
		{
			t++;
			Invalidate();
		}

		private void HintDlg_Load(object sender, System.EventArgs e)
		{
			Stream imageStream = GetType().Assembly.GetManifestResourceStream("Asmex.stars.bmp");

			bg = new Bitmap(imageStream, true);
		}
	}
}
