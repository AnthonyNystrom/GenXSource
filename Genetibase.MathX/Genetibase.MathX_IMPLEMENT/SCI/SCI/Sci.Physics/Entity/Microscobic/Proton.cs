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
    #region Proton
    /// <summary>
    /// p : uud, (S=0, I=1/2), spin=1/2
    /// </summary>
    public sealed class Proton : Nucleon
    {
        public Proton() : base()
        {
            this.symbol = "p";
            base.Add(new UpQuark(new Red()));
            base.Add(new UpQuark(new Green()));
            base.Add(new DownQuark(new Blue()));
            this.antiParticle = new AntiProton(this);
        }
        public Proton(UpQuark u1, UpQuark u2, DownQuark d) : base(u1, u2, d)
        {
            this.symbol = "p";
            this.antiParticle = new AntiProton(this);
        }
    }
    #endregion

    #region AntiProton
    public sealed class AntiProton: AntiNucleon
    {
        public AntiProton(Proton particle) : base(particle) { }
    }
    #endregion
}
