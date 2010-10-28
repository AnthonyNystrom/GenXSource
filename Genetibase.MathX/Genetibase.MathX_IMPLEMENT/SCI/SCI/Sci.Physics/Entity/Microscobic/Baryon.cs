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

namespace Sci.Physics.Entity.Microscobic
{
    #region Baryon
    public class Baryon : Hadron, IBaryon
    {
        protected Baryon() : base()
        {
            this.antiParticle = new AntiBaryon(this);            
        }
        public Baryon(Quark q1, Quark q2, Quark q3) : base(q1, q2, q3)
        {
            this.antiParticle = new AntiBaryon(this);
        }

        #region IBaryon
        public Fraction BaryonNumber
        {
            get 
            {
                Fraction baryonNumber = new Fraction(0);
                foreach (Quark q in constituents)
                {
                    baryonNumber += q.BaryonNumber;
                }
                return baryonNumber; 
            }
        }
        #endregion
    }
    #endregion

    #region AntiBaryon
    public class AntiBaryon : AntiHadron<Baryon>, IBaryon
    {
        public AntiBaryon(Baryon particle) : base(particle){ }

        #region IBaryon
        public Fraction BaryonNumber
        {
            get
            {
                return (((Baryon)antiParticle).BaryonNumber == Fraction.Zero) ?
                    Fraction.Zero :
                    -((Baryon)antiParticle).BaryonNumber;
            }
        }
        #endregion
    }
    #endregion
}
