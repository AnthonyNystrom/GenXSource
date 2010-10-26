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
    /// <summary> An object containig multiple SetOfMolecules and 
    /// the other lower level concepts like rings, sequences, 
    /// fragments, etc.
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    [Serializable]
    public class ChemModel : ChemObject, IChemModel, IChemObjectListener, ICloneable
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the SetOfMolecules of this ChemModel.
        /// 
        /// </summary>
        /// <returns>   The SetOfMolecules of this ChemModel
        /// 
        /// </returns>
        /// <seealso cref="setSetOfMolecules">
        /// </seealso>
        /// <summary> Sets the SetOfMolecules of this ChemModel.
        /// 
        /// </summary>
        /// <param name="setOfMolecules"> the content of this model
        /// 
        /// </param>
        /// <seealso cref="getSetOfMolecules">
        /// </seealso>
        virtual public ISetOfMolecules SetOfMolecules
        {
            get
            {
                return (SetOfMolecules)this.setOfMolecules;
            }

            set
            {
                this.setOfMolecules = value;
                this.setOfMolecules.addListener(this);
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the RingSet of this ChemModel.
        /// 
        /// </summary>
        /// <returns> the ringset of this model
        /// 
        /// </returns>
        /// <seealso cref="setRingSet">
        /// </seealso>
        /// <summary> Sets the RingSet of this ChemModel.
        /// 
        /// </summary>
        /// <param name="ringSet">        the content of this model
        /// 
        /// </param>
        /// <seealso cref="getRingSet">
        /// </seealso>
        virtual public IRingSet RingSet
        {
            get
            {
                return this.ringSet;
            }

            set
            {
                this.ringSet = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Gets the Crystal contained in this ChemModel.
        /// 
        /// </summary>
        /// <returns> The crystal in this model
        /// 
        /// </returns>
        /// <seealso cref="setCrystal">
        /// </seealso>
        /// <summary> Sets the Crystal contained in this ChemModel.
        /// 
        /// </summary>
        /// <param name="crystal"> the Crystal to store in this model
        /// 
        /// </param>
        /// <seealso cref="getCrystal">
        /// </seealso>
        virtual public ICrystal Crystal
        {
            get
            {
                return this.crystal;
            }

            set
            {
                this.crystal = value;
                this.crystal.addListener(this);
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Gets the SetOfReactions contained in this ChemModel.
        /// 
        /// </summary>
        /// <returns> The SetOfReactions in this model
        /// 
        /// </returns>
        /// <seealso cref="setSetOfReactions">
        /// </seealso>
        /// <summary> Sets the SetOfReactions contained in this ChemModel.
        /// 
        /// </summary>
        /// <param name="sor">the SetOfReactions to store in this model
        /// 
        /// </param>
        /// <seealso cref="getSetOfReactions">
        /// </seealso>
        virtual public ISetOfReactions SetOfReactions
        {
            get
            {
                return this.setOfReactions;
            }

            set
            {
                this.setOfReactions = value;
                this.setOfReactions.addListener(this);
                notifyChanged();
            }

        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = -5213425310451366185L;

        /// <summary>  A SetOfMolecules.</summary>
        protected internal ISetOfMolecules setOfMolecules = null;

        /// <summary>  A SetOfReactions.</summary>
        protected internal ISetOfReactions setOfReactions = null;

        /// <summary>  A RingSet.</summary>
        protected internal IRingSet ringSet = null;

        /// <summary>  A Crystal.</summary>
        protected internal ICrystal crystal = null;

        /// <summary>  Constructs an new ChemModel with a null setOfMolecules.</summary>
        public ChemModel()
        {
        }

        /// <summary> Returns a String representation of the contents of this
        /// IChemObject.
        /// 
        /// </summary>
        /// <returns> String representation of content
        /// </returns>
        public override System.String ToString()
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder(64);
            buffer.Append("ChemModel(");
            buffer.Append(GetHashCode());
            if (SetOfMolecules != null)
            {
                buffer.Append(", ");
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                buffer.Append(SetOfMolecules.ToString());
            }
            else
            {
                buffer.Append(", No SetOfMolecules");
            }
            if (Crystal != null)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                buffer.Append(Crystal.ToString());
                buffer.Append(", ");
            }
            else
            {
                buffer.Append(", No Crystal");
            }
            if (SetOfReactions != null)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                buffer.Append(SetOfReactions.ToString());
                buffer.Append(", ");
            }
            else
            {
                buffer.Append(", No SetOfReactions");
            }
            buffer.Append(')');
            return buffer.ToString();
        }

        /// <summary> Clones this <code>ChemModel</code> and its content.
        /// 
        /// </summary>
        /// <returns>  The cloned object
        /// </returns>
        public override System.Object Clone()
        {
            ChemModel clone = (ChemModel)base.Clone();
            // clone the content
            if (setOfMolecules != null)
            {
                clone.setOfMolecules = (SetOfMolecules)((SetOfMolecules)setOfMolecules).Clone();
            }
            else
            {
                clone.setOfMolecules = null;
            }
            if (setOfReactions != null)
            {
                clone.setOfReactions = (ISetOfReactions)((SetOfReactions)setOfReactions).Clone();
            }
            else
            {
                clone.setOfReactions = null;
            }
            if (crystal != null)
            {
                clone.crystal = (Crystal)((Crystal)crystal).Clone();
            }
            else
            {
                clone.crystal = null;
            }
            if (ringSet != null)
            {
                clone.ringSet = (RingSet)((RingSet)ringSet).Clone();
            }
            else
            {
                clone.ringSet = null;
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