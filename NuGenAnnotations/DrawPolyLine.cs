using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Collections;

namespace Genetibase.NuGenAnnotation
{
	/// <summary>
	/// PolyLine graphic object - a PolyLine is a series of connected lines
	/// </summary>
	//[Serializable]
	public class DrawPolyLine : Genetibase.NuGenAnnotation.DrawLine
	{
		// Last Segment start and end points
		private Point startPoint;
		private Point endPoint;

		private ArrayList pointArray;         // list of points
		//private Cursor handleCursor;

		private const string entryLength = "Length";
		private const string entryPoint = "Point";

		public Point StartPoint
		{
			get { return startPoint; }
			set { startPoint = value; }
		}

		public Point EndPoint
		{
			get { return endPoint; }
			set { endPoint = value; }
		}

		/// <summary>
		/// Clone this instance
		/// </summary>
		public override DrawObject Clone()
		{
			DrawPolyLine drawPolyLine = new DrawPolyLine();

			drawPolyLine.startPoint = this.startPoint;
			drawPolyLine.endPoint = this.endPoint;
			drawPolyLine.pointArray = this.pointArray;

			FillDrawObjectFields(drawPolyLine);
			return drawPolyLine;
		}
		
		public DrawPolyLine()
		{
			pointArray = new ArrayList();

			LoadCursor();
			Initialize();
		}

		public DrawPolyLine(int x1, int y1, int x2, int y2)
		{
			pointArray = new ArrayList();
			pointArray.Add(new Point(x1, y1));
			pointArray.Add(new Point(x2, y2));

			//LoadCursor();
			Initialize();
		}

		public DrawPolyLine(int x1, int y1, int x2, int y2, DrawingPens.PenType p)
		{
			pointArray = new ArrayList();
			pointArray.Add(new Point(x1, y1));
			pointArray.Add(new Point(x2, y2));
			DrawPen = DrawingPens.SetCurrentPen(p);
			PenType = p;

			//LoadCursor();
			Initialize();
		}

		public DrawPolyLine(int x1, int y1, int x2, int y2, Color lineColor, int lineWidth)
		{
			pointArray = new ArrayList();
			pointArray.Add(new Point(x1, y1));
			pointArray.Add(new Point(x2, y2));
			Color = lineColor;
			PenWidth = lineWidth;

			//LoadCursor();
			Initialize();
		}

		public override void Draw(Graphics g)
		{
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Pen pen = null;

			if (DrawPen == null)
				pen = new Pen(Color, PenWidth);
			else
				pen = DrawPen.Clone() as Pen;

			Point[] pts = new Point[pointArray.Count];
			for (int i = 0; i < pointArray.Count; i++)
			{
				Point px = (Point)pointArray[i];
				pts[i] = px;
			}
			byte[] types = new byte[pointArray.Count];
			for (int i = 0; i < pointArray.Count; i++)
				types[i] = (byte)PathPointType.Line;
			GraphicsPath gp = new GraphicsPath(pts, types);
			g.DrawPath(pen, gp);
			gp.Dispose();

			pen.Dispose();
		}

		public void AddPoint(Point point)
		{
			pointArray.Add(point);
		}

		public override int HandleCount
		{
			get { return pointArray.Count; }
		}

		/// <summary>
		/// Get handle point by 1-based number
		/// </summary>
		/// <param name="handleNumber"></param>
		/// <returns></returns>
		public override Point GetHandle(int handleNumber)
		{
			if (handleNumber < 1)
				handleNumber = 1;
			if (handleNumber > pointArray.Count)
				handleNumber = pointArray.Count;
			return ((Point)pointArray[handleNumber - 1]);
		}

        //public override Cursor GetHandleCursor(int handleNumber)
        //{
        //    return handleCursor;
        //}

		public override void MoveHandleTo(Point point, int handleNumber)
		{
			if (handleNumber < 1)
				handleNumber = 1;

			if (handleNumber > pointArray.Count)
				handleNumber = pointArray.Count;
			pointArray[handleNumber - 1] = point;
			Dirty = true;
			Invalidate();
		}

		public override void Move(int deltaX, int deltaY)
		{
			int n = pointArray.Count;
			Point point;

			for (int i = 0; i < n; i++)
			{
				point = new Point(((Point)pointArray[i]).X + deltaX, ((Point)pointArray[i]).Y + deltaY);
				pointArray[i] = point;
			}
			Dirty = true;
			Invalidate();
		}

		public override void SaveToStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber, int objectIndex)
		{
			info.AddValue(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}-{2}",
				entryLength, orderNumber, objectIndex),
				pointArray.Count);

			int i = 0;
			foreach (Point p in pointArray)
			{
				info.AddValue(
					String.Format(CultureInfo.InvariantCulture,
					"{0}{1}-{2}-{3}",
					new object[] { entryPoint, orderNumber, objectIndex, i++ }),
					p);
			}
			base.SaveToStream(info, orderNumber, objectIndex);
		}

		public override void LoadFromStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber, int objectIndex)
		{
			Point point;
			int n = info.GetInt32(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}-{2}",
				entryLength, orderNumber, objectIndex));

			for (int i = 0; i < n; i++)
			{
				point = (Point)info.GetValue(
					String.Format(CultureInfo.InvariantCulture,
					"{0}{1}-{2}-{3}",
					new object[] {entryPoint, orderNumber, objectIndex, i}),
					typeof(Point));
				pointArray.Add(point);
			}
			base.LoadFromStream(info, orderNumber, objectIndex);
		}

		/// <summary>
		/// Create graphic object used for hit test
		/// </summary>
		protected override void CreateObjects()
		{
			if (AreaPath != null)
				return;

			// Create closed path which contains all polygon vertexes
			AreaPath = new GraphicsPath();

			int x1 = 0, y1 = 0;     // previous point
			int x2, y2;             // current point

			IEnumerator enumerator = pointArray.GetEnumerator();

			if (enumerator.MoveNext())
			{
				x1 = ((Point)enumerator.Current).X;
				y1 = ((Point)enumerator.Current).Y;
			}

			while (enumerator.MoveNext())
			{
				x2 = ((Point)enumerator.Current).X;
				y2 = ((Point)enumerator.Current).Y;

				AreaPath.AddLine(x1, y1, x2, y2);

				x1 = x2;
				y1 = y2;
			}

			AreaPath.CloseFigure();

			// Create region from the path
			AreaRegion = new Region(AreaPath);
		}

		private void LoadCursor()
		{
			//handleCursor = new Cursor(GetType(), "PolyHandle.cur");
		}
	}
}
