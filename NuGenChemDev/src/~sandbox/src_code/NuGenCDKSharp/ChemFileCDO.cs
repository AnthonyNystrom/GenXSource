/* $RCSfile$
* $Author: egonw $
* $Date: 2006-05-04 01:10:39 +0200 (Thu, 04 May 2006) $
* $Revision: 6153 $
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
using Org.OpenScience.CDK.IO.CML.CDOPI;
using javax.vecmath;
using Org.OpenScience.CDK.Dict;

namespace Org.OpenScience.CDK.IO.CML
{

    /// <summary> CDO object needed as interface with the JCFL library for reading CML
    /// encoded data.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>  Egon Willighagen <egonw@sci.kun.nl>
    /// </author>
    public class ChemFileCDO : IChemFile, IChemicalDocumentObject, System.ICloneable
    {
        virtual public IChemSequence[] ChemSequences
        {
            get
            {
                return currentChemFile.ChemSequences;
            }

        }
        virtual public int ChemSequenceCount
        {
            get
            {
                return currentChemFile.ChemSequenceCount;
            }

        }
        virtual public int ListenerCount
        {
            get
            {
                return currentChemFile.ListenerCount;
            }

        }
        virtual public System.Collections.Hashtable Properties
        {
            get
            {
                return currentChemFile.Properties;
            }

            set
            {
                currentChemFile.Properties = value;
            }

        }
        virtual public System.String ID
        {
            get
            {
                return currentChemFile.ID;
            }

            set
            {
                currentChemFile.ID = value;
            }

        }
        virtual public bool[] Flags
        {
            get
            {
                return currentChemFile.Flags;
            }

            set
            {
                currentChemFile.Flags = value;
            }

        }
        virtual public IChemObjectBuilder Builder
        {
            get
            {
                return currentChemFile.Builder;
            }

        }
        virtual public bool Notification
        {
            get
            {
                return this.doNotification;
            }

            set
            {
                this.doNotification = value;
            }

        }

        private IChemFile currentChemFile;

        private IAtomContainer currentMolecule;
        private ISetOfMolecules currentSetOfMolecules;
        private IChemModel currentChemModel;
        private IChemSequence currentChemSequence;
        private ISetOfReactions currentSetOfReactions;
        private IReaction currentReaction;
        private IAtom currentAtom;
        private System.Collections.Hashtable atomEnumeration;

        private int numberOfAtoms = 0;

        private int bond_a1;
        private int bond_a2;
        private double bond_order;
        private int bond_stereo;
        private System.String bond_id;

        private double crystal_axis_x;
        private double crystal_axis_y;
        private double crystal_axis_z;

        //protected internal LoggingTool logger;

        /// <summary> Basic contructor</summary>
        public ChemFileCDO(IChemFile file)
        {
            //logger = new LoggingTool(this);
            currentChemFile = file;
            currentChemSequence = file.Builder.newChemSequence();
            currentChemModel = file.Builder.newChemModel();
            currentSetOfMolecules = file.Builder.newSetOfMolecules();
            currentSetOfReactions = null;
            currentReaction = null;
            currentMolecule = file.Builder.newMolecule();
            atomEnumeration = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
        }

        // procedures required by CDOInterface

        /// <summary> Procedure required by the CDOInterface. This function is only
        /// supposed to be called by the JCFL library
        /// </summary>
        public virtual void startDocument()
        {
            //logger.info("New CDO Object");
            currentChemSequence = currentChemFile.Builder.newChemSequence();
            currentChemModel = currentChemFile.Builder.newChemModel();
            currentSetOfMolecules = currentChemFile.Builder.newSetOfMolecules();
            currentMolecule = currentChemFile.Builder.newMolecule();
            atomEnumeration = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
        }

        /// <summary> Procedure required by the CDOInterface. This function is only
        /// supposed to be called by the JCFL library
        /// </summary>
        public virtual void endDocument()
        {
            //logger.debug("Closing document");
            if (currentSetOfReactions != null && currentSetOfReactions.ReactionCount == 0 && currentReaction != null)
            {
                //logger.debug("Adding reaction to SetOfReactions");
                currentSetOfReactions.addReaction(currentReaction);
            }
            if (currentSetOfReactions != null && currentChemModel.SetOfReactions == null)
            {
                //logger.debug("Adding SOR to ChemModel");
                currentChemModel.SetOfReactions = currentSetOfReactions;
            }
            if (currentSetOfMolecules != null && currentSetOfMolecules.MoleculeCount != 0)
            {
                //logger.debug("Adding reaction to SetOfMolecules");
                currentChemModel.SetOfMolecules = currentSetOfMolecules;
            }
            if (currentChemSequence.ChemModelCount == 0)
            {
                //logger.debug("Adding ChemModel to ChemSequence");
                currentChemSequence.addChemModel(currentChemModel);
            }
            if (ChemSequenceCount == 0)
            {
                // assume there is one non-animation ChemSequence
                addChemSequence(currentChemSequence);
            }
            //logger.info("End CDO Object");
            //logger.info("Number of sequences:", ChemSequenceCount);
        }

        /// <summary> Procedure required by the CDOInterface. This function is only
        /// supposed to be called by the JCFL library
        /// </summary>
        public virtual void setDocumentProperty(System.String type, System.String value_Renamed)
        {
        }

        /// <summary> Procedure required by the CDOInterface. This function is only
        /// supposed to be called by the JCFL library
        /// </summary>
        public virtual void startObject(System.String objectType)
        {
            //logger.debug("START:" + objectType);
            if (objectType.Equals("Molecule"))
            {
                if (currentChemModel == null)
                    currentChemModel = currentChemFile.Builder.newChemModel();
                if (currentSetOfMolecules == null)
                    currentSetOfMolecules = currentChemFile.Builder.newSetOfMolecules();
                currentMolecule = currentChemFile.Builder.newMolecule();
            }
            else if (objectType.Equals("Atom"))
            {
                currentAtom = currentChemFile.Builder.newAtom("H");
                //logger.debug("Atom # " + numberOfAtoms);
                numberOfAtoms++;
            }
            else if (objectType.Equals("Bond"))
            {
                bond_id = null;
                bond_stereo = -99;
            }
            else if (objectType.Equals("Animation"))
            {
                currentChemSequence = currentChemFile.Builder.newChemSequence();
            }
            else if (objectType.Equals("Frame"))
            {
                currentChemModel = currentChemFile.Builder.newChemModel();
            }
            else if (objectType.Equals("SetOfMolecules"))
            {
                currentSetOfMolecules = currentChemFile.Builder.newSetOfMolecules();
                currentMolecule = currentChemFile.Builder.newMolecule();
            }
            else if (objectType.Equals("Crystal"))
            {
                currentMolecule = currentChemFile.Builder.newCrystal(currentMolecule);
            }
            else if (objectType.Equals("a-axis") || objectType.Equals("b-axis") || objectType.Equals("c-axis"))
            {
                crystal_axis_x = 0.0;
                crystal_axis_y = 0.0;
                crystal_axis_z = 0.0;
            }
            else if (objectType.Equals("SetOfReactions"))
            {
                currentSetOfReactions = currentChemFile.Builder.newSetOfReactions();
            }
            else if (objectType.Equals("Reaction"))
            {
                if (currentSetOfReactions == null)
                    startObject("SetOfReactions");
                currentReaction = currentChemFile.Builder.newReaction();
            }
            else if (objectType.Equals("Reactant"))
            {
                if (currentReaction == null)
                    startObject("Reaction");
                currentMolecule = currentChemFile.Builder.newMolecule();
            }
            else if (objectType.Equals("Product"))
            {
                if (currentReaction == null)
                    startObject("Reaction");
                currentMolecule = currentChemFile.Builder.newMolecule();
            }
        }

        /// <summary> Procedure required by the CDOInterface. This function is only
        /// supposed to be called by the JCFL library
        /// </summary>
        public virtual void endObject(System.String objectType)
        {
            //logger.debug("END: " + objectType);
            if (objectType.Equals("Molecule"))
            {
                if (currentMolecule is IMolecule)
                {
                    //logger.debug("Adding molecule to set");
                    currentSetOfMolecules.addMolecule((IMolecule)currentMolecule);
                    //logger.debug("#mols in set: " + currentSetOfMolecules.MoleculeCount);
                }
                else if (currentMolecule is ICrystal)
                {
                    //logger.debug("Adding crystal to chemModel");
                    currentChemModel.Crystal = (ICrystal)currentMolecule;
                    currentChemSequence.addChemModel(currentChemModel);
                }
            }
            else if (objectType.Equals("SetOfMolecules"))
            {
                currentChemModel.SetOfMolecules = currentSetOfMolecules;
                currentChemSequence.addChemModel(currentChemModel);
            }
            else if (objectType.Equals("Frame"))
            {
                // endObject("Molecule");
            }
            else if (objectType.Equals("Animation"))
            {
                addChemSequence(currentChemSequence);
                //logger.info("This file has " + ChemSequenceCount + " sequence(s).");
            }
            else if (objectType.Equals("Atom"))
            {
                currentMolecule.addAtom(currentAtom);
            }
            else if (objectType.Equals("Bond"))
            {
                //logger.debug("Bond(" + bond_id + "): " + bond_a1 + ", " + bond_a2 + ", " + bond_order);
                if (bond_a1 > currentMolecule.AtomCount || bond_a2 > currentMolecule.AtomCount)
                {
                    //logger.error("Cannot add bond between at least one non-existant atom: " + bond_a1 + " and " + bond_a2);
                }
                else
                {
                    IAtom a1 = currentMolecule.getAtomAt(bond_a1);
                    IAtom a2 = currentMolecule.getAtomAt(bond_a2);
                    IBond b = currentChemFile.Builder.newBond(a1, a2, bond_order);
                    if (bond_id != null)
                        b.ID = bond_id;
                    if (bond_stereo != -99)
                    {
                        b.Stereo = bond_stereo;
                    }
                    if (bond_order == CDKConstants.BONDORDER_AROMATIC)
                    {
                        b.setFlag(CDKConstants.ISAROMATIC, true);
                    }
                    currentMolecule.addBond(b);
                }
            }
            else if (objectType.Equals("a-axis"))
            {
                // set these variables
                if (currentMolecule is ICrystal)
                {
                    ICrystal current = (ICrystal)currentMolecule;
                    current.A = new Vector3d(crystal_axis_x, crystal_axis_y, crystal_axis_z);
                }
                else
                {
                    //logger.warn("Current object is not a crystal");
                }
            }
            else if (objectType.Equals("b-axis"))
            {
                if (currentMolecule is ICrystal)
                {
                    ICrystal current = (ICrystal)currentMolecule;
                    current.B = new Vector3d(crystal_axis_x, crystal_axis_y, crystal_axis_z);
                }
                else
                {
                    //logger.warn("Current object is not a crystal");
                }
            }
            else if (objectType.Equals("c-axis"))
            {
                if (currentMolecule is ICrystal)
                {
                    ICrystal current = (ICrystal)currentMolecule;
                    current.C = new Vector3d(crystal_axis_x, crystal_axis_y, crystal_axis_z);
                }
                else
                {
                    //logger.warn("Current object is not a crystal");
                }
            }
            else if (objectType.Equals("SetOfReactions"))
            {
                currentChemModel.SetOfReactions = currentSetOfReactions;
                currentChemSequence.addChemModel(currentChemModel);
                /* FIXME: this should be when document is closed! */
            }
            else if (objectType.Equals("Reaction"))
            {
                //logger.debug("Adding reaction to SOR");
                currentSetOfReactions.addReaction(currentReaction);
            }
            else if (objectType.Equals("Reactant"))
            {
                currentReaction.addReactant((IMolecule)currentMolecule);
            }
            else if (objectType.Equals("Product"))
            {
                currentReaction.addProduct((IMolecule)currentMolecule);
            }
            else if (objectType.Equals("Crystal"))
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                //logger.debug("Crystal: " + currentMolecule);
            }
        }

        /// <summary> Procedure required by the CDOInterface. This function is only
        /// supposed to be called by the JCFL library
        /// </summary>
        public virtual void setObjectProperty(System.String objectType, System.String propertyType, System.String propertyValue)
        {
            //logger.debug("objectType: " + objectType);
            //logger.debug("propType: " + propertyType);
            //logger.debug("property: " + propertyValue);

            if (objectType == null)
            {
                //logger.error("Cannot add property for null object");
                return;
            }
            if (propertyType == null)
            {
                //logger.error("Cannot add property for null property type");
                return;
            }
            if (propertyValue == null)
            {
                //logger.warn("Will not add null property");
                return;
            }

            if (objectType.Equals("Molecule"))
            {
                if (propertyType.Equals("id"))
                {
                    currentMolecule.ID = propertyValue;
                }
                else if (propertyType.Equals("inchi"))
                {
                    currentMolecule.setProperty("iupac.nist.chemical.identifier", propertyValue);
                }
                else if (propertyType.Equals("pdb:residueName"))
                {
                    currentMolecule.setProperty(new DictRef(propertyType, propertyValue), propertyValue);
                }
                else if (propertyType.Equals("pdb:oneLetterCode"))
                {
                    currentMolecule.setProperty(new DictRef(propertyType, propertyValue), propertyValue);
                }
                else if (propertyType.Equals("pdb:id"))
                {
                    currentMolecule.setProperty(new DictRef(propertyType, propertyValue), propertyValue);
                }
                else
                {
                    //logger.warn("Not adding molecule property!");
                }
            }
            else if (objectType.Equals("PseudoAtom"))
            {
                if (propertyType.Equals("label"))
                {
                    if (!(currentAtom is IPseudoAtom))
                    {
                        currentAtom = currentChemFile.Builder.newPseudoAtom(currentAtom);
                    }
                    ((IPseudoAtom)currentAtom).Label = propertyValue;
                }
            }
            else if (objectType.Equals("Atom"))
            {
                if (propertyType.Equals("type"))
                {
                    if (propertyValue.Equals("R") && !(currentAtom is IPseudoAtom))
                    {
                        currentAtom = currentChemFile.Builder.newPseudoAtom(currentAtom);
                    }
                    currentAtom.Symbol = propertyValue;
                }
                else if (propertyType.Equals("x2"))
                {
                    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    currentAtom.X2d = System.Double.Parse(propertyValue);
                }
                else if (propertyType.Equals("y2"))
                {
                    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    currentAtom.Y2d = System.Double.Parse(propertyValue);
                }
                else if (propertyType.Equals("x3"))
                {
                    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    currentAtom.X3d = System.Double.Parse(propertyValue);
                }
                else if (propertyType.Equals("y3"))
                {
                    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    currentAtom.Y3d = System.Double.Parse(propertyValue);
                }
                else if (propertyType.Equals("z3"))
                {
                    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    currentAtom.Z3d = System.Double.Parse(propertyValue);
                }
                else if (propertyType.Equals("xFract"))
                {
                    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    currentAtom.FractX3d = System.Double.Parse(propertyValue);
                }
                else if (propertyType.Equals("yFract"))
                {
                    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    currentAtom.FractY3d = System.Double.Parse(propertyValue);
                }
                else if (propertyType.Equals("zFract"))
                {
                    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    currentAtom.FractZ3d = System.Double.Parse(propertyValue);
                }
                else if (propertyType.Equals("formalCharge"))
                {
                    currentAtom.setFormalCharge(System.Int32.Parse(propertyValue));
                }
                else if (propertyType.Equals("charge") || propertyType.Equals("partialCharge"))
                {
                    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    currentAtom.setCharge(System.Double.Parse(propertyValue));
                }
                else if (propertyType.Equals("hydrogenCount"))
                {
                    currentAtom.setHydrogenCount(System.Int32.Parse(propertyValue));
                }
                else if (propertyType.Equals("dictRef"))
                {
                    currentAtom.setProperty("org.openscience.cdk.dict", propertyValue);
                }
                else if (propertyType.Equals("atomicNumber"))
                {
                    currentAtom.AtomicNumber = System.Int32.Parse(propertyValue);
                }
                else if (propertyType.Equals("massNumber"))
                {
                    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    currentAtom.MassNumber = (int)(System.Double.Parse(propertyValue));
                }
                else if (propertyType.Equals("spinMultiplicity"))
                {
                    int unpairedElectrons = System.Int32.Parse(propertyValue) - 1;
                    for (int i = 0; i < unpairedElectrons; i++)
                    {
                        currentMolecule.addElectronContainer(currentChemFile.Builder.newSingleElectron(currentAtom));
                    }
                }
                else if (propertyType.Equals("id"))
                {
                    //logger.debug("id: ", propertyValue);
                    currentAtom.ID = propertyValue;
                    atomEnumeration[propertyValue] = (System.Int32)numberOfAtoms;
                }
            }
            else if (objectType.Equals("Bond"))
            {
                if (propertyType.Equals("atom1"))
                {
                    bond_a1 = System.Int32.Parse(propertyValue);
                }
                else if (propertyType.Equals("atom2"))
                {
                    bond_a2 = System.Int32.Parse(propertyValue);
                }
                else if (propertyType.Equals("id"))
                {
                    //logger.debug("id: " + propertyValue);
                    bond_id = propertyValue;
                }
                else if (propertyType.Equals("order"))
                {
                    try
                    {
                        bond_order = System.Double.Parse(propertyValue);
                    }
                    catch (System.Exception e)
                    {
                        //logger.error("Cannot convert to double: " + propertyValue);
                        bond_order = 1.0;
                    }
                }
                else if (propertyType.Equals("stereo"))
                {
                    if (propertyValue.Equals("H"))
                    {
                        bond_stereo = CDKConstants.STEREO_BOND_DOWN;
                    }
                    else if (propertyValue.Equals("W"))
                    {
                        bond_stereo = CDKConstants.STEREO_BOND_UP;
                    }
                }
            }
            else if (objectType.Equals("Reaction"))
            {
                if (propertyType.Equals("id"))
                {
                    currentReaction.ID = propertyValue;
                }
            }
            else if (objectType.Equals("SetOfReactions"))
            {
                if (propertyType.Equals("id"))
                {
                    currentSetOfReactions.ID = propertyValue;
                }
            }
            else if (objectType.Equals("Reactant"))
            {
                if (propertyType.Equals("id"))
                {
                    currentMolecule.ID = propertyValue;
                }
            }
            else if (objectType.Equals("Product"))
            {
                if (propertyType.Equals("id"))
                {
                    currentMolecule.ID = propertyValue;
                }
            }
            else if (objectType.Equals("Crystal"))
            {
                // set these variables
                if (currentMolecule is ICrystal)
                {
                    ICrystal current = (ICrystal)currentMolecule;
                    if (propertyType.Equals("spacegroup"))
                    {
                        //logger.debug("Setting crystal spacegroup to: " + propertyValue);
                        current.SpaceGroup = propertyValue;
                    }
                    else if (propertyType.Equals("z"))
                    {
                        try
                        {
                            //logger.debug("Setting z to: " + propertyValue);
                            current.Z = System.Int32.Parse(propertyValue);
                        }
                        catch (System.FormatException exception)
                        {
                            //logger.error("Error in format of Z value");
                        }
                    }
                }
                else
                {
                    //logger.warn("Cannot add crystal cell parameters to a non " + "Crystal class!");
                }
            }
            else if (objectType.Equals("a-axis") || objectType.Equals("b-axis") || objectType.Equals("c-axis"))
            {
                // set these variables
                if (currentMolecule is ICrystal)
                {
                    //logger.debug("Setting axis (" + objectType + "): " + propertyValue);
                    if (propertyType.Equals("x"))
                    {
                        crystal_axis_x = System.Double.Parse(propertyValue);
                    }
                    else if (propertyType.Equals("y"))
                    {
                        crystal_axis_y = System.Double.Parse(propertyValue);
                    }
                    else if (propertyType.Equals("z"))
                    {
                        crystal_axis_z = System.Double.Parse(propertyValue);
                    }
                }
                else
                {
                    //logger.warn("Cannot add crystal cell parameters to a non " + "Crystal class!");
                }
            }
            //logger.debug("Object property set...");
        }

        /// <summary> Procedure required by the CDOInterface. This function is only
        /// supposed to be called by the JCFL library
        /// </summary>
        public virtual CDOAcceptedObjects acceptObjects()
        {
            CDOAcceptedObjects objects = new CDOAcceptedObjects();
            objects.add("Molecule");
            objects.add("Fragment");
            objects.add("Atom");
            objects.add("Bond");
            objects.add("Animation");
            objects.add("Frame");
            objects.add("Crystal");
            objects.add("a-axis");
            objects.add("b-axis");
            objects.add("c-axis");
            objects.add("SetOfReactions");
            objects.add("Reactions");
            objects.add("Reactant");
            objects.add("Product");
            return objects;
        }

        public virtual void addChemSequence(IChemSequence chemSequence)
        {
            currentChemFile.addChemSequence(chemSequence);
        }

        public virtual IChemSequence getChemSequence(int number)
        {
            return currentChemFile.getChemSequence(number);
        }

        public virtual void addListener(IChemObjectListener col)
        {
            currentChemFile.addListener(col);
        }

        public virtual void removeListener(IChemObjectListener col)
        {
            currentChemFile.removeListener(col);
        }

        public virtual void notifyChanged()
        {
            currentChemFile.notifyChanged();
        }

        public virtual void notifyChanged(IChemObjectChangeEvent evt)
        {
            currentChemFile.notifyChanged(evt);
        }

        public virtual void setProperty(System.Object description, System.Object property)
        {
            currentChemFile.setProperty(description, property);
        }

        public virtual void removeProperty(System.Object description)
        {
            currentChemFile.removeProperty(description);
        }

        public virtual System.Object getProperty(System.Object description)
        {
            return currentChemFile.getProperty(description);
        }

        public virtual void setFlag(int flag_type, bool flag_value)
        {
            currentChemFile.setFlag(flag_type, flag_value);
        }

        public virtual bool getFlag(int flag_type)
        {
            return currentChemFile.getFlag(flag_type);
        }

        public virtual System.Object Clone()
        {
            return currentChemFile.Clone();
        }

        private bool doNotification = true;
    }
}