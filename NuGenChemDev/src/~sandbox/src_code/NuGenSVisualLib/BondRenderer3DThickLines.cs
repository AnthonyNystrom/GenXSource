using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Org.OpenScience.CDK.Interfaces;
using NuGenSVisualLib.Rendering.Shading;
using Microsoft.DirectX;
using Org.OpenScience.CDK;
using NuGenSVisualLib.Rendering.Pipelines;
using NuGenSVisualLib.Settings;
using NuGenSVisualLib.Rendering.Effects;
using System.IO;
using NuGenSVisualLib.Rendering.Chem.Materials;

namespace NuGenSVisualLib.Rendering.Chem
{
    class BondThickLinesBufferCreator : BondGeometryCreator
    {
        int numSides;

        /// <summary>
        /// Initializes a new instance of the BondLinesBufferCreator class.
        /// </summary>
        public BondThickLinesBufferCreator(int numSides)
        {
            this.numSides = numSides;

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

            if (!coDesc.BondShadingDesc.BlendEndClrs)
                numActualBonds *= 2;

            int numTriangles = objs.Count * (numSides * 2);

            SphereMathHelper.EndType endType = SphereMathHelper.EndType.Open;
            float offset = 0.0f;
            switch (coDesc.BondShadingDesc.EndType)
            {
                case BondShadingDesc.BondEndTypes.Open:
                    break;
                case BondShadingDesc.BondEndTypes.Closed:
                    numTriangles += numActualBonds * (numSides * 2);
                    endType = SphereMathHelper.EndType.Flat;
                    break;
                case BondShadingDesc.BondEndTypes.Point:
                    offset = 0.05f;
                    numTriangles += numActualBonds * (numSides * 2);
                    endType = SphereMathHelper.EndType.Flat;
                    break;
                case BondShadingDesc.BondEndTypes.Rounded:
                    offset = 0.075f;
                    numTriangles += numActualBonds * (numSides * 6);
                    endType = SphereMathHelper.EndType.Rounded;
                    break;
            }

            // build template bond
            Vector3[] tCylinderPoints, tCylinderEndPoints1, tCylinderEndPoints2;
            Vector3[] tCylinderNormals, tCylinderEndNormals1, tCylinderEndNormals2;
            int[] tCylinderTris, tCylinderEndTris1, tCylinderEndTris2;

            SphereMathHelper.CalcCylinderTriangles(numSides, 1, 1.0f, new Vector3(0, 0, 0), new Vector3(0, 0, 1),
                                                   true, endType, offset, out tCylinderPoints,
                                                   out tCylinderNormals, out tCylinderTris, out tCylinderEndPoints1,
                                                   out tCylinderEndNormals1, out tCylinderEndTris1,
                                                   out tCylinderEndPoints2, out tCylinderEndNormals2,
                                                   out tCylinderEndTris2);
            int numActualVerts = tCylinderPoints.Length + tCylinderEndPoints1.Length + tCylinderEndPoints2.Length;
            int numActualTris = tCylinderTris.Length + tCylinderEndTris1.Length + tCylinderEndTris2.Length;

            // create buffers
            buffer = new BufferedGeometryData(device, objs.Count);
            buffer.vBuffers = new BufferedGeometryData.VertexData[1];
            buffer.vBuffers[0] = new BufferedGeometryData.VertexData();
            buffer.vBuffers[0].Stride = geomStream.Stride;
            buffer.vBuffers[0].NumElements = numActualVerts;
            buffer.vBuffers[0].Format = geomStream.Format;

            buffer.vBuffers[0].NumElements = numActualVerts * numActualBonds;
            buffer.vBuffers[0].Buffer = new VertexBuffer(device, geomStream.Stride * numActualTris * numActualBonds * 3,
                                                         Usage.WriteOnly, geomStream.Format, Pool.Managed);

            buffer.iBuffers = new BufferedGeometryData.IndexData[1];
            buffer.iBuffers[0] = new BufferedGeometryData.IndexData();
            buffer.iBuffers[0].Desc = BufferedGeometryData.IndexData.Description.Geometry;
            buffer.iBuffers[0].NumPrimitives = numActualTris;
            buffer.iBuffers[0].PrimType = PrimitiveType.TriangleList;

            buffer.iBuffers[0].NumPrimitives = (numActualTris * numActualBonds) / 3;
            /*buffer.iBuffers[0].Buffer = new IndexBuffer(typeof(int), numActualTris * numActualBonds, device,
                                                        Usage.WriteOnly, Pool.Managed);*/

            // lock stream
            GraphicsStream data = buffer.vBuffers[0].Buffer.Lock(0, 0, LockFlags.None);

            // write bonds to buffer
            Vector3 direction, directionUV;
            IAtom[] atoms;
            Vector3[] atomsPos;
            IMoleculeMaterial matA, matB;

            foreach (IBond bond in objs)
            {
                BondLinesBufferCreator.GenericBondSetup(bond, true, coDesc.BondShadingDesc, out direction,
                                                        out directionUV, out atoms, out atomsPos, out matA,
                                                        out matB);

                // calc bond positioning / instances
                Vector3[] bondInstances = null;
                float midPos;
                BondLinesBufferCreator.GenericBondCalcPositions(bond, coDesc.BondShadingDesc, direction,
                                                                directionUV, atoms, atomsPos, 0.15f,
                                                                out bondInstances, out midPos, false);

                long pos = 0;
                for (int bInst = 0; bInst < bondInstances.Length; bInst += 2)
                {
                    // translation
                    Matrix translate = Matrix.Translation(bondInstances[bInst]);

                    // rotation
                    double x = direction.X;
                    double y = direction.Y;
                    double z = direction.Z;

                    double alpha = (z == 0) ? Math.PI / 2 : Math.Atan(x / z);
                    double r = (alpha == 0) ? z : x / Math.Sin(alpha);
                    float sign = 1f;
                    if (z != 0)
                        sign *= Math.Sign(z);
                    else if (x != 0)
                        sign *= Math.Sign(x);
                    if (y != 0)
                        sign *= Math.Sign(y);
                    double theta = -sign * Math.Abs((r == 0) ? Math.PI / 2 : Math.Atan(y / r));

                    Matrix rotation = Matrix.RotationX((float)theta) * Matrix.RotationY((float)alpha);

                    // scaling
                    float zScale;
                    if (coDesc.BondShadingDesc.BlendEndClrs)
                        zScale = (bondInstances[1] - bondInstances[0]).Length();//(atomsPos[1] - atomsPos[0]).Length();//direction.Length();
                    else
                        zScale = midPos;
                    float xyScale = 0.05f; // thickness
                    Matrix scale = Matrix.Scaling(xyScale, xyScale, zScale);

                    // rotate & translate ends
                    if (tCylinderEndPoints1 != null)
                    {
                        Matrix endFinal = Matrix.Scaling(xyScale, xyScale, 1f) * rotation * translate;
                        Vector4[] tfEndTriangles = Vector3.Transform(tCylinderEndPoints1, endFinal);
                        // first end
                        for (int point = 0; point < tCylinderEndTris1.Length; point++)
                        {
                            int pointIdx = tCylinderEndTris1[point];
                            if (positionPos != -1)
                            {
                                data.Seek(pos + positionPos, SeekOrigin.Begin);
                                data.Write(tfEndTriangles[pointIdx].X);
                                data.Write(tfEndTriangles[pointIdx].Y);
                                data.Write(tfEndTriangles[pointIdx].Z);
                            }
                            if (normalPos != -1)
                            {
                                data.Seek(pos + normalPos, SeekOrigin.Begin);
                                data.Write(tCylinderEndNormals1[pointIdx].X);
                                data.Write(tCylinderEndNormals1[pointIdx].Y);
                                data.Write(tCylinderEndNormals1[pointIdx].Z);
                            }
                            if (diffusePos != -1)
                            {
                                data.Seek(pos + diffusePos, SeekOrigin.Begin);
                                data.Write(matA.BaseColor.ToArgb());
                            }
                            pos += geomStream.Stride;
                        }
                        // second end
                        endFinal = Matrix.Scaling(xyScale, xyScale, 1f) * rotation *
                                   Matrix.Translation(bondInstances[bInst + 1]);
                        tfEndTriangles = Vector3.Transform(tCylinderEndPoints2, endFinal);
                        // first end
                        for (int point = 0; point < tCylinderEndTris2.Length; point++)
                        {
                            int pointIdx = tCylinderEndTris2[point];
                            if (positionPos != -1)
                            {
                                data.Seek(pos + positionPos, SeekOrigin.Begin);
                                data.Write(tfEndTriangles[pointIdx].X);
                                data.Write(tfEndTriangles[pointIdx].Y);
                                data.Write(tfEndTriangles[pointIdx].Z);
                            }
                            if (normalPos != -1)
                            {
                                data.Seek(pos + normalPos, SeekOrigin.Begin);
                                data.Write(tCylinderEndNormals2[pointIdx].X);
                                data.Write(tCylinderEndNormals2[pointIdx].Y);
                                data.Write(tCylinderEndNormals2[pointIdx].Z);
                            }
                            if (diffusePos != -1)
                            {
                                data.Seek(pos + diffusePos, SeekOrigin.Begin);
                                data.Write(matB.BaseColor.ToArgb());
                            }
                            pos += geomStream.Stride;
                        }
                    }

                    Matrix final = scale * rotation * translate;

                    if (coDesc.BondShadingDesc.BlendEndClrs)
                    {
                        DrawSolidBondBlended(tCylinderPoints, tCylinderNormals, tCylinderTris,
                                             matA, matB, final, data, positionPos, normalPos,
                                             diffusePos, ref pos, geomStream.Stride, rotation);
                    }
                    else
                    {
                        float bLen2 = (bondInstances[bInst + 1] - (bondInstances[bInst] + (directionUV * midPos))).Length();

                        DrawSolidBondDistinct(tCylinderPoints, tCylinderNormals, tCylinderTris,
                                              matA, matB, final, data, xyScale, bLen2, rotation,
                                              bondInstances[bInst], directionUV, midPos, positionPos,
                                              normalPos, diffusePos, ref pos, geomStream.Stride);
                    }
                }
            }
        }

        public override void ClearCache()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private void DrawSolidBondDistinct(Vector3[] tCylinderPoints, Vector3[] tCylinderNormals,
                                           int[] tCylinderTris, IMoleculeMaterial matA,
                                           IMoleculeMaterial matB, Matrix final,
                                           GraphicsStream data, float xyScale, float bLen2, Matrix rotation,
                                           Vector3 bondInstance1, Vector3 directionUV, float midPos,
                                           int positionPos, int normalPos, int diffusePos, ref long pos,
                                           int stride)
        {
            // side 1
            Vector4[] tfTriangles = Vector3.Transform(tCylinderPoints, final);
            //Vector4[] rtNormals = Vector3.Transform(tCylinderNormals, rotation);
            for (int point = 0; point < tCylinderTris.Length; point++)
            {
                int pointIdx = tCylinderTris[point];
                if (positionPos != -1)
                {
                    data.Seek(pos + positionPos, SeekOrigin.Begin);
                    data.Write(tfTriangles[pointIdx].X);
                    data.Write(tfTriangles[pointIdx].Y);
                    data.Write(tfTriangles[pointIdx].Z);
                }
                if (normalPos != -1)
                {
                    data.Seek(pos + normalPos, SeekOrigin.Begin);
                    data.Write(tCylinderNormals[pointIdx].X);
                    data.Write(tCylinderNormals[pointIdx].Y);
                    data.Write(tCylinderNormals[pointIdx].Z);
                    //bondsVerts[vertsIdx].Normal = new Vector3(rtNormals[pointIdx].X,
                    //                                          rtNormals[pointIdx].Y,
                    //                                          rtNormals[pointIdx].Z);
                    //bondsVerts[vertsIdx].Normal.Normalize();
                }
                if (diffusePos != -1)
                {
                    data.Seek(pos + diffusePos, SeekOrigin.Begin);
                    data.Write(matA.BaseColor.ToArgb());
                }
                pos += stride;
            }

            // side 2
            final = Matrix.Scaling(xyScale, xyScale, bLen2) * rotation *
                    Matrix.Translation(bondInstance1 + (directionUV * midPos));
            tfTriangles = Vector3.Transform(tCylinderPoints, final);
            for (int point = 0; point < tCylinderTris.Length; point++)
            {
                int pointIdx = tCylinderTris[point];
                if (positionPos != -1)
                {
                    data.Seek(pos + positionPos, SeekOrigin.Begin);
                    data.Write(tfTriangles[pointIdx].X);
                    data.Write(tfTriangles[pointIdx].Y);
                    data.Write(tfTriangles[pointIdx].Z);
                }
                if (normalPos != -1)
                {
                    data.Seek(pos + normalPos, SeekOrigin.Begin);
                    data.Write(tCylinderNormals[pointIdx].X);
                    data.Write(tCylinderNormals[pointIdx].Y);
                    data.Write(tCylinderNormals[pointIdx].Z);
                    //bondsVerts[vertsIdx].Normal = new Vector3(rtNormals[pointIdx].X,
                    //                                          rtNormals[pointIdx].Y,
                    //                                          rtNormals[pointIdx].Z);
                    //bondsVerts[vertsIdx].Normal.Normalize();
                }
                if (diffusePos != -1)
                {
                    data.Seek(pos + diffusePos, SeekOrigin.Begin);
                    data.Write(matB.BaseColor.ToArgb());
                }
                pos += stride;
            }
        }

        private void DrawSolidBondBlended(Vector3[] tCylinderPoints, Vector3[] tCylinderNormals,
                                          int[] tCylinderTris, IMoleculeMaterial matA,
                                          IMoleculeMaterial matB, Matrix final, GraphicsStream data,
                                          int positionPos, int normalPos, int diffusePos,
                                          ref long pos, int stride, Matrix rotation)
        {
            // write transformed template to buffer
            Vector4[] tfTriangles = Vector3.Transform(tCylinderPoints, final);
            //Vector4[] rtNormals = Vector3.Transform(tCylinderNormals, rotation);
            int halfWay = tfTriangles.Length / 2;
            for (int point = 0; point < tCylinderTris.Length; point++)
            {
                int pointIdx = tCylinderTris[point];
                if (positionPos != -1)
                {
                    data.Seek(pos + positionPos, SeekOrigin.Begin);
                    data.Write(tfTriangles[pointIdx].X);
                    data.Write(tfTriangles[pointIdx].Y);
                    data.Write(tfTriangles[pointIdx].Z);
                }
                if (normalPos != -1)
                {
                    data.Seek(pos + normalPos, SeekOrigin.Begin);
                    data.Write(tCylinderNormals[pointIdx].X);
                    data.Write(tCylinderNormals[pointIdx].Y);
                    data.Write(tCylinderNormals[pointIdx].Z);
                    //bondsVerts[vertsIdx].Normal = new Vector3(rtNormals[pointIdx].X,
                    //                                          rtNormals[pointIdx].Y,
                    //                                          rtNormals[pointIdx].Z);
                }
                if (diffusePos != -1)
                {
                    data.Seek(pos + diffusePos, SeekOrigin.Begin);
                    if (pointIdx < halfWay)
                        data.Write(matA.BaseColor.ToArgb());
                    else
                        data.Write(matB.BaseColor.ToArgb());
                }
                pos += stride;
            }
        }
    }
}