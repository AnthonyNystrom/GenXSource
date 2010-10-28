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
    #region BottomLambda
    /// <summary>
    /// Lambda0b : udb
    /// </summary>
    public sealed class BottomLambda : Baryon
    {
        public BottomLambda()
            : base()
        {
            this.symbol = "\u039B\u2070b";
            this.spin = new Fraction(1, 2);
            base.Add(new UpQuark(new Red()));
            base.Add(new DownQuark(new Green()));
            base.Add(new BottomQuark(new Blue()));
            this.antiParticle = new AntiBottomLambda(this);
        }
        public BottomLambda(UpQuark u, DownQuark d, BottomQuark b)
            : base(u, d, b)
        {
            this.symbol = "\u039B\u2070b";
            this.spin = new Fraction(1, 2);
            this.antiParticle = new AntiBottomLambda(this);
        }
    }
    #endregion

    #region AntiBottomLambda
    public sealed class AntiBottomLambda : AntiBaryon
    {
        public AntiBottomLambda(BottomLambda particle) : base(particle)
        {
        }
    }
    #endregion
}
