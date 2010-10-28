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

    #region GaugeBoson
    public abstract class GaugeBoson : Particle, IGaugeBoson
    {
        public GaugeBoson() : base() { }
    }
    #endregion

    #region AntiGaugeBoson
    public abstract class AntiGaugeBoson<T> : Antiparticle<T>, IGaugeBoson
        where T : GaugeBoson
    {
        public AntiGaugeBoson(T particle) : base(particle) { }
    }
    #endregion
}
