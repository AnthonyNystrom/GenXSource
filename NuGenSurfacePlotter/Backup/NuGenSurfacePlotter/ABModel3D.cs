using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Windows.Media.Media3D;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace NuGenCRBase.AvalonBridge
{
    class ABGeometry3D
    {
        public PrimitiveType PrimType;
        public Vector3[] Vertices;
        public Vector3[] Normals;
        public Vector2[] TexCoords;
        public int[] PrimIndices;
        public int[] VertexClrs;
        public int Clr = -1;
    }

    struct ABColorARGB
    {
        public byte A, R, G, B;
    }

    class ABMaterial
    {
        public string Name;
        public ABColorARGB Ambient, Diffuse;
        public Texture Texture;
        public string TextureName;
    }

    class ABMaterialIndex
    {
        public ABMaterial Material;
        public int[] Indices;
        public int PrimCount;
    }

    struct ABVolume3
    {
        public Vector3 Min;
        public Vector3 Max;
    }

    class ABModel3D : IDisposable
    {
        protected VertexBuffer vBuffer;
        protected IndexBuffer iBuffer;
        protected VertexFormats vFormat;
        protected int numPrimitives;
        protected IndexBuffer[] matIBuffers;

        public ABGeometry3D Geometry;
        public ABMaterial[] Materials;
        public ABMaterialIndex[] MaterialIndices;
        public Vector3 Transform;
        public Vector4 Rotation;
        public Vector3 Scaling;

        public float BoundingSphere;
        public ABVolume3 BoundingCubeVolume;
        public Vector3 Origin;

        public void BuildBuffers(Device device)
        {
            if (Geometry.Normals != null)
            {
                if (Geometry.VertexClrs != null)
                {
                    vFormat = CustomVertex.PositionNormalColored.Format;
                    vBuffer = new VertexBuffer(typeof(CustomVertex.PositionNormalColored), Geometry.Vertices.Length, device,
                                           Usage.None, CustomVertex.PositionNormalColored.Format, Pool.Managed);
                    CustomVertex.PositionNormalColored[] verts = (CustomVertex.PositionNormalColored[])vBuffer.Lock(0, LockFlags.None);
                    for (int i = 0; i < Geometry.Vertices.Length; i++)
                    {
                        verts[i].Position = Geometry.Vertices[i];
                        verts[i].Normal = Geometry.Normals[i];
                        verts[i].Color = Geometry.VertexClrs[i];
                    }
                    vBuffer.Unlock();
                }
                else
                {
                    if (Geometry.TexCoords != null)
                    {
                        vFormat = CustomVertex.PositionNormalTextured.Format;
                        vBuffer = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), Geometry.Vertices.Length, device,
                                               Usage.None, CustomVertex.PositionNormalTextured.Format, Pool.Managed);
                        CustomVertex.PositionNormalTextured[] verts = (CustomVertex.PositionNormalTextured[])vBuffer.Lock(0, LockFlags.None);
                        for (int i = 0; i < Geometry.Vertices.Length; i++)
                        {
                            verts[i].Position = Geometry.Vertices[i];
                            verts[i].Normal = Geometry.Normals[i];
                            verts[i].Tu = Geometry.TexCoords[i].X;
                            verts[i].Tv = Geometry.TexCoords[i].Y;
                        }
                        vBuffer.Unlock();
                    }
                    else
                    {
                        vFormat = CustomVertex.PositionNormal.Format;
                        vBuffer = new VertexBuffer(typeof(CustomVertex.PositionNormal), Geometry.Vertices.Length, device,
                                               Usage.None, CustomVertex.PositionNormal.Format, Pool.Managed);
                        CustomVertex.PositionNormal[] verts = (CustomVertex.PositionNormal[])vBuffer.Lock(0, LockFlags.None);
                        for (int i = 0; i < Geometry.Vertices.Length; i++)
                        {
                            verts[i].Position = Geometry.Vertices[i];
                            verts[i].Normal = Geometry.Normals[i];
                        }
                        vBuffer.Unlock();
                    }
                }
            }
            else
            {
                if (Geometry.VertexClrs != null)
                {
                    vFormat = CustomVertex.PositionColored.Format;
                    vBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), Geometry.Vertices.Length, device,
                                           Usage.None, CustomVertex.PositionColored.Format, Pool.Managed);
                    CustomVertex.PositionColored[] verts = (CustomVertex.PositionColored[])vBuffer.Lock(0, LockFlags.None);
                    for (int i = 0; i < Geometry.Vertices.Length; i++)
                    {
                        verts[i].Position = Geometry.Vertices[i];
                        verts[i].Color = Geometry.VertexClrs[i];
                    }
                    vBuffer.Unlock();
                }
                else
                {
                    if (Geometry.TexCoords != null)
                    {
                        vFormat = CustomVertex.PositionTextured.Format;
                        vBuffer = new VertexBuffer(typeof(CustomVertex.PositionTextured), Geometry.Vertices.Length, device,
                                               Usage.None, CustomVertex.PositionTextured.Format, Pool.Managed);
                        CustomVertex.PositionTextured[] verts = (CustomVertex.PositionTextured[])vBuffer.Lock(0, LockFlags.None);
                        for (int i = 0; i < Geometry.Vertices.Length; i++)
                        {
                            verts[i].Position = Geometry.Vertices[i];
                            verts[i].Tu = Geometry.TexCoords[i].X;
                            verts[i].Tv = Geometry.TexCoords[i].Y;
                        }
                        vBuffer.Unlock();
                    }
                    else
                    {
                        vFormat = CustomVertex.PositionOnly.Format;
                        vBuffer = new VertexBuffer(typeof(CustomVertex.PositionOnly), Geometry.Vertices.Length, device,
                                               Usage.None, CustomVertex.PositionOnly.Format, Pool.Managed);
                        CustomVertex.PositionOnly[] verts = (CustomVertex.PositionOnly[])vBuffer.Lock(0, LockFlags.None);
                        for (int i = 0; i < Geometry.Vertices.Length; i++)
                        {
                            verts[i].Position = Geometry.Vertices[i];
                        }
                        vBuffer.Unlock();
                    }
                }
            }

            if (Geometry.PrimIndices != null)
            {
                if (MaterialIndices != null)
                {
                    matIBuffers = new IndexBuffer[MaterialIndices.Length];
                    int mibIdx = 0;
                    foreach (ABMaterialIndex matIndex in MaterialIndices)
                    {
                        if (matIndex.Material.Texture == null && matIndex.Material.TextureName != null)
                        {
                            if (File.Exists(matIndex.Material.TextureName))
                                matIndex.Material.Texture = TextureLoader.FromFile(device, matIndex.Material.TextureName);
                        }

                        int numIndices = matIndex.Indices.Length * 3;
                        IndexBuffer buffer = matIBuffers[mibIdx++] = new IndexBuffer(typeof(int), numIndices, device, Usage.None, Pool.Managed);
                        int[] indices = (int[])buffer.Lock(0, LockFlags.None);
                        int iIdx = 0;
                        for (int i = 0; i < matIndex.Indices.Length; i++)
                        {
                            int index = matIndex.Indices[i] * 3;
                            indices[iIdx++] = Geometry.PrimIndices[index];
                            indices[iIdx++] = Geometry.PrimIndices[index + 1];
                            indices[iIdx++] = Geometry.PrimIndices[index + 2];
                        }
                        buffer.Unlock();

                        switch (Geometry.PrimType)
                        {
                            case PrimitiveType.LineList:
                                matIndex.PrimCount = numIndices / 2;
                                break;
                            case PrimitiveType.LineStrip:
                                matIndex.PrimCount = numIndices - 1;
                                break;
                            case PrimitiveType.PointList:
                                matIndex.PrimCount = numIndices;
                                break;
                            case PrimitiveType.TriangleList:
                                matIndex.PrimCount = numIndices / 3;
                                break;
                        }
                    }
                }
                else
                {
                    iBuffer = new IndexBuffer(typeof(int), Geometry.PrimIndices.Length, device, Usage.None, Pool.Managed);
                    //iBuffer.SetData(Geometry.PrimIndices, 0, LockFlags.None);
                    int[] indices = (int[])iBuffer.Lock(0, LockFlags.None);
                    for (int i = 0; i < Geometry.PrimIndices.Length; i++)
                    {
                        indices[i] = Geometry.PrimIndices[i];
                    }
                    iBuffer.Unlock();

                    switch (Geometry.PrimType)
                    {
                        case PrimitiveType.LineList:
                            numPrimitives = Geometry.PrimIndices.Length / 2;
                            break;
                        case PrimitiveType.LineStrip:
                            numPrimitives = Geometry.PrimIndices.Length - 1;
                            break;
                        case PrimitiveType.PointList:
                            numPrimitives = Geometry.PrimIndices.Length;
                            break;
                        case PrimitiveType.TriangleList:
                            numPrimitives = Geometry.PrimIndices.Length / 3;
                            break;
                    }
                }
            }
            else
            {
                switch (Geometry.PrimType)
                {
                    case PrimitiveType.LineList:
                        numPrimitives = Geometry.Vertices.Length / 2;
                        break;
                    case PrimitiveType.LineStrip:
                        numPrimitives = Geometry.Vertices.Length - 1;
                        break;
                    case PrimitiveType.PointList:
                        numPrimitives = Geometry.Vertices.Length;
                        break;
                    case PrimitiveType.TriangleList:
                        numPrimitives = Geometry.Vertices.Length / 3;
                        break;
                }
            }
        }

        public void Render(Device device)
        {
            if (Geometry.Clr != -1)
            {
                device.RenderState.Lighting = true;
                Microsoft.DirectX.Direct3D.Material mt = new Microsoft.DirectX.Direct3D.Material();
                mt.AmbientColor = ColorValue.FromArgb(Geometry.Clr);
                mt.DiffuseColor = ColorValue.FromArgb(Geometry.Clr);
                device.RenderState.DiffuseMaterialSource = ColorSource.Material;
                device.Material = mt;
                device.RenderState.AmbientColor = Geometry.Clr;
            }

            device.VertexFormat = vFormat;
            device.SetStreamSource(0, vBuffer, 0);
            if (matIBuffers != null)
            {
                for (int mat = 0; mat < MaterialIndices.Length; mat++)
                {
                    Microsoft.DirectX.Direct3D.Material oMat = device.Material;
                    if (MaterialIndices[mat].Material.Texture == null)
                    {
                        Microsoft.DirectX.Direct3D.Material material = new Microsoft.DirectX.Direct3D.Material();
                        material.AmbientColor = ColorValue.FromColor(System.Drawing.Color.FromArgb(MaterialIndices[mat].Material.Ambient.A,
                                                                                    MaterialIndices[mat].Material.Ambient.R,
                                                                                    MaterialIndices[mat].Material.Ambient.G,
                                                                                    MaterialIndices[mat].Material.Ambient.B));
                        material.DiffuseColor = ColorValue.FromColor(System.Drawing.Color.FromArgb(MaterialIndices[mat].Material.Diffuse.A,
                                                                                    MaterialIndices[mat].Material.Diffuse.R,
                                                                                    MaterialIndices[mat].Material.Diffuse.G,
                                                                                    MaterialIndices[mat].Material.Diffuse.B));
                        device.Material = material;
                    }
                    else
                        device.SetTexture(0, MaterialIndices[mat].Material.Texture);

                    device.Indices = matIBuffers[mat];
                    device.DrawIndexedPrimitives(Geometry.PrimType, 0, 0, Geometry.Vertices.Length, 0, MaterialIndices[mat].PrimCount);

                    device.Material = oMat;
                }
            }
            else if (iBuffer != null)
            {
                device.Indices = iBuffer;
                device.DrawIndexedPrimitives(Geometry.PrimType, 0, 0, Geometry.Vertices.Length, 0, numPrimitives);
            }
            else
            {
                device.Indices = null;
                device.DrawPrimitives(Geometry.PrimType, 0, numPrimitives);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (vBuffer != null)
                vBuffer.Dispose();
            if (iBuffer != null)
                iBuffer.Dispose();
            if (Materials != null)
            {
                foreach (ABMaterial mat in Materials)
                {
                    if (mat.Texture != null)
                        mat.Texture.Dispose();
                }
            }
        }

        #endregion

        public void UpdateBufferVClrs()
        {
            if (Geometry.Normals != null)
            {
                CustomVertex.PositionNormalColored[] verts = (CustomVertex.PositionNormalColored[])vBuffer.Lock(0, LockFlags.None);
                for (int i = 0; i < Geometry.Vertices.Length; i++)
                {
                    verts[i].Color = Geometry.VertexClrs[i];
                }
                vBuffer.Unlock();

            }
            else
            {
                CustomVertex.PositionColored[] verts = (CustomVertex.PositionColored[])vBuffer.Lock(0, LockFlags.None);
                for (int i = 0; i < Geometry.Vertices.Length; i++)
                {
                    verts[i].Color = Geometry.VertexClrs[i];
                }
                vBuffer.Unlock();
            }
        }

        public void CalcBounds()
        {
            if (Geometry.Vertices != null)
            {
                Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                foreach (Vector3 vertex in Geometry.Vertices)
                {
                    if (vertex.X < min.X)
                        min.X = vertex.X;
                    if (vertex.Y < min.Y)
                        min.Y = vertex.Y;
                    if (vertex.Z < min.Z)
                        min.Z = vertex.Z;

                    if (vertex.X > max.X)
                        max.X = vertex.X;
                    if (vertex.Y > max.Y)
                        max.Y = vertex.Y;
                    if (vertex.Z > max.Z)
                        max.Z = vertex.Z;
                }
                this.BoundingCubeVolume.Max = max;
                this.BoundingCubeVolume.Min = min;

                Vector3 extent = max - min;
                Origin = min + (extent * 0.5f);
                BoundingSphere = (extent * 0.5f).Length();
            }
        }

        public ModelVisual3D[] ToAvalonObj()
        {
            // convert geometry
            if (MaterialIndices == null)
            {
                MeshGeometry3D geometry = new MeshGeometry3D();
                // geometry
                if (Geometry.Vertices != null)
                {
                    geometry.Positions = new Point3DCollection(Geometry.Vertices.Length);
                    for (int vertex = 0; vertex < Geometry.Vertices.Length; vertex++)
                    {
                        Vector3 position = Geometry.Vertices[vertex];
                        geometry.Positions.Add(new Point3D(position.X, position.Y, position.Z));
                    }
                }
                // normals
                if (Geometry.Normals != null)
                {
                    geometry.Normals = new Vector3DCollection(Geometry.Normals.Length);
                    for (int nIdx = 0; nIdx < Geometry.Normals.Length; nIdx++)
                    {
                        Vector3 normal = Geometry.Normals[nIdx];
                        geometry.Normals.Add(new Vector3D(normal.X, normal.Y, normal.Z));
                    }
                }
                // tex coords
                if (Geometry.TexCoords != null)
                {
                    geometry.TextureCoordinates = new PointCollection(Geometry.TexCoords.Length);
                    for (int tcIdx = 0; tcIdx < Geometry.TexCoords.Length; tcIdx++)
                    {
                        Vector2 texCoord = Geometry.TexCoords[tcIdx];
                        geometry.TextureCoordinates.Add(new System.Windows.Point(texCoord.X, texCoord.Y));
                    }
                }

                // triangle indices
                if (Geometry.PrimIndices != null)
                {
                    geometry.TriangleIndices = new Int32Collection(Geometry.PrimIndices.Length);
                    for (int tIdx = 0; tIdx < Geometry.PrimIndices.Length; tIdx++)
                    {
                        geometry.TriangleIndices.Add(Geometry.PrimIndices[tIdx]);
                    }
                }

                // material
                System.Windows.Media.Media3D.Material material = null;
                if (Materials != null)
                {
                    if (Materials[0].TextureName != null)
                    {
                        ImageBrush ib = new ImageBrush();
                        ib.ImageSource = new BitmapImage(new Uri(Materials[0].TextureName));
                        material = new DiffuseMaterial(ib);
                    }
                    else
                    {
                        material = new DiffuseMaterial(new SolidColorBrush(System.Windows.Media.Color.FromArgb(Materials[0].Diffuse.A, Materials[0].Diffuse.R, Materials[0].Diffuse.G, Materials[0].Diffuse.B)));
                    }
                }
                ModelVisual3D model = new ModelVisual3D();
                model.Content = new GeometryModel3D(geometry, material);
                return new ModelVisual3D[] { model };
            }
            else
            {
                // break up by material indices
                ModelVisual3D[] models = new ModelVisual3D[MaterialIndices.Length];
                int[] vertIdxRefs = new int[Geometry.Vertices.Length];
                int[] modelVerts = new int[Geometry.Vertices.Length];
                int vertIdx = 0;
                for (int mIdx = 0; mIdx < MaterialIndices.Length; mIdx++)
                {
                    // wipe ref index
                    for (int idxRef = 0; idxRef < vertIdxRefs.Length; idxRef++)
                    {
                        vertIdxRefs[idxRef] = -1;
                    }

                    MeshGeometry3D geometry = new MeshGeometry3D();
                    // refactor verticies
                    for (int i = 0; i < MaterialIndices[mIdx].Indices.Length; i++)
                    {
                        // check for existing ref
                        int index = MaterialIndices[mIdx].Indices[i];
                        if (vertIdxRefs[index] == -1)
                        {
                            vertIdxRefs[index] = vertIdx;
                            modelVerts[vertIdx++] = index;
                        }
                        geometry.TriangleIndices.Add(vertIdxRefs[index]);
                    }

                    // write verts
                    geometry.Positions = new Point3DCollection(vertIdx);
                    for (int vertex = 0; vertex < vertIdx; vertex++)
                    {
                        Vector3 position = Geometry.Vertices[modelVerts[vertex]];
                        geometry.Positions.Add(new Point3D(position.X, position.Y, position.Z));
                    }
                    // normals
                    if (Geometry.Normals != null)
                    {
                        geometry.Normals = new Vector3DCollection(vertIdx);
                        for (int nIdx = 0; nIdx < vertIdx; nIdx++)
                        {
                            Vector3 normal = Geometry.Normals[modelVerts[nIdx]];
                            geometry.Normals.Add(new Vector3D(normal.X, normal.Y, normal.Z));
                        }
                    }
                    // tex coords
                    if (Geometry.TexCoords != null)
                    {
                        geometry.TextureCoordinates = new PointCollection(vertIdx);
                        for (int tcIdx = 0; tcIdx < vertIdx; tcIdx++)
                        {
                            Vector2 texCoord = Geometry.TexCoords[modelVerts[tcIdx]];
                            geometry.TextureCoordinates.Add(new System.Windows.Point(texCoord.X, texCoord.Y));
                        }
                    }

                    System.Windows.Media.Media3D.Material material = null;
                    if (MaterialIndices[mIdx].Material.TextureName != null)
                    {
                        ImageBrush ib = new ImageBrush();
                        ib.ImageSource = new BitmapImage(new Uri(MaterialIndices[mIdx].Material.TextureName));
                        material = new DiffuseMaterial(ib);
                    }
                    else
                    {
                        ABColorARGB clr = MaterialIndices[mIdx].Material.Diffuse;
                        material = new DiffuseMaterial(new SolidColorBrush(System.Windows.Media.Color.FromArgb(clr.A, clr.R,
                                                                                                               clr.G, clr.B)));
                    }

                    models[mIdx] = new ModelVisual3D();
                    models[mIdx].Content = new GeometryModel3D(geometry, material);
                }
                return models;
            }
        }
    }
}
