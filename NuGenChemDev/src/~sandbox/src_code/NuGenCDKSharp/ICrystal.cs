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
using javax.vecmath;

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary> Class representing a molecular crystal.
    /// The crystal is described with molecules in fractional
    /// coordinates and three cell axes: a,b and c.
    /// 
    /// <p>The crystal is designed to store only the asymetric atoms.
    /// Though this is not enforced, it is assumed by all methods.
    /// 
    /// </summary>
    /// <cdk.module>  interfaces </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  crystal </cdk.keyword>
    public interface ICrystal : IAtomContainer
    {
        /// <summary> Gets the A unit cell axes in carthesian coordinates
        /// as a three element double array.
        /// 
        /// </summary>
        /// <returns> a Vector3D representing the A axis
        /// </returns>
        /// <seealso cref="setA">
        /// </seealso>
        /// <summary> Sets the A unit cell axes in carthesian coordinates in a 
        /// eucledian space.
        /// 
        /// </summary>
        /// <param name="newAxis">the new A axis
        /// </param>
        /// <seealso cref="getA">
        /// </seealso>
        Vector3d A
        {
            get;
            set;
        }

        /// <summary> Gets the B unit cell axes in carthesian coordinates
        /// as a three element double array.
        /// 
        /// </summary>
        /// <returns> a Vector3D representing the B axis
        /// </returns>
        /// <seealso cref="setB">
        /// </seealso>
        /// <summary> Sets the B unit cell axes in carthesian coordinates.
        /// 
        /// </summary>
        /// <param name="newAxis">the new B axis
        /// </param>
        /// <seealso cref="getB">
        /// </seealso>
        Vector3d B
        {
            get;
            set;
        }
        
        /// <summary> Gets the C unit cell axes in carthesian coordinates
        /// as a three element double array.
        /// 
        /// </summary>
        /// <returns> a Vector3D representing the C axis
        /// </returns>
        /// <seealso cref="setC">
        /// </seealso>
        /// <summary> Sets the C unit cell axes in carthesian coordinates.
        /// 
        /// </summary>
        /// <param name="newAxis">the new C axis
        /// </param>
        /// <seealso cref="getC">
        /// </seealso>
        Vector3d C
        {
            get;
            set;
        }

        /// <summary> Gets the space group of this crystal.
        /// 
        /// </summary>
        /// <returns> the space group of this crystal structure
        /// </returns>
        /// <seealso cref="setSpaceGroup">
        /// </seealso>
        /// <summary> Sets the space group of this crystal.
        /// 
        /// </summary>
        /// <param name="group"> the space group of this crystal structure
        /// </param>
        /// <seealso cref="getSpaceGroup">
        /// </seealso>
        String SpaceGroup
        {
            get;
            set;
        }
        
        /// <summary> Gets the number of asymmetric parts in the unit cell.
        /// 
        /// </summary>
        /// <returns> the number of assymetric parts in the unit cell
        /// </returns>
        /// <seealso cref="setZ">
        /// </seealso>
        /// <summary> Sets the number of assymmetric parts in the unit cell.
        /// 
        /// </summary>
        /// <param name="value">the number of assymetric parts in the unit cell
        /// </param>
        /// <seealso cref="getZ">
        /// </seealso>
        int Z
        {
            get;
            set;
        }

        /// <summary> Adds the atoms in the AtomContainer as cell content. Symmetry related 
        /// atoms should not be added unless P1 space group is used.
        /// </summary>
        new void add(IAtomContainer container);

        /// <summary> Adds the atom to the crystal. Symmetry related atoms should
        /// not be added unless P1 space group is used.
        /// </summary>
        new void addAtom(IAtom atom);
    }
}