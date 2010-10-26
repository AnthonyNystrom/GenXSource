/*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-07-14 13:07:00 +0200 (Fri, 14 Jul 2006) $
*  $Revision: 6671 $
*
*  Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
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
using System;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Graph;
using Org.OpenScience.CDK.Config;

namespace Org.OpenScience.CDK.Tools
{
	/// <summary> Provides methods for adding missing hydrogen atoms.
	/// 
	/// <p>An example:
	/// <pre>
	/// Molecule methane = new Molecule();
	/// Atom carbon = new Atom("C");
	/// methane.addAtom(carbon);
	/// HydrogenAdder adder = new HydrogenAdder();
	/// adder.addImplicitHydrogensToSatisfyValency(methane);
	/// int atomCount = methane.getAtomCount(); // = 1
	/// </pre>
	/// As the example shows, this only adjusts the hydrogenCount
	/// on the carbon.
	/// 
	/// <p>If you want to add the hydrogens as separate atoms, you
	/// need to do:
	/// <pre>
	/// Molecule methane = new Molecule();
	/// Atom carbon = new Atom("C");
	/// methane.addAtom(carbon);
	/// HydrogenAdder adder = new HydrogenAdder();
	/// adder.addExplicitHydrogensToSatisfyValency(methane);
	/// int atomCount = methane.getAtomCount(); // = 5
	/// </pre>
	/// 
	/// <p>If you want to add the hydrogens to a specific atom only,
	/// use this example:
	/// <pre>
	/// Molecule ethane = new Molecule();
	/// Atom carbon1 = new Atom("C");
	/// Atom carbon2 = new Atom("C");
	/// ethane.addAtom(carbon1);
	/// ethane.addAtom(carbon2);
	/// HydrogenAdder adder = new HydrogenAdder();
	/// adder.addExplicitHydrogensToSatisfyValency(ethane, carbon1);
	/// int atomCount = ethane.getAtomCount(); // = 5
	/// </pre>
	/// 
	/// </summary>
	/// <cdk.keyword>     hydrogen, adding </cdk.keyword>
	/// <cdk.module>      valencycheck </cdk.module>
	/// <cdk.bug>         1221810 </cdk.bug>
	/// <cdk.bug>         1244612 </cdk.bug>
	public class HydrogenAdder
	{
		
//		private LoggingTool logger;
		private IValencyChecker valencyChecker;
		
		/// <summary> Creates a tool to add missing hydrogens using the SaturationChecker class.
		/// 
		/// </summary>
		/// <seealso cref="org.openscience.cdk.tools.SaturationChecker">
		/// </seealso>
		public HydrogenAdder():this("org.openscience.cdk.tools.SaturationChecker")
		{
		}
		
		/// <summary> Creates a tool to add missing hydrogens using a ValencyCheckerInterface.
		/// 
		/// </summary>
		/// <seealso cref="org.openscience.cdk.tools.IValencyChecker">
		/// </seealso>
		public HydrogenAdder(System.String valencyCheckerInterfaceClassName)
		{
			//logger = new LoggingTool(this);
			try
			{
				if (valencyCheckerInterfaceClassName.Equals("org.openscience.cdk.tools.ValencyChecker"))
				{
					valencyChecker = new ValencyChecker();
				}
				else if (valencyCheckerInterfaceClassName.Equals("org.openscience.cdk.tools.SaturationChecker"))
				{
					valencyChecker = new SaturationChecker();
				}
				else
				{
					//logger.error("Cannot instantiate unknown ValencyCheckerInterface; using SaturationChecker");
					valencyChecker = new SaturationChecker();
				}
			}
			catch (System.Exception exception)
			{
				//logger.error("Could not intantiate a SaturationChecker.");
				//logger.debug(exception);
			}
		}
		
		/// <summary> Creates a tool to add missing hydrogens using a ValencyCheckerInterface.
		/// 
		/// </summary>
		/// <seealso cref="org.openscience.cdk.tools.IValencyChecker">
		/// </seealso>
		public HydrogenAdder(IValencyChecker valencyChecker)
		{
			//logger = new LoggingTool(this);
			this.valencyChecker = valencyChecker;
		}
		
		/// <summary> Method that saturates a molecule by adding explicit hydrogens.
		/// In order to get coordinates for these Hydrogens, you need to 
		/// remember the average bondlength of you molecule (coordinates for 
		/// all atoms should be available) by using
		/// double bondLength = GeometryTools.getBondLengthAverage(atomContainer);
		/// and then use this method here and then use
		/// org.openscience.cdk.HydrogenPlacer(atomContainer, bondLength);
		/// 
		/// </summary>
		/// <param name="molecule"> Molecule to saturate
		/// </param>
		/// <cdk.keyword>           hydrogen, adding </cdk.keyword>
		/// <cdk.keyword>           explicit hydrogen </cdk.keyword>
		public virtual IAtomContainer addHydrogensToSatisfyValency(IMolecule molecule)
		{
			//logger.debug("Start of addHydrogensToSatisfyValency");
			IAtomContainer changedAtomsAndBonds = addExplicitHydrogensToSatisfyValency(molecule);
			//logger.debug("End of addHydrogensToSatisfyValency");
			return changedAtomsAndBonds;
		}
		
		/// <summary> Method that saturates a molecule by adding explicit hydrogens.
		/// In order to get coordinates for these Hydrogens, you need to 
		/// remember the average bondlength of you molecule (coordinates for 
		/// all atoms should be available) by using
		/// double bondLength = GeometryTools.getBondLengthAverage(atomContainer);
		/// and then use this method here and then use
		/// org.openscience.cdk.HydrogenPlacer(atomContainer, bondLength);
		/// 
		/// </summary>
		/// <param name="molecule"> Molecule to saturate
		/// </param>
		/// <cdk.keyword>           hydrogen, adding </cdk.keyword>
		/// <cdk.keyword>           explicit hydrogen </cdk.keyword>
		public virtual IAtomContainer addExplicitHydrogensToSatisfyValency(IMolecule molecule)
		{
			//logger.debug("Start of addExplicitHydrogensToSatisfyValency");
			ISetOfMolecules moleculeSet = ConnectivityChecker.partitionIntoMolecules(molecule);
			IMolecule[] molecules = moleculeSet.Molecules;
			IAtomContainer changedAtomsAndBonds = molecule.Builder.newAtomContainer();
			IAtomContainer intermediateContainer = null;
			for (int k = 0; k < molecules.Length; k++)
			{
				IMolecule molPart = molecules[k];
				IAtom[] atoms = molPart.Atoms;
				for (int i = 0; i < atoms.Length; i++)
				{
					intermediateContainer = addHydrogensToSatisfyValency(molPart, atoms[i], molecule);
					changedAtomsAndBonds.add(intermediateContainer);
				}
			}
			//logger.debug("End of addExplicitHydrogensToSatisfyValency");
			return changedAtomsAndBonds;
		}
		
		/// <summary> Method that saturates an atom in a molecule by adding explicit hydrogens.
		/// In order to get coordinates for these Hydrogens, you need to 
		/// remember the average bondlength of you molecule (coordinates for 
		/// all atoms should be available) by using
		/// double bondLength = GeometryTools.getBondLengthAverage(atomContainer);
		/// and then use this method here and then use
		/// org.openscience.cdk.HydrogenPlacer(atomContainer, bondLength);
		/// 
		/// </summary>
		/// <param name="atom">     Atom to saturate
		/// </param>
		/// <param name="container">AtomContainer containing the atom
		/// </param>
		/// <param name="totalContainer">In case you have a container containing multiple structures, this is the total container, whereas container is a partial structure
		/// 
		/// </param>
		/// <cdk.keyword>           hydrogen, adding </cdk.keyword>
		/// <cdk.keyword>           explicit hydrogen </cdk.keyword>
		/// <summary> 
		/// </summary>
		/// <deprecated>
		/// </deprecated>
		public virtual IAtomContainer addHydrogensToSatisfyValency(IAtomContainer container, IAtom atom, IAtomContainer totalContainer)
		{
			//logger.debug("Start of addHydrogensToSatisfyValency(AtomContainer container, Atom atom)");
			IAtomContainer changedAtomsAndBonds = addExplicitHydrogensToSatisfyValency(container, atom, totalContainer);
			//logger.debug("End of addHydrogensToSatisfyValency(AtomContainer container, Atom atom)");
			return changedAtomsAndBonds;
		}
		
		/// <summary> Method that saturates an atom in a molecule by adding explicit hydrogens.
		/// In order to get coordinates for these Hydrogens, you need to 
		/// remember the average bondlength of you molecule (coordinates for 
		/// all atoms should be available) by using
		/// double bondLength = GeometryTools.getBondLengthAverage(atomContainer);
		/// and then use this method here and then use
		/// org.openscience.cdk.HydrogenPlacer(atomContainer, bondLength);
		/// 
		/// </summary>
		/// <param name="atom">     Atom to saturate
		/// </param>
		/// <param name="container">AtomContainer containing the atom
		/// </param>
		/// <param name="totalContainer">In case you have a container containing multiple structures, this is the total container, whereas container is a partial structure
		/// 
		/// </param>
		/// <cdk.keyword>           hydrogen, adding </cdk.keyword>
		/// <cdk.keyword>           explicit hydrogen </cdk.keyword>
		public virtual IAtomContainer addExplicitHydrogensToSatisfyValency(IAtomContainer container, IAtom atom, IAtomContainer totalContainer)
		{
			// set number of implicit hydrogens to zero
			// add explicit hydrogens
			//logger.debug("Start of addExplicitHydrogensToSatisfyValency(AtomContainer container, Atom atom)");
			int missingHydrogens = valencyChecker.calculateNumberOfImplicitHydrogens(atom, container);
			//logger.debug("According to valencyChecker, " + missingHydrogens + " are missing");
			IAtomContainer changedAtomsAndBonds = addExplicitHydrogensToSatisfyValency(container, atom, missingHydrogens, totalContainer);
			//logger.debug("End of addExplicitHydrogensToSatisfyValency(AtomContainer container, Atom atom)");
			return changedAtomsAndBonds;
		}
		
		/// <summary> Method that saturates an atom in a molecule by adding explicit hydrogens.
		/// 
		/// </summary>
		/// <param name="atom">     Atom to saturate
		/// </param>
		/// <param name="container">AtomContainer containing the atom
		/// </param>
		/// <param name="count">    Number of hydrogens to add
		/// </param>
		/// <param name="totalContainer">In case you have a container containing multiple structures, this is the total container, whereas container is a partial structure
		/// 
		/// </param>
		/// <cdk.keyword>           hydrogen, adding </cdk.keyword>
		/// <cdk.keyword>           explicit hydrogen </cdk.keyword>
		public virtual IAtomContainer addExplicitHydrogensToSatisfyValency(IAtomContainer container, IAtom atom, int count, IAtomContainer totalContainer)
		{
			//boolean create2DCoordinates = GeometryTools.has2DCoordinates(container);
			
			IIsotope isotope = IsotopeFactory.getInstance(container.Builder).getMajorIsotope("H");
			atom.setHydrogenCount(0);
			IAtomContainer changedAtomsAndBonds = container.Builder.newAtomContainer();
			for (int i = 1; i <= count; i++)
			{
				IAtom hydrogen = container.Builder.newAtom("H");
				IsotopeFactory.getInstance(container.Builder).configure(hydrogen, isotope);
				totalContainer.addAtom(hydrogen);
				IBond newBond = container.Builder.newBond((IAtom) atom, hydrogen, 1.0);
				totalContainer.addBond(newBond);
				changedAtomsAndBonds.addAtom(hydrogen);
				changedAtomsAndBonds.addBond(newBond);
			}
			return changedAtomsAndBonds;
		}
		
		/// <summary> Method that saturates a molecule by adding implicit hydrogens.
		/// 
		/// </summary>
		/// <param name="container"> Molecule to saturate
		/// </param>
		/// <cdk.keyword>           hydrogen, adding </cdk.keyword>
		/// <cdk.keyword>           implicit hydrogen </cdk.keyword>
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public virtual System.Collections.Hashtable addImplicitHydrogensToSatisfyValency(IAtomContainer container)
		{
			ISetOfMolecules moleculeSet = ConnectivityChecker.partitionIntoMolecules(container);
			IMolecule[] molecules = moleculeSet.Molecules;
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			System.Collections.Hashtable hydrogenAtomMap = new System.Collections.Hashtable();
			for (int k = 0; k < molecules.Length; k++)
			{
				IMolecule molPart = molecules[k];
				IAtom[] atoms = molPart.Atoms;
				for (int f = 0; f < atoms.Length; f++)
				{
					int[] hydrogens = addImplicitHydrogensToSatisfyValency(molPart, atoms[f]);
					hydrogenAtomMap[atoms[f]] = hydrogens;
				}
			}
			return hydrogenAtomMap;
		}
		
		/// <summary> Method that saturates an atom in a molecule by adding implicit hydrogens.
		/// 
		/// </summary>
		/// <param name="container"> Molecule to saturate
		/// </param>
		/// <param name="atom">     Atom to satureate.
		/// </param>
		/// <cdk.keyword>           hydrogen, adding </cdk.keyword>
		/// <cdk.keyword>           implicit hydrogen </cdk.keyword>
		public virtual int[] addImplicitHydrogensToSatisfyValency(IAtomContainer container, IAtom atom)
		{
			int formerHydrogens = atom.getHydrogenCount();
			int missingHydrogens = valencyChecker.calculateNumberOfImplicitHydrogens(atom, container);
			atom.setHydrogenCount(missingHydrogens);
			int[] hydrogens = new int[2];
			hydrogens[0] = formerHydrogens;
			hydrogens[1] = missingHydrogens;
			return hydrogens;
		}
		
		/*
		* Method that saturates an atom by adding implicit hydrogens.
		*
		* @param  atom      Atom to satureate.
		*
		public void addImplicitHydrogensToSatisfyValency(Atom atom) throws CDKException
		{
		int missingHydrogens = valencyChecker.calculateNumberOfImplicitHydrogens(atom);
		atom.setHydrogenCount(missingHydrogens);
		} */
	}
}