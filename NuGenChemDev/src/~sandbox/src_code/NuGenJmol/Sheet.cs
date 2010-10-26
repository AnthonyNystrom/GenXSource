/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2002-2005  The Jmol Development Team
*
* Contact: jmol-developers@lists.sf.net
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
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using javax.vecmath;

namespace Org.Jmol.Viewer
{
	
	class Sheet : ProteinStructure
	{
		public virtual Vector3f WidthUnitVector
		{
			get
			{
				if (widthUnitVector == null)
					calcSheetUnitVectors();
				return widthUnitVector;
			}
			
		}

		public virtual Vector3f HeightUnitVector
		{
			get
			{
				if (heightUnitVector == null)
					calcSheetUnitVectors();
				return heightUnitVector;
			}
			
		}

        public AminoPolymer aminoPolymer;
        public Sheet(AminoPolymer aminoPolymer, int monomerIndex, int monomerCount)
            : base(aminoPolymer, JmolConstants.PROTEIN_STRUCTURE_SHEET, monomerIndex, monomerCount)
		{
			this.aminoPolymer = aminoPolymer;
		}

        public override void calcAxis()
		{
			if (axisA != null)
				return ;
			if (monomerCount == 2)
			{
				axisA = aminoPolymer.getLeadPoint(monomerIndex);
				axisB = aminoPolymer.getLeadPoint(monomerIndex + 1);
			}
			else
			{
				axisA = new Point3f();
				aminoPolymer.getLeadMidPoint(monomerIndex + 1, axisA);
				axisB = new Point3f();
				aminoPolymer.getLeadMidPoint(monomerIndex + monomerCount - 1, axisB);
			}
			
			axisUnitVector = new Vector3f();
			axisUnitVector.sub(axisB, axisA);
			axisUnitVector.normalize();
			
			Point3f tempA = new Point3f();
			aminoPolymer.getLeadMidPoint(monomerIndex, tempA);
			if (!lowerNeighborIsHelixOrSheet())
				projectOntoAxis(tempA);
			Point3f tempB = new Point3f();
			aminoPolymer.getLeadMidPoint(monomerIndex + monomerCount, tempB);
			if (!upperNeighborIsHelixOrSheet())
				projectOntoAxis(tempB);
			axisA = tempA;
			axisB = tempB;
		}

        public Vector3f widthUnitVector;
        public Vector3f heightUnitVector;

        public virtual void calcSheetUnitVectors()
		{
			if (widthUnitVector == null)
			{
				Vector3f vectorCO = new Vector3f();
				Vector3f vectorCOSum = new Vector3f();
				AminoMonomer amino = (AminoMonomer) aminoPolymer.monomers[monomerIndex];
				vectorCOSum.sub(amino.CarbonylOxygenAtomPoint, amino.CarbonylCarbonAtomPoint);
				for (int i = monomerCount; --i > 0; )
				{
					amino = (AminoMonomer) aminoPolymer.monomers[i];
					vectorCO.sub(amino.CarbonylOxygenAtomPoint, amino.CarbonylCarbonAtomPoint);
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					if (vectorCOSum.angle(vectorCO) < (float) System.Math.PI / 2)
						vectorCOSum.add(vectorCO);
					else
						vectorCOSum.sub(vectorCO);
				}
				heightUnitVector = vectorCO; // just reuse the same temp vector;
				heightUnitVector.cross(axisUnitVector, vectorCOSum);
				heightUnitVector.normalize();
				widthUnitVector = vectorCOSum;
				widthUnitVector.cross(axisUnitVector, heightUnitVector);
			}
		}
	}
}