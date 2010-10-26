/* $RCSfile$
* $Author: egonw $
* $Date: 2006-05-02 09:46:48 +0200 (Tue, 02 May 2006) $
* $Revision: 6119 $
* 
* Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
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

namespace Org.OpenScience.CDK.Isomorphism.Matchers
{
    /// <summary> Defines the abililty to be matched against IAtom's. Most prominent application
    /// is in isomorphism and substructure matching in the UniversalIsomorphismTester.
    /// 
    /// </summary>
    /// <cdk.module>  interfaces </cdk.module>
    /// <seealso cref="org.openscience.cdk.isomorphism.UniversalIsomorphismTester">
    /// </seealso>
    public interface IQueryAtom : IAtom
    {

        /// <summary> Returns true of the given <code>atom</code> matches this IQueryAtom.
        /// 
        /// </summary>
        /// <param name="atom">IAtom to match against
        /// </param>
        /// <returns>     true, if this IQueryAtom matches the given IAtom
        /// </returns>
        bool matches(IAtom atom);
    }
}