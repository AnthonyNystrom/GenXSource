using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using NuGenSVisualLib.Recording;

namespace NuGenSVisualLib.Movement.ThreeD
{
    /// <summary>
    /// Encapsulates the translation of user input into 3D camera movement
    /// </summary>
    class MovementControlHandler3D : IControlHandler
    {
        protected bool enabled;
        protected PointF mouseDown = new PointF(-1, -1);
        protected PointF mouseDelta = new PointF(-1, -1);
        protected Matrix rotation = Matrix.Identity;
        protected ViewRecording.ViewRecorder recorder;

        protected static readonly float smoothingFactor = 0.3f;

        protected float zoomLevel;

        double xRot, yRot;
        Vector3 wAxis;

        public Matrix Rotation3D
        {
            get { return rotation; }
        }

        public float ZoomLevel
        {
            get { return zoomLevel; }
        }

        #region IControlHandler Members

        public bool OnMouseMove(MouseEventArgs e)
        {
            // process mouse movement into scene rotation
            if (e.Button != MouseButtons.Right)
            {
                if (mouseDown.X != -1)
                {
                    mouseDelta.X = (mouseDelta.X * (1 - smoothingFactor)) + (smoothingFactor * (mouseDown.X - e.X));
                    mouseDelta.Y = (mouseDelta.Y * (1 - smoothingFactor)) + (smoothingFactor * (mouseDown.Y - e.Y));

                    float rx = 0, ry = 0;

                    if (mouseDelta.X != 0)
                    {
                        rx = 0.5f * (mouseDelta.X * (float)Math.PI) / 180f;
                        rotation.Multiply(Matrix.RotationY(rx));
                    }
                    if (mouseDelta.Y != 0)
                    {
                        ry = -0.5f * (mouseDelta.Y * (float)Math.PI) / 180f;
                        rotation.Multiply(Matrix.RotationZ(ry));
                    }

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

                    return true;
                }
            }
            return false;
        }

        public bool OnMouseDown(MouseEventArgs e)
        {
            mouseDown.X = e.X;
            mouseDown.Y = e.Y;
            mouseDelta.X = 0;
            mouseDelta.Y = 0;

            return false;
        }

        public bool OnMouseUp(MouseEventArgs e)
        {
            mouseDown.X = mouseDown.Y = -1;
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

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public bool OnMouseWheel(MouseEventArgs e)
        {
            // zoom
            if (e.Delta != 0 && (zoomLevel > -1f || e.Delta < 0))
            {
                float movement = ((float)-e.Delta / 360f) * 0.2f;
                zoomLevel += movement;
                if (zoomLevel < -1f)
                    zoomLevel = -1f;
                return true;
            }
            return false;
        }

        public ViewRecording.ViewRecorder Recorder
        {
            get { return recorder; }
            set
            {
                recorder = value;
                if (recorder != null)
                    recorder.startRotation = Matrix.Multiply(rotation, Matrix.Identity);
            }
        }
        #endregion
    }
}