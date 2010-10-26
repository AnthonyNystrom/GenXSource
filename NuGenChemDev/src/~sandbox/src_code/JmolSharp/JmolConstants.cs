/* $RCSfile: JmolConstants.java,v $
* $Author: migueljmol $
* $Date: 2005/06/24 17:03:41 $
* $Revision: 1.68 $
*
* Copyright (C) 2003-2005  Miguel, Jmol Development, www.jmol.org
*
* Contact: miguel@jmol.org
*
*  This library is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public
*  License as published by the Free Software Foundation; either
*  version 2.1 of the License, or (at your option) any later version.
*
*  This library is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
*  Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public
*  License along with this library; if not, write to the Free Software
*  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA
*  02111-1307  USA.
*/
using System;

namespace Org.Jmol.Viewer
{
    sealed public class JmolConstants
    {
        // for now, just update this by hand
        // perhaps use ant filter later ... but mth doesn't like it :-(
        public const System.String copyright = "(C) 2005 The Jmol Development Team";
        public const System.String version = "10.00.14";
        public const System.String cvsDate = "$Date: 2005/06/24 17:03:41 $";
        //UPGRADE_NOTE: Final was removed from the declaration of 'date '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly System.String date = cvsDate.Substring(7, (23) - (7));

        public const bool officialRelease = false;

        public const sbyte LABEL_NONE = 0;
        public const sbyte LABEL_SYMBOL = 1;
        public const sbyte LABEL_TYPENAME = 2;
        public const sbyte LABEL_ATOMNO = 3;

        //UPGRADE_NOTE: Final was removed from the declaration of 'MAR_DELETED '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly short MAR_DELETED = System.Int16.MinValue;

        public const sbyte STYLE_DELETED = -1;
        public const sbyte STYLE_NONE = 0;
        public const sbyte STYLE_WIREFRAME = 1;
        public const sbyte STYLE_SHADED = 2;

        public const sbyte AXES_NONE = 0;
        public const sbyte AXES_UNIT = 1;
        public const sbyte AXES_BBOX = 2;

        public const int MOUSE_ROTATE = 0;
        public const int MOUSE_ZOOM = 1;
        public const int MOUSE_XLATE = 2;
        public const int MOUSE_PICK = 3;
        public const int MOUSE_DELETE = 4;
        public const int MOUSE_MEASURE = 5;
        public const int MOUSE_ROTATE_Z = 6;
        public const int MOUSE_SLAB_PLANE = 7;
        public const int MOUSE_POPUP_MENU = 8;

        public const sbyte MULTIBOND_NEVER = 0;
        public const sbyte MULTIBOND_WIREFRAME = 1;
        public const sbyte MULTIBOND_SMALL = 2;
        public const sbyte MULTIBOND_ALWAYS = 3;

        public const short madMultipleBondSmallMaximum = 500;

        /// <summary> picking modes</summary>
        public const int PICKING_OFF = 0;
        public const int PICKING_IDENT = 1;
        public const int PICKING_DISTANCE = 2;
        public const int PICKING_MONITOR = 3;
        public const int PICKING_ANGLE = 4;
        public const int PICKING_TORSION = 5;
        public const int PICKING_LABEL = 6;
        public const int PICKING_CENTER = 7;
        public const int PICKING_COORD = 8;
        public const int PICKING_BOND = 9;
        public const int PICKING_SELECT_ATOM = 10;
        public const int PICKING_SELECT_GROUP = 11;
        public const int PICKING_SELECT_CHAIN = 12;

        //UPGRADE_NOTE: Final was removed from the declaration of 'pickingModeNames'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly System.String[] pickingModeNames = new System.String[] { "off", "ident", "distance", "monitor", "angle", "torsion", "label", "center", "coord", "bond", "atom", "group", "chain" };

        /// <summary> listing of model types</summary>
        public const int MODEL_TYPE_OTHER = 0;
        public const int MODEL_TYPE_PDB = 1;
        public const int MODEL_TYPE_XYZ = 2;

        /// <summary> Extended Bond Definition Types
        /// 
        /// </summary>
        public const short BOND_COVALENT_SINGLE = 1;
        public const short BOND_COVALENT_DOUBLE = 2;
        public const short BOND_COVALENT_TRIPLE = 3;
        public const short BOND_COVALENT_MASK = 3;
        public const short BOND_AROMATIC_MASK = (short)((1 << 2));
        public const short BOND_AROMATIC = (1 << 2) | 1;
        public const short BOND_STEREO_MASK = (short)((3 << 3));
        public const short BOND_STEREO_NEAR = (1 << 3) | 1;
        public const short BOND_STEREO_FAR = (2 << 3) | 2;
        public const short BOND_SULFUR_MASK = (short)((1 << 5));
        public const short BOND_HBOND_SHIFT = 6;
        //UPGRADE_NOTE: Final was removed from the declaration of 'BOND_HYDROGEN_MASK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly short BOND_HYDROGEN_MASK = (short)((0x0F << BOND_HBOND_SHIFT));
        //UPGRADE_NOTE: Final was removed from the declaration of 'BOND_H_REGULAR '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly short BOND_H_REGULAR = (short)((1 << BOND_HBOND_SHIFT));
        //UPGRADE_NOTE: Final was removed from the declaration of 'BOND_H_PLUS_2 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly short BOND_H_PLUS_2 = (short)((2 << BOND_HBOND_SHIFT));
        //UPGRADE_NOTE: Final was removed from the declaration of 'BOND_H_PLUS_3 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly short BOND_H_PLUS_3 = (short)((3 << BOND_HBOND_SHIFT));
        //UPGRADE_NOTE: Final was removed from the declaration of 'BOND_H_PLUS_4 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly short BOND_H_PLUS_4 = (short)((4 << BOND_HBOND_SHIFT));
        //UPGRADE_NOTE: Final was removed from the declaration of 'BOND_H_PLUS_5 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly short BOND_H_PLUS_5 = (short)((5 << BOND_HBOND_SHIFT));
        //UPGRADE_NOTE: Final was removed from the declaration of 'BOND_H_MINUS_3 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly short BOND_H_MINUS_3 = (short)((6 << BOND_HBOND_SHIFT));
        //UPGRADE_NOTE: Final was removed from the declaration of 'BOND_H_MINUS_4 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly short BOND_H_MINUS_4 = (short)((7 << BOND_HBOND_SHIFT));
        //UPGRADE_NOTE: Final was removed from the declaration of 'BOND_H_NUCLEOTIDE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly short BOND_H_NUCLEOTIDE = (short)((8 << BOND_HBOND_SHIFT));
        public const short BOND_PARTIAL01 = (short)((1 << 10));
        public const short BOND_PARTIAL12 = (short)((1 << 11));

        public static short BOND_ALL_MASK = -1;

        //UPGRADE_NOTE: Final was removed from the declaration of 'argbsHbondType'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly int[] argbsHbondType = new int[] { unchecked((int)0xFFFF69B4), unchecked((int)0xFFFFFF00), unchecked((int)0xFFFFFFFF), unchecked((int)0xFFFF00FF), unchecked((int)0xFFFF0000), unchecked((int)0xFFFFA500), unchecked((int)0xFF00FFFF), unchecked((int)0xFF00FF00), unchecked((int)0xFFFF8080) };

        /// <summary> The default elementSymbols. Presumably the only entry which may cause
        /// confusion is element 0, whose symbol we have defined as "Xx". 
        /// </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'elementSymbols'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly System.String[] elementSymbols = new System.String[] { "Xx", "H", "He", "Li", "Be", "B", "C", "N", "O", "F", "Ne", "Na", "Mg", "Al", "Si", "P", "S", "Cl", "Ar", "K", "Ca", "Sc", "Ti", "V", "Cr", "Mn", "Fe", "Co", "Ni", "Cu", "Zn", "Ga", "Ge", "As", "Se", "Br", "Kr", "Rb", "Sr", "Y", "Zr", "Nb", "Mo", "Tc", "Ru", "Rh", "Pd", "Ag", "Cd", "In", "Sn", "Sb", "Te", "I", "Xe", "Cs", "Ba", "La", "Ce", "Pr", "Nd", "Pm", "Sm", "Eu", "Gd", "Tb", "Dy", "Ho", "Er", "Tm", "Yb", "Lu", "Hf", "Ta", "W", "Re", "Os", "Ir", "Pt", "Au", "Hg", "Tl", "Pb", "Bi", "Po", "At", "Rn", "Fr", "Ra", "Ac", "Th", "Pa", "U", "Np", "Pu", "Am", "Cm", "Bk", "Cf", "Es", "Fm", "Md", "No", "Lr", "Rf", "Db", "Sg", "Bh", "Hs", "Mt" };

        /// <summary> one larger than the last elementNumber, same as elementSymbols.length</summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'elementNumberMax '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly int elementNumberMax = elementSymbols.Length;

        private static System.Collections.Hashtable htElementMap;

        /// <param name="elementSymbol">First char must be upper case, second char accepts upper or lower case
        /// </param>
        /// <returns> elementNumber
        /// </returns>
        public static sbyte elementNumberFromSymbol(System.String elementSymbol)
        {
            if (htElementMap == null)
            {
                System.Collections.Hashtable map = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
                for (int elementNumber = elementNumberMax; --elementNumber >= 0; )
                {
                    System.String symbol = elementSymbols[elementNumber];
                    System.Int32 boxed = (System.Int32)elementNumber;
                    map[symbol] = boxed;
                    if (symbol.Length == 2)
                    {
                        symbol = "" + symbol[0] + System.Char.ToUpper(symbol[1]);
                        map[symbol] = boxed;
                    }
                    if (elementNumber == 1)
                    {
                        // special case for D = deuterium
                        //
                        // We can put in a special table for these in the future
                        // if there are more 'element symbol aliases'
                        map["D"] = boxed;
                    }
                }
                htElementMap = map;
            }
            if (elementSymbol == null)
                return 0;
            System.Int32 boxedAtomicNumber = (System.Int32)htElementMap[elementSymbol];
            //UPGRADE_TODO: The 'System.Int32' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
            if (boxedAtomicNumber != null)
                return (sbyte)boxedAtomicNumber;
            System.Console.Out.WriteLine("" + elementSymbol + "' is not a recognized symbol");
            return 0;
        }

        //UPGRADE_NOTE: Final was removed from the declaration of 'elementNames'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly System.String[] elementNames = new System.String[] { "unknown", "hydrogen", "helium", "lithium", "beryllium", "boron", "carbon", "nitrogen", "oxygen", "fluorine", "neon", "sodium", "magnesium", "aluminum", "silicon", "phosphorus", "sulfur", "chlorine", "argon", "potassium", "calcium", "scandium", "titanium", "vanadium", "chromium", "manganese", "iron", "cobalt", "nickel", "copper", "zinc", "gallium", "germanium", "arsenic", "selenium", "bromine", "krypton", "rubidium", "strontium", "yttrium", "zirconium", "niobium", "molybdenum", "technetium", "ruthenium", "rhodium", "palladium", "silver", "cadmium", "indium", "tin", "antimony", "tellurium", "iodine", "xenon", "cesium", "barium", "lanthanum", "cerium", "praseodymium", "neodymium", "promethium", "samarium", "europium", "gadolinium", "terbium", "dysprosium", "holmium", "erbium", "thulium", "ytterbium", "lutetium", "hafnium", "tantalum", "tungsten", "rhenium", "osmium", "iridium", "platinum", "gold", "mercury", "thallium", "lead", "bismuth", "polonium", "astatine", "radon", "francium", "radium", "actinium", "thorium", "protactinium", "uranium", "neptunium", "plutonium", "americium", "curium", "berkelium", "californium", "einsteinium", "fermium", "mendelevium", "nobelium", "lawrencium", "rutherfordium", "dubnium", "seaborgium", "bohrium", "hassium", "meitnerium" };

        //UPGRADE_NOTE: Final was removed from the declaration of 'alternateElementNumbers'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly sbyte[] alternateElementNumbers = new sbyte[] { 0, 13, 16, 55 };

        //UPGRADE_NOTE: Final was removed from the declaration of 'alternateElementNames'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly System.String[] alternateElementNames = new System.String[] { "dummy", "aluminium", "sulphur", "caesium" };

        /// <summary> Default table of van der Waals Radii.
        /// values are stored as MAR -- Milli Angstrom Radius
        /// Used for spacefill rendering of atoms.
        /// Values taken from OpenBabel.
        /// </summary>
        /// <seealso cref="<a href="http://openbabel.sourceforge.net">openbabel.sourceforge.net</a>">
        /// </seealso>
        //UPGRADE_NOTE: Final was removed from the declaration of 'vanderwaalsMars'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly short[] vanderwaalsMars = new short[] { 1000, 1200, 1400, 1820, 1700, 2080, 1950, 1850, 1700, 1730, 1540, 2270, 1730, 2050, 2100, 2080, 2000, 1970, 1880, 2750, 1973, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1630, 1400, 1390, 1870, 1700, 1850, 1900, 2100, 2020, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1630, 1720, 1580, 1930, 2170, 2200, 2060, 2150, 2160, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1720, 1660, 1550, 1960, 2020, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1860, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700 };

        /// <summary> Default table of covalent Radii
        /// stored as a short mar ... Milli Angstrom Radius
        /// Values taken from OpenBabel.
        /// </summary>
        /// <seealso cref="<a href="http://openbabel.sourceforge.net">openbabel.sourceforge.net</a>">
        /// </seealso>
        //UPGRADE_NOTE: Final was removed from the declaration of 'covalentMars'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private static readonly short[] covalentMars = new short[] { 0, 230, 930, 680, 350, 830, 680, 680, 680, 640, 1120, 970, 1100, 1350, 1200, 750, 1020, 990, 1570, 1330, 990, 1440, 1470, 1330, 1350, 1350, 1340, 1330, 1500, 1520, 1450, 1220, 1170, 1210, 1220, 1210, 1910, 1470, 1120, 1780, 1560, 1480, 1470, 1350, 1400, 1450, 1500, 1590, 1690, 1630, 1460, 1460, 1470, 1400, 1980, 1670, 1340, 1870, 1830, 1820, 1810, 1800, 1800, 1990, 1790, 1760, 1750, 1740, 1730, 1720, 1940, 1720, 1570, 1430, 1370, 1350, 1370, 1320, 1500, 1500, 1700, 1550, 1540, 1540, 1680, 1700, 2400, 2000, 1900, 1880, 1790, 1610, 1580, 1550, 1530, 1510, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1600, 1600, 1600, 1600, 1600, 1600 };

        /// <summary>*************************************************************
        /// ionic radii are looked up using a pair of parallel arrays
        /// the ionicLookupTable contains both the elementNumber
        /// and the ionization value, represented as follows:
        /// (elementNumber << 4) + (ionizationValue + 4)
        /// if you don't understand this representation, don't worry about
        /// the binary shifting and stuff. It is just a sorted list
        /// of keys
        /// 
        /// the values are stored in the ionicMars table
        /// these two arrays are parallel
        /// 
        /// This data is from
        /// Handbook of Chemistry and Physics. 48th Ed, 1967-8, p. F143
        /// (scanned for Jmol by Phillip Barak, Jan 2004)
        /// **************************************************************
        /// </summary>

        public const int FORMAL_CHARGE_MIN = -4;
        public const int FORMAL_CHARGE_MAX = 7;
        //UPGRADE_NOTE: Final was removed from the declaration of 'ionicLookupTable'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly short[] ionicLookupTable = new short[]{(short) ((1 << 4) + (- 1 + 4)), (short) ((3 << 4) + (1 + 4)), (short) ((4 << 4) + (1 + 4)), (short) ((4 << 4) + (2 + 4)), (short) ((5 << 4) + (1 + 4)), (short) ((5 << 4) + (3 + 4)), (short) ((6 << 4) + (- 4 + 4)), (short) ((6 << 4) + (4 + 4)), (short) ((7 << 4) + (- 3 + 4)), (short) ((7 << 4) + (1 + 4)), (short) ((7 << 4) + (3 + 4)), (short) ((7 << 4) + (5 + 4)), (short) ((8 << 4) + (- 2 + 4)), (short) ((8 << 4) + (- 1 + 4)), (short) ((8 << 4) + (1 + 4)), (short) ((8 << 4) + (6 + 4)), (short) ((9 << 4) + (- 1 + 4)), (short) ((9 << 4) + (7 + 4)), (short) ((10 << 4) + (1 + 4)), (short) ((11 << 4) + (1 + 4)), (short) ((12 << 4) + (1 + 4)), (short) ((12 << 4) + (2 + 4)), (short) ((13 << 4) + (3 + 4)), (short) ((14 << 4) + (- 4 + 4)), (short) ((14 << 4) + (- 1 + 4)), (short) ((14 << 4) + (1 + 4)), (short) ((14 << 4) + (4 + 4)), (short) ((15 << 4) + (- 3 + 4)), (short) ((15 << 4) + (3 + 4)), (short) ((15 << 4) + (5 + 4)), (short) ((16 << 4) + (- 2 + 4)), (short) ((16 << 4) + (2 + 4)), (short) ((16 << 4) + (4 + 4)), (short) ((16 << 4) + (6 + 4)), (short) ((17 << 4) + (- 1 + 4)), (short) ((17 << 4) + (5 + 4)), (short) ((17 << 4) + (7 + 4)), (short) ((18 << 4) + (1 + 4)), (short) ((19 << 4) + (1 + 4)), (short) ((20 << 4) + (1 + 4)), (short) ((20 << 4) + (2 + 4)), (short) ((21 << 4) + (3 + 4)), (short) ((22 << 4) + (1 + 4)), (short) ((22 << 4) + (2 + 4)), (short) ((22 << 4) + (3 + 4)), (short) ((22 << 4) + (4 + 4)), (short) ((23 << 4) + (2 + 4)), (short) ((23 << 4) + (3 + 4)), (short) ((23 << 4) + (4 + 4)), (short) ((23 << 4) + (5 + 4)), (short) ((24 << 4) + (1 + 4)), (short) ((24 << 4) + (2 + 4)), (short) ((24 << 4) + (3 + 4)), (short) ((24 << 4) + (6 + 4)), (short) ((25 << 4) + (2 + 4)), (short) ((25 << 4) + (3 + 4)), (short) ((25 << 4) + (4 + 4)), (short) ((25 << 4) + (7 + 4)), (short) ((26 << 4) + (2 + 4)), (short) ((26 << 4) + (3 + 4)), (short) ((27 << 4) + (2 + 4)), (short) ((27 << 4) + (3 + 4)), (short) ((28 << 4) + (2 + 4)), (short) ((29 << 4
			) + (1 + 4)), (short) ((29 << 4) + (2 + 4)), (short) ((30 << 4) + (1 + 4)), (short) ((30 << 4) + (2 + 4)), (short) ((31 << 4) + (1 + 4)), (short) ((31 << 4) + (3 + 4)), (short) ((32 << 4) + (- 4 + 4)), (short) ((32 << 4) + (2 + 4)), (short) ((32 << 4) + (4 + 4)), (short) ((33 << 4) + (- 3 + 4)), (short) ((33 << 4) + (3 + 4)), (short) ((33 << 4) + (5 + 4)), (short) ((34 << 4) + (- 2 + 4)), (short) ((34 << 4) + (- 1 + 4)), (short) ((34 << 4) + (1 + 4)), (short) ((34 << 4) + (4 + 4)), (short) ((34 << 4) + (6 + 4)), (short) ((35 << 4) + (- 1 + 4)), (short) ((35 << 4) + (5 + 4)), (short) ((35 << 4) + (7 + 4)), (short) ((37 << 4) + (1 + 4)), (short) ((38 << 4) + (2 + 4)), (short) ((39 << 4) + (3 + 4)), (short) ((40 << 4) + (1 + 4)), (short) ((40 << 4) + (4 + 4)), (short) ((41 << 4) + (1 + 4)), (short) ((41 << 4) + (4 + 4)), (short) ((41 << 4) + (5 + 4)), (short) ((42 << 4) + (1 + 4)), (short) ((42 << 4) + (4 + 4)), (short) ((42 << 4) + (6 + 4)), (short) ((43 << 4) + (7 + 4)), (short) ((44 << 4) + (4 + 4)), (short) ((45 << 4) + (3 + 4)), (short) ((46 << 4) + (2 + 4)), (short) ((46 << 4) + (4 + 4)), (short) ((47 << 4) + (1 + 4)), (short) ((47 << 4) + (2 + 4)), (short) ((48 << 4) + (1 + 4)), (short) ((48 << 4) + (2 + 4)), (short) ((49 << 4) + (3 + 4)), (short) ((50 << 4) + (- 4 + 4)), (short) ((50 << 4) + (- 1 + 4)), (short) ((50 << 4) + (2 + 4)), (short) ((50 << 4) + (4 + 4)), (short) ((51 << 4) + (- 3 + 4)), (short) ((51 << 4) + (3 + 4)), (short) ((51 << 4) + (5 + 4)), (short) ((52 << 4) + (- 2 + 4)), (short) ((52 << 4) + (- 1 + 4)), (short) ((52 << 4) + (1 + 4)), (short) ((52 << 4) + (4 + 4)), (short) ((52 << 4) + (6 + 4)), (short) ((53 << 4) + (- 1 + 4)), (short) ((53 << 4) + (5 + 4)), (short) ((53 << 4) + (7 + 4)), (short) ((55 << 4) + (1 + 4)), (short) ((56 << 4) + (1 + 4)), (short) ((56 << 4) + (2 + 4)), (short) ((57 << 4) + (1 + 4)), (short) ((57 << 4) + (3 + 4)), (short) ((58 << 4) + (1 + 4)), (short) ((58 << 4) + (3 + 4)), (short) ((58 << 4) + (4 + 4)), (short) ((59 << 4) + (3 + 4)), (short) ((
			59 << 4) + (4 + 4)), (short) ((60 << 4) + (3 + 4)), (short) ((61 << 4) + (3 + 4)), (short) ((62 << 4) + (3 + 4)), (short) ((63 << 4) + (2 + 4)), (short) ((63 << 4) + (3 + 4)), (short) ((64 << 4) + (3 + 4)), (short) ((65 << 4) + (3 + 4)), (short) ((65 << 4) + (4 + 4)), (short) ((66 << 4) + (3 + 4)), (short) ((67 << 4) + (3 + 4)), (short) ((68 << 4) + (3 + 4)), (short) ((69 << 4) + (3 + 4)), (short) ((70 << 4) + (2 + 4)), (short) ((70 << 4) + (3 + 4)), (short) ((71 << 4) + (3 + 4)), (short) ((72 << 4) + (4 + 4)), (short) ((73 << 4) + (5 + 4)), (short) ((74 << 4) + (4 + 4)), (short) ((74 << 4) + (6 + 4)), (short) ((75 << 4) + (4 + 4)), (short) ((75 << 4) + (7 + 4)), (short) ((76 << 4) + (4 + 4)), (short) ((76 << 4) + (6 + 4)), (short) ((77 << 4) + (4 + 4)), (short) ((78 << 4) + (2 + 4)), (short) ((78 << 4) + (4 + 4)), (short) ((79 << 4) + (1 + 4)), (short) ((79 << 4) + (3 + 4)), (short) ((80 << 4) + (1 + 4)), (short) ((80 << 4) + (2 + 4)), (short) ((81 << 4) + (1 + 4)), (short) ((81 << 4) + (3 + 4)), (short) ((82 << 4) + (2 + 4)), (short) ((82 << 4) + (4 + 4)), (short) ((83 << 4) + (1 + 4)), (short) ((83 << 4) + (3 + 4)), (short) ((83 << 4) + (5 + 4)), (short) ((84 << 4) + (6 + 4)), (short) ((85 << 4) + (7 + 4)), (short) ((87 << 4) + (1 + 4)), (short) ((88 << 4) + (2 + 4)), (short) ((89 << 4) + (3 + 4)), (short) ((90 << 4) + (4 + 4)), (short) ((91 << 4) + (3 + 4)), (short) ((91 << 4) + (4 + 4)), (short) ((91 << 4) + (5 + 4)), (short) ((92 << 4) + (4 + 4)), (short) ((92 << 4) + (6 + 4)), (short) ((93 << 4) + (3 + 4)), (short) ((93 << 4) + (4 + 4)), (short) ((93 << 4) + (7 + 4)), (short) ((94 << 4) + (3 + 4)), (short) ((94 << 4) + (4 + 4)), (short) ((95 << 4) + (3 + 4)), (short) ((95 << 4) + (4 + 4))};

        //UPGRADE_NOTE: Final was removed from the declaration of 'ionicMars'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly short[] ionicMars = new short[] { 1540, 680, 440, 350, 350, 230, 2600, 160, 1710, 250, 160, 130, 1320, 1760, 220, 90, 1330, 80, 1120, 970, 820, 660, 510, 2710, 3840, 650, 420, 2120, 440, 350, 1840, 2190, 370, 300, 1810, 340, 270, 1540, 1330, 1180, 990, 732, 960, 940, 760, 680, 880, 740, 630, 590, 810, 890, 630, 520, 800, 660, 600, 460, 740, 640, 720, 630, 690, 960, 720, 880, 740, 810, 620, 2720, 730, 530, 2220, 580, 460, 1910, 2320, 660, 500, 420, 1960, 470, 390, 1470, 1120, 893, 1090, 790, 1000, 740, 690, 930, 700, 620, 979, 670, 680, 800, 650, 1260, 890, 1140, 970, 810, 2940, 3700, 930, 710, 2450, 760, 620, 2110, 2500, 820, 700, 560, 2200, 620, 500, 1670, 1530, 1340, 1390, 1016, 1270, 1034, 920, 1013, 900, 995, 979, 964, 1090, 950, 938, 923, 840, 908, 894, 881, 870, 930, 858, 850, 780, 680, 700, 620, 720, 560, 880, 690, 680, 800, 650, 1370, 850, 1270, 1100, 1470, 950, 1200, 840, 980, 960, 740, 670, 620, 1800, 1430, 1180, 1020, 1130, 980, 890, 970, 800, 1100, 950, 710, 1080, 930, 1070, 920 };

        public static short getBondingMar(int elementNumber, int charge)
        {
            if (charge != 0)
            {
                // ionicLookupTable is a sorted table of ionic keys
                // lookup doing a binary search
                // when found, return the corresponding value in ionicMars
                // if not found, just return covalent radius
                short ionic = (short)((elementNumber << 4) + (charge + 4));
                int iMin = 0, iMax = ionicLookupTable.Length;
                while (iMin != iMax)
                {
                    int iMid = (iMin + iMax) / 2;
                    if (ionic < ionicLookupTable[iMid])
                        iMax = iMid;
                    else if (ionic > ionicLookupTable[iMid])
                        iMin = iMid + 1;
                    else
                        return ionicMars[iMid];
                }
            }
            return (short)covalentMars[elementNumber];
        }

        public static bool isValidFormalCharge(int elementNumber, int charge)
        {
            if (charge == 0)
                return true;
            if (charge < FORMAL_CHARGE_MIN || charge > FORMAL_CHARGE_MAX)
                return false;
            short ionic = (short)((elementNumber << 4) + (charge + 4));
            int iMin = 0, iMax = ionicLookupTable.Length;
            while (iMin != iMax)
            {
                int iMid = (iMin + iMax) / 2;
                if (ionic < ionicLookupTable[iMid])
                    iMax = iMid;
                else if (ionic > ionicLookupTable[iMid])
                    iMin = iMid + 1;
                else
                    return true;
            }
            return false;
        }

        // maximum number of bonds that an atom can have when
        // autoBonding
        // All bonding is done by distances
        // this is only here for truly pathological cases
        public const int MAXIMUM_AUTO_BOND_COUNT = 20;

        /// <summary> Default table of CPK atom colors.
        /// ghemical colors with a few proposed modifications
        /// </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'argbsCpk'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly int[] argbsCpk = new int[]{unchecked((int) 0xFFFF1493), unchecked((int) 0xFFFFFFFF), unchecked((int) 0xFFD9FFFF), unchecked((int) 0xFFCC80FF), unchecked((int) 0xFFC2FF00), unchecked((int) 0xFFFFB5B5), unchecked((int) 0xFF909090), unchecked((int) 0xFF3050F8), unchecked((int) 0xFFFF0D0D), unchecked((int) 0xFF90E050), unchecked((int) 0xFFB3E3F5), unchecked((int) 0xFFAB5CF2), unchecked((int) 0xFF8AFF00), unchecked((int) 0xFFBFA6A6), unchecked((int) 0xFFF0C8A0), unchecked((int) 0xFFFF8000), unchecked((int) 0xFFFFFF30), unchecked((int) 0xFF1FF01F), unchecked((int) 0xFF80D1E3), unchecked((int) 0xFF8F40D4), unchecked((int) 0xFF3DFF00), unchecked((int) 0xFFE6E6E6), unchecked((int) 0xFFBFC2C7), unchecked((int) 0xFFA6A6AB), unchecked((int) 0xFF8A99C7), unchecked((int) 0xFF9C7AC7), unchecked((int) 0xFFE06633), unchecked((int) 0xFFF090A0), unchecked((int) 0xFF50D050), unchecked((int) 0xFFC88033), unchecked((int) 0xFF7D80B0), unchecked((int) 0xFFC28F8F), unchecked((int) 0xFF668F8F), unchecked((int) 0xFFBD80E3), unchecked((int) 0xFFFFA100), unchecked((int) 0xFFA62929), unchecked((int) 0xFF5CB8D1), unchecked((int) 0xFF702EB0), unchecked((int) 0xFF00FF00), unchecked((int) 0xFF94FFFF), unchecked((int) 0xFF94E0E0), unchecked((int) 0xFF73C2C9), unchecked((int) 0xFF54B5B5), unchecked((int) 0xFF3B9E9E), unchecked((int) 0xFF248F8F), unchecked((int) 0xFF0A7D8C), unchecked((int) 0xFF006985), unchecked((int) 0xFFC0C0C0), unchecked((int) 0xFFFFD98F), unchecked((int) 0xFFA67573), unchecked((int) 0xFF668080), unchecked((int) 0xFF9E63B5), unchecked((int) 0xFFD47A00), unchecked((int) 0xFF940094), unchecked((int) 0xFF429EB0), unchecked((int) 0xFF57178F), unchecked((int) 0xFF00C900), unchecked((int) 0xFF70D4FF), unchecked((int) 0xFFFFFFC7), unchecked((int) 0xFFD9FFC7), unchecked((int) 0xFFC7FFC7), unchecked((int) 0xFFA3FFC7), unchecked((int) 0xFF8FFFC7), unchecked((int) 0xFF61FFC7), unchecked((int) 0xFF45FFC7), unchecked((int) 0xFF30FFC7), unchecked((int) 0xFF1FFFC7), unchecked((int) 0xFF00FF9C), unchecked((int
			) 0xFF00E675), unchecked((int) 0xFF00D452), unchecked((int) 0xFF00BF38), unchecked((int) 0xFF00AB24), unchecked((int) 0xFF4DC2FF), unchecked((int) 0xFF4DA6FF), unchecked((int) 0xFF2194D6), unchecked((int) 0xFF267DAB), unchecked((int) 0xFF266696), unchecked((int) 0xFF175487), unchecked((int) 0xFFD0D0E0), unchecked((int) 0xFFFFD123), unchecked((int) 0xFFB8B8D0), unchecked((int) 0xFFA6544D), unchecked((int) 0xFF575961), unchecked((int) 0xFF9E4FB5), unchecked((int) 0xFFAB5C00), unchecked((int) 0xFF754F45), unchecked((int) 0xFF428296), unchecked((int) 0xFF420066), unchecked((int) 0xFF007D00), unchecked((int) 0xFF70ABFA), unchecked((int) 0xFF00BAFF), unchecked((int) 0xFF00A1FF), unchecked((int) 0xFF008FFF), unchecked((int) 0xFF0080FF), unchecked((int) 0xFF006BFF), unchecked((int) 0xFF545CF2), unchecked((int) 0xFF785CE3), unchecked((int) 0xFF8A4FE3), unchecked((int) 0xFFA136D4), unchecked((int) 0xFFB31FD4), unchecked((int) 0xFFB31FBA), unchecked((int) 0xFFB30DA6), unchecked((int) 0xFFBD0D87), unchecked((int) 0xFFC70066), unchecked((int) 0xFFCC0059), unchecked((int) 0xFFD1004F), unchecked((int) 0xFFD90045), unchecked((int) 0xFFE00038), unchecked((int) 0xFFE6002E), unchecked((int) 0xFFEB0026)};

        //UPGRADE_NOTE: Final was removed from the declaration of 'argbsCpkRasmol'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly int[] argbsCpkRasmol = new int[] { 0x00FF1493 + (0 << 24), 0x00FFFFFF + (1 << 24), 0x00FFC0CB + (2 << 24), 0x00B22222 + (3 << 24), 0x0000FF00 + (5 << 24), 0x00C8C8C8 + (6 << 24), 0x008F8FFF + (7 << 24), 0x00F00000 + (8 << 24), 0x00DAA520 + (9 << 24), 0x000000FF + (11 << 24), 0x00228B22 + (12 << 24), 0x00808090 + (13 << 24), 0x00DAA520 + (14 << 24), 0x00FFA500 + (15 << 24), 0x00FFC832 + (16 << 24), 0x0000FF00 + (17 << 24), 0x00808090 + (20 << 24), 0x00808090 + (22 << 24), 0x00808090 + (24 << 24), 0x00808090 + (25 << 24), 0x00FFA500 + (26 << 24), 0x00A52A2A + (28 << 24), 0x00A52A2A + (29 << 24), 0x00A52A2A + (30 << 24), 0x00A52A2A + (35 << 24), 0x00808090 + (47 << 24), 0x00A020F0 + (53 << 24), 0x00FFA500 + (56 << 24), 0x00DAA520 + (79 << 24) };

        /// <summary> Default table of PdbStructure colors</summary>
        public const sbyte PROTEIN_STRUCTURE_NONE = 0;
        public const sbyte PROTEIN_STRUCTURE_TURN = 1;
        public const sbyte PROTEIN_STRUCTURE_SHEET = 2;
        public const sbyte PROTEIN_STRUCTURE_HELIX = 3;
        public const sbyte PROTEIN_STRUCTURE_DNA = 4;
        public const sbyte PROTEIN_STRUCTURE_RNA = 5;

        /// <summary>*************************************************************
        /// In DRuMS, RasMol, and Chime, quoting from
        /// http://www.umass.edu/microbio/rasmol/rascolor.htm
        /// 
        /// The RasMol structure color scheme colors the molecule by
        /// protein secondary structure.
        /// 
        /// Structure                   Decimal RGB    Hex RGB
        /// Alpha helices  red-magenta  [255,0,128]    FF 00 80  *
        /// Beta strands   yellow       [255,200,0]    FF C8 00  *
        /// 
        /// Turns          pale blue    [96,128,255]   60 80 FF
        /// Other          white        [255,255,255]  FF FF FF
        /// 
        /// *Values given in the 1994 RasMol 2.5 Quick Reference Card ([240,0,128]
        /// and [255,255,0]) are not correct for RasMol 2.6-beta-2a.
        /// This correction was made above on Dec 5, 1998.
        /// **************************************************************
        /// </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'argbsStructure'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly int[] argbsStructure = new int[] { unchecked((int)0xFFFFFFFF), unchecked((int)0xFF6080FF), unchecked((int)0xFFFFC800), unchecked((int)0xFFFF0080), unchecked((int)0xFFAE00FE), unchecked((int)0xFFFD0162) };

        //UPGRADE_NOTE: Final was removed from the declaration of 'argbsAmino'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly int[] argbsAmino = new int[] { unchecked((int)0xFFBEA06E), unchecked((int)0xFFC8C8C8), unchecked((int)0xFF145AFF), unchecked((int)0xFF00DCDC), unchecked((int)0xFFE60A0A), unchecked((int)0xFFE6E600), unchecked((int)0xFF00DCDC), unchecked((int)0xFFE60A0A), unchecked((int)0xFFEBEBEB), unchecked((int)0xFF8282D2), unchecked((int)0xFF0F820F), unchecked((int)0xFF0F820F), unchecked((int)0xFF145AFF), unchecked((int)0xFFE6E600), unchecked((int)0xFF3232AA), unchecked((int)0xFFDC9682), unchecked((int)0xFFFA9600), unchecked((int)0xFFFA9600), unchecked((int)0xFFB45AB4), unchecked((int)0xFF3232AA), unchecked((int)0xFF0F820F), unchecked((int)0xFFFF69B4), unchecked((int)0xFFFF69B4) };

        // hmmm ... what is shapely backbone? seems interesting
        public const int argbShapelyBackbone = unchecked((int)0xFFB8B8B8);
        public const int argbShapelySpecial = unchecked((int)0xFF5E005E);
        public const int argbShapelyDefault = unchecked((int)0xFFFF00FF);
        //UPGRADE_NOTE: Final was removed from the declaration of 'argbsShapely'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly int[] argbsShapely = new int[] { unchecked((int)0xFFFF00FF), unchecked((int)0xFF8CFF8C), unchecked((int)0xFF00007C), unchecked((int)0xFFFF7C70), unchecked((int)0xFFA00042), unchecked((int)0xFFFFFF70), unchecked((int)0xFFFF4C4C), unchecked((int)0xFF660000), unchecked((int)0xFFFFFFFF), unchecked((int)0xFF7070FF), unchecked((int)0xFF004C00), unchecked((int)0xFF455E45), unchecked((int)0xFF4747B8), unchecked((int)0xFFB8A042), unchecked((int)0xFF534C52), unchecked((int)0xFF525252), unchecked((int)0xFFFF7042), unchecked((int)0xFFB84C00), unchecked((int)0xFF4F4600), unchecked((int)0xFF8C704C), unchecked((int)0xFFFF8CFF), unchecked((int)0xFFFF00FF), unchecked((int)0xFFFF00FF), unchecked((int)0xFFFF00FF), unchecked((int)0xFFA0A0FF), unchecked((int)0xFFA0A0FF), unchecked((int)0xFFFF7070), unchecked((int)0xFFFF7070), unchecked((int)0xFF80FFFF), unchecked((int)0xFF80FFFF), unchecked((int)0xFFFF8C4B), unchecked((int)0xFFFF8C4B), unchecked((int)0xFFA0FFA0), unchecked((int)0xFFA0FFA0), unchecked((int)0xFFFF8080), unchecked((int)0xFFFF8080) };

        /// <summary> colors used for chains
        /// 
        /// </summary>

        /// <summary>*************************************************************
        /// some pastel colors
        /// 
        /// C0D0FF - pastel blue
        /// B0FFB0 - pastel green
        /// B0FFFF - pastel cyan
        /// FFC0C8 - pink
        /// FFC0FF - pastel magenta
        /// FFFF80 - pastel yellow
        /// FFDEAD - navajowhite
        /// FFD070 - pastel gold
        /// FF9898 - light coral
        /// B4E444 - light yellow-green
        /// C0C000 - light olive
        /// FF8060 - light tomato
        /// 00FF7F - springgreen
        /// 
        /// cpk on; select atomno>100; label %i; color chain; select selected & hetero; cpk off
        /// **************************************************************
        /// </summary>

        //UPGRADE_NOTE: Final was removed from the declaration of 'argbsChainAtom'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly int[] argbsChainAtom = new int[] { unchecked((int)0xFFffffff), unchecked((int)0xFFC0D0FF), unchecked((int)0xFFB0FFB0), unchecked((int)0xFFFFC0C8), unchecked((int)0xFFFFFF80), unchecked((int)0xFFFFC0FF), unchecked((int)0xFFB0F0F0), unchecked((int)0xFFFFD070), unchecked((int)0xFFF08080), unchecked((int)0xFFF5DEB3), unchecked((int)0xFF00BFFF), unchecked((int)0xFFCD5C5C), unchecked((int)0xFF66CDAA), unchecked((int)0xFF9ACD32), unchecked((int)0xFFEE82EE), unchecked((int)0xFF00CED1), unchecked((int)0xFF00FF7F), unchecked((int)0xFF3CB371), unchecked((int)0xFF00008B), unchecked((int)0xFFBDB76B), unchecked((int)0xFF006400), unchecked((int)0xFF800000), unchecked((int)0xFF808000), unchecked((int)0xFF800080), unchecked((int)0xFF008080), unchecked((int)0xFFB8860B), unchecked((int)0xFFB22222) };

        //UPGRADE_NOTE: Final was removed from the declaration of 'argbsChainHetero'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly uint[] argbsChainHetero = new uint[] { unchecked(0xFFffffff), 0xFFC0D0FF - 0x00303030, 0xFFB0FFB0 - 0x00303018, 0xFFFFC0C8 - 0x00303018, 0xFFFFFF80 - 0x00303010, 0xFFFFC0FF - 0x00303030, 0xFFB0F0F0 - 0x00303030, 0xFFFFD070 - 0x00303010, 0xFFF08080 - 0x00303010, 0xFFF5DEB3 - 0x00303030, 0xFF00BFFF - 0x00001830, 0xFFCD5C5C - 0x00181010, 0xFF66CDAA - 0x00101818, 0xFF9ACD32 - 0x00101808, 0xFFEE82EE - 0x00301030, 0xFF00CED1 - 0x00001830, 0xFF00FF7F - 0x00003010, 0xFF3CB371 - 0x00081810, 0xFF00008B + 0x00000030, 0xFFBDB76B - 0x00181810, 0xFF006400 + 0x00003000, 0xFF800000 + 0x00300000, 0xFF808000 + 0x00303000, 0xFF800080 + 0x00300030, 0xFF008080 + 0x00003030, 0xFFB8860B + 0x00303008, 0xFFB22222 + 0x00101010 };

        /*
        public final static int[] argbsChainAtom = {
        // ' '->0 'A'->1, 'B'->2
        // protein explorer colors
        0xFFffffff, // ' ' & '0' white - pewhite 0xFFffffff
        //
        0xFF40E0D0, // A & 1 turquoise - pecyan 0xFF00ffff
        0xFFDA70D6, // B & 2 orchid - pepurple 0xFFd020ff
        0xFF00FF00, // C & 3 lime - pegreen 0xFF00ff00
        0xFF6495ED, // D & 4 cornflowerblue - peblue 0xFF6060ff
        0xFFFF69B4, // E & 5 hotpink - peviolet 0xFFff80c0
        0xFFA52A2A, // F & 6 brown - pebrown 0xFFa42028
        0xFFFFC0CB, // G & 7 pink - pepink 0xFFffd8d8
        0xFFFFFF00, // H & 8 yellow - peyellow 0xFFffff00
        0xFF228B22, // I & 9 forestgreen - pedarkgreen 0xFF00c000
        0xFFFFA500, // J orange - peorange 0xFFffb000
        0xFF87CEEB, // K skyblue - pelightblue 0xFFb0b0ff
        0xFF008080, // L teal - pedarkcyan 0xFF00a0a0
        0xFF606060, // M pedarkgray 0xFF606060
        // pick two more colors
        0xFF0000CD, // N mediumblue
        0xFFf6f675, // O yellowtint
		
        0xFFFF6347, // P tomato
        0xFFC8A880, // Q a darkened tan
        0xFF800080, // R purple
        0xFF808000, // S olive
        0xFFF4A460, // T sandybrown
        0xFF7FFFD4, // U aquamarine
        0xFFB8860B, // V darkgoldenrod
        0xFFF08080, // W lightcoral
        0xFF9ACD32, // X yellowgreen
        0xFF00008B, // Y darkblue
        0xFFF5DEB3, // Z wheat
        };
		
        public final static int[] argbsChainHetero = {
        // ' '->0 'A'->1, 'B'->2
        0xFFD0D0D0, // ' ' & '0' a light gray
        //
        0xFF20c0b0, // A & 1 a darker turquoise
        0xFFB850b6, // B & 2 a darker orchid
        0xFF00C800, // C & 3 a darker limegreen
        0xFF4070D0, // D & 4 a darker cornflowerblue
        0xFFE04890, // E & 5 a darker hotpink
        0xFF800008, // F & 6 a darker brown
        0xFFD898A0, // G & 7 a darker pink
        0xFFD0D000, // H & 8 a darker yellow
        0xFF006400, // I & 9 darkgreen
        0xFFE08500, // J a darker orange
        0xFF68A8C8, // K a darker skyblue
        0xFF006060, // L a darker teal
        0xFF484848, // M a darker gray
        // pick two more colors
        0xFF0000A0, // N a darker blue
        0xFFC8C858, // O a darker yellow
		
        0xFFd84838, // P a darker tomato
        0xFFA4845C, // Q a darker tan
        0xFF600060, // R a deeper purple
        0xFF606000, // S a darker olive
        0xFFD88840, // T a darker sandybrown
        0xFF58D8AC, // U a darker aquamarine
        0xFF986600, // V a darker darkgoldenrod
        0xFFD86868, // W a darker lightcoral
        0xFF78A810, // X a darker yellowgreen
        0xFF000060, // Y a darker darkblue
        0xFFD8B898, // Z a darker wheat
        };
        */

        //UPGRADE_NOTE: Final was removed from the declaration of 'argbsCharge'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly int[] argbsCharge = new int[] { unchecked((int)0xFFFF0000), unchecked((int)0xFFFF4040), unchecked((int)0xFFFF8080), unchecked((int)0xFFFFC0C0), unchecked((int)0xFFFFFFFF), unchecked((int)0xFFD8D8FF), unchecked((int)0xFFB4B4FF), unchecked((int)0xFF9090FF), unchecked((int)0xFF6C6CFF), unchecked((int)0xFF4848FF), unchecked((int)0xFF2424FF), unchecked((int)0xFF0000FF) };

        //UPGRADE_NOTE: Final was removed from the declaration of 'argbsRwbScale'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly int[] argbsRwbScale = new int[] { unchecked((int)0xFFFF0000), unchecked((int)0xFFFF1010), unchecked((int)0xFFFF2020), unchecked((int)0xFFFF3030), unchecked((int)0xFFFF4040), unchecked((int)0xFFFF5050), unchecked((int)0xFFFF6060), unchecked((int)0xFFFF7070), unchecked((int)0xFFFF8080), unchecked((int)0xFFFF9090), unchecked((int)0xFFFFA0A0), unchecked((int)0xFFFFB0B0), unchecked((int)0xFFFFC0C0), unchecked((int)0xFFFFD0D0), unchecked((int)0xFFFFE0E0), unchecked((int)0xFFFFFFFF), unchecked((int)0xFFE0E0FF), unchecked((int)0xFFD0D0FF), unchecked((int)0xFFC0C0FF), unchecked((int)0xFFB0B0FF), unchecked((int)0xFFA0A0FF), unchecked((int)0xFF9090FF), unchecked((int)0xFF8080FF), unchecked((int)0xFF7070FF), unchecked((int)0xFF6060FF), unchecked((int)0xFF5050FF), unchecked((int)0xFF4040FF), unchecked((int)0xFF3030FF), unchecked((int)0xFF2020FF), unchecked((int)0xFF1010FF), unchecked((int)0xFF0000FF) };

        //UPGRADE_NOTE: Final was removed from the declaration of 'argbsBlueRedRainbow'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly int[] argbsBlueRedRainbow = new int[] { unchecked((int)0xFF0000FF), unchecked((int)0xFF0020FF), unchecked((int)0xFF0040FF), unchecked((int)0xFF0060FF), unchecked((int)0xFF0080FF), unchecked((int)0xFF00A0FF), unchecked((int)0xFF00C0FF), unchecked((int)0xFF00E0FF), unchecked((int)0xFF00FFFF), unchecked((int)0xFF00FFE0), unchecked((int)0xFF00FFC0), unchecked((int)0xFF00FFA0), unchecked((int)0xFF00FF80), unchecked((int)0xFF00FF60), unchecked((int)0xFF00FF40), unchecked((int)0xFF00FF20), unchecked((int)0xFF00FF00), unchecked((int)0xFF20FF00), unchecked((int)0xFF40FF00), unchecked((int)0xFF60FF00), unchecked((int)0xFF80FF00), unchecked((int)0xFFA0FF00), unchecked((int)0xFFC0FF00), unchecked((int)0xFFE0FF00), unchecked((int)0xFFFFFF00), unchecked((int)0xFFFFE000), unchecked((int)0xFFFFC000), unchecked((int)0xFFFFA000), unchecked((int)0xFFFF8000), unchecked((int)0xFFFF6000), unchecked((int)0xFFFF4000), unchecked((int)0xFFFF2000), unchecked((int)0xFFFF0000) };

        //UPGRADE_NOTE: Final was removed from the declaration of 'specialAtomNames'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly System.String[] specialAtomNames = new System.String[] { null, "N", "CA", "C", null, "O5'", "C5'", "C4'", "C3'", "O3'", "C2'", "C1'", "P", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "OXT", "H", "1H", "2H", "3H", "HA", "1HA", "2HA", "O", "O1", null, null, null, "H5T", "O5T", "O1P", "O2P", "O4'", "O2'", "1H5'", "2H5'", "H4'", "H3'", "1H2'", "2H2'", "2HO'", "H1'", "H3T", null, null, null, null, "N1", "C2", "N3", "C4", "C5", "C6", "O2", "N7", "C8", "N9", "N4", "N2", "N6", "C5M", "O6", "O4", "S4" };

        //UPGRADE_NOTE: Final was removed from the declaration of 'ATOMID_MAX '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly int ATOMID_MAX = specialAtomNames.Length;

        ////////////////////////////////////////////////////////////////
        // currently, ATOMIDs must be >= 0 && <= 127
        // if we need more then we can go to 255 by:
        //  1. applying 0xFF mask ... as in atom.specialAtomID & 0xFF;
        //  2. change the interesting atoms table to be shorts
        //     so that we can store negative numbers
        ////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////
        // keep this table in order to make it easier to maintain
        ////////////////////////////////////////////////////////////////

        // atomID 0 => nothing special, just an ordinary atom
        public const sbyte ATOMID_AMINO_NITROGEN = 1;
        public const sbyte ATOMID_ALPHA_CARBON = 2;
        public const sbyte ATOMID_CARBONYL_CARBON = 3;
        public const sbyte ATOMID_O5_PRIME = 5;
        public const sbyte ATOMID_O3_PRIME = 9;
        public const sbyte ATOMID_NUCLEIC_PHOSPHORUS = 12;
        public const sbyte ATOMID_TERMINATING_OXT = 32;
        public const sbyte ATOMID_CARBONYL_OXYGEN = 40;
        public const sbyte ATOMID_O1 = 41;
        public const sbyte ATOMID_H5T_TERMINUS = 45;
        public const sbyte ATOMID_O5T_TERMINUS = 46;
        public const sbyte ATOMID_RNA_O2PRIME = 50;
        public const sbyte ATOMID_H3T_TERMINUS = 59;
        public const sbyte ATOMID_N1 = 64;
        public const sbyte ATOMID_C2 = 65;
        public const sbyte ATOMID_N3 = 66;
        public const sbyte ATOMID_C4 = 67;
        public const sbyte ATOMID_C5 = 68;
        public const sbyte ATOMID_C6 = 69;
        public const sbyte ATOMID_O2 = 70;
        public const sbyte ATOMID_N7 = 71;
        public const sbyte ATOMID_C8 = 72;
        public const sbyte ATOMID_N9 = 73;
        public const sbyte ATOMID_N4 = 74;
        public const sbyte ATOMID_N2 = 75;
        public const sbyte ATOMID_N6 = 76;
        public const sbyte ATOMID_C5M = 77;
        public const sbyte ATOMID_O6 = 78;
        public const sbyte ATOMID_O4 = 79;
        public const sbyte ATOMID_S4 = 80;

        // this is currently defined as C6
        public const sbyte ATOMID_NUCLEIC_WING = 69;

        // this is entries 1 through 3 ... 3 bits ... N, CA, C
        public const int ATOMID_PROTEIN_MASK = 0x07 << 1;
        // this is for groups that only contain an alpha carbon
        //UPGRADE_NOTE: Final was removed from the declaration of 'ATOMID_ALPHA_ONLY_MASK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly int ATOMID_ALPHA_ONLY_MASK = 1 << ATOMID_ALPHA_CARBON;
        // this is entries 5 through through 11 ... 7 bits
        public const int ATOMID_NUCLEIC_MASK = 0x7F << 5;
        // this is for nucleic groups that only contain a phosphorus
        //UPGRADE_NOTE: Final was removed from the declaration of 'ATOMID_PHOSPHORUS_ONLY_MASK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly int ATOMID_PHOSPHORUS_ONLY_MASK = 1 << ATOMID_NUCLEIC_PHOSPHORUS;

        // this is the MAX of the backbone ... everything < MAX is backbone
        public const int ATOMID_DISTINGUISHING_ATOM_MAX = 32;
        public const int ATOMID_BACKBONE_MAX = 64;

        ////////////////////////////////////////////////////////////////
        // GROUP_ID related stuff for special groupIDs
        ////////////////////////////////////////////////////////////////

        /// <summary>*************************************************************
        /// PDB file format spec says that the 'residue name' must be
        /// right-justified. However, Eric Martz says that some files
        /// are not. Therefore, we will be 'flexible' in reading the
        /// group name ... we will trim() when read in the field.
        /// So a 'group3' can now be less than 3 characters long.
        /// **************************************************************
        /// </summary>

        public const int GROUPID_PROLINE = 15;
        public const int GROUPID_PURINE_MIN = 24;
        public const int GROUPID_PURINE_LAST = 29;
        public const int GROUPID_PYRIMIDINE_MIN = 30;
        public const int GROUPID_PYRIMIDINE_LAST = 35;
        public const int GROUPID_GUANINE = 26;
        public const int GROUPID_PLUS_GUANINE = 27;
        public const int GROUPID_GUANINE_1_MIN = 40;
        public const int GROUPID_GUANINE_1_LAST = 46;
        public const int GROUPID_GUANINE_2_MIN = 55;
        public const int GROUPID_GUANINE_2_LAST = 57;


        public const short GROUPID_AMINO_MAX = 23;

        public const short GROUPID_SHAPELY_MAX = 36;

        //UPGRADE_NOTE: Final was removed from the declaration of 'predefinedGroup3Names'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly System.String[] predefinedGroup3Names = new System.String[] { "", "ALA", "ARG", "ASN", "ASP", "CYS", "GLN", "GLU", "GLY", "HIS", "ILE", "LEU", "LYS", "MET", "PHE", "PRO", "SER", "THR", "TRP", "TYR", "VAL", "ASX", "GLX", "UNK", "A", "+A", "G", "+G", "I", "+I", "C", "+C", "T", "+T", "U", "+U", "1MA", "AMO", "5MC", "OMC", "1MG", "2MG", "M2G", "7MG", "G7M", "OMG", "YG", "QUO", "H2U", "5MU", "4SU", "PSU", "AMP", "ADP", "ATP", "GMP", "GDP", "GTP", "IMP", "IDP", "ITP", "CMP", "CDP", "CTP", "TMP", "TDP", "TTP", "UMP", "UDP", "UTP", "HOH", "DOD", "WAT", "PO4", "SO4" };

        ////////////////////////////////////////////////////////////////
        // predefined sets
        ////////////////////////////////////////////////////////////////

        public static System.String[] predefinedSets = new System.String[] { "@amino _g>0 & _g<=23", "@acidic asp,glu", "@basic arg,his,lys", "@charged acidic,basic", "@negative acidic", "@positive basic", "@neutral amino&!(acidic,basic)", "@polar amino&!hydrophobic", "@cyclic his,phe,pro,trp,tyr", "@acyclic amino&!cyclic", "@aliphatic ala,gly,ile,leu,val", "@aromatic his,phe,trp,tyr", "@buried ala,cys,ile,leu,met,phe,trp,val", "@surface !buried", "@hydrophobic ala,gly,ile,leu,met,phe,pro,trp,tyr,val", "@ligand hetero & !solvent", "@mainchain backbone", "@small ala,gly,ser", "@medium asn,asp,cys,pro,thr,val", "@large arg,glu,gln,his,ile,leu,lys,met,phe,trp,tyr", "@c nucleic & within(group,_a=74)", "@g nucleic & within(group,_a=75)", "@cg c,g", "@a nucleic & within(group,_a=76)", "@t nucleic & within(group,_a=77)", "@at a,t", "@i nucleic & within(group,_a=78) & !g", "@u nucleic & within(group,_a=79) & !t", "@tu nucleic & within(group,_a=80)", "@solvent _g>=70 & _g<=74", "@hoh water", "@water _g>=70 & _g<=72", "@ions _g=73,_g=74", "@alpha _a=2", "@backbone (protein,nucleic) & _a>0 & _a<=63", "@sidechain (protein,nucleic) & !backbone", "@base nucleic & !backbone", "@turn _structure=1", "@sheet _structure=2", "@helix _structure=3", "@bonded _bondedcount>0" };

        ////////////////////////////////////////////////////////////////
        // font-related
        ////////////////////////////////////////////////////////////////

        public const System.String DEFAULT_FONTFACE = "SansSerif";
        public const System.String DEFAULT_FONTSTYLE = "Plain";

        public const int LABEL_MINIMUM_FONTSIZE = 6;
        public const int LABEL_MAXIMUM_FONTSIZE = 63;
        public const int LABEL_DEFAULT_FONTSIZE = 13;
        public const int LABEL_DEFAULT_X_OFFSET = 4;
        public const int LABEL_DEFAULT_Y_OFFSET = 4;

        public const int MEASURE_DEFAULT_FONTSIZE = 15;
        public const int AXES_DEFAULT_FONTSIZE = 14;

        ////////////////////////////////////////////////////////////////
        // do not rearrange/modify these shapes without
        // updating the String[] shapeBaseClasses below &&
        // also updating Eval.java to confirm consistent
        // conversion from tokens to shapes
        ////////////////////////////////////////////////////////////////

        public const int SHAPE_BALLS = 0;
        public const int SHAPE_STICKS = 1;
        public const int SHAPE_HSTICKS = 2;
        public const int SHAPE_SSSTICKS = 3;
        public const int SHAPE_LABELS = 4;
        public const int SHAPE_VECTORS = 5;
        public const int SHAPE_MEASURES = 6;
        public const int SHAPE_DOTS = 7;
        public const int SHAPE_BACKBONE = 8;
        public const int SHAPE_TRACE = 9;
        public const int SHAPE_CARTOON = 10;
        public const int SHAPE_STRANDS = 11;
        public const int SHAPE_MESHRIBBON = 12;
        public const int SHAPE_RIBBONS = 13;
        public const int SHAPE_ROCKETS = 14;
        public const int SHAPE_STARS = 15;

        public const int SHAPE_MIN_SELECTION_INDEPENDENT = 16;
        public const int SHAPE_AXES = 16;
        public const int SHAPE_BBCAGE = 17;
        public const int SHAPE_UCCAGE = 18;
        public const int SHAPE_FRANK = 19;
        public const int SHAPE_ECHO = 20;
        public const int SHAPE_HOVER = 21;
        public const int SHAPE_PMESH = 22;
        public const int SHAPE_POLYHEDRA = 23;
        public const int SHAPE_SURFACE = 24;
        public const int SHAPE_PRUEBA = 25;
        public const int SHAPE_MAX = 26;

        //UPGRADE_NOTE: Final was removed from the declaration of 'shapeClassBases'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly System.String[] shapeClassBases = new System.String[] { "Balls", "Sticks", "Hsticks", "Sssticks", "Labels", "Vectors", "Measures", "Dots", "Backbone", "Trace", "Cartoon", "Strands", "MeshRibbon", "Ribbons", "Rockets", "Stars", "Axes", "Bbcage", "Uccage", "Frank", "Echo", "Hover", "Pmesh", "Polyhedra", "Surface", "Prueba" };
        static JmolConstants()
        {
            {
                // if the length of these tables is all the same then the
                // java compiler should eliminate all of this code.
                if ((elementSymbols.Length != elementNames.Length) || (elementSymbols.Length != vanderwaalsMars.Length) || (elementSymbols.Length != covalentMars.Length) || (elementSymbols.Length != argbsCpk.Length))
                {
                    System.Console.Out.WriteLine("ERROR!!! Element table length mismatch:" + "\n elementSymbols.length=" + elementSymbols.Length + "\n elementNames.length=" + elementNames.Length + "\n vanderwaalsMars.length=" + vanderwaalsMars.Length + "\n covalentMars.length=" + covalentMars.Length + "\n argbsCpk.length=" + argbsCpk.Length);
                }
            }
            // all of these things are compile-time constants
            // if they are false then the compiler should take them away
            {
                if (ionicLookupTable.Length != ionicMars.Length)
                {
                    System.Console.Out.WriteLine("ionic table mismatch!");
                    throw new System.NullReferenceException();
                }
                for (int i = ionicLookupTable.Length; --i > 0; )
                {
                    if (ionicLookupTable[i - 1] >= ionicLookupTable[i])
                    {
                        System.Console.Out.WriteLine("ionicLookupTable not sorted properly");
                        throw new System.NullReferenceException();
                    }
                }
                if (argbsCharge.Length != FORMAL_CHARGE_MAX - FORMAL_CHARGE_MIN + 1)
                {
                    System.Console.Out.WriteLine("charge color table length");
                    throw new System.NullReferenceException();
                }
                if (shapeClassBases.Length != SHAPE_MAX)
                {
                    System.Console.Out.WriteLine("graphicBaseClasses wrong length");
                    throw new System.NullReferenceException();
                }
                if (argbsAmino.Length != GROUPID_AMINO_MAX)
                {
                    System.Console.Out.WriteLine("argbsAmino wrong length");
                    throw new System.NullReferenceException();
                }
                if (argbsShapely.Length != GROUPID_SHAPELY_MAX)
                {
                    System.Console.Out.WriteLine("argbsShapely wrong length");
                    throw new System.NullReferenceException();
                }
            }
        }
    }
}