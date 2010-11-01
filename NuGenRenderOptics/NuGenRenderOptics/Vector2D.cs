using System;

namespace NuGenRenderOptics
{
    struct Vector2D
    {
        public double X, Y;

        /// <summary>
        /// Initializes a new instance of the Vector2D structure.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public static Vector2D operator *(Vector2D left, double right)
        {
            return new Vector2D(left.X * right, left.Y * right);
        }

        public static Vector2D operator *(double right, Vector2D left)
        {
            return new Vector2D(left.X * right, left.Y * right);
        }
    }
}