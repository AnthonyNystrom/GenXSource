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
*  */
using System;
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> This class is the interface that all IO writers should implement.
    /// Programs need only care about this interface for any kind of IO.
    /// 
    /// <p>Currently, database IO and file IO is supported. Internet IO is
    /// expected.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <version>   $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
    /// </version>
    public interface IChemObjectWriter : IChemObjectIO
    {

        /// <summary> Writes the content of "object" to output
        /// 
        /// </summary>
        /// <param name="object">   the object of which the content is outputed
        /// 
        /// </param>
        /// <exception cref="CDKException">is thrown if the output
        /// does not support the data in the object
        /// </exception>
        void write(IChemObject object_Renamed);

        /// <summary> Sets the Writer from which this ChemObjectWriter should write
        /// the contents.
        /// </summary>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Writer' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        void setWriter(System.IO.StreamWriter writer);

        /// <summary> Sets the OutputStream from which this ChemObjectWriter should write
        /// the contents.
        /// </summary>
        void setWriter(System.IO.Stream writer);
    }
}