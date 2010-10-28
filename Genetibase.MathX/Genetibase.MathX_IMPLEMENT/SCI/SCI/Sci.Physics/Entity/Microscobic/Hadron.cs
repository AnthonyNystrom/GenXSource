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
    #region Hadron
    public abstract class Hadron : Particle, IHadron
    {
        protected List<Particle> constituents = new List<Particle>();
        protected int bottomness;
        protected int topness;
        protected int strangeness;
        protected int charmness;
        protected Fraction electricCharge = Fraction.Zero;
        protected Fraction isospinZ = Fraction.Zero;
        protected Fraction hypercharge = Fraction.Zero;

        protected Hadron() : base(){}
        public Hadron(params Particle[] constituents) : base()
        {
            foreach (Particle particle in constituents) 
            { 
                Add(particle); 
            }
        }

        protected bool Colorless()
        {
            int red = 0;
            int green = 0;
            int blue = 0;

            int count = constituents.Count;
            for (int i = 0; i < count; i++)
            {
                IInteractStrong particle = constituents[i] as IInteractStrong;
                
                ColorCharge charge = particle.Color.Charge;
                switch (charge)
                {
                    case ColorCharge.RED:
                        red++; break;
                    case ColorCharge.GREEN:
                        green++; break;
                    case ColorCharge.BLUE:
                        blue++; break;
                    case ColorCharge.ANTIRED:
                        red--; break;
                    case ColorCharge.ANTIGREEN:
                        green--; break;
                    case ColorCharge.ANTIBLUE:
                        blue--; break;
                    default:
                        break;
                }
            }
            return (red == green && green == blue) ? true : false;
        }
        protected void Add(Particle particle)
        {
            IInteractStrong strong = particle as IInteractStrong;
            if (strong == null)
                throw new Exception("INVALID HADRONIC STRUCTURE!");

            bottomness += strong.Bottomness;
            topness += strong.Topness;
            strangeness += strong.Strangeness;
            charmness += strong.Charmness;
            isospinZ += strong.IsospinZ;
            hypercharge += strong.Hypercharge;

            IInteractElectromagnetic eCharged = particle as IInteractElectromagnetic;
            if (eCharged != null)
                electricCharge += eCharged.ElectricCharge;

            this.constituents.Add(particle);
        }

        #region IHadron
        public List<Particle> Constituents
        {
            get
            {
                List<Particle> particles = new List<Particle>();
                foreach (Particle p in constituents)
                {
                    particles.Add((Particle)p.Clone());
                }
                return particles;
            }
        }
        public string Makeup
        {
            get
            {
                StringBuilder makeup = new StringBuilder();
                foreach (Particle p in constituents)
                {
                    makeup.Append((p.Symbol != "") ? p.Symbol : "#");
                }
                return makeup.ToString();
            }
        }
        #endregion

        #region IInteractStrong
        public Color Color
        {
            get
            {
                if (!Colorless())
                    throw new Exception("INVALID HADRONIC STRUCTUE!");
                return ColorFactory.GetColourFactory().GetColour(ColorCharge.WHITE);
            }
        }
        public int Bottomness { get { return bottomness; } }
        public int Topness { get { return topness; } }
        public int Strangeness { get { return strangeness; } }
        public int Charmness { get { return charmness; } }
        public Fraction IsospinZ { get { return isospinZ; } }
        public Fraction Hypercharge { get { return hypercharge; } }
        #endregion

        #region IInteractElectromagnetic
        public Fraction ElectricCharge { get { return electricCharge; } }
        #endregion

        public override bool Equals(object obj)
        {
            if (!(obj is Hadron) || !base.Equals(obj)) return false;

            Hadron p = obj as Hadron;
            if(!(this.constituents.Equals(p.constituents)))
                return false;


            return (bottomness.Equals(p.bottomness) &&
                    topness.Equals(p.topness) &&
                    charmness.Equals(p.charmness) &&
                    strangeness.Equals(p.strangeness) &&
                    isospinZ.Equals(p.isospinZ) &&
                    hypercharge.Equals(p.hypercharge)) ? true : false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() ^
                    bottomness.GetHashCode() ^
                    topness.GetHashCode() ^
                    charmness.GetHashCode() ^
                    strangeness.GetHashCode() ^
                    isospinZ.GetHashCode() ^
                    hypercharge.GetHashCode();
        }
    }
    #endregion

    #region AntiHadron
    public abstract class AntiHadron<T> : Antiparticle<T>, IHadron
        where T : Hadron
    {
        public AntiHadron(T particle) : base(particle) { }

        #region IHadron
        public List<Particle> Constituents
        {
            get
            {
                List<Particle> constituents = new List<Particle>();
                foreach (Particle p in ((Hadron)antiParticle).Constituents)
                {
                    constituents.Add((Particle)p.Antiparticle.Clone());
                }
                return constituents;
            }
        }
        public string Makeup
        {
            get
            {
                StringBuilder makeup = new StringBuilder();
                foreach (Particle p in this.Constituents)
                {
                    makeup.Append((p.Symbol != "") ? p.Symbol : "#");
                }
                return makeup.ToString();
            }
        }
        #endregion

        #region IInteractStrong
        public Color Color { get { return (((Hadron)antiParticle).Color); } }
        public int Bottomness{ get { return -(((Hadron)antiParticle).Bottomness); } }
        public int Topness{ get { return -(((Hadron)antiParticle).Topness); } }
        public int Strangeness{ get { return -(((Hadron)antiParticle).Strangeness); } }
        public int Charmness { get { return -(((Hadron)antiParticle).Charmness); } }
        public Fraction IsospinZ
        {
            get 
            {
                return (((Hadron)antiParticle).IsospinZ == Fraction.Zero) ? 
                    Fraction.Zero : 
                    -((Hadron)antiParticle).IsospinZ;
            }
        }
        public Fraction Hypercharge
        {
            get
            {
                return (((Hadron)antiParticle).Hypercharge == Fraction.Zero) ?
                    Fraction.Zero :
                    -((Hadron)antiParticle).Hypercharge;
            }
        }
        #endregion

        #region IInteractElectromagnetic
        public Fraction ElectricCharge
        {
            get
            {
                return (((Hadron)antiParticle).ElectricCharge == Fraction.Zero) ?
                    Fraction.Zero :
                    -((Hadron)antiParticle).ElectricCharge;
            }
        }
        #endregion
    }
    #endregion
}

