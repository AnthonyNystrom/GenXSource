/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2003-2005  Miguel, Jmol Development, www.jmol.org
*
* Contact: miguel@jmol.org
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
	class RibbonsRenderer : MpsRenderer
	{
		// not current for Mcp class

        public Ribbons strands;
        public bool isNucleic;
        public bool ribbonBorder = false;
		
		public readonly Point3f pointT = new Point3f();

        public virtual Point3i[] calcScreens(Point3f[] centers, Vector3f[] vectors, short[] mads, float offsetFraction)
        {
            // this basically does object->screen transformation??
            Point3i[] screens = new Point3i[centers.Length];// viewer.allocTempScreens(centers.Length);
            if (offsetFraction == 0)
            {
                for (int i = centers.Length; --i >= 0; )
                    screens[i] = new Point3i((int)centers[i].x, (int)centers[i].y, (int)centers[i].z);
                    //viewer.transformPoint(centers[i], screens[i]);
            }
            else
            {
                offsetFraction /= 1000;
                for (int i = centers.Length; --i >= 0; )
                {
                    pointT.set_Renamed(vectors[i]);
                    float scale = mads[i] * offsetFraction;
                    pointT.scaleAdd(scale, centers[i]);
                    //if (float.IsNaN(pointT.x))
                    //{
                    //    System.Console.Out.WriteLine(" vectors[" + i + "]=" + vectors[i] + " centers[" + i + "]=" + centers[i] + " mads[" + i + "]=" + mads[i] + " scale=" + scale + " --> " + pointT);
                    //}
                    //viewer.transformPoint(pointT, screens[i]);
                    screens[i] = new Point3i((int)pointT.x, (int)pointT.y, (int)pointT.z);
                }
            }
            return screens;
        }

        public override void renderMpspolymer(Mps.Mpspolymer mpspolymer)
		{
			Ribbons.Schain strandsChain = (Ribbons.Schain) mpspolymer;
			if (strandsChain.wingVectors != null)
			{
				// note that we are not treating a PhosphorusPolymer
				// as nucleic because we are not calculating the wing
				// vector correctly.
				// if/when we do that then this test will become
				//isNucleic = strandsChain.polymer.Nucleic;
                ribbonBorder = false;// viewer.RibbonBorder;
				isNucleic = strandsChain.polymer is NucleicPolymer;
				render1Chain(strandsChain.monomerCount, strandsChain.monomers, strandsChain.leadMidpoints, strandsChain.wingVectors, strandsChain.mads, strandsChain.colixes);
			}
		}

        public virtual void render1Chain(int monomerCount, Monomer[] monomers, Point3f[] centers, Vector3f[] vectors, short[] mads, short[] colixes)
		{
			Point3i[] ribbonTopScreens;
			Point3i[] ribbonBottomScreens;
			
			ribbonTopScreens = calcScreens(centers, vectors, mads, isNucleic?1f:0.5f);
			ribbonBottomScreens = calcScreens(centers, vectors, mads, isNucleic?0f:- 0.5f);
			render2Strand(monomerCount, monomers, mads, colixes, ribbonTopScreens, ribbonBottomScreens);
            //viewer.freeTempScreens(ribbonTopScreens);
            //viewer.freeTempScreens(ribbonBottomScreens);
		}

        public virtual void render2Strand(int monomerCount, Monomer[] monomers, short[] mads, short[] colixes, Point3i[] ribbonTopScreens, Point3i[] ribbonBottomScreens)
		{
			for (int i = monomerCount; --i >= 0; )
				if (mads[i] > 0)
					render2StrandSegment(monomerCount, monomers[i], colixes[i], mads, ribbonTopScreens, ribbonBottomScreens, i);
		}

        public virtual void render2StrandSegment(int monomerCount, Monomer monomer, short colix, short[] mads, Point3i[] ribbonTopScreens, Point3i[] ribbonBottomScreens, int i)
		{
			int iLast = monomerCount;
			int iPrev = i - 1;
			if (iPrev < 0)
				iPrev = 0;
			int iNext = i + 1;
			if (iNext > iLast)
				iNext = iLast;
			int iNext2 = i + 2;
			if (iNext2 > iLast)
				iNext2 = iLast;
            //colix = Graphics3D.inheritColix(colix, monomer.LeadAtom.colixAtom);
			
            g3d.drawHermite(true, ribbonBorder, colix, isNucleic?4:7, ribbonTopScreens[iPrev], ribbonTopScreens[i], ribbonTopScreens[iNext], ribbonTopScreens[iNext2], ribbonBottomScreens[iPrev], ribbonBottomScreens[i], ribbonBottomScreens[iNext], ribbonBottomScreens[iNext2]);
		}
	}
}