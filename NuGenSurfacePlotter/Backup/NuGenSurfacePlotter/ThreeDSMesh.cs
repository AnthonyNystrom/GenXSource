using System;
using System.Collections.Generic;
using System.Text;

namespace NuGenCRBase.SceneFormats.ThreeDS
{
    class ThreeDSMesh
    {
        public struct Vertex
        {
            public float x, y, z;
            public float nx, ny, nz;
        }

        public struct Face
        {
            public ushort p1, p2, p3;
        }

        public struct Colour
        {
            public byte r, g, b;
        }

        public struct Material
        {
            public string name;
            public Colour diffuse;
            public Colour ambient;
            public Colour specular;
            public string texture;
        }

        public struct UV
        {
            public float u;
            public float v;
        }

        public class MaterialMapping
        {
            public string name;
            public ushort[] mappedFaces;
        }

        public class FaceGroup
        {
            public List<MaterialMapping> mappings = new List<MaterialMapping>();
            public Face[] faces;
        }

        public Vertex[] verticies;
        //public Face[] faces;
        //public Material[] materials;
        public UV[] uvMap;
        public List<FaceGroup> groups;

        public ThreeDSMesh()
        {
            groups = new List<FaceGroup>();
        }

        public int GetNumFaces()
        {
            int count = 0;
            foreach (FaceGroup group in groups)
            {
                count += group.faces.Length;
            }
            return count;
        }
    }
}