/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $
* $Revision: 6672 $
*
* Copyright (C) 2000-2006  The Chemistry Development Kit (CDK) project
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
using javax.vecmath;

namespace Org.OpenScience.CDK
{
    /// <summary> Represents the idea of an chemical atom.
    /// 
    /// <p>An Atom class is instantiated with at least the atom symbol:
    /// <pre>
    /// Atom a = new Atom("C");
    /// </pre>
    /// 
    /// <p>Once instantiated all field not filled by passing parameters
    /// to the constructured are null. Atoms can be configured by using
    /// the IsotopeFactory.configure() method:
    /// <pre>
    /// IsotopeFactory if = IsotopeFactory.getInstance(a.getBuilder());
    /// if.configure(a);
    /// </pre>
    /// 
    /// <p>More examples about using this class can be found in the
    /// Junit test for this class.
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>      steinbeck
    /// </author>
    /// <cdk.created>     2000-10-02 </cdk.created>
    /// <cdk.keyword>     atom </cdk.keyword>
    /// <summary> 
    /// </summary>
    /// <seealso cref="org.openscience.cdk.config.IsotopeFactory.getInstance(IChemObjectBuilder)">
    /// </seealso>
    [Serializable]
    public class Atom : AtomType, IAtom, System.ICloneable
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the x coordinate for of the 2D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <returns> the x coordinate for of the 2D location of this atom
        /// 
        /// </returns>
        /// <seealso cref="setX2d">
        /// </seealso>
        /// <summary> Sets the x coordinate for of the 2D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <param name="xCoord"> the new x coordinate for of the 2D location of this atom
        /// 
        /// </param>
        /// <seealso cref="getX2d">
        /// </seealso>
        virtual public double X2d
        {
            get
            {
                if (point2d == null)
                {
                    return 0.0;
                }
                else
                {
                    return point2d.x;
                }
            }

            set
            {
                if (point2d == null)
                {
                    point2d = new javax.vecmath.Point2d();
                }
                point2d.x = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the y coordinate for of the 2D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <returns> the y coordinate for of the 2D location of this atom
        /// 
        /// </returns>
        /// <seealso cref="setY2d">
        /// </seealso>
        /// <summary> Sets the y coordinate for of the 2D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <param name="yCoord"> the new y coordinate for of the 2D location of this atom
        /// 
        /// </param>
        /// <seealso cref="getY2d">
        /// </seealso>
        virtual public double Y2d
        {
            get
            {
                if (point2d == null)
                {
                    return 0.0;
                }
                else
                {
                    return point2d.y;
                }
            }

            set
            {
                if (point2d == null)
                {
                    point2d = new javax.vecmath.Point2d();
                }
                point2d.y = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the x coordinate for of the 3D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <returns> the x coordinate for of the 3D location of this atom
        /// 
        /// </returns>
        /// <seealso cref="setX3d">
        /// </seealso>
        /// <summary> Sets the x coordinate for of the 3D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <param name="xCoord"> the new x coordinate for of the 3D location of this atom
        /// 
        /// </param>
        /// <seealso cref="getX3d">
        /// </seealso>
        virtual public double X3d
        {
            get
            {
                if (point3d == null)
                {
                    return 0.0;
                }
                else
                {
                    return point3d.x;
                }
            }

            set
            {
                if (point3d == null)
                {
                    point3d = new javax.vecmath.Point3d();
                }
                point3d.x = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the y coordinate for of the 3D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <returns> the y coordinate for of the 3D location of this atom
        /// 
        /// </returns>
        /// <seealso cref="setY3d">
        /// </seealso>
        /// <summary> Sets the y coordinate for of the 3D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <param name="yCoord"> the new y coordinate for of the 3D location of this atom
        /// 
        /// </param>
        /// <seealso cref="getY3d">
        /// </seealso>
        virtual public double Y3d
        {
            get
            {
                if (point3d == null)
                {
                    return 0.0;
                }
                else
                {
                    return point3d.y;
                }
            }

            set
            {
                if (point3d == null)
                {
                    point3d = new javax.vecmath.Point3d();
                }
                point3d.y = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the z coordinate for of the 3D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <returns> the z coordinate for of the 3D location of this atom
        /// 
        /// </returns>
        /// <seealso cref="setZ3d">
        /// </seealso>
        /// <summary> Sets the z coordinate for of the 3D location of this atom.
        /// You should know your context here. There is no guarantee that point2d and point3d
        /// contain consistent information. Both are handled independently.
        /// 
        /// </summary>
        /// <param name="zCoord"> the new z coordinate for of the 3d location of this atom
        /// 
        /// </param>
        /// <seealso cref="getZ3d">
        /// </seealso>
        virtual public double Z3d
        {
            get
            {
                if (point3d == null)
                {
                    return 0.0;
                }
                else
                {
                    return point3d.z;
                }
            }

            set
            {
                if (point3d == null)
                {
                    point3d = new javax.vecmath.Point3d();
                }
                point3d.z = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the x coordinate for of the fractional coordinate of this atom.
        /// 
        /// </summary>
        /// <returns> the x coordinate for of the fractional coordinate of this atom.
        /// 
        /// </returns>
        /// <seealso cref="setFractX3d">
        /// </seealso>
        /// <summary> Sets the x coordinate of the fractional coordinate of this atom.
        /// 
        /// </summary>
        /// <param name="xFract">The x coordinate of the fractional coordinate of this atom.
        /// 
        /// </param>
        /// <seealso cref="getFractX3d">
        /// </seealso>
        virtual public double FractX3d
        {
            get
            {
                if (fractionalPoint3d == null)
                {
                    return 0.0;
                }
                else
                {
                    return fractionalPoint3d.x;
                }
            }

            set
            {
                if (fractionalPoint3d == null)
                {
                    fractionalPoint3d = new Point3d();
                }
                fractionalPoint3d.x = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the y coordinate for of the fractional coordinate of this atom.
        /// 
        /// </summary>
        /// <returns> the y coordinate for of the fractional coordinate of this atom.
        /// 
        /// </returns>
        /// <seealso cref="setFractY3d">
        /// </seealso>
        /// <summary> Sets the y coordinate of the fractional coordinate of this atom.
        /// 
        /// </summary>
        /// <param name="yFract">The y coordinate of the fractional coordinate of this atom.
        /// 
        /// </param>
        /// <seealso cref="getFractY3d">
        /// </seealso>
        virtual public double FractY3d
        {
            get
            {
                if (fractionalPoint3d == null)
                {
                    return 0.0;
                }
                else
                {
                    return fractionalPoint3d.y;
                }
            }

            set
            {
                if (fractionalPoint3d == null)
                {
                    fractionalPoint3d = new Point3d();
                }
                fractionalPoint3d.y = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the z coordinate for of the fractional coordinate of this atom.
        /// 
        /// </summary>
        /// <returns> the z coordinate for of the fractional coordinate of this atom.
        /// 
        /// </returns>
        /// <seealso cref="setFractZ3d">
        /// </seealso>
        /// <summary> Sets the z coordinate of the fractional coordinate of this atom.
        /// 
        /// </summary>
        /// <param name="zFract">The z coordinate of the fractional coordinate of this atom.
        /// 
        /// </param>
        /// <seealso cref="getFractZ3d">
        /// </seealso>
        virtual public double FractZ3d
        {
            get
            {
                if (fractionalPoint3d == null)
                {
                    return 0.0;
                }
                else
                {
                    return fractionalPoint3d.z;
                }
            }

            set
            {
                if (fractionalPoint3d == null)
                {
                    fractionalPoint3d = new Point3d();
                }
                fractionalPoint3d.z = value;
                notifyChanged();
            }

        }

        /* Let's keep this exact specification
        * of what kind of point2d we're talking of here,
        * sinces there are so many around in the java standard api */

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = -3137373012494608794L;

        /// <summary>  A 2D point specifying the location of this atom in a 2D coordinate 
        /// space.
        /// </summary>
        protected internal javax.vecmath.Point2d point2d;
        /// <summary>  A 3 point specifying the location of this atom in a 3D coordinate 
        /// space.
        /// </summary>
        protected internal javax.vecmath.Point3d point3d;
        /// <summary>  A 3 point specifying the location of this atom in a crystal unit cell.</summary>
        protected internal javax.vecmath.Point3d fractionalPoint3d;
        /// <summary>  The number of implicitly bound hydrogen atoms for this atom.</summary>
        protected internal int hydrogenCount;
        /// <summary>  A stereo parity descriptor for the stereochemistry of this atom.</summary>
        protected internal int stereoParity;
        /// <summary>  The partial charge of the atom.</summary>
        protected internal double charge;

        /// <summary> Constructs an completely unset Atom.</summary>
        public Atom()
            : this(null)
        {
        }

        /// <summary> Constructs an Atom from a String containing an element symbol.
        /// 
        /// </summary>
        /// <param name="elementSymbol"> The String describing the element for the Atom
        /// </param>
        public Atom(System.String elementSymbol)
            : base(elementSymbol)
        {
            this.fractionalPoint3d = null;
            this.point3d = null;
            this.point2d = null;
        }

        /// <summary> Constructs an Atom from an Element and a Point3d.
        /// 
        /// </summary>
        /// <param name="elementSymbol">  The symbol of the atom
        /// </param>
        /// <param name="point3d">        The 3D coordinates of the atom
        /// </param>
        public Atom(System.String elementSymbol, javax.vecmath.Point3d point3d)
            : this(elementSymbol)
        {
            this.point3d = point3d;
        }

        /// <summary> Constructs an Atom from an Element and a Point2d.
        /// 
        /// </summary>
        /// <param name="elementSymbol">  The Element
        /// </param>
        /// <param name="point2d">        The Point
        /// </param>
        public Atom(System.String elementSymbol, Point2d point2d)
            : this(elementSymbol)
        {
            this.point2d = point2d;
        }

        /// <summary>  Sets the partial charge of this atom.
        /// 
        /// </summary>
        /// <param name="charge"> The partial charge
        /// 
        /// </param>
        /// <seealso cref="getCharge">
        /// </seealso>
        public virtual void setCharge(double charge)
        {
            this.charge = charge;
            notifyChanged();
        }

        /// <summary>  Returns the partial charge of this atom.
        /// 
        /// </summary>
        /// <returns> the charge of this atom
        /// 
        /// </returns>
        /// <seealso cref="setCharge">
        /// </seealso>
        public virtual double getCharge()
        {
            return this.charge;
        }

        /// <summary>  Sets the hydrogen count of this atom.
        /// 
        /// </summary>
        /// <param name="hydrogenCount"> The number of hydrogen atoms bonded to this atom.
        /// 
        /// </param>
        /// <seealso cref="getHydrogenCount">
        /// </seealso>
        public virtual void setHydrogenCount(int hydrogenCount)
        {
            this.hydrogenCount = hydrogenCount;
            notifyChanged();
        }

        /// <summary>  Returns the hydrogen count of this atom.
        /// 
        /// </summary>
        /// <returns>    The hydrogen count of this atom.
        /// 
        /// </returns>
        /// <seealso cref="setHydrogenCount">
        /// </seealso>
        public virtual int getHydrogenCount()
        {
            return this.hydrogenCount;
        }

        /// <summary> 
        /// Sets a point specifying the location of this
        /// atom in a 2D space.
        /// 
        /// </summary>
        /// <param name="point2d"> A point in a 2D plane
        /// 
        /// </param>
        /// <seealso cref="getPoint2d">
        /// </seealso>
        public virtual void setPoint2d(javax.vecmath.Point2d point2d)
        {
            this.point2d = point2d;
            notifyChanged();
        }
        /// <summary> 
        /// Sets a point specifying the location of this
        /// atom in 3D space.
        /// 
        /// </summary>
        /// <param name="point3d"> A point in a 3-dimensional space
        /// 
        /// </param>
        /// <seealso cref="getPoint3d">
        /// </seealso>
        public virtual void setPoint3d(javax.vecmath.Point3d point3d)
        {
            this.point3d = point3d;
            notifyChanged();
        }
        /// <summary> Sets a point specifying the location of this
        /// atom in a Crystal unit cell.
        /// 
        /// </summary>
        /// <param name="point3d"> A point in a 3d fractional unit cell space
        /// 
        /// </param>
        /// <seealso cref="getFractionalPoint3d">
        /// </seealso>
        /// <seealso cref="org.openscience.cdk.Crystal">
        /// </seealso>
        public virtual void setFractionalPoint3d(javax.vecmath.Point3d point3d)
        {
            this.fractionalPoint3d = point3d;
            notifyChanged();
        }
        /// <summary> Sets the stereo parity for this atom.
        /// 
        /// </summary>
        /// <param name="stereoParity"> The stereo parity for this atom
        /// 
        /// </param>
        /// <seealso cref="org.openscience.cdk.CDKConstants for predefined values.">
        /// </seealso>
        /// <seealso cref="getStereoParity">
        /// </seealso>
        public virtual void setStereoParity(int stereoParity)
        {
            this.stereoParity = stereoParity;
            notifyChanged();
        }

        /// <summary> Returns a point specifying the location of this
        /// atom in a 2D space.
        /// 
        /// </summary>
        /// <returns>    A point in a 2D plane. Null if unset.
        /// 
        /// </returns>
        /// <seealso cref="setPoint2d">
        /// </seealso>
        public virtual javax.vecmath.Point2d getPoint2d()
        {
            return this.point2d;
        }
        /// <summary> Returns a point specifying the location of this
        /// atom in a 3D space.
        /// 
        /// </summary>
        /// <returns>    A point in 3-dimensional space. Null if unset.
        /// 
        /// </returns>
        /// <seealso cref="setPoint3d">
        /// </seealso>
        public virtual javax.vecmath.Point3d getPoint3d()
        {
            return this.point3d;
        }

        /// <summary> Returns a point specifying the location of this
        /// atom in a Crystal unit cell.
        /// 
        /// </summary>
        /// <returns>    A point in 3d fractional unit cell space. Null if unset.
        /// 
        /// </returns>
        /// <seealso cref="setFractionalPoint3d">
        /// </seealso>
        /// <seealso cref="org.openscience.cdk.CDKConstants for predefined values.">
        /// </seealso>
        public virtual javax.vecmath.Point3d getFractionalPoint3d()
        {
            return this.fractionalPoint3d;
        }


        /// <summary>  Returns the stereo parity of this atom. It uses the predefined values
        /// found in CDKConstants.
        /// 
        /// </summary>
        /// <returns>    The stereo parity for this atom
        /// 
        /// </returns>
        /// <seealso cref="org.openscience.cdk.CDKConstants">
        /// </seealso>
        /// <seealso cref="setStereoParity">
        /// </seealso>
        public virtual int getStereoParity()
        {
            return this.stereoParity;
        }

        /// <summary> Compares a atom with this atom.
        /// 
        /// </summary>
        /// <param name="object">of type Atom
        /// </param>
        /// <returns>    true, if the atoms are equal
        /// </returns>
        public override bool compare(System.Object object_Renamed)
        {
            if (!(object_Renamed is IAtom))
            {
                return false;
            }
            if (!base.compare(object_Renamed))
            {
                return false;
            }
            Atom atom = (Atom)object_Renamed;
            if (((point2d == atom.point2d) || ((point2d != null) && (point2d.equals(atom.point2d)))) && ((point3d == atom.point3d) || ((point3d != null) && (point3d.equals(atom.point3d)))) && (hydrogenCount == atom.hydrogenCount) && (stereoParity == atom.stereoParity) && (charge == atom.charge))
            {
                return true;
            }
            return false;
        }

        /// <summary> Returns a one line string representation of this Atom.
        /// Methods is conform RFC #9.
        /// 
        /// </summary>
        /// <returns>  The string representation of this Atom
        /// </returns>
        public override System.String ToString()
        {
            System.Text.StringBuilder stringContent = new System.Text.StringBuilder(64);
            stringContent.Append("Atom(");
            stringContent.Append(this.GetHashCode()).Append(", ");
            stringContent.Append(Symbol);
            stringContent.Append(", H:").Append(getHydrogenCount());
            stringContent.Append(", SP:").Append(getStereoParity());
            stringContent.Append(", 2D:[").Append(getPoint2d());
            stringContent.Append("], 3D:[").Append(getPoint3d());
            stringContent.Append("], Fract3D:[").Append(getFractionalPoint3d());
            stringContent.Append("], C:").Append(getCharge());
            stringContent.Append(", FC:").Append(getFormalCharge());
            stringContent.Append(", ").Append(base.ToString());
            stringContent.Append(')');
            return stringContent.ToString();
        }


        /// <summary> Clones this atom object and its content.
        /// 
        /// </summary>
        /// <returns>  The cloned object   
        /// </returns>
        public override System.Object Clone()
        {
            System.Object clone = base.Clone();
            if (point2d != null)
            {
                ((Atom)clone).setPoint2d(new Point2d(point2d.x, point2d.y));
            }
            if (point3d != null)
            {
                ((Atom)clone).setPoint3d(new Point3d(point3d.x, point3d.y, point3d.z));
            }
            if (fractionalPoint3d != null)
            {
                ((Atom)clone).setFractionalPoint3d(new Point3d(fractionalPoint3d.x, fractionalPoint3d.y, fractionalPoint3d.z));
            }
            return clone;
        }
    }
}