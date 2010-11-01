using System.Collections.Generic;
using System.Drawing;
using Genetibase.RasterDatabase.Geometry;

namespace Genetibase.RasterDatabase
{
    public class DataLayer
    {
        readonly ushort bpp;
        ulong pixelCount;
        string dataType;

        readonly List<DataArea> areas;

        readonly string name;

        Rectangle layerArea;

        RectangleGroup rectGroup;

        public DataLayer(string name, ushort bpp, string dataType)
        {
            this.name = name;
            this.bpp = bpp;
            this.dataType = dataType;

            areas = new List<DataArea>();
        }

//        public void QuerySample(int position, Size dimensions, Size resolution)
//        {
//        }

        public IList<DataArea> Areas
        {
            get { return areas; }
        }

        public string Name
        {
            get { return name; }
        }

        public ushort BPP
        {
            get { return bpp; }
        }

        public Rectangle Area
        {
            get { return layerArea; }
        }

        public void AddArea(DataArea area)
        {
            areas.Add(area);

            // re-calc layer area
            if (layerArea.Size == Size.Empty ||
                area.Area.Contains(layerArea))
            {
                // expand layer area
                layerArea = Rectangle.Union(layerArea, area.Area);

                pixelCount += (ulong)(area.Area.Width * area.Area.Height);
            }
        }

        public RectangleGroupQuadTree BuildQuadTree(int maxResolution)
        {
            rectGroup = new RectangleGroup(areas.ToArray());
            return new RectangleGroupQuadTree(maxResolution, rectGroup);
        }
    }
}