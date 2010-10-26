using System;
using Microsoft.DirectX;

namespace Genetibase.VisUI.Rendering
{
    public abstract class RenderingView3D
    {
        protected Matrix world, view, projection;
        protected Vector3 scroll, rotation;
        protected float near, far, centre;
        protected float zoom;
        protected bool modified;

        public RenderingView3D(Matrix world, Matrix view, Matrix proj)
        {
            this.world = world;
            this.view = view;
            projection = proj;
            zoom = 0;
        }

        public Matrix WorldMatrix
        {
            get { return world; }
            set { world = value; }
        }

        public Matrix ViewMatrix
        {
            get { return view; }
            set { view = value; }
        }

        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; }
        }

        public Vector3 Scroll
        {
            get { return scroll; }
            //set { scroll = value; }
        }

        public Vector3 Rotation
        {
            get { return rotation; }
            //set { rotation = value; }
        }

        public float Zoom
        {
            get { return zoom; }
            set { float actualZoom = value * centre; if (actualZoom > -centre - 0.1f) { zoom = actualZoom; modified = true; } }
        }

        public float Near
        {
            get { return near; }
            set { near = value; }
        }

        public float Far
        {
            get { return far; }
            set { far = value; }
        }

        //        public abstract RenderingView3D FromSource(RenderingSource3D source);
        public static RenderingView3D FromSource(RenderingSource3D source)
        {
            // set view to see all scene with current FOV
            float radius = source.Bounds.radius * 1.15f;
            float distance = (float)(radius / (Math.Tan(Math.PI / 4) * 0.5));// +zoom;
            Vector3 cameraDir = Vector3.Normalize(source.Bounds.max);
            Vector3 CameraPos = (cameraDir * distance);

            Matrix view = Matrix.LookAtLH(CameraPos, new Vector3()/*source.Origin*/, new Vector3(0, 1, 0));

            Matrix world = Matrix.Translation(-source.Origin);

            RenderingView3DPer per = new RenderingView3DPer(view, world);
            per.CameraPos = CameraPos;
            per.near = distance - radius;
            if (per.near <= 0)
                per.near = 0.01f;
            per.far = distance + radius;
            per.centre = distance;
            return per;
        }

        public abstract void SetupProjection(int width, int height);
        public abstract void UpdateView(RenderingSource3D source);
    }

    public class RenderingView3DPer : RenderingView3D
    {
        public Vector3 CameraPos;
        int width, height;

        public RenderingView3DPer(Matrix view, Matrix world)
            : base(world, view, Matrix.Identity)
        { }

        public override void SetupProjection(int width, int height)
        {
            this.width = width;
            this.height = height;
            projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, width / (float)height,
                                                 near, far);
        }

        public override void UpdateView(RenderingSource3D source)
        {
            // work out new position from new zoom level
            Vector3 cameraDir = Vector3.Normalize(CameraPos);
            CameraPos = (cameraDir * centre) + (cameraDir * zoom);

            float distance = CameraPos.Length();
            float radius = source.Bounds.radius * 1.15f;

            near = distance - radius;
            if (near <= 0)
                near = 0.01f;
            far = distance + radius;

            view = Matrix.LookAtLH(CameraPos, new Vector3()/*source.Origin*/, new Vector3(0, 1, 0));
            SetupProjection(width, height);
        }
    }
}