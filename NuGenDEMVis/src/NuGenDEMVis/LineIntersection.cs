using System;
using System.Drawing;
using Microsoft.DirectX;

namespace Genetibase.NuGenDEMVis.Graph
{
    /// <summary>
    /// Modified from http://en.wikipedia.org/wiki/Cohen-Sutherland
    /// </summary>
    internal sealed class LiangBarskyClipping
    {
        private Vector2 _clipMin, _clipMax;

        public LiangBarskyClipping(Vector2 clipMin, Vector2 clipMax)
        {
            _clipMin = clipMin;
            _clipMax = clipMax;
        }

        public LiangBarskyClipping(PointF clipMin, PointF clipMax)
        {
            _clipMin = new Vector2(clipMin.X, clipMin.Y);
            _clipMax = new Vector2(clipMax.X, clipMax.Y);
        }

        public LiangBarskyClipping(RectangleF rectangle)
        {
            _clipMin = new Vector2(rectangle.Left, rectangle.Top);
            _clipMax = new Vector2(rectangle.Right, rectangle.Bottom);
        }

        public LiangBarskyClipping(Rectangle rectangle)
        {
            _clipMin = new Vector2(rectangle.Left, rectangle.Top);
            _clipMax = new Vector2(rectangle.Right, rectangle.Bottom);
        }

        public void SetBoundingRectangle(Vector2 start, Vector2 end)
        {
            _clipMin = start;
            _clipMax = end;
        }

        public void SetBoundingRectangle(PointF start, PointF end)
        {
            _clipMin = new Vector2(start.X, start.Y);
            _clipMax = new Vector2(end.X, end.Y);
        }

        private delegate bool ClippingHandler(float p, float q);

        public bool ClipLine(ref Vector2 lineStart, ref Vector2 lineEnd)
        {
            Vector2 P = lineEnd - lineStart;
            float tMinimum = 0, tMaximum = 1;

            ClippingHandler pqClip = delegate(float directedProjection,
                                              float directedDistance)
            {
                if (directedProjection == 0)
                {
                    if (directedDistance < 0) return false;
                }
                else
                {
                    float amount = directedDistance / directedProjection;
                    if (directedProjection < 0)
                    {
                        if (amount > tMaximum) return false;
                        else if (amount > tMinimum) tMinimum = amount;
                    }
                    else
                    {
                        if (amount < tMinimum) return false;
                        else if (amount < tMaximum) tMaximum = amount;
                    }
                }
                return true;
            };

            if (pqClip(-P.X, lineStart.X - _clipMin.X))
            {
                if (pqClip(P.X, _clipMax.X - lineStart.X))
                {
                    if (pqClip(-P.Y, lineStart.Y - _clipMin.Y))
                    {
                        if (pqClip(P.Y, _clipMax.Y - lineStart.Y))
                        {
                            if (tMaximum < 1)
                            {
                                lineEnd.X = lineStart.X + tMaximum * P.X;
                                lineEnd.Y = lineStart.Y + tMaximum * P.Y;
                            }
                            if (tMinimum > 0)
                            {
                                lineStart.X += tMinimum * P.X;
                                lineStart.Y += tMinimum * P.Y;
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool ClipLine(ref PointF lineStart, ref PointF lineEnd)
        {
            PointF P = new PointF(lineEnd.X - lineStart.X, lineEnd.Y - lineStart.Y);
            float tMinimum = 0, tMaximum = 1;

            ClippingHandler pqClip = delegate(float directedProjection,
                                              float directedDistance)
            {
                if (directedProjection == 0)
                {
                    if (directedDistance < 0) return false;
                }
                else
                {
                    float amount = directedDistance / directedProjection;
                    if (directedProjection < 0)
                    {
                        if (amount > tMaximum) return false;
                        else if (amount > tMinimum) tMinimum = amount;
                    }
                    else
                    {
                        if (amount < tMinimum) return false;
                        else if (amount < tMaximum) tMaximum = amount;
                    }
                }
                return true;
            };

            if (pqClip(-P.X, lineStart.X - _clipMin.X))
            {
                if (pqClip(P.X, _clipMax.X - lineStart.X))
                {
                    if (pqClip(-P.Y, lineStart.Y - _clipMin.Y))
                    {
                        if (pqClip(P.Y, _clipMax.Y - lineStart.Y))
                        {
                            if (tMaximum < 1)
                            {
                                lineEnd.X = lineStart.X + tMaximum * P.X;
                                lineEnd.Y = lineStart.Y + tMaximum * P.Y;
                            }
                            if (tMinimum > 0)
                            {
                                lineStart.X += tMinimum * P.X;
                                lineStart.Y += tMinimum * P.Y;
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }

    class IntersectLineAABB
    {
//        public static bool IntersectRectangle(PointF lineStart, PointF lineVector, RectangleF box,
//                                              out PointF highPt, out PointF lowPt)
//        {
//            highPt = new PointF();
//            lowPt = new PointF();
//            float sol_low, sol_high;
//
//            // degenerate cases
//            // warning: we ASSUME that L.dir is not (0,0)
//            if (lineVector.X == 0.0)
//            {
//                if (lineStart.X < box.Left || lineStart.X > box.Right)
//                    return false;
//                else
//                {
//                    sol_low = (box.Bottom - lineStart.Y) / lineVector.Y;
//                    sol_high = (box.Top - lineStart.Y) / lineVector.Y;
//                    EnforceOrder(ref sol_low, ref sol_high);
//                    highPt = lineStart + new SizeF(lineVector.X * sol_high, lineVector.Y * sol_high);
//                    lowPt = lineStart + new SizeF(lineVector.X * sol_low, lineVector.Y * sol_low);
//                    return true;
//                }
//            }
//            if (lineVector.Y == 0.0)
//            {
//                if (lineStart.Y < box.Bottom || lineStart.Y > box.Top)
//                    return false;
//                else
//                {
//                    sol_low = (box.Left - lineStart.X) / lineVector.X;
//                    sol_high = (box.Right - lineStart.X) / lineVector.X;
//                    EnforceOrder(ref sol_low, ref sol_high);
//                    highPt = lineStart + new SizeF(lineVector.X * sol_high, lineVector.Y * sol_high);
//                    lowPt = lineStart + new SizeF(lineVector.X * sol_low, lineVector.Y * sol_low);
//                    return true;
//                }
//            }
//
//            float xlow, xhigh; // solution values in x
//            float ylow, yhigh; // solution values in y
//
//            // no trivial exclusion, solve normally
//            xlow = (box.Left - lineStart.X) / lineVector.X;
//            xhigh = (box.Right - lineStart.X) / lineVector.X;
//
//            ylow = (box.Bottom - lineStart.Y) / lineVector.Y;
//            yhigh = (box.Top - lineStart.Y) / lineVector.Y;
//
//            EnforceOrder(ref xlow, ref xhigh);
//            EnforceOrder(ref ylow, ref yhigh);
//
//            // if the solutions overlap then an intersection exists
//            if (xlow > ylow && xlow < yhigh)
//            {
//                sol_low = xlow;
//                sol_high = Math.Min(xhigh, yhigh);
//                highPt = lineStart + new SizeF(lineVector.X * sol_high, lineVector.Y * sol_high);
//                lowPt = lineStart + new SizeF(lineVector.X * sol_low, lineVector.Y * sol_low);
//                return true;
//            }
//            else if(ylow > xlow && ylow < xhigh)
//            {
//                sol_low = ylow;
//                sol_high = Math.Min(xhigh, yhigh);
//                highPt = lineStart + new SizeF(lineVector.X * sol_high, lineVector.Y * sol_high);
//                lowPt = lineStart + new SizeF(lineVector.X * sol_low, lineVector.Y * sol_low);
//                return true;
//            }
//            return false;
//        }

//        public static bool IntersectRectangle(Vector2 lineStart, Vector2 lineVector, RectangleF box,
//                                              out Vector2 highPt, out Vector2 lowPt)
//        {
//            highPt = new Vector2();
//            lowPt = new Vector2();
//            float sol_low, sol_high;
//
//            // degenerate cases
//            // warning: we ASSUME that L.dir is not (0,0)
//            if (lineVector.X == 0.0)
//            {
//                if (lineStart.X < box.Left || lineStart.X > box.Right)
//                    return false;
//                else
//                {
//                    sol_low = (box.Bottom - lineStart.Y) / lineVector.Y;
//                    sol_high = (box.Top - lineStart.Y) / lineVector.Y;
//                    EnforceOrder(ref sol_low, ref sol_high);
//                    highPt = lineStart + new Vector2(lineVector.X * sol_high, lineVector.Y * sol_high);
//                    lowPt = lineStart + new Vector2(lineVector.X * sol_low, lineVector.Y * sol_low);
//                    return true;
//                }
//            }
//            if (lineVector.Y == 0.0)
//            {
//                if (lineStart.Y < box.Bottom || lineStart.Y > box.Top)
//                    return false;
//                else
//                {
//                    sol_low = (box.Left - lineStart.X) / lineVector.X;
//                    sol_high = (box.Right - lineStart.X) / lineVector.X;
//                    EnforceOrder(ref sol_low, ref sol_high);
//                    highPt = lineStart + new Vector2(lineVector.X * sol_high, lineVector.Y * sol_high);
//                    lowPt = lineStart + new Vector2(lineVector.X * sol_low, lineVector.Y * sol_low);
//                    return true;
//                }
//            }
//
//            float xlow, xhigh; // solution values in x
//            float ylow, yhigh; // solution values in y
//
//            // no trivial exclusion, solve normally
//            xlow = (box.Left - lineStart.X) / lineVector.X;
//            xhigh = (box.Right - lineStart.X) / lineVector.X;
//
//            ylow = (box.Bottom - lineStart.Y) / lineVector.Y;
//            yhigh = (box.Top - lineStart.Y) / lineVector.Y;
//
//            EnforceOrder(ref xlow, ref xhigh);
//            EnforceOrder(ref ylow, ref yhigh);
//
//            // if the solutions overlap then an intersection exists
//            if (xlow > ylow && xlow < yhigh)
//            {
//                sol_low = xlow;
//                sol_high = Math.Min(xhigh, yhigh);
//                highPt = lineStart + new Vector2(lineVector.X * sol_high, lineVector.Y * sol_high);
//                lowPt = lineStart + new Vector2(lineVector.X * sol_low, lineVector.Y * sol_low);
//                return true;
//            }
//            else if (ylow > xlow && ylow < xhigh)
//            {
//                sol_low = ylow;
//                sol_high = Math.Min(xhigh, yhigh);
//                highPt = lineStart + new Vector2(lineVector.X * sol_high, lineVector.Y * sol_high);
//                lowPt = lineStart + new Vector2(lineVector.X * sol_low, lineVector.Y * sol_low);
//                return true;
//            }
//            return false;
//        }


//        static void EnforceOrder(ref float a, ref float b)
//        {
//            if (a > b)
//            {
//                float temp = a;
//                a = b;
//                b = temp;
//            }
//        }

//        /// <summary>
//        /// http://www.gamedev.net/community/forums/topic.asp?topic_id=291161
//        /// </summary>
//        /// <param name="s1"></param>
//        /// <param name="v1"></param>
//        /// <param name="s2"></param>
//        /// <param name="v2"></param>
//        /// <param name="intersection"></param>
//        /// <returns></returns>
//        public static bool Line2LineIntersection(Vector2 s1, Vector2 v1, Vector2 s2, Vector2 v2, out Vector2 intersection)
//        {
//            Vector2 A1 = s1;
//            Vector2 D1 = v1;
//            Vector2 A2 = s2;
//            Vector2 D2 = v2;
//            Vector2 B1 = A1 + D1;
//            Vector2 B2 = A2 + D2;
//
//            // Test normal to seg1 as separating axis
//            float dotA2 = Vector2.Dot(A2 - A1, D1);
//            float dotB2 = Vector2.Dot(B2 - A1, D1);
//
//            // If A2 and B2 are either both positive or negative, perp1 is a separating axis
//            if (dotA2 * dotB2 > 0.0f)
//            {
//                intersection = new Vector2();
//                return false;
//            }
//            
//            // Test normal to seg2 as separating axis
//            float dotA1 = Vector2.Dot(D2, A1 - A2);
//            float dotB1 = Vector2.Dot(D2, B1 - A2);
//
//            // If A1 and B1 are either both positive or negative, perp1 is a separating axis
//            if (dotA1 * dotB1 > 0.0f)
//            {
//                intersection = new Vector2();
//                return false;
//            }
//
//            // Signed length of projection of seg1 onto seg2 normal
//            float length1 = dotA1 - dotB1;
//
//            // If length is non-zero, we know that:
//            // a) seg1 has length
//            // b) seg2 has length, or dotA1 and dotB1 would have been zero
//            // c) seg1 is skew to seg2, and therefore they are not colinear
//            if (length1 != 0.0f)
//            {
//                intersection = s1 + Vector2.Multiply(v1, dotA1 / length1);
//                return true;
//            }
//
//            intersection = new Vector2();
//            return false;
//        }

        public static bool LineIntersection(Vector2 start1, Vector2 vector1,
                                                  Vector2 start2, Vector2 vector2,
                                                  out Vector2 result)
        {
            Vector2 b = vector1;
            Vector2 d = vector2;

            float b_dot_d_perp = b.X * d.Y - b.Y * d.X;

            if (b_dot_d_perp == 0)
            {
                result = new Vector2();
                return false;
            }

            Vector2 c = start2 - start1;
            float t = (c.X * d.Y - c.Y * d.X) / b_dot_d_perp;

            result = new Vector2(start1.X + t * b.X, start1.Y + t * b.Y);
            return true;
        }

        public static bool LineIntersection(float sx1, float sy1, float vx1, float vy1,
                                                  float sx2, float sy2, float vx2, float vy2,
                                                  out Vector2 result)
        {
            float bx = vx1;
            float by = vy1;
            float dx = vx2;
            float dy = vy2;

            float b_dot_d_perp = bx * dy - by * dx;

            if (b_dot_d_perp == 0)
            {
                result = new Vector2();
                return false;
            }

            float cx = sx2 - sx1;
            float cy = sy2 - sy1;

            float t = (cx * dy - cy * dx) / b_dot_d_perp;

            result = new Vector2(sx1 + t * bx, sy1 + t * by);
            return true;
        }

        public static bool LineSegmentIntersection(float start1x, float start1y,
                                                   float end1x, float end1y,
                                                   float start2x, float start2y,
                                                   float end2x, float end2y,
                                                   out Vector2 result)
        {
            float bx = end1x - start1x;
            float by = end1y - start1y;
            float dx = end2x - start2x;
            float dy = end2y - start2y;

            float b_dot_d_perp = bx * dy - by * dx;

            if (b_dot_d_perp == 0)
            {
                result = new Vector2();
                return false;
            }

            float cx = start2x - start1x;
            float cy = start2y - start1y;

            float t = (cx * dy - cy * dx) / b_dot_d_perp;
            if (t < 0 || t > 1)
            {
                result = new Vector2();
                return false;
            }

            float u = (cx * by - cy * bx) / b_dot_d_perp;
            if (u < 0 || u > 1)
            {
                result = new Vector2();
                return false;
            }

            result = new Vector2(start1x + t * bx, start1y + t * by);
            return true;
        }

        /// <summary>
        /// http://www.processinghacks.com/hacks/detecting-line-to-line-intersection
        /// </summary>
        public static bool LineSegmentIntersection(Vector2 start1, Vector2 end1,
                                                   Vector2 start2, Vector2 end2,
                                                   out Vector2 result)
        {
            Vector2 b = end1 - start1;
            Vector2 d = end2 - start2;

            float b_dot_d_perp = b.X * d.Y - b.Y * d.X;

            if (b_dot_d_perp == 0)
            {
                result = new Vector2();
                return false;
            }

            Vector2 c = start2 - start1;

            float t = (c.X * d.Y - c.Y * d.X) / b_dot_d_perp;
            if (t < 0 || t > 1)
            {
                result = new Vector2();
                return false;
            }

            float u = (c.X * b.X - c.X * b.X) / b_dot_d_perp;
            if (u < 0 || u > 1)
            {
                result = new Vector2();
                return false;
            }

            result = new Vector2(start1.X + t * b.X, start1.Y + t * b.Y);
            return true;
        }
    }
}