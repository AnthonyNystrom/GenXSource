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
    #region Xi
    /// <summary>
    /// S=-2, I=1/2
    /// Xi0 : uss
    /// xi- : dss
    /// spin=1/2
    /// </summary>
    public sealed class Xi : Hyperon
    {
        public Xi(UpQuark u, StrangeQuark s1, StrangeQuark s2) : base(u, s1, s2)
        {
            this.symbol = "\u039E\u2070";
            this.spin = new Fraction(1, 2);
            this.antiParticle = new AntiXi(this);
        }
        public Xi(DownQuark d, StrangeQuark s1, StrangeQuark s2) : base(d, s1, s2)
        {
            this.symbol = "\u039E\u207B";
            this.spin = new Fraction(1, 2);
            this.antiParticle = new AntiXi(this);
        }
    }
    #endregion

    #region AntiXi
    public sealed class AntiXi : AntiHyperon
    {
        public AntiXi(Xi particle) : base(particle)
        {
        }
    }
    #endregion
}
