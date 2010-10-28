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
    #region MuonNeutrino
    public sealed class MuonNeutrino : Neutrino
    {
        public MuonNeutrino() : base()
        {
            this.symbol += "\u03BC";
            this.antiParticle = new AntiMuonNeutrino(this);
        }
        #endregion
    }

    #region AntiMuonNeutrino
    public sealed class AntiMuonNeutrino : AntiNeutrino<MuonNeutrino>
    {
        public AntiMuonNeutrino(MuonNeutrino particle) : base(particle)
        {
            this.symbol += "\u03BC";
        }
    }
    #endregion
}
