using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenDEMVis.Graph
{
    class CrossSectionInfo
    {
        public readonly Vector2 StartPoint, EndPoint;
        public readonly float SectionLength;
        public readonly int PointsCount;

        public CrossSectionInfo(Vector2 startPoint, Vector2 endPoint, float sectionLength, int pointsCount)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            SectionLength = sectionLength;
            PointsCount = pointsCount;
        }
    }

	struct GraphNode
	{
		public Vector2 Position;
		public Color Clr;

        public GraphNode(Vector2 position, Color clr)
        {
            Position = position;
            Clr = clr;
        }
    }
	
	struct Point3D
	{
		public Vector3 Position;
		public Color Clr;
	}
	
	class CrossSectionGraph
	{
		Device gDevice;
		Vector3 worldStart, worldEnd;
		GraphNode[] graphNodes;
		Point3D[] points;
		Rectangle area;
//        int sampleRate;
        int density;

        Vector2 sampleStart, sampleEnd;

        public enum SamplingMode
        {
            OnGrid,
            Uniform
        }
		
		protected CrossSectionGraph(Device device, Rectangle area, Vector3 start, Vector3 end, int density)
		{
            gDevice = device;
            this.area = area;
            worldStart = start;
            worldEnd = end;
            this.density = density;
		}

        public void DrawGraph(Bitmap bitmap)
		{
            Graphics g = Graphics.FromImage(bitmap);
            DrawGraph(g);
            g.Dispose();
		}
		
		public void DrawGraph(Graphics g)
		{
            // TODO: Some quality options etc??
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;

			// determine axis scales
			/*Vector2 scale = new Vector2((float)area.Width / bitmap.Width,
			                            (float)area.Height / bitmap.Height);*/
			
			//g.ScaleTransform(scale.X, scale.Y);
            g.FillRectangle(Brushes.Black, g.ClipBounds);
            /*
            PointF[] sPts = new PointF[points.Length];
            int x = 0;
            for (int i = 0; i < sPts.Length; i++)
            {
                sPts[i] = new PointF(x, points[i].Position.Y);
                x += density;
            }
            BSpline spline = new BSpline(sPts);
            spline.Draw(g, 10, new Pen(new LinearGradientBrush(new Point(0, 0), new Point(0, 256), Color.White, Color.Black)));
            */
            if (graphNodes == null)
            {
                graphNodes = new GraphNode[points.Length];
                for (int i = 0; i < graphNodes.Length; i++)
                {
                    graphNodes[i] = new GraphNode(new Vector2(points[i].Position.X, points[i].Position.Z), Color.FromArgb(255, (int)points[i].Position.Y, (int)points[i].Position.Y, (int)points[i].Position.Y));
                }
            }
			
			// draw a line for each node of graph
            int x = 0;
			for (int i = 0; i < graphNodes.Length - 2; i+=2)
			{
                //int x = (int)Math.Floor((new Vector2(points[i].Position.X, points[i].Position.Z) - sampleStart).Length());
                Point p1 = new Point(x, 256 - (int)points[i].Position.Y);
                Point p2 = new Point(x + density, 256 - (int)points[i + 2].Position.Y);
				using (Pen pen = new Pen(new LinearGradientBrush(p1, p2,
				                                                 graphNodes[i].Clr, graphNodes[i + 2].Clr)))
				{
                    g.DrawLine(pen, p1, p2);
				}
                x += density;
			}
			
			g.Flush();
		}
		
		public void Generate3DOverlayLineStrip(out VertexBuffer vBuffer, out int numVerts)
		{
			vBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), points.Length,
                                       gDevice, Usage.WriteOnly, CustomVertex.PositionColored.Format,
                                       Pool.Managed);
            CustomVertex.PositionColored[] verts = (CustomVertex.PositionColored[])vBuffer.Lock(0, LockFlags.None);
            for (int i = 0; i < points.Length; i++)
            {
                verts[i] = new CustomVertex.PositionColored(points[i].Position, points[i].Clr.ToArgb());
            }
            vBuffer.Unlock();
            numVerts = points.Length;
		}

        public static void GenerateFromGeometry()
        {

        }

        public static CrossSectionGraph GenerateFromSampler(Device device, Rectangle area,
                                                            Vector3 start, Vector3 end,
                                                            int density, Bitmap bitmap,
                                                            SamplingMode sampling)
        {
            CrossSectionGraph graph = new CrossSectionGraph(device, area, start, end, density);

            // determine real start and end positions in the data set (if out of bounds)
            // (basicaly rectangle intersection)
            Vector2 intersectHP = new Vector2(graph.worldStart.X, graph.worldStart.Z);
            Vector2 intersectLP = new Vector2(graph.worldEnd.X, graph.worldEnd.Z);
            LiangBarskyClipping clipping = new LiangBarskyClipping(area);
            if (clipping.ClipLine(ref intersectLP, ref intersectHP))
            {
                graph.sampleStart = intersectHP;
                graph.sampleEnd = intersectLP;
                // determine sample points
                Vector2 intersectionV = new Vector2(intersectLP.X - intersectHP.X, intersectLP.Y - intersectHP.Y);
                
                // determine x-axis sample points
                // round start up and end down
                int startX = intersectHP.X != 0 ? (int)Math.Ceiling(intersectHP.X / density) : 0;
                int endX = intersectLP.X != 0 ? (int)Math.Floor(intersectLP.X / density) : 0;
                int xAxisCount = endX - startX;
                startX *= density;
                endX *= density;
                
                int startY = intersectHP.Y != 0 ? (int)Math.Ceiling(intersectHP.Y / density) : 0;
                int endY = intersectLP.Y != 0 ? (int)Math.Floor(intersectLP.Y / density) : 0;
                int yAxisCount = endY - startY;
                startY *= density;
                endY *= density;

                // add extra points for start and end if not exactly on boundaries
                int extra = 0;
                if (intersectHP.X != startX && intersectHP.Y != startY)
                    extra++;
                if (intersectLP.X != endX && intersectLP.Y != endY)
                    extra++;

                // write x values into array
                int pIdx = 0;
                graph.points = new Point3D[xAxisCount + yAxisCount + extra];
                if (intersectHP.X != startX && intersectHP.Y != startY)
                {
                    graph.points[pIdx] = new Point3D();
                    graph.points[pIdx++].Position = new Vector3(intersectHP.X, float.NaN, intersectHP.Y);
                }

                Vector2 v = new Vector2(0, 1);
                Vector2 s = new Vector2(0, area.Top);
                Vector2 hp = new Vector2(intersectHP.X, intersectHP.Y);
                for (int i = startX; i < endX; i += density)
                {
                    // get y intersection point
                    s.X = i;
                    Vector2 p;
                    IntersectLineAABB.LineIntersection(hp, intersectionV, s, v, out p);

                    graph.points[pIdx] = new Point3D();
                    graph.points[pIdx++].Position = new Vector3(i, float.NaN, p.Y);
                }

                v = new Vector2(1, 0);
                s = new Vector2(area.Left, 0);
                for (int i = startY; i < endY; i += density)
                {
                    Vector2 p;
                    s.Y = i;
                    IntersectLineAABB.LineIntersection(hp, intersectionV, s, v, out p);

                    graph.points[pIdx] = new Point3D();
                    graph.points[pIdx++].Position = new Vector3(p.X, float.NaN, i);
                }

                if (intersectLP.X != endX && intersectLP.Y != endY)
                {
                    graph.points[pIdx] = new Point3D();
                    graph.points[pIdx++].Position = new Vector3(intersectLP.X, float.NaN, intersectLP.Y);
                }

                // measure & sort by distance from HP
                float[] distances = new float[graph.points.Length];
                for (int i = 0; i < distances.Length; i++)
                {
                    //if (distances[i] != -1)
                    //{
                        float x = graph.points[i].Position.X - intersectHP.X;
                        float y = graph.points[i].Position.Z - intersectHP.Y;
                        distances[i] = (float)Math.Sqrt((x * x) + (y * y));
                        // check for duplicates
                        /*for (int d = i + 1; d < distances.Length; d++)
                        {
                            if (graph.points[d].Position == graph.points[i].Position)
                            {
                                distances[d] = -1;
                            }
                        }*/
                    //}
                }

                int[] order = new int[distances.Length];
                for (int i = 0; i < distances.Length; i++)
                {
                    order[i] = i;
                }
                for (int o = 0; o < order.Length; o++)
                {
                    for (int d = o + 1; d < order.Length; d++)
                    {
                        if (distances[order[d]] < distances[order[o]])
                        {
                            int temp = order[o];
                            order[o] = order[d];
                            order[d] = temp;
                        }
                    }
                }

                // re-arange array for new order
                Point3D[] newPoints = new Point3D[graph.points.Length];
                for (int i = 0; i < graph.points.Length; i++)
                {
                    newPoints[i] = graph.points[order[i]];
                }
                graph.points = newPoints;

                // Take the actual height samples
                for (int i = 0; i < graph.points.Length; i++)
                {
                    graph.points[i].Position.Y = bitmap.GetPixel((int)graph.points[i].Position.X,
                                                                 (int)graph.points[i].Position.Z).R;
                }

                return graph;
            }
            return null;
        }
	}
}
