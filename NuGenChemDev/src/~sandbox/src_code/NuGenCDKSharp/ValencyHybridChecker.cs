/*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-07-02 13:48:44 +0200 (Sun, 02 Jul 2006) $
*  $Revision: 6537 $
*
*  Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
*
*  Contact: cdk-devel@lists.sourceforge.net
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public License
*  as published by the Free Software Foundation; either version 2.1
*  of the License, or (at your option) any later version.
*  All we ask is that proper credit is given for our work, which includes
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

using Org.OpenScience.CDK.Tools;
using Org.OpenScience.CDK.Config;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Exception;
using System;
namespace Org.OpenScience.CDK.Tsools
{
	/// <summary> This class is an experimental alternative to the ValencyChecker.
	/// The main difference is that this checker uses a different atom type
	/// list and takes formal charges <i>and</i> hybridization into account: 
	/// it first matches against element, formal charge and hybridization,
	/// and then browses the list for possible matching types. It extends
	/// ValencyChecker and only overwrites the <code>couldMatchAtomType</code>
	/// method.
	/// 
	/// <p>The atoms are matched against the hybridization_atomtype.xml list.
	/// 
	/// </summary>
	/// <author>        Egon Willighagen
	/// </author>
	/// <cdk.created>   2004-06-12 </cdk.created>
	/// <cdk.keyword>   atom, valency </cdk.keyword>
	/// <cdk.module>    valencycheck </cdk.module>
	/// <cdk.bug>       1174781 </cdk.bug>
	public class ValencyHybridChecker : IValencyChecker
	{
		
		private System.String atomTypeList = null;
		protected internal AtomTypeFactory structgenATF;
//		protected internal LoggingTool logger;
		
		public ValencyHybridChecker():this("org/openscience/cdk/config/data/hybridization_atomtypes.xml")
		{
		}
		
		public ValencyHybridChecker(System.String atomTypeList)
		{
			this.atomTypeList = atomTypeList;
			//logger = new LoggingTool(this);
			//logger.info("Using configuration file: ", atomTypeList);
		}
		
		/// <summary> Saturates a molecule by setting appropriate bond orders.
		/// 
		/// </summary>
		/// <cdk.keyword>             bond order, calculation </cdk.keyword>
		/// <summary> 
		/// </summary>
		/// <cdk.created>  2003-10-03 </cdk.created>
		public virtual void  saturate(IAtomContainer atomContainer)
		{
			//logger.info("Saturating atomContainer by adjusting bond orders...");
			bool allSaturated = this.allSaturated(atomContainer);
			if (!allSaturated)
			{
				//logger.info("Saturating bond orders is needed...");
				bool succeeded = saturate(atomContainer.Bonds, atomContainer);
				if (!succeeded)
				{
					throw new CDKException("Could not saturate this atomContainer!");
				}
			}
		}
		
		/// <summary> Saturates a set of Bonds in an AtomContainer.</summary>
		public virtual bool saturate(IBond[] bonds, IAtomContainer atomContainer)
		{
			//logger.debug("Saturating bond set of size: ", bonds.Length);
			bool bondsAreFullySaturated = false;
			if (bonds.Length > 0)
			{
				IBond bond = bonds[0];
				
				// determine bonds left
				int leftBondCount = bonds.Length - 1;
				IBond[] leftBonds = new IBond[leftBondCount];
				Array.Copy(bonds, 1, leftBonds, 0, leftBondCount);
				
				// examine this bond
				//logger.debug("Examining this bond: ", bond);
				if (isSaturated(bond, atomContainer))
				{
					//logger.debug("OK, bond is saturated, now try to saturate remaining bonds (if needed)");
					bondsAreFullySaturated = saturate(leftBonds, atomContainer);
				}
				else if (isUnsaturated(bond, atomContainer))
				{
					//logger.debug("Ok, this bond is unsaturated, and can be saturated");
					// two options now: 
					// 1. saturate this one directly
					// 2. saturate this one by saturating the rest
					//logger.debug("Option 1: Saturating this bond directly, then trying to saturate rest");
					// considering organic bonds, the max order is 3, so increase twice
					double increment = 1.0;
					bool bondOrderIncreased = saturateByIncreasingBondOrder(bond, atomContainer, increment);
					bondsAreFullySaturated = bondOrderIncreased && saturate(bonds, atomContainer);
					if (bondsAreFullySaturated)
					{
						//logger.debug("Option 1: worked");
					}
					else
					{
						//logger.debug("Option 1: failed. Trying option 2.");
						//logger.debug("Option 2: Saturing this bond by saturating the rest");
						// revert the increase (if succeeded), then saturate the rest
						if (bondOrderIncreased)
							unsaturateByDecreasingBondOrder(bond, increment);
						bondsAreFullySaturated = saturate(leftBonds, atomContainer) && isSaturated(bond, atomContainer);
						//if (!bondsAreFullySaturated)
							//logger.debug("Option 2: failed");
					}
				}
				else
				{
					//logger.debug("Ok, this bond is unsaturated, but cannot be saturated");
					// try recursing and see if that fixes things
					bondsAreFullySaturated = saturate(leftBonds, atomContainer) && isSaturated(bond, atomContainer);
				}
			}
			else
			{
				bondsAreFullySaturated = true; // empty is saturated by default
			}
			return bondsAreFullySaturated;
		}
		
		public virtual bool unsaturateByDecreasingBondOrder(IBond bond, double decrement)
		{
			if (bond.Order > decrement)
			{
				bond.Order = bond.Order - decrement;
				return true;
			}
			else
			{
				return false;
			}
		}
		
		/// <summary> Returns wether a bond is unsaturated. A bond is unsaturated if 
		/// <b>all</b> Atoms in the bond are unsaturated.
		/// </summary>
		public virtual bool isUnsaturated(IBond bond, IAtomContainer atomContainer)
		{
			//logger.debug("isBondUnsaturated?: ", bond);
			IAtom[] atoms = bond.getAtoms();
			bool isUnsaturated = true;
			for (int i = 0; i < atoms.Length && isUnsaturated; i++)
			{
				isUnsaturated = isUnsaturated && !isSaturated(atoms[i], atomContainer);
			}
			//logger.debug("Bond is unsaturated?: ", isUnsaturated);
			return isUnsaturated;
		}
		
		/// <summary> Tries to saturate a bond by increasing its bond orders by 1.0.
		/// 
		/// </summary>
		/// <returns> true if the bond could be increased
		/// </returns>
		public virtual bool saturateByIncreasingBondOrder(IBond bond, IAtomContainer atomContainer, double increment)
		{
			IAtom[] atoms = bond.getAtoms();
			IAtom atom = atoms[0];
			IAtom partner = atoms[1];
			//logger.debug("  saturating bond: ", atom.Symbol, "-", partner.Symbol);
			IAtomType[] atomTypes1 = getAtomTypeFactory(bond.Builder).getAtomTypes(atom.Symbol);
			IAtomType[] atomTypes2 = getAtomTypeFactory(bond.Builder).getAtomTypes(partner.Symbol);
			for (int atCounter1 = 0; atCounter1 < atomTypes1.Length; atCounter1++)
			{
				IAtomType aType1 = atomTypes1[atCounter1];
				//logger.debug("  condidering atom type: ", aType1);
				if (couldMatchAtomType(atomContainer, atom, aType1))
				{
					//logger.debug("  trying atom type: ", aType1);
					for (int atCounter2 = 0; atCounter2 < atomTypes2.Length; atCounter2++)
					{
						IAtomType aType2 = atomTypes2[atCounter2];
						//logger.debug("  condidering partner type: ", aType1);
						if (couldMatchAtomType(atomContainer, partner, atomTypes2[atCounter2]))
						{
							//logger.debug("    with atom type: ", aType2);
							if (bond.Order < aType2.MaxBondOrder && bond.Order < aType1.MaxBondOrder)
							{
								bond.Order = bond.Order + increment;
								//logger.debug("Bond order now ", bond.Order);
								return true;
							}
						}
					}
				}
			}
			return false;
		}
		
		/// <summary> Returns wether a bond is saturated. A bond is saturated if 
		/// <b>both</b> Atoms in the bond are saturated.
		/// </summary>
		public virtual bool isSaturated(IBond bond, IAtomContainer atomContainer)
		{
			//logger.debug("isBondSaturated?: ", bond);
			IAtom[] atoms = bond.getAtoms();
			bool isSaturated = true;
			for (int i = 0; i < atoms.Length; i++)
			{
				//logger.debug("isSaturated(Bond, AC): atom I=", i);
				isSaturated = isSaturated && this.isSaturated(atoms[i], atomContainer);
			}
			//logger.debug("isSaturated(Bond, AC): result=", isSaturated);
			return isSaturated;
		}
		
		/// <summary> Determines of all atoms on the AtomContainer are saturated.</summary>
		public virtual bool isSaturated(IAtomContainer container)
		{
			return allSaturated(container);
		}
		public virtual bool allSaturated(IAtomContainer ac)
		{
			//logger.debug("Are all atoms saturated?");
			for (int f = 0; f < ac.AtomCount; f++)
			{
				if (!isSaturated(ac.getAtomAt(f), ac))
				{
					return false;
				}
			}
			return true;
		}
		
		/// <summary> Determines if the atom can be of type AtomType. That is, it sees if this
		/// AtomType only differs in bond orders, or implicit hydrogen count.
		/// </summary>
		public virtual bool couldMatchAtomType(IAtom atom, double bondOrderSum, double maxBondOrder, IAtomType type)
		{
			//logger.debug("couldMatchAtomType:   ... matching atom ", atom, " vs ", type);
			int hcount = atom.getHydrogenCount();
			int charge = atom.getFormalCharge();
			if (charge == type.getFormalCharge())
			{
				//logger.debug("couldMatchAtomType:     formal charge matches...");
				if (atom.Hybridization == type.Hybridization)
				{
					//logger.debug("couldMatchAtomType:     hybridization is OK...");
					if (bondOrderSum + hcount <= type.BondOrderSum)
					{
						//logger.debug("couldMatchAtomType:     bond order sum is OK...");
						if (maxBondOrder <= type.MaxBondOrder)
						{
							//logger.debug("couldMatchAtomType:     max bond order is OK... We have a match!");
							return true;
						}
					}
					else
					{
						//logger.debug("couldMatchAtomType:      no match", "" + (bondOrderSum + hcount), " > ", "" + type.BondOrderSum);
					}
				}
			}
			else
			{
				//logger.debug("couldMatchAtomType:     formal charge does NOT match...");
			}
			//logger.debug("couldMatchAtomType:    No Match");
			return false;
		}
		
		/// <summary> Calculates the number of hydrogens that can be added to the given atom to fullfil
		/// the atom's valency. It will return 0 for PseudoAtoms, and for atoms for which it
		/// does not have an entry in the configuration file.
		/// </summary>
		public virtual int calculateNumberOfImplicitHydrogens(IAtom atom, double bondOrderSum, double maxBondOrder, int neighbourCount)
		{
			
			int missingHydrogens = 0;
			if (atom is IPseudoAtom)
			{
				//logger.debug("don't figure it out... it simply does not lack H's");
				return 0;
			}
			
			//logger.debug("Calculating number of missing hydrogen atoms");
			// get default atom
			IAtomType[] atomTypes = getAtomTypeFactory(atom.Builder).getAtomTypes(atom.Symbol);
			if (atomTypes.Length == 0)
			{
				//logger.warn("Element not found in configuration file: ", atom);
				return 0;
			}
			
			//logger.debug("Found atomtypes: ", atomTypes.Length);
			for (int f = 0; f < atomTypes.Length; f++)
			{
				IAtomType type = atomTypes[f];
				if (couldMatchAtomType(atom, bondOrderSum, maxBondOrder, type))
				{
					//logger.debug("This type matches: ", type);
					int formalNeighbourCount = type.FormalNeighbourCount;
					if (type.Hybridization == CDKConstants.HYBRIDIZATION_UNSET)
					{
						//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
						missingHydrogens = (int) (type.BondOrderSum - bondOrderSum);
					}
					else
					{
						switch (atom.Hybridization)
						{
							
							case CDKConstants.HYBRIDIZATION_SP3: 
								missingHydrogens = formalNeighbourCount - neighbourCount; break;
							
							case CDKConstants.HYBRIDIZATION_SP2: 
								missingHydrogens = formalNeighbourCount - neighbourCount; break;
							
							case CDKConstants.HYBRIDIZATION_SP1: 
								missingHydrogens = formalNeighbourCount - neighbourCount; break;
							
							default: 
								//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
								missingHydrogens = (int) (type.BondOrderSum - bondOrderSum);
								break;
							
						}
					}
					break;
				}
			}
			
			//logger.debug("missing hydrogens: ", missingHydrogens);
			return missingHydrogens;
		}
		
		/// <summary> Checks wether an Atom is saturated by comparing it with known AtomTypes.
		/// It returns true if the atom is an PseudoAtom and when the element is not in the list.
		/// </summary>
		public virtual bool isSaturated(IAtom atom, IAtomContainer container)
		{
			if (atom is IPseudoAtom)
			{
				//logger.debug("don't figure it out... it simply does not lack H's");
				return true;
			}
			
			IAtomType[] atomTypes = getAtomTypeFactory(atom.Builder).getAtomTypes(atom.Symbol);
			if (atomTypes.Length == 0)
			{
				//logger.warn("Missing entry in atom type list for ", atom.Symbol);
				return true;
			}
			double bondOrderSum = container.getBondOrderSum(atom);
			double maxBondOrder = container.getMaximumBondOrder(atom);
			int hcount = atom.getHydrogenCount();
			int charge = atom.getFormalCharge();
			
			//logger.debug("Checking saturation of atom ", atom.Symbol);
			//logger.debug("bondOrderSum: ", bondOrderSum);
			//logger.debug("maxBondOrder: ", maxBondOrder);
			//logger.debug("hcount: ", hcount);
			//logger.debug("charge: ", charge);
			
			bool elementPlusChargeMatches = false;
			for (int f = 0; f < atomTypes.Length; f++)
			{
				IAtomType type = atomTypes[f];
				if (couldMatchAtomType(atom, bondOrderSum, maxBondOrder, type))
				{
					if (bondOrderSum + hcount == type.BondOrderSum && maxBondOrder <= type.MaxBondOrder)
					{
						//logger.debug("We have a match: ", type);
						//logger.debug("Atom is saturated: ", atom.Symbol);
						return true;
					}
					else
					{
						// ok, the element and charge matche, but unfulfilled
						elementPlusChargeMatches = true;
					}
				} // else: formal charges don't match
			}
			
			if (elementPlusChargeMatches)
			{
				//logger.debug("No, atom is not saturated.");
				return false;
			}
			
			// ok, the found atom was not in the list
			throw new CDKException("The atom with element " + atom.Symbol + " and charge " + charge + " is not found.");
		}
		
		public virtual int calculateNumberOfImplicitHydrogens(IAtom atom, IAtomContainer container)
		{
			return this.calculateNumberOfImplicitHydrogens(atom, container.getBondOrderSum(atom), container.getMaximumBondOrder(atom), container.getConnectedAtoms(atom).Length);
		}
		
		protected internal virtual AtomTypeFactory getAtomTypeFactory(IChemObjectBuilder builder)
		{
			if (structgenATF == null)
			{
				try
				{
					structgenATF = AtomTypeFactory.getInstance(atomTypeList, builder);
				}
				catch (System.Exception exception)
				{
					//logger.debug(exception);
					throw new CDKException("Could not instantiate AtomTypeFactory!", exception);
				}
			}
			return structgenATF;
		}
		
		/// <summary> Determines if the atom can be of type AtomType.</summary>
		public virtual bool couldMatchAtomType(IAtomContainer container, IAtom atom, IAtomType type)
		{
			double bondOrderSum = container.getBondOrderSum(atom);
			double maxBondOrder = container.getMaximumBondOrder(atom);
			return couldMatchAtomType(atom, bondOrderSum, maxBondOrder, type);
		}
	}
}