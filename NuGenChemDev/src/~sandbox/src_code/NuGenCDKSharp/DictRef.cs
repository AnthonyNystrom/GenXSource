/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-06-07 11:41:42 +0200 (Wed, 07 Jun 2006) $
* $Revision: 6349 $
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

    /// <summary> Object that can be used as key in IChemObject.setProperty(key, value) to
    /// denote that this property is a dictionary reference for this IChemObject.
    /// 
    /// </summary>
    /// <author>       Egon Willighagen
    /// </author>
    /// <cdk.created>  2003-08-24 </cdk.created>
    /// <cdk.module>   standard </cdk.module>
    [Serializable]
    public class DictRef : System.ICloneable
    {
        virtual public System.String Type
        {
            get
            {
                return type;
            }

        }

        private const long serialVersionUID = -3691244168587563625L;

        internal System.String type;
        internal System.String dictRef;

        public DictRef(System.String type, System.String dictRef)
        {
            this.type = type;
            this.dictRef = dictRef;
        }

        public virtual System.String getDictRef()
        {
            return dictRef;
        }

        public override System.String ToString()
        {
            return "DictRef{T=" + this.type + ", R=" + dictRef + "}";
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
        virtual public System.Object Clone()
        {
            return null;
        }
    }
}