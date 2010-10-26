using System;
using System.Collections.Generic;
using System.Text;

namespace NuGenCRBase.SceneFormats.ThreeDS
{
    class ThreeDSObject
    {
        public string name;
        public List<ThreeDSMesh> meshes;

        public ThreeDSObject()
        {
            meshes = new List<ThreeDSMesh>();
        }
    }

    class ThreeDSFileData
    {
        public List<ThreeDSObject> objects = new List<ThreeDSObject>();
        public List<ThreeDSMesh.Material> materials = new List<ThreeDSMesh.Material>();
        public ushort version;

        public ThreeDSFileData()
        {
            objects = new List<ThreeDSObject>();
            materials = new List<ThreeDSMesh.Material>();
        }

        public int GetNumMeshes()
        {
            int count = 0;
            foreach (ThreeDSObject obj in objects)
            {
                count += obj.meshes.Count;
            }
            return count;
        }

        public int GetNumFaceGroups()
        {
            int count = 0;
            foreach (ThreeDSObject obj in objects)
            {
                foreach (ThreeDSMesh mesh in obj.meshes)
                {
                    if (mesh.groups != null && mesh.groups.Count > 0)
                        count += mesh.groups.Count;
                    else
                        count++;
                }
            }
            return count;
        }
    }
}