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
using javax.vecmath;

namespace Org.OpenScience.CDK
{
    /// <summary> Class representing a molecular crystal.
    /// The crystal is described with molecules in fractional
    /// coordinates and three cell axes: a,b and c.
    /// 
    /// <p>The crystal is designed to store only the asymetric atoms.
    /// Though this is not enforced, it is assumed by all methods.
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  crystal </cdk.keyword>
    [Serializable]
    public class Crystal : AtomContainer, ICrystal, ICloneable
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Gets the A unit cell axes in carthesian coordinates
        /// as a three element double array.
        /// 
        /// </summary>
        /// <returns> a Vector3D representing the A axis
        /// 
        /// </returns>
        /// <seealso cref="setA">
        /// </seealso>
        /// <summary> Sets the A unit cell axes in carthesian coordinates in a 
        /// eucledian space.
        /// 
        /// </summary>
        /// <param name="newAxis">the new A axis
        /// 
        /// </param>
        /// <seealso cref="getA">
        /// </seealso>
        virtual public Vector3d A
        {
            get
            {
                return aAxis;
            }

            set
            {
                aAxis = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Gets the B unit cell axes in carthesian coordinates
        /// as a three element double array.
        /// 
        /// </summary>
        /// <returns> a Vector3D representing the B axis
        /// 
        /// </returns>
        /// <seealso cref="setB">
        /// </seealso>
        /// <summary> Sets the B unit cell axes in carthesian coordinates.
        /// 
        /// </summary>
        /// <param name="newAxis">the new B axis
        /// 
        /// </param>
        /// <seealso cref="getB">
        /// </seealso>
        virtual public Vector3d B
        {
            get
            {
                return bAxis;
            }

            set
            {
                bAxis = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Gets the C unit cell axes in carthesian coordinates
        /// as a three element double array.
        /// 
        /// </summary>
        /// <returns> a Vector3D representing the C axis
        /// 
        /// </returns>
        /// <seealso cref="setC">
        /// </seealso>
        /// <summary> Sets the C unit cell axes in carthesian coordinates.
        /// 
        /// </summary>
        /// <param name="newAxis">the new C axis
        /// 
        /// </param>
        /// <seealso cref="getC">
        /// </seealso>
        virtual public Vector3d C
        {
            get
            {
                return cAxis;
            }

            set
            {
                cAxis = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Gets the space group of this crystal.
        /// 
        /// </summary>
        /// <returns> the space group of this crystal structure
        /// 
        /// </returns>
        /// <seealso cref="setSpaceGroup">
        /// </seealso>
        /// <summary> Sets the space group of this crystal.
        /// 
        /// </summary>
        /// <param name="group"> the space group of this crystal structure
        /// 
        /// </param>
        /// <seealso cref="getSpaceGroup">
        /// </seealso>
        virtual public System.String SpaceGroup
        {
            get
            {
                return spaceGroup;
            }

            set
            {
                spaceGroup = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Gets the number of asymmetric parts in the unit cell.
        /// 
        /// </summary>
        /// <returns> the number of assymetric parts in the unit cell
        /// </returns>
        /// <seealso cref="setZ">
        /// </seealso>
        /// <summary> Sets the number of assymmetric parts in the unit cell.
        /// 
        /// </summary>
        /// <param name="value">the number of assymetric parts in the unit cell
        /// </param>
        /// <seealso cref="getZ">
        /// </seealso>
        virtual public int Z
        {
            get
            {
                return zValue;
            }

            set
            {
                this.zValue = value;
                notifyChanged();
            }

        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide/serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = 5919649450390509278L;

        /// <summary>The a axis. </summary>
        private Vector3d aAxis;
        /// <summary>The b axis. </summary>
        private Vector3d bAxis;
        /// <summary>The c axis. </summary>
        private Vector3d cAxis;

        /// <summary> Number of symmetry related atoms.</summary>
        private int zValue = 1;

        /// <summary> Number of symmetry related atoms.</summary>
        private System.String spaceGroup = "P1";

        /// <summary> Constructs a new crystal with zero length cell axis.</summary>
        public Crystal()
            : base()
        {
            setZeroAxes();
        }

        /// <summary> Constructs a new crystal with zero length cell axis
        /// and adds the atoms in the AtomContainer as cell content.
        /// 
        /// </summary>
        /// <param name="container"> the AtomContainer providing the atoms and bonds
        /// </param>
        public Crystal(IAtomContainer container)
            : base(container)
        {
            setZeroAxes();
        }

        /// <summary>  Makes a clone of this crystal.
        /// 
        /// </summary>
        /// <returns> The cloned crystal.
        /// </returns>
        public override System.Object Clone()
        {
            Crystal clone = (Crystal)base.Clone();
            // clone the axes
            clone.A = new Vector3d(this.aAxis);
            clone.B = new Vector3d(this.bAxis);
            clone.C = new Vector3d(this.cAxis);
            return clone;
        }

        /// <summary> Returns a String representation of this crystal.</summary>
        public override System.String ToString()
        {
            System.Text.StringBuilder resultString = new System.Text.StringBuilder(64);
            resultString.Append("Crystal(SG=");
            resultString.Append(SpaceGroup);
            resultString.Append(", Z=").Append(Z);
            resultString.Append(", a=(").Append(aAxis.x).Append(", ").Append(aAxis.y).Append(", ").Append(aAxis.z);
            resultString.Append("), b=(").Append(bAxis.x).Append(", ").Append(bAxis.y).Append(", ").Append(bAxis.z);
            resultString.Append("), c=(").Append(cAxis.x).Append(", ").Append(cAxis.y).Append(", ").Append(cAxis.z);
            resultString.Append("), #A=").Append(AtomCount).Append(')');
            return resultString.ToString();
        }

        /// <summary>  Initializes the unit cell axes to zero length.</summary>
        private void setZeroAxes()
        {
            aAxis = new Vector3d(0.0, 0.0, 0.0);
            bAxis = new Vector3d(0.0, 0.0, 0.0);
            cAxis = new Vector3d(0.0, 0.0, 0.0);
        }
    }
}