using System.Drawing;
using Genetibase.NuGenDEMVis.GIS;
using Genetibase.NuGenRenderCore.Rendering;
using Genetibase.RasterDatabase;
using Genetibase.RasterDatabase.Geometry;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenDEMVis
{
    /// <summary>
    /// Encapsulates static geometry representing a elevation map
    /// </summary>
    class CpuDemGeometry : DEMGeometry
    {
        CpuDEMSubGeometry[] subGeometry;
        IndexBuffer[] iBuffers;
        IndexBuffer[] iBuffersLong2;
        Texture[] tex;
        static readonly int maxRes = 256;
        static readonly int numSamples = 16;

        RectangleGroupQuadTree.GroupNode[] nodes;
        RectangleGroupQuadTree rTree;

        public CpuDemGeometry(Vector3 position, Vector3 dimensions, Vector3 center, ulong vertexCount,
                              uint primitiveCount, PrimitiveType primitivesType, Device gDevice)
            : base(position, dimensions, center, vertexCount, primitiveCount, primitivesType, gDevice)
        {
        }

        public static CpuDemGeometry CreateGeometry(RasterDatabase.RasterDatabase rDatabase, Device gDevice,
                                                    out float maxValue)
        {
            // TODO: Still some atifact problems?

            // just use default LOD for now
            RectangleGroupQuadTree rTree = rDatabase.ProduceLayerMipMap(0, maxRes);
            maxValue = rTree.MaxDataValue;

            // use bottom level only for now
            RectangleGroupQuadTree.GroupNode[] nodes;
            rTree.GetNodes(rTree.Depth, out nodes);

            // create sub-geometry for each node/area
            CpuDEMSubGeometry[] subGeometry = new CpuDEMSubGeometry[nodes.Length];
            for (int i = 0; i < nodes.Length; i++)
            {
                subGeometry[i] = CpuDEMSubGeometry.CreatePointList(nodes[i], gDevice, maxRes / numSamples,
                                                                      rDatabase.Layers[0].Area.Size,
                                                                      rTree.MinDataValue, rTree.MaxDataValue);
            }

            // TODO: Produce diffuse-maps (etc.) for the tree

            IndexBuffer[] iBuffers = CpuDEMSubGeometry.CreateSampleTriStripsIndices(numSamples, numSamples, gDevice);

            CpuDemGeometry geom = new CpuDemGeometry(new Vector3(), new Vector3(), new Vector3(),
                                                     0, 0, PrimitiveType.PointList, gDevice);
            geom.subGeometry = subGeometry;
            geom.iBuffers = iBuffers;
            geom.tex = new Texture[nodes.Length];
            geom.rTree = rTree;
            geom.nodes = nodes;
            
            geom.iBuffersLong2 = CpuDEMSubGeometry.CreateSampleTriStripsIndices(numSamples + 1, numSamples + 1, gDevice);
            return geom;
        }

        public override IDemSubGeometry[] SubGeometry
        {
            get { return subGeometry; }
        }

        public override void Render(GraphicsPipeline gPipe)
        {
            gDevice.VertexFormat = CustomVertex.PositionNormalTextured.Format;
            //gDevice.RenderState.CullMode = Cull.None;
            //gDevice.RenderState.FillMode = FillMode.WireFrame;
            gDevice.SamplerState[0].AddressU = TextureAddress.Clamp;
            gDevice.SamplerState[0].AddressV = TextureAddress.Clamp;
            gDevice.SamplerState[0].MinFilter = TextureFilter.Linear;
            gDevice.SamplerState[0].MagFilter = TextureFilter.Linear;
            gDevice.SamplerState[0].MipFilter = TextureFilter.Anisotropic;
            //gDevice.SamplerState[0].BorderColor = Color.Red;

            gDevice.Lights[0].Type = LightType.Directional;
            gDevice.Lights[0].Direction = new Vector3(1, -1, 1);
            gDevice.Lights[0].DiffuseColor = ColorValue.FromColor(Color.White);
            gDevice.Lights[0].Update();
            gDevice.Lights[0].Enabled = true;

            gDevice.RenderState.Lighting = true;

            int gIdx = 0;
            foreach (CpuDEMSubGeometry geom in subGeometry)
            {
                if (useDiffuseTex)
                    gDevice.SetTexture(0, tex[gIdx++]);
                else
                    gDevice.SetTexture(0, null);

                gDevice.SetStreamSource(0, geom.vBuffer, 0);
                IndexBuffer[] indices;
                if (geom.iBufSize.Width > numSamples)
                    indices = iBuffersLong2;
                else
                    indices = iBuffers;

                foreach (IndexBuffer iBuffer in indices)
                {
                    gDevice.Indices = iBuffer;
                    gDevice.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, geom.iBufSize.Width * 2, 0, (geom.iBufSize.Width - 1) * 2);
                }

                // draw any skirts
                for (int i = 0; i < geom.skirts.Length; i++)
                {
                    gDevice.Indices = geom.skirts[i];
                    gDevice.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, geom.iBufSize.Width * 2, 0, (geom.iBufSize.Width - 1) * 2);
                }
            }

            gDevice.SetTexture(0, null);
            gDevice.RenderState.Lighting = false;
        }

        public override void Dispose()
        {
        }

        public void RebuildDiffuseTextures(IDEMDataSampler sampler)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                // TODO: Render into texture for nodes with more than 1 source
                tex[i] = sampler.GenerateTexture(new Size(256, 256), nodes[i].Rectangles[0], gDevice);
            }
        }

        public void RebuildDiffuseTextures(GDALReader reader)
        {
            SourceDataDiffuseSampler sampler = new SourceDataDiffuseSampler();
            for (int i = 0; i < nodes.Length; i++)
            {
                tex[i] = sampler.GenerateTexture(new Size(256, 256), nodes[i].Rectangles[0], gDevice, reader);
            }
        }
    }

    class CpuDEMSubGeometry : IDemSubGeometry
    {
        private Vector2 position;
        private Vector2 dimensions;
        private Vector2 center;
        private ulong vertexCount;
        private uint primitiveCount;
        private PrimitiveType primitivesType;
        private IDemSubGeometry[] subGeometry;

        public VertexBuffer vBuffer;
        public IndexBuffer[] iBuffers;
        public Size iBufSize;

        public IndexBuffer[] skirts;

        #region IDEMSubGeometry Members

        public Vector2 Position
        {
            get { return position; }
        }

        public Vector2 Dimensions
        {
            get { return dimensions; }
        }

        public Vector2 Center
        {
            get { return center; }
        }

        public ulong VertexCount
        {
            get { return vertexCount; }
        }

        public uint PrimitiveCount
        {
            get { return primitiveCount; }
        }

        public PrimitiveType PrimitivesType
        {
            get { return primitivesType; }
        }

        public IDemSubGeometry[] SubGeometry
        {
            get { return subGeometry; }
        }
        #endregion

        public static CpuDEMSubGeometry CreatePointList(RectangleGroupQuadTree.GroupNode node, Device gDevice,
                                                           int sampleInterval, Size totalAreaSz,
                                                           float minValue, float maxValue)
        {
            // BUG: Not working right for < native?
            int xSamples = node.NodeArea.Width / sampleInterval;
            int ySamples = node.NodeArea.Height / sampleInterval;

            bool blendStartX = (node.NodeArea.Left != 0);
            bool blendEndX = (node.NodeArea.Right != totalAreaSz.Width);
            bool blendStartY = (node.NodeArea.Top != 0);
            bool blendEndY = (node.NodeArea.Bottom != totalAreaSz.Height);

            Size iBufSize = new Size(xSamples, ySamples);

            int xAdditional = 0;
            int yAdditional = 0;

            if (!blendStartX)
                xAdditional++;
            if (!blendEndX)
                xAdditional++;
            if (!blendStartY)
                yAdditional++;
            if (!blendEndY)
                yAdditional++;

            // create buffers
            VertexBuffer vBuffer = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured),
                                                    (xSamples + xAdditional) * (ySamples + yAdditional),
                                                    gDevice, Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format,
                                                    Pool.Managed);
            CustomVertex.PositionNormalTextured[] verts = (CustomVertex.PositionNormalTextured[])vBuffer.Lock(0, LockFlags.None);
            
            // fill x+z and texCoord components independant of y heights
            float yActual = /*-2.5f +*/ ((float)node.NodeArea.Top / totalAreaSz.Height * 5f);
            float xActInc = ((float)node.NodeArea.Width / totalAreaSz.Width * 5) / (xSamples - 1);
            float yActInc = ((float)node.NodeArea.Height / totalAreaSz.Height * 5) / (ySamples - 1);
            int vPos = 0;
            float ty = 0;
            float tyInc = 1f / (ySamples - 1);
            for (int y = 0; y < ySamples; y++)
            {
                float xActual = /*-2.5f + */((float)node.NodeArea.Left / totalAreaSz.Width * 5f);
                float tx = 0;
                float txInc = 1f / (xSamples - 1);
                for (int x = 0; x < xSamples; x++)
                {
                    verts[vPos++] = new CustomVertex.PositionNormalTextured(xActual, 0, yActual,
                                                                            0, 0, 0,
                                                                            tx, ty);

                    xActual += xActInc;
                    tx += txInc;
                }
                yActual += yActInc;
                ty += tyInc;
            }

            // skirts
            IndexBuffer[] skirts = new IndexBuffer[xAdditional + yAdditional];
            int skirtIdx = 0;
            if (!blendStartX)
            {
                yActual = /*-2.5f +*/ ((float)node.NodeArea.Top / totalAreaSz.Height * 5f);
                ty = 0;
                for (int y = 0; y < ySamples; y++)
                {
                    float xActual = /*-2.5f +*/ ((float)node.NodeArea.Left / totalAreaSz.Width * 5f);
                    verts[vPos++] = new CustomVertex.PositionNormalTextured(xActual, -0.5f, yActual,
                                                                            0, 0, 0,
                                                                            -0.1f, ty);
                    yActual += yActInc;
                    ty += tyInc;
                }
                skirts[skirtIdx] = CreateTriStripSkirtIndices(xSamples, gDevice, 0, skirtIdx);
                skirtIdx++;
            }
            if (!blendEndX)
            {
                yActual = /*-2.5f +*/ ((float)node.NodeArea.Top / totalAreaSz.Height * 5f);
                ty = 0;
                for (int y = 0; y < ySamples; y++)
                {
                    float xActual = /*-2.5f +*/ ((float)node.NodeArea.Right / totalAreaSz.Width * 5);
                    verts[vPos++] = new CustomVertex.PositionNormalTextured(xActual, -0.5f, yActual,
                                                                            0, 0, 0,
                                                                            1.1f, ty);
                    yActual += yActInc;
                    ty += tyInc;
                }
                skirts[skirtIdx] = CreateTriStripSkirtIndices(xSamples, gDevice, 1, skirtIdx);
                skirtIdx++;
            }
            if (!blendStartY)
            {
                float xActual = /*-2.5f +*/ ((float)node.NodeArea.Left / totalAreaSz.Width * 5f);
                float tx = 0;
                for (int y = 0; y < ySamples; y++)
                {
                    yActual = /*-2.5f +*/ ((float)node.NodeArea.Top / totalAreaSz.Height * 5f);
                    verts[vPos++] = new CustomVertex.PositionNormalTextured(xActual, -0.5f, yActual,
                                                                            0, 0, 0,
                                                                            tx, -0.1f);
                    xActual += xActInc;
                    tx += tyInc;
                }
                skirts[skirtIdx] = CreateTriStripSkirtIndices(xSamples, gDevice, 2, skirtIdx);
                skirtIdx++;
            }
            if (!blendEndY)
            {
                float xActual = /*-2.5f +*/ ((float)node.NodeArea.Left / totalAreaSz.Width * 5f);
                float tx = 0;
                for (int y = 0; y < ySamples; y++)
                {
                    yActual = /*-2.5f +*/ ((float)node.NodeArea.Bottom / totalAreaSz.Height * 5f);
                    verts[vPos++] = new CustomVertex.PositionNormalTextured(xActual, -0.5f, yActual,
                                                                            0, 0, 0,
                                                                            tx, 1.1f);
                    xActual += xActInc;
                    tx += tyInc;
                }
                skirts[skirtIdx] = CreateTriStripSkirtIndices(xSamples, gDevice, 3, skirtIdx);
                skirtIdx++;
            }

            SizeF onePixel = new SizeF(1f / (node.NodeArea.Width - 1), 1f / (node.NodeArea.Height - 1));

            // fill in from sample rectangles only
            float[] heights = new float[xSamples * ySamples];
            foreach (DataArea area in node.Rectangles)
            {
                SimpleRasterSampler sampler = new MultiSampleRasterSampler(area, sampleInterval, xSamples);
                SimpleRasterSampler singleSampler = new SimpleRasterSampler(area, sampleInterval);
                // TODO: Sample only area not 0 -> (1-1px%)
                float xAdv = 1f / (xSamples - 1);
                float yAdv = 1f / (ySamples - 1);

                float yPos = 0;
                vPos = 0;
                int yStart = 0;
                int yEnd = ySamples;
                // y edge
                if (blendStartY)
                {
                    float xPos = 0;
                    int xStart = 0;
                    int xEnd = xSamples;

                    // xy edge
                    float rawValue1, rawValue2, rawValue3, rawValue4;
                    float sVal, value;
                    if (blendStartX)
                    {
                        if (sampler.DataSource is ByteArea)
                        {
                            rawValue1 = singleSampler.GetByte(xPos, yPos);
                            rawValue2 = singleSampler.GetByte(xPos - onePixel.Width, yPos);
                            rawValue3 = singleSampler.GetByte(xPos, yPos - onePixel.Height);
                            rawValue4 = singleSampler.GetByte(xPos - onePixel.Width, yPos - onePixel.Height);
                        }
                        else
                        {
                            rawValue1 = singleSampler[xPos, yPos];
                            rawValue2 = singleSampler[xPos - onePixel.Width, yPos];
                            rawValue3 = singleSampler[xPos, yPos - onePixel.Height];
                            rawValue4 = singleSampler[xPos - onePixel.Width, yPos - onePixel.Height];
                        }
                        sVal = ((rawValue1 / maxValue) + (rawValue2 / maxValue) + (rawValue3 / maxValue) + (rawValue4 / maxValue)) / 4;
                        value = -0.5f + sVal;
                        heights[vPos++] = value;

                        xPos += xAdv;
                        xStart++;
                    }

                    if (blendEndX)
                        xEnd--;

                    // do normal range
                    for (int x = xStart; x < xEnd; x++)
                    {
                        if (sampler.DataSource is ByteArea)
                        {
                            rawValue1 = singleSampler.GetByte(xPos, yPos);
                            rawValue2 = singleSampler.GetByte(xPos, yPos - onePixel.Height);
                        }
                        else
                        {
                            rawValue1 = singleSampler[xPos, yPos];
                            rawValue2 = singleSampler[xPos, yPos - onePixel.Height];
                        }
                        sVal = ((rawValue1 / maxValue) + (rawValue2 / maxValue)) / 2;
                        value = -0.5f + sVal;
                        heights[vPos++] = value;

                        xPos += xAdv;
                    }

                    // xy edge
                    if (blendEndX)
                    {
                        // take 2 samples
                        if (sampler.DataSource is ByteArea)
                        {
                            rawValue1 = singleSampler.GetByte(xPos, yPos);
                            rawValue2 = singleSampler.GetByte(xPos + onePixel.Width, yPos);
                            rawValue3 = singleSampler.GetByte(xPos, yPos - onePixel.Height);
                            rawValue4 = singleSampler.GetByte(xPos + onePixel.Width, yPos - onePixel.Height);
                        }
                        else
                        {
                            rawValue1 = singleSampler[xPos, yPos];
                            rawValue2 = singleSampler[xPos + onePixel.Width, yPos];
                            rawValue3 = singleSampler[xPos, yPos - onePixel.Height];
                            rawValue4 = singleSampler[xPos + onePixel.Width, yPos - onePixel.Height];
                        }
                        sVal = ((rawValue1 / maxValue) + (rawValue2 / maxValue) + (rawValue3 / maxValue) + (rawValue4 / maxValue)) / 4;
                        value = -0.5f + sVal;
                        heights[vPos++] = value;
                    }

                    yPos += yAdv;
                    yStart++;
                }

                if (blendEndY)
                    yEnd--;

                // normal range + x exclusive edges
                for (int y = yStart; y < yEnd; y++)
                {
                    float xPos = 0;
                    int xStart = 0;
                    int xEnd = xSamples;
                    if (blendStartX)
                    {
                        // take 2 samples
                        float rawValue1, rawValue2;
                        if (sampler.DataSource is ByteArea)
                        {
                            rawValue1 = singleSampler.GetByte(xPos, yPos);
                            rawValue2 = singleSampler.GetByte(xPos - onePixel.Width, yPos);
                        }
                        else
                        {
                            rawValue1 = singleSampler[xPos, yPos];
                            rawValue2 = singleSampler[xPos - onePixel.Width, yPos];
                        }
                        float sVal = ((rawValue1 / maxValue) + (rawValue2 / maxValue)) / 2;
                        float value = -0.5f + sVal;
                        heights[vPos++] = value;

                        xPos += xAdv;
                        xStart++;
                    }

                    if (blendEndX)
                        xEnd--;

                    // do normal range
                    for (int x = xStart; x < xEnd; x++)
                    {
                        float rawValue;
                        if (sampler.DataSource is ByteArea)
                            rawValue = sampler.GetByte(xPos, yPos);
                        else
                            rawValue = sampler[xPos, yPos];
                        float sVal = rawValue / maxValue;
                        float value = -0.5f + sVal;
                        heights[vPos++] = value;

                        xPos += xAdv;
                    }

                    if (blendEndX)
                    {
                        // take 2 samples
                        float rawValue1, rawValue2;
                        if (sampler.DataSource is ByteArea)
                        {
                            rawValue1 = singleSampler.GetByte(xPos, yPos);
                            rawValue2 = singleSampler.GetByte(xPos + onePixel.Width, yPos);
                        }
                        else
                        {
                            rawValue1 = singleSampler[xPos, yPos];
                            rawValue2 = singleSampler[xPos + onePixel.Width, yPos];
                        }
                        float sVal = ((rawValue1 / maxValue) + (rawValue2 / maxValue)) / 2;
                        float value = -0.5f + sVal;
                        heights[vPos++] = value;
                    }
                    
                    yPos += yAdv;
                }

                // y edge
                if (blendEndY)
                {
                    float xPos = 0;
                    int xStart = 0;
                    int xEnd = xSamples;

                    // xy egde
                    float rawValue1, rawValue2, rawValue3, rawValue4;
                    float sVal, value;
                    if (blendStartX)
                    {
                        if (sampler.DataSource is ByteArea)
                        {
                            rawValue1 = singleSampler.GetByte(xPos, yPos);
                            rawValue2 = singleSampler.GetByte(xPos - onePixel.Width, yPos);
                            rawValue3 = singleSampler.GetByte(xPos, yPos + onePixel.Height);
                            rawValue4 = singleSampler.GetByte(xPos - onePixel.Width, yPos + onePixel.Height);
                        }
                        else
                        {
                            rawValue1 = singleSampler[xPos, yPos];
                            rawValue2 = singleSampler[xPos - onePixel.Width, yPos];
                            rawValue3 = singleSampler[xPos, yPos + onePixel.Height];
                            rawValue4 = singleSampler[xPos - onePixel.Width, yPos + onePixel.Height];
                        }
                        sVal = ((rawValue1 / maxValue) + (rawValue2 / maxValue) + (rawValue3 / maxValue) + (rawValue4 / maxValue)) / 4;
                        value = -0.5f + sVal;
                        heights[vPos++] = value;

                        xPos += xAdv;
                        xStart++;
                    }

                    if (blendEndX)
                        xEnd--;

                    // do normal range
                    for (int x = xStart; x < xEnd; x++)
                    {
                        if (sampler.DataSource is ByteArea)
                        {
                            rawValue1 = singleSampler.GetByte(xPos, yPos);
                            rawValue2 = singleSampler.GetByte(xPos, yPos + onePixel.Height);
                        }
                        else
                        {
                            rawValue1 = singleSampler[xPos, yPos];
                            rawValue2 = singleSampler[xPos, yPos + onePixel.Height];
                        }
                        sVal = ((rawValue1 / maxValue) + (rawValue2 / maxValue)) / 2;
                        value = -0.5f + sVal;
                        heights[vPos++] = value;

                        xPos += xAdv;
                    }

                    // xy edge
                    if (blendEndX)
                    {
                        if (sampler.DataSource is ByteArea)
                        {
                            rawValue1 = singleSampler.GetByte(xPos, yPos);
                            rawValue2 = singleSampler.GetByte(xPos + onePixel.Width, yPos);
                            rawValue3 = singleSampler.GetByte(xPos, yPos + onePixel.Height);
                            rawValue4 = singleSampler.GetByte(xPos + onePixel.Width, yPos + onePixel.Height);
                        }
                        else
                        {
                            rawValue1 = singleSampler[xPos, yPos];
                            rawValue2 = singleSampler[xPos + onePixel.Width, yPos];
                            rawValue3 = singleSampler[xPos, yPos + onePixel.Height];
                            rawValue4 = singleSampler[xPos + onePixel.Width, yPos + onePixel.Height];
                        }
                        sVal = ((rawValue1 / maxValue) + (rawValue2 / maxValue) + (rawValue3 / maxValue) + (rawValue4 / maxValue)) / 4;
                        value = -0.5f + sVal;
                        heights[vPos++] = value;
                    }
                }
            }

            // pick up additional heights for internal borders

            // generate triangle normals from heights
            Vector3[] vNormals = new Vector3[xSamples * ySamples];
            vPos = 0;
            Vector3 v0 = new Vector3(0, 0, 0);
            Vector3 v1 = new Vector3(0, 0, 1);
            Vector3 v2 = new Vector3(1, 0, 0);
            Vector3 v3 = new Vector3(1, 0, 1);
            for (int y = 0; y < ySamples - 1; y++)
            {
                for (int x = 0; x < xSamples - 1; x++)
                {
                    v0.Y = heights[vPos];
                    v1.Y = heights[vPos + xSamples];
                    v2.Y = heights[vPos + 1];
                    v3.Y = heights[vPos + xSamples + 1];

                    Vector3 e1 = v1 - v0;
                    Vector3 e2 = v2 - v0;
                    Vector3 normal = Vector3.Normalize(Vector3.Cross(e1, e2));

                    vNormals[vPos] += normal;
                    vNormals[vPos + xSamples] += normal;
                    vNormals[vPos + 1] += normal;

                    e1 = v1 - v2;
                    e2 = v3 - v2;
                    normal = Vector3.Normalize(Vector3.Cross(e1, e2));

                    vNormals[vPos + xSamples] += normal;
                    vNormals[vPos + 1] += normal;
                    vNormals[vPos + xSamples + 1] += normal;

                    vPos++;
                }
            }

            // calculate vertex normals and input heights into vBuffer
            vPos = 0;
            for (int x = 0; x < xSamples; x++)
            {
                Vector3 result = vNormals[vPos] * (1f / 4f);
                verts[vPos].Y = heights[vPos];
                verts[vPos].Normal = result;
                vPos++;
            }
            for (int y = 1; y < ySamples - 1; y++)
            {
                Vector3 result = vNormals[vPos] * (1f / 3f);
                verts[vPos].Y = heights[vPos];
                verts[vPos].Normal = result;
                vPos++;
                
                for (int x = 1; x < xSamples - 1; x++)
                {
                    result = vNormals[vPos] * (1f / 6f);
                    verts[vPos].Y = heights[vPos];
                    verts[vPos].Normal = result;
                    vPos++;
                }

                result = vNormals[vPos] * (1f / 3f);
                verts[vPos].Y = heights[vPos];
                verts[vPos].Normal = result;
                vPos++;
            }
            for (int x = 0; x < xSamples; x++)
            {
                Vector3 result = vNormals[vPos] * (1f / 4f);
                verts[vPos].Y = heights[vPos];
                verts[vPos].Normal = result;
                vPos++;
            }

            vBuffer.Unlock();

            CpuDEMSubGeometry geom = new CpuDEMSubGeometry();
            geom.vBuffer = vBuffer;
            geom.iBufSize = iBufSize;
            geom.skirts = skirts;
            return geom;
        }

        public static IndexBuffer[] CreateSampleTriStripsIndices(int xSamples, int ySamples, Device gDevice)
        {
            // TODO: Use 1 buffer but use segments?
            // NOTE: Can take max of 256x256
            IndexBuffer[] iBuffers = new IndexBuffer[ySamples - 1];
            for (short bufIdx = 0; bufIdx < ySamples - 1; bufIdx++)
            {
                iBuffers[bufIdx] = new IndexBuffer(typeof(short), /*(ySamples - 1) * */(xSamples * 2), gDevice, Usage.WriteOnly, Pool.Managed);
                short[] indices = (short[])iBuffers[bufIdx].Lock(0, LockFlags.None);

                // fill buffer
                int idx = 0;
                for (short i = (short)(xSamples * bufIdx); i < (xSamples * bufIdx) + xSamples/* - 1*/; i++)
                {
                    // NOTE: Won't work if not ^2
                    indices[idx++] = i;
                    indices[idx++] = (short)(i + xSamples);
                    
                    /*indices[idx++] = (short)(i + 1);
                    indices[idx++] = (short)(i + xSamples + 1);*/
                }
                iBuffers[bufIdx].Unlock();
            }
            return iBuffers;
        }

        public static IndexBuffer CreateTriStripSkirtIndices(int numSamples, Device gDevice, int index, int skirtIndex)
        {
            IndexBuffer iBuffer = new IndexBuffer(typeof(short), (numSamples * 2), gDevice, Usage.WriteOnly, Pool.Managed);
            short[] indices = (short[])iBuffer.Lock(0, LockFlags.None);

            int idx = 0;
            int end = (numSamples + skirtIndex) * numSamples;
            if (index == 0)
            {
                // left side
                for (int i = 0; i < numSamples; i++)
                {
                    indices[idx++] = (short)(i * numSamples);
                    indices[idx++] = (short)(end + i);
                }
            }
            else if (index == 1)
            {
                // right side
                for (int i = 0; i < numSamples; i++)
                {
                    indices[idx++] = (short)(end + i);
                    indices[idx++] = (short)(((i + 1) * numSamples) - 1);
                }
            }
            else if (index == 2)
            {
                // front side
                for (int i = 0; i < numSamples; i++)
                {
                    indices[idx++] = (short)(end + i);
                    indices[idx++] = (short)i;
                }
            }
            else if (index == 3)
            {
                // front side
                int geomEndM1 = numSamples * (numSamples - 1);
                for (int i = 0; i < numSamples; i++)
                {
                    indices[idx++] = (short)(geomEndM1 + i);
                    indices[idx++] = (short)(end + i);
                }
            }
            iBuffer.Unlock();

            return iBuffer;
        }
    }
}