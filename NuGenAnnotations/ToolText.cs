using System;
using System.Windows.Forms;
using System.Drawing;


namespace Genetibase.NuGenAnnotation
{
	/// <summary>
	/// Rectangle tool
	/// </summary>
	class ToolText : Genetibase.NuGenAnnotation.ToolObject
	{

		public ToolText()
		{
            System.IO.MemoryStream ms = new System.IO.MemoryStream(Genetibase.NuGenAnnotation.Properties.Resources.Rectangle);
            Cursor = new Cursor(ms);
            ms.Close();
			//Cursor = new Cursor(GetType(), "Rectangle.cur");
		}

		public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
		{
			Point p = drawArea.BackTrackMouse(new Point(e.X, e.Y));
			TextDialog td = new TextDialog();
			if (td.ShowDialog() == DialogResult.OK)
			{
				string t = td.TheText;
				Color c = td.TheColor;
				Font f = td.TheFont;
				AddNewObject(drawArea, new DrawText(p.X, p.Y, t, f, c));
			}
		}

		public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
		{
			drawArea.Cursor = Cursor;
			int al = drawArea.TheLayers.ActiveLayerIndex;
			if (e.Button == MouseButtons.Left)
			{
				Point point = drawArea.BackTrackMouse(new Point(e.X, e.Y));
				drawArea.TheLayers[al].Graphics[0].MoveHandleTo(point, 5);
				drawArea.Refresh();
			}
		}
	}
}
