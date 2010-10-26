/* $RCSfile$
* $Author: rajarshi $
* $Date: 2006-06-10 17:34:12 +0200 (Sat, 10 Jun 2006) $
* $Revision: 6420 $
*
* Copyright (C) 2003  The Jmol project
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
* 
* Contact: cdk-devel@lists.sourceforge.net
* 
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public License
* as published by the Free Software Foundation; either version 2.1
* of the License, or (at your option) any later version.
* All we ask is that proper credit is given for our work, which includes
* - but is not limited to - adding the above copyright notice to the
* beginning of your source code files, and to any copyright notice that
* you may distribute with programs based on this work.
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
using Org.OpenScience.CDK.Exception;

namespace Org.OpenScience.CDK.Graph.Rebond
{
    /// <summary> Provides tools to rebond a molecule from 3D coordinates only.
    /// The algorithm uses an efficient algorithm using a
    /// Binary Space Partitioning Tree (Bspt). It requires that the 
    /// atom types are configured such that the covalent bond radii
    /// for all atoms are set. The AtomTypeFactory can be used for this.
    /// 
    /// </summary>
    /// <cdk.keyword>  rebonding </cdk.keyword>
    /// <cdk.keyword>  bond, recalculation </cdk.keyword>
    /// <cdk.dictref>  blue-obelisk:rebondFrom3DCoordinates </cdk.dictref>
    /// <summary> 
    /// </summary>
    /// <author>       Miguel Howard
    /// </author>
    /// <cdk.created>  2003-05-23 </cdk.created>
    /// <cdk.module>   standard </cdk.module>
    /// <summary> 
    /// </summary>
    /// <seealso cref="org.openscience.cdk.graph.rebond.Bspt">
    /// </seealso>
    public class RebondTool
    {
        private double maxCovalentRadius;
        private double minBondDistance;
        private double bondTolerance;

        private Bspt bspt;

        public RebondTool(double maxCovalentRadius, double minBondDistance, double bondTolerance)
        {
            this.maxCovalentRadius = maxCovalentRadius;
            this.bondTolerance = bondTolerance;
            this.minBondDistance = minBondDistance;
            this.bspt = null;
        }

        /// <summary> Rebonding using a Binary Space Partition Tree. Note, that any bonds
        /// defined will be deleted first. It assumes the unit of 3D space to
        /// be 1 &Acircle;ngstrom.
        /// </summary>
        public virtual void rebond(IAtomContainer container)
        {
            container.removeAllBonds();
            maxCovalentRadius = 0.0;
            // construct a new binary space partition tree
            bspt = new Bspt(3);
            IAtom[] atoms = container.Atoms;
            for (int i = atoms.Length; --i >= 0; )
            {
                IAtom atom = atoms[i];
                double myCovalentRadius = atom.CovalentRadius;
                if (myCovalentRadius == 0.0)
                {
                    //throw new CDKException("Atom(s) does not have covalentRadius defined.");
                }
                else
                {
                    if (myCovalentRadius > maxCovalentRadius)
                        maxCovalentRadius = myCovalentRadius;
                    TupleAtom tupleAtom = new TupleAtom(atom);
                    bspt.addTuple(tupleAtom);
                }
            }
            // rebond all atoms
            for (int i = atoms.Length; --i >= 0; )
            {
                bondAtom(container, atoms[i]);
            }
        }

        /// <summary>
        /// Rebonds one atom by looking up nearby atom using the binary space partition tree.
        /// </summary>
        private void bondAtom(IAtomContainer container, IAtom atom)
        {
            double myCovalentRadius = atom.CovalentRadius;
            double searchRadius = myCovalentRadius + maxCovalentRadius + bondTolerance;
            Point tupleAtom = new Point(atom.X3d, atom.Y3d, atom.Z3d);
            Bspt.EnumerateSphere e = bspt.enumHemiSphere(tupleAtom, searchRadius);
            while (e.MoveNext())
            {
                IAtom atomNear = ((TupleAtom)e.Current).Atom;
                if (atomNear != atom && container.getBond(atom, atomNear) == null)
                {
                    bool isBonded_ = isBonded(myCovalentRadius, atomNear.CovalentRadius, e.foundDistance2());
                    if (isBonded_)
                    {
                        IBond bond = atom.Builder.newBond(atom, atomNear, 1.0);
                        container.addBond(bond);
                    }
                }
            }
        }

        /// <summary> Returns the bond order for the bond. At this moment, it only returns
        /// 0 or 1, but not 2 or 3, or aromatic bond order.
        /// </summary>
        private bool isBonded(double covalentRadiusA, double covalentRadiusB, double distance2)
        {
            double maxAcceptable = covalentRadiusA + covalentRadiusB + bondTolerance;
            double maxAcceptable2 = maxAcceptable * maxAcceptable;
            double minBondDistance2 = this.minBondDistance * this.minBondDistance;
            if (distance2 < minBondDistance2)
                return false;
            return distance2 <= maxAcceptable2;
        }

        internal class TupleAtom : Bspt.Tuple
        {
            IAtom atom;

            public IAtom Atom
            {
                get { return this.atom; }
            }

            public TupleAtom(IAtom atom)
            {
                this.atom = atom;
            }

            public virtual double getDimValue(int dim)
            {
                if (dim == 0)
                    return atom.X3d;
                if (dim == 1)
                    return atom.Y3d;
                return atom.Z3d;
            }

            public override System.String ToString()
            {
                return ("<" + atom.X3d + "," + atom.Y3d + "," + atom.Z3d + ">");
            }
        }
    }
}