using System;
using System.Windows.Forms;
using System.Drawing;

namespace Genetibase.NuGenAnnotation
{
	/// <summary>
	/// Scribble tool
	/// </summary>
	class ToolPolygon : Genetibase.NuGenAnnotation.ToolObject
	{
		public ToolPolygon()
		{
            System.IO.MemoryStream ms = new System.IO.MemoryStream(Genetibase.NuGenAnnotation.Properties.Resources.Pencil);
            Cursor = new Cursor(ms);
            ms.Close();
			//Cursor = new Cursor(GetType(), "Pencil.cur");
		}

		private int lastX;
		private int lastY;
		private DrawPolygon newPolygon;
		private int minDistance = 15 * 15;

		/// <summary>
		/// Left nouse button is pressed
		/// </summary>
		/// <param name="drawArea"></param>
		/// <param name="e"></param>
		public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
		{
			// Create new polygon, add it to the list
			// and keep reference to it
			Point p = drawArea.BackTrackMouse(new Point(e.X, e.Y));
			if (drawArea.PenType == DrawingPens.PenType.Generic)
				newPolygon = new DrawPolygon(p.X, p.Y, p.X + 1, p.Y + 1, drawArea.LineColor, drawArea.LineWidth);
			else
				newPolygon = new DrawPolygon(p.X, p.Y, p.X + 1, p.Y + 1, drawArea.PenType);
			// Set the minimum distance variable according to current zoom level.
			minDistance = Convert.ToInt32((15 * drawArea.Zoom) * (15 * drawArea.Zoom));

			AddNewObject(drawArea, newPolygon);
			lastX = e.X;
			lastY = e.Y;
		}

		/// <summary>
		/// Mouse move - resize new polygon
		/// </summary>
		/// <param name="drawArea"></param>
		/// <param name="e"></param>
		public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
		{
			drawArea.Cursor = Cursor;

			if (e.Button != MouseButtons.Left)
				return;

			if (newPolygon == null)
				return;                 // precaution

			Point point = drawArea.BackTrackMouse(new Point(e.X, e.Y));
			int distance = (e.X - lastX) * (e.X - lastX) + (e.Y - lastY) * (e.Y - lastY);

			if (distance < minDistance)
			{
				// Distance between last two points is less than minimum -
				// move last point
				newPolygon.MoveHandleTo(point, newPolygon.HandleCount);
			} else
			{
				// Add new point
				newPolygon.AddPoint(point);
				lastX = e.X;
				lastY = e.Y;
			}
			drawArea.Refresh();
		}

		public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
		{
			newPolygon = null;
			base.OnMouseUp(drawArea, e);
		}
	}
}
