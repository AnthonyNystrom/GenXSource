using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace NuGenCRBase.SceneFormats.Lwo
{
	class LWVertex
	{
        // 3D location of vertex
	    public Vector3 location;
        // Lighting normal
		public Vector3 normal;
        // Texture coordinate index
		public int uvIndex;
	}

    struct LWVertexIndices
    {
        public int I1, I2, I3;
    }

	class LWPolygon
	{
        // Index of component vertices
		public LWVertexIndices vertexIndices;
        // Polygon normal
		public Vector3 normal;
        // Surface associated with polygon
		public int surfaceIndex;
	}

	class LWVMap
    {
		public string name;
		public int type;

        // Number of vertices in vertex map
		public uint size;

        // Vertex array
		public Vector2 vertices;
	}

	class LWClip
	{
        // Location of source image
		public string sourceFile;
        // Clip index
		public int index;
        // Index of surface associated with clip
		public int surfaceIndex;
        // GL texture index
		//int GLIndex;
	}

	class LWSurface
	{
        // Vertex or polygon normals (smooth or flat shading)
		public bool vertexNormals;
		public int index;
		public string name;
		public Vector4 color;
		public Vector4 luminosity;
		public Vector4 ambient;
		public Vector4 diffuse;
		public Vector4 specular;
		public float glossiness;

        // Index of clip associated with surface
		public int clipIndex;
        // Name of texture uv vertex map
		public string uvMapName;
        // Pointer to clip image
		public LWClip surfaceMap;
        // Pointer to texture uv vertex map
		public LWVMap uvMap;
	}

    class LightwaveObject
    {
        private string file;

        LWVertex[] vertices;
        LWPolygon[] polygons;
        LWSurface[] surfaces;
        LWClip[] clips;
        LWVMap[] vmaps;

        public static bool LoadObject(string filename)
        {
            // Failure state variables for the lwo2 loader
            uint failID;
            int failpos;

            //obj = lwGetObject(filename, &failID, &failpos);

            //if (obj == NULL) {
            //    //_cprintf("[Object Load Failed]\n");
            //    return false;
            //} else {
            //    //_cprintf("[Object Loaded]\n");
            //    strncpy(this->filename, filename, 255);
            //}

            //// Convert the Lightwave Object to our custom format
            //if (!convertObject()) {
            //    //_cprintf("[Object Conversion Failed]\n");
            //    lwFreeObject(obj);
            //    return false;
            //} else {
            //    //_cprintf("[Object Converted To Internal Format]\n");
            //}

            //// We're done with the Lightwave Object, so free it
            //obj.Dispose();

	        return true;
        }

        //private bool ConvertObject(lwObject obj)
        //{
        //}

        #region Properties

        public string File
        {
            get { return file; }
        }

        #endregion
    }
}
