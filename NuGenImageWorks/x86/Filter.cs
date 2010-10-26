using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Genetibase.UI.NuGenImageWorks
{
    class Filter : RibbonItem
    {
        private double saturationlow, saturationmid, saturationhigh;
        private double contrastlow, contrastmid, contrasthigh;
        private double offsetrange;
        private RGB offsetlow, offsetmid, offsethigh;
        private double gainrange;
        private double temperaturelow, temperaturemid, temperaturehigh;
        private double magentalow, magentamid, magentahigh;
        private double overalllow, overallmid, overallhigh;
        private RGB gainlow, gainmid, gainhigh;
        private RGB ingammalow, ingammamid, ingammahigh;
        private RGB outgammalow, outgammamid, outgammahigh;
        
        private RGB tgainlow, tgainmid, tgainhigh;
        private RGB gammalow, gammamid, gammahigh;

        public Filter()
        {
            saturationlow = saturationmid = saturationhigh = 1.0;
            contrastlow = contrastmid = contrasthigh = 0.0;
            offsetrange = 1.0;
            offsetlow = offsetmid = offsethigh = new RGB(0.0, 0.0, 0.0);
            gainrange = 2.0;
            temperaturelow = temperaturemid = temperaturehigh = 0.0;
            magentalow = magentamid = magentahigh = 0.0;
            overalllow = overallmid = overallhigh = 0.0;
            gainlow = gainmid = gainhigh = new RGB(0.0, 0.0, 0.0);
            ingammalow = ingammamid = ingammahigh = new RGB(1.0, 1.0, 1.0);
            outgammalow = outgammamid = outgammahigh = new RGB(1.0, 1.0, 1.0);
            
            tgainlow = tgainmid = tgainhigh = new RGB(1.0, 1.0, 1.0);
            gammalow = gammamid = gammahigh = new RGB(1.0, 1.0, 1.0);
        }

        protected override void OnClick(EventArgs e)
        {
            foreach (Filter filter in this.Parent.Controls)
                filter.IsPressed = false;

            this.IsPressed = true;
            Program.Filter = this;

            if (Program.Photo != null)
            {
                if (Program.Source.Width < Program.Destination.Width)
                    Program.Optimize(Program.Destination.Width, Program.Destination.Height);
                else
                    Program.Optimize2();
                    //Program.Optimize(Program.Source.Width, Program.Source.Height);
            }

            base.OnClick(e);
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
            gammalow = new RGB(ingammalow.Red / outgammalow.Red, ingammalow.Green / outgammalow.Green, ingammalow.Blue / outgammalow.Blue);
            gammamid = new RGB(ingammamid.Red / outgammamid.Red, ingammamid.Green / outgammamid.Green, ingammamid.Blue / outgammamid.Blue);
            gammahigh = new RGB(ingammahigh.Red / outgammahigh.Red, ingammahigh.Green / outgammahigh.Green, ingammahigh.Blue / outgammahigh.Blue);
        }

        private RGB PixelOperation(Color col, double saturation, double contrast, RGB offset, RGB gain, RGB gamma)
        {
            double R = col.R;
            double G = col.G;
            double B = col.B;

            if (saturation < 1.0)
            {
                Color aux = Program.HSL2RGB(col.GetHue() / 360.0, col.GetSaturation() * saturation, col.GetBrightness());

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

            return new RGB(R, G, B);
        }

        private double LinearInterpolation(double p0, double p1, double t)
        {
            return p0 + (p1 - p0) * t;
        }

        public void Do(Bitmap bitmap)
        {
            try
            {
                BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                unsafe
                {
                    int size = bitmap.Width * bitmap.Height;
                    int* pixels = (int*)data.Scan0;

                    for (int i = 0; i < size; i++)
                    {
                        //System.Windows.Forms.Application.DoEvents();
                        Color col = Color.FromArgb(pixels[i]);
                        double R = col.R;
                        double G = col.G;
                        double B = col.B;
                        double L = 0.299 * R + 0.587 * G + 0.114 * B;

                        double fact = L / 255.0;

                        if (fact < 0.5)
                        {
                            RGB Low = PixelOperation(col, saturationlow, contrastlow, offsetlow, tgainlow, gammalow);
                            RGB Mid = PixelOperation(col, saturationmid, contrastmid, offsetmid, tgainmid, gammamid);

                            R = LinearInterpolation(Low.Red, Mid.Red, fact * 2.0);
                            G = LinearInterpolation(Low.Green, Mid.Green, fact * 2.0);
                            B = LinearInterpolation(Low.Blue, Mid.Blue, fact * 2.0);
                        }
                        else
                        {
                            RGB Mid = PixelOperation(col, saturationmid, contrastmid, offsetmid, tgainmid, gammamid);
                            RGB High = PixelOperation(col, saturationhigh, contrasthigh, offsethigh, tgainhigh, gammahigh);

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

                        pixels[i] = Color.FromArgb((int)R, (int)G, (int)B).ToArgb();
                    }
                }

                try
                {
                    bitmap.UnlockBits(data);
                }catch{ }

                this.Image = new Bitmap(bitmap, new Size(48, 32));
            }
            catch (Exception) 
            {                
            }
        }

        public static void Brightness(ref Bitmap b, Single nBrightness)
        {
            try
            {
                ColorMatrix brightnessMatrix = new ColorMatrix(new Single[][]
										{
											new Single[] {1, 0, 0, 0, 0}, 
											new Single[] {0, 1, 0, 0, 0}, 
											new Single[] {0, 0, 1, 0, 0}, 
											new Single[] {0, 0, 0, 1, 0}, 
											new Single[] {nBrightness, nBrightness, nBrightness, 0, 1}});

                ApplyColorMatrix(ref b, ref brightnessMatrix);
            }
            catch { }
        }

        public static void Contrast(ref Bitmap b, Single nContrast)
        {
            try
            {
                ColorMatrix contrastMatrix = new ColorMatrix(new Single[][]
										{
											new Single[] {nContrast, 0, 0, 0, 0}, 
											new Single[] {0, nContrast, 0, 0, 0}, 
											new Single[] {0, 0, nContrast, 0, 0}, 
											new Single[] {0, 0, 0, 1, 0}, 
											new Single[] {0.0001F, 0.0001F, 0.0001F, 0, 1}});

                ApplyColorMatrix(ref b, ref contrastMatrix);
            }
            catch { }
        }

        private static void ApplyColorMatrix(ref Bitmap b, ref ColorMatrix matrix)
        {

            ImageAttributes ImgAtt = new ImageAttributes();
            //Bitmap bmpMatrix=new Bitmap(b.Width, b.Height);
            Graphics grMatrix = Graphics.FromImage(b);

            ImgAtt.SetColorMatrix(matrix);

            grMatrix.DrawImage(b, new Rectangle(0, 0, b.Width, b.Height), 0, 0, b.Width, b.Height, GraphicsUnit.Pixel, ImgAtt);

            //b = bmpMatrix;

            grMatrix.Dispose();
            ImgAtt.Dispose();
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
        public RGB OffsetLow
        {
            get { return offsetlow; }
            set { offsetlow = value; }
        }
        public RGB OffsetMid
        {
            get { return offsetmid; }
            set { offsetmid = value; }
        }
        public RGB OffsetHigh
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
        public RGB GainLow
        {
            get { return gainlow; }
            set { gainlow = value; }
        }
        public RGB GainMid
        {
            get { return gainmid; }
            set { gainmid = value; }
        }
        public RGB GainHigh
        {
            get { return gainhigh; }
            set { gainhigh = value; }
        }
        public RGB InGammaLow
        {
            get { return ingammalow; }
            set { ingammalow = value; }
        }
        public RGB InGammaMid
        {
            get { return ingammamid; }
            set { ingammamid = value; }
        }
        public RGB InGammaHigh
        {
            get { return ingammahigh; }
            set { ingammahigh = value; }
        }
        public RGB OutGammaLow
        {
            get { return outgammalow; }
            set { outgammalow = value; }
        }
        public RGB OutGammaMid
        {
            get { return outgammamid; }
            set { outgammamid = value; }
        }
        public RGB OutGammaHigh
        {
            get { return outgammahigh; }
            set { outgammahigh = value; }
        }
    }

    struct RGB
    {
        public double Red, Green, Blue;

        public RGB(double red, double green, double blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }
    }
}
