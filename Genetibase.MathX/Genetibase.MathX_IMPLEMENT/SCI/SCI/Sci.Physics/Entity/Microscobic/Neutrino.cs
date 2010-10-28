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
    #region Neutrino
    public abstract class Neutrino : Lepton
    {
        public Neutrino() 
            : base(new Fraction(1, 2), new Fraction(-1))
        {
            this.symbol = "\u03BD";
        }
    } 
    #endregion

    #region AntiNeutrino
    public abstract class AntiNeutrino<T> : AntiLepton<T> 
        where T : Neutrino
    {
        public AntiNeutrino(T particle) : base(particle)
        {
            this.symbol = "\u1FE1";
        }
    }
    #endregion
}
