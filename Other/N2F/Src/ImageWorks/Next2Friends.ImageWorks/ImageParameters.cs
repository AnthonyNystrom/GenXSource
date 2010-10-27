using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Next2Friends.ImageWorks.AdjustmentFilters;
using Next2Friends.ImageWorks.EffectsFilters;
using Next2Friends.ImageWorks.UI.NuGenImageWorks;

namespace Next2Friends.ImageWorks
{
    public enum FlipType
    {
        None,
        Horizontal,
        Vertical,
        Both
    }

    public class ImageParameters
    {

        private ImageParameters() { ObjectLock = ""; }

        public Object ObjectLock {get; set;}
        public Dispatcher Dispatcher { get; set; }
        public event ImageChangedDelegate ImageChanged;
        public delegate void ImageChangedDelegate();
        private Thread worker;

        //Applies the current special effect and stores it
        private void ApplyEffect()
        {
            if (SpecialEffect == null)
            {
                EffectImage = null;
                return;
            }
            BitmapSource b = null;
            int[] sourceImagePixels = null;
            int sourceWidth = -1;
            int sourceHeight = -1;

            Dispatcher.Invoke(DispatcherPriority.Normal,
                new DispatcherOperationCallback(delegate(object arg)
                {
                    BitmapSource bitmap = SourceImage;

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

            b = SpecialEffect.ExecuteFilter(b);

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
                   EffectImage = BitmapSource.Create(width, height, 96, 96, format,
                      null, p, width * 4);
                   return null;
               }
           ), null);
        }

        private AdjustmentFilter adjustmentFilter = new AdjustmentFilter();
        public AdjustmentFilter AdjustmentFilter
        {
            get
            {

                return adjustmentFilter;
            }           
        }

        private List<EffectFilter> effectFilters = new List<EffectFilter>();
        public List<EffectFilter> EffectFilters
        {
            get
            {

                return effectFilters;
            }
        }

        private bool working = false;
        private EffectFilter specialEffect;
        private EffectFilter SpecialEffect
        {
            get
            {
                return specialEffect;
            }

            set
            {
                specialEffect = value;
            }
        }

        //Sets the effect and applies it, allows for asynchronous or sychronous execution
        public void SetSpecialEffect(EffectFilter filter, bool interrupt)
        {
            specialEffect = filter;

            if (interrupt)
            {
                if (worker != null)
                {
                    worker.Abort();

                    while (worker.IsAlive)
                    {
                        Thread.Sleep(10);
                    }
                }

                working = true;
                ApplyEffect();
                ImageChanged();
                working = false;
            }
            else
            {
                if (!working)
                {
                    working = true;
                    worker = new Thread(new ThreadStart(delegate()
                    {
                        ApplyEffect();

                        Dispatcher.Invoke(DispatcherPriority.Normal,
                            new DispatcherOperationCallback(delegate(object arg)
                            {
                                ImageChanged();
                                return null;
                            }
                        ), null);

                        working = false;
                    }));
                    worker.SetApartmentState(ApartmentState.STA);
                    worker.Start();
                }
            }
        }

        //Reset the instance, preserve critical data
        public static void ResetInstance()
        {
            m_Instance = new ImageParameters();
        }

        //Singleton Properties
        private static ImageParameters m_Instance;
        public static ImageParameters Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new ImageParameters();
                }

                return m_Instance;
            }
        }

        private BitmapSource sourceImage;
        public BitmapSource SourceImage
        {
            get
            {
                return sourceImage;
            }
            set
            {
                sourceImage = value;
            }
        }

        private BitmapSource effectImage;
        public BitmapSource EffectImage
        {
            get
            {
                if (effectImage == null)
                {
                    return sourceImage;
                }
                else
                {
                    return effectImage;
                }
            }

            set
            {
                effectImage = value;
                ImageChanged();
            }
        }

        #region Adjustment Filter Properties

        //Appearance
        private double brightness = 0.0;
        public double Brightness {
            get
            {
                return brightness;
            }

            set
            {
                if (Monitor.TryEnter(ObjectLock, 10))
                {
                    try
                    {
                        brightness = value;

                        foreach (PixelAdjustment filter in adjustmentFilter.PixelAdjustments)
                        {
                            if (filter is BrightnessContrastFilter)
                            {
                                ((BrightnessContrastFilter)filter).Brightness = (int)(255 * brightness);
                                return;
                            }
                        }

                        adjustmentFilter.PixelAdjustments.Add(new BrightnessContrastFilter((int)(255 * brightness), (int)(100 * contrast)));
                    }
                    finally
                    {
                        Monitor.Exit(ObjectLock);
                    }
                }
            }
        }

        private double contrast = 0.0;
        public double Contrast
        {
            get
            {
                return contrast;
            }

            set
            {
                if (Monitor.TryEnter(ObjectLock, 10))
                {
                    try
                    {
                        contrast = value;

                        foreach (PixelAdjustment filter in adjustmentFilter.PixelAdjustments)
                        {
                            if (filter is BrightnessContrastFilter)
                            {
                                ((BrightnessContrastFilter)filter).Contrast = (int)(100 * contrast);
                                return;
                            }
                        }

                        adjustmentFilter.PixelAdjustments.Add(new BrightnessContrastFilter((int)(255 * brightness), (int)(100 * contrast)));
                    }
                    finally
                    {
                        Monitor.Exit(ObjectLock);
                    }
                }
            }
        }

        private double temperature = 0.0;
        public double Temperature
        {
            get
            {
                return temperature;
            }

            set
            {
                if (Monitor.TryEnter(ObjectLock, 10))
                {
                    try
                    {
                        temperature = value;

                        if (temperature == 0)
                        {
                            lock (ImageParameters.Instance)
                            {
                                adjustmentFilter.PixelAdjustments.RemoveAll(new Predicate<PixelAdjustment>(
                                    delegate(PixelAdjustment target)
                                    {
                                        return target is TemperatureFilter;
                                    }));
                            }
                        }
                        else
                        {
                            foreach (PixelAdjustment filter in adjustmentFilter.PixelAdjustments)
                            {
                                if (filter is TemperatureFilter)
                                {
                                    ((TemperatureFilter)filter).Temperature = temperature;
                                    return;
                                }
                            }

                            adjustmentFilter.PixelAdjustments.Add(new TemperatureFilter(temperature));
                        }
                    }
                    finally
                    {
                        Monitor.Exit(ObjectLock);
                    }
                }
            }            
        }

        private double hue = 0.0;
        public double Hue
        {
            get
            {
                return hue;
            }

            set
            {
                if (Monitor.TryEnter(ObjectLock, 10))
                {
                    try
                    {
                        hue = value;

                        foreach (PixelAdjustment filter in adjustmentFilter.PixelAdjustments)
                        {
                            if (filter is HSLFilter)
                            {
                                ((HSLFilter)filter).Hue = (int)hue;
                                return;
                            }
                        }

                        adjustmentFilter.PixelAdjustments.Add(new HSLFilter((int)hue, (int)saturation, (int)lightness));
                    }
                    finally
                    {
                        Monitor.Exit(ObjectLock);
                    }
                }   
            }
        }

        private double saturation = 100.0;
        public double Saturation
        {
            get
            {
                return saturation;
            }

            set
            {
                if (Monitor.TryEnter(ObjectLock, 10))
                {
                    try
                    {
                        saturation = value;

                        foreach (PixelAdjustment filter in adjustmentFilter.PixelAdjustments)
                        {
                            if (filter is HSLFilter)
                            {
                                ((HSLFilter)filter).Saturation = (int)saturation;
                                return;
                            }
                        }

                        adjustmentFilter.PixelAdjustments.Add(new HSLFilter((int)hue, (int)saturation, (int)lightness));
                    }
                    finally
                    {
                        Monitor.Exit(ObjectLock);
                    }
                }
            }
        }

        private double lightness = 0.0;
        public double Lightness
        {
            get
            {
                return lightness;
            }

            set
            {
                if (Monitor.TryEnter(ObjectLock, 10))
                {
                    try
                    {
                        lightness = value;

                        foreach (PixelAdjustment filter in adjustmentFilter.PixelAdjustments)
                        {
                            if (filter is HSLFilter)
                            {
                                ((HSLFilter)filter).Lightness = (int)lightness;
                                return;
                            }
                        }

                        adjustmentFilter.PixelAdjustments.Add(new HSLFilter((int)hue, (int)saturation, (int)lightness));
                    }
                    finally
                    {
                        Monitor.Exit(ObjectLock);
                    }
                }
            }
        }

        //Detail
        private double smooth = 0.0;
        public double Smooth
        {
            get
            {
                return smooth;
            }
            set
            {
                if (Monitor.TryEnter(ObjectLock, 10))
                {
                    try
                    {
                        smooth = value;

                        if (smooth == 0)
                        {
                            lock (ImageParameters.Instance)
                            {
                                effectFilters.RemoveAll(new Predicate<EffectFilter>(delegate(EffectFilter target)
                                    {
                                        return target is GaussianBlurFilter;
                                    }));
                            }
                        }
                        else
                        {
                            foreach (EffectFilter filter in effectFilters)
                            {
                                if (filter is GaussianBlurFilter)
                                {
                                    ((GaussianBlurFilter)filter).Radius = (int)(value + 1);
                                    return;
                                }
                            }

                            effectFilters.Add(new GaussianBlurFilter((int)(value + 1)));
                        }
                    }
                    finally
                    {
                        Monitor.Exit(ObjectLock);
                    }
                }
            }
        }        

        private double sharpen = 0.0;
        public double Sharpen
        {
            get
            {
                return sharpen;
            }
            set
            {
                //if (Monitor.TryEnter(ObjectLock, 10))
                //{
                //    try
                //    {
                //        sharpen = value;
                //        //Effects.sharpen = (int)(value);                        
                //    }
                //    finally
                //    {
                //        Monitor.Exit(ObjectLock);
                //    }
                //}
            }
        }
        #endregion

        #region Advanced Adjustment Parameters
        #region Enhance Detail Parameters

        //Low
        private double detailSaturationLow = 1.0;
        public double DetailSaturationLow
        {
            get
            {
                return detailSaturationLow;
            }

            set
            {
                detailSaturationLow = value;
                AdvancedAdjustments.Instance.SaturationLow = value;
                App.DoFilter = true;
            }
        }

        private double detailContrastLow = 0.0;
        public double DetailContrastLow
        {
            get
            {
                return detailContrastLow;
            }

            set
            {
                detailContrastLow = value;
                AdvancedAdjustments.Instance.ContrastLow = value * 100.0;
                App.DoFilter = true;
            }
        }

        //Mid
        private double detailSaturationMid = 1.0;
        public double DetailSaturationMid
        {
            get
            {
                return detailSaturationMid;
            }

            set
            {
                detailSaturationMid = value;
                AdvancedAdjustments.Instance.SaturationMid = value;
                App.DoFilter = true;
            }
        }

        private double detailContrastMid = 0.0;
        public double DetailContrastMid
        {
            get
            {
                return detailContrastMid;
            }

            set
            {
                detailContrastMid = value;
                AdvancedAdjustments.Instance.ContrastMid = value * 100.0;
                App.DoFilter = true;
            }
        }

        //High
        private double detailSaturationHigh = 1.0;
        public double DetailSaturationHigh
        {
            get
            {
                return detailSaturationHigh;
            }

            set
            {
                detailSaturationHigh = value;
                AdvancedAdjustments.Instance.SaturationHigh = value;
                App.DoFilter = true;
            }
        }

        private double detailContrastHigh = 0.0;
        public double DetailContrastHigh
        {
            get
            {
                return detailContrastHigh;
            }

            set
            {
                detailContrastHigh = value;
                AdvancedAdjustments.Instance.ContrastHigh = value * 100.0;
                App.DoFilter = true;
            }
        }

        #endregion

        #region Offset Parameters

        //Low
        private double redOffsetLow = 0.0;
        public double RedOffsetLow
        {
            get
            {
                return redOffsetLow;
            }

            set
            {
                redOffsetLow = value;
                AdvancedAdjustments.Instance.OffsetLow =
                    new DoubleRGB(redOffsetLow * 255.0, greenOffsetLow * 255.0, blueOffsetLow * 255.0);
                App.DoFilter = true;
            }
        }

        private double greenOffsetLow = 0.0;
        public double GreenOffsetLow
        {
            get
            {
                return greenOffsetLow;
            }

            set
            {
                greenOffsetLow = value;
                AdvancedAdjustments.Instance.OffsetLow =
                    new DoubleRGB(redOffsetLow * 255.0, greenOffsetLow * 255.0, blueOffsetLow * 255.0);
                App.DoFilter = true;
            }
        }

        private double blueOffsetLow = 0.0;
        public double BlueOffsetLow
        {
            get
            {
                return blueOffsetLow;
            }

            set
            {
                blueOffsetLow = value;
                AdvancedAdjustments.Instance.OffsetLow =
                    new DoubleRGB(redOffsetLow * 255.0, greenOffsetLow * 255.0, blueOffsetLow * 255.0);
                App.DoFilter = true;
            }
        }

        //Mid
        private double redOffsetMid = 0.0;
        public double RedOffsetMid
        {
            get
            {
                return redOffsetMid;
            }

            set
            {
                redOffsetMid = value;
                AdvancedAdjustments.Instance.OffsetMid =
                    new DoubleRGB(redOffsetMid * 255.0, greenOffsetMid * 255.0, blueOffsetMid * 255.0);
                App.DoFilter = true;
            }
        }

        private double greenOffsetMid = 0.0;
        public double GreenOffsetMid
        {
            get
            {
                return greenOffsetMid;
            }

            set
            {
                greenOffsetMid = value;
                AdvancedAdjustments.Instance.OffsetMid =
                    new DoubleRGB(redOffsetMid * 255.0, greenOffsetMid * 255.0, blueOffsetMid * 255.0);
                App.DoFilter = true;
            }
        }

        private double blueOffsetMid = 0.0;
        public double BlueOffsetMid
        {
            get
            {
                return blueOffsetMid;
            }

            set
            {
                blueOffsetMid = value;
                AdvancedAdjustments.Instance.OffsetMid =
                    new DoubleRGB(redOffsetMid * 255.0, greenOffsetMid * 255.0, blueOffsetMid * 255.0);
                App.DoFilter = true;
            }
        }

        //High
        private double redOffsetHigh = 0.0;
        public double RedOffsetHigh
        {
            get
            {
                return redOffsetHigh;
            }

            set
            {
                redOffsetHigh = value;
                AdvancedAdjustments.Instance.OffsetHigh =
                    new DoubleRGB(redOffsetHigh * 255.0, greenOffsetHigh * 255.0, blueOffsetHigh * 255.0);
                App.DoFilter = true;
            }
        }

        private double greenOffsetHigh = 0.0;
        public double GreenOffsetHigh
        {
            get
            {
                return greenOffsetHigh;
            }

            set
            {
                greenOffsetHigh = value;
                AdvancedAdjustments.Instance.OffsetHigh =
                    new DoubleRGB(redOffsetHigh * 255.0, greenOffsetHigh * 255.0, blueOffsetHigh * 255.0);
                App.DoFilter = true;
            }
        }

        private double blueOffsetHigh = 0.0;
        public double BlueOffsetHigh
        {
            get
            {
                return blueOffsetHigh;
            }

            set
            {
                blueOffsetHigh = value;
                AdvancedAdjustments.Instance.OffsetHigh =
                    new DoubleRGB(redOffsetHigh * 255.0, greenOffsetHigh * 255.0, blueOffsetHigh * 255.0);
                App.DoFilter = true;
            }
        }

        #endregion

        #region Gain Parameters

        //Low
        private double redGainLow = 0.0;
        public double RedGainLow
        {
            get
            {
                return redGainLow;
            }

            set
            {
                redGainLow = value;
                AdvancedAdjustments.Instance.GainLow =
                    new DoubleRGB(redGainLow, greenGainLow, blueGainLow);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        private double greenGainLow = 0.0;
        public double GreenGainLow
        {
            get
            {
                return greenGainLow;
            }

            set
            {
                greenGainLow = value;
                AdvancedAdjustments.Instance.GainLow =
                    new DoubleRGB(redGainLow, greenGainLow, blueGainLow);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        private double blueGainLow = 0.0;
        public double BlueGainLow
        {
            get
            {
                return blueGainLow;
            }

            set
            {
                blueGainLow = value;
                AdvancedAdjustments.Instance.GainLow =
                    new DoubleRGB(redGainLow, greenGainLow, blueGainLow);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        private double temperatureGainLow = 0.0;
        public double TemperatureGainLow
        {
            get
            {
                return temperatureGainLow;
            }

            set
            {
                temperatureGainLow = value;
                AdvancedAdjustments.Instance.TemperatureLow = temperatureGainLow;
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        private double magentaGainLow = 0.0;
        public double MagentaGainLow
        {
            get
            {
                return magentaGainLow;
            }

            set
            {
                magentaGainLow = value;
                AdvancedAdjustments.Instance.MagentaLow = magentaGainLow;
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        private double overallGainLow = 0.0;
        public double OverallGainLow
        {
            get
            {
                return overallGainLow;
            }

            set
            {
                overallGainLow = value;
                AdvancedAdjustments.Instance.OverallLow = overallGainLow;
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        //Mid
        private double redGainMid = 0.0;
        public double RedGainMid
        {
            get
            {
                return redGainMid;
            }

            set
            {
                redGainMid = value;
                AdvancedAdjustments.Instance.GainMid =
                    new DoubleRGB(redGainMid, greenGainMid, blueGainMid);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        private double greenGainMid = 0.0;
        public double GreenGainMid
        {
            get
            {
                return greenGainMid;
            }

            set
            {
                greenGainMid = value;
                AdvancedAdjustments.Instance.GainMid =
                    new DoubleRGB(redGainMid, greenGainMid, blueGainMid);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        private double blueGainMid = 0.0;
        public double BlueGainMid
        {
            get
            {
                return blueGainMid;
            }

            set
            {
                blueGainMid = value;
                AdvancedAdjustments.Instance.GainMid =
                    new DoubleRGB(redGainMid, greenGainMid, blueGainMid);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        private double temperatureGainMid = 0.0;
        public double TemperatureGainMid
        {
            get
            {
                return temperatureGainMid;
            }

            set
            {
                temperatureGainMid = value;
                AdvancedAdjustments.Instance.TemperatureMid = temperatureGainMid;
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        private double magentaGainMid = 0.0;
        public double MagentaGainMid
        {
            get
            {
                return magentaGainMid;
            }

            set
            {
                magentaGainMid = value;
                AdvancedAdjustments.Instance.MagentaMid = magentaGainMid;
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        private double overallGainMid = 0.0;
        public double OverallGainMid
        {
            get
            {
                return overallGainMid;
            }

            set
            {
                overallGainMid = value;
                AdvancedAdjustments.Instance.OverallMid = overallGainMid;
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        //High
        private double redGainHigh = 0.0;
        public double RedGainHigh
        {
            get
            {
                return redGainHigh;
            }

            set
            {
                redGainHigh = value;
                AdvancedAdjustments.Instance.GainHigh =
                    new DoubleRGB(redGainHigh, greenGainHigh, blueGainHigh);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        private double greenGainHigh = 0.0;
        public double GreenGainHigh
        {
            get
            {
                return greenGainHigh;
            }

            set
            {
                greenGainHigh = value;
                AdvancedAdjustments.Instance.GainHigh =
                    new DoubleRGB(redGainHigh, greenGainHigh, blueGainHigh);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        private double blueGainHigh = 0.0;
        public double BlueGainHigh
        {
            get
            {
                return blueGainHigh;
            }

            set
            {
                blueGainHigh = value;
                AdvancedAdjustments.Instance.GainHigh =
                    new DoubleRGB(redGainHigh, greenGainHigh, blueGainHigh);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        private double temperatureGainHigh = 0.0;
        public double TemperatureGainHigh
        {
            get
            {
                return temperatureGainHigh;
            }

            set
            {
                temperatureGainHigh = value;
                AdvancedAdjustments.Instance.TemperatureHigh = temperatureGainHigh;
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        private double magentaGainHigh = 0.0;
        public double MagentaGainHigh
        {
            get
            {
                return magentaGainHigh;
            }

            set
            {
                magentaGainHigh = value;
                AdvancedAdjustments.Instance.MagentaHigh = magentaGainHigh;
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        private double overallGainHigh = 0.0;
        public double OverallGainHigh
        {
            get
            {
                return overallGainHigh;
            }

            set
            {
                overallGainHigh = value;
                AdvancedAdjustments.Instance.OverallHigh = overallGainHigh;
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGain();
            }
        }

        #endregion

        #region Gamma Parameters

        //Low
        private double inRedLow = 0.0;
        public double InRedLow
        {
            get
            {
                return inRedLow;
            }

            set
            {
                inRedLow = value;
                AdvancedAdjustments.Instance.InGammaLow =
                    new DoubleRGB(1 + inRedLow * 2, 1 + inGreenLow * 2, 1 + inBlueLow * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        private double inGreenLow = 0.0;
        public double InGreenLow
        {
            get
            {
                return inGreenLow;
            }

            set
            {
                inGreenLow = value;
                AdvancedAdjustments.Instance.InGammaLow =
                    new DoubleRGB(1 + inRedLow * 2, 1 + inGreenLow * 2, 1 + inBlueLow * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        private double inBlueLow = 0.0;
        public double InBlueLow
        {
            get
            {
                return inBlueLow;
            }

            set
            {
                inBlueLow = value;
                AdvancedAdjustments.Instance.InGammaLow =
                    new DoubleRGB(1 + inRedLow * 2, 1 + inGreenLow * 2, 1 + inBlueLow * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        private double outRedLow = 0.0;
        public double OutRedLow
        {
            get
            {
                return outRedLow;
            }

            set
            {
                outRedLow = value;
                AdvancedAdjustments.Instance.OutGammaLow =
                    new DoubleRGB(1 + outRedLow * 2, 1 + outGreenLow * 2, 1 + outBlueLow * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        private double outGreenLow = 0.0;
        public double OutGreenLow
        {
            get
            {
                return outGreenLow;
            }

            set
            {
                outGreenLow = value;
                AdvancedAdjustments.Instance.OutGammaLow =
                    new DoubleRGB(1 + outRedLow * 2, 1 + outGreenLow * 2, 1 + outBlueLow * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        private double outBlueLow = 0.0;
        public double OutBlueLow
        {
            get
            {
                return outBlueLow;
            }

            set
            {
                outBlueLow = value;
                AdvancedAdjustments.Instance.OutGammaLow =
                    new DoubleRGB(1 + outRedLow * 2, 1 + outGreenLow * 2, 1 + outBlueLow * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        //Mid
        private double inRedMid = 0.0;
        public double InRedMid
        {
            get
            {
                return inRedMid;
            }

            set
            {
                inRedMid = value;
                AdvancedAdjustments.Instance.InGammaMid =
                    new DoubleRGB(1 + inRedMid * 2, 1 + inGreenMid * 2, 1 + inBlueMid * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        private double inGreenMid = 0.0;
        public double InGreenMid
        {
            get
            {
                return inGreenMid;
            }

            set
            {
                inGreenMid = value;
                AdvancedAdjustments.Instance.InGammaMid =
                    new DoubleRGB(1 + inRedMid * 2, 1 + inGreenMid * 2, 1 + inBlueMid * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        private double inBlueMid = 0.0;
        public double InBlueMid
        {
            get
            {
                return inBlueMid;
            }

            set
            {
                inBlueMid = value;
                AdvancedAdjustments.Instance.InGammaMid =
                    new DoubleRGB(1 + inRedMid * 2, 1 + inGreenMid * 2, 1 + inBlueMid * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        private double outRedMid = 0.0;
        public double OutRedMid
        {
            get
            {
                return outRedMid;
            }

            set
            {
                outRedMid = value;
                AdvancedAdjustments.Instance.OutGammaMid =
                    new DoubleRGB(1 + outRedMid * 2, 1 + outGreenMid * 2, 1 + outBlueMid * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        private double outGreenMid = 0.0;
        public double OutGreenMid
        {
            get
            {
                return outGreenMid;
            }

            set
            {
                outGreenMid = value;
                AdvancedAdjustments.Instance.OutGammaMid =
                    new DoubleRGB(1 + outRedMid * 2, 1 + outGreenMid * 2, 1 + outBlueMid * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        private double outBlueMid = 0.0;
        public double OutBlueMid
        {
            get
            {
                return outBlueMid;
            }

            set
            {
                outBlueMid = value;
                AdvancedAdjustments.Instance.OutGammaMid =
                    new DoubleRGB(1 + outRedMid * 2, 1 + outGreenMid * 2, 1 + outBlueMid * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        //High
        private double inRedHigh = 0.0;
        public double InRedHigh
        {
            get
            {
                return inRedHigh;
            }

            set
            {
                inRedHigh = value;
                AdvancedAdjustments.Instance.InGammaHigh =
                    new DoubleRGB(1 + inRedHigh * 2, 1 + inGreenHigh * 2, 1 + inBlueHigh * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        private double inGreenHigh = 0.0;
        public double InGreenHigh
        {
            get
            {
                return inGreenHigh;
            }

            set
            {
                inGreenHigh = value;
                AdvancedAdjustments.Instance.InGammaHigh =
                    new DoubleRGB(1 + inRedHigh * 2, 1 + inGreenHigh * 2, 1 + inBlueHigh * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        private double inBlueHigh = 0.0;
        public double InBlueHigh
        {
            get
            {
                return inBlueHigh;
            }

            set
            {
                inBlueHigh = value;
                AdvancedAdjustments.Instance.InGammaHigh =
                    new DoubleRGB(1 + inRedHigh * 2, 1 + inGreenHigh * 2, 1 + inBlueHigh * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        private double outRedHigh = 0.0;
        public double OutRedHigh
        {
            get
            {
                return outRedHigh;
            }

            set
            {
                outRedHigh = value;
                AdvancedAdjustments.Instance.OutGammaHigh =
                    new DoubleRGB(1 + outRedHigh * 2, 1 + outGreenHigh * 2, 1 + outBlueHigh * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        private double outGreenHigh = 0.0;
        public double OutGreenHigh
        {
            get
            {
                return outGreenHigh;
            }

            set
            {
                outGreenHigh = value;
                AdvancedAdjustments.Instance.OutGammaHigh =
                    new DoubleRGB(1 + outRedHigh * 2, 1 + outGreenHigh * 2, 1 + outBlueHigh * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        private double outBlueHigh = 0.0;
        public double OutBlueHigh
        {
            get
            {
                return outBlueHigh;
            }

            set
            {
                outBlueHigh = value;
                AdvancedAdjustments.Instance.OutGammaHigh =
                    new DoubleRGB(1 + outRedHigh * 2, 1 + outGreenHigh * 2, 1 + outBlueHigh * 2);
                App.DoFilter = true;
                AdvancedAdjustments.Instance.CalcGamma();
            }
        }

        #endregion 
        #endregion

        #region Operation Parameters

        private Rotation rotation;
        public Rotation Rotate
        {
            get
            {
                return rotation;
            }

            set
            {
                rotation = value;
            }
        }

        private FlipType flip;
        public FlipType Flip
        {
            get
            {
                return flip;
            }

            set
            {
                flip = value;                
            }
        }

        private Rect cropData;
        public Rect CropData
        {
            get
            {
                return cropData;
            }

            set
            {
                cropData = value;                
            }
        }

        #endregion

        #region Effect Filters Parameters

        //Effects        
        public bool Grayscale { get; set; }

        private float curvature = 2.0f;
        public float Curvature
        {
            get
            {
                return curvature;
            }

            set
            {
                curvature = value;

                SpecialEffect = new FisheyeFilter(curvature);
            }
        }

        private int glowRadius = 6;
        public int GlowRadius
        {
            get
            {
                return glowRadius;
            }

            set
            {
                glowRadius = value;

                SpecialEffect = new GlowEffect(glowRadius, glowBrightness, glowContrast);
            }
        }

        private int glowBrightness = 10;
        public int GlowBrightness
        {
            get
            {
                return glowBrightness;
            }

            set
            {
                glowBrightness = value;

                SpecialEffect = new GlowEffect(glowRadius, glowBrightness, glowContrast);
            }
        }

        private int glowContrast = 10;
        public int GlowContrast
        {
            get
            {
                return glowContrast;
            }

            set
            {
                glowContrast = value;

                SpecialEffect = new GlowEffect(glowRadius, glowBrightness, glowContrast);
            }
        }

        private int inkOutline = 50;
        public int InkOutline
        {
            get
            {
                return inkOutline;
            }

            set
            {
                inkOutline = value;

                SpecialEffect = new InkSketch(inkOutline, coloring);
            }
        }

        private int coloring = 50;
        public int Coloring
        {
            get
            {
                return coloring;
            }

            set
            {
                coloring = value;

                SpecialEffect = new InkSketch(inkOutline, coloring);
            }
        }

        private int brushSize = 3;
        public int BrushSize
        {
            get
            {
                return brushSize;
            }

            set
            {
                brushSize = value;

                SpecialEffect = new OilPainting(brushSize, coarseness);
            }
        }

        private byte coarseness = 20;
        public byte Coarseness
        {
            get
            {
                return coarseness;
            }

            set
            {
                coarseness = value;

                SpecialEffect = new OilPainting(brushSize, coarseness);
            }
        }

        private int softness = 5;
        public int Softness
        {
            get
            {
                return softness;
            }

            set
            {
                softness = value;

                SpecialEffect = new PortraitEffect(softness, lighting, warmth);
            }
        }

        private int lighting = 0;
        public int Lighting
        {
            get
            {
                return lighting;
            }

            set
            {
                lighting = value;

                SpecialEffect = new PortraitEffect(softness, lighting, warmth);
            }
        }

        public int warmth = 10;
        public int Warmth
        {
            get
            {
                return warmth;
            }

            set
            {
                warmth = value;

                SpecialEffect = new PortraitEffect(softness, lighting, warmth);
            }
        }

        #endregion
    }
}
