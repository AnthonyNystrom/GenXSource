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
    #region CharmedXi
    /// <summary>
    /// Xi+c    : usc
    /// xi0c    : dsc
    /// </summary>
    public sealed class CharmedXi : Hyperon
    {
        public CharmedXi(UpQuark u, StrangeQuark s, CharmQuark c) : base(u, c, s)
        {
            this.symbol = "\u039E\u207Ac";
            this.spin = new Fraction(1, 2);
            this.antiParticle = new AntiCharmedXi(this);
        }
        public CharmedXi(DownQuark d, StrangeQuark s, CharmQuark c) : base(d, c, s)
        {
            this.symbol = "\u039E\u2070c";
            this.spin = new Fraction(1, 2);
            this.antiParticle = new AntiCharmedXi(this);
        }
    }
    #endregion

    #region AntiCharmedXi
    public sealed class AntiCharmedXi : AntiHyperon
    {
        public AntiCharmedXi(CharmedXi particle) : base(particle)
        {
        }
    }
    #endregion
}
