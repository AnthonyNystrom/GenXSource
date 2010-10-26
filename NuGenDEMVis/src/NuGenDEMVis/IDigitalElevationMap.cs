using System;
using System.Drawing;

namespace Genetibase.NuGenDEMVis
{
    /// <summary>
    /// Encapsultes a 1D range using floats
    /// </summary>
    public struct RangeF
    {
        public float Max, Min;

        /// <summary>
        /// Initializes a new instance of the RangeF structure.
        /// </summary>
        /// <param name="max">Top limit of range</param>
        /// <param name="min">Bottom limit of range</param>
        public RangeF(float max, float min)
        {
            Max = max;
            Min = min;
        }

        public static RangeF Empty
        {
            get { return new RangeF(float.NaN, float.NaN); }
        }
    }

    /// <summary>
    /// Encapsulates a map of discreet heights
    /// </summary>
    interface IDigitalElevationMap : IDisposable
    {
        /// <summary>
        /// The size of the map (in pixels / discreet native dimensions)
        /// </summary>
        Size MapSize { get; }

        /// <summary>
        /// The range of heights in the map (typically 0.0 - 1.0)
        /// </summary>
        RangeF HeightRange { get; }

        /// <summary>
        /// Returns the absolute discreet height value for this position on the map
        /// </summary>
        /// <param name="x">The absolute x position on the map</param>
        /// <param name="y">The absolute y position on he map</param>
        /// <returns>The height value at this point (0.0 - 1.0)</returns>
        float this[int x, int y] { get; }

        /// <summary>
        /// The largest dimension of the map
        /// </summary>
        int MaxDimension { get; }

        /// <summary>
        /// Returns the absolute discreet height value for this position on the map.
        /// Positions should be of range 0.0 - 1.0 and are rounded towards 0
        /// </summary>
        /// <param name="x">The x position on the map</param>
        /// <param name="y">The y position on he map</param>
        /// <returns>The height value at this point (0.0 - 1.0</returns>
        //float this[float x, float y] { get; }
    }
}