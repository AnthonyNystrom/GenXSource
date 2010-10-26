using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Pipelines;
using Org.OpenScience.CDK.Interfaces;
using Microsoft.DirectX;
using Org.OpenScience.CDK;
using System.Reflection;
using System.IO;
using NuGenSVisualLib.Rendering.Chem.Schemes;
using NuGenSVisualLib.Rendering.Effects;
using NuGenSVisualLib.Rendering.Chem.Materials;

namespace NuGenSVisualLib.Rendering.Chem
{
    class AtomSpriteBufferCreator : AtomGeometryCreator
    {
        Texture spriteTexture;

        public struct PointSprite
        {
            public Vector3 Position;
            public float Size;
            public int Diffuse;
        }

        public AtomSpriteBufferCreator()
        {
            availableFields = new DataFields[] { new DataFields(VertexFormats.Position, "POSITION"),
                                                 new DataFields(VertexFormats.PointSize, "POINTSIZE"),
                                                 new DataFields(VertexFormats.Diffuse, "DIFFUSE"),
                                               };
        }

        public override void CreateGeometryForObjects(Device device, ICollection<IAtom> objs,
                                                      GeomDataBufferStream geomStream, int stream,
                                                      ref BufferedGeometryData buffer, CompleteOutputDescription coDesc)
        {
            if (spriteTexture == null)
            {
                Stream texstm = Assembly.GetExecutingAssembly().GetManifestResourceStream("NuGenSVisualLib.Resources.Atom.PNG");
                spriteTexture = TextureLoader.FromStream(device, texstm);
            }

            // fillable fields
            int positionPos = -1;
            int sizePos = -1;
            int diffusePos = -1;

            // match field locations
            for (int i = 0; i < fields.Length; i++)
            {
                for (int gf = 0; gf < geomStream.Fields.Length; gf++)
                {
                    if (fields[i].Format == geomStream.Fields[gf])
                    {
                        if (fields[i].Usage == "POSITION")
                            positionPos = geomStream.FieldPositions[gf];
                        else if (fields[i].Usage == "DIFFUSE")
                            diffusePos = geomStream.FieldPositions[gf];
                        else if (fields[i].Usage == "POINTSIZE")
                            sizePos = geomStream.FieldPositions[gf];
                        break;
                    }
                }
            }

            // create buffers
            buffer = new BufferedGeometryData(device, objs.Count);
            buffer.vBuffers = new BufferedGeometryData.VertexData[1];
            buffer.vBuffers[0] = new BufferedGeometryData.VertexData();
            buffer.vBuffers[0].Buffer = new VertexBuffer(typeof(PointSprite), objs.Count, device, Usage.WriteOnly,
                                                         geomStream.Format, Pool.Managed);
                /*new VertexBuffer(device, geomStream.Stride * objs.Count,
                                                         Usage.None, geomStream.Format, Pool.Managed);*/
            buffer.vBuffers[0].Stride = geomStream.Stride;
            buffer.vBuffers[0].NumElements = objs.Count;
            buffer.vBuffers[0].Format = geomStream.Format;

            buffer.iBuffers = new BufferedGeometryData.IndexData[1];
            buffer.iBuffers[0] = new BufferedGeometryData.IndexData();
            buffer.iBuffers[0].Desc = BufferedGeometryData.IndexData.Description.Sprites;
            buffer.iBuffers[0].NumPrimitives = objs.Count;
            buffer.iBuffers[0].PrimType = PrimitiveType.PointList;

            buffer.iBuffers[0].Textures = new Texture[] { spriteTexture };

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
                    data.Write((float)period * 0.4f);
                }
                if (diffusePos != -1)
                {
                    IMoleculeMaterialLookup lookup = aShading.MoleculeMaterials;
                    IMoleculeMaterialTemplate matTemp = lookup.ResolveBySymbol(atom.Symbol);
                    IMoleculeMaterial material = null;
                    if (matTemp != null)
                        material = matTemp.BySymbol;
                    else
                    {
                        PeriodicTableElement pe = (PeriodicTableElement)atom.Properties["PeriodicTableElement"];
                        if (pe != null)
                            material = lookup.GetBySeries(pe.ChemicalSerie);
                    }

                    data.Seek(pos + diffusePos, SeekOrigin.Begin);
                    data.Write(material.BaseColor.ToArgb());
                }
                pos += geomStream.Stride;
            }

            /*Dictionary<int, List<int>> atomSizeGroups = new Dictionary<int, List<int>>();
            // first split into group counts
            int aIdx = 0;
            foreach (IAtom[] atoms in atomSets)
            {
                foreach (IAtom atom in atoms)
                {
                    int period = 1;
                    if (atom.Properties.ContainsKey("Period"))
                        period = (int)atom.Properties["Period"];

                    List<int> groupAtoms = null;
                    if (!atomSizeGroups.TryGetValue(period, out groupAtoms))
                        atomSizeGroups.Add(period, groupAtoms = new List<int>());

                    groupAtoms.Add(aIdx++);
                }
            }

            int vertsIdx = 0;
            Dictionary<int, List<int>>.Enumerator group = atomSizeGroups.GetEnumerator();
            sBuffer.groupLengths = new int[atomSizeGroups.Count];
            sBuffer.groupSizes = new int[atomSizeGroups.Count];
            sBuffer.groupStarts = new int[atomSizeGroups.Count];
            int bIdx = 0;
            while (group.MoveNext())
            {
                int groupPeriod = group.Current.Key;
                List<int> groupMembers = group.Current.Value;
                aIdx = 0;
                int gIdx = 0;
                sBuffer.groupSizes[bIdx] = groupPeriod;
                sBuffer.groupStarts[bIdx] = vertsIdx;
                sBuffer.groupLengths[bIdx] = groupMembers.Count;
                foreach (IAtom[] atoms in atomSets)
                {
                    foreach (IAtom atom in atoms)
                    {
                        if (aIdx == groupMembers[gIdx])
                        {
                            IMoleculeMaterialLookup lookup = aShading.MoleculeMaterials;
                            IMoleculeMaterialTemplate matTemp = lookup.ResolveBySymbol(atom.Symbol);
                            IMoleculeMaterial material = null;
                            if (matTemp != null)
                                material = matTemp.BySymbol;
                            else
                            {
                                PeriodicTableElement pe = (PeriodicTableElement)atom.Properties["PeriodicTableElement"];
                                if (pe != null)
                                    material = lookup.GetBySeries(pe.ChemicalSerie);
                            }

                            atomVerts[vertsIdx].Position = new Vector3((float)atom.X3d, (float)atom.Y3d, (float)atom.Z3d);
                            atomVerts[vertsIdx].Color = material.BaseColor.ToArgb();
                            vertsIdx++;
                            gIdx++;
                        }
                        if (gIdx == groupMembers.Count)
                            break;
                        aIdx++;
                    }
                    if (gIdx == groupMembers.Count)
                        break;
                }
                bIdx++;
            }*/
            buffer.vBuffers[0].Buffer.Unlock();
        }

        public override void ClearCache()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
