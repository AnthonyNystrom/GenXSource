/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
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

namespace Org.OpenScience.CDK.IO.Listener
{
    /// <summary> Allows monitoring of progress of file reader activities.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>  Bradley A. Smith <bradley@baysmith.com>
    /// </author>
    public delegate void IReaderListenerDelegate(Object sender, ReaderEvent IReaderListenerDelegateParam);
    public interface IReaderListener : IChemObjectIOListener
    {
        /// <summary> Indicates that a new frame has been read.
        /// 
        /// </summary>
        /// <param name="event">information about the event.
        /// </param>
        void frameRead(System.Object event_sender, ReaderEvent event_Renamed);
    }
}