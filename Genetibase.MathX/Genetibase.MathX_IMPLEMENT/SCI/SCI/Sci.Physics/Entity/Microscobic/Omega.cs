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
    #region Omega
    /// <summary>
    /// Omega-  : sss, (S=-3, I=0), S=3/2
    /// </summary>
    public sealed class Omega : Hyperon
    {
        public Omega() : base()
        {
            this.symbol = "\u03A9\u207B";
            this.spin = new Fraction(3, 2);
            base.Add(new StrangeQuark(new Red()));
            base.Add(new StrangeQuark(new Green()));
            base.Add(new StrangeQuark(new Blue()));
            this.antiParticle = new AntiOmega(this);
        }
        public Omega(StrangeQuark s1, StrangeQuark s2, StrangeQuark s3) : base(s1, s2, s3)
        {
            this.symbol = "\u03A9\u207B";
            this.spin = new Fraction(3, 2);
            this.antiParticle = new AntiOmega(this);
        }
    }
    #endregion

    #region AntiOmega
    public sealed class AntiOmega : AntiHyperon
    {
        public AntiOmega(Omega particle) : base(particle)
        {
        }
    }
    #endregion
}
