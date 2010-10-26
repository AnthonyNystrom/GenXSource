using System;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace NuGenSVisualLib.Rendering.ThreeD
{
    /// <summary>
    /// Encapsulates an 3D axis entity
    /// </summary>
    public class Axis3D : IEntity
    {
        Device device;
        VertexBuffer vBuffer;

        #region IEntity Members

        public void Render()
        {
            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.SetStreamSource(0, vBuffer, 0);
            device.DrawPrimitives(PrimitiveType.LineList, 0, 3);
        }

        public void Init(Device device)
        {
            this.device = device;

            vBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), 6, device, Usage.None, CustomVertex.PositionColored.Format, Pool.Managed);
            CustomVertex.PositionColored[] verts = (CustomVertex.PositionColored[])vBuffer.Lock(0, LockFlags.None);

            verts[0].Color = verts[1].Color = Color.Red.ToArgb();
            verts[1].Position = new Vector3(1, 0, 0);

            verts[2].Color = verts[3].Color = Color.Green.ToArgb();
            verts[3].Position = new Vector3(0, 1, 0);

            verts[4].Color = verts[5].Color = Color.Blue.ToArgb();
            verts[5].Position = new Vector3(0, 0, 1);

            vBuffer.Unlock();
        }

        public BoundingBox BoundingBox
        {
            get { return null; }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            vBuffer.Dispose();
        }

        #endregion
    }

    public class Ray3D : IEntity
    {
        Device device;
        CustomVertex.PositionColored[] ray;
        Vector3 start, end;

        public Ray3D(Vector3 start, Vector3 dir, float length)
        {
            this.start = start;
            end = start + (dir * length);
        }

        #region IEntity Members

        public void Render()
        {
            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.DrawUserPrimitives(PrimitiveType.LineList, 1, ray);
        }

        public void Init(Device device)
        {
            this.device = device;
            ray = new CustomVertex.PositionColored[2];
            ray[0] = new CustomVertex.PositionColored(start, Color.Red.ToArgb());
            ray[1] = new CustomVertex.PositionColored(end, Color.Red.ToArgb());
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