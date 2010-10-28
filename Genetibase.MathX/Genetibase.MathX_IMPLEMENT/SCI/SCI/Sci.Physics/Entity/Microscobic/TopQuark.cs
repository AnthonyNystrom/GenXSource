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
    #region TopQuark
    public sealed class TopQuark : Quark
    {
        public TopQuark(Color color)
            : base(color, 0, 1, 0, 0, new Fraction(0), new Fraction(1, 2))
        {
            this.symbol = "t";
            this.antiParticle = new AntiTopQuark(this);
        }
    } 
    #endregion

    #region AntiTopQuark
    public sealed class AntiTopQuark : AntiQuark<TopQuark>
    {
        public AntiTopQuark(TopQuark particle) : base(particle)
        {
            this.symbol = "t";
        }
    }  
    #endregion
}
