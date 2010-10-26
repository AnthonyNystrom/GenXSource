/*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $
*  $Revision: 6672 $
*
*  Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
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
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK
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
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>      steinbeck
    /// </author>
    /// <cdk.created>     2000-10-02 </cdk.created>
    [Serializable]
    public class AtomContainer : ChemObject, IAtomContainer
    {
        /// <summary>  Returns the array of atoms of this AtomContainer.
        /// 
        /// </summary>
        /// <returns>    The array of atoms of this AtomContainer
        /// </returns>
        /// <seealso cref="setAtoms">
        /// </seealso>
        /// <summary>  Sets the array of atoms of this AtomContainer.
        /// 
        /// </summary>
        /// <param name="atoms"> The array of atoms to be assigned to this AtomContainer
        /// </param>
        /// <seealso cref="getAtoms">
        /// </seealso>
        virtual public IAtom[] Atoms
        {
            get
            {
                IAtom[] returnAtoms = new IAtom[AtomCount];
                Array.Copy(this.atoms_Renamed_Field, 0, returnAtoms, 0, returnAtoms.Length);
                return returnAtoms;
            }

            set
            {
                this.atoms_Renamed_Field = value;
                for (int f = 0; f < value.Length; f++)
                {
                    value[f].addListener(this);
                }
                AtomCount = value.Length;
                notifyChanged();
            }

        }
        
        /// <summary>  Returns the array of electronContainers of this AtomContainer.
        /// 
        /// </summary>
        /// <returns>    The array of electronContainers of this AtomContainer
        /// </returns>
        /// <seealso cref="setElectronContainers">
        /// </seealso>
        /// <summary>  Sets the array of electronContainers of this AtomContainer.
        /// 
        /// </summary>
        /// <param name="electronContainers"> The array of electronContainers to be assigned to
        /// this AtomContainer
        /// </param>
        /// <seealso cref="getElectronContainers">
        /// </seealso>
        virtual public IElectronContainer[] ElectronContainers
        {
            get
            {
                IElectronContainer[] returnElectronContainers = new IElectronContainer[ElectronContainerCount];
                Array.Copy(this.electronContainers, 0, returnElectronContainers, 0, returnElectronContainers.Length);
                return returnElectronContainers;
            }

            set
            {
                this.electronContainers = value;
                for (int f = 0; f < value.Length; f++)
                {
                    value[f].addListener(this);
                }
                ElectronContainerCount = value.Length;
                notifyChanged();
            }

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
        virtual public int ElectronContainerCount
        {
            get
            {
                return this.electronContainerCount;
            }

            set
            {
                this.electronContainerCount = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
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
        virtual public int AtomCount
        {
            get
            {
                return this.atomCount;
            }

            set
            {
                this.atomCount = value;
                notifyChanged();
            }

        }
        /// <summary>  Returns the array of Bonds of this AtomContainer.
        /// 
        /// </summary>
        /// <returns>    The array of Bonds of this AtomContainer
        /// </returns>
        /// <seealso cref="getElectronContainers">
        /// </seealso>
        virtual public IBond[] Bonds
        {
            get
            {
                int bondCount = getBondCount();
                IBond[] result = new IBond[bondCount];
                int bondCounter = 0;
                for (int i = 0; i < ElectronContainerCount; i++)
                {
                    IElectronContainer electronContainer = getElectronContainerAt(i);
                    if (electronContainer is IBond)
                    {
                        result[bondCounter] = (Bond)electronContainer;
                        bondCounter++;
                    }
                }
                return result;
            }

        }
        /// <summary>  Returns the atom at position 0 in the container.
        /// 
        /// </summary>
        /// <returns>    The atom at position 0 .
        /// </returns>
        virtual public IAtom FirstAtom
        {
            get
            {
                return (Atom)atoms_Renamed_Field[0];
            }

        }
        /// <summary>  Returns the atom at the last position in the container.
        /// 
        /// </summary>
        /// <returns>    The atom at the last position
        /// </returns>
        virtual public IAtom LastAtom
        {
            get
            {
                return (Atom)atoms_Renamed_Field[AtomCount - 1];
            }

        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = 5678100348445919254L;

        /// <summary>  Number of atoms contained by this object.</summary>
        protected internal int atomCount;

        /// <summary>  Number of electronContainers contained by this object.</summary>
        protected internal int electronContainerCount;

        /// <summary>  Amount by which the bond and arom arrays grow when elements are added and
        /// the arrays are not large enough for that.
        /// </summary>
        protected internal int growArraySize = 10;

        /// <summary>  Internal array of atoms.</summary>
        protected internal IAtom[] atoms_Renamed_Field;

        /// <summary>  Internal array of bond.</summary>
        protected internal IElectronContainer[] electronContainers;

        /// <summary> Internal list of atom parities.</summary>
        protected internal System.Collections.Hashtable atomParities;


        /// <summary>  Constructs an empty AtomContainer.</summary>
        public AtomContainer()
            : this(10, 10)
        {
        }


        /// <summary> Constructs an AtomContainer with a copy of the atoms and electronContainers
        /// of another AtomContainer (A shallow copy, i.e., with the same objects as in
        /// the original AtomContainer).
        /// 
        /// </summary>
        /// <param name="container"> An AtomContainer to copy the atoms and electronContainers from
        /// </param>
        public AtomContainer(IAtomContainer container)
        {
            this.atomCount = container.AtomCount;
            this.electronContainerCount = container.ElectronContainerCount;
            atoms_Renamed_Field = new IAtom[this.atomCount];
            electronContainers = new IElectronContainer[this.electronContainerCount];
            atomParities = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable((int)(atomCount / 2)));

            for (int f = 0; f < container.AtomCount; f++)
            {
                atoms_Renamed_Field[f] = container.getAtomAt(f);
                container.getAtomAt(f).addListener(this);
            }
            for (int f = 0; f < container.ElectronContainerCount; f++)
            {
                electronContainers[f] = container.getElectronContainerAt(f);
                container.getElectronContainerAt(f).addListener(this);
            }
        }


        /// <summary>  Constructs an empty AtomContainer that will contain a certain number of
        /// atoms and electronContainers. It will set the starting array lengths to the
        /// defined values, but will not create any Atom or ElectronContainer's.
        /// 
        /// </summary>
        /// <param name="atomCount">              Number of atoms to be in this container
        /// </param>
        /// <param name="electronContainerCount"> Number of electronContainers to be in this
        /// container
        /// </param>
        public AtomContainer(int atomCount, int electronContainerCount)
        {
            this.atomCount = 0;
            this.electronContainerCount = 0;
            atoms_Renamed_Field = new IAtom[atomCount];
            electronContainers = new IElectronContainer[electronContainerCount];
            atomParities = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable((int)(atomCount / 2)));
        }

        /// <summary> Adds an AtomParity to this container. If a parity is already given for the
        /// affected Atom, it is overwritten.
        /// 
        /// </summary>
        /// <param name="parity">The new AtomParity for this container
        /// </param>
        /// <seealso cref="getAtomParity">
        /// </seealso>
        public virtual void addAtomParity(IAtomParity parity)
        {
            atomParities[parity.Atom] = parity;
        }

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
        public virtual IAtomParity getAtomParity(IAtom atom)
        {
            return (AtomParity)atomParities[atom];
        }


        /// <summary>  Set the atom at position <code>number</code> in [0,..].
        /// 
        /// </summary>
        /// <param name="number"> The position of the atom to be set.
        /// </param>
        /// <param name="atom">   The atom to be stored at position <code>number</code>
        /// </param>
        /// <seealso cref="getAtomAt">
        /// </seealso>
        public virtual void setAtomAt(int number, IAtom atom)
        {
            atom.addListener(this);
            atoms_Renamed_Field[number] = atom;
            notifyChanged();
        }


        /// <summary>  Get the atom at position <code>number</code> in [0,..].
        /// 
        /// </summary>
        /// <param name="number"> The position of the atom to be retrieved.
        /// </param>
        /// <returns>         The atomAt value
        /// </returns>
        /// <seealso cref="setAtomAt">
        /// </seealso>
        public virtual IAtom getAtomAt(int number)
        {
            return atoms_Renamed_Field[number];
        }


        /// <summary>  Get the bond at position <code>number</code> in [0,..].
        /// 
        /// </summary>
        /// <param name="number"> The position of the bond to be retrieved.
        /// </param>
        /// <returns>         The bondAt value
        /// </returns>
        /// <seealso cref="setElectronContainerAt">
        /// </seealso>
        public virtual IBond getBondAt(int number)
        {
            return Bonds[number];
        }



        /// <summary> Sets the ElectronContainer at position <code>number</code> in [0,..].
        /// 
        /// </summary>
        /// <param name="number">           The position of the ElectronContainer to be set.
        /// </param>
        /// <param name="electronContainer">The ElectronContainer to be stored at position <code>number</code>
        /// </param>
        /// <seealso cref="getElectronContainerAt">
        /// </seealso>
        public virtual void setElectronContainerAt(int number, IElectronContainer electronContainer)
        {
            electronContainer.addListener(this);
            electronContainers[number] = electronContainer;
            notifyChanged();
        }


        /// <summary>  Returns an AtomEnumeration for looping over all atoms in this container.
        /// 
        /// </summary>
        /// <returns>    An AtomEnumeration with the atoms in this container
        /// </returns>
        /// <seealso cref="getAtoms">
        /// </seealso>
        public virtual System.Collections.IEnumerator atoms()
        {
            return new AtomEnumeration(this);
        }


        /// <summary>  Returns the array of Bonds of this AtomContainer.
        /// 
        /// </summary>
        /// <returns>    The array of Bonds of this AtomContainer
        /// </returns>
        /// <seealso cref="getElectronContainers">
        /// </seealso>
        /// <seealso cref="getBonds">
        /// </seealso>
        public virtual ILonePair[] getLonePairs()
        {
            int count = getLonePairCount();
            ILonePair[] result = new ILonePair[count];
            int counter = 0;
            for (int i = 0; i < ElectronContainerCount; i++)
            {
                IElectronContainer electronContainer = getElectronContainerAt(i);
                if (electronContainer is ILonePair)
                {
                    result[counter] = (ILonePair)electronContainer;
                    counter++;
                }
            }
            return result;
        }


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
        public virtual ILonePair[] getLonePairs(IAtom atom)
        {
            System.Collections.IList lps = new System.Collections.ArrayList();
            for (int i = 0; i < ElectronContainerCount; i++)
            {
                IElectronContainer electronContainer = getElectronContainerAt(i);
                if ((electronContainer is ILonePair) && (((ILonePair)electronContainer).contains(atom)))
                {
                    lps.Add(electronContainer);
                }
            }
            ILonePair[] result = new ILonePair[lps.Count];
            for (int i = 0; i < lps.Count; i++)
            {
                result[i] = (ILonePair)lps[i];
            }
            return result;
        }


        /// <summary>  Returns the position of a given atom in the atoms array. It returns -1 if
        /// the atom atom does not exist.
        /// 
        /// </summary>
        /// <param name="atom"> The atom to be sought
        /// </param>
        /// <returns>       The Position of the atom in the atoms array in [0,..].
        /// </returns>
        public virtual int getAtomNumber(IAtom atom)
        {
            for (int f = 0; f < AtomCount; f++)
            {
                if (getAtomAt(f) == atom)
                {
                    return f;
                }
            }
            return -1;
        }


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
        public virtual int getBondNumber(IAtom atom1, IAtom atom2)
        {
            return (getBondNumber(getBond(atom1, atom2)));
        }


        /// <summary>  Returns the position of a given bond in the electronContainers array. It
        /// returns -1 if the bond does not exist.
        /// 
        /// </summary>
        /// <param name="bond"> The bond to be sought
        /// </param>
        /// <returns>       The Position of the bond in the electronContainers array in [0,..].
        /// </returns>
        public virtual int getBondNumber(IBond bond)
        {
            for (int f = 0; f < ElectronContainerCount; f++)
            {
                if (getElectronContainerAt(f) == bond)
                {
                    return f;
                }
            }
            return -1;
        }


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
        public virtual IElectronContainer getElectronContainerAt(int number)
        {
            return (ElectronContainer)electronContainers[number];
        }


        /// <summary> Returns the bond that connectes the two given atoms.
        /// 
        /// </summary>
        /// <param name="atom1"> The first atom
        /// </param>
        /// <param name="atom2"> The second atom
        /// </param>
        /// <returns>        The bond that connectes the two atoms
        /// </returns>
        public virtual IBond getBond(IAtom atom1, IAtom atom2)
        {
            for (int i = 0; i < ElectronContainerCount; i++)
            {
                if (electronContainers[i] is IBond && ((IBond)electronContainers[i]).contains(atom1) && ((IBond)electronContainers[i]).getConnectedAtom(atom1) == atom2)
                {
                    return (IBond)electronContainers[i];
                }
            }
            return null;
        }


        /// <summary>  Returns an array of all atoms connected to the given atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom the bond partners are searched of.
        /// </param>
        /// <returns>       The array of <code>Atom</code>s with the size of connected
        /// atoms
        /// </returns>
        public virtual IAtom[] getConnectedAtoms(IAtom atom)
        {
            System.Collections.IList atomList = getConnectedAtomsVector(atom);
            IAtom[] atoms = new IAtom[atomList.Count];
            for (int i = 0; i < atomList.Count; i++)
            {
                atoms[i] = (IAtom)atomList[i];
            }
            return atoms;
        }


        /// <summary>  Returns a vector of all atoms connected to the given atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom the bond partners are searched of.
        /// </param>
        /// <returns>       The vector with the size of connected atoms
        /// </returns>
        public virtual System.Collections.IList getConnectedAtomsVector(IAtom atom)
        {
            System.Collections.IList atomsVec = new System.Collections.ArrayList();
            IElectronContainer electronContainer;
            for (int i = 0; i < electronContainerCount; i++)
            {
                electronContainer = (IElectronContainer)electronContainers[i];
                if (electronContainer is IBond && ((IBond)electronContainer).contains(atom))
                {
                    atomsVec.Add(((IBond)electronContainer).getConnectedAtom(atom));
                }
            }
            return atomsVec;
        }


        /// <summary>  Returns an array of all Bonds connected to the given atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom the connected bonds are searched of
        /// </param>
        /// <returns>       The array with the size of connected atoms
        /// </returns>
        public virtual IBond[] getConnectedBonds(IAtom atom)
        {
            System.Collections.IList bondList = getConnectedBondsVector(atom);
            IBond[] bonds = new IBond[bondList.Count];
            for (int i = 0; i < bondList.Count; i++)
            {
                bonds[i] = (IBond)bondList[i];
            }
            return bonds;
        }

        /// <summary>  Returns a Vector of all Bonds connected to the given atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom the connected bonds are searched of
        /// </param>
        /// <returns>       The vector with the size of connected atoms
        /// </returns>
        public virtual System.Collections.IList getConnectedBondsVector(IAtom atom)
        {
            System.Collections.IList bondsVec = new System.Collections.ArrayList();
            for (int i = 0; i < electronContainerCount; i++)
            {
                if (electronContainers[i] is IBond && ((IBond)electronContainers[i]).contains(atom))
                {
                    bondsVec.Add(electronContainers[i]);
                }
            }
            return (bondsVec);
        }


        /// <summary>  Returns an array of all electronContainers connected to the given atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom the connected electronContainers are searched of
        /// </param>
        /// <returns>       The array with the size of connected atoms
        /// </returns>
        public virtual IElectronContainer[] getConnectedElectronContainers(IAtom atom)
        {
            System.Collections.IList bondsVec = new System.Collections.ArrayList();
            for (int i = 0; i < electronContainerCount; i++)
            {
                if (electronContainers[i] is IBond && ((IBond)electronContainers[i]).contains(atom))
                {
                    bondsVec.Add(electronContainers[i]);
                }
                else if (electronContainers[i] is ILonePair && ((ILonePair)electronContainers[i]).contains((Atom)atom))
                {
                    bondsVec.Add(electronContainers[i]);
                }
                else if (electronContainers[i] is ISingleElectron && ((ISingleElectron)electronContainers[i]).contains((Atom)atom))
                {
                    bondsVec.Add(electronContainers[i]);
                }
            }
            IElectronContainer[] cons = new IElectronContainer[bondsVec.Count];
            for (int i = 0; i < bondsVec.Count; i++)
            {
                cons[i] = (IElectronContainer)bondsVec[i];
            }
            return cons;
        }


        /// <summary>  Returns the number of connected atoms (degree) to the given atom.
        /// 
        /// </summary>
        /// <param name="atomnumber"> The atomnumber the degree is searched for
        /// </param>
        /// <returns>             The number of connected atoms (degree)
        /// </returns>
        public virtual int getBondCount(int atomnumber)
        {
            return getBondCount(getAtomAt(atomnumber));
        }


        /// <summary>  Returns the number of LonePairs in this Container.
        /// 
        /// </summary>
        /// <returns>    The number of LonePairs in this Container
        /// </returns>
        public virtual int getLonePairCount()
        {
            int count = 0;
            for (int i = 0; i < electronContainerCount; i++)
            {
                if (electronContainers[i] is ILonePair)
                {
                    count++;
                }
            }
            return count;
        }


        /// <summary>  Returns the number of Bonds in this Container.
        /// 
        /// </summary>
        /// <returns>    The number of Bonds in this Container
        /// </returns>
        public virtual int getBondCount()
        {
            int bondCount = 0;
            for (int i = 0; i < electronContainerCount; i++)
            {
                if (electronContainers[i] is IBond)
                {
                    bondCount++;
                }
            }
            return bondCount;
        }


        /// <summary>  Returns the number of Bonds for a given Atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom
        /// </param>
        /// <returns>       The number of Bonds for this atom
        /// </returns>
        public virtual int getBondCount(IAtom atom)
        {
            int count = 0;
            for (int i = 0; i < ElectronContainerCount; i++)
            {
                if (electronContainers[i] is IBond && ((Bond)electronContainers[i]).contains(atom))
                {
                    count++;
                }
            }
            return count;
        }


        /// <summary>  Returns the number of LonePairs for a given Atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom
        /// </param>
        /// <returns>       The number of LonePairs for this atom
        /// </returns>
        public virtual int getLonePairCount(IAtom atom)
        {
            int count = 0;
            for (int i = 0; i < ElectronContainerCount; i++)
            {
                if (electronContainers[i] is ILonePair && ((ILonePair)electronContainers[i]).contains(atom))
                {
                    count++;
                }
            }
            return count;
        }
        /// <summary>  Returns an array of all SingleElectron connected to the given atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom on which the single electron is located
        /// </param>
        /// <returns>       The array of SingleElectron of this AtomContainer
        /// </returns>
        public virtual ISingleElectron[] getSingleElectron(IAtom atom)
        {
            System.Collections.IList lps = new System.Collections.ArrayList();
            for (int i = 0; i < ElectronContainerCount; i++)
            {
                if ((electronContainers[i] is ISingleElectron) && (((ISingleElectron)electronContainers[i]).contains((Atom)atom)))
                {
                    lps.Add(electronContainers[i]);
                }
            }
            ISingleElectron[] result = new ISingleElectron[lps.Count];
            for (int i = 0; i < lps.Count; i++)
            {
                result[i] = (ISingleElectron)lps[i];
            }
            return result;
        }
        /// <summary>  Returns the sum of the SingleElectron for a given Atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom on which the single electron is located
        /// </param>
        /// <returns>       The array of SingleElectron of this AtomContainer
        /// </returns>
        public virtual int getSingleElectronSum(IAtom atom)
        {
            int count = 0;
            for (int i = 0; i < ElectronContainerCount; i++)
            {
                if ((electronContainers[i] is ISingleElectron) && (((ISingleElectron)electronContainers[i]).contains((Atom)atom)))
                {
                    count++;
                }
            }
            return count;
        }
        /// <summary> Returns the sum of the bond orders for a given Atom.
        /// 
        /// </summary>
        /// <param name="atom"> The atom
        /// </param>
        /// <returns>       The number of bondorders for this atom
        /// </returns>
        public virtual double getBondOrderSum(IAtom atom)
        {
            double count = 0;
            for (int i = 0; i < ElectronContainerCount; i++)
            {
                if (electronContainers[i] is IBond && ((Bond)electronContainers[i]).contains(atom))
                {
                    count += ((Bond)electronContainers[i]).Order;
                }
            }
            return count;
        }

        /// <summary> Returns the maximum bond order that this atom currently has in the context
        /// of this AtomContainer.
        /// 
        /// </summary>
        /// <param name="atom"> The atom
        /// </param>
        /// <returns>       The maximum bond order that this atom currently has
        /// </returns>
        public virtual double getMaximumBondOrder(IAtom atom)
        {
            double max = 0.0;
            for (int i = 0; i < ElectronContainerCount; i++)
            {
                if (electronContainers[i] is IBond && ((Bond)electronContainers[i]).contains(atom) && ((Bond)electronContainers[i]).Order > max)
                {
                    max = ((Bond)electronContainers[i]).Order;
                }
            }
            return max;
        }


        /// <summary>  Returns the minimum bond order that this atom currently has in the context
        /// of this AtomContainer.
        /// 
        /// </summary>
        /// <param name="atom"> The atom
        /// </param>
        /// <returns>       The minimim bond order that this atom currently has
        /// </returns>
        public virtual double getMinimumBondOrder(IAtom atom)
        {
            double min = 6;
            for (int i = 0; i < ElectronContainerCount; i++)
            {
                if (electronContainers[i] is IBond && ((Bond)electronContainers[i]).contains(atom) && ((Bond)electronContainers[i]).Order < min)
                {
                    min = ((Bond)electronContainers[i]).Order;
                }
            }
            return min;
        }



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

        public virtual IAtomContainer getIntersection(IAtomContainer container)
        {
            IAtomContainer intersection = Builder.newAtomContainer();

            for (int i = 0; i < AtomCount; i++)
            {
                if (container.contains(getAtomAt(i)))
                {
                    intersection.addAtom(getAtomAt(i));
                }
            }
            for (int i = 0; i < ElectronContainerCount; i++)
            {
                if (container.contains(getElectronContainerAt(i)))
                {
                    intersection.addElectronContainer(getElectronContainerAt(i));
                }
            }
            return intersection;
        }

        /// <summary>  Adds the <code>ElectronContainer</code>s found in atomContainer to this
        /// container.
        /// 
        /// </summary>
        /// <param name="atomContainer"> AtomContainer with the new ElectronContainers
        /// </param>
        public virtual void addElectronContainers(IAtomContainer atomContainer)
        {
            for (int f = 0; f < atomContainer.ElectronContainerCount; f++)
            {
                if (!contains(atomContainer.getElectronContainerAt(f)))
                {
                    addElectronContainer(atomContainer.getElectronContainerAt(f));
                }
            }
            notifyChanged();
        }


        /// <summary>  Adds all atoms and electronContainers of a given atomcontainer to this
        /// container.
        /// 
        /// </summary>
        /// <param name="atomContainer"> The atomcontainer to be added
        /// </param>
        public virtual void add(IAtomContainer atomContainer)
        {
            for (int f = 0; f < atomContainer.AtomCount; f++)
            {
                if (!contains(atomContainer.getAtomAt(f)))
                {
                    addAtom(atomContainer.getAtomAt(f));
                }
            }
            for (int f = 0; f < atomContainer.ElectronContainerCount; f++)
            {
                if (!contains(atomContainer.getElectronContainerAt(f)))
                {
                    addElectronContainer(atomContainer.getElectronContainerAt(f));
                }
            }
            notifyChanged();
        }


        /// <summary>  Adds an atom to this container.
        /// 
        /// </summary>
        /// <param name="atom"> The atom to be added to this container
        /// </param>
        public virtual void addAtom(IAtom atom)
        {
            if (contains(atom))
            {
                return;
            }

            if (atomCount + 1 >= atoms_Renamed_Field.Length)
            {
                growAtomArray();
            }
            atom.addListener(this);
            atoms_Renamed_Field[atomCount] = atom;
            atomCount++;
            notifyChanged();
        }


        /// <summary>  Wrapper method for adding Bonds to this AtomContainer.
        /// 
        /// </summary>
        /// <param name="bond"> The bond to added to this container
        /// </param>
        public virtual void addBond(IBond bond)
        {
            addElectronContainer(bond);
            notifyChanged();
        }


        /// <summary>  Adds a ElectronContainer to this AtomContainer.
        /// 
        /// </summary>
        /// <param name="electronContainer"> The ElectronContainer to added to this container
        /// </param>
        public virtual void addElectronContainer(IElectronContainer electronContainer)
        {
            if (electronContainerCount + 1 >= electronContainers.Length)
            {
                growElectronContainerArray();
            }
            // are we supposed to check if the atoms forming this bond are
            // already in here and add them if neccessary? No, core classes
            // must not check parameter input.
            electronContainer.addListener(this);
            electronContainers[electronContainerCount] = electronContainer;
            electronContainerCount++;
            notifyChanged();
        }


        /// <summary>  Removes all atoms and electronContainers of a given atomcontainer from this
        /// container.
        /// 
        /// </summary>
        /// <param name="atomContainer"> The atomcontainer to be removed
        /// </param>
        public virtual void remove(IAtomContainer atomContainer)
        {
            for (int f = 0; f < atomContainer.AtomCount; f++)
            {
                removeAtom(atomContainer.getAtomAt(f));
            }
            for (int f = 0; f < atomContainer.ElectronContainerCount; f++)
            {
                removeElectronContainer(atomContainer.getElectronContainerAt(f));
            }
            notifyChanged();
        }


        /// <summary> Removes the bond at the given position from this container.
        /// 
        /// </summary>
        /// <param name="position"> The position of the bond in the electronContainers array
        /// </param>
        /// <returns>           Bond that was removed
        /// </returns>
        public virtual IElectronContainer removeElectronContainer(int position)
        {
            IElectronContainer electronContainer = getElectronContainerAt(position);
            electronContainer.removeListener(this);
            for (int i = position; i < electronContainerCount - 1; i++)
            {
                electronContainers[i] = electronContainers[i + 1];
            }
            electronContainers[electronContainerCount - 1] = null;
            electronContainerCount--;
            notifyChanged();
            return electronContainer;
        }


        /// <summary> Removes this ElectronContainer from this container.
        /// 
        /// </summary>
        /// <param name="electronContainer">   The electronContainer to be removed
        /// </param>
        /// <returns>                      Bond that was removed
        /// </returns>
        public virtual IElectronContainer removeElectronContainer(IElectronContainer electronContainer)
        {
            for (int i = ElectronContainerCount - 1; i >= 0; i--)
            {
                if (electronContainers[i].Equals(electronContainer))
                {
                    /* we don't notifyChanged here because the
                    method called below does is already  */
                    return removeElectronContainer(i);
                }
            }
            return null;
        }


        /// <summary> Removes the bond that connects the two given atoms.
        /// 
        /// </summary>
        /// <param name="atom1"> The first atom
        /// </param>
        /// <param name="atom2"> The second atom
        /// </param>
        /// <returns>        The bond that connectes the two atoms
        /// </returns>
        public virtual IBond removeBond(IAtom atom1, IAtom atom2)
        {
            IBond bond = getBond(atom1, atom2);
            if (bond != null)
                removeElectronContainer(bond);
            return bond;
        }



        /// <summary>  Removes the atom at the given position from the AtomContainer. Note that
        /// the electronContainers are unaffected: you also have to take care of
        /// removing all electronContainers to this atom from the container manually.
        /// 
        /// </summary>
        /// <param name="position"> The position of the atom to be removed.
        /// </param>
        public virtual void removeAtom(int position)
        {
            atoms_Renamed_Field[position].removeListener(this);
            for (int i = position; i < atomCount - 1; i++)
            {
                atoms_Renamed_Field[i] = atoms_Renamed_Field[i + 1];
            }
            atoms_Renamed_Field[atomCount - 1] = null;
            atomCount--;
            notifyChanged();
        }


        /// <summary>  Removes the given atom and all connected electronContainers from the
        /// AtomContainer.
        /// 
        /// </summary>
        /// <param name="atom"> The atom to be removed
        /// </param>
        public virtual void removeAtomAndConnectedElectronContainers(IAtom atom)
        {
            int position = getAtomNumber(atom);
            if (position != -1)
            {
                IElectronContainer[] electronContainers = getConnectedElectronContainers(atom);
                for (int f = 0; f < electronContainers.Length; f++)
                {
                    removeElectronContainer(electronContainers[f]);
                }
                removeAtom(position);
            }
            notifyChanged();
        }


        /// <summary>  Removes the given atom from the AtomContainer. Note that the
        /// electronContainers are unaffected: you also have to take care of removeing
        /// all electronContainers to this atom from the container.
        /// 
        /// </summary>
        /// <param name="atom"> The atom to be removed
        /// </param>
        public virtual void removeAtom(IAtom atom)
        {
            int position = getAtomNumber(atom);
            if (position != -1)
            {
                removeAtom(position);
            }
            notifyChanged();
        }


        /// <summary> Removes all atoms and bond from this container.</summary>
        public virtual void removeAllElements()
        {
            for (int f = 0; f < AtomCount; f++)
            {
                getAtomAt(f).removeListener(this);
            }
            for (int f = 0; f < ElectronContainerCount; f++)
            {
                getElectronContainerAt(f).removeListener(this);
            }
            atoms_Renamed_Field = new IAtom[growArraySize];
            electronContainers = new IElectronContainer[growArraySize];
            atomCount = 0;
            electronContainerCount = 0;
            notifyChanged();
        }


        /// <summary>  Removes electronContainers from this container.</summary>
        public virtual void removeAllElectronContainers()
        {
            for (int f = 0; f < ElectronContainerCount; f++)
            {
                getElectronContainerAt(f).removeListener(this);
            }
            electronContainers = new IElectronContainer[growArraySize];
            electronContainerCount = 0;
            notifyChanged();
        }

        /// <summary>  Removes all Bonds from this container.</summary>
        public virtual void removeAllBonds()
        {
            IBond[] bonds = Bonds;
            for (int i = 0; i < bonds.Length; i++)
            {
                removeElectronContainer(bonds[i]);
            }
            notifyChanged();
        }

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
        public virtual void addBond(int atom1, int atom2, double order, int stereo)
        {
            IBond bond = Builder.newBond(getAtomAt(atom1), getAtomAt(atom2), order, stereo);

            if (contains(bond))
            {
                return;
            }

            if (electronContainerCount >= electronContainers.Length)
            {
                growElectronContainerArray();
            }
            addBond(bond);
            /* no notifyChanged() here because addBond(bond) does 
            it already */
        }


        /// <summary>  Adds a bond to this container.
        /// 
        /// </summary>
        /// <param name="atom1"> Id of the first atom of the Bond in [0,..]
        /// </param>
        /// <param name="atom2"> Id of the second atom of the Bond in [0,..]
        /// </param>
        /// <param name="order"> Bondorder
        /// </param>
        public virtual void addBond(int atom1, int atom2, double order)
        {
            IBond bond = Builder.newBond(getAtomAt(atom1), getAtomAt(atom2), order);

            if (electronContainerCount >= electronContainers.Length)
            {
                growElectronContainerArray();
            }
            addBond(bond);
            /* no notifyChanged() here because addBond(bond) does 
            it already */
        }


        /// <summary>  Adds a LonePair to this Atom.
        /// 
        /// </summary>
        /// <param name="atomID"> The atom number to which the LonePair is added in [0,..]
        /// </param>
        public virtual void addLonePair(int atomID)
        {
            IElectronContainer lonePair = Builder.newLonePair((Atom)atoms_Renamed_Field[atomID]);
            lonePair.addListener(this);
            addElectronContainer(lonePair);
            /* no notifyChanged() here because addElectronContainer() does 
            it already */
        }


        /// <summary>  True, if the AtomContainer contains the given ElectronContainer object.
        /// 
        /// </summary>
        /// <param name="electronContainer">ElectronContainer that is searched for
        /// </param>
        /// <returns>                   True, if the AtomContainer contains the given bond object
        /// </returns>
        public virtual bool contains(IElectronContainer electronContainer)
        {
            for (int i = 0; i < ElectronContainerCount; i++)
            {
                if (electronContainer == electronContainers[i])
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>  True, if the AtomContainer contains the given atom object.
        /// 
        /// </summary>
        /// <param name="atom"> the atom this AtomContainer is searched for
        /// </param>
        /// <returns>       True, if the AtomContainer contains the given atom object
        /// </returns>
        public virtual bool contains(IAtom atom)
        {
            for (int i = 0; i < AtomCount; i++)
            {
                if (atom == atoms_Renamed_Field[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>  Returns a one line string representation of this Container. This method is
        /// conform RFC #9.
        /// 
        /// </summary>
        /// <returns>    The string representation of this Container
        /// </returns>
        public override System.String ToString()
        {
            IElectronContainer electronContainer;
            System.Text.StringBuilder stringContent = new System.Text.StringBuilder(64);
            stringContent.Append("AtomContainer(");
            stringContent.Append(this.GetHashCode());
            stringContent.Append(", #A:").Append(AtomCount);
            stringContent.Append(", #EC:").Append(ElectronContainerCount).Append(", ");
            for (int i = 0; i < AtomCount; i++)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                stringContent.Append(getAtomAt(i).ToString()).Append(", ");
            }
            for (int i = 0; i < ElectronContainerCount; i++)
            {
                electronContainer = getElectronContainerAt(i);
                // this check should be removed!
                if (electronContainer != null)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    stringContent.Append(electronContainer.ToString()).Append(", ");
                }
            }
            stringContent.Append(", AP:[#").Append(atomParities.Count).Append(", ");
            System.Collections.IEnumerator parities = atomParities.Values.GetEnumerator();
            //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
            while (parities.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
                stringContent.Append(((AtomParity)parities.Current).ToString());
                //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
                if (parities.MoveNext())
                    stringContent.Append(", ");
            }
            stringContent.Append("])");
            return stringContent.ToString();
        }


        /// <summary> Clones this AtomContainer object and its content.
        /// 
        /// </summary>
        /// <returns>    The cloned object
        /// </returns>
        /// <seealso cref="shallowCopy">
        /// </seealso>
        public override System.Object Clone()
        {
            IElectronContainer electronContainer = null;
            IElectronContainer newEC = null;
            IAtom[] natoms;
            IAtom[] newAtoms;
            IAtomContainer clone = (IAtomContainer)base.Clone();
            // start from scratch
            clone.removeAllElements();
            // clone all atoms
            for (int f = 0; f < AtomCount; f++)
            {
                clone.addAtom((Atom)getAtomAt(f).Clone());
            }
            // clone the electronContainer
            for (int f = 0; f < ElectronContainerCount; f++)
            {
                electronContainer = this.getElectronContainerAt(f);
                newEC = Builder.newElectronContainer();
                if (electronContainer is IBond)
                {
                    IBond bond = (IBond)electronContainer;
                    newEC = (IElectronContainer)bond.Clone();
                    natoms = bond.getAtoms();
                    newAtoms = new IAtom[natoms.Length];
                    for (int g = 0; g < bond.AtomCount; g++)
                    {
                        newAtoms[g] = clone.getAtomAt(getAtomNumber(natoms[g]));
                    }
                    ((IBond)newEC).setAtoms(newAtoms);
                }
                else if (electronContainer is ILonePair)
                {
                    IAtom atom = ((ILonePair)electronContainer).Atom;
                    newEC = (ILonePair)electronContainer.Clone();
                    ((ILonePair)newEC).Atom = clone.getAtomAt(getAtomNumber(atom));
                }
                else if (electronContainer is ISingleElectron)
                {
                    IAtom atom = ((ISingleElectron)electronContainer).Atom;
                    newEC = (ISingleElectron)electronContainer.Clone();
                    ((ISingleElectron)newEC).Atom = clone.getAtomAt(getAtomNumber(atom));
                }
                else
                {
                    //System.out.println("Expecting EC, got: " + electronContainer.getClass().getName());
                    newEC = (IElectronContainer)electronContainer.Clone();
                }
                clone.addElectronContainer(newEC);
            }
            return clone;
        }

        /// <summary>  Grows the ElectronContainer array by a given size.
        /// 
        /// </summary>
        /// <seealso cref="growArraySize">
        /// </seealso>
        protected internal virtual void growElectronContainerArray()
        {
            growArraySize = (electronContainers.Length < growArraySize) ? growArraySize : electronContainers.Length;
            IElectronContainer[] newelectronContainers = new IElectronContainer[electronContainers.Length + growArraySize];
            Array.Copy(electronContainers, 0, newelectronContainers, 0, electronContainers.Length);
            electronContainers = newelectronContainers;
        }


        /// <summary>  Grows the atom array by a given size.
        /// 
        /// </summary>
        /// <seealso cref="growArraySize">
        /// </seealso>
        protected internal virtual void growAtomArray()
        {
            growArraySize = (atoms_Renamed_Field.Length < growArraySize) ? growArraySize : atoms_Renamed_Field.Length;
            IAtom[] newatoms = new IAtom[atoms_Renamed_Field.Length + growArraySize];
            Array.Copy(atoms_Renamed_Field, 0, newatoms, 0, atoms_Renamed_Field.Length);
            atoms_Renamed_Field = newatoms;
        }

        /// <summary>  Called by objects to which this object has
        /// registered as a listener.
        /// 
        /// </summary>
        /// <param name="event"> A change event pointing to the source of the change
        /// </param>
        public virtual void stateChanged(IChemObjectChangeEvent event_Renamed)
        {
            notifyChanged(event_Renamed);
        }
    }
}