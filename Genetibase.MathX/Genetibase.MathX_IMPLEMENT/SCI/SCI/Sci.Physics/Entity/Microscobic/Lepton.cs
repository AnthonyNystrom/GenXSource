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
    #region Lepton
    public abstract class Lepton : Particle, ILepton
    {
        private int leptonNumber;
        private Fraction weakIsospin;
        private Fraction weakHypercharge;
        protected Fraction electricCharge;

        public Lepton(Fraction weakIsospin, Fraction weakHypercharge) : base()
        {
            this.leptonNumber = 1;
            this.spin = new Fraction(1, 2);
            this.weakIsospin = weakIsospin;
            this.weakHypercharge = weakHypercharge;

            this.electricCharge = weakIsospin + weakHypercharge / 2;
        }

        #region ILepton
        public int LeptonNumber { get { return leptonNumber; } }
        #endregion

        #region IInteractWeak
        public Fraction WeakIsospin { get { return weakIsospin; } }
        public Fraction WeakHypercharge { get { return weakHypercharge; } }
        #endregion

        public override bool Equals(object obj)
        {
            if (!(obj is Lepton) || !base.Equals(obj)) return false;

            Lepton p = obj as Lepton;
            return (leptonNumber.Equals(p.leptonNumber) &&
                    weakIsospin.Equals(p.weakIsospin) &&
                    weakHypercharge.Equals(p.weakHypercharge)) ? true : false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                    leptonNumber.GetHashCode() ^
                    weakIsospin.GetHashCode() ^
                    weakHypercharge.GetHashCode();
        }
    } 
    #endregion

    #region AntiLepton
    public abstract class AntiLepton<T> : Antiparticle<T>, ILepton
        where T : Lepton
    {
        public AntiLepton(T particle) : base(particle) { }

        #region ILepton
        public int LeptonNumber
        {
            get 
            {
                ILepton particle = antiParticle as ILepton;
                return (particle.LeptonNumber == 0) ? 0 : -particle.LeptonNumber;  
            }
        }
        #endregion

        #region IInteractWeak
        public Fraction WeakIsospin
        {
            get
            {
                return (((Lepton)antiParticle).WeakIsospin == Fraction.Zero) ?
                    Fraction.Zero :
                    -((Lepton)antiParticle).WeakIsospin;
            }
        }
        public Fraction WeakHypercharge
        {
            get
            {
                return (((Lepton)antiParticle).WeakHypercharge == Fraction.Zero) ?
                    Fraction.Zero :
                    -((Lepton)antiParticle).WeakHypercharge;
            }
        }
        #endregion
    } 
    #endregion
}
