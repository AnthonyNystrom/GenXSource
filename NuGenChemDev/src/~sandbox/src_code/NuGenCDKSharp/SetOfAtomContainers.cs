/*
*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $
*  $Revision: 6672 $
*
*  Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
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
    /// <summary> A set of AtomContainers.
    /// 
    /// </summary>
    /// <author>         hel
    /// </author>
    /// <cdk.module>     data </cdk.module>
    [Serializable]
    public class SetOfAtomContainers : ChemObject, ISetOfAtomContainers, IChemObjectListener, ICloneable
    {
        /// <summary>  Returns the array of AtomContainers of this container.
        /// 
        /// </summary>
        /// <returns>    The array of AtomContainers of this container
        /// </returns>
        virtual public IAtomContainer[] AtomContainers
        {
            get
            {
                IAtomContainer[] result = new AtomContainer[atomContainerCount];
                Array.Copy(this.atomContainers, 0, result, 0, result.Length);
                return result;
            }

        }
        /// <summary> Returns the number of AtomContainers in this Container.
        /// 
        /// </summary>
        /// <returns>    The number of AtomContainers in this Container
        /// </returns>
        virtual public int AtomContainerCount
        {
            get
            {
                return atomContainerCount;
            }
        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is incompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide/serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = -521290297592768395L;

        /// <summary>Array of AtomContainers. </summary>
        protected internal IAtomContainer[] atomContainers;

        /// <summary>Number of AtomContainers contained by this container. </summary>
        protected internal int atomContainerCount;

        /// <summary> Defines the number of instances of a certain molecule
        /// in the set. It is 1 by default.
        /// </summary>
        protected internal double[] multipliers;

        /// <summary>  Amount by which the AtomContainers array grows when elements are added and
        /// the array is not large enough for that.
        /// </summary>
        protected internal int growArraySize = 5;


        /// <summary>Constructs an empty SetOfAtomContainers. </summary>
        public SetOfAtomContainers()
        {
            atomContainerCount = 0;
            atomContainers = new AtomContainer[growArraySize];
            multipliers = new double[growArraySize];
        }

        /// <summary> Adds an atomContainer to this container.
        /// 
        /// </summary>
        /// <param name="atomContainer"> The atomContainer to be added to this container
        /// </param>
        public virtual void addAtomContainer(IAtomContainer atomContainer)
        {
            atomContainer.addListener(this);
            addAtomContainer(atomContainer, 1.0);
            /*
            *  notifyChanged is called below
            */
        }

        /// <summary> Removes an AtomContainer from this container.
        /// 
        /// </summary>
        /// <param name="atomContainer"> The atomContainer to be removed from this container
        /// </param>
        public virtual void removeAtomContainer(IAtomContainer atomContainer)
        {
            for (int i = 0; i < atomContainerCount; i++)
            {
                if (atomContainers[i] == atomContainer)
                    removeAtomContainer(i);
            }
        }

        /// <summary> Removes all AtomContainer from this container.</summary>
        public virtual void removeAllAtomContainers()
        {
            for (int pos = atomContainerCount - 1; pos >= 0; pos--)
            {
                atomContainers[pos].removeListener(this);
                multipliers[pos] = 0;
                atomContainers[pos] = null;
            }
            atomContainerCount = 0;
            notifyChanged();
        }


        /// <summary> Removes an AtomContainer from this container.
        /// 
        /// </summary>
        /// <param name="pos"> The position of the AtomContainer to be removed from this container
        /// </param>
        public virtual void removeAtomContainer(int pos)
        {
            atomContainers[pos].removeListener(this);
            for (int i = pos; i < atomContainerCount - 1; i++)
            {
                atomContainers[i] = atomContainers[i + 1];
                multipliers[i] = multipliers[i + 1];
            }
            atomContainers[atomContainerCount - 1] = null;
            atomContainerCount--;
            notifyChanged();
        }

        /// <summary> Sets the coefficient of a AtomContainer to a given value.
        /// 
        /// </summary>
        /// <param name="container">  The AtomContainer for which the multiplier is set
        /// </param>
        /// <param name="multiplier"> The new multiplier for the AtomContatiner
        /// </param>
        /// <returns>             true if multiplier has been set
        /// </returns>
        /// <seealso cref="getMultiplier(IAtomContainer)">
        /// </seealso>
        public virtual bool setMultiplier(IAtomContainer container, double multiplier)
        {
            for (int i = 0; i < atomContainers.Length; i++)
            {
                if (atomContainers[i] == container)
                {
                    multipliers[i] = multiplier;
                    notifyChanged();
                    return true;
                }
            }
            return false;
        }

        /// <summary> Sets the coefficient of a AtomContainer to a given value.
        /// 
        /// </summary>
        /// <param name="position">   The position of the AtomContainer for which the multiplier is
        /// set in [0,..]
        /// </param>
        /// <param name="multiplier"> The new multiplier for the AtomContatiner at
        /// <code>position</code>
        /// </param>
        /// <seealso cref="getMultiplier(int)">
        /// </seealso>
        public virtual void setMultiplier(int position, double multiplier)
        {
            multipliers[position] = multiplier;
            notifyChanged();
        }

        /// <summary> Returns an array of double with the stoichiometric coefficients
        /// of the products.
        /// 
        /// </summary>
        /// <returns>    The multipliers for the AtomContainer's in this set
        /// </returns>
        /// <seealso cref="setMultipliers">
        /// </seealso>
        public virtual double[] getMultipliers()
        {
            double[] returnArray = new double[this.atomContainerCount];
            Array.Copy(this.multipliers, 0, returnArray, 0, this.atomContainerCount);
            return returnArray;
        }

        /// <summary> Sets the multipliers of the AtomContainers.
        /// 
        /// </summary>
        /// <param name="newMultipliers"> The new multipliers for the AtomContainers in this set
        /// </param>
        /// <returns>                 true if multipliers have been set.
        /// </returns>
        /// <seealso cref="getMultipliers">
        /// </seealso>
        public virtual bool setMultipliers(double[] newMultipliers)
        {
            if (newMultipliers.Length == atomContainerCount)
            {
                Array.Copy(newMultipliers, 0, multipliers, 0, atomContainerCount);
                notifyChanged();
                return true;
            }

            return false;
        }

        /// <summary> Adds an atomContainer to this container with the given
        /// multiplier.
        /// 
        /// </summary>
        /// <param name="atomContainer"> The atomContainer to be added to this container
        /// </param>
        /// <param name="multiplier">    The multiplier of this atomContainer
        /// </param>
        public virtual void addAtomContainer(IAtomContainer atomContainer, double multiplier)
        {
            if (atomContainerCount + 1 >= atomContainers.Length)
            {
                growAtomContainerArray();
            }
            atomContainer.addListener(this);
            atomContainers[atomContainerCount] = atomContainer;
            multipliers[atomContainerCount] = multiplier;
            atomContainerCount++;
            notifyChanged();
        }

        /// <summary>  Adds all atomContainers in the SetOfAtomContainers to this container.
        /// 
        /// </summary>
        /// <param name="atomContainerSet"> The SetOfAtomContainers
        /// </param>
        public virtual void add(ISetOfAtomContainers atomContainerSet)
        {
            IAtomContainer[] mols = atomContainerSet.AtomContainers;
            for (int i = 0; i < mols.Length; i++)
            {
                addAtomContainer(mols[i]);
            }
            /*
            *  notifyChanged() is called by addAtomContainer()
            */
        }


        /// <summary> Returns the AtomContainer at position <code>number</code> in the
        /// container.
        /// 
        /// </summary>
        /// <param name="number"> The position of the AtomContainer to be returned.
        /// </param>
        /// <returns>         The AtomContainer at position <code>number</code> .
        /// </returns>
        public virtual IAtomContainer getAtomContainer(int number)
        {
            return (AtomContainer)atomContainers[number];
        }

        /// <summary> Returns the multiplier for the AtomContainer at position <code>number</code> in the
        /// container.
        /// 
        /// </summary>
        /// <param name="number"> The position of the multiplier of the AtomContainer to be returned.
        /// </param>
        /// <returns>         The multiplier for the AtomContainer at position <code>number</code> .
        /// </returns>
        /// <seealso cref="setMultiplier(int, double)">
        /// </seealso>
        public virtual double getMultiplier(int number)
        {
            return multipliers[number];
        }

        /// <summary> Returns the multiplier of the given AtomContainer.
        /// 
        /// </summary>
        /// <param name="container"> The AtomContainer for which the multiplier is given
        /// </param>
        /// <returns>            -1, if the given molecule is not a container in this set
        /// </returns>
        /// <seealso cref="setMultiplier(IAtomContainer, double)">
        /// </seealso>
        public virtual double getMultiplier(IAtomContainer container)
        {
            for (int i = 0; i < atomContainerCount; i++)
            {
                if (atomContainers[i].Equals(container))
                {
                    return multipliers[i];
                }
            }
            return -1.0;
        }

        /// <summary>  Grows the atomContainer array by a given size.
        /// 
        /// </summary>
        /// <seealso cref="growArraySize">
        /// </seealso>
        protected internal virtual void growAtomContainerArray()
        {
            growArraySize = atomContainers.Length;
            AtomContainer[] newatomContainers = new AtomContainer[atomContainers.Length + growArraySize];
            Array.Copy(atomContainers, 0, newatomContainers, 0, atomContainers.Length);
            atomContainers = newatomContainers;
            double[] newMultipliers = new double[multipliers.Length + growArraySize];
            Array.Copy(multipliers, 0, newMultipliers, 0, multipliers.Length);
            multipliers = newMultipliers;
        }

        /// <summary> Returns the String representation of this SetOfAtomContainers.
        /// 
        /// </summary>
        /// <returns>    The String representation of this SetOfAtomContainers
        /// </returns>
        public override System.String ToString()
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder(32);
            buffer.Append("SetOfAtomContainers(");
            buffer.Append(this.GetHashCode());
            buffer.Append(", M=").Append(AtomContainerCount).Append(", ");
            IAtomContainer[] atomContainers = AtomContainers;
            for (int i = 0; i < atomContainers.Length; i++)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                buffer.Append(atomContainers[i].ToString());
                if (i < atomContainers.Length - 1)
                {
                    buffer.Append(", ");
                }
            }
            buffer.Append(')');
            return buffer.ToString();
        }


        /// <summary>  Clones this SetOfAtomContainers and its content.
        /// 
        /// </summary>
        /// <returns>    the cloned Object
        /// </returns>
        public override System.Object Clone()
        {
            SetOfAtomContainers clone = (SetOfAtomContainers)base.Clone();
            IAtomContainer[] result = AtomContainers;
            for (int i = 0; i < result.Length; i++)
            {
                clone.addAtomContainer((AtomContainer)((AtomContainer)result[i]).Clone(), 1.0);
            }
            return (System.Object)clone;
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