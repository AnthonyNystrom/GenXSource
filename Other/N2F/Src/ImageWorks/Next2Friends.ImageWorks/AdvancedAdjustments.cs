using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Next2Friends.ImageWorks.UI.NuGenImageWorks
{
    public class AdvancedAdjustments
    {
        private double saturationlow, saturationmid, saturationhigh;
        private double contrastlow, contrastmid, contrasthigh;
        private double offsetrange;
        private DoubleRGB offsetlow, offsetmid, offsethigh;
        private double gainrange;
        private double temperaturelow, temperaturemid, temperaturehigh;
        private double magentalow, magentamid, magentahigh;
        private double overalllow, overallmid, overallhigh;
        private DoubleRGB gainlow, gainmid, gainhigh;
        private DoubleRGB ingammalow, ingammamid, ingammahigh;
        private DoubleRGB outgammalow, outgammamid, outgammahigh;
        
        private DoubleRGB tgainlow, tgainmid, tgainhigh;
        private DoubleRGB gammalow, gammamid, gammahigh;

        private static AdvancedAdjustments instance;
        public static AdvancedAdjustments Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AdvancedAdjustments();
                }

                return instance;
            }
        }

        private AdvancedAdjustments()
        {
            saturationlow = saturationmid = saturationhigh = 1.0;
            contrastlow = contrastmid = contrasthigh = 0.0;
            offsetrange = 1.0;
            offsetlow = offsetmid = offsethigh = new DoubleRGB(0.0, 0.0, 0.0);
            gainrange = 2.0;
            temperaturelow = temperaturemid = temperaturehigh = 0.0;
            magentalow = magentamid = magentahigh = 0.0;
            overalllow = overallmid = overallhigh = 0.0;
            gainlow = gainmid = gainhigh = new DoubleRGB(0.0, 0.0, 0.0);
            ingammalow = ingammamid = ingammahigh = new DoubleRGB(1.0, 1.0, 1.0);
            outgammalow = outgammamid = outgammahigh = new DoubleRGB(1.0, 1.0, 1.0);
            
            tgainlow = tgainmid = tgainhigh = new DoubleRGB(1.0, 1.0, 1.0);
            gammalow = gammamid = gammahigh = new DoubleRGB(1.0, 1.0, 1.0);
        }

        public void CalcGain()
        {
            tgainlow.Red = Math.Pow(gainrange, gainlow.Red + (overalllow + (temperaturelow + magentalow * 0.5)));
            tgainlow.Green = Math.Pow(gainrange, gainlow.Green + (overalllow - magentalow));
            tgainlow.Blue = Math.Pow(gainrange, gainlow.Blue + (overalllow - (temperaturelow + magentalow * 0.5)));
            tgainmid.Red = Math.Pow(gainrange, gainmid.Red + (overallmid + (temperaturemid + magentamid * 0.5)));
            tgainmid.Green = Math.Pow(gainrange, gainmid.Green + (overallmid - magentamid));
            tgainmid.Blue = Math.Pow(gainrange, gainmid.Blue + (overallmid - (temperaturemid + magentamid * 0.5)));
            tgainhigh.Red = Math.Pow(gainrange, gainhigh.Red + (overallhigh + (temperaturehigh + magentahigh * 0.5)));
            tgainhigh.Green = Math.Pow(gainrange, gainhigh.Green + (overallhigh - magentahigh));
            tgainhigh.Blue = Math.Pow(gainrange, gainhigh.Blue + (overallhigh - (temperaturehigh + magentahigh * 0.5)));
        }
        public void CalcGamma()
        {
            gammalow = new DoubleRGB(ingammalow.Red / outgammalow.Red, ingammalow.Green / outgammalow.Green, ingammalow.Blue / outgammalow.Blue);
            gammamid = new DoubleRGB(ingammamid.Red / outgammamid.Red, ingammamid.Green / outgammamid.Green, ingammamid.Blue / outgammamid.Blue);
            gammahigh = new DoubleRGB(ingammahigh.Red / outgammahigh.Red, ingammahigh.Green / outgammahigh.Green, ingammahigh.Blue / outgammahigh.Blue);
        }

        private DoubleRGB PixelOperation(DoubleRGB col, double saturation, double contrast, DoubleRGB offset, DoubleRGB gain, DoubleRGB gamma)
        {
            double R = col.Red;
            double G = col.Green;
            double B = col.Blue;

            if (saturation < 1.0)
            {
                double hue, sat, lum;
                
                Utility.RGB2HSL((byte)R, (byte)G, (byte)B, out hue, out sat, out lum);

                Color aux = Utility.HSL2RGB(hue / 360.0, sat * saturation, lum);

                R = aux.R;
                G = aux.G;
                B = aux.B;
            }

            if (contrast != 0.0)
            {
                double rc;
                if (contrast < -100)
                    rc = 0.0;
                else if (contrast > 100)
                    rc = 2.0;
                else
                    rc = (100.0 + contrast) / 100.0;

                rc *= rc;

                double red = R / 255.0;
                red -= 0.5;
                red *= rc;
                red += 0.5;
                R = red * 255.0;

                double gc;
                if (contrast < -100)
                    gc = 0.0;
                else if (contrast > 100)
                    gc = 2.0;
                else
                    gc = (100.0 + contrast) / 100.0;

                gc *= gc;

                double green = G / 255.0;
                green -= 0.5;
                green *= gc;
                green += 0.5;
                G = green * 255.0;

                double bc;
                if (contrast < -100)
                    bc = 0.0;
                else if (contrast > 100)
                    bc = 2.0;
                else
                    bc = (100.0 + contrast) / 100.0;

                bc *= bc;

                double blue = B / 255.0;
                blue -= 0.5;
                blue *= bc;
                blue += 0.5;
                B = blue * 255.0;
            }

            if (gamma.Red == 1.0)
                R = (R + offset.Red) * gain.Red;
            else
                R = Math.Pow(((R + offset.Red) * gain.Red) / 255.0, gamma.Red) * 255.0;

            if (gamma.Green == 1.0)
                G = (G + offset.Green) * gain.Green;
            else
                G = Math.Pow(((G + offset.Green) * gain.Green) / 255.0, gamma.Green) * 255.0;

            if (gamma.Blue == 1.0)
                B = (B + offset.Blue) * gain.Blue;
            else
                B = Math.Pow(((B + offset.Blue) * gain.Blue) / 255.0, gamma.Blue) * 255.0;

            DoubleRGB retval;
            retval.Red = R;
            retval.Green = G;
            retval.Blue = B;

            return retval;
        }

        private double LinearInterpolation(double p0, double p1, double t)
        {
            return p0 + (p1 - p0) * t;
        }

        public BitmapSource Do(BitmapSource bitmap)
        {
            try
            {
                byte[] bytes = new byte[bitmap.PixelWidth * bitmap.PixelHeight * 4];

                bitmap.CopyPixels(bytes, bitmap.PixelWidth * 4, 0);
                int size = bitmap.PixelWidth * bitmap.PixelHeight * 4;

                for (int i = 0; i < size; i += 4)
                {
                    double R = bytes[i + 2];
                    double G = bytes[i + 1];
                    double B = bytes[i];
                    double L = 0.299 * R + 0.587 * G + 0.114 * B;

                    DoubleRGB col;
                    col.Red = R;
                    col.Green = G;
                    col.Blue = B;

                    double fact = L / 255.0;

                    if (fact < 0.5)
                    {
                        DoubleRGB Low = PixelOperation(col, saturationlow, contrastlow, offsetlow, tgainlow, gammalow);
                        DoubleRGB Mid = PixelOperation(col, saturationmid, contrastmid, offsetmid, tgainmid, gammamid);

                        R = LinearInterpolation(Low.Red, Mid.Red, fact * 2.0);
                        G = LinearInterpolation(Low.Green, Mid.Green, fact * 2.0);
                        B = LinearInterpolation(Low.Blue, Mid.Blue, fact * 2.0);
                    }
                    else
                    {
                        DoubleRGB Mid = PixelOperation(col, saturationmid, contrastmid, offsetmid, tgainmid, gammamid);
                        DoubleRGB High = PixelOperation(col, saturationhigh, contrasthigh, offsethigh, tgainhigh, gammahigh);

                        R = LinearInterpolation(Mid.Red, High.Red, (fact - 0.5) * 2.0);
                        G = LinearInterpolation(Mid.Green, High.Green, (fact - 0.5) * 2.0);
                        B = LinearInterpolation(Mid.Blue, High.Blue, (fact - 0.5) * 2.0);
                    }

                    if (R > 255.0)
                        R = 255.0;
                    else if (R < 0.0)
                        R = 0.0;

                    if (G > 255.0)
                        G = 255.0;
                    else if (G < 0.0)
                        G = 0.0;

                    if (B > 255.0)
                        B = 255.0;
                    else if (B < 0.0)
                        B = 0.0;

                    bytes[i] = (byte)B;
                    bytes[i + 1] = (byte)G;
                    bytes[i + 2] = (byte)R;
                }

                bitmap = BitmapSource.Create(bitmap.PixelWidth, bitmap.PixelHeight, 96, 96,
                    PixelFormats.Bgra32, null, bytes, bitmap.PixelWidth * 4);
            }
            catch (Exception)
            {
            }

            return bitmap;
        }

        public static void Brightness(BitmapSource b, Single nBrightness)
        {
            try
            {
                Single[][] brightnessMatrix = new Single[][]
										{
											new Single[] {1, 0, 0, 0, 0}, 
											new Single[] {0, 1, 0, 0, 0}, 
											new Single[] {0, 0, 1, 0, 0}, 
											new Single[] {0, 0, 0, 1, 0}, 
											new Single[] {nBrightness, nBrightness, nBrightness, 0, 1}};

                ApplyColorMatrix(b, brightnessMatrix);
            }
            catch { }
        }

        public static void Contrast(BitmapSource b, Single nContrast)
        {
            try
            {
                Single[][] contrastMatrix = new Single[][]
										{
											new Single[] {nContrast, 0, 0, 0, 0}, 
											new Single[] {0, nContrast, 0, 0, 0}, 
											new Single[] {0, 0, nContrast, 0, 0}, 
											new Single[] {0, 0, 0, 1, 0}, 
											new Single[] {0.0001F, 0.0001F, 0.0001F, 0, 1}};

                ApplyColorMatrix(b, contrastMatrix);
            }
            catch { }
        }

        private static void ApplyColorMatrix(BitmapSource b, Single[][] matrix)
        {
            byte[] bytes = new byte[b.PixelWidth * b.PixelHeight * 4];

            for (int i = 0; i < bytes.Length; i++)
            {
                float blue  = bytes[i];
                float green = bytes[i + 1];
                float red = bytes[i + 2];
                float alpha = bytes[i + 3];

                red = red * matrix[0][0] + green * matrix[1][0] + blue * matrix[2][0] + 
                    alpha * matrix[3][0] + matrix[4][0];

                green = red * matrix[0][1] + green * matrix[1][1] + blue * matrix[2][1] +
                    alpha * matrix[3][1] + matrix[4][1];

                blue = red * matrix[0][2] + green * matrix[1][2] + blue * matrix[2][2] +
                    alpha * matrix[3][2] + matrix[4][2];

                alpha = red * matrix[0][3] + green * matrix[1][3] + blue * matrix[2][3] +
                    alpha * matrix[3][3] + matrix[4][3];

                if (blue < 0)
                {
                    blue = 0;
                }
                else if (blue > 255)
                {
                    blue = 255;
                }

                if (green < 0)
                {
                    green = 0;
                }
                else if (green > 255)
                {
                    green = 255;
                }

                if (red < 0)
                {
                    red = 0;
                }
                else if (red > 255)
                {
                    red = 255;
                }

                if (alpha < 0)
                {
                    alpha = 0;
                }
                else if (alpha > 255)
                {
                    alpha = 255;
                }

                bytes[i] = (byte)blue;
                bytes[i + 1] = (byte)green;
                bytes[i + 2] = (byte)red;
                bytes[i + 3] = (byte)alpha;
            }
        }

        public double SaturationLow
        {
            get { return saturationlow; }
            set { saturationlow = value; }
        }
        public double SaturationMid
        {
            get { return saturationmid; }
            set { saturationmid = value; }
        }
        public double SaturationHigh
        {
            get { return saturationhigh; }
            set { saturationhigh = value; }
        }
        public double ContrastLow
        {
            get { return contrastlow; }
            set { contrastlow = value; }
        }
        public double ContrastMid
        {
            get { return contrastmid; }
            set { contrastmid = value; }
        }
        public double ContrastHigh
        {
            get { return contrasthigh; }
            set { contrasthigh = value; }
        }
        public double OffsetRange
        {
            get { return offsetrange; }
            set { offsetrange = value; }
        }
        public DoubleRGB OffsetLow
        {
            get { return offsetlow; }
            set { offsetlow = value; }
        }
        public DoubleRGB OffsetMid
        {
            get { return offsetmid; }
            set { offsetmid = value; }
        }
        public DoubleRGB OffsetHigh
        {
            get { return offsethigh; }
            set { offsethigh = value; }
        }
        public double GainRange
        {
            get { return gainrange; }
            set { gainrange = value; }
        }
        public double TemperatureLow
        {
            get { return temperaturelow; }
            set { temperaturelow = value; }
        }
        public double TemperatureMid
        {
            get { return temperaturemid; }
            set { temperaturemid = value; }
        }
        public double TemperatureHigh
        {
            get { return temperaturehigh; }
            set { temperaturehigh = value; }
        }
        public double MagentaLow
        {
            get { return magentalow; }
            set { magentalow = value; }
        }
        public double MagentaMid
        {
            get { return magentamid; }
            set { magentamid = value; }
        }
        public double MagentaHigh
        {
            get { return magentahigh; }
            set { magentahigh = value; }
        }
        public double OverallLow
        {
            get { return overalllow; }
            set { overalllow = value; }
        }
        public double OverallMid
        {
            get { return overallmid; }
            set { overallmid = value; }
        }
        public double OverallHigh
        {
            get { return overallhigh; }
            set { overallhigh = value; }
        }
        public DoubleRGB GainLow
        {
            get { return gainlow; }
            set { gainlow = value; }
        }
        public DoubleRGB GainMid
        {
            get { return gainmid; }
            set { gainmid = value; }
        }
        public DoubleRGB GainHigh
        {
            get { return gainhigh; }
            set { gainhigh = value; }
        }
        public DoubleRGB InGammaLow
        {
            get { return ingammalow; }
            set { ingammalow = value; }
        }
        public DoubleRGB InGammaMid
        {
            get { return ingammamid; }
            set { ingammamid = value; }
        }
        public DoubleRGB InGammaHigh
        {
            get { return ingammahigh; }
            set { ingammahigh = value; }
        }
        public DoubleRGB OutGammaLow
        {
            get { return outgammalow; }
            set { outgammalow = value; }
        }
        public DoubleRGB OutGammaMid
        {
            get { return outgammamid; }
            set { outgammamid = value; }
        }
        public DoubleRGB OutGammaHigh
        {
            get { return outgammahigh; }
            set { outgammahigh = value; }
        }
    }
}