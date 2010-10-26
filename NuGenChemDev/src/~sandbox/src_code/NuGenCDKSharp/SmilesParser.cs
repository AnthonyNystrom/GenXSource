/*
*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-07-10 08:01:17 +0200 (Mon, 10 Jul 2006) $
*  $Revision: 6631 $
*
*  Copyright (C) 2002-2006  The Chemistry Development Kit (CDK) project
*
*  Contact: cdk-devel@lists.sourceforge.net
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public License
*  as published by the Free Software Foundation; either version 2.1
*  of the License, or (at your option) any later version.
*  All I ask is that proper credit is given for my work, which includes
*  - but is not limited to - adding the above copyright notice to the beginning
*  of your source code files, and to any copyright notice that you may distribute
*  with programs based on this work.
*
*  This program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public License
*  along with this program; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*
*/
using System;
using Org.OpenScience.CDK.Tools;
using Org.OpenScience.CDK.Tsools;
using Support;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Graph;
using Org.OpenScience.CDK.Exception;
using Org.OpenScience.CDK.Aromaticity;

namespace Org.OpenScience.CDK.Smiles
{
	/// <summary> Parses a SMILES {@cdk.cite SMILESTUT} string and an AtomContainer. The full
	/// SSMILES subset {@cdk.cite SSMILESTUT} and the '%' tag for more than 10 rings
	/// at a time are supported. An example:
	/// <pre>
	/// try {
	/// SmilesParser sp = new SmilesParser();
	/// Molecule m = sp.parseSmiles("c1ccccc1");
	/// } catch (InvalidSmilesException ise) {
	/// }
	/// </pre>
	/// 
	/// <p>This parser does not parse stereochemical information, but the following
	/// features are supported: reaction smiles, partitioned structures, charged
	/// atoms, implicit hydrogen count, '*' and isotope information.
	/// 
	/// <p>See {@cdk.cite WEI88} for further information.
	/// 
	/// </summary>
	/// <author>          Christoph Steinbeck
	/// </author>
	/// <author>          Egon Willighagen
	/// </author>
	/// <cdk.module>      smiles </cdk.module>
	/// <cdk.created>     2002-04-29 </cdk.created>
	/// <cdk.keyword>     SMILES, parser </cdk.keyword>
	/// <cdk.bug>         1095696 </cdk.bug>
	/// <cdk.bug>         1235852 </cdk.bug>
	/// <cdk.bug>         1274464 </cdk.bug>
	/// <cdk.bug>         1296113 </cdk.bug>
	/// <cdk.bug>         1363882 </cdk.bug>
	/// <cdk.bug>         1365547 </cdk.bug>
	/// <cdk.bug>         1503541 </cdk.bug>
	/// <cdk.bug>         1519183 </cdk.bug>
	public class SmilesParser
	{
//		private LoggingTool logger;
		private HydrogenAdder hAdder;
		private ValencyHybridChecker valencyChecker;
		private int status = 0;
		
		
		/// <summary>  Constructor for the SmilesParser object</summary>
		public SmilesParser()
		{
			//logger = new LoggingTool(this);
			try
			{
				valencyChecker = new ValencyHybridChecker();
				hAdder = new HydrogenAdder(valencyChecker);
			}
			catch (System.Exception exception)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				//logger.error("Could not instantiate valencyChecker or hydrogenAdder: ", exception.Message);
				//logger.debug(exception);
			}
		}
		
		
		internal int position = - 1;
		internal int nodeCounter = - 1;
		internal System.String smiles = null;
		internal double bondStatus = - 1;
		internal double bondStatusForRingClosure = 1;
		internal bool bondIsAromatic = false;
		internal Atom[] rings = null;
		internal double[] ringbonds = null;
		internal int thisRing = - 1;
		internal Molecule molecule = null;
		internal System.String currentSymbol = null;
		
		
		/// <summary>  Description of the Method
		/// 
		/// </summary>
		/// <param name="smiles">                     Description of the Parameter
		/// </param>
		/// <returns>                             Description of the Return Value
		/// </returns>
		/// <exception cref="InvalidSmilesException"> Description of the Exception
		/// </exception>
		public virtual Reaction parseReactionSmiles(System.String smiles)
		{
			SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(smiles, ">");
			System.String reactantSmiles = tokenizer.NextToken();
			System.String agentSmiles = "";
			System.String productSmiles = tokenizer.NextToken();
			if (tokenizer.HasMoreTokens())
			{
				agentSmiles = productSmiles;
				productSmiles = tokenizer.NextToken();
			}
			
			Reaction reaction = new Reaction();
			
			// add reactants
			IMolecule reactantContainer = parseSmiles(reactantSmiles);
			ISetOfMolecules reactantSet = ConnectivityChecker.partitionIntoMolecules(reactantContainer);
			IMolecule[] reactants = reactantSet.Molecules;
			for (int i = 0; i < reactants.Length; i++)
			{
				reaction.addReactant(reactants[i]);
			}
			
			// add reactants
			if (agentSmiles.Length > 0)
			{
				IMolecule agentContainer = parseSmiles(agentSmiles);
				ISetOfMolecules agentSet = ConnectivityChecker.partitionIntoMolecules(agentContainer);
				IMolecule[] agents = agentSet.Molecules;
				for (int i = 0; i < agents.Length; i++)
				{
					reaction.addAgent(agents[i]);
				}
			}
			
			// add products
			IMolecule productContainer = parseSmiles(productSmiles);
			ISetOfMolecules productSet = ConnectivityChecker.partitionIntoMolecules(productContainer);
			IMolecule[] products = productSet.Molecules;
			for (int i = 0; i < products.Length; i++)
			{
				reaction.addProduct(products[i]);
			}
			
			return reaction;
		}
		
		
		/// <summary>  Parses a SMILES string and returns a Molecule object.
		/// 
		/// </summary>
		/// <param name="smiles">                     A SMILES string
		/// </param>
		/// <returns>                             A Molecule representing the constitution
		/// given in the SMILES string
		/// </returns>
		/// <exception cref="InvalidSmilesException"> Exception thrown when the SMILES string
		/// is invalid
		/// </exception>
		public virtual Molecule parseSmiles(System.String smiles)
		{
			//logger.debug("parseSmiles()...");
			Bond bond = null;
			nodeCounter = 0;
			bondStatus = 0;
			bondIsAromatic = false;
			bool bondExists = true;
			thisRing = - 1;
			currentSymbol = null;
			molecule = new Molecule();
			position = 0;
			// we don't want more than 1024 rings
			rings = new Atom[1024];
			ringbonds = new double[1024];
			for (int f = 0; f < 1024; f++)
			{
				rings[f] = null;
				ringbonds[f] = - 1;
			}
			
			char mychar = 'X';
			char[] chars = new char[1];
			Atom lastNode = null;
			System.Collections.ArrayList atomStack = new System.Collections.ArrayList();
			System.Collections.ArrayList bondStack = new System.Collections.ArrayList();
			Atom atom = null;
			do 
			{
				try
				{
					mychar = smiles[position];
					//logger.debug("");
					//logger.debug("Processing: " + mychar);
					if (lastNode != null)
					{
						//logger.debug("Lastnode: ", lastNode.GetHashCode());
					}
					if ((mychar >= 'A' && mychar <= 'Z') || (mychar >= 'a' && mychar <= 'z') || (mychar == '*'))
					{
						status = 1;
						//logger.debug("Found a must-be 'organic subset' element");
						// only 'organic subset' elements allowed
						atom = null;
						if (mychar == '*')
						{
							currentSymbol = "*";
							atom = new PseudoAtom("*");
						}
						else
						{
							currentSymbol = getSymbolForOrganicSubsetElement(smiles, position);
							if (currentSymbol != null)
							{
								if (currentSymbol.Length == 1)
								{
									if (!(currentSymbol.ToUpper()).Equals(currentSymbol))
									{
										currentSymbol = currentSymbol.ToUpper();
										atom = new Atom(currentSymbol);
										atom.Hybridization = CDKConstants.HYBRIDIZATION_SP2;
									}
									else
									{
										atom = new Atom(currentSymbol);
									}
								}
								else
								{
									atom = new Atom(currentSymbol);
								}
								//logger.debug("Made atom: ", atom);
							}
							else
							{
								throw new InvalidSmilesException("Found element which is not a 'organic subset' element. You must " + "use [" + mychar + "].");
							}
						}
						
						molecule.addAtom(atom);
						//logger.debug("Adding atom ", atom.GetHashCode());
						if ((lastNode != null) && bondExists)
						{
							//logger.debug("Creating bond between ", atom.Symbol, " and ", lastNode.Symbol);
							bond = new Bond(atom, lastNode, bondStatus);
							if (bondIsAromatic)
							{
								bond.setFlag(CDKConstants.ISAROMATIC, true);
							}
							molecule.addBond(bond);
						}
						bondStatus = CDKConstants.BONDORDER_SINGLE;
						lastNode = atom;
						nodeCounter++;
						position = position + currentSymbol.Length;
						bondExists = true;
						bondIsAromatic = false;
					}
					else if (mychar == '=')
					{
						position++;
						if (status == 2 || smiles.Length == position + 1 || !(smiles[position] >= '0' && smiles[position] <= '9'))
						{
							bondStatus = CDKConstants.BONDORDER_DOUBLE;
						}
						else
						{
							bondStatusForRingClosure = CDKConstants.BONDORDER_DOUBLE;
						}
					}
					else if (mychar == '#')
					{
						position++;
						if (status == 2 || smiles.Length == position + 1 || !(smiles[position] >= '0' && smiles[position] <= '9'))
						{
							bondStatus = CDKConstants.BONDORDER_TRIPLE;
						}
						else
						{
							bondStatusForRingClosure = CDKConstants.BONDORDER_TRIPLE;
						}
					}
					else if (mychar == '(')
					{
						atomStack.Add(lastNode);
						//logger.debug("Stack:");
						System.Collections.IEnumerator ses = atomStack.GetEnumerator();
						//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
						while (ses.MoveNext())
						{
							//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
							Atom a = (Atom) ses.Current;
							//logger.debug("", a.GetHashCode());
						}
						//logger.debug("------");
						bondStack.Add((double) bondStatus);
						position++;
					}
					else if (mychar == ')')
					{
						lastNode = (Atom) SupportClass.StackSupport.Pop(atomStack);
						//logger.debug("Stack:");
						System.Collections.IEnumerator ses = atomStack.GetEnumerator();
						//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
						while (ses.MoveNext())
						{
							//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
							Atom a = (Atom) ses.Current;
							//logger.debug("", a.GetHashCode());
						}
						//logger.debug("------");
						bondStatus = ((System.Double) SupportClass.StackSupport.Pop(bondStack));
						position++;
					}
					else if (mychar >= '0' && mychar <= '9')
					{
						status = 2;
						chars[0] = mychar;
						currentSymbol = new System.String(chars);
						thisRing = (System.Int32.Parse(currentSymbol));
						handleRing(lastNode);
						position++;
					}
					else if (mychar == '%')
					{
						currentSymbol = getRingNumber(smiles, position);
						thisRing = (System.Int32.Parse(currentSymbol));
						handleRing(lastNode);
						position += currentSymbol.Length + 1;
					}
					else if (mychar == '[')
					{
						currentSymbol = getAtomString(smiles, position);
						atom = assembleAtom(currentSymbol);
						molecule.addAtom(atom);
						//logger.debug("Added atom: ", atom);
						if (lastNode != null && bondExists)
						{
							bond = new Bond(atom, lastNode, bondStatus);
							if (bondIsAromatic)
							{
								bond.setFlag(CDKConstants.ISAROMATIC, true);
							}
							molecule.addBond(bond);
							//logger.debug("Added bond: ", bond);
						}
						bondStatus = CDKConstants.BONDORDER_SINGLE;
						bondIsAromatic = false;
						lastNode = atom;
						nodeCounter++;
						position = position + currentSymbol.Length + 2;
						// plus two for [ and ]
						bondExists = true;
					}
					else if (mychar == '.')
					{
						bondExists = false;
						position++;
					}
					else if (mychar == '-')
					{
						bondExists = true;
						// a simple single bond
						position++;
					}
					else if (mychar == ':')
					{
						bondExists = true;
						bondIsAromatic = true;
						position++;
					}
					else if (mychar == '/' || mychar == '\\')
					{
						//logger.warn("Ignoring stereo information for double bond");
						position++;
					}
					else if (mychar == '@')
					{
						if (position < smiles.Length - 1 && smiles[position + 1] == '@')
						{
							position++;
						}
						//logger.warn("Ignoring stereo information for atom");
						position++;
					}
					else
					{
						throw new InvalidSmilesException("Unexpected character found: " + mychar);
					}
				}
				catch (InvalidSmilesException exc)
				{
					//logger.error("InvalidSmilesException while parsing char (in parseSmiles()): " + mychar);
					//logger.debug(exc);
					throw exc;
				}
				catch (System.Exception exception)
				{
					//logger.error("Error while parsing char: " + mychar);
					//logger.debug(exception);
					throw new InvalidSmilesException("Error while parsing char: " + mychar);
				}
				//logger.debug("Parsing next char");
			}
			while (position < smiles.Length);
			
			// add implicit hydrogens
			try
			{
				//logger.debug("before H-adding: ", molecule);
				hAdder.addImplicitHydrogensToSatisfyValency(molecule);
				//logger.debug("after H-adding: ", molecule);
			}
			catch (System.Exception exception)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				//logger.error("Error while calculation Hcount for SMILES atom: ", exception.Message);
			}
			
			// setup missing bond orders
			try
			{
				valencyChecker.saturate(molecule);
				//logger.debug("after adding missing bond orders: ", molecule);
			}
			catch (System.Exception exception)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				//logger.error("Error while calculation Hcount for SMILES atom: ", exception.Message);
			}
			
			// conceive aromatic perception
			IMolecule[] moleculeSet = ConnectivityChecker.partitionIntoMolecules(molecule).Molecules;
			//logger.debug("#mols ", moleculeSet.Length);
			for (int i = 0; i < moleculeSet.Length; i++)
			{
				//logger.debug("mol: ", moleculeSet[i]);
				try
				{
					valencyChecker.saturate(moleculeSet[i]);
					//logger.debug(" after saturation: ", moleculeSet[i]);
					if (HueckelAromaticityDetector.detectAromaticity(moleculeSet[i]))
					{
						//logger.debug("Structure is aromatic...");
					}
				}
				catch (System.Exception exception)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					//logger.error("Could not perceive aromaticity: ", exception.Message);
					//logger.debug(exception);
				}
			}
			
			return molecule;
		}
		
		
		/// <summary>  Gets the AtomString attribute of the SmilesParser object
		/// 
		/// </summary>
		/// <param name="pos">                        Description of the Parameter
		/// </param>
		/// <param name="smiles">                     Description of the Parameter
		/// </param>
		/// <returns>                             The AtomString value
		/// </returns>
		/// <exception cref="InvalidSmilesException"> Description of the Exception
		/// </exception>
		private System.String getAtomString(System.String smiles, int pos)
		{
			//logger.debug("getAtomString()");
			System.Text.StringBuilder atomString = new System.Text.StringBuilder();
			try
			{
				for (int f = pos + 1; f < smiles.Length; f++)
				{
					char character = smiles[f];
					if (character == ']')
					{
						break;
					}
					else
					{
						atomString.Append(character);
					}
				}
			}
			catch (System.Exception exception)
			{
				System.String message = "Problem parsing Atom specification given in brackets.\n";
				message += ("Invalid SMILES string was: " + smiles);
				//logger.error(message);
				//logger.debug(exception);
				throw new InvalidSmilesException(message);
			}
			return atomString.ToString();
		}
		
		
		/// <summary>  Gets the Charge attribute of the SmilesParser object
		/// 
		/// </summary>
		/// <param name="chargeString"> Description of the Parameter
		/// </param>
		/// <param name="position">     Description of the Parameter
		/// </param>
		/// <returns>               The Charge value
		/// </returns>
		private int getCharge(System.String chargeString, int position)
		{
			//logger.debug("getCharge(): Parsing charge from: ", chargeString.Substring(position));
			int charge = 0;
			if (chargeString[position] == '+')
			{
				charge = + 1;
				position++;
			}
			else if (chargeString[position] == '-')
			{
				charge = - 1;
				position++;
			}
			else
			{
				return charge;
			}
			System.Text.StringBuilder multiplier = new System.Text.StringBuilder();
			while (position < chargeString.Length && System.Char.IsDigit(chargeString[position]))
			{
				multiplier.Append(chargeString[position]);
				position++;
			}
			if (multiplier.Length > 0)
			{
				//logger.debug("Found multiplier: ", multiplier);
				try
				{
					charge = charge * System.Int32.Parse(multiplier.ToString());
				}
				catch (System.Exception exception)
				{
					//logger.error("Could not parse positive atomic charge!");
					//logger.debug(exception);
				}
			}
			//logger.debug("Found charge: ", charge);
			return charge;
		}
		
		
		/// <summary>  Gets the implicitHydrogenCount attribute of the SmilesParser object
		/// 
		/// </summary>
		/// <param name="s">        Description of the Parameter
		/// </param>
		/// <param name="position"> Description of the Parameter
		/// </param>
		/// <returns>           The implicitHydrogenCount value
		/// </returns>
		private int getImplicitHydrogenCount(System.String s, int position)
		{
			//logger.debug("getImplicitHydrogenCount(): Parsing implicit hydrogens from: " + s);
			int count = 1;
			if (s[position] == 'H')
			{
				System.Text.StringBuilder multiplier = new System.Text.StringBuilder();
				while (position < (s.Length - 1) && System.Char.IsDigit(s[position + 1]))
				{
					multiplier.Append(position + 1);
					position++;
				}
				if (multiplier.Length > 0)
				{
					try
					{
						count = count + System.Int32.Parse(multiplier.ToString());
					}
					catch (System.Exception exception)
					{
						//logger.error("Could not parse number of implicit hydrogens!");
						//logger.debug(exception);
					}
				}
			}
			return count;
		}
		
		
		/// <summary>  Gets the ElementSymbol attribute of the SmilesParser object
		/// 
		/// </summary>
		/// <param name="s">   Description of the Parameter
		/// </param>
		/// <param name="pos"> Description of the Parameter
		/// </param>
		/// <returns>      The ElementSymbol value
		/// </returns>
		private System.String getElementSymbol(System.String s, int pos)
		{
			//logger.debug("getElementSymbol(): Parsing element symbol (pos=" + pos + ") from: " + s);
			// try to match elements not in the organic subset.
			// first, the two char elements
			if (pos < s.Length - 1)
			{
				System.String possibleSymbol = s.Substring(pos, (pos + 2) - (pos));
				//logger.debug("possibleSymbol: ", possibleSymbol);
				if (("HeLiBeNeNaMgAlSiClArCaScTiCrMnFeCoNiCuZnGaGeAsSe".IndexOf(possibleSymbol) >= 0) || ("BrKrRbSrZrNbMoTcRuRhPdAgCdInSnSbTeXeCsBaLuHfTaRe".IndexOf(possibleSymbol) >= 0) || ("OsIrPtAuHgTlPbBiPoAtRnFrRaLrRfDbSgBhHsMtDs".IndexOf(possibleSymbol) >= 0))
				{
					return possibleSymbol;
				}
			}
			// if that fails, the one char elements
			System.String possibleSymbol2 = s.Substring(pos, (pos + 1) - (pos));
			//logger.debug("possibleSymbol: ", possibleSymbol2);
			if (("HKUVY".IndexOf(possibleSymbol2) >= 0))
			{
				return possibleSymbol2;
			}
			// if that failed too, then possibly a organic subset element
			return getSymbolForOrganicSubsetElement(s, pos);
		}
		
		
		/// <summary>  Gets the ElementSymbol for an element in the 'organic subset' for which
		/// brackets may be omited. <p>
		/// 
		/// See: <a href="http://www.daylight.com/dayhtml/smiles/smiles-atoms.html">
		/// http://www.daylight.com/dayhtml/smiles/smiles-atoms.html</a> .
		/// 
		/// </summary>
		/// <param name="s">   Description of the Parameter
		/// </param>
		/// <param name="pos"> Description of the Parameter
		/// </param>
		/// <returns>      The symbolForOrganicSubsetElement value
		/// </returns>
		private System.String getSymbolForOrganicSubsetElement(System.String s, int pos)
		{
			//logger.debug("getSymbolForOrganicSubsetElement(): Parsing organic subset element from: ", s);
			if (pos < s.Length - 1)
			{
				System.String possibleSymbol = s.Substring(pos, (pos + 2) - (pos));
				if (("ClBr".IndexOf(possibleSymbol) >= 0))
				{
					return possibleSymbol;
				}
			}
			if ("BCcNnOoFPSsI".IndexOf((System.Char) (s[pos])) >= 0)
			{
				return s.Substring(pos, (pos + 1) - (pos));
			}
			if ("fpi".IndexOf((System.Char) (s[pos])) >= 0)
			{
				//logger.warn("Element ", s, " is normally not sp2 hybridisized!");
				return s.Substring(pos, (pos + 1) - (pos));
			}
			//logger.warn("Subset element not found!");
			return null;
		}
		
		
		/// <summary>  Gets the RingNumber attribute of the SmilesParser object
		/// 
		/// </summary>
		/// <param name="s">   Description of the Parameter
		/// </param>
		/// <param name="pos"> Description of the Parameter
		/// </param>
		/// <returns>      The RingNumber value
		/// </returns>
		private System.String getRingNumber(System.String s, int pos)
		{
			//logger.debug("getRingNumber()");
			System.String retString = "";
			pos++;
			do 
			{
				retString += s[pos];
				pos++;
			}
			while (pos < s.Length && (s[pos] >= '0' && s[pos] <= '9'));
			return retString;
		}
		
		
		/// <summary>  Description of the Method
		/// 
		/// </summary>
		/// <param name="s">                          Description of the Parameter
		/// </param>
		/// <returns>                             Description of the Return Value
		/// </returns>
		/// <exception cref="InvalidSmilesException"> Description of the Exception
		/// </exception>
		private Atom assembleAtom(System.String s)
		{
			//logger.debug("assembleAtom(): Assembling atom from: ", s);
			Atom atom = null;
			int position = 0;
			System.String currentSymbol = null;
			System.Text.StringBuilder isotopicNumber = new System.Text.StringBuilder();
			char mychar;
			//logger.debug("Parse everythings before and including element symbol");
			do 
			{
				try
				{
					mychar = s[position];
					//logger.debug("Parsing char: " + mychar);
					if ((mychar >= 'A' && mychar <= 'Z') || (mychar >= 'a' && mychar <= 'z'))
					{
						currentSymbol = getElementSymbol(s, position);
						if (currentSymbol == null)
						{
							throw new InvalidSmilesException("Expected element symbol, found null!");
						}
						else
						{
							//logger.debug("Found element symbol: ", currentSymbol);
							position = position + currentSymbol.Length;
							if (currentSymbol.Length == 1)
							{
								if (!(currentSymbol.ToUpper()).Equals(currentSymbol))
								{
									currentSymbol = currentSymbol.ToUpper();
									atom = new Atom(currentSymbol);
									atom.Hybridization = CDKConstants.HYBRIDIZATION_SP2;
									if (atom.getHydrogenCount() > 0)
									{
										atom.setHydrogenCount(atom.getHydrogenCount() - 1);
									}
								}
								else
								{
									atom = new Atom(currentSymbol);
								}
							}
							else
							{
								atom = new Atom(currentSymbol);
							}
							//logger.debug("Made atom: ", atom);
						}
						break;
					}
					else if (mychar >= '0' && mychar <= '9')
					{
						isotopicNumber.Append(mychar);
						position++;
					}
					else if (mychar == '*')
					{
						currentSymbol = "*";
						atom = new PseudoAtom(currentSymbol);
						//logger.debug("Made atom: ", atom);
						position++;
						break;
					}
					else
					{
						throw new InvalidSmilesException("Found unexpected char: " + mychar);
					}
				}
				catch (InvalidSmilesException exc)
				{
					//logger.error("InvalidSmilesException while parsing atom string: " + s);
					//logger.debug(exc);
					throw exc;
				}
				catch (System.Exception exception)
				{
					//logger.error("Could not parse atom string: ", s);
					//logger.debug(exception);
					throw new InvalidSmilesException("Could not parse atom string: " + s);
				}
			}
			while (position < s.Length);
			if (isotopicNumber.ToString().Length > 0)
			{
				try
				{
					atom.MassNumber = System.Int32.Parse(isotopicNumber.ToString());
				}
				catch (System.Exception exception)
				{
					//logger.error("Could not set atom's atom number.");
					//logger.debug(exception);
				}
			}
			//logger.debug("Parsing part after element symbol (like charge): ", s.Substring(position));
			int charge = 0;
			int implicitHydrogens = 0;
			while (position < s.Length)
			{
				try
				{
					mychar = s[position];
					//logger.debug("Parsing char: " + mychar);
					if (mychar == 'H')
					{
						// count implicit hydrogens
						implicitHydrogens = getImplicitHydrogenCount(s, position);
						position++;
						if (implicitHydrogens > 1)
						{
							position++;
						}
						atom.setHydrogenCount(implicitHydrogens);
					}
					else if (mychar == '+' || mychar == '-')
					{
						charge = getCharge(s, position);
						position++;
						if (charge < - 1 || charge > 1)
						{
							position++;
						}
						atom.setFormalCharge(charge);
					}
					else if (mychar == '@')
					{
						if (position < s.Length - 1 && s[position + 1] == '@')
						{
							position++;
						}
						//logger.warn("Ignoring stereo information for atom");
						position++;
					}
					else
					{
						throw new InvalidSmilesException("Found unexpected char: " + mychar);
					}
				}
				catch (InvalidSmilesException exc)
				{
					//logger.error("InvalidSmilesException while parsing atom string: ", s);
					//logger.debug(exc);
					throw exc;
				}
				catch (System.Exception exception)
				{
					//logger.error("Could not parse atom string: ", s);
					//logger.debug(exception);
					throw new InvalidSmilesException("Could not parse atom string: " + s);
				}
			}
			return atom;
		}
		
		
		/// <summary>  We call this method when a ring (depicted by a number) has been found.
		/// 
		/// </summary>
		/// <param name="atom"> Description of the Parameter
		/// </param>
		private void  handleRing(Atom atom)
		{
			//logger.debug("handleRing():");
			double bondStat = bondStatusForRingClosure;
			Bond bond = null;
			Atom partner = null;
			Atom thisNode = rings[thisRing];
			// lookup
			if (thisNode != null)
			{
				partner = thisNode;
				bond = new Bond(atom, partner, bondStat);
				if (bondIsAromatic)
				{
					
					bond.setFlag(CDKConstants.ISAROMATIC, true);
				}
				molecule.addBond(bond);
				bondIsAromatic = false;
				rings[thisRing] = null;
				ringbonds[thisRing] = - 1;
			}
			else
			{
				/*
				*  First occurence of this ring:
				*  - add current atom to list
				*/
				rings[thisRing] = atom;
				ringbonds[thisRing] = bondStatusForRingClosure;
			}
			bondStatusForRingClosure = 1;
		}
	}
}