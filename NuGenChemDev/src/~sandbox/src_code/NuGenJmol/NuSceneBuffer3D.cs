using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace NuGenJmol
{
    public class NuSceneBuffer3D
    {
        public abstract class NuBufferItem
        {
            public Vector3 translation;
        }

        public class NuBufferMeshItem : NuBufferItem
        {
            public Mesh mesh;
        }

        public List<NuBufferMeshItem> meshes;
        public List<Vector3> lines;
        public List<Vector3> triangles;
        public List<Vector3[]> triangleStrips;

        public NuSceneBuffer3D()
        {
            // TODO: Share mesh buffers
            meshes = new List<NuBufferMeshItem>();
            lines = new List<Vector3>();
            triangles = new List<Vector3>();
            triangleStrips = new List<Vector3[]>();
        }

        public void Clear()
        {
            meshes.Clear();
            lines.Clear();
            triangles.Clear();
            triangleStrips.Clear();
        }
    }
}