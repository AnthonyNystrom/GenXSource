namespace Genetibase.RasterDatabase
{
    public class SimpleRasterSampler : RasterSampler
    {
        int width, height;

        public SimpleRasterSampler(DataArea dataSource, int sampleSize)
            : base(dataSource, sampleSize)
        {
            width = dataSource.Area.Width - 1;
            height = dataSource.Area.Height - 1;
        }

        public override float this[int x, int y]
        {
            get { return (byte)DataSource[x * SampleSize, y * SampleSize]; }
        }

        public override float this[float x, float y]
        {
            get
            {
                // scale to tex coords
                //x = (x * dataSource.TexCoords.Width) + dataSource.TexCoords.Left;
                //y = (y * dataSource.TexCoords.Height) + dataSource.TexCoords.Top;

                // convert to discreet lookup location
                int xPos = (int)(x * width) + dataSource.Area.Left;
                int yPos = (int)(y * height) + dataSource.Area.Top;

                return (float)DataSource[xPos, yPos];
            }
        }

        public virtual float GetByte(float x, float y)
        {
            int xPos = (int)(x * width) + dataSource.Area.Left;
            int yPos = (int)(y * height) + dataSource.Area.Top;

            return (byte)DataSource[xPos, yPos];
        }
    }
}