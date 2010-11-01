using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Genetibase.NuGenTransform
{
    enum PixelState
    {
        PixelOff = 0,
        PixelOnRemovedStage1,
        PixelOnRemovedStage2,
        PixelOnRemovedStage3,
        PixelOnRemovedStage4,
        PixelOn
    };

    enum MMUnits
    {
        mmCartesian,
        mmDegrees,
        mmGradians,
        mmRadians
    };

    /*
     * This class takes the responsibility of removing the gridlines from a given image.
     * 
     * There are three methods for doing this.
     * 
     * 1) The simplest way is by isolating the color of the grid from the image and setting
     *    it to the background color, removing it effectively.
     *      The problem with this method is that the grid may be the color as some other
     *      data which you don't want to erase.
     * 
     * 2) The next way is to erase pixels around the user-defined gridlines (based on axes)
     *      This way is more effective at eliminating only the grid lines and preserving
     *      the rest of the image, however it is dependent on the user defining the axes
     *      accurately, and it tends to remove more than it needs to.
     * 
     * 3) This is probably the most effective way to remove the gridlines, this method
     *      just goes through the entire image and removes thin lines of pixels that
     *      are parallel to the axes.  The chances of this removing more data than it 
     *      and normally removes all unwanted lines.  It is not perfect, it will remove
     *      too much in some cases, and it also requires well defined axes, inaccurate axes
     *      will cause it to remove the wrong lines.
     *      
     * */
    public class NuGenGridRemoval
    {
        private Bitmap bmp;
        private NuGenDiscretize discretize;

        private double xBasisXS, xBasisYS, yBasisXS, yBasisYS; // basis vectors in screen coordinates
        private List<SearchPoint> searchPatternPlusX = new List<SearchPoint>(); // search pattern in +x direction
        private List<SearchPoint> searchPatternPlusY = new List<SearchPoint>(); // search pattern in +y direction
        private List<SearchPoint> searchPatternMinusX = new List<SearchPoint>(); // search pattern in -x direction
        private List<SearchPoint> searchPatternMinusY = new List<SearchPoint>(); // search pattern in -y direction  '

        private Dictionary<int, Neuron> neuronDict = new Dictionary<int, Neuron>();

        private const int removalMaxRecursion = 4000;

        //Creates this grid remover operating on the image, using the given discretize settings
        public NuGenGridRemoval(Image img, NuGenDiscretize discretize)
        {
            bmp = new Bitmap(img);
            this.discretize = discretize;
        }

        //Remove gridlines and reconnect gaps
        public void RemoveAndConnect(NuGenScreenTranslate transform, CoordSettings coordSettings, GridRemovalSettings gridRemovalSettings, Color bgColor)
        {
            int width = bmp.Width;
            int height = bmp.Height;

            PixelState[,] pixels = InitializePixels();

            // 1) Color removal
            if (gridRemovalSettings.removeColor)
            {
                RemoveColor(pixels, gridRemovalSettings, PixelState.PixelOnRemovedStage1);
            }


            // 2) Remove pixels around gridlines
            if (gridRemovalSettings.removeGridlines && gridRemovalSettings.gridDistance > 0.0)
            {
                RemoveGridlines(pixels, transform, coordSettings, gridRemovalSettings, PixelState.PixelOnRemovedStage2);
            }

            // 3) Remove thin lines parallel to the axes
            if (gridRemovalSettings.removeThinLines && gridRemovalSettings.thinThickness > 0.0)
            {
                RemoveThinLines(pixels, coordSettings, transform, gridRemovalSettings.thinThickness,
                    PixelState.PixelOnRemovedStage3, PixelState.PixelOnRemovedStage4);
            }

            // Reconnect the gaps created from the prior steps
            if (gridRemovalSettings.gapSeparation > 0.0)
            {
                ConnectNeuronsAcrossGaps(pixels, gridRemovalSettings.gapSeparation);
            }

            //Write the image
            SavePixels(pixels, bgColor);

            //Save the image
            discretize.SetImage(bmp);
        }

        //Connects neurons which are seperated by synapses
        private void ConnectNeuronsAcrossGaps(PixelState[,] pixels, double gapSeperation)
        {
            int[,] pixel2Neuron = InitializePixel2Neuron(pixels);

            //neuronDict.Clear();            

            for (int i = 0; i < neuronDict.Count; i++)
            {
                neuronDict[i].ConnectNeuronAcrossGaps(pixels, pixel2Neuron, neuronDict, this.bmp.Width, this.bmp.Height, gapSeperation);
            }
        }

        //Initializes the pixel array to create neurons
        private int[,] InitializePixel2Neuron(PixelState[,] pixels)
        {
            int width = bmp.Width;
            int height = bmp.Height;

            // each pixel gets assigned to a neuron. each neuron has its own index
            int[,] pixel2Neuron = new int[width, height];

            int x, y;
            for (x = 0; x < width; x++)
                for (y = 0; y < height; y++)
                    pixel2Neuron[x, y] = -1;

            for (x = 0; x < width; x++)
                for (y = 0; y < height; y++)
                    RecursivelySetPixel2Neuron(pixels, pixel2Neuron, x, y, width, height, null, null, 0);

            return pixel2Neuron;
        }

        private void RecursivelySetPixel2Neuron(PixelState[,] pixels, int[,] pixel2Neuron, int x, int y, int width, int height, Neuron activeNeuron, Synapse activeSynapse, int level)
        {
            // if this pixel should belong to a neuron, then assign it to the active neuron.
            // if there is no active neuron (activeNeuron is null), then create one
            int xDelta, yDelta;

            // do not set this pixel if it is not on, or if it has already been assigned
            // to a neuron
            if ((pixels[x, y] != PixelState.PixelOn) || (pixel2Neuron[x, y] != -1))
                return;

            // this pixel needs to be assigned to a neuron
            if (activeNeuron == null)
            {
                activeNeuron = new Neuron(neuronDict);
            }

            pixel2Neuron[x, y] = activeNeuron.Index;

            // does this pixel need to be assigned to a synapse? look at eight
            // nearest neighbors
            bool needSynapse = false;
            for (xDelta = -1; !needSynapse && (xDelta <= 1); xDelta++)
                for (yDelta = -1; !needSynapse && (yDelta <= 1); yDelta++)
                    if ((xDelta != 0) || (yDelta != 0))
                    {
                        int xNeighbor = x + xDelta;
                        int yNeighbor = y + yDelta;
                        if ((0 <= xNeighbor) && (xNeighbor < width) &&
                          (0 <= yNeighbor) && (yNeighbor < height))
                        {
                            if ((pixels[xNeighbor, yNeighbor] != PixelState.PixelOff) &&
                              (pixels[xNeighbor, yNeighbor] != PixelState.PixelOn))
                            {
                                needSynapse = true;
                            }
                        }
                    }

            // assign to synapse, creating a new one if necessary
            if (needSynapse)
            {
                if (activeSynapse == null)
                {
                    activeSynapse = activeNeuron.AddSynapse();
                }
                activeSynapse.addPixel(x, y);
            }
            else
                activeSynapse = null;

            // limit the levels of recursion since Microsoft Windows will run out of stack
            // space. specifically, the default stack size of one megabyte only handles
            // 5700 levels of recursion here, and a stack size of two megabytes only
            // handles 11000 levels of recursion. extreme amounts of recursion happen in
            // images with extreme numbers of lines
            if (level < removalMaxRecursion)
            {
                // also set the eight nearest neighbors
                for (xDelta = -1; xDelta <= 1; xDelta++)
                    for (yDelta = -1; yDelta <= 1; yDelta++)
                        if ((xDelta != 0) || (yDelta != 0))
                        {
                            int xNeighbor = x + xDelta;
                            int yNeighbor = y + yDelta;
                            if ((0 <= xNeighbor) && (xNeighbor < width) &&
                              (0 <= yNeighbor) && (yNeighbor < height))
                            {
                                RecursivelySetPixel2Neuron(pixels, pixel2Neuron, xNeighbor, yNeighbor, width, height, activeNeuron, activeSynapse, level + 1);
                            }
                        }
            }
        }

        //Removes thin lines which are parallel to the user defined axes
        private void RemoveThinLines(PixelState[,] pixels, CoordSettings coordSettings, NuGenScreenTranslate transform,
                double thinThickness, PixelState pixelStateRemovedPass1, PixelState pixelStateRemovedPass2)
        {
            if (transform.ValidAxes)
            {
                InitializeThin(coordSettings, transform, thinThickness);

                // first pass erases the gridMeshSettings lines, except for the junctions
                EraseThinPixels(pixels, thinThickness, pixelStateRemovedPass1);

                // second pass erases the gridMeshSettings line junctions, which are little islands of on-pixels
                EraseThinPixels(pixels, thinThickness, pixelStateRemovedPass2);
            }
        }

        private void EraseThinPixels(PixelState[,] pixels, double thinThickness, PixelState pixelThreshold)
        {
            // loop through pixels, removing those on thin lines
            for (int x = 0; x < this.bmp.Width; x++)
            {
                for (int y = 0; y < this.bmp.Height; y++)
                {
                    if (pixels[x, y] == PixelState.PixelOn)
                    {
                        double dPlusX = DistanceToOffPixel(pixels, x, y, searchPatternPlusX,
                          pixelThreshold);
                        double dMinusX = DistanceToOffPixel(pixels, x, y, searchPatternMinusX,
                          pixelThreshold);
                        if (dMinusX + dPlusX <= thinThickness)
                        {
                            pixels[x, y] = pixelThreshold;
                            continue;
                        }
                        else
                        {
                            double dPlusY = DistanceToOffPixel(pixels, x, y, searchPatternPlusY,
                              pixelThreshold);
                            double dMinusY = DistanceToOffPixel(pixels, x, y, searchPatternMinusY,
                              pixelThreshold);
                            if (dMinusY + dPlusY <= thinThickness)
                            {
                                pixels[x, y] = pixelThreshold;
                                continue;
                            }
                        }
                    }
                }
            }
        }

        private double DistanceToOffPixel(PixelState[,] pixels, int x, int y, List<SearchPoint> searchPattern, PixelState pixelThreshold)
        {
            // pixels having values of pixelThreshold and higher are considered "on"
            foreach (SearchPoint p in searchPattern)
            {
                int i = x + p.X;
                if (0 <= i && i < this.bmp.Width)
                {
                    int j = y + p.Y;
                    if (0 <= j && j < this.bmp.Height)
                    {
                        if ((int)pixels[i, j] < (int)pixelThreshold)
                        {
                            // reduce by one half since the distance is from the center of the on pixel
                            // to the boundary of the off pixel
                            return p.Distance - 0.5;
                        }
                    }
                }
            }

            // return really big number
            return (double)this.bmp.Width;
        }

        private void InitializeThin(CoordSettings coordSettings, NuGenScreenTranslate transform, double thinThickness)
        {
            transform.XBasisScreen(coordSettings, out xBasisXS, out xBasisYS);
            transform.YBasisScreen(coordSettings, out yBasisXS, out yBasisYS);

            // initialize search patterns
            InitializeThinSearch(thinThickness, xBasisXS, yBasisXS, searchPatternPlusX);
            InitializeThinSearch(thinThickness, xBasisYS, yBasisYS, searchPatternPlusY);
            InitializeThinSearch(thinThickness, -xBasisXS, yBasisXS, searchPatternMinusX);
            InitializeThinSearch(thinThickness, xBasisYS, -yBasisYS, searchPatternMinusY);
        }

        private void InitializeThinSearch(double thinThickness, double xBasis, double yBasis, List<SearchPoint> pattern)
        {
            // simply loop through all pixels that could possibly be closer than
            // a half pixel from a line through (0,0) and parallel to the basis vector
            int ijmax = (int)(thinThickness + 0.5) + 1;
            for (int j = -ijmax; j <= ijmax; j++)
                for (int i = -ijmax; i <= ijmax; i++)
                {
                    double x = (double)i;
                    double y = (double)j;

                    // this point must be in same direction as the basis vector. use dot product
                    if (x * xBasis + y * yBasis > 0)
                    {
                        // this point must be within a half pixel of the basis vector
                        if (NuGenMath.DistanceToLine(x, y, 0.0, 0.0, xBasis, yBasis) <= 0.5)
                        {
                            // add new search point
                            SearchPoint p = new SearchPoint(i, j, Math.Sqrt(i * i + j * j));

                            pattern.Add(p);
                        }
                    }
                }

            pattern.Sort();
        }

        //Saves the pixel states to the image
        private void SavePixels(PixelState[,] pixels, Color bgColor)
        {
            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, bmp.PixelFormat);

            byte r = bgColor.R;
            byte g = bgColor.G;
            byte b = bgColor.B;
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    PixelState state = pixels[x, y];
                    if (state != PixelState.PixelOff && state != PixelState.PixelOn)
                    {
                        NuGenImageProcessor.SetPixelAt(bmData, x, y, r, g, b);
                    }
                }
            }
            bmp.UnlockBits(bmData);
        }

        private PixelState[,] InitializePixels()
        {
            PixelState[,] pixels = new PixelState[bmp.Width, bmp.Height];
            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    int r, g, b;
                    NuGenImageProcessor.GetPixelAt(bmData, x, y, out r, out g, out b);

                    if ((Math.Round(((double)Math.Max(Math.Max(r, g), b)) * 255.0 / 100.0)) < 50)
                        pixels[x, y] = PixelState.PixelOn;
                    else
                        pixels[x, y] = PixelState.PixelOff;
                }
            }

            bmp.UnlockBits(bmData);
            return pixels;
        }

        //Removes pixels of a certain color
        private void RemoveColor(PixelState[,] pixels, GridRemovalSettings gridSettings, PixelState pxState)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    int value = discretize.DiscretizeValueForeground(x, y, gridSettings.color);

                    if (!discretize.PixelIsOn(value, gridSettings))
                        pixels[x, y] = pxState;
                }
            }
        }

        //Removes pixels around user defined gridlines
        private void RemoveGridlines(PixelState[,] pixels, NuGenScreenTranslate transform,
            CoordSettings coordSettings, GridRemovalSettings gridRemovalSettings, PixelState pxState)
        {
            if (transform.ValidAxes)
            {
                List<GridlineScreen> gridlines;
                gridlines = NuGenGridMesh.MakeGridLines(transform, coordSettings, gridRemovalSettings.gridMesh);

                foreach (GridlineScreen gridline in gridlines)
                {
                    int xStart = gridline.Start.X;
                    int yStart = gridline.Start.Y;
                    int xStop = gridline.Stop.X;
                    int yStop = gridline.Stop.Y;

                    if (Math.Abs(xStop - xStart) < Math.Abs(yStop - yStart))
                    {
                        //Vertical lines
                        RemoveGridlineVertical(pixels, xStart, yStart, xStop, yStop, gridRemovalSettings, pxState);
                    }
                    else
                    {
                        //Horizontal lines
                        RemoveGridlineHorizontal(pixels, xStart, yStart, xStop, yStop, gridRemovalSettings, pxState);
                    }
                }
            }
        }

        private void RemoveGridlineHorizontal(PixelState[,] pixels, int xStart, int yStart, int xStop, int yStop, GridRemovalSettings gridSettings, PixelState pixelStateRemoved)
        {
            // theta is the angle between the vertical column and the gridline. since we
            // divide by sinTheta, this function should NOT be used for vertical gridlines
            // since divide-by-zero error would occur
            if (xStart > xStop)
            {
                int temp = xStart;
                xStart = xStop;
                xStop = temp;
                temp = yStart;
                yStart = yStop;
                yStop = temp;
            }

            double sinTheta;
            double atan;

            try
            {
                atan = Math.Atan2(xStop - xStart, yStop - yStart);
            }
            catch (Exception e)
            {
                atan = 0;
            }

            sinTheta = Math.Sin(atan);

            for (int x = (int)(xStart - gridSettings.gridDistance + 0.5);
              x < (int)(xStop + gridSettings.gridDistance + 0.5); x++)
            {
                // interest this pixel column (x=xc) with the gridline (x-x0)/(x1-x0)=(y-y0)/(y1-y0)
                // to get (xp,yp)
                double sLine1, sLine2;
                try
                {
                    double[] results = IntersectTwoLines(x, 0.0, x, this.bmp.Height, xStart, yStart, xStop, yStop);
                    sLine1 = results[0];
                    sLine2 = results[1];
                }
                catch (Exception e)
                {
                    //The lines did not intersect, so continue
                    continue;
                }

                double yp = (1.0 - sLine2) * yStart + sLine2 * yStop;
                int yLow = (int)(-gridSettings.gridDistance / sinTheta + yp + 0.5);
                int yHigh = (int)(gridSettings.gridDistance / sinTheta + yp + 0.5);
                for (int y = yLow; y <= yHigh; y++)
                {
                    bool include = true;
                    if (sLine2 < 0.0)
                    {
                        // at start end, the pixels have to be on the same side of (xStart,yStart) as
                        // (xStop,yStop), so use dot product to see if this pixel is on the same side
                        double dotProduct = (x - xStart) * (xStop - xStart) + (y - yStart) * (yStop - yStart);
                        include = (dotProduct >= 0.0);
                    }
                    else if (sLine2 > 1.0)
                    {
                        // at stop end, the pixels have to be on the same side of (xStop,yStop) as
                        // (xStart,yStart), so use dot product to see if this pixel is on the same side
                        double dotProduct = (x - xStop) * (xStart - xStop) + (y - yStop) * (yStart - yStop);
                        include = (dotProduct >= 0.0);
                    }

                    if (include && (0 <= x) && (x < this.bmp.Width) && (0 <= y) && (y < this.bmp.Height))
                    {
                        // overwrite this pixel with background color
                        pixels[x, y] = pixelStateRemoved;
                    }
                }
            }
        }

        private void RemoveGridlineVertical(PixelState[,] pixels, int xStart, int yStart, int xStop, int yStop, GridRemovalSettings gridSettings, PixelState pixelStateRemove)
        {
            // theta is the angle between the horizontal row and the gridline. since we
            // divide by sinTheta, this function should NOT be used for horizontal gridlines
            // since divide-by-zero error would occur
            if (yStart > yStop)
            {
                int temp = yStart;
                yStart = yStop;
                yStop = temp;
                temp = xStart;
                xStart = xStop;
                xStop = temp;
            }

            double sinTheta;
            double atan;

            try
            {
                atan = Math.Atan2(yStop - yStart, xStop - xStart);
            }
            catch (Exception e)
            {
                atan = 0;
            }

            sinTheta = Math.Sin(atan);

            for (int y = (int)(yStart - gridSettings.gridDistance + 0.5);
              y < (int)(yStop + gridSettings.gridDistance + 0.5); y++)
            {
                // interest this pixel row (y=yc) with the gridline (x-x0)/(x1-x0)=(y-y0)/(y1-y0)
                // to get (xp,yp)
                double sLine1, sLine2;
                try
                {
                    double[] results = IntersectTwoLines(0.0, y, this.bmp.Width, y, xStart, yStart, xStop, yStop);
                    sLine1 = results[0];
                    sLine2 = results[1];
                }
                catch (Exception e)
                {
                    //The lines did not intersect, so continue
                    continue;
                }
                double xp = (1.0 - sLine2) * xStart + sLine2 * xStop;
                int xLow = (int)(-gridSettings.gridDistance / sinTheta + xp + 0.5);
                int xHigh = (int)(gridSettings.gridDistance / sinTheta + xp + 0.5);
                for (int x = xLow; x <= xHigh; x++)
                {
                    bool include = true;
                    if (sLine2 < 0.0)
                    {
                        // at start end, the pixels have to be on the same side of (xStart,yStart) as
                        // (xStop,yStop), so use dot product to see if this pixel is on the same side
                        double dotProduct = (x - xStart) * (xStop - xStart) + (y - yStart) * (yStop - yStart);
                        include = (dotProduct >= 0.0);
                    }
                    else if (sLine2 > 1.0)
                    {
                        // at stop end, the pixels have to be on the same side of (xStop,yStop) as
                        // (xStart,yStart), so use dot product to see if this pixel is on the same side
                        double dotProduct = (x - xStop) * (xStart - xStop) + (y - yStop) * (yStart - yStop);
                        include = (dotProduct >= 0.0);
                    }

                    if (include && (0 <= x) && (x < this.bmp.Width) && (0 <= y) && (y < this.bmp.Height))
                    {
                        // overwrite this pixel with background color
                        pixels[x, y] = pixelStateRemove;
                    }
                }
            }
        }

        public Image GetImage()
        {
            return bmp;
        }


        /*    intersect two line segments defined using the two-point equation.
      return value is true if an intersection occurred. The parameter sLine1Int
    says where the intersection occurred along the first line, where sLine1Int=0
    is at point a and sLine1Int=1 is at point bmp. The parameter sLine2Int
    works the same but for the second line */
        public static double[] IntersectTwoLines(double xLine1a, double yLine1a, double xLine1b, double yLine1b,
                    double xLine2a, double yLine2a, double xLine2b, double yLine2b)
        {
            /* parameterize the two-point lines as x=(1-s)*xa+s*xb, y=(1-s)*ya+s*yb and
               then intersect to get s = numerator / denominator */
            double denominator, numeratorLine1, numeratorLine2;

            denominator =
              (yLine2b - yLine2a) * (xLine1b - xLine1a) -
              (yLine1b - yLine1a) * (xLine2b - xLine2a);

            if (Math.Abs(denominator) < 0.000001)
                throw new Exception("Lines do not intersect"); ; // either zero or an infinite number of points intersect

            numeratorLine1 =
              (yLine1a - yLine2a) * (xLine2b - xLine2a) -
              (yLine2b - yLine2a) * (xLine1a - xLine2a);
            numeratorLine2 =
              (yLine1b - yLine1a) * (xLine2a - xLine1a) -
              (yLine2a - yLine1a) * (xLine1b - xLine1a);

            double[] result = new double[2];

            result[0] = numeratorLine1 / denominator;
            result[1] = numeratorLine2 / denominator;

            return result;
        }
    }

    class SearchPoint : IComparable
    {
        private Point p;
        private double distance;

        public SearchPoint(int x, int y, double distance)
        {
            p.X = x;
            p.Y = y;
            this.distance = distance;
        }

        public int X
        {
            get
            {
                return p.X;
            }
        }

        public int Y
        {
            get
            {
                return p.Y;
            }
        }

        public double Distance
        {
            get
            {
                return distance;
            }
        }

        public int CompareTo(object other)
        {
            SearchPoint otherPoint = (SearchPoint)other;

            return (int)(distance - otherPoint.Distance);
        }
    }

    #region Synapse

    //A synapse is analgous to a synapse in the brain, it is a gap between two neurons,
    // which in this case are sets of connected pixels, which is used to bridge these gaps
    class Synapse
    {
        private int index;

        private int countPixels;
        private int xSum;
        private int ySum;

        public Synapse(Dictionary<int, Synapse> synapseDict)
        {
            index = synapseDict.Count;

            synapseDict.Add(index, this);
        }

        public void addPixel(int x, int y)
        {
            ++countPixels;
            xSum += x;
            ySum += y;
        }

        public int Index
        {
            get
            {
                return index;
            }
        }

        public int XCenterOfMass
        {
            get
            {
                if (countPixels < 1)
                    return 0;
                else
                    return (int)(xSum / (double)countPixels + 0.5);
            }
        }

        public int YCenterOfMass
        {
            get
            {
                if (countPixels < 1)
                    return 0;
                else
                    return (int)(ySum / (double)countPixels + 0.5);
            }
        }
    }

    #endregion

    #region Neuron

    // a neuron is a complete set of adjacent on-pixels. each neuron is identified by
    // its index in the neuron list. each neuron has zero or more synapses
    class Neuron
    {
        private int index;

        private Dictionary<int, Synapse> synapseDict = new Dictionary<int, Synapse>();

        public Neuron(Dictionary<int, Neuron> neuronDict)
        {
            index = neuronDict.Count;
            neuronDict.Add(index, this);
        }

        public Synapse AddSynapse()
        {
            Synapse synapse = new Synapse(synapseDict);

            return synapse;
        }

        public void ConnectNeuronAcrossGaps(PixelState[,] pixels, int[,] pixel2Neuron,
          Dictionary<int, Neuron> neuronDict, int width, int height, double gapSeparation)
        {

            int searchHalfWidth = (int)(gapSeparation + 0.5);

            for (int i = 0; i < synapseDict.Count; i++)
            {
                Synapse s = synapseDict[i];
                int x = s.XCenterOfMass;
                int y = s.YCenterOfMass;

                // look at all pixels within a square centered around this synapse center-of-mass,
                // but only halfway around, starting straight up and going clockwise) around the
                // current pixel for other neurons. do not look in a full circle since then
                // we would end up with two connections for each pair of synapses (one in
                // either direction)

                List<int> neuronsProcessed = new List<int>(); ;

                int xDelta, yDelta;

                for (xDelta = 0; xDelta <= searchHalfWidth; xDelta++)
                {
                    for (yDelta = -searchHalfWidth; yDelta <= searchHalfWidth; yDelta++)
                    {
                        if ((0 < xDelta) || (yDelta < 0))
                            ConnectSynapseToSynapsesSource(pixels, pixel2Neuron, neuronDict,
                                width, height, gapSeparation, x, y, x + xDelta, y + yDelta, neuronsProcessed);
                    }
                }
            }
        }

        public void ConnectSynapseToSynapsesDestination(PixelState[,] pixels, int height, double gapSeperation,
               int xSource, int ySource)
        {
            for (int i = 0; i < synapseDict.Count; i++)
            {
                Synapse s = synapseDict[i];
                int xDestination = s.XCenterOfMass;
                int yDestination = s.YCenterOfMass;
                double seperation = Math.Sqrt((xDestination - xSource) * (xDestination - xSource) +
                    (yDestination - ySource) * (yDestination - ySource));

                if (seperation <= gapSeperation)
                {
                    // draw a line from the source to the destination. we sacrifice efficiency for
                    // simplicity, and do not use the Bresenham line-drawing algorithm (this is
                    // a one-time cost and at most there should be less than a few hundred
                    // pixels drawn in total)
                    int nSteps = 1 + (int)(seperation / 0.5);
                    double xDelta = (double)(xDestination - xSource) / (double)nSteps;
                    double yDelta = (double)(yDestination - ySource) / (double)nSteps;
                    double x = xSource + xDelta, y = ySource + yDelta;

                    for (int j = 0; j < nSteps; j++)
                    {
                        // any pixel within a half pixel of (x,y) will be turned on

                        int xLine = (int)x;
                        int yLine = (int)y;
                        if (Math.Sqrt((x - xLine) * (x - xLine) + (y - yLine) * (y - yLine)) < 0.5)
                            pixels[xLine, yLine] = PixelState.PixelOn;
                        xLine += 1;
                        if (Math.Sqrt((x - xLine) * (x - xLine) + (y - yLine) * (y - yLine)) < 0.5)
                            pixels[xLine, yLine] = PixelState.PixelOn;
                        yLine += 1;
                        if (Math.Sqrt((x - xLine) * (x - xLine) + (y - yLine) * (y - yLine)) < 0.5)
                            pixels[xLine, yLine] = PixelState.PixelOn;
                        xLine -= 1;
                        if (Math.Sqrt((x - xLine) * (x - xLine) + (y - yLine) * (y - yLine)) < 0.5)
                            pixels[xLine, yLine] = PixelState.PixelOn;

                        x += xDelta;
                        y += yDelta;
                    }
                }
            }
        }

        public void ConnectSynapseToSynapsesSource(PixelState[,] pixels, int[,] pixel2Neuron,
                        Dictionary<int, Neuron> neuronDict, int width, int height,
                        double gapSeparation, int x, int y,
                        int xLook, int yLook, List<int> neuronsProcessed)
        {
            // this function tries to connect this synapse with center-of-mass at (x,y),
            // to the synapses of another neuron. the other neuron actually does the
            // work of making the connections

            if ((0 <= xLook) && (xLook < width) && (0 <= yLook) && (yLook < height))
            {
                // see if pixel belongs to a neuron, that is not this neuron,
                // and this synapse has not already been connected to that neuron
                int n = pixel2Neuron[xLook, yLook];
                if ((n != 0) &&
                  (n != index) &&
                  (neuronsProcessed.Contains(n)))
                {
                    // let the other neuron do the actual connections, since there may be
                    // zero, one or more and it alone knows where its synapses are
                    neuronDict[n].ConnectSynapseToSynapsesDestination(pixels, height, gapSeparation,
                      x, y);

                    // update list of process neurons
                    neuronsProcessed.Add(n);
                }
            }
        }

        public int Index
        {
            get
            {
                return index;
            }
        }
    }

    #endregion
}
