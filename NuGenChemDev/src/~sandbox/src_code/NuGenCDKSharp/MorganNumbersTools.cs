/*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
*  $Revision: 5855 $
*
*  Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
*
*  Contact: cdk-devel@lists.sourceforge.net
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public License
*  as published by the Free Software Foundation; either version 2.1
*  of the License, or (at your option) any later version.
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

namespace Org.OpenScience.CDK.Graph.Invariant
{

    /// <summary> Tool for calculating Morgan numbers {@cdk.cite MOR65}.
    /// 
    /// </summary>
    /// <cdk.module>  standard </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>      shk3
    /// </author>
    /// <cdk.created>     2003-06-30 </cdk.created>
    /// <cdk.keyword>     Morgan number </cdk.keyword>
    public class MorganNumbersTools
    {

        /// <summary>  Makes an array containing the morgan numbers of the atoms of atomContainer.
        /// 
        /// </summary>
        /// <param name="atomContainer"> The atomContainer to analyse.
        /// </param>
        /// <returns>                The morgan numbers value.
        /// </returns>
        public static int[] getMorganNumbers(IAtomContainer atomContainer)
        {
            int[] morganMatrix;
            int[] tempMorganMatrix;
            int N = atomContainer.AtomCount;
            morganMatrix = new int[N];
            tempMorganMatrix = new int[N];
            IAtom[] atoms = null;
            for (int f = 0; f < N; f++)
            {
                morganMatrix[f] = atomContainer.getBondCount(f);
                tempMorganMatrix[f] = atomContainer.getBondCount(f);
            }
            for (int e = 0; e < N; e++)
            {
                for (int f = 0; f < N; f++)
                {
                    morganMatrix[f] = 0;
                    atoms = atomContainer.getConnectedAtoms(atomContainer.getAtomAt(f));
                    for (int g = 0; g < atoms.Length; g++)
                    {
                        morganMatrix[f] += tempMorganMatrix[atomContainer.getAtomNumber(atoms[g])];
                    }
                }
                Array.Copy(morganMatrix, 0, tempMorganMatrix, 0, N);
            }
            return tempMorganMatrix;
        }


        /// <summary>  Makes an array containing the morgan numbers+element symbol of the atoms of atomContainer. This method
        /// puts the element symbol before the morgan number, usefull for finding out how many different rests are connected to an atom.
        /// 
        /// </summary>
        /// <param name="atomContainer"> The atomContainer to analyse.
        /// </param>
        /// <returns>                The morgan numbers value.
        /// </returns>
        public static System.String[] getMorganNumbersWithElementSymbol(IAtomContainer atomContainer)
        {
            int[] morgannumbers = getMorganNumbers(atomContainer);
            System.String[] morgannumberswithelement = new System.String[morgannumbers.Length];
            for (int i = 0; i < morgannumbers.Length; i++)
            {
                morgannumberswithelement[i] = atomContainer.getAtomAt(i).Symbol + "-" + morgannumbers[i];
            }
            return (morgannumberswithelement);
        }
    }
}