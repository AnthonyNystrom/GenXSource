using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;


namespace Genetibase.NuGenAnnotation
{
	/// <summary>
	/// Rectangle tool
	/// </summary>
	class ToolRectangle : Genetibase.NuGenAnnotation.ToolObject
	{

		public ToolRectangle()
		{
            MemoryStream ms = new MemoryStream(Genetibase.NuGenAnnotation.Properties.Resources.Rectangle);
            Cursor = new Cursor(ms);
            ms.Close();
		}

		public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
		{
			Point p = drawArea.BackTrackMouse(new Point(e.X, e.Y));
			if (drawArea.CurrentPen == null)
				AddNewObject(drawArea, new DrawRectangle(p.X, p.Y, 1, 1, drawArea.LineColor, drawArea.FillColor, drawArea.DrawFilled, drawArea.LineWidth));
			else
				AddNewObject(drawArea, new DrawRectangle(p.X, p.Y, 1, 1, drawArea.PenType, drawArea.FillColor, drawArea.DrawFilled));
		}

		public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
		{
            try
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
            catch { }
		}
	}
}
