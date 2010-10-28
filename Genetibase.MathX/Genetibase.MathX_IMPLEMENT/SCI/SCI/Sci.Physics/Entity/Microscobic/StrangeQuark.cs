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
    #region StrangeQuark
    public sealed class StrangeQuark : Quark
    {
        public StrangeQuark(Color color)
            : base(color, 0, 0, 0, -1, new Fraction(0), new Fraction(-1, 2))
        {
            this.symbol = "s";
            this.antiParticle = new AntiStrangeQuark(this);
        }
    }
    #endregion

    #region AntiStrangeQuark
    public sealed class AntiStrangeQuark : AntiQuark<StrangeQuark>
    {
        public AntiStrangeQuark(StrangeQuark particle) : base(particle)
        {
            this.symbol = "s";
        }
    }
    #endregion
}


