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
*  */
using System;

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary> A Monomer is an AtomContainer which stores additional monomer specific 
    /// informations for a group of Atoms.
    /// 
    /// </summary>
    /// <cdk.module>   interfaces </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       Edgar Luttmann <edgar@uni-paderborn.de>
    /// </author>
    /// <cdk.created>  2001-08-06  </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  monomer </cdk.keyword>
    public interface IMonomer : IAtomContainer
    {
        /// <summary> Retrieve the monomer name.
        /// 
        /// </summary>
        /// <returns> The name of the Monomer object
        /// </returns>
        /// <seealso cref="setMonomerName">
        /// </seealso>
        /// <summary> Set the name of the Monomer object.
        /// 
        /// </summary>
        /// <param name="cMonomerName"> The new name for this monomer
        /// </param>
        /// <seealso cref="getMonomerName">
        /// </seealso>
        String MonomerName
        {
            get;
            set;
        }
        
        /// <summary> Retrieve the monomer type.
        /// 
        /// </summary>
        /// <returns> The type of the Monomer object
        /// </returns>
        /// <seealso cref="setMonomerType">
        /// </seealso>
        /// <summary> Set the type of the Monomer object.
        /// 
        /// </summary>
        /// <param name="cMonomerType"> The new type for this monomer
        /// </param>
        /// <seealso cref="getMonomerType">
        /// </seealso>
        String MonomerType
        {
            get;
            set;
        }
    }
}