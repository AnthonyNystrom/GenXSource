/*
*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-07-02 13:48:44 +0200 (Sun, 02 Jul 2006) $
*  $Revision: 6537 $
*
*  Copyright (C) 2002-2006  The Chemistry Development Kit (CDK) Project
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
using Org.OpenScience.CDK.RingSearch;
using Org.OpenScience.CDK.Config;
using Org.OpenScience.CDK.Graph.Invariant;
using Support;
using Org.OpenScience.CDK.Exception;
using Org.OpenScience.CDK.Graph;
using Org.OpenScience.CDK.Aromaticity;
using Org.OpenScience.CDK.Tools.Manipulator;
using Org.OpenScience.CDK.Geometry;

namespace Org.OpenScience.CDK.Smiles
{
    /// <summary> Generates SMILES strings {@cdk.cite WEI88, WEI89}. It takes into account the
    /// isotope and formal charge information of the atoms. In addition to this it
    /// takes stereochemistry in account for both Bond's and Atom's. IMPORTANT: The
    /// aromaticity detection for this SmilesGenerator relies on AllRingsFinder,
    /// which is known to take very long for some molecules with many cycles or
    /// special cyclic topologies. Thus, the AllRingsFinder has a built-in timeout
    /// of 5 seconds after which it aborts and throws an Exception. If you want your
    /// SMILES generated at any expense, you need to create your own AllRingsFinder,
    /// set the timeout to a higher value, and assign it to this SmilesGenerator. In
    /// the vast majority of cases, however, the defaults will be fine.
    /// 
    /// </summary>
    /// <author>          Oliver Horlacher,
    /// </author>
    /// <author>          Stefan Kuhn (chiral smiles)
    /// </author>
    /// <cdk.created>     2002-02-26 </cdk.created>
    /// <cdk.keyword>     SMILES, generator </cdk.keyword>
    /// <cdk.module>      smiles </cdk.module>
    /// <cdk.bug>         1014344 </cdk.bug>
    /// <cdk.bug>         1257438 </cdk.bug>
    /// <cdk.bug>         1494527 </cdk.bug>
    public class SmilesGenerator
    {
        //UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassComparator' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
        private class AnonymousClassComparator : System.Collections.IComparer
        {
            public AnonymousClassComparator(SmilesGenerator enclosingInstance)
            {
                InitBlock(enclosingInstance);
            }
            private void InitBlock(SmilesGenerator enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }
            private SmilesGenerator enclosingInstance;
            public SmilesGenerator Enclosing_Instance
            {
                get
                {
                    return enclosingInstance;
                }

            }
            public virtual int Compare(System.Object o1, System.Object o2)
            {
                return (int)((long)((System.Int64)((IAtom)o1).getProperty("CanonicalLable")) - (long)((System.Int64)((IAtom)o2).getProperty("CanonicalLable")));
            }
        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary>  Returns the current AllRingsFinder instance
        /// 
        /// </summary>
        /// <returns>   the current AllRingsFinder instance
        /// </returns>
        /// <summary>  Sets the current AllRingsFinder instance
        /// Use this if you want to customize the timeout for 
        /// the AllRingsFinder. AllRingsFinder is stopping its 
        /// quest to find all rings after a default of 5 seconds.
        /// 
        /// </summary>
        /// <seealso cref="org.openscience.cdk.ringsearch.AllRingsFinder">
        /// 
        /// </seealso>
        /// <param name="ringFinder"> The value to assign ringFinder.
        /// </param>
        virtual public AllRingsFinder RingFinder
        {
            get
            {
                return ringFinder;
            }

            set
            {
                this.ringFinder = value;
            }

        }
        //private final static boolean debug = false;

        /// <summary>  The number of rings that have been opened</summary>
        private int ringMarker = 0;

        /// <summary>  Collection of all the bonds that were broken</summary>
        private System.Collections.ArrayList brokenBonds = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

        /// <summary>  The isotope factory which is used to write the mass is needed</summary>
        private IsotopeFactory isotopeFactory;

        internal AllRingsFinder ringFinder;

        /// <summary> RingSet that holds all rings of the molecule</summary>
        private IRingSet rings = null;

        /// <summary>  The canonical labler</summary>
        private CanonicalLabeler canLabler = new CanonicalLabeler();
        //UPGRADE_NOTE: Final was removed from the declaration of 'RING_CONFIG '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private System.String RING_CONFIG = "stereoconfig";
        //UPGRADE_NOTE: Final was removed from the declaration of 'UP '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private System.String UP = "up";
        //UPGRADE_NOTE: Final was removed from the declaration of 'DOWN '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private System.String DOWN = "down";

        //private IChemObjectBuilder builder;

        /// <summary>  Default constructor</summary>
        public SmilesGenerator(IChemObjectBuilder builder)
        {
            //this.builder = builder;
            try
            {
                isotopeFactory = IsotopeFactory.getInstance(builder);
            }
            catch (System.IO.IOException e)
            {
                SupportClass.WriteStackTrace(e, Console.Error);
            }
            //UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
            catch (System.Exception e)
            {
                //UPGRADE_ISSUE: Method 'java.lang.ClassNotFoundException.printStackTrace' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassNotFoundExceptionprintStackTrace'"
                //e.printStackTrace();
            }
        }


        /// <summary>  Tells if a certain bond is center of a valid double bond configuration.
        /// 
        /// </summary>
        /// <param name="container"> The atomcontainer.
        /// </param>
        /// <param name="bond">      The bond.
        /// </param>
        /// <returns>            true=is a potential configuration, false=is not.
        /// </returns>
        public virtual bool isValidDoubleBondConfiguration(IAtomContainer container, IBond bond)
        {
            IAtom[] atoms = bond.getAtoms();
            IAtom[] connectedAtoms = container.getConnectedAtoms(atoms[0]);
            IAtom from = null;
            for (int i = 0; i < connectedAtoms.Length; i++)
            {
                if (connectedAtoms[i] != atoms[1])
                {
                    from = connectedAtoms[i];
                }
            }
            bool[] array = new bool[container.Bonds.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = true;
            }
            if (isStartOfDoubleBond(container, atoms[0], from, array) && isEndOfDoubleBond(container, atoms[1], atoms[0], array) && !bond.getFlag(CDKConstants.ISAROMATIC))
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }

        /// <summary> Provide a reference to a RingSet that holds ALL rings of the molecule.<BR>
        /// During creation of a SMILES the aromaticity of the molecule has to be detected.
        /// This, in turn, requires the dermination of all rings of the molecule. If this
        /// computationally expensive calculation has been done beforehand, a RingSet can
        /// be handed over to the SmilesGenerator to save the effort of another all-rings-
        /// calculation.
        /// 
        /// </summary>
        /// <param name="rings"> RingSet that holds ALL rings of the molecule
        /// </param>
        /// <returns>        reference to the SmilesGenerator object this method was called for
        /// </returns>
        public virtual SmilesGenerator setRings(IRingSet rings)
        {
            this.rings = rings;
            return this;
        }

        /// <summary>  Generate canonical SMILES from the <code>molecule</code>. This method
        /// canonicaly lables the molecule but does not perform any checks on the
        /// chemical validity of the molecule.
        /// IMPORTANT: A precomputed Set of All Rings (SAR) can be passed to this 
        /// SmilesGenerator in order to avoid recomputing it. Use setRings() to 
        /// assign the SAR.
        /// 
        /// </summary>
        /// <param name="molecule"> The molecule to evaluate
        /// </param>
        /// <returns>           Description of the Returned Value
        /// </returns>
        /// <seealso cref="org.openscience.cdk.graph.invariant.CanonicalLabeler.canonLabel(IAtomContainer)">
        /// </seealso>
        //UPGRADE_NOTE: Synchronized keyword was removed from method 'createSMILES'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
        public virtual System.String createSMILES(IMolecule molecule)
        {
            lock (this)
            {
                try
                {
                    return (createSMILES(molecule, false, new bool[molecule.getBondCount()]));
                }
                catch (CDKException exception)
                {
                    // This exception can only happen if a chiral smiles is requested
                    return ("");
                }
            }
        }


        /// <summary>  Generate a SMILES for the given <code>Reaction</code>.
        /// 
        /// </summary>
        /// <param name="reaction">         Description of the Parameter
        /// </param>
        /// <returns>                   Description of the Return Value
        /// </returns>
        /// <exception cref="CDKException"> Description of the Exception
        /// </exception>
        //UPGRADE_NOTE: Synchronized keyword was removed from method 'createSMILES'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
        public virtual System.String createSMILES(IReaction reaction)
        {
            lock (this)
            {
                System.Text.StringBuilder reactionSMILES = new System.Text.StringBuilder();
                IMolecule[] reactants = reaction.Reactants.Molecules;
                for (int i = 0; i < reactants.Length; i++)
                {
                    reactionSMILES.Append(createSMILES(reactants[i]));
                    if (i + 1 < reactants.Length)
                    {
                        reactionSMILES.Append('.');
                    }
                }
                reactionSMILES.Append('>');
                IMolecule[] agents = reaction.Agents.Molecules;
                for (int i = 0; i < agents.Length; i++)
                {
                    reactionSMILES.Append(createSMILES(agents[i]));
                    if (i + 1 < agents.Length)
                    {
                        reactionSMILES.Append('.');
                    }
                }
                reactionSMILES.Append('>');
                IMolecule[] products = reaction.Products.Molecules;
                for (int i = 0; i < products.Length; i++)
                {
                    reactionSMILES.Append(createSMILES(products[i]));
                    if (i + 1 < products.Length)
                    {
                        reactionSMILES.Append('.');
                    }
                }
                return reactionSMILES.ToString();
            }
        }


        /// <summary>  Generate canonical and chiral SMILES from the <code>molecule</code>. This
        /// method canonicaly lables the molecule but dose not perform any checks on
        /// the chemical validity of the molecule. The chiral smiles is done like in
        /// the <a href="http://www.daylight.com/dayhtml/doc/theory/theory.smiles.html">
        /// daylight theory manual</a> . I did not find rules for canonical and chiral
        /// smiles, therefore there is no guarantee that the smiles complies to any
        /// externeal rules, but it is canonical compared to other smiles produced by
        /// this method. The method checks if there are 2D coordinates but does not
        /// check if coordinates make sense. Invalid stereo configurations are ignored;
        /// if there are no valid stereo configuration the smiles will be the same as
        /// the non-chiral one. Note that often stereo configurations are only complete
        /// and can be converted to a smiles if explicit Hs are given.
        /// IMPORTANT: A precomputed Set of All Rings (SAR) can be passed to this 
        /// SmilesGenerator in order to avoid recomputing it. Use setRings() to 
        /// assign the SAR.
        /// 
        /// </summary>
        /// <param name="molecule">                The molecule to evaluate
        /// </param>
        /// <param name="doubleBondConfiguration"> Description of Parameter
        /// </param>
        /// <returns>                          Description of the Returned Value
        /// </returns>
        /// <exception cref="CDKException">        At least one atom has no Point2D;
        /// coordinates are needed for creating the chiral smiles.
        /// </exception>
        /// <seealso cref="org.openscience.cdk.graph.invariant.CanonicalLabeler.canonLabel(IAtomContainer)">
        /// </seealso>
        //UPGRADE_NOTE: Synchronized keyword was removed from method 'createChiralSMILES'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
        public virtual System.String createChiralSMILES(IMolecule molecule, bool[] doubleBondConfiguration)
        {
            lock (this)
            {
                return (createSMILES(molecule, true, doubleBondConfiguration));
            }
        }


        /// <summary>  Generate canonical SMILES from the <code>molecule</code>. This method
        /// canonicaly lables the molecule but dose not perform any checks on the
        /// chemical validity of the molecule. This method also takes care of multiple
        /// molecules.
        /// IMPORTANT: A precomputed Set of All Rings (SAR) can be passed to this 
        /// SmilesGenerator in order to avoid recomputing it. Use setRings() to 
        /// assign the SAR.
        /// 
        /// </summary>
        /// <param name="molecule">                The molecule to evaluate
        /// </param>
        /// <param name="chiral">                  true=SMILES will be chiral, false=SMILES
        /// will not be chiral.
        /// </param>
        /// <param name="doubleBondConfiguration"> Description of Parameter
        /// </param>
        /// <returns>                          Description of the Returned Value
        /// </returns>
        /// <exception cref="CDKException">        At least one atom has no Point2D;
        /// coordinates are needed for crating the chiral smiles. This excpetion
        /// can only be thrown if chiral smiles is created, ignore it if you want a
        /// non-chiral smiles (createSMILES(AtomContainer) does not throw an
        /// exception).
        /// </exception>
        /// <seealso cref="org.openscience.cdk.graph.invariant.CanonicalLabeler.canonLabel(IAtomContainer)">
        /// </seealso>
        //UPGRADE_NOTE: Synchronized keyword was removed from method 'createSMILES'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
        public virtual System.String createSMILES(IMolecule molecule, bool chiral, bool[] doubleBondConfiguration)
        {
            lock (this)
            {
                ISetOfMolecules moleculeSet = ConnectivityChecker.partitionIntoMolecules(molecule);
                if (moleculeSet.MoleculeCount > 1)
                {
                    System.Text.StringBuilder fullSMILES = new System.Text.StringBuilder();
                    IMolecule[] molecules = moleculeSet.Molecules;
                    for (int i = 0; i < molecules.Length; i++)
                    {
                        IMolecule molPart = molecules[i];
                        fullSMILES.Append(createSMILESWithoutCheckForMultipleMolecules(molPart, chiral, doubleBondConfiguration));
                        if (i < (molecules.Length - 1))
                        {
                            // are there more molecules?
                            fullSMILES.Append('.');
                        }
                    }
                    return fullSMILES.ToString();
                }
                else
                {
                    return (createSMILESWithoutCheckForMultipleMolecules(molecule, chiral, doubleBondConfiguration));
                }
            }
        }


        /// <summary>  Generate canonical SMILES from the <code>molecule</code>. This method
        /// canonicaly lables the molecule but dose not perform any checks on the
        /// chemical validity of the molecule. Does not care about multiple molecules.
        /// IMPORTANT: A precomputed Set of All Rings (SAR) can be passed to this 
        /// SmilesGenerator in order to avoid recomputing it. Use setRings() to 
        /// assign the SAR.
        /// 
        /// </summary>
        /// <param name="molecule">                The molecule to evaluate
        /// </param>
        /// <param name="chiral">                  true=SMILES will be chiral, false=SMILES
        /// will not be chiral.
        /// </param>
        /// <param name="doubleBondConfiguration"> Description of Parameter
        /// </param>
        /// <returns>                          Description of the Returned Value
        /// </returns>
        /// <exception cref="CDKException">        At least one atom has no Point2D;
        /// coordinates are needed for creating the chiral smiles. This excpetion
        /// can only be thrown if chiral smiles is created, ignore it if you want a
        /// non-chiral smiles (createSMILES(AtomContainer) does not throw an
        /// exception).
        /// </exception>
        /// <seealso cref="org.openscience.cdk.graph.invariant.CanonicalLabeler.canonLabel(IAtomContainer)">
        /// </seealso>
        //UPGRADE_NOTE: Synchronized keyword was removed from method 'createSMILESWithoutCheckForMultipleMolecules'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
        public virtual System.String createSMILESWithoutCheckForMultipleMolecules(IMolecule molecule, bool chiral, bool[] doubleBondConfiguration)
        {
            lock (this)
            {
                if (molecule.AtomCount == 0)
                {
                    return "";
                }
                canLabler.canonLabel(molecule);
                brokenBonds.Clear();
                ringMarker = 0;
                IAtom[] all = molecule.Atoms;
                IAtom start = null;
                for (int i = 0; i < all.Length; i++)
                {
                    IAtom atom = all[i];
                    if (chiral && atom.getPoint2d() == null)
                    {
                        throw new CDKException("Atom number " + i + " has no 2D coordinates, but 2D coordinates are needed for creating chiral smiles");
                    }
                    //System.out.println("Setting all VISITED flags to false");
                    atom.setFlag(CDKConstants.VISITED, false);
                    if ((long)((System.Int64)atom.getProperty("CanonicalLable")) == 1)
                    {
                        start = atom;
                    }
                }

                //detect aromaticity
                if (rings == null)
                {
                    if (ringFinder == null)
                    {
                        ringFinder = new AllRingsFinder();
                    }
                    rings = ringFinder.findAllRings(molecule);
                }
                HueckelAromaticityDetector.detectAromaticity(molecule, rings, false);
                if (chiral && rings.AtomContainerCount > 0)
                {
                    System.Collections.ArrayList v = RingPartitioner.partitionRings(rings);
                    //System.out.println("RingSystems: " + v.size());
                    for (int i = 0; i < v.Count; i++)
                    {
                        int counter = 0;
                        IAtomContainer allrings = RingSetManipulator.getAllInOneContainer((IRingSet)v[i]);
                        for (int k = 0; k < allrings.AtomCount; k++)
                        {
                            if (!BondTools.isStereo(molecule, allrings.getAtomAt(k)) && hasWedges(molecule, allrings.getAtomAt(k)) != null)
                            {
                                IBond bond = molecule.getBond(allrings.getAtomAt(k), hasWedges(molecule, allrings.getAtomAt(k)));
                                if (bond.Stereo == CDKConstants.STEREO_BOND_UP)
                                {
                                    allrings.getAtomAt(k).setProperty(RING_CONFIG, UP);
                                }
                                else
                                {
                                    allrings.getAtomAt(k).setProperty(RING_CONFIG, DOWN);
                                }
                                counter++;
                            }
                        }
                        if (counter == 1)
                        {
                            for (int k = 0; k < allrings.AtomCount; k++)
                            {
                                allrings.getAtomAt(k).setProperty(RING_CONFIG, UP);
                            }
                        }
                    }
                }

                System.Text.StringBuilder l = new System.Text.StringBuilder();
                createSMILES(start, l, molecule, chiral, doubleBondConfiguration);
                rings = null;
                return l.ToString();
            }
        }


        /// <summary>  Description of the Method
        /// 
        /// </summary>
        /// <param name="ac"> Description of the Parameter
        /// </param>
        /// <param name="a">  Description of the Parameter
        /// </param>
        /// <returns>     Description of the Return Value
        /// </returns>
        private IAtom hasWedges(IAtomContainer ac, IAtom a)
        {
            IAtom[] atoms = ac.getConnectedAtoms(a);
            for (int i = 0; i < atoms.Length; i++)
            {
                if (ac.getBond(a, atoms[i]).Stereo != CDKConstants.STEREO_BOND_NONE && !atoms[i].Symbol.Equals("H"))
                {
                    return (atoms[i]);
                }
            }
            for (int i = 0; i < atoms.Length; i++)
            {
                if (ac.getBond(a, atoms[i]).Stereo != CDKConstants.STEREO_BOND_NONE)
                {
                    return (atoms[i]);
                }
            }
            return (null);
        }


        /// <summary>  Says if an atom is the end of a double bond configuration
        /// 
        /// </summary>
        /// <param name="atom">                    The atom which is the end of configuration
        /// </param>
        /// <param name="container">               The atomContainer the atom is in
        /// </param>
        /// <param name="parent">                  The atom we came from
        /// </param>
        /// <param name="doubleBondConfiguration"> The array indicating where double bond
        /// configurations are specified (this method ensures that there is
        /// actually the possibility of a double bond configuration)
        /// </param>
        /// <returns>                          false=is not end of configuration, true=is
        /// </returns>
        private bool isEndOfDoubleBond(IAtomContainer container, IAtom atom, IAtom parent, bool[] doubleBondConfiguration)
        {
            if (container.getBondNumber(atom, parent) == -1 || doubleBondConfiguration.Length <= container.getBondNumber(atom, parent) || !doubleBondConfiguration[container.getBondNumber(atom, parent)])
            {
                return false;
            }
            int lengthAtom = container.getConnectedAtoms(atom).Length + atom.getHydrogenCount();
            int lengthParent = container.getConnectedAtoms(parent).Length + parent.getHydrogenCount();
            if (container.getBond(atom, parent) != null)
            {
                if (container.getBond(atom, parent).Order == CDKConstants.BONDORDER_DOUBLE && (lengthAtom == 3 || (lengthAtom == 2 && atom.Symbol.Equals("N"))) && (lengthParent == 3 || (lengthParent == 2 && parent.Symbol.Equals("N"))))
                {
                    IAtom[] atoms = container.getConnectedAtoms(atom);
                    IAtom one = null;
                    IAtom two = null;
                    for (int i = 0; i < atoms.Length; i++)
                    {
                        if (atoms[i] != parent && one == null)
                        {
                            one = atoms[i];
                        }
                        else if (atoms[i] != parent && one != null)
                        {
                            two = atoms[i];
                        }
                    }
                    System.String[] morgannumbers = MorganNumbersTools.getMorganNumbersWithElementSymbol(container);
                    if ((one != null && two == null && atom.Symbol.Equals("N") && System.Math.Abs(BondTools.giveAngleBothMethods(parent, atom, one, true)) > System.Math.PI / 10) || (!atom.Symbol.Equals("N") && one != null && two != null && !morgannumbers[container.getAtomNumber(one)].Equals(morgannumbers[container.getAtomNumber(two)])))
                    {
                        return (true);
                    }
                    else
                    {
                        return (false);
                    }
                }
            }
            return (false);
        }


        /// <summary>  Says if an atom is the start of a double bond configuration
        /// 
        /// </summary>
        /// <param name="a">                       The atom which is the start of configuration
        /// </param>
        /// <param name="container">               The atomContainer the atom is in
        /// </param>
        /// <param name="parent">                  The atom we came from
        /// </param>
        /// <param name="doubleBondConfiguration"> The array indicating where double bond
        /// configurations are specified (this method ensures that there is
        /// actually the possibility of a double bond configuration)
        /// </param>
        /// <returns>                          false=is not start of configuration, true=is
        /// </returns>
        private bool isStartOfDoubleBond(IAtomContainer container, IAtom a, IAtom parent, bool[] doubleBondConfiguration)
        {
            int lengthAtom = container.getConnectedAtoms(a).Length + a.getHydrogenCount();
            if (lengthAtom != 3 && (lengthAtom != 2 && (System.Object)a.Symbol != (System.Object)("N")))
            {
                return (false);
            }
            IAtom[] atoms = container.getConnectedAtoms(a);
            IAtom one = null;
            IAtom two = null;
            bool doubleBond = false;
            IAtom nextAtom = null;
            for (int i = 0; i < atoms.Length; i++)
            {
                if (atoms[i] != parent && container.getBond(atoms[i], a).Order == CDKConstants.BONDORDER_DOUBLE && isEndOfDoubleBond(container, atoms[i], a, doubleBondConfiguration))
                {
                    doubleBond = true;
                    nextAtom = atoms[i];
                }
                if (atoms[i] != nextAtom && one == null)
                {
                    one = atoms[i];
                }
                else if (atoms[i] != nextAtom && one != null)
                {
                    two = atoms[i];
                }
            }
            System.String[] morgannumbers = MorganNumbersTools.getMorganNumbersWithElementSymbol(container);
            if (one != null && ((!a.Symbol.Equals("N") && two != null && !morgannumbers[container.getAtomNumber(one)].Equals(morgannumbers[container.getAtomNumber(two)]) && doubleBond && doubleBondConfiguration[container.getBondNumber(a, nextAtom)]) || (doubleBond && a.Symbol.Equals("N") && System.Math.Abs(BondTools.giveAngleBothMethods(nextAtom, a, parent, true)) > System.Math.PI / 10)))
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }


        /// <summary>  Gets the bondBroken attribute of the SmilesGenerator object
        /// 
        /// </summary>
        /// <param name="a1"> Description of Parameter
        /// </param>
        /// <param name="a2"> Description of Parameter
        /// </param>
        /// <returns>     The bondBroken value
        /// </returns>
        private bool isBondBroken(IAtom a1, IAtom a2)
        {
            System.Collections.IEnumerator it = brokenBonds.GetEnumerator();
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (it.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                BrokenBond bond = ((BrokenBond)it.Current);
                if ((bond.A1.Equals(a1) || bond.A1.Equals(a2)) && (bond.A2.Equals(a1) || bond.A2.Equals(a2)))
                {
                    return (true);
                }
            }
            return false;
        }


        /// <summary>  Determines if the atom <code>a</code> is a atom with a ring marker.
        /// 
        /// </summary>
        /// <param name="a"> the atom to test
        /// </param>
        /// <returns>    true if the atom participates in a bond that was broken in the
        /// first pass.
        /// </returns>
        //	private boolean isRingOpening(IAtom a)
        //	{
        //		Iterator it = brokenBonds.iterator();
        //		while (it.hasNext())
        //		{
        //			BrokenBond bond = (BrokenBond) it.next();
        //			if (bond.getA1().equals(a) || bond.getA2().equals(a))
        //			{
        //				return true;
        //			}
        //		}
        //		return false;
        //	}


        /// <summary>  Determines if the atom <code>a</code> is a atom with a ring marker.
        /// 
        /// </summary>
        /// <param name="a1"> Description of Parameter
        /// </param>
        /// <param name="v">  Description of the Parameter
        /// </param>
        /// <returns>     true if the atom participates in a bond that was broken in the
        /// first pass.
        /// </returns>
        private bool isRingOpening(IAtom a1, System.Collections.ArrayList v)
        {
            System.Collections.IEnumerator it = brokenBonds.GetEnumerator();
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (it.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                BrokenBond bond = (BrokenBond)it.Current;
                for (int i = 0; i < v.Count; i++)
                {
                    if ((bond.A1.Equals(a1) && bond.A2.Equals((IAtom)v[i])) || (bond.A1.Equals((IAtom)v[i]) && bond.A2.Equals(a1)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>  Return the neighbours of atom <code>a</code> in canonical order with the
        /// atoms that have high bond order at the front.
        /// 
        /// </summary>
        /// <param name="a">         the atom whose neighbours are to be found.
        /// </param>
        /// <param name="container"> the AtomContainer that is being parsed.
        /// </param>
        /// <returns>            Vector of atoms in canonical oreder.
        /// </returns>
        private System.Collections.IList getCanNeigh(IAtom a, IAtomContainer container)
        {
            System.Collections.IList v = container.getConnectedAtomsVector(a);
            if (v.Count > 1)
            {
                SupportClass.CollectionsSupport.Sort(v, new AnonymousClassComparator(this));
            }
            return v;
        }


        /// <summary>  Gets the ringOpenings attribute of the SmilesGenerator object
        /// 
        /// </summary>
        /// <param name="a">      Description of Parameter
        /// </param>
        /// <param name="vbonds"> Description of the Parameter
        /// </param>
        /// <returns>         The ringOpenings value
        /// </returns>
        private System.Collections.ArrayList getRingOpenings(IAtom a, System.Collections.ArrayList vbonds)
        {
            System.Collections.IEnumerator it = brokenBonds.GetEnumerator();
            System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (it.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                BrokenBond bond = (BrokenBond)it.Current;
                if (bond.A1.Equals(a) || bond.A2.Equals(a))
                {
                    v.Add((System.Int32)bond.Marker);
                    if (vbonds != null)
                    {
                        vbonds.Add(bond.A1.Equals(a) ? bond.A2 : bond.A1);
                    }
                }
            }
            SupportClass.CollectionsSupport.Sort(v, null);
            return v;
        }


        /// <summary>  Returns true if the <code>atom</code> in the <code>container</code> has
        /// been marked as a chiral center by the user.
        /// 
        /// </summary>
        /// <param name="atom">      Description of Parameter
        /// </param>
        /// <param name="container"> Description of Parameter
        /// </param>
        /// <returns>            The chiralCenter value
        /// </returns>
        //	private boolean isChiralCenter(IAtom atom, IAtomContainer container)
        //	{
        //		IBond[] bonds = container.getConnectedBonds(atom);
        //		for (int i = 0; i < bonds.length; i++)
        //		{
        //			IBond bond = bonds[i];
        //			int stereo = bond.getStereo();
        //			if (stereo == CDKConstants.STEREO_BOND_DOWN ||
        //					stereo == CDKConstants.STEREO_BOND_UP)
        //			{
        //				return true;
        //			}
        //		}
        //		return false;
        //	}


        /// <summary>  Gets the last atom object (not Vector) in a Vector as created by
        /// createDSFTree.
        /// 
        /// </summary>
        /// <param name="v">      The Vector
        /// </param>
        /// <param name="result"> The feature to be added to the Atoms attribute
        /// </param>
        private void addAtoms(System.Collections.ArrayList v, System.Collections.ArrayList result)
        {
            for (int i = 0; i < v.Count; i++)
            {
                if (v[i] is IAtom)
                {
                    result.Add((IAtom)v[i]);
                }
                else
                {
                    addAtoms((System.Collections.ArrayList)v[i], result);
                }
            }
        }


        /// <summary>  Performes a DFS search on the <code>atomContainer</code>. Then parses the
        /// resulting tree to create the SMILES string.
        /// 
        /// </summary>
        /// <param name="a">                       the atom to start the search at.
        /// </param>
        /// <param name="line">                    the StringBuffer that the SMILES is to be
        /// appended to.
        /// </param>
        /// <param name="chiral">                  true=SMILES will be chiral, false=SMILES
        /// will not be chiral.
        /// </param>
        /// <param name="atomContainer">           the AtomContainer that the SMILES string is
        /// generated for.
        /// </param>
        /// <param name="doubleBondConfiguration"> Description of Parameter
        /// </param>
        private void createSMILES(IAtom a, System.Text.StringBuilder line, IAtomContainer atomContainer, bool chiral, bool[] doubleBondConfiguration)
        {
            System.Collections.ArrayList tree = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            createDFSTree(a, tree, null, atomContainer);
            //System.out.println("Done with tree");
            parseChain(tree, line, atomContainer, null, chiral, doubleBondConfiguration, System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10)));
        }


        /// <summary>  Recursively perform a DFS search on the <code>container</code> placing
        /// atoms and branches in the vector <code>tree</code>.
        /// 
        /// </summary>
        /// <param name="a">         the atom being visited.
        /// </param>
        /// <param name="tree">      vector holding the tree.
        /// </param>
        /// <param name="parent">    the atom we came from.
        /// </param>
        /// <param name="container"> the AtomContainer that we are parsing.
        /// </param>
        private void createDFSTree(IAtom a, System.Collections.ArrayList tree, IAtom parent, IAtomContainer container)
        {
            tree.Add(a);
            System.Collections.IList neighbours = getCanNeigh(a, container);
            neighbours.Remove(parent);
            IAtom next;
            a.setFlag(CDKConstants.VISITED, true);
            //System.out.println("Starting with DFSTree and AtomContainer of size " + container.getAtomCount());
            //System.out.println("Current Atom has " + neighbours.size() + " neighbours");
            System.Collections.IEnumerator iter = neighbours.GetEnumerator();
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (iter.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                next = (IAtom)iter.Current;
                if (!next.getFlag(CDKConstants.VISITED))
                {
                    //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                    if (!iter.MoveNext())
                    {
                        //Last neighbour therefore in this chain
                        createDFSTree(next, tree, a, container);
                    }
                    else
                    {
                        System.Collections.ArrayList branch = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
                        tree.Add(branch);
                        //System.out.println("adding branch");
                        createDFSTree(next, branch, a, container);
                    }
                }
                else
                {
                    //Found ring closure between next and a
                    //System.out.println("found ringclosure in DFTTreeCreation");
                    ringMarker++;
                    BrokenBond bond = new BrokenBond(this, a, next, ringMarker);
                    if (!brokenBonds.Contains(bond))
                    {
                        brokenBonds.Add(bond);
                    }
                    else
                    {
                        ringMarker--;
                    }
                }
            }
        }


        /// <summary>  Parse a branch
        /// 
        /// </summary>
        /// <param name="v">                       Description of Parameter
        /// </param>
        /// <param name="buffer">                  Description of Parameter
        /// </param>
        /// <param name="container">               Description of Parameter
        /// </param>
        /// <param name="parent">                  Description of Parameter
        /// </param>
        /// <param name="chiral">                  Description of Parameter
        /// </param>
        /// <param name="doubleBondConfiguration"> Description of Parameter
        /// </param>
        /// <param name="atomsInOrderOfSmiles">    Description of Parameter
        /// </param>
        private void parseChain(System.Collections.ArrayList v, System.Text.StringBuilder buffer, IAtomContainer container, IAtom parent, bool chiral, bool[] doubleBondConfiguration, System.Collections.ArrayList atomsInOrderOfSmiles)
        {
            int positionInVector = 0;
            IAtom atom;
            //System.out.println("in parse chain. Size of tree: " + v.size());
            for (int h = 0; h < v.Count; h++)
            {
                System.Object o = v[h];
                if (o is IAtom)
                {
                    atom = (IAtom)o;
                    if (parent != null)
                    {
                        parseBond(buffer, atom, parent, container);
                    }
                    else
                    {
                        if (chiral && BondTools.isStereo(container, atom))
                        {
                            parent = (IAtom)((System.Collections.ArrayList)v[1])[0];
                        }
                    }
                    parseAtom(atom, buffer, container, chiral, doubleBondConfiguration, parent, atomsInOrderOfSmiles, v);
                    //System.out.println("in parseChain after parseAtom()");
                    /*
                    *  The principle of making chiral smiles is quite simple, although the code is
                    *  pretty uggly. The Atoms connected to the chiral center are put in sorted[] in the
                    *  order they have to appear in the smiles. Then the Vector v is rearranged according
                    *  to sorted[]
                    */
                    if (chiral && BondTools.isStereo(container, atom) && container.getBond(parent, atom) != null)
                    {
                        //System.out.println("in parseChain in isChiral");
                        IAtom[] sorted = null;
                        System.Collections.IList chiralNeighbours = container.getConnectedAtomsVector(atom);
                        if (BondTools.isTetrahedral(container, atom, false) > 0)
                        {
                            sorted = new IAtom[3];
                        }
                        if (BondTools.isTetrahedral(container, atom, false) == 1)
                        {
                            if (container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_DOWN)
                            {
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (chiralNeighbours[i] != parent)
                                    {
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == 0 && BondTools.isLeft(((IAtom)chiralNeighbours[i]), parent, atom) && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[2] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == 0 && !BondTools.isLeft(((IAtom)chiralNeighbours[i]), parent, atom) && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[1] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_UP && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[0] = (IAtom)chiralNeighbours[i];
                                        }
                                    }
                                }
                            }
                            if (container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_UP)
                            {
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (chiralNeighbours[i] != parent)
                                    {
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == 0 && BondTools.isLeft(((IAtom)chiralNeighbours[i]), parent, atom) && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[1] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == 0 && !BondTools.isLeft(((IAtom)chiralNeighbours[i]), parent, atom) && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[2] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_DOWN && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[0] = (IAtom)chiralNeighbours[i];
                                        }
                                    }
                                }
                            }
                            if (container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_UNDEFINED || container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_NONE)
                            {
                                bool normalBindingIsLeft = false;
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (chiralNeighbours[i] != parent)
                                    {
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == 0)
                                        {
                                            if (BondTools.isLeft(((IAtom)chiralNeighbours[i]), parent, atom))
                                            {
                                                normalBindingIsLeft = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (chiralNeighbours[i] != parent)
                                    {
                                        if (normalBindingIsLeft)
                                        {
                                            if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == 0)
                                            {
                                                sorted[0] = (IAtom)chiralNeighbours[i];
                                            }
                                            if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_UP)
                                            {
                                                sorted[2] = (IAtom)chiralNeighbours[i];
                                            }
                                            if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_DOWN)
                                            {
                                                sorted[1] = (IAtom)chiralNeighbours[i];
                                            }
                                        }
                                        else
                                        {
                                            if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_UP)
                                            {
                                                sorted[1] = (IAtom)chiralNeighbours[i];
                                            }
                                            if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == 0)
                                            {
                                                sorted[0] = (IAtom)chiralNeighbours[i];
                                            }
                                            if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_DOWN)
                                            {
                                                sorted[2] = (IAtom)chiralNeighbours[i];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (BondTools.isTetrahedral(container, atom, false) == 2)
                        {
                            if (container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_UP)
                            {
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (chiralNeighbours[i] != parent)
                                    {
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_DOWN && BondTools.isLeft(((IAtom)chiralNeighbours[i]), parent, atom) && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[1] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_DOWN && !BondTools.isLeft(((IAtom)chiralNeighbours[i]), parent, atom) && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[2] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_UP && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[0] = (IAtom)chiralNeighbours[i];
                                        }
                                    }
                                }
                            }
                            if (container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_DOWN)
                            {
                                double angle1 = 0;
                                double angle2 = 0;
                                IAtom atom1 = null;
                                IAtom atom2 = null;
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (chiralNeighbours[i] != parent)
                                    {
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_UP && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            if (angle1 == 0)
                                            {
                                                angle1 = BondTools.giveAngle(atom, parent, (IAtom)chiralNeighbours[i]);
                                                atom1 = (IAtom)chiralNeighbours[i];
                                            }
                                            else
                                            {
                                                angle2 = BondTools.giveAngle(atom, parent, (IAtom)chiralNeighbours[i]);
                                                atom2 = (IAtom)chiralNeighbours[i];
                                            }
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_DOWN && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[1] = (IAtom)chiralNeighbours[i];
                                        }
                                    }
                                }
                                if (angle1 < angle2)
                                {
                                    sorted[0] = atom2;
                                    sorted[2] = atom1;
                                }
                                else
                                {
                                    sorted[0] = atom1;
                                    sorted[2] = atom2;
                                }
                            }
                        }
                        if (BondTools.isTetrahedral(container, atom, false) == 3)
                        {
                            if (container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_UP)
                            {
                                //UPGRADE_ISSUE: Class hierarchy differences between 'java.util.TreeMap' and 'System.Collections.SortedList' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
                                //UPGRADE_TODO: Constructor 'java.util.TreeMap.TreeMap' was converted to 'System.Collections.SortedList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilTreeMapTreeMap'"
                                System.Collections.SortedList hm = new System.Collections.SortedList();
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (chiralNeighbours[i] != parent && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                    {
                                        hm[(double)BondTools.giveAngle(atom, parent, ((IAtom)chiralNeighbours[i]))] = (System.Int32)i;
                                    }
                                }
                                //UPGRADE_TODO: Method 'java.util.TreeMap.values' was converted to 'System.Collections.SortedList.Values' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilTreeMapvalues'"
                                System.Object[] ohere = SupportClass.ICollectionSupport.ToArray(hm.Values);
                                for (int i = ohere.Length - 1; i > -1; i--)
                                {
                                    sorted[i] = ((IAtom)chiralNeighbours[((System.Int32)ohere[i])]);
                                }
                            }
                            if (container.getBond(parent, atom).Stereo == 0)
                            {
                                double angle1 = 0;
                                double angle2 = 0;
                                IAtom atom1 = null;
                                IAtom atom2 = null;
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (chiralNeighbours[i] != parent)
                                    {
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == 0 && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            if (angle1 == 0)
                                            {
                                                angle1 = BondTools.giveAngle(atom, parent, (IAtom)chiralNeighbours[i]);
                                                atom1 = (IAtom)chiralNeighbours[i];
                                            }
                                            else
                                            {
                                                angle2 = BondTools.giveAngle(atom, parent, (IAtom)chiralNeighbours[i]);
                                                atom2 = (IAtom)chiralNeighbours[i];
                                            }
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_UP && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[0] = (IAtom)chiralNeighbours[i];
                                        }
                                    }
                                }
                                if (angle1 < angle2)
                                {
                                    sorted[1] = atom2;
                                    sorted[2] = atom1;
                                }
                                else
                                {
                                    sorted[1] = atom1;
                                    sorted[2] = atom2;
                                }
                            }
                        }
                        if (BondTools.isTetrahedral(container, atom, false) == 4)
                        {
                            if (container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_DOWN)
                            {
                                //UPGRADE_ISSUE: Class hierarchy differences between 'java.util.TreeMap' and 'System.Collections.SortedList' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
                                //UPGRADE_TODO: Constructor 'java.util.TreeMap.TreeMap' was converted to 'System.Collections.SortedList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilTreeMapTreeMap'"
                                System.Collections.SortedList hm = new System.Collections.SortedList();
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (chiralNeighbours[i] != parent && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                    {
                                        hm[(double)BondTools.giveAngle(atom, parent, ((IAtom)chiralNeighbours[i]))] = (System.Int32)i;
                                    }
                                }
                                //UPGRADE_TODO: Method 'java.util.TreeMap.values' was converted to 'System.Collections.SortedList.Values' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilTreeMapvalues'"
                                System.Object[] ohere = SupportClass.ICollectionSupport.ToArray(hm.Values);
                                for (int i = ohere.Length - 1; i > -1; i--)
                                {
                                    sorted[i] = ((IAtom)chiralNeighbours[((System.Int32)ohere[i])]);
                                }
                            }
                            if (container.getBond(parent, atom).Stereo == 0)
                            {
                                double angle1 = 0;
                                double angle2 = 0;
                                IAtom atom1 = null;
                                IAtom atom2 = null;
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (chiralNeighbours[i] != parent)
                                    {
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == 0 && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            if (angle1 == 0)
                                            {
                                                angle1 = BondTools.giveAngle(atom, parent, (IAtom)chiralNeighbours[i]);
                                                atom1 = (IAtom)chiralNeighbours[i];
                                            }
                                            else
                                            {
                                                angle2 = BondTools.giveAngle(atom, parent, (IAtom)chiralNeighbours[i]);
                                                atom2 = (IAtom)chiralNeighbours[i];
                                            }
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_DOWN && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[2] = (IAtom)chiralNeighbours[i];
                                        }
                                    }
                                }
                                if (angle1 < angle2)
                                {
                                    sorted[1] = atom2;
                                    sorted[0] = atom1;
                                }
                                else
                                {
                                    sorted[1] = atom1;
                                    sorted[0] = atom2;
                                }
                            }
                        }
                        if (BondTools.isTetrahedral(container, atom, false) == 5)
                        {
                            if (container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_DOWN)
                            {
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (chiralNeighbours[i] != parent)
                                    {
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_UP)
                                        {
                                            sorted[0] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == 0)
                                        {
                                            sorted[2] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_DOWN)
                                        {
                                            sorted[1] = (IAtom)chiralNeighbours[i];
                                        }
                                    }
                                }
                            }
                            if (container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_UP)
                            {
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (chiralNeighbours[i] != parent)
                                    {
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_DOWN && BondTools.isLeft(((IAtom)chiralNeighbours[i]), parent, atom) && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[0] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_DOWN && !BondTools.isLeft(((IAtom)chiralNeighbours[i]), parent, atom) && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[2] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == 0)
                                        {
                                            sorted[1] = (IAtom)chiralNeighbours[i];
                                        }
                                    }
                                }
                            }
                            if (container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_UNDEFINED || container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_NONE)
                            {
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (chiralNeighbours[i] != parent)
                                    {
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_DOWN && BondTools.isLeft(((IAtom)chiralNeighbours[i]), parent, atom) && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[0] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_DOWN && !BondTools.isLeft(((IAtom)chiralNeighbours[i]), parent, atom) && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[2] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_UP)
                                        {
                                            sorted[1] = (IAtom)chiralNeighbours[i];
                                        }
                                    }
                                }
                            }
                        }
                        if (BondTools.isTetrahedral(container, atom, false) == 6)
                        {
                            if (container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_UP)
                            {
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (chiralNeighbours[i] != parent)
                                    {
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_UP)
                                        {
                                            sorted[0] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == 0)
                                        {
                                            sorted[2] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_DOWN)
                                        {
                                            sorted[1] = (IAtom)chiralNeighbours[i];
                                        }
                                    }
                                }
                            }
                            if (container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_DOWN)
                            {
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (chiralNeighbours[i] != parent)
                                    {
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_UP && BondTools.isLeft(((IAtom)chiralNeighbours[i]), parent, atom) && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[2] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_UP && !BondTools.isLeft(((IAtom)chiralNeighbours[i]), parent, atom) && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[0] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == 0)
                                        {
                                            sorted[1] = (IAtom)chiralNeighbours[i];
                                        }
                                    }
                                }
                            }
                            if (container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_UNDEFINED || container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_NONE)
                            {
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (chiralNeighbours[i] != parent)
                                    {
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_UP && BondTools.isLeft(((IAtom)chiralNeighbours[i]), parent, atom) && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[2] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_UP && !BondTools.isLeft(((IAtom)chiralNeighbours[i]), parent, atom) && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                        {
                                            sorted[0] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond((IAtom)chiralNeighbours[i], atom).Stereo == CDKConstants.STEREO_BOND_DOWN)
                                        {
                                            sorted[1] = (IAtom)chiralNeighbours[i];
                                        }
                                    }
                                }
                            }
                        }
                        if (BondTools.isSquarePlanar(container, atom))
                        {
                            sorted = new IAtom[3];
                            //This produces a U=SP1 order in every case
                            //UPGRADE_ISSUE: Class hierarchy differences between 'java.util.TreeMap' and 'System.Collections.SortedList' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
                            //UPGRADE_TODO: Constructor 'java.util.TreeMap.TreeMap' was converted to 'System.Collections.SortedList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilTreeMapTreeMap'"
                            System.Collections.SortedList hm = new System.Collections.SortedList();
                            for (int i = 0; i < chiralNeighbours.Count; i++)
                            {
                                if (chiralNeighbours[i] != parent && !isBondBroken((IAtom)chiralNeighbours[i], atom))
                                {
                                    hm[(double)BondTools.giveAngle(atom, parent, ((IAtom)chiralNeighbours[i]))] = (System.Int32)i;
                                }
                            }
                            //UPGRADE_TODO: Method 'java.util.TreeMap.values' was converted to 'System.Collections.SortedList.Values' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilTreeMapvalues'"
                            System.Object[] ohere = SupportClass.ICollectionSupport.ToArray(hm.Values);
                            for (int i = 0; i < ohere.Length; i++)
                            {
                                sorted[i] = ((IAtom)chiralNeighbours[((System.Int32)ohere[i])]);
                            }
                        }
                        if (BondTools.isTrigonalBipyramidalOrOctahedral(container, atom) != 0)
                        {
                            sorted = new IAtom[container.getConnectedAtoms(atom).Length - 1];
                            //UPGRADE_ISSUE: Class hierarchy differences between 'java.util.TreeMap' and 'System.Collections.SortedList' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
                            //UPGRADE_TODO: Constructor 'java.util.TreeMap.TreeMap' was converted to 'System.Collections.SortedList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilTreeMapTreeMap'"
                            System.Collections.SortedList hm = new System.Collections.SortedList();
                            if (container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_UP)
                            {
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (container.getBond(atom, (IAtom)chiralNeighbours[i]).Stereo == 0)
                                    {
                                        hm[(double)BondTools.giveAngle(atom, parent, ((IAtom)chiralNeighbours[i]))] = (System.Int32)i;
                                    }
                                    if (container.getBond(atom, (IAtom)chiralNeighbours[i]).Stereo == CDKConstants.STEREO_BOND_DOWN)
                                    {
                                        sorted[sorted.Length - 1] = (IAtom)chiralNeighbours[i];
                                    }
                                }
                                //UPGRADE_TODO: Method 'java.util.TreeMap.values' was converted to 'System.Collections.SortedList.Values' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilTreeMapvalues'"
                                System.Object[] ohere = SupportClass.ICollectionSupport.ToArray(hm.Values);
                                for (int i = 0; i < ohere.Length; i++)
                                {
                                    sorted[i] = ((IAtom)chiralNeighbours[((System.Int32)ohere[i])]);
                                }
                            }
                            if (container.getBond(parent, atom).Stereo == CDKConstants.STEREO_BOND_DOWN)
                            {
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (container.getBond(atom, (IAtom)chiralNeighbours[i]).Stereo == 0)
                                    {
                                        hm[(double)BondTools.giveAngle(atom, parent, ((IAtom)chiralNeighbours[i]))] = (System.Int32)i;
                                    }
                                    if (container.getBond(atom, (IAtom)chiralNeighbours[i]).Stereo == CDKConstants.STEREO_BOND_UP)
                                    {
                                        sorted[sorted.Length - 1] = (IAtom)chiralNeighbours[i];
                                    }
                                }
                                //UPGRADE_TODO: Method 'java.util.TreeMap.values' was converted to 'System.Collections.SortedList.Values' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilTreeMapvalues'"
                                System.Object[] ohere = SupportClass.ICollectionSupport.ToArray(hm.Values);
                                for (int i = 0; i < ohere.Length; i++)
                                {
                                    sorted[i] = ((IAtom)chiralNeighbours[((System.Int32)ohere[i])]);
                                }
                            }
                            if (container.getBond(parent, atom).Stereo == 0)
                            {
                                for (int i = 0; i < chiralNeighbours.Count; i++)
                                {
                                    if (chiralNeighbours[i] != parent)
                                    {
                                        if (container.getBond(atom, (IAtom)chiralNeighbours[i]).Stereo == 0)
                                        {
                                            hm[(double)(BondTools.giveAngleFromMiddle(atom, parent, ((IAtom)chiralNeighbours[i])))] = (System.Int32)i;
                                        }
                                        if (container.getBond(atom, (IAtom)chiralNeighbours[i]).Stereo == CDKConstants.STEREO_BOND_UP)
                                        {
                                            sorted[0] = (IAtom)chiralNeighbours[i];
                                        }
                                        if (container.getBond(atom, (IAtom)chiralNeighbours[i]).Stereo == CDKConstants.STEREO_BOND_DOWN)
                                        {
                                            sorted[sorted.Length - 2] = (IAtom)chiralNeighbours[i];
                                        }
                                    }
                                }
                                //UPGRADE_TODO: Method 'java.util.TreeMap.values' was converted to 'System.Collections.SortedList.Values' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilTreeMapvalues'"
                                System.Object[] ohere = SupportClass.ICollectionSupport.ToArray(hm.Values);
                                sorted[sorted.Length - 1] = ((IAtom)chiralNeighbours[((System.Int32)ohere[ohere.Length - 1])]);
                                if (ohere.Length == 2)
                                {
                                    sorted[sorted.Length - 3] = ((IAtom)chiralNeighbours[((System.Int32)ohere[0])]);
                                    if (BondTools.giveAngleFromMiddle(atom, parent, ((IAtom)chiralNeighbours[((System.Int32)ohere[1])])) < 0)
                                    {
                                        IAtom dummy = sorted[sorted.Length - 2];
                                        sorted[sorted.Length - 2] = sorted[0];
                                        sorted[0] = dummy;
                                    }
                                }
                                if (ohere.Length == 3)
                                {
                                    sorted[sorted.Length - 3] = sorted[sorted.Length - 2];
                                    sorted[sorted.Length - 2] = ((IAtom)chiralNeighbours[((System.Int32)ohere[ohere.Length - 2])]);
                                    sorted[sorted.Length - 4] = ((IAtom)chiralNeighbours[((System.Int32)ohere[ohere.Length - 3])]);
                                }
                            }
                        }
                        //This builds an onew[] containing the objects after the center of the chirality in the order given by sorted[]
                        if (sorted != null)
                        {
                            int numberOfAtoms = 3;
                            if (BondTools.isTrigonalBipyramidalOrOctahedral(container, atom) != 0)
                            {
                                numberOfAtoms = container.getConnectedAtoms(atom).Length - 1;
                            }
                            System.Object[] omy = new System.Object[numberOfAtoms];
                            System.Object[] onew = new System.Object[numberOfAtoms];
                            for (int k = getRingOpenings(atom, null).Count; k < numberOfAtoms; k++)
                            {
                                if (positionInVector + 1 + k - getRingOpenings(atom, null).Count < v.Count)
                                {
                                    omy[k] = v[positionInVector + 1 + k - getRingOpenings(atom, null).Count];
                                }
                            }
                            for (int k = 0; k < sorted.Length; k++)
                            {
                                if (sorted[k] != null)
                                {
                                    for (int m = 0; m < omy.Length; m++)
                                    {
                                        if (omy[m] is IAtom)
                                        {
                                            if (omy[m] == sorted[k])
                                            {
                                                onew[k] = omy[m];
                                            }
                                        }
                                        else
                                        {
                                            if (omy[m] == null)
                                            {
                                                onew[k] = null;
                                            }
                                            else
                                            {
                                                if (((System.Collections.ArrayList)omy[m])[0] == sorted[k])
                                                {
                                                    onew[k] = omy[m];
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    onew[k] = null;
                                }
                            }
                            //This is a workaround for 3624.MOL.2 I don't have a better solution currently
                            bool doubleentry = false;
                            for (int m = 0; m < onew.Length; m++)
                            {
                                for (int k = 0; k < onew.Length; k++)
                                {
                                    if (m != k && onew[k] == onew[m])
                                    {
                                        doubleentry = true;
                                    }
                                }
                            }
                            if (!doubleentry)
                            {
                                //Make sure that the first atom in onew is the first one in the original smiles order. This is important to have a canonical smiles.
                                if (positionInVector + 1 < v.Count)
                                {
                                    System.Object atomAfterCenterInOriginalSmiles = v[positionInVector + 1];
                                    int l = 0;
                                    while (onew[0] != atomAfterCenterInOriginalSmiles)
                                    {
                                        System.Object placeholder = onew[onew.Length - 1];
                                        for (int k = onew.Length - 2; k > -1; k--)
                                        {
                                            onew[k + 1] = onew[k];
                                        }
                                        onew[0] = placeholder;
                                        l++;
                                        if (l > onew.Length)
                                        {
                                            break;
                                        }
                                    }
                                }
                                //This cares about ring openings. Here the ring closure (represendted by a figure) must be the first atom. In onew the closure is null.
                                if (getRingOpenings(atom, null).Count > 0)
                                {
                                    int l = 0;
                                    while (onew[0] != null)
                                    {
                                        System.Object placeholder = onew[0];
                                        for (int k = 1; k < onew.Length; k++)
                                        {
                                            onew[k - 1] = onew[k];
                                        }
                                        onew[onew.Length - 1] = placeholder;
                                        l++;
                                        if (l > onew.Length)
                                        {
                                            break;
                                        }
                                    }
                                }
                                //The last in onew is a vector: This means we need to exchange the rest of the original smiles with the rest of this vector.
                                if (onew[numberOfAtoms - 1] is System.Collections.ArrayList)
                                {
                                    for (int i = 0; i < numberOfAtoms; i++)
                                    {
                                        if (onew[i] is IAtom)
                                        {
                                            System.Collections.ArrayList vtemp = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
                                            vtemp.Add(onew[i]);
                                            for (int k = positionInVector + 1 + numberOfAtoms; k < v.Count; k++)
                                            {
                                                vtemp.Add(v[k]);
                                            }
                                            onew[i] = vtemp;
                                            for (int k = v.Count - 1; k > positionInVector + 1 + numberOfAtoms - 1; k--)
                                            {
                                                v.RemoveAt(k);
                                            }
                                            for (int k = 1; k < ((System.Collections.ArrayList)onew[numberOfAtoms - 1]).Count; k++)
                                            {
                                                v.Add(((System.Collections.ArrayList)onew[numberOfAtoms - 1])[k]);
                                            }
                                            onew[numberOfAtoms - 1] = ((System.Collections.ArrayList)onew[numberOfAtoms - 1])[0];
                                            break;
                                        }
                                    }
                                }
                                //Put the onew objects in the original Vector
                                int k2 = 0;
                                for (int m = 0; m < onew.Length; m++)
                                {
                                    if (onew[m] != null)
                                    {
                                        v[positionInVector + 1 + k2] = onew[m];
                                        k2++;
                                    }
                                }
                            }
                        }
                    }
                    parent = atom;
                }
                else
                {
                    //Have Vector
                    //System.out.println("in parseChain after else");
                    bool brackets = true;
                    System.Collections.ArrayList result = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
                    addAtoms((System.Collections.ArrayList)o, result);
                    if (isRingOpening(parent, result) && container.getBondCount(parent) < 4)
                    {
                        brackets = false;
                    }
                    if (brackets)
                    {
                        buffer.Append('(');
                    }
                    parseChain((System.Collections.ArrayList)o, buffer, container, parent, chiral, doubleBondConfiguration, atomsInOrderOfSmiles);
                    if (brackets)
                    {
                        buffer.Append(')');
                    }
                }

                positionInVector++;
                //System.out.println("in parseChain after positionVector++");
            }
        }


        /// <summary>  Append the symbol for the bond order between <code>a1</code> and <code>a2</code>
        /// to the <code>line</code>.
        /// 
        /// </summary>
        /// <param name="line">          the StringBuffer that the bond symbol is appended to.
        /// </param>
        /// <param name="a1">            Atom participating in the bond.
        /// </param>
        /// <param name="a2">            Atom participating in the bond.
        /// </param>
        /// <param name="atomContainer"> the AtomContainer that the SMILES string is generated
        /// for.
        /// </param>
        private void parseBond(System.Text.StringBuilder line, IAtom a1, IAtom a2, IAtomContainer atomContainer)
        {
            //System.out.println("in parseBond()");
            if (a1.getFlag(CDKConstants.ISAROMATIC) && a1.getFlag(CDKConstants.ISAROMATIC))
            {
                return;
            }
            if (atomContainer.getBond(a1, a2) == null)
            {
                return;
            }
            int type = 0;
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            type = (int)atomContainer.getBond(a1, a2).Order;
            if (type == 1)
            {
            }
            else if (type == 2)
            {
                line.Append("=");
            }
            else if (type == 3)
            {
                line.Append("#");
            }
            else
            {
                // //System.out.println("Unknown bond type");
            }
        }


        /// <summary>  Generates the SMILES string for the atom
        /// 
        /// </summary>
        /// <param name="a">                       the atom to generate the SMILES for.
        /// </param>
        /// <param name="buffer">                  the string buffer that the atom is to be
        /// apended to.
        /// </param>
        /// <param name="container">               the AtomContainer to analyze.
        /// </param>
        /// <param name="chiral">                  is a chiral smiles wished?
        /// </param>
        /// <param name="parent">                  the atom we came from.
        /// </param>
        /// <param name="atomsInOrderOfSmiles">    a vector containing the atoms in the order
        /// they are in the smiles.
        /// </param>
        /// <param name="currentChain">            The chain we currently deal with.
        /// </param>
        /// <param name="doubleBondConfiguration"> Description of Parameter
        /// </param>
        private void parseAtom(IAtom a, System.Text.StringBuilder buffer, IAtomContainer container, bool chiral, bool[] doubleBondConfiguration, IAtom parent, System.Collections.ArrayList atomsInOrderOfSmiles, System.Collections.ArrayList currentChain)
        {
            System.String symbol = a.Symbol;
            bool stereo = BondTools.isStereo(container, a);
            bool brackets = symbol.Equals("B") || symbol.Equals("C") || symbol.Equals("N") || symbol.Equals("O") || symbol.Equals("P") || symbol.Equals("S") || symbol.Equals("F") || symbol.Equals("Br") || symbol.Equals("I") || symbol.Equals("Cl");
            brackets = !brackets;
            //System.out.println("in parseAtom()");
            //Deal with the start of a double bond configuration
            if (isStartOfDoubleBond(container, a, parent, doubleBondConfiguration))
            {
                buffer.Append('/');
            }

            if (a is IPseudoAtom)
            {
                buffer.Append("[*]");
            }
            else
            {
                System.String mass = generateMassString(a);
                brackets = brackets | !mass.Equals("");

                System.String charge = generateChargeString(a);
                brackets = brackets | !charge.Equals("");

                if (chiral && stereo)
                {
                    brackets = true;
                }
                if (brackets)
                {
                    buffer.Append('[');
                }
                buffer.Append(mass);
                if (a.getFlag(CDKConstants.ISAROMATIC))
                {
                    // Strictly speaking, this is wrong. Lower case is only used for sp2 atoms!
                    buffer.Append(a.Symbol.ToLower());
                }
                else if (a.Hybridization == CDKConstants.HYBRIDIZATION_SP2)
                {
                    buffer.Append(a.Symbol.ToLower());
                }
                else
                {
                    buffer.Append(symbol);
                }
                if (a.getProperty(RING_CONFIG) != null && a.getProperty(RING_CONFIG).Equals(UP))
                {
                    buffer.Append('/');
                }
                if (a.getProperty(RING_CONFIG) != null && a.getProperty(RING_CONFIG).Equals(DOWN))
                {
                    buffer.Append('\\');
                }
                if (chiral && stereo && (BondTools.isTrigonalBipyramidalOrOctahedral(container, a) != 0 || BondTools.isSquarePlanar(container, a) || BondTools.isTetrahedral(container, a, false) != 0))
                {
                    buffer.Append('@');
                }
                if (chiral && stereo && BondTools.isSquarePlanar(container, a))
                {
                    buffer.Append("SP1");
                }
                //chiral
                //hcount
                buffer.Append(charge);
                if (brackets)
                {
                    buffer.Append(']');
                }
            }
            //System.out.println("in parseAtom() after dealing with Pseudoatom or not");
            //Deal with the end of a double bond configuration
            if (isEndOfDoubleBond(container, a, parent, doubleBondConfiguration))
            {
                IAtom viewFrom = null;
                for (int i = 0; i < currentChain.Count; i++)
                {
                    if (currentChain[i] == parent)
                    {
                        int k = i - 1;
                        while (k > -1)
                        {
                            if (currentChain[k] is IAtom)
                            {
                                viewFrom = (IAtom)currentChain[k];
                                break;
                            }
                            k--;
                        }
                    }
                }
                if (viewFrom == null)
                {
                    for (int i = 0; i < atomsInOrderOfSmiles.Count; i++)
                    {
                        if (atomsInOrderOfSmiles[i] == parent)
                        {
                            viewFrom = (IAtom)atomsInOrderOfSmiles[i - 1];
                        }
                    }
                }
                bool afterThisAtom = false;
                IAtom viewTo = null;
                for (int i = 0; i < currentChain.Count; i++)
                {
                    if (afterThisAtom && currentChain[i] is IAtom)
                    {
                        viewTo = (IAtom)currentChain[i];
                        break;
                    }
                    if (afterThisAtom && currentChain[i] is System.Collections.ArrayList)
                    {
                        viewTo = (IAtom)((System.Collections.ArrayList)currentChain[i])[0];
                        break;
                    }
                    if (a == currentChain[i])
                    {
                        afterThisAtom = true;
                    }
                }
                try
                {
                    if (BondTools.isCisTrans(viewFrom, a, parent, viewTo, container))
                    {
                        buffer.Append('\\');
                    }
                    else
                    {
                        buffer.Append('/');
                    }
                }
                catch (CDKException ex)
                {
                    //If the user wants a double bond configuration, where there is none, we ignore this.
                }
            }
            System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            System.Collections.IEnumerator it = getRingOpenings(a, v).GetEnumerator();
            System.Collections.IEnumerator it2 = v.GetEnumerator();
            //System.out.println("in parseAtom() after checking for Ring openings");
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (it.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                System.Int32 integer = (System.Int32)it.Current;
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                IBond b = container.getBond((IAtom)it2.Current, a);
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                int type = (int)b.Order;
                if (type == 2 && !b.getFlag(CDKConstants.ISAROMATIC))
                {
                    buffer.Append("=");
                }
                else if (type == 3 && !b.getFlag(CDKConstants.ISAROMATIC))
                {
                    buffer.Append("#");
                }
                buffer.Append(integer);
            }
            atomsInOrderOfSmiles.Add(a);
            //System.out.println("End of parseAtom()");
        }


        /// <summary>  Creates a string for the charge of atom <code>a</code>. If the charge is 1
        /// + is returned if it is -1 - is returned. The positive values all have + in
        /// front of them.
        /// 
        /// </summary>
        /// <param name="a"> Description of Parameter
        /// </param>
        /// <returns>    string representing the charge on <code>a</code>
        /// </returns>
        private System.String generateChargeString(IAtom a)
        {
            int charge = a.getFormalCharge();
            System.Text.StringBuilder buffer = new System.Text.StringBuilder(3);
            if (charge > 0)
            {
                //Positive
                buffer.Append('+');
                if (charge > 1)
                {
                    buffer.Append(charge);
                }
            }
            else if (charge < 0)
            {
                //Negative
                if (charge == -1)
                {
                    buffer.Append('-');
                }
                else
                {
                    buffer.Append(charge);
                }
            }
            return buffer.ToString();
        }


        /// <summary>  Creates a string containing the mass of the atom <code>a</code>. If the
        /// mass is the same as the majour isotope an empty string is returned.
        /// 
        /// </summary>
        /// <param name="a"> the atom to create the mass
        /// </param>
        /// <returns>    Description of the Returned Value
        /// </returns>
        private System.String generateMassString(IAtom a)
        {
            IIsotope majorIsotope = isotopeFactory.getMajorIsotope(a.Symbol);
            if (majorIsotope.MassNumber == a.MassNumber)
            {
                return "";
            }
            else if (a.MassNumber == 0)
            {
                return "";
            }
            else
            {
                return System.Convert.ToString(a.MassNumber);
            }
        }


        //UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'BrokenBond' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
        /// <summary>  Description of the Class
        /// 
        /// </summary>
        /// <author>          shk3
        /// </author>
        /// <cdk.created>     2003-06-17 </cdk.created>
        internal class BrokenBond
        {
            private void InitBlock(SmilesGenerator enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }
            private SmilesGenerator enclosingInstance;
            /// <summary>  Getter method for a1 property
            /// 
            /// </summary>
            /// <returns>    The a1 value
            /// </returns>
            virtual public IAtom A1
            {
                get
                {
                    return a1;
                }

            }
            /// <summary>  Getter method for a2 property
            /// 
            /// </summary>
            /// <returns>    The a2 value
            /// </returns>
            virtual public IAtom A2
            {
                get
                {
                    return a2;
                }

            }
            /// <summary>  Getter method for marker property
            /// 
            /// </summary>
            /// <returns>    The marker value
            /// </returns>
            virtual public int Marker
            {
                get
                {
                    return marker;
                }

            }
            public SmilesGenerator Enclosing_Instance
            {
                get
                {
                    return enclosingInstance;
                }

            }

            /// <summary>  The atoms which close the ring</summary>
            private IAtom a1, a2;

            /// <summary>  The number of the marker</summary>
            private int marker;


            /// <summary>  Construct a BrokenBond between <code>a1</code> and <code>a2</code> with
            /// the marker <code>marker</code>.
            /// 
            /// </summary>
            /// <param name="marker"> the ring closure marker. (Great comment!)
            /// </param>
            /// <param name="a1">     Description of Parameter
            /// </param>
            /// <param name="a2">     Description of Parameter
            /// </param>
            internal BrokenBond(SmilesGenerator enclosingInstance, IAtom a1, IAtom a2, int marker)
            {
                InitBlock(enclosingInstance);
                this.a1 = a1;
                this.a2 = a2;
                this.marker = marker;
            }


            /// <summary>  Description of the Method
            /// 
            /// </summary>
            /// <returns>    Description of the Returned Value
            /// </returns>
            public override System.String ToString()
            {
                return System.Convert.ToString(marker);
            }


            /// <summary>  Description of the Method
            /// 
            /// </summary>
            /// <param name="o"> Description of Parameter
            /// </param>
            /// <returns>    Description of the Returned Value
            /// </returns>
            public override bool Equals(System.Object o)
            {
                if (!(o is BrokenBond))
                {
                    return false;
                }
                BrokenBond bond = (BrokenBond)o;
                return (a1.Equals(bond.A1) && a2.Equals(bond.A2)) || (a1.Equals(bond.A2) && a2.Equals(bond.A1));
            }
            //UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}