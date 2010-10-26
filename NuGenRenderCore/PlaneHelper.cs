using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenRenderCore.Rendering.Helpers
{
    public class PlaneHelper
    {
        public struct PatchVertex
        {
            public float X, Y, Z;
            public float Tu2, Tv2;
            public float Tu1, Tv1;
            public int Color;

            /// <summary>
            /// Initializes a new instance of the PatchVertex structure.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z"></param>
            /// <param name="tu1"></param>
            /// <param name="tv1"></param>
            /// <param name="tu2"></param>
            /// <param name="tv1"></param>
            /// <param name="color"></param>
            public PatchVertex(float x, float y, float z, float tu1, float tv1, float tu2, float tv2, int color)
            {
                X = x;
                Y = y;
                Z = z;
                Tu1 = tu1;
                Tv1 = tv1;
                Tu2 = tu2;
                Tv2 = tv2;
                Color = color;
            }

            public static VertexFormats Format = VertexFormats.Position | VertexFormats.Texture0 | VertexFormats.Texture1 | VertexFormats.Diffuse;
        }

        public static void CreateIndexBuffer(Device gDevice, int width, int height, out IndexBuffer iBuffer)
        {
            // create buffer
            iBuffer = new IndexBuffer(typeof(int), (width - 1) * (height - 1) * 6, gDevice, Usage.WriteOnly, Pool.Managed);
            int[] indices = (int[])iBuffer.Lock(0, LockFlags.None);
            
            // fill buffer
            int bufIdx = 0;
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    // fill quad (2xtri)
                    int pos = (y * width) + x;
                    indices[bufIdx++] = pos;
                    indices[bufIdx++] = pos + width;
                    indices[bufIdx++] = pos + 1;

                    indices[bufIdx++] = pos + 1;
                    indices[bufIdx++] = pos + width;
                    indices[bufIdx++] = pos + 1 + width;
                }
            }
            iBuffer.Unlock();
        }

        public static void CreateVertexBufferInside(Device gDevice, int width, int height, Vector2 area, out VertexBuffer vBuffer)
        {
            // create buffer
            vBuffer = new VertexBuffer(typeof(CustomVertex.PositionTextured), (width - 2) * (height - 2), gDevice,
                                       Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Managed);
            CustomVertex.PositionTextured[] verts = (CustomVertex.PositionTextured[])vBuffer.Lock(0, LockFlags.None);
            
            // fill buffer
            Vector2 scale = new Vector2(area.X / (width - 1), area.Y / (height - 1));
            Vector2 texScale = new Vector2(1f / (width - 1), 1f / (height - 1));
            float halfPixel = texScale.X / 2;
            texScale = new Vector2((1 - texScale.X) / (width - 1), (1 - texScale.Y) / (height - 1));
            int vIdx = 0;
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    verts[vIdx++] = new CustomVertex.PositionTextured(x * scale.X, 0, y * scale.Y, (x * texScale.X) + halfPixel, (y * texScale.Y) + halfPixel);
                }
            }
            vBuffer.Unlock();
        }

        public static void CreateVertexBuffer(Device gDevice, int width, int height, Vector2 area, out VertexBuffer vBuffer)
        {
            // create buffer
            vBuffer = new VertexBuffer(typeof(CustomVertex.PositionTextured), width * height, gDevice,
                                       Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Managed);
            CustomVertex.PositionTextured[] verts = (CustomVertex.PositionTextured[])vBuffer.Lock(0, LockFlags.None);

            // fill buffer
            Vector2 scale = new Vector2(area.X / (width - 1), area.Y / (height - 1));
            Vector2 texScale = new Vector2(1f / (width - 1), 1f / (height - 1));
            /*float halfPixel = texScale.X / 2;
            texScale = new Vector2((1 - texScale.X) / (width - 1), (1 - texScale.Y) / (height - 1));*/
            int vIdx = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    verts[vIdx++] = new CustomVertex.PositionTextured(x * scale.X, 0, y * scale.Y, (x * texScale.X) /*+ halfPixel*/, (y * texScale.Y) /*+ halfPixel*/);
                }
            }
            vBuffer.Unlock();
        }


        public static void CreatePatchVertexBuffer(Device gDevice, int width, int height, Vector2 area, out VertexBuffer vBuffer)
        {
            vBuffer = new VertexBuffer(typeof(CustomVertex.PositionColoredTextured), (width * 4) + (height * 4), gDevice,
                                       Usage.WriteOnly, CustomVertex.PositionColoredTextured.Format, Pool.Managed);

            CustomVertex.PositionColoredTextured[] verts = (CustomVertex.PositionColoredTextured[])vBuffer.Lock(0, LockFlags.None);
            float xScale = area.X / (width - 1);
            float yScale = area.Y / (height - 1);
            float xTexScale = (1 - (1f / (width - 1))) / (width - 1);
            float yTexScale = (1 - (1f / (height - 1))) / (height - 1);
            int vIdx = 0;
            float halfPixel = (1f / (width - 1)) / 2;
            
            bool even = true;
            // create vertical strips (left-right)
            int evenClr = Color.White.ToArgb();
            int oddClr = Color.Gray.ToArgb();
            int zeroClr = Color.Black.ToArgb();
            for (int y = 0; y < height; y++)
            {
                verts[vIdx++] = new CustomVertex.PositionColoredTextured(0, 0, y * yScale, even ? evenClr : oddClr,
                                                halfPixel, (y * yTexScale) + halfPixel
                                                /*1, 0,*/
                                                );
                verts[vIdx++] = new CustomVertex.PositionColoredTextured(xScale, 0, y * yScale, zeroClr,
                                                xTexScale + halfPixel, (y * yTexScale) + halfPixel
                                                /*1, 0,*/
                                                );
                even = !even;
            }
            even = true;
            for (int y = 0; y < height; y++)
            {
                verts[vIdx++] = new CustomVertex.PositionColoredTextured(area.X - xScale, 0, y * yScale, zeroClr,
                                                ((height - 1) * xTexScale) - halfPixel, (y * yTexScale) + halfPixel
                                                /*0, 0,*/
                                                );
                verts[vIdx++] = new CustomVertex.PositionColoredTextured(area.X, 0, y * yScale, even ? evenClr : oddClr,
                                                ((height - 1) * xTexScale) + halfPixel, (y * yTexScale) + halfPixel
                                                /*1, 0,*/
                                                );
                even = !even;
            }

            // create horizonal strips (bottom-top)
            even = true;
            for (int x = 0; x < width; x++)
            {
                verts[vIdx++] = new CustomVertex.PositionColoredTextured(x * xScale, 0, 0, even ? evenClr : oddClr,
                                                (x * xTexScale) + halfPixel, halfPixel
                    /*1, 0,*/
                                                );
                verts[vIdx++] = new CustomVertex.PositionColoredTextured(x * xScale, 0, yScale, zeroClr,
                                                (x * xTexScale) + halfPixel, yTexScale + halfPixel
                    /*1, 0,*/
                                                );
                even = !even;
            }
            even = true;
            for (int x = 0; x < width; x++)
            {
                verts[vIdx++] = new CustomVertex.PositionColoredTextured(x * xScale, 0, area.Y - yScale, zeroClr,
                                                (x * xTexScale) + halfPixel, ((width - 1) * yTexScale) - halfPixel
                    /*0, 0,*/
                                                );
                verts[vIdx++] = new CustomVertex.PositionColoredTextured(x * xScale, 0, area.Y, even ? evenClr : oddClr,
                                                (x * xTexScale) + halfPixel, ((width - 1) * yTexScale) + halfPixel
                    /*0, 0,*/
                                                );
                even = !even;
            }

            vBuffer.Unlock();
        }

        public static void CreatePatchIndexBuffer(Device gDevice, int width, int height, out IndexBuffer iBuffer)
        {
            // create buffer
            iBuffer = new IndexBuffer(typeof(int), /*(((width - 1) * 4) + ((height - 1) * 4))*/(width - 1) * 4 * 6, gDevice, Usage.WriteOnly, Pool.Managed);
            int[] indices = (int[])iBuffer.Lock(0, LockFlags.None);

            // fill buffer
            int bufIdx = 0;
            for (int y = 0; y < (height - 1) * 2; y += 2)
            {
                // fill quad (2xtri)
                indices[bufIdx++] = y;
                indices[bufIdx++] = y + 2;
                indices[bufIdx++] = y + 1;

                indices[bufIdx++] = y + 1;
                indices[bufIdx++] = y + 2;
                indices[bufIdx++] = y + 3;
            }
            for (int y = 0; y < (height - 1) * 2; y += 2)
            {
                // fill quad (2xtri)
                indices[bufIdx++] = y;
                indices[bufIdx++] = y + 1;
                indices[bufIdx++] = y + 2;

                indices[bufIdx++] = y + 1;
                indices[bufIdx++] = y + 3;
                indices[bufIdx++] = y + 2;
            }
            /*for (int y = height * 2; y < (height - 1) * 4; y += 2)
            {
                // fill quad (2xtri)
                indices[bufIdx++] = y;
                indices[bufIdx++] = y + 2;
                indices[bufIdx++] = y + 1;

                indices[bufIdx++] = y + 1;
                indices[bufIdx++] = y + 2;
                indices[bufIdx++] = y + 3;
            }*/
            /*for (int x = height * 4; x < (height * 4) + ((width - 1) * 2); x+=2)
            {
                // fill quad (2xtri)
                indices[bufIdx++] = x;
                indices[bufIdx++] = x + width;
                indices[bufIdx++] = x + 1;

                indices[bufIdx++] = x + 1;
                indices[bufIdx++] = x + width;
                indices[bufIdx++] = x + 1 + width;
            }
            for (int x = (height * 4) + ((width - 1) * 2); x < (height * 4) + ((width - 1) * 4); x+=2)
            {
                // fill quad (2xtri)
                indices[bufIdx++] = x;
                indices[bufIdx++] = x + width;
                indices[bufIdx++] = x + 1;

                indices[bufIdx++] = x + 1;
                indices[bufIdx++] = x + width;
                indices[bufIdx++] = x + 1 + width;
            }*/
            iBuffer.Unlock();
        }
    }
}