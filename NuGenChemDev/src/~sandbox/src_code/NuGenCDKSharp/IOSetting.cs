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

namespace Org.OpenScience.CDK.IO.Setting
{
    /// <summary> An interface for reader settings. It is subclassed by implementations,
    /// one for each type of field, e.g. IntReaderSetting.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>  Egon Willighagen <egonw@sci.kun.nl>
    /// </author>
    public abstract class IOSetting
    {
        virtual public System.String Name
        {
            get
            {
                return this.name;
            }

        }
        virtual public System.String Question
        {
            get
            {
                return this.question;
            }

        }
        virtual public System.String DefaultSetting
        {
            get
            {
                return this.setting;
            }

        }
        virtual public int Level
        {
            get
            {
                return this.level;
            }

        }

        /// <summary>The levels available:
        /// HIGH         important question
        /// MEDIUM
        /// LOW          unimportant question
        /// </summary>
        public const int HIGH = 0;
        public const int MEDIUM = 1;
        public const int LOW = 2;

        protected internal int level;
        protected internal System.String name;
        protected internal System.String question;
        protected internal System.String setting;

        /// <summary> The default constructor that sets this field. All textual 
        /// information is supposed to be English. Localization is taken care
        /// off by the ReaderConfigurator.
        /// 
        /// </summary>
        /// <param name="name">          Name of the setting
        /// </param>
        /// <param name="level">         Level at which question is asked
        /// </param>
        /// <param name="question">      Question that is poped to the user when the 
        /// ReaderSetting needs setting
        /// </param>
        /// <param name="defaultSetting">The default setting, used if not overwritten
        /// by a user
        /// </param>
        public IOSetting(System.String name, int level, System.String question, System.String defaultSetting)
        {
            this.level = level;
            this.name = name;
            this.question = question;
            this.setting = defaultSetting;
        }

        /// <summary> Sets the setting for a certain question. It will throw
        /// a CDKException when the setting is not valid.    
        /// 
        /// </summary>
        public virtual void setSetting(System.String setting)
        {
            // by default, except all input, so no setting checking                     
            this.setting = setting;
        }

        /// <summary> Sets the setting for a certain question. It will throw
        /// a CDKException when the setting is not valid.    
        /// 
        /// </summary>
        public virtual System.String getSetting()
        {
            // by default, except all input, so no setting checking                     
            return this.setting;
        }
    }
}