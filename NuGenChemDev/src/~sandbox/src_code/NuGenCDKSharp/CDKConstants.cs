/* $RCSfile$
* $Author: egonw $    
* $Date: 2006-07-05 22:18:23 +0200 (Wed, 05 Jul 2006) $    
* $Revision: 6608 $
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

namespace Org.OpenScience.CDK
{

    /// <summary> An interface providing predefined values for a number of
    /// constants used throughout the CDK. Classes using these constants should
    /// <b>not</b> implement this interface, but use it like:
    /// <pre>
    /// double singleBondOrder = CDKConstants.BONDORDER_SINGLE;
    /// </pre>
    /// 
    /// <p>The lazyCreation patch has been applied to this class.
    /// 
    /// </summary>
    /// <cdk.module>   core </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  bond order </cdk.keyword>
    /// <cdk.keyword>  stereochemistry </cdk.keyword>
    public class CDKConstants
    {

        //UPGRADE_NOTE: Final was removed from the declaration of 'UNSET '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly int UNSET = System.Int32.MinValue;

        /// <summary>A bond of degree 1.0. </summary>
        public const double BONDORDER_SINGLE = 1.0;

        /// <summary>A bond of degree 1.5. To indicate aromaticity, the flag ISAROMATIC should be used instead.
        /// This constant is used for specific file formats only, and should generally not be used. 
        /// </summary>
        public const double BONDORDER_AROMATIC = 1.5;

        /// <summary>A bond of degree 2.0. </summary>
        public const double BONDORDER_DOUBLE = 2.0;

        /// <summary>A bond of degree 3.0. </summary>
        public const double BONDORDER_TRIPLE = 3.0;

        /// <summary>A bonds which end is above the drawing plane. </summary>
        public const int STEREO_BOND_UP = 1;
        /// <summary>A bonds which start is above the drawing plane. </summary>
        public const int STEREO_BOND_UP_INV = 2;

        /// <summary>A bonds for which the stereochemistry is undefined. </summary>
        public const int STEREO_BOND_UNDEFINED = 4;

        /// <summary>A bonds for which there is no stereochemistry. </summary>
        public const int STEREO_BOND_NONE = 0;

        /// <summary>A bonds which end is below the drawing plane.
        /// The bond is draw from the first to the second bond atom.
        /// </summary>
        public const int STEREO_BOND_DOWN = -1;
        /// <summary>A bonds which end is below the drawing plane.
        /// The bond is draw from the second to the first bond atom.
        /// </summary>
        public const int STEREO_BOND_DOWN_INV = -2;

        /// <summary>A positive atom parity. </summary>
        public const int STEREO_ATOM_PARITY_PLUS = 1;
        /// <summary>A negative atom parity. </summary>
        public const int STEREO_ATOM_PARITY_MINUS = -1;
        /// <summary>A undefined atom parity. </summary>
        public const int STEREO_ATOM_PARITY_UNDEFINED = 0;

        /// <summary>A undefined hybridization. </summary>
        public const int HYBRIDIZATION_UNSET = 0;
        /// <summary>A geometry of neighboring atoms when an s orbital is hybridized
        /// with one p orbital. 
        /// </summary>
        public const int HYBRIDIZATION_SP1 = 1;
        /// <summary>A geometry of neighboring atoms when an s orbital is hybridized
        /// with two p orbitals. 
        /// </summary>
        public const int HYBRIDIZATION_SP2 = 2;
        /// <summary>A geometry of neighboring atoms when an s orbital is hybridized
        /// with three p orbitals. 
        /// </summary>
        public const int HYBRIDIZATION_SP3 = 3;
        /// <summary>A geometry of neighboring atoms when an s orbital is hybridized
        /// with three p orbitals with one d orbital. 
        /// </summary>
        public const int HYBRIDIZATION_SP3D1 = 4;
        /// <summary>A geometry of neighboring atoms when an s orbital is hybridized
        /// with three p orbitals with two d orbitals. 
        /// </summary>
        public const int HYBRIDIZATION_SP3D2 = 5;
        /// <summary>A geometry of neighboring atoms when an s orbital is hybridized
        /// with three p orbitals with three d orbitals. 
        /// </summary>
        public const int HYBRIDIZATION_SP3D3 = 6;
        /// <summary>A geometry of neighboring atoms when an s orbital is hybridized
        /// with three p orbitals with four d orbitals. 
        /// </summary>
        public const int HYBRIDIZATION_SP3D4 = 7;
        /// <summary>A geometry of neighboring atoms when an s orbital is hybridized
        /// with three p orbitals with five d orbitals. 
        /// </summary>
        public const int HYBRIDIZATION_SP3D5 = 8;
        /// <summary> Carbon NMR shift contant for use as a key in the
        /// IChemObject.physicalProperties hashtable.
        /// </summary>
        /// <seealso cref="org.openscience.cdk.ChemObject">
        /// </seealso>
        public const System.String NMRSHIFT_CARBON = "carbon nmr shift";
        /// <summary>Hydrogen NMR shift contant for use as a key in the
        /// IChemObject.physicalProperties hashtable.
        /// </summary>
        /// <seealso cref="org.openscience.cdk.ChemObject">
        /// </seealso>
        public const System.String NMRSHIFT_HYDROGEN = "hydrogen nmr shift";
        /// <summary>Nitrogen NMR shift contant for use as a key in the
        /// IChemObject.physicalProperties hashtable.
        /// </summary>
        /// <seealso cref="org.openscience.cdk.ChemObject">
        /// </seealso>
        public const System.String NMRSHIFT_NITROGEN = "nitrogen nmr shift";

        /// <summary>Phosphorus NMR shift contant for use as a key in the
        /// IChemObject.physicalProperties hashtable.
        /// </summary>
        /// <seealso cref="org.openscience.cdk.ChemObject">
        /// </seealso>
        public const System.String NMRSHIFT_PHOSPORUS = "phosphorus nmr shift";

        /// <summary>Fluorine NMR shift contant for use as a key in the
        /// IChemObject.physicalProperties hashtable.
        /// </summary>
        /// <seealso cref="org.openscience.cdk.ChemObject">
        /// </seealso>
        public const System.String NMRSHIFT_FLUORINE = "fluorine nmr shift";

        /// <summary>Deuterium NMR shift contant for use as a key in the
        /// IChemObject.physicalProperties hashtable.
        /// </summary>
        /// <seealso cref="org.openscience.cdk.ChemObject">
        /// </seealso>
        public const System.String NMRSHIFT_DEUTERIUM = "deuterium nmr shift";


        /// <summary>*************************************
        /// Some predefined flags - keep the     *
        /// numbers below 50 free for other      *
        /// purposes                             *
        /// **************************************
        /// </summary>

        /// <summary>Flag that is set if the chemobject is placed (somewhere).</summary>
        public const int ISPLACED = 0;
        /// <summary>Flag that is set when the chemobject is part of a ring.</summary>
        public const int ISINRING = 1;
        /// <summary>Flag that is set when the chemobject is part of a ring.</summary>
        public const int ISNOTINRING = 2;
        /// <summary>Flag that is set if a chemobject is part of an alipahtic chain.</summary>
        public const int ISALIPHATIC = 3;
        /// <summary>Flag is set if chemobject has been visited.</summary>
        public const int VISITED = 4; // Use in tree searches
        /// <summary>Flag is set if chemobject is part of an aromatic system. </summary>
        public const int ISAROMATIC = 5;
        /// <summary>Flag is set if chemobject is part of a conjugated system. </summary>
        public const int ISCONJUGATED = 6;
        /// <summary>Flag is set if a chemobject is mapped to another chemobject.
        /// It is used for example in subgraph isomorphism search.
        /// </summary>
        public const int MAPPED = 7;

        /// <summary>Set to true if the atom is an hydrogen bond donor. </summary>
        public const int IS_HYDROGENBOND_DONOR = 8;
        /// <summary>Set to true if the atom is an hydrogen bond acceptor. </summary>
        public const int IS_HYDROGENBOND_ACCEPTOR = 9;

        /// <summary>Flag is set if a chemobject has reactive center.
        /// It is used for example in reaction.
        /// </summary>
        public const int REACTIVE_CENTER = 10;
        /// <summary> Maximum flags array index.</summary>
        public const int MAX_FLAG_INDEX = 10;
        /// <summary> Flag used for JUnit testing the pointer functionality.</summary>
        public const int DUMMY_POINTER = 1;
        /// <summary> Maximum pointers array index.</summary>
        public const int MAX_POINTER_INDEX = 1;

        /// <summary>*************************************
        /// Some predefined property names for    *
        /// ChemObjects                           *
        /// **************************************
        /// </summary>

        /// <summary>The title for a IChemObject. </summary>
        public const System.String TITLE = "Title";

        /// <summary>A remark for a IChemObject.</summary>
        public const System.String REMARK = "Remark";

        /// <summary>A String comment. </summary>
        public const System.String COMMENT = "Comment";

        /// <summary>A List of names. </summary>
        public const System.String NAMES = "Names";

        /// <summary>A List of annotation remarks. </summary>
        public const System.String ANNOTATIONS = "Annotations";

        /// <summary>A description for a IChemObject. </summary>
        public const System.String DESCRIPTION = "Description";


        /// <summary>*************************************
        /// Some predefined property names for    *
        /// Molecules                             *
        /// **************************************
        /// </summary>

        /// <summary>The IUPAC compatible name generated with AutoNom. </summary>
        public const System.String AUTONOMNAME = "AutonomName";

        /// <summary>The Beilstein Registry Number. </summary>
        public const System.String BEILSTEINRN = "BeilsteinRN";

        /// <summary>The CAS Registry Number. </summary>
        public const System.String CASRN = "CasRN";

        /// <summary>A set of all rings computed for this molecule. </summary>
        public const System.String ALL_RINGS = "AllRings";

        /// <summary>A smallest set of smallest rings computed for this molecule. </summary>
        public const System.String SMALLEST_RINGS = "SmallestRings";

        /// <summary>The essential rings computed for this molecule. 
        /// The concept of Essential Rings is defined in 
        /// SSSRFinder
        /// </summary>
        public const System.String ESSENTIAL_RINGS = "EssentialRings";

        /// <summary>The relevant rings computed for this molecule. 
        /// The concept of relevant Rings is defined in 
        /// SSSRFinder
        /// </summary>
        public const System.String RELEVANT_RINGS = "RelevantRings";


        /// <summary>*************************************
        /// Some predefined property names for    *
        /// Atoms                                 *
        /// **************************************
        /// </summary>

        /// <summary>The Isotropic Shielding, usually calculated by
        /// a quantum chemistry program like Gaussian.
        /// This is a property used for calculating NMR chemical
        /// shifts by subtracting the value from the 
        /// isotropic shielding value of a standard (e.g. TMS).
        /// </summary>
        public const System.String ISOTROPIC_SHIELDING = "IsotropicShielding";

        /// <summary>*************************************
        /// Some predefined property names for    *
        /// AtomTypes                             *
        /// **************************************
        /// </summary>

        /// <summary>Used as property key for indicating the ring size of a certain atom type. </summary>
        public const System.String PART_OF_RING_OF_SIZE = "Part of ring of size";

        /// <summary>Used as property key for indicating the chemical group of a certain atom type. </summary>
        public const System.String CHEMICAL_GROUP_CONSTANT = "Chemical Group";

        /// <summary>Used as property key for indicating the HOSE code for a certain atom type. </summary>
        public const System.String SPHERICAL_MATCHER = "HOSE code spherical matcher";
    }
}