using Genetibase.NuGenRenderCore.Rendering;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenDEMVis
{
    abstract class DEMGeometry : IDEMGeometry
    {
        protected Device gDevice;
        protected Vector3 position;
        protected Vector3 dimensions;
        protected Vector3 center;
        protected ulong vertexCount;
        protected uint primitiveCount;
        protected PrimitiveType primitivesType;
        protected bool useDiffuseTex;
        //            protected TextureSampler[] textureSamples;

        /// <summary>
        /// Initializes a new instance of the HeightMapGeometry class.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="dimensions"></param>
        /// <param name="center"></param>
        /// <param name="vertexCount"></param>
        /// <param name="primitiveCount"></param>
        /// <param name="primitivesType"></param>
        /// <param name="gDevice"></param>
        public DEMGeometry(Vector3 position, Vector3 dimensions, Vector3 center,
                           ulong vertexCount, uint primitiveCount, PrimitiveType primitivesType,
                           Device gDevice)
        {
            this.position = position;
            this.dimensions = dimensions;
            this.center = center;
            this.vertexCount = vertexCount;
            this.primitiveCount = primitiveCount;
            this.primitivesType = primitivesType;
            this.gDevice = gDevice;
        }

        #region IHeightMapGeometry Members

        public Vector3 Position
        {
            get { return position; }
        }

        public Vector3 Dimensions
        {
            get { return dimensions; }
        }

        public Vector3 Center
        {
            get { return center; }
        }

        public ulong VertexCount
        {
            get { return vertexCount; }
        }

        public uint PrimitiveCount
        {
            get { return primitiveCount; }
        }

        public PrimitiveType PrimitivesType
        {
            get { return primitivesType; }
        }

        public abstract IDemSubGeometry[] SubGeometry
        {
            get;
        }

//        public TextureSampler[] TextureSamplers
//        {
//            get { return textureSamples; }
//        }

        public abstract void Render(GraphicsPipeline gPipe);
        #endregion

        #region IDisposable Members

        public abstract void Dispose();
        #endregion

        public virtual void ProcessSamplers() { }

        public bool UseDiffuseTexture
        {
            get { return useDiffuseTex; }
            set { useDiffuseTex = value; }
        }
    }
}