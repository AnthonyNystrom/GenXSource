/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-04-02 21:10:27 +0200 (dim., 02 avr. 2006) $
* $Revision: 4881 $
*
* Copyright (C) 2004-2005  The Jmol Development Team
*
* Contact: jmol-developers@lists.sf.net
*
*  This library is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public
*  License as published by the Free Software Foundation; either
*  version 2.1 of the License, or (at your option) any later version.
*
*  This library is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
*  Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public
*  License along with this library; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using System.Collections;

namespace Org.Jmol.Viewer
{
	class NucleicPolymer:Polymer
	{
        public NucleicPolymer(Monomer[] monomers)
            : base(monomers)
		{ }

        public virtual Atom getNucleicPhosphorusAtom(int monomerIndex)
		{
			return monomers[monomerIndex].LeadAtom;
		}

        public override bool hasWingPoints()
		{
			return true;
		}

        public override void calcHydrogenBonds(BitArray bsA, BitArray bsB)
		{
			for (int i = model.PolymerCount; --i >= 0; )
			{
				Polymer otherPolymer = model.getPolymer(i);
				if (otherPolymer == this)
				// don't look at self
					continue;
				if (otherPolymer == null || !(otherPolymer is NucleicPolymer))
					continue;
				lookForHbonds((NucleicPolymer) otherPolymer, bsA, bsB);
			}
		}

        public virtual void lookForHbonds(NucleicPolymer other, BitArray bsA, BitArray bsB)
		{
			//System.out.println("NucleicPolymer.lookForHbonds()");
			for (int i = monomerCount; --i >= 0; )
			{
				NucleicMonomer myNucleotide = (NucleicMonomer) monomers[i];
				if (!myNucleotide.Purine)
					continue;
				Atom myN1 = myNucleotide.N1;
				Atom bestN3 = null;
				float minDist2 = 5 * 5;
				NucleicMonomer bestNucleotide = null;
				for (int j = other.monomerCount; --j >= 0; )
				{
					NucleicMonomer otherNucleotide = (NucleicMonomer) other.monomers[j];
					if (!otherNucleotide.Pyrimidine)
						continue;
					Atom otherN3 = otherNucleotide.N3;
					float dist2 = myN1.point3f.distanceSquared(otherN3.point3f);
					if (dist2 < minDist2)
					{
						bestNucleotide = otherNucleotide;
						bestN3 = otherN3;
						minDist2 = dist2;
					}
				}
				if (bestN3 != null)
				{
					createHydrogenBond(myN1, bestN3, bsA, bsB);
					if (myNucleotide.Guanine)
					{
						createHydrogenBond(myNucleotide.N2, bestNucleotide.O2, bsA, bsB);
						createHydrogenBond(myNucleotide.O6, bestNucleotide.N4, bsA, bsB);
					}
					else
					{
						createHydrogenBond(myNucleotide.N6, bestNucleotide.O4, bsA, bsB);
					}
				}
			}
		}

        public virtual void createHydrogenBond(Atom atom1, Atom atom2, BitArray bsA, BitArray bsB)
		{
			//System.out.println("createHydrogenBond:" + atom1.getAtomNumber() + "<->" + atom2.getAtomNumber());
            if (atom1 != null && atom2 != null)
            {
                Frame frame = model.mmset.frame;
                frame.bondAtoms(atom1, atom2, JmolConstants.BOND_H_NUCLEOTIDE, bsA, bsB);
            }
		}
	}
}