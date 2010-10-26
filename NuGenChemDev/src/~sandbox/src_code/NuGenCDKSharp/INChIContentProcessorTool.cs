/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
*
* Copyright (C) 2002-2006  The Chemistry Development Kit (CDK) project
*
* Contact: cdk-devel@lists.sf.net
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
using Org.OpenScience.CDK.Interfaces;
using System.Text.RegularExpressions;

namespace Org.OpenScience.CDK.IO.Inchi
{
    /// <summary> Tool to help process INChI 1.12beta content.
    /// 
    /// </summary>
    /// <cdk.module>  experimental </cdk.module>
    public class INChIContentProcessorTool
    {
        //private LoggingTool //logger;

        public INChIContentProcessorTool()
        {
            //logger = new LoggingTool(this);
        }

        /// <summary> Processes the content from the formula field of the INChI.
        /// Typical values look like C6H6, from INChI=1.12Beta/C6H6/c1-2-4-6-5-3-1/h1-6H.
        /// </summary>
        public virtual IAtomContainer processFormula(IAtomContainer parsedContent, System.String atomsEncoding)
        {
            //logger.debug("Parsing atom data: ", atomsEncoding);

            Regex pattern = new Regex("([A-Z][a-z]?)(\\d+)?(.*)");
            //Pattern pattern = Pattern.compile("([A-Z][a-z]?)(\\d+)?(.*)");
            System.String remainder = atomsEncoding;
            while (remainder.Length > 0)
            {
                //logger.debug("Remaining: ", remainder);
                Match matcher = pattern.Match(remainder);
                //Matcher matcher = pattern.matcher(remainder);
                //if (matcher.matches())
                if (matcher != null && matcher.Success)
                {
                    System.String symbol = matcher.Groups[1].Value;
                    //logger.debug("Atom symbol: ", symbol);
                    if (symbol.Equals("H"))
                    {
                        // don't add explicit hydrogens
                    }
                    else
                    {
                        System.String occurenceStr = matcher.Groups[2].Value;
                        int occurence = 1;
                        if (occurenceStr != null)
                        {
                            occurence = System.Int32.Parse(occurenceStr);
                        }
                        //logger.debug("  occurence: ", occurence);
                        for (int i = 1; i <= occurence; i++)
                        {
                            parsedContent.addAtom(parsedContent.Builder.newAtom(symbol));
                        }
                    }
                    remainder = matcher.Groups[3].Value;
                    if (remainder == null)
                        remainder = "";
                    //logger.debug("  Remaining: ", remainder);
                }
                else
                {
                    //logger.error("No match found!");
                    remainder = "";
                }
                //logger.debug("NO atoms: ", parsedContent.AtomCount);
            }
            return parsedContent;
        }

        /// <summary> Processes the content from the connections field of the INChI.
        /// Typical values look like 1-2-4-6-5-3-1, from INChI=1.12Beta/C6H6/c1-2-4-6-5-3-1/h1-6H.
        /// 
        /// </summary>
        /// <param name="bondsEncoding">the content of the INChI connections field
        /// </param>
        /// <param name="container">    the atomContainer parsed from the formula field
        /// </param>
        /// <param name="source">       the atom to build the path upon. If -1, then start new path
        /// 
        /// </param>
        /// <seealso cref="processFormula">
        /// </seealso>
        public virtual void processConnections(System.String bondsEncoding, IAtomContainer container, int source)
        {
            //logger.debug("Parsing bond data: ", bondsEncoding);

            IBond bondToAdd = null;
            /* Fixme: treatment of branching is too limited! */
            System.String remainder = bondsEncoding;
            while (remainder.Length > 0)
            {
                //logger.debug("Bond part: ", remainder);
                if (remainder[0] == '(')
                {
                    System.String branch = chopBranch(remainder);
                    processConnections(branch, container, source);
                    if (branch.Length + 2 <= remainder.Length)
                    {
                        remainder = remainder.Substring(branch.Length + 2);
                    }
                    else
                    {
                        remainder = "";
                    }
                }
                else
                {
                    Regex pattern = new Regex("^(\\d+)-?(.*)");
                    //Pattern pattern = Pattern.compile("^(\\d+)-?(.*)");
                    //Matcher matcher = pattern.matcher(remainder);
                    Match matcher = pattern.Match(remainder);
                    //if (matcher.matches())
                    if (matcher != null && matcher.Success)
                    {
                        System.String targetStr = matcher.Groups[1].Value;
                        int target = System.Int32.Parse(targetStr);
                        //logger.debug("Source atom: ", source);
                        //logger.debug("Target atom: ", targetStr);
                        IAtom targetAtom = container.getAtomAt(target - 1);
                        if (source != -1)
                        {
                            IAtom sourceAtom = container.getAtomAt(source - 1);
                            bondToAdd = container.Builder.newBond(sourceAtom, targetAtom, 1.0);
                            container.addBond(bondToAdd);
                        }
                        remainder = matcher.Groups[2].Value;
                        source = target;
                        //logger.debug("  remainder: ", remainder);
                    }
                    else
                    {
                        //logger.error("Could not get next bond info part");
                        return;
                    }
                }
            }
        }

        /// <summary> Extracts the first full branch. It extracts everything between the first
        /// '(' and the corresponding ')' char.
        /// </summary>
        private System.String chopBranch(System.String remainder)
        {
            bool doChop = false;
            int branchLevel = 0;
            System.Text.StringBuilder choppedString = new System.Text.StringBuilder();
            for (int i = 0; i < remainder.Length; i++)
            {
                char currentChar = remainder[i];
                if (currentChar == '(')
                {
                    if (doChop)
                        choppedString.Append(currentChar);
                    doChop = true;
                    branchLevel++;
                }
                else if (currentChar == ')')
                {
                    branchLevel--;
                    if (branchLevel == 0)
                        doChop = false;
                    if (doChop)
                        choppedString.Append(currentChar);
                }
                else if (doChop)
                {
                    choppedString.Append(currentChar);
                }
            }
            return choppedString.ToString();
        }
    }
}