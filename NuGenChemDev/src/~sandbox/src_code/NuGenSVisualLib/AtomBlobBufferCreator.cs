using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Chem.Materials;
using NuGenSVisualLib.Rendering.Effects;
using Org.OpenScience.CDK;
using Org.OpenScience.CDK.Interfaces;

namespace NuGenSVisualLib.Rendering.Chem
{
    class AtomBlobBufferCreator : AtomGeometryCreator
    {
        public AtomBlobBufferCreator()
        {
            availableFields = new DataFields[] { new DataFields(VertexFormats.Position, "POSITION"),
                                                 new DataFields(VertexFormats.PointSize, "SIZEFLOAT"),
                                                 new DataFields(VertexFormats.Normal, "DIFFUSE") };
        }

        public override void CreateGeometryForObjects(Device device, ICollection<IAtom> objs,
                                                      GeomDataBufferStream geomStream, int stream,
                                                      ref BufferedGeometryData buffer, CompleteOutputDescription coDesc)
        {
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
                        else if (fields[i].Usage == "SIZEFLOAT")
                            sizePos = geomStream.FieldPositions[gf];
                        break;
                    }
                }
            }

            // create buffers
            buffer = new BufferedGeometryData(device, objs.Count);
            buffer.vBuffers = new BufferedGeometryData.VertexData[1];
            buffer.vBuffers[0] = new BufferedGeometryData.VertexData();
            buffer.vBuffers[0].Buffer = new VertexBuffer(typeof(MetaBlobsEffect.PointVertex), objs.Count, device, Usage.None, VertexFormats.Position | VertexFormats.PointSize | VertexFormats.Diffuse, Pool.SystemMemory);
                /*new VertexBuffer(device, geomStream.Stride * objs.Count,
                                                         Usage.WriteOnly, geomStream.Format, Pool.SystemMemory);*/
            buffer.vBuffers[0].Stride = geomStream.Stride;
            buffer.vBuffers[0].NumElements = objs.Count;
            buffer.vBuffers[0].Format = geomStream.Format;
            buffer.DataValidity = BufferedGeometryData.DataValidityType.Source;
            buffer.Target = BufferedGeometryData.DataTarget.Geometry;

            // lock stream
            GraphicsStream data = buffer.vBuffers[0].Buffer.Lock(0, 0, LockFlags.None);

            // fill fields

            // create points
            AtomShadingDesc aShading = coDesc.AtomShadingDesc;
            long pos = 0;
            foreach (IAtom atom in objs)
            {
                int period = 1;
                if (atom.Properties.ContainsKey("Period"))
                    period = (int)atom.Properties["Period"];

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
                    data.Write((float)period);
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
                    if (material != null)
                    {
                        data.Seek(pos + diffusePos, SeekOrigin.Begin);
                        //data.Write((float)material.BaseColor.ToArgb());
                        if (material.BaseColor.R > 0)
                            data.Write(255f / material.BaseColor.R);
                        else
                            data.Write((float)0);
                        if (material.BaseColor.G > 0)
                            data.Write(255f / material.BaseColor.G);
                        else
                            data.Write((float)0);
                        if (material.BaseColor.B > 0)
                            data.Write(255f / material.BaseColor.B);
                        else
                            data.Write((float)0);
                    }
                }
                pos += geomStream.Stride;
            }
            buffer.vBuffers[0].Buffer.Unlock();
        }

        public override void SetupForCreation(int[] reqFields)
        {
            if (reqFields == null)
            {
                fields = availableFields;
            }
            else
            {
                fields = new DataFields[reqFields.Length];
                for (int i = 0; i < reqFields.Length; i++)
                {
                    fields[i] = availableFields[reqFields[i]];
                }
            }
        }

        public override void ClearCache()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}