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
    #region Neutron
    /// <summary>
    /// n : udd, (S=0, I=1/2), spin=1/2
    /// </summary>
    public sealed class Neutron: Nucleon
    {
        public Neutron() : base()
        {
            this.symbol = "n";
            base.Add(new UpQuark(new Red()));
            base.Add(new DownQuark(new Green()));
            base.Add(new DownQuark(new Blue()));
            this.antiParticle = new AntiNeutron(this);
        }
        public Neutron(UpQuark u, DownQuark d1, DownQuark d2) : base(u, d1, d2)
        {
            this.symbol = "n";
            this.antiParticle = new AntiNeutron(this);
        }
    }
    #endregion

    #region AntiNeutron
    public sealed class AntiNeutron : AntiNucleon
    {
        public AntiNeutron(Neutron particle) : base(particle) { }
    }
    #endregion
}
