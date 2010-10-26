/* $RCSfile$
* $Author: shk3 $ 
* $Date: 2006-07-03 12:46:38 +0200 (Mon, 03 Jul 2006) $
* $Revision: 6562 $
* 
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
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
*  */
using System;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Exception;
using Support;

namespace Org.OpenScience.CDK.Tools.Manipulator
{
    /// <summary> Class with convenience methods that provide methods to manipulate
    /// AtomContainer's. For example:
    /// <pre>
    /// AtomContainerManipulator.replaceAtomByAtom(container, atom1, atom2);
    /// </pre>
    /// will replace the Atom in the AtomContainer, but in all the ElectronContainer's
    /// it participates too.
    /// 
    /// </summary>
    /// <cdk.module>  standard </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>   Egon Willighagen
    /// </author>
    /// <cdk.created>  2003-08-07 </cdk.created>
    public class AtomContainerManipulator
    {

        /// <summary> Returna an atom in an atomcontainer identified by id
        /// 
        /// </summary>
        /// <param name="ac">The AtomContainer to search in
        /// </param>
        /// <param name="id">The id to search for
        /// </param>
        /// <returns> An atom having id id
        /// </returns>
        /// <throws>  CDKException There is no such atom </throws>
        public static IAtom getAtomById(IAtomContainer ac, System.String id)
        {
            for (int i = 0; i < ac.AtomCount; i++)
            {
                if (ac.getAtomAt(i).ID != null && ac.getAtomAt(i).ID.Equals(id))
                    return ac.getAtomAt(i);
            }
            throw new CDKException("no suc atom");
        }

        public static bool replaceAtomByAtom(IAtomContainer container, IAtom atom, IAtom newAtom)
        {
            if (!container.contains(atom))
            {
                // it should complain
                return false;
            }
            else
            {
                container.setAtomAt(container.getAtomNumber(atom), newAtom);
                IElectronContainer[] electronContainers = container.ElectronContainers;
                for (int i = 0; i < electronContainers.Length; i++)
                {
                    if (electronContainers[i] is IBond)
                    {
                        IBond bond = (IBond)electronContainers[i];
                        if (bond.contains(atom))
                        {
                            for (int j = 0; j < bond.AtomCount; j++)
                            {
                                if (atom.Equals(bond.getAtomAt(j)))
                                {
                                    bond.setAtomAt(newAtom, j);
                                }
                            }
                        }
                    }
                    else if (electronContainers[i] is ILonePair)
                    {
                        ILonePair lonePair = (ILonePair)electronContainers[i];
                        if (atom.Equals(lonePair.Atom))
                        {
                            lonePair.Atom = newAtom;
                        }
                    }
                }
                return true;
            }
        }


        /// <returns> The summed charges of all atoms in this AtomContainer.
        /// </returns>
        public static double getTotalCharge(IAtomContainer atomContainer)
        {
            double charge = 0.0;
            for (int i = 0; i < atomContainer.AtomCount; i++)
            {
                charge += atomContainer.getAtomAt(i).getCharge();
            }
            return charge;
        }

        /// <returns> The summed formal charges of all atoms in this AtomContainer.
        /// </returns>
        public static int getTotalFormalCharge(IAtomContainer atomContainer)
        {
            int chargeP = getTotalNegativeFormalCharge(atomContainer);
            int chargeN = getTotalPositiveFormalCharge(atomContainer);
            int totalCharge = chargeP + chargeN;

            return totalCharge;
        }
        /// <returns> The summed negative formal charges of all atoms in this AtomContainer. 
        /// </returns>
        public static int getTotalNegativeFormalCharge(IAtomContainer atomContainer)
        {
            int charge = 0;
            for (int i = 0; i < atomContainer.AtomCount; i++)
            {
                double chargeI = atomContainer.getAtomAt(i).getFormalCharge();
                if (chargeI < 0)
                    charge = (int)(charge + chargeI);
            }
            return charge;
        }
        /// <returns> The summed positive formal charges of all atoms in this AtomContainer. 
        /// </returns>
        public static int getTotalPositiveFormalCharge(IAtomContainer atomContainer)
        {
            int charge = 0;
            for (int i = 0; i < atomContainer.AtomCount; i++)
            {
                double chargeI = atomContainer.getAtomAt(i).getFormalCharge();
                if (chargeI > 0)
                    charge = (int)(charge + chargeI);
            }
            return charge;
        }

        /// <returns> The summed implicit hydrogens of all atoms in this AtomContainer.
        /// </returns>
        public static int getTotalHydrogenCount(IAtomContainer atomContainer)
        {
            int hCount = 0;
            for (int i = 0; i < atomContainer.AtomCount; i++)
            {
                hCount += atomContainer.getAtomAt(i).getHydrogenCount();
            }
            return hCount;
        }

        public static System.Collections.ArrayList getAllIDs(IAtomContainer mol)
        {
            System.Collections.ArrayList idList = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            if (mol != null)
            {
                if (mol.ID != null)
                    idList.Add(mol.ID);
                IAtom[] atoms = mol.Atoms;
                for (int i = 0; i < atoms.Length; i++)
                {
                    IAtom atom = atoms[i];
                    if (atom.ID != null)
                        idList.Add(atom.ID);
                }
                IBond[] bonds = mol.Bonds;
                for (int i = 0; i < bonds.Length; i++)
                {
                    IBond bond = bonds[i];
                    if (bond.ID != null)
                        idList.Add(bond.ID);
                }
            }
            return idList;
        }


        /// <summary> Produces an AtomContainer without explicit Hs but with H count from one with Hs.
        /// The new molecule is a deep copy.
        /// 
        /// </summary>
        /// <param name="atomContainer">The AtomContainer from which to remove the hydrogens
        /// </param>
        /// <returns>              The molecule without Hs.
        /// </returns>
        /// <cdk.keyword>          hydrogen, removal </cdk.keyword>
        public static IAtomContainer removeHydrogens(IAtomContainer atomContainer)
        {
            //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
            System.Collections.IDictionary map = new System.Collections.Hashtable(); // maps original atoms to clones.
            System.Collections.IList remove = new System.Collections.ArrayList(); // lists removed Hs.

            // Clone atoms except those to be removed.
            IMolecule mol = atomContainer.Builder.newMolecule();
            int count = atomContainer.AtomCount;
            for (int i = 0; i < count; i++)
            {
                // Clone/remove this atom?
                IAtom atom = atomContainer.getAtomAt(i);
                if (!atom.Symbol.Equals("H"))
                {
                    IAtom clonedAtom = null;
                    try
                    {
                        clonedAtom = (IAtom)atom.Clone();
                    }
                    //UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
                    catch (System.Exception e)
                    {
                        // TODO Auto-generated catch block
                        SupportClass.WriteStackTrace(e, Console.Error);
                    }
                    clonedAtom.setHydrogenCount(0);
                    mol.addAtom(clonedAtom);
                    map[atom] = clonedAtom;
                }
                else
                {
                    remove.Add(atom); // maintain list of removed H.
                }
            }

            // Clone bonds except those involving removed atoms.
            count = atomContainer.getBondCount();
            for (int i = 0; i < count; i++)
            {
                // Check bond.
                //UPGRADE_NOTE: Final was removed from the declaration of 'bond '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
                IBond bond = atomContainer.getBondAt(i);
                IAtom[] atoms = bond.getAtoms();
                bool removedBond = false;
                //UPGRADE_NOTE: Final was removed from the declaration of 'length '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
                int length = atoms.Length;
                for (int k = 0; k < length; k++)
                {
                    if (remove.Contains(atoms[k]))
                    {
                        removedBond = true;
                        break;
                    }
                }

                // Clone/remove this bond?
                if (!removedBond)
                // if (!remove.contains(atoms[0]) && !remove.contains(atoms[1]))
                {
                    IBond clone = null;
                    try
                    {
                        clone = (IBond)atomContainer.getBondAt(i).Clone();
                    }
                    //UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
                    catch (System.Exception e)
                    {
                        // TODO Auto-generated catch block
                        SupportClass.WriteStackTrace(e, Console.Error);
                    }
                    clone.setAtoms(new IAtom[] { (IAtom)map[atoms[0]], (IAtom)map[atoms[1]] });
                    mol.addBond(clone);
                }
            }

            // Recompute hydrogen counts of neighbours of removed Hydrogens.
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            for (System.Collections.IEnumerator i = remove.GetEnumerator(); i.MoveNext(); )
            {
                // Process neighbours.
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                for (System.Collections.IEnumerator n = atomContainer.getConnectedAtomsVector((IAtom)i.Current).GetEnumerator(); n.MoveNext(); )
                {
                    //UPGRADE_NOTE: Final was removed from the declaration of 'neighb '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
                    //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                    IAtom neighb = (IAtom)map[n.Current];
                    neighb.setHydrogenCount(neighb.getHydrogenCount() + 1);
                }
            }
            mol.Properties = atomContainer.Properties;
            mol.Flags = atomContainer.Flags;

            return (mol);
        }

        /// <summary> Sets a property on all <code>Atom</code>s in the given container.</summary>
        public static void setAtomProperties(IAtomContainer container, System.Object propKey, System.Object propVal)
        {
            if (container != null)
            {
                IAtom[] atoms = container.Atoms;
                for (int i = 0; i < atoms.Length; i++)
                {
                    IAtom atom = atoms[i];
                    atom.setProperty(propKey, propVal);
                }
            }
        }

        /// <summary>  A method to remove ElectronContainerListeners. 
        /// ElectronContainerListeners are used to detect changes 
        /// in ElectronContainers (like bonds) and to notifiy
        /// registered Listeners in the event of a change.
        /// If an object looses interest in such changes, it should 
        /// unregister with this AtomContainer in order to improve 
        /// performance of this class.
        /// </summary>
        public static void unregisterElectronContainerListeners(IAtomContainer container)
        {
            for (int f = 0; f < container.ElectronContainerCount; f++)
            {
                container.getElectronContainerAt(f).removeListener(container);
            }
        }

        /// <summary>  A method to remove AtomListeners. 
        /// AtomListeners are used to detect changes 
        /// in Atom objects within this AtomContainer and to notifiy
        /// registered Listeners in the event of a change.
        /// If an object looses interest in such changes, it should 
        /// unregister with this AtomContainer in order to improve 
        /// performance of this class.
        /// </summary>
        public static void unregisterAtomListeners(IAtomContainer container)
        {
            for (int f = 0; f < container.AtomCount; f++)
            {
                container.getAtomAt(f).removeListener(container);
            }
        }
    }
}