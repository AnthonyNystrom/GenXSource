/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
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
*
*/
using System;

namespace Org.OpenScience.CDK
{
    /// <summary> Atom that represents part of an residue in an enzyme, like Arg255.
    /// 
    /// </summary>
    /// <seealso cref="PseudoAtom">
    /// </seealso>
    [Serializable]
    public class EnzymeResidueLocator : PseudoAtom
    {
        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = -4267555433142927412L;

        /// <summary> Constructs an EnzymeResidueLocator from a String containing the locator.
        /// 
        /// </summary>
        /// <param name="label"> The String describing the residue and its location.
        /// </param>
        public EnzymeResidueLocator(System.String label)
            : base(label)
        {
        }

        /// <summary> Constructs an EnzymeResidueLocator from an existing Atom.
        /// 
        /// </summary>
        /// <param name="atom">Atom that should be converted into a EnzymeResidueLocator.
        /// </param>
        public EnzymeResidueLocator(Atom atom)
            : base(atom)
        {
            if (atom is PseudoAtom)
            {
                this.Label = ((PseudoAtom)atom).Label;
            }
        }
    }
}