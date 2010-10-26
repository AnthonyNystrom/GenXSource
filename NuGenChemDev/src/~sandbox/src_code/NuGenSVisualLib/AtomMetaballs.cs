using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Maths.Volumes;
using NuGenSVisualLib.Rendering.Chem.Materials;
using NuGenSVisualLib.Rendering.Effects;
using Org.OpenScience.CDK;
using Org.OpenScience.CDK.Interfaces;

namespace NuGenSVisualLib.Rendering.Chem
{
    class AtomMetaballsBufferCreator : AtomGeometryCreator
    {
        private bool pointsOnly = false;

        public AtomMetaballsBufferCreator(bool pointsOnly)
        {
            /*availableFields = new DataFields[] { new DataFields(VertexFormats.Position, "POSITION"),
                                                 new DataFields(VertexFormats.Normal, "NORMAL"),
                                                 new DataFields(VertexFormats.Diffuse, "DIFFUSE")
                                               };*/
            PointsOnly = pointsOnly;
        }

        public bool PointsOnly
        {
            get { return pointsOnly; }
            set
            {
                pointsOnly = value;
                if (!pointsOnly)
                {
                    availableFields = new DataFields[] { new DataFields(VertexFormats.Position, "POSITION"),
                                                 new DataFields(VertexFormats.Normal, "NORMAL"),
                                                 new DataFields(VertexFormats.Diffuse, "DIFFUSE")
                                               };
                }
                else
                {
                    availableFields = new DataFields[] { new DataFields(VertexFormats.Position, "POSITION"),
                                                 new DataFields(VertexFormats.Diffuse, "DIFFUSE")
                                               };
                }
            }
        }

        public override void CreateGeometryForObjects(Device device, ICollection<IAtom> objs,
                                                      GeomDataBufferStream geomStream, int stream,
                                                      ref BufferedGeometryData buffer, CompleteOutputDescription coDesc)
        {
            // fillable fields
            int positionPos = -1;
            int normalPos = -1;
            int diffusePos = -1;

            // match field locations
            for (int i = 0; i < fields.Length; i++)
            {
                for (int gf = 0; gf < geomStream.Fields.Length; gf++)
                {
                    if (fields[i].Format == geomStream.Fields[gf])
                    {
                        if (fields[i].Usage == "POSITION")
                            positionPos = geomStream.FieldPositions[gf];
                        else if (fields[i].Usage == "NORMAL")
                            normalPos = geomStream.FieldPositions[gf];
                        else if (fields[i].Usage == "DIFFUSE")
                            diffusePos = geomStream.FieldPositions[gf];
                        break;
                    }
                }
            }

            // actually create the metaball triangles or points
            IVolume[] volumes = new IVolume[objs.Count];
            int sIdx = 0;
            AtomShadingDesc aShading = coDesc.AtomShadingDesc;
            IMoleculeMaterialLookup lookup = aShading.MoleculeMaterials;
            foreach (IAtom atom in objs)
            {
                IMoleculeMaterialTemplate matTemp = lookup.ResolveBySymbol(atom.Symbol);
                IMoleculeMaterial material = null;
                if (matTemp != null)
                    material = matTemp.BySymbol;
                else
                {
                    PeriodicTableElement pe = (PeriodicTableElement)atom.Properties["PeriodicTableElement"];
                    if (pe != null)
                        material = lookup.GetBySeries(pe.ChemicalSerie);
                }

                volumes[sIdx++] = new Metaball(new Vector3((float)atom.X3d, (float)atom.Y3d, (float)atom.Z3d), 0.17f, material.BaseColor);
            }

            // process volume into triangles
            GenericVolumeScene scene = new GenericVolumeScene(volumes);
            int[] triangles = null;
            Vector3[] vertices;
            Color[] colours;
            Vector3[] normals = null;

            if (!pointsOnly)
            {
                IsosurfaceGenerator3D.GenerateSimpleMesh(scene, new Vector3(), scene.EstimateVolumeMaxSize(), 40, false, out triangles, out vertices, out colours);
                MeshOptimzer.GenerateTriPointNormals(triangles, vertices, out normals);
            }
            else
                IsosurfaceGenerator3D.GenerateSimplePointOutline(scene, new Vector3(), scene.EstimateVolumeMaxSize(), 40, out vertices, out colours);

            // create buffers
            buffer = new BufferedGeometryData(device, objs.Count);
            buffer.vBuffers = new BufferedGeometryData.VertexData[1];
            buffer.vBuffers[0] = new BufferedGeometryData.VertexData();
            buffer.vBuffers[0].Buffer = new VertexBuffer(device, geomStream.Stride * vertices.Length,
                                                         Usage.WriteOnly, geomStream.Format, Pool.Managed);
            buffer.vBuffers[0].Stride = geomStream.Stride;
            buffer.vBuffers[0].NumElements = vertices.Length;
            buffer.vBuffers[0].Format = geomStream.Format;

            buffer.iBuffers = new BufferedGeometryData.IndexData[1];
            buffer.iBuffers[0] = new BufferedGeometryData.IndexData();
            buffer.iBuffers[0].Desc = BufferedGeometryData.IndexData.Description.Geometry;
            if (pointsOnly)
            {
                buffer.iBuffers[0].NumPrimitives = vertices.Length;
                buffer.iBuffers[0].PrimType = PrimitiveType.PointList;
                buffer.Light = false;
            }
            else
            {
                buffer.iBuffers[0].NumPrimitives = triangles.Length / 3;
                buffer.iBuffers[0].PrimType = PrimitiveType.TriangleList;
                buffer.iBuffers[0].Buffer = new IndexBuffer(typeof(int), triangles.Length, device, Usage.WriteOnly, Pool.Managed);
            }

            // lock stream
            GraphicsStream data = buffer.vBuffers[0].Buffer.Lock(0, 0, LockFlags.None);

            // fill fields

            int clr = Color.FromArgb(255, 255, 255).ToArgb();
            long pos = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (positionPos != -1)
                {
                    data.Seek(pos + positionPos, SeekOrigin.Begin);
                    data.Write(vertices[i].X);
                    data.Write(vertices[i].Y);
                    data.Write(vertices[i].Z);
                }
                if (normalPos != -1 && !pointsOnly)
                {
                    data.Seek(pos + normalPos, SeekOrigin.Begin);
                    data.Write(normals[i].X);
                    data.Write(normals[i].Y);
                    data.Write(normals[i].Z);
                }
                if (diffusePos != -1)
                {
                    data.Seek(pos + diffusePos, SeekOrigin.Begin);
                    data.Write(colours[i].ToArgb());
                }
                //verts[i].Color = colours[i].ToArgb();
                pos += geomStream.Stride;
            }

            buffer.vBuffers[0].Buffer.Unlock();

            if (!pointsOnly)
                buffer.iBuffers[0].Buffer.SetData(triangles, 0, LockFlags.None);

            // dispose of temp data
        }

        public override void ClearCache()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}