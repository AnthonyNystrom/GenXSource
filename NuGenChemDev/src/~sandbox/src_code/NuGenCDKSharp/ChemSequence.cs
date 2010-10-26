/* $RCSfile$
* $Author: egonw $    
* $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $    
* $Revision: 6672 $
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
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK
{
    /// <summary> A sequence of ChemModels, which can, for example, be used to
    /// store the course of a reaction. Each state of the reaction would be
    /// stored in one ChemModel.
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  animation </cdk.keyword>
    /// <cdk.keyword>  reaction </cdk.keyword>
    [Serializable]
    public class ChemSequence : ChemObject, IChemSequence, IChemObjectListener, ICloneable
    {
        /// <summary> Returns an array of ChemModels of length matching the number of ChemModels 
        /// in this container.
        /// 
        /// </summary>
        /// <returns>    The array of ChemModels in this container
        /// 
        /// </returns>
        /// <seealso cref="addChemModel">
        /// </seealso>
        virtual public IChemModel[] ChemModels
        {
            get
            {
                IChemModel[] returnModels = new ChemModel[ChemModelCount];
                Array.Copy(this.chemModels, 0, returnModels, 0, returnModels.Length);
                return returnModels;
            }

        }
        /// <summary> Returns the number of ChemModels in this Container.
        /// 
        /// </summary>
        /// <returns>    The number of ChemModels in this Container
        /// </returns>
        virtual public int ChemModelCount
        {
            get
            {
                return this.chemModelCount;
            }

        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = 2199218627455492000L;

        /// <summary>  Array of ChemModels.</summary>
        protected internal IChemModel[] chemModels;

        /// <summary>  Number of ChemModels contained by this container.</summary>
        protected internal int chemModelCount;

        /// <summary>  Amount by which the chemModels array grows when elements are added and
        /// the array is not large enough for that. 
        /// </summary>
        protected internal int growArraySize = 4;



        /// <summary>  Constructs an empty ChemSequence.</summary>
        public ChemSequence()
        {
            chemModelCount = 0;
            chemModels = new ChemModel[growArraySize];
        }



        /// <summary>  Adds an chemModel to this container.
        /// 
        /// </summary>
        /// <param name="chemModel"> The chemModel to be added to this container
        /// 
        /// </param>
        /// <seealso cref="getChemModel">
        /// </seealso>
        public virtual void addChemModel(IChemModel chemModel)
        {
            if (chemModelCount + 1 >= chemModels.Length)
            {
                growChemModelArray();
            }
            chemModels[chemModelCount] = chemModel;
            chemModelCount++;
            chemModel.addListener(this);
            notifyChanged();
        }


        /// <summary> 
        /// Returns the ChemModel at position <code>number</code> in the
        /// container.
        /// 
        /// </summary>
        /// <param name="number"> The position of the ChemModel to be returned.
        /// </param>
        /// <returns>         The ChemModel at position <code>number</code>.
        /// 
        /// </returns>
        /// <seealso cref="addChemModel">
        /// </seealso>
        public virtual IChemModel getChemModel(int number)
        {
            return chemModels[number];
        }

        /// <summary>  Grows the chemModel array by a given size.
        /// 
        /// </summary>
        /// <seealso cref="growArraySize">
        /// </seealso>
        protected internal virtual void growChemModelArray()
        {
            ChemModel[] newchemModels = new ChemModel[chemModels.Length + growArraySize];
            Array.Copy(chemModels, 0, newchemModels, 0, chemModels.Length);
            chemModels = newchemModels;
        }

        public override System.String ToString()
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder(32);
            buffer.Append("ChemSequence(#M=");
            IChemModel[] models = ChemModels;
            buffer.Append(models.Length);
            buffer.Append(", ");
            for (int i = 0; i < models.Length; i++)
            {
                IChemModel model = models[i];
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                buffer.Append(model.ToString());
            }
            buffer.Append(')');
            return buffer.ToString();
        }

        public override System.Object Clone()
        {
            ChemSequence clone = (ChemSequence)base.Clone();
            // clone the chemModels
            clone.chemModelCount = ChemModelCount;
            clone.chemModels = new ChemModel[clone.chemModelCount];
            for (int f = 0; f < clone.chemModelCount; f++)
            {
                clone.chemModels[f] = (ChemModel)((ChemModel)chemModels[f]).Clone();
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