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
    /// <summary> Used to store and retrieve data of a particular isotope.
    /// 
    /// </summary>
    /// <cdk.module>  interfaces </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       egonw
    /// </author>
    /// <cdk.created>  2005-08-24 </cdk.created>
    /// <cdk.keyword>  isotope </cdk.keyword>
    public interface IIsotope : IElement
    {
        /// <summary> Gets the NaturalAbundance attribute of the Isotope object.
        /// Returns null when unconfigured.
        /// 
        /// </summary>
        /// <returns>    The NaturalAbundance value
        /// 
        /// </returns>
        /// <seealso cref="setNaturalAbundance">
        /// </seealso>
        /// <summary> Sets the NaturalAbundance attribute of the Isotope object.
        /// 
        /// </summary>
        /// <param name="naturalAbundance"> The new NaturalAbundance value
        /// 
        /// </param>
        /// <seealso cref="getNaturalAbundance">
        /// </seealso>
        double NaturalAbundance
        {
            get;
            set;
        }
        
        /// <summary> Returns the atomic mass of this element.
        /// Returns null when unconfigured.
        /// 
        /// </summary>
        /// <returns> The atomic mass of this element
        /// 
        /// </returns>
        /// <seealso cref="setMassNumber(int)">
        /// </seealso>
        /// <summary> Sets the atomic mass of this element.
        /// 
        /// </summary>
        /// <param name="massNumber">The atomic mass to be assigned to this element
        /// 
        /// </param>
        /// <seealso cref="getMassNumber">
        /// </seealso>
        int MassNumber
        {
            get;
            set;
        }

        /// <summary> Sets the ExactMass attribute of the Isotope object.
        /// 
        /// </summary>
        /// <param name="exactMass"> The new ExactMass value
        /// 
        /// </param>
        /// <seealso cref="getExactMass">
        /// </seealso>
        void setExactMass(double exactMass);

        /// <summary> Gets the ExactMass attribute of the Isotope object.
        /// Returns null when unconfigured.
        /// 
        /// </summary>
        /// <returns>    The ExactMass value
        /// 
        /// </returns>
        /// <seealso cref="setExactMass">
        /// </seealso>
        double getExactMass();
    }
}