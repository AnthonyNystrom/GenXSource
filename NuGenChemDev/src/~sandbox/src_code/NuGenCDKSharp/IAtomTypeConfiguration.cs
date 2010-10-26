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
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK.Config
{
    /// <summary> Interface that allows reading atom type configuration data from some
    /// source.
    /// 
    /// </summary>
    /// <cdk.module>  core </cdk.module>
    public interface IAtomTypeConfigurator
    {
        /// <summary> Sets the file containing the config data.
        /// 
        /// </summary>
        /// <param name="ins">InputStream from which the atom type definitions are to be read
        /// </param>
        System.IO.Stream InputStream
        {
            set;

        }

        /// <summary> Reads a set of configured AtomType's into a Vector.
        /// 
        /// </summary>
        /// <param name="builder">ChemObjectBuilder used to instantiate the AtomType's.
        /// 
        /// </param>
        /// <returns> A Vector containing the AtomTypes extracted from the InputStream
        /// </returns>
        /// <throws>  IOException when something went wrong with reading the data </throws>
        System.Collections.ArrayList readAtomTypes(IChemObjectBuilder builder);
    }
}