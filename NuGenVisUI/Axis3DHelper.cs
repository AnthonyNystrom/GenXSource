using System.Drawing;
using Genetibase.NuGenRenderCore.Rendering;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Scene;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.VisUI.Entities.Helpers
{
    /// <summary>
    /// Encapsulates an 3D axis entity
    /// </summary>
    public class Axis3DHelper : SceneEntity
    {
        VertexBuffer vBuffer;
        ScreenSpaceText[] labels;
        Plane[] axisPlanes;

        static readonly float Tolerance;

        VertexBuffer selectionVBuffer;
        //IndexBuffer selectionIBuffer;

        public Axis3DHelper(Vector3 position)
            : base(new Vector3(), new Vector3(1, 1, 1))
        {
            Move(position);

            // setup planes
            axisPlanes = new Plane[3];
            axisPlanes[0] = Plane.FromPointNormal(new Vector3(), new Vector3(1, 0, 0));
            axisPlanes[1] = Plane.FromPointNormal(new Vector3(), new Vector3(0, 1, 0));
            axisPlanes[2] = Plane.FromPointNormal(new Vector3(), new Vector3(0, 0, 1));
        }

        public override int InternalProbe(Vector3 rayOrigin, Vector3 rayDir)
        {
            // intersect with planes first to get order
            Vector3 raySegment = rayDir * 1000;
            Vector3 xInt = Plane.IntersectLine(axisPlanes[0], rayOrigin, raySegment);
            Vector3 yInt = Plane.IntersectLine(axisPlanes[1], rayOrigin, raySegment);
            Vector3 zInt = Plane.IntersectLine(axisPlanes[2], rayOrigin, raySegment);

            // order
            int[] order = new int[3];
            float[] distances = new float[3];
            distances[0] = (xInt - rayOrigin).Length();
            distances[1] = (yInt - rayOrigin).Length();
            distances[2] = (zInt - rayOrigin).Length();

            for (int i = 0; i < 3; i++)
            {
                int pos = 0;
                int shift = 0;
                for (int i2 = 0; i2 < 3; i2++)
                {
                    if (i != i2)
                    {
                        if (distances[i] > distances[i2])
                            pos++;
                        // calc shift based on equal count
                        else if (distances[i] == distances[i2] && i2 < i)
                            shift++;
                    }
                }
                order[pos + shift] = i;
            }

            // check segment intersection by plane order
            if (CheckAxisIntersection(order[0], xInt, yInt, zInt))
                return 1;
            else if (CheckAxisIntersection(order[1], xInt, yInt, zInt))
                return 2;
            else if (CheckAxisIntersection(order[2], xInt, yInt, zInt))
                return 3;

            return -1;
        }

        private static bool CheckAxisIntersection(int index, Vector3 xInt, Vector3 yInt, Vector3 zInt)
        {
            // TODO: Use tolerance based on camera location etc.

            // check to see if point is near to the meet of 2 planes
            if (index == 0)
            {
                // hit x axis so check y and z
                return CheckAxis(xInt.X, xInt.Z, xInt.Y);
            }
            else if (index == 1)
            {
                // hit y axis so check x and z
                return CheckAxis(yInt.Y, yInt.X, yInt.Z);
            }
            else if (index == 2)
            {
                // hit z axis so check x and y
                return CheckAxis(zInt.Z, zInt.Y, zInt.X);
            }
            return false;
        }

        private static bool CheckAxis(float x, float z, float y)
        {
            if (x > -Tolerance && x < Tolerance)
            {
                if (z > -Tolerance && z < 1 + Tolerance)
                {
                    return true;
                }
                else if (y > -Tolerance && y < 1 + Tolerance)
                {
                    return true;
                }
            }
            return false;
        }

        #region IWorldEntity Members

        public override void Render(GraphicsPipeline gPipeline)
        {
            gPipeline.BeginScene();

            gDevice.RenderState.ZBufferEnable = false;

            gDevice.VertexFormat = CustomVertex.PositionColored.Format;
            gDevice.SetStreamSource(0, vBuffer, 0);
            gDevice.DrawPrimitives(PrimitiveType.LineList, 0, 3);

            if (Selected)
            {
                // just draw yellow line over selected axis
                gDevice.SetStreamSource(0, selectionVBuffer, 0);
                gDevice.DrawPrimitives(PrimitiveType.LineList, InternalSelectedValue - 1, 1);

                // TODO: draw triangular tube on that axis
                /*gDevice.Indices = selectionIBuffer;
                gDevice.SetStreamSource(0, selectionVBuffers[InternalSelectedValue - 1], 0);
                gDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 6, 0, 6);*/
            }

            gDevice.RenderState.ZBufferEnable = true;

            gPipeline.EndScene();
        }

        public override void Init(DeviceInterface devIf, SceneManager sManager)
        {
 	        base.Init(devIf, sManager);

            vBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), 6, gDevice, Usage.None,
                                       CustomVertex.PositionColored.Format, Pool.Managed);
            CustomVertex.PositionColored[] verts = (CustomVertex.PositionColored[])vBuffer.Lock(0, LockFlags.None);

            verts[0].Color = verts[1].Color = Color.Red.ToArgb();
            verts[1].Position = new Vector3(1, 0, 0);

            verts[2].Color = verts[3].Color = Color.Green.ToArgb();
            verts[3].Position = new Vector3(0, 1, 0);

            verts[4].Color = verts[5].Color = Color.Blue.ToArgb();
            verts[5].Position = new Vector3(0, 0, 1);

            vBuffer.Unlock();

            // build selection lines
            selectionVBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), 6, gDevice, Usage.None,
                                                    CustomVertex.PositionColored.Format, Pool.Managed);
            verts = (CustomVertex.PositionColored[])selectionVBuffer.Lock(0, LockFlags.None);

            verts[0].Color = verts[1].Color = Color.Yellow.ToArgb();
            verts[1].Position = new Vector3(1, 0, 0);

            verts[2].Color = verts[3].Color = Color.Yellow.ToArgb();
            verts[3].Position = new Vector3(0, 1, 0);

            verts[4].Color = verts[5].Color = Color.Yellow.ToArgb();
            verts[5].Position = new Vector3(0, 0, 1);

            selectionVBuffer.Unlock();

            labels = new ScreenSpaceText[3];
            labels[0] = new ScreenSpaceText("x", Color.Red, "Verdana", FontWeight.Normal, 10,
                                            new Vector3(1.1f, 0, 0), this);
            labels[0].Init(devIf, sManager);
            sManager.AddEntity(labels[0]);
            labels[1] = new ScreenSpaceText("y", Color.Green, "Verdana", FontWeight.Normal, 10,
                                            new Vector3(0, 1.1f, 0), this);
            labels[1].Init(devIf, sManager);
            sManager.AddEntity(labels[1]);
            labels[2] = new ScreenSpaceText("z", Color.Blue, "Verdana", FontWeight.Normal, 10,
                                            new Vector3(0, 0, 1.1f), this);
            labels[2].Init(devIf, sManager);
            sManager.AddEntity(labels[2]);
        }
        #endregion

        protected override void ManagedDispose()
        {
            if (vBuffer != null)
                vBuffer.Dispose();
        }
    }

    public class Ray3DHelper : SceneEntity
    {
        CustomVertex.PositionColored[] ray;
        readonly Vector3 start, end;

        public Ray3DHelper(Vector3 start, Vector3 dir, float length)
            : base(start, dir * length)
        {// NOTE: ^ may be wrong
            this.start = start;
            end = start + (dir * length);
        }

        #region IWorldEntity Members

        public override void Render(GraphicsPipeline gPipeline)
        {
            gDevice.VertexFormat = CustomVertex.PositionColored.Format;
            gDevice.DrawUserPrimitives(PrimitiveType.LineList, 1, ray);
        }

        public override void Init(DeviceInterface devIf, SceneManager sManager)
        {
            base.Init(devIf, sManager);

            ray = new CustomVertex.PositionColored[2];
            ray[0] = new CustomVertex.PositionColored(start, Color.Red.ToArgb());
            ray[1] = new CustomVertex.PositionColored(end, Color.Red.ToArgb());
        }
        #endregion
    }
}