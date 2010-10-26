using Genetibase.RasterDatabase;

namespace Genetibase.NuGenDEMVis.Raster
{
    class RasterDatabaseLookup
    {
        private readonly DataLayer lookupLayer;

        public RasterDatabaseLookup(DataLayer lookupLayer)
        {
            this.lookupLayer = lookupLayer;
        }

        public float ValueLookup(float x, float y)
        {
            // convert to actual pixels
            int xActual = lookupLayer.Area.Left + (int)(lookupLayer.Area.Width * x);
            int yActual = lookupLayer.Area.Top + (int)(lookupLayer.Area.Height * y);
            return ValueLookup(xActual, yActual);
        }

        public float ValueLookup(int x, int y)
        {
            // find area
            DataArea lookupArea = null;
            foreach (DataArea area in lookupLayer.Areas)
            {
                if (area.Area.Contains(x, y))
                {
                    lookupArea = area;
                    break;
                }
            }
            if (lookupArea != null)
            {
                int xActual = x - lookupArea.Area.Left;
                int yActual = y - lookupArea.Area.Top;

                if (lookupArea is FloatArea)
                    return (float)lookupArea[xActual, yActual];
                else
                    return (byte)lookupArea[xActual, yActual];
            }
            return 0;
        }
    }
}