/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-06-07 11:41:42 +0200 (Wed, 07 Jun 2006) $
* $Revision: 6349 $
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
using System.IO;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Exception;
using Support;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Reads a molecule from an MDL RXN file {@cdk.cite DAL92}.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>      Egon Willighagen
    /// </author>
    /// <cdk.created>     2003-07-24 </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>     file format, MDL RXN </cdk.keyword>
    public class MDLRXNReader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new MDLRXNFormat();
            }

        }

        internal System.IO.StreamReader input = null;
        //private LoggingTool //logger = null;

        /// <summary> Contructs a new MDLReader that can read Molecule from a given Reader.
        /// 
        /// </summary>
        /// <param name="in"> The Reader to read from
        /// </param>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public MDLRXNReader(System.IO.StreamReader in_Renamed)
        {
            //logger = new LoggingTool(this);
            //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            input = new System.IO.StreamReader(in_Renamed.BaseStream, in_Renamed.CurrentEncoding);
        }

        public MDLRXNReader(System.IO.Stream input)
            : this(new System.IO.StreamReader(input, System.Text.Encoding.Default))
        {
        }

        public MDLRXNReader()
            : this((StreamReader)null)
        {
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
                //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
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
                if (typeof(IReaction).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        /// <summary> Takes an object which subclasses IChemObject, e.g.Molecule, and will read
        /// this (from file, database, internet etc). If the specific implementation
        /// does not support a specific IChemObject it will throw an Exception.
        /// 
        /// </summary>
        /// <param name="object">                             The object that subclasses
        /// IChemObject
        /// </param>
        /// <returns>                                     The IChemObject read
        /// </returns>
        /// <exception cref="CDKException">
        /// </exception>
        public override IChemObject read(IChemObject object_Renamed)
        {
            if (object_Renamed is IReaction)
            {
                return (IChemObject)readReaction(object_Renamed.Builder);
            }
            else if (object_Renamed is IChemModel)
            {
                IChemModel model = object_Renamed.Builder.newChemModel();
                ISetOfReactions reactionSet = object_Renamed.Builder.newSetOfReactions();
                reactionSet.addReaction(readReaction(object_Renamed.Builder));
                model.SetOfReactions = reactionSet;
                return model;
            }
            else if (object_Renamed is IChemFile)
            {
                IChemFile chemFile = object_Renamed.Builder.newChemFile();
                IChemSequence sequence = object_Renamed.Builder.newChemSequence();
                sequence.addChemModel((IChemModel)read(object_Renamed.Builder.newChemModel()));
                chemFile.addChemSequence(sequence);
                return chemFile;
            }
            else
            {
                throw new CDKException("Only supported are Reaction and ChemModel, and not " + object_Renamed.GetType().FullName + ".");
            }
        }

        public virtual bool accepts(IChemObject object_Renamed)
        {
            if (object_Renamed is IReaction)
            {
                return true;
            }
            else if (object_Renamed is IChemModel)
            {
                return true;
            }
            else if (object_Renamed is IChemFile)
            {
                return true;
            }
            return false;
        }


        /// <summary> Read a Reaction from a file in MDL RXN format
        /// 
        /// </summary>
        /// <returns>  The Reaction that was read from the MDL file.
        /// </returns>
        private IReaction readReaction(IChemObjectBuilder builder)
        {
            IReaction reaction = builder.newReaction();
            try
            {
                input.ReadLine(); // first line should be $RXN
                input.ReadLine(); // second line
                input.ReadLine(); // third line
                input.ReadLine(); // fourth line
            }
            catch (System.IO.IOException exception)
            {
                //logger.debug(exception);
                throw new CDKException("Error while reading header of RXN file", exception);
            }

            int reactantCount = 0;
            int productCount = 0;
            try
            {
                System.String countsLine = input.ReadLine();
                /* this line contains the number of reactants
                and products */
                SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(countsLine);
                reactantCount = System.Int32.Parse(tokenizer.NextToken());
                //logger.info("Expecting " + reactantCount + " reactants in file");
                productCount = System.Int32.Parse(tokenizer.NextToken());
                //logger.info("Expecting " + productCount + " products in file");
            }
            catch (System.Exception exception)
            {
                //logger.debug(exception);
                throw new CDKException("Error while counts line of RXN file", exception);
            }

            // now read the reactants
            try
            {
                for (int i = 1; i <= reactantCount; i++)
                {
                    System.Text.StringBuilder molFile = new System.Text.StringBuilder();
                    input.ReadLine(); // announceMDLFileLine
                    System.String molFileLine = "";
                    do
                    {
                        molFileLine = input.ReadLine();
                        molFile.Append(molFileLine);
                        molFile.Append("\n");
                    }
                    while (!molFileLine.Equals("M  END"));

                    // read MDL molfile content
                    MDLReader reader = new MDLReader(new StreamReader(molFile.ToString()));
                    IMolecule reactant = (IMolecule)reader.read(builder.newMolecule());

                    // add reactant
                    reaction.addReactant(reactant);
                }
            }
            catch (CDKException exception)
            {
                // rethrow exception from MDLReader
                throw exception;
            }
            catch (System.Exception exception)
            {
                //logger.debug(exception);
                throw new CDKException("Error while reading reactant", exception);
            }

            // now read the products
            try
            {
                for (int i = 1; i <= productCount; i++)
                {
                    System.Text.StringBuilder molFile = new System.Text.StringBuilder();
                    input.ReadLine(); // String announceMDLFileLine = 
                    System.String molFileLine = "";
                    do
                    {
                        molFileLine = input.ReadLine();
                        molFile.Append(molFileLine);
                        molFile.Append("\n");
                    }
                    while (!molFileLine.Equals("M  END"));

                    // read MDL molfile content
                    MDLReader reader = new MDLReader(new StreamReader(molFile.ToString()));
                    IMolecule product = (IMolecule)reader.read(builder.newMolecule());

                    // add reactant
                    reaction.addProduct(product);
                }
            }
            catch (CDKException exception)
            {
                // rethrow exception from MDLReader
                throw exception;
            }
            catch (System.Exception exception)
            {
                //logger.debug(exception);
                throw new CDKException("Error while reading products", exception);
            }

            // now try to map things, if wanted
            //logger.info("Reading atom-atom mapping from file");
            // distribute all atoms over two AtomContainer's
            IAtomContainer reactingSide = builder.newAtomContainer();
            IMolecule[] molecules = reaction.Reactants.Molecules;
            for (int i = 0; i < molecules.Length; i++)
            {
                reactingSide.add(molecules[i]);
            }
            IAtomContainer producedSide = builder.newAtomContainer();
            molecules = reaction.Products.Molecules;
            for (int i = 0; i < molecules.Length; i++)
            {
                producedSide.add(molecules[i]);
            }

            // map the atoms
            int mappingCount = 0;
            IAtom[] reactantAtoms = reactingSide.Atoms;
            IAtom[] producedAtoms = producedSide.Atoms;
            for (int i = 0; i < reactantAtoms.Length; i++)
            {
                for (int j = 0; j < producedAtoms.Length; j++)
                {
                    if (reactantAtoms[i].ID != null && reactantAtoms[i].ID.Equals(producedAtoms[j].ID))
                    {
                        reaction.addMapping(builder.newMapping(reactantAtoms[i], producedAtoms[j]));
                        mappingCount++;
                        break;
                    }
                }
            }
            //logger.info("Mapped atom pairs: " + mappingCount);

            return reaction;
        }

        public override void close()
        {
            input.Close();
        }
    }
}