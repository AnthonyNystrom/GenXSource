/*
*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-07-14 11:39:20 +0200 (Fri, 14 Jul 2006) $
*  $Revision: 6669 $
*
*  Copyright (C) 2002-2003  The Jmol Project
*  Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
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
using javax.vecmath;
using Org.OpenScience.CDK.Exception;
using Org.OpenScience.CDK.Graph.Invariant;
using Support;

namespace Org.OpenScience.CDK.Geometry
{
    /// <summary>
    /// A set of static utility classes for geometric calculations on Bonds.
    /// </summary>
    /// <author>       shk3
    /// </author>
    /// <cdk.created>  2005-08-04 </cdk.created>
    public class BondTools
    {
        /// <summary> 
        /// Tells if a certain bond is center of a valid double bond configuration.
        /// </summary>
        /// <param name="container"> The atomcontainer.
        /// </param>
        /// <param name="bond">      The bond.
        /// </param>
        /// <returns>            true=is a potential configuration, false=is not.
        /// </returns>
        public static bool isValidDoubleBondConfiguration(IAtomContainer container, IBond bond)
        {
            IAtom[] atoms = bond.getAtoms();
            IAtom[] connectedAtoms = container.getConnectedAtoms(atoms[0]);
            IAtom from = null;
            for (int i = 0; i < connectedAtoms.Length; i++)
            {
                if (connectedAtoms[i] != atoms[1])
                {
                    from = connectedAtoms[i];
                }
            }
            bool[] array = new bool[container.Bonds.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = true;
            }
            if (isStartOfDoubleBond(container, atoms[0], from, array) && isEndOfDoubleBond(container, atoms[1], atoms[0], array) && !bond.getFlag(CDKConstants.ISAROMATIC))
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }


        /// <summary>  Says if two atoms are in cis or trans position around a double bond.
        /// The atoms have to be given to the method like this:  firstOuterAtom - firstInnerAtom = secondInnterAtom - secondOuterAtom
        /// 
        /// </summary>
        /// <param name="firstOuterAtom">   See above.
        /// </param>
        /// <param name="firstInnerAtom">   See above.
        /// </param>
        /// <param name="secondInnerAtom">  See above.
        /// </param>
        /// <param name="secondOuterAtom">  See above.
        /// </param>
        /// <param name="ac">               The atom container the atoms are in.
        /// </param>
        /// <returns>                   true=trans, false=cis.
        /// </returns>
        /// <exception cref="CDKException"> The atoms are not in a double bond configuration (no double bond in the middle, same atoms on one side)
        /// </exception>
        public static bool isCisTrans(IAtom firstOuterAtom, IAtom firstInnerAtom, IAtom secondInnerAtom, IAtom secondOuterAtom, IAtomContainer ac)
        {
            if (!isValidDoubleBondConfiguration(ac, ac.getBond(firstInnerAtom, secondInnerAtom)))
            {
                throw new CDKException("There is no valid double bond configuration between your inner atoms!");
            }
            bool firstDirection = isLeft(firstOuterAtom, firstInnerAtom, secondInnerAtom);
            bool secondDirection = isLeft(secondOuterAtom, secondInnerAtom, firstInnerAtom);
            if (firstDirection == secondDirection)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>  Says if an atom is on the left side of a another atom seen from a certain
        /// atom or not
        /// 
        /// </summary>
        /// <param name="whereIs">  The atom the position of which is returned
        /// </param>
        /// <param name="viewFrom"> The atom from which to look
        /// </param>
        /// <param name="viewTo">   The atom to which to look
        /// </param>
        /// <returns>           true=is left, false = is not
        /// </returns>
        public static bool isLeft(IAtom whereIs, IAtom viewFrom, IAtom viewTo)
        {
            double angle = giveAngleBothMethods(viewFrom, viewTo, whereIs, false);
            if (angle < 0)
            {
                return (false);
            }
            else
            {
                return (true);
            }
        }


        /// <summary> Returns true if the two atoms are within the distance fudge
        /// factor of each other.
        /// 
        /// </summary>
        /// <param name="atom1">               Description of Parameter
        /// </param>
        /// <param name="atom2">               Description of Parameter
        /// </param>
        /// <param name="distanceFudgeFactor"> Description of Parameter
        /// </param>
        /// <returns>                      Description of the Returned Value
        /// </returns>
        /// <cdk.keyword>                  join-the-dots </cdk.keyword>
        /// <cdk.keyword>                  bond creation </cdk.keyword>
        public static bool closeEnoughToBond(IAtom atom1, IAtom atom2, double distanceFudgeFactor)
        {

            if (atom1 != atom2)
            {
                double distanceBetweenAtoms = atom1.getPoint3d().distance(atom2.getPoint3d());
                double bondingDistance = atom1.CovalentRadius + atom2.CovalentRadius;
                if (distanceBetweenAtoms <= (distanceFudgeFactor * bondingDistance))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary> 
        /// Gives the angle between two lines starting at atom from and going to to1
        /// and to2. If bool=false the angle starts from the middle line and goes from
        /// 0 to PI or 0 to -PI if the to2 is on the left or right side of the line. If
        /// bool=true the angle goes from 0 to 2PI.
        /// </summary>
        /// <param name="from">the atom to view from.</param>
        /// <param name="to1">first direction to look in.</param>
        /// <param name="to2">second direction to look in.</param>
        /// <param name="useNegativePI">true=angle is 0 to 2PI, false=angel is -PI to PI.</param>
        /// <returns>The angle in rad.</returns>
        public static double giveAngleBothMethods(IAtom from, IAtom to1, IAtom to2, bool useNegativePI)
        {
            return giveAngleBothMethods(from.getPoint2d(), to1.getPoint2d(), to2.getPoint2d(), useNegativePI);
        }


        public static double giveAngleBothMethods(Point2d from, Point2d to1, Point2d to2, bool bool_Renamed)
        {
            double[] A = new double[2];
            from.get_Renamed(A);
            double[] B = new double[2];
            to1.get_Renamed(B);
            double[] C = new double[2];
            to2.get_Renamed(C);
            double angle1 = System.Math.Atan2((B[1] - A[1]), (B[0] - A[0]));
            double angle2 = System.Math.Atan2((C[1] - A[1]), (C[0] - A[0]));
            double angle = angle2 - angle1;
            if (angle2 < 0 && angle1 > 0 && angle2 < -(System.Math.PI / 2))
            {
                angle = System.Math.PI + angle2 + System.Math.PI - angle1;
            }
            if (angle2 > 0 && angle1 < 0 && angle1 < -(System.Math.PI / 2))
            {
                angle = -System.Math.PI + angle2 - System.Math.PI - angle1;
            }
            if (bool_Renamed && angle < 0)
            {
                return (2 * System.Math.PI + angle);
            }
            else
            {
                return (angle);
            }
        }

        /// <summary>  Says if an atom is the end of a double bond configuration
        /// 
        /// </summary>
        /// <param name="atom">                    The atom which is the end of configuration
        /// </param>
        /// <param name="container">               The atomContainer the atom is in
        /// </param>
        /// <param name="parent">                  The atom we came from
        /// </param>
        /// <param name="doubleBondConfiguration"> The array indicating where double bond
        /// configurations are specified (this method ensures that there is
        /// actually the possibility of a double bond configuration)
        /// </param>
        /// <returns>                          false=is not end of configuration, true=is
        /// </returns>
        private static bool isEndOfDoubleBond(IAtomContainer container, IAtom atom, IAtom parent, bool[] doubleBondConfiguration)
        {
            if (container.getBondNumber(atom, parent) == -1 || doubleBondConfiguration.Length <= container.getBondNumber(atom, parent) || !doubleBondConfiguration[container.getBondNumber(atom, parent)])
            {
                return false;
            }
            int lengthAtom = container.getConnectedAtoms(atom).Length + atom.getHydrogenCount();
            int lengthParent = container.getConnectedAtoms(parent).Length + parent.getHydrogenCount();
            if (container.getBond(atom, parent) != null)
            {
                if (container.getBond(atom, parent).Order == CDKConstants.BONDORDER_DOUBLE && (lengthAtom == 3 || (lengthAtom == 2 && atom.Symbol.Equals("N"))) && (lengthParent == 3 || (lengthParent == 2 && parent.Symbol.Equals("N"))))
                {
                    IAtom[] atoms = container.getConnectedAtoms(atom);
                    IAtom one = null;
                    IAtom two = null;
                    for (int i = 0; i < atoms.Length; i++)
                    {
                        if (atoms[i] != parent && one == null)
                        {
                            one = atoms[i];
                        }
                        else if (atoms[i] != parent && one != null)
                        {
                            two = atoms[i];
                        }
                    }
                    System.String[] morgannumbers = MorganNumbersTools.getMorganNumbersWithElementSymbol(container);
                    if ((one != null && two == null && atom.Symbol.Equals("N") && System.Math.Abs(giveAngleBothMethods(parent, atom, one, true)) > System.Math.PI / 10) || (!atom.Symbol.Equals("N") && one != null && two != null && !morgannumbers[container.getAtomNumber(one)].Equals(morgannumbers[container.getAtomNumber(two)])))
                    {
                        return (true);
                    }
                    else
                    {
                        return (false);
                    }
                }
            }
            return (false);
        }


        /// <summary>  Says if an atom is the start of a double bond configuration
        /// 
        /// </summary>
        /// <param name="a">                       The atom which is the start of configuration
        /// </param>
        /// <param name="container">               The atomContainer the atom is in
        /// </param>
        /// <param name="parent">                  The atom we came from
        /// </param>
        /// <param name="doubleBondConfiguration"> The array indicating where double bond
        /// configurations are specified (this method ensures that there is
        /// actually the possibility of a double bond configuration)
        /// </param>
        /// <returns>                          false=is not start of configuration, true=is
        /// </returns>
        private static bool isStartOfDoubleBond(IAtomContainer container, IAtom a, IAtom parent, bool[] doubleBondConfiguration)
        {
            int lengthAtom = container.getConnectedAtoms(a).Length + a.getHydrogenCount();
            if (lengthAtom != 3 && (lengthAtom != 2 && (System.Object)a.Symbol != (System.Object)("N")))
            {
                return (false);
            }
            IAtom[] atoms = container.getConnectedAtoms(a);
            IAtom one = null;
            IAtom two = null;
            bool doubleBond = false;
            IAtom nextAtom = null;
            for (int i = 0; i < atoms.Length; i++)
            {
                if (atoms[i] != parent && container.getBond(atoms[i], a).Order == CDKConstants.BONDORDER_DOUBLE && isEndOfDoubleBond(container, atoms[i], a, doubleBondConfiguration))
                {
                    doubleBond = true;
                    nextAtom = atoms[i];
                }
                if (atoms[i] != nextAtom && one == null)
                {
                    one = atoms[i];
                }
                else if (atoms[i] != nextAtom && one != null)
                {
                    two = atoms[i];
                }
            }
            System.String[] morgannumbers = MorganNumbersTools.getMorganNumbersWithElementSymbol(container);
            if (one != null && ((!a.Symbol.Equals("N") && two != null && !morgannumbers[container.getAtomNumber(one)].Equals(morgannumbers[container.getAtomNumber(two)]) && doubleBond && doubleBondConfiguration[container.getBondNumber(a, nextAtom)]) || (doubleBond && a.Symbol.Equals("N") && System.Math.Abs(giveAngleBothMethods(nextAtom, a, parent, true)) > System.Math.PI / 10)))
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }


        /// <summary>  Says if an atom as a center of a tetrahedral chirality
        /// 
        /// </summary>
        /// <param name="a">         The atom which is the center
        /// </param>
        /// <param name="container"> The atomContainer the atom is in
        /// </param>
        /// <returns>            0=is not tetrahedral;>1 is a certain depiction of
        /// tetrahedrality (evaluated in parse chain)
        /// </returns>
        public static int isTetrahedral(IAtomContainer container, IAtom a, bool strict)
        {
            IAtom[] atoms = container.getConnectedAtoms(a);
            if (atoms.Length != 4)
            {
                return (0);
            }
            IBond[] bonds = container.getConnectedBonds(a);
            int normal = 0;
            int up = 0;
            int down = 0;
            for (int i = 0; i < bonds.Length; i++)
            {
                if (bonds[i].Stereo == CDKConstants.STEREO_BOND_NONE || bonds[i].Stereo == CDKConstants.STEREO_BOND_UNDEFINED)
                {
                    normal++;
                }
                if (bonds[i].Stereo == CDKConstants.STEREO_BOND_UP)
                {
                    up++;
                }
                if (bonds[i].Stereo == CDKConstants.STEREO_BOND_DOWN)
                {
                    down++;
                }
            }
            if (up == 1 && down == 1)
            {
                return 1;
            }
            if (up == 2 && down == 2)
            {
                if (stereosAreOpposite(container, a))
                {
                    return 2;
                }
                return 0;
            }
            if (up == 1 && down == 0 && !strict)
            {
                return 3;
            }
            if (down == 1 && up == 0 && !strict)
            {
                return 4;
            }
            if (down == 2 && up == 1 && !strict)
            {
                return 5;
            }
            if (down == 1 && up == 2 && !strict)
            {
                return 6;
            }
            return 0;
        }


        /// <summary>  Says if an atom as a center of a trigonal-bipyramidal or actahedral
        /// chirality
        /// 
        /// </summary>
        /// <param name="a">         The atom which is the center
        /// </param>
        /// <param name="container"> The atomContainer the atom is in
        /// </param>
        /// <returns>            true=is square planar, false=is not
        /// </returns>
        public static int isTrigonalBipyramidalOrOctahedral(IAtomContainer container, IAtom a)
        {
            IAtom[] atoms = container.getConnectedAtoms(a);
            if (atoms.Length < 5 || atoms.Length > 6)
            {
                return (0);
            }
            IBond[] bonds = container.getConnectedBonds(a);
            int normal = 0;
            int up = 0;
            int down = 0;
            for (int i = 0; i < bonds.Length; i++)
            {
                if (bonds[i].Stereo == CDKConstants.STEREO_BOND_UNDEFINED || bonds[i].Stereo == CDKConstants.STEREO_BOND_NONE)
                {
                    normal++;
                }
                if (bonds[i].Stereo == CDKConstants.STEREO_BOND_UP)
                {
                    up++;
                }
                if (bonds[i].Stereo == CDKConstants.STEREO_BOND_DOWN)
                {
                    down++;
                }
            }
            if (up == 1 && down == 1)
            {
                if (atoms.Length == 5)
                    return 1;
                else
                    return 2;
            }
            return 0;
        }


        /// <summary>  Says if an atom as a center of any valid stereo configuration or not
        /// 
        /// </summary>
        /// <param name="a">         The atom which is the center
        /// </param>
        /// <param name="container"> The atomContainer the atom is in
        /// </param>
        /// <returns>            true=is a stereo atom, false=is not
        /// </returns>
        public static bool isStereo(IAtomContainer container, IAtom a)
        {
            IAtom[] atoms = container.getConnectedAtoms(a);
            if (atoms.Length < 4 || atoms.Length > 6)
            {
                return (false);
            }
            IBond[] bonds = container.getConnectedBonds(a);
            int stereo = 0;
            for (int i = 0; i < bonds.Length; i++)
            {
                if (bonds[i].Stereo != 0)
                {
                    stereo++;
                }
            }
            if (stereo == 0)
            {
                return false;
            }
            int differentAtoms = 0;
            for (int i = 0; i < atoms.Length; i++)
            {
                bool isDifferent = true;
                for (int k = 0; k < i; k++)
                {
                    if (atoms[i].Symbol.Equals(atoms[k].Symbol))
                    {
                        isDifferent = false;
                        break;
                    }
                }
                if (isDifferent)
                {
                    differentAtoms++;
                }
            }
            if (differentAtoms != atoms.Length)
            {
                int[] morgannumbers = MorganNumbersTools.getMorganNumbers(container);
                System.Collections.ArrayList differentSymbols = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
                for (int i = 0; i < atoms.Length; i++)
                {
                    if (!differentSymbols.Contains(atoms[i].Symbol))
                    {
                        differentSymbols.Add(atoms[i].Symbol);
                    }
                }
                int[] onlyRelevantIfTwo = new int[2];
                if (differentSymbols.Count == 2)
                {
                    for (int i = 0; i < atoms.Length; i++)
                    {
                        if (differentSymbols.IndexOf(atoms[i].Symbol) == 0)
                        {
                            onlyRelevantIfTwo[0]++;
                        }
                        else
                        {
                            onlyRelevantIfTwo[1]++;
                        }
                    }
                }
                bool[] symbolsWithDifferentMorganNumbers = new bool[differentSymbols.Count];
                System.Collections.ArrayList[] symbolsMorganNumbers = new System.Collections.ArrayList[differentSymbols.Count];
                for (int i = 0; i < symbolsWithDifferentMorganNumbers.Length; i++)
                {
                    symbolsWithDifferentMorganNumbers[i] = true;
                    symbolsMorganNumbers[i] = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
                }
                for (int k = 0; k < atoms.Length; k++)
                {
                    int elementNumber = differentSymbols.IndexOf(atoms[k].Symbol);
                    if (symbolsMorganNumbers[elementNumber].Contains((System.Int32)morgannumbers[container.getAtomNumber(atoms[k])]))
                    {
                        symbolsWithDifferentMorganNumbers[elementNumber] = false;
                    }
                    else
                    {
                        symbolsMorganNumbers[elementNumber].Add((System.Int32)morgannumbers[container.getAtomNumber(atoms[k])]);
                    }
                }
                int numberOfSymbolsWithDifferentMorganNumbers = 0;
                for (int i = 0; i < symbolsWithDifferentMorganNumbers.Length; i++)
                {
                    if (symbolsWithDifferentMorganNumbers[i] == true)
                    {
                        numberOfSymbolsWithDifferentMorganNumbers++;
                    }
                }
                if (numberOfSymbolsWithDifferentMorganNumbers != differentSymbols.Count)
                {
                    if ((atoms.Length == 5 || atoms.Length == 6) && (numberOfSymbolsWithDifferentMorganNumbers + differentAtoms > 2 || (differentAtoms == 2 && onlyRelevantIfTwo[0] > 1 && onlyRelevantIfTwo[1] > 1)))
                    {
                        return (true);
                    }
                    if (isSquarePlanar(container, a) && (numberOfSymbolsWithDifferentMorganNumbers + differentAtoms > 2 || (differentAtoms == 2 && onlyRelevantIfTwo[0] > 1 && onlyRelevantIfTwo[1] > 1)))
                    {
                        return (true);
                    }
                    return false;
                }
            }
            return (true);
        }


        /// <summary>  Says if an atom as a center of a square planar chirality
        /// 
        /// </summary>
        /// <param name="a">         The atom which is the center
        /// </param>
        /// <param name="container"> The atomContainer the atom is in
        /// </param>
        /// <returns>            true=is square planar, false=is not
        /// </returns>
        public static bool isSquarePlanar(IAtomContainer container, IAtom a)
        {
            IAtom[] atoms = container.getConnectedAtoms(a);
            if (atoms.Length != 4)
            {
                return (false);
            }
            IBond[] bonds = container.getConnectedBonds(a);
            int normal = 0;
            int up = 0;
            int down = 0;
            for (int i = 0; i < bonds.Length; i++)
            {
                if (bonds[i].Stereo == CDKConstants.STEREO_BOND_UNDEFINED || bonds[i].Stereo == CDKConstants.STEREO_BOND_NONE)
                {
                    normal++;
                }
                if (bonds[i].Stereo == CDKConstants.STEREO_BOND_UP)
                {
                    up++;
                }
                if (bonds[i].Stereo == CDKConstants.STEREO_BOND_DOWN)
                {
                    down++;
                }
            }
            if (up == 2 && down == 2 && !stereosAreOpposite(container, a))
            {
                return true;
            }
            return false;
        }


        /// <summary>  Says if of four atoms connected two one atom the up and down bonds are
        /// opposite or not, i. e.if it's tetrehedral or square planar. The method
        /// doesnot check if there are four atoms and if two or up and two are down
        /// 
        /// </summary>
        /// <param name="a">         The atom which is the center
        /// </param>
        /// <param name="container"> The atomContainer the atom is in
        /// </param>
        /// <returns>            true=are opposite, false=are not
        /// </returns>
        public static bool stereosAreOpposite(IAtomContainer container, IAtom a)
        {
            System.Collections.IList atoms = container.getConnectedAtomsVector(a);
            //UPGRADE_ISSUE: Class hierarchy differences between 'java.util.TreeMap' and 'System.Collections.SortedList' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
            //UPGRADE_TODO: Constructor 'java.util.TreeMap.TreeMap' was converted to 'System.Collections.SortedList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilTreeMapTreeMap'"
            System.Collections.SortedList hm = new System.Collections.SortedList();
            for (int i = 1; i < atoms.Count; i++)
            {
                hm[(double)giveAngle(a, (IAtom)atoms[0], ((IAtom)atoms[i]))] = (System.Int32)i;
            }
            //UPGRADE_TODO: Method 'java.util.TreeMap.values' was converted to 'System.Collections.SortedList.Values' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilTreeMapvalues'"
            System.Object[] ohere = SupportClass.ICollectionSupport.ToArray(hm.Values);
            int stereoOne = container.getBond(a, (IAtom)atoms[0]).Stereo;
            int stereoOpposite = container.getBond(a, (IAtom)atoms[(((System.Int32)ohere[1]))]).Stereo;
            if (stereoOpposite == stereoOne)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Calls giveAngleBothMethods with bool = true
        /// </summary>
        /// <param name="from"> the atom to view from</param>
        /// <param name="to1">first direction to look in</param>
        /// <param name="to2">second direction to look in</param>
        /// <returns>The angle in rad from 0 to 2*PI</returns>
        public static double giveAngle(IAtom from, IAtom to1, IAtom to2)
        {
            return (giveAngleBothMethods(from, to1, to2, true));
        }

        /// <summary> 
        /// Calls giveAngleBothMethods with bool = false
        /// </summary>
        /// <param name="from">the atom to view from</param>
        /// <param name="to1"> first direction to look in</param>
        /// <param name="to2"> second direction to look in</param>
        /// <returns>The angle in rad from -PI to PI</returns>
        public static double giveAngleFromMiddle(IAtom from, IAtom to1, IAtom to2)
        {
            return (giveAngleBothMethods(from, to1, to2, false));
        }

        public static void makeUpDownBonds(IAtomContainer container)
        {
            for (int i = 0; i < container.AtomCount; i++)
            {
                IAtom a = container.getAtomAt(i);
                if (container.getConnectedAtoms(a).Length == 4)
                {
                    int up = 0;
                    int down = 0;
                    int hs = 0;
                    IAtom h = null;
                    for (int k = 0; k < 4; k++)
                    {
                        if (container.getBond(a, container.getConnectedAtoms(a)[k]).Stereo == CDKConstants.STEREO_BOND_UP)
                        {
                            up++;
                        }
                        if (container.getBond(a, container.getConnectedAtoms(a)[k]).Stereo == CDKConstants.STEREO_BOND_DOWN)
                        {
                            down++;
                        }
                        if (container.getBond(a, container.getConnectedAtoms(a)[k]).Stereo == CDKConstants.STEREO_BOND_NONE && container.getConnectedAtoms(a)[k].Symbol.Equals("H"))
                        {
                            h = container.getConnectedAtoms(a)[k];
                            hs++;
                        }
                    }
                    if (up == 0 && down == 1 && h != null && hs == 1)
                    {
                        container.getBond(a, h).Stereo = CDKConstants.STEREO_BOND_UP;
                    }
                    if (up == 1 && down == 0 && h != null && hs == 1)
                    {
                        container.getBond(a, h).Stereo = CDKConstants.STEREO_BOND_DOWN;
                    }
                }
            }
        }
    }
}