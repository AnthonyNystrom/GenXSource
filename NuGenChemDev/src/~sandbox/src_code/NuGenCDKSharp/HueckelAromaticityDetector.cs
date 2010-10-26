/*  $RCSfile: $
*  $Author: egonw $
*  $Date: 2006-05-03 23:24:28 +0200 (Wed, 03 May 2006) $
*  $Revision: 6152 $
*
*  Copyright (C) 2002-2006  The Chemistry Development Kit (CDK) project
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
*/
using System;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.RingSearch;
using Org.OpenScience.CDK.Tools.Manipulator;

namespace Org.OpenScience.CDK.Aromaticity
{
    /// <summary> The HueckelAromaticityDetector detects the aromaticity based on the Hueckel
    /// 4n+2 pi-electrons Rule. This is done by one of the detectAromaticity
    /// methods. They set the aromaticity flags of appropriate Atoms, Bonds and
    /// Rings. After the detection, you can use getFlag(CDKConstants.ISAROMATIC) on
    /// these ChemObjects.
    /// 
    /// </summary>
    /// <author>          steinbeck
    /// </author>
    /// <author>          kaihartmann
    /// </author>
    /// <cdk.module>      standard </cdk.module>
    /// <cdk.created>     2001-09-04 </cdk.created>
    /// <summary> 
    /// </summary>
    /// <seealso cref="org.openscience.cdk.CDKConstants">
    /// </seealso>
    public class HueckelAromaticityDetector
    {
        public static IRingSet RingSet
        {
            get
            {
                return ringSet;
            }

        }
        /// <summary>  This method sets the aromaticity flags for a RingSet from the Atom flags.
        /// It can be used after the aromaticity detection to set the appropriate flags
        /// for a RingSet from the SSSR search.
        /// 
        /// </summary>
        /// <param name="ringset"> the RingSet to set the flags for
        /// </param>
        public static IRingSet RingFlags
        {
            set
            {
                for (int i = 0; i < value.AtomContainerCount; i++)
                {
                    bool aromatic = true;
                    IRing ring = (IRing)value.getAtomContainer(i);
                    for (int j = 0; j < ring.AtomCount; j++)
                    {
                        if (ring.getAtomAt(j).getFlag(CDKConstants.ISAROMATIC) != true)
                        {
                            aromatic = false;
                            break;
                        }
                    }
                    if (aromatic)
                    {
                        ring.setFlag(CDKConstants.ISAROMATIC, true);
                    }
                    else
                    {
                        ring.setFlag(CDKConstants.ISAROMATIC, false);
                    }
                }
            }

        }
        /// <summary>  Sets the current AllRingsFinder instance Use this if you want to customize
        /// the timeout for the AllRingsFinder. AllRingsFinder is stopping its quest to
        /// find all rings after a default of 5 seconds.
        /// 
        /// </summary>
        /// <param name="ringFinder"> The value to assign ringFinder.
        /// </param>
        /// <seealso cref="org.openscience.cdk.ringsearch.AllRingsFinder">
        /// </seealso>
        virtual public AllRingsFinder RingFinder
        {
            set
            {
                this.ringFinder = value;
            }

        }
        public static long Timeout
        {
            set
            {
                HueckelAromaticityDetector.timeout = value;
            }

            /*
            *  public static boolean isAromatic(AtomContainer ac, Ring ring)
            *  {
            *  return AromaticityCalculator.isAromatic(ring, ac);
            *  }
            *  *	public static boolean isAromatic(AtomContainer ac, Ring ring)
            *  {
            *  int piElectronCount = 0;
            *  int freeElectronPairCount = 0;
            *  Atom atom = null;
            *  Bond bond = null;
            *  int aromaCounter = 0;
            *  if (debug) System.out.println("isAromatic() -> ring.size(): " + ring.getAtomCount());
            *  for (int g = 0; g < ring.getAtomCount(); g++)
            *  {
            *  atom = ring.getAtomAt(g);
            *  if ("O-N-S-P".indexOf(atom.getSymbol()) > -1)
            *  {
            *  freeElectronPairCount += 1;
            *  }
            *  if (atom.getFlag(CDKConstants.ISAROMATIC))
            *  {
            *  aromaCounter ++;
            *  }
            *  }
            *  for (int g = 0; g < ring.getElectronContainerCount(); g++) {
            *  ElectronContainer ec = ring.getElectronContainerAt(g);
            *  if (ec instanceof org.openscience.cdk.interfaces.Bond) {
            *  bond = (Bond)ec;
            *  if (bond.getOrder() > 1) {
            *  piElectronCount += 2*(bond.getOrder()-1);
            *  }
            *  }
            *  }
            *  for (int f = 0; f < ((ring.getAtomCount() - 2)/4) + 2; f ++)
            *  {
            *  if (debug) System.out.println("isAromatic() -> freeElectronPairCount: " + freeElectronPairCount);
            *  if (debug) System.out.println("isAromatic() -> piElectronCount: " + piElectronCount);
            *  if (debug) System.out.println("isAromatic() -> f: " + f);
            *  if (debug) System.out.println("isAromatic() -> (4 * f) + 2: " + ((4 * f) + 2));
            *  if (debug) System.out.println("isAromatic() -> ring.size(): " + ring.getAtomCount());
            *  if (debug) System.out.println("isAromatic() -> aromaCounter: " + aromaCounter);
            *  if (aromaCounter == ring.getAtomCount()) return true;
            *  else if ((piElectronCount == ring.getAtomCount())&&((4 * f) + 2 == piElectronCount)) return true;
            *  else if ((4 * f) + 2 == piElectronCount + (freeElectronPairCount * 2) && ring.getAtomCount() < piElectronCount + (freeElectronPairCount * 2)) return true;
            *  }
            *  return false;
            *  }
            */

        }

        //UPGRADE_NOTE: The initialization of  '//logger' was moved to static method 'org.openscience.cdk.aromaticity.HueckelAromaticityDetector'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        //internal static LoggingTool //logger;
        internal AllRingsFinder ringFinder = null;
        internal static IRingSet ringSet = null;
        internal static long timeout = 5000;

        /// <summary> Retrieves the set of all rings and performs an aromaticity detection based
        /// on Hueckels 4n+2 rule.
        /// 
        /// </summary>
        /// <param name="atomContainer		AtomContainer">to detect rings in
        /// </param>
        /// <returns>                      True if the molecule has aromatic features
        /// </returns>
        /// <exception cref="CDKException">	Thrown if something goes wrong or in
        /// case of a AllRingsFinder timeout 
        /// </exception>
        public static bool detectAromaticity(IAtomContainer atomContainer)
        {
            return (detectAromaticity(atomContainer, true));
        }


        /// <summary> Uses precomputed set of ALL rings and performs an aromaticity detection
        /// based on Hueckels 4n+2 rule.
        /// 
        /// </summary>
        /// <param name="ringSet		"> set of ALL rings
        /// </param>
        /// <param name="atomContainer"> The AtomContainer to detect rings in
        /// </param>
        /// <returns>                True if molecule has aromatic features
        /// </returns>
        /// <exception cref="org.openscience.cdk.exception.CDKException"> 
        /// </exception>
        public static bool detectAromaticity(IAtomContainer atomContainer, IRingSet ringSet)
        {
            return (detectAromaticity(atomContainer, ringSet, true));
        }


        /// <summary>  Retrieves the set of all rings and performs an aromaticity detection based
        /// on Hueckels 4n + 2 rule.
        /// 
        /// </summary>
        /// <param name="removeAromatictyFlags"> When true, we leaves ChemObjects that 
        /// are already marked as aromatic as they are
        /// </param>
        /// <param name="atomContainer">         AtomContainer to be searched for
        /// rings
        /// </param>
        /// <returns>			True, if molecule has aromatic features                               	 
        /// </returns>
        /// <exception cref="CDKException"> 	Thrown in case of errors or an 
        /// AllRingsFinder timeout
        /// </exception>
        public static bool detectAromaticity(IAtomContainer atomContainer, bool removeAromatictyFlags)
        {
            return detectAromaticity(atomContainer, removeAromatictyFlags, null);
        }


        /// <summary>  Retrieves the set of all rings and performs an aromaticity detection based
        /// on Hueckels 4n + 2 rule. An AllRingsFinder with customized timeout may be
        /// assigned to this method.
        /// </summary>
        /// <param name="removeAromatictyFlags"> When true, we leaves ChemObjects that 
        /// are already marked as aromatic as they are
        /// </param>
        /// <param name="atomContainer">         AtomContainer to be searched for
        /// </param>
        /// <param name="arf">                   AllRingsFinder to be employed for the
        /// ringsearch. Use this to customize the 
        /// AllRingsFinder timeout feature
        /// rings
        /// </param>
        /// <returns>			True, if molecule has aromatic features                               	 
        /// </returns>
        /// <exception cref="CDKException"> 	Thrown in case of errors or an 
        /// AllRingsFinder timeout
        /// </exception>
        public static bool detectAromaticity(IAtomContainer atomContainer, bool removeAromatictyFlags, AllRingsFinder arf)
        {
            //logger.debug("Entered Aromaticity Detection");
            //logger.debug("Starting AllRingsFinder");
            long before = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
            if (arf == null)
            {
                arf = new AllRingsFinder();
                arf.setTimeout(timeout);
            }
            ringSet = arf.findAllRings(atomContainer);
            long after = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
            //logger.debug("time for finding all rings: " + (after - before) + " milliseconds");
            //logger.debug("Finished AllRingsFinder");
            if (ringSet.AtomContainerCount > 0)
            {
                return detectAromaticity(atomContainer, ringSet, removeAromatictyFlags);
            }
            return false;
        }


        /// <summary>  Uses precomputed set of ALL rings and performs an aromaticity detection
        /// based on Hueckels 4n + 2 rule.
        /// 
        /// </summary>
        /// <param name="ringSet">                set of ALL rings
        /// </param>
        /// <param name="removeAromaticityFlags"> Leaves ChemObjects that are already marked as
        /// aromatic as they are
        /// </param>
        /// <param name="atomContainer">          AtomContainer to be searched for rings
        /// </param>
        /// <returns>                         True, if molecules contains an
        /// aromatic feature
        /// </returns>
        public static bool detectAromaticity(IAtomContainer atomContainer, IRingSet ringSet, bool removeAromaticityFlags)
        {
            bool foundSomething = false;
            if (removeAromaticityFlags)
            {
                for (int f = 0; f < atomContainer.AtomCount; f++)
                {
                    atomContainer.getAtomAt(f).setFlag(CDKConstants.ISAROMATIC, false);
                }
                for (int f = 0; f < atomContainer.ElectronContainerCount; f++)
                {
                    IElectronContainer electronContainer = atomContainer.getElectronContainerAt(f);
                    if (electronContainer is IBond)
                    {
                        electronContainer.setFlag(CDKConstants.ISAROMATIC, false);
                    }
                }
                for (int f = 0; f < ringSet.AtomContainerCount; f++)
                {
                    ((IRing)ringSet.getAtomContainer(f)).setFlag(CDKConstants.ISAROMATIC, false);
                }
            }

            IRing ring = null;
            RingSetManipulator.sort(ringSet);
            for (int f = 0; f < ringSet.AtomContainerCount; f++)
            {
                ring = (IRing)ringSet.getAtomContainer(f);
                //logger.debug("Testing for aromaticity in ring no ", f);
                if (AromaticityCalculator.isAromatic(ring, atomContainer))
                {
                    ring.setFlag(CDKConstants.ISAROMATIC, true);

                    for (int g = 0; g < ring.AtomCount; g++)
                    {
                        ring.getAtomAt(g).setFlag(CDKConstants.ISAROMATIC, true);
                    }

                    for (int g = 0; g < ring.ElectronContainerCount; g++)
                    {
                        IElectronContainer electronContainer = ring.getElectronContainerAt(g);
                        if (electronContainer is IBond)
                        {
                            electronContainer.setFlag(CDKConstants.ISAROMATIC, true);
                        }
                    }

                    foundSomething = true;
                    //logger.debug("This ring is aromatic: ", f);
                }
                else
                {
                    //logger.debug("This ring is *not* aromatic: ", f);
                }
            }
            return foundSomething;
        }
        static HueckelAromaticityDetector()
        {
            //logger = new LoggingTool(typeof(HueckelAromaticityDetector));
        }
    }
}