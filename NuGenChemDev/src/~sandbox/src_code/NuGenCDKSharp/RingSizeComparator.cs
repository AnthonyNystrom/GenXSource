/* $RCSfile$
* $Author: egonw $ 
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
* 
* Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
* 
* Contact: cdk-devel@lists.sourceforge.net
* 
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public License
* as published by the Free Software Foundation; either version 2.1
* of the License, or (at your option) any later version.
* All we ask is that proper credit is given for our work, which includes
* - but is not limited to - adding the above copyright notice to the beginning
* of your source code files, and to any copyright notice that you may distribute
* with programs based on this work.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK.Tools.Manipulator
{
    /// <cdk.module>  standard </cdk.module>
    public class RingSizeComparator : System.Collections.IComparer
    {

        /// <summary>Flag to denote that the set is order with the largest ring first? </summary>
        public const int LARGE_FIRST = 1;
        /// <summary>Flag to denote that the set is order with the smallest ring first? </summary>
        public const int SMALL_FIRST = 2;

        internal int sortOrder = SMALL_FIRST;

        /// <summary> Constructs a new comparator to sort rings by size.
        /// 
        /// </summary>
        /// <param name="order"> Sort order: either RingSet.SMALL_FIRST or
        /// RingSet.LARGE_FIRST.
        /// </param>
        public RingSizeComparator(int order)
        {
            sortOrder = order;
        }

        public virtual int Compare(System.Object object1, System.Object object2)
        {
            int size1 = ((IRing)object1).AtomCount;
            int size2 = ((IRing)object2).AtomCount;
            if (size1 == size2)
                return 0;
            if (size2 > size1 && sortOrder == SMALL_FIRST)
            {
                return 1;
            }
            if (size2 > size1 && sortOrder == LARGE_FIRST)
            {
                return -1;
            }
            if (size2 < size1 && sortOrder == SMALL_FIRST)
            {
                return -1;
            }
            if (size2 < size1 && sortOrder == LARGE_FIRST)
            {
                return 1;
            }
            return 0;
        }
    }
}