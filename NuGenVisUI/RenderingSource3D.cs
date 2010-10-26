using Microsoft.DirectX;

namespace Genetibase.VisUI.Rendering
{
    public class Bounds3D
    {
        public Vector3 min, max;
        public float radius;

        public Bounds3D() { }

        public Bounds3D(Vector3 min, Vector3 max, float radius)
        {
            this.min = min;
            this.max = max;
            this.radius = radius;
        }
    }

    public abstract class RenderingSource3D
    {
        protected Bounds3D bounds;
        protected Vector3 origin;

        public Bounds3D Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        public Vector3 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public abstract void SourceModified();
    }
}