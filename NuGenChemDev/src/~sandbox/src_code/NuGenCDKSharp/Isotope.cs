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
* All I ask is that proper credit is given for my work, which includes
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
    /// <summary> Used to store and retrieve data of a particular isotope.
    /// For example, an carbon 13 isotope can be created with:
    /// <pre>
    /// Isotope carbon = new Isotope("C", 13);
    /// </pre>
    /// 
    /// <p>A full specification can be constructed with:
    /// <pre>
    /// // make deuterium
    /// Isotope carbon = new Isotope(1, "H", 2, 2.01410179, 100.0);
    /// </pre>
    /// 
    /// <p>Once instantiated all field not filled by passing parameters
    /// to the constructured are null. Isotopes can be configured by using
    /// the IsotopeFactory.configure() method:
    /// <pre>
    /// Isotope isotope = new Isotope("C", 13);
    /// IsotopeFactory if = IsotopeFactory.getInstance(isotope.getBuilder());
    /// if.configure(isotope);
    /// </pre>
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>      steinbeck
    /// </author>
    /// <cdk.created>     2001-08-21 </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>      isotope </cdk.keyword>
    [Serializable]
    public class Isotope : Element, IIsotope, ICloneable
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary>  Gets the NaturalAbundance attribute of the Isotope object.
        /// 
        /// <p>Once instantiated all field not filled by passing parameters
        /// to the constructured are null. Isotopes can be configured by using
        /// the IsotopeFactory.configure() method:
        /// <pre>
        /// Isotope isotope = new Isotope("C", 13);
        /// IsotopeFactory if = IsotopeFactory.getInstance(isotope.getBuilder());
        /// if.configure(isotope);
        /// </pre>
        /// </p>
        /// 
        /// </summary>
        /// <returns>    The NaturalAbundance value
        /// 
        /// </returns>
        /// <seealso cref="setNaturalAbundance">
        /// </seealso>
        /// <summary>  Sets the NaturalAbundance attribute of the Isotope object.
        /// 
        /// </summary>
        /// <param name="naturalAbundance"> The new NaturalAbundance value
        /// 
        /// </param>
        /// <seealso cref="getNaturalAbundance">
        /// </seealso>
        virtual public double NaturalAbundance
        {
            get
            {
                return this.naturalAbundance;
            }

            set
            {
                this.naturalAbundance = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the atomic mass of this element.
        /// 
        /// <p>Once instantiated all field not filled by passing parameters
        /// to the constructured are null. Isotopes can be configured by using
        /// the IsotopeFactory.configure() method:
        /// <pre>
        /// Isotope isotope = new Isotope("C", 13);
        /// IsotopeFactory if = IsotopeFactory.getInstance(isotope.getBuilder());
        /// if.configure(isotope);
        /// </pre>
        /// </p>
        /// 
        /// </summary>
        /// <returns> The atomic mass of this element
        /// 
        /// </returns>
        /// <seealso cref="setMassNumber(int)">
        /// </seealso>
        /// <summary> Sets the atomic mass of this element.
        /// 
        /// </summary>
        /// <param name="massNumber">The atomic mass to be assigned to this element
        /// 
        /// </param>
        /// <seealso cref="getMassNumber">
        /// </seealso>
        virtual public int MassNumber
        {
            get
            {

                return this.massNumber;
            }

            set
            {
                this.massNumber = value;
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
        private const long serialVersionUID = 6389365978927575858L;

        /// <summary>Exact mass of this isotope. </summary>
        public double exactMass = (double)(-1);
        /// <summary>Natural abundance of this isotope. </summary>
        public double naturalAbundance = (double)(-1);
        /// <summary>The mass number for this isotope. </summary>
        private int massNumber = 0;


        /// <summary>  Constructor for the Isotope object.
        /// 
        /// </summary>
        /// <param name="elementSymbol"> The element symbol, "O" for Oxygen, etc.
        /// </param>
        public Isotope(System.String elementSymbol)
            : base(elementSymbol)
        {
        }

        /// <summary>  Constructor for the Isotope object.
        /// 
        /// </summary>
        /// <param name="atomicNumber">  The atomic number of the isotope
        /// </param>
        /// <param name="elementSymbol"> The element symbol, "O" for Oxygen, etc.
        /// </param>
        /// <param name="massNumber">    The atomic mass of the isotope, 16 for Oxygen, e.g.
        /// </param>
        /// <param name="exactMass">     The exact mass of the isotope, be a little more explicit here :-)
        /// </param>
        /// <param name="abundance">     The natural abundance of the isotope
        /// </param>
        public Isotope(int atomicNumber, System.String elementSymbol, int massNumber, double exactMass, double abundance)
            : this(atomicNumber, elementSymbol, exactMass, abundance)
        {
            this.massNumber = massNumber;
        }


        /// <summary>  Constructor for the Isotope object.
        /// 
        /// </summary>
        /// <param name="atomicNumber">  The atomic number of the isotope, 8 for Oxygen
        /// </param>
        /// <param name="elementSymbol"> The element symbol, "O" for Oxygen, etc.
        /// </param>
        /// <param name="exactMass">     The exact mass of the isotope, be a little more explicit here :-)
        /// </param>
        /// <param name="abundance">     The natural abundance of the isotope
        /// </param>
        public Isotope(int atomicNumber, System.String elementSymbol, double exactMass, double abundance)
            : base(elementSymbol, atomicNumber)
        {
            this.exactMass = exactMass;
            this.naturalAbundance = abundance;
        }

        /// <summary> Constructor for the Isotope object.
        /// 
        /// </summary>
        /// <param name="elementSymbol"> The element symbol, "O" for Oxygen, etc.
        /// </param>
        /// <param name="massNumber">    The atomic mass of the isotope, 16 for Oxygen, e.g.
        /// </param>
        public Isotope(System.String elementSymbol, int massNumber)
            : base(elementSymbol)
        {
            this.massNumber = massNumber;
        }


        /// <summary>  Sets the ExactMass attribute of the Isotope object.
        /// 
        /// </summary>
        /// <param name="exactMass"> The new ExactMass value
        /// 
        /// </param>
        /// <seealso cref="getExactMass">
        /// </seealso>
        public virtual void setExactMass(double exactMass)
        {
            this.exactMass = exactMass;
            notifyChanged();
        }


        /// <summary>  Gets the ExactMass attribute of the Isotope object.
        /// <p>Once instantiated all field not filled by passing parameters
        /// to the constructured are null. Isotopes can be configured by using
        /// the IsotopeFactory.configure() method:
        /// <pre>
        /// Isotope isotope = new Isotope("C", 13);
        /// IsotopeFactory if = IsotopeFactory.getInstance(isotope.getBuilder());
        /// if.configure(isotope);
        /// </pre>
        /// </p>
        /// 
        /// </summary>
        /// <returns>    The ExactMass value
        /// 
        /// </returns>
        /// <seealso cref="setExactMass">
        /// </seealso>
        public virtual double getExactMass()
        {
            return this.exactMass;
        }

        /// <summary>  A string representation of this isotope.
        /// 
        /// </summary>
        /// <returns>    A string representation of this isotope
        /// </returns>
        public override System.String ToString()
        {
            System.Text.StringBuilder resultString = new System.Text.StringBuilder(32);
            resultString.Append("Isotope("); resultString.Append(massNumber);
            resultString.Append(", EM:"); resultString.Append(exactMass);
            resultString.Append(", AB:"); resultString.Append(naturalAbundance);
            resultString.Append(", ");
            resultString.Append(base.ToString());
            resultString.Append(')');
            return resultString.ToString();
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
            if (!(object_Renamed is Isotope))
            {
                return false;
            }
            if (!base.compare(object_Renamed))
            {
                return false;
            }
            Isotope isotope = (Isotope)object_Renamed;
            if (massNumber == isotope.massNumber && exactMass == isotope.exactMass && naturalAbundance == isotope.naturalAbundance)
            {
                return true;
            }
            return false;
        }
        //UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
        //virtual public System.Object Clone()
        //{
        //    return null;
        //}
    }
}