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
    /// <summary> A sequence of ChemModels, which can, for example, be used to
    /// store the course of a reaction. Each state of the reaction would be
    /// stored in one ChemModel.
    /// 
    /// </summary>
    /// <cdk.module>   interfaces </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  animation </cdk.keyword>
    /// <cdk.keyword>  reaction </cdk.keyword>
    public interface IChemSequence : IChemObject
    {
        /// <summary> Returns an array of ChemModels of length matching the number of ChemModels 
        /// in this container.
        /// 
        /// </summary>
        /// <returns>    The array of ChemModels in this container
        /// </returns>
        /// <seealso cref="addChemModel">
        /// </seealso>
        IChemModel[] ChemModels
        {
            get;
        }

        /// <summary> Returns the number of ChemModels in this Container.
        /// 
        /// </summary>
        /// <returns>    The number of ChemModels in this Container
        /// </returns>
        int ChemModelCount
        {
            get;
        }

        /// <summary> Adds an chemModel to this container.
        /// 
        /// </summary>
        /// <param name="chemModel">The chemModel to be added to this container
        /// </param>
        /// <seealso cref="getChemModel">
        /// </seealso>
        void addChemModel(IChemModel chemModel);

        /// <summary> Returns the ChemModel at position <code>number</code> in the
        /// container.
        /// 
        /// </summary>
        /// <param name="number"> The position of the ChemModel to be returned.
        /// </param>
        /// <returns>         The ChemModel at position <code>number</code>.
        /// </returns>
        /// <seealso cref="addChemModel">
        /// </seealso>
        IChemModel getChemModel(int number);
    }
}