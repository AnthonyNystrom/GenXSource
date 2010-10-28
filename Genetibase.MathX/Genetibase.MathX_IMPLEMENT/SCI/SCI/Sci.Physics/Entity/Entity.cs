/* *****************************************************************************
 * AUTHOR       : Coskun OBA
 * EMAIL        : oba.coskun@hotmail.com
 * 
 * DATE         : JANUARY 2007
 * *****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Sci.Physics.Entity
{
    #region Entity
    public abstract class Entity
    {
        public Entity() { }

        public abstract Entity Clone();
    }
    #endregion
}
