using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Genetibase.NuGenAnnotation
{
	/// <summary>
	/// Image tool
	/// </summary>
	class ToolImage : Genetibase.NuGenAnnotation.ToolObject
	{

		public ToolImage()
		{
            System.IO.MemoryStream ms = new System.IO.MemoryStream(Genetibase.NuGenAnnotation.Properties.Resources.Rectangle);
            Cursor = new Cursor(ms);
            ms.Close();

			//Cursor = new Cursor(GetType(), "Rectangle.cur");

        }

		public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
		{
			Point p = drawArea.BackTrackMouse(new Point(e.X, e.Y));
			AddNewObject(drawArea, new DrawImage(p.X, p.Y));
		}

		public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
		{
			drawArea.Cursor = Cursor;

			if (e.Button == MouseButtons.Left)
			{
				Point point = drawArea.BackTrackMouse(new Point(e.X, e.Y));
				int al = drawArea.TheLayers.ActiveLayerIndex;
				drawArea.TheLayers[al].Graphics[0].MoveHandleTo(point, 5);
				drawArea.Refresh();
			}
		}
		public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
		{
			Point p = drawArea.BackTrackMouse(new Point(e.X, e.Y));
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Select an Image to insert into map";
			ofd.Filter = "Bitmap (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|Fireworks (*.png)|*.png|GIF (*.gif)|*.gif|Icon (*.ico)|*.ico|All files|*.*";
			ofd.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
			int al = drawArea.TheLayers.ActiveLayerIndex;
			if (ofd.ShowDialog() == DialogResult.OK)
				((DrawImage)drawArea.TheLayers[al].Graphics[0]).TheImage = (Bitmap)Bitmap.FromFile(ofd.FileName);
			ofd.Dispose();
			base.OnMouseUp(drawArea, e);
		}
	}
}
