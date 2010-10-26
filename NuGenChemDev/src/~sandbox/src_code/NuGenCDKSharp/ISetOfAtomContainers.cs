/*
*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
*  $Revision: 5855 $
*
*  Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
*
*  Contact: cdk-devel@lists.sourceforge.net
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public License
*  as published by the Free Software Foundation; either version 2.1
*  of the License, or (at your option) any later version.
*
*  This program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public License
*  along with this program; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary> A set of AtomContainers.
    /// 
    /// </summary>
    /// <author>      egonw
    /// </author>
    /// <cdk.module>  interfaces </cdk.module>
    public interface ISetOfAtomContainers : IChemObject
    {
        /// <summary> Returns the array of AtomContainers of this container.
        /// 
        /// </summary>
        /// <returns>    The array of AtomContainers of this container
        /// </returns>
        IAtomContainer[] AtomContainers
        {
            get;
        }

        /// <summary> Returns the number of AtomContainers in this Container.
        /// 
        /// </summary>
        /// <returns>    The number of AtomContainers in this Container
        /// </returns>
        int AtomContainerCount
        {
            get;
        }

        /// <summary> Adds an atomContainer to this container.
        /// 
        /// </summary>
        /// <param name="atomContainer"> The atomContainer to be added to this container
        /// </param>
        void addAtomContainer(IAtomContainer atomContainer);

        /// <summary> Removes an AtomContainer from this container.
        /// 
        /// </summary>
        /// <param name="atomContainer"> The atomContainer to be removed from this container
        /// </param>
        void removeAtomContainer(IAtomContainer atomContainer);

        /// <summary> Removes all AtomContainer from this container.</summary>
        void removeAllAtomContainers();

        /// <summary> Removes an AtomContainer from this container.
        /// 
        /// </summary>
        /// <param name="pos"> The position of the AtomContainer to be removed from this container
        /// </param>
        void removeAtomContainer(int pos);

        /// <summary> Sets the coefficient of a AtomContainer to a given value.
        /// 
        /// </summary>
        /// <param name="container">  The AtomContainer for which the multiplier is set
        /// </param>
        /// <param name="multiplier"> The new multiplier for the AtomContatiner
        /// </param>
        /// <returns>             true if multiplier has been set
        /// </returns>
        /// <seealso cref="getMultiplier(IAtomContainer)">
        /// </seealso>
        bool setMultiplier(IAtomContainer container, double multiplier);

        /// <summary> Sets the coefficient of a AtomContainer to a given value.
        /// 
        /// </summary>
        /// <param name="position">   The position of the AtomContainer for which the multiplier is
        /// set in [0,..]
        /// </param>
        /// <param name="multiplier"> The new multiplier for the AtomContatiner at
        /// <code>position</code>
        /// </param>
        /// <seealso cref="getMultiplier(int)">
        /// </seealso>
        void setMultiplier(int position, double multiplier);

        /// <summary> Returns an array of double with the stoichiometric coefficients
        /// of the products.
        /// 
        /// </summary>
        /// <returns>    The multipliers for the AtomContainer's in this set
        /// </returns>
        /// <seealso cref="setMultipliers">
        /// </seealso>
        double[] getMultipliers();

        /// <summary> Sets the multipliers of the AtomContainers.
        /// 
        /// </summary>
        /// <param name="newMultipliers"> The new multipliers for the AtomContainers in this set
        /// </param>
        /// <returns>                 true if multipliers have been set.
        /// </returns>
        /// <seealso cref="getMultipliers">
        /// </seealso>
        bool setMultipliers(double[] newMultipliers);

        /// <summary> Adds an atomContainer to this container with the given
        /// multiplier.
        /// 
        /// </summary>
        /// <param name="atomContainer"> The atomContainer to be added to this container
        /// </param>
        /// <param name="multiplier">    The multiplier of this atomContainer
        /// </param>
        void addAtomContainer(IAtomContainer atomContainer, double multiplier);

        /// <summary> Adds all atomContainers in the SetOfAtomContainers to this container.
        /// 
        /// </summary>
        /// <param name="atomContainerSet"> The SetOfAtomContainers
        /// </param>
        void add(ISetOfAtomContainers atomContainerSet);

        /// <summary> Returns the AtomContainer at position <code>number</code> in the
        /// container.
        /// 
        /// </summary>
        /// <param name="number"> The position of the AtomContainer to be returned.
        /// </param>
        /// <returns>         The AtomContainer at position <code>number</code> .
        /// </returns>
        IAtomContainer getAtomContainer(int number);

        /// <summary> Returns the multiplier for the AtomContainer at position <code>number</code> in the
        /// container.
        /// 
        /// </summary>
        /// <param name="number"> The position of the multiplier of the AtomContainer to be returned.
        /// </param>
        /// <returns>         The multiplier for the AtomContainer at position <code>number</code> .
        /// </returns>
        /// <seealso cref="setMultiplier(int, double)">
        /// </seealso>
        double getMultiplier(int number);

        /// <summary> Returns the multiplier of the given AtomContainer.
        /// 
        /// </summary>
        /// <param name="container"> The AtomContainer for which the multiplier is given
        /// </param>
        /// <returns>            -1, if the given molecule is not a container in this set
        /// </returns>
        /// <seealso cref="setMultiplier(IAtomContainer, double)">
        /// </seealso>
        double getMultiplier(IAtomContainer container);
    }
}