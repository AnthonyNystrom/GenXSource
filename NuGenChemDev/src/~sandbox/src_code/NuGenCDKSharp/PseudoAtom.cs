/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $
* $Revision: 6672 $
*
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
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
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK
{
    /// <summary> Represents the idea of a non-chemical atom-like entity, like Me,
    /// R, X, Phe, His, etc.
    /// 
    /// <p>This should be replaced by the mechanism explained in RFC #8.
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <seealso cref="Atom">
    /// </seealso>
    [Serializable]
    public class PseudoAtom : Atom, ICloneable, IPseudoAtom
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the label of this PseudoAtom.
        /// 
        /// </summary>
        /// <returns> The label for this PseudoAtom
        /// </returns>
        /// <seealso cref="setLabel">
        /// </seealso>
        /// <summary> Sets the label of this PseudoAtom.
        /// 
        /// </summary>
        /// <param name="label">The new label for this PseudoAtom
        /// </param>
        /// <seealso cref="getLabel">
        /// </seealso>
        virtual public System.String Label
        {
            get
            {
                return label;
            }

            set
            {
                this.label = value;
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
        private const long serialVersionUID = 1L;

        private System.String label;

        /// <summary> Constructs an empty PseudoAtom.</summary>
        public PseudoAtom()
            : this("")
        {
        }

        /// <summary> Constructs an Atom from a String containing an element symbol.
        /// 
        /// </summary>
        /// <param name="label"> The String describing the PseudoAtom
        /// </param>
        public PseudoAtom(System.String label)
            : base("R")
        {
            this.label = label;
            base.fractionalPoint3d = null;
            base.point3d = null;
            base.point2d = null;
            // set these default, unchangable values
            base.hydrogenCount = 0;
            base.stereoParity = 0;
            base.exactMass = 0.0;
            base.formalCharge = 0;
            base.charge = 0.0;
        }

        /// <summary> Constructs an Atom from a String containing an element symbol.
        /// 
        /// </summary>
        /// <param name="atom"> Atom from which the PseudoAtom is constructed
        /// </param>
        public PseudoAtom(IAtom atom)
            : base("R")
        {
            base.fractionalPoint3d = atom.getFractionalPoint3d();
            base.point3d = atom.getPoint3d();
            base.point2d = atom.getPoint2d();
            this.label = atom.Symbol;
            base.hydrogenCount = 0;
            base.stereoParity = 0;
            base.exactMass = 0.0;
            base.formalCharge = 0;
            base.charge = 0.0;
        }

        /// <summary> Constructs an Atom from an Element and a Point3d.
        /// 
        /// </summary>
        /// <param name="label"> The String describing the PseudoAtom
        /// </param>
        /// <param name="point3d">        The 3D coordinates of the atom
        /// </param>
        public PseudoAtom(System.String label, javax.vecmath.Point3d point3d)
            : this(label)
        {
            this.point3d = point3d;
        }

        /// <summary> Constructs an Atom from an Element and a Point2d.
        /// 
        /// </summary>
        /// <param name="label"> The String describing the PseudoAtom
        /// </param>
        /// <param name="point2d">        The Point
        /// </param>
        public PseudoAtom(System.String label, javax.vecmath.Point2d point2d)
            : this(label)
        {
            this.point2d = point2d;
        }

        /// <summary> Dummy method: the exact mass is 0, final. </summary>
        public override void setExactMass(double mass)
        {
            // exact mass = 0, always
        }

        /// <summary> Dummy method: the hydrogen count is 0, final. </summary>
        public override void setHydrogenCount(int hydrogenCount)
        {
            // hydrogen count = 0, always
        }

        /// <summary> Dummy method: the formal charge is 0, final. </summary>
        public override void setFormalCharge(int charge)
        {
            // formal charge = 0, always
        }

        /// <summary> Dummy method: the partial charge is 0, final. </summary>
        public override void setCharge(double charge)
        {
            // partial charge = 0, always
        }
        /// <summary> Dummy method: the stereo parity is undefined, final.</summary>
        public override void setStereoParity(int stereoParity)
        {
            // this is undefined, always
        }

        /// <summary> Returns a one line string representation of this Atom.
        /// Methods is conform RFC #9.
        /// 
        /// </summary>
        /// <returns>  The string representation of this Atom
        /// </returns>
        public override System.String ToString()
        {
            System.Text.StringBuilder description = new System.Text.StringBuilder();
            description.Append("PseudoAtom(");
            description.Append(this.GetHashCode()).Append(", ");
            description.Append(Label).Append(", ");
            description.Append(base.ToString());
            description.Append(')');
            return description.ToString();
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
        virtual public System.Object Clone()
        {
            return null;
        }
    }
}