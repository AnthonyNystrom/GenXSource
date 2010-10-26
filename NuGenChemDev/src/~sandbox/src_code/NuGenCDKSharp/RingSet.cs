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
    /// <summary> Maintains a set of Ring objects.
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>      ring, set of </cdk.keyword>
    [Serializable]
    public class RingSet : SetOfAtomContainers, IRingSet, ICloneable
    {

        private const long serialVersionUID = 7168431521057961434L;

        /// <summary>Flag to denote that the set is order with the largest ring first? </summary>
        public const int LARGE_FIRST = 1;
        /// <summary>Flag to denote that the set is order with the smallest ring first? </summary>
        public const int SMALL_FIRST = 2;

        /// <summary> The constructor.
        /// 
        /// </summary>
        public RingSet()
            : base()
        {
        }

        /// <summary> Checks - and returns 'true' - if a certain ring is already
        /// stored in this setOfRings.
        /// 
        /// </summary>
        /// <param name="newRing"> The ring to be tested if it is already stored here
        /// </param>
        /// <returns>     true if it is already stored
        /// </returns>
        public virtual bool ringAlreadyInSet(IRing newRing)
        {
            IRing ring;
            IBond[] bonds;
            IBond[] newBonds;
            IBond bond;
            int equalCount;
            bool equals;
            for (int f = 0; f < AtomContainerCount; f++)
            {
                equals = false;
                equalCount = 0;
                ring = (IRing)getAtomContainer(f);
                bonds = ring.Bonds;
                newBonds = newRing.Bonds;
                if (bonds.Length == newBonds.Length)
                {
                    for (int i = 0; i < bonds.Length; i++)
                    {
                        bond = newBonds[i];
                        for (int n = 0; n < bonds.Length; n++)
                        {
                            if (bond == bonds[n])
                            {
                                equals = true;
                                equalCount++;
                                break;
                            }
                        }
                        if (!equals)
                            break;
                    }
                }
                if (equalCount == bonds.Length)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary> Returns a vector of all rings that this bond is part of.
        /// 
        /// </summary>
        /// <param name="bond"> The bond to be checked
        /// </param>
        /// <returns>   A vector of all rings that this bond is part of  
        /// </returns>

        public virtual System.Collections.IList getRings(IBond bond)
        {
            System.Collections.IList rings = new System.Collections.ArrayList();
            Ring ring;
            for (int i = 0; i < AtomContainerCount; i++)
            {
                ring = (Ring)getAtomContainer(i);
                if (ring.contains(bond))
                {
                    rings.Add(ring);
                }
            }
            return rings;
        }

        /// <summary> Returns a vector of all rings that this atom is part of.
        /// 
        /// </summary>
        /// <param name="atom"> The atom to be checked
        /// </param>
        /// <returns>   A vector of all rings that this bond is part of  
        /// </returns>

        public virtual IRingSet getRings(IAtom atom)
        {
            IRingSet rings = new RingSet();
            IRing ring;
            for (int i = 0; i < AtomContainerCount; i++)
            {
                ring = (Ring)getAtomContainer(i);
                if (ring.contains(atom))
                {
                    rings.addAtomContainer(ring);
                }
            }
            return rings;
        }

        /// <summary> Returns all the rings in the RingSet that share
        /// one or more atoms with a given ring.
        /// 
        /// </summary>
        /// <param name="ring"> A ring with which all return rings must share one or more atoms
        /// </param>
        /// <returns>  All the rings that share one or more atoms with a given ring.   
        /// </returns>

        public virtual System.Collections.IList getConnectedRings(IRing ring)
        {
            System.Collections.IList connectedRings = new System.Collections.ArrayList();
            IRing tempRing;
            IAtom atom;
            for (int i = 0; i < ring.AtomCount; i++)
            {
                atom = ring.getAtomAt(i);
                for (int j = 0; j < AtomContainerCount; j++)
                {
                    tempRing = (IRing)getAtomContainer(j);
                    if (tempRing != ring && tempRing.contains(atom))
                    {
                        connectedRings.Add(tempRing);
                    }
                }
            }
            return connectedRings;
        }

        /// <summary> Adds all rings of another RingSet if they are not allready part of this ring set.
        /// 
        /// </summary>
        /// <param name="ringSet"> the ring set to be united with this one.
        /// </param>
        public virtual void add(IRingSet ringSet)
        {
            for (int f = 0; f < ringSet.AtomContainerCount; f++)
            {
                if (!contains((IRing)ringSet.getAtomContainer(f)))
                {
                    addAtomContainer(ringSet.getAtomContainer(f));
                }
            }
        }

        /// <summary> True, if at least one of the rings in the ringset cotains
        /// the given atom.
        /// 
        /// </summary>
        /// <param name="atom">Atom to check
        /// </param>
        /// <returns>      true, if the ringset contains the atom
        /// </returns>
        public virtual bool contains(IAtom atom)
        {
            for (int i = 0; i < AtomContainerCount; i++)
            {
                if (((IRing)getAtomContainer(i)).contains(atom))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool contains(IAtomContainer ring)
        {
            for (int i = 0; i < AtomContainerCount; i++)
            {
                if (ring == getAtomContainer(i))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary> Clones this <code>RingSet</code> including the Rings.
        /// 
        /// </summary>
        /// <returns>  The cloned object
        /// </returns>
        public override System.Object Clone()
        {
            RingSet clone = (RingSet)base.Clone();
            IAtomContainer[] result = AtomContainers;
            for (int i = 0; i < result.Length; i++)
            {
                clone.addAtomContainer((IAtomContainer)result[i].Clone());
            }
            return clone;
        }

        /// <summary> Returns the String representation of this RingSet.
        /// 
        /// </summary>
        /// <returns> The String representation of this RingSet
        /// </returns>
        public override System.String ToString()
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder(32);
            buffer.Append("RingSet(");
            buffer.Append(this.GetHashCode());
            buffer.Append(", R=").Append(AtomContainerCount).Append(", ");
            IAtomContainer[] rings = AtomContainers;
            for (int i = 0; i < rings.Length; i++)
            {
                IRing possibleRing = (IRing)rings[i];
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                buffer.Append(possibleRing.ToString());
                if (i++ < rings.Length)
                {
                    buffer.Append(", ");
                }
            }
            buffer.Append(')');
            return buffer.ToString();
        }
    }
}