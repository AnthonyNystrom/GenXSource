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
    #region ElectronNeutrino
    public sealed class ElectronNeutrino : Neutrino
    {
        public ElectronNeutrino() : base()
        {
            this.symbol += "e";
            this.antiParticle = new AntiElectronNeutrino(this);
        }
    } 
    #endregion

    #region AntiElectronNeutrino
    public sealed class AntiElectronNeutrino : AntiNeutrino<ElectronNeutrino>
    {
        public AntiElectronNeutrino(ElectronNeutrino particle) : base(particle)
        {
            this.symbol += "e";
        }
    }
    #endregion
}
