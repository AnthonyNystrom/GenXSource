using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Pipelines;
using Org.OpenScience.CDK.Interfaces;
using Microsoft.DirectX;

namespace NuGenSVisualLib.Rendering.Chem
{
    //class BondThickLineBufferedData : BondBufferedData
    //{
    //    public BondThickLineBufferedData(Device device, int numBonds, CompleteOutputDescription coDesc)
    //        : base(device, numBonds, coDesc)
    //    {
    //        this.reqInter = false;

    //        vBuffers = new VertexData[1];
    //        vBuffers[0] = new VertexData();
    //        vBuffers[0].Format = CustomVertex.PositionNormalColored.Format;

    //        iBuffers = new IndexData[1];
    //        iBuffers[0] = new IndexData();
    //        iBuffers[0].Desc = IndexData.Description.Geometry;
    //        iBuffers[0].PrimType = PrimitiveType.TriangleList;
    //    }

    //    public override void DrawBuffers(GraphicsPipeline3D pipeline)
    //    {
    //        device.RenderState.CullMode = Cull.CounterClockwise;
    //        device.RenderState.Lighting = true;
    //        //device.RenderState.FillMode = FillMode.WireFrame;

    //        device.VertexFormat = vBuffers[0].Format;
    //        device.SetStreamSource(0, vBuffers[0].Buffer, 0);
    //        device.DrawPrimitives(iBuffers[0].PrimType, 0, iBuffers[0].NumPrimitives);
    //    }

    //    public override void DrawBuffer(GraphicsPipeline3D pipeline, int index)
    //    {
    //        throw new Exception("The method or operation is not implemented.");
    //    }
    //}

    //class BondThickLineBufferCreator : BondBufferCreator
    //{
    //    public BondThickLineBufferCreator()
    //        : base(false, false, false, true, true, false)
    //    { }

    //    public override object FillBuffer(BondBufferedData buffer, List<IBond[]> bondSets, int numBonds)
    //    {
    //        int[] tCylinderTris;
    //        Vector3[] tCylinderPoints;
    //        Vector3[] tCylinderNormals;
    //        int[] tCylinderEndTris1;
    //        Vector3[] tCylinderEndPoints1;
    //        Vector3[] tCylinderEndNormals1;
    //        int[] tCylinderEndTris2;
    //        Vector3[] tCylinderEndPoints2;
    //        Vector3[] tCylinderEndNormals2;

    //        // count bond orders via preview data
    //        int numActualBonds = 0;
    //        foreach (IBond[] bonds in bondSets)
    //        {
    //            foreach (IBond bond in bonds)
    //            {
    //                numActualBonds += (int)bond.Order;
    //            }
    //        }

    //        if (!buffer.coDesc.BondShadingDesc.BlendEndClrs)
    //            numActualBonds *= 2;

    //        int numTriangles = numActualBonds * 10;

    //        SphereMathHelper.EndType endType = SphereMathHelper.EndType.Open;
    //        float offset = 0.0f;
    //        switch (buffer.coDesc.BondShadingDesc.EndType)
    //        {
    //            case BondShadingDesc.BondEndTypes.Open:
    //                break;
    //            case BondShadingDesc.BondEndTypes.Closed:
    //                numTriangles += numActualBonds * 10;
    //                endType = SphereMathHelper.EndType.Flat;
    //                break;
    //            case BondShadingDesc.BondEndTypes.Point:
    //                offset = 0.05f;
    //                numTriangles += numActualBonds * 10;
    //                endType = SphereMathHelper.EndType.Flat;
    //                break;
    //            case BondShadingDesc.BondEndTypes.Rounded:
    //                offset = 0.075f;
    //                numTriangles += numActualBonds * 30;
    //                endType = SphereMathHelper.EndType.Rounded;
    //                break;
    //        }

    //        buffer.iBuffers[0].NumPrimitives = numTriangles;

    //        // 5x1 cylinder
    //        buffer.vBuffers[0].BufferSize = numTriangles * 3;
    //        buffer.vBuffers[0].Buffer = new VertexBuffer(typeof(CustomVertex.PositionNormalColored), numTriangles * 3, buffer.device,
    //                                                     Usage.None, CustomVertex.PositionNormalColored.Format, Pool.Managed);

    //        // build template bond
    //        SphereMathHelper.CalcCylinderTriangles(5, 1, 1.0f, new Vector3(0, 0, 0), new Vector3(0, 0, 1),
    //                                               true, endType, offset, out tCylinderPoints,
    //                                               out tCylinderNormals, out tCylinderTris, out tCylinderEndPoints1,
    //                                               out tCylinderEndNormals1, out tCylinderEndTris1,
    //                                               out tCylinderEndPoints2, out tCylinderEndNormals2,
    //                                               out tCylinderEndTris2);

    //        CustomVertex.PositionNormalColored[] bondsVerts = (CustomVertex.PositionNormalColored[])buffer.vBuffers[0].Buffer.Lock(0, LockFlags.None);
    //        int vertsIdx = 0;
    //        BondShadingDesc bShading = buffer.coDesc.BondShadingDesc;
    //        // create bonds
    //        foreach (IBond[] bonds in bondSets)
    //        {
    //            foreach (IBond bond in bonds)
    //            {
    //                Vector3 direction, directionUV;
    //                IAtom[] atoms;
    //                Vector3[] atomsPos;
    //                IMoleculeMaterial matA, matB;
    //                BondLinesBufferCreator.GenericBondSetup(bond, true, bShading, out direction, out directionUV,
    //                                                        out atoms, out atomsPos, out matA, out matB);

    //                // calc bond positioning / instances
    //                Vector3[] bondInstances = null;
    //                float midPos;
    //                BondLinesBufferCreator.GenericBondCalcPositions(bond, bShading, direction, directionUV,
    //                                                                atoms, atomsPos, 0.15f, out bondInstances,
    //                                                                out midPos, false);

    //                // draw bond instances
    //                for (int bInst = 0; bInst < bondInstances.Length; bInst += 2)
    //                {
    //                    // translation
    //                    Matrix translate = Matrix.Translation(bondInstances[bInst]);

    //                    // rotation
    //                    double x = direction.X;
    //                    double y = direction.Y;
    //                    double z = direction.Z;

    //                    double alpha = (z == 0) ? Math.PI / 2 : Math.Atan(x / z);
    //                    double r = (alpha == 0) ? z : x / Math.Sin(alpha);
    //                    float sign = 1f;
    //                    if (z != 0)
    //                        sign *= Math.Sign(z);
    //                    else if (x != 0)
    //                        sign *= Math.Sign(x);
    //                    if (y != 0)
    //                        sign *= Math.Sign(y);
    //                    double theta = -sign * Math.Abs((r == 0) ? Math.PI / 2 : Math.Atan(y / r));

    //                    Matrix rotation = Matrix.RotationX((float)theta) * Matrix.RotationY((float)alpha);

    //                    // scaling
    //                    float zScale;
    //                    if (bShading.BlendEndClrs)
    //                        zScale = (bondInstances[1] - bondInstances[0]).Length();//(atomsPos[1] - atomsPos[0]).Length();//direction.Length();
    //                    else
    //                        zScale = midPos;
    //                    float xyScale = 0.05f; // thickness
    //                    Matrix scale = Matrix.Scaling(xyScale, xyScale, zScale);

    //                    // rotate & translate ends
    //                    if (tCylinderEndPoints1 != null)
    //                    {
    //                        Matrix endFinal = Matrix.Scaling(xyScale, xyScale, 1f) * rotation * translate;
    //                        Vector4[] tfEndTriangles = Vector3.Transform(tCylinderEndPoints1, endFinal);
    //                        // first end
    //                        for (int point = 0; point < tCylinderEndTris1.Length; point++)
    //                        {
    //                            int pointIdx = tCylinderEndTris1[point];
    //                            bondsVerts[vertsIdx].Position = new Vector3(tfEndTriangles[pointIdx].X,
    //                                                                        tfEndTriangles[pointIdx].Y,
    //                                                                        tfEndTriangles[pointIdx].Z);
    //                            bondsVerts[vertsIdx].Normal = tCylinderEndNormals1[pointIdx];
    //                            bondsVerts[vertsIdx++].Color = matA.BaseColor.ToArgb();
    //                        }
    //                        // second end
    //                        endFinal = Matrix.Scaling(xyScale, xyScale, 1f) * rotation *
    //                                   Matrix.Translation(bondInstances[bInst + 1]);
    //                        tfEndTriangles = Vector3.Transform(tCylinderEndPoints2, endFinal);
    //                        // first end
    //                        for (int point = 0; point < tCylinderEndTris2.Length; point++)
    //                        {
    //                            int pointIdx = tCylinderEndTris2[point];
    //                            bondsVerts[vertsIdx].Position = new Vector3(tfEndTriangles[pointIdx].X,
    //                                                                        tfEndTriangles[pointIdx].Y,
    //                                                                        tfEndTriangles[pointIdx].Z);
    //                            bondsVerts[vertsIdx].Normal = tCylinderEndNormals2[pointIdx];
    //                            bondsVerts[vertsIdx++].Color = matB.BaseColor.ToArgb();
    //                        }
    //                    }

    //                    Matrix final = scale * rotation * translate;

    //                    if (bShading.BlendEndClrs)
    //                    {
    //                        DrawSolidBondBlended(tCylinderPoints, tCylinderNormals, matA, matB, final, bondsVerts, rotation,
    //                                             tCylinderTris, ref vertsIdx);
    //                    }
    //                    else
    //                    {
    //                        float bLen2 = (bondInstances[bInst + 1] - (bondInstances[bInst] + (directionUV * midPos))).Length();

    //                        DrawSolidBondDistinct(tCylinderPoints, tCylinderNormals, matA, matB, final, bondsVerts,
    //                                              xyScale, bLen2, rotation, bondInstances[bInst], directionUV, midPos,
    //                                              tCylinderTris, ref vertsIdx);
    //                    }
    //                }
    //            }
    //        }

    //        buffer.vBuffers[0].Buffer.Unlock();

    //        return null;
    //    }

    //    private void DrawSolidBondDistinct(Vector3[] tCylinderPoints, Vector3[] tCylinderNormals, IMoleculeMaterial matA,
    //                                       IMoleculeMaterial matB, Matrix final,
    //                                       CustomVertex.PositionNormalColored[] bondsVerts,
    //                                       float xyScale, float bLen2, Matrix rotation,
    //                                       Vector3 bondInstance1, Vector3 directionUV, float midPos,
    //                                       int[] tCylinderTris, ref int vertsIdx)
    //    {
    //        // side 1
    //        Vector4[] tfTriangles = Vector3.Transform(tCylinderPoints, final);
    //        //Vector4[] rtNormals = Vector3.Transform(tCylinderNormals, rotation);
    //        for (int point = 0; point < tCylinderTris.Length; point++)
    //        {
    //            int pointIdx = tCylinderTris[point];
    //            bondsVerts[vertsIdx].Position = new Vector3(tfTriangles[pointIdx].X,
    //                                                        tfTriangles[pointIdx].Y,
    //                                                        tfTriangles[pointIdx].Z);
    //            //bondsVerts[vertsIdx].Normal = new Vector3(rtNormals[pointIdx].X,
    //            //                                          rtNormals[pointIdx].Y,
    //            //                                          rtNormals[pointIdx].Z);
    //            //bondsVerts[vertsIdx].Normal.Normalize();
    //            bondsVerts[vertsIdx].Normal = tCylinderNormals[pointIdx];

    //            bondsVerts[vertsIdx++].Color = matA.BaseColor.ToArgb();
    //        }

    //        // side 2
    //        final = Matrix.Scaling(xyScale, xyScale, bLen2) * rotation *
    //                Matrix.Translation(bondInstance1 + (directionUV * midPos));
    //        tfTriangles = Vector3.Transform(tCylinderPoints, final);
    //        for (int point = 0; point < tCylinderTris.Length; point++)
    //        {
    //            int pointIdx = tCylinderTris[point];
    //            bondsVerts[vertsIdx].Position = new Vector3(tfTriangles[pointIdx].X,
    //                                                        tfTriangles[pointIdx].Y,
    //                                                        tfTriangles[pointIdx].Z);
    //            //bondsVerts[vertsIdx].Normal = new Vector3(rtNormals[pointIdx].X,
    //            //                                          rtNormals[pointIdx].Y,
    //            //                                          rtNormals[pointIdx].Z);
    //            //bondsVerts[vertsIdx].Normal.Normalize();
    //            bondsVerts[vertsIdx].Normal = tCylinderNormals[pointIdx];

    //            bondsVerts[vertsIdx++].Color = matB.BaseColor.ToArgb();
    //        }
    //    }

    //    private void DrawSolidBondBlended(Vector3[] tCylinderPoints, Vector3[] tCylinderNormals, IMoleculeMaterial matA,
    //                                      IMoleculeMaterial matB, Matrix final, CustomVertex.PositionNormalColored[] bondsVerts,
    //                                      Matrix rotation, int[] tCylinderTris, ref int vertsIdx)
    //    {
    //        // write transformed template to buffer
    //        Vector4[] tfTriangles = Vector3.Transform(tCylinderPoints, final);
    //        //Vector4[] rtNormals = Vector3.Transform(tCylinderNormals, rotation);
    //        int halfWay = tfTriangles.Length / 2;
    //        for (int point = 0; point < tCylinderTris.Length; point++)
    //        {
    //            int pointIdx = tCylinderTris[point];
    //            bondsVerts[vertsIdx].Position = new Vector3(tfTriangles[pointIdx].X,
    //                                                        tfTriangles[pointIdx].Y,
    //                                                        tfTriangles[pointIdx].Z);
    //            //bondsVerts[vertsIdx].Normal = new Vector3(rtNormals[pointIdx].X,
    //            //                                          rtNormals[pointIdx].Y,
    //            //                                          rtNormals[pointIdx].Z);
    //            bondsVerts[vertsIdx].Normal = tCylinderNormals[pointIdx];

    //            if (pointIdx < halfWay)
    //                bondsVerts[vertsIdx++].Color = matA.BaseColor.ToArgb();
    //            else
    //                bondsVerts[vertsIdx++].Color = matB.BaseColor.ToArgb();
    //        }
    //    }

    //    public override BondBufferedData CreateBuffer(Device device, CompleteOutputDescription coDesc, int size)
    //    {
    //        return new BondThickLineBufferedData(device, -1, coDesc);
    //    }

    //    public override void FinalizeBuffer(BondBufferedData buffer, object iData)
    //    {
    //    }

    //    public override bool NeedsUpdateBuffer(BondBufferedData buffer, CompleteOutputDescription newCoDesc)
    //    {
    //        throw new Exception("The method or operation is not implemented.");
    //    }

    //    public override void UpdateBuffer(BondBufferedData buffer, CompleteOutputDescription newCoDesc,
    //                                      GraphicsPipeline3D pipeline,
    //                                      BufferedGeometryData.DataValidityType changes)
    //    {
    //        throw new Exception("The method or operation is not implemented.");
    //    }
    //}
}
