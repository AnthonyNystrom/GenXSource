using System;
using System.Windows.Forms;
using System.Drawing;

namespace Genetibase.NuGenMediImage.UI.Controls
{    
	/// <summary>
	/// Summary description for picBoxMain.
	/// </summary>
	public class CustomPictureBox:PictureBox
	{     

		private float currentBrightness = 0;
		private float currentContrast = 1;
		private string currentfileName = string.Empty;
		private Color currentOverlayColor = Color.White;
		private bool overlaying = false;

        public void SetImage(Bitmap img)
        {
            this.Image = img;
        }
		
		protected override void OnPaint(PaintEventArgs e)
		{
            try
            {
                base.OnPaint(e);

                if (overlaying)
                    OverLayHeader(currentOverlayColor, currentfileName, currentBrightness, currentContrast, e.Graphics);
            }
            catch { }
		}

		public void ClearOverLay()
		{
			overlaying = false;
			this.Refresh();
		}

		public void SetOverLayHeaderParam( Color color, string fileName, float brightness, float contrast )
		{
			currentOverlayColor = color;
			currentfileName = fileName;
			currentBrightness = brightness;
			currentContrast = contrast;
		}

		public void OverLayHeader( Color color, string fileName, float brightness, float contrast )
		{
			this.OverLayHeader( color, fileName, brightness, contrast, this.CreateGraphics());
		}

		private void InitializeComponent()
		{

		}

		public void OverLayHeader( Color color, string fileName, float brightness, float contrast, Graphics g )
		{
			overlaying = true;

			currentOverlayColor = color;
			currentfileName = fileName;

			string sB = "B: " +  Math.Ceiling(brightness * 100);
			string sC = "C: " + Math.Floor(contrast * 100);
			
			Brush b = new SolidBrush( color );			
			g.DrawString( currentfileName, new Font(FontFamily.GenericMonospace.Name,12),b, 0,0);
			g.DrawString( sB, new Font(FontFamily.GenericMonospace.Name,12),b,0,15);
			g.DrawString( sC, new Font(FontFamily.GenericMonospace.Name,12),b,0,30);

			b.Dispose();
		}

		public new Image Image
		{
			get
			{
				return base.Image;
			}
			set
			{
				base.Image = value;               

				if( overlaying )
					OverLayHeader(currentOverlayColor, currentfileName, currentBrightness, currentContrast);
			}
		}


	}
}
