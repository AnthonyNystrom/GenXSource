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
    #region DownQuark
    public sealed class DownQuark : Quark
    {
        public DownQuark(Color color)
            : base(color, 0, 0, 0, 0, new Fraction(-1, 2), new Fraction(-1, 2))
        {
            this.symbol = "d";
            this.antiParticle = new AntiDownQuark(this);
        }
    }
    #endregion

    #region AntiDownQuark
    public sealed class AntiDownQuark : AntiQuark<DownQuark>
    {
        public AntiDownQuark(DownQuark particle)
            : base(particle)
        {
            this.symbol = "d";
        }
    }
    #endregion
}

