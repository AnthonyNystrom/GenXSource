/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-02 13:48:44 +0200 (Sun, 02 Jul 2006) $  
* $Revision: 6537 $
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
    /// <summary> This class is the interface that all ResourceFormat's should implement.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       Egon Willighagen <egonw@users.sf.net>
    /// </author>
    /// <cdk.created>  2006-03-04 </cdk.created>
    /// <cdk.bug>      1485289 </cdk.bug>
    public interface IResourceFormat
    {
        /// <summary> Returns a one-lined format name of the format.</summary>
        System.String FormatName
        {
            get;

        }
        /// <summary> Returns the preferred resource name extension.</summary>
        System.String PreferredNameExtension
        {
            get;

        }
        /// <summary> Returns an array of common resource name extensions.</summary>
        System.String[] NameExtensions
        {
            get;

        }
        /// <summary> Returns the accepted MIME type for this format.
        /// 
        /// </summary>
        /// <returns> null if no MIME type has been accepted on
        /// </returns>
        System.String MIMEType
        {
            get;

        }
        /// <summary> Indicates if the format is an XML-based language.
        /// 
        /// </summary>
        /// <returns> if the format is XML-based.
        /// </returns>
        bool XMLBased
        {
            get;

        }
    }
}