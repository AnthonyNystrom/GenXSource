using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenImageWorks.Undo
{

    public class BoxFilterProp
    {
        public int BoxDepth = 30;
        public int Angle = 0;

        public Color BoxStartColor = Color.Empty;
        public Color BoxEndColor = Color.Empty;

        public bool DrawImageOnSides = false;

        public BoxFilterProp(int boxDepth, int angle, Color startColor, Color endColor,bool drawImageOnSides)
        {
            this.BoxDepth = boxDepth;
            this.Angle = angle;
            this.BoxStartColor = startColor;
            this.BoxEndColor = endColor;
            this.DrawImageOnSides = drawImageOnSides;
        }
    }

    public class FloorReflectionFilterProp
    {
        public int AlphaStart = 150;
        public int AlphaEnd = 50;

        public DockStyle DockPosition = DockStyle.Bottom;
        public int Offset = 50;

        public FloorReflectionFilterProp(int alphaStart, int alphaEnd, DockStyle dockPosition, int offset)
        {
            this.AlphaStart = alphaStart;
            this.AlphaEnd = alphaEnd;
            this.DockPosition = dockPosition;
            this.Offset = offset;
        }
    }

    internal class UndoHelper : Entity
    {
        private MainForm frmMain = null;        
        public UndoHelper(MainForm frmMain)
        {
            this.frmMain = frmMain;
        }

        public void Clear()
        {
            this.mHistory.ClearHistory();
        }

        #region Enhance Properties
        public double RibbonTrackEnhance1
        {
            get
            {
                return frmMain.ribbonTrackEnhance1.Value;
            }
            set
            {
                AddHistory("RibbonTrackEnhance1", value);

                if( this.mBeingUndone )
                    frmMain.ribbonTrackEnhance1.Value = value;
            }
        }

        public double RibbonTrackEnhance2
        {
            get
            {
                return frmMain.ribbonTrackEnhance2.Value;
            }
            set
            {
                AddHistory("RibbonTrackEnhance2", value);
                if (this.mBeingUndone)
                    frmMain.ribbonTrackEnhance2.Value = value;
            }
        }

        public double RibbonTrackEnhance3
        {
            get
            {
                return frmMain.ribbonTrackEnhance3.Value;
            }
            set
            {
                AddHistory("RibbonTrackEnhance3", value);
                if (this.mBeingUndone)
                    frmMain.ribbonTrackEnhance3.Value = value;
            }
        }

        public double RibbonTrackEnhance4
        {
            get
            {
                return frmMain.ribbonTrackEnhance4.Value;
            }
            set
            {
                AddHistory("RibbonTrackEnhance4", value);
                if (this.mBeingUndone)
                    frmMain.ribbonTrackEnhance4.Value = value;
            }
        }

        public double RibbonTrackEnhance5
        {
            get
            {
                return frmMain.ribbonTrackEnhance5.Value;
            }
            set
            {
                AddHistory("RibbonTrackEnhance5", value);
                if (this.mBeingUndone)
                    frmMain.ribbonTrackEnhance5.Value = value;
            }
        }

        public double RibbonTrackEnhance6
        {
            get
            {
                return frmMain.ribbonTrackEnhance6.Value;
            }
            set
            {
                AddHistory("RibbonTrackEnhance6", value);
                if (this.mBeingUndone)
                    frmMain.ribbonTrackEnhance6.Value = value;
            }
        }
        #endregion

        #region Offset Properties
        public double RibbonTrackOffset1
        {
            get
            {
                return frmMain.ribbonTrackOffset1.Value;
            }
            set
            {
                AddHistory("RibbonTrackOffset1", value);

                if (this.mBeingUndone)
                    frmMain.ribbonTrackOffset1.Value = value;
            }
        }
        public double RibbonTrackOffset2
        {
            get
            {
                return frmMain.ribbonTrackOffset2.Value;
            }
            set
            {
                AddHistory("RibbonTrackOffset2", value);

                if (this.mBeingUndone)
                    frmMain.ribbonTrackOffset2.Value = value;
            }
        }

        public double RibbonTrackOffset3
        {
            get
            {
                return frmMain.ribbonTrackOffset3.Value;
            }
            set
            {
                AddHistory("RibbonTrackOffset3", value);

                if (this.mBeingUndone)
                    frmMain.ribbonTrackOffset3.Value = value;
            }
        }


        public double RibbonTrackOffset4
        {
            get
            {
                return frmMain.ribbonTrackOffset4.Value;
            }
            set
            {
                AddHistory("RibbonTrackOffset4", value);

                if (this.mBeingUndone)
                    frmMain.ribbonTrackOffset4.Value = value;
            }
        }

        public double RibbonTrackOffset5
        {
            get
            {
                return frmMain.ribbonTrackOffset5.Value;
            }
            set
            {
                AddHistory("RibbonTrackOffset5", value);

                if (this.mBeingUndone)
                    frmMain.ribbonTrackOffset5.Value = value;
            }
        }

        public double RibbonTrackOffset6
        {
            get
            {
                return frmMain.ribbonTrackOffset6.Value;
            }
            set
            {
                AddHistory("RibbonTrackOffset6", value);

                if (this.mBeingUndone)
                    frmMain.ribbonTrackOffset6.Value = value;
            }
        }
        public double RibbonTrackOffset7
        {
            get
            {
                return frmMain.ribbonTrackOffset7.Value;
            }
            set
            {
                AddHistory("RibbonTrackOffset7", value);

                if (this.mBeingUndone)
                    frmMain.ribbonTrackOffset7.Value = value;
            }
        }
        public double RibbonTrackOffset8
        {
            get
            {
                return frmMain.ribbonTrackOffset8.Value;
            }
            set
            {
                AddHistory("RibbonTrackOffset8", value);

                if (this.mBeingUndone)
                    frmMain.ribbonTrackOffset8.Value = value;
            }
        }
        public double RibbonTrackOffset9
        {
            get
            {
                return frmMain.ribbonTrackOffset9.Value;
            }
            set
            {
                AddHistory("RibbonTrackOffset9", value);

                if (this.mBeingUndone)
                    frmMain.ribbonTrackOffset9.Value = value;
            }
        }
        #endregion

        #region Gain Properties (Generated programmatically)


        public double RibbonTrackGain1 { get { return frmMain.ribbonTrackGain1.Value; } set { AddHistory("RibbonTrackGain1", value); if (this.mBeingUndone)frmMain.ribbonTrackGain1.Value = value; } }

        public double RibbonTrackGain2 { get { return frmMain.ribbonTrackGain2.Value; } set { AddHistory("RibbonTrackGain2", value); if (this.mBeingUndone)frmMain.ribbonTrackGain2.Value = value; } }

        public double RibbonTrackGain3 { get { return frmMain.ribbonTrackGain3.Value; } set { AddHistory("RibbonTrackGain3", value); if (this.mBeingUndone)frmMain.ribbonTrackGain3.Value = value; } }

        public double RibbonTrackGain4 { get { return frmMain.ribbonTrackGain4.Value; } set { AddHistory("RibbonTrackGain4", value); if (this.mBeingUndone)frmMain.ribbonTrackGain4.Value = value; } }

        public double RibbonTrackGain5 { get { return frmMain.ribbonTrackGain5.Value; } set { AddHistory("RibbonTrackGain5", value); if (this.mBeingUndone)frmMain.ribbonTrackGain5.Value = value; } }

        public double RibbonTrackGain6 { get { return frmMain.ribbonTrackGain6.Value; } set { AddHistory("RibbonTrackGain6", value); if (this.mBeingUndone)frmMain.ribbonTrackGain6.Value = value; } }

        public double RibbonTrackGain7 { get { return frmMain.ribbonTrackGain7.Value; } set { AddHistory("RibbonTrackGain7", value); if (this.mBeingUndone)frmMain.ribbonTrackGain7.Value = value; } }

        public double RibbonTrackGain8 { get { return frmMain.ribbonTrackGain8.Value; } set { AddHistory("RibbonTrackGain8", value); if (this.mBeingUndone)frmMain.ribbonTrackGain8.Value = value; } }

        public double RibbonTrackGain9 { get { return frmMain.ribbonTrackGain9.Value; } set { AddHistory("RibbonTrackGain9", value); if (this.mBeingUndone)frmMain.ribbonTrackGain9.Value = value; } }

        public double RibbonTrackGain10 { get { return frmMain.ribbonTrackGain10.Value; } set { AddHistory("RibbonTrackGain10", value); if (this.mBeingUndone)frmMain.ribbonTrackGain10.Value = value; } }

        public double RibbonTrackGain11 { get { return frmMain.ribbonTrackGain11.Value; } set { AddHistory("RibbonTrackGain11", value); if (this.mBeingUndone)frmMain.ribbonTrackGain11.Value = value; } }

        public double RibbonTrackGain12 { get { return frmMain.ribbonTrackGain12.Value; } set { AddHistory("RibbonTrackGain12", value); if (this.mBeingUndone)frmMain.ribbonTrackGain12.Value = value; } }

        public double RibbonTrackGain13 { get { return frmMain.ribbonTrackGain13.Value; } set { AddHistory("RibbonTrackGain13", value); if (this.mBeingUndone)frmMain.ribbonTrackGain13.Value = value; } }

        public double RibbonTrackGain14 { get { return frmMain.ribbonTrackGain14.Value; } set { AddHistory("RibbonTrackGain14", value); if (this.mBeingUndone)frmMain.ribbonTrackGain14.Value = value; } }

        public double RibbonTrackGain15 { get { return frmMain.ribbonTrackGain15.Value; } set { AddHistory("RibbonTrackGain15", value); if (this.mBeingUndone)frmMain.ribbonTrackGain15.Value = value; } }

        public double RibbonTrackGain16 { get { return frmMain.ribbonTrackGain16.Value; } set { AddHistory("RibbonTrackGain16", value); if (this.mBeingUndone)frmMain.ribbonTrackGain16.Value = value; } }

        public double RibbonTrackGain17 { get { return frmMain.ribbonTrackGain17.Value; } set { AddHistory("RibbonTrackGain17", value); if (this.mBeingUndone)frmMain.ribbonTrackGain17.Value = value; } }

        public double RibbonTrackGain18 { get { return frmMain.ribbonTrackGain18.Value; } set { AddHistory("RibbonTrackGain18", value); if (this.mBeingUndone)frmMain.ribbonTrackGain18.Value = value; } }
        #endregion

        #region Gamma Properties (Generated programmatically)

        public double RibbonTrackGamma1 { get { return frmMain.ribbonTrackGamma1.Value; } set { AddHistory("RibbonTrackGamma1", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma1.Value = value; } }

        public double RibbonTrackGamma2 { get { return frmMain.ribbonTrackGamma2.Value; } set { AddHistory("RibbonTrackGamma2", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma2.Value = value; } }

        public double RibbonTrackGamma3 { get { return frmMain.ribbonTrackGamma3.Value; } set { AddHistory("RibbonTrackGamma3", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma3.Value = value; } }

        public double RibbonTrackGamma4 { get { return frmMain.ribbonTrackGamma4.Value; } set { AddHistory("RibbonTrackGamma4", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma4.Value = value; } }

        public double RibbonTrackGamma5 { get { return frmMain.ribbonTrackGamma5.Value; } set { AddHistory("RibbonTrackGamma5", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma5.Value = value; } }

        public double RibbonTrackGamma6 { get { return frmMain.ribbonTrackGamma6.Value; } set { AddHistory("RibbonTrackGamma6", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma6.Value = value; } }

        public double RibbonTrackGamma7 { get { return frmMain.ribbonTrackGamma7.Value; } set { AddHistory("RibbonTrackGamma7", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma7.Value = value; } }

        public double RibbonTrackGamma8 { get { return frmMain.ribbonTrackGamma8.Value; } set { AddHistory("RibbonTrackGamma8", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma8.Value = value; } }

        public double RibbonTrackGamma9 { get { return frmMain.ribbonTrackGamma9.Value; } set { AddHistory("RibbonTrackGamma9", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma9.Value = value; } }

        public double RibbonTrackGamma10 { get { return frmMain.ribbonTrackGamma10.Value; } set { AddHistory("RibbonTrackGamma10", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma10.Value = value; } }

        public double RibbonTrackGamma11 { get { return frmMain.ribbonTrackGamma11.Value; } set { AddHistory("RibbonTrackGamma11", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma11.Value = value; } }

        public double RibbonTrackGamma12 { get { return frmMain.ribbonTrackGamma12.Value; } set { AddHistory("RibbonTrackGamma12", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma12.Value = value; } }

        public double RibbonTrackGamma13 { get { return frmMain.ribbonTrackGamma13.Value; } set { AddHistory("RibbonTrackGamma13", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma13.Value = value; } }

        public double RibbonTrackGamma14 { get { return frmMain.ribbonTrackGamma14.Value; } set { AddHistory("RibbonTrackGamma14", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma14.Value = value; } }

        public double RibbonTrackGamma15 { get { return frmMain.ribbonTrackGamma15.Value; } set { AddHistory("RibbonTrackGamma15", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma15.Value = value; } }

        public double RibbonTrackGamma16 { get { return frmMain.ribbonTrackGamma16.Value; } set { AddHistory("RibbonTrackGamma16", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma16.Value = value; } }

        public double RibbonTrackGamma17 { get { return frmMain.ribbonTrackGamma17.Value; } set { AddHistory("RibbonTrackGamma17", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma17.Value = value; } }

        public double RibbonTrackGamma18 { get { return frmMain.ribbonTrackGamma18.Value; } set { AddHistory("RibbonTrackGamma18", value); if (this.mBeingUndone)frmMain.ribbonTrackGamma18.Value = value; } }
        #endregion

        #region Atmosphere Properties (Generated programmatically)

        public double RibbonTrackAtm1 { get { return frmMain.ribbonTrackAtm1.Value; } set { AddHistory("RibbonTrackAtm1", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm1.Value = value; } }

        public double RibbonTrackAtm2 { get { return frmMain.ribbonTrackAtm2.Value; } set { AddHistory("RibbonTrackAtm2", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm2.Value = value; } }

        public double RibbonTrackAtm3 { get { return frmMain.ribbonTrackAtm3.Value; } set { AddHistory("RibbonTrackAtm3", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm3.Value = value; } }

        public double RibbonTrackAtm4 { get { return frmMain.ribbonTrackAtm4.Value; } set { AddHistory("RibbonTrackAtm4", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm4.Value = value; } }

        public double RibbonTrackAtm5 { get { return frmMain.ribbonTrackAtm5.Value; } set { AddHistory("RibbonTrackAtm5", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm5.Value = value; } }

        public double RibbonTrackAtm6 { get { return frmMain.ribbonTrackAtm6.Value; } set { AddHistory("RibbonTrackAtm6", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm6.Value = value; } }

        public double RibbonTrackAtm7 { get { return frmMain.ribbonTrackAtm7.Value; } set { AddHistory("RibbonTrackAtm7", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm7.Value = value; } }

        public double RibbonTrackAtm8 { get { return frmMain.ribbonTrackAtm8.Value; } set { AddHistory("RibbonTrackAtm8", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm8.Value = value; } }

        public double RibbonTrackAtm9 { get { return frmMain.ribbonTrackAtm9.Value; } set { AddHistory("RibbonTrackAtm9", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm9.Value = value; } }

        public double RibbonTrackAtm10 { get { return frmMain.ribbonTrackAtm10.Value; } set { AddHistory("RibbonTrackAtm10", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm10.Value = value; } }

        public double RibbonTrackAtm11 { get { return frmMain.ribbonTrackAtm11.Value; } set { AddHistory("RibbonTrackAtm11", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm11.Value = value; } }

        public double RibbonTrackAtm12 { get { return frmMain.ribbonTrackAtm12.Value; } set { AddHistory("RibbonTrackAtm12", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm12.Value = value; } }

        public double RibbonTrackAtm13 { get { return frmMain.ribbonTrackAtm13.Value; } set { AddHistory("RibbonTrackAtm13", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm13.Value = value; } }

        public double RibbonTrackAtm14 { get { return frmMain.ribbonTrackAtm14.Value; } set { AddHistory("RibbonTrackAtm14", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm14.Value = value; } }

        public double RibbonTrackAtm15 { get { return frmMain.ribbonTrackAtm15.Value; } set { AddHistory("RibbonTrackAtm15", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm15.Value = value; } }

        public double RibbonTrackAtm16 { get { return frmMain.ribbonTrackAtm16.Value; } set { AddHistory("RibbonTrackAtm16", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm16.Value = value; } }

        public double RibbonTrackAtm17 { get { return frmMain.ribbonTrackAtm17.Value; } set { AddHistory("RibbonTrackAtm17", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm17.Value = value; } }

        public double RibbonTrackAtm18 { get { return frmMain.ribbonTrackAtm18.Value; } set { AddHistory("RibbonTrackAtm18", value); if (this.mBeingUndone)frmMain.ribbonTrackAtm18.Value = value; } }

        #endregion

        #region Lens Properties (Generated programmatically)

        public double RibbonTrackLens1 { get { return frmMain.ribbonTrackLens1.Value; } set { AddHistory("RibbonTrackLens1", value); if (this.mBeingUndone)frmMain.ribbonTrackLens1.Value = value; } }

        public double RibbonTrackLens2 { get { return frmMain.ribbonTrackLens2.Value; } set { AddHistory("RibbonTrackLens2", value); if (this.mBeingUndone)frmMain.ribbonTrackLens2.Value = value; } }

        public double RibbonTrackLens3 { get { return frmMain.ribbonTrackLens3.Value; } set { AddHistory("RibbonTrackLens3", value); if (this.mBeingUndone)frmMain.ribbonTrackLens3.Value = value; } }

        public double RibbonTrackLens4 { get { return frmMain.ribbonTrackLens4.Value; } set { AddHistory("RibbonTrackLens4", value); if (this.mBeingUndone)frmMain.ribbonTrackLens4.Value = value; } }

        public double RibbonTrackLens5 { get { return frmMain.ribbonTrackLens5.Value; } set { AddHistory("RibbonTrackLens5", value); if (this.mBeingUndone)frmMain.ribbonTrackLens5.Value = value; } }

        public double RibbonTrackLens6 { get { return frmMain.ribbonTrackLens6.Value; } set { AddHistory("RibbonTrackLens6", value); if (this.mBeingUndone)frmMain.ribbonTrackLens6.Value = value; } }

        public double RibbonTrackLens7 { get { return frmMain.ribbonTrackLens7.Value; } set { AddHistory("RibbonTrackLens7", value); if (this.mBeingUndone)frmMain.ribbonTrackLens7.Value = value; } }

        public double RibbonTrackLens8 { get { return frmMain.ribbonTrackLens8.Value; } set { AddHistory("RibbonTrackLens8", value); if (this.mBeingUndone)frmMain.ribbonTrackLens8.Value = value; } }

        public double RibbonTrackLens9 { get { return frmMain.ribbonTrackLens9.Value; } set { AddHistory("RibbonTrackLens9", value); if (this.mBeingUndone)frmMain.ribbonTrackLens9.Value = value; } }

        public double RibbonTrackLens10 { get { return frmMain.ribbonTrackLens10.Value; } set { AddHistory("RibbonTrackLens10", value); if (this.mBeingUndone)frmMain.ribbonTrackLens10.Value = value; } }

        public double RibbonTrackLens11 { get { return frmMain.ribbonTrackLens11.Value; } set { AddHistory("RibbonTrackLens11", value); if (this.mBeingUndone)frmMain.ribbonTrackLens11.Value = value; } }

        public double RibbonTrackLens12 { get { return frmMain.ribbonTrackLens12.Value; } set { AddHistory("RibbonTrackLens12", value); if (this.mBeingUndone)frmMain.ribbonTrackLens12.Value = value; } }

        public double RibbonTrackLens13 { get { return frmMain.ribbonTrackLens13.Value; } set { AddHistory("RibbonTrackLens13", value); if (this.mBeingUndone)frmMain.ribbonTrackLens13.Value = value; } }

        public double RibbonTrackLens14 { get { return frmMain.ribbonTrackLens14.Value; } set { AddHistory("RibbonTrackLens14", value); if (this.mBeingUndone)frmMain.ribbonTrackLens14.Value = value; } }

        public double RibbonTrackLens15 { get { return frmMain.ribbonTrackLens15.Value; } set { AddHistory("RibbonTrackLens15", value); if (this.mBeingUndone)frmMain.ribbonTrackLens15.Value = value; } }

        public double RibbonTrackLens16 { get { return frmMain.ribbonTrackLens16.Value; } set { AddHistory("RibbonTrackLens16", value); if (this.mBeingUndone)frmMain.ribbonTrackLens16.Value = value; } }

        public double RibbonTrackLens17 { get { return frmMain.ribbonTrackLens17.Value; } set { AddHistory("RibbonTrackLens17", value); if (this.mBeingUndone)frmMain.ribbonTrackLens17.Value = value; } }

        public double RibbonTrackLens18 { get { return frmMain.ribbonTrackLens18.Value; } set { AddHistory("RibbonTrackLens18", value); if (this.mBeingUndone)frmMain.ribbonTrackLens18.Value = value; } }

        public double RibbonTrackLens19 { get { return frmMain.ribbonTrackLens19.Value; } set { AddHistory("RibbonTrackLens19", value); if (this.mBeingUndone)frmMain.ribbonTrackLens19.Value = value; } }

        public double RibbonTrackLens20 { get { return frmMain.ribbonTrackLens20.Value; } set { AddHistory("RibbonTrackLens20", value); if (this.mBeingUndone)frmMain.ribbonTrackLens20.Value = value; } }

#endregion

        #region File Properties (Generated programmatically)
        public double RibbonTrackFilm1 { get { return frmMain.ribbonTrackFilm1.Value; } set { AddHistory("RibbonTrackFilm1", value); if (this.mBeingUndone)frmMain.ribbonTrackFilm1.Value = value; } }
        public double RibbonTrackFilm2 { get { return frmMain.ribbonTrackFilm2.Value; } set { AddHistory("RibbonTrackFilm2", value); if (this.mBeingUndone)frmMain.ribbonTrackFilm2.Value = value; } }
        public double RibbonTrackFilm3 { get { return frmMain.ribbonTrackFilm3.Value; } set { AddHistory("RibbonTrackFilm3", value); if (this.mBeingUndone)frmMain.ribbonTrackFilm3.Value = value; } }
        public double RibbonTrackFilm4 { get { return frmMain.ribbonTrackFilm4.Value; } set { AddHistory("RibbonTrackFilm4", value); if (this.mBeingUndone)frmMain.ribbonTrackFilm4.Value = value; } }

        #endregion

        #region WaterMark Properties

        public Image WaterMarkImage
        {
            get 
            {
                return frmMain.WaterMarkImage;
            } 
            set 
            {
                AddHistory("WaterMarkImage", value); 
                if (this.mBeingUndone)
                    frmMain.WaterMarkImage = value; 
            }
        }

        public String WaterMarkText
        {
            get
            {
                return frmMain.WaterMarkText;
            }
            set
            {
                AddHistory("WaterMarkText", value);
                if (this.mBeingUndone)
                    frmMain.WaterMarkText = value; 
            }
        }

        public Font WaterMarkFont
        {
            get
            {
                return frmMain.WaterMarkFont;
            }
            set
            {
                AddHistory("WaterMarkFont", value);
                if (this.mBeingUndone)
                    frmMain.WaterMarkFont = value; 
            }
        }

        public ContentAlignment ImageAlign
        {
            get
            {
                return frmMain.ImageAlign;
            }
            set
            {
                AddHistory("ImageAlign", value);
                if (this.mBeingUndone)
                    frmMain.ImageAlign = value; 
            }
        }

        public ContentAlignment TextAlign
        {
            get
            {
                return frmMain.TextAlign;
            }
            set
            {
                AddHistory("TextAlign", value);
                if (this.mBeingUndone)
                    frmMain.TextAlign = value; 
            }
        }
        #endregion

        #region EnhanceN Properties
        public double RibbonTrackEnhanceN1
        {
            get
            {
                return frmMain.ribbonTrackEnhanceN1.Value;
            }
            set
            {
                AddHistory("RibbonTrackEnhanceN1", value);

                if (this.mBeingUndone)
                    frmMain.ribbonTrackEnhanceN1.Value = value;
            }
        }

        public double RibbonTrackEnhanceN2
        {
            get
            {
                return frmMain.ribbonTrackEnhanceN2.Value;
            }
            set
            {
                AddHistory("RibbonTrackEnhanceN2", value);
                if (this.mBeingUndone)
                    frmMain.ribbonTrackEnhanceN2.Value = value;
            }
        }

        public double RibbonTrackEnhanceN3
        {
            get
            {
                return frmMain.ribbonTrackEnhanceN3.Value;
            }
            set
            {
                AddHistory("RibbonTrackEnhanceN3", value);
                if (this.mBeingUndone)
                    frmMain.ribbonTrackEnhanceN3.Value = value;
            }
        }

        public double RibbonTrackEnhanceN4
        {
            get
            {
                return frmMain.ribbonTrackEnhanceN4.Value;
            }
            set
            {
                AddHistory("RibbonTrackEnhanceN4", value);
                if (this.mBeingUndone)
                    frmMain.ribbonTrackEnhanceN4.Value = value;
            }
        }

        public double RibbonTrackEnhanceN5
        {
            get
            {
                return frmMain.ribbonTrackEnhanceN5.Value;
            }
            set
            {
                AddHistory("RibbonTrackEnhanceN5", value);
                if (this.mBeingUndone)
                    frmMain.ribbonTrackEnhanceN5.Value = value;
            }
        }

        #endregion

        #region Operations Properties
        public Rectangle CropData
        {
            get
            {
                return frmMain.CropData;
            }
            set
            {
                AddHistory("CropData", value);
                if (this.mBeingUndone)
                    frmMain.CropData = value;
            }
        }

        public BoxFilterProp BoxFilter
        {
            get
            {
                return Effects2.BoxFilter;
            }
            set
            {
                AddHistory("BoxFilter", value);
                if (this.mBeingUndone)
                    Effects2.BoxFilter = value;
            }
        }

        public int FishEyeCurvature
        {
            get
            {
                return Effects2.curvature;
            }
            set
            {
                AddHistory("FishEyeCurvature", value);
                if (this.mBeingUndone)
                    Effects2.curvature = value;
            }
        }

        public FloorReflectionFilterProp FloorReflection
        {
            get
            {
                return Effects2.FloorReflectionFilter;
            }
            set
            {
                AddHistory("FloorReflection", value);
                if (this.mBeingUndone)
                    Effects2.FloorReflectionFilter = value;
            }
        }

        public RotateFlipType Rotate
        {
            get
            {
                return Effects2.RotateForUndo;
            }
            set
            {
                AddHistory("Rotate", value);
                if (this.mBeingUndone)
                    Effects2.RotateForUndo = value;
            }
        }

        public bool GrayScale
        {
            get
            {
                return Effects2.grayscale;
            }
            set
            {
                AddHistory("GrayScale", value);
                if (this.mBeingUndone)
                {
                    Effects2.grayscale = value;
                    frmMain.chkGrayScale.Checked = value;
                }
            }
        }

        public bool DropShadow
        {
            get
            {
                return Effects2.dropshadow;
            }
            set
            {
                AddHistory("DropShadow", value);
                if (this.mBeingUndone)
                {
                    Effects2.dropshadow = value;
                    frmMain.chkDropShadow.Checked = value;
                }
            }
        }

        public bool Rounded
        {
            get
            {
                return Effects2.rounded;
            }
            set
            {
                AddHistory("Rounded", value);
                if (this.mBeingUndone)
                {
                    Effects2.rounded = value;
                    frmMain.chkRounded.Checked = value;
                }
            }
        }


        public RotateFlipType Flip
        {
            get
            {
                return Effects2.FlipForUndo;
            }
            set
            {
                AddHistory("Flip", value);
                if (this.mBeingUndone)
                    Effects2.FlipForUndo = value;
            }
        }


        #endregion

        public Image Img
        {
            get
            {
                return frmMain.Img;
            }
            set
            {
                AddHistory("Img", value);
                if (this.mBeingUndone)
                    frmMain.Img = value;                
            }
        }
    }

}
