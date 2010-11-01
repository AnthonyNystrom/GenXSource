using System.Collections.Generic;
using System.Drawing;
using Genetibase.RasterDatabase.Geometry;

namespace Genetibase.RasterDatabase
{
    public class RasterDatabase
    {
        readonly List<DataLayer> layers;
        Rectangle area;

        public RasterDatabase()
        {
            layers = new List<DataLayer>();
        }

        public void AddLayer(DataLayer layer)
        {
            layers.Add(layer);
        }

        public IList<DataLayer> Layers
        {
            get { return layers; }
        }

        public Rectangle Area
        {
            get { return area; }
        }

        public RectangleGroupQuadTree ProduceLayerMipMap(DataLayer layer, int maxResolution)
        {
             return layer.BuildQuadTree(maxResolution);
        }

        public RectangleGroupQuadTree ProduceLayerMipMap(int layer, int maxResolution)
        {
            return layers[layer].BuildQuadTree(maxResolution);
        }

        public void UpdateArea()
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            foreach (DataLayer layer in layers)
            {
                if (layer.Area.Left < minX)
                    minX = layer.Area.Left;
                if (layer.Area.Right > maxX)
                    maxX = layer.Area.Right;
                if (layer.Area.Bottom < minY)
                    minY = layer.Area.Bottom;
                if (layer.Area.Top > maxY)
                    maxY = layer.Area.Top;
            }
            area = new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }
    }
}