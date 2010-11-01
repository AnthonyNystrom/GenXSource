namespace NuGenRenderOptics
{
    class Ray
    {
        protected Vector3D origin;
        protected Vector3D directionUV;
        protected double intensity;
        protected double length;
        protected double maxLength;

        protected int level = 0;
        public ISceneManager scene;

        public Ray(Vector3D origin, Vector3D directionUV, double intensity,
                   double length, double maxLength, ISceneManager scene)
        {
            this.origin = origin;
            this.directionUV = directionUV;
            this.intensity = intensity;
            this.length = length;
            this.maxLength = maxLength;
            this.scene = scene;
        }

        #region Properties

        public Vector3D Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Vector3D DirectionUV
        {
            get { return directionUV; }
            set { directionUV = value; }
        }

        public double Intensity
        {
            get { return intensity; }
            set { intensity = value; }
        }

        public double Length
        {
            get { return length; }
            set { length = value; }
        }

        public double MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }
        #endregion

        public RGBA_D Trace(Ray ray)
        {
            level++;
            RGBA_D point_color = RGBA_D.Empty;
            if (level > 3)
            {
                level--;
                return point_color;
            }
            IOpticalSceneObject obj;
            Vector3D iPos;
            double iDist;
            uint subIdx;
            if (scene.GetFirstIntersection(ray.origin, ray.directionUV, ray.maxLength - ray.length,
                                           out obj, out iPos, out iDist, out subIdx))
            {
                // reduce ray length
                ray.length -= iDist;
                // shade
                Ray reflection, refraction;
                point_color = obj.Shade(ray, iPos, subIdx, out reflection, out refraction, scene);
                // do any refraction
                if (reflection != null)
                {
                    double rIntensity = 1 - ray.intensity;
                    RGBA_D rflClr = reflection.Trace(reflection);
                    if (rflClr != RGBA_D.Empty)
                    {
                        // blend colour
                        double alpha = rflClr.A / 255;
                        double ra = 1 - alpha;
                        rflClr.B = (byte)((0 * ra) + (rflClr.B * alpha)) * rIntensity;
                        rflClr.G = (byte)((0 * ra) + (rflClr.G * alpha)) * rIntensity;
                        rflClr.R = (byte)((0 * ra) + (rflClr.R * alpha)) * rIntensity;
                        // combine colours
                        point_color.R = (point_color.R + rflClr.R) / 2;
                        point_color.G = (point_color.G + rflClr.G) / 2;
                        point_color.B = (point_color.B + rflClr.B) / 2;
                    }
                    if (refraction != null)
                    {
                        RGBA_D rfcClr = refraction.Trace(refraction);
                        if (rfcClr != RGBA_D.Empty)
                        {
                            // combine colours
                            point_color.R = (3 * point_color.R + rflClr.R + 5 * rfcClr.R) / 9;
                            point_color.G = (3 * point_color.G + rflClr.G + 5 * rfcClr.G) / 9;
                            point_color.B = (3 * point_color.B + rflClr.B + 5 * rfcClr.B) / 9;
                        }
                    }
                }
                if (refraction != null)
                {
                    RGBA_D rfcClr = refraction.Trace(refraction);
                    if (rfcClr != RGBA_D.Empty)
                    {
                        // combine colours
                        point_color.R = (3 * point_color.R + 5 * rfcClr.R) / 8;
                        point_color.G = (3 * point_color.G + 5 * rfcClr.G) / 8;
                        point_color.B = (3 * point_color.B + 5 * rfcClr.B) / 8;
                    }
                    if (reflection != null)
                    {
                        RGBA_D rflClr = reflection.Trace(reflection);
                        if (rflClr != RGBA_D.Empty)
                        {
                            // combine colours
                            point_color.R = (3 * point_color.R + rflClr.R + 5 * rfcClr.R) / 9;
                            point_color.G = (3 * point_color.G + rflClr.G + 5 * rfcClr.G) / 9;
                            point_color.B = (3 * point_color.B + rflClr.B + 5 * rfcClr.B) / 9;
                        }
                    }
                }
            }
            level--;
            return point_color;
        }

        public bool TraceRayToLight(Vector3D startPoint, Vector3D lightPoint)
        {
            // just do this simply for now

            Vector3D lv = startPoint - lightPoint;
            lv.Normalize();

            IOpticalSceneObject obj;
            Vector3D iPos;
            double iDist;
            uint subIdx;
            if (scene.GetFirstIntersection(startPoint, lv, 100, out obj, out iPos, out iDist, out subIdx))
            {
                return false;
            }
            return true;
        }
    }
}
