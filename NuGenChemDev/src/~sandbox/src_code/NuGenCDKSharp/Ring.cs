/* $RCSfile$
* $Author: egonw $    
* $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $    
* $Revision: 6672 $
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
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK
{
    /// <summary> Class representing a ring structure in a molecule.
    /// A ring is a linear sequence of
    /// N atoms interconnected to each other by covalent bonds,
    /// such that atom i (1 < i < N) is bonded to
    /// atom i-1 and atom i + 1 and atom 1 is bonded to atom N and atom 2.
    /// 
    /// </summary>
    /// <cdk.module>   data </cdk.module>
    /// <cdk.keyword>  ring </cdk.keyword>
    /// <cdk.bug>      1117765 </cdk.bug>
    [Serializable]
    public class Ring : AtomContainer, IRing
    {
        /// <summary> Returns the number of atoms\edges in this ring.
        /// 
        /// </summary>
        /// <returns>   The number of atoms\edges in this ring   
        /// </returns>
        virtual public int RingSize
        {
            get
            {
                return this.atomCount;
            }
        }
        /// <summary> Returns the sum of all bond orders in the ring.
        /// 
        /// </summary>
        /// <returns> the sum of all bond orders in the ring
        /// </returns>
        virtual public int OrderSum
        {
            get
            {
                int orderSum = 0;
                Bond tempBond;
                for (int i = 0; i < ElectronContainerCount; i++)
                {
                    IElectronContainer electronContainer = getElectronContainerAt(i);
                    if (electronContainer is IBond)
                    {
                        tempBond = (Bond)electronContainer;
                        orderSum = (int)(orderSum + tempBond.Order);
                    }
                }
                return orderSum;
            }

        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = 6604894792331865990L;

        /// <summary> Constructs an empty ring.
        /// 
        /// </summary>
        public Ring()
            : base()
        {
        }

        /// <summary> Constructs a ring from the atoms in an IAtomContainer object.
        /// 
        /// </summary>
        /// <param name="atomContainer">The IAtomContainer object containing the atoms to form the ring
        /// </param>
        public Ring(IAtomContainer atomContainer)
            : base(atomContainer)
        {
        }

        /// <summary> Constructs a ring that will have a certain number of atoms of the given elements.
        /// 
        /// </summary>
        /// <param name="ringSize">  The number of atoms and bonds the ring will have
        /// </param>
        /// <param name="elementSymbol">  The element of the atoms the ring will have
        /// </param>
        public Ring(int ringSize, System.String elementSymbol)
            : this(ringSize)
        {
            base.atomCount = ringSize;
            base.electronContainerCount = ringSize;
            atoms_Renamed_Field[0] = new Atom(elementSymbol);
            for (int i = 1; i < ringSize; i++)
            {
                atoms_Renamed_Field[i] = new Atom(elementSymbol);
                base.electronContainers[i - 1] = new Bond(atoms_Renamed_Field[i - 1], atoms_Renamed_Field[i], 1);
            }
            base.electronContainers[ringSize - 1] = new Bond(atoms_Renamed_Field[ringSize - 1], atoms_Renamed_Field[0], 1);
        }


        /// <summary> Constructs an empty ring that will have a certain size.
        /// 
        /// </summary>
        /// <param name="ringSize"> The size (number of atoms) the ring will have
        /// </param>

        public Ring(int ringSize)
            : base(ringSize, ringSize)
        {
        }


        /// <summary> Returns the next bond in order, relative to a given bond and atom.
        /// Example: Let the ring be composed of 0-1, 1-2, 2-3 and 3-0. A request getNextBond(1-2, 2)
        /// will return Bond 2-3.
        /// 
        /// </summary>
        /// <param name="bond"> A bond for which an atom from a consecutive bond is sought
        /// </param>
        /// <param name="atom"> A atom from the bond above to assign a search direction
        /// </param>
        /// <returns>  The next bond in the order given by the above assignment   
        /// </returns>
        public virtual IBond getNextBond(IBond bond, IAtom atom)
        {
            Bond tempBond;
            for (int f = 0; f < ElectronContainerCount; f++)
            {
                IElectronContainer electronContainer = getElectronContainerAt(f);
                if (electronContainer is IBond)
                {
                    tempBond = (Bond)electronContainer;
                    if (tempBond.contains(atom) && bond != tempBond)
                    {
                        return tempBond;
                    }
                }
            }
            return null;
        }

        public override System.String ToString()
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder();
            buffer.Append("Ring(");
            buffer.Append(base.ToString());
            buffer.Append(')');
            return buffer.ToString();
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
        virtual public System.Object Clone()
        {
            return null;
        }
    }
}