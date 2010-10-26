/* $RCSfile$
* $Author: egonw $
* $Date: 2006-05-09 21:32:32 +0200 (Tue, 09 May 2006) $  
* $Revision: 6204 $
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
using Org.OpenScience.CDK.IO.Formats;
using Org.OpenScience.CDK.IO.Setting;
using Org.OpenScience.CDK.IO.Listener;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> This class is the interface that all IO readers should implement.
    /// Programs need only care about this interface for any kind of IO.
    /// Currently, database IO and file IO is supported.
    /// 
    /// <p>The easiest way to implement a new ChemObjectReader is to
    /// subclass the DefaultChemObjectReader.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <seealso cref="DefaultChemObjectReader">
    /// 
    /// </seealso>
    /// <author>  Egon Willighagen <egonw@sci.kun.nl>
    /// 
    /// </author>
    public interface IChemObjectIO
    {
        /// <summary> Returns the ChemFormat class for this IO class.</summary>
        IResourceFormat Format
        {
            get;
        }

        /// <summary> Returns an Array of IOSettings defined by this reader.</summary>
        IOSetting[] IOSettings
        {
            get;
        }

        /// <summary> Returns whether the given IChemObject can be read or not.</summary>
        bool accepts(Type classObject);

        /// <summary> Closes the Reader's resources.</summary>
        void close();

        /// <summary> Adds a ChemObjectIOListener to this ChemObjectReader.
        /// 
        /// </summary>
        /// <param name="listener">the reader listener to add.
        /// </param>
        void addChemObjectIOListener(IChemObjectIOListener listener);

        /// <summary> Removes a ChemObjectIOListener from this ChemObjectReader.
        /// 
        /// </summary>
        /// <param name="listener">the reader listener to remove.
        /// </param>
        void removeChemObjectIOListener(IChemObjectIOListener listener);
    }
}