/* $RCSfile$
* $Author: egonw $    
* $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $    
* $Revision: 6672 $
*
*  Copyright (C) 2001-2006  The Chemistry Development Kit (CDK) project
*
*  Contact: cdk-devel@lists.sourceforge.net
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public License
*  as published by the Free Software Foundation; either version 2.1
*  of the License, or (at your option) any later version.
*  All we ask is that proper credit is given for our work, which includes
*  - but is not limited to - adding the above copyright notice to the beginning
*  of your source code files, and to any copyright notice that you may distribute
*  with programs based on this work.
*
*  This program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public License
*  along with this program; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*
*/
using System;
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK
{
    /// <summary> The base class for atom types. Atom types are typically used to describe the
    /// behaviour of an atom of a particular element in different environment like 
    /// sp<sup>3</sup>
    /// hybridized carbon C3, etc., in some molecular modelling applications.
    /// 
    /// </summary>
    /// <author>        steinbeck
    /// </author>
    /// <cdk.created>   2001-08-08 </cdk.created>
    /// <cdk.module>    data </cdk.module>
    /// <cdk.keyword>   atom, type </cdk.keyword>
    [Serializable]
    public class AtomType : Isotope, IAtomType, ICloneable
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary>  Gets the id attribute of the AtomType object.
        /// 
        /// </summary>
        /// <returns>    The id value
        /// 
        /// </returns>
        /// <seealso cref="setAtomTypeName">
        /// </seealso>
        /// <summary>  Sets the if attribute of the AtomType object.
        /// 
        /// </summary>
        /// <param name="identifier"> The new AtomTypeID value. Null if unset.
        /// 
        /// </param>
        /// <seealso cref="getAtomTypeName">
        /// </seealso>
        virtual public System.String AtomTypeName
        {
            get
            {
                return this.identifier;
            }

            set
            {
                this.identifier = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary>  Gets the MaxBondOrder attribute of the AtomType object.
        /// 
        /// </summary>
        /// <returns>    The MaxBondOrder value
        /// 
        /// </returns>
        /// <seealso cref="setMaxBondOrder">
        /// </seealso>
        /// <summary>  Sets the MaxBondOrder attribute of the AtomType object.
        /// 
        /// </summary>
        /// <param name="maxBondOrder"> The new MaxBondOrder value
        /// 
        /// </param>
        /// <seealso cref="getMaxBondOrder">
        /// </seealso>
        virtual public double MaxBondOrder
        {
            get
            {
                return maxBondOrder;
            }

            set
            {
                this.maxBondOrder = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary>  Gets the bondOrderSum attribute of the AtomType object.
        /// 
        /// </summary>
        /// <returns>    The bondOrderSum value
        /// 
        /// </returns>
        /// <seealso cref="setBondOrderSum">
        /// </seealso>
        /// <summary>  Sets the the exact bond order sum attribute of the AtomType object.
        /// 
        /// </summary>
        /// <param name="bondOrderSum"> The new bondOrderSum value
        /// 
        /// </param>
        /// <seealso cref="getBondOrderSum">
        /// </seealso>
        virtual public double BondOrderSum
        {
            get
            {
                return bondOrderSum;
            }

            set
            {
                this.bondOrderSum = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the formal neighbour count of this atom.
        /// 
        /// </summary>
        /// <returns> the formal neighbour count of this atom
        /// 
        /// </returns>
        /// <seealso cref="setFormalNeighbourCount">
        /// </seealso>
        /// <summary> Sets the formal neighbour count of this atom.
        /// 
        /// </summary>
        /// <param name="count"> The neighbour count
        /// 
        /// </param>
        /// <seealso cref="getFormalNeighbourCount">
        /// </seealso>
        virtual public int FormalNeighbourCount
        {
            get
            {
                return this.formalNeighbourCount;
            }

            set
            {
                this.formalNeighbourCount = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary>  Returns the hybridization of this atom.
        /// 
        /// </summary>
        /// <returns> the hybridization of this atom
        /// 
        /// </returns>
        /// <seealso cref="setHybridization">
        /// </seealso>
        /// <summary>  Sets the hybridization of this atom.
        /// 
        /// </summary>
        /// <param name="hybridization"> The hybridization
        /// 
        /// </param>
        /// <seealso cref="getHybridization">
        /// </seealso>
        virtual public int Hybridization
        {
            get
            {
                return this.hybridization;
            }

            set
            {
                this.hybridization = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the Vanderwaals radius for this AtomType.
        /// 
        /// </summary>
        /// <returns> The Vanderwaals radius for this AtomType
        /// </returns>
        /// <seealso cref="setVanderwaalsRadius">
        /// </seealso>
        /// <summary> Sets the Vanderwaals radius for this AtomType.
        /// 
        /// </summary>
        /// <param name="radius">The Vanderwaals radius for this AtomType
        /// </param>
        /// <seealso cref="getVanderwaalsRadius">
        /// </seealso>
        virtual public double VanderwaalsRadius
        {
            get
            {
                return this.vanderwaalsRadius;
            }

            set
            {
                this.vanderwaalsRadius = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the covalent radius for this AtomType.
        /// 
        /// </summary>
        /// <returns> The covalent radius for this AtomType
        /// </returns>
        /// <seealso cref="setCovalentRadius">
        /// </seealso>
        /// <summary> Sets the covalent radius for this AtomType.
        /// 
        /// </summary>
        /// <param name="radius">The covalent radius for this AtomType
        /// </param>
        /// <seealso cref="getCovalentRadius">
        /// </seealso>
        virtual public double CovalentRadius
        {
            get
            {
                return this.covalentRadius;
            }

            set
            {
                this.covalentRadius = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary>  Gets the the exact electron valency of the AtomType object.
        /// 
        /// </summary>
        /// <returns> The valency value
        /// </returns>
        /// <seealso cref="setValency">
        /// 
        /// </seealso>
        /// <summary>  Sets the the exact electron valency of the AtomType object.
        /// 
        /// </summary>
        /// <param name="valency"> The new valency value
        /// </param>
        /// <seealso cref="getValency">
        /// 
        /// </seealso>
        virtual public int Valency
        {
            get
            {
                return this.electronValency;
            }

            set
            {
                this.electronValency = value;
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
        private const long serialVersionUID = -7950397716808229972L;

        /// <summary>  The maximum bond order allowed for this atom type.</summary>
        internal double maxBondOrder;
        /// <summary>  The maximum sum of all bondorders allowed for this atom type.</summary>
        internal double bondOrderSum;

        /// <summary> The Vanderwaals radius of this atom type.</summary>
        internal double vanderwaalsRadius;
        /// <summary> The covalent radius of this atom type.</summary>
        internal double covalentRadius;

        /// <summary>  The formal charge of the atom with 0 as default. Implements RFC #6.</summary>
        protected internal int formalCharge;

        /// <summary> The hybridization state of this atom with CDKConstants.HYBRIDIZATION_UNSET
        /// as default.
        /// </summary>
        protected internal int hybridization;

        /// <summary>  The electron Valency of this atom with CDKConstants.UNSET as default.</summary>
        protected internal int electronValency;

        /// <summary> The formal number of neighbours this atom type can have with CDKConstants_UNSET
        /// as default. This includes explicitely and implicitely connected atoms, including
        /// implicit hydrogens.
        /// </summary>
        protected internal int formalNeighbourCount;

        /// <summary> String representing the identifier for this atom type with null as default.</summary>
        private System.String identifier;

        /// <summary>  Constructor for the AtomType object.
        /// 
        /// </summary>
        /// <param name="elementSymbol"> Symbol of the atom
        /// </param>
        public AtomType(System.String elementSymbol)
            : base(elementSymbol)
        {
            this.identifier = null;
            this.formalNeighbourCount = CDKConstants.UNSET;
            this.electronValency = CDKConstants.UNSET;
            this.hybridization = CDKConstants.HYBRIDIZATION_UNSET;
            this.formalCharge = 0;
        }


        /// <summary>  Constructor for the AtomType object.
        /// 
        /// </summary>
        /// <param name="identifier">    An id for this atom type, like C3 for sp3 carbon
        /// </param>
        /// <param name="elementSymbol"> The element symbol identifying the element to which this atom type applies
        /// </param>
        public AtomType(System.String identifier, System.String elementSymbol)
            : this(elementSymbol)
        {
            this.identifier = identifier;
        }

        /// <summary>  Sets the formal charge of this atom.
        /// 
        /// </summary>
        /// <param name="charge"> The formal charge
        /// 
        /// </param>
        /// <seealso cref="getFormalCharge">
        /// </seealso>
        public virtual void setFormalCharge(int charge)
        {
            this.formalCharge = charge;
            notifyChanged();
        }

        /// <summary>  Returns the formal charge of this atom.
        /// 
        /// </summary>
        /// <returns> the formal charge of this atom
        /// 
        /// </returns>
        /// <seealso cref="setFormalCharge">
        /// </seealso>
        public virtual int getFormalCharge()
        {
            return this.formalCharge;
        }

        /// <summary> Compare a atom type with this atom type.
        /// 
        /// </summary>
        /// <param name="object">Object of type AtomType
        /// </param>
        /// <returns>        Return true, if the atomtypes are equal
        /// </returns>
        public override bool compare(System.Object object_Renamed)
        {
            if (!(object_Renamed is IAtomType))
            {
                return false;
            }
            if (!base.compare(object_Renamed))
            {
                return false;
            }
            AtomType type = (AtomType)object_Renamed;
            if (((System.Object)AtomTypeName == (System.Object)type.AtomTypeName) && (maxBondOrder == type.maxBondOrder) && (bondOrderSum == type.bondOrderSum))
            {
                return true;
            }
            return false;
        }

        public override System.String ToString()
        {
            System.Text.StringBuilder resultString = new System.Text.StringBuilder(64);
            resultString.Append("AtomType(");
            resultString.Append(AtomTypeName);
            resultString.Append(", MBO:").Append(MaxBondOrder);
            resultString.Append(", BOS:").Append(BondOrderSum);
            resultString.Append(", FC:").Append(getFormalCharge());
            resultString.Append(", H:").Append(Hybridization);
            resultString.Append(", NC:").Append(FormalNeighbourCount);
            resultString.Append(", CR:").Append(CovalentRadius);
            resultString.Append(", VDWR:").Append(VanderwaalsRadius);
            resultString.Append(", EV:").Append(Valency).Append(", ");
            resultString.Append(base.ToString());
            resultString.Append(')');
            return resultString.ToString();
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
        //virtual public System.Object Clone()
        //{
        //    return null;
        //}
    }
}