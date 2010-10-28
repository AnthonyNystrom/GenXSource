using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
namespace FPlotLibrary
{
	/// <summary>
	/// A class that can be used to print the GraphControl.
	/// </summary>
	public class GraphPrintDocument: PrintDocument {
		private GraphControl graph;
		private GraphModel model;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public GraphPrintDocument(GraphControl graph) {
			this.graph = graph;
		}

		/// <summary>
		/// This method is called when printing starts. It creates a local copy of the
		/// <see cref="GraphModel">GraphModel</see> from the graph that was passed to the constructor. 
		/// </summary>
		protected override void OnBeginPrint(PrintEventArgs e) {
			base.OnBeginPrint (e);
			model = (GraphModel)graph.Model.Clone();
		}

		/// <summary>
		/// This method is called to print an individual page.
		/// </summary>
		protected override void OnPrintPage(System.Drawing.Printing.PrintPageEventArgs e) {
			GraphControl printControl = new GraphControl();
			Rectangle b = e.MarginBounds, clip;
			Graphics g = e.Graphics;
			g.TranslateTransform(b.X, b.Y, MatrixOrder.Append);
			clip = new Rectangle(0, 0, b.Width, b.Height);
			g.SetClip(clip);
			printControl.Bounds = b;
			printControl.SetRange(printControl.x0, printControl.x1, printControl.y0, printControl.y1);
			printControl.Model = model;
			printControl.AsyncDraw = false;
			g.SmoothingMode = SmoothingMode.HighQuality;
			printControl.PaintGraph(g);
			printControl.Dispose();
		}

	}
}
