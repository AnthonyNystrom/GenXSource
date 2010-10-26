/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-06-07 11:41:42 +0200 (Wed, 07 Jun 2006) $
* $Revision: 6349 $
*
* Copyright (C) 2002-2006  The Jmol Development Team
*
* Contact: cdk-devel@lists.sourceforge.net
*
*  This library is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public
*  License as published by the Free Software Foundation; either
*  version 2.1 of the License, or (at your option) any later version.
*
*  This library is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
*  Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public
*  License along with this library; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Signals that something has happened in a file reader. This class is
    /// primarily in place for future development when additional information
    /// may be passed to <code>ReaderListener</code>s.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>  Bradley A. Smith <bradley@baysmith.com>
    /// </author>
    [Serializable]
    public class ReaderEvent : System.EventArgs
    {
        private const long serialVersionUID = 660541244342274716L;

        /// <summary> Creates a reader event.
        /// 
        /// </summary>
        /// <param name="source">the object on which the event initially occurred.
        /// </param>
        public ReaderEvent(System.Object source)
            : base()
        {
        }
    }
}