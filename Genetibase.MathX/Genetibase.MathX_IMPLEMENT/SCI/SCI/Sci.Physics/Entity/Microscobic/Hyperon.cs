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
    #region Hyperon
    /// <summary>
    /// S != 0, B=0, C=0
    /// </summary>
    public abstract class Hyperon : Baryon
    {
        protected Hyperon() : base()
        {
        }
        public Hyperon(Quark q1, Quark q2, StrangeQuark s)  : base(q1, q2, s)
        {
        }
    }
    #endregion

    #region AntiHyperon
    public abstract class AntiHyperon : AntiBaryon
    {
        public AntiHyperon(Hyperon particle) : base(particle)
        {
        }
    }
    #endregion
}
