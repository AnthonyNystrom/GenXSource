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
	class Helix : ProteinStructure
	{
		public Helix(AlphaPolymer apolymer, int monomerIndex, int monomerCount)
            : base(apolymer, JmolConstants.PROTEIN_STRUCTURE_HELIX, monomerIndex, monomerCount)
		{
			//    System.out.println("new Helix('" + polymer.chain.chainID + "'," +
			//                       polymerIndex + "," + monomerCount + ")");
		}
		
		// copied from sheet -- not correct
        public override void calcAxis()
		{
			if (axisA != null)
				return ;
			
			axisA = new Point3f();
			if (lowerNeighborIsHelixOrSheet())
				apolymer.getLeadMidPoint(monomerIndex, axisA);
			else
				apolymer.getLeadMidPoint(monomerIndex + 1, axisA);
			
			axisB = new Point3f();
			if (upperNeighborIsHelixOrSheet())
				apolymer.getLeadMidPoint(monomerIndex + monomerCount, axisB);
			else
				apolymer.getLeadMidPoint(monomerIndex + monomerCount - 1, axisB);
			
			axisUnitVector = new Vector3f();
			axisUnitVector.sub(axisB, axisA);
			axisUnitVector.normalize();
			
			Point3f tempA = new Point3f();
			apolymer.getLeadMidPoint(monomerIndex, tempA);
			projectOntoAxis(tempA);
			Point3f tempB = new Point3f();
			apolymer.getLeadMidPoint(monomerIndex + monomerCount, tempB);
			projectOntoAxis(tempB);
			axisA = tempA;
			axisB = tempB;
		}
		
		
		/// <summary>*************************************************************
		/// see:
		/// Defining the Axis of a Helix
		/// Peter C Kahn
		/// Computers Chem. Vol 13, No 3, pp 185-189, 1989
		/// 
		/// Simple Methods for Computing the Least Squares Line
		/// in Three Dimensions
		/// Peter C Kahn
		/// Computers Chem. Vol 13, No 3, pp 191-195, 1989
		/// **************************************************************
		/// </summary>
        public virtual void calcCenter()
		{
			if (center == null)
			{
				int i = monomerIndex + monomerCount - 1;
				center = new Point3f(apolymer.getLeadPoint(i));
				while (--i >= monomerIndex)
					center.add(apolymer.getLeadPoint(i));
				center.scale(1f / monomerCount);
				//      System.out.println("structure center is at :" + center);
			}
		}

        public static float length(Point3f point)
		{
			return (float) Math.Sqrt(point.x * point.x + point.y * point.y + point.z * point.z);
		}

        public float sumXiLi, sumYiLi, sumZiLi;
        public virtual void calcSums(int count, Point3f[] points, float[] lengths)
		{
			sumXiLi = sumYiLi = sumZiLi = 0;
			for (int i = count; --i >= 0; )
			{
				Point3f point = points[i];
				float length = lengths[i];
				sumXiLi += point.x * length;
				sumYiLi += point.y * length;
				sumZiLi += point.z * length;
			}
		}

        public float cosineX, cosineY, cosineZ;
        public virtual void calcDirectionCosines()
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float denominator = (float) System.Math.Sqrt(sumXiLi * sumXiLi + sumYiLi * sumYiLi + sumZiLi * sumZiLi);
			cosineX = sumXiLi / denominator;
			cosineY = sumYiLi / denominator;
			cosineZ = sumZiLi / denominator;
		}
	}
}