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
    #region Lambda
    /// <summary>
    /// Lambda0 : uds, (S=-1, I=0), spin=1/2
    /// </summary>
    public sealed class Lambda : Hyperon
    {
        public Lambda() : base()
        {
            this.symbol = "\u039B\u2070";
            this.spin = new Fraction(1, 2);
            base.Add(new UpQuark(new Red()));
            base.Add(new DownQuark(new Green()));
            base.Add(new StrangeQuark(new Blue()));
            this.antiParticle = new AntiLambda(this);
        }
        public Lambda(UpQuark u, DownQuark d, StrangeQuark s) : base(u, d, s)
        {
            this.symbol = "\u039B\u2070";
            this.spin = new Fraction(1, 2);
            this.antiParticle = new AntiLambda(this);
        }
    }
    #endregion

    #region AntiLambda
    public sealed class AntiLambda : AntiHyperon
    {
        public AntiLambda(Lambda particle) : base(particle)
        {
        }
    }
    #endregion
}
