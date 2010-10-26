using System;
using Microsoft.DirectX;

namespace Genetibase.VisUI.Maths
{
    public class CircleHelper
    {
        public static void CalcCirclePointsCCW(int numPoints, float radius, Vector2 center, out Vector2[] points)
        {
            CalcArcPointsCCW(numPoints, radius, center, 360f, out points);
        }

        public static void CalcArcPointsCCW(int numPoints, float radius, Vector2 center, float arcAngle,
                                                 out Vector2[] points)
        {
            // calc intervals
            float intervalDeg = arcAngle / (numPoints - 1.0f);
            float intervalRad = (float)Math.PI * (intervalDeg / 180.0f);

            // calc positions for all points
            points = new Vector2[numPoints - 1];
            for (int p = 0; p < numPoints - 1; p++)
            {
                float x = center.X + ((float)Math.Cos(intervalRad * p) * radius);
                float y = center.Y + ((float)Math.Sin(intervalRad * p) * radius);
                points[p] = new Vector2(x, y);
            }
        }
    }
}