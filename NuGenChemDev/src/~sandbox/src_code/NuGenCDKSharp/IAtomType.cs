/* $RCSfile$
* $Author: egonw $
* $Date: 2006-05-02 09:46:48 +0200 (Tue, 02 May 2006) $
* $Revision: 6119 $
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

    /// <summary> The base class for atom types. Atom types are typically used to describe the
    /// behaviour of an atom of a particular element in different environment like 
    /// sp<sup>3</sup> hybridized carbon C3, etc., in some molecular modelling 
    /// applications.
    /// 
    /// </summary>
    /// <cdk.module>  interfaces </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       egonw
    /// </author>
    /// <cdk.created>  2005-08-24 </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  atom, type </cdk.keyword>
    public interface IAtomType : IIsotope
    {
        /// <summary> Gets the id attribute of the AtomType object.
        /// 
        /// </summary>
        /// <returns>    The id value
        /// </returns>
        /// <seealso cref="setAtomTypeName">
        /// </seealso>
        /// <summary> Sets the if attribute of the AtomType object.
        /// 
        /// </summary>
        /// <param name="identifier"> The new AtomTypeID value. Null if unset.
        /// </param>
        /// <seealso cref="getAtomTypeName">
        /// </seealso>
        String AtomTypeName
        {
            get;
            set;
        }
        
        /// <summary> Gets the MaxBondOrder attribute of the AtomType object.
        /// 
        /// </summary>
        /// <returns>    The MaxBondOrder value
        /// </returns>
        /// <seealso cref="setMaxBondOrder">
        /// </seealso>
        /// <summary> Sets the MaxBondOrder attribute of the AtomType object.
        /// 
        /// </summary>
        /// <param name="maxBondOrder"> The new MaxBondOrder value
        /// </param>
        /// <seealso cref="getMaxBondOrder">
        /// </seealso>
        double MaxBondOrder
        {
            get;
            set;
        }
        
        /// <summary> Gets the bondOrderSum attribute of the AtomType object.
        /// 
        /// </summary>
        /// <returns>    The bondOrderSum value
        /// </returns>
        /// <seealso cref="setBondOrderSum">
        /// </seealso>
        /// <summary> Sets the the exact bond order sum attribute of the AtomType object.
        /// 
        /// </summary>
        /// <param name="bondOrderSum"> The new bondOrderSum value
        /// </param>
        /// <seealso cref="getBondOrderSum">
        /// </seealso>
        double BondOrderSum
        {
            get;
            set;
        }
        
        /// <summary> Returns the formal neighbour count of this atom.
        /// 
        /// </summary>
        /// <returns> the formal neighbour count of this atom
        /// </returns>
        /// <seealso cref="setFormalNeighbourCount">
        /// </seealso>
        /// <summary> Sets the formal neighbour count of this atom.
        /// 
        /// </summary>
        /// <param name="count"> The neighbour count
        /// </param>
        /// <seealso cref="getFormalNeighbourCount">
        /// </seealso>
        int FormalNeighbourCount
        {
            get;
            set;
        }
        
        /// <summary> Returns the hybridization of this atom.
        /// 
        /// </summary>
        /// <returns> the hybridization of this atom
        /// </returns>
        /// <seealso cref="setHybridization">
        /// </seealso>
        /// <summary> Sets the hybridization of this atom.
        /// 
        /// </summary>
        /// <param name="hybridization"> The hybridization
        /// </param>
        /// <seealso cref="getHybridization">
        /// </seealso>
        int Hybridization
        {
            get;
            set;
        }
        
        /// <summary> Returns the Vanderwaals radius for this AtomType.
        /// 
        /// </summary>
        /// <returns> The Vanderwaals radius for this AtomType
        /// </returns>
        /// <seealso cref="setVanderwaalsRadius">
        /// </seealso>
        /// <summary> Sets the Vanderwaals radius for this AtomType.
        /// 
        /// </summary>
        /// <param name="radius">The Vanderwaals radius for this AtomType
        /// </param>
        /// <seealso cref="getVanderwaalsRadius">
        /// </seealso>
        double VanderwaalsRadius
        {
            get;
            set;
        }
        
        /// <summary> Returns the covalent radius for this AtomType.
        /// 
        /// </summary>
        /// <returns> The covalent radius for this AtomType
        /// </returns>
        /// <seealso cref="setCovalentRadius">
        /// </seealso>
        /// <summary> Sets the covalent radius for this AtomType.
        /// 
        /// </summary>
        /// <param name="radius">The covalent radius for this AtomType
        /// </param>
        /// <seealso cref="getCovalentRadius">
        /// </seealso>
        double CovalentRadius
        {
            get;
            set;
        }
        
        /// <summary> Gets the the exact electron valency of the AtomType object.
        /// 
        /// </summary>
        /// <returns> The valency value
        /// </returns>
        /// <seealso cref="setValency(int)">
        /// </seealso>
        /// <summary> Sets the the exact electron valency of the AtomType object.
        /// 
        /// </summary>
        /// <param name="valency"> The new valency value
        /// </param>
        /// <seealso cref="getValency()">
        /// </seealso>
        int Valency
        {
            get;
            set;
        }

        /// <summary> Sets the formal charge of this atom.
        /// 
        /// </summary>
        /// <param name="charge"> The formal charge
        /// </param>
        /// <seealso cref="getFormalCharge">
        /// </seealso>
        void setFormalCharge(int charge);

        /// <summary> Returns the formal charge of this atom.
        /// 
        /// </summary>
        /// <returns> the formal charge of this atom
        /// </returns>
        /// <seealso cref="setFormalCharge">
        /// </seealso>
        int getFormalCharge();
    }
}