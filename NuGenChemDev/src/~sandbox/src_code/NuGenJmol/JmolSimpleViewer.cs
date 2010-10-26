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
using System.Drawing;

namespace Org.Jmol.Api
{
    /// <summary>
    /// This is the high-level API for the JmolViewer for simple access.
    /// </summary>
    abstract class JmolSimpleViewer
    {
        public static JmolSimpleViewer allocateSimpleViewer(/*Component awtComponent, JmolAdapter jmolAdapter*/)
        {
            return Viewer.Viewer.allocateViewer(/*awtComponent, jmolAdapter*/);
        }

        abstract public void renderScreenImage(Graphics g, ref Size size, ref Rectangle clip);

        public abstract string evalFile(string strFilename);
        public abstract string evalstring(string strScript);

        public abstract void openstringInline(string strModel);
        public abstract void openDOM(object DOMNode);
        public abstract void openFile(string name);
        //public abstract string getOpenFileError();
    }

}