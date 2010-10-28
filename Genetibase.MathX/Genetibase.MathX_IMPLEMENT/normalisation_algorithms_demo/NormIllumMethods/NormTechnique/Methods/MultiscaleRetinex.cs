using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Math;
using AForge.Imaging.Filters;

namespace NormIllumMethods.Techniques
{
    public class MultiscaleRetinex : ImageOperations
    {

        double[] sigmas; //different scales of gaussian sigma 
        double[] widths; //weighting term of gaussian sigma
        int size;        //size of filter

        public MultiscaleRetinex(double[] sigmas,double[] widths,int size)
        {
            this.sigmas = sigmas;
            this.size = size;
            this.widths = widths;
        }

        public override unsafe Bitmap Apply(Bitmap bitmap)
        {
            int count = sigmas.Length;
            Bitmap bmp;
            double[,] sum = new double[bitmap.Width, bitmap.Height];
            
            for (int i = 0; i < count;i++ )
            {
                bmp = new GaussianBlur(sigmas[i], size).Apply(bitmap);
                sum = SumBitmap(bmp,sum,widths[i]);
            }
            return Normalise(DivBitmap(bitmap, sum));
        }
    }

}
