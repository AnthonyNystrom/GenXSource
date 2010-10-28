/* *****************************************************************************
 * AUTHOR       : Coskun OBA
 * EMAIL        : oba.coskun@hotmail.com
 * 
 * DATE         : JANUARY 2007
 * *****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Sci.Physics.Entity.Microscobic
{
    #region TauonNeutrino
    public sealed class TauonNeutrino : Neutrino
    {
        #region Constructors
        public TauonNeutrino() : base()
        {
            this.symbol += "\u03C4";
            this.antiParticle = new AntiTauonNeutrino(this);
        }
        #endregion
    }
    #endregion

    #region AntiTauonNeutrino
    public sealed class AntiTauonNeutrino : AntiNeutrino<TauonNeutrino>
    {
        public AntiTauonNeutrino(TauonNeutrino particle) : base(particle)
        {
            this.symbol += "\u03C4";
        }
    }
    #endregion
}
