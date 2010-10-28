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
    #region Muon
    public sealed class Muon : Lepton, IInteractElectromagnetic
    {
        public Muon()
            : base(new Fraction(-1, 2), new Fraction(-1))
        {
            this.symbol = "\u03BC\u207B";
            this.antiParticle = new AntiMuon(this);
        }

        #region IInteractElectromagnetic
        public Fraction ElectricCharge { get { return electricCharge; } }
        #endregion
    }
    #endregion

    #region AntiMuon
    public sealed class AntiMuon : AntiLepton<Muon>, IInteractElectromagnetic 
    {
        public AntiMuon(Muon particle) : base(particle)
        {
            this.symbol = "\u03BC\u207A";
        }

        #region IInteractElectromagnetic
        public Fraction ElectricCharge
        {
            get
            {
                IInteractElectromagnetic particle = antiParticle as IInteractElectromagnetic;
                return (particle.ElectricCharge == Fraction.Zero) ?
                    Fraction.Zero : -particle.ElectricCharge;
            }
        }
        #endregion
    }
    #endregion
}
