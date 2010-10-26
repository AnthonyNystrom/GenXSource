/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
*
* Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
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
using Org.OpenScience.CDK.IO.CML.CDOPI;
using javax.vecmath;
using Support;
using Org.OpenScience.CDK.Geometry;

namespace Org.OpenScience.CDK.IO.CML
{
    /// <summary> Core CML 1.x and 2.0 elements are parsed by this class.
    /// 
    /// <p>Please file a bug report if this parser fails to parse
    /// a certain element or attribute value in a valid CML document.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>  Egon Willighagen <egonw@sci.kun.nl>
    /// 
    /// </author>
    public class CMLCoreModule : ICMLModule
    {

        //protected internal org.openscience.cdk.tools.LoggingTool //logger;
        //UPGRADE_NOTE: Final was removed from the declaration of 'SYSTEMID '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        protected internal System.String SYSTEMID = "CML-1999-05-15";
        protected internal IChemicalDocumentObject cdo;

        protected internal int atomCounter;
        protected internal System.Collections.ArrayList elsym;
        protected internal System.Collections.ArrayList eltitles;
        protected internal System.Collections.ArrayList elid;
        protected internal System.Collections.ArrayList formalCharges;
        protected internal System.Collections.ArrayList partialCharges;
        protected internal System.Collections.ArrayList isotope;
        protected internal System.Collections.ArrayList x3;
        protected internal System.Collections.ArrayList y3;
        protected internal System.Collections.ArrayList z3;
        protected internal System.Collections.ArrayList x2;
        protected internal System.Collections.ArrayList y2;
        protected internal System.Collections.ArrayList xfract;
        protected internal System.Collections.ArrayList yfract;
        protected internal System.Collections.ArrayList zfract;
        protected internal System.Collections.ArrayList hCounts;
        protected internal System.Collections.ArrayList atomParities;
        protected internal System.Collections.ArrayList atomDictRefs;
        protected internal System.Collections.ArrayList spinMultiplicities;

        protected internal int bondCounter;
        protected internal System.Collections.ArrayList bondid;
        protected internal System.Collections.ArrayList bondARef1;
        protected internal System.Collections.ArrayList bondARef2;
        protected internal System.Collections.ArrayList order;
        protected internal System.Collections.ArrayList bondStereo;
        protected internal System.Collections.ArrayList bondDictRefs;
        protected internal System.Collections.ArrayList bondElid;
        protected internal bool stereoGiven;
        protected internal System.String inchi;
        protected internal int curRef;
        protected internal int CurrentElement;
        protected internal System.String BUILTIN;
        protected internal System.String DICTREF;
        protected internal System.String elementTitle;
        protected internal System.String currentChars;

        protected internal double[] unitcellparams;
        protected internal int crystalScalar;

        private Vector3d aAxis;
        private Vector3d bAxis;
        private Vector3d cAxis;
        internal bool cartesianAxesSet = false;

        public CMLCoreModule(IChemicalDocumentObject cdo)
        {
            ////logger = new LoggingTool(this);
            this.cdo = cdo;
        }

        public CMLCoreModule(ICMLModule conv)
        {
            inherit(conv);
        }

        public virtual void inherit(ICMLModule convention)
        {
            if (convention is CMLCoreModule)
            {
                CMLCoreModule conv = (CMLCoreModule)convention;
                //this.//logger = conv.//logger;
                this.cdo = conv.returnCDO();
                this.BUILTIN = conv.BUILTIN;
                this.atomCounter = conv.atomCounter;
                this.elsym = conv.elsym;
                this.eltitles = conv.eltitles;
                this.elid = conv.elid;
                this.formalCharges = conv.formalCharges;
                this.partialCharges = conv.partialCharges;
                this.isotope = conv.isotope;
                this.x3 = conv.x3;
                this.y3 = conv.y3;
                this.z3 = conv.z3;
                this.x2 = conv.x2;
                this.y2 = conv.y2;
                this.xfract = conv.xfract;
                this.yfract = conv.yfract;
                this.zfract = conv.zfract;
                this.hCounts = conv.hCounts;
                this.atomParities = conv.atomParities;
                this.atomDictRefs = conv.atomDictRefs;
                this.spinMultiplicities = conv.spinMultiplicities;
                this.bondCounter = conv.bondCounter;
                this.bondid = conv.bondid;
                this.bondARef1 = conv.bondARef1;
                this.bondARef2 = conv.bondARef2;
                this.order = conv.order;
                this.bondStereo = conv.bondStereo;
                this.bondDictRefs = conv.bondDictRefs;
                this.curRef = conv.curRef;
                this.unitcellparams = conv.unitcellparams;
                this.inchi = conv.inchi;
            }
            else
            {
                ////logger.warn("Cannot inherit information from module: ", convention.GetType().FullName);
            }
        }

        public virtual IChemicalDocumentObject returnCDO()
        {
            return (IChemicalDocumentObject)this.cdo;
        }

        /// <summary> Clean all data about parsed data.</summary>
        protected internal virtual void newMolecule()
        {
            newMoleculeData();
            newAtomData();
            newBondData();
            newCrystalData();
        }

        /// <summary> Clean all data about the molecule itself.</summary>
        protected internal virtual void newMoleculeData()
        {
            this.inchi = null;
        }

        /// <summary> Clean all data about read atoms.</summary>
        protected internal virtual void newAtomData()
        {
            atomCounter = 0;
            elsym = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            elid = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            eltitles = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            formalCharges = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            partialCharges = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            isotope = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            x3 = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            y3 = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            z3 = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            x2 = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            y2 = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            xfract = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            yfract = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            zfract = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            hCounts = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            atomParities = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            atomDictRefs = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            spinMultiplicities = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
        }

        /// <summary> Clean all data about read bonds.</summary>
        protected internal virtual void newBondData()
        {
            bondCounter = 0;
            bondid = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            bondARef1 = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            bondARef2 = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            order = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            bondStereo = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            bondDictRefs = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            bondElid = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
        }

        /// <summary> Clean all data about read bonds.</summary>
        protected internal virtual void newCrystalData()
        {
            unitcellparams = new double[6];
            cartesianAxesSet = false;
            crystalScalar = 0;
            aAxis = new Vector3d();
            bAxis = new Vector3d();
            cAxis = new Vector3d();
        }

        public virtual void startDocument()
        {
            ////logger.info("Start XML Doc");
            cdo.startDocument();
            newMolecule();
            BUILTIN = "";
            curRef = 0;
        }

        public virtual void endDocument()
        {
            cdo.endDocument();
            ////logger.info("End XML Doc");
        }

        public virtual void startElement(CMLStack xpath, System.String uri, System.String local, System.String raw, SaxAttributesSupport atts)
        {

            System.String name = local;
            ////logger.debug("StartElement");
            currentChars = "";

            BUILTIN = "";
            DICTREF = "";

            for (int i = 0; i < atts.GetLength(); i++)
            {
                System.String qname = atts.GetFullName(i);
                if (qname.Equals("builtin"))
                {
                    BUILTIN = atts.GetValue(i);
                    ////logger.debug(name, "->BUILTIN found: ", atts.GetValue(i));
                }
                else if (qname.Equals("dictRef"))
                {
                    DICTREF = atts.GetValue(i);
                    ////logger.debug(name, "->DICTREF found: ", atts.GetValue(i));
                }
                else if (qname.Equals("title"))
                {
                    elementTitle = atts.GetValue(i);
                    ////logger.debug(name, "->TITLE found: ", atts.GetValue(i));
                }
                else
                {
                    ////logger.debug("Qname: ", qname);
                }
            }

            if ("atom".Equals(name))
            {
                atomCounter++;
                for (int i = 0; i < atts.GetLength(); i++)
                {

                    System.String att = atts.GetFullName(i);
                    System.String value_Renamed = atts.GetValue(i);

                    if (att.Equals("id"))
                    {
                        // this is supported in CML 1.x
                        elid.Add(value_Renamed);
                    }
                    // this is supported in CML 2.0 
                    else if (att.Equals("elementType"))
                    {
                        elsym.Add(value_Renamed);
                    }
                    // this is supported in CML 2.0 
                    else if (att.Equals("title"))
                    {
                        eltitles.Add(value_Renamed);
                    }
                    // this is supported in CML 2.0 
                    else if (att.Equals("x2"))
                    {
                        x2.Add(value_Renamed);
                    }
                    // this is supported in CML 2.0 
                    else if (att.Equals("xy2"))
                    {
                        SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(value_Renamed);
                        x2.Add(tokenizer.NextToken());
                        y2.Add(tokenizer.NextToken());
                    }
                    // this is supported in CML 2.0 
                    else if (att.Equals("xyzFract"))
                    {
                        SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(value_Renamed);
                        xfract.Add(tokenizer.NextToken());
                        yfract.Add(tokenizer.NextToken());
                        zfract.Add(tokenizer.NextToken());
                    }
                    // this is supported in CML 2.0 
                    else if (att.Equals("xyz3"))
                    {
                        SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(value_Renamed);
                        x3.Add(tokenizer.NextToken());
                        y3.Add(tokenizer.NextToken());
                        z3.Add(tokenizer.NextToken());
                    }
                    // this is supported in CML 2.0 
                    else if (att.Equals("y2"))
                    {
                        y2.Add(value_Renamed);
                    }
                    // this is supported in CML 2.0 
                    else if (att.Equals("x3"))
                    {
                        x3.Add(value_Renamed);
                    }
                    // this is supported in CML 2.0 
                    else if (att.Equals("y3"))
                    {
                        y3.Add(value_Renamed);
                    }
                    // this is supported in CML 2.0 
                    else if (att.Equals("z3"))
                    {
                        z3.Add(value_Renamed);
                    }
                    // this is supported in CML 2.0 
                    else if (att.Equals("xFract"))
                    {
                        xfract.Add(value_Renamed);
                    }
                    // this is supported in CML 2.0 
                    else if (att.Equals("yFract"))
                    {
                        yfract.Add(value_Renamed);
                    }
                    // this is supported in CML 2.0 
                    else if (att.Equals("zFract"))
                    {
                        zfract.Add(value_Renamed);
                    }
                    // this is supported in CML 2.0 
                    else if (att.Equals("formalCharge"))
                    {
                        formalCharges.Add(value_Renamed);
                    }
                    // this is supported in CML 2.0 
                    else if (att.Equals("hydrogenCount"))
                    {
                        hCounts.Add(value_Renamed);
                    }
                    else if (att.Equals("isotope"))
                    {
                        isotope.Add(value_Renamed);
                    }
                    else if (att.Equals("dictRef"))
                    {
                        atomDictRefs.Add(value_Renamed);
                    }
                    else if (att.Equals("spinMultiplicity"))
                    {
                        spinMultiplicities.Add(value_Renamed);
                    }
                    else
                    {
                        ////logger.warn("Unparsed attribute: " + att);
                    }
                }
            }
            else if ("atomArray".Equals(name))
            {
                bool atomsCounted = false;
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    System.String att = atts.GetFullName(i);
                    int count = 0;
                    if (att.Equals("atomID"))
                    {
                        count = addArrayElementsTo(elid, atts.GetValue(i));
                    }
                    else if (att.Equals("elementType"))
                    {
                        count = addArrayElementsTo(elsym, atts.GetValue(i));
                    }
                    else if (att.Equals("x2"))
                    {
                        count = addArrayElementsTo(x2, atts.GetValue(i));
                    }
                    else if (att.Equals("y2"))
                    {
                        count = addArrayElementsTo(y2, atts.GetValue(i));
                    }
                    else if (att.Equals("x3"))
                    {
                        count = addArrayElementsTo(x3, atts.GetValue(i));
                    }
                    else if (att.Equals("y3"))
                    {
                        count = addArrayElementsTo(y3, atts.GetValue(i));
                    }
                    else if (att.Equals("z3"))
                    {
                        count = addArrayElementsTo(z3, atts.GetValue(i));
                    }
                    else if (att.Equals("xFract"))
                    {
                        count = addArrayElementsTo(xfract, atts.GetValue(i));
                    }
                    else if (att.Equals("yFract"))
                    {
                        count = addArrayElementsTo(yfract, atts.GetValue(i));
                    }
                    else if (att.Equals("zFract"))
                    {
                        count = addArrayElementsTo(zfract, atts.GetValue(i));
                    }
                    else
                    {
                        ////logger.warn("Unparsed attribute: " + att);
                    }
                    if (!atomsCounted)
                    {
                        atomCounter += count;
                        atomsCounted = true;
                    }
                }
            }
            else if ("bond".Equals(name))
            {
                bondCounter++;
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    System.String att = atts.GetFullName(i);
                    ////logger.debug("B2 ", att, "=", atts.GetValue(i));

                    if (att.Equals("id"))
                    {
                        bondid.Add(atts.GetValue(i));
                        ////logger.debug("B3 ", bondid);
                    }
                    else if (att.Equals("atomRefs") || att.Equals("atomRefs2"))
                    {
                        // this is CML 2.0 support

                        // expect exactly two references
                        try
                        {
                            SupportClass.Tokenizer st = new SupportClass.Tokenizer(atts.GetValue(i));
                            bondARef1.Add((System.String)st.NextToken());
                            bondARef2.Add((System.String)st.NextToken());
                        }
                        catch (System.Exception e)
                        {
                            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                            ////logger.error("Error in CML file: ", e.Message);
                            ////logger.debug(e);
                        }
                    }
                    else if (att.Equals("order"))
                    {
                        // this is CML 2.0 support
                        order.Add(atts.GetValue(i).Trim());
                    }
                    else if (att.Equals("dictRef"))
                    {
                        bondDictRefs.Add(atts.GetValue(i).Trim());
                    }
                }

                stereoGiven = false;
                curRef = 0;
            }
            else if ("bondArray".Equals(name))
            {
                bool bondsCounted = false;
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    System.String att = atts.GetFullName(i);
                    int count = 0;
                    if (att.Equals("bondID"))
                    {
                        count = addArrayElementsTo(bondid, atts.GetValue(i));
                    }
                    else if (att.Equals("atomRefs1"))
                    {
                        count = addArrayElementsTo(bondARef1, atts.GetValue(i));
                    }
                    else if (att.Equals("atomRefs2"))
                    {
                        count = addArrayElementsTo(bondARef2, atts.GetValue(i));
                    }
                    else if (att.Equals("atomRef1"))
                    {
                        count = addArrayElementsTo(bondARef1, atts.GetValue(i));
                    }
                    else if (att.Equals("atomRef2"))
                    {
                        count = addArrayElementsTo(bondARef2, atts.GetValue(i));
                    }
                    else if (att.Equals("order"))
                    {
                        count = addArrayElementsTo(order, atts.GetValue(i));
                    }
                    else
                    {
                        ////logger.warn("Unparsed attribute: " + att);
                    }
                    if (!bondsCounted)
                    {
                        bondCounter += count;
                        bondsCounted = true;
                    }
                }
                curRef = 0;
            }
            else if ("molecule".Equals(name))
            {
                newMolecule();
                BUILTIN = "";
                cdo.startObject("Molecule");
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    if (atts.GetFullName(i).Equals("id"))
                    {
                        cdo.setObjectProperty("Molecule", "id", atts.GetValue(i));
                    }
                    else if (atts.GetFullName(i).Equals("dictRef"))
                    {
                        cdo.setObjectProperty("Molecule", "dictRef", atts.GetValue(i));
                    }
                }
            }
            else if ("crystal".Equals(name))
            {
                newCrystalData();
                cdo.startObject("Crystal");
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    System.String att = atts.GetFullName(i);
                    if (att.Equals("z"))
                    {
                        cdo.setObjectProperty("Crystal", "z", atts.GetValue(i));
                    }
                }
            }
            else if ("symmetry".Equals(name))
            {
                for (int i = 0; i < atts.GetLength(); i++)
                {
                    System.String att = atts.GetFullName(i);
                    if (att.Equals("spaceGroup"))
                    {
                        cdo.setObjectProperty("Crystal", "spacegroup", atts.GetValue(i));
                    }
                }
            }
            else if ("scalar".Equals(name))
            {
                if (xpath.ToString().EndsWith("crystal/scalar/"))
                    crystalScalar++;
            }
            else if ("list".Equals(name))
            {
                cdo.startObject("SetOfMolecules");
            }
        }

        public virtual void endElement(CMLStack xpath, System.String uri, System.String name, System.String raw)
        {
            ////logger.debug("EndElement: ", name);

            System.String cData = currentChars;

            if ("bond".Equals(name))
            {
                if (!stereoGiven)
                    bondStereo.Add("");
                if (bondStereo.Count > bondDictRefs.Count)
                    bondDictRefs.Add(null);
            }
            else if ("atom".Equals(name))
            {
                if (atomCounter > eltitles.Count)
                {
                    eltitles.Add(null);
                }
                if (atomCounter > hCounts.Count)
                {
                    /* while strictly undefined, assume zero 
                    implicit hydrogens when no number is given */
                    hCounts.Add("0");
                }
                if (atomCounter > atomDictRefs.Count)
                {
                    atomDictRefs.Add(null);
                }
                if (atomCounter > isotope.Count)
                {
                    isotope.Add(null);
                }
                if (atomCounter > spinMultiplicities.Count)
                {
                    spinMultiplicities.Add(null);
                }
                if (atomCounter > formalCharges.Count)
                {
                    /* while strictly undefined, assume zero 
                    implicit hydrogens when no number is given */
                    formalCharges.Add("0");
                }
                /* It may happen that not all atoms have
                associated 2D or 3D coordinates. accept that */
                if (atomCounter > x2.Count && x2.Count != 0)
                {
                    /* apparently, the previous atoms had atomic
                    coordinates, add 'null' for this atom */
                    x2.Add(null);
                    y2.Add(null);
                }
                if (atomCounter > x3.Count && x3.Count != 0)
                {
                    /* apparently, the previous atoms had atomic
                    coordinates, add 'null' for this atom */
                    x3.Add(null);
                    y3.Add(null);
                    z3.Add(null);
                }

                if (atomCounter > xfract.Count && xfract.Count != 0)
                {
                    /* apparently, the previous atoms had atomic
                    coordinates, add 'null' for this atom */
                    xfract.Add(null);
                    yfract.Add(null);
                    zfract.Add(null);
                }
            }
            else if ("molecule".Equals(name))
            {
                storeData();
                cdo.endObject("Molecule");
            }
            else if ("crystal".Equals(name))
            {
                if (crystalScalar > 0)
                {
                    // convert unit cell parameters to cartesians
                    Vector3d[] axes = CrystalGeometryTools.notionalToCartesian(unitcellparams[0], unitcellparams[1], unitcellparams[2], unitcellparams[3], unitcellparams[4], unitcellparams[5]);
                    aAxis = axes[0];
                    bAxis = axes[1];
                    cAxis = axes[2];
                    cartesianAxesSet = true;
                    cdo.startObject("a-axis");
                    cdo.setObjectProperty("a-axis", "x", aAxis.x.ToString());
                    cdo.setObjectProperty("a-axis", "y", aAxis.y.ToString());
                    cdo.setObjectProperty("a-axis", "z", aAxis.z.ToString());
                    cdo.endObject("a-axis");
                    cdo.startObject("b-axis");
                    cdo.setObjectProperty("b-axis", "x", bAxis.x.ToString());
                    cdo.setObjectProperty("b-axis", "y", bAxis.y.ToString());
                    cdo.setObjectProperty("b-axis", "z", bAxis.z.ToString());
                    cdo.endObject("b-axis");
                    cdo.startObject("c-axis");
                    cdo.setObjectProperty("c-axis", "x", cAxis.x.ToString());
                    cdo.setObjectProperty("c-axis", "y", cAxis.y.ToString());
                    cdo.setObjectProperty("c-axis", "z", cAxis.z.ToString());
                    cdo.endObject("c-axis");
                }
                else
                {
                    ////logger.error("Could not find crystal unit cell parameters");
                }
                cdo.endObject("Crystal");
            }
            else if ("list".Equals(name))
            {
                cdo.endObject("SetOfMolecules");
            }
            else if ("coordinate3".Equals(name))
            {
                if (BUILTIN.Equals("xyz3"))
                {
                    ////logger.debug("New coord3 xyz3 found: ", currentChars);

                    try
                    {

                        SupportClass.Tokenizer st = new SupportClass.Tokenizer(currentChars);
                        x3.Add(st.NextToken());
                        y3.Add(st.NextToken());
                        z3.Add(st.NextToken());
                        ////logger.debug("coord3 x3.length: ", x3.Count);
                        ////logger.debug("coord3 y3.length: ", y3.Count);
                        ////logger.debug("coord3 z3.length: ", z3.Count);
                    }
                    catch (System.Exception exception)
                    {
                        ////logger.error("CMLParsing error while setting coordinate3!");
                        ////logger.debug(exception);
                    }
                }
                else
                {
                    ////logger.warn("Unknown coordinate3 BUILTIN: " + BUILTIN);
                }
            }
            else if ("string".Equals(name))
            {
                if (BUILTIN.Equals("elementType"))
                {
                    ////logger.debug("Element: ", cData.Trim());
                    elsym.Add(cData);
                }
                else if (BUILTIN.Equals("atomRef"))
                {
                    curRef++;
                    ////logger.debug("Bond: ref #", curRef);

                    if (curRef == 1)
                    {
                        bondARef1.Add(cData.Trim());
                    }
                    else if (curRef == 2)
                    {
                        bondARef2.Add(cData.Trim());
                    }
                }
                else if (BUILTIN.Equals("order"))
                {
                    ////logger.debug("Bond: order ", cData.Trim());
                    order.Add(cData.Trim());
                }
                else if (BUILTIN.Equals("formalCharge"))
                {
                    // NOTE: this combination is in violation of the CML DTD!!!
                    ////logger.warn("formalCharge BUILTIN accepted but violating CML DTD");
                    ////logger.debug("Charge: ", cData.Trim());
                    System.String charge = cData.Trim();
                    if (charge.StartsWith("+") && charge.Length > 1)
                    {
                        charge = charge.Substring(1);
                    }
                    formalCharges.Add(charge);
                }
            }
            else if ("float".Equals(name))
            {
                if (BUILTIN.Equals("x3"))
                {
                    x3.Add(cData.Trim());
                }
                else if (BUILTIN.Equals("y3"))
                {
                    y3.Add(cData.Trim());
                }
                else if (BUILTIN.Equals("z3"))
                {
                    z3.Add(cData.Trim());
                }
                else if (BUILTIN.Equals("x2"))
                {
                    x2.Add(cData.Trim());
                }
                else if (BUILTIN.Equals("y2"))
                {
                    y2.Add(cData.Trim());
                }
                else if (BUILTIN.Equals("order"))
                {
                    // NOTE: this combination is in violation of the CML DTD!!!
                    order.Add(cData.Trim());
                }
                else if (BUILTIN.Equals("charge") || BUILTIN.Equals("partialCharge"))
                {
                    partialCharges.Add(cData.Trim());
                }
            }
            else if ("integer".Equals(name))
            {
                if (BUILTIN.Equals("formalCharge"))
                {
                    formalCharges.Add(cData.Trim());
                }
            }
            else if ("coordinate2".Equals(name))
            {
                if (BUILTIN.Equals("xy2"))
                {
                    ////logger.debug("New coord2 xy2 found.", cData);

                    try
                    {

                        SupportClass.Tokenizer st = new SupportClass.Tokenizer(cData);
                        x2.Add(st.NextToken());
                        y2.Add(st.NextToken());
                    }
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        notify("CMLParsing error: " + e, SYSTEMID, 175, 1);
                    }
                }
            }
            else if ("stringArray".Equals(name))
            {
                if (BUILTIN.Equals("id") || BUILTIN.Equals("atomId") || BUILTIN.Equals("atomID"))
                {
                    // invalid according to CML1 DTD but found in OpenBabel 1.x output

                    try
                    {
                        bool countAtoms = (atomCounter == 0) ? true : false;
                        SupportClass.Tokenizer st = new SupportClass.Tokenizer(cData);

                        while (st.HasMoreTokens())
                        {
                            if (countAtoms)
                            {
                                atomCounter++;
                            }
                            System.String token = st.NextToken();
                            ////logger.debug("StringArray (Token): ", token);
                            elid.Add(token);
                        }
                    }
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        notify("CMLParsing error: " + e, SYSTEMID, 186, 1);
                    }
                }
                else if (BUILTIN.Equals("elementType"))
                {

                    try
                    {
                        bool countAtoms = (atomCounter == 0) ? true : false;
                        SupportClass.Tokenizer st = new SupportClass.Tokenizer(cData);

                        while (st.HasMoreTokens())
                        {
                            if (countAtoms)
                            {
                                atomCounter++;
                            }
                            elsym.Add(st.NextToken());
                        }
                    }
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        notify("CMLParsing error: " + e, SYSTEMID, 194, 1);
                    }
                }
                else if (BUILTIN.Equals("atomRefs"))
                {
                    curRef++;
                    ////logger.debug("New atomRefs found: ", curRef);

                    try
                    {
                        bool countBonds = (bondCounter == 0) ? true : false;
                        SupportClass.Tokenizer st = new SupportClass.Tokenizer(cData);

                        while (st.HasMoreTokens())
                        {
                            if (countBonds)
                            {
                                bondCounter++;
                            }
                            System.String token = st.NextToken();
                            ////logger.debug("Token: ", token);

                            if (curRef == 1)
                            {
                                bondARef1.Add(token);
                            }
                            else if (curRef == 2)
                            {
                                bondARef2.Add(token);
                            }
                        }
                    }
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        notify("CMLParsing error: " + e, SYSTEMID, 194, 1);
                    }
                }
                else if (BUILTIN.Equals("atomRef"))
                {
                    curRef++;
                    ////logger.debug("New atomRef found: ", curRef); // this is CML1 stuff, we get things like:
                    /*
                    <bondArray>
                    <stringArray builtin="atomRef">a2 a2 a2 a2 a3 a3 a4 a4 a5 a6 a7 a9</stringArray>
                    <stringArray builtin="atomRef">a9 a11 a12 a13 a5 a4 a6 a9 a7 a8 a8 a10</stringArray>
                    <stringArray builtin="order">1 1 1 1 2 1 2 1 1 1 2 2</stringArray>
                    </bondArray>
                    */

                    try
                    {
                        bool countBonds = (bondCounter == 0) ? true : false;
                        SupportClass.Tokenizer st = new SupportClass.Tokenizer(cData);

                        while (st.HasMoreTokens())
                        {
                            if (countBonds)
                            {
                                bondCounter++;
                            }
                            System.String token = st.NextToken();
                            ////logger.debug("Token: ", token);

                            if (curRef == 1)
                            {
                                bondARef1.Add(token);
                            }
                            else if (curRef == 2)
                            {
                                bondARef2.Add(token);
                            }
                        }
                    }
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        notify("CMLParsing error: " + e, SYSTEMID, 194, 1);
                    }
                }
                else if (BUILTIN.Equals("order"))
                {
                    ////logger.debug("New bond order found.");

                    try
                    {

                        SupportClass.Tokenizer st = new SupportClass.Tokenizer(cData);

                        while (st.HasMoreTokens())
                        {

                            System.String token = st.NextToken();
                            ////logger.debug("Token: ", token);
                            order.Add(token);
                        }
                    }
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        notify("CMLParsing error: " + e, SYSTEMID, 194, 1);
                    }
                }
            }
            else if ("integerArray".Equals(name))
            {
                ////logger.debug("IntegerArray: builtin = ", BUILTIN);

                if (BUILTIN.Equals("formalCharge"))
                {

                    try
                    {

                        SupportClass.Tokenizer st = new SupportClass.Tokenizer(cData);

                        while (st.HasMoreTokens())
                        {

                            System.String token = st.NextToken();
                            ////logger.debug("Charge added: ", token);
                            formalCharges.Add(token);
                        }
                    }
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        notify("CMLParsing error: " + e, SYSTEMID, 205, 1);
                    }
                }
            }
            else if ("scalar".Equals(name))
            {
                if (xpath.ToString().EndsWith("crystal/scalar/"))
                {
                    ////logger.debug("Going to set a crystal parameter: " + crystalScalar, " to ", cData);
                    try
                    {
                        unitcellparams[crystalScalar - 1] = System.Double.Parse(cData.Trim());
                    }
                    catch (System.FormatException exception)
                    {
                        ////logger.error("Content must a float: " + cData);
                    }
                }
                else
                {
                    if (xpath.ToString().EndsWith("bond/scalar/"))
                    {
                        if (DICTREF.Equals("mdl:stereo"))
                        {
                            bondStereo.Add(cData.Trim());
                            stereoGiven = true;
                        }
                    }
                    else
                    {
                        if (xpath.ToString().EndsWith("atom/scalar/"))
                        {
                            if (DICTREF.Equals("cdk:partialCharge"))
                            {
                                partialCharges.Add(cData.Trim());
                            }
                        }
                        else
                        {
                            if (xpath.ToString().EndsWith("molecule/scalar/"))
                            {
                                if (DICTREF.Equals("pdb:id"))
                                {
                                    cdo.setObjectProperty("Molecule", DICTREF, cData);
                                }
                            }
                            else
                            {
                                ////logger.warn("Ignoring scalar: " + xpath);
                            }
                        }
                    }
                }
            }
            else if ("floatArray".Equals(name))
            {
                if (BUILTIN.Equals("x3"))
                {

                    try
                    {

                        SupportClass.Tokenizer st = new SupportClass.Tokenizer(cData);

                        while (st.HasMoreTokens())
                            x3.Add(st.NextToken());
                    }
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        notify("CMLParsing error: " + e, SYSTEMID, 205, 1);
                    }
                }
                else if (BUILTIN.Equals("y3"))
                {

                    try
                    {

                        SupportClass.Tokenizer st = new SupportClass.Tokenizer(cData);

                        while (st.HasMoreTokens())
                            y3.Add(st.NextToken());
                    }
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        notify("CMLParsing error: " + e, SYSTEMID, 213, 1);
                    }
                }
                else if (BUILTIN.Equals("z3"))
                {

                    try
                    {

                        SupportClass.Tokenizer st = new SupportClass.Tokenizer(cData);

                        while (st.HasMoreTokens())
                            z3.Add(st.NextToken());
                    }
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        notify("CMLParsing error: " + e, SYSTEMID, 221, 1);
                    }
                }
                else if (BUILTIN.Equals("x2"))
                {
                    ////logger.debug("New floatArray found.");

                    try
                    {

                        SupportClass.Tokenizer st = new SupportClass.Tokenizer(cData);

                        while (st.HasMoreTokens())
                            x2.Add(st.NextToken());
                    }
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        notify("CMLParsing error: " + e, SYSTEMID, 205, 1);
                    }
                }
                else if (BUILTIN.Equals("y2"))
                {
                    ////logger.debug("New floatArray found.");

                    try
                    {

                        SupportClass.Tokenizer st = new SupportClass.Tokenizer(cData);

                        while (st.HasMoreTokens())
                            y2.Add(st.NextToken());
                    }
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        notify("CMLParsing error: " + e, SYSTEMID, 454, 1);
                    }
                }
                else if (BUILTIN.Equals("partialCharge"))
                {
                    ////logger.debug("New floatArray with partial charges found.");

                    try
                    {

                        SupportClass.Tokenizer st = new SupportClass.Tokenizer(cData);

                        while (st.HasMoreTokens())
                            partialCharges.Add(st.NextToken());
                    }
                    catch (System.Exception e)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        notify("CMLParsing error: " + e, SYSTEMID, 462, 1);
                    }
                }
            }
            else if ("basic".Equals(name))
            {
                // assuming this is the child element of <identifier>
                this.inchi = cData;
            }
            else if ("name".Equals(name))
            {
                if (xpath.ToString().EndsWith("molecule/name/"))
                {
                    cdo.setObjectProperty("Molecule", DICTREF, cData);
                }
            }
            else
            {
                ////logger.warn("Skipping element: " + name);
            }

            currentChars = "";
            BUILTIN = "";
            elementTitle = "";
        }

        public virtual void characterData(CMLStack xpath, char[] ch, int start, int length)
        {
            currentChars = currentChars + new System.String(ch, start, length);
            ////logger.debug("CD: ", currentChars);
        }

        protected internal virtual void notify(System.String message, System.String systemId, int line, int column)
        {
        //    //logger.debug("Message: ", message);
        //    //logger.debug("SystemId: ", systemId);
        //    //logger.debug("Line: ", line);
        //    //logger.debug("Column: ", column);
        }

        protected internal virtual void storeData()
        {
            if (inchi != null)
            {
                cdo.setObjectProperty("Molecule", "inchi", inchi);
            }
            storeAtomData();
            storeBondData();
        }

        protected internal virtual void storeAtomData()
        {
            ////logger.debug("No atoms: ", atomCounter);
            if (atomCounter == 0)
            {
                return;
            }

            bool hasID = false;
            bool has3D = false;
            bool has3Dfract = false;
            bool has2D = false;
            bool hasFormalCharge = false;
            bool hasPartialCharge = false;
            bool hasHCounts = false;
            bool hasSymbols = false;
            bool hasTitles = false;
            bool hasIsotopes = false;
            bool hasDictRefs = false;
            bool hasSpinMultiplicities = false;

            if (elid.Count == atomCounter)
            {
                hasID = true;
            }
            else
            {
                ////logger.debug("No atom ids: " + elid.Count, " != " + atomCounter);
            }

            if (elsym.Count == atomCounter)
            {
                hasSymbols = true;
            }
            else
            {
                ////logger.debug("No atom symbols: " + elsym.Count, " != " + atomCounter);
            }

            if (eltitles.Count == atomCounter)
            {
                hasTitles = true;
            }
            else
            {
                ////logger.debug("No atom titles: " + eltitles.Count, " != " + atomCounter);
            }

            if ((x3.Count == atomCounter) && (y3.Count == atomCounter) && (z3.Count == atomCounter))
            {
                has3D = true;
            }
            else
            {
                ////logger.debug("No 3D info: " + x3.Count, " " + y3.Count, " " + z3.Count, " != " + atomCounter);
            }

            if ((xfract.Count == atomCounter) && (yfract.Count == atomCounter) && (zfract.Count == atomCounter))
            {
                has3Dfract = true;
            }
            else
            {
                ////logger.debug("No 3D fractional info: " + xfract.Count, " " + yfract.Count, " " + zfract.Count, " != " + atomCounter);
            }

            if ((x2.Count == atomCounter) && (y2.Count == atomCounter))
            {
                has2D = true;
            }
            else
            {
                ////logger.debug("No 2D info: " + x2.Count, " " + y2.Count, " != " + atomCounter);
            }

            if (formalCharges.Count == atomCounter)
            {
                hasFormalCharge = true;
            }
            else
            {
                ////logger.debug("No formal Charge info: " + formalCharges.Count, " != " + atomCounter);
            }

            if (partialCharges.Count == atomCounter)
            {
                hasPartialCharge = true;
            }
            else
            {
                ////logger.debug("No partial Charge info: " + partialCharges.Count, " != " + atomCounter);
            }

            if (hCounts.Count == atomCounter)
            {
                hasHCounts = true;
            }
            else
            {
                ////logger.debug("No hydrogen Count info: " + hCounts.Count, " != " + atomCounter);
            }

            if (spinMultiplicities.Count == atomCounter)
            {
                hasSpinMultiplicities = true;
            }
            else
            {
                ////logger.debug("No spinMultiplicity info: " + spinMultiplicities.Count, " != " + atomCounter);
            }

            if (atomDictRefs.Count == atomCounter)
            {
                hasDictRefs = true;
            }
            else
            {
                ////logger.debug("No dictRef info: " + atomDictRefs.Count, " != " + atomCounter);
            }

            if (isotope.Count == atomCounter)
            {
                hasIsotopes = true;
            }
            else
            {
                ////logger.debug("No isotope info: " + isotope.Count, " != " + atomCounter);
            }

            for (int i = 0; i < atomCounter; i++)
            {
                ////logger.info("Storing atom: ", i);
                cdo.startObject("Atom");
                if (hasID)
                {
                    cdo.setObjectProperty("Atom", "id", (System.String)elid[i]);
                }
                if (hasTitles)
                {
                    if (hasSymbols)
                    {
                        System.String symbol = (System.String)elsym[i];
                        if (symbol.Equals("Du") || symbol.Equals("Dummy"))
                        {
                            cdo.setObjectProperty("PseudoAtom", "label", (System.String)eltitles[i]);
                        }
                        else
                        {
                            cdo.setObjectProperty("Atom", "title", (System.String)eltitles[i]);
                        }
                    }
                    else
                    {
                        cdo.setObjectProperty("Atom", "title", (System.String)eltitles[i]);
                    }
                }

                // store optional atom properties
                if (hasSymbols)
                {
                    System.String symbol = (System.String)elsym[i];
                    if (symbol.Equals("Du") || symbol.Equals("Dummy"))
                    {
                        symbol = "R";
                    }
                    cdo.setObjectProperty("Atom", "type", symbol);
                }

                if (has3D)
                {
                    cdo.setObjectProperty("Atom", "x3", (System.String)x3[i]);
                    cdo.setObjectProperty("Atom", "y3", (System.String)y3[i]);
                    cdo.setObjectProperty("Atom", "z3", (System.String)z3[i]);
                }

                if (has3Dfract)
                {
                    // ok, need to convert fractional into eucledian coordinates
                    cdo.setObjectProperty("Atom", "xFract", (System.String)xfract[i]);
                    cdo.setObjectProperty("Atom", "yFract", (System.String)yfract[i]);
                    cdo.setObjectProperty("Atom", "zFract", (System.String)zfract[i]);
                }

                if (hasFormalCharge)
                {
                    cdo.setObjectProperty("Atom", "formalCharge", (System.String)formalCharges[i]);
                }

                if (hasPartialCharge)
                {
                    ////logger.debug("Storing partial atomic charge...");
                    cdo.setObjectProperty("Atom", "partialCharge", (System.String)partialCharges[i]);
                }

                if (hasHCounts)
                {
                    cdo.setObjectProperty("Atom", "hydrogenCount", (System.String)hCounts[i]);
                }

                if (has2D)
                {
                    if (x2[i] != null)
                        cdo.setObjectProperty("Atom", "x2", (System.String)x2[i]);
                    if (y2[i] != null)
                        cdo.setObjectProperty("Atom", "y2", (System.String)y2[i]);
                }

                if (hasDictRefs)
                {
                    cdo.setObjectProperty("Atom", "dictRef", (System.String)atomDictRefs[i]);
                }

                if (hasSpinMultiplicities && spinMultiplicities[i] != null)
                {
                    cdo.setObjectProperty("Atom", "spinMultiplicity", (System.String)spinMultiplicities[i]);
                }

                if (hasIsotopes)
                {
                    cdo.setObjectProperty("Atom", "massNumber", (System.String)isotope[i]);
                }

                cdo.endObject("Atom");
            }
            if (elid.Count > 0)
            {
                // assume this is the current working list
                bondElid = elid;
            }
            newAtomData();
        }

        protected internal virtual void storeBondData()
        {
            ////logger.debug("Testing a1,a2,stereo,order = count: " + bondARef1.Count, "," + bondARef2.Count, "," + bondStereo.Count, "," + order.Count, "=" + bondCounter);

            if ((bondARef1.Count == bondCounter) && (bondARef2.Count == bondCounter))
            {
                ////logger.debug("About to add bond info to ", cdo.GetType().FullName);

                System.Collections.IEnumerator orders = order.GetEnumerator();
                System.Collections.IEnumerator ids = bondid.GetEnumerator();
                System.Collections.IEnumerator bar1s = bondARef1.GetEnumerator();
                System.Collections.IEnumerator bar2s = bondARef2.GetEnumerator();
                System.Collections.IEnumerator stereos = bondStereo.GetEnumerator();

                while (bar1s.MoveNext() && bar2s.MoveNext())
                {
                    cdo.startObject("Bond");
                    if (ids.MoveNext())
                    {
                        cdo.setObjectProperty("Bond", "id", (System.String)ids.Current);
                    }
                    cdo.setObjectProperty("Bond", "atom1", ((System.Int32)bondElid.IndexOf((System.String)bar1s.Current)).ToString());
                    cdo.setObjectProperty("Bond", "atom2", ((System.Int32)bondElid.IndexOf((System.String)bar2s.Current)).ToString());

                    if (orders.MoveNext())
                    {
                        System.String bondOrder = (System.String)orders.Current;

                        if ("S".Equals(bondOrder))
                            cdo.setObjectProperty("Bond", "order", "1");
                        else if ("D".Equals(bondOrder))
                            cdo.setObjectProperty("Bond", "order", "2");
                        else if ("T".Equals(bondOrder))
                            cdo.setObjectProperty("Bond", "order", "3");
                        else if ("A".Equals(bondOrder))
                            cdo.setObjectProperty("Bond", "order", "1.5");
                        else
                            cdo.setObjectProperty("Bond", "order", bondOrder);
                    }

                    if (stereos.MoveNext())
                        cdo.setObjectProperty("Bond", "stereo", (System.String)stereos.Current);

                    cdo.endObject("Bond");
                }
            }
            newBondData();
        }

        private int addArrayElementsTo(System.Collections.ArrayList toAddto, System.String array)
        {
            SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(array);
            int i = 0;
            while (tokenizer.HasMoreTokens())
            {
                toAddto.Add(tokenizer.NextToken());
                i++;
            }
            return i;
        }
    }
}