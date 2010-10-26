/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-04-11 04:43:39 +0200 (mar., 11 avr. 2006) $
* $Revision: 4951 $
*
* Copyright (C) 2002-2006  Miguel, Jmol Development, www.jmol.org
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
using Org.Jmol.Api;
using javax.vecmath;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;

namespace Org.Jmol.Viewer
{
    /// <summary>
    /// The JmolViewer can be used to render client molecules. Clients
    /// implement the JmolAdapter. JmolViewer uses this interface
    /// to extract information from the client data structures and
    /// render the molecule to the supplied java.awt.Component
    ///
    /// The JmolViewer runs on Java 1.1 virtual machines.
    /// The 3d graphics rendering package is a software implementation
    /// of a z-buffer. It does not use Java3D and does not use Graphics2D
    /// from Java 1.2. Therefore, it is well suited to building web browser
    /// applets that will run on a wide variety of system configurations.
    /// </summary>
	class Viewer : JmolViewer
	{
        //public System.Windows.Forms.Control AwtComponent
        //{
        //    get { return awtComponent; }
        //}

		public float TranslationXPercent
		{
            get { return 0;/* transformManager.TranslationXPercent;*/ }
		}

		public float TranslationYPercent
		{
            get { return 0;/* transformManager.TranslationYPercent;*/ }
		}

        public float TranslationZPercent
		{
            get { return 0;/* transformManager.TranslationZPercent;*/ }
		}
        public override int ZoomPercent
		{
            get { return 0;/* transformManager.zoomPercent;*/ }
		}

        public int ZoomPercentSetting
		{
            get { return 0;/* transformManager.zoomPercentSetting;*/ }
		}

        public bool ZoomEnabled
		{
            get { return false;/* transformManager.zoomEnabled;*/ }
			set
			{
				/*transformManager.ZoomEnabled = value;*/
				refresh();
			}
		}

        public bool SlabEnabled
		{
            get
            { return false;/*transformManager.slabEnabled;*/ }
			set
			{
                //transformManager.SlabEnabled = value;
				refresh();
			}
		}

        public int SlabPercentSetting
		{
            get { return 0;/* transformManager.slabPercentSetting;*/ }
		}

        public int ModeSlab
		{
            get { return 0;/* transformManager.modeSlab;*/ }
			set
			{
                //transformManager.ModeSlab = value;
				refresh();
			}
		}

        public override Matrix4f UnscaledTransformMatrix
		{
            get { return null;/* transformManager.UnscaledTransformMatrix;*/ }
		}

        public float ScalePixelsPerAngstrom
		{
            get { return 0;/* transformManager.scalePixelsPerAngstrom;*/ }
		}

        public override bool PerspectiveDepth
		{
            get { return false;/* transformManager.perspectiveDepth; */}
			set
			{
                //transformManager.PerspectiveDepth = value;
				refresh();
			}
		}

        public override bool AxesOrientationRasmol
		{
            get { return false;/* transformManager.axesOrientationRasmol;*/ }
			set
			{
                //transformManager.AxesOrientationRasmol = value;
				refresh();
			}
		}

        public float CameraDepth
		{
            get { return 0;/* transformManager.cameraDepth;*/ }
            set { /*transformManager.CameraDepth = value;*/ }
		}

        public override Size ScreenDimension
		{
			set
			{
				// There is a bug in Netscape 4.7*+MacOS 9 when comparing dimension objects
				// so don't try dim1.equals(dim2)
				int height = value.Height;
				int width = value.Width;
				if (StereoMode == JmolConstants.STEREO_DOUBLE)
					width = (width + 1) / 2;
				if (dimScreen.Width == width && dimScreen.Height == height)
					return ;
				dimScreen.Width = width;
				dimScreen.Height = height;
                //transformManager.setScreenDimension(width, height);
                //transformManager.scaleFitToScreen();
                //g3d.setWindowSize(width, height, enableFullSceneAntialiasing);
			}
		}

        public override int ScreenWidth
		{
            get { return dimScreen.Width; }
		}

        public override int ScreenHeight
		{
			get { return dimScreen.Height; }
		}

        public Rectangle RectClip
		{
			set
			{
				if (value.IsEmpty)
				{
					rectClip.X = rectClip.Y = 0;
					rectClip.Size = dimScreen;
				}
				else
				{
                    //SupportClass.RectangleSupport.SetBoundsRectangle(ref rectClip, value);
					// on Linux platform with Sun 1.4.2_02 I am getting a clipping rectangle
					// that is wider than the current window during window resize
					if (rectClip.X < 0)
						rectClip.X = 0;
					if (rectClip.Y < 0)
						rectClip.Y = 0;
					if (rectClip.X + rectClip.Width > dimScreen.Width)
						rectClip.Width = dimScreen.Width - rectClip.X;
					if (rectClip.Y + rectClip.Height > dimScreen.Height)
						rectClip.Height = dimScreen.Height - rectClip.Y;
				}
			}
		}

        public float ScaleAngstromsPerInch
		{
            set { /*transformManager.ScaleAngstromsPerInch = value;*/ }
		}

		public override float VibrationPeriod
		{
            set { /*transformManager.VibrationPeriod = value;*/ }
		}

        public float VibrationT
		{
			set { /*transformManager.VibrationT = value;*/ }
		}

        public float VibrationRadians
		{
            get { return 0;/* transformManager.vibrationRadians;*/ }
		}

        public int SpinX
        {
            get { return 0;/* transformManager.spinX;*/ }
            set { /*transformManager.SpinX = value;*/ }
        }

        public int SpinY
		{
            get { return 0;/* transformManager.spinY;*/ }
            set { /*transformManager.SpinY = value;*/ }
		}

        public int SpinZ
		{
            get { return 0;/* transformManager.spinZ;*/ }
            set { /*transformManager.SpinZ = value;*/ }
		}

        public int SpinFps
		{
            get { return 0;/* transformManager.spinFps;*/ }
            set { /*transformManager.SpinFps = value;*/ }
		}

        public bool SpinOn
		{
            get { return false;/* transformManager.spinOn;*/ }
            set { /*transformManager.SpinOn = value;*/ }
		}

        public string OrientationText
		{
            get { return null;/* transformManager.OrientationText; */}
		}

		public string TransformText
		{
            get { return null;/* transformManager.TransformText;*/ }
		}

        public string DefaultColors
		{
			/////////////////////////////////////////////////////////////////
			// delegated to ColorManager
			/////////////////////////////////////////////////////////////////
            set { /*colorManager.DefaultColors = value;*/ }
		}

		public int SelectionArgb
		{
			set
			{
                //colorManager.SelectionArgb = value;
				refresh();
			}
		}

        public short ColixSelection
		{
            get { return 0;/* colorManager.ColixSelection;*/ }
		}

        public int RubberbandArgb
		{
            set { /*colorManager.RubberbandArgb = value;*/ }
		}

        public short ColixRubberband
		{
            get { return 0;/* colorManager.colixRubberband;*/ }
		}

        public override int BackgroundArgb
		{
            get { return 0;/* colorManager.argbBackground;*/ }
			set
			{
                //colorManager.BackgroundArgb = value;
				refresh();
			}
		}

        public override string ColorBackground
		{
			set
			{
                //colorManager.ColorBackground = value;
				refresh();
			}
		}

        public short ColixBackgroundContrast
		{
            get { return 0;/* colorManager.colixBackgroundContrast;*/ }
		}

        public bool Specular
		{
            get { return false; /*colorManager.Specular;*/ }
            set { /*colorManager.Specular = value;*/ }
		}

        public int SpecularPower
		{
            set { /*colorManager.SpecularPower = value;*/ }
		}

        public int AmbientPercent
		{
            set { /*colorManager.AmbientPercent = value;*/ }
		}

        public int DiffusePercent
		{
            set { /*colorManager.DiffusePercent = value;*/ }
		}

        public int SpecularPercent
		{
            set { /*colorManager.SpecularPercent = value;*/ }
		}

        public float LightsourceZ
		{
			// x & y light source coordinates are fixed at -1,-1
			// z should be in the range 0, +/- 3 ?
            set { /*colorManager.LightsourceZ = value; */}
		}

        public int Selection
		{
			set
			{
                //selectionManager.Selection = value;
				refresh();
			}
		}

        public bool BondSelectionModeOr
		{
			get { return bondSelectionModeOr; }
			set
			{
				this.bondSelectionModeOr = value;
				refresh();
			}
		}

        public override BitArray SelectionSet
		{
            get { return null;/* selectionManager.bsSelection;*/ }
			set
			{
                //selectionManager.SelectionSet = value;
				refresh();
			}
		}

        public int SelectionCount
		{
            get { return 0;/*selectionManager.SelectionCount;*/ }
		}

        public override int ModeMouse
		{
			/////////////////////////////////////////////////////////////////
			// delegated to MouseManager
			/////////////////////////////////////////////////////////////////
            set { } // deprecated
		}

        public Rectangle RubberBandSelection
		{
            get { return Rectangle.Empty;/* mouseManager.RubberBand;*/ }
		}

        public int CursorX
		{
            get { return 0;/* mouseManager.xCurrent;*/ }
		}

		public int CursorY
		{
            get { return 0;/* mouseManager.yCurrent;*/ }
		}

        //public string OpenFileError
        //{
        //    get
        //    {
        //        string errorMsg = OpenFileError1;
        //        return errorMsg;
        //    }
        //}

        //public string OpenFileError1
        //{
        //    get
        //    {
        //        string fullPathName = fileManager.FullPathName;
        //        string fileName = fileManager.FileName;
        //        object clientFile = fileManager.waitForClientFileOrErrorMessage();
        //        if (clientFile is string || clientFile == null)
        //        {
        //            string errorMsg = (string) clientFile;
        //            notifyFileNotLoaded(fullPathName, errorMsg);
        //            return errorMsg;
        //        }
        //        openClientFile(fullPathName, fileName, clientFile);
        //        notifyFileLoaded(fullPathName, fileName, modelManager.ModelSetName, clientFile);
        //        return null;
        //    }
        //}

        //public string CurrentFileAsString
        //{
        //    get
        //    {
        //        string pathName = modelManager.ModelSetPathName;
        //        if (pathName == null)
        //            return null;
        //        return fileManager.getFileAsString(pathName);
        //    }
        //}

        //public string ModelSetName
        //{
        //    get { return modelManager.ModelSetName; }
        //}

        //public string ModelSetFileName
        //{
        //    get { return modelManager.ModelSetFileName; }
        //}

        //public string ModelSetPathName
        //{
        //    get { return modelManager.ModelSetPathName; }
        //}

        //public string ModelSetTypeName
        //{
        //    get { return modelManager.ModelSetTypeName; }
        //}

        public object ClientFile
		{
            get
            {
                // DEPRECATED - use getExportJmolAdapter()
                return null;
            }
		}

		/// <summary>*************************************************************
		/// This is the method that should be used to extract the model
		/// data from Jmol.
		/// Note that the API provided by JmolAdapter is used to
		/// import data into Jmol and to export data out of Jmol.
		/// 
		/// When exporting, a few of the methods in JmolAdapter do
		/// not make sense.
		/// openBufferedReader(...)
		/// Others may be implemented in the future, but are not currently
		/// all pdb specific things
		/// Just pass in null for the methods that want a clientFile.
		/// The main methods to use are
		/// getFrameCount(null) -> currently always returns 1
		/// getAtomCount(null, 0)
		/// getAtomIterator(null, 0)
		/// getBondIterator(null, 0)
		/// 
		/// The AtomIterator and BondIterator return Objects as unique IDs
		/// to identify the atoms.
		/// atomIterator.getAtomUid()
		/// bondIterator.getAtomUid1() & bondIterator.getAtomUid2()
		/// The ExportJmolAdapter will return the 0-based atom index as
		/// a boxed Integer. That means that you can cast the results to get
		/// a zero-based atom index
		/// int atomIndex = ((Integer)atomIterator.getAtomUid()).intValue();
		/// ...
		/// int bondedAtom1 = ((Integer)bondIterator.getAtomUid1()).intValue();
		/// int bondedAtom2 = ((Integer)bondIterator.getAtomUid2()).intValue();
		/// 
		/// post questions to jmol-developers@lists.sf.net
		/// </summary>
		/// <returns> A JmolAdapter
		/// **************************************************************
		/// </returns>
        //public JmolAdapter ExportJmolAdapter
        //{
        //    get { return modelManager.ExportJmolAdapter; }
        //}

        public Frame Frame
		{
            get { return null;/* modelManager.Frame;*/ }
		}

        public override float RotationRadius
		{
            get { return 0;/* modelManager.RotationRadius;*/ }
		}

        public Point3f RotationCenter
		{
			get { return null;/*modelManager.getRotationCenter();*/ }
		}

        public Point3f DefaultRotationCenter
		{
            get { return null;/* modelManager.DefaultRotationCenter; */}
		}

        public Point3f BoundBoxCenter
		{
            get { return null;/* modelManager.BoundBoxCenter;*/ }
		}

        public Vector3f BoundBoxCornerVector
		{
            get { return null;/* modelManager.BoundBoxCornerVector;*/ }
		}

        public int BoundBoxCenterX
		{
			get
			{
				// FIXME mth 2003 05 31
				// used by the labelRenderer for rendering labels away from the center
				// for now this is returning the center of the screen
				// need to transform the center of the bounding box and return that point
				return dimScreen.Width / 2;
			}
		}

        public int BoundBoxCenterY
		{
            get { return dimScreen.Height / 2; }
		}

        public override int ModelCount
		{
            get { return 0;/* modelManager.ModelCount;*/ }
		}

		public override NameValueCollection ModelSetProperties
		{
            get { return null;/* modelManager.ModelSetProperties;*/ }
		}

        public override int ChainCount
		{
            get { return 0;/* modelManager.ChainCount;*/ }
		}

        public override int GroupCount
		{
            get { return 0;/* modelManager.GroupCount;*/ }
		}

        public override int PolymerCount
		{
            get { return 0;/* modelManager.PolymerCount;*/ }
		}

        public override int AtomCount
		{
            get { return 0;/* modelManager.AtomCount;*/ }
		}

        public override int BondCount
		{
            get { return 0;/* modelManager.BondCount;*/ }
		}

        public BitArray CenterBitSet
		{
			set
			{
                //modelManager.CenterBitSet = value;
				if (windowCenteredFlag)
					scaleFitToScreen();
				refresh();
			}
		}

		public int CenterPicked
		{
			set
			{
                //setCenter(modelManager.getAtomPoint3f(value));
				
				/*
				* This method is called exclusively by PickingManager when the user
				* clicks on an atom and we have "set picking center"
				* 
				* Formerly, PickingManager went directly to Viewer.setCenter; the
				* inclusion of setCenterPicked() allows for more flexibility in future
				* development. 
				* 
				* In Bob's opinion, the above is a bug. We have two different results for
				* 
				* set picking center
				* [user clicks on an atom]
				* 
				* and 
				* 
				* center (atom expression)
				* 
				* In the clicking case, we are going through setCenter(Point3f) 
				* and disregarding any setting of the "frieda/windowCentered switch" -- 
				* whether the clicked atom jumps to the window center 
				* (set windowCentered ON) or remains in place (set windowCentered OFF).
				* 
				* In the scripted case, we are going through setCenterBitset(), which
				* considers the windowCentered state and possibly rescales.
				* 
				* Basically, as it currently stands, "set picking center" and
				* "set windowCentered OFF" (the Frieda switch) are incompatible. 
				* This should not be the case. 
				*  
				* A further undesirable programming aspect is that windowCenteredFlag
				* is being checked first in Viewer.setCenterBitSet() and then again in
				* ModelManager.setCenterBitSet(). This seems inappropriate to Bob. In
				* Bob's opinion, all checking of the windowCenteredFlag should be in one 
				* method, namely ModelManager.setCenterBitSet().   
				* 
				* The issue is somewhat complicated in that Viewer.setCenterBitSet()
				* is also called indirectly by DefineCenterAction events in the App.
				* 
				* To be correct, Bob thinks, Viewer.setCenter() should ONLY be passed
				* through by a call to Viewer.homePosition(), which implements the 
				* equivalent of a scripted "reset" in various contexts.
				* 
				* To fix the Frieda/windowCentered incompatibility issue, one would
				* disable the above line and substitute the three lines below, so that
				* all user-directed and scripted centering is going through 
				* Viewer.setCenterBitSet().
				* 
				* These notes are meant solely as a guide to development and should be
				* removed when the issues relating to them are resolved.
				* 
				*  Bob Hanson 4/06
				*  
				BitSet bsCenter = new BitSet();
				bsCenter.set(atomIndex);
				setCenterBitSet(bsCenter);
				*/
			}
		}

        public bool WindowCentered
		{
            get { return windowCenteredFlag; }
            set { windowCenteredFlag = value; }
		}

        public float BondTolerance
		{
            get { return 0;/* modelManager.bondTolerance;*/ }
			set
			{
                //modelManager.BondTolerance = value;
				refresh();
			}
		}

        public float MinBondDistance
		{
            get { return 0;/* modelManager.minBondDistance;*/ }
			set
			{
                //modelManager.MinBondDistance = value;
				refresh();
			}
		}

        public override bool AutoBond
		{
            get { return false;/* modelManager.autoBond;*/ }
			set
			{
                //modelManager.AutoBond = value;
				refresh();
			}
		}

        public float SolventProbeRadius
		{
            get { return 0;/* modelManager.solventProbeRadius;*/ }
            set { /*modelManager.SolventProbeRadius = value;*/ }
		}

        public float CurrentSolventProbeRadius
		{
            get { return 0;/*modelManager.solventOn ? modelManager.solventProbeRadius : 0;*/ }
		}

        public bool SolventOn
		{
            get { return false;/* modelManager.solventOn;*/ }
            set { /*modelManager.SolventOn = value;*/ }
		}

        public override BitArray ElementsPresentBitSet
		{
            get { return null;/* modelManager.ElementsPresentBitSet;*/ }
		}

        public override BitArray GroupsPresentBitSet
		{
            get { return null;/* modelManager.GroupsPresentBitSet;*/ }
		}

        public override int MeasurementCount
		{
			get
			{
				int count = getShapePropertyAsInt(JmolConstants.SHAPE_MEASURES, "count");
				return count <= 0?0:count;
			}
		}

        public int[] PendingMeasurement
		{
			set
			{
				setShapeProperty(JmolConstants.SHAPE_MEASURES, "pending", value);
			}
		}

        public int AnimationDirection
		{
            get { return 0;/* repaintManager.animationDirection;*/ }
			/////////////////////////////////////////////////////////////////
			// delegated to RepaintManager
			/////////////////////////////////////////////////////////////////
			set
			{
				// 1 or -1
                //repaintManager.AnimationDirection = value;
			}
		}

        public override int AnimationFps
		{
            get { return 0;/* repaintManager.animationFps;*/ }
            set { /*repaintManager.AnimationFps = value;*/ }
		}

        public FrameRenderer FrameRenderer
		{
            get { return null;/* repaintManager.frameRenderer;*/ }
		}

        public override int MotionEventNumber
		{
            get { return motionEventNumber; }
		}

        public bool InMotion
		{
            get { return false;/* repaintManager.inMotion;*/ }
			set
			{
				//System.out.println("viewer.setInMotion("+inMotion+")");
				if (wasInMotion ^ value)
				{
					if (value)
						++motionEventNumber;
                    //repaintManager.InMotion = value;
					wasInMotion = value;
				}
			}
		}

        public override Image ScreenImage
		{
			get
			{
                //bool antialiasThisFrame = true;
                //System.Drawing.Rectangle tempAux = System.Drawing.Rectangle.Empty;
                //RectClip = tempAux;
                //g3d.beginRendering(rectClip.X, rectClip.Y, rectClip.Width, rectClip.Height, transformManager.getStereoRotationMatrix(false), antialiasThisFrame);
                //repaintManager.render(g3d, rectClip, modelManager.Frame, repaintManager.displayModelIndex);
                //g3d.endRendering();
                //return g3d.ScreenImage;
                return null;
			}
		}

        //public Eval Eval
        //{
        //    /////////////////////////////////////////////////////////////////
        //    // routines for script support
        //    /////////////////////////////////////////////////////////////////
        //    get
        //    {
        //        if (eval == null)
        //            eval = new Eval(this);
        //        return eval;
        //    }
        //}

        public override bool ScriptExecuting
		{
            get { return false;/* eval.ScriptExecuting;*/ }
		}

        public bool ChainCaseSensitive
		{
            get { return chainCaseSensitive; }
            set { this.chainCaseSensitive = value; }
		}

        public bool RibbonBorder
		{
            get { return ribbonBorder; }
            set { this.ribbonBorder = value; }
		}

        public bool HideNameInPopup
		{
            get { return hideNameInPopup; }
            set { this.hideNameInPopup = value; }
		}

        public bool SsbondsBackbone
		{
            get { return false;/* styleManager.ssbondsBackbone;*/ }
            set { /*styleManager.SsbondsBackbone = value;*/ }
		}

        public bool HbondsBackbone
		{
            get { return false;/*styleManager.hbondsBackbone;*/ }
            set { /*styleManager.HbondsBackbone = value;*/ }
		}

        public bool HbondsSolid
		{
            get { return false;/* styleManager.hbondsSolid;*/ }
            set { /*styleManager.HbondsSolid = value;*/ }
		}

		public short MarBond
		{
            set
            {
                //styleManager.MarBond = value;
                setShapeSize(JmolConstants.SHAPE_STICKS, value * 2);
            }
		}

        public string Label
		{
			set
			{
                //if (value != null)
				// force the class to load and display
                    //setShapeSize(JmolConstants.SHAPE_LABELS, styleManager.pointsLabelFontSize);
				setShapeProperty(JmolConstants.SHAPE_LABELS, "label", value);
			}
		}

        public BitArray BitSetSelection
		{
            get { return null;/* selectionManager.bsSelection;*/ }
		}

        public bool RasmolHydrogenSetting
		{
            get { return rasmolHydrogenSetting; }
            set { rasmolHydrogenSetting = value; }
		}

        public bool RasmolHeteroSetting
		{
            get { return rasmolHeteroSetting; }
            set { rasmolHeteroSetting = value; }
		}

        //public JmolStatusListener JmolStatusListener
        //{
        //    set { this.jmolStatusListener = value; }
        //}

        public int PickingMode
		{
            set { /*pickingManager.PickingMode = value;*/ }
		}

		public bool TestFlag1
		{
            get { return testFlag1; }
            set { testFlag1 = value; }
		}

        public bool TestFlag2
		{
            get { return testFlag2; }
            set { testFlag2 = value; }
		}

        public bool TestFlag3
		{
            get { return testFlag3; }
            set { testFlag3 = value; }
		}

        public bool TestFlag4
		{
            get { return testFlag4; }
            set { testFlag4 = value; }
		}

        public bool GreyscaleRendering
		{
            get { return greyscaleRendering; }
			set
			{
				this.greyscaleRendering = value;
                //g3d.setGreyscaleMode(value);
				refresh();
			}
		}

        public bool DisablePopupMenu
		{
            get { return disablePopupMenu; }
            set { this.disablePopupMenu = value; }
		}

        public override int PercentVdwAtom
		{
            get { return 0;/* styleManager.percentVdwAtom;*/ }
			/*
			* for rasmol compatibility with continued menu operation:
			*  - if it is from the menu & nothing selected
			*    * set the setting
			*    * apply to all
			*  - if it is from the menu and something is selected
			*    * apply to selection
			*  - if it is from a script
			*    * apply to selection
			*    * possibly set the setting for some things
			*/
			set
			{
                //styleManager.PercentVdwAtom = value;
				setShapeSize(JmolConstants.SHAPE_BALLS, - value);
			}
		}

        public short MadAtom
		{
            get { return 0;/* (short)(-styleManager.percentVdwAtom);*/ }
		}

        public override short MadBond
		{
            get { return 0;/*(short)(styleManager.marBond * 2);*/ }
		}

        public sbyte ModeMultipleBond
		{
            get { return 0;/* styleManager.modeMultipleBond;*/ }
			set
			{
                //styleManager.ModeMultipleBond = value;
				refresh();
			}
		}

        public bool ShowMultipleBonds
		{
            get { return false;/* styleManager.showMultipleBonds;*/ }
            set
			{
                //styleManager.ShowMultipleBonds = value;
				refresh();
			}
		}

        public override bool ShowHydrogens
		{
            get { return false;/* styleManager.showHydrogens;*/ }
			set
			{
                //styleManager.ShowHydrogens = value;
				refresh();
			}
		}

        public override bool ShowBbcage
		{
            get { return getShapeShow(JmolConstants.SHAPE_BBCAGE); }
			set
			{
				setShapeShow(JmolConstants.SHAPE_BBCAGE, value);
			}
		}

        public override bool ShowAxes
		{
            get { return getShapeShow(JmolConstants.SHAPE_AXES); }
            set { setShapeShow(JmolConstants.SHAPE_AXES, value); }
		}

        public override bool ShowMeasurements
		{
            get { return false;/* styleManager.showMeasurements;*/ }
			set
			{
                //styleManager.ShowMeasurements = value;
				refresh();
			}
		}

        public bool ShowMeasurementLabels
		{
            get { return false;/* styleManager.showMeasurementLabels;*/ }
			set
			{
                //styleManager.ShowMeasurementLabels = value;
				refresh();
			}
		}

        public bool ZeroBasedXyzRasmol
		{
            get { return false;/* styleManager.zeroBasedXyzRasmol; */}
            set { /*styleManager.ZeroBasedXyzRasmol = value;*/ }
		}

        public int LabelFontSize
		{
			set
			{
                //styleManager.LabelFontSize = value;
				refresh();
			}
		}

        public int LabelOffsetX
		{
            get { return 0;/* styleManager.labelOffsetX;*/ }
		}

        public int LabelOffsetY
		{
            get { return 0;/*styleManager.labelOffsetY;*/ }
		}

        public int StereoMode
		{
            get { return 0;/* transformManager.stereoMode;*/ }
			////////////////////////////////////////////////////////////////
			// stereo support
			////////////////////////////////////////////////////////////////
            set { /*transformManager.StereoMode = value;*/ }
		}

        public float StereoDegrees
		{
            get { return 0;/* transformManager.stereoDegrees;*/ }
            set { /*transformManager.StereoDegrees = value;*/ }
		}

		public bool Jvm12orGreater
		{
            get { return jvm12orGreater; }
		}

        //public string OperatingSystemName
        //{
        //    get
        //    {
        //        return strOSName;
        //    }
        //}

        //public string JavaVendor
        //{
        //    get
        //    {
        //        return strJavaVendor;
        //    }
        //}

        //public string JavaVersion
        //{
        //    get
        //    {
        //        return strJavaVersion;
        //    }
        //}
        //internal Graphics3D Graphics3D
        //{
        //    get
        //    {
        //        return g3d;
        //    }
			
        //}

        //public System.Windows.Forms.Control awtComponent;
        //internal ColorManager colorManager;
        //internal TransformManager transformManager;
        //internal SelectionManager selectionManager;
        //internal MouseManager mouseManager;
        //internal FileManager fileManager;
        //internal ModelManager modelManager;
        //internal RepaintManager repaintManager;
        //internal StyleManager styleManager;
        //internal TempManager tempManager;
        //internal PickingManager pickingManager;
        //internal Eval eval;
        //internal Graphics3D g3d;
		
		//internal JmolAdapter modelAdapter;
		
        //internal string strJavaVendor;
        //internal string strJavaVersion;
        //internal string strOSName;
        public bool jvm11orGreater = false;
        public bool jvm12orGreater = false;
        public bool jvm14orGreater = false;

        //public JmolStatusListener jmolStatusListener;

        public Viewer(/*System.Windows.Forms.Control awtComponent, JmolAdapter modelAdapter*/)
		{
            //this.awtComponent = awtComponent;
            //this.modelAdapter = modelAdapter;
			
            //strJavaVendor = System_Renamed.getProperty("java.vendor");
            //strOSName = System.Environment.GetEnvironmentVariable("OS");
            //strJavaVersion = System_Renamed.getProperty("java.version");
            //jvm11orGreater = (String.CompareOrdinal(strJavaVersion, "1.1") >= 0 && !(strJavaVendor.StartsWith("Netscape") && String.CompareOrdinal(strJavaVersion, "1.1.5") <= 0 && "Mac OS".Equals(strOSName)));
            //jvm12orGreater = (String.CompareOrdinal(strJavaVersion, "1.2") >= 0);
            //jvm14orGreater = (String.CompareOrdinal(strJavaVersion, "1.4") >= 0);
			
            //System.Console.Out.WriteLine(JmolConstants.copyright + "\nJmol Version " + JmolConstants.version + "  " + JmolConstants.date + "\njava.vendor:" + strJavaVendor + "\njava.version:" + strJavaVersion + "\nos.name:" + strOSName);
			
            //g3d = new Graphics3D(awtComponent);
            //colorManager = new ColorManager(this, g3d);
            //transformManager = new TransformManager(this);
            //selectionManager = new SelectionManager(this);
            //if (jvm14orGreater)
            //    mouseManager = MouseWrapper14.alloc(awtComponent, this);
            //else if (jvm11orGreater)
            //    mouseManager = MouseWrapper11.alloc(awtComponent, this);
            //else
            //    mouseManager = new MouseManager10(awtComponent, this);
            //fileManager = new FileManager(this, modelAdapter);
            //repaintManager = new RepaintManager(this);
            //modelManager = new ModelManager(this, modelAdapter);
            //styleManager = new StyleManager(this);
            //tempManager = new TempManager(this);
            //pickingManager = new PickingManager(this);
		}
		
		//UPGRADE_ISSUE: Class 'java.awt.Event' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
        //public bool handleOldJvm10Event(Event e)
        //{
        //    return mouseManager.handleOldJvm10Event(e);
        //}
		
		public override void homePosition()
		{
			setCenter(null);
            //transformManager.homePosition();
			refresh();
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'imageCache '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public Hashtable imageCache = Hashtable.Synchronized(new System.Collections.Hashtable());

        public void flushCachedImages()
		{
			imageCache.Clear();
            //colorManager.flushCachedColors();
		}

        public void logError(string strMsg)
		{
			System.Console.Out.WriteLine(strMsg);
		}
		
		/////////////////////////////////////////////////////////////////
		// delegated to TransformManager
		/////////////////////////////////////////////////////////////////

        public void rotateXYBy(int xDelta, int yDelta)
		{
            //transformManager.rotateXYBy(xDelta, yDelta);
			refresh();
		}

        public void rotateZBy(int zDelta)
		{
            //transformManager.rotateZBy(zDelta);
			refresh();
		}

        public override void rotateFront()
		{
            //transformManager.rotateFront();
			refresh();
		}

        public override void rotateToX(float angleRadians)
		{
            //transformManager.rotateToX(angleRadians);
			refresh();
		}

        public override void rotateToY(float angleRadians)
		{
            //transformManager.rotateToY(angleRadians);
			refresh();
		}

        public override void rotateToZ(float angleRadians)
		{
            //transformManager.rotateToZ(angleRadians);
			refresh();
		}

        public override void rotateToX(int angleDegrees)
		{
			rotateToX(angleDegrees * radiansPerDegree);
		}

        public override void rotateToY(int angleDegrees)
		{
			rotateToY(angleDegrees * radiansPerDegree);
		}

        public void rotateToZ(int angleDegrees)
		{
			rotateToZ(angleDegrees * radiansPerDegree);
		}

        public void rotateXRadians(float angleRadians)
		{
            //transformManager.rotateXRadians(angleRadians);
			refresh();
		}

        public void rotateYRadians(float angleRadians)
		{
            //transformManager.rotateYRadians(angleRadians);
			refresh();
		}

        public void rotateZRadians(float angleRadians)
		{
            //transformManager.rotateZRadians(angleRadians);
			refresh();
		}

        public void rotateXDegrees(float angleDegrees)
		{
			rotateXRadians(angleDegrees * radiansPerDegree);
		}

        public void rotateYDegrees(float angleDegrees)
		{
			rotateYRadians(angleDegrees * radiansPerDegree);
		}

        public void rotateZDegrees(float angleDegrees)
		{
			rotateZRadians(angleDegrees * radiansPerDegree);
		}

        public void rotateZDegreesScript(float angleDegrees)
		{
            //transformManager.rotateZRadiansScript(angleDegrees * radiansPerDegree);
			refresh();
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'radiansPerDegree '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        public static readonly float radiansPerDegree = (float)(2f * System.Math.PI / 360f);
		//UPGRADE_NOTE: Final was removed from the declaration of 'degreesPerRadian '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        public static readonly float degreesPerRadian = (float)(360f / (2f * System.Math.PI));

        public void rotate(AxisAngle4f axisAngle)
		{
            //transformManager.rotate(axisAngle);
			refresh();
		}

        public void rotateAxisAngle(float x, float y, float z, float degrees)
		{
            //transformManager.rotateAxisAngle(x, y, z, degrees);
		}

        public void rotateTo(float xAxis, float yAxis, float zAxis, float degrees)
		{
            //transformManager.rotateTo(xAxis, yAxis, zAxis, degrees);
		}

        public void rotateTo(AxisAngle4f axisAngle)
		{
            //transformManager.rotateTo(axisAngle);
		}

        public void translateXYBy(int xDelta, int yDelta)
		{
            //transformManager.translateXYBy(xDelta, yDelta);
			refresh();
		}

        public void translateToXPercent(float percent)
		{
            //transformManager.translateToXPercent(percent);
			refresh();
		}

        public void translateToYPercent(float percent)
		{
            //transformManager.translateToYPercent(percent);
			refresh();
		}

        public void translateToZPercent(float percent)
		{
            //transformManager.translateToZPercent(percent);
			refresh();
		}

        public void translateByXPercent(float percent)
		{
			translateToXPercent(TranslationXPercent + percent);
		}

        public void translateByYPercent(float percent)
		{
			translateToYPercent(TranslationYPercent + percent);
		}

        public void translateByZPercent(float percent)
		{
			translateToZPercent(TranslationZPercent + percent);
		}

        public void translateCenterTo(int x, int y)
		{
            //transformManager.translateCenterTo(x, y);
		}

        public void zoomBy(int pixels)
		{
            //transformManager.zoomBy(pixels);
			refresh();
		}
		
		public const int MAXIMUM_ZOOM_PERCENTAGE = 20000;
		/*
		* OK, I give. We have a real limitation with perspective depth.
		* Zoom is back to where it was.  
		* When it is on and we go very far past this in zoom, we can see some 
		* nasty rendering issues. I believe this is because we are hitting
		* a point where z*z > int.MAX_VALUE, but I can't be sure. I believe
		* that means that the real limit for z is a short.   
		* 
		* These notes are meant solely as a guide to development and should be
		* removed when the issues relating to them are resolved.
		* 
		*  Bob Hanson 4/06
		*  
		*/

        public void zoomToPercent(int percent)
		{
            //transformManager.zoomToPercent(percent);
			refresh();
		}

        public void zoomByPercent(int percent)
		{
            //transformManager.zoomByPercent(percent);
			refresh();
		}

        public void slabByPixels(int pixels)
		{
            //transformManager.slabByPercentagePoints(pixels);
			refresh();
		}

        public void depthByPixels(int pixels)
		{
            //transformManager.depthByPercentagePoints(pixels);
			refresh();
		}

        public void slabDepthByPixels(int pixels)
		{
            //transformManager.slabDepthByPercentagePoints(pixels);
			refresh();
		}

        public void slabToPercent(int percentSlab)
		{
            //transformManager.slabToPercent(percentSlab);
			refresh();
		}

        public void depthToPercent(int percentDepth)
		{
            //transformManager.depthToPercent(percentDepth);
			refresh();
		}

        public void calcTransformMatrices()
		{
            //transformManager.calcTransformMatrices();
		}

        public Point3i transformPoint(Point3f pointAngstroms)
		{
            return null;// transformManager.transformPoint(pointAngstroms);
		}

        public Point3i transformPoint(Point3f pointAngstroms, Vector3f vibrationVector)
		{
            return null;// transformManager.transformPoint(pointAngstroms, vibrationVector);
		}

        public void transformPoint(Point3f pointAngstroms, Vector3f vibrationVector, Point3i pointScreen)
		{
            //transformManager.transformPoint(pointAngstroms, vibrationVector, pointScreen);
		}

        public void transformPoint(Point3f pointAngstroms, Point3i pointScreen)
		{
            //transformManager.transformPoint(pointAngstroms, pointScreen);
		}

        public void transformPoint(Point3f pointAngstroms, Point3f pointScreen)
		{
            //transformManager.transformPoint(pointAngstroms, pointScreen);
		}

        public void transformPoints(Point3f[] pointsAngstroms, Point3i[] pointsScreens)
		{
            //transformManager.transformPoints(pointsAngstroms.length, pointsAngstroms, pointsScreens);
		}

        public void transformVector(Vector3f vectorAngstroms, Vector3f vectorTransformed)
		{
            //transformManager.transformVector(vectorAngstroms, vectorTransformed);
		}

        public float scaleToScreen(int z, float sizeAngstroms)
		{
            return 0;// transformManager.scaleToScreen(z, sizeAngstroms);
		}

        public short scaleToScreen(int z, int milliAngstroms)
		{
            return 0;// transformManager.scaleToScreen(z, milliAngstroms);
		}

        public float scaleToPerspective(int z, float sizeAngstroms)
		{
            return 0;// transformManager.scaleToPerspective(z, sizeAngstroms);
		}

        public void scaleFitToScreen()
		{
            //transformManager.scaleFitToScreen();
		}

        public void checkCameraDistance()
		{
            //if (transformManager.increaseRotationRadius)
            //    modelManager.increaseRotationRadius(transformManager.RotationRadiusIncrease);
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'dimScreen '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public Size dimScreen = new Size(0, 0);
		//UPGRADE_NOTE: Final was removed from the declaration of 'rectClip '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public Rectangle rectClip = new Rectangle();

        public bool enableFullSceneAntialiasing = false;

        public void setSlabAndDepthValues(int slabValue, int depthValue)
		{
            //g3d.setSlabAndDepthValues(slabValue, depthValue);
		}

        public void getAxisAngle(AxisAngle4f axisAngle)
		{
            //transformManager.getAxisAngle(axisAngle);
		}

        public void setRotation(Matrix3f matrixRotation)
		{
            //transformManager.setRotation(matrixRotation);
		}

        public void getRotation(Matrix3f matrixRotation)
		{
            //transformManager.getRotation(matrixRotation);
		}

        public int getColixArgb(short colix)
		{
            return 0;// g3d.getColixArgb(colix);
		}

        public void setElementArgb(int elementNumber, int argb)
		{
            //colorManager.setElementArgb(elementNumber, argb);
		}

        public override float VectorScale
		{
            get { return 0;/* transformManager.vectorScale;*/ }
		}
		
		public void setVectorScale(float scale)
		{
            //transformManager.VectorScale = scale;
		}

        public override float VibrationScale
		{
            set { /*transformManager.VibrationScale = scale;*/ }
            get { return 0;/* transformManager.vibrationScale;*/ }
		}

        public int getArgbFromString(string colorName)
		{
            return 0;// Graphics3D.getArgbFromString(colorName);
		}

        public short getColixAtom(Atom atom)
		{
            return 0;// colorManager.getColixAtom(atom);
		}

        public short getColixAtomPalette(Atom atom, string palette)
		{
            return 0;// colorManager.getColixAtomPalette(atom, palette);
		}

        public short getColixHbondType(short order)
		{
            return 0;// colorManager.getColixHbondType(order);
		}

        public short getColixFromPalette(float val, float rangeMin, float rangeMax, string palette)
		{
            return 0;// colorManager.getColixFromPalette(val, rangeMin, rangeMax, palette);
		}
		
		/////////////////////////////////////////////////////////////////
		// delegated to SelectionManager
		/////////////////////////////////////////////////////////////////

        public void addSelection(int atomIndex)
		{
            //selectionManager.addSelection(atomIndex);
			refresh();
		}

        public void addSelection(System.Collections.BitArray set_Renamed)
		{
            //selectionManager.addSelection(set_Renamed);
			refresh();
		}

        public void toggleSelection(int atomIndex)
		{
            //selectionManager.toggleSelection(atomIndex);
			refresh();
		}

        public bool isSelected(int atomIndex)
		{
            return false;// selectionManager.isSelected(atomIndex);
		}

        public bool hasSelectionHalo(int atomIndex)
		{
            return false;// selectionHaloEnabled && selectionManager.isSelected(atomIndex);
		}

        public bool selectionHaloEnabled = false;
		
        public override bool SelectionHaloEnabled
		{
            set
            {
                //if (this.selectionHaloEnabled != selectionHaloEnabled)
                //{
                //    this.selectionHaloEnabled = selectionHaloEnabled;
                //    refresh();
                //}
            }
            get { return false;/* selectionHaloEnabled;*/ }
		}
		
		private bool bondSelectionModeOr;

        public override void selectAll()
		{
            //selectionManager.selectAll();
			refresh();
		}

        public override void clearSelection()
		{
            //selectionManager.clearSelection();
			refresh();
		}

        public void toggleSelectionSet(System.Collections.BitArray set_Renamed)
		{
            //selectionManager.toggleSelectionSet(set_Renamed);
			refresh();
		}

        public void invertSelection()
		{
            //selectionManager.invertSelection();
			// only used from a script, so I do not think a refresh() is necessary
		}

        public void excludeSelectionSet(BitArray set_Renamed)
		{
            //selectionManager.excludeSelectionSet(set_Renamed);
			// only used from a script, so I do not think a refresh() is necessary
		}
		
        //public void addSelectionListener(JmolSelectionListener listener)
        //{
        //    selectionManager.addListener(listener);
        //}
		
        //public void  removeSelectionListener(JmolSelectionListener listener)
        //{
        //    selectionManager.addListener(listener);
        //}

        public void popupMenu(int x, int y)
		{
            //if (!disablePopupMenu && jmolStatusListener != null)
            //    jmolStatusListener.handlePopupMenu(x, y);
		}
		
		/////////////////////////////////////////////////////////////////
		// delegated to FileManager
		/////////////////////////////////////////////////////////////////
		
        //public void setAppletContext(System.Uri documentBase, System.Uri codeBase, string appletProxy)
        //{
        //    fileManager.setAppletContext(documentBase, codeBase, appletProxy);
        //}

        public object getInputStreamOrErrorMessageFromName(string name)
		{
            return null;// fileManager.getInputStreamOrErrorMessageFromName(name);
		}

        public object getUnzippedBufferedReaderOrErrorMessageFromName(string name)
		{
            return null;// fileManager.getUnzippedBufferedReaderOrErrorMessageFromName(name);
		}
		
		public override void openFile(string name)
		{
			/*
			System.out.println("openFile(" + name + ") thread:" + Thread.currentThread() +
			" priority:" + Thread.currentThread().getPriority());
			*/
			clear();
			// keep old screen image while new file is being loaded
			//    forceRefresh();
            //long timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
            //fileManager.openFile(name);
            //long ms = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin;
            //System.Console.Out.WriteLine("openFile(" + name + ") " + ms + " ms");
		}
		
		public void openFiles(string modelName, string[] names)
		{
			clear();
			// keep old screen image while new file is being loaded
			//    forceRefresh();
            //long timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
            //fileManager.openFiles(modelName, names);
            //long ms = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin;
            //System.Console.Out.WriteLine("openFiles() " + ms + " ms");
		}

        public override void openstringInline(string strModel)
        {
            clear();
            //fileManager.openStringInline(strModel);
            //string generatedAux = OpenFileError;
        }
	
		public override void openDOM(object DOMNode)
		{
			clear();
            //long timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
            //fileManager.openDOM(DOMNode);
            //long ms = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin;
            //System.Console.Out.WriteLine("openDOM " + ms + " ms");
            //string generatedAux4 = OpenFileError;
		}
		
		/// <summary> Opens the file, given the reader.
		/// 
		/// name is a text name of the file ... to be displayed in the window
		/// no need to pass a BufferedReader ...
		/// ... the FileManager will wrap a buffer around it
		/// </summary>
		/// <param name="fullPathName">
		/// </param>
		/// <param name="name">
		/// </param>
		/// <param name="reader">
		/// </param>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public void openReader(string fullPathName, string name, System.IO.StreamReader reader)
		{
			clear();
            //fileManager.openReader(fullPathName, name, reader);
            //string generatedAux = OpenFileError;
            //System.GC.Collect();
		}

        public string getFileAsString(string pathName)
		{
            return null;// fileManager.getFileAsString(pathName);
		}
		
		/////////////////////////////////////////////////////////////////
		// delegated to ModelManager
		/////////////////////////////////////////////////////////////////
		
		public void openClientFile(string fullPathName, string fileName, object clientFile)
		{
			// maybe there needs to be a call to clear()
			// or something like that here
			// for when CdkEditBus calls this directly
            //pushHoldRepaint();
            //modelManager.setClientFile(fullPathName, fileName, clientFile);
            //homePosition();
            //selectAll();
            //if (eval != null)
            //    eval.clearDefinitionsAndLoadPredefined();
            //// there probably needs to be a better startup mechanism for shapes
            //if (modelManager.hasVibrationVectors())
            //    setShapeSize(JmolConstants.SHAPE_VECTORS, 1);
            //setFrankOn(styleManager.frankOn);
			
            //popHoldRepaint();
		}

        public void clear()
		{
            //repaintManager.clearAnimation();
            //transformManager.clearVibration();
            //modelManager.setClientFile(null, null, (object) null);
            //selectionManager.clearSelection();
			clearMeasurements();
			WindowCentered = true;
			notifyFileLoaded(null, null, null, (object) null);
			refresh();
		}


        public override bool haveFrame()
		{
            return false;// modelManager.frame != null;
		}

        public string getClientAtomStringProperty(object clientAtomReference, string propertyName)
		{
            return null;// modelManager.getClientAtomStringProperty(clientAtomReference, propertyName);
		}

        public override int getModelNumber(int modelIndex)
		{
            return 0;// modelManager.getModelNumber(modelIndex);
		}

        public override string getModelName(int modelIndex)
		{
            return null;// modelManager.getModelName(modelIndex);
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public override NameValueCollection getModelProperties(int modelIndex)
		{
            return null;// modelManager.getModelProperties(modelIndex);
		}

        public override string getModelProperty(int modelIndex, string propertyName)
		{
            return null;// modelManager.getModelProperty(modelIndex, propertyName);
		}

        public int getModelNumberIndex(int modelNumber)
		{
            return 0;// modelManager.getModelNumberIndex(modelNumber);
		}

        public bool modelSetHasVibrationVectors()
		{
            return false;// modelManager.modelSetHasVibrationVectors();
		}

        public override bool modelHasVibrationVectors(int modelIndex)
		{
            return false;// modelManager.modelHasVibrationVectors(modelIndex);
		}

        public override int getPolymerCountInModel(int modelIndex)
		{
            return 0;// modelManager.getPolymerCountInModel(modelIndex);
		}

        public bool frankClicked(int x, int y)
		{
            return false;// modelManager.frankClicked(x, y);
		}

        public int findNearestAtomIndex(int x, int y)
		{
            return 0;// modelManager.findNearestAtomIndex(x, y);
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		internal BitArray findAtomsInRectangle(ref Rectangle rectRubberBand)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
            return null;// modelManager.findAtomsInRectangle(ref rectRubberBand);
		}

        public void setCenter(Point3f center)
		{
            //modelManager.RotationCenter = center;
			refresh();
		}

        public Point3f getCenter()
		{
            return null;// modelManager.getRotationCenter();
		}

        public void setCenter(string relativeTo, float x, float y, float z)
		{
            //modelManager.setRotationCenter(relativeTo, x, y, z);
			scaleFitToScreen();
		}

        public override void setCenterSelected()
		{
            //CenterBitSet = selectionManager.bsSelection;
		}

        public bool windowCenteredFlag = true;

        public int getAtomIndexFromAtomNumber(int atomNumber)
		{
            return 0;// modelManager.getAtomIndexFromAtomNumber(atomNumber);
		}

        public void calcSelectedGroupsCount()
		{
            //modelManager.calcSelectedGroupsCount(selectionManager.bsSelection);
		}

        public void calcSelectedMonomersCount()
		{
            //modelManager.calcSelectedMonomersCount(selectionManager.bsSelection);
		}
		
		/// <summary>*************************************************************
		/// delegated to MeasurementManager
		/// **************************************************************
		/// </summary>

        public override void clearMeasurements()
		{
			setShapeProperty(JmolConstants.SHAPE_MEASURES, "clear", (object) null);
			refresh();
		}

        public override string getMeasurementStringValue(int i)
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			return "" + getShapeProperty(JmolConstants.SHAPE_MEASURES, "stringValue", i);
		}

        public override int[] getMeasurementCountPlusIndices(int i)
		{
			return (int[]) getShapeProperty(JmolConstants.SHAPE_MEASURES, "countPlusIndices", i);
		}

        public void defineMeasurement(int[] atomCountPlusIndices)
		{
			setShapeProperty(JmolConstants.SHAPE_MEASURES, "define", atomCountPlusIndices);
		}

        public override void deleteMeasurement(int i)
		{
			setShapeProperty(JmolConstants.SHAPE_MEASURES, "delete", (object) i);
		}

        public void deleteMeasurement(int[] atomCountPlusIndices)
		{
			setShapeProperty(JmolConstants.SHAPE_MEASURES, "delete", atomCountPlusIndices);
		}

        public void toggleMeasurement(int[] atomCountPlusIndices)
		{
			setShapeProperty(JmolConstants.SHAPE_MEASURES, "toggle", atomCountPlusIndices);
		}

        public void clearAllMeasurements()
		{
			setShapeProperty(JmolConstants.SHAPE_MEASURES, "clear", (object) null);
		}

        public void setAnimationReplayMode(int replay, float firstFrameDelay, float lastFrameDelay)
		{
			// 0 means once
			// 1 means loop
			// 2 means palindrome
            //repaintManager.setAnimationReplayMode(replay, firstFrameDelay, lastFrameDelay);
		}

        public int getAnimationReplayMode()
		{
            return 0;// repaintManager.animationReplayMode;
		}

        public void setAnimationOn(bool animationOn)
		{
            //bool wasAnimating = repaintManager.animationOn;
            //repaintManager.setAnimationOn(animationOn);
            //if (animationOn != wasAnimating)
            //    refresh();
		}

        public void setAnimationOn(bool animationOn, int framePointer)
		{
            //bool wasAnimating = repaintManager.animationOn;
            //System.Console.Out.WriteLine(" setAnimationOn " + wasAnimating + " " + animationOn + " " + framePointer);
            //repaintManager.setAnimationOn(animationOn, framePointer);
            //if (animationOn != wasAnimating)
            //    refresh();
		}

        public bool isAnimationOn()
		{
            return false;// repaintManager.animationOn;
		}

        public void setAnimationNext()
		{
            //if (repaintManager.setAnimationNext())
            //    refresh();
		}

        public void setAnimationPrevious()
		{
            //if (repaintManager.setAnimationPrevious())
            //    refresh();
		}

        public bool setDisplayModelIndex(int modelIndex)
		{
            return false;// repaintManager.setDisplayModelIndex(modelIndex);
		}
		
		public override int DisplayModelIndex
		{
            get { return 0;/* repaintManager.displayModelIndex;*/ }
		}

        public int motionEventNumber;

        public bool wasInMotion = false;

        public System.Drawing.Image takeSnapshot()
		{
            return null;// repaintManager.takeSnapshot();
		}

        public override void pushHoldRepaint()
		{
            //repaintManager.pushHoldRepaint();
		}

        public override void popHoldRepaint()
		{
            //repaintManager.popHoldRepaint();
		}
		
		internal void forceRefresh()
		{
            //repaintManager.forceRefresh();
		}

        public override void refresh()
		{
            //repaintManager.refresh();
		}

        public void requestRepaintAndWait()
		{
            //repaintManager.requestRepaintAndWait();
		}

        public override void notifyRepainted()
		{
            //repaintManager.notifyRepainted();
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
        public override void renderScreenImage(Graphics g, ref Size size, ref Rectangle clip)
		{
			manageScriptTermination();
			if (!size.IsEmpty)
				ScreenDimension = size;
			int stereoMode = StereoMode;
			switch (stereoMode)
			{
				
				case JmolConstants.STEREO_NONE: 
					RectClip = clip;
                    //render1(g, transformManager.getStereoRotationMatrix(false), false, 0, 0);
					break;
				
				case JmolConstants.STEREO_DOUBLE: 
					System.Drawing.Rectangle tempAux = System.Drawing.Rectangle.Empty;
					RectClip = tempAux;
                    //render1(g, transformManager.getStereoRotationMatrix(false), false, 0, 0);
                    //render1(g, transformManager.getStereoRotationMatrix(true), false, dimScreen.Width, 0);
					break;
				
				case JmolConstants.STEREO_REDCYAN: 
				case JmolConstants.STEREO_REDBLUE: 
				case JmolConstants.STEREO_REDGREEN: 
                    //System.Drawing.Rectangle tempAux2 = System.Drawing.Rectangle.Empty;
                    //RectClip = tempAux2;
                    //g3d.beginRendering(rectClip.X, rectClip.Y, rectClip.Width, rectClip.Height, transformManager.getStereoRotationMatrix(true), false);
                    //repaintManager.render(g3d, rectClip, modelManager.Frame, repaintManager.displayModelIndex);
                    //g3d.endRendering();
                    //g3d.snapshotAnaglyphChannelBytes();
                    //g3d.beginRendering(rectClip.X, rectClip.Y, rectClip.Width, rectClip.Height, transformManager.getStereoRotationMatrix(false), false);
                    //repaintManager.render(g3d, rectClip, modelManager.Frame, repaintManager.displayModelIndex);
                    //g3d.endRendering();
                    //if (stereoMode == JmolConstants.STEREO_REDCYAN)
                    //    g3d.applyCyanAnaglyph();
                    //else
                    //    g3d.applyBlueOrGreenAnaglyph(stereoMode == JmolConstants.STEREO_REDBLUE);
                    //System.Drawing.Image img = g3d.ScreenImage;
                    //try
                    //{
                    //    //UPGRADE_WARNING: Method 'java.awt.Graphics.drawImage' was converted to 'System.Drawing.Graphics.drawImage' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
                    //    g.DrawImage(img, 0, 0);
                    //}
                    //catch (System.NullReferenceException npe)
                    //{
                    //    System.Console.Out.WriteLine("Sun!! ... fix graphics your bugs!");
                    //}
                    //g3d.releaseScreenImage();
					break;
				}
			notifyRepainted();
		}

        public void render1(Graphics g, Matrix3f matrixRotate, bool antialias, int x, int y)
		{
            //g3d.beginRendering(rectClip.X, rectClip.Y, rectClip.Width, rectClip.Height, matrixRotate, antialias);
            //repaintManager.render(g3d, rectClip, modelManager.Frame, repaintManager.displayModelIndex);
            //// mth 2003-01-09 Linux Sun JVM 1.4.2_02
            //// Sun is throwing a NullPointerExceptions inside graphics routines
            //// while the window is resized. 
            //g3d.endRendering();
            //System.Drawing.Image img = g3d.ScreenImage;
            //try
            //{
            //    //UPGRADE_WARNING: Method 'java.awt.Graphics.drawImage' was converted to 'System.Drawing.Graphics.drawImage' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
            //    g.DrawImage(img, x, y);
            //}
            //catch (System.NullReferenceException npe)
            //{
            //    System.Console.Out.WriteLine("Sun!! ... fix graphics your bugs!");
            //}
            //g3d.releaseScreenImage();
		}
		
		public override void releaseScreenImage()
		{
            //g3d.releaseScreenImage();
		}

        public override string evalFile(string strFilename)
		{
            //if (strFilename != null)
            //{
            //    if (!Eval.loadScriptFile(strFilename, false))
            //        return eval.ErrorMessage;
            //    eval.start();
            //}
			return null;
		}

        public override string evalstring(string strScript)
        {
            //if (strScript != null)
            //{
            //    if (!Eval.loadScriptString(strScript, false))
            //        return eval.ErrorMessage;
            //    eval.start();
            //}
            return null;
        }

        public override string evalStringQuiet(string strScript)
		{
            //if (strScript != null)
            //{
            //    if (!Eval.loadScriptString(strScript, true))
            //        return eval.ErrorMessage;
            //    eval.start();
            //}
			return null;
		}
		
		public override void haltScriptExecution()
		{
            //if (eval != null)
            //    eval.haltExecution();
		}

        public bool chainCaseSensitive = false;

        public bool ribbonBorder = false;

        public bool hideNameInPopup = false;

        public int hoverAtomIndex = -1;
        public void hoverOn(int atomIndex)
		{
            //if ((eval == null || !eval.Active) && atomIndex != hoverAtomIndex)
            //{
            //    loadShape(JmolConstants.SHAPE_HOVER);
            //    setShapeProperty(JmolConstants.SHAPE_HOVER, "target", (object) atomIndex);
            //    hoverAtomIndex = atomIndex;
            //}
		}

        public void hoverOff()
		{
			if (hoverAtomIndex >= 0)
			{
				setShapeProperty(JmolConstants.SHAPE_HOVER, "target", (object) null);
				hoverAtomIndex = - 1;
			}
		}

        public void togglePickingLabel(int atomIndex)
		{
            //if (atomIndex != - 1)
            //{
            //    // hack to force it to load
            //    setShapeSize(JmolConstants.SHAPE_LABELS, styleManager.pointsLabelFontSize);
            //    modelManager.setShapeProperty(JmolConstants.SHAPE_LABELS, "pickingLabel", (object) atomIndex, null);
            //    refresh();
            //}
		}

        public void setShapeShow(int shapeID, bool show)
		{
			setShapeSize(shapeID, show?- 1:0);
		}

        public bool getShapeShow(int shapeID)
		{
			return getShapeSize(shapeID) != 0;
		}

        public void loadShape(int shapeID)
		{
            //modelManager.loadShape(shapeID);
		}

        public void setShapeSize(int shapeID, int size)
		{
            //modelManager.setShapeSize(shapeID, size, selectionManager.bsSelection);
			refresh();
		}

        public int getShapeSize(int shapeID)
		{
            return 0;// modelManager.getShapeSize(shapeID);
		}

        public void setShapeProperty(int shapeID, string propertyName, object value_Renamed)
		{
			
			/*
			System.out.println("JmolViewer.setShapeProperty("+
			JmolConstants.shapeClassBases[shapeID]+
			"," + propertyName + "," + value + ")");
			*/
            //modelManager.setShapeProperty(shapeID, propertyName, value_Renamed, selectionManager.bsSelection);
			refresh();
		}

        public void setShapeProperty(int shapeID, string propertyName, int value_Renamed)
		{
			setShapeProperty(shapeID, propertyName, (object) value_Renamed);
		}

        public void setShapePropertyArgb(int shapeID, string propertyName, int argb)
		{
			//UPGRADE_TODO: The 'System.Int32' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
            //setShapeProperty(shapeID, propertyName, (object) (argb == 0?null:(System.Int32) (argb | unchecked((int) 0xFF000000))));
		}

        public void setShapeColorProperty(int shapeType, int argb)
		{
			setShapePropertyArgb(shapeType, "color", argb);
		}

        public object getShapeProperty(int shapeType, string propertyName)
		{
            return null;// modelManager.getShapeProperty(shapeType, propertyName, System.Int32.MinValue);
		}

        public object getShapeProperty(int shapeType, string propertyName, int index)
		{
            return null;// modelManager.getShapeProperty(shapeType, propertyName, index);
		}

        public int getShapePropertyAsInt(int shapeID, string propertyName)
		{
			object value_Renamed = getShapeProperty(shapeID, propertyName);
			return value_Renamed == null || !(value_Renamed is System.Int32)?System.Int32.MinValue:((System.Int32) value_Renamed);
		}

        public int getShapeID(string shapeName)
		{
			for (int i = JmolConstants.SHAPE_MAX; --i >= 0; )
				if (JmolConstants.shapeClassBases[i].Equals(shapeName))
					return i;
			string msg = "Unrecognized shape name:" + shapeName;
			System.Console.Out.WriteLine(msg);
			throw new System.NullReferenceException(msg);
		}

        public short getColix(object object_Renamed)
		{
            return 0;// Graphics3D.getColix(object_Renamed);
		}

        public bool rasmolHydrogenSetting = true;

        public bool rasmolHeteroSetting = true;

        public void notifyFrameChanged(int frameNo)
		{
            //if (jmolStatusListener != null)
            //    jmolStatusListener.notifyFrameChanged(frameNo);
		}

        public void notifyFileLoaded(string fullPathName, string fileName, string modelName, object clientFile)
		{
            //if (jmolStatusListener != null)
            //    jmolStatusListener.notifyFileLoaded(fullPathName, fileName, modelName, clientFile, null);
		}

        public void notifyFileNotLoaded(string fullPathName, string errorMsg)
		{
        //    if (jmolStatusListener != null)
        //        jmolStatusListener.notifyFileLoaded(fullPathName, null, null, null, errorMsg);
        }
		
		private void manageScriptTermination()
		{
            //if (eval != null && eval.hasTerminationNotification())
            //{
            //    string strErrorMessage = eval.ErrorMessage;
            //    int msWalltime = eval.ExecutionWalltime;
            //    eval.resetTerminationNotification();
            //    if (jmolStatusListener != null)
            //        jmolStatusListener.notifyScriptTermination(strErrorMessage, msWalltime);
            //}
		}

        public void scriptEcho(string strEcho)
		{
            //if (jmolStatusListener != null)
            //    jmolStatusListener.scriptEcho(strEcho);
		}

        public bool debugScript = false;
        public bool getDebugScript()
		{
			return debugScript;
		}

        public override void setDebugScript(bool debugScript)
		{
			this.debugScript = debugScript;
		}

        public void scriptStatus(string strStatus)
		{
            //if (jmolStatusListener != null)
            //    jmolStatusListener.scriptStatus(strStatus);
		}
		
		/*
		void measureSelection(int iatom) {
		if (jmolStatusListener != null)
		jmolStatusListener.measureSelection(iatom);
		}
		*/

        public void notifyMeasurementsChanged()
		{
            //if (jmolStatusListener != null)
            //    jmolStatusListener.notifyMeasurementsChanged();
		}

        public void atomPicked(int atomIndex, bool shiftKey)
		{
            //pickingManager.atomPicked(atomIndex, shiftKey);
		}

        public void clearClickCount()
		{
            //mouseManager.clearClickCount();
		}

        public void notifyAtomPicked(int atomIndex)
		{
            //if (atomIndex != - 1 && jmolStatusListener != null)
            //    jmolStatusListener.notifyAtomPicked(atomIndex, modelManager.getAtomInfo(atomIndex));
		}
		
		public void showUrl(string urlString)
		{
            //if (jmolStatusListener != null)
            //    jmolStatusListener.showUrl(urlString);
		}
		
		public void showConsole(bool showConsole)
		{
            //if (jmolStatusListener != null)
            //    jmolStatusListener.showConsole(showConsole);
		}

        public string getAtomInfo(int atomIndex)
		{
            return null;// modelManager.getAtomInfo(atomIndex);
		}
		
		/// <summary>*************************************************************
		/// mth 2003 05 31 - needs more work
		/// this should be implemented using properties
		/// or as a hashtable using boxed/wrapped values so that the
		/// values could be shared
		/// </summary>
		/// <param name="key">
		/// </param>
		/// <returns> the boolean property
		/// mth 2005 06 24
		/// and/or these property names should be interned strings
		/// so that we can just do == comparisions between strings
		/// **************************************************************
		/// </returns>

        public override bool getBooleanProperty(string key)
		{
			if (key.ToUpper().Equals("perspectiveDepth".ToUpper()))
				return PerspectiveDepth;
			if (key.ToUpper().Equals("showAxes".ToUpper()))
				return getShapeShow(JmolConstants.SHAPE_AXES);
			if (key.ToUpper().Equals("showBoundBox".ToUpper()))
				return getShapeShow(JmolConstants.SHAPE_BBCAGE);
			if (key.ToUpper().Equals("showUnitcell".ToUpper()))
				return getShapeShow(JmolConstants.SHAPE_UCCAGE);
			if (key.ToUpper().Equals("showHydrogens".ToUpper()))
				return ShowHydrogens;
			if (key.ToUpper().Equals("showMeasurements".ToUpper()))
				return ShowMeasurements;
            if (key.ToUpper().Equals("showSelections".ToUpper()))
                return SelectionHaloEnabled;
			if (key.ToUpper().Equals("axesOrientationRasmol".ToUpper()))
				return AxesOrientationRasmol;
			if (key.ToUpper().Equals("windowCentered".ToUpper()))
				return WindowCentered;
			if (key.ToUpper().Equals("zeroBasedXyzRasmol".ToUpper()))
				return ZeroBasedXyzRasmol;
			if (key.ToUpper().Equals("testFlag1".ToUpper()))
				return TestFlag1;
			if (key.ToUpper().Equals("testFlag2".ToUpper()))
				return TestFlag2;
			if (key.ToUpper().Equals("testFlag3".ToUpper()))
				return TestFlag3;
			if (key.ToUpper().Equals("testFlag4".ToUpper()))
				return TestFlag4;
			if (key.ToUpper().Equals("chainCaseSensitive".ToUpper()))
				return ChainCaseSensitive;
			if (key.ToUpper().Equals("hideNameInPopup".ToUpper()))
				return HideNameInPopup;
			if (key.ToUpper().Equals("autobond".ToUpper()))
				return AutoBond;
			if (key.ToUpper().Equals("greyscaleRendering".ToUpper()))
				return GreyscaleRendering;
			if (key.ToUpper().Equals("disablePopupMenu".ToUpper()))
				return DisablePopupMenu;
			System.Console.Out.WriteLine("viewer.getBooleanProperty(" + key + ") - unrecognized");
			return false;
		}

        public override void setBooleanProperty(string key, bool value_Renamed)
		{
			refresh();
			if (key.ToUpper().Equals("perspectiveDepth".ToUpper()))
			{
				PerspectiveDepth = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("showAxes".ToUpper()))
			{
				setShapeShow(JmolConstants.SHAPE_AXES, value_Renamed); return ;
			}
			if (key.ToUpper().Equals("showBoundBox".ToUpper()))
			{
				setShapeShow(JmolConstants.SHAPE_BBCAGE, value_Renamed); return ;
			}
			if (key.ToUpper().Equals("showUnitcell".ToUpper()))
			{
				setShapeShow(JmolConstants.SHAPE_UCCAGE, value_Renamed); return ;
			}
			if (key.ToUpper().Equals("showHydrogens".ToUpper()))
			{
				ShowHydrogens = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("showHydrogens".ToUpper()))
			{
				ShowHydrogens = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("showMeasurements".ToUpper()))
			{
				ShowMeasurements = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("showSelections".ToUpper()))
			{
				SelectionHaloEnabled = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("axesOrientationRasmol".ToUpper()))
			{
				AxesOrientationRasmol = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("windowCentered".ToUpper()))
			{
				WindowCentered = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("zeroBasedXyzRasmol".ToUpper()))
			{
				ZeroBasedXyzRasmol = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("frieda".ToUpper()))
			//deprecated
			{
				WindowCentered = !value_Renamed; return ;
			}
			if (key.ToUpper().Equals("testFlag1".ToUpper()))
			{
				TestFlag1 = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("testFlag2".ToUpper()))
			{
				TestFlag2 = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("testFlag3".ToUpper()))
			{
				TestFlag3 = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("testFlag4".ToUpper()))
			{
				TestFlag4 = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("chainCaseSensitive".ToUpper()))
			{
				ChainCaseSensitive = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("ribbonBorder".ToUpper()))
			{
				RibbonBorder = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("hideNameInPopup".ToUpper()))
			{
				HideNameInPopup = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("autobond".ToUpper()))
			{
				AutoBond = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("greyscaleRendering".ToUpper()))
			{
				GreyscaleRendering = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("disablePopupMenu".ToUpper()))
			{
				DisablePopupMenu = value_Renamed; return ;
			}
			System.Console.Out.WriteLine("viewer.setBooleanProperty(" + key + "," + value_Renamed + ") - unrecognized");
		}

        public bool testFlag1;
        public bool testFlag2;
        public bool testFlag3;
        public bool testFlag4;
		
		/// <summary>*************************************************************
		/// Graphics3D
		/// **************************************************************
		/// </summary>

        public bool greyscaleRendering;

        public bool disablePopupMenu;
		
		/////////////////////////////////////////////////////////////////
		// Frame
		/////////////////////////////////////////////////////////////////
		/*
		private BondIterator bondIteratorSelected(byte bondType) {
		return
		getFrame().getBondIterator(bondType, selectionManager.bsSelection);
		}
		*/
		//UPGRADE_NOTE: Final was removed from the declaration of 'nullAtomIterator '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public AtomIterator nullAtomIterator = new NullAtomIterator();

        public class NullAtomIterator : AtomIterator
		{
			public bool hasNext()
			{
				return false;
			}
			public Atom next()
			{
				return null;
			}
			public void  release()
			{
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'nullBondIterator '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public BondIterator nullBondIterator = new NullBondIterator();

        public class NullBondIterator : BondIterator
		{
			public bool hasNext()
			{
				return false;
			}
			public int nextIndex()
			{
				return - 1;
			}
			public Bond next()
			{
				return null;
			}
		}
		
		/////////////////////////////////////////////////////////////////
		// delegated to StyleManager
		/////////////////////////////////////////////////////////////////

        public override void setFrankOn(bool frankOn)
		{
            //styleManager.FrankOn = frankOn;
			setShapeSize(JmolConstants.SHAPE_FRANK, frankOn?- 1:0);
		}

        public bool getFrankOn()
		{
            return false;// styleManager.frankOn;
		}
		
		/*
		short getMeasurementMad() {
		return styleManager.measurementMad;
		}
		*/

        public bool setMeasureDistanceUnits(string units)
		{
            //if (!styleManager.setMeasureDistanceUnits(units))
            //    return false;
			setShapeProperty(JmolConstants.SHAPE_MEASURES, "reformatDistances", (object) null);
			return true;
		}

        public string getMeasureDistanceUnits()
		{
            return null;// styleManager.measureDistanceUnits;
		}

        public override void setJmolDefaults()
		{
            //styleManager.setJmolDefaults();
		}

        public override void setRasmolDefaults()
		{
            //styleManager.setRasmolDefaults();
		}
		
		internal void setLabelOffset(int xOffset, int yOffset)
		{
            //styleManager.setLabelOffset(xOffset, yOffset);
			refresh();
		}
		
		////////////////////////////////////////////////////////////////
		// temp manager
		////////////////////////////////////////////////////////////////

        public Point3f[] allocTempPoints(int size)
		{
            return null;// tempManager.allocTempPoints(size);
		}

        public void freeTempPoints(Point3f[] tempPoints)
		{
            //tempManager.freeTempPoints(tempPoints);
		}

        public Point3i[] allocTempScreens(int size)
		{
            return null;// tempManager.allocTempScreens(size);
		}

        public void freeTempScreens(Point3i[] tempScreens)
		{
            //tempManager.freeTempScreens(tempScreens);
		}

        public bool[] allocTempBooleans(int size)
		{
            return null;// tempManager.allocTempBooleans(size);
		}

        public void freeTempBooleans(bool[] tempBooleans)
		{
            //tempManager.freeTempBooleans(tempBooleans);
		}
		
		////////////////////////////////////////////////////////////////
		// font stuff
		////////////////////////////////////////////////////////////////
        //public Font3D getFont3D(int fontSize)
        //{
        //    return g3d.getFont3D(JmolConstants.DEFAULT_FONTFACE, JmolConstants.DEFAULT_FONTSTYLE, fontSize);
        //}

        //public Font3D getFont3D(string fontFace, string fontStyle, int fontSize)
        //{
        //    return g3d.getFont3D(fontFace, fontStyle, fontSize);
        //}
		
		////////////////////////////////////////////////////////////////
		// Access to atom properties for clients
		////////////////////////////////////////////////////////////////

        public string getElementSymbol(int i)
		{
            return null;// modelManager.getElementSymbol(i);
		}

        public int getElementNumber(int i)
		{
            return 0;/// modelManager.getElementNumber(i);
		}

        public override string getAtomName(int i)
		{
            return null;// modelManager.getAtomName(i);
		}

        public override int getAtomNumber(int i)
		{
            return 0;// modelManager.getAtomNumber(i);
		}

        public float getAtomX(int i)
		{
            return 0;// modelManager.getAtomX(i);
		}

        public float getAtomY(int i)
		{
            return 0;// modelManager.getAtomY(i);
		}

        public float getAtomZ(int i)
		{
            return 0;// modelManager.getAtomZ(i);
		}

        public override Point3f getAtomPoint3f(int i)
		{
            return null;// modelManager.getAtomPoint3f(i);
		}

        public override float getAtomRadius(int i)
		{
            return 0;// modelManager.getAtomRadius(i);
		}

        public override int getAtomArgb(int i)
		{
            return 0;// g3d.getColixArgb(modelManager.getAtomColix(i));
		}

        public string getAtomChain(int i)
		{
            return null;// modelManager.getAtomChain(i);
		}

        public override int getAtomModelIndex(int i)
		{
            return 0;// modelManager.getAtomModelIndex(i);
		}

        public string getAtomSequenceCode(int i)
		{
            return null;// modelManager.getAtomSequenceCode(i);
		}

        public override Point3f getBondPoint3f1(int i)
		{
            return null;// modelManager.getBondPoint3f1(i);
		}

        public override Point3f getBondPoint3f2(int i)
		{
            return null;// modelManager.getBondPoint3f2(i);
		}

        public override float getBondRadius(int i)
		{
            return 0;// modelManager.getBondRadius(i);
		}
		
		public override short getBondOrder(int i)
		{
            return 0;// modelManager.getBondOrder(i);
		}

        public override int getBondArgb1(int i)
		{
            return 0;// g3d.getColixArgb(modelManager.getBondColix1(i));
		}

        public override int getBondModelIndex(int i)
		{
            return 0;// modelManager.getBondModelIndex(i);
		}

        public override int getBondArgb2(int i)
		{
            return 0;// g3d.getColixArgb(modelManager.getBondColix2(i));
		}
		
		public override Point3f[] getPolymerLeadMidPoints(int modelIndex, int polymerIndex)
		{
            return null;// modelManager.getPolymerLeadMidPoints(modelIndex, polymerIndex);
		}

        public override bool showModelSetDownload()
		{
			return true;
		}
		
		internal string formatDecimal(float value_Renamed, int decimalDigits)
		{
            return null;// styleManager.formatDecimal(value_Renamed, decimalDigits);
		}
	}
}