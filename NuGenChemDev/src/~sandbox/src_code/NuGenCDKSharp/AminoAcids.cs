/* $RCSfile$
* $Author: egonw $    
* $Date: 2006-05-12 14:29:31 +0200 (Fri, 12 May 2006) $    
* $Revision: 6249 $
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
using Org.OpenScience.CDK.IO;
using System.Reflection;
using Org.OpenScience.CDK.Tools.Manipulator;
using Org.OpenScience.CDK.Dict;

namespace Org.OpenScience.CDK.Templates
{
    /// <summary> Tool that provides templates for the (natural) amino acids.
    /// 
    /// </summary>
    /// <author>       Martin Eklund <martin.eklund@farmbio.uu.se>
    /// </author>
    /// <cdk.module>   pdb </cdk.module>
    /// <cdk.keyword>  templates </cdk.keyword>
    /// <cdk.keyword>  amino acids, stuctures </cdk.keyword>
    /// <cdk.created>  2005-02-08 </cdk.created>
    public class AminoAcids
    {
        /// <summary> Returns a HashMap where the key is one of G, A, V, L, I, S, T, C, M, D,
        /// N, E, Q, R, K, H, F, Y, W and P.
        /// </summary>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static System.Collections.Hashtable HashMapBySingleCharCode
        {
            get
            {
                AminoAcid[] monomers = createAAs();
                //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
                System.Collections.Hashtable map = new System.Collections.Hashtable();
                for (int i = 0; i < monomers.Length; i++)
                {
                    map[monomers[i].getProperty(RESIDUE_NAME_SHORT)] = monomers[i];
                }
                return map;
            }

        }
        /// <summary> Returns a HashMap where the key is one of GLY, ALA, VAL, LEU, ILE, SER,
        /// THR, CYS, MET, ASP, ASN, GLU, GLN, ARG, LYS, HIS, PHE, TYR, TRP AND PRO.
        /// </summary>
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static System.Collections.Hashtable HashMapByThreeLetterCode
        {
            get
            {
                AminoAcid[] monomers = createAAs();
                //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
                System.Collections.Hashtable map = new System.Collections.Hashtable();
                for (int i = 0; i < monomers.Length; i++)
                {
                    map[monomers[i].getProperty(RESIDUE_NAME)] = monomers[i];
                }
                return map;
            }

        }

        //UPGRADE_NOTE: Final was removed from the declaration of '//logger '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        //UPGRADE_NOTE: The initialization of  '//logger' was moved to static method 'org.openscience.cdk.templates.AminoAcids'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        //private static readonly LoggingTool //logger;

        /// <summary> Creates matrix with info about the bonds in the amino acids.
        /// 0 = bond id, 1 = atom1 in bond, 2 = atom2 in bond, 3 = bond order.
        /// </summary>
        /// <returns> info
        /// </returns>
        public static int[][] aaBondInfo()
        {

            if (aminoAcids == null)
            {
                createAAs();
            }

            int[][] info = new int[153][];
            for (int i = 0; i < 153; i++)
            {
                info[i] = new int[4];
            }

            int counter = 0;
            int total = 0;
            for (int aa = 0; aa < aminoAcids.Length; aa++)
            {
                AminoAcid acid = aminoAcids[aa];
                IBond[] bonds = acid.Bonds;
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                //logger.debug("#bonds for ", acid.getProperty(RESIDUE_NAME).ToString(), " = " + bonds.Length);
                total += bonds.Length;
                //logger.debug("total #bonds: ", total);
                for (int bCounter = 0; bCounter < bonds.Length; bCounter++)
                {
                    info[counter][0] = counter;
                    info[counter][1] = acid.getAtomNumber(bonds[bCounter].getAtomAt(0));
                    info[counter][2] = acid.getAtomNumber(bonds[bCounter].getAtomAt(1));
                    //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                    info[counter][3] = (int)bonds[bCounter].Order;
                    counter++;
                }
            }

            if (counter > 153)
            {
                //logger.error("Error while creating AA info! Bond count is too large: ", counter);
                return null;
            }

            return info;
        }

        private static AminoAcid[] aminoAcids = null;

        public const System.String RESIDUE_NAME = "residueName";
        public const System.String RESIDUE_NAME_SHORT = "residueNameShort";
        public const System.String NO_ATOMS = "noOfAtoms";
        public const System.String NO_BONDS = "noOfBonds";
        public const System.String ID = "id";

        /// <summary> Creates amino acid AminoAcid objects.
        /// 
        /// </summary>
        /// <returns> aminoAcids, a HashMap containing the amino acids as AminoAcids.
        /// </returns>
        public static AminoAcid[] createAAs()
        {
            if (aminoAcids != null)
            {
                return aminoAcids;
            }

            // Create set of AtomContainers
            aminoAcids = new AminoAcid[20];

            IChemFile list = new ChemFile();
            //UPGRADE_ISSUE: Method 'java.lang.ClassLoader.getResourceAsStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassLoader'"
            //UPGRADE_ISSUE: Method 'java.lang.Class.getClassLoader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetClassLoader'"
            CMLReader reader = new CMLReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("NuGenCDKSharp." + "list_aminoacids.cml"));//typeof(AminoAcids).getClassLoader().getResourceAsStream("data/templates/list_aminoacids.cml"));
            try
            {
                list = (IChemFile)reader.read(list);
                IAtomContainer[] containers = ChemFileManipulator.getAllAtomContainers(list);
                for (int i = 0; i < containers.Length; i++)
                {
                    //logger.debug("Adding AA: ", containers[i]);
                    // convert into an AminoAcid
                    AminoAcid aminoAcid = new AminoAcid();
                    IAtom[] atoms = containers[i].Atoms;
                    System.Collections.IEnumerator props = containers[i].Properties.Keys.GetEnumerator();
                    //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
                    while (props.MoveNext())
                    {
                        //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
                        System.Object next = props.Current;
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        //logger.debug("Prop class: " + next.GetType().FullName);
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        //logger.debug("Prop: " + next.ToString());
                        if (next is DictRef)
                        {
                            DictRef dictRef = (DictRef)next;
                            // System.out.println("DictRef type: " + dictRef.getType());
                            if (dictRef.Type.Equals("pdb:residueName"))
                            {
                                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                                aminoAcid.setProperty(RESIDUE_NAME, containers[i].getProperty(dictRef).ToString().ToUpper());
                            }
                            else if (dictRef.Type.Equals("pdb:oneLetterCode"))
                            {
                                aminoAcid.setProperty(RESIDUE_NAME_SHORT, containers[i].getProperty(dictRef));
                            }
                            else if (dictRef.Type.Equals("pdb:id"))
                            {
                                aminoAcid.setProperty(ID, containers[i].getProperty(dictRef));
                                //logger.debug("Set AA ID to: ", containers[i].getProperty(dictRef));
                            }
                            else
                            {
                                //logger.error("Cannot deal with dictRef!");
                            }
                        }
                    }
                    for (int atomCount = 0; atomCount < atoms.Length; atomCount++)
                    {
                        IAtom atom = atoms[atomCount];
                        System.String dictRef = (System.String)atom.getProperty("org.openscience.cdk.dict");
                        if (dictRef != null && dictRef.Equals("pdb:nTerminus"))
                        {
                            aminoAcid.addNTerminus(atom);
                        }
                        else if (dictRef != null && dictRef.Equals("pdb:cTerminus"))
                        {
                            aminoAcid.addCTerminus(atom);
                        }
                        else
                        {
                            aminoAcid.addAtom(atom);
                        }
                    }
                    IBond[] bonds = containers[i].Bonds;
                    for (int bondCount = 0; bondCount < bonds.Length; bondCount++)
                    {
                        aminoAcid.addBond(bonds[bondCount]);
                    }
                    AminoAcidManipulator.removeAcidicOxygen(aminoAcid);
                    aminoAcid.setProperty(NO_ATOMS, "" + aminoAcid.AtomCount);
                    aminoAcid.setProperty(NO_BONDS, "" + aminoAcid.getBondCount());
                    if (i < aminoAcids.Length)
                    {
                        aminoAcids[i] = aminoAcid;
                    }
                    else
                    {
                        //logger.error("Could not store AminoAcid! Array too short!");
                    }
                }
            }
            catch (System.Exception exception)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                //logger.error("Failed reading file: ", exception.Message);
                //logger.debug(exception);
            }

            return aminoAcids;
        }
        static AminoAcids()
        {
            //logger = new LoggingTool(typeof(AminoAcids));
        }
    }
}