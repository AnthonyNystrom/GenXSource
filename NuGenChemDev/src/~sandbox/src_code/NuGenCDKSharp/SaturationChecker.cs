/*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-07-14 13:07:00 +0200 (Fri, 14 Jul 2006) $
*  $Revision: 6671 $
*
*  Copyright (C) 2001-2006  The Chemistry Development Kit (CDK) project
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

using Org.OpenScience.CDK.Config;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Exception;
using System;
using Org.OpenScience.CDK.RingSearch;
using Org.OpenScience.CDK.Tools.Manipulator;
namespace Org.OpenScience.CDK.Tools
{
	
	/// <summary> Provides methods for checking whether an atoms valences are saturated with
	/// respect to a particular atom type.
	/// 
	/// <p>Important: this class does not deal with hybridization states, which makes
	/// it fail, for example, for situations where bonds are marked as aromatic (either
	/// 1.5 or single an AROMATIC).
	/// 
	/// </summary>
	/// <author>      steinbeck
	/// </author>
	/// <author>   Egon Willighagen
	/// </author>
	/// <cdk.created>     2001-09-04 </cdk.created>
	/// <summary> 
	/// </summary>
	/// <cdk.keyword>     saturation </cdk.keyword>
	/// <cdk.keyword>     atom, valency </cdk.keyword>
	/// <summary> 
	/// </summary>
	/// <cdk.module>      valencycheck </cdk.module>
	/// <cdk.bug>         1167386 </cdk.bug>
	public class SaturationChecker : IValencyChecker
	{
		
		internal AtomTypeFactory structgenATF;
		
//		private LoggingTool logger;
		
		public SaturationChecker()
		{
			//logger = new LoggingTool(this);
		}
		
		/// <param name="builder">the ChemObjectBuilder implementation used to construct the AtomType's.
		/// </param>
		protected internal virtual AtomTypeFactory getAtomTypeFactory(IChemObjectBuilder builder)
		{
			if (structgenATF == null)
			{
				try
				{
					structgenATF = AtomTypeFactory.getInstance("org/openscience/cdk/config/data/structgen_atomtypes.xml", builder);
				}
				catch (System.Exception exception)
				{
					//logger.debug(exception);
					throw new CDKException("Could not instantiate AtomTypeFactory!", exception);
				}
			}
			return structgenATF;
		}
		
		public virtual bool hasPerfectConfiguration(IAtom atom, IAtomContainer ac)
		{
			double bondOrderSum = ac.getBondOrderSum(atom);
			double maxBondOrder = ac.getMaximumBondOrder(atom);
			IAtomType[] atomTypes = getAtomTypeFactory(atom.Builder).getAtomTypes(atom.Symbol);
			if (atomTypes.Length == 0)
				return true;
			//logger.debug("*** Checking for perfect configuration ***");
			try
			{
				//logger.debug("Checking configuration of atom " + ac.getAtomNumber(atom));
				//logger.debug("Atom has bondOrderSum = " + bondOrderSum);
				//logger.debug("Atom has max = " + bondOrderSum);
			}
			catch (System.Exception exc)
			{
			}
			for (int f = 0; f < atomTypes.Length; f++)
			{
				if (bondOrderSum == atomTypes[f].BondOrderSum && maxBondOrder == atomTypes[f].MaxBondOrder)
				{
					try
					{
						//logger.debug("Atom " + ac.getAtomNumber(atom) + " has perfect configuration");
					}
					catch (System.Exception exc)
					{
					}
					return true;
				}
			}
			try
			{
				//logger.debug("*** Atom " + ac.getAtomNumber(atom) + " has imperfect configuration ***");
			}
			catch (System.Exception exc)
			{
			}
			return false;
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
		
		/// <summary> Returns wether a bond is unsaturated. A bond is unsaturated if 
		/// <b>both</b> Atoms in the bond are unsaturated.
		/// </summary>
		public virtual bool isUnsaturated(IBond bond, IAtomContainer atomContainer)
		{
			
			IAtom[] atoms = bond.getAtoms();
			bool isUnsaturated = true;
			for (int i = 0; i < atoms.Length; i++)
			{
				isUnsaturated = isUnsaturated && !isSaturated(atoms[i], atomContainer);
			}
			return isUnsaturated;
		}
		
		/// <summary> Returns wether a bond is saturated. A bond is saturated if 
		/// <b>both</b> Atoms in the bond are saturated.
		/// </summary>
		public virtual bool isSaturated(IBond bond, IAtomContainer atomContainer)
		{
			IAtom[] atoms = bond.getAtoms();
			bool isSaturated = true;
			for (int i = 0; i < atoms.Length; i++)
			{
				isSaturated = isSaturated && this.isSaturated(atoms[i], atomContainer);
			}
			return isSaturated;
		}
		
		/// <summary> Checks wether an Atom is saturated by comparing it with known AtomTypes.</summary>
		public virtual bool isSaturated(IAtom atom, IAtomContainer ac)
		{
			IAtomType[] atomTypes = getAtomTypeFactory(atom.Builder).getAtomTypes(atom.Symbol);
			if (atomTypes.Length == 0)
				return true;
			double bondOrderSum = ac.getBondOrderSum(atom);
			double maxBondOrder = ac.getMaximumBondOrder(atom);
			int hcount = atom.getHydrogenCount();
			int charge = atom.getFormalCharge();
			try
			{
				//logger.debug("*** Checking saturation of atom ", atom.Symbol, "" + ac.getAtomNumber(atom) + " ***");
				//logger.debug("bondOrderSum: " + bondOrderSum);
				//logger.debug("maxBondOrder: " + maxBondOrder);
				//logger.debug("hcount: " + hcount);
			}
			catch (System.Exception exc)
			{
				//logger.debug(exc);
			}
			for (int f = 0; f < atomTypes.Length; f++)
			{
				if (bondOrderSum - charge + hcount == atomTypes[f].BondOrderSum && maxBondOrder <= atomTypes[f].MaxBondOrder)
				{
					//logger.debug("*** Good ! ***");
					return true;
				}
			}
			//logger.debug("*** Bad ! ***");
			return false;
		}
		
		/// <summary> Checks if the current atom has exceeded its bond order sum value.
		/// 
		/// </summary>
		/// <param name="atom">The Atom to check
		/// </param>
		/// <param name="ac">  The atomcontainer context
		/// </param>
		/// <returns>      oversaturated or not
		/// </returns>
		public virtual bool isOverSaturated(IAtom atom, IAtomContainer ac)
		{
			IAtomType[] atomTypes = getAtomTypeFactory(atom.Builder).getAtomTypes(atom.Symbol);
			if (atomTypes.Length == 0)
				return false;
			double bondOrderSum = ac.getBondOrderSum(atom);
			double maxBondOrder = ac.getMaximumBondOrder(atom);
			int hcount = atom.getHydrogenCount();
			int charge = atom.getFormalCharge();
			try
			{
				//logger.debug("*** Checking saturation of atom " + ac.getAtomNumber(atom) + " ***");
				//logger.debug("bondOrderSum: " + bondOrderSum);
				//logger.debug("maxBondOrder: " + maxBondOrder);
				//logger.debug("hcount: " + hcount);
			}
			catch (System.Exception exc)
			{
			}
			for (int f = 0; f < atomTypes.Length; f++)
			{
				if (bondOrderSum - charge + hcount > atomTypes[f].BondOrderSum)
				{
					//logger.debug("*** Good ! ***");
					return true;
				}
			}
			//logger.debug("*** Bad ! ***");
			return false;
		}
		
		/// <summary> Returns the currently maximum formable bond order for this atom.
		/// 
		/// </summary>
		/// <param name="atom"> The atom to be checked
		/// </param>
		/// <param name="ac">   The AtomContainer that provides the context
		/// </param>
		/// <returns>       the currently maximum formable bond order for this atom
		/// </returns>
		public virtual double getCurrentMaxBondOrder(IAtom atom, IAtomContainer ac)
		{
			IAtomType[] atomTypes = getAtomTypeFactory(atom.Builder).getAtomTypes(atom.Symbol);
			if (atomTypes.Length == 0)
				return 0;
			double bondOrderSum = ac.getBondOrderSum(atom);
			int hcount = atom.getHydrogenCount();
			double max = 0;
			double current = 0;
			for (int f = 0; f < atomTypes.Length; f++)
			{
				current = hcount + bondOrderSum;
				if (atomTypes[f].BondOrderSum - current > max)
				{
					max = atomTypes[f].BondOrderSum - current;
				}
			}
			return max;
		}
		
		
		/// <summary> Resets the bond orders of all atoms to 1.0.</summary>
		public virtual void  unsaturate(IAtomContainer atomContainer)
		{
			unsaturate(atomContainer.Bonds);
		}
		
		/// <summary> Resets the bond order of the Bond to 1.0.</summary>
		public virtual void  unsaturate(IBond[] bonds)
		{
			for (int i = 1; i < bonds.Length; i++)
			{
				bonds[i].Order = 1.0;
			}
		}
		
		/// <summary> Saturates a molecule by setting appropriate bond orders.
		/// 
		/// </summary>
		/// <cdk.keyword>  bond order, calculation </cdk.keyword>
		/// <cdk.created>  2003-10-03 </cdk.created>
		public virtual void  newSaturate(IAtomContainer atomContainer)
		{
			//logger.info("Saturating atomContainer by adjusting bond orders...");
			bool allSaturated = this.allSaturated(atomContainer);
			if (!allSaturated)
			{
				bool succeeded = newSaturate(atomContainer.Bonds, atomContainer);
				IBond[] bonds = atomContainer.Bonds;
				for (int i = 0; i < bonds.Length; i++)
				{
					if (bonds[i].Order == 2 && bonds[i].getFlag(CDKConstants.ISAROMATIC) && (bonds[i].getAtomAt(0).Symbol.Equals("N") && bonds[i].getAtomAt(1).Symbol.Equals("N")))
					{
						int atomtohandle = 0;
						if (bonds[i].getAtomAt(0).Symbol.Equals("N"))
							atomtohandle = 1;
						IBond[] bondstohandle = atomContainer.getConnectedBonds(bonds[i].getAtomAt(atomtohandle));
						for (int k = 0; k < bondstohandle.Length; k++)
						{
							if (bondstohandle[k].Order == 1 && bondstohandle[k].getFlag(CDKConstants.ISAROMATIC))
							{
								bondstohandle[k].Order = 2;
								bonds[i].Order = 1;
								break;
							}
						}
					}
				}
				if (!succeeded)
				{
					throw new CDKException("Could not saturate this atomContainer!");
				}
			}
		}
		
		/// <summary> Saturates a set of Bonds in an AtomContainer.</summary>
		public virtual bool newSaturate(IBond[] bonds, IAtomContainer atomContainer)
		{
			//logger.debug("Saturating bond set of size: " + bonds.Length);
			bool bondsAreFullySaturated = true;
			if (bonds.Length > 0)
			{
				IBond bond = bonds[0];
				
				// determine bonds left
				int leftBondCount = bonds.Length - 1;
				IBond[] leftBonds = new IBond[leftBondCount];
				Array.Copy(bonds, 1, leftBonds, 0, leftBondCount);
				
				// examine this bond
				if (isUnsaturated(bond, atomContainer))
				{
					// either this bonds should be saturated or not
					
					// try to leave this bond unsaturated and saturate the left bondssaturate this bond
					if (leftBondCount > 0)
					{
						//logger.debug("Recursing with unsaturated bond with #bonds: " + leftBondCount);
						bondsAreFullySaturated = newSaturate(leftBonds, atomContainer) && !isUnsaturated(bond, atomContainer);
					}
					else
					{
						bondsAreFullySaturated = false;
					}
					
					// ok, did it work? if not, saturate this bond, and recurse
					if (!bondsAreFullySaturated)
					{
						//logger.debug("First try did not work...");
						// ok, revert saturating this bond, and recurse again
						bool couldSaturate = newSaturate(bond, atomContainer);
						if (couldSaturate)
						{
							if (leftBondCount > 0)
							{
								//logger.debug("Recursing with saturated bond with #bonds: " + leftBondCount);
								bondsAreFullySaturated = newSaturate(leftBonds, atomContainer);
							}
							else
							{
								bondsAreFullySaturated = true;
							}
						}
						else
						{
							bondsAreFullySaturated = false;
							// no need to recurse, because we already know that this bond
							// unsaturated does not work
						}
					}
				}
				else if (isSaturated(bond, atomContainer))
				{
					//logger.debug("This bond is already saturated.");
					if (leftBondCount > 0)
					{
						//logger.debug("Recursing with #bonds: " + leftBondCount);
						bondsAreFullySaturated = newSaturate(leftBonds, atomContainer);
					}
					else
					{
						bondsAreFullySaturated = true;
					}
				}
				else
				{
					//logger.debug("Cannot saturate this bond");
					// but, still recurse (if possible)
					if (leftBondCount > 0)
					{
						//logger.debug("Recursing with saturated bond with #bonds: " + leftBondCount);
						bondsAreFullySaturated = newSaturate(leftBonds, atomContainer) && !isUnsaturated(bond, atomContainer);
					}
					else
					{
						bondsAreFullySaturated = !isUnsaturated(bond, atomContainer);
					}
				}
			}
			//logger.debug("Is bond set fully saturated?: " + bondsAreFullySaturated);
			//logger.debug("Returning to level: " + (bonds.Length + 1));
			return bondsAreFullySaturated;
		}
		
		/// <summary> Saturate atom by adjusting its bond orders.</summary>
		public virtual bool newSaturate(IBond bond, IAtomContainer atomContainer)
		{
			IAtom[] atoms = bond.getAtoms();
			IAtom atom = atoms[0];
			IAtom partner = atoms[1];
			//logger.debug("  saturating bond: ", atom.Symbol, "-", partner.Symbol);
			IAtomType[] atomTypes1 = getAtomTypeFactory(bond.Builder).getAtomTypes(atom.Symbol);
			IAtomType[] atomTypes2 = getAtomTypeFactory(bond.Builder).getAtomTypes(partner.Symbol);
			bool bondOrderIncreased = true;
			while (bondOrderIncreased && !isSaturated(bond, atomContainer))
			{
				//logger.debug("Can increase bond order");
				bondOrderIncreased = false;
				for (int atCounter1 = 0; atCounter1 < atomTypes1.Length && !bondOrderIncreased; atCounter1++)
				{
					IAtomType aType1 = atomTypes1[atCounter1];
					//logger.debug("  condidering atom type: ", aType1);
					if (couldMatchAtomType(atomContainer, atom, aType1))
					{
						//logger.debug("  trying atom type: ", aType1);
						for (int atCounter2 = 0; atCounter2 < atomTypes2.Length && !bondOrderIncreased; atCounter2++)
						{
							IAtomType aType2 = atomTypes2[atCounter2];
							//logger.debug("  condidering partner type: ", aType1);
							if (couldMatchAtomType(atomContainer, partner, atomTypes2[atCounter2]))
							{
								//logger.debug("    with atom type: ", aType2);
								if (bond.Order >= aType2.MaxBondOrder || bond.Order >= aType1.MaxBondOrder)
								{
									//logger.debug("Bond order not increased: atoms has reached (or exceeded) maximum bond order for this atom type");
								}
								else if (bond.Order < aType2.MaxBondOrder && bond.Order < aType1.MaxBondOrder)
								{
									bond.Order = bond.Order + 1;
									//logger.debug("Bond order now " + bond.Order);
									bondOrderIncreased = true;
								}
							}
						}
					}
				}
			}
			return isSaturated(bond, atomContainer);
		}
		
		/// <summary> Determines if the atom can be of type AtomType.</summary>
		public virtual bool couldMatchAtomType(IAtomContainer atomContainer, IAtom atom, IAtomType atomType)
		{
			//logger.debug("   ... matching atom ", atom.Symbol, " vs ", atomType);
			if (atomContainer.getBondOrderSum(atom) + atom.getHydrogenCount() < atomType.BondOrderSum)
			{
				//logger.debug("    Match!");
				return true;
			}
			//logger.debug("    No Match");
			return false;
		}
		
		/// <summary> The method is known to fail for certain compounds. For more information, see
		/// cdk.test.limitations package.
		/// 
		/// </summary>
		public virtual void  saturate(IAtomContainer atomContainer)
		{
			/* newSaturate(atomContainer);
			}
			public void oldSaturate(AtomContainer atomContainer) throws CDKException { */
			IAtom partner = null;
			IAtom atom = null;
			IAtom[] partners = null;
			IAtomType[] atomTypes1 = null;
			IAtomType[] atomTypes2 = null;
			IBond bond = null;
			for (int i = 1; i < 4; i++)
			{
				// handle atoms with degree 1 first and then proceed to higher order
				for (int f = 0; f < atomContainer.AtomCount; f++)
				{
					atom = atomContainer.getAtomAt(f);
					//logger.debug("symbol: ", atom.Symbol);
					atomTypes1 = getAtomTypeFactory(atom.Builder).getAtomTypes(atom.Symbol);
					if (atomTypes1.Length > 0)
					{
						//logger.debug("first atom type: ", atomTypes1[0]);
						if (atomContainer.getBondCount(atom) == i)
						{
							if (atom.getFlag(CDKConstants.ISAROMATIC) && atomContainer.getBondOrderSum(atom) < atomTypes1[0].BondOrderSum - atom.getHydrogenCount())
							{
								partners = atomContainer.getConnectedAtoms(atom);
								for (int g = 0; g < partners.Length; g++)
								{
									partner = partners[g];
									//logger.debug("Atom has " + partners.Length + " partners");
									atomTypes2 = getAtomTypeFactory(atom.Builder).getAtomTypes(partner.Symbol);
									if (atomTypes2.Length == 0)
										return ;
									if (atomContainer.getBond(partner, atom).getFlag(CDKConstants.ISAROMATIC) && atomContainer.getBondOrderSum(partner) < atomTypes2[0].BondOrderSum - partner.getHydrogenCount())
									{
										//logger.debug("Partner has " + atomContainer.getBondOrderSum(partner) + ", may have: " + atomTypes2[0].BondOrderSum);
										bond = atomContainer.getBond(atom, partner);
										//logger.debug("Bond order was " + bond.Order);
										bond.Order = bond.Order + 1;
										//logger.debug("Bond order now " + bond.Order);
										break;
									}
								}
							}
							if (atomContainer.getBondOrderSum(atom) < atomTypes1[0].BondOrderSum - atom.getHydrogenCount())
							{
								//logger.debug("Atom has " + atomContainer.getBondOrderSum(atom) + ", may have: " + atomTypes1[0].BondOrderSum);
								partners = atomContainer.getConnectedAtoms(atom);
								for (int g = 0; g < partners.Length; g++)
								{
									partner = partners[g];
									//logger.debug("Atom has " + partners.Length + " partners");
									atomTypes2 = getAtomTypeFactory(atom.Builder).getAtomTypes(partner.Symbol);
									if (atomTypes2.Length == 0)
										return ;
									if (atomContainer.getBondOrderSum(partner) < atomTypes2[0].BondOrderSum - partner.getHydrogenCount())
									{
										//logger.debug("Partner has " + atomContainer.getBondOrderSum(partner) + ", may have: " + atomTypes2[0].BondOrderSum);
										bond = atomContainer.getBond(atom, partner);
										//logger.debug("Bond order was " + bond.Order);
										bond.Order = bond.Order + 1;
										//logger.debug("Bond order now " + bond.Order);
										break;
									}
								}
							}
						}
					}
				}
			}
		}
		
		public virtual void  saturateRingSystems(IAtomContainer atomContainer)
		{
			IRingSet rs = new SSSRFinder(atomContainer.Builder.newMolecule(atomContainer)).findSSSR();
			System.Collections.ArrayList ringSets = RingPartitioner.partitionRings(rs);
			IAtomContainer ac = null;
			IAtom atom = null;
			int[] temp;
			for (int f = 0; f < ringSets.Count; f++)
			{
				rs = (IRingSet) ringSets[f];
				ac = RingSetManipulator.getAllInOneContainer(rs);
				temp = new int[ac.AtomCount];
				for (int g = 0; g < ac.AtomCount; g++)
				{
					atom = ac.getAtomAt(g);
					temp[g] = atom.getHydrogenCount();
					atom.setHydrogenCount(atomContainer.getBondCount(atom) - ac.getBondCount(atom) - temp[g]);
				}
				saturate(ac);
				for (int g = 0; g < ac.AtomCount; g++)
				{
					atom = ac.getAtomAt(g);
					atom.setHydrogenCount(temp[g]);
				}
			}
		}
		
		/*
		* Recursivly fixes bond orders in a molecule for 
		* which only connectivities but no bond orders are know.
		*
		*@ param  molecule  The molecule to fix the bond orders for
		*@ param  bond      The number of the bond to treat in this recursion step
		*@ return           true if the bond order which was implemented was ok.
		*/
		/*private boolean recursiveBondOrderFix(Molecule molecule, int bondNumber)
		{	
		
		Atom partner = null;
		Atom atom = null;
		Atom[] partners = null;
		AtomType[] atomTypes1 = null;
		AtomType[] atomTypes2 = null;
		int maxBondOrder = 0;
		int oldBondOrder = 0;
		if (bondNumber < molecule.getBondCount())
		{	
		Bond bond = molecule.getBondAt(f);
		}
		else 
		{
		return true;
		}
		atom = bond.getAtomAt(0);
		partner = bond.getAtomAt(1);
		atomTypes1 = atf.getAtomTypes(atom.getSymbol(), atf.ATOMTYPE_ID_STRUCTGEN);
		atomTypes2 = atf.getAtomTypes(partner.getSymbol(), atf.ATOMTYPE_ID_STRUCTGEN);
		maxBondOrder = Math.min(atomTypes1[0].getMaxBondOrder(), atomTypes2[0].getMaxBondOrder());
		for (int f = 1; f <= maxBondOrder; f++)
		{
		oldBondOrder = bond.getOrder()
		bond.setOrder(f);
		if (!isOverSaturated(atom, molecule) && !isOverSaturated(partner, molecule))
		{
		if (!recursiveBondOrderFix(molecule, bondNumber + 1)) break;
		
		}
		else
		{
		bond.setOrder(oldBondOrder);
		return false;	
		}
		}
		return true;
		}*/
		
		/// <summary> Calculate the number of missing hydrogens by substracting the number of
		/// bonds for the atom from the expected number of bonds. Charges are included
		/// in the calculation. The number of expected bonds is defined by the AtomType
		/// generated with the AtomTypeFactory.
		/// 
		/// </summary>
		/// <param name="atom">     Description of the Parameter
		/// </param>
		/// <param name="container">Description of the Parameter
		/// </param>
		/// <returns>           Description of the Return Value
		/// </returns>
		/// <seealso cref="AtomTypeFactory">
		/// </seealso>
		public virtual int calculateNumberOfImplicitHydrogens(IAtom atom, IAtomContainer container)
		{
			return this.calculateNumberOfImplicitHydrogens(atom, container, false);
		}
		
		public virtual int calculateNumberOfImplicitHydrogens(IAtom atom)
		{
			IBond[] bonds = new IBond[0];
			return this.calculateNumberOfImplicitHydrogens(atom, 0, bonds, false);
		}
		
		public virtual int calculateNumberOfImplicitHydrogens(IAtom atom, IAtomContainer container, bool throwExceptionForUnknowAtom)
		{
			return this.calculateNumberOfImplicitHydrogens(atom, container.getBondOrderSum(atom), container.getConnectedBonds(atom), throwExceptionForUnknowAtom);
		}
		
		/// <summary> Calculate the number of missing hydrogens by substracting the number of
		/// bonds for the atom from the expected number of bonds. Charges are included
		/// in the calculation. The number of expected bonds is defined by the AtomType
		/// generated with the AtomTypeFactory.
		/// 
		/// </summary>
		/// <param name="atom">     Description of the Parameter
		/// </param>
		/// <param name="throwExceptionForUnknowAtom"> Should an exception be thrown if an unknown atomtype is found or 0 returned ?
		/// </param>
		/// <returns>           Description of the Return Value
		/// </returns>
		/// <seealso cref="AtomTypeFactory">
		/// </seealso>
		public virtual int calculateNumberOfImplicitHydrogens(IAtom atom, double bondOrderSum, IBond[] connectedBonds, bool throwExceptionForUnknowAtom)
		{
			int missingHydrogen = 0;
			if (atom is IPseudoAtom)
			{
				// don't figure it out... it simply does not lack H's
			}
			else if (atom.AtomicNumber == 1 || atom.Symbol.Equals("H"))
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				missingHydrogen = (int) (1 - bondOrderSum - atom.getFormalCharge());
			}
			else
			{
				//logger.info("Calculating number of missing hydrogen atoms");
				// get default atom
				IAtomType[] atomTypes = getAtomTypeFactory(atom.Builder).getAtomTypes(atom.Symbol);
				if (atomTypes.Length == 0 && throwExceptionForUnknowAtom)
					return 0;
				//logger.debug("Found atomtypes: " + atomTypes.Length);
				if (atomTypes.Length > 0)
				{
					IAtomType defaultAtom = atomTypes[0];
					//logger.debug("DefAtom: ", defaultAtom);
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					missingHydrogen = (int) (defaultAtom.BondOrderSum - bondOrderSum + atom.getFormalCharge());
					if (atom.getFlag(CDKConstants.ISAROMATIC))
					{
						bool subtractOne = true;
						for (int i = 0; i < connectedBonds.Length; i++)
						{
							if (connectedBonds[i].Order == 2 || connectedBonds[i].Order == CDKConstants.BONDORDER_AROMATIC)
								subtractOne = false;
						}
						if (subtractOne)
							missingHydrogen--;
					}
					//logger.debug("Atom: ", atom.Symbol);
					//logger.debug("  max bond order: " + defaultAtom.BondOrderSum);
					//logger.debug("  bond order sum: " + bondOrderSum);
					//logger.debug("  charge        : " + atom.getFormalCharge());
				}
				else
				{
					//logger.warn("Could not find atom type for ", atom.Symbol);
				}
			}
			return missingHydrogen;
		}
	}
}