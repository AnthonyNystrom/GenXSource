using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Org.OpenScience.CDK.Interfaces;
using NuGenSVisualLib.Rendering.Pipelines;
using Microsoft.DirectX;
using Org.OpenScience.CDK;
using NuGenSVisualLib.Rendering.Effects;
using System.IO;
using NuGenSVisualLib.Rendering.Chem.Materials;

namespace NuGenSVisualLib.Rendering.Chem
{
    class BondLinesBufferCreator : BondGeometryCreator
    {
        public BondLinesBufferCreator()
	    {
            availableFields = new DataFields[] { new DataFields(VertexFormats.Position, "POSITION"),
                                                 //new DataFields(VertexFormats.Texture1, "TEXTURE0"),
                                                 new DataFields(VertexFormats.Normal, "NORMAL"),
                                                 new DataFields(VertexFormats.Diffuse, "DIFFUSE"),
                                               };
	    }

        public override void CreateGeometryForObjects(Device device, ICollection<IBond> objs,
                                                      GeomDataBufferStream geomStream, int stream,
                                                      ref BufferedGeometryData buffer, CompleteOutputDescription coDesc)
        {
            // fillable fields
            int positionPos = -1;
            int normalPos = -1;
            int diffusePos = -1;
            int texPos = -1;

            // match field locations
            //int[] fieldsPos = new int[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                for (int gf = 0; gf < geomStream.Fields.Length; gf++)
                {
                    if (fields[i].Format == geomStream.Fields[gf])
                    {
                        if (fields[i].Usage == "POSITION")
                            positionPos = geomStream.FieldPositions[gf];
                        else if (fields[i].Usage == "NORMAL")
                            normalPos = geomStream.FieldPositions[gf];
                        else if (fields[i].Usage == "DIFFUSE")
                            diffusePos = geomStream.FieldPositions[gf];
                        else if (fields[i].Usage == "TEXTURE0")
                            texPos = geomStream.FieldPositions[gf];
                        //fieldsPos[i] = geomStream.FieldPositions[gf];
                        break;
                    }
                }
            }

            // count bond orders via preview data
            int numActualBonds = 0;
            foreach (IBond bond in objs)
            {
                numActualBonds += (int)bond.Order;
            }

            int numVerts = 0;
            if (coDesc.BondShadingDesc.BlendEndClrs)
                numVerts = numActualBonds * 2;
            else
                numVerts = numActualBonds * 4;

            // create buffers
            buffer = new BufferedGeometryData(device, objs.Count);
            buffer.vBuffers = new BufferedGeometryData.VertexData[1];
            buffer.vBuffers[0] = new BufferedGeometryData.VertexData();
            buffer.vBuffers[0].Stride = geomStream.Stride;
            buffer.vBuffers[0].NumElements = numVerts;
            buffer.vBuffers[0].Format = geomStream.Format;

            buffer.iBuffers = new BufferedGeometryData.IndexData[1];
            buffer.iBuffers[0] = new BufferedGeometryData.IndexData();
            buffer.iBuffers[0].Desc = BufferedGeometryData.IndexData.Description.Geometry;
            buffer.iBuffers[0].NumPrimitives = numVerts / 2;
            buffer.iBuffers[0].PrimType = PrimitiveType.LineList;

            buffer.vBuffers[0].NumElements = numVerts;
            buffer.vBuffers[0].Buffer = new VertexBuffer(device, geomStream.Stride * numVerts,
                                                         Usage.WriteOnly, geomStream.Format, Pool.Managed);

            // write bonds to buffer
            Vector3 direction, directionUV;
            IAtom[] atoms;
            Vector3[] atomsPos;
            IMoleculeMaterial materialA, materialB;
            Vector3[] bondInstances = null;
            float midPos;
            BondShadingDesc bShading = coDesc.BondShadingDesc;

            // lock stream
            GraphicsStream data = buffer.vBuffers[0].Buffer.Lock(0, 0, LockFlags.None);

            long pos = 0;
            foreach (IBond bond in objs)
            {
                GenericBondSetup(bond, false, bShading, out direction, out directionUV,
                                 out atoms, out atomsPos, out materialA, out materialB);

                // calc bond positioning / instances
                GenericBondCalcPositions(bond, bShading, direction, directionUV,
                                         atoms, atomsPos, 0.1f, out bondInstances,
                                         out midPos, true);

                // draw bond instances
                for (int bInst = 0; bInst < bondInstances.Length; bInst += 2)
                {
                    if (bShading.BlendEndClrs)
                    {
                        GenericDrawBondSolidBlended(bondInstances[bInst], bondInstances[bInst + 1],
                                                    materialA, materialB, data, ref pos, geomStream.Stride, positionPos, diffusePos);
                        //GenericDrawBondDashedBlended(bondInstances[bInst], bondInstances[bInst + 1],
                        //                            materialA, materialB);
                    }
                    else
                    {
                        Vector3 sectA = directionUV * midPos;

                        // convert into points (2 lines)
                        if (positionPos != -1)
                        {
                            data.Seek(pos + positionPos, SeekOrigin.Begin);
                            data.Write(bondInstances[bInst].X);
                            data.Write(bondInstances[bInst].Y);
                            data.Write(bondInstances[bInst].Z);

                            data.Seek(pos + geomStream.Stride + positionPos, SeekOrigin.Begin);
                            Vector3 p = bondInstances[bInst] + sectA;
                            data.Write(p.X);
                            data.Write(p.Y);
                            data.Write(p.Z);

                            data.Seek(pos + geomStream.Stride + geomStream.Stride + positionPos, SeekOrigin.Begin);
                            data.Write(p.X);
                            data.Write(p.Y);
                            data.Write(p.Z);

                            data.Seek(pos + geomStream.Stride + geomStream.Stride + geomStream.Stride + positionPos, SeekOrigin.Begin);
                            data.Write(bondInstances[bInst + 1].X);
                            data.Write(bondInstances[bInst + 1].Y);
                            data.Write(bondInstances[bInst + 1].Z);
                        }
                        if (diffusePos != -1)
                        {
                            data.Seek(pos + diffusePos, SeekOrigin.Begin);
                            int clr = materialA.BaseColor.ToArgb();
                            data.Write(clr);

                            data.Seek(pos + geomStream.Stride + diffusePos, SeekOrigin.Begin);
                            data.Write(clr);

                            clr = materialB.BaseColor.ToArgb();
                            data.Seek(pos + geomStream.Stride + geomStream.Stride + diffusePos, SeekOrigin.Begin);
                            data.Write(clr);

                            data.Seek(pos + geomStream.Stride + geomStream.Stride + geomStream.Stride + diffusePos, SeekOrigin.Begin);
                            data.Write(clr);
                        }

                        pos += geomStream.Stride * 4;
                    }
                }
            }

            buffer.vBuffers[0].Buffer.Unlock();
        }

        public override void ClearCache()
        {
        }

        public static void GenericBondSetup(IBond bond, bool sort, BondShadingDesc bShading,
                                            out Vector3 direction, out Vector3 directionUV,
                                            out IAtom[] atoms, out Vector3[] atomsPos,
                                            out IMoleculeMaterial matA, out IMoleculeMaterial matB)
        {
            atoms = bond.getAtoms();

            Vector3 v1 = new Vector3((float)atoms[0].X3d, (float)atoms[0].Y3d, (float)atoms[0].Z3d);
            Vector3 v2 = new Vector3((float)atoms[1].X3d, (float)atoms[1].Y3d, (float)atoms[1].Z3d);
            direction = v2 - v1;

            atomsPos = new Vector3[2];
            if (sort)
            {
                if (direction.Z < (v1 - v2).Z)
                {
                    IAtom temp = atoms[0];
                    atoms[0] = atoms[1];
                    atoms[1] = temp;

                    atomsPos[0] = v2;
                    atomsPos[1] = v1;
                }
                else
                {
                    atomsPos[0] = v1;
                    atomsPos[1] = v2;
                }

                direction = Vector3.Normalize(atomsPos[1] - atomsPos[0]);
            }
            else
            {
                atomsPos[0] = v1;
                atomsPos[1] = v2;
            }

            directionUV = Vector3.Normalize(direction);

            IMoleculeMaterialLookup lookup = bShading.MoleculeMaterials;
            IMoleculeMaterialTemplate matTemp = lookup.ResolveBySymbol(atoms[0].Symbol);
            if (matTemp != null)
                matA = matTemp.BySymbol;
            else
            {
                PeriodicTableElement pe = (PeriodicTableElement)atoms[0].Properties["PeriodicTableElement"];
                matA = lookup.GetBySeries(pe.ChemicalSerie);
            }

            matTemp = lookup.ResolveBySymbol(atoms[1].Symbol);
            if (matTemp != null)
                matB = matTemp.BySymbol;
            else
            {
                PeriodicTableElement pe = (PeriodicTableElement)atoms[1].Properties["PeriodicTableElement"];
                matB = lookup.GetBySeries(pe.ChemicalSerie);
            }
        }

        public static void GenericBondCalcPositions(IBond bond, BondShadingDesc bShading, Vector3 direction,
                                            Vector3 directionUV, IAtom[] atoms, Vector3[] atomsPos,
                                            float orderSpacing, out Vector3[] bondInstances, out float midPos,
                                            bool ignoreEnds)
        {
            float orderSpace = ((float)bond.Order - 1f) * orderSpacing;

            Vector3 bondCross = Vector3.Normalize(Vector3.Cross(direction, new Vector3(0, 1, 0)));

            // calc A->B spacing positions
            Vector3 startPosA = atomsPos[0] -
                                (bondCross * (orderSpace / 2f));
            Vector3 startPosB = atomsPos[1] -
                                (bondCross * (orderSpace / 2f));

            // determine weighting of the 2 atoms
            int periodA = 1;
            if (atoms[0].Properties.ContainsKey("Period"))
                periodA = (int)atoms[0].Properties["Period"];
            int periodB = 1;
            if (atoms[1].Properties.ContainsKey("Period"))
                periodB = (int)atoms[1].Properties["Period"];

            float periodDif = periodA - periodB;

            float sizeA = 0.2f * periodA;
            float sizeB = 0.2f * periodB;

            // adjust for ends
            if (!ignoreEnds)
            {
                Vector3 endShift;
                switch (bShading.EndType)
                {
                    case BondShadingDesc.BondEndTypes.Point:
                        endShift = directionUV * 0.05f;
                        startPosA += endShift;
                        startPosB -= endShift;
                        break;
                    case BondShadingDesc.BondEndTypes.Rounded:
                        endShift = directionUV * 0.075f;
                        startPosA += endShift;
                        startPosB -= endShift;
                        break;
                }
            }

            // calc point exactly between both atoms
            float midLen = ((startPosB - startPosA).Length()/*direction.Length()*/ - sizeA - sizeB) / 2f;
            midPos = sizeA + midLen;

            // adjust for spacing
            switch (bShading.Spacing)
            {
                case BondShadingDesc.BondSpacings.CenterSpace:
                    startPosA += directionUV * sizeA;
                    startPosB -= directionUV * sizeB;

                    // recalc
                    midLen = (startPosB - startPosA).Length() / 2;
                    midPos = midLen;
                    break;
            }

            // calc instances for both ends
            bondInstances = new Vector3[(int)(bond.Order * 2f)];
            int bondIdx = 0;
            float shift = 0;
            for (int order = 0; order < bond.Order; order++)
            {
                bondInstances[bondIdx] = startPosA + (bondCross * shift);
                bondInstances[bondIdx + 1] = startPosB + (bondCross * shift);

                bondIdx += 2;
                shift += orderSpacing;
            }
        }

        private void GenericDrawBondDashedBlended(Vector3 start, Vector3 end, IMoleculeMaterial matA, IMoleculeMaterial matB,
                                          ref int vertsIdx, CustomVertex.PositionColored[] bondsVerts)
        {
            float dashFreq = 0.1f;

            // generate lnear dashed 3d line
            Vector3[] dashes;
            LineHelperMath.CalcDashedLineLinear(start, end, dashFreq, out dashes);

            // NOTE: Batch sampling or something? to improve speed
            ColourHelperMath.ColourGradient gradient;
            ColourHelperMath.GenerateLinearGradient(matA.BaseColor, matB.BaseColor, out gradient);

            // copy to buffer
            int numDashes = dashes.Length / 2;
            float lineLen = (end - start).Length();
            float dashScale = 1f / (lineLen / (dashFreq * 2f));
            float startScale = 0f;
            float endScale = dashScale;
            for (int dash = 0; dash < numDashes; dash += 2)
            {
                bondsVerts[vertsIdx].Position = dashes[dash];
                bondsVerts[vertsIdx].Color = gradient.Sample(startScale);
                vertsIdx++;

                bondsVerts[vertsIdx].Position = dashes[dash + 1];
                bondsVerts[vertsIdx].Color = gradient.Sample(endScale);
                vertsIdx++;

                startScale += dashScale;
                endScale += dashScale;
            }
        }

        private void GenericDrawBondSolidBlended(Vector3 start, Vector3 end,
                                                 IMoleculeMaterial matA, IMoleculeMaterial matB,
                                                 GraphicsStream data, ref long pos, int stride,
                                                 int positionPos, int diffusePos)
        {
            if (positionPos != -1)
            {
                data.Seek(pos + positionPos, SeekOrigin.Begin);
                data.Write(start.X);
                data.Write(start.Y);
                data.Write(start.Z);

                data.Seek(pos + stride + positionPos, SeekOrigin.Begin);
                data.Write(end.X);
                data.Write(end.Y);
                data.Write(end.Z);
            }
            if (diffusePos != -1)
            {
                data.Seek(pos + diffusePos, SeekOrigin.Begin);
                data.Write(matA.BaseColor.ToArgb());

                data.Seek(pos + stride + diffusePos, SeekOrigin.Begin);
                data.Write(matB.BaseColor.ToArgb());
            }
            pos += stride * 2;
        }
    }
}
