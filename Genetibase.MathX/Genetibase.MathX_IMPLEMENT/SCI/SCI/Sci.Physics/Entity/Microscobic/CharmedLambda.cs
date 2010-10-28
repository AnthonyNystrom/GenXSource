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
    #region CHARMED LAMBDA
    /// <summary>
    /// Lambda+c : udc, S=1/2
    /// </summary>
    public sealed class CharmedLambda : Baryon
    {
        public CharmedLambda() : base()
        {
            this.symbol = "\u039B\u207Ac";
            this.spin = new Fraction(1, 2);
            base.Add(new UpQuark(new Red()));
            base.Add(new DownQuark(new Green()));
            base.Add(new CharmQuark(new Blue()));
            this.antiParticle = new AntiCharmedLambda(this);
        }
        public CharmedLambda(UpQuark u, DownQuark d, CharmQuark c) : base(u, d, c)
        {
            this.symbol = "\u039B\u207Ac";
            this.spin = new Fraction(1, 2);
            this.antiParticle = new AntiCharmedLambda(this);
        }
    }
    #endregion

    #region ANTICHARMEDLAMBDA
    public sealed class AntiCharmedLambda : AntiBaryon
    {
        #region Constructors
        public AntiCharmedLambda(CharmedLambda particle)
            : base(particle)
        {
        }
        #endregion
    }
    #endregion
}
