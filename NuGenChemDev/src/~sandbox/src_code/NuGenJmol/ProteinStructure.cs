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
	abstract class ProteinStructure
	{
		public virtual int MonomerCount
		{
            get { return monomerCount; }
		}

		public virtual int MonomerIndex
		{
            get { return monomerIndex; }
		}

		public virtual Point3f[] Segments
		{
			get
			{
				if (segments == null)
					calcSegments();
				return segments;
			}
		}

		public virtual Point3f AxisStartPoint
		{
			get
			{
				calcAxis();
				return axisA;
			}
		}

		public virtual Point3f AxisEndPoint
		{
			get
			{
				calcAxis();
				return axisB;
			}
		}

        public AlphaPolymer apolymer;
        public sbyte type;
        public int monomerIndex;
        public int monomerCount;
        public Point3f center;
        public Point3f axisA, axisB;
        public Vector3f axisUnitVector;
        public Point3f[] segments;

        public ProteinStructure(AlphaPolymer apolymer, sbyte type, int monomerIndex, int monomerCount)
		{
			this.apolymer = apolymer;
			this.type = type;
			this.monomerIndex = monomerIndex;
			this.monomerCount = monomerCount;
		}

        public virtual void calcAxis() { }

        public virtual void calcSegments()
		{
			if (segments != null)
				return ;
			calcAxis();
			/*
			System.out.println("axisA=" + axisA.x + "," + axisA.y + "," + axisA.z);
			System.out.println("axisB=" + axisB.x + "," + axisB.y + "," + axisB.z);
			*/
			segments = new Point3f[monomerCount + 1];
			segments[monomerCount] = axisB;
			segments[0] = axisA;
			for (int i = monomerCount; --i > 0; )
			{
				Point3f point = segments[i] = new Point3f();
				apolymer.getLeadMidPoint(monomerIndex + i, point);
				projectOntoAxis(point);
			}
			/*
			for (int i = 0; i < segments.length; ++i) {
			Point3f point = segments[i];
			System.out.println("segment[" + i + "]=" +
			point.x + "," + point.y + "," + point.z);
			}
			*/
		}

        public virtual bool lowerNeighborIsHelixOrSheet()
		{
			if (monomerIndex == 0)
				return false;
			return apolymer.monomers[monomerIndex - 1].HelixOrSheet;
		}

        public virtual bool upperNeighborIsHelixOrSheet()
		{
			int upperNeighborIndex = monomerIndex + monomerCount;
			if (upperNeighborIndex == apolymer.monomerCount)
				return false;
			return apolymer.monomers[upperNeighborIndex].HelixOrSheet;
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorProjection '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public Vector3f vectorProjection = new Vector3f();

        public virtual void projectOntoAxis(Point3f point)
		{
			// assumes axisA, axisB, and axisUnitVector are set;
			vectorProjection.sub(point, axisA);
			float projectedLength = vectorProjection.dot(axisUnitVector);
			point.set_Renamed(axisUnitVector);
			point.scaleAdd(projectedLength, axisA);
		}

        public virtual int getIndex(Monomer monomer)
		{
			Monomer[] monomers = apolymer.monomers;
			int i;
			for (i = monomerCount; --i >= 0; )
				if (monomers[monomerIndex + i] == monomer)
					break;
			return i;
		}

        public virtual Point3f getStructureMidPoint(int index)
		{
			if (segments == null)
				calcSegments();
			/*
			Point3f point = segments[residueIndex - startResidueIndex];
			System.out.println("Structure.getStructureMidpoint(" +
			residueIndex + ") -> " +
			point.x + "," + point.y + "," + point.z);
			*/
			return segments[index];
		}
	}
}