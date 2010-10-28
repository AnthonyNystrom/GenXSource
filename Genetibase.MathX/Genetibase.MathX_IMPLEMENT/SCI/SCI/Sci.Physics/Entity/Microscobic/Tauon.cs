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
    #region Tauon
    public sealed class Tauon : Lepton, IInteractElectromagnetic
    {
        public Tauon() 
            : base(new Fraction(-1, 2), new Fraction(-1))
        {
            this.symbol = "\u03C4\u207B";            
            this.antiParticle = new AntiTauon(this);
        }

        #region IInteractElectromagnetic
        public Fraction ElectricCharge { get { return electricCharge; } }
        #endregion
    }
    #endregion

    #region AntiTauon
    public sealed class AntiTauon : AntiLepton<Tauon>, IInteractElectromagnetic 
    {
        public AntiTauon(Tauon particle) : base(particle)
        {
            this.symbol = "\u03C4\u207A";
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
