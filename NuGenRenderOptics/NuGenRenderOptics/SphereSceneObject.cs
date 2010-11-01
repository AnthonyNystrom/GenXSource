using System;

namespace NuGenRenderOptics
{
    abstract class OpticalSceneObject : IOpticalSceneObject
    {
        protected Vector3D origin;
        private MaterialShader shader;
        protected double radius;

        public OpticalSceneObject(Vector3D origin, MaterialShader shader, double radius)
        {
            this.origin = origin;
            this.shader = shader;
            this.radius = radius;
        }

        #region IOpticalSceneObject Members

        public Vector3D Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public MaterialShader Shader
        {
            get { return shader; }
            set { shader = value; }
        }

        public double Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public RGBA_D Shade(Ray ray, Vector3D pos, uint subIdx, out Ray reflection, out Ray refraction,
                            ISceneManager scene)
        {
            if (shader != null)
                return shader.Shade(ray, pos, subIdx, this, scene, out reflection, out refraction);
            reflection = null;
            refraction = null;
            return RGBA_D.Empty;
        }

        public abstract bool GetIntersect(Vector3D p1, Vector3D p2, Vector3D uv, out Vector3D iPos, out double iDist, out uint subIdx);
        public abstract Vector3D GetNormal(Vector3D pos, uint subIdx);
        public abstract Vector2D GetTexCoord(Vector3D p, uint subIdx);
        #endregion
    }

    class SphereSceneObject : OpticalSceneObject
    {
        public SphereSceneObject(Vector3D origin, MaterialShader shader, double radius)
            : base(origin, shader, radius)
        {
        }

        public override Vector3D GetNormal(Vector3D pos, uint subIdx)
        {
            return Vector3D.Normalize(new Vector3D(pos.X - origin.X, pos.Y - origin.Y, pos.Z - origin.Z));
        }

        public override bool GetIntersect(Vector3D p1, Vector3D p2, Vector3D uv, out Vector3D iPos,
                                          out double iDist, out uint subIdx)
        {
            subIdx = 0;
            if (IntersectRaySphere(p1, uv, origin, radius * radius, out iDist))
            {
                iPos = p1 - (iDist * uv);
                return true;
            }

            iPos = Vector3D.Empty;
            return false;
            /*subIdx = 0;

            Vector3D dst = p1 - origin;
            double B = Vector3D.Dot(dst, uv);
            double C = Vector3D.Dot(dst, dst) - (radius * radius);
            double D = B * B - C;
            if (D > 0)
            {
                iDist = -B - Math.Sqrt(D);
                iPos = p1 + (uv * iDist);
                return true;
            }
            iDist = -1;
            iPos = Vector3D.Empty;
            return false;*/

            // x-xo 2 + y-yo 2 + z-zo 2 = r 2
            // x,y,z = p+tv 
            // At2 + Bt + C = 0
            /*double vx = p2.X - p1.X;
            double vy = p2.Y - p1.Y;
            double vz = p2.Z - p1.Z;

            double A = (vx * vx + vy * vy + vz * vz);
            double B = 2.0 * (p1.X * vx + p1.Y * vy +
                       p1.Z * vz - vx * origin.X - vy * origin.Y - vz * origin.Z);
            double C = p1.X * p1.X - 2 * p1.X * origin.X + origin.X * origin.X +
                       p1.Y * p1.Y - 2 * p1.Y * origin.Y + origin.Y * origin.Y +
                       p1.Z * p1.Z - 2 * p1.Z * origin.Z + origin.Z * origin.Z -
                       radius * radius;
            double D = B * B - 4 * A * C;
            iDist = -1.0;
            subIdx = 0;

            if (D >= 0)
            {
                double t1 = (-B - Math.Sqrt(D)) / (2.0 * A);
                double t2 = (-B + Math.Sqrt(D)) / (2.0 * A);
                if (t1 < t2)
                    iDist = t1;
                else
                    iDist = t2;

                Vector3D hLen = new Vector3D(iDist * vx, iDist * vy, iDist * vz);
                iDist = hLen.Length();
                iPos = p1 + hLen;//new Vector3D(p1.X + (iDist * vx), p1.Y + (iDist * vy), p1.Z + (iDist * vz));
                return true;
            }
            iPos = Vector3D.Empty;
            return false;*/
        }

        public static bool IntersectRaySphere(Vector3D rOrigin, Vector3D rUV, Vector3D sOrigin,
                                       double radius2, out double distance)
        {
            Vector3D oc = rOrigin - sOrigin;
            double l20c = Vector3D.Dot(oc, oc);
            if (l20c < radius2)
            {
                // starts inside of the sphere
                double tca = Vector3D.Dot(oc, rUV) / Vector3D.Dot(rUV, rUV);			// omit division if ray.d is normalized
                double l2hc = (radius2 - l20c) / Vector3D.Dot(rUV, rUV) + tca * tca;  // division
                distance = tca + Math.Sqrt(l2hc);
                return true;
            }
            else
            {
                double tca = Vector3D.Dot(oc, rUV);
                if (tca < 0) // points away from the sphere
                {
                    distance = double.NaN;
                    return false;
                }
                double l2hc = (radius2 - l20c) / Vector3D.Dot(rUV, rUV) + (tca * tca);	// division
                if (l2hc > 0)
                {
                    distance = tca - Math.Sqrt(l2hc);
                    double real = oc.Length();
                    return true;
                }
                distance = double.NaN;
                return false;
            }
        }

//        public static double GetCoord(double i1, double i2, double w1, double w2, double p)
//        {
//            return ((p - i1) / (i2 - i1)) * (w2 - w1) + w1;
//        }

        public override Vector2D GetTexCoord(Vector3D p, uint subIdx)
        {
            p = p - origin;

            Vector3D.RotX(1.5, ref p.Y, ref p.Z);
            Vector3D.RotZ(-2.5, ref p.X, ref p.Y);

            double phi = Math.Acos(p.Z / radius);
            double S = Math.Sqrt(p.X * p.X + p.Y * p.Y);
            double theta;

            if (p.X > 0)
                theta = Math.Asin(p.Y / S);//Atan(py/px);
            else
                theta = Math.PI - Math.Asin(p.Y / S);// Math.Atan(py / px);

            if (theta < 0)
                theta = 2.0 * Math.PI + theta;

            double x1 = Vector3D.GetCoord(0.0, Math.PI * 2.0,
                           0.0, 1/*imgBitmap.Width - 1*/, theta);
            double y1 = Vector3D.GetCoord(0.0, Math.PI, 0.0,
                              1/*imgBitmap.Height - 1*/, phi);
            // NOTE: Do clamping etc. elsewhere
            return new Vector2D(x1, y1);

            /*int i1 = (int)x1, j1 = (int)y1;
            if (i1 >= 0 && j1 >= 0 && i1 < imgBitmap.Width &&
                           j1 < imgBitmap.Height)
            {
                Color clr = imgBitmap.GetPixel(i1, j1);
                color.x = clr.R;
                color.y = clr.G;
                color.z = clr.B;
            }*/
        }
    }
}