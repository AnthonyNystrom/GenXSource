/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-06-06 10:55:35 +0200 (Tue, 06 Jun 2006) $
* $Revision: 6346 $
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
*/
using System;
using Org.OpenScience.CDK.IO.Formats;
using Org.OpenScience.CDK.IO.Setting;
using System.Text.RegularExpressions;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Exception;
using Org.OpenScience.CDK.Tools.Manipulator;
using Org.OpenScience.CDK.Dict;
using Support;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Reads an export from the MACiE enzyme reaction database.
    /// Information about this database can be obtained from
    /// Gemma Holiday, Cambridge University, UK, and Gail Bartlett,
    /// European Bioinformatics Institute, Hinxton, UK.
    /// 
    /// <p>This implementation is based on a dump from their database
    /// on 2003-07-14.
    /// 
    /// </summary>
    /// <cdk.module>  experimental </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>      Egon Willighagen
    /// </author>
    /// <cdk.created>     2003-07-24 </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>     file format, MACiE RDF </cdk.keyword>
    /// <cdk.require>  java1.4+ </cdk.require>
    public class MACiEReader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new MACiEFormat();
            }

        }
        override public IOSetting[] IOSettings
        {
            get
            {
                IOSetting[] settings = new IOSetting[3];
                settings[0] = selectedEntry;
                settings[1] = readSecondaryFiles;
                settings[2] = readSecondaryDir;
                return settings;
            }

        }

        /// <summary>Property it will put on ChemModel </summary>
        public const System.String CreationDate = "org.openscience.cdk.io.MACiE.CreationDate";
        /// <summary>Property it will put on ChemModel </summary>
        public const System.String MedlineID = "org.openscience.cdk.io.MACiE.MedlineID";
        /// <summary>Property it will put on ChemModel </summary>
        public const System.String PDBCode = "org.openscience.cdk.io.MACiE.PDBCode";
        /// <summary>Property it will put on ChemModel </summary>
        public const System.String ECNumber = "org.openscience.cdk.io.MACiE.ECNumber";
        /// <summary>Property it will put on ChemModel </summary>
        public const System.String EnzymeName = "org.openscience.cdk.io.MACiE.EnzymeName";

        private System.IO.StreamReader input = null;
        //private LoggingTool //logger = null;

        private IntegerIOSetting selectedEntry;
        private BooleanIOSetting readSecondaryFiles;
        private StringIOSetting readSecondaryDir;

        private Regex topLevelDatum;
        private Regex subLevelDatum;
        private Regex annotationTuple;
        private Regex residueLocator;

        private ChemModel currentEntry;
        private Reaction currentReaction;
        private SetOfReactions currentReactionStepSet;

        private System.String reactionStepAnnotation;
        private System.String reactionStepComments;

        private bool readThisEntry = true;

        /// <summary> Contructs a new MACiEReader that can read Molecule from a given Reader.
        /// 
        /// </summary>
        /// <param name="in"> The Reader to read from
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public MACiEReader(System.IO.StreamReader in_Renamed)
            : this()
        {
            this.input = new System.IO.StreamReader(in_Renamed.BaseStream, in_Renamed.CurrentEncoding);
        }

        public MACiEReader(System.IO.Stream input)
            : this(new System.IO.StreamReader(input, System.Text.Encoding.Default))
        {
        }

        public MACiEReader()
        {
            //logger = new LoggingTool(this);

            /* compile patterns */
            topLevelDatum = new Regex("(.+):(.+)");
            subLevelDatum = new Regex("(.+):(.+)\\((.+)\\):(.+)");
            annotationTuple = new Regex("(\\w+)=\\((.+?)\\);(.*)");
            residueLocator = new Regex("[A-Z][a-z][a-z]\\d{1,5}"); // e.g. Lys150

            initIOSettings();
        }

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public override void setReader(System.IO.StreamReader input)
        {
            if (input is System.IO.StreamReader)
            {
                this.input = (System.IO.StreamReader)input;
            }
            else
            {
                this.input = new System.IO.StreamReader(input.BaseStream, input.CurrentEncoding);
            }
        }

        public override void setReader(System.IO.Stream input)
        {
            setReader(new System.IO.StreamReader(input, System.Text.Encoding.Default));
        }

        public override bool accepts(System.Type classObject)
        {
            System.Type[] interfaces = classObject.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (typeof(IChemModel).Equals(interfaces[i]))
                    return true;
                if (typeof(IChemFile).Equals(interfaces[i]))
                    return true;
                if (typeof(IChemSequence).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        /// <summary> Takes an object which subclasses IChemObject, e.g. Molecule, and will read
        /// this (from file, database, internet etc). If the specific implementation
        /// does not support a specific IChemObject it will throw an Exception.
        /// 
        /// </summary>
        /// <param name="object">The object that subclasses IChemObject
        /// </param>
        /// <returns>        The IChemObject read
        /// </returns>
        /// <exception cref="CDKException">
        /// </exception>
        public override IChemObject read(IChemObject object_Renamed)
        {
            customizeJob();

            try
            {
                if (object_Renamed is IChemSequence)
                {
                    return readReactions(false);
                }
                else if (object_Renamed is IChemModel)
                {
                    return readReactions(true);
                }
                else if (object_Renamed is IChemFile)
                {
                    IChemFile chemFile = object_Renamed.Builder.newChemFile();
                    chemFile.addChemSequence((ChemSequence)readReactions(false));
                    return chemFile;
                }
            }
            catch (System.IO.IOException exception)
            {
                //UPGRADE_ISSUE: Method 'java.io.LineNumberReader.getLineNumber' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioLineNumberReadergetLineNumber'"
                System.String message = "Error while reading file, line number: ";// +input.getLineNumber();
                //logger.error(message);
                //logger.debug(exception);
                throw new CDKException(message, exception);
            }
            throw new CDKException("Only supported are ChemSequence and ChemModel.");
        }

        public virtual bool accepts(IChemObject object_Renamed)
        {
            if (object_Renamed is ChemSequence)
            {
                return true;
            }
            else if (object_Renamed is ChemModel)
            {
                return true;
            }
            else if (object_Renamed is ChemFile)
            {
                return true;
            }
            else if (object_Renamed == null)
            {
                //logger.warn("MACiEReader can not read null objects.");
            }
            else
            {
                //logger.warn("MACiEReader can not read IChemObject of type: ", object_Renamed.GetType().FullName);
            }
            return false;
        }



        /// <summary> Read a Reaction from a file in MACiE RDF format.
        /// 
        /// </summary>
        /// <returns>  The Reaction that was read from the MDL file.
        /// </returns>
        private IChemObject readReactions(bool selectEntry)
        {
            ChemSequence entries = new ChemSequence();
            currentEntry = null;
            int entryCounter = 0;
            currentReactionStepSet = null;

            while (input.Peek() != -1)
            {
                //UPGRADE_WARNING: Method 'java.io.LineNumberReader.readLine' was converted to 'System.IO.StreamReader.ReadLine' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
                System.String line = input.ReadLine();
                if (line.StartsWith("$RDFILE"))
                {
                    entries = new ChemSequence();
                }
                else if (line.StartsWith("$DATM"))
                {
                    entries.setProperty(CreationDate, line.Substring(7));
                }
                else if (line.StartsWith("$RIREG"))
                {
                    // new entry, store previous entry if any
                    if (currentEntry != null)
                    {
                        // store previous entry
                        currentEntry.SetOfReactions = currentReactionStepSet;
                        createNiceMACiETitle(currentEntry);
                        entries.addChemModel(currentEntry);
                        fireFrameRead();
                        if (selectEntry && (entryCounter == selectedEntry.SettingValue))
                        {
                            //logger.info("Starting reading wanted frame: ", selectedEntry);
                            return currentEntry;
                        }
                        else
                        {
                            //logger.debug("Not reading unwanted frame: " + entryCounter);
                        }
                    }
                    currentEntry = new ChemModel();
                    entryCounter++;
                    if (!selectEntry || entryCounter == selectedEntry.SettingValue)
                    {
                        readThisEntry = true;
                    }
                    else
                    {
                        readThisEntry = false;
                    }
                    currentReactionStepSet = new SetOfReactions();
                }
                else if (line.StartsWith("$DTYPE"))
                {
                    System.String[] tuple = readDtypeDatumTuple(line);
                    System.String dataType = tuple[0];
                    System.String datum = tuple[1];

                    // now some regular expression wizardry
                    Match subLevelMatcher = subLevelDatum.Match(dataType);
                    if (subLevelMatcher.Success)
                    {
                        // sub level field found
                        System.String field = subLevelMatcher.Groups[2].Value;
                        System.String fieldNumber = subLevelMatcher.Groups[3].Value;
                        System.String subfield = subLevelMatcher.Groups[4].Value;
                        processSubLevelField(field, fieldNumber, subfield, datum);
                    }
                    else
                    {
                        Match topLevelMatcher = topLevelDatum.Match(dataType);
                        if (topLevelMatcher.Success)
                        {
                            // top level field found
                            System.String field = topLevelMatcher.Groups[2].Value;
                            processTopLevelField(field, datum);
                        }
                        else
                        {
                            //UPGRADE_ISSUE: Method 'java.io.LineNumberReader.getLineNumber' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioLineNumberReadergetLineNumber'"
                            //logger.error("Could not parse datum tuple of type ", dataType, " around line " + input.getLineNumber());
                        }
                    }
                }
                else
                {
                    //UPGRADE_ISSUE: Method 'java.io.LineNumberReader.getLineNumber' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioLineNumberReadergetLineNumber'"
                    //logger.warn("Unrecognized command on line " + input.getLineNumber(), ": ", line);
                }
            }

            if (currentEntry != null)
            {
                createNiceMACiETitle(currentEntry);
                // store last entry
                currentEntry.SetOfReactions = currentReactionStepSet;
                entries.addChemModel(currentEntry);
                fireFrameRead();
            }

            if (selectEntry)
            {
                // apparently selected last one, other already returned
                return currentEntry;
            }
            return entries;
        }

        private void createNiceMACiETitle(ChemModel chemModel)
        {
            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            chemModel.setProperty(CDKConstants.TITLE, "MACIE " + currentEntry.getProperty(EnzymeName) + "= " + "PDB: " + currentEntry.getProperty(PDBCode) + ", " + "EC: " + currentEntry.getProperty(ECNumber));
        }

        private System.String[] readDtypeDatumTuple(System.String triggerLine)
        {
            System.String dTypeLine = triggerLine;
            //UPGRADE_WARNING: Method 'java.io.LineNumberReader.readLine' was converted to 'System.IO.StreamReader.ReadLine' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
            System.String datumLine = input.ReadLine();
            System.String type = dTypeLine.Substring(7);
            System.String datum = datumLine.Substring(7);
            //logger.debug("Tuple TYPE: ", type);
            System.String line = datum;
            if (datum.EndsWith("$MFMT"))
            {
                // deal with MDL mol content
                System.Text.StringBuilder fullDatum = new System.Text.StringBuilder();
                do
                {
                    //UPGRADE_WARNING: Method 'java.io.LineNumberReader.readLine' was converted to 'System.IO.StreamReader.ReadLine' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
                    line = input.ReadLine();
                    fullDatum.Append(line);
                }
                while (!(line.Equals("M  END")));
                datum = fullDatum.ToString();
            }
            else if (datum.EndsWith("+") && (datum.Length >= 74))
            {
                // deal with multiline fields
                System.Text.StringBuilder fullDatum = new System.Text.StringBuilder();
                fullDatum.Append(datum.Substring(0, (datum.Length - 1) - (0)));
                do
                {
                    //UPGRADE_WARNING: Method 'java.io.LineNumberReader.readLine' was converted to 'System.IO.StreamReader.ReadLine' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
                    line = input.ReadLine();
                    if (line.Length > 0)
                        fullDatum.Append(line.Substring(0, (line.Length - 1) - (0)));
                }
                while (line.EndsWith("+"));
                datum = fullDatum.ToString();
            }
            //logger.debug("     DATUM: ", datum);
            System.String[] tuple = new System.String[2];
            tuple[0] = type;
            tuple[1] = datum;
            return tuple;
        }

        private void processTopLevelField(System.String field, System.String datum)
        {
            //logger.debug("Processing top level field");
            if (field.Equals("UNIQUE IDENTIFIER"))
            {
                currentEntry.ID = "MACIE-" + datum;
            }
            else if (field.Equals("EC NUMBER"))
            {
                currentEntry.setProperty(ECNumber, datum);
            }
            else if (field.Equals("PDB CODE"))
            {
                currentEntry.setProperty(PDBCode, datum);
            }
            else if (field.Equals("ENZYME NAME"))
            {
                currentEntry.setProperty(EnzymeName, datum);
            }
            else
            {
                //UPGRADE_ISSUE: Method 'java.io.LineNumberReader.getLineNumber' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioLineNumberReadergetLineNumber'"
                //logger.warn("Unrecognized ROOT field ", field, " around line " + input.getLineNumber());
            }
        }

        private void processSubLevelField(System.String field, System.String fieldNumber, System.String subfield, System.String datum)
        {
            //logger.debug("Processing sub level field");
            if (field.Equals("OVERALL REACTION"))
            {
                if (subfield.Equals("REACTION_ID"))
                {
                    if (readSecondaryFiles.Set && readThisEntry)
                    {
                        // parse referenced file
                        System.String filename = readSecondaryDir.getSetting() + datum + ".rxn";
                        System.IO.FileInfo file = new System.IO.FileInfo(filename);
                        bool tmpBool;
                        if (System.IO.File.Exists(file.FullName))
                            tmpBool = true;
                        else
                            tmpBool = System.IO.Directory.Exists(file.FullName);
                        if (tmpBool)
                        {
                            //logger.info("Reading overall reaction from: ", filename);
                            //UPGRADE_TODO: Constructor 'java.io.FileReader.FileReader' was converted to 'System.IO.StreamReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                            System.IO.StreamReader reader = new System.IO.StreamReader(file.FullName, System.Text.Encoding.Default);
                            IChemObjectReader rxnReader = new ReaderFactory().createReader(reader);
                            currentReaction = (Reaction)rxnReader.read(new Reaction());
                            currentReaction.ID = datum;
                            currentReaction.setProperty(CDKConstants.TITLE, "Overall Reaction");
                            // don't add it now, wait until annotation is parsed
                        }
                        else
                        {
                            System.String error = "Cannot find secondary file: " + filename;
                            //logger.error(error);
                            throw new CDKException(error);
                        }
                    }
                    else
                    {
                        //logger.warn("Not reading overall reaction for this entry");
                    }
                }
                else if (subfield.Equals("OVERALL REACTION ANNOTATION"))
                {
                    parseReactionAnnotation(datum, currentReaction);
                    currentReactionStepSet.addReaction(currentReaction);
                }
            }
            else if (field.Equals("REACTION STAGES"))
            {
                if (subfield.Equals("REACTION STAGES"))
                {
                    // new reaction step
                    // cannot create one, because CDK io does not
                    // allow that (yet)
                    reactionStepAnnotation = null;
                    reactionStepComments = null;
                }
                else if (subfield.Equals("ANNOTATION"))
                {
                    reactionStepAnnotation = datum;
                }
                else if (subfield.Equals("COMMENTS"))
                {
                    reactionStepComments = datum;
                }
                else if (subfield.Equals("STEP_ID"))
                {
                    // read secondary RXN files?
                    if (readSecondaryFiles.Set && readThisEntry)
                    {
                        // parse referenced file
                        System.String filename = readSecondaryDir.getSetting() + datum + ".rxn";
                        System.IO.FileInfo file = new System.IO.FileInfo(filename);
                        bool tmpBool2;
                        if (System.IO.File.Exists(file.FullName))
                            tmpBool2 = true;
                        else
                            tmpBool2 = System.IO.Directory.Exists(file.FullName);
                        if (tmpBool2)
                        {
                            //logger.info("Reading reaction step from: ", filename);
                            //UPGRADE_TODO: Constructor 'java.io.FileReader.FileReader' was converted to 'System.IO.StreamReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
                            System.IO.StreamReader reader = new System.IO.StreamReader(file.FullName, System.Text.Encoding.Default);
                            IChemObjectReader rxnReader = new ReaderFactory().createReader(reader);
                            currentReaction = (Reaction)rxnReader.read(new Reaction());
                            currentReaction.ID = datum;
                            currentReaction.setProperty(CDKConstants.TITLE, "Step " + fieldNumber);
                        }
                        else
                        {
                            //logger.error("Cannot find secondary file: ", filename);
                        }
                        // convert PseudoAtom's in EnzymeResidueLocator's if appropriate
                        markEnzymeResidueLocatorAtoms(currentReaction);
                        // now parse annotation
                        if (reactionStepAnnotation != null)
                        {
                            parseReactionAnnotation(reactionStepAnnotation, currentReaction);
                        }
                        // and set comments
                        if (reactionStepComments != null)
                        {
                            currentReaction.setProperty(CDKConstants.COMMENT, reactionStepComments);
                        }
                        // now, I'm ready to add reaction
                        currentReactionStepSet.addReaction(currentReaction);
                    }
                    else
                    {
                        //logger.warn("Not reading reactions of this entry.");
                    }
                }
            }
            else if (field.Equals("SUBSTRATES"))
            {
                //logger.warn("Ignoring top level definition of substrates");
            }
            else if (field.Equals("PRODUCTS"))
            {
                //logger.warn("Ignoring top level definition of products");
            }
            else if (field.Equals("REFERENCES"))
            {
                if (subfield.Equals("MEDLINE_ID"))
                {
                    currentEntry.setProperty(MedlineID, datum);
                }
            }
            else
            {
                //UPGRADE_ISSUE: Method 'java.io.LineNumberReader.getLineNumber' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioLineNumberReadergetLineNumber'"
                //logger.error("Unrecognized sub level field ", field, " around line " + input.getLineNumber());
            }
        }

        private void markEnzymeResidueLocatorAtoms(Reaction currentReaction)
        {
            IAtom[] atoms = ReactionManipulator.getAllInOneContainer(currentReaction).Atoms;
            for (int i = 0; i < atoms.Length; i++)
            {
                if (atoms[i] is EnzymeResidueLocator)
                {
                    // skip atom
                }
                else if (atoms[i] is PseudoAtom)
                {
                    PseudoAtom pseudo = (PseudoAtom)atoms[i];
                    //logger.debug("pseudo atom label: ", pseudo.Label);
                    //logger.debug("pseudo class: ", pseudo.GetType().FullName);
                    Match residueLocatorMatcher = residueLocator.Match(pseudo.Label);
                    if (residueLocatorMatcher.Success)
                    {
                        //logger.debug("Found residueLocator: ", pseudo.Label);
                        // replace atom with enzymeResidueLocator
                        IAtomContainer container = ReactionManipulator.getRelevantAtomContainer(currentReaction, pseudo);
                        //logger.debug("Replacing the pseudo atom with a ezymeResidueLocator atom");
                        AtomContainerManipulator.replaceAtomByAtom(container, pseudo, new EnzymeResidueLocator(pseudo));
                    }
                }
            }
        }

        private void parseReactionAnnotation(System.String annotation, Reaction reaction)
        {
            //logger.debug("Parsing annotation...");
            Match annotationTupleMatcher = annotationTuple.Match(annotation);
            while (annotationTupleMatcher.Success)
            {
                System.String field = annotationTupleMatcher.Groups[1].Value;
                System.String value_Renamed = annotationTupleMatcher.Groups[2].Value;
                processAnnotation(field, value_Renamed, reaction);
                // eat next part of annotation
                System.String remainder = annotationTupleMatcher.Groups[3].Value;
                annotationTupleMatcher = annotationTuple.Match(remainder);
            }
        }

        private void processAnnotation(System.String field, System.String value_Renamed, Reaction reaction)
        {
            //logger.debug("Annote: ", field, "=", value_Renamed);
            if (field.Equals("RxnAtts") || field.Equals("RxnType"))
            {
                // reaction attributes
                System.String dictionary = "macie";
                if (value_Renamed.Equals("Acid") || value_Renamed.Equals("Base"))
                {
                    dictionary = "chemical";
                }
                addDictRefedAnnotation(reaction, "Attributes", value_Renamed);
            }
            else if (field.Equals("ResiduesPresent") || field.Equals("GroupTransferred") || field.Equals("BondFormed") || field.Equals("ReactiveCentres") || field.Equals("BondCleaved") || field.Equals("BondFormed") || field.Equals("Products") || field.Equals("ResiduesPresent"))
            {
                reaction.setProperty(new DictRef("macie:" + field, value_Renamed), value_Renamed);
            }
            else if (field.Equals("Reversible"))
            {
                if (value_Renamed.ToUpper().Equals("yes".ToUpper()))
                {
                    reaction.Direction = IReaction_Fields.BIDIRECTIONAL;
                    addDictRefedAnnotation(reaction, "ReactionType", "ReversibleReaction");
                }
            }
            else if (field.Equals("OverallReactionType"))
            {
                SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(value_Renamed, ",");
                int i = 0;
                while (tokenizer.HasMoreTokens())
                {
                    System.String token = tokenizer.NextToken();
                    i++;
                    reaction.setProperty(DictionaryDatabase.DICTREFPROPERTYNAME + ":field:overallReactionType:" + i, "macie:" + token.ToLower());
                }
            }
            else
            {
                Match residueLocatorMatcher = residueLocator.Match(field);
                if (residueLocatorMatcher.Success)
                {
                    //logger.debug("Found residueLocator: ", field);
                    IAtom[] atoms = ReactionManipulator.getAllInOneContainer(reaction).Atoms;
                    bool found = false;
                    //logger.debug("Searching for given residueLocator through #atom: ", atoms.Length);
                    //logger.debug("Taken from reaction ", reaction.ID);
                    for (int i = 0; (i < atoms.Length && !found); i++)
                    {
                        if (atoms[i] is PseudoAtom)
                        {
                            // that is what we are looking for
                            PseudoAtom atom = (PseudoAtom)atoms[i];
                            if (atom.Label.Equals(field))
                            {
                                // we have a hit, now mark Atom with dict refs
                                addDictRefedAnnotation(atom, "ResidueRole", value_Renamed);
                                found = true;
                            }
                        }
                    }
                    if (!found)
                    {
                        //logger.error("MACiE annotation mentions a residue that does not exist: " + field);
                    }
                }
                else
                {
                    //logger.error("Did not parse annotation: ", field);
                }
            }
        }

        private void addDictRefedAnnotation(IChemObject object_Renamed, System.String type, System.String values)
        {
            SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(values, ",");
            while (tokenizer.HasMoreTokens())
            {
                System.String token = tokenizer.NextToken();
                object_Renamed.setProperty(new DictRef("macie:" + type, token), token);
                //logger.debug("Added dict ref ", token, " to ", object_Renamed.GetType().FullName);
            }
        }

        public override void close()
        {
            input.Close();
        }

        private void initIOSettings()
        {
            selectedEntry = new IntegerIOSetting("SelectedEntry", IOSetting.LOW, "Which entry should I read?", "1");

            readSecondaryFiles = new BooleanIOSetting("ReadSecondaryFiles", IOSetting.LOW, "Should I read the secondary files (if available)?", "true");

            //UPGRADE_TODO: Method 'java.lang.System.getProperty' was converted to 'System.Environment.GetEnvironmentVariable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperty_javalangString'"
            //UPGRADE_TODO: Method 'java.lang.System.getProperty' was converted to 'System.IO.Path.DirectorySeparatorChar.ToString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperty_javalangString'"
            readSecondaryDir = new StringIOSetting("ReadSecondaryDir", IOSetting.LOW, "Where can the secondary files be found?", System.Environment.GetEnvironmentVariable("userprofile") + System.IO.Path.DirectorySeparatorChar.ToString());
        }

        private void customizeJob()
        {
            fireIOSettingQuestion(selectedEntry);
            fireIOSettingQuestion(readSecondaryFiles);
            fireIOSettingQuestion(readSecondaryDir);
        }
    }
}