using Genetibase.RasterDatabase;

namespace Genetibase.NuGenDEMVis
{
    public class MultiSampleRasterSampler : SimpleRasterSampler
    {
        readonly int halfSampleSz;
        readonly float halfSampleSzF;

        public MultiSampleRasterSampler(DataArea dataSource, int sampleSize, int numSamples)
            : base(dataSource, sampleSize)
        {
            halfSampleSz = sampleSize / 2;
            halfSampleSzF = 1f / numSamples;
        }

        public override float this[int x, int y]
        {
            get
            {
                // get 5 samples
                float avr = base[x, y] +
                            base[x + halfSampleSz, y] +
                            base[x - halfSampleSz, y] +
                            base[x, y + halfSampleSz] +
                            base[x, y - halfSampleSz];
                return avr / 5;
            }
        }

        public override float this[float x, float y]
        {
            get
            {
                // get 5 samples
                float xMin = x - halfSampleSzF;
                float xMax = x + halfSampleSzF;
                float yMin = y - halfSampleSzF;
                float yMax = y + halfSampleSzF;

                if (xMin < 0)
                    xMin = x;
                else if (xMax > 1)
                    xMax = x;
                if (yMin < 0)
                    yMin = y;
                else if (yMax > 1)
                    yMax = y;

                float avr = base[x, y] +
                            base[xMax, y] +
                            base[xMin, y] +
                            base[x, yMax] +
                            base[x, yMin];
                return avr / 5;
            }
        }

        public override float GetByte(float x, float y)
        {
            // get 5 samples
            float xMin = x - halfSampleSzF;
            float xMax = x + halfSampleSzF;
            float yMin = y - halfSampleSzF;
            float yMax = y + halfSampleSzF;

            if (xMin < 0)
                xMin = x;
            else if (xMax > 1)
                xMax = x;
            if (yMin < 0)
                yMin = y;
            else if (yMax > 1)
                yMax = y;
            
            float avr = base.GetByte(x, y) +
                    base.GetByte(xMax, y) +
                    base.GetByte(xMin, y) +
                    base.GetByte(x, yMax) +
                    base.GetByte(x, yMin);
            return avr / 5;
        }
    }
}