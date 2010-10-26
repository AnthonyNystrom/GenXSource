/*  $RCSfile$
*  $Author: kaihartmann $
*  $Date: 2006-06-07 11:41:42 +0200 (Wed, 07 Jun 2006) $
*  $Revision: 6349 $
*
*  Copyright (C) 2002-2006  The Chemistry Development Kit (CDK) Project
*
*  Contact: cdk-devel@lists.sourceforge.net
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
*  */
using System;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Math;

namespace Org.OpenScience.CDK.Smiles
{
    /// <summary> This is used to hold the invariance numbers for the cannonical labeling of
    /// AtomContainers.
    /// 
    /// </summary>
    /// <cdk.module>  standard </cdk.module>
    [Serializable]
    public class InvPair
    {
        virtual public long Last
        {
            get
            {
                return last;
            }

            set
            {
                last = value;
            }

        }
        virtual public long Curr
        {
            get
            {
                return curr;
            }

            /*
            * Todo make the following robust!
            */

            set
            {
                curr = value;
            }

        }
        virtual public IAtom Atom
        {
            get
            {
                return atom;
            }

            set
            {
                atom = value;
            }

        }

        private const long serialVersionUID = -1397634098919863122L;

        /// <summary>The description used to set the invatiance numbers in the atom's property</summary>
        public static System.String INVARIANCE_PAIR = "InvariancePair";

        private long last = 0;

        private long curr = 0;

        private IAtom atom;

        private int prime;

        public InvPair()
        {
        }

        public InvPair(long c, IAtom a)
        {
            curr = c;
            atom = a;
            a.setProperty(INVARIANCE_PAIR, this);
        }

        public override bool Equals(System.Object e)
        {
            if (e is InvPair)
            {
                InvPair o = (InvPair)e;
                //      System.out.println("Last " + last + "o.last " + o.getLast() + " curr " + curr + " o.curr " + o.getCurr() + " equals " +(last == o.getLast() && curr == o.getCurr()));
                return (last == o.Last && curr == o.Curr);
            }
            else
            {
                return false;
            }
        }

        public virtual void comit()
        {
            atom.setProperty("CanonicalLable", (System.Object)curr);
        }

        public override System.String ToString()
        {
            System.Text.StringBuilder buff = new System.Text.StringBuilder();
            buff.Append(curr);
            buff.Append("\t");
            return buff.ToString();
        }

        public virtual int getPrime()
        {
            return prime;
        }

        public virtual void setPrime()
        {
            prime = Primes.getPrimeAt((int)curr - 1);
        }
        //UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}