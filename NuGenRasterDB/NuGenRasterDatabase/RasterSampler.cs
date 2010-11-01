namespace Genetibase.RasterDatabase
{
    public abstract class RasterSampler : IRasterSampler
    {
        protected readonly DataArea dataSource;
        private int sampleSize;
        private float sampleSizeF;

        /// <summary>
        /// Initializes a new instance of the RasterSampler class.
        /// </summary>
        /// <param name="dataSource">The data area source</param>
        /// <param name="sampleSize">The initial sample size to take</param>
        public RasterSampler(DataArea dataSource, int sampleSize)
        {
            this.dataSource = dataSource;
            SampleSize = sampleSize;
        }

        /// <summary>
        /// Initializes a new instance of the RasterSampler class.
        /// </summary>
        /// <param name="dataSource">The data area source</param>
        /// <param name="sampleSizeF">The initial sample size to take</param>
        public RasterSampler(DataArea dataSource, float sampleSizeF)
        {
            this.dataSource = dataSource;
            SampleSizeF = sampleSizeF;
        }

        public void GetTexCoords(float x, float y, out float tx, out float ty)
        {
            tx = dataSource.TexCoords.Left + (dataSource.TexCoords.Width * x);
            ty = dataSource.TexCoords.Top + (dataSource.TexCoords.Height * y);
        }

        #region IRasterSampler Members

        public DataArea DataSource
        {
            get { return dataSource; }
        }

        public int SampleSize
        {
            get { return sampleSize; }
            set { sampleSize = value; sampleSizeF = (float)sampleSize / dataSource.Area.Width; }
            // NOTE: ^ Not uniform accross 2Ds??
        }

        public float SampleSizeF
        {
            get { return sampleSizeF; }
            set { sampleSizeF = value; sampleSize = (int)(sampleSizeF * dataSource.Area.Width); }
        }

        public int SampleComplexity
        {
            get { return 1; }
        }

        public abstract float this[int x, int y]
        {
            get;
        }

        public abstract float this[float x, float y]
        {
            get;
        }
        #endregion
    }
}