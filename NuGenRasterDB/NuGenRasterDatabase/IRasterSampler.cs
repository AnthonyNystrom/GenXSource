namespace Genetibase.RasterDatabase
{
    public interface IRasterSampler
    {
        /// <summary>
        /// The data area to sample
        /// </summary>
        DataArea DataSource { get; }

        /// <summary>
        /// The number of native points to sample over
        /// </summary>
        int SampleSize { get; }

        /// <summary>
        /// The number of native points to sample over
        /// </summary>
        float SampleSizeF { get; }

        /// <summary>
        /// The number of lookups per sample taken
        /// </summary>
        int SampleComplexity { get; }

        /// <summary>
        /// Takes a height sample for a specified point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        float this[int x, int y] { get; }

        float this[float x, float y] { get; }
    }
}