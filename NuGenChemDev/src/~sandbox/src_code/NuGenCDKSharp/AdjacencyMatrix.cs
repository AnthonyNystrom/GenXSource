/*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
*  $Revision: 5855 $
*
*  Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
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
*/
using System;
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK.Graph.Matrix
{
    /// <summary> Calculator for a adjacency matrix representation of this AtomContainer. An
    /// adjacency matrix is a matrix of quare NxN matrix, where N is the number of
    /// atoms in the AtomContainer. The element i,j of the matrix is 1, if the i-th
    /// and the j-th atom in the atomcontainer share a bond. Otherwise it is zero.
    /// See {@cdk.cite TRI1992}.
    /// 
    /// </summary>
    /// <cdk.module>   standard </cdk.module>
    /// <cdk.keyword>  adjacency matrix </cdk.keyword>
    /// <summary> 
    /// </summary>
    /// <author>       steinbeck
    /// </author>
    /// <cdk.created>  2004-07-04 </cdk.created>
    /// <cdk.dictref>  blue-obelisk:calculateAdjecencyMatrix </cdk.dictref>
    public class AdjacencyMatrix : IGraphMatrix
    {
        /// <summary> Returns the adjacency matrix for the given AtomContainer.
        /// 
        /// </summary>
        /// <param name="container">The AtomContainer for which the matrix is calculated
        /// </param>
        /// <returns>           A adjacency matrix representating this AtomContainer
        /// </returns>
        public static int[][] getMatrix(IAtomContainer container)
        {
            IElectronContainer electronContainer = null;
            int indexAtom1;
            int indexAtom2;
            int[][] conMat = new int[container.AtomCount][];
            for (int i = 0; i < container.AtomCount; i++)
            {
                conMat[i] = new int[container.AtomCount];
            }
            for (int f = 0; f < container.ElectronContainerCount; f++)
            {
                electronContainer = container.getElectronContainerAt(f);
                if (electronContainer is IBond)
                {
                    IBond bond = (IBond)electronContainer;
                    indexAtom1 = container.getAtomNumber(bond.getAtomAt(0));
                    indexAtom2 = container.getAtomNumber(bond.getAtomAt(1));
                    conMat[indexAtom1][indexAtom2] = 1;
                    conMat[indexAtom2][indexAtom1] = 1;
                }
            }
            return conMat;
        }
    }
}