/* $RCSfile$
 * $Author: egonw $
 * $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
 *
 * Copyright (C) 2003-2005  Miguel, Jmol Development, www.jmol.org
 *
 * Contact: miguel@jmol.org
 *
 *  This library is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  This library is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 */
namespace Org.Jmol.Bspt
{
    /// <summary>
    /// the internal tree is made up of elements ... either Node or Leaf
    /// </summary>
    /// <author>Miguel, miguel@jmol.org</author>
    abstract class Element
    {
        protected Bspt bspt;
        public int count;

        public abstract Element addTuple(int level, Tuple tuple);
        /*
          abstract void dump(int level);
        */
    }
}
