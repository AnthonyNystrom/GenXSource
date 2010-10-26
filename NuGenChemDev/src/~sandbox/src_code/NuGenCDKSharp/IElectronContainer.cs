/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
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
    /// <summary> Base class for entities containing electrons, like bonds, orbitals, lone-pairs.
    /// 
    /// </summary>
    /// <cdk.module>  interfaces </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  electron </cdk.keyword>
    public interface IElectronContainer : IChemObject
    {
        /// <summary> Returns the number of electrons in this electron container.
        /// 
        /// </summary>
        /// <returns> The number of electrons in this electron container.
        /// </returns>
        /// <seealso cref="setElectronCount">
        /// </seealso>
        int getElectronCount();

        /// <summary> Sets the number of electorn in this electron container.
        /// 
        /// </summary>
        /// <param name="electronCount">The number of electrons in this electron container.
        /// </param>
        /// <seealso cref="getElectronCount">
        /// </seealso>
        void setElectronCount(int electronCount);
    }
}