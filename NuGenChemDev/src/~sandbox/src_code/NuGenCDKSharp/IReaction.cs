/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-14 09:45:29 +0200 (Fri, 14 Jul 2006) $
* $Revision: 6663 $
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

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary> Represents the idea of a chemical reaction. The reaction consists of 
    /// a set of reactants and a set of products.
    /// 
    /// <p>The class mostly represents abstract reactions, such as 2D diagrams,
    /// and is not intended to represent reaction trajectories. Such can better
    /// be represented with a ChemSequence.
    /// 
    /// </summary>
    /// <cdk.module>   interfaces </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       Egon Willighagen <elw38@cam.ac.uk>
    /// </author>
    /// <cdk.created>  2003-02-13 </cdk.created>
    /// <cdk.keyword>  reaction </cdk.keyword>
    public struct IReaction_Fields
    {// TODO: Conv to enum
        /// <summary>Reaction of which the equilibrium is not set. </summary>
        public readonly static int UNKNOWN_DIRECTION = 0;
        /// <summary>Reaction equalibrium which is (almost) fully on the product side. 
        /// Often denoted with a forward arrow. 
        /// </summary>
        public readonly static int FORWARD = 1;
        /// <summary>Reaction equalibrium which is (almost) fully on the reactant side. 
        /// Often denoted with a backward arrow. 
        /// </summary>
        public readonly static int BACKWARD = 2;
        /// <summary>Reaction equalibrium state. Often denoted by a double arrow. </summary>
        public readonly static int BIDIRECTIONAL = 3;
    }

    public interface IReaction : IChemObject
    {
        /// <summary> Returns the number of reactants in this reaction.
        /// 
        /// </summary>
        /// <returns> The number of reactants in this reaction
        /// </returns>
        int ReactantCount
        {
            get;
        }
        
        /// <summary> Returns the number of products in this reaction.
        /// 
        /// </summary>
        /// <returns> The number of products in this reaction
        /// </returns>
        int ProductCount
        {
            get;

        }
        
        /// <summary> Returns a ISetOfMolecules containing the reactants in this reaction.
        /// 
        /// </summary>
        /// <returns> A ISetOfMolecules containing the reactants in this reaction
        /// </returns>
        /// <seealso cref="setReactants">
        /// </seealso>
        /// <summary> Assigns a ISetOfMolecules to the reactants in this reaction.
        /// 
        /// </summary>
        /// <param name="reactants">The new set of reactants
        /// </param>
        /// <seealso cref="getReactants">
        /// </seealso>
        ISetOfMolecules Reactants
        {
            get;
            set;
        }
        
        /// <summary> Returns a ISetOfMolecules containing the products of this reaction.
        /// 
        /// </summary>
        /// <returns> A ISetOfMolecules containing the products in this reaction
        /// </returns>
        /// <seealso cref="setProducts">
        /// </seealso>
        /// <summary> Assigns a ISetOfMolecules to the products of this reaction.
        /// 
        /// </summary>
        /// <param name="products">The new set of products
        /// </param>
        /// <seealso cref="getProducts">
        /// </seealso>
        ISetOfMolecules Products
        {
            get;
            set;
        }
        
        /// <summary> Returns a ISetOfMolecules containing the agents in this reaction.
        /// 
        /// </summary>
        /// <returns> A ISetOfMolecules containing the agents in this reaction
        /// </returns>
        /// <seealso cref="addAgent">
        /// </seealso>
        ISetOfMolecules Agents
        {
            get;
        }
        /// <summary> Returns the mappings between the reactant and the product side.
        /// 
        /// </summary>
        /// <returns> An array of Mapping's.
        /// </returns>
        /// <seealso cref="addMapping">
        /// </seealso>
        IMapping[] Mappings
        {
            get;
        }
        
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
        int Direction
        {
            get;
            set;
        }

        /// <summary> Adds a reactant to this reaction.
        /// 
        /// </summary>
        /// <param name="reactant">  Molecule added as reactant to this reaction
        /// </param>
        /// <seealso cref="getReactants">
        /// </seealso>
        void addReactant(IMolecule reactant);

        /// <summary> Adds an agent to this reaction.
        /// 
        /// </summary>
        /// <param name="agent">  Molecule added as agent to this reaction
        /// </param>
        /// <seealso cref="getAgents">
        /// </seealso>
        void addAgent(IMolecule agent);

        /// <summary> Adds a reactant to this reaction with a stoichiometry coefficient.
        /// 
        /// </summary>
        /// <param name="reactant">   Molecule added as reactant to this reaction
        /// </param>
        /// <param name="coefficient">Stoichiometry coefficient for this molecule
        /// </param>
        /// <seealso cref="getReactants">
        /// </seealso>
        void addReactant(IMolecule reactant, double coefficient);

        /// <summary> Adds a product to this reaction.
        /// 
        /// </summary>
        /// <param name="product">   Molecule added as product to this reaction
        /// </param>
        /// <seealso cref="getProducts">
        /// </seealso>
        void addProduct(IMolecule product);

        /// <summary> Adds a product to this reaction.
        /// 
        /// </summary>
        /// <param name="product">    Molecule added as product to this reaction
        /// </param>
        /// <param name="coefficient">Stoichiometry coefficient for this molecule
        /// </param>
        /// <seealso cref="getProducts">
        /// </seealso>
        void addProduct(IMolecule product, double coefficient);

        /// <summary> Returns the stoichiometry coefficient of the given reactant.
        /// 
        /// </summary>
        /// <param name="reactant">Reactant for which the coefficient is returned.
        /// </param>
        /// <returns> -1, if the given molecule is not a product in this Reaction
        /// </returns>
        /// <seealso cref="setReactantCoefficient">
        /// </seealso>
        double getReactantCoefficient(IMolecule reactant);

        /// <summary> Returns the stoichiometry coefficient of the given product.
        /// 
        /// </summary>
        /// <param name="product">Product for which the coefficient is returned.
        /// </param>
        /// <returns> -1, if the given molecule is not a product in this Reaction
        /// </returns>
        /// <seealso cref="setProductCoefficient">
        /// </seealso>
        double getProductCoefficient(IMolecule product);

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
        bool setReactantCoefficient(IMolecule reactant, double coefficient);

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
        bool setProductCoefficient(IMolecule product, double coefficient);

        /// <summary> Returns an array of double with the stoichiometric coefficients
        /// of the reactants.
        /// 
        /// </summary>
        /// <returns> An array of double's containing the coefficients of the reactants
        /// </returns>
        /// <seealso cref="setReactantCoefficients">
        /// </seealso>
        double[] getReactantCoefficients();

        /// <summary> Returns an array of double with the stoichiometric coefficients
        /// of the products.
        /// 
        /// </summary>
        /// <returns> An array of double's containing the coefficients of the products
        /// </returns>
        /// <seealso cref="setProductCoefficients">
        /// </seealso>
        double[] getProductCoefficients();

        /// <summary> Sets the coefficients of the reactants.
        /// 
        /// </summary>
        /// <param name="coefficients">An array of double's containing the coefficients of the reactants
        /// </param>
        /// <returns>  true if coefficients have been set.
        /// </returns>
        /// <seealso cref="getReactantCoefficients">
        /// </seealso>
        bool setReactantCoefficients(double[] coefficients);

        /// <summary> Sets the coefficient of the products.
        /// 
        /// </summary>
        /// <param name="coefficients">An array of double's containing the coefficients of the products
        /// </param>
        /// <returns>  true if coefficients have been set.
        /// </returns>
        /// <seealso cref="getProductCoefficients">
        /// </seealso>
        bool setProductCoefficients(double[] coefficients);

        /// <summary> Adds a mapping between the reactant and product side to this
        /// Reaction.
        /// 
        /// </summary>
        /// <param name="mapping">Mapping to add.
        /// </param>
        /// <seealso cref="getMappings">
        /// </seealso>
        void addMapping(IMapping mapping);
    }
}