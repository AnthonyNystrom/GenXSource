using System;
using System.Collections.Specialized;
using System.Windows.Media.Imaging;

namespace Next2Friends.ImageWorks.EffectsFilters
{
    public class FisheyeFilter : EffectFilter
    {
        public float Curvature { get; set; }

        public FisheyeFilter(float curvature)
        {
            Curvature = curvature;
        }

        public static new EffectFilter Default
        {
            get
            {
                return new FisheyeFilter(6.0f);
            }
        }

        public override BitmapSource ExecuteFilter(BitmapSource inputImage)
        {
            if (Curvature <= 0.0f)
            {
                return inputImage;
            }

            // get source image size
            int width = inputImage.PixelWidth;
            int height = inputImage.PixelHeight;
            int halfWidth = width / 2;
            int halfHeight = height / 2;

            //Obtain the Maxium Size Of the Fish Eye
            int maxRadius = (int)Math.Min(width, height) / 2;            
            double s = maxRadius / Math.Log(Curvature * maxRadius + 1);

            byte[] srcBytes = new byte[inputImage.PixelWidth * inputImage.PixelHeight * 4];
            byte[] destBytes = new byte[inputImage.PixelWidth * inputImage.PixelHeight * 4];

            inputImage.CopyPixels(srcBytes, inputImage.PixelWidth * 4, 0);
            inputImage.CopyPixels(destBytes, inputImage.PixelWidth * 4, 0);

            // Perform The Fish Eye Conversion
            //Start from the middle because point (0,0) in cartesian co-ordinates should not be in the center
            for (int y = -1 * halfHeight; y < height - halfHeight; y++)
            {
                for (int x = -1 * halfWidth; x < width - halfWidth; x++)
                {
                    //Get the Current Pixels Polar Co-ordinates
                    double radius = Math.Sqrt(x * x + y * y);
                    double angle = Math.Atan2(y, x);

                    //Check to see if the polar pixel is out of bounds
                    if (radius <= maxRadius)
                    {
                        //Current Pixel is inside the Fish Eye

                        //newRad is the pixel that we want to shift to current pixel based on the fish eye posistion
                        double newRad = (Math.Exp(radius / s) - 1) / Curvature;
                        //Current Pixels Polar Cordinates Back To Cartesian
                        int newX = (int)(newRad * Math.Cos(angle));
                        int newY = (int)(newRad * Math.Sin(angle));

                        newX += halfWidth;
                        newY += halfHeight;

                        int stride = inputImage.PixelWidth * 4;

                        int srcPixelOffset = newY * stride + newX * 4;
                        int destPixelOffset = (y + halfHeight) * stride + (x + halfWidth) * 4;

                        destBytes[destPixelOffset] = srcBytes[srcPixelOffset];
                        destBytes[destPixelOffset + 1] = srcBytes[srcPixelOffset + 1];
                        destBytes[destPixelOffset + 2] = srcBytes[srcPixelOffset + 2];
                    }
                    else
                    {
                        int stride = inputImage.PixelWidth * 4;

                        int srcPixelOffset = (y + halfHeight) * stride + (x + halfWidth) * 4;
                        int destPixelOffset = (y + halfHeight) * stride + (x + halfWidth) * 4;

                        destBytes[destPixelOffset] = srcBytes[srcPixelOffset];
                        destBytes[destPixelOffset + 1] = srcBytes[srcPixelOffset + 1];
                        destBytes[destPixelOffset + 2] = srcBytes[srcPixelOffset + 2];
                    }
                }
            }

            return BitmapSource.Create(inputImage.PixelWidth, inputImage.PixelHeight, 96, 96, inputImage.Format,
                null, destBytes, inputImage.PixelWidth * 4);
        }

    }
}
