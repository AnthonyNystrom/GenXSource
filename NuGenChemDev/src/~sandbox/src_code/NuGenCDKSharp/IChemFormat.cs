/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-04 18:45:46 +0200 (Tue, 04 Jul 2006) $  
* $Revision: 6586 $
*
* Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
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
*/
using System;

namespace Org.OpenScience.CDK.IO.Formats
{
    /// <summary> This class is the interface that all ChemFormat's should implement.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       Egon Willighagen <egonw@sci.kun.nl>
    /// </author>
    /// <cdk.created>  2004-10-25 </cdk.created>
    /// <cdk.bug>    1495502 </cdk.bug>
    public interface IChemFormat : IResourceFormat
    {
        /// <summary> Returns the class name of the CDK Reader for this format.
        /// 
        /// </summary>
        /// <returns> null if no CDK Reader is available.
        /// </returns>
        String ReaderClassName
        {
            get;

        }
        /// <summary> Returns the class name of the CDK Writer for this format.
        /// 
        /// </summary>
        /// <returns> null if no CDK Writer is available.
        /// </returns>
        System.String WriterClassName
        {
            get;

        }
        /// <summary> Returns an integer indicating the data features that this 
        /// format supports. The integer is composed as explained in 
        /// DataFeatures. May be set to DataFeatures.NONE as default.
        /// 
        /// </summary>
        /// <seealso cref="org.openscience.cdk.tools.DataFeatures">
        /// </seealso>
        int SupportedDataFeatures
        {
            get;

        }
        /// <summary> Returns an integer indicating the data features that this 
        /// format requires. For example, the XYZ format requires 3D
        /// coordinates.
        /// 
        /// </summary>
        /// <seealso cref="org.openscience.cdk.tools.DataFeatures">
        /// </seealso>
        int RequiredDataFeatures
        {
            get;

        }
    }
}