using System;
using System.Collections.Generic;
using System.Text;
using Next2Friends.ImageWorks;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Next2Friends.ImageWorks.UI.NuGenImageWorks.Undo
{    
    public class UndoHelper : Entity
    {
        private ImageParameters img;

        public UndoHelper(ImageParameters img)
        {
            this.img = img;
        }

        public void Clear()
        {
            this.mHistory.ClearHistory();
        }

        // Allows access to a historic copy of the image so that an undo operation can
        // revert if the source image is switched.
        public BitmapSource Img
        {
            get
            {
                return img.SourceImage;
            }
            set
            {
                AddHistory("Img", value);
                if (this.mBeingUndone)
                    img.SourceImage = value;
            }
        }

        #region Enhance Properties
        public double DetailSaturationLow
        {
            get
            {                                
                return img.DetailSaturationLow;
            }
            set
            {
                AddHistory("DetailSaturationLow", value);

                if( this.mBeingUndone )
                    img.DetailSaturationLow = value;
            }
        }

        public double DetailContrastLow
        {
            get
            {
                return img.DetailContrastLow;
            }
            set
            {
                AddHistory("DetailContrastLow", value);
                if (this.mBeingUndone)
                    img.DetailContrastLow = value;
            }
        }

        public double DetailSaturationMid
        {
            get
            {
                return img.DetailSaturationMid;
            }
            set
            {
                AddHistory("DetailSaturationMid", value);
                if (this.mBeingUndone)
                    img.DetailSaturationMid = value;
            }
        }

        public double DetailContrastMid
        {
            get
            {
                return img.DetailContrastMid;
            }
            set
            {
                AddHistory("DetailContrastMid", value);
                if (this.mBeingUndone)
                    img.DetailContrastMid = value;
            }
        }

        public double DetailSaturationHigh
        {
            get
            {
                return img.DetailSaturationHigh;
            }
            set
            {
                AddHistory("RibbonTrackEnhance5", value);
                if (this.mBeingUndone)
                    img.DetailSaturationHigh = value;
            }
        }

        public double DetailContrastHigh
        {
            get
            {
                return img.DetailContrastHigh;
            }
            set
            {
                AddHistory("DetailContrastHigh", value);
                if (this.mBeingUndone)
                    img.DetailContrastHigh = value;
            }
        }
        #endregion

        #region Offset Properties
        public double RedOffsetLow
        {
            get
            {
                return img.RedOffsetLow;
            }
            set
            {
                AddHistory("RedOffsetLow", value);

                if (this.mBeingUndone)
                    img.RedOffsetLow = value;
            }
        }
        public double GreenOffsetLow
        {
            get
            {
                return img.GreenOffsetLow;
            }
            set
            {
                AddHistory("GreenOffsetLow", value);

                if (this.mBeingUndone)
                    img.GreenOffsetLow = value;
            }
        }

        public double BlueOffsetLow
        {
            get
            {
                return img.BlueOffsetLow;
            }
            set
            {
                AddHistory("BlueOffsetLow", value);

                if (this.mBeingUndone)
                    img.BlueOffsetLow = value;
            }
        }


        public double RedOffsetMid
        {
            get
            {
                return img.RedOffsetMid;
            }
            set
            {
                AddHistory("RedOffsetMid", value);

                if (this.mBeingUndone)
                    img.RedOffsetMid = value;
            }
        }

        public double GreenOffsetMid
        {
            get
            {
                return img.GreenOffsetMid;
            }
            set
            {
                AddHistory("GreenOffsetMid", value);

                if (this.mBeingUndone)
                    img.GreenOffsetMid = value;
            }
        }

        public double BlueOffsetMid
        {
            get
            {
                return img.BlueOffsetMid;
            }
            set
            {
                AddHistory("BlueOffsetMid", value);

                if (this.mBeingUndone)
                    img.BlueOffsetMid = value;
            }
        }
        public double RedOffsetHigh
        {
            get
            {
                return img.RedOffsetHigh;
            }
            set
            {
                AddHistory("RedOffsetHigh", value);

                if (this.mBeingUndone)
                    img.RedOffsetHigh = value;
            }
        }
        public double GreenOffsetHigh
        {
            get
            {
                return img.GreenOffsetHigh;
            }
            set
            {
                AddHistory("GreenOffsetHigh", value);

                if (this.mBeingUndone)
                    img.GreenOffsetHigh = value;
            }
        }
        public double BlueOffsetHigh
        {
            get
            {
                return img.BlueOffsetHigh;
            }
            set
            {
                AddHistory("BlueOffsetHigh", value);

                if (this.mBeingUndone)
                    img.BlueOffsetHigh = value;
            }
        }
        #endregion

        #region Gain Properties (Generated programmatically)


        public double TemperatureGainLow { get { return img.TemperatureGainLow; } set { AddHistory("TemperatureGainLow", value); if (this.mBeingUndone)img.TemperatureGainLow = value; } }

        public double MagentaGainLow { get { return img.MagentaGainLow; } set { AddHistory("MagentaGainLow", value); if (this.mBeingUndone)img.MagentaGainLow = value; } }

        public double OverallGainLow { get { return img.OverallGainLow; } set { AddHistory("OverallGainLow", value); if (this.mBeingUndone)img.OverallGainLow = value; } }

        public double RedGainLow { get { return img.RedGainLow; } set { AddHistory("RedGainLow", value); if (this.mBeingUndone)img.RedGainLow = value; } }

        public double GreenGainLow { get { return img.GreenGainLow; } set { AddHistory("GreenGainLow", value); if (this.mBeingUndone)img.GreenGainLow = value; } }

        public double BlueGainLow { get { return img.BlueGainLow; } set { AddHistory("BlueGainLow", value); if (this.mBeingUndone)img.BlueGainLow = value; } }

        public double TemperatureGainMid { get { return img.TemperatureGainMid; } set { AddHistory("TemperatureGainMid", value); if (this.mBeingUndone)img.TemperatureGainMid = value; } }

        public double MagentaGainMid { get { return img.MagentaGainMid; } set { AddHistory("MagentaGainMid", value); if (this.mBeingUndone)img.MagentaGainMid = value; } }

        public double OverallGainMid { get { return img.OverallGainMid; } set { AddHistory("OverallGainMid", value); if (this.mBeingUndone)img.OverallGainMid = value; } }

        public double RedGainMid { get { return img.RedGainMid; } set { AddHistory("RedGainMid", value); if (this.mBeingUndone)img.RedGainMid = value; } }

        public double GreenGainMid { get { return img.GreenGainMid; } set { AddHistory("GreenGainMid", value); if (this.mBeingUndone)img.GreenGainMid = value; } }

        public double BlueGainMid { get { return img.BlueGainMid; } set { AddHistory("BlueGainMid", value); if (this.mBeingUndone)img.BlueGainMid = value; } }

        public double TemperatureGainHigh { get { return img.TemperatureGainHigh; } set { AddHistory("TemperatureGainHigh", value); if (this.mBeingUndone)img.TemperatureGainHigh = value; } }

        public double MagentaGainHigh { get { return img.MagentaGainHigh; } set { AddHistory("MagentaGainHigh", value); if (this.mBeingUndone)img.MagentaGainHigh = value; } }

        public double OverallGainHigh { get { return img.OverallGainHigh; } set { AddHistory("OverallGainHigh", value); if (this.mBeingUndone)img.OverallGainHigh = value; } }

        public double RedGainHigh { get { return img.RedGainHigh; } set { AddHistory("RedGainHigh", value); if (this.mBeingUndone)img.RedGainHigh = value; } }

        public double GreenGainHigh { get { return img.GreenGainHigh; } set { AddHistory("GreenGainHigh", value); if (this.mBeingUndone)img.GreenGainHigh = value; } }

        public double BlueGainHigh { get { return img.BlueGainHigh; } set { AddHistory("BlueGainHigh", value); if (this.mBeingUndone)img.BlueGainHigh = value; } }
        #endregion

        #region Gamma Properties (Generated programmatically)

        public double InRedLow { get { return img.InRedLow; } set { AddHistory("InRedLow", value); if (this.mBeingUndone)img.InRedLow = value; } }

        public double InGreenLow { get { return img.InGreenLow; } set { AddHistory("InGreenLow", value); if (this.mBeingUndone)img.InGreenLow = value; } }

        public double InBlueLow { get { return img.InBlueLow; } set { AddHistory("InBlueLow", value); if (this.mBeingUndone)img.InBlueLow = value; } }

        public double OutRedLow { get { return img.OutRedLow; } set { AddHistory("OutRedLow", value); if (this.mBeingUndone)img.OutRedLow = value; } }

        public double OutGreenLow { get { return img.OutGreenLow; } set { AddHistory("OutGreenLow", value); if (this.mBeingUndone)img.OutGreenLow = value; } }

        public double OutBlueLow { get { return img.OutBlueLow; } set { AddHistory("OutBlueLow", value); if (this.mBeingUndone)img.OutBlueLow = value; } }

        public double InRedMid { get { return img.InRedMid; } set { AddHistory("InRedMid", value); if (this.mBeingUndone)img.InRedMid = value; } }

        public double InGreenMid { get { return img.InGreenMid; } set { AddHistory("InGreenMid", value); if (this.mBeingUndone)img.InGreenMid = value; } }

        public double InBlueMid { get { return img.InBlueMid; } set { AddHistory("InBlueMid", value); if (this.mBeingUndone)img.InBlueMid = value; } }

        public double OutRedMid { get { return img.OutRedMid; } set { AddHistory("OutRedMid", value); if (this.mBeingUndone)img.OutRedMid = value; } }

        public double OutGreenMid { get { return img.OutGreenMid; } set { AddHistory("OutGreenMid", value); if (this.mBeingUndone)img.OutGreenMid = value; } }

        public double OutBlueMid { get { return img.OutBlueMid; } set { AddHistory("OutBlueMid", value); if (this.mBeingUndone)img.OutBlueMid = value; } }

        public double InRedHigh { get { return img.InRedHigh; } set { AddHistory("InRedHigh", value); if (this.mBeingUndone)img.InRedHigh = value; } }

        public double InGreenHigh { get { return img.InGreenHigh; } set { AddHistory("InGreenHigh", value); if (this.mBeingUndone)img.InGreenHigh = value; } }

        public double InBlueHigh { get { return img.InBlueHigh; } set { AddHistory("InBlueHigh", value); if (this.mBeingUndone)img.InBlueHigh = value; } }

        public double OutRedHigh { get { return img.OutRedHigh; } set { AddHistory("OutRedHigh", value); if (this.mBeingUndone)img.OutRedHigh = value; } }

        public double OutGreenHigh { get { return img.OutGreenHigh; } set { AddHistory("OutGreenHigh", value); if (this.mBeingUndone)img.OutGreenHigh = value; } }

        public double OutBlueHigh { get { return img.OutBlueHigh; } set { AddHistory("OutBlueHigh", value); if (this.mBeingUndone)img.OutBlueHigh = value; } }
        #endregion        

        #region EnhanceN Properties
        public double Brightness
        {
            get
            {
                return img.Brightness;
            }
            set
            {
                AddHistory("Brightness", value);

                if (this.mBeingUndone)
                    img.Brightness = value;
            }
        }

        public double Contrast
        {
            get
            {
                return img.Contrast;
            }
            set
            {
                AddHistory("Contrast", value);
                if (this.mBeingUndone)
                    img.Contrast = value;
            }
        }

        public double Smooth
        {
            get
            {
                return img.Smooth;
            }
            set
            {
                AddHistory("Smooth", value);
                if (this.mBeingUndone)
                    img.Smooth = value;
            }
        }

        public double Sharpen
        {
            get
            {
                return img.Sharpen;
            }
            set
            {
                AddHistory("Sharpen", value);
                if (this.mBeingUndone)
                    img.Sharpen = value;
            }
        }

        #endregion

        #region Operations Properties
        public Rect CropData
        {
            get
            {
                return img.CropData;
            }
            set
            {
                AddHistory("CropData", value);
                if (this.mBeingUndone)
                    img.CropData = value;
            }
        }

        public float FishEyeCurvature
        {
            get
            {
                return ImageParameters.Instance.Curvature;
            }
            set
            {
                AddHistory("FishEyeCurvature", value);
                if (this.mBeingUndone)
                    ImageParameters.Instance.Curvature = value;
            }
        }

        public bool GrayScale
        {
            get
            {
                return ImageParameters.Instance.Grayscale;
            }
            set
            {
                AddHistory("GrayScale", value);
                if (this.mBeingUndone)
                {
                    ImageParameters.Instance.Grayscale = value;
                    //frmMain.chkGrayScale.Checked = value;
                    img.Grayscale = value;
                }
            }
        }

        #endregion
    }

}
