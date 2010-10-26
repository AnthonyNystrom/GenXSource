/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $
* $Revision: 6672 $
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
using Org.OpenScience.CDK.Interfaces;
using javax.vecmath;

namespace Org.OpenScience.CDK
{
    /// <summary> A helper class to instantiate a IChemObject for a specific implementation.
    /// 
    /// </summary>
    /// <author>         egonw
    /// </author>
    /// <cdk.module>     data </cdk.module>
    public class DefaultChemObjectBuilder : IChemObjectBuilder
    {
        public static DefaultChemObjectBuilder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DefaultChemObjectBuilder();
                }
                return instance;
            }

        }

        private static DefaultChemObjectBuilder instance = null;

        private DefaultChemObjectBuilder()
        {
        }

        public virtual IAminoAcid newAminoAcid()
        {
            return new AminoAcid();
        }

        public virtual IAtom newAtom()
        {
            return new Atom();
        }

        public virtual IAtom newAtom(String elementSymbol)
        {
            return new Atom(elementSymbol);
        }

        public virtual IAtom newAtom(String elementSymbol, Point2d point2d)
        {
            return new Atom(elementSymbol, point2d);
        }

        public virtual IAtom newAtom(String elementSymbol, Point3d point3d)
        {
            return new Atom(elementSymbol, point3d);
        }

        public virtual IAtomContainer newAtomContainer()
        {
            return new AtomContainer();
        }

        public virtual IAtomContainer newAtomContainer(int atomCount, int electronContainerCount)
        {
            return new AtomContainer(atomCount, electronContainerCount);
        }

        public virtual IAtomContainer newAtomContainer(IAtomContainer container)
        {
            return new AtomContainer(container);
        }

        public virtual IAtomParity newAtomParity(IAtom centralAtom, IAtom first, IAtom second, IAtom third, IAtom fourth, int parity)
        {
            return new AtomParity(centralAtom, first, second, third, fourth, parity);
        }

        public virtual IAtomType newAtomType(String elementSymbol)
        {
            return new AtomType(elementSymbol);
        }

        public virtual IAtomType newAtomType(String identifier, String elementSymbol)
        {
            return new AtomType(identifier, elementSymbol);
        }

        public virtual IBioPolymer newBioPolymer()
        {
            return new BioPolymer();
        }

        public virtual IBond newBond()
        {
            return new Bond();
        }

        public virtual IBond newBond(IAtom atom1, IAtom atom2)
        {
            return new Bond(atom1, atom2);
        }

        public virtual IBond newBond(IAtom atom1, IAtom atom2, double order)
        {
            return new Bond(atom1, atom2, order);
        }

        public virtual IBond newBond(IAtom atom1, IAtom atom2, double order, int stereo)
        {
            return new Bond(atom1, atom2, order, stereo);
        }

        public virtual IChemFile newChemFile()
        {
            return new ChemFile();
        }

        public virtual IChemModel newChemModel()
        {
            return new ChemModel();
        }

        public virtual IChemObject newChemObject()
        {
            return new ChemObject();
        }

        public virtual IChemSequence newChemSequence()
        {
            return new ChemSequence();
        }

        public virtual ICrystal newCrystal()
        {
            return new Crystal();
        }

        public virtual ICrystal newCrystal(IAtomContainer container)
        {
            return new Crystal(container);
        }

        public virtual IElectronContainer newElectronContainer()
        {
            return new ElectronContainer();
        }

        public virtual IElement newElement()
        {
            return new Element();
        }

        public virtual IElement newElement(System.String symbol)
        {
            return new Element(symbol);
        }

        public virtual IElement newElement(System.String symbol, int atomicNumber)
        {
            return new Element(symbol, atomicNumber);
        }

        public virtual IIsotope newIsotope(System.String elementSymbol)
        {
            return new Isotope(elementSymbol);
        }

        public virtual IIsotope newIsotope(int atomicNumber, System.String elementSymbol, int massNumber, double exactMass, double abundance)
        {
            return new Isotope(atomicNumber, elementSymbol, massNumber, exactMass, abundance);
        }

        public virtual IIsotope newIsotope(int atomicNumber, System.String elementSymbol, double exactMass, double abundance)
        {
            return new Isotope(atomicNumber, elementSymbol, exactMass, abundance);
        }

        public virtual IIsotope newIsotope(System.String elementSymbol, int massNumber)
        {
            return new Isotope(elementSymbol, massNumber);
        }

        public virtual ILonePair newLonePair()
        {
            return new LonePair();
        }

        public virtual ILonePair newLonePair(IAtom atom)
        {
            return new LonePair(atom);
        }

        public virtual IMapping newMapping(IChemObject objectOne, IChemObject objectTwo)
        {
            return new Mapping(objectOne, objectTwo);
        }

        public virtual IMolecule newMolecule()
        {
            return new Molecule();
        }

        public virtual IMolecule newMolecule(int atomCount, int electronContainerCount)
        {
            return new Molecule(atomCount, electronContainerCount);
        }

        public virtual IMolecule newMolecule(IAtomContainer container)
        {
            return new Molecule(container);
        }

        public virtual IMonomer newMonomer()
        {
            return new Monomer();
        }

        public virtual IPolymer newPolymer()
        {
            return new Polymer();
        }

        public virtual IReaction newReaction()
        {
            return new Reaction();
        }

        public virtual IRing newRing()
        {
            return new Ring();
        }

        public virtual IRing newRing(IAtomContainer container)
        {
            return new Ring(container);
        }

        public virtual IRing newRing(int ringSize, System.String elementSymbol)
        {
            return new Ring(ringSize, elementSymbol);
        }

        public virtual IRing newRing(int ringSize)
        {
            return new Ring(ringSize);
        }

        public virtual IRingSet newRingSet()
        {
            return new RingSet();
        }

        public virtual ISetOfAtomContainers newSetOfAtomContainers()
        {
            return new SetOfAtomContainers();
        }

        public virtual ISetOfMolecules newSetOfMolecules()
        {
            return new SetOfMolecules();
        }

        public virtual ISetOfReactions newSetOfReactions()
        {
            return new SetOfReactions();
        }

        public virtual ISingleElectron newSingleElectron()
        {
            return new SingleElectron();
        }

        public virtual ISingleElectron newSingleElectron(IAtom atom)
        {
            return new SingleElectron(atom);
        }

        public virtual IStrand newStrand()
        {
            return new Strand();
        }

        public virtual IPseudoAtom newPseudoAtom()
        {
            return new PseudoAtom();
        }

        public virtual IPseudoAtom newPseudoAtom(System.String label)
        {
            return new PseudoAtom(label);
        }

        public virtual IPseudoAtom newPseudoAtom(IAtom atom)
        {
            return new PseudoAtom(atom);
        }

        public virtual IPseudoAtom newPseudoAtom(String label, Point3d point3d)
        {
            return new PseudoAtom(label, point3d);
        }

        public virtual IPseudoAtom newPseudoAtom(String label, Point2d point2d)
        {
            return new PseudoAtom(label, point2d);
        }
    }
}