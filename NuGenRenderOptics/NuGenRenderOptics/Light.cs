using System.Drawing;

namespace NuGenRenderOptics
{
    abstract class Light
    {
        protected Color clr;
        protected Vector3D position;
        protected double intensity;
        protected Vector3D clr3D;

        public Light(Color clr, Vector3D position, double intensity)
        {
            this.clr = clr;
            this.position = position;
            this.intensity = intensity;
            clr3D = new Vector3D((double)clr.R / 255, (double)clr.G / 255, (double)clr.B / 255);
        }

        #region Properties

        public Color Clr
        {
            get { return clr; }
        }

        public Vector3D Clr3D
        {
            get { return clr3D; }
        }

        public Vector3D Position
        {
            get { return position; }
        }

        public double Intensity
        {
            get { return intensity; }
        }
        #endregion

        public abstract double LuminosityForPoint(Vector3D point);
    }

    class DirectionalLight : Light
    {
        public DirectionalLight(Color clr, Vector3D position, double intensity)
            : base(clr, position, intensity)
        {
        }

        public override double LuminosityForPoint(Vector3D point)
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <see cref="http://www.ogre3d.org/wiki/index.php/Point_Light_Attenuation"/>
    class PointLight : Light
    {
        double range, linear, quadratic;

        public PointLight(Color clr, Vector3D position, double constant, double range, double linear, double quadratic)
            : base(clr, position, 1 / constant)
        {
            this.range = range;
            this.linear = linear;
            this.quadratic = quadratic;
        }

        public override double LuminosityForPoint(Vector3D point)
        {
            double distance = (position - point).Length();
            if (distance > range)
                return 0;
            double attenuation = intensity + linear * distance + quadratic * (distance * distance);
            return 1 / attenuation;
        }
    }
}