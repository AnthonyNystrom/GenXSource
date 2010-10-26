/* $RCSfile$
* $Author: egonw $
* $Date: 2006-05-11 22:05:31 +0200 (Thu, 11 May 2006) $
* $Revision: 6236 $
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
using System.Collections;

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary>  Base class for all chemical objects that maintain a list of Atoms and
    /// ElectronContainers. <p>
    /// 
    /// Looping over all Bonds in the AtomContainer is typically done like: <pre>
    /// Bond[] bonds = atomContainer.getBonds();
    /// for (int i = 0; i < bonds.length; i++) {
    /// Bond b = bonds[i];
    /// }
    /// </pre>
    /// 
    /// </summary>
    /// <cdk.module>  interfaces </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>      steinbeck
    /// </author>
    /// <cdk.created>     2000-10-02 </cdk.created>
    public interface IAtomContainer : IChemObject, IChemObjectListener
    {
        /// <summary>  Returns the array of atoms of this AtomContainer.
        /// 
        /// </summary>
        /// <returns>    The array of atoms of this AtomContainer
        /// </returns>
        /// <seealso cref="setAtoms">
        /// </seealso>
        /// <summary> Sets the array of atoms of this AtomContainer.
        /// 
        /// </summary>
        /// <param name="atoms"> The array of atoms to be assigned to this AtomContainer
        /// </param>
        /// <seealso cref="getAtoms">
        /// </seealso>
        IAtom[] Atoms
        {
            get;
            set;
        }
        
        /// <summary>  Returns the array of electronContainers of this AtomContainer.
        /// 
        /// </summary>
        /// <returns>    The array of electronContainers of this AtomContainer
        /// </returns>
        /// <seealso cref="setElectronContainers">
        /// </seealso>
        /// <summary> Sets the array of electronContainers of this AtomContainer.
        /// 
        /// </summary>
        /// <param name="electronContainers"> The array of electronContainers to be assigned to
        /// this AtomContainer
        /// </param>
        /// <seealso cref="getElectronContainers">
        /// </seealso>
        IElectronContainer[] ElectronContainers
        {
            get;
            set;
        }
        
        /// <summary> Returns the number of ElectronContainers in this Container.
        /// 
        /// </summary>
        /// <returns>    The number of ElectronContainers in this Container
        /// </returns>
        /// <seealso cref="setElectronContainerCount">
        /// </seealso>
        /// <summary> Sets the number of electronContainers in this container.
        /// 
        /// </summary>
        /// <param name="electronContainerCount"> The number of electronContainers in this
        /// container
        /// </param>
        /// <seealso cref="getElectronContainerCount">
        /// </seealso>
        int ElectronContainerCount
        {
            get;
            set;
        }
        
        /// <summary>  Returns the number of Atoms in this Container.
        /// 
        /// </summary>
        /// <returns>    The number of Atoms in this Container
        /// </returns>
        /// <seealso cref="setAtomCount">
        /// </seealso>
        /// <summary>  Sets the number of atoms in this container.
        /// 
        /// </summary>
        /// <param name="atomCount"> The number of atoms in this container
        /// </param>
        /// <seealso cref="getAtomCount">
        /// </seealso>
        int AtomCount
        {
            get;
            set;
        }
        
        /// <summary>  Returns the array of Bonds of this AtomContainer.
        /// 
        /// </summary>
        /// <returns>    The array of Bonds of this AtomContainer
        /// </returns>
        /// <seealso cref="getElectronContainers">
        /// </seealso>
        IBond[] Bonds
        {
            get;
        }
        
        /// <summary>  Returns the atom at position 0 in the container.
        /// 
        /// </summary>
        /// <returns>    The atom at position 0 .
        /// </returns>
        IAtom FirstAtom
        {
            get;
        }
        
        /// <summary>  Returns the atom at the last position in the container.
        /// 
        /// </summary>
        /// <returns>    The atom at the last position
        /// </returns>
        IAtom LastAtom
        {
            get;
        }

        /// <summary> Adds an AtomParity to this container. If a parity is already given for the
        /// affected Atom, it is overwritten.
        /// 
        /// </summary>
        /// <param name="parity">The new AtomParity for this container
        /// </param>
        /// <seealso cref="getAtomParity">
        /// </seealso>
        void addAtomParity(IAtomParity parity);

        /// <summary> Returns the atom parity for the given Atom. If no parity is associated
        /// with the given Atom, it returns null.
        /// 
        /// </summary>
        /// <param name="atom">  Atom for which the parity must be returned
        /// </param>
        /// <returns> The AtomParity for the given Atom, or null if that Atom does
        /// not have an associated AtomParity
        /// </returns>
        /// <seealso cref="addAtomParity">
        /// </seealso>
        IAtomParity getAtomParity(IAtom atom);

        /// <summary> Set the atom at position <code>number</code> in [0,..].
        /// 
        /// </summary>
        /// <param name="number"> The position of the atom to be set.
        /// </param>
        /// <param name="atom">   The atom to be stored at position <code>number</code>
        /// </param>
        /// <seealso cref="getAtomAt">
        /// </seealso>
        void setAtomAt(int number, IAtom atom);

        /// <summary> Get the atom at position <code>number</code> in [0,..].
        /// 
        /// </summary>
        /// <param name="number"> The position of the atom to be retrieved.
        /// </param>
        /// <returns>         The atomAt value
        /// </returns>
        /// <seealso cref="setAtomAt">
        /// </seealso>
        IAtom getAtomAt(int number);

        /// <summary>  Get the bond at position <code>number</code> in [0,..].
        /// 
        /// </summary>
        /// <param name="number"> The position of the bond to be retrieved.
        /// </param>
        /// <returns>         The bondAt value
        /// </returns>
        /// <seealso cref="setElectronContainerAt">
        /// </seealso>
        IBond getBondAt(int number);

        /// <summary> Sets the ElectronContainer at position <code>number</code> in [0,..].
        /// 
        /// </summary>
        /// <param name="number">           The position of the ElectronContainer to be set.
        /// </param>
        /// <param name="electronContainer">The ElectronContainer to be stored at position <code>number</code>
        /// </param>
        /// <seealso cref="getElectronContainerAt">
        /// </seealso>
        void setElectronContainerAt(int number, IElectronContainer electronContainer);


        /// <summary>  Returns an AtomEnumeration for looping over all atoms in this container.
        /// 
        /// </summary>
        /// <returns>    An AtomEnumeration with the atoms in this container
        /// </returns>
        /// <seealso cref="getAtoms">
        /// </seealso>
        IEnumerator atoms();


        /// <summary>  Returns the array of Bonds of this AtomContainer.
        /// 
        /// </summary>
        /// <returns>    The array of Bonds of this AtomContainer
        /// </returns>
        /// <seealso cref="getElectronContainers">
        /// </seealso>
        /// <seealso cref="getBonds">
        /// </seealso>
        ILonePair[] getLonePairs();


        /// <summary>  Returns the array of Bonds of this AtomContainer.
        /// 
        /// </summary>
        /// <param name="atom"> Description of the Parameter
        /// </param>
        /// <returns>       The array of Bonds of this AtomContainer
        /// </returns>
        /// <seealso cref="getElectronContainers">
        /// </seealso>
        /// <seealso cref="getBonds">
        /// </seealso>
        ILonePair[] getLonePairs(IAtom atom);


        /// <summary>  Returns the position of a given atom in the atoms array. It returns -1 if
        /// the atom atom does not exist.
        /// 
        /// </summary>
        /// <param name="atom"> The atom to be sought
        /// </param>
        /// <returns>       The Position of the atom in the atoms array in [0,..].
        /// </returns>
        int getAtomNumber(IAtom atom);


        /// <summary>  Returns the position of the bond between two given atoms in the
        /// electronContainers array. It returns -1 if the bond does not exist.
        /// 
        /// </summary>
        /// <param name="atom1"> The first atom
        /// </param>
        /// <param name="atom2"> The second atom
        /// </param>
        /// <returns>        The Position of the bond between a1 and a2 in the
        /// electronContainers array.
        /// </returns>
        int getBondNumber(IAtom atom1, IAtom atom2);


        /// <summary>  Returns the position of a given bond in the electronContainers array. It
        /// returns -1 if the bond does not exist.
        /// 
        /// </summary>
        /// <param name="bond"> The bond to be sought
        /// </param>
        /// <returns>       The Position of the bond in the electronContainers array in [0,..].
        /// </returns>
        int getBondNumber(IBond bond);


        /// <summary>  Returns the ElectronContainer at position <code>number</code> in the
        /// container.
        /// 
        /// </summary>
        /// <param name="number"> The position of the ElectronContainer to be returned.
        /// </param>
        /// <returns>         The ElectronContainer at position <code>number</code>.
        /// </returns>
        /// <seealso cref="setElectronContainerAt">
        /// </seealso>
        IElectronContainer getElectronContainerAt(int number);


        /// <summary> Returns the bond that connectes the two given atoms.
        /// 
        /// </summary>
        /// <param name="atom1"> The first atom
        /// </param>
        /// <param name="atom2"> The second atom
        /// </param>
        /// <returns>        The bond that connectes the two atoms
        /// </returns>
        IBond getBond(IAtom atom1, IAtom atom2);


        /// <summary>  Returns an array of all atoms connected to the given atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom the bond partners are searched of.
        /// </param>
        /// <returns>       The array of <code>Atom</code>s with the size of connected
        /// atoms
        /// </returns>
        IAtom[] getConnectedAtoms(IAtom atom);


        /// <summary>  Returns a vector of all atoms connected to the given atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom the bond partners are searched of.
        /// </param>
        /// <returns>       The vector with the size of connected atoms
        /// </returns>
        System.Collections.IList getConnectedAtomsVector(IAtom atom);


        /// <summary>  Returns an array of all Bonds connected to the given atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom the connected bonds are searched of
        /// </param>
        /// <returns>       The array with the size of connected atoms
        /// </returns>
        IBond[] getConnectedBonds(IAtom atom);

        /// <summary>  Returns a Vector of all Bonds connected to the given atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom the connected bonds are searched of
        /// </param>
        /// <returns>       The vector with the size of connected atoms
        /// </returns>
        IList getConnectedBondsVector(IAtom atom);


        /// <summary>  Returns an array of all electronContainers connected to the given atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom the connected electronContainers are searched of
        /// </param>
        /// <returns>       The array with the size of connected atoms
        /// </returns>
        IElectronContainer[] getConnectedElectronContainers(IAtom atom);


        /// <summary>  Returns the number of connected atoms (degree) to the given atom.
        /// 
        /// </summary>
        /// <param name="atomnumber"> The atomnumber the degree is searched for
        /// </param>
        /// <returns>             The number of connected atoms (degree)
        /// </returns>
        int getBondCount(int atomnumber);


        /// <summary>  Returns the number of LonePairs in this Container.
        /// 
        /// </summary>
        /// <returns>    The number of LonePairs in this Container
        /// </returns>
        int getLonePairCount();


        /// <summary>  Returns the number of Bonds in this Container.
        /// 
        /// </summary>
        /// <returns>    The number of Bonds in this Container
        /// </returns>
        int getBondCount();


        /// <summary>  Returns the number of Bonds for a given Atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom
        /// </param>
        /// <returns>       The number of Bonds for this atom
        /// </returns>
        int getBondCount(IAtom atom);


        /// <summary>  Returns the number of LonePairs for a given Atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom
        /// </param>
        /// <returns>       The number of LonePairs for this atom
        /// </returns>
        int getLonePairCount(IAtom atom);

        /// <summary>  Returns an array of all SingleElectron connected to the given atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom on which the single electron is located
        /// </param>
        /// <returns>       The array of SingleElectron of this AtomContainer
        /// </returns>
        ISingleElectron[] getSingleElectron(IAtom atom);
        /// <summary>  Returns the sum of the SingleElectron for a given Atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom on which the single electron is located
        /// </param>
        /// <returns>       The array of SingleElectron of this AtomContainer
        /// </returns>
        int getSingleElectronSum(IAtom atom);
        /// <summary> Returns the sum of the bond orders for a given Atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom
        /// </param>
        /// <returns>       The number of bondorders for this atom
        /// </returns>
        double getBondOrderSum(IAtom atom);

        /// <summary> Returns the maximum bond order that this atom currently has in the context
        /// of this AtomContainer.
        /// 
        /// </summary>
        /// <param name="atom"> The atom
        /// </param>
        /// <returns>       The maximum bond order that this atom currently has
        /// </returns>
        double getMaximumBondOrder(IAtom atom);


        /// <summary>  Returns the minimum bond order that this atom currently has in the context
        /// of this AtomContainer.
        /// 
        /// </summary>
        /// <param name="atom"> The atom
        /// </param>
        /// <returns>       The minimim bond order that this atom currently has
        /// </returns>
        double getMinimumBondOrder(IAtom atom);



        /// <summary> Compares this AtomContainer with another given AtomContainer and returns
        /// the Intersection between them. <p>
        /// 
        /// <b>Important Note</b> : This is not the maximum common substructure.
        /// 
        /// </summary>
        /// <param name="container"> an AtomContainer object
        /// </param>
        /// <returns>            An AtomContainer containing the Intersection between this
        /// AtomContainer and another given one
        /// </returns>

        IAtomContainer getIntersection(IAtomContainer container);

        /// <summary>  Adds the <code>ElectronContainer</code>s found in atomContainer to this
        /// container.
        /// 
        /// </summary>
        /// <param name="atomContainer"> AtomContainer with the new ElectronContainers
        /// </param>
        void addElectronContainers(IAtomContainer atomContainer);


        /// <summary>  Adds all atoms and electronContainers of a given atomcontainer to this
        /// container.
        /// 
        /// </summary>
        /// <param name="atomContainer"> The atomcontainer to be added
        /// </param>
        void add(IAtomContainer atomContainer);


        /// <summary>  Adds an atom to this container.
        /// 
        /// </summary>
        /// <param name="atom"> The atom to be added to this container
        /// </param>
        void addAtom(IAtom atom);


        /// <summary>  Wrapper method for adding Bonds to this AtomContainer.
        /// 
        /// </summary>
        /// <param name="bond"> The bond to added to this container
        /// </param>
        void addBond(IBond bond);


        /// <summary>  Adds a ElectronContainer to this AtomContainer.
        /// 
        /// </summary>
        /// <param name="electronContainer"> The ElectronContainer to added to this container
        /// </param>
        void addElectronContainer(IElectronContainer electronContainer);


        /// <summary>  Removes all atoms and electronContainers of a given atomcontainer from this
        /// container.
        /// 
        /// </summary>
        /// <param name="atomContainer"> The atomcontainer to be removed
        /// </param>
        void remove(IAtomContainer atomContainer);


        /// <summary> Removes the bond at the given position from this container.
        /// 
        /// </summary>
        /// <param name="position"> The position of the bond in the electronContainers array
        /// </param>
        /// <returns>           Bond that was removed
        /// </returns>
        IElectronContainer removeElectronContainer(int position);


        /// <summary> Removes this ElectronContainer from this container.
        /// 
        /// </summary>
        /// <param name="electronContainer">   The electronContainer to be removed
        /// </param>
        /// <returns>                      Bond that was removed
        /// </returns>
        IElectronContainer removeElectronContainer(IElectronContainer electronContainer);


        /// <summary> Removes the bond that connects the two given atoms.
        /// 
        /// </summary>
        /// <param name="atom1"> The first atom
        /// </param>
        /// <param name="atom2"> The second atom
        /// </param>
        /// <returns>        The bond that connectes the two atoms
        /// </returns>
        IBond removeBond(IAtom atom1, IAtom atom2);



        /// <summary>  Removes the atom at the given position from the AtomContainer. Note that
        /// the electronContainers are unaffected: you also have to take care of
        /// removing all electronContainers to this atom from the container manually.
        /// 
        /// </summary>
        /// <param name="position"> The position of the atom to be removed.
        /// </param>
        void removeAtom(int position);


        /// <summary>  Removes the given atom and all connected electronContainers from the
        /// AtomContainer.
        /// 
        /// </summary>
        /// <param name="atom"> The atom to be removed
        /// </param>
        void removeAtomAndConnectedElectronContainers(IAtom atom);


        /// <summary>  Removes the given atom from the AtomContainer. Note that the
        /// electronContainers are unaffected: you also have to take care of removeing
        /// all electronContainers to this atom from the container.
        /// 
        /// </summary>
        /// <param name="atom"> The atom to be removed
        /// </param>
        void removeAtom(IAtom atom);


        /// <summary> Removes all atoms and bond from this container.</summary>
        void removeAllElements();


        /// <summary>  Removes electronContainers from this container.</summary>
        void removeAllElectronContainers();

        /// <summary>  Removes all Bonds from this container.</summary>
        void removeAllBonds();

        /// <summary>  Adds a bond to this container.
        /// 
        /// </summary>
        /// <param name="atom1">  Id of the first atom of the Bond in [0,..]
        /// </param>
        /// <param name="atom2">  Id of the second atom of the Bond in [0,..]
        /// </param>
        /// <param name="order">  Bondorder
        /// </param>
        /// <param name="stereo"> Stereochemical orientation
        /// </param>
        void addBond(int atom1, int atom2, double order, int stereo);


        /// <summary>  Adds a bond to this container.
        /// 
        /// </summary>
        /// <param name="atom1"> Id of the first atom of the Bond in [0,..]
        /// </param>
        /// <param name="atom2"> Id of the second atom of the Bond in [0,..]
        /// </param>
        /// <param name="order"> Bondorder
        /// </param>
        void addBond(int atom1, int atom2, double order);


        /// <summary>  Adds a LonePair to this Atom.
        /// 
        /// </summary>
        /// <param name="atomID"> The atom number to which the LonePair is added in [0,..]
        /// </param>
        void addLonePair(int atomID);


        /// <summary>  True, if the AtomContainer contains the given ElectronContainer object.
        /// 
        /// </summary>
        /// <param name="electronContainer">ElectronContainer that is searched for
        /// </param>
        /// <returns>                   True, if the AtomContainer contains the given bond object
        /// </returns>
        bool contains(IElectronContainer electronContainer);


        /// <summary>  True, if the AtomContainer contains the given atom object.
        /// 
        /// </summary>
        /// <param name="atom"> the atom this AtomContainer is searched for
        /// </param>
        /// <returns>       True, if the AtomContainer contains the given atom object
        /// </returns>
        bool contains(IAtom atom);
    }
}