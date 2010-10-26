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
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;

namespace Org.OpenScience.CDK.Math
{
    /// <summary> Class providing convenience methods for simple mathematical operations.
    /// 
    /// </summary>
    /// <cdk.module>  standard </cdk.module>
    public class MathTools
    {
        /// <summary> Analog of Math.max that returns the largest int value in an array of ints
        /// 
        /// </summary>
        public static int max(int[] values)
        {
            int max = values[0];
            for (int f = 0; f < values.Length; f++)
            {
                if (values[f] > max)
                {
                    max = values[f];
                }
            }
            return max;
        }

        /// <summary> Analog of Math.max that returns the largest int value in an array of ints
        /// 
        /// </summary>
        public static int min(int[] values)
        {
            int min = values[0];
            for (int f = 0; f < values.Length; f++)
            {
                if (values[f] < min)
                {
                    min = values[f];
                }
            }
            return min;
        }

        public static bool isOdd(int intValue)
        {
            return !MathTools.isEven(intValue);
        }

        public static bool isEven(int intValue)
        {
            if (System.Math.Floor((double)intValue / 2.0) * 2.0 == (double)intValue)
            {
                return true;
            }
            return false;
        }
    }
}