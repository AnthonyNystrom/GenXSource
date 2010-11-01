using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Genetibase.NuGenAnnotation
{
	/// <summary>
	/// Ellipse tool
	/// </summary>
	class ToolEllipse : Genetibase.NuGenAnnotation.ToolRectangle
	{
		public ToolEllipse()
		{
            System.IO.MemoryStream ms = new System.IO.MemoryStream(Genetibase.NuGenAnnotation.Properties.Resources.Ellipse);
            Cursor = new Cursor(ms);
            ms.Close();
			//Cursor = new Cursor(GetType(), "Ellipse.cur");
		}

		public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
		{
			Point p = drawArea.BackTrackMouse(new Point(e.X, e.Y));
			if (drawArea.CurrentPen == null)
				AddNewObject(drawArea, new DrawEllipse(p.X, p.Y, 1, 1, drawArea.LineColor, drawArea.FillColor, drawArea.DrawFilled, drawArea.LineWidth));
			else
				AddNewObject(drawArea, new DrawEllipse(p.X, p.Y, 1, 1, drawArea.PenType, drawArea.FillColor, drawArea.DrawFilled));
		}
	}
}
