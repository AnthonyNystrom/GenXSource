using System;
using System.Collections.Generic;
using System.Text;
using Org.OpenScience.CDK.Interfaces;
using System.Collections;
using NuGenSVisualLib.Rendering.Shading;
using NuGenSVisualLib.Settings;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using Org.OpenScience.CDK;
using System.Drawing;
using NuGenSVisualLib.Rendering.Pipelines;
using NuGenSVisualLib.Rendering.Effects;
using System.IO;
using NuGenSVisualLib.Rendering.Chem.Materials;

namespace NuGenSVisualLib.Rendering.Chem
{
    class AtomSphereBufferCreator : AtomGeometryCreator
    {
        int sphereDetail1, sphereDetail2;
        private float _Scaling = 1.0f;

        public AtomSphereBufferCreator(float scale, int detail1, int detail2)
        {
            availableFields = new DataFields[] { new DataFields(VertexFormats.Position, "POSITION"),
                                                 new DataFields(VertexFormats.Normal, "NORMAL"),
                                                 new DataFields(VertexFormats.Diffuse, "DIFFUSE"),
                                                 new DataFields(VertexFormats.Texture1, "TEXTURE0")
                                               };

            this._Scaling = scale;
            this.sphereDetail1 = detail1;
            this.sphereDetail2 = detail2;
        }
        
        public float Scaling
        {
            get { return _Scaling; }
            set { _Scaling = value; }
        }

        public override void CreateGeometryForObjects(Device device, ICollection<IAtom> objs, GeomDataBufferStream geomStream,
                                                      int stream, ref BufferedGeometryData buffer,
                                                      CompleteOutputDescription coDesc)
        {
            // fillable fields
            int positionPos = -1;
            int normalPos = -1;
            int diffusePos = -1;
            int texPos = -1;

            // match field locations
            //int[] fieldsPos = new int[fields.Length];
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
                        else if (fields[i].Usage == "TEXTURE0")
                            texPos = geomStream.FieldPositions[gf];
                        //fieldsPos[i] = geomStream.FieldPositions[gf];
                        break;
                    }
                }
            }

            int numVerts = sphereDetail1 * sphereDetail2 * 6;
            int numTris = sphereDetail1 * sphereDetail2 * 2;

            // create buffers
            buffer = new BufferedGeometryData(device, objs.Count);
            buffer.vBuffers = new BufferedGeometryData.VertexData[1];
            buffer.vBuffers[0] = new BufferedGeometryData.VertexData();
            buffer.vBuffers[0].Buffer = new VertexBuffer(device, geomStream.Stride * numVerts * objs.Count,
                                                         Usage.WriteOnly, geomStream.Format, Pool.Managed);
            buffer.vBuffers[0].Stride = geomStream.Stride;
            buffer.vBuffers[0].NumElements = objs.Count * numVerts;
            buffer.vBuffers[0].Format = geomStream.Format;

            buffer.iBuffers = new BufferedGeometryData.IndexData[1];
            buffer.iBuffers[0] = new BufferedGeometryData.IndexData();
            buffer.iBuffers[0].Desc = BufferedGeometryData.IndexData.Description.Geometry;
            buffer.iBuffers[0].NumPrimitives = objs.Count * numTris;
            buffer.iBuffers[0].PrimType = PrimitiveType.TriangleList;

            // lock stream
            GraphicsStream data = buffer.vBuffers[0].Buffer.Lock(0, 0, LockFlags.None);

            // fill fields
            bool newWay = true;
            if (!newWay)
            {
                // create template sphere
                SphereMathHelper.SphereN sphere = SphereMathHelper.CalcSphereWNormals(sphereDetail1, sphereDetail2, 1.0f, new Vector3(), true);

                // clone and scale template for each size required
                Vector3 center = new Vector3();
                Dictionary<int, Vector3[]> spheres = new Dictionary<int, Vector3[]>();
                AtomShadingDesc aShading = coDesc.AtomShadingDesc;

                //int vertsIdx = 0;
                long pos = 0;
                foreach (IAtom atom in objs)
                {
                    int period = 1;
                    if (atom.Properties.ContainsKey("Period"))
                        period = (int)atom.Properties["Period"];

                    // check for existing
                    if (!spheres.ContainsKey(period))
                    {
                        // scale template
                        float size = period * _Scaling;
                        Vector4[] vertices = Vector3.Transform(sphere.Positions, Matrix.Scaling(size, size, size));
                        Vector3[] vertices3 = new Vector3[vertices.Length];
                        for (int i = 0; i < vertices.Length; i++)
                        {
                            vertices3[i] = new Vector3(vertices[i].X, vertices[i].Y, vertices[i].Z);
                        }
                        spheres.Add(period, vertices3);
                    }
                    Vector3[] sphereData = spheres[period];

                    // write to buffer
                    IMoleculeMaterialLookup lookup = aShading.MoleculeMaterials;
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

                    // copy sphere
                    Vector3 atomSpace = new Vector3((float)atom.X3d, (float)atom.Y3d, (float)atom.Z3d);
                    /*Vector4 clr = new Vector4((float)material.BaseColor.R / 255f, (float)material.BaseColor.G / 255f,
                                              (float)material.BaseColor.B / 255f, (float)material.BaseColor.A / 255f);*/
                    int clr = material.BaseColor.ToArgb();
                    for (int v = 0; v < sphereData.Length; v++)
                    {
                        if (positionPos != -1)
                        {
                            Vector3 position = sphereData[v] + atomSpace;
                            data.Seek(pos + positionPos, SeekOrigin.Begin);
                            data.Write(position.X);
                            data.Write(position.Y);
                            data.Write(position.Z);
                        }
                        if (texPos != -1)
                        {
                            data.Seek(pos + texPos, SeekOrigin.Begin);
                            data.Write(0f);//sphere.TexCoords[v].X);
                            data.Write(0f);//sphere.TexCoords[v].Y);
                        }
                        if (normalPos != -1)
                        {
                            data.Seek(pos + normalPos, SeekOrigin.Begin);
                            data.Write(sphere.Normals[v].X);
                            data.Write(sphere.Normals[v].Y);
                            data.Write(sphere.Normals[v].Z);
                        }
                        if (diffusePos != -1)
                        {
                            data.Seek(pos + diffusePos, SeekOrigin.Begin);
                            /*data.Write(clr.X);
                            data.Write(clr.Y);
                            data.Write(clr.Z);
                            data.Write(clr.W);*/
                            data.Write((Int32)clr);
                        }
                        pos += geomStream.Stride;
                    }
                }
            }
            else
            {
                Vector3[] Positions, normals;
                Vector2[] texCoords;
                SphereMathHelper.CreateSphereTriangles(new Vector3(), 1, sphereDetail1, out Positions,
                                                       out normals, out texCoords);

                // clone and scale template for each size required
                Vector3 center = new Vector3();
                Dictionary<int, Vector3[]> spheres = new Dictionary<int, Vector3[]>();
                AtomShadingDesc aShading = coDesc.AtomShadingDesc;

                int vertsIdx = 0;
                long pos = 0;
                foreach (IAtom atom in objs)
                {
                    int period = 1;
                    if (atom.Properties.ContainsKey("Period"))
                        period = (int)atom.Properties["Period"];

                    // check for existing
                    if (!spheres.ContainsKey(period))
                    {
                        // scale template
                        float size = period * _Scaling;
                        Vector4[] vertices = Vector3.Transform(Positions, Matrix.Scaling(size, size, size));
                        Vector3[] vertices3 = new Vector3[vertices.Length];
                        for (int i = 0; i < vertices.Length; i++)
                        {
                            vertices3[i] = new Vector3(vertices[i].X, vertices[i].Y, vertices[i].Z);
                        }
                        spheres.Add(period, vertices3);
                    }
                    Vector3[] sphereData = spheres[period];

                    // write to buffer
                    IMoleculeMaterialLookup lookup = aShading.MoleculeMaterials;
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

                    // copy sphere
                    Vector3 atomSpace = new Vector3((float)atom.X3d, (float)atom.Y3d, (float)atom.Z3d);
                    int clr = material.BaseColor.ToArgb();
                    for (int v = 0; v < sphereData.Length; v++)
                    {
                        if (positionPos != -1)
                        {
                            Vector3 position = sphereData[v] + atomSpace;
                            data.Seek(pos + positionPos, SeekOrigin.Begin);
                            data.Write(position.X);
                            data.Write(position.Y);
                            data.Write(position.Z);
                        }
                        if (texPos != -1)
                        {
                            data.Seek(pos + texPos, SeekOrigin.Begin);
                            data.Write(texCoords[v].X);
                            data.Write(texCoords[v].Y);
                        }
                        if (normalPos != -1)
                        {
                            data.Seek(pos + normalPos, SeekOrigin.Begin);
                            data.Write(normals[v].X);
                            data.Write(normals[v].Y);
                            data.Write(normals[v].Z);
                        }
                        if (diffusePos != -1)
                        {
                            data.Seek(pos + diffusePos, SeekOrigin.Begin);
                            /*data.Write(clr.X);
                            data.Write(clr.Y);
                            data.Write(clr.Z);
                            data.Write(clr.W);*/
                            data.Write((Int32)clr);
                        }
                        pos += geomStream.Stride;
                    }
                }
            }

            buffer.vBuffers[0].Buffer.Unlock();
        }

        public override void ClearCache()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}