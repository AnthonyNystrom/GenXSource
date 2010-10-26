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
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK
{
    /// <summary> Base class for entities containing electrons, like bonds, orbitals, lone-pairs.
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  orbital </cdk.keyword>
    /// <cdk.keyword>  lone-pair </cdk.keyword>
    /// <cdk.keyword>  bond </cdk.keyword>
    [Serializable]
    public class ElectronContainer : ChemObject, IElectronContainer
    {
        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = -2207894536767670743L;

        /// <summary>Number of electrons in the ElectronContainer. </summary>
        protected internal int electronCount;

        /// <summary> Constructs an empty ElectronContainer.</summary>
        public ElectronContainer()
        {
            electronCount = 0;
        }

        /// <summary> Returns the number of electrons in this electron container.
        /// 
        /// </summary>
        /// <returns> The number of electrons in this electron container.
        /// 
        /// </returns>
        /// <seealso cref="setElectronCount">
        /// </seealso>
        public virtual int getElectronCount()
        {
            return this.electronCount;
        }


        /// <summary> Sets the number of electorn in this electron container.
        /// 
        /// </summary>
        /// <param name="electronCount">The number of electrons in this electron container.
        /// 
        /// </param>
        /// <seealso cref="getElectronCount">
        /// </seealso>
        public virtual void setElectronCount(int electronCount)
        {
            this.electronCount = electronCount;
            notifyChanged();
        }
    }
}