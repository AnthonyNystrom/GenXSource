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
    /// <summary> A set of molecules, for example those taking part in a reaction.
    /// 
    /// To retrieve the molecules from the set, there are two options:
    /// 
    /// <pre>
    /// Molecule[] mols = setOfMolecules.getMolecules();
    /// for (int i=0; i < mols.length; i++) {
    /// Molecule mol = mols[i];
    /// }
    /// </pre>
    /// 
    /// and
    /// 
    /// <pre>
    /// for (int i=0; i < setOfMolecules.getMoleculeCount(); i++) {
    /// Molecule mol = setOfMolecules.getMolecule(i);
    /// }
    /// </pre>
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  reaction </cdk.keyword>
    /// <cdk.keyword>  molecule </cdk.keyword>
    [Serializable]
    public class SetOfMolecules : SetOfAtomContainers, ISetOfMolecules, System.ICloneable
    {
        /// <summary>  Returns the array of Molecules of this container.
        /// 
        /// </summary>
        /// <returns>    The array of Molecules of this container 
        /// </returns>
        /// <seealso cref="setMolecules">
        /// </seealso>
        virtual public IMolecule[] Molecules
        {
            get
            {
                Molecule[] result = new Molecule[base.AtomContainerCount];
                IAtomContainer[] containers = base.AtomContainers;
                for (int i = 0; i < containers.Length; i++)
                {
                    result[i] = (Molecule)containers[i];
                }
                return result;
            }

            set
            {
                if (atomContainerCount > 0)
                    removeAllAtomContainers();
                for (int f = 0; f < value.Length; f++)
                {
                    addMolecule(value[f]);
                }
            }

        }
        /// <summary> Returns the number of Molecules in this Container.
        /// 
        /// </summary>
        /// <returns>     The number of Molecules in this Container
        /// </returns>
        virtual public int MoleculeCount
        {
            get
            {
                return base.AtomContainerCount;
            }

        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// 
        /// </summary>
        private const long serialVersionUID = -861287315770869699L;

        public SetOfMolecules()
        {
        }

        /// <summary>  Adds an molecule to this container.
        /// 
        /// </summary>
        /// <param name="molecule"> The molecule to be added to this container 
        /// </param>
        public virtual void addMolecule(IMolecule molecule)
        {
            base.addAtomContainer(molecule);
            /* notifyChanged() called in super.addAtomContainer() */
        }

        /// <summary>  Adds all molecules in the SetOfMolecules to this container.
        /// 
        /// </summary>
        /// <param name="moleculeSet"> The SetOfMolecules 
        /// </param>
        public virtual void add(ISetOfMolecules moleculeSet)
        {
            IMolecule[] mols = moleculeSet.Molecules;
            for (int i = 0; i < mols.Length; i++)
            {
                addMolecule(mols[i]);
            }
            /* notifyChanged() called in super.addAtomContainer() */
        }


        /// <summary>  
        /// Returns the Molecule at position <code>number</code> in the
        /// container.
        /// 
        /// </summary>
        /// <param name="number"> The position of the Molecule to be returned. 
        /// </param>
        /// <returns>         The Molecule at position <code>number</code> . 
        /// </returns>
        public virtual IMolecule getMolecule(int number)
        {
            return (Molecule)base.getAtomContainer(number);
        }


        /// <summary>  Clones this SetOfMolecules and its content.
        /// 
        /// </summary>
        /// <returns>    the cloned object
        /// </returns>
        public override System.Object Clone()
        {
            SetOfMolecules clone = (SetOfMolecules)base.Clone();
            IMolecule[] result = Molecules;
            for (int i = 0; i < result.Length; i++)
            {
                clone.addMolecule((Molecule)result[i].Clone());
            }
            return (System.Object)clone;
        }

        public override System.String ToString()
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder();
            buffer.Append("SetOfMolecules(");
            buffer.Append(base.ToString());
            buffer.Append(')');
            return buffer.ToString();
        }

        /// <summary>  Called by objects to which this object has
        /// registered as a listener.
        /// 
        /// </summary>
        /// <param name="event"> A change event pointing to the source of the change
        /// </param>
        public override void stateChanged(IChemObjectChangeEvent event_Renamed)
        {
            notifyChanged(event_Renamed);
        }
    }
}