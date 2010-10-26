/* $RCSfile$
* $Author: egonw $
* $Date: 2006-05-03 23:24:28 +0200 (Wed, 03 May 2006) $
* $Revision: 6152 $
* 
* Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
* 
* Contact: cdk-devel@lists.sourceforge.net
* 
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public License
* as published by the Free Software Foundation; either version 2.1
* of the License, or (at your option) any later version.
* All we ask is that proper credit is given for our work, which includes
* - but is not limited to - adding the above copyright notice to the beginning
* of your source code files, and to any copyright notice that you may distribute
* with programs based on this work.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA. 
* 
*/
using System;
using NuGenCDKSharp;
using org._3pq.jgrapht;
using Org.OpenScience.CDK.Graph;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Ringsearch.Cyclebasis;
using Support;

namespace Org.OpenScience.CDK.RingSearch
{
    /// <summary> Finds the Smallest Set of Smallest Rings. 
    /// This is an implementation of an algorithm 
    /// by Franziska Berger, Peter Gritzmann, and Sven deVries, TU M&uuml;nchen,
    /// {@cdk.cite BGdV04a}.
    /// 
    /// Additional related algorithms from {@cdk.cite BGdV04b}.
    /// 
    /// </summary>
    /// <author>  Ulrich Bauer <baueru@cs.tum.edu>
    /// 
    /// </author>
    /// <cdk.module>  standard </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  smallest-set-of-rings </cdk.keyword>
    /// <cdk.keyword>  ring search </cdk.keyword>
    /// <cdk.dictref>  blue-obelisk:findSmallestSetOfSmallestRings_Berger </cdk.dictref>
    /// <summary> 
    /// </summary>
    /// <cdk.builddepends>  jgrapht-0.5.3.jar </cdk.builddepends>
    /// <cdk.depends>  jgrapht-0.5.3.jar </cdk.depends>

    public class SSSRFinder
    {
        /// <summary> Returns a vector containing the lengths of the rings in a SSSR.
        /// The vector is uniquely defined for any SSSR of a molecule.
        /// 
        /// </summary>
        /// <returns> An <code>int[]</code> containing the length of the rings in a SSSR
        /// </returns>
        virtual public int[] SSSRWeightVector
        {
            get
            {
                return cycleBasis().weightVector();
            }

        }
        /// <summary> Returns a vector containing the size of the "interchangeability" equivalence classes.
        /// The vector is uniquely defined for any SSSR of a molecule.
        /// 
        /// </summary>
        /// <returns> An <code>int[]</code> containing the size of the equivalence classes in a SSSR
        /// </returns>
        virtual public int[] EquivalenceClassesSizeVector
        {
            get
            {
                System.Collections.IList equivalenceClasses = cycleBasis().equivalenceClasses();
                int[] result = new int[equivalenceClasses.Count];
                for (int i = 0; i < equivalenceClasses.Count; i++)
                {
                    result[i] = ((System.Collections.ICollection)equivalenceClasses[i]).Count;
                }
                return result;
            }

        }

        private IAtomContainer atomContainer;
        private CycleBasis.CycleBasis cycleBasis_Renamed_Field;

        /// <summary> Constructs a SSSRFinder.
        /// 
        /// </summary>
        /// <deprecated> Replaced by {@link #SSSRFinder(IAtomContainer)}
        /// </deprecated>
        public SSSRFinder()
        {
        }

        /// <summary> Constructs a SSSRFinder for a specified molecule.
        /// 
        /// </summary>
        /// <param name="ac">the molecule to be searched for rings 
        /// </param>
        public SSSRFinder(IAtomContainer ac)
        {
            this.atomContainer = ac;
        }

        /// <summary> Finds a Smallest Set of Smallest Rings.
        /// The returned set is not uniquely defined.
        /// 
        /// </summary>
        /// <returns>      a RingSet containing the SSSR   
        /// </returns>
        public virtual IRingSet findSSSR()
        {
            if (atomContainer == null)
            {
                return null;
            }
            IRingSet ringSet = toRingSet(atomContainer, cycleBasis().cycles());
            atomContainer.setProperty(CDKConstants.SMALLEST_RINGS, ringSet);
            return toRingSet(atomContainer, cycleBasis().cycles());
        }

        /// <summary> Finds the Set of Essential Rings.
        /// These rings are contained in every possible SSSR.
        /// The returned set is uniquely defined.
        /// 
        /// </summary>
        /// <returns>      a RingSet containing the Essential Rings
        /// </returns>
        public virtual IRingSet findEssentialRings()
        {
            if (atomContainer == null)
            {
                return null;
            }
            IRingSet ringSet = toRingSet(atomContainer, cycleBasis().cycles());
            atomContainer.setProperty(CDKConstants.ESSENTIAL_RINGS, ringSet);

            return toRingSet(atomContainer, cycleBasis().essentialCycles());
        }

        /// <summary> Finds the Set of Relevant Rings.
        /// These rings are contained in everry possible SSSR.
        /// The returned set is uniquely defined.
        /// 
        /// </summary>
        /// <returns>      a RingSet containing the Relevant Rings
        /// </returns>
        public virtual IRingSet findRelevantRings()
        {
            if (atomContainer == null)
            {
                return null;
            }

            IRingSet ringSet = toRingSet(atomContainer, cycleBasis().cycles());
            atomContainer.setProperty(CDKConstants.RELEVANT_RINGS, ringSet);

            //UPGRADE_TODO: Method 'java.util.Map.keySet' was converted to 'CSGraphT.SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilMapkeySet'"
            return toRingSet(atomContainer, new CSGraphT.SupportClass.HashSetSupport(cycleBasis().relevantCycles().Keys));
        }

        /// <summary> Finds the "interchangeability" equivalence classes.
        /// The interchangeability relation is described in [GLS00].
        /// 
        /// </summary>
        /// <returns>      a List of RingSets containing the rings in an equivalence class    
        /// </returns>
        public virtual System.Collections.IList findEquivalenceClasses()
        {
            if (atomContainer == null)
            {
                return null;
            }

            System.Collections.IList equivalenceClasses = new System.Collections.ArrayList();
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            for (System.Collections.IEnumerator i = cycleBasis().equivalenceClasses().GetEnumerator(); i.MoveNext(); )
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                equivalenceClasses.Add(toRingSet(atomContainer, (System.Collections.ICollection)i.Current));
            }

            return equivalenceClasses;
        }



        /// <summary> Finds a Smallest Set of Smallest Rings.
        /// The returned set is not uniquely defined.
        /// 
        /// </summary>
        /// <deprecated> replaced by {@link #findSSSR()}
        /// </deprecated>
        /// <param name="ac">the molecule to be searched for rings 
        /// </param>
        /// <returns>      a RingSet containing the SSSR
        /// </returns>
        static public IRingSet findSSSR(IAtomContainer ac)
        {
            UndirectedGraph molGraph = MoleculeGraphs.getMoleculeGraph(ac);

            CycleBasis.CycleBasis cycleBasis = new CycleBasis.CycleBasis(molGraph);

            return toRingSet(ac, cycleBasis.cycles());
        }

        private CycleBasis.CycleBasis cycleBasis()
        {
            if (cycleBasis_Renamed_Field == null)
            {
                UndirectedGraph molGraph = MoleculeGraphs.getMoleculeGraph(atomContainer);

                cycleBasis_Renamed_Field = new CycleBasis.CycleBasis(molGraph);
            }
            return cycleBasis_Renamed_Field;
        }

        private static IRingSet toRingSet(IAtomContainer ac, System.Collections.ICollection cycles)
        {

            IRingSet ringSet = ac.Builder.newRingSet();

            System.Collections.IEnumerator cycleIterator = cycles.GetEnumerator();

            while (cycleIterator.MoveNext())
            {
                SimpleCycle cycle = (SimpleCycle)cycleIterator.Current;

                IRing ring = ac.Builder.newRing();

                System.Collections.IList vertices = cycle.vertexList();

                IAtom[] atoms = new IAtom[vertices.Count];
                atoms[0] = (IAtom)vertices[0];
                for (int i = 1; i < vertices.Count; i++)
                {
                    atoms[i] = (IAtom)vertices[i];
                    ring.addElectronContainer(ac.getBond(atoms[i - 1], atoms[i]));
                }
                ring.addElectronContainer(ac.getBond(atoms[vertices.Count - 1], atoms[0]));
                ring.Atoms = atoms;

                ringSet.addAtomContainer(ring);
            }

            return ringSet;
        }
    }
}