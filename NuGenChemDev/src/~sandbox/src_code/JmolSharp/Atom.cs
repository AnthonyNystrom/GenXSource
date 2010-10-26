/* $RCSfile: Atom.java,v $
* $Author: migueljmol $
* $Date: 2005/06/05 15:51:17 $
* $Revision: 1.24 $
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

namespace Org.Jmol.Adapter.Smarter
{
    class Atom : System.ICloneable
    {
        virtual internal System.String ElementSymbol
        {
            get
            {
                if (elementSymbol == null)
                    if (atomName != null)
                    {
                        int len = atomName.Length;
                        int ichFirst = 0;
                        char chFirst = (char)(0);
                        while (ichFirst < len && !isValidFirstSymbolChar(chFirst = atomName[ichFirst]))
                            ++ichFirst;
                        switch (len - ichFirst)
                        {

                            case 0:
                                break;

                            default:
                                char chSecond = atomName[ichFirst + 1];
                                if (isValidElementSymbolNoCaseSecondChar(chFirst, chSecond))
                                {
                                    elementSymbol = "" + chFirst + chSecond;
                                    break;
                                }
                                // fall into
                                goto case 1;


                            case 1:
                                if (isValidElementSymbol(chFirst))
                                    elementSymbol = "" + chFirst;
                                break;
                        }
                    }
                return elementSymbol;
            }

        }
        internal int atomSetIndex;
        internal System.String elementSymbol;
        internal sbyte elementNumber = -1;
        internal System.String atomName;
        internal int formalCharge = System.Int32.MinValue;
        internal float partialCharge = System.Single.NaN;
        internal float x = System.Single.NaN, y = System.Single.NaN, z = System.Single.NaN;
        internal float vectorX = System.Single.NaN, vectorY = System.Single.NaN, vectorZ = System.Single.NaN;
        internal float bfactor = System.Single.NaN;
        internal int occupancy = 100;
        internal bool isHetero;
        internal int atomSerial = System.Int32.MinValue;
        internal char chainID = '\x0000';
        internal char alternateLocationID = '\x0000';
        internal System.String group3;
        internal int sequenceNumber = System.Int32.MinValue;
        internal char insertionCode = '\x0000';

        internal Atom()
        {
        }

        internal virtual Atom cloneAtom()
        {
            try
            {
                return (Atom)base.MemberwiseClone();
            }
            //UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
            catch (System.Exception cnse)
            {
                return null;
            }
        }

        /// <summary> Bits which indicate whether or not an element symbol is valid.
        /// <p>
        /// If the high bit is set, then it is valid as a standalone char.
        /// otherwise, bits 0-25 say whether or not is valid when followed
        /// by the letters a-z.
        /// </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'elementCharMasks'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        internal static readonly int[] elementCharMasks = new int[]{1 << (int) ('c' - 'a') | 1 << (int) ('g' - 'a') | 1 << (int) ('l' - 'a') | 1 << (int) ('m' - 'a') | 1 << (int) ('r' - 'a') | 1 << (int) ('s' - 'a') | 1 << (int) ('t' - 'a') | 1 << (int) ('u' - 'a'), 1 << 31 | 1 << (int) ('a' - 'a') | 1 << (int) ('e' - 'a') | 1 << (int) ('h' - 'a') | 1 << (int) ('i' - 'a') | 1 << (int) ('k' - 'a') | 1 << (int) ('r' - 'a'), 1 << 31 | 1 << (int) ('a' - 'a') | 1 << (int) ('d' - 'a') | 1 << (int) ('e' - 'a') | 1 << (int) ('f' - 'a') | 1 << (int) ('l' - 'a') | 1 << (int) ('m' - 'a') | 1 << (int) ('o' - 'a') | 1 << (int) ('r' - 'a') | 1 << (int) ('s' - 'a') | 1 << (int) ('u' - 'a'), 1 << (int) ('d' - 'a') | 1 << (int) ('y' - 'a'), 1 << (int) ('r' - 'a') | 1 << (int) ('s' - 'a') | 1 << (int) ('u' - 'a'), 1 << 31 | 1 << (int) ('e' - 'a') | 1 << (int) ('m' - 'a') | 1 << (int) ('r' - 'a'), 1 << (int) ('a' - 'a') | 1 << (int) ('d' - 'a') | 1 << (int) ('e' - 'a'), 1 << 31 | 1 << (int) ('e' - 'a') | 1 << (int) ('f' - 'a') | 1 << (int) ('g' - 'a') | 1 << (int) ('o' - 'a') | 1 << (int) ('s' - 'a'), 1 << 31 | 1 << (int) ('n' - 'a') | 1 << (int) ('r' - 'a'), 0, 1 << 31 | 1 << (int) ('r' - 'a'), 1 << (int) ('a' - 'a') | 1 << (int) ('i' - 'a') | 1 << (int) ('r' - 'a') | 1 << (int) ('u' - 'a'), 1 << (int) ('d' - 'a') | 1 << (int) ('g' - 'a') | 1 << (int) ('n' - 'a') | 1 << (int) ('o' - 'a') | 1 << (int) ('t' - 'a'), 1 << 31 | 1 << (int) ('a' - 'a') | 1 << (int) ('b' - 'a') | 1 << (int) ('d' - 'a') | 1 << (int) ('e' - 'a') | 1 << (int) ('i' - 'a') | 1 << (int) ('o' - 'a') | 1 << (int) ('p' - 'a'), 1 << 31 | 1 << (int) ('s' - 'a'), 1 << 31 | 1 << (int) ('a' - 'a') | 1 << (int) ('b' - 'a') | 1 << (int) ('d' - 'a') | 1 << (int) ('m' - 'a') | 1 << (int) ('o' - 'a') | 1 << (int) ('r' - 'a') | 1 << (int) ('t' - 'a') | 1 << (int) ('u' - 'a'), 0, 1 << (int) ('a' - 'a') | 1 << (int) ('b' - 'a') | 1 << (int) ('e' - 'a') | 1 << (int) ('f' - 'a') | 1 << (int) ('h' - 'a') | 1 << (int) ('n' - 'a') | 1 << (int) ('u' - 'a'), 1 << 31 | 1 << (int) 
			('b' - 'a') | 1 << (int) ('c' - 'a') | 1 << (int) ('e' - 'a') | 1 << (int) ('g' - 'a') | 1 << (int) ('i' - 'a') | 1 << (int) ('m' - 'a') | 1 << (int) ('n' - 'a') | 1 << (int) ('r' - 'a'), 1 << (int) ('a' - 'a') | 1 << (int) ('b' - 'a') | 1 << (int) ('c' - 'a') | 1 << (int) ('e' - 'a') | 1 << (int) ('h' - 'a') | 1 << (int) ('i' - 'a') | 1 << (int) ('l' - 'a') | 1 << (int) ('m' - 'a'), 1 << 31, 1 << 31, 1 << 31, 1 << (int) ('e' - 'a') | 1 << (int) ('x' - 'a'), 1 << 31 | 1 << (int) ('b' - 'a'), 1 << (int) ('n' - 'a') | 1 << (int) ('r' - 'a')};

        internal static bool isValidElementSymbol(char ch)
        {
            return ch >= 'A' && ch <= 'Z' && elementCharMasks[ch - 'A'] < 0;
        }

        internal static bool isValidElementSymbol(char chFirst, char chSecond)
        {
            if (chFirst < 'A' || chFirst > 'Z' || chSecond < 'a' || chSecond > 'z')
                return false;
            return ((elementCharMasks[chFirst - 'A'] >> (int)(chSecond - 'a')) & 1) != 0;
        }

        internal static bool isValidElementSymbolNoCaseSecondChar(char chFirst, char chSecond)
        {
            if (chSecond >= 'A' && chSecond <= 'Z')
                chSecond = (char)((int)chSecond + ((int)'a' - (int)'A'));
            if (chFirst < 'A' || chFirst > 'Z' || chSecond < 'a' || chSecond > 'z')
                return false;
            return ((elementCharMasks[chFirst - 'A'] >> (int)(chSecond - 'a')) & 1) != 0;
        }

        internal static bool isValidFirstSymbolChar(char ch)
        {
            return ch >= 'A' && ch <= 'Z' && elementCharMasks[ch - 'A'] != 0;
        }

        internal static bool isValidElementSymbolNoCaseSecondChar(System.String str)
        {
            if (str == null)
                return false;
            int length = str.Length;
            if (length == 0)
                return false;
            char chFirst = str[0];
            if (length == 1)
                return isValidElementSymbol(chFirst);
            if (length > 2)
                return false;
            char chSecond = str[1];
            return isValidElementSymbolNoCaseSecondChar(chFirst, chSecond);
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
        virtual public System.Object Clone()
        {
            return null;
        }
    }
}