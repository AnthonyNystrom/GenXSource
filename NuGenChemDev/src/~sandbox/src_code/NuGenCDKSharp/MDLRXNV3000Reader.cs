/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-06-07 11:41:42 +0200 (Wed, 07 Jun 2006) $
* $Revision: 6349 $
*
* Copyright (C) 2002-2006  The Jmol Development Team
*
* Contact: cdk-devel@lists.sourceforge.net
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
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using Org.OpenScience.CDK.IO.Formats;
using Org.OpenScience.CDK.IO.Setting;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Exception;
using System.IO;
using Support;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Class that implements the new MDL mol format introduced in August 2002.
    /// The overall syntax is compatible with the old format, but I consider
    /// the format completely different, and thus implemented a separate Reader
    /// for it.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>   Egon Willighagen <egonw@sci.kun.nl>
    /// </author>
    /// <cdk.created>  2003-10-05 </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  MDL V3000 </cdk.keyword>
    /// <cdk.require>  java1.4+ </cdk.require>
    public class MDLRXNV3000Reader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new MDLRXNV3000Format();
            }

        }
        virtual public bool Ready
        {
            get
            {
                try
                {
                    return input.Peek() != -1;
                }
                catch (System.Exception exception)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    System.String error = "Unexpected error while reading file: " + exception.Message;
                    //logger.error(error);
                    //logger.debug(exception);
                    throw new CDKException(error, exception);
                }
            }

        }
        override public IOSetting[] IOSettings
        {
            get
            {
                return new IOSetting[0];
            }

        }

        internal System.IO.StreamReader input = null;
        //private LoggingTool //logger = null;

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public MDLRXNV3000Reader(System.IO.StreamReader in_Renamed)
        {
            //logger = new LoggingTool(this);
            //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            input = new System.IO.StreamReader(in_Renamed.BaseStream, in_Renamed.CurrentEncoding);
            initIOSettings();
        }

        public MDLRXNV3000Reader(System.IO.Stream input)
            : this(new System.IO.StreamReader(input, System.Text.Encoding.Default))
        {
        }

        public MDLRXNV3000Reader()
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
                if (typeof(IReaction).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        public override IChemObject read(IChemObject object_Renamed)
        {
            if (object_Renamed is IReaction)
            {
                return readReaction(object_Renamed.Builder);
            }
            else if (object_Renamed is IChemModel)
            {
                IChemModel model = object_Renamed.Builder.newChemModel();
                ISetOfReactions reactionSet = object_Renamed.Builder.newSetOfReactions();
                reactionSet.addReaction(readReaction(object_Renamed.Builder));
                model.SetOfReactions = reactionSet;
                return model;
            }
            else
            {
                throw new CDKException("Only supported are Reaction and ChemModel, and not " + object_Renamed.GetType().FullName + ".");
            }
        }

        /// <summary> Reads the command on this line. If the line is continued on the next, that
        /// part is added.
        /// 
        /// </summary>
        /// <returns> Returns the command on this line.
        /// </returns>
        public virtual System.String readCommand()
        {
            System.String line = readLine();
            if (line.StartsWith("M  V30 "))
            {
                System.String command = line.Substring(7);
                if (command.EndsWith("-"))
                {
                    command = command.Substring(0, (command.Length - 1) - (0));
                    command += readCommand();
                }
                return command;
            }
            else
            {
                throw new CDKException("Could not read MDL file: unexpected line: " + line);
            }
        }

        public virtual System.String readLine()
        {
            System.String line = null;
            try
            {
                line = input.ReadLine();
                //logger.debug("read line: " + line);
            }
            catch (System.Exception exception)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                System.String error = "Unexpected error while reading file: " + exception.Message;
                //logger.error(error);
                //logger.debug(exception);
                throw new CDKException(error, exception);
            }
            return line;
        }

        private IReaction readReaction(IChemObjectBuilder builder)
        {
            IReaction reaction = builder.newReaction();
            readLine(); // first line should be $RXN
            readLine(); // second line
            readLine(); // third line
            readLine(); // fourth line

            int reactantCount = 0;
            int productCount = 0;
            bool foundCOUNTS = false;
            while (Ready && !foundCOUNTS)
            {
                System.String command = readCommand();
                if (command.StartsWith("COUNTS"))
                {
                    SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(command);
                    try
                    {
                        tokenizer.NextToken();
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
                    foundCOUNTS = true;
                }
                else
                {
                    //logger.warn("Waiting for COUNTS line, but found: " + command);
                }
            }

            // now read the reactants
            for (int i = 1; i <= reactantCount; i++)
            {
                System.Text.StringBuilder molFile = new System.Text.StringBuilder();
                System.String announceMDLFileLine = readCommand();
                if (!announceMDLFileLine.Equals("BEGIN REACTANT"))
                {
                    System.String error = "Excepted start of reactant, but found: " + announceMDLFileLine;
                    //logger.error(error);
                    throw new CDKException(error);
                }
                System.String molFileLine = "";
                while (!molFileLine.EndsWith("END REACTANT"))
                {
                    molFileLine = readLine();
                    molFile.Append(molFileLine);
                    molFile.Append("\n");
                };

                try
                {
                    // read MDL molfile content
                    MDLV3000Reader reader = new MDLV3000Reader(new StreamReader(molFile.ToString()));
                    IMolecule reactant = (IMolecule)reader.read(builder.newMolecule());

                    // add reactant
                    reaction.addReactant(reactant);
                }
                catch (System.Exception exception)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    System.String error = "Error while reading reactant: " + exception.Message;
                    //logger.error(error);
                    //logger.debug(exception);
                    throw new CDKException(error, exception);
                }
            }

            // now read the products
            for (int i = 1; i <= productCount; i++)
            {
                System.Text.StringBuilder molFile = new System.Text.StringBuilder();
                System.String announceMDLFileLine = readCommand();
                if (!announceMDLFileLine.Equals("BEGIN PRODUCT"))
                {
                    System.String error = "Excepted start of product, but found: " + announceMDLFileLine;
                    //logger.error(error);
                    throw new CDKException(error);
                }
                System.String molFileLine = "";
                while (!molFileLine.EndsWith("END PRODUCT"))
                {
                    molFileLine = readLine();
                    molFile.Append(molFileLine);
                    molFile.Append("\n");
                };

                try
                {
                    // read MDL molfile content
                    MDLV3000Reader reader = new MDLV3000Reader(new StreamReader(molFile.ToString()));
                    IMolecule product = (IMolecule)reader.read(builder.newMolecule());

                    // add product
                    reaction.addProduct(product);
                }
                catch (System.Exception exception)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    System.String error = "Error while reading product: " + exception.Message;
                    //logger.error(error);
                    //logger.debug(exception);
                    throw new CDKException(error, exception);
                }
            }

            return reaction;
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
            return false;
        }

        public override void close()
        {
            input.Close();
        }

        private void initIOSettings()
        {
        }
    }
}