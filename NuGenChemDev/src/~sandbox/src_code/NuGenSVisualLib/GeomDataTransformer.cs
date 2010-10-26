using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using NuGenSVisualLib.Rendering.Chem;
using NuGenSVisualLib.Rendering.Effects;

namespace NuGenSVisualLib
{
    /// <summary>
    /// Encapsulates a stream into a vertex buffer of multiple composite geometry/source streams
    /// </summary>
    public class GeomDataBufferStream
    {
        //public struct StreamFields
        //{
        //    public int[] indices;
        //}

        //public StreamFields[] streams;
        public VertexFormats[] Fields;
        public int[] FieldPositions;
        public VertexFormats Format;
        /*public VertexBuffer buffer;
        public GraphicsStream data;
        public int index;*/
        public int Stride;

        //public void Finalize(ref BufferedGeometryData buffer)
        //{
        //    //this.buffer.Unlock();
        //    //BufferedGeometryData.VertexData vertData = new BufferedGeometryData.VertexData();
        //    //vertData.Buffer = this.buffer;
        //    //vertData.BufferSize = Stride;
        //    //vertData.Format = Format;
        //    //buffer.vBuffers = new BufferedGeometryData.VertexData[] { vertData };
        //}
    }

    class GeomDataTransformer
    {
        public static void CreateBufferStream(ICollection<DataFields[]> streams, out GeomDataBufferStream bufferStream)
        {
            bufferStream = new GeomDataBufferStream();

            // create format order
            Dictionary<VertexFormats,int> fields = new Dictionary<VertexFormats,int>();
            List<DataFields> dataFields = new List<DataFields>();

            int index = 0;
            foreach (DataFields[] stream in streams)
            {
                foreach (DataFields field in stream)
                {
                    if (!fields.ContainsKey(field.Format))
                    {
                        dataFields.Add(field);
                        fields.Add(field.Format, index++);
                    }
                }
            }
            bufferStream.Fields = new VertexFormats[dataFields.Count];
            bufferStream.FieldPositions = new int[dataFields.Count];
            VertexFormats format = VertexFormats.None;
            int pos = 0;
            for (int i = 0; i < dataFields.Count; i++)
            {
                bufferStream.Fields[i] = dataFields[i].Format;
                format |= dataFields[i].Format;
                bufferStream.FieldPositions[i] = pos;

                pos += VertexFormatStride(dataFields[i].Format);
            }

            bufferStream.Stride = pos;
            bufferStream.Format = format;
        }

        public static int VertexFormatStride(VertexFormats format)
        {
            const int floatSz = 4;

            const int float4 = floatSz * 4;
            const int float3 = floatSz * 3;
            const int float2 = floatSz * 2;
            switch (format)
            {
                case VertexFormats.Diffuse:
                    return floatSz;
                case VertexFormats.Normal:
                    return float3;
                case VertexFormats.Position:
                    return float3;
                case VertexFormats.Texture0:
                case VertexFormats.Texture1:
                case VertexFormats.Texture2:
                case VertexFormats.Texture3:
                case VertexFormats.Texture4:
                    return float2;
                case VertexFormats.PointSize:
                    return floatSz;
                case VertexFormats.Transformed:
                    return float4;
                default:
                    return 0;
            }
        }
    }
}