using System;
using System.Windows.Forms;
using System.Drawing;

namespace Genetibase.NuGenAnnotation
{
	/// <summary>
	/// Ellipse graphic object
	/// </summary>
	[Serializable]
	public class DrawEllipse : Genetibase.NuGenAnnotation.DrawRectangle
	{
		public DrawEllipse()
		{
			SetRectangle(0, 0, 1, 1);
			Initialize();
		}

		/// <summary>
		/// Clone this instance
		/// </summary>
		public override DrawObject Clone()
		{
			DrawEllipse drawEllipse = new DrawEllipse();
			drawEllipse.Rectangle = this.Rectangle;

			FillDrawObjectFields(drawEllipse);
			return drawEllipse;
		}
		public DrawEllipse(int x, int y, int width, int height)
		{
			Rectangle = new Rectangle(x, y, width, height);
			Initialize();
		}

		public DrawEllipse(int x, int y, int width, int height, Color lineColor, Color fillColor, bool filled)
		{
			Rectangle = new Rectangle(x, y, width, height);
			Color = lineColor;
			FillColor = fillColor;
			Filled = filled;
			Initialize();
		}

		public DrawEllipse(int x, int y, int width, int height, DrawingPens.PenType pType, Color fillColor, bool filled)
		{
			Rectangle = new Rectangle(x, y, width, height);
			DrawPen = DrawingPens.SetCurrentPen(pType);
			PenType = pType;
			FillColor = fillColor;
			Filled = filled;
			Initialize();
		}

		public DrawEllipse(int x, int y, int width, int height, Color lineColor, Color fillColor, bool filled, int lineWidth)
		{
			Rectangle = new Rectangle(x, y, width, height);
			Color = lineColor;
			FillColor = fillColor;
			Filled = filled;
			PenWidth = lineWidth;
			Initialize();
		}

		public override void Draw(Graphics g)
		{
			Pen pen = new Pen(Color.Black);
			Brush b = new SolidBrush(FillColor);
			
			if (DrawPen == null)
				pen = new Pen(Color, PenWidth);
			else
				pen = (Pen)DrawPen.Clone();

			g.DrawEllipse(pen, DrawRectangle.GetNormalizedRectangle(Rectangle));
			if (Filled == true)
				g.FillEllipse(b, DrawRectangle.GetNormalizedRectangle(Rectangle));

			pen.Dispose();
			b.Dispose();
		}
	}
}
