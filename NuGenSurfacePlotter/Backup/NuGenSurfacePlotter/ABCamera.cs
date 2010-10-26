using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace NuGenCRBase.AvalonBridge
{
    class ABCamera
    {
        protected Vector3 position;
        protected Vector3 target;

        public Vector3 Position
        {
            get { return position; }
        }

        public Vector3 Target
        {
            get { return target; }
        }

        public virtual void Scroll(Vector3 aPosition)
        {
            // simply move the camera position
            position = aPosition;
        }
    }

    class ABCameraSpherical : ABCamera
    {
        private float sphereRadius;

        public ABCameraSpherical(float sphereRadius, Vector3 target)
        {
            this.sphereRadius = sphereRadius;
            this.target = target;
        }

        public override void Scroll(Vector3 aPosition)
        {
            // calculate position on sphere surface
            float xRad = (float)(aPosition.X * Math.PI * 2f);
            float yRad = (float)(aPosition.Y * Math.PI * 2f);
            float zRad = (float)(aPosition.Y * Math.PI * 2f);

            float x = (float)Math.Cos(xRad) * sphereRadius;
            float z = (float)Math.Sin(xRad) * sphereRadius;
            float y = (float)Math.Sin(yRad) * sphereRadius;

            position = new Vector3(x, y, z);
        }
    }
}