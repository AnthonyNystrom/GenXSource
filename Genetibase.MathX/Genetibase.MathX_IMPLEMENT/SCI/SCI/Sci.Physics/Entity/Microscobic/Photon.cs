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
    #region Photon
    /// <summary>
    /// INCOMPLETE
    /// </summary>
    public sealed class Photon : GaugeBoson
    {
        public Photon() : base()
        {
            this.symbol = "\u03B3";
            this.spin = new Fraction(1);
            this.antiParticle = this;
        }
    }
    #endregion
}
