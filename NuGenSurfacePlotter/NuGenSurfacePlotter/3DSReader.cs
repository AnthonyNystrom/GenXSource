using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NuGenCRBase.SceneFormats.ThreeDS
{
    class ThreeDSReader
    {
        enum ChunkCodes
        {
            BLANK = 0,
            PRIMARY = 0x4D4D,
            
            // Main Chunks
            EDIT3DS = 0x3D3D,   // Start of our actual objects
            KEYF3DS = 0xB000,   // Start of the keyframe information
            
            // General Chunks
            VERSION = 0x0002,
            MESH_VERSION = 0x3D3E,
            KFVERSION = 0x0005,
            COLOR_F = 0x0010,
            COLOR_24 = 0x0011,
            LIN_COLOR_24 = 0x0012,
            LIN_COLOR_F = 0x0013,
            INT_PERCENTAGE = 0x0030,
            FLOAT_PERC = 0x0031,
            MASTER_SCALE = 0x0100,
            IMAGE_FILE = 0x1100,
            AMBIENT_LIGHT = 0X2100,
            
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

        class Chunk
        {
            public ushort ID;
            public uint length;
        }

        public static ThreeDSFileData ParseFile(string filename)
        {
            FileStream file = new FileStream(filename, FileMode.Open);

            Chunk first = ReadChunk(file);

            // Check file
            if (first.ID != (ushort)ChunkCodes.PRIMARY)
                throw new Exception("Not a valid 3ds file");

            ThreeDSFileData data = null;

            // Read top level
            uint bytesRead = 6;
            while (bytesRead < first.length)
            {
                Chunk chunk = ReadChunk(file);

                switch ((ChunkCodes)chunk.ID)
                {
                    case ChunkCodes.BLANK:
                        break;
                    case ChunkCodes.EDIT3DS:
                        data = ReadEDIT3DS(file, chunk);
                        break;
                    /*case ChunkCodes.KEYF3DS:
                        ReadKEYF3DS(file, chunk);
                        break;*/
                    default:
                        file.Seek(chunk.length-6, SeekOrigin.Current);
                        break;
                }

                if (chunk.length == 0)
                    break;
                bytesRead += chunk.length;
            }

            file.Close();

            return data;
        }

        private static void ReadKEYF3DS(FileStream file, Chunk chunk)
        {
        }

        private static ThreeDSFileData ReadEDIT3DS(FileStream file, Chunk thischunk)
        {
            ThreeDSFileData data = new ThreeDSFileData();

            uint bytesRead = 6;
            while (bytesRead < thischunk.length)
            {
                Chunk chunk = ReadChunk(file);

                switch ((ChunkCodes)chunk.ID)
                {
                    case ChunkCodes.BLANK:
                        break;
                    case ChunkCodes.NAMED_OBJECT:
                        data.objects.Add(ReadNAMED_OBJECT(file, chunk));
                        break;
                    case ChunkCodes.MATERIAL:
                        data.materials.Add(ReadMATERIAL(file, chunk));
                        break;
                    default:
                        file.Seek(chunk.length-6, SeekOrigin.Current);
                        break;
                }

                bytesRead += chunk.length;
            }

            return data;
        }

        private static ThreeDSMesh.Material ReadMATERIAL(FileStream file, Chunk thischunk)
        {
            uint bytesRead = 6;

            ThreeDSMesh.Material material = new ThreeDSMesh.Material();

            while (bytesRead < thischunk.length)
            {
                Chunk chunk = ReadChunk(file);

                switch ((ChunkCodes)chunk.ID)
                {
                    case ChunkCodes.BLANK:
                        break;
                    case ChunkCodes.MAT_NAME:
                        uint size = 0;
                        while (file.ReadByte() != 0) size++;
                        size++;
                        file.Seek(-size, SeekOrigin.Current);
                        byte[] buffer = new byte[size];
                        file.Read(buffer, 0, (int)size);
                        material.name = ASCIIEncoding.ASCII.GetString(buffer);
                        bytesRead += size;
                        break;
                    case ChunkCodes.MAT_DIFFUSE:
                        material.diffuse = ReadMATDIFFUSE(file, chunk);
                        break;
                    case ChunkCodes.MAT_SHININESS:
                        file.Seek(chunk.length - 6, SeekOrigin.Current);
                        break;
                    case ChunkCodes.MAT_AMBIENT:
                        material.ambient = ReadMATDIFFUSE(file, chunk);
                        break;
                    case ChunkCodes.MAT_SPECULAR:
                        file.Seek(chunk.length - 6, SeekOrigin.Current);
                        break;
                    case ChunkCodes.MAT_SHIN_STR:
                        file.Seek(chunk.length - 6, SeekOrigin.Current);
                        break;
                    case ChunkCodes.MAT_SELF_ILLU:
                        file.Seek(chunk.length - 6, SeekOrigin.Current);
                        break;
                    /*case ChunkCodes.MAT_TEXMAP:
                        ReadMATERIAL(file, chunk);
                        break;*/
                    /*case ChunkCodes.MAT_TEXFLNM:
                        while (file.ReadByte() != 0) bytesRead++;
                        break;*/
                    default:
                        file.Seek(chunk.length-6, SeekOrigin.Current);
                        break;
                }

                bytesRead += chunk.length;
            }

            return material;
        }

        private static ThreeDSMesh.Colour ReadMATDIFFUSE(FileStream file, Chunk chunk)
        {
            byte[] buffer = new byte[6];
            file.Read(buffer, 0, 6);
            ThreeDSMesh.Colour colour = new ThreeDSMesh.Colour();
            colour.r = (byte)file.ReadByte();
            colour.g = (byte)file.ReadByte();
            colour.b = (byte)file.ReadByte();

            file.Seek(chunk.length-9, SeekOrigin.Current);

            return colour;
        }

        private static ThreeDSObject ReadNAMED_OBJECT(FileStream file, Chunk thischunk)
        {
            ThreeDSObject obj = new ThreeDSObject();

            uint bytesRead = 7;
            // Establish the length of the object name
            uint nameLen = 0;
            while (file.ReadByte() != 0) nameLen++;

            bytesRead += nameLen;
            // Go back and then read the name into a buffer
            /*file.Seek(-nameLen, SeekOrigin.Current);
            byte[] name = new byte[];
            file.Read(*/

            while (bytesRead < thischunk.length)
            {
                Chunk chunk = ReadChunk(file);

                switch ((ChunkCodes)chunk.ID)
                {
                    case ChunkCodes.BLANK:
                        break;
                    case ChunkCodes.OBJ_MESH:
                        obj.meshes.Add(LoadMeshOBJ_MESH(file, chunk));
                        break;
                    default:
                        file.Seek(chunk.length, SeekOrigin.Current);
                        break;
                }

                bytesRead += chunk.length;
            }

            return obj;
        }

        private static ThreeDSMesh LoadMeshOBJ_MESH(FileStream file, Chunk thischunk)
        {
            uint bytesRead = 6;
            ThreeDSMesh mesh = new ThreeDSMesh();

            while (bytesRead < thischunk.length)
            {
                Chunk chunk = ReadChunk(file);

                switch ((ChunkCodes)chunk.ID)
                {
                    case ChunkCodes.BLANK:
                        break;
                    case ChunkCodes.MESH_VERTICES:
                        mesh.verticies = ReadVerticesMESH_VERTICES(file);
                        break;
                    case ChunkCodes.MESH_FACES:
                        mesh.groups.Add(ReadVerticesMESH_FACES(file, chunk));
                        break;
                    case ChunkCodes.MESH_TEX_VERT:
                        mesh.uvMap = ReadTEXVERTS(file, chunk);
                        break;
                    /*case ChunkCodes.MESH_MATER:
                        mesh.mappings.Add(ReadMESHMATERIAL(file, chunk));
                        break;*/
                    case ChunkCodes.VERTEX_FLAGS:
                        goto END;
                    default:
                        END:
                        file.Seek(chunk.length-6, SeekOrigin.Current);
                        break;
                }

                bytesRead += chunk.length;
            }

            return mesh;
        }

        private static ThreeDSMesh.MaterialMapping ReadMESHMATERIAL(FileStream file, Chunk chunk)
        {
            ThreeDSMesh.MaterialMapping mapping = new ThreeDSMesh.MaterialMapping();
            int length = 0;
            while (file.ReadByte() != 0) length++;
            length++;
            file.Seek(-length, SeekOrigin.Current);

            byte[] name = new byte[length];
            file.Read(name, 0, length);

            mapping.name = ASCIIEncoding.ASCII.GetString(name);

            byte[] buffer = new byte[length];
            file.Read(buffer, 0, 2);
            ushort numFaces = BitConverter.ToUInt16(buffer, 0);
            
            mapping.mappedFaces = new ushort[numFaces];

            buffer = new byte[sizeof(ushort)];
            for (int i = 0; i < numFaces; i++)
            {
                file.Read(buffer, 0, sizeof(ushort));
                mapping.mappedFaces[i] = BitConverter.ToUInt16(buffer, 0);
            }

            return mapping;
        }

        private static ThreeDSMesh.UV[] ReadTEXVERTS(FileStream file, Chunk chunk)
        {
            byte[] buffer = new byte[2];
            file.Read(buffer, 0, 2);
            ushort numVerts = BitConverter.ToUInt16(buffer, 0);

            ThreeDSMesh.UV[] map = new ThreeDSMesh.UV[numVerts];
            int size = sizeof(float) * 2;
            buffer = new byte[size];
            for (int i = 0; i < numVerts; i++)
            {
                file.Read(buffer, 0, size);
                map[i].u = BitConverter.ToSingle(buffer, 0);
                map[i].v = BitConverter.ToSingle(buffer, sizeof(float));
            }

            return map;
        }

        private static ThreeDSMesh.FaceGroup ReadVerticesMESH_FACES(FileStream file, Chunk thischunk)
        {
            ThreeDSMesh.FaceGroup group = new ThreeDSMesh.FaceGroup();
            byte[] buffer = new byte[4];
            file.Read(buffer, 0, 2);
            uint numFaces = BitConverter.ToUInt32(buffer, 0);

            ThreeDSMesh.Face[] faces = new ThreeDSMesh.Face[numFaces];

            int size = sizeof(ushort);
            int size2 = size * 2;
            int size3 = size * 4;
            buffer = new byte[size3];
            for (int face = 0; face < numFaces; face++)
            {
                file.Read(buffer, 0, size3);

                faces[face].p1 = BitConverter.ToUInt16(buffer, 0);
                faces[face].p2 = BitConverter.ToUInt16(buffer, size);
                faces[face].p3 = BitConverter.ToUInt16(buffer, size2);
            }

            group.faces = faces;

            group.mappings = new List<ThreeDSMesh.MaterialMapping>();
            uint bytesRead = (numFaces * 8) + 8;

            while (bytesRead < thischunk.length)
            {
                Chunk chunk = ReadChunk(file);

                switch ((ChunkCodes)chunk.ID)
                {
                    case ChunkCodes.MESH_MATER:
                        group.mappings.Add(ReadMESHMATERIAL(file, chunk));
                        break;
                    case ChunkCodes.TRI_SMOOTH:
                        ReadTRI_SMOOTH(file, chunk);
                        break;
                }

                bytesRead += chunk.length;
            }

            return group;
        }

        private static void ReadTRI_SMOOTH(FileStream file, Chunk chunk)
        {
            byte[] buffer = new byte[2];
            file.Read(buffer, 0, 2);
            ushort numGroups = BitConverter.ToUInt16(buffer, 0);

            float size = chunk.length / sizeof(float);
        }

        private static ThreeDSMesh.Vertex[] ReadVerticesMESH_VERTICES(FileStream file)
        {
            byte[] buffer = new byte[4];
            file.Read(buffer, 0, 2);
            uint numVerts = BitConverter.ToUInt32(buffer, 0);

            ThreeDSMesh.Vertex[] verts = new ThreeDSMesh.Vertex[numVerts];

            int floatSize = sizeof(float);
            int floatSizeTwo = floatSize * 2;
            int floatSizeThree = floatSize * 3;

            buffer = new byte[floatSizeThree];

            for (int vert = 0; vert < numVerts; vert++)
            {
                file.Read(buffer, 0, floatSizeThree);

                verts[vert].x = BitConverter.ToSingle(buffer, 0);
                verts[vert].y = BitConverter.ToSingle(buffer, floatSize);
                verts[vert].z = BitConverter.ToSingle(buffer, floatSizeTwo);
            }

            return verts;
        }

        private static Chunk ReadChunk(FileStream file)
        {
            byte[] buffer = new byte[6];
            Chunk chunk = new Chunk();
            file.Read(buffer, 0, 6);
            chunk.ID = BitConverter.ToUInt16(buffer, 0);
            chunk.length = BitConverter.ToUInt32(buffer, 2);

            return chunk;
        }
    }
}