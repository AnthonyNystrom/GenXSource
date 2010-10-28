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
    #region W
    /// <summary>
    /// INCOMPLETE
    /// </summary>
    public sealed class W : GaugeBoson, IInteractElectromagnetic
    {
        private Fraction electricCharge;

        public W() : base()
        {
            this.symbol = "W\u207B";
            this.spin = new Fraction(1);
            this.electricCharge = new Fraction(-1);
            this.antiParticle = new AntiW(this);
        }

        #region IInteractElectromagnetic
        public Fraction ElectricCharge
        {
            get { return this.electricCharge; }
        }
        #endregion
    }
    #endregion

    #region ANTIW
    public sealed class AntiW : AntiGaugeBoson<W>, IInteractElectromagnetic
    {
        public AntiW(W particle) : base(particle)
        {
            this.symbol = "W\u207A";
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
