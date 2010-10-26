using System.Drawing;
using Genetibase.NuGenRenderCore.Rendering;
using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Scene;
using Genetibase.VisUI.Maths;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.VisUI.Entities.Helpers
{
    public class RotationAxis3DHelper : SceneEntity
    {
        VertexBuffer vBuffer;
        int detailCount;
        int[] ringClrs;

        public RotationAxis3DHelper(Vector3 position)
            : base(position, new Vector3(1, 1, 1))
        { }

        private void BuildRings()
        {
            detailCount = 40;
            ringClrs = new int[] { Color.Red.ToArgb(), Color.Green.ToArgb(), Color.Blue.ToArgb() };
            vBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), (detailCount + 1) * 3,
                                       gDevice, Usage.WriteOnly, CustomVertex.PositionColored.Format,
                                       Pool.Managed);
            CustomVertex.PositionColored[] verts = (CustomVertex.PositionColored[])vBuffer.Lock(0, LockFlags.None);

            Vector2[] points;
            CircleHelper.CalcCirclePointsCCW(detailCount + 1, 1, new Vector2(), out points);
            BuildRingOnX(verts, (detailCount + 1) * 0, ringClrs[0], points);
            BuildRingOnY(verts, (detailCount + 1) * 1, ringClrs[1], points);
            BuildRingOnZ(verts, (detailCount + 1) * 2, ringClrs[2], points);
            
            vBuffer.Unlock();
        }

        private static void BuildRingOnX(CustomVertex.PositionColored[] verts, int index, int clr, Vector2[] points)
        {
            int pIdx = 0;
            for (int i = index; i < index + points.Length; i++)
            {
                verts[i] = new CustomVertex.PositionColored(0, points[pIdx].Y, points[pIdx].X, clr);
                pIdx++;
            }
            verts[index + points.Length] = new CustomVertex.PositionColored(0, points[0].Y, points[0].X, clr);
        }

        private static void BuildRingOnY(CustomVertex.PositionColored[] verts, int index, int clr, Vector2[] points)
        {
            int pIdx = 0;
            for (int i = index; i < index + points.Length; i++)
            {
                verts[i] = new CustomVertex.PositionColored(points[pIdx].Y, 0, points[pIdx].X, clr);
                pIdx++;
            }
            verts[index + points.Length] = new CustomVertex.PositionColored(points[0].Y, 0, points[0].X, clr);
        }

        private static void BuildRingOnZ(CustomVertex.PositionColored[] verts, int index, int clr, Vector2[] points)
        {
            int pIdx = 0;
            for (int i = index; i < index + points.Length; i++)
            {
                verts[i] = new CustomVertex.PositionColored(points[pIdx].X, points[pIdx].Y, 0, clr);
                pIdx++;
            }
            verts[index + points.Length] = new CustomVertex.PositionColored(points[0].X, points[0].Y, 0, clr);
        }

        #region IWorldEntity Members

        public override void Render(GraphicsPipeline gPipeline)
        {
            gDevice.RenderState.ZBufferEnable = false;

            gDevice.VertexFormat = CustomVertex.PositionColored.Format;
            gDevice.SetStreamSource(0, vBuffer, 0);
            for (int i = 0; i < 3; i++)
            {
                gDevice.DrawPrimitives(PrimitiveType.LineStrip, (detailCount + 1) * i, detailCount);
            }

            gDevice.RenderState.ZBufferEnable = true;
        }

        public override void Init(DeviceInterface devIf, SceneManager sManager)
        {
            base.Init(devIf, sManager);

            BuildRings();
        }
        #endregion

        protected override void ManagedDispose()
        {
            if (vBuffer != null)
                vBuffer.Dispose();
        }
    }
}