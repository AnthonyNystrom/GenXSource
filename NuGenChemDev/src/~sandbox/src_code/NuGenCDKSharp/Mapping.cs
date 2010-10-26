/* $RCSfile$
* $Author: egonw $
* $Date: 2006-05-04 01:10:39 +0200 (Thu, 04 May 2006) $
* $Revision: 6153 $
*
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
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
    /// <summary> A Mapping is an relation between two ChemObjects in a non-chemical
    /// entity. It is not a Bond, nor a Association, merely a relation.
    /// An example of such a mapping, is the mapping between corresponding atoms
    /// in a Reaction.
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  reaction, atom mapping </cdk.keyword>
    /// <summary> 
    /// </summary>
    /// <author>   Egon Willighagen
    /// </author>
    /// <cdk.created>  2003-08-16 </cdk.created>
    [Serializable]
    public class Mapping : ChemObject, System.ICloneable, IMapping
    {
        /// <summary> Returns an array of the two IChemObject's.
        /// 
        /// </summary>
        /// <returns> An array of two IChemObject's that define the mapping
        /// </returns>
        virtual public IChemObject[] RelatedChemObjects
        {
            get
            {
                return (IChemObject[])relation;
            }

        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = -6541915644492043503L;

        private IChemObject[] relation;

        /// <summary> Constructs an unconnected lone pair.
        /// 
        /// </summary>
        /// <param name="objectOne">The first IChemObject of the mapping
        /// </param>
        /// <param name="objectTwo">The second IChemObject of the mapping
        /// </param>
        public Mapping(IChemObject objectOne, IChemObject objectTwo)
        {
            relation = new IChemObject[2];
            relation[0] = objectOne;
            relation[1] = objectTwo;
        }

        /// <summary> Clones this <code>Mapping</code> and the mapped <code>IChemObject</code>s.
        /// 
        /// </summary>
        /// <returns>  The cloned object
        /// </returns>
        public override System.Object Clone()
        {
            Mapping clone = (Mapping)base.Clone();
            // clone the related IChemObject's
            if (relation != null)
            {
                ((Mapping)clone).relation = new IChemObject[relation.Length];
                for (int f = 0; f < relation.Length; f++)
                {
                    if (relation[f] != null)
                    {
                        ((Mapping)clone).relation[f] = (IChemObject)relation[f].Clone();
                    }
                }
            }
            return clone;
        }
    }
}