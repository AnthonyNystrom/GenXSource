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
    #region Gluon 
    /// <summary>
    /// INCOMPLETE
    /// </summary>
    public class Gluon : GaugeBoson
    {
        public Gluon() : base()
        {
            this.symbol = "\u0067";
            this.spin = new Fraction(1);
            this.antiParticle = this;
        }
    }
    #endregion
}
