using System;

namespace Examples
{
    static class MathUtil
    {
        const double PiDividedBy2 = Math.PI / 2;

        public static double SolveQuadraticEquation(double a, double b, double c)
        {
            double solution1 = ((-b + Math.Sqrt((b * b) - (4.0 * a * c))) / (2.0 * a));
            double solution2 = ((-b - Math.Sqrt((b * b) - (4.0 * a * c))) / (2.0 * a));

            return Math.Max(solution1, solution2);
        }

        public static double FindInnerAnglePhase1(double bottomLength, double dominoHeight, double outerAngle)
        {
            double tan = Math.Tan(outerAngle);

            double a = (tan * tan) + 1;
            double b = 2 * bottomLength;
            double c = (bottomLength * bottomLength) - (dominoHeight * dominoHeight);

            double solution = SolveQuadraticEquation(a, b, c);

            double innerAngle = Math.Acos((bottomLength + solution) / dominoHeight);

            return innerAngle;
        }


        static double GetProjectedLength(double tiltAngle, double depth)
        {
            // We use the projected length to figure out the bottomLength parameter for FindInnerAnglePhase1.
            // It goes like this:
            // bottomLength = (distanceBetweenDominoes + dominoDepth) - projectedLength;

            return depth / Math.Sin(tiltAngle);
        }

        static double FindInnerAnglePhase2(double outerAngle, double depth, double distanceBetween)
        {
            double y = depth * Math.Sin(PiDividedBy2 - outerAngle);
            double x = depth * Math.Cos(PiDividedBy2 - outerAngle);

            return Math.Atan(y / (distanceBetween + depth - x));
        }

        public static double FindInnerAngle(double outerAngle, double dominoHeight, double dominoDepth, double distanceBetween)
        {
            outerAngle = outerAngle * Math.PI / 180;

            double projectedLength = GetProjectedLength(outerAngle, dominoDepth);

            double bottomLength = dominoDepth + distanceBetween - projectedLength;

            if (bottomLength > 0)
            {
                return FindInnerAnglePhase1(bottomLength, dominoHeight, outerAngle) * 180 / Math.PI;
            }
            else
            {
                return FindInnerAnglePhase2(outerAngle, dominoDepth, distanceBetween) * 180 / Math.PI;
            }
        }
    }

}
