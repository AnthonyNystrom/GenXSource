using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Org.OpenScience.CDK.Interfaces;
using Microsoft.DirectX;
using NuGenSVisualLib.Rendering.Effects;
using System.IO;

namespace NuGenSVisualLib.Rendering.Chem
{
    class AtomSymbolSpriteBufferCreator : AtomGeometryCreator
    {
        public struct PointSprite
        {
            public Vector3 Position;
            public float Size;
        }

        public AtomSymbolSpriteBufferCreator()
        {
            availableFields = new DataFields[] { new DataFields(VertexFormats.Position, "POSITION"),
                                                 new DataFields(VertexFormats.PointSize, "POINTSIZE")
                                               };
        }

        public override void CreateGeometryForObjects(Device device, ICollection<IAtom> objs,
                                                      GeomDataBufferStream geomStream, int stream,
                                                      ref BufferedGeometryData buffer, CompleteOutputDescription coDesc)
        {
            // fillable fields
            int positionPos = -1;
            int sizePos = -1;

            // match field locations
            for (int i = 0; i < fields.Length; i++)
            {
                for (int gf = 0; gf < geomStream.Fields.Length; gf++)
                {
                    if (fields[i].Format == geomStream.Fields[gf])
                    {
                        if (fields[i].Usage == "POSITION")
                            positionPos = geomStream.FieldPositions[gf];
                        else if (fields[i].Usage == "POINTSIZE")
                            sizePos = geomStream.FieldPositions[gf];
                        break;
                    }
                }
            }

            // pre-scan for index groups
            Dictionary<string, List<int>> groups = new Dictionary<string, List<int>>();
            int aIdx = 0;
            foreach (IAtom atom in objs)
            {
                if (!groups.ContainsKey(atom.Symbol))
                    groups[atom.Symbol] = new List<int>();
                groups[atom.Symbol].Add(aIdx++);
            }

            // create buffers
            buffer = new BufferedGeometryData(device, objs.Count);
            buffer.vBuffers = new BufferedGeometryData.VertexData[1];
            buffer.vBuffers[0] = new BufferedGeometryData.VertexData();
            buffer.vBuffers[0].Buffer = new VertexBuffer(typeof(PointSprite), objs.Count, device, Usage.WriteOnly,
                                                         geomStream.Format, Pool.Managed);
            buffer.vBuffers[0].Stride = geomStream.Stride;
            buffer.vBuffers[0].NumElements = objs.Count;
            buffer.vBuffers[0].Format = geomStream.Format;

            buffer.DataValidity = BufferedGeometryData.DataValidityType.View;

            ChemSymbolTextures cTex = ChemSymbolTextures.Instance;

            buffer.iBuffers = new BufferedGeometryData.IndexData[groups.Count];

            int gIdx = 0;
            foreach (KeyValuePair<string,List<int>> group in groups)
            {
                buffer.iBuffers[gIdx] = new BufferedGeometryData.IndexData();
                buffer.iBuffers[gIdx].Desc = BufferedGeometryData.IndexData.Description.Sprites;
                buffer.iBuffers[gIdx].NumPrimitives = group.Value.Count;
                buffer.iBuffers[gIdx].PrimType = PrimitiveType.PointList;
                buffer.iBuffers[gIdx].Textures = new Texture[] { cTex[group.Key] };

                buffer.iBuffers[gIdx].Buffer = new IndexBuffer(typeof(int), group.Value.Count, device,
                                                               Usage.WriteOnly, Pool.Managed);
                int[] indices = (int[])buffer.iBuffers[gIdx].Buffer.Lock(0, LockFlags.None);
                for (int i = 0; i < indices.Length; i++)
                {
                    indices[i] = group.Value[i];
                }
                buffer.iBuffers[gIdx].Buffer.Unlock();

                gIdx++;
            }

            // lock stream
            GraphicsStream data = buffer.vBuffers[0].Buffer.Lock(0, 0, LockFlags.None);

            AtomShadingDesc aShading = coDesc.AtomShadingDesc;

            long pos = 0;
            foreach (IAtom atom in objs)
            {
                if (positionPos != -1)
                {
                    data.Seek(pos + positionPos, SeekOrigin.Begin);
                    data.Write((float)atom.X3d);
                    data.Write((float)atom.Y3d);
                    data.Write((float)atom.Z3d);
                }
                if (sizePos != -1)
                {
                    data.Seek(pos + sizePos, SeekOrigin.Begin);
                    int period = 1;
                    if (atom.Properties.ContainsKey("Period"))
                        period = (int)atom.Properties["Period"];
                    data.Write((float)period * 0.2f);
                }
                pos += geomStream.Stride;
            }
            buffer.vBuffers[0].Buffer.Unlock();
        }

        public override void ClearCache()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UpdateBuffer(BufferedGeometryData buffer, Vector3 viewPos, Matrix worldMat,
                                 IAtom[] atoms)
        {
            int stride = buffer.vBuffers[0].Stride;
            int objs = buffer.vBuffers[0].NumElements;

            GraphicsStream data = buffer.vBuffers[0].Buffer.Lock(0, 0, LockFlags.None);

            long pos = 0;
            foreach (IAtom atom in atoms)
            {
                // transform atom point to world-space
                Vector3 atomPos = new Vector3((float)atom.X3d, (float)atom.Y3d, (float)atom.Z3d);
                Vector4 atomPosT = Vector3.Transform(atomPos, worldMat);
                // calc V to view pos
                Vector3 atomToView = new Vector3(atomPosT.X, atomPosT.Y, atomPosT.Z) - viewPos;
                atomToView.Normalize();
                Vector3 atomNewPos = (atomToView * 0.3f) + atomPos;

                data.Seek(pos, SeekOrigin.Begin);
                data.Write(atomNewPos.X);
                data.Write(atomNewPos.Y);
                data.Write(atomNewPos.Z);

                pos += stride;
            }

            buffer.vBuffers[0].Buffer.Unlock();
        }
    }
}
