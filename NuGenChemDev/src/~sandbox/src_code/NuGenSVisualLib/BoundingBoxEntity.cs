using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace NuGenSVisualLib.Rendering
{
    /// <summary>
    /// Encapsulates a wireframe or solid box that represents a bounding box
    /// </summary>
    /// <remarks>Need to add solid w/alpha support</remarks>
    class BoundingBoxEntity : IEntity
    {
        Device device;

        readonly BoundingBox bBox;
        bool drawAxis;
        readonly int clr;

        VertexBuffer vBufferBox, vBufferAxis;

        public BoundingBoxEntity(BoundingBox bBox, bool drawAxis, int clr)
        {
            this.bBox = bBox;
            this.drawAxis = drawAxis;
            this.clr = clr;
        }

        public bool DrawAxis
        {
            get { return drawAxis; }
            set { drawAxis = value; }
        }

        #region IEntity Members

        public void Render()
        {
            // draw box wireframe
            device.BeginScene();

            device.SetStreamSource(0, vBufferBox, 0);
            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.Indices = null;
            device.DrawPrimitives(PrimitiveType.LineList, 0, 12);

            // draw axis
            if (drawAxis)
            {
                device.RenderState.ZBufferEnable = false;

                device.SetStreamSource(0, vBufferAxis, 0);
                device.DrawPrimitives(PrimitiveType.LineList, 0, 3);

                device.RenderState.ZBufferEnable = true;
            }

            device.EndScene();
        }

        public void Init(Device device)
        {
            this.device = device;

            // create box vBuffer
            vBufferBox = new VertexBuffer(typeof(CustomVertex.PositionColored), 24, device, Usage.None,
                                          CustomVertex.PositionColored.Format, Pool.Managed);
            CustomVertex.PositionColored[] lines = (CustomVertex.PositionColored[])vBufferBox.Lock(0, LockFlags.None);

            // front side
            lines[0] = new CustomVertex.PositionColored(bBox.Position/*Centre - bBox.Dimensions*/, clr);
            lines[1] = new CustomVertex.PositionColored(new Vector3(bBox.Position.X,//Centre.X - bBox.Dimensions.X,
                                                                    bBox.Position.Y,//Centre.Y - bBox.Dimensions.Y,
                                                                    bBox.Position.Z + bBox.Extent.Z),//Centre.Z + bBox.Dimensions.Z),
                                                        clr);
            lines[2] = lines[1];
            lines[3] = new CustomVertex.PositionColored(new Vector3(bBox.Position.X,//Centre.X - bBox.Dimensions.X,
                                                                    bBox.Position.Y + bBox.Extent.Y,//Centre.Y + bBox.Dimensions.Y,
                                                                    bBox.Position.Z + bBox.Extent.Z),//Centre.Z + bBox.Dimensions.Z),
                                                        clr);
            lines[4] = lines[3];
            lines[5] = new CustomVertex.PositionColored(new Vector3(bBox.Position.X,//Centre.X - bBox.Dimensions.X,
                                                                    bBox.Position.Y + bBox.Extent.Y,//Centre.Y + bBox.Dimensions.Y,
                                                                    bBox.Position.Z),//Centre.Z - bBox.Dimensions.Z),
                                                        clr);
            lines[6] = lines[5];
            lines[7] = lines[0];

            // back side
            lines[8] = new CustomVertex.PositionColored(new Vector3(bBox.Position.X + bBox.Extent.X,//Centre.X + bBox.Dimensions.X,
                                                                    bBox.Position.Y,//Centre.Y - bBox.Dimensions.Y,
                                                                    bBox.Position.Z),//Centre.Z - bBox.Dimensions.Z),
                                                        clr);
            lines[9] = new CustomVertex.PositionColored(new Vector3(bBox.Position.X + bBox.Extent.X,//Centre.X + bBox.Dimensions.X,
                                                                    bBox.Position.Y,//Centre.Y - bBox.Dimensions.Y,
                                                                    bBox.Position.Z + bBox.Extent.Z),//Centre.Z + bBox.Dimensions.Z),
                                                        clr);
            lines[10] = lines[9];
            lines[11] = new CustomVertex.PositionColored(bBox.Position + bBox.Extent/*Centre + bBox.Dimensions*/, clr);
            
            lines[12] = lines[11];
            lines[13] = new CustomVertex.PositionColored(new Vector3(bBox.Position.X + bBox.Extent.X,//Centre.X + bBox.Dimensions.X,
                                                                    bBox.Position.Y + bBox.Extent.Y,//Centre.Y + bBox.Dimensions.Y,
                                                                    bBox.Position.Z),//Centre.Z - bBox.Dimensions.Z),
                                                        clr);
            lines[14] = lines[13];
            lines[15] = lines[8];

            // connecting lines
            lines[16] = lines[0];
            lines[17] = lines[8];

            lines[18] = lines[1];
            lines[19] = lines[9];

            lines[20] = lines[4];
            lines[21] = lines[12];

            lines[22] = lines[5];
            lines[23] = lines[13];

            vBufferBox.Unlock();

            // create axis
            vBufferAxis = new VertexBuffer(typeof(CustomVertex.PositionColored), 6, device, Usage.None,
                                           CustomVertex.PositionColored.Format, Pool.Managed);
            CustomVertex.PositionColored[] verts = (CustomVertex.PositionColored[])vBufferAxis.Lock(0, LockFlags.None);

            verts[0].Color = verts[1].Color = Color.Red.ToArgb();
            verts[1].Position = bBox.Centre + new Vector3(1, 0, 0);
            verts[0].Position = bBox.Centre;

            verts[2].Color = verts[3].Color = Color.Green.ToArgb();
            verts[3].Position = bBox.Centre + new Vector3(0, 1, 0);
            verts[2].Position = bBox.Centre;

            verts[4].Color = verts[5].Color = Color.Blue.ToArgb();
            verts[5].Position = bBox.Centre + new Vector3(0, 0, 1);
            verts[4].Position = bBox.Centre;

            vBufferAxis.Unlock();
        }

        public BoundingBox BoundingBox
        {
            get { return bBox; }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (vBufferBox != null)
                vBufferBox.Dispose();
            if (vBufferAxis != null)
                vBufferAxis.Dispose();
        }
        #endregion
    }
}
