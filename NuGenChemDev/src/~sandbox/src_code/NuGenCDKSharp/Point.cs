/* $RCSfile$
* $Author: egonw $
* $Date: 2006-05-12 11:05:38 +0200 (Fri, 12 May 2006) $
*
* Copyright (C) 2003-2006  The Jmol Development Team
* Copyright (C) 2003-2006  The CDK Project
*
* Contact: cdk-devel@lists.sf.net
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
*  Foundation, 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA. 
*/
using System;
namespace Org.OpenScience.CDK.Graph.Rebond
{
    /// <author>       Miguel Howard
    /// </author>
    /// <cdk.created>  2003-05 </cdk.created>
    /// <cdk.module>   standard </cdk.module>
    public class Point : Bspt.Tuple
    {
        internal double x;
        internal double y;
        internal double z;

        internal Point(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public virtual double getDimValue(int dim)
        {
            if (dim == 0)
                return x;
            if (dim == 1)
                return y;
            return z;
        }

        public override System.String ToString()
        {
            return "<" + x + "," + y + "," + z + ">";
        }
    }
}