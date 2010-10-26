/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $
* $Revision: 6672 $
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
*
*/
using System;
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK
{
    /// <summary> Represents the idea of a chemical reaction. The reaction consists of 
    /// a set of reactants and a set of products.
    /// 
    /// <p>The class mostly represents abstract reactions, such as 2D diagrams,
    /// and is not intended to represent reaction trajectories. Such can better
    /// be represented with a ChemSequence.
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       Egon Willighagen <elw38@cam.ac.uk>
    /// </author>
    /// <cdk.created>  2003-02-13 </cdk.created>
    /// <cdk.keyword>  reaction </cdk.keyword>
    [Serializable]
    public class Reaction : ChemObject, IReaction, ICloneable
    {
        /// <summary> Returns the number of reactants in this reaction.
        /// 
        /// </summary>
        /// <returns> The number of reactants in this reaction
        /// </returns>
        virtual public int ReactantCount
        {
            get
            {
                return reactants.AtomContainerCount;
            }

        }
        /// <summary> Returns the number of products in this reaction.
        /// 
        /// </summary>
        /// <returns> The number of products in this reaction
        /// </returns>
        virtual public int ProductCount
        {
            get
            {
                return products.AtomContainerCount;
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns a SetOfMolecules containing the reactants in this reaction.
        /// 
        /// </summary>
        /// <returns> A SetOfMolecules containing the reactants in this reaction
        /// </returns>
        /// <seealso cref="setReactants">
        /// </seealso>
        /// <summary> Assigns a SetOfMolecules to the reactants in this reaction.
        /// 
        /// </summary>
        /// <param name="setOfMolecules">The new set of reactants
        /// </param>
        /// <seealso cref="getReactants">
        /// </seealso>
        virtual public ISetOfMolecules Reactants
        {
            get
            {
                return (SetOfMolecules)reactants;
            }

            set
            {
                reactants = value;
                notifyChanged();
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns a SetOfMolecules containing the products of this reaction.
        /// 
        /// </summary>
        /// <returns> A SetOfMolecules containing the products in this reaction
        /// </returns>
        /// <seealso cref="setProducts">
        /// </seealso>
        /// <summary> Assigns a SetOfMolecules to the products of this reaction.
        /// 
        /// </summary>
        /// <param name="setOfMolecules">The new set of products
        /// </param>
        /// <seealso cref="getProducts">
        /// </seealso>
        virtual public ISetOfMolecules Products
        {
            get
            {
                return (SetOfMolecules)products;
            }

            set
            {
                products = value;
                notifyChanged();
            }

        }
        /// <summary> Returns a SetOfMolecules containing the agents in this reaction.
        /// 
        /// </summary>
        /// <returns> A SetOfMolecules containing the agents in this reaction
        /// </returns>
        /// <seealso cref="addAgent">
        /// </seealso>
        virtual public ISetOfMolecules Agents
        {
            get
            {
                return (SetOfMolecules)agents;
            }

        }
        /// <summary> Returns the mappings between the reactant and the product side.
        /// 
        /// </summary>
        /// <returns> An array of Mapping's.
        /// </returns>
        /// <seealso cref="addMapping">
        /// </seealso>
        virtual public IMapping[] Mappings
        {
            get
            {
                Mapping[] returnMappings = new Mapping[mappingCount];
                Array.Copy(this.map, 0, returnMappings, 0, returnMappings.Length);
                return returnMappings;
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary> Returns the direction of the reaction.
        /// 
        /// </summary>
        /// <returns> The direction of this reaction (FORWARD, BACKWARD or BIDIRECTIONAL).
        /// </returns>
        /// <seealso cref="BIDIRECTIONAL">
        /// </seealso>
        /// <seealso cref="setDirection">
        /// </seealso>
        /// <summary> Sets the direction of the reaction.
        /// 
        /// </summary>
        /// <param name="direction">The new reaction direction
        /// </param>
        /// <seealso cref="getDirection">
        /// </seealso>
        virtual public int Direction
        {
            get
            {
                return reactionDirection;
            }

            set
            {
                reactionDirection = value;
                notifyChanged();
            }

        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = -554752528363533678L;

        protected internal int growArraySize = 3;

        protected internal ISetOfMolecules reactants;
        protected internal ISetOfMolecules products;
        /// <summary>These are the used solvent, catalysist etc that normally appear above
        /// the reaction arrow 
        /// </summary>
        protected internal ISetOfMolecules agents;

        protected internal IMapping[] map;
        protected internal int mappingCount;

        private int reactionDirection;

        /// <summary> Constructs an empty, forward reaction.</summary>
        public Reaction()
        {
            this.reactants = new SetOfMolecules();
            this.products = new SetOfMolecules();
            this.agents = new SetOfMolecules();
            this.map = new Mapping[growArraySize];
            mappingCount = 0;
            reactionDirection = IReaction_Fields.FORWARD;
        }

        /// <summary> Adds a reactant to this reaction.
        /// 
        /// </summary>
        /// <param name="reactant">  Molecule added as reactant to this reaction
        /// </param>
        /// <seealso cref="getReactants">
        /// </seealso>
        public virtual void addReactant(IMolecule reactant)
        {
            addReactant(reactant, 1.0);
            /* notifyChanged() is called by 
            addReactant(Molecule reactant, double coefficient) */
        }

        /// <summary> Adds an agent to this reaction.
        /// 
        /// </summary>
        /// <param name="agent">  Molecule added as agent to this reaction
        /// </param>
        /// <seealso cref="getAgents">
        /// </seealso>
        public virtual void addAgent(IMolecule agent)
        {
            agents.addAtomContainer(agent);
            notifyChanged();
        }

        /// <summary> Adds a reactant to this reaction with a stoichiometry coefficient.
        /// 
        /// </summary>
        /// <param name="reactant">   Molecule added as reactant to this reaction
        /// </param>
        /// <param name="coefficient">Stoichiometry coefficient for this molecule
        /// </param>
        /// <seealso cref="getReactants">
        /// </seealso>
        public virtual void addReactant(IMolecule reactant, double coefficient)
        {
            reactants.addAtomContainer(reactant, coefficient);
            notifyChanged();
        }

        /// <summary> Adds a product to this reaction.
        /// 
        /// </summary>
        /// <param name="product">   Molecule added as product to this reaction
        /// </param>
        /// <seealso cref="getProducts">
        /// </seealso>
        public virtual void addProduct(IMolecule product)
        {
            this.addProduct(product, 1.0);
            /* notifyChanged() is called by 
            addProduct(Molecule product, double coefficient)*/
        }

        /// <summary> Adds a product to this reaction.
        /// 
        /// </summary>
        /// <param name="product">    Molecule added as product to this reaction
        /// </param>
        /// <param name="coefficient">Stoichiometry coefficient for this molecule
        /// </param>
        /// <seealso cref="getProducts">
        /// </seealso>
        public virtual void addProduct(IMolecule product, double coefficient)
        {
            products.addAtomContainer(product, coefficient);
            /* notifyChanged() is called by 
            addReactant(Molecule reactant, double coefficient) */
        }

        /// <summary> Returns the stoichiometry coefficient of the given reactant.
        /// 
        /// </summary>
        /// <param name="reactant">Reactant for which the coefficient is returned.
        /// </param>
        /// <returns> -1, if the given molecule is not a product in this Reaction
        /// </returns>
        /// <seealso cref="setReactantCoefficient">
        /// </seealso>
        public virtual double getReactantCoefficient(IMolecule reactant)
        {
            return reactants.getMultiplier(reactant);
        }

        /// <summary> Returns the stoichiometry coefficient of the given product.
        /// 
        /// </summary>
        /// <param name="product">Product for which the coefficient is returned.
        /// </param>
        /// <returns> -1, if the given molecule is not a product in this Reaction
        /// </returns>
        /// <seealso cref="setProductCoefficient">
        /// </seealso>
        public virtual double getProductCoefficient(IMolecule product)
        {
            return products.getMultiplier(product);
        }

        /// <summary> Sets the coefficient of a a reactant to a given value.
        /// 
        /// </summary>
        /// <param name="reactant">   Reactant for which the coefficient is set
        /// </param>
        /// <param name="coefficient">The new coefficient for the given reactant
        /// </param>
        /// <returns>  true if Molecule has been found and stoichiometry has been set.
        /// </returns>
        /// <seealso cref="getReactantCoefficient">
        /// </seealso>
        public virtual bool setReactantCoefficient(IMolecule reactant, double coefficient)
        {
            notifyChanged();
            return reactants.setMultiplier(reactant, coefficient);
        }


        /// <summary> Sets the coefficient of a a product to a given value.
        /// 
        /// </summary>
        /// <param name="product">    Product for which the coefficient is set
        /// </param>
        /// <param name="coefficient">The new coefficient for the given product
        /// </param>
        /// <returns>  true if Molecule has been found and stoichiometry has been set.
        /// </returns>
        /// <seealso cref="getProductCoefficient">
        /// </seealso>
        public virtual bool setProductCoefficient(IMolecule product, double coefficient)
        {
            notifyChanged();
            return products.setMultiplier(product, coefficient);
        }

        /// <summary> Returns an array of double with the stoichiometric coefficients
        /// of the reactants.
        /// 
        /// </summary>
        /// <returns> An array of double's containing the coefficients of the reactants
        /// </returns>
        /// <seealso cref="setReactantCoefficients">
        /// </seealso>
        public virtual double[] getReactantCoefficients()
        {
            return reactants.getMultipliers();
        }

        /// <summary> Returns an array of double with the stoichiometric coefficients
        /// of the products.
        /// 
        /// </summary>
        /// <returns> An array of double's containing the coefficients of the products
        /// </returns>
        /// <seealso cref="setProductCoefficients">
        /// </seealso>
        public virtual double[] getProductCoefficients()
        {
            return products.getMultipliers();
        }


        /// <summary> Sets the coefficients of the reactants.
        /// 
        /// </summary>
        /// <param name="coefficients">An array of double's containing the coefficients of the reactants
        /// </param>
        /// <returns>  true if coefficients have been set.
        /// </returns>
        /// <seealso cref="getReactantCoefficients">
        /// </seealso>
        public virtual bool setReactantCoefficients(double[] coefficients)
        {
            notifyChanged();
            return reactants.setMultipliers(coefficients);
        }

        /// <summary> Sets the coefficient of the products.
        /// 
        /// </summary>
        /// <param name="coefficients">An array of double's containing the coefficients of the products
        /// </param>
        /// <returns>  true if coefficients have been set.
        /// </returns>
        /// <seealso cref="getProductCoefficients">
        /// </seealso>
        public virtual bool setProductCoefficients(double[] coefficients)
        {
            notifyChanged();
            return products.setMultipliers(coefficients);
        }

        /// <summary> Adds a mapping between the reactant and product side to this
        /// Reaction.
        /// 
        /// </summary>
        /// <param name="mapping">Mapping to add.
        /// </param>
        /// <seealso cref="getMappings">
        /// </seealso>
        public virtual void addMapping(IMapping mapping)
        {
            if (mappingCount + 1 >= map.Length)
                growMappingArray();
            map[mappingCount] = mapping;
            mappingCount++;
            notifyChanged();
        }

        protected internal virtual void growMappingArray()
        {
            Mapping[] newMap = new Mapping[map.Length + growArraySize];
            Array.Copy(map, 0, newMap, 0, map.Length);
            map = newMap;
        }

        /// <summary> Returns a one line string representation of this Atom.
        /// Methods is conform RFC #9.
        /// 
        /// </summary>
        /// <returns>  The string representation of this Atom
        /// </returns>
        public override System.String ToString()
        {
            System.Text.StringBuilder description = new System.Text.StringBuilder(64);
            description.Append("Reaction(");
            description.Append(ID);
            description.Append(", #M:").Append(mappingCount);
            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            description.Append(", reactants=").Append(reactants.ToString());
            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            description.Append(", products=").Append(products.ToString());
            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            description.Append(", agents=").Append(agents.ToString());
            description.Append(')');
            return description.ToString();
        }

        /// <summary> Clones this <code>Reaction</code> and its content.
        /// 
        /// </summary>
        /// <returns>  The cloned object
        /// </returns>
        public override System.Object Clone()
        {
            Reaction clone = (Reaction)base.Clone();
            // clone the reactants, products and agents
            clone.reactants = (SetOfMolecules)((SetOfMolecules)reactants).Clone();
            clone.agents = (SetOfMolecules)((SetOfMolecules)agents).Clone();
            clone.products = (SetOfMolecules)((SetOfMolecules)products).Clone();
            // create a Map of corresponding atoms for molecules (key: original Atom, 
            // value: clone Atom)
            System.Collections.Hashtable atomatom = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
            for (int i = 0; i < ((SetOfMolecules)reactants).MoleculeCount; ++i)
            {
                Molecule mol = (Molecule)((SetOfMolecules)reactants).getMolecule(i);
                Molecule mol2 = (Molecule)clone.reactants.getMolecule(i);
                for (int j = 0; j < mol.AtomCount; ++j)
                    atomatom[mol.getAtomAt(j)] = mol2.getAtomAt(j);
            }

            // clone the maps
            clone.map = new Mapping[map.Length];
            for (int f = 0; f < mappingCount; f++)
            {
                IChemObject[] rel = map[f].RelatedChemObjects;
                clone.map[f] = new Mapping((ChemObject)atomatom[rel[0]], (ChemObject)atomatom[rel[1]]);
            }
            return clone;
        }
    }
}