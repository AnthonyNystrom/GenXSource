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
    #region Electron
    public sealed class Electron : Lepton, IInteractElectromagnetic
    {
        public Electron()
            : base(new Fraction(-1, 2), new Fraction(-1))
        {
            this.symbol = "e\u207B";
            this.antiParticle = new AntiElectron(this);
        }

        #region IInteractElectromagnetic
        public Fraction ElectricCharge { get { return electricCharge; } }
        #endregion
    } 
    #endregion

    #region AntiElectron
    public sealed class AntiElectron : AntiLepton<Electron>, IInteractElectromagnetic 
    {
        public AntiElectron(Electron particle) : base(particle)
        {
            this.symbol = "e\u207A";
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
