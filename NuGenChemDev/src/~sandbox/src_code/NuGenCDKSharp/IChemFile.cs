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
using Org.OpenScience.CDK.IO.Formats;

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary> A Object containing a number of ChemSequences. This is supposed to be the
    /// top level container, which can contain all the concepts stored in a chemical
    /// document
    /// 
    /// </summary>
    /// <author>      egonw
    /// </author>
    /// <cdk.module>  interfaces </cdk.module>
    public interface IChemFile : IChemObject
    {
        /// <summary> Returns the array of ChemSequences of this container.
        /// 
        /// </summary>
        /// <returns>    The array of ChemSequences of this container
        /// </returns>
        /// <seealso cref="addChemSequence">
        /// </seealso>
        IChemSequence[] ChemSequences
        {
            get;

        }

        /// <summary> Returns the number of ChemSequences in this Container.
        /// 
        /// </summary>
        /// <returns>    The number of ChemSequences in this Container
        /// </returns>
        int ChemSequenceCount
        {
            get;

        }

        /// <summary> Adds an ChemSequence to this container.
        /// 
        /// </summary>
        /// <param name="chemSequence"> The chemSequence to be added to this container
        /// </param>
        /// <seealso cref="getChemSequences">
        /// </seealso>
        void addChemSequence(IChemSequence chemSequence);

        /// <summary> Returns the ChemSequence at position <code>number</code> in the container.
        /// 
        /// </summary>
        /// <param name="number"> The position of the ChemSequence to be returned.
        /// </param>
        /// <returns>         The ChemSequence at position <code>number</code>.
        /// </returns>
        /// <seealso cref="addChemSequence">
        /// </seealso>
        IChemSequence getChemSequence(int number);
    }
}