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
    #region CharmedOmega
    /// <summary>
    /// Omega0c : ssc, spin=3/2
    /// </summary>
    public sealed class CharmedOmega : Hyperon
    {
        public CharmedOmega() : base()
        {
            this.symbol = "\u03A9\u2070c";
            this.spin = new Fraction(3, 2);
            base.Add(new StrangeQuark(new Green()));
            base.Add(new StrangeQuark(new Blue()));
            base.Add(new CharmQuark(new Red()));
            this.antiParticle = new AntiCharmedOmega(this);
        }
        public CharmedOmega(StrangeQuark s1, StrangeQuark s2, CharmQuark c) : base(c, s1, s2)
        {
            this.symbol = "\u03A9\u2070c";
            this.spin = new Fraction(3, 2);
            this.antiParticle = new AntiCharmedOmega(this);
        }
    }
    #endregion

    #region AntiCharmedOmega
    public sealed class AntiCharmedOmega : AntiHyperon
    {
        public AntiCharmedOmega(CharmedOmega particle) : base(particle)
        {
        }
    }
    #endregion
}
