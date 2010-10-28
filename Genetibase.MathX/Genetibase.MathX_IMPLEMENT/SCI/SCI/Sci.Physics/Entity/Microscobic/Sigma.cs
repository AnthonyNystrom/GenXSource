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
    #region Sigma
    /// <summary>
    /// S=-1, I=1
    /// Sigma+  : uus
    /// Sigma0  : uds
    /// Sigma-  : dds
    /// spin=1/2
    /// </summary>
    public sealed class Sigma : Hyperon
    {
        public Sigma(UpQuark u1, UpQuark u2, StrangeQuark s) : base(u1, u2, s)
        {
            this.symbol = "\u03A3\u207A";
            this.spin = new Fraction(1, 2);
            this.antiParticle = new AntiSigma(this);
        }
        public Sigma(UpQuark u, DownQuark d, StrangeQuark s) : base(u, d, s)
        {
            this.symbol = "\u03A3\u2070";
            this.spin = new Fraction(1, 2);
            this.antiParticle = new AntiSigma(this);
        }
        public Sigma(DownQuark d1, DownQuark d2, StrangeQuark s) : base(d1, d2, s)
        {
            this.symbol = "\u03A3\u207B";
            this.spin = new Fraction(1, 2);
            this.antiParticle = new AntiSigma(this);
        }
    }
    #endregion

    #region AntiSigma
    public sealed class AntiSigma : AntiHyperon
    {
        public AntiSigma(Sigma particle) : base(particle)
        {
        }
    }
    #endregion
}
