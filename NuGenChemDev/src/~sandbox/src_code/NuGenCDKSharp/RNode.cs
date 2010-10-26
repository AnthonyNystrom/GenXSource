/*
*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
*  $Revision: 5855 $
*
*  Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
*
*  This code has been kindly provided by Stephane Werner
*  and Thierry Hanser from IXELIS mail@ixelis.net
*
*  IXELIS sarl - Semantic Information Systems
*  17 rue des C?dres 67200 Strasbourg, France
*  Tel/Fax : +33(0)3 88 27 81 39 Email: mail@ixelis.net
*
*  CDK Contact: cdk-devel@lists.sf.net
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
*
*/
using System;
using Support;

namespace Org.OpenScience.CDK.Isomorphism.MCSS
{
    /// <summary>  Node of the resolution graphe (RGraph) An RNode represents an association
    /// betwwen two edges of the source graphs G1 and G2 that are compared. Two
    /// edges may be associated if they have at least one common feature. The
    /// association is defined outside this class. The node keeps tracks of the ID
    /// of the mapped edges (in an RMap), of its neighbours in the RGraph it belongs
    /// to and of the set of incompatible nodes (nodes that may not be along with
    /// this node in the same solution)
    /// 
    /// </summary>
    /// <author>       Stephane Werner from IXELIS mail@ixelis.net
    /// </author>
    /// <cdk.created>  2002-07-17 </cdk.created>
    /// <cdk.module>   standard </cdk.module>
    public class RNode
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary>  Gets the rMap attribute of the RNode object
        /// 
        /// </summary>
        /// <returns>    The rMap value
        /// </returns>
        /// <summary>  Sets the rMap attribute of the RNode object
        /// 
        /// </summary>
        /// <param name="rMap"> The new rMap value
        /// </param>
        virtual public RMap RMap
        {
            get
            {
                return rMap;
            }

            set
            {
                this.rMap = value;
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary>  Gets the extension attribute of the RNode object
        /// 
        /// </summary>
        /// <returns>    The extension value
        /// </returns>
        /// <summary>  Sets the extension attribute of the RNode object
        /// 
        /// </summary>
        /// <param name="extension"> The new extension value
        /// </param>
        virtual public System.Collections.BitArray Extension
        {
            get
            {
                return extension;
            }

            set
            {
                this.extension = value;
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary>  Gets the forbidden attribute of the RNode object
        /// 
        /// </summary>
        /// <returns>    The forbidden value
        /// </returns>
        /// <summary>  Sets the forbidden attribute of the RNode object
        /// 
        /// </summary>
        /// <param name="forbidden"> The new forbidden value
        /// </param>
        virtual public System.Collections.BitArray Forbidden
        {
            get
            {
                return forbidden;
            }

            set
            {
                this.forbidden = value;
            }

        }
        // G1/G2 mapping
        internal RMap rMap = null;

        // set of neighbour nodes in the RGraph
        internal System.Collections.BitArray extension = null;

        // set of incompatible nodes in the RGraph
        internal System.Collections.BitArray forbidden = null;


        /// <summary>  Constructor for the RNode object
        /// 
        /// </summary>
        /// <param name="id1"> number of the bond in the graphe 1
        /// </param>
        /// <param name="id2"> number of the bond in the graphe 2
        /// </param>
        public RNode(int id1, int id2)
        {
            rMap = new RMap(id1, id2);
            extension = new System.Collections.BitArray(64);
            forbidden = new System.Collections.BitArray(64);
        }



        /// <summary>  Returns a string representation of the RNode
        /// 
        /// </summary>
        /// <returns>    the string representation of the RNode
        /// </returns>
        public override System.String ToString()
        {
            //UPGRADE_TODO: The equivalent in .NET for method 'java.util.BitSet.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            return ("id1 : " + rMap.id1 + ", id2 : " + rMap.id2 + "\n" + "extension : " + SupportClass.BitArraySupport.ToString(extension) + "\n" + "forbiden : " + SupportClass.BitArraySupport.ToString(forbidden));
        }
    }
}