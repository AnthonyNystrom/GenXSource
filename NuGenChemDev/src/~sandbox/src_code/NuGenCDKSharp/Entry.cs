/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
*
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
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

namespace Org.OpenScience.CDK.Dict
{
    /// <summary> Entry in a Dictionary.
    /// 
    /// </summary>
    /// <author>        Egon Willighagen <egonw@users.sf.net>
    /// </author>
    /// <cdk.created>   2003-08-23 </cdk.created>
    /// <cdk.keyword>   dictionary </cdk.keyword>
    /// <summary> 
    /// </summary>
    /// <seealso cref="Dictionary">
    /// </seealso>
    public class Entry
    {
        virtual public System.String Label
        {
            get
            {
                return this.label;
            }

            set
            {
                this.label = value;
            }

        }
        virtual public System.String ID
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value.ToLower();
            }

        }
        virtual public System.String Definition
        {
            get
            {
                return this.definition;
            }

            set
            {
                this.definition = value;
            }

        }
        virtual public System.String Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.description = value;
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <returns> Returns the rawContent.
        /// </returns>
        /// <param name="rawContent">The rawContent to set.
        /// </param>
        virtual public System.Object RawContent
        {
            get
            {
                return rawContent;
            }

            set
            {
                this.rawContent = value;
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <returns> Returns the className.
        /// </returns>
        /// <param name="className">The className to set.
        /// </param>
        virtual public System.String ClassName
        {
            get
            {
                return className;
            }

            set
            {
                this.className = value;
            }

        }

        private System.String className;
        private System.String label;
        private System.String id;
        private System.Collections.ArrayList descriptorInfo;
        private System.String definition;
        private System.String description;
        private System.Object rawContent;

        public Entry(System.String id, System.String term)
        {
            this.id = id.ToLower();
            this.label = term;
            this.descriptorInfo = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
        }

        public Entry(System.String id)
            : this(id, "")
        {
        }

        public Entry()
            : this("", "")
        {
        }

        public virtual void setDescriptorMetadata(System.String metadata)
        {
            this.descriptorInfo.Add(metadata);
        }

        public virtual System.Collections.ArrayList getDescriptorMetadata()
        {
            return this.descriptorInfo;
        }

        public override System.String ToString()
        {
            return "Entry[" + ID + "](" + Label + ")";
        }
    }
}