/*
*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
*  $Revision: 5855 $
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
*/
using System;
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK.Tools.Manipulator
{
    /// <summary> Class with utilities for the <code>AtomType</code> class.
    /// - changed 21/7/05 by cho: add properties for mmff94 atom type 
    /// 
    /// </summary>
    /// <author>      mfe4
    /// </author>
    /// <author>      egonw
    /// </author>
    /// <cdk.module>  standard </cdk.module>
    public class AtomTypeManipulator
    {

        /// <summary> Method that assign properties to an atom given a particular atomType.
        /// 
        /// </summary>
        /// <param name="atom"> Atom to configure
        /// </param>
        /// <param name="atomType">   AtomType
        /// </param>
        public static void configure(IAtom atom, IAtomType atomType)
        {
            atom.AtomTypeName = atomType.AtomTypeName;
            atom.MaxBondOrder = atomType.MaxBondOrder;
            atom.BondOrderSum = atomType.BondOrderSum;
            atom.VanderwaalsRadius = atomType.VanderwaalsRadius;
            atom.CovalentRadius = atomType.CovalentRadius;
            atom.Valency = atomType.Valency;
            atom.setFormalCharge(atomType.getFormalCharge());
            atom.Hybridization = atomType.Hybridization;
            atom.FormalNeighbourCount = atomType.FormalNeighbourCount;
            atom.setFlag(CDKConstants.IS_HYDROGENBOND_ACCEPTOR, atomType.getFlag(CDKConstants.IS_HYDROGENBOND_ACCEPTOR));
            atom.setFlag(CDKConstants.IS_HYDROGENBOND_DONOR, atomType.getFlag(CDKConstants.IS_HYDROGENBOND_DONOR));
            System.Object constant = atomType.getProperty(CDKConstants.CHEMICAL_GROUP_CONSTANT);
            if (constant != null)
            {
                atom.setProperty(CDKConstants.CHEMICAL_GROUP_CONSTANT, constant);
            }
            atom.setFlag(CDKConstants.ISAROMATIC, atomType.getFlag(CDKConstants.ISAROMATIC));

            System.Object color = atomType.getProperty("org.openscience.cdk.renderer.color");
            if (color != null)
            {
                atom.setProperty("org.openscience.cdk.renderer.color", color);
            }
            if (atomType.AtomicNumber != 0)
            {
                atom.AtomicNumber = atomType.AtomicNumber;
            }
            if (atomType.getExactMass() > 0.0)
            {
                atom.setExactMass(atomType.getExactMass());
            }
        }
    }
}