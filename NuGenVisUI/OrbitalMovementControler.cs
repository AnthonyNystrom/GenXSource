using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;

namespace NuGenVisUI
{
    class OrbitalMovementControler : IViewInputController
    {
        private bool enabled;
        protected PointF mouseDown = new PointF(-1, -1);
        protected PointF mouseDelta = new PointF(-1, -1);
        protected Matrix rotation = Matrix.Identity;
//        protected ViewRecording.ViewRecorder recorder;

        protected static readonly float smoothingFactor = 0.3f;

        protected float zoomLevel;

//        double xRot, yRot;
//        Vector3 wAxis;

        bool xLocked, yLocked, zLocked;

        public Matrix Rotation3D
        {
            get { return rotation; }
        }

        public float ZoomLevel
        {
            get { return zoomLevel; }
        }

        #region IViewInputController Members

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public bool OnMouseMove(MouseEventArgs e)
        {
            // process mouse movement into scene rotation
            if (e.Button == MouseButtons.Right)
            {
                if (mouseDown.X != -1)
                {
                    Matrix xRot = Matrix.Identity, zRot = Matrix.Identity;
                    if (!xLocked)
                    {
                        mouseDelta.X = (mouseDelta.X * (1 - smoothingFactor)) + (smoothingFactor * (mouseDown.X - e.X));
                        if (mouseDelta.X != 0)
                        {
                            float rx = 0.5f * (mouseDelta.X * (float)Math.PI) / 180f;
                            //rotation.Multiply(Matrix.RotationY(rx));
                            xRot = Matrix.RotationY(rx);
                        }
                    }
                    if (!zLocked)
                    {
                        mouseDelta.Y = (mouseDelta.Y * (1 - smoothingFactor)) + (smoothingFactor * (mouseDown.Y - e.Y));
                        if (mouseDelta.Y != 0)
                        {
                            float ry = -0.5f * (mouseDelta.Y * (float)Math.PI) / 180f;
                            //rotation.Multiply(Matrix.RotationZ(ry));
                            zRot = Matrix.RotationZ(ry);
                        }
                    }

                    rotation *= zRot * xRot;

                    /*if (recorder != null && (rx != 0 || ry != 0))
                        recorder.RecordRotation(rx, ry, 0f);

                    xRot += mouseDelta.X * 0.005f;
                    yRot += mouseDelta.Y * 0.005f;

                    rotation = Matrix.RotationY((float)((Math.PI * 2) * xRot));

                    //Vector4 temp = Vector3.Transform(new Vector3(1, 0, 0), rotation);
                    wAxis = new Vector3(0, 0, 1);//temp.X, temp.Y, temp.Z);

                    rotation *= Matrix.RotationAxis(wAxis, (float)((Math.PI * 2) * yRot));*/

                    mouseDown.X = e.X;
                    mouseDown.Y = e.Y;
                }
                return true;
            }
            return false;
        }

        public bool OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                mouseDown.X = e.X;
                mouseDown.Y = e.Y;
                mouseDelta.X = 0;
                mouseDelta.Y = 0;
                return true;
            }
            return false;
        }

        public bool OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                mouseDown.X = mouseDown.Y = -1;
                return true;
            }
            return false;
        }

        public bool OnKeyDown(KeyEventArgs e)
        {
            return false;
        }

        public bool OnKeyUp(KeyEventArgs e)
        {
            return false;
        }

        public bool OnMouseWheel(MouseEventArgs e)
        {
            // zoom
            if (e.Delta != 0 && (zoomLevel > -1f || e.Delta < 0))
            {
                float movement = (-e.Delta / 360f) * 0.2f;
                zoomLevel += movement;
                if (zoomLevel < -1f)
                    zoomLevel = -1f;
                return true;
            }
            return false;
        }

        public void LockXAxis(bool _lock)
        {
            xLocked = _lock;
        }

        public void LockYAxis(bool _lock)
        {
            yLocked = _lock;
        }

        public void LockZAxis(bool _lock)
        {
            zLocked = _lock;
        }
        #endregion
    }
}