/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
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
* 
*/
using System;

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary> A Single Electron is an orbital which is occupied by only one electron.
    /// A radical in CDK is represented by an AtomContainer that contains an Atom
    /// and a SingleElectron type ElectronContainer:
    /// <pre>
    /// AtomContainer radical = new AtomContainer();
    /// Atom carbon = new Atom("C");
    /// carbon.setImplicitHydrogens(3);
    /// radical.addElectronContainer(new SingleElectron(carbon));
    /// </pre> 
    /// 
    /// </summary>
    /// <cdk.module>  interfaces </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  radical </cdk.keyword>
    /// <cdk.keyword>  electron, unpaired </cdk.keyword>
    public interface ISingleElectron : IElectronContainer
    {
        /// <summary> Returns the associated Atom.
        /// 
        /// </summary>
        /// <returns> the associated Atom.
        /// </returns>
        /// <seealso cref="setAtom">
        /// </seealso>
        /// <summary> Sets the associated Atom.
        /// 
        /// </summary>
        /// <param name="atom">the Atom this SingleElectron will be associated with
        /// </param>
        /// <seealso cref="getAtom">
        /// </seealso>
        IAtom Atom
        {
            get;
            set;
        }

        /// <summary> Returns true if the given atom participates in this SingleElectron.
        /// 
        /// </summary>
        /// <param name="atom"> The atom to be tested if it participates in this bond
        /// </param>
        /// <returns>     true if this SingleElectron is associated with the atom
        /// </returns>
        bool contains(IAtom atom);
    }
}