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
    #region Quark
    public abstract class Quark : Particle, IQuark
    {
        private Fraction baryonNumber;
        private Color color;
        private int bottomness;
        private int topness;
        private int strangeness;
        private int charmness;
        private Fraction isospinZ;
        private Fraction hypercharge;
        private Fraction electricCharge;
        private Fraction weakIsospin;
        private Fraction weakHypercharge;

        public Quark(   Color color, 
                        int bottomness, 
                        int topness, 
                        int charmness, 
                        int strangeness, 
                        Fraction isospinZ, 
                        Fraction weakIsospin
                    ) : base()
        {
            this.baryonNumber = new Fraction(1, 3);
            this.color = (!(color is AntiColor)) ? color : new Red();
            this.spin = new Fraction(1, 2);
            this.bottomness = bottomness;
            this.topness = topness;
            this.charmness = charmness;
            this.strangeness = strangeness;
            this.isospinZ = isospinZ;
            this.weakIsospin = weakIsospin;

            this.hypercharge = baryonNumber + strangeness + charmness + bottomness + topness;
            this.weakHypercharge = 2 * (isospinZ + hypercharge / 2 - weakIsospin);
            this.electricCharge = isospinZ + hypercharge / 2;
        }

        #region IQuark
        public Fraction BaryonNumber { get { return baryonNumber; } }
        #endregion

        #region IInteractStrong
        public Color Color { get { return color; } }
        public int Bottomness { get { return bottomness; } }
        public int Topness { get { return topness; } }
        public int Strangeness { get { return strangeness; } }
        public int Charmness { get { return charmness; } }
        public Fraction IsospinZ { get { return isospinZ; } }
        public Fraction Hypercharge { get { return hypercharge; } }
        #endregion

        #region IInteractElectroweak
        public Fraction ElectricCharge { get { return electricCharge; } }
        public Fraction WeakIsospin { get { return weakIsospin; } }
        public Fraction WeakHypercharge { get { return weakHypercharge; } }
        #endregion

        public override bool Equals(object obj)
        {
            if (!(obj is Quark) || !base.Equals(obj)) return false;

            Quark p = obj as Quark;
            return (baryonNumber.Equals(p.baryonNumber) &&
                    color.Equals(p.color) &&
                    bottomness.Equals(p.bottomness) &&
                    topness.Equals(p.topness) &&
                    strangeness.Equals(p.strangeness) &&
                    charmness.Equals(p.charmness) &&
                    isospinZ.Equals(p.isospinZ) &&
                    hypercharge.Equals(p.hypercharge) &&
                    weakIsospin.Equals(p.weakIsospin) &&
                    weakHypercharge.Equals(p.weakHypercharge)
                    ) ? true : false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                    baryonNumber.GetHashCode() ^
                    color.GetHashCode() ^
                    bottomness.GetHashCode()^
                    topness.GetHashCode()^
                    strangeness.GetHashCode()^
                    charmness.GetHashCode()^
                    isospinZ.GetHashCode()^
                    hypercharge.GetHashCode()^
                    weakIsospin.GetHashCode()^
                    weakHypercharge.GetHashCode();
        }
    }
    #endregion

    #region AntiQuark
    public abstract class AntiQuark<T> : Antiparticle<T>, IQuark
        where T : Quark
    {
        protected AntiColor color;

        public AntiQuark(T particle) : base(particle)
        {
            IInteractStrong quark = particle as IInteractStrong;
            switch (quark.Color.Charge)
            {
                case ColorCharge.RED:
                    color = (AntiColor)(ColorFactory.GetColourFactory().GetColour(ColorCharge.ANTIRED));
                    break;
                case ColorCharge.GREEN:
                    color = (AntiColor)(ColorFactory.GetColourFactory().GetColour(ColorCharge.ANTIGREEN));
                    break;
                case ColorCharge.BLUE:
                    color = (AntiColor)(ColorFactory.GetColourFactory().GetColour(ColorCharge.ANTIBLUE));
                    break;
                default:
                    color = (AntiColor)(ColorFactory.GetColourFactory().GetColour(ColorCharge.ANTIRED));
                    break;
            }
        }

        #region IQuark
        public Fraction BaryonNumber
        {
            get
            {
                return (((Quark)antiParticle).BaryonNumber == Fraction.Zero) ?
                    Fraction.Zero :
                    -((Quark)antiParticle).BaryonNumber;
            }
        }
        #endregion

        #region IInteractStrong
        public Color Color { get { return color; } }
        public int Bottomness { get { return -((Quark)antiParticle).Bottomness; } }
        public int Topness{ get { return -((Quark)antiParticle).Topness; } }
        public int Strangeness{ get { return -((Quark)antiParticle).Strangeness; } }
        public int Charmness { get { return -((Quark)antiParticle).Charmness; } }
        public Fraction IsospinZ
        {
            get
            {
                return (((Quark)antiParticle).IsospinZ == Fraction.Zero) ?
                    Fraction.Zero :
                    -((Quark)antiParticle).IsospinZ;
            }
        }
        public Fraction Hypercharge
        {
            get
            {
                return (((Quark)antiParticle).Hypercharge == Fraction.Zero) ?
                    Fraction.Zero :
                    -((Quark)antiParticle).Hypercharge;
            }
        }
        #endregion

        #region IInteractElectroweak
        public Fraction ElectricCharge
        {
            get
            {
                return (((Quark)antiParticle).ElectricCharge == Fraction.Zero) ?
                    Fraction.Zero :
                    -((Quark)antiParticle).ElectricCharge;
            }
        }
        public Fraction WeakIsospin
        {
            get
            {
                return (((Quark)antiParticle).WeakIsospin == Fraction.Zero) ?
                    Fraction.Zero :
                    -((Quark)antiParticle).WeakIsospin;
            }
        }
        public Fraction WeakHypercharge
        {
            get
            {
                return (((Quark)antiParticle).WeakHypercharge == Fraction.Zero) ?
                    Fraction.Zero :
                    -((Quark)antiParticle).WeakHypercharge;      
            }
        }
        #endregion
    }
    #endregion
}
