/* $RCSfile$
 * $Author: migueljmol $
 * $Date: 2006-04-02 16:06:20 +0200 (dim., 02 avr. 2006) $
 * $Revision: 4871 $
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
using System.Collections;
using System.Collections.Specialized;
using javax.vecmath;
namespace Org.Jmol.Api
{
    /// <summary>
    /// This is the high-level API for the JmolViewer for simple access.
    /// <para>
    /// We will implement a low-level API at some point
    /// </summary>
    abstract class JmolViewer : JmolSimpleViewer
    {
        public static JmolViewer allocateViewer(/*Component awtComponent, JmolAdapter jmolAdapter*/)
        {
            return Viewer.Viewer.allocateViewer(/*awtComponent, jmolAdapter*/);
        }

        //public abstract void setJmolStatusListener(JmolStatusListener jmolStatusListener);

        //public abstract void setAppletContext(URL documentBase, URL codeBase, String appletProxy);

        public abstract void haltScriptExecution();

        //public abstract bool isJvm12orGreater();
        //public abstract string getOperatingSystemName();
        //public abstract string getJavaVersion();
        //public abstract string getJavaVendor();

        public abstract bool haveFrame();

        public abstract void pushHoldRepaint();
        public abstract void popHoldRepaint();

        public abstract void setJmolDefaults();
        public abstract void setRasmolDefaults();
        public abstract void setDebugScript(bool debugScript);

        public abstract void setFrankOn(bool frankOn);

        // change this to width, height
        public abstract Size ScreenDimension { set; }
        public abstract int ScreenWidth { get; }
        public abstract int ScreenHeight { get; }

        public abstract Image ScreenImage { get; }
        public abstract void releaseScreenImage();


        public abstract void notifyRepainted();

        //public abstract bool handleOldJvm10Event(Event e);

        public abstract int MotionEventNumber { get; }

        //public abstract void openReader(String fullPathName, String name, Reader reader);
        //public abstract void openClientFile(String fullPathName, String fileName, Object clientFile);

        //public abstract void showUrl(String urlString);

        public abstract void deleteMeasurement(int i);
        public abstract void clearMeasurements();
        public abstract int MeasurementCount { get; }
        public abstract string getMeasurementStringValue(int i);
        public abstract int[] getMeasurementCountPlusIndices(int i);

        //public abstract Component getAwtComponent();

        public abstract BitArray ElementsPresentBitSet { get; }

        public abstract int AnimationFps { get; set; }

        public abstract string evalStringQuiet(string script);
        public abstract bool ScriptExecuting { get; }

        public abstract float VectorScale { get; }
        public abstract float VibrationScale { get; set; }
        public abstract float VibrationPeriod { set; }

        //public abstract string ModelSetName { get; }
        //public abstract string ModelSetFileName { get; }
        //public abstract string ModelSetPathName { get; }
        public abstract NameValueCollection ModelSetProperties { get; }
        public abstract int getModelNumber(int atomSetIndex);
        public abstract string getModelName(int atomSetIndex);
        public abstract NameValueCollection getModelProperties(int atomSetIndex);
        public abstract string getModelProperty(int atomSetIndex, string propertyName);
        public abstract bool modelHasVibrationVectors(int atomSetIndex);

        public abstract int ModelCount { get; }
        public abstract int DisplayModelIndex { get; }
        public abstract int AtomCount { get; }
        public abstract int BondCount { get; }
        public abstract int GroupCount { get; }
        public abstract int ChainCount { get; }
        public abstract int PolymerCount { get; }
        public abstract int getPolymerCountInModel(int modelIndex);

        public abstract int ModeMouse { set; }
        public abstract bool SelectionHaloEnabled { get; set; }

        public abstract bool ShowHydrogens { get; set; }
        public abstract bool ShowMeasurements { get; set; }

        public abstract void selectAll();
        public abstract void clearSelection();

        //public abstract void addSelectionListener(JmolSelectionListener listener);
        //public abstract void removeSelectionListener(JmolSelectionListener listener);
        public abstract BitArray SelectionSet { get; set; }

        public abstract void homePosition();
        public abstract void rotateFront();
        public abstract void rotateToX(int degrees);
        public abstract void rotateToY(int degrees);

        public abstract void rotateToX(float radians);
        public abstract void rotateToY(float radians);
        public abstract void rotateToZ(float radians);

        public abstract void setCenterSelected();

        public abstract BitArray GroupsPresentBitSet { get; }

        public abstract bool PerspectiveDepth { get; set; }

        public abstract bool ShowAxes { get; set; }
        public abstract bool ShowBbcage { get; set; }

        public abstract int getAtomNumber(int i);
        public abstract string getAtomName(int i);

        public abstract float RotationRadius { get; }

        public abstract int ZoomPercent { get; }
        public abstract Matrix4f UnscaledTransformMatrix { get; }

        public abstract int BackgroundArgb { get; set; }
        public abstract string ColorBackground { set; }

        public abstract float getAtomRadius(int i);
        public abstract Point3f getAtomPoint3f(int i);
        public abstract int getAtomArgb(int i);
        public abstract int getAtomModelIndex(int i);

        public abstract float getBondRadius(int i);
        public abstract Point3f getBondPoint3f1(int i);
        public abstract Point3f getBondPoint3f2(int i);
        public abstract int getBondArgb1(int i);
        public abstract int getBondArgb2(int i);
        public abstract short getBondOrder(int i);
        public abstract int getBondModelIndex(int i);

        public abstract Point3f[] getPolymerLeadMidPoints(int modelIndex, int polymerIndex);

        public abstract bool AxesOrientationRasmol { get; set; }
        public abstract int PercentVdwAtom { get; set; }

        public abstract bool AutoBond { get; set; }

        // EVIL!
        public abstract short MadBond { get; }

        public abstract void refresh();

        public abstract bool getBooleanProperty(string propertyName);
        public abstract void setBooleanProperty(string propertyName, bool value);

        public abstract bool showModelSetDownload();
    }
}