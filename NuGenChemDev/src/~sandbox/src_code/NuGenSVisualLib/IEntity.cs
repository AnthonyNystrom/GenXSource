using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace NuGenSVisualLib.Rendering
{
    public class BoundingBox
    {
        //protected Vector3[] points;
        protected Vector3 dimensions;
        protected Vector3 centre;
        protected Vector3 position;
        protected Vector3 extent;

        public BoundingBox(float[] points)
        {
            //this.points = points;

            dimensions = new Vector3();
            dimensions.X = points[1] - points[0];
            dimensions.Y = points[3] - points[2];
            dimensions.Z = points[5] - points[4];

            centre = new Vector3();
            centre.X = points[0] + (dimensions.X / 2.0f);
            centre.Y = points[2] + (dimensions.Y / 2.0f);
            centre.Z = points[4] + (dimensions.Z / 2.0f);
        }

        public BoundingBox(Vector3 min, Vector3 max)
        {
            /*min *= 1.3f;
            max *= 1.3f;*/

            position = min;
            extent = max - min;

            dimensions = extent * 0.5f;
            centre = position + dimensions;
        }

        public BoundingBox()
        { }

        public Vector3 Centre
        {
            get { return centre; }
            set { centre = value; }
        }

        public Vector3 Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector3 Extent
        {
            get { return extent; }
            set { extent = value; }
        }
    }

    public interface IEntity : IDisposable
    {
        void Render();
        void Init(Device device);

        BoundingBox BoundingBox { get; }
    }

    public interface IScreenSpaceEntity : IDisposable
    {
        // TODO: Need to have local atom & bond entities to tie other things to - that can be related/index geometry buffers
        void Init(Device device);
        void Update(Matrix world, Matrix view, Matrix proj);
        void Render();
    }
}