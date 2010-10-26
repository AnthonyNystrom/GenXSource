using System;
using System.Collections.Generic;
using System.Drawing;
using Genetibase.NuGenDEMVis.GIS;
using Genetibase.NuGenRenderCore.Rendering;
using Genetibase.NuGenRenderCore.Rendering.Helpers;
using Genetibase.NuGenRenderCore.Shaders;
using Genetibase.RasterDatabase.Geometry;
using Genetibase.VisUI.Maths;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenDEMVis
{
    class GpuDemGeometry : DEMGeometry
    {
        SortedList<ulong, CachedTexture> texturesCache;
        VertexBuffer geomPlaneVerts, geomPatchVerts;
        IndexBuffer geomPlaneIndices, geomPatchIndices;

        PureQuadTree demPQT;
        List<GpuDemSubGeometry> geometryTree;

        GDALReader reader;
        RectangleGroupQuadTree.GroupNode[][] dataNodes;

        enum Directions
        {
            Up = 1,
            Down = 2,
            Left = 4,
            Right = 8
        }

        class CachedTexture
        {
            public Texture Heights, Normals;
            public ulong LocationCode;
            public ushort Level;

            public CachedTexture(Texture tex, ulong locationCode, ushort level, Texture normals)
            {
                Heights = tex;
                Normals = normals;
                LocationCode = locationCode;
                Level = level;
            }
        }

        class GpuDemSubGeometry : IDemSubGeometry
        {
            private readonly Vector2 position;
            private readonly Vector2 dimensions;
            private readonly Vector2 center;
            private GpuDemSubGeometry[] subGeometry;

            ulong code;
            readonly ushort level;
            ulong[] sideCodes;
            byte childNum;

            /// <summary>
            /// Initializes a new instance of the GpuDemSubGeometry class.
            /// </summary>
            /// <param name="position"></param>
            /// <param name="dimensions"></param>
            /// <param name="center"></param>
            /// <param name="subGeometry"></param>
            /// <param name="code"></param>
            /// <param name="level"></param>
            /// <param name="sideCodes"></param>
            public GpuDemSubGeometry(Vector2 position, Vector2 dimensions, Vector2 center,
                                     GpuDemSubGeometry[] subGeometry, ulong code, ushort level,
                                     ulong[] sideCodes, byte childNum)
            {
                this.position = position;
                this.sideCodes = sideCodes;
                this.childNum = childNum;
                this.dimensions = dimensions;
                this.center = center;
                this.subGeometry = subGeometry;
                this.code = code;
                this.level = level;
            }

            #region IDemSubGeometry Members

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
                get { return 0; }
            }

            public uint PrimitiveCount
            {
                get { return 0; }
            }

            public PrimitiveType PrimitivesType
            {
                get { return PrimitiveType.TriangleList; }
            }

            public IDemSubGeometry[] SubGeometry
            {
                get { return subGeometry; }
            }
            #endregion

            public ushort Level
            {
                get { return level; }
            }

            public ulong Code
            {
                get { return code; }
            }

            public ulong[] SideCodes
            {
                get { return sideCodes; }
            }

            public byte ChildNum
            {
                get { return childNum; }
            }
        }

        public GpuDemGeometry(Vector3 position, Vector3 dimensions, Vector3 center,
                              ulong vertexCount, uint primitiveCount, PrimitiveType primitivesType,
                              Device gDevice)
            : base(position, dimensions, center, vertexCount, primitiveCount, primitivesType, gDevice)
        {
            texturesCache = new SortedList<ulong, CachedTexture>();
            demPQT = new PureQuadTree(new Vector2(position.X, position.Z), new Vector2(dimensions.X, dimensions.Z));
            geometryTree = new List<GpuDemSubGeometry>();
        }

        public void ViewUpdated(Vector3 viewPos)
        {
            // TODO: determine if tree changes needed
            // TODO: Combine vis testing with tree
            demPQT.Fork(1, false);
            demPQT.Children[3].Fork(1, false);
            demPQT.Children[3].Children[3].Fork(1, false);
            demPQT.Children[1].Fork(1, false);
            demPQT.Children[1].Children[3].Fork(1, false);
            demPQT.Children[1].Children[1].Fork(1, false);

            // build local objects needed
            SourceDataDiffuseSampler sampler = new SourceDataDiffuseSampler();
            NormalMapGenerator normalGen = new NormalMapGenerator();
            foreach (PureQuadTreeNode leaf in demPQT)
            {
                if (leaf.Level != 0)
                {
                    // trace each leaf to see if patches required
                    ulong[] codes;
                    byte dirs = TraceLeaf(leaf, out codes);
                    TestInternals(leaf, ref codes);

                    // create object
                    geometryTree.Add(new GpuDemSubGeometry(leaf.Location, leaf.Size, leaf.Centre, null, leaf.Code, leaf.Level, codes, (byte)leaf.ChildNum));
                    // find matching data node and create texture if needed
                    if (!texturesCache.ContainsKey(leaf.Code))
                    {
                        Point pos = new Point((int)(leaf.Location.X / 10f * /*1024f*/2048f), (int)(leaf.Location.Y / 10f * /*1024f*/2048f));
                        // find corrosponding node
                        Texture texture = null, normalTex = null;
                        for (int node = 0; node < dataNodes[leaf.Level].Length; node++)
                        {
                            if (dataNodes[leaf.Level][node].NodeArea.Location == pos)
                            {
                                texture = //TextureLoader.FromFile(gDevice, "c:/0-test.jpg");
                                    sampler.GenerateTexture(new Size(16, 16), dataNodes[leaf.Level][node].Rectangles[0],
                                                                  gDevice, reader);
                                normalTex = normalGen.GenerateTexture(new Size(256, 256),
                                                                      dataNodes[leaf.Level][node].Rectangles[0],
                                                                      gDevice, reader);
                            }
                        }
                        texturesCache[leaf.Code] = new CachedTexture(texture, leaf.Code, leaf.Level, normalTex);
                    }
                }
            }
        }

        private byte TraceLeaf(PureQuadTreeNode leaf, out ulong[] codes)
        {
            byte result = 0;
            codes = new ulong[] { ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue };
            // decide which directions to look at
            switch (leaf.ChildNum)
            {
                case 0:
                    if (TraceNode(leaf, true, ref codes[0]))
                    {
                        result = (byte)Directions.Left;
                        if (TraceNode(leaf, false, ref codes[3]))
                            result |= (byte)Directions.Down;
                    }
                    else if (TraceNode(leaf, false, ref codes[3]))
                        result = (byte)Directions.Down;
                    break;
                case 1:
                    if (TraceNode(leaf, true, ref codes[1]))
                    {
                        result = (byte)Directions.Right;
                        if (TraceNode(leaf, false, ref codes[3]))
                            result |= (byte)Directions.Down;
                    }
                    else if (TraceNode(leaf, false, ref codes[3]))
                        result = (byte)Directions.Down;
                    break;
                case 2:
                    if (TraceNode(leaf, true, ref codes[0]))
                    {
                        result = (byte)Directions.Left;
                        if (TraceNode(leaf, false, ref codes[2]))
                            result |= (byte)Directions.Up;
                    }
                    else if (TraceNode(leaf, false, ref codes[2]))
                        result = (byte)Directions.Up;
                    break;
                case 3:
                    if (TraceNode(leaf, true, ref codes[1]))
                    {
                        result = (byte)Directions.Right;
                        if (TraceNode(leaf, false, ref codes[2]))
                            result |= (byte)Directions.Up;
                        break;
                    }
                    else if (TraceNode(leaf, false, ref codes[2]))
                        result = (byte)Directions.Up;
                    break;
            }
            return result;
        }

        private void TestInternals(PureQuadTreeNode leaf, ref ulong[] codes)
        {
            if (leaf.ChildNum == 0)
            {
                if (leaf.Parent.Children[1].Children == null)
                {
                    codes[1] = leaf.Parent.Children[1].Code;
                }
                if (leaf.Parent.Children[2].Children == null)
                {
                    codes[2] = leaf.Parent.Children[2].Code;
                }
            }
            else if (leaf.ChildNum == 1)
            {
                if (leaf.Parent.Children[0].Children == null)
                {
                    codes[0] = leaf.Parent.Children[0].Code;
                }
                if (leaf.Parent.Children[3].Children == null)
                {
                    codes[2] = leaf.Parent.Children[3].Code;
                }
            }
            else if (leaf.ChildNum == 2)
            {
                if (leaf.Parent.Children[0].Children == null)
                {
                    codes[3] = leaf.Parent.Children[0].Code;
                }
                if (leaf.Parent.Children[3].Children == null)
                {
                    codes[1] = leaf.Parent.Children[3].Code;
                }
            }
            else if (leaf.ChildNum == 3)
            {
                if (leaf.Parent.Children[2].Children == null)
                {
                    codes[0] = leaf.Parent.Children[2].Code;
                }
                if (leaf.Parent.Children[1].Children == null)
                {
                    codes[3] = leaf.Parent.Children[1].Code;
                }
            }
        }

        private static int FlipNode(ushort node, bool axis)
        {
            switch (node)
            {
                case 0:
                    if (axis)
                        return 1;
                    else
                        return 2;
                case 1:
                    if (axis)
                        return 0;
                    else
                        return 3;
                case 2:
                    if (axis)
                        return 3;
                    else
                        return 0;
                case 3:
                    if (axis)
                        return 2;
                    else
                        return 1;
            }
            throw new Exception("Unknown flip params");
        }


        private bool TraceNode(PureQuadTreeNode node, bool axis, ref ulong result)
        {
            PureQuadTreeNode current = node.Parent;
            // move upwards until we can switch over
            Stack<byte> upTrace = new Stack<byte>();
            upTrace.Push((byte)node.ChildNum);
            while (current != null && current.Level > 0)
            {
                bool flip = false;
                if (axis)
                {
                    if (node.ChildNum == 0 || node.ChildNum == 2)
                    {
                        if (current.ChildNum == 1 || current.ChildNum == 3)
                            flip = true;
                    }
                    else if (node.ChildNum == 1 || node.ChildNum == 3)
                    {
                        if (current.ChildNum == 0 || current.ChildNum == 2)
                            flip = true;
                    }
                }
                else
                {
                    if (node.ChildNum == 0 || node.ChildNum == 1)
                    {
                        if (current.ChildNum == 2 || current.ChildNum == 3)
                            flip = true;
                    }
                    else if (node.ChildNum == 2 || node.ChildNum == 3)
                    {
                        if (current.ChildNum == 0 || current.ChildNum == 1)
                            flip = true;
                    }
                }
                if (flip)
                {
                    // flip this and move down tree by slipping the stack until we hit and end
                    //upTrace.Push((byte)current.ChildNum);
                    int target = FlipNode(current.ChildNum, axis);
                    current = current.Parent.Children[target];
                    ushort level = current.Level;
                    ulong code = current.Code;
                    while (current != null)
                    {
                        level = current.Level;
                        code = current.Code;
                        if (level == node.Level)
                            break;
                        if (level >= node.Level)
                            return false;

                        target = FlipNode(upTrace.Pop(), axis);
                        if (current.Children != null)
                            current = current.Children[target];
                        else
                            current = null;
                    }
                    result = code;
                    return true;
                }
                upTrace.Push((byte)current.ChildNum);
                current = current.Parent;
            }
            return false;
        }


        public override IDemSubGeometry[] SubGeometry
        {
            get { return geometryTree.ToArray(); }
        }

        public override void Render(GraphicsPipeline gPipe)
        {
            // draw visible tree
            if (geomPlaneVerts != null)
            {
                gDevice.RenderState.Lighting = false;
                gDevice.SetTexture(0, null);

                ShaderInterface shader = gPipe.ShaderIf;

                foreach (GpuDemSubGeometry subGeom in geometryTree)
                {
                    //if (subGeom.Level != 2 || subGeom.Code == 60)
                    //    continue;
                    gPipe.Push();

                    shader.Effect.Technique = shader.Effect.GetTechnique("Basic");
                    shader.Effect.Begin(FX.None);
                    shader.Effect.BeginPass(0);

                    // draw main chunks - i.e. the middles
                    gDevice.Indices = geomPlaneIndices;
                    gDevice.SetStreamSource(0, geomPlaneVerts, 0);

                    gDevice.VertexFormat = CustomVertex.PositionTextured.Format;
                    
                    gPipe.ShaderIf.Effect.SetValue("dem_level", subGeom.Level);
                    gPipe.ShaderIf.Effect.SetValue("DiffuseTexture", texturesCache[subGeom.Code].Heights);
                    gPipe.ShaderIf.Effect.SetValue("NormalMapTexture", texturesCache[subGeom.Code].Normals);

                    gPipe.WorldMatrix = Matrix.Scaling(new Vector3(subGeom.Dimensions.X, 1, subGeom.Dimensions.Y)) *
                                        Matrix.Translation(new Vector3(subGeom.Position.X, 0, subGeom.Position.Y)) *
                                        gPipe.WorldMatrix;
                    
                    gDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 14 * 14, 0, 13 * 13 * 2);

                    // draw any non-blending patches - i.e. fill the gaps
                    //gDevice.VertexFormat = PlaneHelper.PatchVertex.Format;
                    gDevice.VertexFormat = CustomVertex.PositionColoredTextured.Format;
                    gDevice.SetStreamSource(0, geomPatchVerts, 0);
                    gDevice.Indices = geomPatchIndices;

                    if (subGeom.SideCodes[3] == ulong.MaxValue)
                        gDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, (16 * 4)/* + 2*/, 0, 16 * 2, 3 * 15 * 2, 15 * 2);
                    if (subGeom.SideCodes[2] == ulong.MaxValue)
                        gDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, (16 * 6)/* + 2*/, 0, 16 * 2, 3 * 15 * 2, 15 * 2);
                    if (subGeom.SideCodes[0] == ulong.MaxValue)
                        gDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, /*2*/0, 0, 16 * 2, 0, 15 * 2);
                    if (subGeom.SideCodes[1] == ulong.MaxValue)
                        gDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, (16 * 2)/* + 2*/, 0, 16 * 2, 0, 15 * 2);

                    shader.Effect.EndPass();
                    shader.Effect.End();

                    // draw patches required
                    if (subGeom.SideCodes[3] != ulong.MaxValue)
                    {
                        CachedTexture tex = texturesCache[subGeom.SideCodes[3]];
                        gPipe.ShaderIf.Effect.SetValue("NextLevelHeightTexture", tex.Heights);
                        if (tex.Level == subGeom.Level)
                        {
                            shader.Effect.Technique = shader.Effect.GetTechnique("Basic_Patch_Int");
                            shader.Effect.Begin(FX.None);
                            shader.Effect.BeginPass(0);
                        }
                        else
                        {
                            if (subGeom.ChildNum == 1)
                            {
                                shader.Effect.Technique = shader.Effect.GetTechnique("Basic_Patch_Inv");
                                shader.Effect.Begin(FX.None);
                                shader.Effect.BeginPass(0);

                                gPipe.ShaderIf.Effect.SetValue("shift", 0.5f);
                                gPipe.ShaderIf.Effect.SetValue("scale", 0.5f);
                            }
                            else
                            {
                                shader.Effect.Technique = shader.Effect.GetTechnique("Basic_Patch");
                                shader.Effect.Begin(FX.None);
                                shader.Effect.BeginPass(0);

                                gPipe.ShaderIf.Effect.SetValue("shift", 0);
                                gPipe.ShaderIf.Effect.SetValue("scale", 0.5f);
                            }
                        }
                        gPipe.ShaderIf.Effect.SetValue("NormalMapTexture", texturesCache[subGeom.Code].Normals);
                        gPipe.ShaderIf.Effect.SetValue("axis", 0);
                        gPipe.ShaderIf.Effect.CommitChanges();

                        gDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, (16 * 4) + 2, 0, 16 * 2, 3 * 15 * 2, 13 * 2);

                        shader.Effect.EndPass();
                        shader.Effect.End();
                    }
                    if (subGeom.SideCodes[2] != ulong.MaxValue)
                    {
                        CachedTexture tex = texturesCache[subGeom.SideCodes[2]];
                        gPipe.ShaderIf.Effect.SetValue("NextLevelHeightTexture", tex.Heights);
                        if (tex.Level == subGeom.Level)
                        {
                            shader.Effect.Technique = shader.Effect.GetTechnique("Basic_Patch_Int");
                            shader.Effect.Begin(FX.None);
                            shader.Effect.BeginPass(0);
                        }
                        else
                        {
                            if (subGeom.ChildNum == 3)
                            {
                                shader.Effect.Technique = shader.Effect.GetTechnique("Basic_Patch_Inv");
                                shader.Effect.Begin(FX.None);
                                shader.Effect.BeginPass(0);

                                gPipe.ShaderIf.Effect.SetValue("shift", 0.5f);
                                gPipe.ShaderIf.Effect.SetValue("scale", 0.5f);
                            }
                            else
                            {
                                shader.Effect.Technique = shader.Effect.GetTechnique("Basic_Patch");
                                shader.Effect.Begin(FX.None);
                                shader.Effect.BeginPass(0);

                                gPipe.ShaderIf.Effect.SetValue("shift", 0);
                                gPipe.ShaderIf.Effect.SetValue("scale", 0.5f);
                            }
                        }
                        gPipe.ShaderIf.Effect.SetValue("NormalMapTexture", texturesCache[subGeom.Code].Normals);
                        gPipe.ShaderIf.Effect.SetValue("axis", 0);
                        gPipe.ShaderIf.Effect.CommitChanges();

                        gDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, (16 * 6) + 2, 0, 16 * 2, 3 * 15 * 2, 13 * 2);

                        shader.Effect.EndPass();
                        shader.Effect.End();
                    }
                    if (subGeom.SideCodes[0] != ulong.MaxValue)
                    {
                        CachedTexture tex = texturesCache[subGeom.SideCodes[0]];
                        gPipe.ShaderIf.Effect.SetValue("NextLevelHeightTexture", tex.Heights);
                        if (tex.Level == subGeom.Level)
                        {
                            shader.Effect.Technique = shader.Effect.GetTechnique("Basic_Patch_Int");
                            shader.Effect.Begin(FX.None);
                            shader.Effect.BeginPass(0);
                        }
                        else
                        {
                            if (subGeom.ChildNum == 2)
                            {
                                shader.Effect.Technique = shader.Effect.GetTechnique("Basic_Patch_Inv");
                                shader.Effect.Begin(FX.None);
                                shader.Effect.BeginPass(0);

                                gPipe.ShaderIf.Effect.SetValue("shift", 0.5f);
                                gPipe.ShaderIf.Effect.SetValue("scale", 0.5f);
                            }
                            else
                            {
                                shader.Effect.Technique = shader.Effect.GetTechnique("Basic_Patch");
                                shader.Effect.Begin(FX.None);
                                shader.Effect.BeginPass(0);

                                gPipe.ShaderIf.Effect.SetValue("shift", 0);
                                gPipe.ShaderIf.Effect.SetValue("scale", 0.5f);
                            }
                        }
                        gPipe.ShaderIf.Effect.SetValue("NormalMapTexture", texturesCache[subGeom.Code].Normals);
                        gPipe.ShaderIf.Effect.SetValue("axis", 1);
                        gPipe.ShaderIf.Effect.CommitChanges();

                        gDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 2, 0, 16 * 2, 0, 13 * 2);

                        shader.Effect.EndPass();
                        shader.Effect.End();
                    }
                    if (subGeom.SideCodes[1] != ulong.MaxValue)
                    {
                        CachedTexture tex = texturesCache[subGeom.SideCodes[1]];
                        gPipe.ShaderIf.Effect.SetValue("NextLevelHeightTexture", tex.Heights);
                        if (tex.Level == subGeom.Level)
                        {
                            shader.Effect.Technique = shader.Effect.GetTechnique("Basic_Patch_Int");
                            shader.Effect.Begin(FX.None);
                            shader.Effect.BeginPass(0);
                        }
                        else
                        {
                            if (subGeom.ChildNum == 0)
                            {
                                shader.Effect.Technique = shader.Effect.GetTechnique("Basic_Patch_Inv");
                                shader.Effect.Begin(FX.None);
                                shader.Effect.BeginPass(0);

                                gPipe.ShaderIf.Effect.SetValue("shift", 0.5f);
                                gPipe.ShaderIf.Effect.SetValue("scale", 0.5f);
                            }
                            else
                            {
                                shader.Effect.Technique = shader.Effect.GetTechnique("Basic_Patch");
                                shader.Effect.Begin(FX.None);
                                shader.Effect.BeginPass(0);

                                gPipe.ShaderIf.Effect.SetValue("shift", 0);
                                gPipe.ShaderIf.Effect.SetValue("scale", 0.5f);
                            }
                        }
                        gPipe.ShaderIf.Effect.SetValue("NormalMapTexture", texturesCache[subGeom.Code].Normals);
                        gPipe.ShaderIf.Effect.SetValue("axis", 1);
                        gPipe.ShaderIf.Effect.CommitChanges();

                        gDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, (16 * 2) + 2, 0, 16 * 2, 0, 13 * 2);

                        shader.Effect.EndPass();
                        shader.Effect.End();
                    }

                    //TextureLoader.Save("c:/" + subGeom.Code + ".jpg", ImageFileFormat.Jpg, texturesCache[subGeom.Code].Tex);

                    gPipe.Pop();
                }
            }
        }

        public override void Dispose()
        {
            if (geomPlaneVerts != null && !geomPlaneVerts.Disposed)
                geomPlaneVerts.Dispose();
            if (geomPlaneIndices != null && !geomPlaneIndices.Disposed)
                geomPlaneIndices.Dispose();
        }

        public static GpuDemGeometry CreateGeometry(RasterDatabase.RasterDatabase rDatabase, Device gDevice,
                                                    out float maxValue, GDALReader reader)
        {
            // create actual geometry - tesselated plane
            IndexBuffer geomPlaneIndices;
            PlaneHelper.CreateIndexBuffer(gDevice, 14, 14, out geomPlaneIndices);
            VertexBuffer geomPlaneVerts;
            PlaneHelper.CreateVertexBufferInside(gDevice, 16, 16, new Vector2(1, 1), out geomPlaneVerts);
            VertexBuffer geomPatchVerts;
            PlaneHelper.CreatePatchVertexBuffer(gDevice, 16, 16, new Vector2(1, 1), out geomPatchVerts);
            IndexBuffer geomPatchIndices;
            PlaneHelper.CreatePatchIndexBuffer(gDevice, 16, 16, out geomPatchIndices);

            // create cache of all depths
            RectangleGroupQuadTree rTree = rDatabase.ProduceLayerMipMap(0, 256);
            RectangleGroupQuadTree.GroupNode[][] dataNodes = new RectangleGroupQuadTree.GroupNode[rTree.Depth][];
            for (int depth = 0; depth < rTree.Depth; depth++)
            {
                RectangleGroupQuadTree.GroupNode[] nodes;
                rTree.GetNodes(depth + 1, out nodes);
                dataNodes[depth] = nodes;
            }

            GpuDemGeometry geom = new GpuDemGeometry(new Vector3(), new Vector3(10, 0, 10), new Vector3(5, 0, 5),
                                                     16 * 16, 15 * 15 * 2, PrimitiveType.TriangleList,
                                                     gDevice);
            geom.geomPlaneVerts = geomPlaneVerts;
            geom.geomPlaneIndices = geomPlaneIndices;
            geom.geomPatchIndices = geomPatchIndices;
            geom.geomPatchVerts = geomPatchVerts;
            geom.reader = reader;
            geom.dataNodes = dataNodes;
            geom.ViewUpdated(new Vector3());
            
            maxValue = 1;
            return geom;
        }
    }
}