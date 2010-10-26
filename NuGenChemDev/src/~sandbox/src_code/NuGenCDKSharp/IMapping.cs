/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-02-15 17:26:42 +0100 (Wed, 15 Feb 2006) $
* $Revision: 5717 $
*
* Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
*
* Contact: cdk-devel@lists.sourceforge.net
*
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public License
* as published by the Free Software Foundation; either version 2.1
* of the License, or (at your option) any later version.
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

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary> Represents a set of Molecules.
    /// 
    /// </summary>
    /// <cdk.module>   interfaces </cdk.module>
    /// <author>       kaihartmann
    /// </author>
    /// <cdk.created>  2006-02-15 </cdk.created>
    public interface IMapping : IChemObject
    {
        /// <summary> Returns an array of the two IChemObject's.
        /// 
        /// </summary>
        /// <returns> An array of two IChemObject's that define the mapping
        /// </returns>
        IChemObject[] RelatedChemObjects
        {
            get;
        }
    }
}