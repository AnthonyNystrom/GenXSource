using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Genetibase.NuGenTransform
{
    class ImageHistogram : Panel
    {
        private Image image;
        private DiscretizeSettings settings;
        private NuGenDiscretize discretize;       

        private readonly int pad = 10;
        private readonly int slidersSize = 60;

        private Panel innerPanel;
        private TrackBar lowBar;
        private TrackBar highBar;

        public delegate void Delegate_ValueChanged(bool ignoreThread);
        private Delegate_ValueChanged valueChanged;

        public delegate void Delegate_ProgressUpdated(double progress);
        private Delegate_ProgressUpdated progressUpdated;

        public Delegate_ValueChanged ValueChanged
        {
            get
            {
                return valueChanged;
            }

            set
            {
                valueChanged = value;
            }
        }

        public Delegate_ProgressUpdated ProgressUpdated
        {
            get
            {
                return progressUpdated;
            }

            set
            {
                progressUpdated = value;
            }
        }

        private int[] histogramData;
        private int[] displayData;
        public ImageHistogram(Image image, DiscretizeSettings settings)
        {
            this.image = new Bitmap(image.Clone() as Image);
            this.settings = settings;

            discretize = new NuGenDiscretize(new Bitmap(image), settings);

            switch (settings.discretizeMethod)
            {
                case DiscretizeMethod.DiscretizeForeground :
                    histogramData = new int[101]; displayData = new int[101]; break;
                case DiscretizeMethod.DiscretizeHue :
                    histogramData = new int[361]; displayData = new int[361]; break;
                case DiscretizeMethod.DiscretizeIntensity:
                    histogramData = new int[101]; displayData = new int[101]; break;
                case DiscretizeMethod.DiscretizeSaturation:
                    histogramData = new int[101]; displayData = new int[101]; break;
                case DiscretizeMethod.DiscretizeValue:
                    histogramData = new int[101]; displayData = new int[101]; break;
            }

            innerPanel = new Panel();
            innerPanel.Size = new Size(Width - pad, Height - pad - slidersSize);
            innerPanel.Location = new Point(pad / 2, pad / 2);
            innerPanel.Paint += new PaintEventHandler(innerPanel_Paint);

            lowBar = new TrackBar();
            lowBar.Location = new Point(0, innerPanel.Location.Y + innerPanel.Height + 5);
            lowBar.Size = new Size(Width, 25);
            lowBar.TickStyle = TickStyle.None;
            lowBar.Minimum = 0;
            lowBar.Maximum = 100;
            lowBar.AutoSize = false;
            lowBar.ValueChanged += new EventHandler(lowBar_ValueChanged);
            lowBar.MouseUp += new MouseEventHandler(lowBar_MouseUp);

            highBar = new TrackBar();
            highBar.Location = new Point(lowBar.Location.X, lowBar.Location.Y + lowBar.Height + 5);
            highBar.Size = new Size(Width, 25);
            highBar.TickStyle = TickStyle.None;
            highBar.AutoSize = false;
            highBar.Minimum = 0;
            highBar.Maximum = 100;
            highBar.ValueChanged += new EventHandler(highBar_ValueChanged);
            highBar.MouseUp += new MouseEventHandler(highBar_MouseUp);
            
            this.DoubleBuffered = true;            

            Controls.Add(innerPanel);
            Controls.Add(lowBar);
            Controls.Add(highBar);

            this.BorderStyle = BorderStyle.Fixed3D;

            lowBar.Value = GetLowThreshold();
            highBar.Value = GetHighThreshold();
        }

        void highBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (valueChanged != null)
                valueChanged(true);
        }

        void lowBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (valueChanged != null)
                valueChanged(true);            
        }

        void highBar_ValueChanged(object sender, EventArgs e)
        {
            SetHighThreshold(highBar.Value);
            Refresh();
            if(valueChanged != null)
                valueChanged(false);
        }

        void lowBar_ValueChanged(object sender, EventArgs e)
        {
            SetLowThreshold(lowBar.Value);
            Refresh();

            if(valueChanged != null)
                valueChanged(false);
        }

        private void DrawHighlights(Graphics g)
        {
            double valueWidth =  (double)innerPanel.Width / (double)histogramData.Length;

            int xLow = (int)(valueWidth * lowBar.Value + 0.5);
            int xHigh = (int)(valueWidth * highBar.Value + 0.5);

            SolidBrush brush = new SolidBrush(Color.FromArgb(50, 125, 125, 125));

            if (xLow > xHigh)
            {
                Rectangle r1 = Rectangle.FromLTRB(0, 0, xHigh, innerPanel.Height);
                Rectangle r2 = Rectangle.FromLTRB(xLow, 0, innerPanel.Width, innerPanel.Height);

                g.FillRectangle(brush, r1);
                g.FillRectangle(brush, r2);
            }
            else
            {
                Rectangle r = Rectangle.FromLTRB(xLow, 0, xHigh, innerPanel.Height);

                g.FillRectangle(brush, r);
            }
        }

        private int GetLowThreshold()
        {
            switch (settings.discretizeMethod)
            {
                case DiscretizeMethod.DiscretizeForeground:
                    return settings.foregroundThresholdLow;
                case DiscretizeMethod.DiscretizeHue:
                    return settings.hueThresholdLow;
                case DiscretizeMethod.DiscretizeIntensity:
                    return settings.intensityThresholdLow;
                case DiscretizeMethod.DiscretizeSaturation:
                    return settings.saturationThresholdLow;
                case DiscretizeMethod.DiscretizeValue:
                    return settings.valueThresholdLow;
            }

            return 0;
        }

        private int GetHighThreshold()
        {
            switch (settings.discretizeMethod)
            {
                case DiscretizeMethod.DiscretizeForeground:
                    return settings.foregroundThresholdHigh;
                case DiscretizeMethod.DiscretizeHue:
                    return settings.hueThresholdHigh;
                case DiscretizeMethod.DiscretizeIntensity:
                    return settings.intensityThresholdHigh;
                case DiscretizeMethod.DiscretizeSaturation:
                    return settings.saturationThresholdHigh;
                case DiscretizeMethod.DiscretizeValue:
                    return settings.valueThresholdHigh;
            }

            return 0;
        }

        private void SetLowThreshold(int value)
        {
            switch (settings.discretizeMethod)
            {
                case DiscretizeMethod.DiscretizeForeground:
                    settings.foregroundThresholdLow = value; break;
                case DiscretizeMethod.DiscretizeHue:
                    settings.hueThresholdLow = value; break;
                case DiscretizeMethod.DiscretizeIntensity:
                    settings.intensityThresholdLow = value; break;
                case DiscretizeMethod.DiscretizeSaturation:
                    settings.saturationThresholdLow = value; break;
                case DiscretizeMethod.DiscretizeValue:
                    settings.valueThresholdLow = value; break;
            }

            discretize.Settings = settings;
        }

        private void SetHighThreshold(int value)
        {
            switch (settings.discretizeMethod)
            {
                case DiscretizeMethod.DiscretizeForeground:
                    settings.foregroundThresholdHigh = value; break;
                case DiscretizeMethod.DiscretizeHue:
                    settings.hueThresholdHigh = value; break;
                case DiscretizeMethod.DiscretizeIntensity:
                    settings.intensityThresholdHigh = value; break;
                case DiscretizeMethod.DiscretizeSaturation:
                    settings.saturationThresholdHigh = value; break;
                case DiscretizeMethod.DiscretizeValue:
                    settings.valueThresholdHigh = value; break;
            }

            discretize.Settings = settings;
        }

        void innerPanel_Paint(object sender, PaintEventArgs e)
        {
            double valueWidth = ((double)innerPanel.Width / (double)histogramData.Length);

            for (int i = 0; i < histogramData.Length; i++)
            {
                int x = (int)(i * valueWidth);

                Brush brush = new LinearGradientBrush(new Rectangle(0, 0, innerPanel.Width, innerPanel.Height), Color.LightSteelBlue, Color.SteelBlue, LinearGradientMode.Vertical);
                Pen p = new Pen(brush, (float)valueWidth);
                e.Graphics.DrawLine(p, x, innerPanel.Height, x, innerPanel.Height - displayData[i]);
            }

            DrawHighlights(e.Graphics);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

            innerPanel.Size = new Size(Width - pad, Height - pad - slidersSize);
            innerPanel.Location = new Point(pad/2, pad/2);

            lowBar.Location = new Point(0, innerPanel.Location.Y + innerPanel.Height + 5);
            lowBar.Size = new Size(Width, 25);            
            lowBar.TickStyle = TickStyle.None;
            lowBar.Minimum = 0;
            lowBar.Maximum = 100;
            lowBar.AutoSize = false;            

            highBar.Location = new Point(lowBar.Location.X, lowBar.Location.Y + lowBar.Height + 5);
            highBar.Size = new Size(Width, 25);
            highBar.TickStyle = TickStyle.None;
            highBar.AutoSize = false;
            highBar.Minimum = 0;
            highBar.Maximum = 100;
        }

        public void MakeHistogramData()
        {
            Bitmap bmp = new Bitmap(image);
            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    int value;

                    if (settings.discretizeMethod == DiscretizeMethod.DiscretizeForeground)
                    {
                        value = discretize.DiscretizeValueForeground(i, j, bmData);
                    }
                    else
                    {
                        value = discretize.DiscretizeValue(i, j, bmData);
                    }

                    histogramData[value]++;
                }

                progressUpdated((double)i / ((double)image.Width - 1));
            }

            bmp.UnlockBits(bmData);

            //Now scale the data logarithmically

            int pixels = image.Width * image.Height;

            for (int i = 0; i < histogramData.Length; i++)
            {
                int point = (int)((innerPanel.Height) * (1.0 - Math.Log(histogramData[i]) / Math.Log(pixels)) + 0.5);

                if (point < 0)
                    point = 0;
                else if (point > innerPanel.Height)
                    point = innerPanel.Height;

                displayData[i] = point;
            }

        }

        public DiscretizeSettings Settings
        {
            get
            {
                return settings;
            }

            set
            {
                settings = value;

                switch (settings.discretizeMethod)
                {
                    case DiscretizeMethod.DiscretizeForeground:
                        histogramData = new int[101]; displayData = new int[101]; break;
                    case DiscretizeMethod.DiscretizeHue:
                        histogramData = new int[361]; displayData = new int[361]; break;
                    case DiscretizeMethod.DiscretizeIntensity:
                        histogramData = new int[101]; displayData = new int[101]; break;
                    case DiscretizeMethod.DiscretizeSaturation:
                        histogramData = new int[101]; displayData = new int[101]; break;
                    case DiscretizeMethod.DiscretizeValue:
                        histogramData = new int[101]; displayData = new int[101]; break;
                }

                discretize = new NuGenDiscretize(new Bitmap(image), settings);

                if (settings.discretizeMethod == DiscretizeMethod.DiscretizeHue)
                {
                    lowBar.Maximum = 360;
                    lowBar.Minimum = 0;
                    highBar.Maximum = 360;
                    highBar.Minimum = 0;
                }
                else
                {
                    lowBar.Maximum = 100;
                    lowBar.Minimum = 0;
                    highBar.Maximum = 100;
                    highBar.Minimum = 0;
                }

                lowBar.Value = GetLowThreshold();
                highBar.Value = GetHighThreshold();

                MakeHistogramData();
                Refresh();
            }
        }

        public NuGenDiscretize Discretize
        {
            get
            {
                return discretize;
            }
        }
    }
}
