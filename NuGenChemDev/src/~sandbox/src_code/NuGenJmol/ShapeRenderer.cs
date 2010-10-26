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
using NuGenJmol;

namespace Org.Jmol.Viewer
{
	abstract class ShapeRenderer
	{
        //public Viewer viewer;
        public FrameRenderer frameRenderer;
        public NuGraphics3D g3d;
        public System.Drawing.Rectangle rectClip;
        public Frame frame;
        public int displayModelIndex;
        public Shape shape;

        public void setViewerFrameRenderer(/*Viewer viewer,*/ FrameRenderer frameRenderer, NuGraphics3D g3d)
		{
            //this.viewer = viewer;
			this.frameRenderer = frameRenderer;
            this.g3d = g3d;
			initRenderer();
		}

        public virtual void initRenderer()
		{ }
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
        public virtual void render(NuGraphics3D g3d, ref System.Drawing.Rectangle rectClip, Frame frame, int displayModelIndex, Shape shape)
		{
            this.g3d = g3d;
			this.rectClip = rectClip;
			this.frame = frame;
			this.displayModelIndex = displayModelIndex;
			this.shape = shape;
			render();
		}

        public abstract void render();
	}
}