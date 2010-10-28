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
    #region CharmQuark
    public sealed class CharmQuark : Quark
    {
        public CharmQuark(Color color)
            : base(color, 0, 0, 1, 0, new Fraction(0), new Fraction(1, 2))
        {
            this.symbol = "c";
            this.antiParticle = new AntiCharmQuark(this);
        }
    }
    #endregion

    #region AntiCharmQuark
    public sealed class AntiCharmQuark : AntiQuark<CharmQuark>
    {
        public AntiCharmQuark(CharmQuark particle) : base(particle)
        {
            this.symbol = "c";
        }
    }
    #endregion
}

