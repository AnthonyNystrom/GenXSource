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
    /// <summary> Implements the concept of a covalent bond between two or more atoms. A bond is
    /// considered to be a number of electrons connecting two ore more atoms.
    /// 
    /// </summary>
    /// <cdk.module>  interfaces </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       egonw
    /// </author>
    /// <cdk.created>  2005-08-24 </cdk.created>
    /// <cdk.keyword>  bond </cdk.keyword>
    /// <cdk.keyword>  atom </cdk.keyword>
    /// <cdk.keyword>  electron </cdk.keyword>
    public interface IBond : IElectronContainer
    {
        /// <summary> Returns the number of Atoms in this Bond.
        /// 
        /// </summary>
        /// <returns>    The number of Atoms in this Bond
        /// </returns>
        int AtomCount
        {
            get;
        }
        
        /// <summary> Returns the bond order of this bond.
        /// 
        /// </summary>
        /// <returns> The bond order of this bond
        /// </returns>
        /// <seealso cref="org.openscience.cdk.CDKConstants org.openscience.cdk.CDKConstants">
        /// for predefined values.
        /// </seealso>
        /// <seealso cref="setOrder">
        /// </seealso>
        /// <summary> Sets the bond order of this bond.
        /// 
        /// </summary>
        /// <param name="order">The bond order to be assigned to this bond
        /// </param>
        /// <seealso cref="org.openscience.cdk.CDKConstants for predefined values.">
        /// </seealso>
        /// <seealso cref="getOrder">
        /// </seealso>
        double Order
        {
            get;
            set;
        }
        
        /// <summary> Returns the stereo descriptor for this bond.
        /// 
        /// </summary>
        /// <returns>    The stereo descriptor for this bond
        /// </returns>
        /// <seealso cref="setStereo">
        /// </seealso>
        /// <seealso cref="org.openscience.cdk.CDKConstants for predefined values.">
        /// </seealso>
        /// <summary> Sets the stereo descriptor for this bond.
        /// 
        /// </summary>
        /// <param name="stereo"> The stereo descriptor to be assigned to this bond.
        /// </param>
        /// <seealso cref="getStereo">
        /// </seealso>
        /// <seealso cref="org.openscience.cdk.CDKConstants for predefined values.">
        /// </seealso>
        int Stereo
        {
            get;
            set;
        }

        /// <summary> Returns the array of atoms making up this bond.
        /// 
        /// </summary>
        /// <returns>    An array of atoms participating in this bond
        /// </returns>
        /// <seealso cref="setAtoms">
        /// </seealso>
        IAtom[] getAtoms();

        /// <summary> Sets the array of atoms making up this bond.
        /// 
        /// </summary>
        /// <param name="atoms"> An array of atoms that forms this bond
        /// </param>
        /// <seealso cref="getAtoms">
        /// </seealso>
        void setAtoms(IAtom[] atoms);

        /// <summary> Returns an Atom from this bond.
        /// 
        /// </summary>
        /// <param name="position"> The position in this bond where the atom is
        /// </param>
        /// <returns>           The atom at the specified position
        /// </returns>
        /// <seealso cref="setAtomAt">
        /// </seealso>
        IAtom getAtomAt(int position);


        /// <summary> Returns the atom connected to the given atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom the bond partner is searched of
        /// </param>
        /// <returns>       the connected atom or null
        /// </returns>
        IAtom getConnectedAtom(IAtom atom);

        /// <summary> Returns true if the given atom participates in this bond.
        /// 
        /// </summary>
        /// <param name="atom"> The atom to be tested if it participates in this bond
        /// </param>
        /// <returns>       true if the atom participates in this bond
        /// </returns>
        bool contains(IAtom atom);

        /// <summary> Sets an Atom in this bond.
        /// 
        /// </summary>
        /// <param name="atom">     The atom to be set
        /// </param>
        /// <param name="position"> The position in this bond where the atom is to be inserted
        /// </param>
        /// <seealso cref="getAtomAt">
        /// </seealso>
        void setAtomAt(IAtom atom, int position);

        /// <summary> Returns the geometric 2D center of the bond.
        /// 
        /// </summary>
        /// <returns>    The geometric 2D center of the bond
        /// </returns>
        Point2d get2DCenter();

        /// <summary> Returns the geometric 3D center of the bond.
        /// 
        /// </summary>
        /// <returns>    The geometric 3D center of the bond
        /// </returns>
        Point3d get3DCenter();

        /// <summary> Compares a bond with this bond.
        /// 
        /// </summary>
        /// <param name="object"> Object of type Bond
        /// </param>
        /// <returns>         Return true, if the bond is equal to this bond
        /// </returns>
        bool compare(Object object_Renamed);

        /// <summary> Checks wether a bond is connected to another one.
        /// This can only be true if the bonds have an Atom in common.
        /// 
        /// </summary>
        /// <param name="bond"> The bond which is checked to be connect with this one
        /// </param>
        /// <returns>       True, if the bonds share an atom, otherwise false
        /// </returns>
        bool isConnectedTo(IBond bond);
    }
}