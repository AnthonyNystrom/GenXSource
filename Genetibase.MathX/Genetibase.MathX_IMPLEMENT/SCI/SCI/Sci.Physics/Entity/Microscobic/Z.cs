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
    #region Z
    /// <summary>
    /// INCOMPLETE
    /// </summary>
    public sealed class Z : GaugeBoson
    {
        public Z()
            : base()
        {
            this.symbol = "Z\u2070";
            this.spin = new Fraction(1);
            this.antiParticle = this;
        }        
    }
    #endregion
}
