/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
*
* Copyright (C) 2003-2006  The CDK Development Team
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
using Org.OpenScience.CDK.Exception;

namespace Org.OpenScience.CDK.IO.Setting
{
    /// <summary> An class for a reader setting which must be of type String.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>  Egon Willighagen <egonw@sci.kun.nl>
    /// </author>
    public class BooleanIOSetting : IOSetting
    {
        virtual public bool Set
        {
            get
            {
                if (setting.Equals("true"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public BooleanIOSetting(System.String name, int level, System.String question, System.String defaultSetting)
            : base(name, level, question, defaultSetting)
        {
        }

        /// <summary> Sets the setting for a certain question. The setting
        /// is a boolean, and it accepts only "true" and "false".
        /// </summary>
        public override void setSetting(System.String setting)
        {
            if (setting.Equals("true") || setting.Equals("false"))
            {
                this.setting = setting;
            }
            else if (setting.Equals("yes") || setting.Equals("y"))
            {
                this.setting = "true";
            }
            else if (setting.Equals("no") || setting.Equals("n"))
            {
                this.setting = "false";
            }
            else
            {
                throw new CDKException("Setting " + setting + " is not a boolean.");
            }
        }
    }
}