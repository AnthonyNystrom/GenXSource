using System;
using System.Windows.Forms;

namespace FPlot
{
	/// <summary>
	/// Summary description for ImageControl.
	/// </summary>
	public class ImageControl : System.Windows.Forms.Control
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.Drawing.Image image;

		public System.Drawing.Image Image {
			get { return image; }
			set {
				lock(this) {
					image = value;
				}
				this.Invalidate();
			}
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			lock(this) {
				if (image != null) {
					pe.Graphics.DrawImage(image, 0, 0);
				}
			}
		}
	}
}
