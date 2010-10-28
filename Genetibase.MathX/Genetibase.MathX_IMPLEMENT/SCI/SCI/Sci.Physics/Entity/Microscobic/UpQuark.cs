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
    #region UpQuark
    public sealed class UpQuark : Quark
    {
        public UpQuark(Color color)
            : base(color, 0, 0, 0, 0, new Fraction(1, 2), new Fraction(1, 2))
        {
            this.symbol = "u";
            this.antiParticle = new AntiUpQuark(this);
        }
    }
    #endregion

    #region AntiUpQuark
    public sealed class AntiUpQuark : AntiQuark<UpQuark>
    {
        public AntiUpQuark(UpQuark particle) : base(particle)
        {
            this.symbol = "\u016B";
        }
    }
    #endregion
}


