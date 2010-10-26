/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
*
* Copyright (C) 2003-2006  The CDK Project Team
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
using Org.OpenScience.CDK.IO.Setting;

namespace Org.OpenScience.CDK.IO.Listener
{
    /// <summary> Allows monitoring of progress of file reader activities.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>  Egon Willighagen
    /// </author>
    public interface IChemObjectIOListener
    {
        /// <summary> Prompted when the IO filter needs information to process the
        /// file. The ReaderListener may redirect this question to the
        /// user, disregard it (and let the Reader take the default),
        /// or answer it itself.
        /// </summary>
        void processIOSettingQuestion(IOSetting setting);
    }
}