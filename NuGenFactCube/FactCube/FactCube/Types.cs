using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;

namespace Genetibase.FactCube
{
    /// <summary>
    /// IBounds interface
    /// </summary>
    public interface IBounds
    {
		/// <summary>
		/// Determine if bounds intersect plane
		/// </summary>
		/// <param name="plane">Plane</param>
		/// <returns>Intersection type</returns>
        PlaneIntersectionType Intersects( Plane plane );

        /// <summary>
        /// Get bounds positions
        /// </summary>
        IList<Point3D> Positions { get; }

        /// <summary>
        /// Get union of bounds
        /// </summary>
        /// <param name="bounds">Bounds</param>
        /// <returns>Union of bounds</returns>
        void Merge( IBounds bounds );
    }

    /// <summary>
    /// IBSPItem interface
    /// </summary>
    public interface IBSPItem
    {
        PlaneIntersectionType Intersects( Plane plane, out IBSPItem frontItem, out IBSPItem backItem );
        IBounds Bounds { get; }
    }

    /// <summary>
    /// Plane intersection type
    /// </summary>
    public enum PlaneIntersectionType
    {
        Front,
        Back,
        Intersecting
    }

    /// <summary>
    /// Halfspace identifier
    /// </summary>
    public enum Halfspace
    {
        Negative = -1,
        Coincident = 0,
        Positive = 1
    }

    /// <summary>
    /// Volume containment type
    /// </summary>
    public enum ContainmentType
    {
        Disjoint,
        Contains,
        Intersects
    }
}