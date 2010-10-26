/*  $RCSfile$    
 *  $Author: kaihartmann $    
 *  $Date: 2006-06-07 11:41:42 +0200 (Wed, 07 Jun 2006) $    
 *  $Revision: 6349 $
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
 *
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Org.OpenScience.CDK.Interfaces;

namespace NuGenCDKSharp
{
    /// <summary>
    /// Implementation of a Path as needed by {@cdk.cite HAN96}.
    ///
    /// @cdk.module standard
    /// 
    /// @cdk.keyword graph, path
    /// 
    /// @author     steinbeck
    /// @cdk.created    2002-02-28
    /// </summary>
    class Path : ArrayList
    {
        /// <summary>
        /// Constructs an empty path
        /// </summary>
        public Path()
        { }

        /// <summary>
        /// Constructs a new Path with two Atoms 
        /// </summary>
        /// <param name="atom1">first atom in the new path</param>
        /// <param name="atom2">second atom in the new path</param>
        public Path(IAtom atom1, IAtom atom2)
        {
            Add(atom1);
            Add(atom2);
        }

        /// <summary>
        /// Joins two paths. The joint point is given by an atom
        /// which is shared by the two pathes. 
        /// </summary>
        /// <param name="path1">First path to join</param>
        /// <param name="path2">Second path to join</param>
        /// <param name="atom">The atom which is the joint point</param>
        /// <returns>The newly formed longer path</returns>
        public static Path join(Path path1, Path path2, IAtom atom)
        {
            Path newPath = new Path();
            Path tempPath = new Path();
            if (path1[0] == atom)
            {
                path1.revert();
            }
            newPath.AddRange(path1);
            if (path2[path2.Count-1] == atom)
            {
                path2.revert();
            }
            tempPath.AddRange(path2);
            tempPath.Remove(atom);
            newPath.AddRange(tempPath);
            return newPath;
        }

        public int getIntersectionSize(Path other)
        {
            IAtom a1, a2;
            int iSize = 0;
            for (int i = 0; i < Count; i++)
            {
                a1 = (IAtom)this[i];
                for (int j = 0; j < other.Count; j++)
                {
                    a2 = (IAtom)other[j];
                    if (a1 == a2) iSize++;
                }
            }
            return iSize;
        }

        public void revert()
        {
            Object o = null;
            int size = Count;
            int i = (int)(size / 2);
            for (int f = 0; f < i; f++)
            {
                o = this[f];
                this[f] = this[size - f - 1];
                this[size - f - 1] = o;
            }
        }

        public String toString(IAtomContainer ac)
	    {
		    String s = "Path of length " + Count + ": ";
            for (int f = 0; f < Count; f++)
		    {
			    s += ac.getAtomNumber((IAtom)this[f]) + " ";
		    }
		    return s;
	    }
    }
}