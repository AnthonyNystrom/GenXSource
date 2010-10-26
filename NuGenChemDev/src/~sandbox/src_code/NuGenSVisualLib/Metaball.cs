using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using System.Drawing;

namespace NuGenSVisualLib.Maths.Volumes
{
    public interface IVolume
    {
        float GetPotentialAtPoint(Vector3 point);
        bool IsOutside(Vector3 point);
        float GetBoundingSphere();

        Vector3 Origin { get; }
        Color Colour { get; }
    }

    public class BoxVolume : IVolume
    {
        float width, height, depth;
        Vector3 origin;
        Color clr;

        public BoxVolume(Vector3 origin, float width, float height, float depth, Color clr)
        {
            this.origin = origin;
            this.width = width;
            this.height = height;
            this.depth = depth;
            this.clr = clr;
        }

        #region IVolume Members

        public Vector3 Origin
        {
            get { return origin; }
        }

        public Color Colour
        {
            get { return clr; }
        }

        public float GetPotentialAtPoint(Vector3 point)
        {
            if (point.X > origin.X - width && point.X < origin.X + width &&
                point.Y > origin.Y - depth && point.Y < origin.Y + depth &&
                point.Z > origin.Z - height && point.Z < origin.Z + height)
            {
                return 1.0f;
            }
            return 0;
        }

        public bool IsOutside(Vector3 point)
        {
            return (GetPotentialAtPoint(point) < 0.1f);
        }

        public float GetBoundingSphere()
        {
            float largest = width;
            if (height > largest)
                largest = height;
            if (depth > largest)
                largest = depth;
            return largest;
        }

        #endregion
    }

    public class Metaball : IVolume
    {
        Vector3 origin;
        float radius;
        float threshold = 0.1f;
        Color clr;

        public Metaball(Vector3 origin, float radius, Color clr)
        {
            this.origin = origin;
            this.radius = radius;
            this.clr = clr;
        }

        #region IVolume Members

        public Vector3 Origin
        {
            get { return origin; }
        }

        public Color Colour
        {
            get { return clr; }
        }

        public float GetPotentialAtPoint(Vector3 point)
        {
            Vector3 rPos = point - origin;
            return (radius * radius) / ((rPos.X * rPos.X) + (rPos.Y * rPos.Y) + (rPos.Z * rPos.Z));
        }

        public bool IsOutside(Vector3 point)
        {
            return (GetPotentialAtPoint(point) < threshold);
        }

        public float GetBoundingSphere()
        {
            return (threshold * (radius * radius));
        }

        #endregion
    }
}