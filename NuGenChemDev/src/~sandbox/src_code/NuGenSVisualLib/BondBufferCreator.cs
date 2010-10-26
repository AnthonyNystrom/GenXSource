using System;
using System.Collections.Generic;
using System.Text;
using Org.OpenScience.CDK.Interfaces;
using Microsoft.DirectX.Direct3D;

namespace NuGenSVisualLib.Rendering.Chem
{
    /// <summary>
    /// Encapsulates a set of buffered data that represents the geometry of a bond
    /// </summary>
    //public abstract class BondBufferedData : BufferedGeometryData
    //{
    //    public BondBufferedData(Device device, int numBonds, CompleteOutputDescription coDesc)
    //        : base(device, numBonds, coDesc)
    //    { }
    //}

    ///// <summary>
    ///// Encapsulates bond buffer creation
    ///// </summary>
    //public abstract class BondBufferCreator : BufferedDataCreator<BondBufferedData, IBond>
    //{
    //    public BondBufferCreator(bool allowsSingleFill, bool allowsArrayFill, bool allowsListFill,
    //                             bool allowsListArrayFill, bool allowsOverwrite,
    //                             bool requiresIntermediateData)
    //        : base(allowsSingleFill, allowsArrayFill, allowsListFill, allowsListArrayFill,
    //               allowsOverwrite, requiresIntermediateData)
    //    { }

    //    public override void FillBuffer(BondBufferedData buffer, IBond bond) { throw new Exception("The method or operation is not implemented."); }
    //    public override void FillBuffer(BondBufferedData buffer, IBond[] bonds) { throw new Exception("The method or operation is not implemented."); }
    //    public override void FillBuffer(BondBufferedData buffer, List<IBond> bonds) { throw new Exception("The method or operation is not implemented."); }
    //    public override object FillBuffer(BondBufferedData buffer, List<IBond[]> bondSets, int numBonds) { throw new Exception("The method or operation is not implemented."); }
    //    public override void OverwriteBuffer(BondBufferedData buffer, IBond bond, int location) { throw new Exception("The method or operation is not implemented."); }
    //}
}