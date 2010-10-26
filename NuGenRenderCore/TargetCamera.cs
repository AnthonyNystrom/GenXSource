using System;
using System.Drawing;
using Microsoft.DirectX;

namespace Genetibase.NuGenRenderCore.View
{
    public class TargetCamera : Camera
    {
        Vector3 target;

        public TargetCamera(Size viewSize, Vector3 position,
                            Vector3 target, float fov)
            : base(viewSize, position, fov, true)
        {
            this.target = target;
        }

        private void CalcView()
        {
            view = Matrix.LookAtLH(position, target, new Vector3(0, 1, 0));
        }

        #region Properties

        public Vector3 Target
        {
            get { return target; }
            set { target = value; CalcView(); }
        }

        public override IViewInputHandler DefaultHandler
        {
            get { return new SphericalRotationHandler(viewSize, this, 100); }
        }

        #endregion

        #region Camera Members

        public override void Update(bool updProj, bool updView, bool updWorld)
        {
            if (updProj)
                CalcProjMat();
            if (updView)
                CalcView();
            if (updWorld)
                world = Matrix.Identity;
        }
        #endregion
    }

    public class SphericalRotationHandler : ViewInputHandler
    {
        float radius;

        public SphericalRotationHandler(Size viewSize, TargetCamera camera, float radius)
            : base(viewSize, camera)
        {
            this.radius = radius;
        }

        public override void MoveMouse(Point point)
        {
            currentMouseMove.X += point.X;
            currentMouseMove.Y += point.Y;
            float xPer = (float)currentMouseMove.X / viewSize.Width;
            float yPer = (float)currentMouseMove.Y / viewSize.Height;

            TargetCamera tCam = (TargetCamera)camera;

            // calculate position on sphere surface
            float xRad = (float)(xPer * Math.PI * 2f);
            float yRad = (float)(yPer * Math.PI);
            //float zRad = (float)(point.Y * Math.PI * 2f);

            float x = (float)Math.Cos(xRad) * radius;
            float z = (float)Math.Sin(xRad) * radius;
            float y = (float)Math.Sin(yRad) * radius;

            tCam.Position = tCam.Target + new Vector3(x, y, z);
        }

        public override void MouseWheelScroll(float scroll)
        {
            throw new System.NotImplementedException();
        }

        public float Radius
        {
            get { return radius; }
            set { radius = value; MoveMouse(new Point()); }
        }
    }
}