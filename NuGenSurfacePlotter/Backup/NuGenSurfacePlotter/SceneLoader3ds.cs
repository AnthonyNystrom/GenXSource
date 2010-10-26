using System;
using System.Collections.Generic;
using System.Text;
using NuGenCRBase.AvalonBridge;
using Microsoft.DirectX;
using NuGenCRBase.SceneFormats.ThreeDS;
using System.IO;

namespace NuGenCRBase.SceneFormats.ThreeDS
{
    class SceneLoader3ds : IABSceneLoader
    {
        #region IABSceneLoader Members

        public ABScene3D LoadScene(string file)
        {
            string path = file.Substring(0, file.Length - Path.GetFileName(file).Length);
            // parse file into 3ds structures
            ThreeDSFileData data = ThreeDSParser.ParseFromFile(file);

            // convert to AB
            ABScene3D scene = new ABScene3D();
            ABMaterial[] materials = null;
            if (data.materials != null && data.materials.Count > 0)
            {
                materials = new ABMaterial[data.materials.Count];
                for (int mIdx = 0; mIdx < data.materials.Count; mIdx++)
                {
                    ABMaterial material = materials[mIdx] = new ABMaterial();
                    material.Name = data.materials[mIdx].name;

                    material.Ambient = new ABColorARGB();
                    material.Ambient.A = 255;
                    material.Ambient.R = data.materials[mIdx].ambient.r;
                    material.Ambient.G = data.materials[mIdx].ambient.g;
                    material.Ambient.B = data.materials[mIdx].ambient.b;

                    material.Diffuse = new ABColorARGB();
                    material.Diffuse.A = 255;
                    material.Diffuse.R = data.materials[mIdx].diffuse.r;
                    material.Diffuse.G = data.materials[mIdx].diffuse.g;
                    material.Diffuse.B = data.materials[mIdx].diffuse.b;

                    // convert to absolute path
                    if (data.materials[mIdx].texture == null || Path.IsPathRooted(data.materials[mIdx].texture))
                        material.TextureName = data.materials[mIdx].texture;
                    else
                        material.TextureName = path + data.materials[mIdx].texture;

                    // match file
                    if (material.TextureName != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(material.TextureName).ToLower();
                        string ext = Path.GetExtension(material.TextureName).ToLower();
                        string[] files = Directory.GetFiles(path);
                        foreach (string fn in files)
                        {
                            string fno = Path.GetFileNameWithoutExtension(fn).ToLower();
                            if (fno.StartsWith(filename) && Path.GetExtension(fn).ToLower() == ext)
                            {
                                material.TextureName = fn;
                                break;
                            }
                        }
                    }
                }
            }

            int numMeshes = data.GetNumFaceGroups();
            if (numMeshes > 0)
            {
                scene.Models = new ABModel3D[numMeshes];
                int meshIdx = 0;
                foreach (ThreeDSObject obj in data.objects)
                {
                    foreach (ThreeDSMesh mesh in obj.meshes)
                    {
                        scene.Models[meshIdx++] = ConvertMesh(mesh, materials);
                    }
                }
            }
            return scene;
        }

        private ABModel3D ConvertMesh(ThreeDSMesh mesh, ABMaterial[] materials)
        {
            // convert vertices
            ABModel3D model = new ABModel3D();
            model.Geometry = new ABGeometry3D();
            if (mesh.verticies != null && mesh.verticies.Length > 0)
            {
                int numVerts = mesh.verticies.Length;
                model.Geometry.Vertices = new Vector3[numVerts];
                model.Geometry.Normals = new Vector3[numVerts];
                for (int vert = 0; vert < numVerts; vert++)
                {
                    model.Geometry.Vertices[vert] = new Vector3(mesh.verticies[vert].x,
                                                                mesh.verticies[vert].y,
                                                                mesh.verticies[vert].z);
                    model.Geometry.Normals[vert] = new Vector3(mesh.verticies[vert].nx,
                                                               mesh.verticies[vert].ny,
                                                               mesh.verticies[vert].nz);
                }
            }

            // uv map
            if (mesh.uvMap != null)
            {
                int numCoords = mesh.uvMap.Length;
                model.Geometry.TexCoords = new Vector2[numCoords];
                for (int coord = 0; coord < numCoords; coord++)
                {
                    model.Geometry.TexCoords[coord] = new Vector2(mesh.uvMap[coord].u,
                                                                  -mesh.uvMap[coord].v);
                }
            }
            model.Geometry.PrimType = Microsoft.DirectX.Direct3D.PrimitiveType.TriangleList;
            // indices
            if (mesh.groups != null)
            {
                model.Geometry.PrimIndices = new int[mesh.GetNumFaces() * 3];
                int idx = 0;
                foreach (ThreeDSMesh.FaceGroup group in mesh.groups)
                {
                    foreach (ThreeDSMesh.Face face in group.faces)
                    {
                        model.Geometry.PrimIndices[idx++] = face.p1;
                        model.Geometry.PrimIndices[idx++] = face.p2;
                        model.Geometry.PrimIndices[idx++] = face.p3;
                    }
                    if (group.mappings != null && group.mappings.Count > 0)
                    {
                        int numMatIndices = 0;
                        foreach (ThreeDSMesh.MaterialMapping mapping in group.mappings)
                        {
                            if (mapping.mappedFaces != null && mapping.mappedFaces.Length > 0)
                                numMatIndices++;
                        }
                        model.MaterialIndices = new ABMaterialIndex[numMatIndices];
                        int miIdx = 0;
                        foreach (ThreeDSMesh.MaterialMapping mapping in group.mappings)
                        {
                            if (mapping.mappedFaces != null && mapping.mappedFaces.Length > 0)
                            {
                                ABMaterialIndex matIndex = model.MaterialIndices[miIdx++] = new ABMaterialIndex();
                                matIndex.Indices = new int[mapping.mappedFaces.Length];
                                foreach (ABMaterial material in materials)
                                {
                                    if (material.Name == mapping.name)
                                        matIndex.Material = material;
                                }
                                for (int face = 0; face < mapping.mappedFaces.Length; face++)
                                {
                                    matIndex.Indices[face] = mapping.mappedFaces[face];
                                }
                            }
                        }
                    }
                }
            }

            return model;
        }

        #endregion
    }
}