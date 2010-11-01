using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Genetibase.RasterDatabase.Geometry
{
    public class RectangleGroup
    {
        Region region;
        Rectangle groupArea;
        DataArea[] rectangles;

        DataArea[] breakdownRects;

        public RectangleGroup(DataArea[] rectangles)
        {
            this.rectangles = rectangles;
            region = new Region(rectangles[0].Area);
            int right = int.MinValue, bottom = int.MinValue;
            groupArea.X = int.MaxValue;
            groupArea.Y = int.MaxValue;
            for (int i = 0; i < rectangles.Length; i++)
            {
                DataArea area = rectangles[i];
                if (i != 0)
                    region.Union(area.Area);

                if (area.Area.X < groupArea.X)
                    groupArea.X = area.Area.X;
                if (area.Area.Y < groupArea.Y)
                    groupArea.Y = area.Area.Y;

                if (area.Area.Right > right)
                    right = area.Area.Right;
                if (area.Area.Bottom > bottom)
                    bottom = area.Area.Bottom;
            }
            groupArea.Width = right - groupArea.X;
            groupArea.Height = bottom - groupArea.Y;
        }

        public Rectangle Bounds
        {
            get { return groupArea; }
        }

        public DataArea[] BreakdownGroup()
        {
            if (breakdownRects != null)
                return breakdownRects;

            RectangleF[] rectsF = region.GetRegionScans(new Matrix());
            DataArea[] rects = breakdownRects = new DataArea[rectsF.Length];

            // convert rectangles
            for (int i = 0; i < rects.Length; i++)
            {
                Rectangle rect = Rectangle.Truncate(rectsF[i]);
                // match to source rect
                DataArea src = null;
                foreach (DataArea tRect in rectangles)
                {
                    if (tRect.Area.Contains(rect))
                    {
                        src = tRect;
                        break;
                    }
                }
                if (src != null)
                {
                    // calc tex-coords
                    int xStartPos = rect.X - src.Area.X;
                    float xScale = src.TexCoords.Width / src.Area.Width;
                    float xStart = src.TexCoords.X + (xStartPos * xScale);

                    float xEnd = rect.Width * xScale;

                    int yStartPos = rect.Y - src.Area.Y;
                    float yScale = src.TexCoords.Height / src.Area.Height;
                    float yStart = src.TexCoords.Y + (yStartPos * yScale);

                    float yEnd = rect.Height * yScale;

                    if (src.Data is byte[])
                        rects[i] = new ByteArea(rect, new RectangleF(xStart, yStart, xEnd, yEnd), (byte[])src.Data, src.DataSize);
                    else
                        rects[i] = new FloatArea(rect, new RectangleF(xStart, yStart, xEnd, yEnd), (float[])src.Data, src.DataSize);
                }
            }

            return rects;
        }
    }

    public class RectangleGroupQuadTree
    {
        public class GroupNode
        {
            public DataArea[] Rectangles;
            public Rectangle NodeArea;

            public GroupNode[] SubNodes;

            public float MaxDataValue, MinDataValue, AvrDataValue;
        }

        GroupNode root;
        readonly int maxResolution;
        int treeDepth;

        public RectangleGroupQuadTree(int maxResolution, RectangleGroup rectGroup)
        {
            this.maxResolution = maxResolution;

            BuildTree(rectGroup);
        }

        #region Properties

        public int Depth
        {
            get { return treeDepth; }
        }

        public float MaxDataValue
        {
            get { return root.MaxDataValue; }
        }

        public float MinDataValue
        {
            get { return root.MinDataValue; }
        }

        public float AverageDataValue
        {
            get { return root.AvrDataValue; }
        }
        #endregion

        private void BuildTree(RectangleGroup group)
        {
            // calculate total area size
            int size = maxResolution;
            treeDepth = 1;
            int maxBounds = group.Bounds.Width > group.Bounds.Height ? group.Bounds.Width : group.Bounds.Height;
            while (size < maxBounds)
            {
                size *= 2;
                treeDepth++;
            }

            // create tree
            DataArea[] rects = group.BreakdownGroup();
            root = CreateNode(size, new Point(0, 0),  rects, 1, treeDepth);
            CalcStats(root);
        }

        private static void CalcStats(GroupNode node)
        {
            if (node.SubNodes != null && node.SubNodes.Length > 0)
            {
                float min = float.MaxValue, max = float.MinValue, avr = 0;
                int numNodes = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (node.SubNodes[i] != null)
                    {
                        CalcStats(node.SubNodes[i]);
                        if (node.SubNodes[i].MinDataValue < min)
                            min = node.SubNodes[i].MinDataValue;
                        if (node.SubNodes[i].MaxDataValue > max)
                            max = node.SubNodes[i].MaxDataValue;
                        avr += node.SubNodes[i].AvrDataValue;
                        numNodes++;
                    }
                }
                // collate stats
                node.MinDataValue = min;
                node.MaxDataValue = max;
                node.AvrDataValue = avr / numNodes;
            }
            else
            {
                // collect stats on this leaf node
                float min = float.MaxValue, max = float.MinValue, avr = 0;
                foreach (DataArea area in node.Rectangles)
                {
                    area.CalculateStats();

                    if (area.MinDataValue < min)
                        min = area.MinDataValue;
                    if (area.MaxDataValue > max)
                        max = area.MaxDataValue;
                    avr += area.AverageDataValue;
                }
                node.MinDataValue = min;
                node.MaxDataValue = max;
                node.AvrDataValue = avr / node.Rectangles.Length;
            }
        }

        private static GroupNode CreateNode(int bounds, Point location, DataArea[] rects,
                                            int depth, int maxDepth)
        {
            GroupNode node = new GroupNode();
            node.NodeArea = new Rectangle(location, new Size(bounds, bounds));

            Rectangle nodeArea = new Rectangle(node.NodeArea.X, node.NodeArea.Y, node.NodeArea.Width/* + 1*/, node.NodeArea.Height/* + 1*/);

            // cut triangles to make up node contents
            List<DataArea> contents = new List<DataArea>();
            foreach (DataArea rect in rects)
            {
                if (node.NodeArea.IntersectsWith(rect.Area))
                {
                    // cut rect to fit
                    Rectangle intersection = Rectangle.Intersect(rect.Area, nodeArea);
                    float xScale = rect.TexCoords.Width / rect.Area.Width;
                    float xStart = rect.TexCoords.X + ((intersection.X - rect.Area.X) * xScale);
                    float xWidth = intersection.Width * xScale;

                    float yScale = rect.TexCoords.Height / rect.Area.Height;
                    float yStart = rect.TexCoords.Y + ((intersection.Y - rect.Area.Y) * yScale);
                    float yHeight = intersection.Height * yScale;

                    RectangleF texCoords = new RectangleF(xStart, yStart, xWidth, yHeight);
                    if (rect.Data is byte[])
                        contents.Add(new ByteArea(intersection, texCoords, (byte[])rect.Data, rect.DataSize));
                    else
                        contents.Add(new FloatArea(intersection, texCoords, (float[])rect.Data, rect.DataSize));
                }
            }

            if (contents.Count == 0)
                return null;
            
            node.Rectangles = contents.ToArray();

            // do sub-groups
            if (depth != maxDepth)
            {
                node.SubNodes = new GroupNode[4];
                int subBounds = bounds / 2;
                node.SubNodes[0] = CreateNode(subBounds, location, rects, depth + 1, maxDepth);
                node.SubNodes[1] = CreateNode(subBounds, location + new Size(subBounds, 0), rects, depth + 1, maxDepth);
                node.SubNodes[2] = CreateNode(subBounds, location + new Size(0, subBounds), rects, depth + 1, maxDepth);
                node.SubNodes[3] = CreateNode(subBounds, location + new Size(subBounds, subBounds), rects, depth + 1, maxDepth);
            }

            return node;
        }

        public void GetNodes(int depth, out GroupNode[] nodes)
        {
            List<GroupNode> nodeList = new List<GroupNode>();

            // walk tree
            CollectNodes(root, 1, depth, nodeList);

            if (nodeList.Count > 0)
                nodes = nodeList.ToArray();
            else
                nodes = null;
        }

        private static void CollectNodes(GroupNode node, int depth, int targetDepth, List<GroupNode> nodeList)
        {
            if (depth == targetDepth)
            {
                nodeList.Add(node);
                return;
            }
            if (node.SubNodes != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (node.SubNodes[i] != null)
                        CollectNodes(node.SubNodes[i], depth + 1, targetDepth, nodeList);
                }
            }
        }
    }
}