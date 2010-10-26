/*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
*  $Revision: 5855 $
*
*  Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
*
*  Contact: cdk-devel@lists.sourceforge.net
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public License
*  as published by the Free Software Foundation; either version 2.1
*  of the License, or (at your option) any later version.
*
*  This program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public License
*  along with this program; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using System.Collections;

namespace Org.OpenScience.CDK
{
    /// <summary>  An Enumeration of the Atoms in a particular AtomContainer.
    /// The typical use is:
    /// 
    /// <pre>
    /// AtomEnumeration atoms = ((Molecule)molecule).atoms();
    /// while (atoms.hasMoreElements()) {
    /// Atom a = (Atom)atoms.nextElement();
    /// // do something with atom
    /// }
    /// </pre>
    /// 
    /// <p>The Enumeration does not clone the AtomContainer from which
    /// it is constructed, which might lead to errors.
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>      steinbeck
    /// </author>
    /// <cdk.created>     2000-10-02 </cdk.created>
    [Serializable]
    public class AtomEnumeration : ICloneable, IEnumerator
    {
        private System.Object tempAuxObj;
        public virtual bool MoveNext()
        {
            bool result = hasMoreElements();
            if (result)
            {
                tempAuxObj = nextElement();
            }
            return result;
        }
        public virtual void Reset()
        {
            tempAuxObj = null;
        }
        public virtual System.Object Current
        {
            get
            {
                return tempAuxObj;
            }

        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = -1792810428672771080L;

        /// <summary>Counts the current element. </summary>
        private int atomEnumerationCounter = 0;
        /// <summary>Contains the atoms to enumerate. </summary>
        private AtomContainer container;

        /// <summary>  Constructs a new AtomEnumeration.
        /// 
        /// </summary>
        /// <param name="container"> AtomContainer which contains the atoms
        /// </param>
        public AtomEnumeration(AtomContainer container)
        {
            this.container = container;
        }

        /// <summary>  Returns true if the Enumeration still has atoms left.</summary>
        //UPGRADE_NOTE: The equivalent of method 'java.util.Enumeration.hasMoreElements' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
        public virtual bool hasMoreElements()
        {
            if (container.AtomCount > atomEnumerationCounter)
            {
                return true;
            }
            return false;
        }

        /// <summary>  Returns next atom in Enumeration.
        /// 
        /// </summary>
        /// <returns> Uncasted Atom class.
        /// </returns>
        //UPGRADE_NOTE: The equivalent of method 'java.util.Enumeration.nextElement' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
        public virtual System.Object nextElement()
        {
            atomEnumerationCounter++;
            return container.getAtomAt(atomEnumerationCounter - 1);
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
        virtual public System.Object Clone()
        {
            return null;
        }
    }
}