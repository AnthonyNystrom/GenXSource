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
    /// <summary> An object containig multiple SetOfMolecules and 
    /// the other lower level concepts like rings, sequences, 
    /// fragments, etc.
    /// 
    /// </summary>
    /// <cdk.module>  interfaces </cdk.module>
    public interface IChemModel : IChemObject
    {
        /// <summary> Returns the SetOfMolecules of this ChemModel.
        /// 
        /// </summary>
        /// <returns>   The SetOfMolecules of this ChemModel
        /// </returns>
        /// <seealso cref="setSetOfMolecules">
        /// </seealso>
        /// <summary> Sets the SetOfMolecules of this ChemModel.
        /// 
        /// </summary>
        /// <param name="setOfMolecules"> the content of this model
        /// </param>
        /// <seealso cref="getSetOfMolecules">
        /// </seealso>
        ISetOfMolecules SetOfMolecules
        {
            get;
            set;
        }

        /// <summary> Returns the RingSet of this ChemModel.
        /// 
        /// </summary>
        /// <returns> the ringset of this model
        /// </returns>
        /// <seealso cref="setRingSet">
        /// </seealso>
        /// <summary> Sets the RingSet of this ChemModel.
        /// 
        /// </summary>
        /// <param name="ringSet">        the content of this model
        /// </param>
        /// <seealso cref="getRingSet">
        /// </seealso>
        IRingSet RingSet
        {
            get;
            set;
        }
        
        /// <summary> Gets the Crystal contained in this ChemModel.
        /// 
        /// </summary>
        /// <returns> The crystal in this model
        /// </returns>
        /// <seealso cref="setCrystal">
        /// </seealso>
        /// <summary> Sets the Crystal contained in this ChemModel.
        /// 
        /// </summary>
        /// <param name="crystal"> the Crystal to store in this model
        /// </param>
        /// <seealso cref="getCrystal">
        /// </seealso>
        ICrystal Crystal
        {
            get;
            set;
        }
        
        /// <summary> Gets the SetOfReactions contained in this ChemModel.
        /// 
        /// </summary>
        /// <returns> The SetOfReactions in this model
        /// </returns>
        /// <seealso cref="setSetOfReactions">
        /// </seealso>
        /// <summary> Sets the SetOfReactions contained in this ChemModel.
        /// 
        /// </summary>
        /// <param name="sor">the SetOfReactions to store in this model
        /// </param>
        /// <seealso cref="getSetOfReactions">
        /// </seealso>
        ISetOfReactions SetOfReactions
        {
            get;
            set;
        }
    }
}