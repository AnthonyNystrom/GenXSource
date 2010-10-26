using System;
using System.Collections.Generic;
using System.Text;
using Org.OpenScience.CDK.Interfaces;
using System.Collections;
using NuGenSVisualLib.Rendering.Shading;
using NuGenSVisualLib.Rendering.Pipelines;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Settings;

namespace NuGenSVisualLib.Rendering.Chem
{
    public class BufferedGeometryData : IDisposable
    {
        /// <summary>
        /// Determines how the data gets invalidated
        /// </summary>
        /// <remarks>Bitwise</remarks>
        public enum DataValidityType
        {
            Perm    = 0,
            Source  = 1,
            View    = 2
        }

        public enum DataTarget
        {
            GeometryRender = 0,
            Geometry = 1
        }

        /// <summary>
        /// Pure geometric data
        /// </summary>
        public class VertexData
        {
            public VertexBuffer Buffer;
            public VertexFormats Format;
            public int NumElements;
            public int Stride;
        }

        /// <summary>
        /// Encapsulates the instructions to render indexed or non-index geometry
        /// </summary>
        public class IndexData
        {
            public enum Description
            {
                Geometry,
                Sprites,
                Effects
            }

            public IndexBuffer Buffer;
            public Description Desc;
            public int VBufferIdx;
            public int NumPrimitives;
            public PrimitiveType PrimType;
            public Texture[] Textures;
            public FillMode Fill = FillMode.Solid;
        }

        internal Device device;
        internal VertexData[] vBuffers;
        internal IndexData[] iBuffers;
        protected int numItems;
        DataTarget target;

        protected DataValidityType dataValidity;
        protected bool light = true;

        public BufferedGeometryData(Device device, int numItems)
        {
            this.device = device;
            this.numItems = numItems;

            dataValidity = DataValidityType.Source;
        }

        public virtual void OnDeviceReset() { }

        #region Properties

        public int NumItems
        {
            get { return numItems; }
        }

        public int NumBuffers
        {
            get { return iBuffers.Length; }
        }

        public int NumVertexBuffers
        {
            get { return vBuffers.Length; }
        }

        public DataValidityType DataValidity
        {
            get { return dataValidity; }
            set { dataValidity = value; }
        }

        public DataTarget Target
        {
            get { return target; }
            set { target = value; }
        }

        public bool Light
        {
            get { return light; }
            set { light = value; }
        }

        #endregion

        #region IDisposable Members

        public virtual void Dispose()
        {
            if (vBuffers != null)
            {
                foreach (VertexData buffer in vBuffers)
                {
                    if (buffer.Buffer != null)
                        buffer.Buffer.Dispose();
                }
            }
            if (iBuffers != null)
            {
                foreach (IndexData buffer in iBuffers)
                {
                    if (buffer.Buffer != null)
                        buffer.Buffer.Dispose();
                }
            }
        }

        #endregion
    }
}