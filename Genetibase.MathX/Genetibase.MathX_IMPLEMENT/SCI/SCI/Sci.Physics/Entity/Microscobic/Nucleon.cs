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
    #region Nucleon
    /// <summary>
    /// N : uud/udd, (S=0, I=1/2), spin=1/2
    /// </summary>
    public class Nucleon : Baryon
    {
        protected Nucleon() : base()
        {
            this.spin = new Fraction(1, 2);
            this.antiParticle = new AntiNucleon(this);
        }
        public Nucleon(UpQuark u1, UpQuark u2, DownQuark d) : base(u1, u2, d)
        {
            this.spin = new Fraction(1, 2);
            this.antiParticle = new AntiNucleon(this);
        }
        public Nucleon(UpQuark u, DownQuark d1, DownQuark d2) : base(u, d1, d2)
        {
            this.spin = new Fraction(1, 2);
            this.antiParticle = new AntiNucleon(this);
        }
    }
    #endregion

    #region AntiNucleon
    public class AntiNucleon : AntiBaryon
    {
        public AntiNucleon(Nucleon particle) : base(particle) { }
    }
    #endregion
}
