using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace NuGenCRBase.SceneFormats.ThreeDS
{
    class ThreeDSParser
    {
        enum ChunkCodes
        {
            BLANK           = 0,
            PRIMARY         = 0x4D4D,

            // Main Chunks
            EDIT3DS         = 0x3D3D,   // Start of our actual objects
            KEYF3DS         = 0xB000,   // Start of the keyframe information

            // General Chunks
            VERSION         = 0x0002,
            MESH_VERSION    = 0x3D3E,
            KFVERSION       = 0x0005,
            COLOR_F         = 0x0010,
            COLOR_24        = 0x0011,
            LIN_COLOR_24    = 0x0012,
            LIN_COLOR_F     = 0x0013,
            INT_PERCENTAGE  = 0x0030,
            FLOAT_PERC      = 0x0031,
            MASTER_SCALE    = 0x0100,
            IMAGE_FILE      = 0x1100,
            AMBIENT_LIGHT   = 0X2100,

            // Object Chunks
            NAMED_OBJECT = 0x4000,
            OBJ_MESH = 0x4100,
            MESH_VERTICES = 0x4110,
            VERTEX_FLAGS = 0x4111,
            MESH_FACES = 0x4120,
            MESH_MATER = 0x4130,
            MESH_TEX_VERT = 0x4140,
            MESH_XFMATRIX = 0x4160,
            MESH_COLOR_IND = 0x4165,
            MESH_TEX_INFO = 0x4170,
            HEIRARCHY = 0x4F00,
            TRI_SMOOTH = 0x4150,

            // Material Chunks
            MATERIAL = 0xAFFF,
            MAT_NAME = 0xA000,
            MAT_AMBIENT = 0xA010,
            MAT_DIFFUSE = 0xA020,
            MAT_SPECULAR = 0xA030,
            MAT_SHININESS = 0xA040,
            MAT_SHIN_STR = 0xA041,
            MAT_FALLOFF = 0xA052,
            MAT_EMISSIVE = 0xA080,
            MAT_SELF_ILLU = 0x084,
            MAT_SHADER = 0xA100,
            MAT_TEXMAP = 0xA200,
            MAT_TEXFLNM = 0xA300,

            OBJ_LIGHT = 0x4600,
            OBJ_CAMERA = 0x4700,

            // KeyFrames Chunks
            ANIM_HEADER = 0xB00A,
            ANIM_OBJ = 0xB002,

            ANIM_NAME = 0xB010,
            ANIM_POS = 0xB020,
            ANIM_ROT = 0xB021,
            ANIM_SCALE = 0xB022
        }

        public static ThreeDSFileData ParseFromFile(string file)
        {
            DataReader3DS data = new DataReader3DS(file);
            return ParseFromData(data);
        }

        static ThreeDSFileData ParseFromData(DataReader3DS data)
        {
            DataReader3DS dataSubSegment = null;

            if ((ChunkCodes)data.Tag == ChunkCodes.PRIMARY)
            {
                dataSubSegment = data.GetNextSubSegment();
            }
            else
                return null;

            ThreeDSFileData file = new ThreeDSFileData();

            while (dataSubSegment != null)
            {
                // Check the tag to see what sort of data is in this subsegment (or "chunk")
                switch ((ChunkCodes)dataSubSegment.Tag)
                {
                    case ChunkCodes.VERSION:
                        file.version = dataSubSegment.GetUShort();
                        break;
                    case ChunkCodes.EDIT3DS:
                        Parse3DSData(dataSubSegment, file);
                        break;
                    default:
                        break;
                }
                dataSubSegment = data.GetNextSubSegment();
            }

            return file;
        }

        private static void Parse3DSData(DataReader3DS dataSegment, ThreeDSFileData file)
        {
            DataReader3DS subSegment = dataSegment.GetNextSubSegment();

            while (subSegment != null)
            {
                switch ((ChunkCodes)subSegment.Tag)
                {
                    case ChunkCodes.MATERIAL:
                        file.materials.Add(ParseMaterialData(subSegment));
                        break;
                    case ChunkCodes.NAMED_OBJECT:
                        file.objects.Add(ParseMeshData(subSegment));
                        break;
                    default:
                        break;
                }
                subSegment = dataSegment.GetNextSubSegment();
            }
        }

        private static ThreeDSObject ParseMeshData(DataReader3DS dataSegment)
        {
            string name = dataSegment.GetString();
            DataReader3DS subSegment = dataSegment.GetNextSubSegment();

            ThreeDSObject obj = new ThreeDSObject();
            obj.name = name;

            while (subSegment != null)
            {
                switch ((ChunkCodes)subSegment.Tag)
                {
                    case ChunkCodes.OBJ_MESH:	// Current subsegment contains the polygonal information
                        obj.meshes.Add(ParsePolygonalData(subSegment));
                        break;
                    default:		// Ignore all other subsegment types
                        break;
                }
                subSegment = dataSegment.GetNextSubSegment();
            }

            return obj;
        }

        private static ThreeDSMesh ParsePolygonalData(DataReader3DS dataSegment)
        {
            DataReader3DS subSegment = dataSegment.GetNextSubSegment();	// working data subsegment

            ThreeDSMesh mesh = new ThreeDSMesh();

            while (subSegment != null)
            {
                switch ((ChunkCodes)subSegment.Tag)
                {
                    case ChunkCodes.MESH_VERTICES:	// Subsegment contains vertex information
                        mesh.verticies = new ThreeDSMesh.Vertex[subSegment.GetUShort()];
                        for (int vertex = 0; vertex < mesh.verticies.Length; vertex++)
                        {
                            mesh.verticies[vertex].x = subSegment.GetFloat();
                            mesh.verticies[vertex].y = subSegment.GetFloat();
                            mesh.verticies[vertex].z = subSegment.GetFloat();
                        }
                        break;
                    case ChunkCodes.MESH_XFMATRIX:	// Subsegment contains translation matrix info (ignore for now)
                        break;
                    case ChunkCodes.MESH_FACES:	// Subsegment contains face information
                        mesh.groups.Add(ParseFaceData(subSegment));
                        break;
                    case ChunkCodes.MESH_TEX_VERT:	// Subsegment contains texture mapping information
                        mesh.uvMap = ParseTexVerts(subSegment);
                        break;
                }
                subSegment = dataSegment.GetNextSubSegment();
            }

            // Also use face data to calculate vertex normals
            CalculateVertexNormals(mesh);

            return mesh;
        }

        private static void CalculateVertexNormals(ThreeDSMesh mesh)
        {
            // TODO: Use opt one from SM c++ imp
            Vector3 faceV1 = new Vector3();
            Vector3 faceV2 = new Vector3();
            Vector3 faceV3 = new Vector3();
            Vector3 faceEdge1;
            Vector3 faceEdge2;
            //Vector3[][] faceNormals = new Vector3[mesh.groups.Count][];
            int gIndex = 0;
            int[] vertexMults = new int[mesh.verticies.Length];
            Vector3[] normals = new Vector3[mesh.verticies.Length];
            foreach (ThreeDSMesh.FaceGroup group in mesh.groups)
            {
                //Vector3[] normals = faceNormals[gIndex] = new Vector3[group.faces.Length];
                //int fIndex = 0;
                foreach (ThreeDSMesh.Face face in group.faces)
                {
                    faceV1.X = mesh.verticies[face.p1].x;
                    faceV1.Y = mesh.verticies[face.p1].y;
                    faceV1.Z = mesh.verticies[face.p1].z;
                    vertexMults[face.p1]++;

                    faceV2.X = mesh.verticies[face.p2].x;
                    faceV2.Y = mesh.verticies[face.p2].y;
                    faceV2.Z = mesh.verticies[face.p2].z;
                    vertexMults[face.p2]++;

                    faceV3.X = mesh.verticies[face.p3].x;
                    faceV3.Y = mesh.verticies[face.p3].y;
                    faceV3.Z = mesh.verticies[face.p3].z;
                    vertexMults[face.p3]++;

                    Vector3 e1 = faceV2 - faceV1, e2 = faceV3 - faceV1;
                    Vector3 normal = Vector3.Normalize(Vector3.Cross(e1, e2));

                    //faceEdge1 = Vector3.Subtract(faceV2, faceV1);
                    //faceEdge2 = Vector3.Subtract(faceV3, faceV1);

                    //normals[fIndex] = Vector3.Cross(faceEdge1, faceEdge2);
                    //normals[fIndex++].Normalize();
                    //Vector3 normal = Vector3.Cross(faceEdge1, faceEdge2);

                    normals[face.p1] += normal;
                    normals[face.p2] += normal;
                    normals[face.p3] += normal;

                    /*mesh.verticies[face.p1].nx = mesh.verticies[face.p2].nx = mesh.verticies[face.p3].nx = nor.X;
                    mesh.verticies[face.p1].ny = mesh.verticies[face.p2].ny = mesh.verticies[face.p3].ny = nor.Y;
                    mesh.verticies[face.p1].nz = mesh.verticies[face.p2].nz = mesh.verticies[face.p3].nz = nor.Z;*/
                }
            }

            for (int vertex = 0; vertex < mesh.verticies.Length; vertex++)
            {
                normals[vertex] *= 1f / (float)vertexMults[vertex];
                mesh.verticies[vertex].nx = normals[vertex].X;
                mesh.verticies[vertex].ny = normals[vertex].Y;
                mesh.verticies[vertex].nz = normals[vertex].Z;
            }

            //int faceCount;
            //Vector3 vertexVectorSum;
            //for (int vertex = 0; vertex < mesh.verticies.Length; vertex++)
            //{
            //    faceCount = 0;
            //    vertexVectorSum = Vector3.Empty;
            //    gIndex = 0;
            //    foreach (ThreeDSMesh.FaceGroup group in mesh.groups)
            //    {
            //        int fIndex = 0;
            //        foreach (ThreeDSMesh.Face face in group.faces)
            //        {
            //            if ((vertex == face.p1) || (vertex == face.p2) || (vertex == face.p3))
            //            {
            //                faceCount++;
            //                vertexVectorSum = Vector3.Add(vertexVectorSum, faceNormals[gIndex][fIndex]);
            //            }
            //        }
            //    }

            //    if (faceCount > 0)
            //    {
            //        Vector3 normal = Vector3.Multiply(vertexVectorSum, 1f/(float)faceCount);
            //        normal.Normalize();

            //        mesh.verticies[vertex].nx = normal.X;
            //        mesh.verticies[vertex].ny = normal.Y;
            //        mesh.verticies[vertex].nz = normal.Z;
            //    }
            //    else
            //        mesh.verticies[vertex].nx = mesh.verticies[vertex].ny = mesh.verticies[vertex].nz = 0;
            //}
        }

        private static ThreeDSMesh.UV[] ParseTexVerts(DataReader3DS subSegment)
        {
            ThreeDSMesh.UV[] uv = new ThreeDSMesh.UV[subSegment.GetUShort()];

            for (int coord = 0; coord < uv.Length; coord++)
            {
                uv[coord].u = subSegment.GetFloat();
                uv[coord].v = subSegment.GetFloat();
            }

            return uv;
        }

        private static ThreeDSMesh.FaceGroup ParseFaceData(DataReader3DS dataSegment)
        {
            DataReader3DS subSegment;	// will be used to read other subsegments (do not initialize yet)

            ThreeDSMesh.FaceGroup group = new ThreeDSMesh.FaceGroup();
            group.faces = new ThreeDSMesh.Face[dataSegment.GetUShort()];

            // Read face data
            for (int face = 0; face < group.faces.Length; face++)
            {
                group.faces[face].p1 = dataSegment.GetUShort();
                group.faces[face].p2 = dataSegment.GetUShort();
                group.faces[face].p3 = dataSegment.GetUShort();
                // Ignore flag
                dataSegment.GetUShort();
            }

            // Read other subsegments
            subSegment = dataSegment.GetNextSubSegment();
            while (subSegment != null)
            {
                switch ((ChunkCodes)subSegment.Tag)
                {
                    case ChunkCodes.MESH_MATER:	// Name of material used
                        group.mappings.Add(ParseMeshMaterial(subSegment));
                        break;
                    case ChunkCodes.TRI_SMOOTH:
                        break;
                }
                subSegment = dataSegment.GetNextSubSegment();
            }

            return group;
        }

        private static ThreeDSMesh.MaterialMapping ParseMeshMaterial(DataReader3DS subSegment)
        {
            ThreeDSMesh.MaterialMapping mapping = new ThreeDSMesh.MaterialMapping();
            mapping.name = subSegment.GetString();

            // Read mapped faces
            mapping.mappedFaces = new ushort[subSegment.GetUShort()];
            for (int face = 0; face < mapping.mappedFaces.Length; face++)
            {
                mapping.mappedFaces[face] = subSegment.GetUShort();
            }

            return mapping;
        }

        private static ThreeDSMesh.Material ParseMaterialData(DataReader3DS dataSegment)
        {
            DataReader3DS subSegment = dataSegment.GetNextSubSegment();
            ThreeDSMesh.Material mat = new ThreeDSMesh.Material();

            while (subSegment != null)
            {
                switch ((ChunkCodes)subSegment.Tag)
                {
                    case ChunkCodes.MAT_NAME:	// Subsegment holds material name
                        mat.name = subSegment.GetString();
                        break;
                    case ChunkCodes.MAT_AMBIENT:	// Subsegment holds ambient color
                        mat.ambient = ParseColorData(subSegment.GetNextSubSegment());
                        break;
                    case ChunkCodes.MAT_DIFFUSE:	// Subsegment holds diffuse color (this is iffy...)
                        mat.diffuse = ParseColorData(subSegment.GetNextSubSegment());
                        break;
                    case ChunkCodes.MAT_TEXMAP:	// Subsegment holds texture map info
                        /*ParseTextureWeight(*/subSegment.GetNextSubSegment();
                        mat.texture = subSegment.GetNextSubSegment().GetString();
                        break;
                    default:		// Ignore all other subsegment types
                        break;
                }
                subSegment = dataSegment.GetNextSubSegment();
            }

            // Check if this is a duplicate material entry (by name)
            // NOTE: Duplicate material name update should be in form materialname_submeshname
            /*if (materialDataStore.ContainsKey(currentMaterialData.name))
            {
                // Select an "owning" submesh for the material
                referencingSubmeshes = (ArrayList)(submeshesUsingMaterials[currentMaterialData.name]);
                owningSubmesh = (SubmeshData3DS)(referencingSubmeshes[referencingSubmeshes.Count - 1]);
                // Qualify the material name with the submesh's name as well (see above note)
                currentMaterialData.name = currentMaterialData.name + "_" + owningSubmesh.name;
                owningSubmesh.materialUsed = currentMaterialData.name;
                // Clean up and update submesh reference lists
                referencingSubmeshes.RemoveAt(referencingSubmeshes.Count - 1);
                referencingSubmeshes = new ArrayList();
                referencingSubmeshes.Add(owningSubmesh);
                submeshesUsingMaterials.Add(currentMaterialData.name, referencingSubmeshes);
            }*/

            return mat;
        }

        private static ThreeDSMesh.Colour ParseColorData(DataReader3DS dataSegment)
        {
            ThreeDSMesh.Colour clr = new ThreeDSMesh.Colour();

            switch ((ChunkCodes)dataSegment.Tag)
            {
                case ChunkCodes.COLOR_F:	// Color is in float format
                    clr.r = (byte)(1.0f / dataSegment.GetFloat());
                    clr.g = (byte)(1.0f / dataSegment.GetFloat());
                    clr.b = (byte)(1.0f / dataSegment.GetFloat());
                    break;
                case ChunkCodes.COLOR_24:	// Color is in byte format
                    clr.r = dataSegment.GetByte();
                    clr.g = dataSegment.GetByte();
                    clr.b = dataSegment.GetByte();
                    break;
                default:		// If there are any other formats, then we ignore them
                    break;
            }

            return clr;
        }
    }
}