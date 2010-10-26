/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $
* $Revision: 6672 $
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

namespace Org.OpenScience.CDK
{
    /// <summary> A Monomer is an AtomContainer which stores additional monomer specific 
    /// informations for a group of Atoms.
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>      Edgar Luttmann <edgar@uni-paderborn.de>
    /// </author>
    /// <cdk.created>     2001-08-06  </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>     monomer </cdk.keyword>
    /// <summary> 
    /// </summary>
    [Serializable]
    public class Monomer : AtomContainer, IMonomer, ICloneable
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> 
        /// Retrieve the monomer name.
        /// 
        /// </summary>
        /// <returns> The name of the Monomer object
        /// 
        /// </returns>
        /// <seealso cref="setMonomerName">
        /// </seealso>
        /// <summary> 
        /// Set the name of the Monomer object.
        /// 
        /// </summary>
        /// <param name="cMonomerName"> The new name for this monomer
        /// 
        /// </param>
        /// <seealso cref="getMonomerName">
        /// </seealso>
        virtual public System.String MonomerName
        {
            get
            {
                return monomerName;
            }

            set
            {
                monomerName = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> 
        /// Retrieve the monomer type.
        /// 
        /// </summary>
        /// <returns> The type of the Monomer object
        /// 
        /// </returns>
        /// <seealso cref="setMonomerType">
        /// </seealso>
        /// <summary> 
        /// Set the type of the Monomer object.
        /// 
        /// </summary>
        /// <param name="cMonomerType"> The new type for this monomer
        /// 
        /// </param>
        /// <seealso cref="getMonomerType">
        /// </seealso>
        virtual public System.String MonomerType
        {
            get
            {
                return monomerType;
            }

            set
            {
                monomerType = value;
                notifyChanged();
            }

        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = -6084164963937650703L;

        /// <summary>The name of this monomer (e.g. Trp42). </summary>
        private System.String monomerName;
        /// <summary>The type of this monomer (e.g. TRP). </summary>
        private System.String monomerType;

        /// <summary> 
        /// Contructs a new Monomer.
        /// 
        /// </summary>
        public Monomer()
            : base()
        {
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
        virtual public System.Object Clone()
        {
            return null;
        }
    }
}