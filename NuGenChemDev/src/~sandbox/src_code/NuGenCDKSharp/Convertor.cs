/*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-07-14 11:39:20 +0200 (Fri, 14 Jul 2006) $
*  $Revision: 6669 $
*
*  Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
*
*  Contact: cdk-devel@lists.sourceforge.net
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public License
*  as published by the Free Software Foundation; either version 2.1
*  of the License, or (at your option) any later version.
*  All we ask is that proper credit is given for our work, which includes
*  - but is not limited to - adding the above copyright notice to the beginning
*  of your source code files, and to any copyright notice that you may distribute
*  with programs based on this work.
*
*  This program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public License
*  along with this program; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*
*/
using System;
using Org.OpenScience.CDK.Interfaces;
using Org.Jmol.Adapter.Smarter;

namespace Org.OpenScience.CDK.Libio.Jmol
{
    /// <summary> Only converts Jmol objects to CDK objects; the CdkJmolAdapter is not used
    /// right now.
    /// 
    /// </summary>
    /// <author>         egonw
    /// </author>
    /// <author>         Miguel Howard
    /// </author>
    /// <cdk.created>    2004-04-25 </cdk.created>
    /// <cdk.module>     io-jmol </cdk.module>
    /// <cdk.keyword>    adapter, Jmol </cdk.keyword>
    /// <summary> 
    /// </summary>
    /// <cdk.depends>    jmolApis.jar </cdk.depends>
    /// <cdk.depends>    jmolIO.jar </cdk.depends>
    public class Convertor
    {
        private IChemObjectBuilder builder = null;

        public Convertor(IChemObjectBuilder builder)
        {
            this.builder = builder;
        }

        /// <summary> Converts a Jmol <i>model</i> to a CDK AtomContainer.
        /// 
        /// </summary>
        /// <param name="model">A Jmol model as returned by the method ModelAdapter.openBufferedReader()
        /// </param>
        public virtual IAtomContainer convert(System.Object model)
        {
            IAtomContainer atomContainer = builder.newAtomContainer();
            SmarterJmolAdapter adapter = new SmarterJmolAdapter();
            // use this hashtable to map the ModelAdapter Unique IDs to
            // our CDK Atom's
            System.Collections.Hashtable htMapUidsToAtoms = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
            Org.Jmol.Api.JmolAdapter.AtomIterator atomIterator = adapter.getAtomIterator(model);
            while (atomIterator.hasNext())
            {
                IAtom atom = builder.newAtom(atomIterator.ElementSymbol);
                atom.X3d = atomIterator.X;
                atom.Y3d = atomIterator.Y;
                atom.Z3d = atomIterator.Z;
                htMapUidsToAtoms[atomIterator.UniqueID] = atom;
                atomContainer.addAtom(atom);
            }
            Org.Jmol.Api.JmolAdapter.BondIterator bondIterator = adapter.getBondIterator(model);
            while (bondIterator.hasNext())
            {
                System.Object uid1 = bondIterator.AtomUniqueID1;
                System.Object uid2 = bondIterator.AtomUniqueID2;
                int order = bondIterator.EncodedOrder;
                // now, look up the uids in our atom map.
                IAtom atom1 = (IAtom)htMapUidsToAtoms[uid1];
                IAtom atom2 = (IAtom)htMapUidsToAtoms[uid2];
                IBond bond = builder.newBond(atom1, atom2, (double)order);
                atomContainer.addBond(bond);
            }
            return atomContainer;
        }


        /// <summary> Empty stub to convert a CDK object into a Jmol object.
        /// 
        /// </summary>
        /// <param name="container">
        /// </param>
        /// <returns> null
        /// 
        /// </returns>
        /// <deprecated> Use a Jmol ModelAdapter instead.
        /// </deprecated>
        public virtual System.Object convert(IAtomContainer container)
        {
            // I need something like the CdkModelAdapter from Jmol here
            return null;
        }
    }
}