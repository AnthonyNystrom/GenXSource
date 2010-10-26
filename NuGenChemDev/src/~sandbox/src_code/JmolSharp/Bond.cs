/* $RCSfile: Bond.java,v $
* $Author: migueljmol $
* $Date: 2005/03/14 21:45:32 $
* $Revision: 1.5 $
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
*  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA
*  02111-1307  USA.
*/
using System;

namespace Org.Jmol.Adapter.Smarter
{
    class Bond
    {
        internal int atomIndex1;
        internal int atomIndex2;
        internal int order;

        internal Bond()
        {
            atomIndex1 = atomIndex2 = -1;
            order = 1;
        }

        internal Bond(int atomIndex1, int atomIndex2, int order)
        {
            this.atomIndex1 = atomIndex1;
            this.atomIndex2 = atomIndex2;
            this.order = order;
        }
    }
}