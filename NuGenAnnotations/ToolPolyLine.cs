using System;
using System.Windows.Forms;
using System.Drawing;

namespace Genetibase.NuGenAnnotation
{
	/// <summary>
	/// PolyLine tool (a PolyLine is a series of connected straight lines where each line is drawn individually)
	/// </summary>
	class ToolPolyLine : Genetibase.NuGenAnnotation.ToolObject
	{
		public ToolPolyLine()
		{
            System.IO.MemoryStream ms = new System.IO.MemoryStream(Genetibase.NuGenAnnotation.Properties.Resources.Pencil);
            Cursor = new Cursor(ms);
            ms.Close();
			//Cursor = new Cursor(GetType(), "Pencil.cur");
		}

		private int lastX;
		private int lastY;
		private DrawPolyLine newPolyLine;
		private bool _drawingInProcess = false;		// Set to true when drawing
		/// <summary>
		/// Left nouse button is pressed
		/// </summary>
		/// <param name="drawArea"></param>
		/// <param name="e"></param>
		public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				_drawingInProcess = false;
				newPolyLine = null;
			} else
			{
				Point p = drawArea.BackTrackMouse(new Point(e.X, e.Y));
				if (drawArea.PenType == DrawingPens.PenType.Generic)
				{
					if (_drawingInProcess == false)
					{
						newPolyLine = new DrawPolyLine(p.X, p.Y, p.X + 1, p.Y + 1, drawArea.LineColor, drawArea.LineWidth);
						newPolyLine.EndPoint = new Point(p.X + 1, p.Y + 1);
						AddNewObject(drawArea, newPolyLine);
						lastX = e.X;
						lastY = e.Y;
						_drawingInProcess = true;
					} else
					{
						// Drawing is in process, so simply add a new point
						newPolyLine.AddPoint(p);
						newPolyLine.EndPoint = p;
					}
				} else
				{
					if (_drawingInProcess == false)
					{
						newPolyLine = new DrawPolyLine(p.X, p.Y, p.X + 1, p.Y + 1, drawArea.PenType);
						newPolyLine.EndPoint = new Point(p.X + 1, p.Y + 1);
						AddNewObject(drawArea, newPolyLine);
						lastX = e.X;
						lastY = e.Y;
						_drawingInProcess = true;
					} else
					{
						// Drawing is in process, so simply add a new point
						newPolyLine.AddPoint(p);
						newPolyLine.EndPoint = p;
					}
				}
			}
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

			if (newPolyLine == null)
				return;                 // precaution

			Point point = drawArea.BackTrackMouse(new Point(e.X, e.Y));
			// move last point
			newPolyLine.MoveHandleTo(point, newPolyLine.HandleCount);
			drawArea.Refresh();
		}

		public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
		{
			base.OnMouseUp(drawArea, e);
		}
	}
}
