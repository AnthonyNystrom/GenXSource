/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
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
*
*/
using System;

namespace Org.OpenScience.CDK.IO.CML.CDOPI
{

    /// <author>  Egon Willighagen <egonw@sci.kun.nl>
    /// 
    /// </author>
    /// <cdk.module>  io </cdk.module>
    public interface IChemicalDocumentObject
    {

        /// <summary> Called just before XML parsing is started.</summary>
        void startDocument();

        /// <summary> Called just after XML parsing has ended.</summary>
        void endDocument();

        /// <summary> Sets a property for this document.
        /// 
        /// </summary>
        /// <param name="type"> Type of the property.
        /// </param>
        /// <param name="value">Value of the property.
        /// </param>
        void setDocumentProperty(System.String type, System.String value_Renamed);

        /// <summary> Start the process of adding a new object to the CDO of a certain type.
        /// 
        /// </summary>
        /// <param name="objectType"> Type of the object being added.
        /// </param>
        void startObject(System.String objectType);

        /// <summary> End the process of adding a new object to the CDO of a certain type.
        /// 
        /// </summary>
        /// <param name="objectType"> Type of the object being added.
        /// </param>
        void endObject(System.String objectType);

        /// <summary> Sets a property of the object being added.
        /// 
        /// </summary>
        /// <param name="objectType">         Type of the object being added.
        /// </param>
        /// <param name="propertyType">       Type of the property being set.
        /// </param>
        /// <param name="propertyValue">      Value of the property being set.
        /// </param>
        void setObjectProperty(System.String objectType, System.String propertyType, System.String propertyValue);

        /// <summary> The next procedure must be implemented by each CDO and
        /// return a CDOAcceptedObjects class with the names of the 
        /// objects that can be handled.
        /// 
        /// </summary>
        CDOAcceptedObjects acceptObjects();
    }
}