/* $RCSfile$
* $Author: egonw $    
* $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $    
* $Revision: 6672 $
* 
* Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
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
using Support;

namespace Org.OpenScience.CDK
{
    /// <summary> A BioPolymer is a subclass of a Polymer which is supposed to store
    /// additional informations about the Polymer which are connected to BioPolymers.
    /// 
    /// </summary>
    /// <cdk.module>   data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       Edgar Luttmann <edgar@uni-paderborn.de>
    /// </author>
    /// <author>       Martin Eklund
    /// </author>
    /// <cdk.created>  2001-08-06  </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  polymer </cdk.keyword>
    /// <cdk.keyword>  biopolymer </cdk.keyword>
    [Serializable]
    public class BioPolymer : Polymer, IBioPolymer
    {
        /// <summary> Return the number of monomers present in BioPolymer.
        /// 
        /// </summary>
        /// <returns> number of monomers
        /// 
        /// </returns>
        override public int MonomerCount
        {
            get
            {
                System.Collections.IEnumerator keys = strands.Keys.GetEnumerator();
                int number = 0;

                //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
                while (keys.MoveNext())
                {
                    //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
                    Strand tmp = (Strand)strands[keys.Current]; // Cast exception?!
                    number += (tmp.Monomers).Count - 1;
                }
                return number;
            }

        }
        /// <summary> Returns a collection of the names of all <code>Monomer</code>s in this
        /// BioPolymer.
        /// 
        /// </summary>
        /// <returns> a <code>Collection</code> of all the monomer names.
        /// </returns>
        override public System.Collections.ICollection MonomerNames
        {
            get
            {
                System.Collections.IEnumerator keys = strands.Keys.GetEnumerator();
                System.Collections.Hashtable monomers = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());

                //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
                while (keys.MoveNext())
                {
                    //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
                    Strand oStrand = (Strand)strands[keys.Current];
                    SupportClass.MapSupport.PutAll(monomers, oStrand.Monomers);
                }
                return new CSGraphT.SupportClass.HashSetSupport(monomers);
            }

        }
        /// <summary> 
        /// Return the number of strands present in the BioPolymer.
        /// 
        /// </summary>
        /// <returns> number of strands
        /// 
        /// </returns>
        virtual public int StrandCount
        {
            get
            {
                return strands.Count;
            }

        }
        /// <summary> Returns a collection of the names of all <code>Strand</code>s in this
        /// BioPolymer.
        /// 
        /// </summary>
        /// <returns> a <code>Collection</code> of all the strand names.
        /// </returns>
        virtual public System.Collections.ICollection StrandNames
        {
            get
            {
                return new CSGraphT.SupportClass.HashSetSupport(strands);
            }

        }
        /// <returns> hashtable containing the monomers in the strand.
        /// </returns>
        virtual public System.Collections.Hashtable Strands
        {
            get
            {
                return strands;
            }

        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = -5001873073769634393L;

        private System.Collections.Hashtable strands; // the list of all the contained Strands.

        /// <summary> Contructs a new Polymer to store the Strands.</summary>
        public BioPolymer()
            : base()
        {
            // Strand stuff
            strands = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
        }

        /// <summary> Adds the atom oAtom to a specified Strand, whereas the Monomer is unspecified. Hence
        /// the atom will be added to a Monomer of type UNKNOWN in the specified Strand.
        /// 
        /// </summary>
        /// <param name="oAtom">  The atom to add
        /// </param>
        /// <param name="oStrand">The strand the atom belongs to
        /// </param>
        public virtual void addAtom(IAtom oAtom, IStrand oStrand)
        {

            int atomCount = base.AtomCount;

            // Add atom to AtomContainer
            base.addAtom(oAtom);

            if (atomCount != base.AtomCount && oStrand != null)
            {
                // Maybe better to throw nullpointer exception here, so user realises that
                // Strand == null and Atom only gets added to this BioPolymer, but not to a Strand.
                oStrand.addAtom(oAtom);
                if (!strands.ContainsValue(oStrand.StrandName))
                {
                    strands[oStrand.StrandName] = oStrand;
                }
            }
            /* notifyChanged() is called by addAtom in
            AtomContainer */
        }

        /// <summary> Adds the atom to a specified Strand and a specified Monomer.
        /// 
        /// </summary>
        /// <param name="oAtom">
        /// </param>
        /// <param name="oMonomer">
        /// </param>
        /// <param name="oStrand">
        /// </param>
        public virtual void addAtom(IAtom oAtom, IMonomer oMonomer, IStrand oStrand)
        {

            int atomCount = base.AtomCount;

            // Add atom to AtomContainer
            base.addAtom(oAtom);

            if (atomCount != base.AtomCount && oStrand != null)
            {
                oStrand.addAtom(oAtom, oMonomer); // Same problem as above: better to throw nullpointer exception?
                if (!strands.ContainsKey(oStrand.StrandName))
                {
                    strands[oStrand.StrandName] = oStrand;
                }
            }
            /* The reasoning above is: 
            * All Monomers have to belong to a Strand and all atoms belonging to strands have to belong to a Monomer =>
            * ? oMonomer != null and oStrand != null, oAtom is added to BioPolymer and to oMonomer in oStrand
            * ? oMonomer == null and oStrand != null, oAtom is added to BioPolymer and default Monomer in oStrand
            * ? oMonomer != null and oStrand == null, oAtom is added to BioPolymer, but not to a Monomer or Strand (especially good to maybe throw exception in this case)
            * ? oMonomer == null and oStrand == null, oAtom is added to BioPolymer, but not to a Monomer or Strand
            * */
        }

        /// <summary> Retrieve a Monomer object by specifying its name. [You have to specify the strand to enable
        /// monomers with the same name in different strands. There is at least one such case: every
        /// strand contains a monomer called "".]
        /// 
        /// </summary>
        /// <param name="monName"> The name of the monomer to look for
        /// </param>
        /// <returns> The Monomer object which was asked for
        /// 
        /// </returns>
        public virtual IMonomer getMonomer(System.String monName, System.String strandName)
        {
            Strand strand = (Strand)strands[strandName];

            if (strand != null)
            {
                return (Monomer)strand.getMonomer(monName);
            }
            else
            {
                return null;
            }
        }

        /*	Could look like this if you ensured individual name giving for ALL monomers:
        * 	
        public Monomer getMonomer(String cName) {
        Enumeration keys = strands.keys();
        Monomer oMonomer = null;
		
        while(keys.hasMoreElements())	{
		
        if(((Strand)strands.get(keys.nextElement())).getMonomers().containsKey(cName))	{
        Strand oStrand = (Strand)strands.get(keys.nextElement());
        oMonomer = oStrand.getMonomer(cName);
        break;
        }
        }
        return oMonomer;
        }
        */

        /// <summary> 
        /// Retrieve a Monomer object by specifying its name.
        /// 
        /// </summary>
        /// <param name="cName"> The name of the monomer to look for
        /// </param>
        /// <returns> The Monomer object which was asked for
        /// 
        /// </returns>
        public virtual IStrand getStrand(System.String cName)
        {
            return (Strand)strands[cName];
        }

        /// <summary> Removes a particular strand, specified by its name.
        /// 
        /// </summary>
        /// <param name="name">name of the strand to remove
        /// </param>
        public virtual void removeStrand(System.String name)
        {
            if (strands.ContainsKey(name))
            {
                Strand strand = (Strand)strands[name];
                this.remove(strand);
                strands.Remove(name);
            }
        }

        public override System.String ToString()
        {
            System.Text.StringBuilder stringContent = new System.Text.StringBuilder();
            stringContent.Append("BioPolymer(");
            stringContent.Append(this.GetHashCode()).Append(", ");
            stringContent.Append(base.ToString());
            stringContent.Append(')');
            return stringContent.ToString();
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
        virtual public System.Object Clone()
        {
            return null;
        }
    }
}