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
    #region BottomSigma
    /// <summary>
    /// Sigma+b  : uub
    /// Sigma-b  : ddb
    /// </summary>
    public sealed class BottomSigma : Baryon
    {
        public BottomSigma(UpQuark u1, UpQuark u2, BottomQuark b) : base(u1, u2, b)
        {
            this.symbol = "\u03A3\u207Ab";
            this.spin = new Fraction(1, 2);
            this.antiParticle = new AntiBottomSigma(this);
        }
        public BottomSigma(DownQuark d1, DownQuark d2, BottomQuark b) : base(d1, d2, b)
        {
            this.symbol = "\u03A3\u2070b";
            this.spin = new Fraction(1, 2);
            this.antiParticle = new AntiBottomSigma(this);
        }
    }
    #endregion

    #region AntiBottomSigma
    public sealed class AntiBottomSigma : AntiBaryon
    {
        public AntiBottomSigma(BottomSigma particle) : base(particle)
        {
        }
    }
    #endregion
}
