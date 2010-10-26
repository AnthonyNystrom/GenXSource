/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2003-2005  The Jmol Development Team
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
	abstract class MpsRenderer : ShapeRenderer
	{
        public override void render()
		{
			if (shape == null)
				return ;
			Mps mcps = (Mps) shape;
			for (int m = mcps.MpsmodelCount; --m >= 0; )
			{
				Mps.Mpsmodel mcpsmodel = mcps.getMpsmodel(m);
				if (displayModelIndex >= 0 && displayModelIndex != mcpsmodel.modelIndex)
					continue;
				for (int c = mcpsmodel.MpspolymerCount; --c >= 0; )
				{
					Mps.Mpspolymer mpspolymer = mcpsmodel.getMpspolymer(c);
					if (mpspolymer.monomerCount >= 2)
						renderMpspolymer(mpspolymer);
				}
			}
		}

        public abstract void renderMpspolymer(Mps.Mpspolymer mpspolymer);
		
		////////////////////////////////////////////////////////////////
		// some utilities
        public bool[] calcIsSpecials(int monomerCount, Monomer[] monomers)
		{
            bool[] isSpecials = new bool[monomerCount + 1];// null;// viewer.allocTempBooleans(monomerCount + 1);
			for (int i = monomerCount; --i >= 0; )
				isSpecials[i] = monomers[i].HelixOrSheet;
			isSpecials[monomerCount] = isSpecials[monomerCount - 1];
			return isSpecials;
		}

        public Point3i[] calcScreenLeadMidpoints(int monomerCount, Point3f[] leadMidpoints)
		{
			int count = monomerCount + 1;
            Point3i[] leadMidpointScreens = new Point3i[count];// null;// viewer.allocTempScreens(count);
			for (int i = count; --i >= 0; )
			{
                leadMidpointScreens[i] = new Point3i((int)leadMidpoints[i].x, (int)leadMidpoints[i].y, (int)leadMidpoints[i].z);
				//viewer.transformPoint(leadMidpoints[i], leadMidpointScreens[i]);
				//g3d.fillSphereCentered(Graphics3D.CYAN, 15, leadMidpointScreens[i]);
                g3d.fillSphereCentered(0, 15, leadMidpointScreens[i]);
			}
			return leadMidpointScreens;
		}

        public void renderRopeSegment(short colix, short[] mads, int i, int monomerCount, Monomer[] monomers, Point3i[] leadMidpointScreens, bool[] isSpecials)
		{
			int iPrev1 = i - 1;
			if (iPrev1 < 0)
				iPrev1 = 0;
			int iNext1 = i + 1;
			if (iNext1 > monomerCount)
				iNext1 = monomerCount;
			int iNext2 = i + 2;
			if (iNext2 > monomerCount)
				iNext2 = monomerCount;
			
			int madThis, madBeg, madEnd;
			madThis = madBeg = madEnd = mads[i];
			if (isSpecials != null)
			{
				if (!isSpecials[iPrev1])
					madBeg = (mads[iPrev1] + madThis) / 2;
				if (!isSpecials[iNext1])
					madEnd = (mads[iNext1] + madThis) / 2;
			}
            int diameterBeg = leadMidpointScreens[i].z;//viewer.scaleToScreen(leadMidpointScreens[i].z, madBeg);
            int diameterEnd = leadMidpointScreens[iNext1].z;//viewer.scaleToScreen(leadMidpointScreens[iNext1].z, madEnd);
            int diameterMid = monomers[i].LeadAtom.ScreenZ;//viewer.scaleToScreen(monomers[i].LeadAtom.ScreenZ, madThis);
            g3d.fillHermite(colix, monomers[i].Nucleic?4:7, diameterBeg, diameterMid, diameterEnd, leadMidpointScreens[iPrev1], leadMidpointScreens[i], leadMidpointScreens[iNext1], leadMidpointScreens[iNext2]);
		}
	}
}