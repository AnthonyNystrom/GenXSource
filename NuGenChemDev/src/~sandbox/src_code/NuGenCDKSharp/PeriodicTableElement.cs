/*
*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $
*  $Revision: 6672 $
*
*  Copyright (C) 2004  The Chemistry Development Kit (CDK) Project
*
*  Contact: cdk-devel@lists.sourceforge.net
*
*  This library is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public
*  License as published by the Free Software Foundation; either
*  version 2.1 of the License, or (at your option) any later version.
*
*  This library is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
*  Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public
*  License along with this library; if not, write to the Free Software
*  Foundation, 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA. 
*/
using System;

namespace Org.OpenScience.CDK
{

    /// <summary> subclass of Elements for PeriodicTable.
    /// 
    /// </summary>
    /// <author>         Miguel Rojas
    /// </author>
    /// <cdk.created>    May 8, 2005 </cdk.created>
    /// <cdk.keyword>    element </cdk.keyword>
    /// <cdk.module>     extra </cdk.module>
    [Serializable]
    public class PeriodicTableElement : Element, System.ICloneable
    {
        /// <summary> Returns the name of this element.
        /// 
        /// </summary>
        /// <returns> The name of this element. Null if unset.
        /// 
        /// </returns>
        /// <seealso cref="setName">
        /// </seealso>
        /// <summary> Sets the name of this element.
        /// 
        /// </summary>
        /// <param name="name">The name to be assigned to this element
        /// 
        /// </param>
        /// <seealso cref="getName">
        /// </seealso>
        virtual public System.String Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                notifyChanged();
            }

        }
        
        /// <summary> Returns the chemical serie of this element.
        /// 
        /// </summary>
        /// <returns> The chemical serie of this element. Null if unset.
        /// 
        /// </returns>
        /// <seealso cref="setChemicalSerie">
        /// </seealso>
        /// <summary> Sets the chemical serie of this element.
        /// 
        /// </summary>
        /// <param name="chemicalSerie">The chemical serie to be assigned to this element
        /// 
        /// </param>
        /// <seealso cref="getChemicalSerie">
        /// </seealso>
        virtual public System.String ChemicalSerie
        {
            get
            {
                return this.chemicalSerie;
            }

            set
            {
                this.chemicalSerie = value;
                notifyChanged();
            }

        }
        
        /// <summary> Returns the period which this element belongs.
        /// 
        /// </summary>
        /// <returns> The period of this element. Null if unset.
        /// 
        /// </returns>
        /// <seealso cref="setPeriod">
        /// </seealso>
        /// <summary> Sets the chemical serie of this element.
        /// 
        /// </summary>
        /// <param name="period">The period to be assigned to this element
        /// 
        /// </param>
        /// <seealso cref="getPeriod">
        /// </seealso>
        virtual public System.String Period
        {
            get
            {
                return this.period;
            }

            set
            {
                this.period = value;
                notifyChanged();
            }

        }
        
        /// <summary> Returns the Group which this element belongs.
        /// 
        /// </summary>
        /// <returns> The group of this element. Null if unset.
        /// 
        /// </returns>
        /// <seealso cref="setGroup">
        /// </seealso>
        /// <summary> Sets the group, which this element belongs.
        /// 
        /// </summary>
        /// <param name="group">The group to be assigned to this atom
        /// 
        /// </param>
        /// <seealso cref="getGroup">
        /// </seealso>
        virtual public System.String Group
        {
            get
            {
                return this.group;
            }

            set
            {
                this.group = value;
                notifyChanged();
            }

        }
        
        /// <summary> Returns the phase which this element find.
        /// 
        /// </summary>
        /// <returns> The phase of this element. Null if unset.
        /// 
        /// </returns>
        /// <seealso cref="setPhase">
        /// </seealso>
        /// <summary> Sets the phase, which this element finds.
        /// 
        /// </summary>
        /// <param name="phase">The phase to be assigned to this element
        /// 
        /// </param>
        /// <seealso cref="getGroup">
        /// </seealso>
        /// <seealso cref="getPhase">
        /// </seealso>
        virtual public System.String Phase
        {
            get
            {
                return this.phase;
            }

            set
            {
                this.phase = value;
                notifyChanged();
            }

        }
        
        /// <summary> Returns the CAS (Chemical Abstracts Service), which this element has.
        /// 
        /// </summary>
        /// <returns> The CAS of this element. Null if unset.
        /// 
        /// </returns>
        /// <seealso cref="setCASid">
        /// </seealso>
        /// <summary> Sets the CAS (Chemical Abstracts Service), which this element has.
        /// 
        /// </summary>
        /// <param name="casId">The CAS number to be assigned to this element
        /// 
        /// </param>
        /// <seealso cref="getCASid">
        /// </seealso>
        virtual public System.String CASid
        {
            get
            {
                return this.casId;
            }

            set
            {
                this.casId = value;
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
        private const long serialVersionUID = -2508810950266128526L;

        //private static LoggingTool //logger;

        /// <summary>The name for this element. </summary>
        protected internal System.String name;

        /// <summary>The chemical serie for this element. 
        /// A chemical series is a group of chemical elements whose physical and 
        /// chemical characteristics vary progressively from one end of the series 
        /// to another.
        /// Chemical series were discovered before the creation of the periodic 
        /// table of the chemical elements, which was created to try to organise 
        /// the elements according to their chemical properties. 
        /// </summary>
        protected internal System.String chemicalSerie;

        /// <summary>The period which this element belong. 
        /// In the periodic table of the elements, a period is a row of the table.
        /// </summary>
        protected internal System.String period;

        /// <summary>The group which this element belong.
        /// In the periodic table of the elements, a period is a row of the table.
        /// the elements in a same group have similar configurations of the outermost 
        /// electron shells of their atoms
        /// </summary>
        protected internal System.String group;

        /// <summary>The phase which this element find.
        /// In the physical sciences, a phase is a set of states of a macroscopic 
        /// physical system that have relatively uniform chemical composition 
        /// and physical properties.
        /// Most familiar examples of phases are solids, liquids, and gases
        /// </summary>
        protected internal System.String phase;

        /// <summary>   The CAS (Chemical Abstracts Service) number which this element has.</summary>
        protected internal System.String casId;


        /// <summary>  Constructor for the PeriodicTableElement object.
        /// 
        /// </summary>
        /// <param name="symbol">The symbol of the element
        /// </param>
        public PeriodicTableElement(System.String symbol)
        {
            this.symbol = symbol;
            //logger = new LoggingTool(this);
        }

        /// <summary> Clones this element object.
        /// 
        /// </summary>
        /// <returns>  The cloned object   
        /// </returns>
        public override System.Object Clone()
        {
            System.Object clone = null;
            try
            {
                clone = base.Clone();
            }
            catch (System.Exception exception)
            {
                //logger.debug(exception);
            }
            return clone;
        }
        /// <summary>  Configures an element. Finds the correct element type
        /// by looking at the element symbol.
        /// 
        /// </summary>
        /// <param name="elementPT">  The element of the Periodic Table to be configure
        /// </param>
        /// <returns> element     The configured element
        /// </returns>
        public static Element configure(PeriodicTableElement elementPT)
        {
            Element element = new Element(elementPT.Symbol);

            element.Symbol = elementPT.Symbol;
            element.AtomicNumber = elementPT.AtomicNumber;
            //element.setName(ElementInt.getName());
            //element.setChemicalSerie(ElementInt.getChemicalSerie());
            //element.setPeriod(ElementInt.getPeriod());
            //element.setGroup(ElementInt.getGroup());
            //element.setPhase(ElementInt.getPhase());
            //element.setCASid(ElementInt.getCASid());
            return element;
        }
        /// <summary> </summary>
        /// <returns> resultString  String
        /// </returns>
        public override System.String ToString()
        {
            System.Text.StringBuilder resultString = new System.Text.StringBuilder();
            resultString.Append("PeriodicTableElement(");

            resultString.Append(Symbol);
            resultString.Append(", AN:"); resultString.Append(AtomicNumber);
            resultString.Append(", N:"); resultString.Append(Name);
            resultString.Append(", CS:"); resultString.Append(ChemicalSerie);
            resultString.Append(", P:"); resultString.Append(Period);
            resultString.Append(", G:"); resultString.Append(Group);
            resultString.Append(", Ph:"); resultString.Append(Phase);
            resultString.Append(", CAS:"); resultString.Append(CASid);

            resultString.Append(')');
            return resultString.ToString();
        }
    }
}