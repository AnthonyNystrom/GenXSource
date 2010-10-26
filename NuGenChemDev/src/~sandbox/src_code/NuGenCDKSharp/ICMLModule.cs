
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
using Support;
using Org.OpenScience.CDK.IO.CML.CDOPI;

namespace Org.OpenScience.CDK.IO.CML
{

    /// <summary> This interface describes the procedures classes must implement to be plugable
    /// into the CMLHandler. Most procedures reflect those in SAX2.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>  Egon Willighagen <egonw@sci.kun.nl>
    /// 
    /// </author>
    public interface ICMLModule
    {

        void startDocument();
        void endDocument();
        void startElement(CMLStack xpath, System.String uri, System.String local, System.String raw, SaxAttributesSupport atts);
        void endElement(CMLStack xpath, System.String uri, System.String local, System.String raw);
        void characterData(CMLStack xpath, char[] ch, int start, int length);

        IChemicalDocumentObject returnCDO();

        void inherit(ICMLModule conv);
    }
}