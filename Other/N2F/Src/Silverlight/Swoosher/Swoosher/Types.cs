using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Next2Friends.Swoosher.Media3D;

namespace Next2Friends.Swoosher
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

/// <summary>
/// Movement event arguments
/// </summary>
public class MoveEventArgs : EventArgs
{
    private Point delta;

    public MoveEventArgs( Point delta )
    {
        this.delta = delta;
    }

    public Point Delta { get { return delta; } }
}

public delegate void MoveEventHandler( object sender, MoveEventArgs e );