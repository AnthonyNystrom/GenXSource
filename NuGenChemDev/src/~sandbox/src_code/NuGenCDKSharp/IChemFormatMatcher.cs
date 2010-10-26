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

namespace Org.OpenScience.CDK.IO.Formats
{
    /// <summary> This interface is used for classes that are able to match a certain 
    /// chemical file format. For example: Chemical Markup Language, PDB etc.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       Egon Willighagen <egonw@sci.kun.nl>
    /// </author>
    /// <cdk.created>  2004-10-25 </cdk.created>
    /// <summary> 
    /// </summary>
    public interface IChemFormatMatcher : IChemFormat
    {
        /// <summary> Method that checks wether the given line is part of the format
        /// read by this reader.
        /// 
        /// </summary>
        /// <param name="lineNumber"> number of the line
        /// </param>
        /// <param name="line">       line in the file being checked
        /// 
        /// </param>
        /// <returns> true if the line is of a file format read by this reader
        /// </returns>
        bool matches(int lineNumber, System.String line);
    }
}