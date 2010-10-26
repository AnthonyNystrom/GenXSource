using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using Genetibase.NuGenRenderCore.Rendering;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenRenderCore.Resources
{
    public class ShapeContentLoader
    {
        public static Shape LoadShape(string file, Device gDevice)
        {
            // TODO: Special xml loader?
            XmlDocument xml = new XmlDocument();
            xml.Load(file);
            return LoadShape(xml, gDevice);
        }

        public static Shape LoadShape(Device gDevice, string xml)
        {
            // TODO: Special xml loader?
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return LoadShape(xmlDoc, gDevice);
        }

        public static Shape LoadShape(Stream stream, Device gDevice)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(stream);
            return LoadShape(xml, gDevice);
        }

        public static Shape LoadShape(XmlDocument xml, Device gDevice)
        {
            XmlNode shapeEl = xml.SelectSingleNode("shape");
            if (shapeEl != null)
            {
                string[] values = shapeEl.Attributes["scale"].InnerText.Split(' ');
                Vector3 scale = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));

                // load all verts
                XmlNodeList vertElList = shapeEl.SelectNodes("vertices");
                List<VertexBuffer> vertList = new List<VertexBuffer>();
                List<int> vertCountList = new List<int>();
                List<VertexFormats> vFormats = new List<VertexFormats>();
                Color clr1 = Color.LightBlue, clr2 = Color.Blue;
                Vector3 clrScaled = new Vector3((clr1.R - clr2.R) / 255f, (clr1.G - clr2.G) / 255f, (clr1.B - clr2.B) / 255f);
                float clrScale = 0.5f;
                Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue),
                        max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                foreach (XmlElement vertsEl in vertElList)
                {
                    // get origin
                    Vector3 origin = new Vector3();
                    if (vertsEl.Attributes["origin"] != null)
                    {
                        string[] xyz = vertsEl.Attributes["origin"].InnerText.Split(' ');
                        origin.X = float.Parse(xyz[0]);
                        origin.Y = float.Parse(xyz[1]);
                        origin.Z = float.Parse(xyz[2]);
                    }
                    string[] vertValues = vertsEl.InnerText.Split(new char[] { ' ', '\t', '\r', '\n' },
                                                                  StringSplitOptions.RemoveEmptyEntries);
                    VertexBuffer vBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), vertValues.Length / 3,
                                                            gDevice, Usage.WriteOnly, CustomVertex.PositionColored.Format,
                                                            Pool.Managed);
                    CustomVertex.PositionColored[] verts = (CustomVertex.PositionColored[])vBuffer.Lock(0, LockFlags.None);

                    int valueIDx = 0;
                    for (int vIdx = 0; vIdx < verts.Length; vIdx++)
                    {
                        float x = (origin.X + float.Parse(vertValues[valueIDx++])) * scale.X;
                        float y = (origin.Y + float.Parse(vertValues[valueIDx++])) * scale.Y;
                        float z = (origin.Z + float.Parse(vertValues[valueIDx++])) * scale.Z;

                        if (x < min.X)
                            min.X = x;
                        if (x > max.X)
                            max.X = x;
                        if (y < min.Y)
                            min.Y = y;
                        if (y > max.Y)
                            max.Y = y;
                        if (z < min.Z)
                            min.Z = z;
                        if (z > max.Z)
                            max.Z = z;

                        // calc grad clr
                        float clrAmount = y * clrScale;
                        Color clr = Color.FromArgb(clr2.R + (int)(clrScaled.X * clrAmount * 255),
                                                   clr2.G + (int)(clrScaled.Y * clrAmount * 255),
                                                   clr2.B + (int)(clrScaled.Z * clrAmount * 255));
                        verts[vIdx] = new CustomVertex.PositionColored(x, y, z, clr.ToArgb());
                    }
                    vBuffer.Unlock();
                    vertList.Add(vBuffer);
                    vertCountList.Add(verts.Length);
                    vFormats.Add(CustomVertex.PositionColored.Format);
                }

                // load indices
                XmlNodeList indexElList = shapeEl.SelectNodes("indicies");
                List<IndexBuffer> indexList = new List<IndexBuffer>();
                List<int> indexCountList = new List<int>();
                List<int> vRefs = new List<int>();
                List<PrimitiveType> primTypes = new List<PrimitiveType>();
                List<int> primCounts = new List<int>();
                foreach (XmlElement indicesEl in indexElList)
                {
                    string[] indexValues = indicesEl.InnerText.Split(new char[] { ' ', '\t', '\r', '\n' },
                                                                     StringSplitOptions.RemoveEmptyEntries);
                    IndexBuffer iBuffer = new IndexBuffer(typeof(int), indexValues.Length, gDevice, Usage.WriteOnly, Pool.Managed);
                    int[] indices = (int[])iBuffer.Lock(0, LockFlags.None);

                    for (int i = 0; i < indices.Length; i++)
                    {
                        indices[i] = int.Parse(indexValues[i]);
                    }
                    iBuffer.Unlock();
                    indexList.Add(iBuffer);
                    indexCountList.Add(indices.Length);
                    vRefs.Add(int.Parse(indicesEl.Attributes["v"].InnerText));
                    primTypes.Add(PrimitiveType.TriangleList);
                    primCounts.Add(indices.Length / 3);
                }

                Shape shape = new Shape(null, vertList.ToArray(), indexList.ToArray(),
                                        vertCountList.ToArray(), indexCountList.ToArray(),
                                        vRefs.ToArray(), primTypes.ToArray(), vFormats.ToArray(),
                                        primCounts.ToArray(), new BoundingBox(min, max));
                return shape;
            }
            return null;
        }
    }
}