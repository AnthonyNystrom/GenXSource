using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Genetibase.UI.NuGenImageWorks.WaitCursor;
using Genetibase.UI.NuGenImageWorks.AnimatedCursor;
using System.IO;

namespace Genetibase.UI.NuGenImageWorks
{
    static class Program
    {
        public static bool fireEvent = true;

        public static Color HSL2RGB(double hue, double saturation, double luminance)
        {
            double r = 0.0, g = 0.0, b = 0.0;

            if (luminance == 0.0)
                r = g = b = 0.0;
            else if (saturation == 0.0)
                r = g = b = luminance;
            else
            {
                double aux2 = ((luminance <= 0.5) ? luminance * (1.0 + saturation) : luminance + saturation - (luminance * saturation));
                double aux1 = 2.0 * luminance - aux2;

                double[] t3 = new double[] { hue + 1.0 / 3.0, hue, hue - 1.0 / 3.0 };
                double[] clr = new double[] { 0.0, 0.0, 0.0 };

                for (int i = 0; i < 3; i++)
                {
                    if (t3[i] < 0)
                        t3[i] += 1.0;
                    if (t3[i] > 1)
                        t3[i] -= 1.0;

                    if (6.0 * t3[i] < 1.0)
                        clr[i] = aux1 + (aux2 - aux1) * t3[i] * 6.0;
                    else if (2.0 * t3[i] < 1.0)
                        clr[i] = aux2;
                    else if (3.0 * t3[i] < 2.0)
                        clr[i] = (aux1 + (aux2 - aux1) * ((2.0 / 3.0) - t3[i]) * 6.0);
                    else
                        clr[i] = aux1;
                }

                r = clr[0];
                g = clr[1];
                b = clr[2];
            }

            if (r > 1.0)
                r = 1.0;
            if (g > 1.0)
                g = 1.0;
            if (b > 1.0)
                b = 1.0;

            return Color.FromArgb((int)(255.0 * r), (int)(255.0 * g), (int)(255.0 * b));
        }
        
        public static void Optimize(int width, int height)
        {
            if (Program.Photo != null)
            {
                int w = Program.Photo.Width;
                int h = Program.Photo.Height;

                if (h > w)
                {
                    h = height;
                    w = (int)((double)w * ((double)h / (double)Program.Photo.Height));
                }
                else
                {
                    w = width;
                    h = (int)((double)h * ((double)w / (double)Program.Photo.Width));
                }

                w = Math.Min(w, Program.Photo.Width);
                h = Math.Min(h, Program.Photo.Height);

                percentage = (float)w / Program.Photo.Width;

                
                Bitmap temp = new Bitmap(Program.Photo, w, h);
                Program.Source.Image = temp;
                Application.DoEvents();
                frmMain.FilterEffects();

                //if (Program.Effects != null)
                //{
                //    Program.Destination.Image = Program.Effects.Do(temp);
                //}                

                //if (Program.Filter != null)
                //{
                //    Program.Filter.Do((Bitmap)Program.Destination.Image);                    
                //}

                //Effects2.Do((Bitmap)Program.Destination.Image);

                //if (frmMain.WaterMarkText != null || frmMain.WaterMarkImage != null)
                //{
                //    WaterMark wm = new WaterMark(frmMain.WaterMarkText, frmMain.TextAlign, frmMain.WaterMarkFont, frmMain.WaterMarkImage, frmMain.ImageAlign);
                //    Image DispTemp = Program.Destination.Image;
                //    Program.Destination.Image = (Bitmap)wm.MarkImage(DispTemp);
                //    DispTemp.Dispose();
                //    DispTemp = null;
                //}          
      

                //if (Program.Destination.Image != null)
                //{
                //    Image temp2 = Program.Destination.Image;
                //    Program.Destination.Image = (Image)Program.Destination.Image.Clone();
                //    temp2.Dispose();
                //}
                
            }
        }

        private static void newResizeCode()
        {
            int w = Program.Photo.Width;
            int h = Program.Photo.Height;

            int w2 = frmMain.panel1.Width;
            int h2 = frmMain.panel1.Height;

            int newW = 0;
            int newH = 0;

            double ratio = 1.0;

            if ((double)w / (double)w2 > (double)h / (double)h2)
            {
                ratio = (double)w2 / (double)w;
                newW = w2;
                newH = (int)(h * ratio);
            }
            else
            {
                ratio = (double)h2 / (double)h;
                newH = h2;
                newW = (int)(w * ratio);
            }

            Program.Source.Width = newW;
            Program.Source.Height = newH;
        }

        public static void Optimize2()
        {
            try
            {
                if (Program.Photo != null)
                {
                    newResizeCode();

                    int w = Program.Source.Width;
                    int h = Program.Source.Height;

                    percentage = (float)w / Program.Photo.Width;

                    Bitmap temp = new Bitmap(Program.Photo, w, h);
                    Program.Source.Image = temp;
                    frmMain.FilterEffects();
                }
            }
            catch(Exception ex) { }
        }

        public static Bitmap Photo = null;
        public static PictureBoxEx Source = null;
        public static PictureBoxEx Destination = null;
        public static Label Title = null;
        public static Effects Effects = null;
        public static Filter Filter = null;
        public static string FileName = null;
        public static MainForm frmMain = null;
        public static double percentage = 1.0f;


        public static AniCursor aniCursor;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            byte[] busyCursor = Genetibase.UI.NuGenImageWorks.Properties.Resources.busy;
            string tempFile = Path.GetTempFileName();

            FileStream fs = File.Open(tempFile, FileMode.OpenOrCreate, FileAccess.Write);

            BinaryWriter wr = new BinaryWriter( fs );
            wr.Write(busyCursor);
            wr.Close();
            fs.Close();

            aniCursor = new AniCursor( tempFile );
            File.Delete(tempFile);

            // Add the following line to your Application Startup code somewhere (the earlier the better)
            ApplicationWaitCursor.Cursor = aniCursor.Cursor;		// You can use an Cursor you like aswell

            // Use the following line (at any point in your App, or just once) to configure the length
            // of time to wait before showing the WaitCursor
            ApplicationWaitCursor.Delay = new TimeSpan(0, 0, 0, 0, 250);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Splash.ShowSplash(500,false);
            Application.Run(new MainForm());
        }
    }
}
