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
* 
*/
using System;
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK
{
    /// <summary> Implements the idea of an element in the periodic table.
    /// 
    /// <p>Use the IsotopeFactory to get a ready-to-use elements
    /// by symbol or atomic number:
    /// <pre>
    /// IsotopeFactory if = IsotopeFactory.getInstance(new Element().getBuilder());
    /// Element e1 = if.getElement("C");
    /// Element e2 = if.getElement(12);
    /// </pre>
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  element </cdk.keyword>
    /// <summary> 
    /// </summary>
    /// <seealso cref="org.openscience.cdk.config.IsotopeFactory">
    /// </seealso>
    [Serializable]
    public class Element : ChemObject, IElement, ICloneable
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the atomic number of this element.
        /// 
        /// <p>Once instantiated all field not filled by passing parameters
        /// to the constructured are null. Elements can be configured by using
        /// the IsotopeFactory.configure() method:
        /// <pre>
        /// Element element = new Element("C");
        /// IsotopeFactory if = IsotopeFactory.getInstance(element.getBuilder());
        /// if.configure(element);
        /// </pre>
        /// </p>      
        /// 
        /// </summary>
        /// <returns> The atomic number of this element    
        /// 
        /// </returns>
        /// <seealso cref="setAtomicNumber">
        /// </seealso>
        /// <summary> Sets the atomic number of this element.
        /// 
        /// </summary>
        /// <param name="atomicNumber">The atomic mass to be assigned to this element
        /// 
        /// </param>
        /// <seealso cref="getAtomicNumber">
        /// </seealso>
        virtual public int AtomicNumber
        {
            get
            {
                return this.atomicNumber;
            }

            set
            {
                this.atomicNumber = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the element symbol of this element.
        /// 
        /// </summary>
        /// <returns> The element symbol of this element. Null if unset.
        /// 
        /// </returns>
        /// <seealso cref="setSymbol">
        /// </seealso>
        /// <summary> Sets the element symbol of this element.
        /// 
        /// </summary>
        /// <param name="symbol">The element symbol to be assigned to this atom
        /// 
        /// </param>
        /// <seealso cref="getSymbol">
        /// </seealso>
        virtual public System.String Symbol
        {
            get
            {
                return this.symbol;
            }

            set
            {
                this.symbol = value;
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
        private const long serialVersionUID = 3062529834691231436L;

        /// <summary>The element symbol for this element as listed in the periodic table. </summary>
        protected internal System.String symbol;

        /// <summary>The atomic number for this element giving their position in the periodic table. </summary>
        protected internal int atomicNumber = 0;

        /// <summary> Constructs an empty Element.</summary>
        public Element()
            : base()
        {
            this.symbol = null;
        }

        /// <summary> Constructs an Element with a given 
        /// element symbol.
        /// 
        /// </summary>
        /// <param name="symbol">The element symbol that this element should have.  
        /// </param>
        public Element(System.String symbol)
            : this()
        {
            this.symbol = symbol;
        }

        /// <summary> Constructs an Element with a given element symbol, 
        /// atomic number and atomic mass.
        /// 
        /// </summary>
        /// <param name="symbol"> The element symbol of this element.
        /// </param>
        /// <param name="atomicNumber"> The atomicNumber of this element.
        /// </param>
        public Element(System.String symbol, int atomicNumber)
            : this(symbol)
        {
            this.atomicNumber = atomicNumber;
        }

        public override System.String ToString()
        {
            System.Text.StringBuilder resultString = new System.Text.StringBuilder(32);
            resultString.Append("Element(");
            resultString.Append(Symbol);
            resultString.Append(", ID:").Append(ID);
            resultString.Append(", AN:").Append(AtomicNumber);
            resultString.Append(')');
            return resultString.ToString();
        }

        /// <summary> Compare an Element with this Element.
        /// 
        /// </summary>
        /// <param name="object">Object of type AtomType
        /// </param>
        /// <returns>        Return true, if the atomtypes are equal
        /// </returns>
        public override bool compare(System.Object object_Renamed)
        {
            if (!(object_Renamed is Element))
            {
                return false;
            }
            if (!base.compare(object_Renamed))
            {
                return false;
            }
            Element elem = (Element)object_Renamed;
            if (atomicNumber == elem.atomicNumber && (System.Object)symbol == (System.Object)elem.symbol)
            {
                return true;
            }
            return false;
        }
    }
}