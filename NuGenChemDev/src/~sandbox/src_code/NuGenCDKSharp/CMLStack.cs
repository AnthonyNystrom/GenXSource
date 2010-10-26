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

namespace Org.OpenScience.CDK.IO.CML
{
    /// <summary> Low weigth alternative to Sun's Stack class.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  stack </cdk.keyword>
    public class CMLStack
    {
        internal System.String[] stack = new System.String[64];
        internal int sp = 0;

        /// <summary> Adds an entry to the stack.</summary>
        public virtual void push(System.String item)
        {
            if (sp == stack.Length)
            {
                System.String[] temp = new System.String[2 * sp];
                Array.Copy(stack, 0, temp, 0, sp);
                stack = temp;
            }
            stack[sp++] = item;
        }

        /// <summary> Retrieves and deletes to last added entry.
        /// 
        /// </summary>
        /// <seealso cref="current()">
        /// </seealso>
        public virtual System.String pop()
        {
            return stack[--sp];
        }

        /// <summary> Returns the last added entry.
        /// 
        /// </summary>
        /// <seealso cref="pop()">
        /// </seealso>
        public virtual System.String current()
        {
            if (sp > 0)
            {
                return stack[sp - 1];
            }
            else
            {
                return "";
            }
        }

        /// <summary> Returns a String representation of the stack.</summary>
        public override System.String ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("/");
            for (int i = 0; i < sp; ++i)
            {
                sb.Append(stack[i]);
                sb.Append("/");
            }
            return sb.ToString();
        }
    }
}