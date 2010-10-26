/* $RCSfile$
* $Author: tohel $
* $Date: 2006-07-05 16:46:29 +0200 (Wed, 05 Jul 2006) $
* $Revision: 6592 $
*
* Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
*
* Contact: cdk-devel@lists.sourceforge.net
*
* This library is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public
* License as published by the Free Software Foundation; either
* version 2.1 of the License, or (at your option) any later version.
*
*  This library is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
* Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public
* License along with this library; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using Org.OpenScience.CDK.Tools;

namespace Org.OpenScience.CDK.IO.Formats
{
    /// <summary> See <a http://wwmm.ch.cam.ac.uk/moin/ChemicalMarkupLanguage"></a></summary>
    /// <cdk.module>  io </cdk.module>
    /// <cdk.set>     io-formats </cdk.set>
    public class CMLFormat : IChemFormatMatcher
    {
        virtual public System.String FormatName
        {
            get
            {
                return "Chemical Markup Language";
            }

        }
        virtual public System.String MIMEType
        {
            get
            {
                return "chemical/x-cml";
            }

        }
        virtual public System.String PreferredNameExtension
        {
            get
            {
                return NameExtensions[0];
            }

        }
        virtual public System.String[] NameExtensions
        {
            get
            {
                return new System.String[] { "cml", "xml" };
            }

        }
        virtual public System.String ReaderClassName
        {
            get
            {
                return "Org.OpenScience.CDK.IO.CMLReader";
            }

        }
        virtual public System.String WriterClassName
        {
            get
            {
                return "Org.OpenScience.CDK.IO.CMLWriter";
            }

        }
        virtual public bool XMLBased
        {
            get
            {
                return true;
            }

        }
        virtual public int SupportedDataFeatures
        {
            get
            {
                return DataFeatures.HAS_2D_COORDINATES | DataFeatures.HAS_3D_COORDINATES | DataFeatures.HAS_ATOM_PARTIAL_CHARGES | DataFeatures.HAS_ATOM_FORMAL_CHARGES | DataFeatures.HAS_ATOM_MASS_NUMBERS | DataFeatures.HAS_ATOM_ISOTOPE_NUMBERS | DataFeatures.HAS_GRAPH_REPRESENTATION | DataFeatures.HAS_ATOM_ELEMENT_SYMBOL;
            }

        }
        virtual public int RequiredDataFeatures
        {
            get
            {
                return DataFeatures.NONE;
            }

        }

        public CMLFormat()
        {
        }

        public virtual bool matches(int lineNumber, System.String line)
        {
            if ((line.IndexOf("http://www.xml-cml.org/schema") != -1) || (line.IndexOf("<atom") != -1) || (line.IndexOf("<molecule") != -1) || (line.IndexOf("<reaction") != -1) || (line.IndexOf("<cml") != -1) || (line.IndexOf("<bond") != -1))
            {
                return true;
            }
            return false;
        }
    }
}