using System;
using System.Drawing;
using Genetibase.RasterDatabase;
using Genetibase.RasterDatabase.Geometry;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenDEMVis.Raster
{
    /// <summary>
    /// Encapsulates a Transformed Textured Quad in the form of a Trangle Strip
    /// </summary>
    struct TexturedQuad
    {
        public CustomVertex.TransformedTextured[] Quad;
        public Texture Texture;

        public TexturedQuad(CustomVertex.TransformedTextured[] quad, Texture texture)
        {
            Quad = quad;
            Texture = texture;
        }
    }

    /// <summary>
    /// Encapsulates a 2D array of textures
    /// </summary>
    class TextureMatrixLayer
    {
        public readonly Texture[][] MatrixSquares;
        public readonly int MatrixSquareSize;
        public readonly int MatrixSquareCount1D;

        public TextureMatrixLayer(Texture[][] matrixSquares, int matrixSquareSize, int matrixSquareCount1D)
        {
            MatrixSquares = matrixSquares;
            MatrixSquareSize = matrixSquareSize;
            MatrixSquareCount1D = matrixSquareCount1D;
        }

        public bool BuildTexturedQuads(Rectangle targetArea, out TexturedQuad[] quads)
        {
            // calc where each point is
            int left = (int)Math.Floor((double)targetArea.Left / MatrixSquareCount1D);
            int right = (int)Math.Floor((double)targetArea.Right / MatrixSquareCount1D);
            if (targetArea.Right % MatrixSquareCount1D == 0)
                right--;
            int top = (int)Math.Floor((double)targetArea.Top / MatrixSquareCount1D);
            int bottom = (int)Math.Floor((double)targetArea.Bottom / MatrixSquareCount1D);
            if (targetArea.Bottom % MatrixSquareCount1D == 0)
                bottom--;

            // process intersections into quads
            TexturedQuad[] temp = new TexturedQuad[(right - left) * (bottom - top)];
            int tempCount = 0;
            for (int x = left; x <= right; x++)
            {
                for (int y = top; y < bottom; y++)
                {
                    Texture square = MatrixSquares[x][y];
                    if (square != null)
                    {
                        int bLeft = x * MatrixSquareSize;
                        int bRight = bLeft + MatrixSquareSize;
                        int bTop = y * MatrixSquareSize;
                        int bBottom = bTop + MatrixSquareSize;

                        // test to see if intersects this square
                        if (targetArea.Left > bRight ||
                            targetArea.Right < bLeft ||
                            targetArea.Top > bBottom ||
                            targetArea.Bottom < bTop)
                            continue;

                        // clip original target area
                        int rLeft = targetArea.Left < bLeft ? bLeft : targetArea.Left;
                        int rRight = targetArea.Right > bRight ? bRight : targetArea.Right;
                        int rTop = targetArea.Top < bTop ? bTop : targetArea.Top;
                        int rBottom = targetArea.Bottom > bBottom ? bBottom : targetArea.Bottom;

                        CustomVertex.TransformedTextured[] quad = new CustomVertex.TransformedTextured[]
                        {
                            new CustomVertex.TransformedTextured(rLeft, rTop, 1, 1, 0, 0),
                            new CustomVertex.TransformedTextured(rRight, rTop, 1, 1, 1, 0),
                            new CustomVertex.TransformedTextured(rLeft, rBottom, 1, 1, 0, 1),
                            new CustomVertex.TransformedTextured(rRight, rBottom, 1, 1, 1, 1) 
                        };

                        temp[tempCount++] = new TexturedQuad(quad, square);
                    }
                }
            }

            // refactor quads array
            if (tempCount > 0)
            {
                quads = new TexturedQuad[tempCount];
                Array.Copy(temp, quads, tempCount);
                return true;
            }
            else
            {
                quads = null;
                return false;
            }
        }
    }

    class RasterDatabaseRenderer
    {
        public static TextureMatrixLayer[] RenderDatabaseTree(RasterDatabase.RasterDatabase db, Device device)
        {
            // setup device
            device.RenderState.ZBufferEnable = false;
            device.Indices = null;
            device.VertexFormat = CustomVertex.TransformedTextured.Format;
            //device.Transform.World = Matrix.Identity;
            Surface rt0 = device.GetRenderTarget(0);

            // setup template quad
            CustomVertex.TransformedTextured[] tQuad = new CustomVertex.TransformedTextured[4];

            foreach(DataLayer layer in db.Layers)
            {
                RectangleGroupQuadTree tree = db.ProduceLayerMipMap(layer, 2048);
                Texture[][] textures = new Texture[tree.Depth][];
                for (int i = 1; i <= tree.Depth; i++)
                {
                    RectangleGroupQuadTree.GroupNode[] nodes;
                    tree.GetNodes(i, out nodes);
                    textures[i] = new Texture[nodes.Length];

                    // render each node to texture
                    int texIdx = 0;
                    foreach (RectangleGroupQuadTree.GroupNode node in nodes)
                    {
                        Texture texture = textures[i][texIdx++] = new Texture(device, node.NodeArea.Width,
                                                                              node.NodeArea.Height, 0,
                                                                              Usage.WriteOnly, Format.X8R8G8B8,
                                                                              Pool.Managed);
                        device.SetRenderTarget(0, texture.GetSurfaceLevel(0));

                        device.Clear(ClearFlags.Target, Color.Black, 1, 0);
                        device.BeginScene();

                        // draw each rectangle quad
                        foreach (DataArea area in node.Rectangles)
                        {
                            // setup quad
                            tQuad[0] = new CustomVertex.TransformedTextured(area.Area.Left, area.Area.Top, 1, 1,
                                                                            area.TexCoords.Left, area.TexCoords.Top);
                            tQuad[1] = new CustomVertex.TransformedTextured(area.Area.Right, area.Area.Top, 1, 1,
                                                                            area.TexCoords.Right, area.TexCoords.Top);
                            tQuad[2] = new CustomVertex.TransformedTextured(area.Area.Left, area.Area.Bottom, 1, 1,
                                                                            area.TexCoords.Left, area.TexCoords.Bottom);
                            tQuad[3] = new CustomVertex.TransformedTextured(area.Area.Right, area.Area.Bottom, 1, 1,
                                                                            area.TexCoords.Right, area.TexCoords.Bottom);

                            // render quad
                            device.SetTexture(0, (Texture)area.Data);
                            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, tQuad);
                        }
                        device.EndScene();
                    }
                }
            }

            device.SetRenderTarget(0, rt0);
            device.RenderState.ZBufferEnable = true;

            return null;
        }
    }
}