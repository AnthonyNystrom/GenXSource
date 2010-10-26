using System;
using System.Drawing;
using System.Drawing.Imaging;
using Genetibase.Common.Interfaces;
using System.Collections.Specialized;

namespace Genetibase.BasicFilters
{
  /// <summary>
  /// This fisheye filter code (by Paul Kinlan) was taken partly from the following site (with permission of course)
  /// http://www.kinlan.co.uk/2005/05/fisheye-example-code-that-integrates.html
  /// </summary>
  public class FisheyeFilter : BasicFilter
  {

    private float _curvature = 0.1f;

    /// <summary>
    /// Determins the FishEye effect intensity
    /// </summary>
    public float Curvature
    {
      get { return _curvature; }
      set { _curvature = value; }
    }

    #region IFilter Members
    /// <summary>
    /// Executes this filter on the input image and returns the result
    /// </summary>
    /// <param name="inputImage">input image</param>
    /// <returns>Creates a FishEye effect</returns>
    /// <example>
    /// <code>
    /// Image transformed;
    /// FisheyeFilter fishI = new FisheyeFilter();
    /// fishI.Curvature = 0.15f
    /// transformed = fishI.ExecuteFilter(myImg);
    /// </code>
    /// </example>
    public override Image ExecuteFilter(Image rawImage)
    {
      FishEye myEye = new FishEye(_curvature);
      Image filteredImage = myEye.Apply((Bitmap)rawImage);
      return filteredImage;
    }


    #endregion


    private class FishEye
    {
      private float _curvature;
      public float Curvature
      {
        get { return _curvature; }
        set { _curvature = value; }
      }
      #region IFilter Members


      public FishEye(float inCurvature)
      {
        _curvature = inCurvature;
      }


      public Bitmap Apply(Bitmap srcImg)
      {
        if (_curvature <= 0.0f)
        {
          throw new ArgumentException("Curvature Must Be Set and It Must Be > 0.0f");
        }

        // get source image size
        int width = srcImg.Width;
        int height = srcImg.Height;
        int halfWidth = width / 2;
        int halfHeight = height / 2;

        //Obtain the Maxium Size Of the Fish Eye
        int maxRadius = (int)Math.Min(width, height) / 2;
        //The S parameter mentioned in Jason Walton Algo.
        double s = maxRadius / Math.Log(_curvature * maxRadius + 1);

        PixelFormat fmt = (srcImg.PixelFormat == PixelFormat.Format8bppIndexed) ?
          PixelFormat.Format8bppIndexed : PixelFormat.Format24bppRgb;

        // lock source bitmap data
        BitmapData srcData = srcImg.LockBits(
          new Rectangle(0, 0, width, height),
          ImageLockMode.ReadOnly, fmt);

        // create new image
        //Bitmap dstImg = (fmt == PixelFormat.Format8bppIndexed) ?
        //  Tiger.Imaging.Image.CreateGrayscaleImage(width, height) :
        //  new Bitmap(width, height, fmt);

        Bitmap dstImg = new Bitmap(width, height, fmt);

        // lock destination bitmap data
        BitmapData dstData = dstImg.LockBits(
          new Rectangle(0, 0, width, height),
          ImageLockMode.ReadWrite, fmt);

        int stride = srcData.Stride;
        int offset = stride - ((fmt == PixelFormat.Format8bppIndexed) ? width : width * 3);

        // Perform The Fish Eye Conversion
        //
        unsafe
        {
          byte* src = (byte*)srcData.Scan0.ToPointer();
          byte* dst = (byte*)dstData.Scan0.ToPointer();
          byte* p;

          if (fmt == PixelFormat.Format8bppIndexed)
          {
            #region Grayscale
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
                  double newRad = (Math.Exp(radius / s) - 1) / _curvature;
                  int newX = (int)(newRad * Math.Cos(angle));
                  int newY = (int)(newRad * Math.Sin(angle));

                  newX += halfWidth;
                  newY += halfHeight;

                  p = src + newY * stride + newX;
                  byte* dst2 = dst + (y + halfHeight) * stride + (x + halfWidth);
                  dst2 = p;

                }
                else
                {
                  *dst = src[y * stride + x];
                }

              }
              dst += offset;
            }
            #endregion
          }
          else
          {
            #region RGB
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
                  double newRad = (Math.Exp(radius / s) - 1) / _curvature;
                  //Current Pixels Polar Cordinates Back To Cartesian
                  int newX = (int)(newRad * Math.Cos(angle));
                  int newY = (int)(newRad * Math.Sin(angle));

                  newX += halfWidth;
                  newY += halfHeight;

                  //Chage the Pointer to the Src
                  p = src + newY * stride + newX * 3;
                  //Change the Pointer to the destination
                  byte* dst2 = dst + (y + halfHeight) * stride + (x + halfWidth) * 3;
                  dst2[0] = p[0];
                  dst2[1] = p[1];
                  dst2[2] = p[2];

                }
                else
                {
                  //Current Pixel is outside the Fish Eye thus should be output as is
                  p = src + (y + halfHeight) * stride + (x + halfWidth) * 3;

                  //Change the Pointer to the destination
                  byte* dst2 = dst + (y + halfHeight) * stride + (x + halfWidth) * 3;

                  dst2[0] = p[0];
                  dst2[1] = p[1];
                  dst2[2] = p[2];
                }
              }
            }
            #endregion
          }
        }
        // unlock both images
        dstImg.UnlockBits(dstData);
        srcImg.UnlockBits(srcData);

        return dstImg;
      }
      #endregion
    }

  }

  
}
