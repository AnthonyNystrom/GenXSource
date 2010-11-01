using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Genetibase.NuGenTransform
{
    enum PixelStates
    {
      PixelOff,
      PixelOnUnscanned,
      PixelOnScanned
    };

    public struct PointMatchTriplet
    {
      // screen coordinates of created point
      public int x;
      public int y;

      // correlation of sample point with pixels around this created point
      public double correlation;
    };

    //This static class provides methods which allow calling classes to find points on a scatter plot from an image
    static class NuGenPointMatch
    {
        private static void ConvertImageToArray(Image imageProcessed, out int[,] imageArray, out int imageWidth, out int imageHeight)
        {
            // compute bounds
            imageWidth = imageProcessed.Width;
            imageHeight = imageProcessed.Height;

            // allocate memory
            imageArray = new int[imageWidth, imageHeight];

            // initialize memory
            Bitmap b = new Bitmap(imageProcessed);
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, b.PixelFormat);

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    imageArray[x, y] = NuGenDiscretize.ProcessedPixelIsOn(bmData, x, y) ? (int)PixelStates.PixelOnUnscanned : (int)PixelStates.PixelOff;
                }
            }

            b.UnlockBits(bmData);
        }

        //Takes a list of pixel locations and gets a boolean array and descriptive data as output
        private static void ConvertSampleToArray(List<Point> samplePointPixels, out bool[,] sampleMaskArray, ref int sampleMaskWidth,
            ref int sampleMaskHeight, out int xCenter, out int yCenter)
        {

            // compute bounds
            bool first = true;
            int xMin = sampleMaskWidth;
            int yMin = sampleMaskHeight;
            int xMax = 0;
            int yMax = 0;

            for (int i = 0; i < samplePointPixels.Count; i++)
            {
                int x = samplePointPixels[i].X;
                int y = samplePointPixels[i].Y;
                if (first || (x < xMin))
                    xMin = x;
                if (first || (x > xMax))
                    xMax = x;
                if (first || (y < yMin))
                    yMin = y;
                if (first || (y > yMax))
                    yMax = y;

                first = false;
            }

            int border = 1; // #pixels in border on each side

            sampleMaskWidth = (xMax + border) - xMin + border + 1;
            sampleMaskHeight = (yMax + border) - yMin + border + 1;

            // allocate memory
            sampleMaskArray = new bool[sampleMaskWidth, sampleMaskHeight];

            // initialize memory
            for (int x = 0; x < sampleMaskWidth; x++)
                for (int y = 0; y < sampleMaskHeight; y++)
                    sampleMaskArray[x, y] = false;

            int xSum = 0, ySum = 0;
            for (int i = 0; i < samplePointPixels.Count; i++)
            {
                int x = samplePointPixels[i].X - xMin + border;
                int y = samplePointPixels[i].Y - yMin + border;
                sampleMaskArray[x, y] = true;

                xSum += x;
                ySum += y;
            }

            // compute center of mass location  
            xCenter = (int)((double)xSum / (double)samplePointPixels.Count + 0.5);
            yCenter = (int)((double)ySum / (double)samplePointPixels.Count + 0.5);
        }

        //Gets the correlation of the sample input
        //
        // The correlation is basically how closely the pixels of the sample match up with the
        // pixels of the next candidate match point
        private static bool Correlation(bool[,] sampleMaskArray, int sampleMaskWidth, int sampleMaskHeight, int sampleXCenter,
            int sampleYCenter, int[,] imageArray, int imageWidth, int imageHeight, int x, int y, out double corr)
        {
            corr = 0.0;
            if ((x - sampleXCenter < 0) ||
              (y - sampleYCenter < 0) ||
              (imageWidth <= x - sampleXCenter + sampleMaskWidth) ||
              (imageHeight <= y - sampleXCenter + sampleMaskHeight))
            {
                // mask does not extend out of image
                return false;
            }

            for (int xS = 0; xS < sampleMaskWidth; xS++)
                for (int yS = 0; yS < sampleMaskHeight; yS++)
                {
                    // are corresponding pixels in the sample and image on?
                    bool sample = sampleMaskArray[xS, yS];
                    bool image = (imageArray[x - sampleXCenter + xS, y - sampleYCenter + yS] != (int)PixelStates.PixelOff);

                    if (sample == image)
                        corr += 1.0;
                    else
                        corr -= 1.0;
                }

            return true;
        }

        public static bool IsolateSampleMatchPoint(List<Point> samplePointPixels, BitmapData bmData, PointMatchSettings settings,
            int xStart, int yStart, int x, int y)
        {
            if ((x < 0) || (y < 0) || (bmData.Width <= x) || (bmData.Height <= y))
                return false; // out of bounds

            if (!NuGenDiscretize.ProcessedPixelIsOn(bmData, x, y))
                return false; // pixel is off

            if (Math.Abs(x - xStart) > settings.pointSize / 2)
                return false; // point is too far from start
            if (Math.Abs(y - yStart) > settings.pointSize / 2)
                return false; // point is too far from start

            bool found = (samplePointPixels.Count > 0);
            if (found)
            {
                foreach (Point p in samplePointPixels)
                {
                    if (p.X == x && p.Y == y)
                    {
                        found = true;
                        break;
                    }
                    found = false;
                }
            }

            if (found)
                return true; // already in list

            // add this point
            samplePointPixels.Add(new Point(x, y));

            // recurse. diagonal points are included so single-pixel wide polygonal outlines will be traversed,
            // but for a 2x speed increase we only go diagonal if the adjacent nondiagonal pixels are off
            bool right =
              IsolateSampleMatchPoint(samplePointPixels, bmData, settings, xStart, yStart, x + 1, y);
            bool up =
              IsolateSampleMatchPoint(samplePointPixels, bmData, settings, xStart, yStart, x, y + 1);
            bool left =
              IsolateSampleMatchPoint(samplePointPixels, bmData, settings, xStart, yStart, x - 1, y);
            bool down =
              IsolateSampleMatchPoint(samplePointPixels, bmData, settings, xStart, yStart, x, y - 1);
            if (!right && !up)
                IsolateSampleMatchPoint(samplePointPixels, bmData, settings, xStart, yStart, x + 1, y + 1);
            if (!up && !left)
                IsolateSampleMatchPoint(samplePointPixels, bmData, settings, xStart, yStart, x - 1, y + 1);
            if (!left && !down)
                IsolateSampleMatchPoint(samplePointPixels, bmData, settings, xStart, yStart, x - 1, y - 1);
            if (!down && !right)
                IsolateSampleMatchPoint(samplePointPixels, bmData, settings, xStart, yStart, x + 1, y - 1);

            return true;
        }

        //Matches a point to the sample matched point
        public static void MatchSamplePoint(Image imageProcessed, PointMatchSettings settings, List<Point> samplePointPixels,
            List<Point> pointsExisting, List<PointMatchTriplet> pointsCreated)
        {

            // create sample point array
            bool[,] sampleMaskArray;
            int sampleMaskWidth = 0, sampleMaskHeight = 0;
            int sampleXCenter, sampleYCenter;

            ConvertSampleToArray(samplePointPixels, out sampleMaskArray,
              ref sampleMaskWidth, ref sampleMaskHeight,
              out sampleXCenter, out sampleYCenter);

            // create image array
            int[,] imageArray;
            int imageWidth, imageHeight;
            ConvertImageToArray(imageProcessed, out imageArray, out imageWidth, out imageHeight);

            RemovePixelsNearCurrentPoints(imageArray, imageWidth, imageHeight, pointsExisting,
              settings.pointSeparation);

            List<PointMatchTriplet> listCreated = new List<PointMatchTriplet>();
            ScanImage(sampleMaskArray, sampleMaskWidth, sampleMaskHeight,
              sampleXCenter, sampleYCenter, settings, imageArray, imageWidth, imageHeight, listCreated);
            Comparison<PointMatchTriplet> comparison = new Comparison<PointMatchTriplet>(CompareTriplet);
            listCreated.Sort(comparison);            

            foreach (PointMatchTriplet t in listCreated)
            {
                pointsCreated.Add(t);
            }
        }

        //delegate method for sorting triplets based on correlation
        private static int CompareTriplet(PointMatchTriplet p1, PointMatchTriplet p2)
        {
            if (p2.correlation > p1.correlation)
                return 1;
            else if (p2.correlation < p1.correlation)
                return -1;
            else
                return 0;
        }

        private static void RecurseThroughOnRegion(bool[,] sampleMaskArray, int sampleMaskWidth, int sampleMaskHeight,
            int sampleXCenter, int sampleYCenter, int[,] imageArray, int imageWidth, int imageHeight, int x, int y,
            ref bool firstMax, ref int onCount, ref int xSum, ref int ySum, ref int xMin, ref int xMax, ref int yMin, ref int yMax, ref double correlationMax)
        {
            if ((x < 0) || (y < 0) ||
              (imageWidth <= x) || (imageHeight <= y))
            {
                // this point is off the screen
                return;
            }

            if (imageArray[x, y] != (int)PixelStates.PixelOnUnscanned)
            {
                // we only consider points that are on, and have not already been scanned
                return;
            }

            // update statistics on this region
            onCount += 1;
            xSum += x;
            ySum += y;
            if (x < xMin)
                xMin = x;
            if (xMax < x)
                xMax = x;
            if (y < yMin)
                yMin = y;
            if (yMax < y)
                yMax = y;

            // mark this point so it is never rescanned
            imageArray[x, y] = (int)PixelStates.PixelOnScanned;

            double corr;
            if (Correlation(sampleMaskArray, sampleMaskWidth, sampleMaskHeight,
              sampleXCenter, sampleYCenter, imageArray, imageWidth, imageHeight, x, y,
              out corr))
            {
                // correlation for this point could be performed
                if (firstMax || (corr > correlationMax))
                {
                    // this is the highest correlation so far, so save it
                    firstMax = false;
                    correlationMax = corr;
                }
            }

            // recurse through neighbors. originally only vertical neighbors were
            // included (so that separate points that just touched each other could
            // be separated), but diagonal neighbors were included so a diagonal line
            // (in a triangle for instance) could be handled
            RecurseThroughOnRegion(sampleMaskArray, sampleMaskWidth, sampleMaskHeight,
              sampleXCenter, sampleYCenter, imageArray, imageWidth, imageHeight, x - 1, y - 1,
              ref firstMax, ref onCount, ref xSum, ref ySum, ref xMin, ref xMax, ref yMin, ref yMax, ref correlationMax);
            RecurseThroughOnRegion(sampleMaskArray, sampleMaskWidth, sampleMaskHeight,
              sampleXCenter, sampleYCenter, imageArray, imageWidth, imageHeight, x - 1, y,
              ref firstMax, ref onCount, ref xSum, ref ySum, ref xMin, ref xMax, ref yMin, ref yMax, ref correlationMax);
            RecurseThroughOnRegion(sampleMaskArray, sampleMaskWidth, sampleMaskHeight,
              sampleXCenter, sampleYCenter, imageArray, imageWidth, imageHeight, x - 1, y + 1,
              ref firstMax, ref onCount, ref xSum, ref ySum, ref xMin, ref xMax, ref yMin, ref yMax, ref correlationMax);
            RecurseThroughOnRegion(sampleMaskArray, sampleMaskWidth, sampleMaskHeight,
              sampleXCenter, sampleYCenter, imageArray, imageWidth, imageHeight, x, y - 1,
              ref firstMax, ref onCount, ref xSum, ref ySum, ref xMin, ref xMax, ref yMin, ref yMax, ref correlationMax);
            RecurseThroughOnRegion(sampleMaskArray, sampleMaskWidth, sampleMaskHeight,
              sampleXCenter, sampleYCenter, imageArray, imageWidth, imageHeight, x, y + 1,
              ref firstMax, ref onCount, ref xSum, ref ySum, ref xMin, ref xMax, ref yMin, ref yMax, ref correlationMax);
            RecurseThroughOnRegion(sampleMaskArray, sampleMaskWidth, sampleMaskHeight,
              sampleXCenter, sampleYCenter, imageArray, imageWidth, imageHeight, x + 1, y - 1,
              ref firstMax, ref onCount, ref xSum, ref ySum, ref xMin, ref xMax, ref yMin, ref yMax, ref correlationMax);
            RecurseThroughOnRegion(sampleMaskArray, sampleMaskWidth, sampleMaskHeight,
              sampleXCenter, sampleYCenter, imageArray, imageWidth, imageHeight, x + 1, y,
              ref firstMax, ref onCount, ref xSum, ref ySum, ref xMin, ref xMax, ref yMin, ref yMax, ref correlationMax);
            RecurseThroughOnRegion(sampleMaskArray, sampleMaskWidth, sampleMaskHeight,
              sampleXCenter, sampleYCenter, imageArray, imageWidth, imageHeight, x + 1, y + 1,
              ref firstMax, ref onCount, ref xSum, ref ySum, ref xMin, ref xMax, ref yMin, ref yMax, ref correlationMax);
        }

        public static void RemovePixelsNearCurrentPoints(int[,] imageArray, int imageWidth, int imageHeight, List<Point> pointsExisting, int pointSeparation)
        {
            int count = 0;

            for (int i = 0; i < pointsExisting.Count; i++)
            {
                int xPoint = pointsExisting[i].X;
                int yPoint = pointsExisting[i].Y;

                // loop through rows of pixels
                int yMin = yPoint - pointSeparation;
                if (yMin < 0)
                    yMin = 0;
                int yMax = yPoint + pointSeparation;
                if (imageHeight < yMax)
                    yMax = imageHeight;

                for (int y = yMin; y < yMax; y++)
                {
                    // pythagorean theorem gives range of x values
                    int radical = pointSeparation * pointSeparation - (y - yPoint) * (y - yPoint);
                    if (0 < radical)
                    {
                        int xMin = (int)(xPoint - Math.Sqrt(radical));
                        if (xMin < 0)
                            xMin = 0;
                        int xMax = xPoint + (xPoint - xMin);
                        if (imageWidth < xMax)
                            xMax = imageWidth;

                        // turn off pixels in this row of pixels
                        for (int x = xMin; x < xMax; x++)
                        {
                            if (imageArray[x, y] != (int)PixelStates.PixelOff)
                            {
                                imageArray[x, y] = (int)PixelStates.PixelOff;
                                ++count;
                            }
                        }
                    }
                }
            }
        }

        public static void ScanImage(bool[,] sampleMaskArray, int sampleMaskWidth, int sampleMaskHeight, int sampleXCenter,
            int sampleYCenter, PointMatchSettings settings, int[,] imageArray, int imageWidth, int imageHeight, List<PointMatchTriplet> pointsCreated)
        {
            // loop through image to first on-pixel
            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    bool firstMax = true;
                    int onCount = 0, xSum = 0, ySum = 0;
                    int xMin = imageWidth, xMax = 0;
                    int yMin = imageHeight, yMax = 0;
                    double correlationMax = 0.0;
                    RecurseThroughOnRegion(sampleMaskArray, sampleMaskWidth, sampleMaskHeight,
                      sampleXCenter, sampleYCenter,
                      imageArray, imageWidth, imageHeight,
                      x, y, ref firstMax, ref onCount, ref xSum, ref ySum,
                      ref xMin, ref xMax, ref yMin, ref yMax,
                      ref correlationMax);

                    // save max if its correlation is positive and the point is not too big. a zero
                    // correlation is would be expected from two uncorrelated sets of pixels, and
                    // a negative correlation means the sets of pixels are inverses
                    if (!firstMax && (correlationMax > 0) &&
                      (xMax - xMin < settings.pointSize) &&
                      (yMax - yMin < settings.pointSize))
                    {
                        PointMatchTriplet p = new PointMatchTriplet();

                        if (onCount < 0)
                            onCount = 1;
                        p.x = (int)((double)xSum / (double)onCount + 0.5);
                        p.y = (int)((double)ySum / (double)onCount + 0.5);
                        p.correlation = correlationMax;

                        pointsCreated.Add(p);
                    }
                }
            }
        }
    }
}
