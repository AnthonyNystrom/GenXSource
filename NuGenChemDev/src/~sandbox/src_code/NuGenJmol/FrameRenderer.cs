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
using NuGenJmol;

namespace Org.Jmol.Viewer
{
	class FrameRenderer
	{
        //public Viewer viewer;
		
		public ShapeRenderer[] renderers = new ShapeRenderer[JmolConstants.SHAPE_MAX];
		
		public FrameRenderer(/*Viewer viewer*/)
		{
            //this.viewer = viewer;
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
        public virtual void render(NuGraphics3D g3d, ref System.Drawing.Rectangle rectClip, Frame frame, int displayModelIndex, int shapeType)
		{
            if (frame == null || frame.atomCount <= 0)
                return;

            //viewer.calcTransformMatrices();

            //for (int i = 0; i < JmolConstants.SHAPE_MAX; ++i)
            //{
            Shape shape = frame.shapes[shapeType/*i*/];
                //if (shape == null)
                //    continue;
            if (shape != null)
                getRenderer(shapeType/*i*/, g3d).render(g3d, ref rectClip, frame, displayModelIndex, shape);
            //}

            g3d.Flush();
		}

        public virtual ShapeRenderer getRenderer(int refShape, NuGraphics3D g3d)
		{
			if (renderers[refShape] == null)
				renderers[refShape] = allocateRenderer(refShape, g3d);
			return renderers[refShape];
		}

        public virtual ShapeRenderer allocateRenderer(int refShape, NuGraphics3D g3d)
		{
			string classBase = JmolConstants.shapeClassBases[refShape] + "Renderer";
			string className = "Org.Jmol.Viewer." + classBase;
			
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				System.Type shapeClass = System.Type.GetType(className);
				ShapeRenderer renderer = (ShapeRenderer) System.Activator.CreateInstance(shapeClass);
                renderer.setViewerFrameRenderer(/*viewer, */this, g3d);
				return renderer;
			}
			catch (System.Exception e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine("Could not instantiate renderer:" + classBase + "\n" + e);
				SupportClass.WriteStackTrace(e, Console.Error);
			}
			return null;
		}

        //public virtual void renderStringOutside(string str, short colix, Font3D font3d, Point3i screen, Graphics3D g3d)
        //{
        //    renderStringOutside(str, colix, font3d, screen.x, screen.y, screen.z, g3d);
        //}

        //public virtual void renderStringOutside(string str, short colix, Font3D font3d, int x, int y, int z, Graphics3D g3d)
        //{
        //    System.Drawing.Font fontMetrics = font3d.fontMetrics;
        //    //UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
        //    int strAscent = SupportClass.GetAscent(fontMetrics);
        //    //UPGRADE_ISSUE: Method 'java.awt.FontMetrics.stringWidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtFontMetricsstringWidth_javalangString'"
        //    int strWidth = fontMetrics.stringWidth(str);
        //    int xStrCenter, yStrCenter;
        //    int xCenter = viewer.BoundBoxCenterX;
        //    int yCenter = viewer.BoundBoxCenterY;
        //    int dx = x - xCenter;
        //    int dy = y - yCenter;
        //    if (dx == 0 && dy == 0)
        //    {
        //        xStrCenter = x;
        //        yStrCenter = y;
        //    }
        //    else
        //    {
        //        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        //        int dist = (int) System.Math.Sqrt(dx * dx + dy * dy);
        //        xStrCenter = xCenter + ((dist + 2 + (strWidth + 1) / 2) * dx / dist);
        //        yStrCenter = yCenter + ((dist + 3 + (strAscent + 1) / 2) * dy / dist);
        //    }
        //    int xStrBaseline = xStrCenter - strWidth / 2;
        //    int yStrBaseline = yStrCenter + strAscent / 2;
        //    g3d.drawString(str, font3d, colix, xStrBaseline, yStrBaseline, z);
        //}
	}
}