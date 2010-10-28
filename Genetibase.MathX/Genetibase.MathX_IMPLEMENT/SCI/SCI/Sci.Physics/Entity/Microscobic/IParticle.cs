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
using Sci.Physics.Interaction;

namespace Sci.Physics.Entity.Microscobic
{
    public interface IParticle 
    {
        string Symbol { get;set;}
        bool Observed { get;set;}
        double Mass { get;set;}
        Fraction Spin { get;set;}
        Particle Antiparticle { get;}
    }

    public interface IFundamentalParticle : IParticle { }
    public interface ICompositeParticle : IParticle
    {
        List<Particle> Constituents { get;}
    }
    public interface IFermion { }
    public interface IBoson { }

    #region IGaugeBoson
    public interface IGaugeBoson : IFundamentalParticle, 
        IBoson 
    {
    }
    #endregion
    #region ILepton
    public interface ILepton : IFundamentalParticle , 
        IFermion,
        IInteractWeak
    {
        int LeptonNumber { get; }
    }
    #endregion
    #region IQuark
    public interface IQuark : IFundamentalParticle, 
        IFermion,
        IInteractStrong, 
        IInteractElectroweak
    {
        Fraction BaryonNumber { get;}
    }
    #endregion

    #region IHadron
    public interface IHadron : ICompositeParticle, 
        IInteractStrong ,
        IInteractElectromagnetic
    {
        string Makeup { get; }
    }
    #endregion
    #region IBaryon
    public interface IBaryon : IHadron, IFermion 
    { 
        Fraction BaryonNumber { get; } 
    }
    #endregion
    #region IMeson
    public interface IMeson : IHadron, IBoson { }
    #endregion
}
