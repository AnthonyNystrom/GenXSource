/*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
*  $Revision: 5855 $
*
*  Copyright (C) 2002-2006  The Chemistry Development Kit (CDK) project
*
*  This code has been kindly provided by Stephane Werner
*  and Thierry Hanser from IXELIS mail@ixelis.net
*
*  IXELIS sarl - Semantic Information Systems
*  17 Rue des Cedres 67200 Strasbourg, France
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

namespace Org.OpenScience.CDK.Isomorphism.MCSS
{

    /// <summary>  An RMap implements the association between an edge (bond) in G1 and an edge
    /// (bond) in G2, G1 and G2 being the compared graphs in a RGraph context.
    /// 
    /// </summary>
    /// <author>       Stephane Werner, IXELIS <mail@ixelis.net>
    /// </author>
    /// <cdk.created>  2002-07-24 </cdk.created>
    /// <cdk.module>   standard </cdk.module>
    public class RMap
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary>  Gets the id1 attribute of the RMap object
        /// 
        /// </summary>
        /// <returns>    The id1 value
        /// </returns>
        /// <summary>  Sets the id1 attribute of the RMap object
        /// 
        /// </summary>
        /// <param name="id1"> The new id1 value
        /// </param>
        virtual public int Id1
        {
            get
            {
                return id1;
            }

            set
            {
                this.id1 = value;
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary>  Gets the id2 attribute of the RMap object
        /// 
        /// </summary>
        /// <returns>    The id2 value
        /// </returns>
        /// <summary>  Sets the id2 attribute of the RMap object
        /// 
        /// </summary>
        /// <param name="id2"> The new id2 value
        /// </param>
        virtual public int Id2
        {
            get
            {
                return id2;
            }

            set
            {
                this.id2 = value;
            }

        }
        internal int id1 = 0;
        internal int id2 = 0;


        /// <summary>  Constructor for the RMap
        /// 
        /// </summary>
        /// <param name="id1"> number of the edge (bond) in the graphe 1
        /// </param>
        /// <param name="id2"> number of the edge (bond) in the graphe 2
        /// </param>
        public RMap(int id1, int id2)
        {
            this.id1 = id1;
            this.id2 = id2;
        }


        /// <summary>  The equals method.
        /// 
        /// </summary>
        /// <param name="o"> The object to compare.
        /// </param>
        /// <returns>    true=if both ids equal, else false.
        /// </returns>
        public override bool Equals(System.Object o)
        {
            if (((RMap)o).id1 == id1 && ((RMap)o).id2 == id2)
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }
        //UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}