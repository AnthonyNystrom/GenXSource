/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-06-07 11:41:42 +0200 (Wed, 07 Jun 2006) $
* $Revision: 6349 $
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

namespace Org.OpenScience.CDK.Events
{
    /// <summary> Event fired by cdk classes to their registered listeners
    /// in case something changes within them.
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    [Serializable]
    public class ChemObjectChangeEvent : EventArgs, Org.OpenScience.CDK.Interfaces.IChemObjectChangeEvent
    {
        private const long serialVersionUID = 5418604788783986725L;

        private Object source;

        /// <summary> Constructs a ChemObjectChangeEvent with a reference 
        /// to the object where it originated.
        /// 
        /// </summary>
        /// <param name="source">The reference to the object where this change event originated
        /// </param>
        public ChemObjectChangeEvent(Object source)
            : base()
        {
            this.source = source;
        }

        #region IChemObjectChangeEvent Members

        public object Source
        {
            get { return source; }
        }

        #endregion
    }
}