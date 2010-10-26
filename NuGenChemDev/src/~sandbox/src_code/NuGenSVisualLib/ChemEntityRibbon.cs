using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using NuGenJmol;
using System.Drawing;
using Microsoft.DirectX;

namespace NuGenSVisualLib.Rendering.Chem.Structures
{
    /// <summary>
    /// Encapsulates a Ribbon structure, or a set of ribbons
    /// </summary>
    class ChemEntityRibbon : ChemEntityStructure
    {
        Device device;
        VertexBuffer[] triStrips, triStripsMirrored;
        int[] triStripSizes;
        VertexFormats vFormat;

        public ChemEntityRibbon(NuSceneBuffer3D sceneBuffer)
            : base(sceneBuffer)
        { }

        public override void Render()
        {
            // render all tri strips
            device.VertexFormat = vFormat;
            device.RenderState.Lighting = true;
            device.RenderState.DiffuseMaterialSource = ColorSource.Material;
            Material m = new Material();
            m.DiffuseColor = ColorValue.FromColor(Color.Orange);
            device.Material = m;
            for (int strip=0; strip < triStrips.Length; strip++)
            {
                // fronts
                device.SetStreamSource(0, triStrips[strip], 0);
                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, triStripSizes[strip]);
                device.RenderState.CullMode = Cull.CounterClockwise;
                // backs
                device.SetStreamSource(0, triStripsMirrored[strip], 0);
                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, triStripSizes[strip]);
                device.RenderState.CullMode = Cull.Clockwise;
            }
        }

        public override void Init(Device device, GeneralStructuresShadingDesc shading)
        {
            RebuildStructure(device, sbLocalCopy, shading);
        }

        protected void RebuildStructure(Device device, NuSceneBuffer3D sceneBuffer,
                                        GeneralStructuresShadingDesc shading)
        {
            boundingBox = GenerateBoundingBoxFromPoints(sceneBuffer.triangleStrips);

            bool useClrAxis = shading.RibbonsShadingDesc.PropertiesInUse.ContainsKey("UseClrAxis");
            Vector4 xClrStep = new Vector4(), yClrStep = new Vector4(), zClrStep = new Vector4();
            Vector4 xClrStart = new Vector4(), yClrStart = new Vector4(), zClrStart = new Vector4();
            if (useClrAxis)
            {
                // pre-calculate colour ranges scale from bounding box
                Color xClrA, xClrB;
                if (shading.RibbonsShadingDesc.Clrs.XEnabled)
                {
                    xClrA = shading.RibbonsShadingDesc.Clrs.Xa;
                    xClrB = shading.RibbonsShadingDesc.Clrs.Xb;
                }
                else
                    xClrA = xClrB = shading.RibbonsShadingDesc.DefaultClr;
                Color yClrA, yClrB;
                if (shading.RibbonsShadingDesc.Clrs.YEnabled)
                {
                    yClrA = shading.RibbonsShadingDesc.Clrs.Ya;
                    yClrB = shading.RibbonsShadingDesc.Clrs.Yb;
                }
                else
                    yClrA = yClrB = shading.RibbonsShadingDesc.DefaultClr;
                Color zClrA, zClrB;
                if (shading.RibbonsShadingDesc.Clrs.ZEnabled)
                {
                    zClrA = shading.RibbonsShadingDesc.Clrs.Za;
                    zClrB = shading.RibbonsShadingDesc.Clrs.Zb;
                }
                else
                    zClrA = zClrB = shading.RibbonsShadingDesc.DefaultClr;

                float difA = xClrB.A - xClrA.A;
                float difR = xClrB.R - xClrA.R;
                float difG = xClrB.G - xClrA.G;
                float difB = xClrB.B - xClrA.B;

                // calc steps
                xClrStep = new Vector4(difA / boundingBox.Dimensions.X,
                                       difR / boundingBox.Dimensions.X,
                                       difG / boundingBox.Dimensions.X,
                                       difB / boundingBox.Dimensions.X);

                difA = yClrB.A - yClrA.A;
                difR = yClrB.R - yClrA.R;
                difG = yClrB.G - yClrA.G;
                difB = yClrB.B - yClrA.B;
                yClrStep = new Vector4(difA / boundingBox.Dimensions.Y,
                                       difR / boundingBox.Dimensions.Y,
                                       difG / boundingBox.Dimensions.Y,
                                       difB / boundingBox.Dimensions.Y);

                difA = zClrB.A - zClrA.A;
                difR = zClrB.R - zClrA.R;
                difG = zClrB.G - zClrA.G;
                difB = zClrB.B - zClrA.B;
                zClrStep = new Vector4(difA / boundingBox.Dimensions.Z,
                                       difR / boundingBox.Dimensions.Z,
                                       difG / boundingBox.Dimensions.Z,
                                       difB / boundingBox.Dimensions.Z);
            }

            if (shading.RibbonsShadingDesc.ShadingType == RibbonsShadingDesc.Shading.Solid)
            {
                // make triangle strips
                List<Vector3[]> triStripsList = sceneBuffer.triangleStrips;

                triStrips = new VertexBuffer[triStripsList.Count];
                triStripsMirrored = new VertexBuffer[triStripsList.Count];
                triStripSizes = new int[triStripsList.Count];
                for (int strip = 0; strip < triStripsList.Count; strip++)
                {
                    Vector3[] points = triStripsList[strip];
                    if (useClrAxis)
                    {
                        vFormat = CustomVertex.PositionNormalColored.Format;
                        triStrips[strip] = new VertexBuffer(typeof(CustomVertex.PositionNormalColored),
                                                            points.Length, device, Usage.None,
                                                            CustomVertex.PositionNormalColored.Format,
                                                            Pool.Managed);
                        triStripSizes[strip] = points.Length - 2;
                        CustomVertex.PositionNormalColored[] verts = (CustomVertex.PositionNormalColored[])
                                                                     triStrips[strip].Lock(0, LockFlags.None);
                        float xStart = boundingBox.Centre.X - (boundingBox.Dimensions.X / 2.0f);
                        float yStart = boundingBox.Centre.Y - (boundingBox.Dimensions.Y / 2.0f);
                        float zStart = boundingBox.Centre.Z - (boundingBox.Dimensions.Z / 2.0f);
                        for (int v = 1; v < points.Length - 1; v++)
                        {
                            // V1
                            float xSteps = points[v - 1].X - xStart;
                            float ySteps = points[v - 1].Y - yStart;
                            float zSteps = points[v - 1].Z - zStart;

                            // blend XYZ colours
                            int A = (int)((xClrStep.X * xSteps) + (yClrStep.X * ySteps) + (zClrStep.X * zSteps));
                            int R = (int)((xClrStep.Y * xSteps) + (yClrStep.Y * ySteps) + (zClrStep.Y * zSteps));
                            int G = (int)((xClrStep.Z * xSteps) + (yClrStep.Z * ySteps) + (zClrStep.Z * zSteps));
                            int B = (int)((xClrStep.W * xSteps) + (yClrStep.W * ySteps) + (zClrStep.W * zSteps));

                            verts[v - 1].Color = Color.FromArgb(A, R, G, B).ToArgb();
                            verts[v - 1].Position = points[v - 1];

                            // V2
                            xSteps = points[v].X - xStart;
                            ySteps = points[v].Y - yStart;
                            zSteps = points[v].Z - zStart;

                            // blend XYZ colours
                            A = (int)((xClrStep.X * xSteps) + (yClrStep.X * ySteps) + (zClrStep.X * zSteps));
                            R = (int)((xClrStep.Y * xSteps) + (yClrStep.Y * ySteps) + (zClrStep.Y * zSteps));
                            G = (int)((xClrStep.Z * xSteps) + (yClrStep.Z * ySteps) + (zClrStep.Z * zSteps));
                            B = (int)((xClrStep.W * xSteps) + (yClrStep.W * ySteps) + (zClrStep.W * zSteps));

                            verts[v].Color = Color.FromArgb(A, R, G, B).ToArgb();
                            verts[v].Position = points[v];

                            // V3
                            xSteps = points[v + 1].X - xStart;
                            ySteps = points[v + 1].Y - yStart;
                            zSteps = points[v + 1].Z - zStart;

                            // blend XYZ colours
                            A = (int)((xClrStep.X * xSteps) + (yClrStep.X * ySteps) + (zClrStep.X * zSteps));
                            R = (int)((xClrStep.Y * xSteps) + (yClrStep.Y * ySteps) + (zClrStep.Y * zSteps));
                            G = (int)((xClrStep.Z * xSteps) + (yClrStep.Z * ySteps) + (zClrStep.Z * zSteps));
                            B = (int)((xClrStep.W * xSteps) + (yClrStep.W * ySteps) + (zClrStep.W * zSteps));

                            verts[v + 1].Color = Color.FromArgb(A, R, G, B).ToArgb();
                            verts[v + 1].Position = points[v + 1];


                            // normals
                            Vector3 v0 = verts[v + 1].Position;
                            Vector3 v1 = verts[v].Position;
                            Vector3 v2 = verts[v - 1].Position;

                            Vector3 e1 = v1 - v0, e2 = v2 - v0;
                            Vector3 vNormal = Vector3.Normalize(Vector3.Cross(e1, e2));

                            verts[v - 1].Normal += vNormal;
                            verts[v].Normal += vNormal;
                            verts[v + 1].Normal += vNormal;

                            if (v != 1)
                                verts[v - 1].Normal *= 0.5f;
                            if (v == points.Length - 1)
                                verts[v].Normal *= 0.5f;
                        }

                        // mirror for double-sided
                        triStripsMirrored[strip] = new VertexBuffer(typeof(CustomVertex.PositionNormalColored),
                                                                    points.Length, device, Usage.None,
                                                                    CustomVertex.PositionNormalColored.Format,
                                                                    Pool.Managed);
                        // clone
                        triStripsMirrored[strip].SetData(verts, 0, LockFlags.None);
                        triStrips[strip].Unlock();
                        // flip
                        FlipNormalsVB(triStripsMirrored[strip], CustomVertex.PositionNormalColored.Format,
                                      triStripSizes[strip]);
                    }
                    else
                    {
                        vFormat = CustomVertex.PositionNormal.Format;
                        triStrips[strip] = new VertexBuffer(typeof(CustomVertex.PositionNormal),
                                                            points.Length, device, Usage.None,
                                                            CustomVertex.PositionNormal.Format,
                                                            Pool.Managed);
                        triStripSizes[strip] = points.Length - 2;
                        CustomVertex.PositionNormal[] verts = (CustomVertex.PositionNormal[])
                                                              triStrips[strip].Lock(0, LockFlags.None);
                        for (int v = 1; v < points.Length - 1; v++)
                        {
                            // positions
                            verts[v - 1].Position = points[v + 1];
                            verts[v].Position = points[v];
                            verts[v + 1].Position = points[v - 1];

                            // normals
                            Vector3 v0 = verts[v - 1].Position;
                            Vector3 v1 = verts[v].Position;
                            Vector3 v2 = verts[v + 1].Position;

                            Vector3 e1 = v1 - v0, e2 = v2 - v0;
                            Vector3 vNormal = Vector3.Normalize(Vector3.Cross(e1, e2));

                            verts[v - 1].Normal += vNormal;
                            verts[v].Normal += vNormal;
                            verts[v + 1].Normal += vNormal;

                            if (v != 1)
                                verts[v - 1].Normal *= 0.5f;
                            if (v == points.Length - 1)
                                verts[v].Normal *= 0.5f;
                        }
                        

                        // mirror for double-sided
                        triStripsMirrored[strip] = new VertexBuffer(typeof(CustomVertex.PositionNormal),
                                                                    points.Length, device, Usage.None,
                                                                    CustomVertex.PositionNormal.Format,
                                                                    Pool.Managed);
                        // clone
                        triStripsMirrored[strip].SetData(verts, 0, LockFlags.None);
                        triStrips[strip].Unlock();
                        // flip
                        FlipNormalsVB(triStripsMirrored[strip], CustomVertex.PositionNormal.Format,
                                      triStripSizes[strip]);
                    }
                }
            }
            else if (shading.RibbonsShadingDesc.ShadingType == RibbonsShadingDesc.Shading.Edges)
            {
            }
        }

        public override void Init(Device device)
        {
        }

        public override void Dispose()
        {
            if (triStrips != null)
            {
                foreach (VertexBuffer vb in triStrips)
                {
                    vb.Dispose();
                }
            }
        }
    }
}