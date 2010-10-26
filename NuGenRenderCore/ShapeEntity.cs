using Genetibase.NuGenRenderCore.Rendering.Devices;
using Genetibase.NuGenRenderCore.Scene;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Resource=Genetibase.NuGenRenderCore.Resources.Resource;

namespace Genetibase.NuGenRenderCore.Rendering
{
    public class Shape : Resource
    {
        readonly VertexBuffer[] vBuffers;
        readonly IndexBuffer[] iBuffers;
        readonly PrimitiveType[] pTypes;
        readonly VertexFormats[] vFormats;

        readonly int[] vertexCount;
        readonly int[] indexCount;
        readonly int[] indexVRefs;
        readonly int[] primCounts;

        readonly BoundingBox bBox;

        public Shape(string id, VertexBuffer[] vertexBuffers, IndexBuffer[] indexBuffers,
                     int[] numVertices, int[] numIndices, int[] indexVRefs,
                     PrimitiveType[] pTypes, VertexFormats[] vFormats,
                     int[] primCounts, BoundingBox bBox)
            : base(id, null, null)
        {
            vBuffers = vertexBuffers;
            iBuffers = indexBuffers;

            vertexCount = numVertices;
            indexCount = numIndices;
            this.indexVRefs = indexVRefs;
            this.pTypes = pTypes;
            this.vFormats = vFormats;
            this.primCounts = primCounts;
            this.bBox = bBox;
        }

        public override void Dispose()
        {
            if (vBuffers != null)
            {
                for (int i = 0; i < vBuffers.Length; i++)
                {
                    vBuffers[i].Dispose();
                }
            }
            if (iBuffers != null)
            {
                for (int i = 0; i < iBuffers.Length; i++)
                {
                    iBuffers[i].Dispose();
                }
            }
        }

        public bool IsIndexed
        {
            get { return iBuffers != null; }
        }

        public VertexBuffer[] Vertices
        {
            get { return vBuffers; }
        }

        public IndexBuffer[] Indices
        {
            get { return iBuffers; }
        }

        public int[] VertexCount
        {
            get { return vertexCount; }
        }

        public int[] IndexCount
        {
            get { return indexCount; }
        }

        public PrimitiveType[] PrimTypes
        {
            get { return pTypes; }
        }

        public VertexFormats[] VertexFormats
        {
            get { return vFormats; }
        }

        public int[] PrimitiveCounts
        {
            get { return primCounts; }
        }

        public int[] IndexVRefs
        {
            get { return indexVRefs; }
        }

        public BoundingBox Bounds
        {
            get { return bBox; }
        }
    }

    public class ShapeEntity : SceneEntity
    {
        protected Shape shape;
        protected float opacity;
        protected int clr;

        public ShapeEntity(Shape shape, Vector3 centreOffset)
            : base(shape.Bounds.Centre + centreOffset, shape.Bounds.Dimensions)
        {
            this.shape = shape;
        }

        public float Opacity
        {
            get { return opacity; }
            set { opacity = value; }
        }

        public int Clr
        {
            get { return clr; }
            set { clr = value; }
        }

        #region SceneEntity Members

        public override void Render(GraphicsPipeline gPipeline)
        {
            // set scale
            gPipeline.BeginScene();

            base.Render(gPipeline);

            if (shape.IsIndexed)
            {
                for (int i = 0; i < shape.Indices.Length; i++)
                {
                    gDevice.Indices = shape.Indices[i];
                    int vIdx = shape.IndexVRefs[i];
                    gDevice.VertexFormat = shape.VertexFormats[vIdx];
                    gDevice.SetStreamSource(0, shape.Vertices[vIdx], 0);
                    gDevice.DrawIndexedPrimitives(shape.PrimTypes[i], 0, 0, shape.VertexCount[vIdx], 0, shape.PrimitiveCounts[i]);
                }
            }
            else
            {
                for (int i = 0; i < shape.Vertices.Length; i++)
                {
                    gDevice.VertexFormat = shape.VertexFormats[i];
                    gDevice.SetStreamSource(0, shape.Vertices[i], 0);
                    gDevice.DrawPrimitives(shape.PrimTypes[i], 0, shape.PrimitiveCounts[i]);
                }
            }

            gPipeline.EndScene();
        }


        public override void Init(DeviceInterface devIf, SceneManager sManager)
        {
            gDevice = devIf.Device;
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}