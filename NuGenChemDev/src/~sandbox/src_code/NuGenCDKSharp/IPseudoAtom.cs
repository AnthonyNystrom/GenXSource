/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
*
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
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
*
*/
using System;

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary> Represents the idea of a non-chemical atom-like entity, like Me,
    /// R, X, Phe, His, etc.
    /// 
    /// <p>This should be replaced by the mechanism explained in RFC #8.
    /// 
    /// </summary>
    /// <cdk.module>  interfaces </cdk.module>
    /// <summary> 
    /// </summary>
    /// <seealso cref="IAtom">
    /// </seealso>
    public interface IPseudoAtom : IAtom
    {
        /// <summary> Returns the label of this PseudoAtom.
        /// 
        /// </summary>
        /// <returns> The label for this PseudoAtom
        /// </returns>
        /// <seealso cref="setLabel">
        /// </seealso>
        /// <summary> Sets the label of this PseudoAtom.
        /// 
        /// </summary>
        /// <param name="label">The new label for this PseudoAtom
        /// </param>
        /// <seealso cref="getLabel">
        /// </seealso>
        String Label
        {
            get;
            set;
        }
    }
}