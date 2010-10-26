using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Drawing;

namespace NuGenSVisualLib.Rendering.ThreeD
{
    class SphereAxis3D : IEntity
    {
        Device device;
        VertexBuffer axis1, axis2, axis3;
        VertexBuffer marks1, marks2, marks3;
        int orbitLineSz;
        float scale;

        private void CreateAxisSphere(ref VertexBuffer vBuffer, Vector3 dir, int clr)
        {
            double angle = Math.PI * ((360 / (double)orbitLineSz) / 180);
            vBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), orbitLineSz + 1, device, Usage.None,
                                       CustomVertex.PositionOnly.Format, Pool.Managed);
            CustomVertex.PositionColored[] verts = (CustomVertex.PositionColored[])vBuffer.Lock(0, LockFlags.None);

            double currentAngle = 0;
            int vIdx = 0;
            if (dir.X > 0)
            {
                for (int i = 0; i < orbitLineSz + 1; i++)
                {
                    float x = scale * (float)Math.Cos(currentAngle);
                    float y = scale * (float)Math.Sin(currentAngle);

                    verts[vIdx].Color = clr;
                    verts[vIdx++].Position = new Vector3(0, x, y);

                    currentAngle += angle;
                }
            }
            else if (dir.Y > 0)
            {
                for (int i = 0; i < orbitLineSz + 1; i++)
                {
                    float x = scale * (float)Math.Cos(currentAngle);
                    float y = scale * (float)Math.Sin(currentAngle);

                    verts[vIdx].Color = clr;
                    verts[vIdx++].Position = new Vector3(x, 0, y);

                    currentAngle += angle;
                }
            }
            else if (dir.Z > 0)
            {
                for (int i = 0; i < orbitLineSz + 1; i++)
                {
                    float x = scale * (float)Math.Cos(currentAngle);
                    float y = scale * (float)Math.Sin(currentAngle);

                    verts[vIdx].Color = clr;
                    verts[vIdx++].Position = new Vector3(x, y, 0);

                    currentAngle += angle;
                }
            }

            vBuffer.Unlock();
        }

        private void CreateMarks(ref VertexBuffer vBuffer, Vector3 dir, int clr)
        {
            double angle = Math.PI * ((360 / (double)8) / 180);
            vBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), 8 * 2, device, Usage.None,
                                       CustomVertex.PositionOnly.Format, Pool.Managed);
            CustomVertex.PositionColored[] verts = (CustomVertex.PositionColored[])vBuffer.Lock(0, LockFlags.None);

            double currentAngle = 0;
            int vIdx = 0;
            float scale2 = scale * 0.8f;
            if (dir.X > 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    float x = scale * (float)Math.Cos(currentAngle);
                    float y = scale * (float)Math.Sin(currentAngle);

                    verts[vIdx].Color = clr;
                    verts[vIdx++].Position = new Vector3(0, x, y);

                    x = scale2 * (float)Math.Cos(currentAngle);
                    y = scale2 * (float)Math.Sin(currentAngle);

                    verts[vIdx].Color = clr;
                    verts[vIdx++].Position = new Vector3(0, x, y);

                    currentAngle += angle;
                }
            }
            else if (dir.Y > 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    float x = scale * (float)Math.Cos(currentAngle);
                    float y = scale * (float)Math.Sin(currentAngle);

                    verts[vIdx].Color = clr;
                    verts[vIdx++].Position = new Vector3(x, 0, y);

                    x = scale2 * (float)Math.Cos(currentAngle);
                    y = scale2 * (float)Math.Sin(currentAngle);

                    verts[vIdx].Color = clr;
                    verts[vIdx++].Position = new Vector3(x, 0, y);

                    currentAngle += angle;
                }
            }
            else if (dir.Z > 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    float x = scale * (float)Math.Cos(currentAngle);
                    float y = scale * (float)Math.Sin(currentAngle);

                    verts[vIdx].Color = clr;
                    verts[vIdx++].Position = new Vector3(x, y, 0);

                    x = scale2 * (float)Math.Cos(currentAngle);
                    y = scale2 * (float)Math.Sin(currentAngle);

                    verts[vIdx].Color = clr;
                    verts[vIdx++].Position = new Vector3(x, y, 0);

                    currentAngle += angle;
                }
            }

            vBuffer.Unlock();
        }

        #region IEntity Members

        public void Render()
        {
            device.BeginScene();

            device.SetStreamSource(0, axis1, 0);
            device.Indices = null;
            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.DrawPrimitives(PrimitiveType.LineStrip, 0, orbitLineSz);

            device.SetStreamSource(0, marks1, 0);
            device.DrawPrimitives(PrimitiveType.LineList, 0, 8);

            device.SetStreamSource(0, axis2, 0);
            device.DrawPrimitives(PrimitiveType.LineStrip, 0, orbitLineSz);

            device.SetStreamSource(0, marks2, 0);
            device.DrawPrimitives(PrimitiveType.LineList, 0, 8);

            //device.SetStreamSource(0, axis3, 0);
            //device.DrawPrimitives(PrimitiveType.LineStrip, 0, orbitLineSz);

            device.EndScene();
        }

        public void Init(Device device)
        {
            this.device = device;

            // create sphere axis
            orbitLineSz = 30;
            scale = 2;
            CreateAxisSphere(ref axis1, new Vector3(1, 0, 0), Color.Red.ToArgb());
            CreateMarks(ref marks1, new Vector3(1, 0, 0), Color.Red.ToArgb());
            CreateAxisSphere(ref axis2, new Vector3(0, 1, 0), Color.Green.ToArgb());
            CreateMarks(ref marks2, new Vector3(0, 1, 0), Color.Green.ToArgb());

            //CreateAxisSphere(ref axis3, new Vector3(0, 0, 1), Color.Blue.ToArgb());
        }

        public BoundingBox BoundingBox
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
