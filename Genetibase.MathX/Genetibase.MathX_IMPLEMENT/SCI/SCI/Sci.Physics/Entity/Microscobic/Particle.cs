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
using Sci.Physics.Entity;

namespace Sci.Physics.Entity.Microscobic
{
    #region Particle
    public abstract class Particle : Entity, IParticle
    {
        protected string symbol = string.Empty;
        protected bool observed;
        protected double mass;
        protected Fraction spin = Fraction.Zero;
        protected Particle antiParticle;

        public Particle() : base()
        {
            this.observed = true;
            this.spin = new Fraction(0);
        }

        public string Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }
        public double Mass
        {
            get { return mass; }
            set { mass = value; }
        }
        public bool Observed
        {
            get { return observed; }
            set { observed = value; }
        }
        public Fraction Spin
        {
            get { return spin; }
            set 
            {
                if (this is IFermion)
                {
                    if (value.Denominator != 2)
                        throw new Exception("Invalid Fermion!");
                }
                else if (this is IBoson)
                {
                    if ((value != Fraction.Zero && value.Denominator != 1))
                        throw new Exception("Invalid Boson!");
                }
                else
                {
                    // If the particle is not specified as a Fermion/Boson
                    // make sure it has either integer or half-integer spin
                    if (value != Fraction.Zero ||
                        value.Denominator != 1 ||
                        value.Denominator != 2)
                        throw new Exception("Invalid Spin!");
                }
                spin = value;
            }
        }
        public Particle Antiparticle
        {
            get { return antiParticle; }
        }

        public override Entity Clone() { return this.MemberwiseClone() as Particle; }
        public override bool Equals(object obj)
        {
            if(!(obj is Particle) || !base.Equals(obj)) return false;

            Particle p = obj as Particle;
            return(symbol.Equals(p.symbol) && 
                    observed.Equals(p.observed) && 
                    mass.Equals(p.mass) &&
                    spin.Equals(p.spin)) ? true : false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode()^
                    symbol.GetHashCode()^
                    observed.GetHashCode()^
                    mass.GetHashCode()^
                    spin.GetHashCode();
        }
        public override string ToString() { return this.GetType().Name;}
    } 
    #endregion

    #region Antiparticle
    public abstract class Antiparticle<T> : Particle 
        where T : Particle
    {
        public Antiparticle(T particle)
        {
            this.mass = particle.Mass;
            this.spin = particle.Spin;
            this.observed = particle.Observed;
            this.antiParticle = particle;
        }
    }
    #endregion
}
