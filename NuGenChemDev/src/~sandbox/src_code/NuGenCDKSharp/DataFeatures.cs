/* $RCSfile: $
* $Author: egonw $
* $Date: 2006-05-09 21:32:32 +0200 (Tue, 09 May 2006) $  
* $Revision: 6204 $
*
* Copyright (C) 2006  The Chemistry Development Kit (CDK) project
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

namespace Org.OpenScience.CDK.Tools
{
    /// <summary> Class with constants for possible data features defined in the
    /// a Data Feature Ontology. Actual integers are random
    /// and should <b>not</b> be used directly.
    /// 
    /// <p>To test wether a IChemFormat supports a certain feature, the
    /// following code can be used:
    /// <pre>
    /// int features = new XYZFormat().getSupportedDataFeatures();
    /// boolean has3DCoords = (features & HAS_3D_COORDINATES) == HAS_3D_COORDINATES;
    /// </pre>
    /// 
    /// <p>This list of constants matches the latest <a href="http://qsar.sourceforge.net/ontologies/data-features/index.xhtml"
    /// >Blue Obelisk Data Features Ontology</a>.
    /// 
    /// </summary>
    /// <author>      Egon Willighagen <ewilligh@uni-koeln.de>
    /// </author>
    /// <cdk.module>  core </cdk.module>
    /// <summary> 
    /// </summary>
    public class DataFeatures
    {
        public const int NONE = 0;

        // The int allows for up to 750 different properties. Should
        // be enough for now.

        // COORDINATE SYSTEMS

        /// <cdk.dictref>  bodf:coordinates2D  </cdk.dictref>
        public const int HAS_2D_COORDINATES = 1 << 0;
        /// <cdk.dictref>  bodf:coordinates3D  </cdk.dictref>
        public const int HAS_3D_COORDINATES = 1 << 1;
        /// <cdk.dictref>  bodf:fractionalUnitCellCoordinatesCoordinates  </cdk.dictref>
        public const int HAS_FRACTIONAL_CRYSTAL_COORDINATES = 1 << 2;

        // ATOMIC FEATURES
        //                      HAS_ATOMS ??

        /// <cdk.dictref>  bodf:hasAtomElementSymbol  </cdk.dictref>
        public const int HAS_ATOM_ELEMENT_SYMBOL = 1 << 3;
        /// <cdk.dictref>  bodf:partialAtomicCharges  </cdk.dictref>
        public const int HAS_ATOM_PARTIAL_CHARGES = 1 << 4;
        /// <cdk.dictref>  bodf:formalAtomicCharges  </cdk.dictref>
        public const int HAS_ATOM_FORMAL_CHARGES = 1 << 5;
        /// <summary>FIXME: NOT YET IN BODF !!! *</summary>
        public const int HAS_ATOM_HYBRIDIZATIONS = 1 << 6;
        /// <cdk.dictref>  bodf:massNumbers  </cdk.dictref>
        public const int HAS_ATOM_MASS_NUMBERS = 1 << 7;
        /// <cdk.dictref>  bodf:isotopeNumbers  </cdk.dictref>
        public const int HAS_ATOM_ISOTOPE_NUMBERS = 1 << 8;

        // GRAPH FEATURES

        /// <cdk.dictref>  bodf:graphRepresentation  </cdk.dictref>
        public const int HAS_GRAPH_REPRESENTATION = 1 << 9;
        /// <cdk.dictref>  bodf:dietzRepresentation  </cdk.dictref>
        public const int HAS_DIETZ_REPRESENTATION = 1 << 10;

        // MODEL FEATURES

        /// <summary>FIXME: NOT YET IN BODF !!! *</summary>
        public const int HAS_UNITCELL_PARAMETERS = 1 << 11;
        /// <summary>FIXME: NOT YET IN BODF !!! *</summary>
        public const int HAS_REACTIONS = 1 << 12;
    }
}