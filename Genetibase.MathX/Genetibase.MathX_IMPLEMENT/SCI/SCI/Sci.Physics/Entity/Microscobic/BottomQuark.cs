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
    #region BottomQuark
    public sealed class BottomQuark : Quark
    {
        public BottomQuark(Color color)
            : base(color, -1, 0, 0, 0, new Fraction(0), new Fraction(-1, 2))
        {
            this.symbol = "b";
            this.antiParticle = new AntiBottomQuark(this);
        }
    }
    #endregion

    #region AntiBottomQuark
    public sealed class AntiBottomQuark : AntiQuark<BottomQuark>
    {
        public AntiBottomQuark(BottomQuark particle) : base(particle)
        {
            this.symbol = "b";
        }
    }
    #endregion
}

