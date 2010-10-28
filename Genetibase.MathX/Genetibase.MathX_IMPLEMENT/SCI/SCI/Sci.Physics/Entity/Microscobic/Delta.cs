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
    #region Delta
    /// <summary>
    /// S=0, I=3/2, spin=3/2
    /// Delta++ : uuu
    /// Delta+  : uud
    /// Delta0  : udd
    /// Delta-  : ddd
    /// </summary>
    public sealed class Delta : Baryon
    {       
        /// <summary>
        /// DELTA PLUS PLUS
        /// </summary>
        public Delta(UpQuark u1, UpQuark u2, UpQuark u3) : base(u1, u2, u3)
        {
            this.symbol = "\u0394\u207A\u207A";
            this.spin = new Fraction(3, 2);
            this.antiParticle = new AntiDelta(this);
        }
        /// <summary>
        ///  DELTA PLUS
        /// </summary>
        public Delta(UpQuark u1, UpQuark u2, DownQuark d) : base(u1, u2, d)
        {
            this.symbol = "\u0394\u207A";
            this.spin = new Fraction(3, 2);
            this.antiParticle = new AntiDelta(this);
        }
        /// <summary>
        ///  DELTA ZERO
        /// </summary>
        public Delta(UpQuark u, DownQuark d1, DownQuark d2) : base(u, d1, d2)
        {
            this.symbol = "\u0394\u2070";
            this.spin = new Fraction(3, 2);
            this.antiParticle = new AntiDelta(this);
        }
        /// <summary>
        ///  DELTA MINUS
        /// </summary>
        public Delta(DownQuark d1, DownQuark d2, DownQuark d3) : base(d1, d2, d3)
        {
            this.symbol = "\u0394\u207B";
            this.spin = new Fraction(3, 2);
            this.antiParticle = new AntiDelta(this);
        }
        
        public static Delta PLUSPLUS
        {
            get
            {
                return new Delta(new UpQuark(new Red()),
                                    new UpQuark(new Green()),
                                    new UpQuark(new Blue()));
            }
        }
        public static Delta PLUS
        {
            get
            {
                return new Delta(new UpQuark(new Red()),
                                    new UpQuark(new Green()),
                                    new DownQuark(new Blue()));
            }
        }
        public static Delta ZERO
        {
            get
            {
                return new Delta(new UpQuark(new Red()),
                                    new DownQuark(new Green()),
                                    new DownQuark(new Blue()));
            }
        }
        public static Delta MINUS
        {
            get
            {
                return new Delta(new DownQuark(new Red()),
                                    new DownQuark(new Green()),
                                    new DownQuark(new Blue()));
            }
        }

    }
    #endregion

    #region AntiDelta
    public sealed class AntiDelta : AntiBaryon
    {
        public AntiDelta(Delta particle) : base(particle)
        {
        }
    }
    #endregion
}
