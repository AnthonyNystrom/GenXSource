/* *****************************************************************************
 * AUTHOR       : Coskun OBA
 * EMAIL        : oba.coskun@hotmail.com
 * 
 * DATE         : JANUARY 2007
 * *****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

using Sci.Math;

namespace Sci.Physics.Interaction
{
    #region IInteractStrong
    public interface IInteractStrong
    {
        Color Color { get; }
        int Bottomness { get;}
        int Topness { get;}
        int Strangeness { get;}
        int Charmness { get; }
        Fraction IsospinZ { get; }
        Fraction Hypercharge { get; }
    }
    #endregion

    #region IInteractElectromagnetic
    public interface IInteractElectromagnetic
    {
        Fraction ElectricCharge { get; }
    }
    #endregion

    #region IInteractWeak
    public interface IInteractWeak
    {
        Fraction WeakIsospin { get; }
        Fraction WeakHypercharge { get; }
    }
    #endregion

    #region IInteractElectroweak
    public interface IInteractElectroweak : IInteractElectromagnetic,
        IInteractWeak
    {
    }
    #endregion

    #region IInteractGravitational
    public interface IInteractGravitational
    {
    }
    #endregion
}
