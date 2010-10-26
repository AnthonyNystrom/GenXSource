/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
*
* Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
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

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary> Implements the idea of an element in the periodic table.
    /// 
    /// </summary>
    /// <cdk.module>  interfaces </cdk.module>
    /// <cdk.keyword>  element </cdk.keyword>
    public interface IElement : IChemObject
    {
        /// <summary> Returns the atomic number of this element.
        /// 
        /// <p>Once instantiated all field not filled by passing parameters
        /// to the constructured are null. Elements can be configured by using
        /// the IsotopeFactory.configure() method:
        /// <pre>
        /// Element element = new Element("C");
        /// IsotopeFactory if = IsotopeFactory.getInstance(element.getBuilder());
        /// if.configure(element);
        /// </pre>
        /// </p>      
        /// 
        /// </summary>
        /// <returns> The atomic number of this element    
        /// 
        /// </returns>
        /// <seealso cref="setAtomicNumber">
        /// </seealso>
        /// <summary> Sets the atomic number of this element.
        /// 
        /// </summary>
        /// <param name="atomicNumber">The atomic mass to be assigned to this element
        /// 
        /// </param>
        /// <seealso cref="getAtomicNumber">
        /// </seealso>
        int AtomicNumber
        {
            get;
            set;
        }
        
        /// <summary> Returns the element symbol of this element.
        /// 
        /// </summary>
        /// <returns> The element symbol of this element. Null if unset.
        /// 
        /// </returns>
        /// <seealso cref="setSymbol">
        /// </seealso>
        /// <summary> Sets the element symbol of this element.
        /// 
        /// </summary>
        /// <param name="symbol">The element symbol to be assigned to this atom
        /// 
        /// </param>
        /// <seealso cref="getSymbol">
        /// </seealso>
        String Symbol
        {
            get;
            set;
        }
    }
}