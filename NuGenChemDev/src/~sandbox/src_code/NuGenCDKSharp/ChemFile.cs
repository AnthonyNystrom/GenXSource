/*
*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $
*  $Revision: 6672 $
*
*  Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
*
*  Contact: cdk-devel@lists.sourceforge.net
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public License
*  as published by the Free Software Foundation; either version 2.1
*  of the License, or (at your option) any later version.
*
*  This program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public License
*  along with this program; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK
{
    /// <summary>  A Object containing a number of ChemSequences. This is supposed to be the
    /// top level container, which can contain all the concepts stored in a chemical
    /// document
    /// 
    /// </summary>
    /// <author>         steinbeck
    /// </author>
    /// <cdk.module>     data </cdk.module>
    [Serializable]
    public class ChemFile : ChemObject, ICloneable, IChemFile, IChemObjectListener
    {
        /// <summary>  Returns the array of ChemSequences of this container.
        /// 
        /// </summary>
        /// <returns>    The array of ChemSequences of this container
        /// </returns>
        /// <seealso cref="addChemSequence">
        /// </seealso>
        virtual public IChemSequence[] ChemSequences
        {
            get
            {
                ChemSequence[] returnChemSequences = new ChemSequence[ChemSequenceCount];
                Array.Copy(this.chemSequences, 0, returnChemSequences, 0, returnChemSequences.Length);
                return returnChemSequences;
            }

        }
        /// <summary>  Returns the number of ChemSequences in this Container.
        /// 
        /// </summary>
        /// <returns>    The number of ChemSequences in this Container
        /// </returns>
        virtual public int ChemSequenceCount
        {
            get
            {
                return this.chemSequenceCount;
            }

        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = 1926781734333430132L;

        /// <summary>  Array of ChemSquences.</summary>
        protected internal IChemSequence[] chemSequences;

        /// <summary>  Number of ChemSequences contained by this container.</summary>
        protected internal int chemSequenceCount;

        /// <summary>  Amount by which the chemsequence array grows when elements are added and
        /// the array is not large enough for that.
        /// </summary>
        protected internal int growArraySize = 4;


        /// <summary>  Constructs an empty ChemFile.</summary>
        public ChemFile()
        {
            chemSequenceCount = 0;
            chemSequences = new ChemSequence[growArraySize];
        }


        /// <summary>  Adds an ChemSequence to this container.
        /// 
        /// </summary>
        /// <param name="chemSequence"> The chemSequence to be added to this container
        /// </param>
        /// <seealso cref="getChemSequences">
        /// </seealso>
        public virtual void addChemSequence(IChemSequence chemSequence)
        {
            chemSequence.addListener(this);
            if (chemSequenceCount + 1 >= chemSequences.Length)
            {
                growChemSequenceArray();
            }
            chemSequences[chemSequenceCount] = chemSequence;
            chemSequenceCount++;
            notifyChanged();
        }


        /// <summary>  Returns the ChemSequence at position <code>number</code> in the container.
        /// 
        /// </summary>
        /// <param name="number"> The position of the ChemSequence to be returned.
        /// </param>
        /// <returns>         The ChemSequence at position <code>number</code>.
        /// </returns>
        /// <seealso cref="addChemSequence">
        /// </seealso>
        public virtual IChemSequence getChemSequence(int number)
        {
            return (ChemSequence)chemSequences[number];
        }


        /// <summary>  Grows the ChemSequence array by a given size.
        /// 
        /// </summary>
        /// <seealso cref="growArraySize">
        /// </seealso>
        protected internal virtual void growChemSequenceArray()
        {
            growArraySize = chemSequences.Length;
            IChemSequence[] newchemSequences = new ChemSequence[chemSequences.Length + growArraySize];
            Array.Copy(chemSequences, 0, newchemSequences, 0, chemSequences.Length);
            chemSequences = newchemSequences;
        }


        /// <summary> Returns a String representation of this class. It implements
        /// RFC #9.
        /// 
        /// </summary>
        /// <returns>    String representation of the Object
        /// </returns>
        public override System.String ToString()
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder();
            buffer.Append("ChemFile(#S=");
            IChemSequence[] seqs = ChemSequences;
            buffer.Append(seqs.Length);
            buffer.Append(", ");
            for (int i = 0; i < seqs.Length; i++)
            {
                IChemSequence sequence = seqs[i];
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                buffer.Append(sequence.ToString());
            }
            buffer.Append(')');
            return buffer.ToString();
        }


        /// <summary>  Allows for getting an clone of this object.
        /// 
        /// </summary>
        /// <returns>    a clone of this object
        /// </returns>
        public override System.Object Clone()
        {
            ChemFile clone = (ChemFile)base.Clone();
            // clone the chemModels
            clone.chemSequenceCount = ChemSequenceCount;
            clone.chemSequences = new ChemSequence[clone.chemSequenceCount];
            for (int f = 0; f < clone.chemSequenceCount; f++)
            {
                clone.chemSequences[f] = (ChemSequence)((ChemSequence)chemSequences[f]).Clone();
            }
            return clone;
        }


        /// <summary>  Called by objects to which this object has
        /// registered as a listener.
        /// 
        /// </summary>
        /// <param name="event"> A change event pointing to the source of the change
        /// </param>
        public virtual void stateChanged(IChemObjectChangeEvent event_Renamed)
        {
            notifyChanged(event_Renamed);
        }
    }
}