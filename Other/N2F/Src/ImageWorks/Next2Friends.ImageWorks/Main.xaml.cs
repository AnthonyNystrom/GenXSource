using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Next2Friends.ImageWorks.EffectsFilters;
using Next2Friends.ImageWorks.Filters;
using Next2Friends.ImageWorks.SnapUpService;
using Next2Friends.ImageWorks.UI.NuGenImageWorks;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Web;
using System.Windows.Interop;

namespace Next2Friends.ImageWorks
{
    public partial class Main : Page
    {
        //Holds the region that is being cropped
        private Rect cropData = Rect.Empty;
        //Holds the region that may be cropped but hasn't confirmed yet
        private Rect tempCropData = Rect.Empty;

        private string Nickname = "hash";
        private string Password = "passhash";

        private string lastActivity = "";

        //Flags wheter or not we are in crop mode
        private bool crop = false;
        public bool Crop
        {
            get
            {
                return crop;
            }

            set
            {
                crop = value;

                if (crop == false)
                {
                    image1_Surface.Source = null;
                }
            }
        }

        //The images currently available for processing in the current collection
        public PhotoItem[] Images { get; set; }

        //The list of collections our user has on their account
        public PhotoCollectionItem[] Collections { get; set; }

        //The pivot point for dragging the crop rectangle
        private Point cropPivot;

        // This flags whether or not the application is currently processing
        // the image so that mulitple requests dont get queued up
        private bool working = false;

        //Maps tab pages to labels for easy tab switching
        private Dictionary<String, int> tabPageDictionary;

        //Maps sliders to labels so that sliders can identify with a common set of operations
        private Dictionary<Slider, Label> sliderLabelDictionary;

        //The service for online operations
        private SnapUpServiceSoapClient client;

        //Has the UI been setup yet
        private bool setup = false;

        //Has the image been modified
        private bool modified = false;

        //The image that is selected
        private String selectedImageId = "";

        //Histograms for auto adjustments
        private ColorHistogram colorHistogram;
        private BrightnessHistogram brightnessHistogram;

        public Main()
        {                        
            InitializeComponent();        
        }

        private void SetUp()
        {
            setup = false;

            ImageParameters.Instance.SourceImage = image1.Source as BitmapSource;
            ImageParameters.Instance.Dispatcher = Dispatcher;
            ImageParameters.Instance.ImageChanged += new ImageParameters.ImageChangedDelegate(Instance_ImageChanged);

            colorHistogram = new ColorHistogram(image1.Source as BitmapSource);
            brightnessHistogram = new BrightnessHistogram(image1.Source as BitmapSource);

            CreateDataBindings();
            CreateTabDictionary();
            CreateSliderLabelDictionary();

            setup = true;
        }

        void Instance_ImageChanged()
        {
            FilterEffects();
            Cursor = Cursors.Arrow;
        }        

        private void CreateDataBindings()
        {
            Binding cropToggleBinding = new Binding("Crop");
            cropToggleBinding.Source = this;
            button_CropToggle.SetBinding(ToggleButton.IsCheckedProperty, cropToggleBinding);

            #region Appearance

            Binding brightnessBinding = new Binding("Brightness");
            brightnessBinding.Source = ImageParameters.Instance;
            sliderBrightness.SetBinding(Slider.ValueProperty, brightnessBinding);

            Binding contrastBinding = new Binding("Contrast");
            contrastBinding.Source = ImageParameters.Instance;
            sliderContrast.SetBinding(Slider.ValueProperty, contrastBinding);

            Binding temperatureBinding = new Binding("Temperature");
            temperatureBinding.Source = ImageParameters.Instance;
            sliderTemperature.SetBinding(Slider.ValueProperty, temperatureBinding);

            Binding hueBinding = new Binding("Hue");
            hueBinding.Source = ImageParameters.Instance;
            sliderHue.SetBinding(Slider.ValueProperty, hueBinding);

            Binding saturationBinding = new Binding("Saturation");
            saturationBinding.Source = ImageParameters.Instance;
            sliderSaturation.SetBinding(Slider.ValueProperty, saturationBinding);

            Binding lightnessBinding = new Binding("Lightness");
            lightnessBinding.Source = ImageParameters.Instance;
            sliderLightness.SetBinding(Slider.ValueProperty, lightnessBinding);

            #endregion

            #region Detail
            
            Binding smoothBinding = new Binding("Smooth");
            smoothBinding.Source = ImageParameters.Instance;
            sliderSmooth.SetBinding(Slider.ValueProperty, smoothBinding);

            Binding sharpenBinding = new Binding("Sharpen");
            sharpenBinding.Source = ImageParameters.Instance;
            sliderSharpen.SetBinding(Slider.ValueProperty, sharpenBinding);

            #endregion

            #region Enhance Detail

            Binding detailContrastLowBinding = new Binding("DetailContrastLow");
            detailContrastLowBinding.Source = ImageParameters.Instance;
            sliderConEnhLow.SetBinding(Slider.ValueProperty, detailContrastLowBinding);

            Binding detailSaturationLowBinding = new Binding("DetailSaturationLow");
            detailSaturationLowBinding.Source = ImageParameters.Instance;
            sliderSatEnhLow.SetBinding(Slider.ValueProperty, detailSaturationLowBinding);

            Binding detailContrastMidBinding = new Binding("DetailContrastMid");
            detailContrastMidBinding.Source = ImageParameters.Instance;
            sliderConEnhMid.SetBinding(Slider.ValueProperty, detailContrastMidBinding);

            Binding detailSaturationMidBinding = new Binding("DetailSaturationMid");
            detailSaturationMidBinding.Source = ImageParameters.Instance;
            sliderSatEnhMid.SetBinding(Slider.ValueProperty, detailSaturationMidBinding);

            Binding detailContrastHighBinding = new Binding("DetailContrastHigh");
            detailContrastHighBinding.Source = ImageParameters.Instance;
            sliderConEnhHigh.SetBinding(Slider.ValueProperty, detailContrastHighBinding);

            Binding detailSaturationHighBinding = new Binding("DetailSaturationHigh");
            detailSaturationHighBinding.Source = ImageParameters.Instance;
            sliderSatEnhHigh.SetBinding(Slider.ValueProperty, detailSaturationHighBinding);

            #endregion

            #region Offset

            Binding offsetRedLowBinding = new Binding("RedOffsetLow");
            offsetRedLowBinding.Source = ImageParameters.Instance;
            sliderOffsetRedLow.SetBinding(Slider.ValueProperty, offsetRedLowBinding);

            Binding offsetGreenLowBinding = new Binding("GreenOffsetLow");
            offsetGreenLowBinding.Source = ImageParameters.Instance;
            sliderOffsetGreenLow.SetBinding(Slider.ValueProperty, offsetGreenLowBinding);

            Binding offsetBlueLowBinding = new Binding("BlueOffsetLow");
            offsetBlueLowBinding.Source = ImageParameters.Instance;
            sliderOffsetBlueLow.SetBinding(Slider.ValueProperty, offsetBlueLowBinding);

            Binding offsetRedMidBinding = new Binding("RedOffsetMid");
            offsetRedMidBinding.Source = ImageParameters.Instance;
            sliderOffsetRedMid.SetBinding(Slider.ValueProperty, offsetRedMidBinding);

            Binding offsetGreenMidBinding = new Binding("GreenOffsetMid");
            offsetGreenMidBinding.Source = ImageParameters.Instance;
            sliderOffsetGreenMid.SetBinding(Slider.ValueProperty, offsetGreenMidBinding);

            Binding offsetBlueMidBinding = new Binding("BlueOffsetMid");
            offsetBlueMidBinding.Source = ImageParameters.Instance;
            sliderOffsetBlueMid.SetBinding(Slider.ValueProperty, offsetBlueMidBinding);

            Binding offsetRedHighBinding = new Binding("RedOffsetHigh");
            offsetRedHighBinding.Source = ImageParameters.Instance;
            sliderOffsetRedHigh.SetBinding(Slider.ValueProperty, offsetRedHighBinding);

            Binding offsetGreenHighBinding = new Binding("GreenOffsetHigh");
            offsetGreenHighBinding.Source = ImageParameters.Instance;
            sliderOffsetGreenHigh.SetBinding(Slider.ValueProperty, offsetGreenHighBinding);

            Binding offsetBlueHighBinding = new Binding("BlueOffsetHigh");
            offsetBlueHighBinding.Source = ImageParameters.Instance;
            sliderOffsetBlueHigh.SetBinding(Slider.ValueProperty, offsetBlueHighBinding);

            #endregion

            #region Gain

            //Low
            Binding gainTemperatureLowBinding = new Binding("TemperatureGainLow");
            gainTemperatureLowBinding.Source = ImageParameters.Instance;
            sliderGainTempLow.SetBinding(Slider.ValueProperty, gainTemperatureLowBinding);

            Binding gainMagentaLowBinding = new Binding("MagentaGainLow");
            gainMagentaLowBinding.Source = ImageParameters.Instance;
            sliderGainMagentaLow.SetBinding(Slider.ValueProperty, gainMagentaLowBinding);

            Binding gainOverallLowBinding = new Binding("OverallGainLow");
            gainOverallLowBinding.Source = ImageParameters.Instance;
            sliderGainOverallLow.SetBinding(Slider.ValueProperty, gainOverallLowBinding);

            Binding gainRedLowBinding = new Binding("RedGainLow");
            gainRedLowBinding.Source = ImageParameters.Instance;
            sliderGainRedLow.SetBinding(Slider.ValueProperty, gainRedLowBinding);

            Binding gainGreenLowBinding = new Binding("GreenGainLow");
            gainGreenLowBinding.Source = ImageParameters.Instance;
            sliderGainGreenLow.SetBinding(Slider.ValueProperty, gainGreenLowBinding);

            Binding gainBlueLowBinding = new Binding("BlueGainLow");
            gainBlueLowBinding.Source = ImageParameters.Instance;
            sliderGainBlueLow.SetBinding(Slider.ValueProperty, gainBlueLowBinding);

            //Mid
            Binding gainTemperatureMidBinding = new Binding("TemperatureGainMid");
            gainTemperatureMidBinding.Source = ImageParameters.Instance;
            sliderGainTempMid.SetBinding(Slider.ValueProperty, gainTemperatureMidBinding);

            Binding gainMagentaMidBinding = new Binding("MagentaGainMid");
            gainMagentaMidBinding.Source = ImageParameters.Instance;
            sliderGainMagentaMid.SetBinding(Slider.ValueProperty, gainMagentaMidBinding);

            Binding gainOverallMidBinding = new Binding("OverallGainMid");
            gainOverallMidBinding.Source = ImageParameters.Instance;
            sliderGainOverallMid.SetBinding(Slider.ValueProperty, gainOverallMidBinding);

            Binding gainRedMidBinding = new Binding("RedGainMid");
            gainRedMidBinding.Source = ImageParameters.Instance;
            sliderGainRedMid.SetBinding(Slider.ValueProperty, gainRedMidBinding);

            Binding gainGreenMidBinding = new Binding("GreenGainMid");
            gainGreenMidBinding.Source = ImageParameters.Instance;
            sliderGainGreenMid.SetBinding(Slider.ValueProperty, gainGreenMidBinding);

            Binding gainBlueMidBinding = new Binding("BlueGainMid");
            gainBlueMidBinding.Source = ImageParameters.Instance;
            sliderGainBlueMid.SetBinding(Slider.ValueProperty, gainBlueMidBinding);

            //High
            Binding gainTemperatureHighBinding = new Binding("TemperatureGainHigh");
            gainTemperatureHighBinding.Source = ImageParameters.Instance;
            sliderGainTempHigh.SetBinding(Slider.ValueProperty, gainTemperatureHighBinding);

            Binding gainMagentaHighBinding = new Binding("MagentaGainHigh");
            gainMagentaHighBinding.Source = ImageParameters.Instance;
            sliderGainMagentaHigh.SetBinding(Slider.ValueProperty, gainMagentaHighBinding);

            Binding gainOverallHighBinding = new Binding("OverallGainHigh");
            gainOverallHighBinding.Source = ImageParameters.Instance;
            sliderGainOverallHigh.SetBinding(Slider.ValueProperty, gainOverallHighBinding);

            Binding gainRedHighBinding = new Binding("RedGainHigh");
            gainRedHighBinding.Source = ImageParameters.Instance;
            sliderGainRedHigh.SetBinding(Slider.ValueProperty, gainRedHighBinding);

            Binding gainGreenHighBinding = new Binding("GreenGainHigh");
            gainGreenHighBinding.Source = ImageParameters.Instance;
            sliderGainGreenHigh.SetBinding(Slider.ValueProperty, gainGreenHighBinding);

            Binding gainBlueHighBinding = new Binding("BlueGainHigh");
            gainBlueHighBinding.Source = ImageParameters.Instance;
            sliderGainBlueHigh.SetBinding(Slider.ValueProperty, gainBlueHighBinding);

            #endregion

            #region Gamma

            //Low
            Binding gammaInRedBindingLow = new Binding("InRedLow");
            gammaInRedBindingLow.Source = ImageParameters.Instance;
            sliderGammaInRedLow.SetBinding(Slider.ValueProperty, gammaInRedBindingLow);

            Binding gammaInGreenBindingLow = new Binding("InGreenLow");
            gammaInGreenBindingLow.Source = ImageParameters.Instance;
            sliderGammaInGreenLow.SetBinding(Slider.ValueProperty, gammaInGreenBindingLow);

            Binding gammaInBlueBindingLow = new Binding("InBlueLow");
            gammaInBlueBindingLow.Source = ImageParameters.Instance;
            sliderGammaInBlueLow.SetBinding(Slider.ValueProperty, gammaInBlueBindingLow);

            Binding gammaOutRedBindingLow = new Binding("OutRedLow");
            gammaOutRedBindingLow.Source = ImageParameters.Instance;
            sliderGammaOutRedLow.SetBinding(Slider.ValueProperty, gammaOutRedBindingLow);

            Binding gammaOutGreenBindingLow = new Binding("OutGreenLow");
            gammaOutGreenBindingLow.Source = ImageParameters.Instance;
            sliderGammaOutGreenLow.SetBinding(Slider.ValueProperty, gammaOutGreenBindingLow);

            Binding gammaOutBlueBindingLow = new Binding("OutBlueLow");
            gammaOutBlueBindingLow.Source = ImageParameters.Instance;
            sliderGammaOutBlueLow.SetBinding(Slider.ValueProperty, gammaOutBlueBindingLow);

            //Mid
            Binding gammaInRedBindingMid = new Binding("InRedMid");
            gammaInRedBindingMid.Source = ImageParameters.Instance;
            sliderGammaInRedMid.SetBinding(Slider.ValueProperty, gammaInRedBindingMid);

            Binding gammaInGreenBindingMid = new Binding("InGreenMid");
            gammaInGreenBindingMid.Source = ImageParameters.Instance;
            sliderGammaInGreenMid.SetBinding(Slider.ValueProperty, gammaInGreenBindingMid);

            Binding gammaInBlueBindingMid = new Binding("InBlueMid");
            gammaInBlueBindingMid.Source = ImageParameters.Instance;
            sliderGammaInBlueMid.SetBinding(Slider.ValueProperty, gammaInBlueBindingMid);

            Binding gammaOutRedBindingMid = new Binding("OutRedMid");
            gammaOutRedBindingMid.Source = ImageParameters.Instance;
            sliderGammaOutRedMid.SetBinding(Slider.ValueProperty, gammaOutRedBindingMid);

            Binding gammaOutGreenBindingMid = new Binding("OutGreenMid");
            gammaOutGreenBindingMid.Source = ImageParameters.Instance;
            sliderGammaOutGreenMid.SetBinding(Slider.ValueProperty, gammaOutGreenBindingMid);

            Binding gammaOutBlueBindingMid = new Binding("OutBlueMid");
            gammaOutBlueBindingMid.Source = ImageParameters.Instance;
            sliderGammaOutBlueMid.SetBinding(Slider.ValueProperty, gammaOutBlueBindingMid);

            //High
            Binding gammaInRedBindingHigh = new Binding("InRedHigh");
            gammaInRedBindingHigh.Source = ImageParameters.Instance;
            sliderGammaInRedHigh.SetBinding(Slider.ValueProperty, gammaInRedBindingHigh);

            Binding gammaInGreenBindingHigh = new Binding("InGreenHigh");
            gammaInGreenBindingHigh.Source = ImageParameters.Instance;
            sliderGammaInGreenHigh.SetBinding(Slider.ValueProperty, gammaInGreenBindingHigh);

            Binding gammaInBlueBindingHigh = new Binding("InBlueHigh");
            gammaInBlueBindingHigh.Source = ImageParameters.Instance;
            sliderGammaInBlueHigh.SetBinding(Slider.ValueProperty, gammaInBlueBindingHigh);

            Binding gammaOutRedBindingHigh = new Binding("OutRedHigh");
            gammaOutRedBindingHigh.Source = ImageParameters.Instance;
            sliderGammaOutRedHigh.SetBinding(Slider.ValueProperty, gammaOutRedBindingHigh);

            Binding gammaOutGreenBindingHigh = new Binding("OutGreenHigh");
            gammaOutGreenBindingHigh.Source = ImageParameters.Instance;
            sliderGammaOutGreenHigh.SetBinding(Slider.ValueProperty, gammaOutGreenBindingHigh);

            Binding gammaOutBlueBindingHigh = new Binding("OutBlueHigh");
            gammaOutBlueBindingHigh.Source = ImageParameters.Instance;
            sliderGammaOutBlueHigh.SetBinding(Slider.ValueProperty, gammaOutBlueBindingHigh);

            #endregion

            #region Effects

            Binding curvatureBinding = new Binding("Curvature");
            curvatureBinding.Source = ImageParameters.Instance;
            sliderCurvature.SetBinding(Slider.ValueProperty, curvatureBinding);

            Binding glowRadiusBinding = new Binding("GlowRadius");
            glowRadiusBinding.Source = ImageParameters.Instance;
            sliderGlowRadius.SetBinding(Slider.ValueProperty, glowRadiusBinding);

            Binding glowBrightnessBinding = new Binding("GlowBrightness");
            glowBrightnessBinding.Source = ImageParameters.Instance;
            sliderGlowBrightness.SetBinding(Slider.ValueProperty, glowBrightnessBinding);

            Binding glowContrastBinding = new Binding("GlowContrast");
            glowContrastBinding.Source = ImageParameters.Instance;
            sliderGlowContrast.SetBinding(Slider.ValueProperty, glowContrastBinding);

            Binding inkOutlineBinding = new Binding("InkOutline");
            inkOutlineBinding.Source = ImageParameters.Instance;
            sliderInkOutline.SetBinding(Slider.ValueProperty, inkOutlineBinding);

            Binding coloringBinding = new Binding("Coloring");
            coloringBinding.Source = ImageParameters.Instance;
            sliderColoring.SetBinding(Slider.ValueProperty, coloringBinding);

            Binding brushSizeBinding = new Binding("BrushSize");
            brushSizeBinding.Source = ImageParameters.Instance;
            sliderBrushSize.SetBinding(Slider.ValueProperty, brushSizeBinding);

            Binding coarsenessBinding = new Binding("Coarseness");
            coarsenessBinding.Source = ImageParameters.Instance;
            sliderCoarseness.SetBinding(Slider.ValueProperty, coarsenessBinding);

            Binding softnessBinding = new Binding("Softness");
            softnessBinding.Source = ImageParameters.Instance;
            sliderSoftness.SetBinding(Slider.ValueProperty, softnessBinding);

            Binding lightingBinding = new Binding("Lighting");
            lightingBinding.Source = ImageParameters.Instance;
            sliderLighting.SetBinding(Slider.ValueProperty, lightingBinding);

            Binding warmthBinding = new Binding("Warmth");
            warmthBinding.Source = ImageParameters.Instance;
            sliderWarmth.SetBinding(Slider.ValueProperty, warmthBinding);

            #endregion
        }

        private void CreateTabDictionary()
        {
            tabPageDictionary = new Dictionary<String, int>();

            tabPageDictionary.Add("lblAppearance", 0);
            tabPageDictionary.Add("lblDetail", 1);

            tabPageDictionary.Add("lblEnhLow", 2);
            tabPageDictionary.Add("lblEnhMid", 3);
            tabPageDictionary.Add("lblEnhHigh", 4);

            tabPageDictionary.Add("lblOffsetLow", 5);
            tabPageDictionary.Add("lblOffsetMid", 6);
            tabPageDictionary.Add("lblOffsetHigh", 7);

            tabPageDictionary.Add("lblGainLow", 8);
            tabPageDictionary.Add("lblGainMid", 9);
            tabPageDictionary.Add("lblGainHigh", 10);

            tabPageDictionary.Add("lblGammaLow", 11);
            tabPageDictionary.Add("lblGammaMid", 12);
            tabPageDictionary.Add("lblGammaHigh", 13);

            tabPageDictionary.Add("lblImage", 14);
            tabPageDictionary.Add("lblFisheye", 15);
            tabPageDictionary.Add("lblGlow", 16);
            tabPageDictionary.Add("lblSketch", 17);
            tabPageDictionary.Add("lblOilPainting", 18);
            tabPageDictionary.Add("lblPortrait", 19);
        }

        private void CreateSliderLabelDictionary()
        {
            sliderLabelDictionary = new Dictionary<Slider, Label>();

            sliderLabelDictionary.Add(sliderGlowBrightness, lblGlow);
            sliderLabelDictionary.Add(sliderGlowContrast, lblGlow);
            sliderLabelDictionary.Add(sliderGlowRadius, lblGlow);

            sliderLabelDictionary.Add(sliderBrushSize, lblOilPainting);
            sliderLabelDictionary.Add(sliderCoarseness, lblOilPainting);

            sliderLabelDictionary.Add(sliderInkOutline, lblSketch);
            sliderLabelDictionary.Add(sliderColoring, lblSketch);

            sliderLabelDictionary.Add(sliderCurvature, lblFisheye);

            sliderLabelDictionary.Add(sliderLighting, lblPortrait);
            sliderLabelDictionary.Add(sliderWarmth, lblPortrait);
            sliderLabelDictionary.Add(sliderSoftness, lblPortrait);
        }

        public void FilterEffects()
        {
            if (ImageParameters.Instance.EffectImage != null)
            {
                try
                {
                    BitmapSource b = null;
                    int[] sourceImagePixels = null;
                    int sourceWidth = -1;
                    int sourceHeight = -1;

                    Dispatcher.Invoke(DispatcherPriority.Normal,
                        new DispatcherOperationCallback(delegate(object arg)
                        {
                            BitmapSource bitmap = ImageParameters.Instance.EffectImage;

                            sourceWidth = bitmap.PixelWidth;
                            sourceHeight = bitmap.PixelHeight;

                            sourceImagePixels = new int[sourceWidth * sourceHeight * 4];
                            bitmap.CopyPixels(sourceImagePixels, sourceWidth * 4, 0);

                            return null;
                        }
                    ), null);

                    if (sourceWidth < 0 || sourceHeight < 0 || sourceImagePixels == null)
                    {
                        return;
                    }

                    //Clone so that this thread owns the image
                    b = BitmapSource.Create(sourceWidth, sourceHeight, 96, 96, PixelFormats.Bgra32,
                        null, sourceImagePixels, sourceWidth * 4);

                    /*
                     * Sequence of applying effects and operations to the image is as follows:
                     * 
                     * Crop
                     * Rotate
                     * Flip
                     * Adjustment Filters
                     * Advanced Adjustment Filter
                     * Effects Filters
                     * */

                    if (cropData != Rect.Empty)
                    {
                        // Create a CroppedBitmap based off of a xaml defined resource.
                        b = new CroppedBitmap(b, new Int32Rect(
                            (int)(cropData.X),
                            (int)(cropData.Y),
                            (int)(cropData.Width),
                            (int)(cropData.Height)));
                    }

                    if (ImageParameters.Instance.Rotate != Rotation.Rotate0)
                    {
                        TransformedBitmap rotate = new TransformedBitmap();
                        rotate.BeginInit();

                        double rotationAngle = 0.0;

                        switch (ImageParameters.Instance.Rotate)
                        {
                            case Rotation.Rotate180: rotationAngle = 180.0;
                                break;
                            case Rotation.Rotate270: rotationAngle = 270.0;
                                break;
                            case Rotation.Rotate90: rotationAngle = 90.0;
                                break;
                        }

                        rotate.Source = b;
                        rotate.Transform = new RotateTransform(rotationAngle);
                        rotate.EndInit();

                        int[] pixels = new int[rotate.PixelWidth * rotate.PixelHeight];
                        rotate.CopyPixels(pixels, rotate.PixelWidth * 4, 0);

                        b = BitmapSource.Create(rotate.PixelWidth, rotate.PixelHeight,
                            96, 96, PixelFormats.Bgr32, null, pixels, rotate.PixelWidth * 4);
                    }

                    if (ImageParameters.Instance.Flip != FlipType.None)
                    {
                        TransformedBitmap flip = new TransformedBitmap();
                        flip.BeginInit();

                        double scaleX = 1.0;
                        double scaleY = 1.0;

                        switch (ImageParameters.Instance.Flip)
                        {
                            case FlipType.Horizontal:
                                scaleX = -1.0; break;
                            case FlipType.Vertical:
                                scaleY = -1.0; break;
                            case FlipType.Both:
                                scaleX = scaleY = -1.0; break;
                        }

                        flip.Source = b;
                        flip.Transform = new ScaleTransform(scaleX, scaleY);
                        flip.EndInit();

                        int[] pixels = new int[flip.PixelWidth * flip.PixelHeight];
                        flip.CopyPixels(pixels, flip.PixelWidth * 4, 0);

                        b = BitmapSource.Create(flip.PixelWidth, flip.PixelHeight,
                            96, 96, PixelFormats.Bgr32, null, pixels, flip.PixelWidth * 4);
                    }

                    //Run the basic adjustments
                    b = ImageParameters.Instance.AdjustmentFilter.ExecuteFilter(b);

                    //Do the advanced adjustments
                    //Only filter if the filter parameters have been set
                    if (App.DoFilter)
                    {
                        b = AdvancedAdjustments.Instance.Do(b);
                    }

                    //Apply any effects filters
                    foreach (EffectFilter filter in ImageParameters.Instance.EffectFilters)
                    {
                        b = filter.ExecuteFilter(b);
                    }

                    //It is necessary to pass the pixels raw between threads
                    // to avoid cross thread calls to wpf objects
                    int[] p = new int[b.PixelWidth * b.PixelHeight];
                    b.CopyPixels(p, b.PixelWidth * 4, 0);

                    int width = b.PixelWidth;
                    int height = b.PixelHeight;
                    PixelFormat format = b.Format;

                    Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                       new DispatcherOperationCallback(delegate(object arg)
                       {
                           image2.Source = BitmapSource.Create(width, height, 96, 96, format,
                               null, p, width * 4);
                           return null;
                       }
                   ), null);
                }
                catch (Exception e)
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        new DispatcherOperationCallback(delegate(object arg)
                        {
                            image2.Source = (BitmapSource)image1.Source.Clone();
                            return null;
                        }
                    ), null);

                    GC.Collect();
                }
            }

            GC.Collect();
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Cursor != Cursors.Wait)
            {
                Cursor = Cursors.Hand;
            }
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Cursor != Cursors.Wait)
            {
                Cursor = Cursors.Arrow;
            }
        }

        private Thread worker;
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (setup)
            {
                modified = true;
            }

            if (!working && !SenderIsEffectSlider(sender))
            {
                worker = new Thread(new ThreadStart(delegate()
                    {
                        Monitor.Enter(ImageParameters.Instance.ObjectLock);

                        try
                        {
                            working = true;
                            FilterEffects();
                        }
                        finally
                        {
                            working = false;
                            Monitor.Exit(ImageParameters.Instance.ObjectLock);
                        }                        
                    }
                ));

                worker.Priority = ThreadPriority.BelowNormal;
                worker.SetApartmentState(ApartmentState.STA);
                worker.Start();                
            }
        }

        private bool SenderIsEffectSlider(object sender)
        {
            try
            {
                TabItem t = ((TabItem)((Grid)((Slider)sender).Parent).Parent);
                return (t.Name.StartsWith("effect"));
            }
            catch
            {
                return false;
            }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {                        
            if(e.Source is Label && setup)
            {
                Label l = (Label)e.Source;

                if (l.Parent is TreeViewItem)
                {
                    TreeViewItem parent = (TreeViewItem)l.Parent;

                    if ((String)parent.Header == "Filters")
                    {
                        EffectFilterLabelClick(l, parent);
                    }
                    else
                    {
                        if (tabPageDictionary.ContainsKey(l.Name))
                        {
                            tabControl1.SelectedIndex = tabPageDictionary[l.Name];
                        }

                        l.Foreground = Brushes.DeepSkyBlue;

                        foreach (TreeViewItem item in treeView1.Items)
                        {
                            foreach (object o in item.Items)
                            {
                                if (o is Label && o != l && o != lblGrayScale)
                                {
                                    ((Label)o).Foreground = Brushes.White;
                                }

                                if (o is TreeViewItem)
                                {
                                    TreeViewItem item2 = (TreeViewItem)o;
                                    foreach (object o2 in item2.Items)
                                    {
                                        if (o2 is Label && o2 != l && o2 != lblGrayScale)
                                        {
                                            Label l2 = (Label)o2;

                                            if (l2.Tag != null)
                                            {
                                                if ((String)l2.Tag == "False")
                                                {
                                                    l2.Foreground = Brushes.White;
                                                }
                                                else
                                                {
                                                    l2.Foreground = Brushes.Lime;
                                                }
                                            } else
                                            {
                                                l2.Foreground = Brushes.White;
                                            }
                                        }
                                    }                                
                                }
                            }
                        }
                    }
                }
            }            
        }

        private void EffectFilterLabelClick(Label l, TreeViewItem parent)
        {
            modified = true;
            Cursor = Cursors.Wait;

            foreach (object item in parent.Items)
            {
                if (item is Label && item != l && item != lblGrayScale)
                {
                    ((Label)item).Foreground = Brushes.White;
                    ((Label)item).Tag = "False";
                }
            }

            if ((String)l.Tag == "False")
            {
                if (tabPageDictionary.ContainsKey(l.Name))
                {
                    tabControl1.SelectedIndex = tabPageDictionary[l.Name];
                }

                l.Foreground = Brushes.DeepSkyBlue;
                l.Tag = "True";

                AddSpecialEffect(l.Content as String, true);

                foreach (TreeViewItem item in treeView1.Items)
                {
                    foreach (object o in item.Items)
                    {
                        if (o is Label && o != l && o != lblGrayScale)
                        {
                            ((Label)o).Foreground = Brushes.White;
                        }

                        if (o is TreeViewItem)
                        {
                            TreeViewItem item2 = (TreeViewItem)o;

                            if ((String)item.Header != "Filters")
                            {
                                foreach (object o2 in item2.Items)
                                {
                                    if (o2 is Label && o2 != l && o2 != lblGrayScale)
                                    {
                                        Label l2 = (Label)o2;
                                        l2.Foreground = Brushes.White;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                l.Foreground = Brushes.White;
                l.Tag = "False";

                tabControl1.SelectedIndex = tabControl1.Items.Count - 1;

                AddSpecialEffect(null, true);
            }
        }

        private void AddSpecialEffect(String name, bool interrupt)
        {
            Cursor = Cursors.Wait;

            if (name == null)
            {
                ImageParameters.Instance.SetSpecialEffect(null, interrupt);
            }
            else if (name == lblGlow.Content as String)
            {
                ImageParameters.Instance.SetSpecialEffect(new GlowEffect(
                    ImageParameters.Instance.GlowRadius,
                    ImageParameters.Instance.GlowBrightness,
                    ImageParameters.Instance.GlowContrast), interrupt);
            }
            else if (name == lblSketch.Content as String)
            {
                ImageParameters.Instance.SetSpecialEffect(new InkSketch(
                    ImageParameters.Instance.InkOutline,
                    ImageParameters.Instance.Coloring), interrupt);
            }
            else if (name == lblOilPainting.Content as String)
            {
                ImageParameters.Instance.SetSpecialEffect(new OilPainting(
                    ImageParameters.Instance.BrushSize,
                    ImageParameters.Instance.Coarseness), interrupt);
            }
            else if (name == lblPortrait.Content as String)
            {
                ImageParameters.Instance.SetSpecialEffect(new PortraitEffect(
                    ImageParameters.Instance.Softness,
                    ImageParameters.Instance.Lighting,
                    ImageParameters.Instance.Warmth), interrupt);
            }
            else if (name == lblFisheye.Content as String)
            {
                ImageParameters.Instance.SetSpecialEffect(new FisheyeFilter(
                    ImageParameters.Instance.Curvature), interrupt);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button)
            {
                Button b = (Button)e.OriginalSource;

                String sliderName = b.Tag as String;

                if (sliderName != null)
                {
                    try
                    {
                        //
                        //This looks up the slider by it's field name
                        Type classType = this.GetType();
                        FieldInfo info = classType.GetField(sliderName, 
                            BindingFlags.NonPublic | BindingFlags.Instance);
                        object o = info.GetValue(this);

                        if (o is Slider)
                        {
                            Slider slider = (Slider)o;

                            slider.Value = Double.Parse((String)slider.Tag);

                            if (sliderLabelDictionary.ContainsKey(slider))
                            {
                                AddSpecialEffect(sliderLabelDictionary[slider].Content as String, true);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //This shouldn't happen unless something goes wrong on WPF's end
                    }
                }


                //Now that we've reset the value, run a full filter effect with wait cycle
                while (worker != null && worker.IsAlive)
                {
                    worker.Abort();
                }

                Cursor = Cursors.Wait;
                working = true;
                FilterEffects();
                working = false;
                Cursor = Cursors.Arrow;
            }
        }

        private void image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Send,
                new DispatcherOperationCallback(delegate(object p)
            {
                if (crop)
                {
                    cropPivot = e.GetPosition(image1);

                    Image source = e.Source as Image;

                    if (source == null)
                        return null;

                    //The difference between rendered and in-image coordinates
                    double widthFactor = source.Source.Width / source.ActualWidth;
                    double heightFactor = source.Source.Height / source.ActualHeight;

                    cropPivot.X *= widthFactor;
                    cropPivot.Y *= heightFactor;
                }
                else if (e.ClickCount == 2)
                {                    
                    if (Grid.GetColumnSpan(image1) == 2)
                    {
                        image2.Visibility = Visibility.Visible;
                        splitter.Visibility = Visibility.Visible;
                        Grid.SetColumnSpan(image1, 1);
                        Grid.SetColumnSpan(image1_Surface, 1);
                    }
                    else
                    {
                        image2.Visibility = Visibility.Hidden;
                        splitter.Visibility = Visibility.Hidden;
                        Grid.SetColumnSpan(image1, 2);
                        Grid.SetColumnSpan(image1_Surface, 2);
                    }
                }
            
                return null;
            }), null);
        }

        private void image2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Send,
                new DispatcherOperationCallback(delegate(object p)
                {
                    if (e.ClickCount == 2)
                    {                        
                        if (Grid.GetColumnSpan(image2) == 2)
                        {
                            image1.Visibility = Visibility.Visible;
                            splitter.Visibility = Visibility.Visible;
                            Grid.SetColumnSpan(image2, 1);
                            Grid.SetColumn(image2, 2);
                        }
                        else
                        {
                            image1.Visibility = Visibility.Hidden;
                            splitter.Visibility = Visibility.Hidden;
                            Grid.SetColumnSpan(image2, 2);
                            Grid.SetColumn(image2, 1);
                        }
                    }

                    return null;
                }), null);
        }

        private void image1_MouseMove(object sender, MouseEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Send,
                new DispatcherOperationCallback(delegate(object p)
                {
                    if (crop && e.LeftButton == MouseButtonState.Pressed)
                    {
                        Point newPoint = e.GetPosition(image1);

                        DrawingGroup d = new DrawingGroup();
                        DrawingContext ctxt = d.Open();

                        Image source = e.Source as Image;

                        if (source == null)
                            return null;

                        //The difference between rendered and in-image coordinates
                        double widthFactor = source.Source.Width / source.ActualWidth;
                        double heightFactor = source.Source.Height / source.ActualHeight;

                        newPoint.X *= widthFactor;
                        newPoint.Y *= heightFactor;

                        tempCropData = new Rect(newPoint, cropPivot);

                        if (tempCropData.Left < 0 ||
                           tempCropData.Right > source.Source.Width ||
                           tempCropData.Top < 0 ||
                           tempCropData.Bottom > source.Source.Height)
                        {
                            return null;
                        }

                        Geometry geom1 = new RectangleGeometry(
                            new Rect(0, 0, source.Source.Width, source.Source.Height));
                        Geometry geom2 = new RectangleGeometry(tempCropData);

                        CombinedGeometry geom = new CombinedGeometry(geom1, geom2);
                        geom.GeometryCombineMode = GeometryCombineMode.Exclude;

                        Color fadedColor = Color.FromArgb(200, 0, 0, 0);
                        Pen dotted = new Pen(Brushes.Red, 1.5);

                        dotted.DashStyle = new DashStyle(new double[] { 4.0, 4.0 }, 0);

                        ctxt.DrawGeometry(new SolidColorBrush(fadedColor), null, geom);

                        //If the rect will cause the surface to resize, make it fit
                        Rect smallerRect = new Rect(tempCropData.Location, tempCropData.Size);

                        if (tempCropData.Left <= 2)
                        {
                            smallerRect.X = 2;
                        }

                        if (tempCropData.Right >= (source.Source.Width - 2))
                        {
                            smallerRect.Width = source.Source.Width - 2 - smallerRect.Left;
                        }

                        ctxt.DrawRectangle(null, dotted, smallerRect);

                        String str = Math.Round(tempCropData.Width).ToString(); ;
                        str += " , ";
                        str += Math.Round(tempCropData.Height).ToString();

                        FormattedText text = new FormattedText(str,
                            CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                            new Typeface("Arial"), 32, Brushes.White);

                        Point textOrigin = tempCropData.BottomRight;

                        textOrigin.X -= 150;
                        textOrigin.Y += 5;

                        bool drawText = true;

                        if (textOrigin.Y + text.Height > source.Source.Height)
                        {
                            textOrigin.Y = tempCropData.Top - text.Height - 5;

                            if (textOrigin.Y < 0)
                            {
                                drawText = false;
                            }
                        }

                        if (textOrigin.X < 0)
                        {
                            textOrigin.X = 0;
                        }

                        if (drawText)
                        {
                            ctxt.DrawText(text, textOrigin);
                        }

                        ctxt.Close();

                        image1_Surface.Source = new DrawingImage(d);
                    }

                    return null;
                }), null);
        }

        private void button_CropAccept_Click(object sender, RoutedEventArgs e)
        {
            ImageParameters.Instance.CropData = tempCropData;
            cropData = tempCropData;

            image1_Surface.Source = null;
            button_CropToggle.IsChecked = false;

            FilterEffects();
        }

        private void buttonRotateCCW_Click(object sender, RoutedEventArgs e)
        {
            switch (ImageParameters.Instance.Rotate)
            {
                case Rotation.Rotate0:
                    ImageParameters.Instance.Rotate = Rotation.Rotate90; break;
                case Rotation.Rotate90:
                    ImageParameters.Instance.Rotate = Rotation.Rotate180; break;
                case Rotation.Rotate180:
                    ImageParameters.Instance.Rotate = Rotation.Rotate270; break;
                case Rotation.Rotate270:
                    ImageParameters.Instance.Rotate = Rotation.Rotate0; break;
            }

            FilterEffects();
        }

        private void buttonRotateCW_Click(object sender, RoutedEventArgs e)
        {
            switch (ImageParameters.Instance.Rotate)
            {
                case Rotation.Rotate0:
                    ImageParameters.Instance.Rotate = Rotation.Rotate270; break;
                case Rotation.Rotate90:
                    ImageParameters.Instance.Rotate = Rotation.Rotate0; break;
                case Rotation.Rotate180:
                    ImageParameters.Instance.Rotate = Rotation.Rotate90; break;
                case Rotation.Rotate270:
                    ImageParameters.Instance.Rotate = Rotation.Rotate180; break;
            }

            FilterEffects();
        }

        private void buttonFlipVertical_Click(object sender, RoutedEventArgs e)
        {
            switch (ImageParameters.Instance.Flip)
            {
                case FlipType.Horizontal:
                    ImageParameters.Instance.Flip = FlipType.Both; break;
                case FlipType.Vertical:
                    ImageParameters.Instance.Flip = FlipType.None; break;
                case FlipType.Both:
                    ImageParameters.Instance.Flip = FlipType.Horizontal; break;
                case FlipType.None:
                    ImageParameters.Instance.Flip = FlipType.Vertical; break;
            }

            FilterEffects();
        }

        private void buttonFlipHorizontal_Click(object sender, RoutedEventArgs e)
        {
            switch (ImageParameters.Instance.Flip)
            {
                case FlipType.Horizontal:
                    ImageParameters.Instance.Flip = FlipType.None; break;      
                case FlipType.Vertical:
                    ImageParameters.Instance.Flip = FlipType.Both; break;
                case FlipType.Both:
                    ImageParameters.Instance.Flip = FlipType.Vertical; break;
                case FlipType.None:
                    ImageParameters.Instance.Flip = FlipType.Horizontal; break;
            }

            FilterEffects();
        }

        private void slider_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!(e.OriginalSource is Button))
            {
                if (SenderIsEffectSlider(sender))
                {
                    if (sender is Slider)
                    {
                        ((Slider)sender).Value = ((Slider)sender).Value;
                        AddSpecialEffect(sliderLabelDictionary[((Slider)sender)].Content as String, true);
                    }
                }
                else
                {
                    if (worker != null)
                    {
                        worker.Abort();
                    }

                    lock (ImageParameters.Instance.ObjectLock)
                    {
                        ((Slider)sender).Value = ((Slider)sender).Value;
                    }

                    Cursor = Cursors.Wait;
                    working = true;
                    FilterEffects();
                    working = false;
                    Cursor = Cursors.Arrow;
                }
            }
        }

        //Loading loadingDialog;
        BackgroundWorker pageLoadWorker;
        System.Net.CookieContainer c = new System.Net.CookieContainer();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            NameValueCollection params_BIH = GetQueryStringParameters(BrowserInteropHelper.Source);

            try
            {
                lastActivity = params_BIH["lastActivity"];
                
            }catch{}

            //loadingDialog = new Loading();
            //loadingDialog.Show();

            pageLoadWorker = new BackgroundWorker();
            pageLoadWorker.DoWork += new DoWorkEventHandler(pageLoadWorker_DoWork);
            pageLoadWorker.ProgressChanged += new ProgressChangedEventHandler(new ProgressChangedEventHandler(
                delegate(object o, ProgressChangedEventArgs ev)
                {
                    //loadingDialog.progressBar1.Value = ev.ProgressPercentage;
                }));
            pageLoadWorker.WorkerReportsProgress = true;

            pageLoadWorker.RunWorkerAsync();
        }

        //Extract URL parameters from given Uri
        private NameValueCollection GetQueryStringParameters(Uri launchUri)
        {
            NameValueCollection nameValueTable = HttpUtility.ParseQueryString(launchUri.Query);
            return nameValueTable;
        }

        void pageLoadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Send, new ThreadStart(
                delegate()
                {
                    theGrid.Opacity = 0.1;

                    Cursor = Cursors.Wait;
                }));

            pageLoadWorker.ReportProgress(10);
            client = new SnapUpServiceSoapClient();

            string []userPass = new string[]{"hash","passhash"};//client.GetMember(lastActivity);
            
            Nickname = userPass[0];
            Password = userPass[1];

            MessageBox.Show(Nickname + " " + Password);

            pageLoadWorker.ReportProgress(25);
            Collections = client.GetCollections(Nickname, Password);
            pageLoadWorker.ReportProgress(65);

            Dispatcher.Invoke(DispatcherPriority.Send, new ThreadStart(
                delegate()
                {
                    foreach (PhotoCollectionItem col in Collections)
                        listBox2.Items.Add(col.Name);
                }));    

            pageLoadWorker.ReportProgress(95);

            Dispatcher.Invoke(DispatcherPriority.Send, new ThreadStart(
                delegate()
                {
                    theGrid.Opacity = 1;
                    Cursor = Cursors.Arrow;
                    //loadingDialog.Close();
                }));          
        }

        private void lblAutoAppearance_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (setup)
            {
                Cursor = Cursors.Wait;

                double brightnessAdj = Math.Exp((0x80 - brightnessHistogram.Mean) / 0x80) * 0x10;
                double contrastAdj = Math.Exp((0x80 - brightnessHistogram.Mean) / 0x80);

                sliderBrightness.Value = brightnessAdj / 256.0;
                sliderContrast.Value = contrastAdj / 256.0;

                FilterEffects();

                Cursor = Cursors.Arrow;
            }
        }

        private void lblGrayScale_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Cursor = Cursors.Wait;

            EffectFilter filter = ImageParameters.Instance.EffectFilters.Find(
                new Predicate<EffectFilter>(delegate(EffectFilter target)
                {
                    return target is GrayScaleFilter;
                }));

            if (filter == null)
            {
                lblGrayScale.Foreground = Brushes.Lime;
                ImageParameters.Instance.EffectFilters.Add(new GrayScaleFilter());
            }
            else
            {
                lblGrayScale.Foreground = Brushes.White;
                ImageParameters.Instance.EffectFilters.Remove(filter);
            }

            FilterEffects();

            Cursor = Cursors.Arrow;
        }

        private void listBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Cursor = Cursors.Wait;
            listBox1.Items.Clear();

            Images = client.GetPhotosByCollection(
                Nickname,
                Password,
                Collections[listBox2.SelectedIndex].WebPhotoCollectionID);            

            foreach (PhotoItem photo in Images)
            {                
                listBox1.Items.Add(new BitmapImage(new Uri(photo.ThumbnailURL)));
            }
            Cursor = Cursors.Arrow;
        }
        
        private void listBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (selectedImageId != Images[listBox1.SelectedIndex].WebPhotoID)
            {
                selectedImageId = Images[listBox1.SelectedIndex].WebPhotoID;                
                MessageBoxResult res = MessageBoxResult.OK;
                if (modified)
                {
                    res = MessageBox.Show("Warning: This will discard any changes you have made to the current image.",
                        "Warning", MessageBoxButton.OKCancel);
                }

                if (res == MessageBoxResult.OK)
                {
                    //loadingDialog = new Loading();
                    //loadingDialog.Show();

                    BitmapSource newSource = new BitmapImage(new Uri(Images[listBox1.SelectedIndex].MainPhotoURL));

                    newSource.DownloadProgress += new EventHandler<DownloadProgressEventArgs>(newSource_DownloadProgress);
                    newSource.DownloadCompleted += new EventHandler(newSource_DownloadCompleted);

                    if (!newSource.IsDownloading)
                    {
                        //loadingDialog.Close();
                    }
                    
                    Reset(newSource);
                }
            }
        }

        void newSource_DownloadCompleted(object sender, EventArgs e)
        {            
            //loadingDialog.Close();
        }

        void newSource_DownloadProgress(object sender, DownloadProgressEventArgs e)
        {
            //loadingDialog.progressBar1.Value = e.Progress;
        }

        private void Reset(BitmapSource newSource)
        {            
            modified = false;

            ImageParameters.ResetInstance();
            ImageParameters.Instance.SourceImage = newSource;

            image1.Source = ImageParameters.Instance.SourceImage;                   

            SetUp();            

            tabControl1.SelectedIndex = tabControl1.Items.Count - 1;

            lblFisheye.Tag = "False";
            lblSketch.Tag = "False";
            lblOilPainting.Tag = "False";
            lblGlow.Tag = "False";
            lblPortrait.Tag = "False";

            cropData = Rect.Empty;

            ResetLabels(treeView1);            
        }

        private void ResetLabels(ItemsControl control)
        {
            foreach (Object o in control.Items)
            {
                if (o is Label)
                {
                    ((Label)o).Foreground = Brushes.White;
                }
                else if (o is ItemsControl)
                {
                    if (o is TreeViewItem)
                    {
                        ((TreeViewItem)o).Foreground = Brushes.White;
                    }

                    ResetLabels((ItemsControl)o);
                }
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBoxResult.Yes;

            if (modified)
            {
                modified = true;
                res = MessageBox.Show("Are you sure you want to keep these changes?",
                    "Save Changes?", MessageBoxButton.YesNo);
            }

            if (res == MessageBoxResult.Yes)
            {               
                Cursor = Cursors.Wait;
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(ImageParameters.Instance.EffectImage));

                MemoryStream stream = new MemoryStream();
                encoder.Save(stream);

                client.DeletePhoto(Nickname, Password, Images[listBox1.SelectedIndex].WebPhotoID);
                client.UploadPhoto(Nickname, Password, Collections[listBox2.SelectedIndex].WebPhotoCollectionID, stream.GetBuffer(), DateTime.Now);

                listBox2_SelectionChanged(sender, null);

                Cursor = Cursors.Arrow;
            }
        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBoxResult.Yes;

            if (modified)
            {
                modified = true;
                res = MessageBox.Show("Are you sure you want to discard these changes?",
                    "Reset", MessageBoxButton.YesNo);
            }

            if (res == MessageBoxResult.Yes)
            {
                Reset(image1.Source as BitmapSource);
            }
        }
    }

    /// <summary>
    /// Converter for the custom slider template to convert from double to String
    /// </summary>
    public class SliderValueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                Double doubleValue = (Double)value;

                return doubleValue.ToString("0.###");                
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
