/* $RCSfile$
* $Author: egonw $
* $Date: 2006-05-03 10:11:01 +0200 (Wed, 03 May 2006) $
* $Revision: 6125 $
*
* Copyright (C) 2005-2006  The Chemistry Development Kit (CDK) project
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
    /// <summary> A helper class to instantiate a IChemObject for a specific implementation.
    /// 
    /// </summary>
    /// <author>         egonw
    /// </author>
    /// <cdk.module>     interfaces </cdk.module>
    public interface IChemObjectBuilder
    {
        /// <summary> Constructs an completely unset IAminoAcid.
        /// 
        /// </summary>
        /// <returns> IAminoAcid implementation defined for this IChemObjectBuilder
        /// </returns>
        IAminoAcid newAminoAcid();

        /// <summary> Constructs an completely unset IAtom.
        /// 
        /// </summary>
        /// <returns> IAtom implementation defined for this IChemObjectBuilder
        /// </returns>
        IAtom newAtom();

        /// <summary> Constructs an IAtom from a String containing an element symbol.
        /// 
        /// </summary>
        /// <param name="elementSymbol"> The String describing the element for the Atom
        /// </param>
        /// <returns>                 IAtom implementation defined for this IChemObjectBuilder
        /// </returns>
        IAtom newAtom(String elementSymbol);

        /// <summary> Constructs an IAtom from an Element and a Point2d.
        /// 
        /// </summary>
        /// <param name="elementSymbol">  The Element
        /// </param>
        /// <param name="point2d">        The Point
        /// </param>
        /// <returns>                  IAtom implementation defined for this IChemObjectBuilder
        /// </returns>
        IAtom newAtom(String elementSymbol, Point2d point2d);

        /// <summary> Constructs an IAtom from an Element and a Point3d.
        /// 
        /// </summary>
        /// <param name="elementSymbol">  The symbol of the atom
        /// </param>
        /// <param name="point3d">        The 3D coordinates of the atom
        /// </param>
        /// <returns>                  IAtom implementation defined for this IChemObjectBuilder
        /// </returns>
        IAtom newAtom(String elementSymbol, Point3d point3d);

        /// <summary> Constructs an empty IAtomContainer.
        /// 
        /// </summary>
        /// <returns> IAtomContainer implementation defined for this IChemObjectBuilder
        /// </returns>
        IAtomContainer newAtomContainer();

        /// <summary> Constructs an empty IAtomContainer that will contain a certain number of
        /// atoms and electronContainers. It will set the starting array lengths to the
        /// defined values, but will not create any IAtom or IElectronContainer's.
        /// 
        /// </summary>
        /// <param name="atomCount">              Number of atoms to be in this container
        /// </param>
        /// <param name="electronContainerCount"> Number of electronContainers to be in this
        /// container
        /// </param>
        /// <returns>                         IAtomContainer implementation defined for
        /// this IChemObjectBuilder
        /// </returns>
        IAtomContainer newAtomContainer(int atomCount, int electronContainerCount);

        /// <summary> Constructs an IAtomContainer with a copy of the atoms and electronContainers
        /// of another IAtomContainer (A shallow copy, i.e., with the same objects as in
        /// the original IAtomContainer).
        /// 
        /// </summary>
        /// <param name="container"> An AtomContainer to copy the atoms and electronContainers from
        /// </param>
        /// <returns>            IAtomContainer implementation defined for this IChemObjectBuilder
        /// </returns>
        IAtomContainer newAtomContainer(IAtomContainer container);

        /// <summary> Constructs an IAtomParity.
        /// 
        /// </summary>
        /// <param name="centralAtom">Atom for which the parity is defined
        /// </param>
        /// <param name="first">      First Atom of four that define the stereochemistry
        /// </param>
        /// <param name="second">     Second Atom of four that define the stereochemistry
        /// </param>
        /// <param name="third">      Third Atom of four that define the stereochemistry
        /// </param>
        /// <param name="fourth">     Fourth Atom of four that define the stereochemistry
        /// </param>
        /// <param name="parity">     +1 or -1, defining the parity
        /// </param>
        /// <returns>            IAtomParity implementation defined for this IChemObjectBuilder
        /// </returns>
        IAtomParity newAtomParity(IAtom centralAtom, IAtom first, IAtom second, IAtom third, IAtom fourth, int parity);

        /// <summary> Constructor for the IAtomType object.
        /// 
        /// </summary>
        /// <param name="elementSymbol"> Symbol of the atom
        /// </param>
        /// <returns>               IAtomType implementation defined for this IChemObjectBuilder
        /// </returns>
        IAtomType newAtomType(String elementSymbol);

        /// <summary> Constructor for the IAtomType object.
        /// 
        /// </summary>
        /// <param name="identifier">    An id for this atom type, like C3 for sp3 carbon
        /// </param>
        /// <param name="elementSymbol"> The element symbol identifying the element to which this atom type applies
        /// </param>
        /// <returns>                IAtomType implementation defined for this IChemObjectBuilder
        /// </returns>
        IAtomType newAtomType(String identifier, String elementSymbol);

        /// <summary> Contructs a new IBioPolymer to store the IStrands.
        /// 
        /// </summary>
        /// <returns>  IBioPolymer implementation defined for this IChemObjectBuilder
        /// </returns>
        IBioPolymer newBioPolymer();

        /// <summary> Constructs an empty IBond.
        /// 
        /// </summary>
        /// <returns> IBond implementation defined for this IChemObjectBuilder
        /// </returns>
        IBond newBond();

        /// <summary> Constructs a IBond with a single bond order..
        /// 
        /// </summary>
        /// <param name="atom1"> the first IAtom in the bond
        /// </param>
        /// <param name="atom2"> the second IAtom in the bond
        /// </param>
        /// <returns> IBond  implementation defined for this IChemObjectBuilder
        /// </returns>
        IBond newBond(IAtom atom1, IAtom atom2);

        /// <summary> Constructs a IBond with a given order.
        /// 
        /// </summary>
        /// <param name="atom1"> the first IAtom in the bond
        /// </param>
        /// <param name="atom2"> the second IAtom in the bond
        /// </param>
        /// <param name="order"> the bond order
        /// </param>
        /// <returns> IBond  implementation defined for this IChemObjectBuilder
        /// </returns>
        IBond newBond(IAtom atom1, IAtom atom2, double order);

        /// <summary> Constructs a IBond with a given order and stereo orientation from an array
        /// of atoms.
        /// 
        /// </summary>
        /// <param name="atom1">  the first Atom in the bond
        /// </param>
        /// <param name="atom2">  the second Atom in the bond
        /// </param>
        /// <param name="order">  the bond order
        /// </param>
        /// <param name="stereo"> a descriptor the stereochemical orientation of this bond
        /// </param>
        /// <returns> IBond   implementation defined for this IChemObjectBuilder
        /// </returns>
        IBond newBond(IAtom atom1, IAtom atom2, double order, int stereo);

        /// <summary> Constructs an empty IChemFile.
        /// 
        /// </summary>
        /// <returns> IChemFile implementation defined for this IChemObjectBuilder
        /// </returns>
        IChemFile newChemFile();

        /// <summary> Constructs an new IChemModel with a null ISetOfMolecules.
        /// 
        /// </summary>
        /// <returns> IChemModel implementation defined for this IChemObjectBuilder
        /// </returns>
        IChemModel newChemModel();

        /// <summary> Constructs an new IChemObject.
        /// 
        /// </summary>
        /// <returns> IChemObject implementation defined for this IChemObjectBuilder
        /// </returns>
        IChemObject newChemObject();

        /// <summary> Constructs an empty IChemSequence.
        /// 
        /// </summary>
        /// <returns> IChemSequence implementation defined for this IChemObjectBuilder
        /// </returns>
        IChemSequence newChemSequence();

        /// <summary> Constructs a new ICrystal with zero length cell axis.
        /// 
        /// </summary>
        /// <returns> ICrystal implementation defined for this IChemObjectBuilder
        /// </returns>
        ICrystal newCrystal();

        /// <summary> Constructs a new ICrystal with zero length cell axis
        /// and adds the atoms in the IAtomContainer as cell content.
        /// 
        /// </summary>
        /// <param name="container"> the IAtomContainer providing the atoms and bonds
        /// </param>
        /// <returns>           ICrystal implementation defined for this IChemObjectBuilder
        /// </returns>
        ICrystal newCrystal(IAtomContainer container);

        /// <summary> Constructs an empty IElectronContainer.
        /// 
        /// </summary>
        /// <returns> IElectronContainer implementation defined for this IChemObjectBuilder
        /// </returns>
        IElectronContainer newElectronContainer();

        /// <summary> Constructs an empty IElement.
        /// 
        /// </summary>
        /// <returns> IElement implementation defined for this IChemObjectBuilder
        /// </returns>
        IElement newElement();

        /// <summary> Constructs an IElement with a given element symbol.
        /// 
        /// </summary>
        /// <param name="symbol">The element symbol that this element should have.
        /// </param>
        /// <returns>        IElement implementation defined for this IChemObjectBuilder
        /// </returns>
        IElement newElement(System.String symbol);

        /// <summary> Constructs an IElement with a given element symbol, 
        /// atomic number and atomic mass.
        /// 
        /// </summary>
        /// <param name="symbol">      The element symbol of this element.
        /// </param>
        /// <param name="atomicNumber">The atomicNumber of this element.
        /// </param>
        /// <returns>               IElement implementation defined for this IChemObjectBuilder
        /// </returns>
        IElement newElement(System.String symbol, int atomicNumber);

        /// <summary> Constructor for the IIsotope object.
        /// 
        /// </summary>
        /// <param name="elementSymbol"> The element symbol, "O" for oxygen, etc.
        /// </param>
        /// <returns>                IIsotope implementation defined for this IChemObjectBuilder
        /// </returns>
        IIsotope newIsotope(System.String elementSymbol);

        /// <summary> Constructor for the IIsotope object.
        /// 
        /// </summary>
        /// <param name="atomicNumber">  The atomic number of the isotope
        /// </param>
        /// <param name="elementSymbol"> The element symbol, "O" for oxygen, etc.
        /// </param>
        /// <param name="massNumber">    The atomic mass of the isotope, 16 for oxygen, e.g.
        /// </param>
        /// <param name="exactMass">     The exact mass of the isotope, be a little more explicit here :-)
        /// </param>
        /// <param name="abundance">     The natural abundance of the isotope
        /// </param>
        /// <returns>                IIsotope implementation defined for this IChemObjectBuilder
        /// </returns>
        IIsotope newIsotope(int atomicNumber, System.String elementSymbol, int massNumber, double exactMass, double abundance);

        /// <summary> Constructor for the IIsotope object.
        /// 
        /// </summary>
        /// <param name="atomicNumber">  The atomic number of the isotope, 8 for oxygen
        /// </param>
        /// <param name="elementSymbol"> The element symbol, "O" for oxygen, etc.
        /// </param>
        /// <param name="exactMass">     The exact mass of the isotope, be a little more explicit here :-)
        /// </param>
        /// <param name="abundance">     The natural abundance of the isotope
        /// </param>
        /// <returns>                IIsotope implementation defined for this IChemObjectBuilder
        /// </returns>
        IIsotope newIsotope(int atomicNumber, System.String elementSymbol, double exactMass, double abundance);

        /// <summary> Constructor for the IIsotope object.
        /// 
        /// </summary>
        /// <param name="elementSymbol"> The element symbol, "O" for oxygen, etc.
        /// </param>
        /// <param name="massNumber">    The atomic mass of the isotope, 16 for oxygen, e.g.
        /// </param>
        /// <returns>                IIsotope implementation defined for this IChemObjectBuilder
        /// </returns>
        IIsotope newIsotope(System.String elementSymbol, int massNumber);

        /// <summary> Constructs an unconnected ILonePair.
        /// 
        /// </summary>
        /// <returns>  ILonePair implementation defined for this IChemObjectBuilder
        /// </returns>
        ILonePair newLonePair();

        /// <summary> Constructs an ILonePair on an IAtom.
        /// 
        /// </summary>
        /// <param name="atom"> IAtom to which this lone pair is connected
        /// </param>
        /// <returns>      ILonePair implementation defined for this IChemObjectBuilder
        /// </returns>
        ILonePair newLonePair(IAtom atom);

        /// <summary> Creates a directional IMapping between IChemObject's.
        /// 
        /// </summary>
        /// <param name="objectOne">object which is being mapped 
        /// </param>
        /// <param name="objectTwo">object to which is being mapped
        /// </param>
        /// <returns>          IMapping implementation defined for this IChemObjectBuilder
        /// </returns>
        IMapping newMapping(IChemObject objectOne, IChemObject objectTwo);

        /// <summary> Creates an IMolecule without any IAtoms and IBonds.
        /// 
        /// </summary>
        /// <returns> IMolecule implementation defined for this IChemObjectBuilder
        /// </returns>
        IMolecule newMolecule();

        /// <summary> Constructor for the IMolecule object. The parameters define the
        /// initial capacity of the arrays.
        /// 
        /// </summary>
        /// <param name="atomCount">              init capacity of IAtom array
        /// </param>
        /// <param name="electronContainerCount"> init capacity of IElectronContainer array
        /// </param>
        /// <returns>                         IMolecule implementation defined for this IChemObjectBuilder
        /// </returns>
        IMolecule newMolecule(int atomCount, int electronContainerCount);

        /// <summary> Constructs an IMolecule with
        /// a shallow copy of the atoms and bonds of an IAtomContainer.
        /// 
        /// </summary>
        /// <param name="container"> An IMolecule to copy the atoms and bonds from
        /// </param>
        /// <returns>             IMolecule implementation defined for this IChemObjectBuilder
        /// </returns>
        IMolecule newMolecule(IAtomContainer container);

        /// <summary> Contructs a new IMonomer.
        /// 
        /// </summary>
        /// <returns> IMonomer implementation defined for this IChemObjectBuilder
        /// </returns>
        IMonomer newMonomer();

        /// <summary> Contructs a new IPolymer to store the IMonomers.
        /// 
        /// </summary>
        /// <returns> IPolymer implementation defined for this IChemObjectBuilder
        /// </returns>
        IPolymer newPolymer();

        /// <summary> Constructs an empty, forward IReaction.
        /// 
        /// </summary>
        /// <returns> IReaction implementation defined for this IChemObjectBuilder
        /// </returns>
        IReaction newReaction();

        /// <summary> Constructs an empty IRing.
        /// 
        /// </summary>
        /// <returns> IRing implementation defined for this IChemObjectBuilder
        /// </returns>
        IRing newRing();

        /// <summary> Constructs a IRing from an IAtomContainer.
        /// 
        /// </summary>
        /// <param name="container">IAtomContainer to create the IRing from
        /// </param>
        /// <returns>           IRing implementation defined for this IChemObjectBuilder
        /// </returns>
        IRing newRing(IAtomContainer container);

        /// <summary> Constructs a ring that will have a certain number of atoms of the given elements.
        /// 
        /// </summary>
        /// <param name="ringSize">     The number of atoms and bonds the ring will have
        /// </param>
        /// <param name="elementSymbol">The element of the atoms the ring will have
        /// </param>
        /// <returns>               IRing implementation defined for this IChemObjectBuilder
        /// </returns>
        IRing newRing(int ringSize, System.String elementSymbol);

        /// <summary> Constructs an empty IRing that will have a certain size.
        /// 
        /// </summary>
        /// <param name="ringSize">The size (number of atoms) the ring will have
        /// </param>
        /// <returns>          IRing implementation defined for this IChemObjectBuilder
        /// </returns>
        IRing newRing(int ringSize);

        /// <summary> Constructs an empty IRingSet.
        /// 
        /// </summary>
        /// <returns> IRingSet implementation defined for this IChemObjectBuilder
        /// </returns>
        IRingSet newRingSet();

        /// <summary> Constructs an empty ISetOfAtomContainers.
        /// 
        /// </summary>
        /// <returns> ISetOfAtomContainers implementation defined for this IChemObjectBuilder
        /// </returns>
        ISetOfAtomContainers newSetOfAtomContainers();

        /// <summary> Constructs an empty ISetOfMolecules.
        /// 
        /// </summary>
        /// <returns> ISetOfMolecules implementation defined for this IChemObjectBuilder
        /// </returns>
        ISetOfMolecules newSetOfMolecules();

        /// <summary> Constructs an empty ISetOfReactions.
        /// 
        /// </summary>
        /// <returns> ISetOfReactions implementation defined for this IChemObjectBuilder
        /// </returns>
        ISetOfReactions newSetOfReactions();

        /// <summary> Constructs an single electron orbital with an associated IAtom.
        /// 
        /// </summary>
        /// <returns> ISingleElectron implementation defined for this IChemObjectBuilder
        /// </returns>
        ISingleElectron newSingleElectron();

        /// <summary> Constructs an single electron orbital on an IAtom.
        /// 
        /// </summary>
        /// <param name="atom">The atom to which the single electron belongs.
        /// </param>
        /// <returns>     ISingleElectron implementation defined for this IChemObjectBuilder
        /// </returns>
        ISingleElectron newSingleElectron(IAtom atom);

        /// <summary> Contructs a new IStrand.
        /// 
        /// </summary>
        /// <returns> IStrand implementation defined for this IChemObjectBuilder
        /// </returns>
        IStrand newStrand();

        /// <summary> Constructs an empty IPseudoAtom.
        /// 
        /// </summary>
        /// <returns> IPseudoAtom implementation defined for this IChemObjectBuilder
        /// </returns>
        IPseudoAtom newPseudoAtom();

        /// <summary> Constructs an IPseudoAtom from a label.
        /// 
        /// </summary>
        /// <param name="label"> The String describing the PseudoAtom
        /// </param>
        /// <returns> IPseudoAtom implementation defined for this IChemObjectBuilder
        /// </returns>
        IPseudoAtom newPseudoAtom(System.String label);

        /// <summary> Constructs an IPseudoAtom from an existing IAtom object.
        /// 
        /// </summary>
        /// <param name="atom"> IAtom from which the IPseudoAtom is constructed
        /// </param>
        /// <returns>        IPseudoAtom implementation defined for this IChemObjectBuilder
        /// </returns>
        IPseudoAtom newPseudoAtom(IAtom atom);

        /// <summary> Constructs an IPseudoAtom from a label and a Point3d.
        /// 
        /// </summary>
        /// <param name="label">  The String describing the IPseudoAtom
        /// </param>
        /// <param name="point3d">The 3D coordinates of the IPseudoAtom
        /// </param>
        /// <returns>          IPseudoAtom implementation defined for this IChemObjectBuilder
        /// </returns>
        IPseudoAtom newPseudoAtom(System.String label, javax.vecmath.Point3d point3d);

        /// <summary> Constructs an IPseudoAtom from a label and a Point2d.
        /// 
        /// </summary>
        /// <param name="label">  The String describing the IPseudoAtom
        /// </param>
        /// <param name="point2d">The 2D coordinates of the IPseudoAtom
        /// </param>
        /// <returns>          IPseudoAtom implementation defined for this IChemObjectBuilder
        /// </returns>
        IPseudoAtom newPseudoAtom(System.String label, javax.vecmath.Point2d point2d);
    }
}