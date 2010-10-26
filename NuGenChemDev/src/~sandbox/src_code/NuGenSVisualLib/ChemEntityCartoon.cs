using System;
using System.Collections.Generic;
using System.Text;
using NuGenJmol;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Drawing;

namespace NuGenSVisualLib.Rendering.Chem.Structures
{
    class ChemEntityCartoon : ChemEntityStructure
    {
        Device device;
        VertexBuffer[] triStrips;
        int[] triStripSizes;

        public ChemEntityCartoon(NuSceneBuffer3D sceneBuffer)
         : base(sceneBuffer)
        { }

        public override void Render()
        {
            // render all tri strips
            device.VertexFormat = CustomVertex.PositionNormal.Format;
            device.RenderState.Lighting = false;
            device.RenderState.DiffuseMaterialSource = ColorSource.Material;
            Material m = new Material();
            m.DiffuseColor = ColorValue.FromColor(Color.SeaGreen);
            device.Material = m;
            for (int strip = 0; strip < triStrips.Length; strip++)
            {
                // fronts
                device.SetStreamSource(0, triStrips[strip], 0);
                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, triStripSizes[strip]);
                //device.RenderState.CullMode = Cull.CounterClockwise;
                // backs
                //device.SetStreamSource(0, triStripsMirrored[strip], 0);
                //device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, triStripSizes[strip]);
                //device.RenderState.CullMode = Cull.Clockwise;
            }

            Matrix world = device.Transform.World * Matrix.Identity;
            foreach (NuSceneBuffer3D.NuBufferMeshItem mesh in sbLocalCopy.meshes)
            {
                device.Transform.World = Matrix.Translation(mesh.translation) * world;
                mesh.mesh.DrawSubset(0);
            }
        }

        public override void Init(Device device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Init(Device device, GeneralStructuresShadingDesc shading)
        {
            RebuildStructure(device, sbLocalCopy, shading);
        }

        public override void Dispose()
        {
        }

        protected void RebuildStructure(Device device, NuSceneBuffer3D sceneBuffer,
                                        GeneralStructuresShadingDesc shading)
        {
            boundingBox = GenerateBoundingBoxFromPoints(sceneBuffer.triangleStrips);
            // TODO: Use meshes also for BB calc

            // build tri strips
            List<Vector3[]> triStripsList = sceneBuffer.triangleStrips;

            triStrips = new VertexBuffer[triStripsList.Count];
            triStripSizes = new int[triStripsList.Count];
            for (int strip = 0; strip < triStripsList.Count; strip++)
            {
                Vector3[] points = triStripsList[strip];

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
                    //Vector3 v0 = verts[v - 1].Position;
                    //Vector3 v1 = verts[v].Position;
                    //Vector3 v2 = verts[v + 1].Position;

                    //Vector3 e1 = v1 - v0, e2 = v2 - v0;
                    //Vector3 vNormal = Vector3.Normalize(Vector3.Cross(e1, e2));

                    //verts[v - 1].Normal += vNormal;
                    //verts[v].Normal += vNormal;
                    //verts[v + 1].Normal += vNormal;

                    //if (v != 1)
                    //    verts[v - 1].Normal *= 0.5f;
                    //if (v == points.Length - 1)
                    //    verts[v].Normal *= 0.5f;
                }

                triStrips[strip].Unlock();
            }
        }
    }
}
